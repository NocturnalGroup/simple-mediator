using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NocturnalGroup.SimpleMediator.Abstractions.Events;

namespace NocturnalGroup.SimpleMediator.InMemory.Events;

/// <summary>
/// In-Memory implementation of <see cref="IEventSender"/>.
/// </summary>
internal sealed class EventSender : IEventSender
{
	private readonly IServiceProvider _services;
	private readonly ILogger<EventSender> _logger;

	public EventSender(IServiceProvider services, ILogger<EventSender> logger)
	{
		_logger = logger;
		_services = services;
	}

	/// <inheritdoc />
	public async Task PublishEventAsync<TEvent>(TEvent data, CancellationToken? ct)
		where TEvent : IEvent
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("Publishing event {EventType}", typeof(TEvent).FullName);
		}

		var handlers = _services.GetServices<IEventHandler<TEvent>>();
		var tasks = handlers.Select(h => InvokeHandlerAsync(h, data, ct ?? CancellationToken.None));

		// If the event handlers fail, we want to throw an aggregation exception.
		// However, awaiting the Task.WhenAll task will throw the first exception only.
		// So we need to await the task and then reach into the task to get the actual aggregation exception.
		// https://github.com/dotnet/core/issues/7011
		var whenAllTask = Task.WhenAll(tasks);
		try
		{
			await whenAllTask.ConfigureAwait(false);
		}
		catch (Exception)
		{
			throw whenAllTask.Exception!;
		}
	}

	/// <summary>
	/// Helper method that invokes an event handler with logging and error handling.
	/// </summary>
	private async Task InvokeHandlerAsync<TEvent>(IEventHandler<TEvent> handler, TEvent data, CancellationToken ct)
		where TEvent : IEvent
	{
		try
		{
			if (_logger.IsEnabled(LogLevel.Debug))
			{
				_logger.LogDebug("Invoking event handler {EventHandler}", handler.ToString());
			}

			await handler.HandleEventAsync(data, ct).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{EventHandler} threw an exception", handler.ToString());
			}
			throw;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;

namespace NocturnalGroup.SimpleMediator.InMemory.Requests;

/// <summary>
/// In-Memory implementation of <see cref="IRequestSender"/>.
/// </summary>
internal sealed class RequestSender : IRequestSender
{
	private readonly IServiceProvider _services;
	private readonly ILogger<RequestSender> _logger;

	public RequestSender(IServiceProvider services, ILogger<RequestSender> logger)
	{
		_logger = logger;
		_services = services;
	}

	public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken? ct)
		where TRequest : IRequest<TResponse>
	{
		if (_logger.IsEnabled(LogLevel.Information))
		{
			_logger.LogInformation("Processing request {RequestType}", typeof(TRequest).FullName);
		}

		var handler = _services.GetService<IRequestHandler<TRequest, TResponse>>();
		if (handler is null)
		{
			var requestTypeName = typeof(TRequest).FullName;
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError("No request handler registered for {RequestType}", requestTypeName);
			}
			throw new InvalidOperationException($"No request handler registered for {requestTypeName}");
		}

		try
		{
			return await handler.HandleRequestAsync(request, ct ?? CancellationToken.None).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			if (_logger.IsEnabled(LogLevel.Error))
			{
				_logger.LogError(ex, "{RequestHandler} threw an exception", handler.ToString());
			}
			throw;
		}
	}
}

using Microsoft.Extensions.DependencyInjection;
using NocturnalGroup.SimpleMediator.Abstractions;
using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;

namespace NocturnalGroup.SimpleMediator.InMemory;

/// <summary>
/// Convenience wrapper for types that need to publish events and send request.
/// </summary>
internal sealed class Mediator : IMediator
{
	private readonly IServiceProvider _services;

	public Mediator(IServiceProvider services)
	{
		_services = services;
	}

	/// <inheritdoc />
	public Task PublishEventAsync<TEvent>(TEvent data, CancellationToken? ct = null)
		where TEvent : IEvent
	{
		var sender = _services.GetRequiredService<IEventSender>();
		return sender.PublishEventAsync(data, ct);
	}

	/// <inheritdoc />
	public Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken? ct = null)
		where TRequest : IRequest<TResponse>
	{
		var sender = _services.GetRequiredService<IRequestSender>();
		return sender.SendRequestAsync<TRequest, TResponse>(request, ct);
	}
}

using Microsoft.Extensions.DependencyInjection;

namespace NocturnalGroup.SimpleMediator.Abstractions.Events;

/// <summary>
/// A handler that processes events sent through the mediator.
/// </summary>
/// <typeparam name="TEvent">The type of event the handler handles.</typeparam>
public interface IEventHandler<in TEvent>
	where TEvent : IEvent
{
	/// <summary>
	/// Handles an event.
	/// </summary>
	/// <param name="data">The event data.</param>
	/// <param name="ct">The cancellation token provided by the sender.</param>
	Task HandleEventAsync(TEvent data, CancellationToken ct);
}

/// <summary>
/// Event handler extension methods.
/// </summary>
public static class EventHandlerExtensions
{
	/// <summary>
	/// Registers an event handler into the <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The service collection to register the handler into.</param>
	/// <param name="lifetime">The lifetime of the handler.</param>
	/// <typeparam name="TEvent">The type of event the handler handles.</typeparam>
	/// <typeparam name="THandler">The handler to register.</typeparam>
	public static IServiceCollection AddEventHandler<TEvent, THandler>(
		this IServiceCollection services,
		ServiceLifetime lifetime = ServiceLifetime.Transient
	)
		where THandler : IEventHandler<TEvent>
		where TEvent : IEvent
	{
		services.Add(new ServiceDescriptor(typeof(IEventHandler<TEvent>), typeof(THandler), lifetime));
		return services;
	}
}

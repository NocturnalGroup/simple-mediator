namespace NocturnalGroup.SimpleMediator.Abstractions.Events;

/// <summary>
/// Service that publishes events through the mediator.
/// </summary>
public interface IEventSender
{
	/// <summary>
	/// Publishes an event through the mediator.
	/// </summary>
	/// <param name="data">The event data to send.</param>
	/// <param name="ct">The <see cref="CancellationToken"/> to provide to the handlers.</param>
	/// <typeparam name="TEvent">The type of event being published.</typeparam>
	Task PublishEventAsync<TEvent>(TEvent data, CancellationToken? ct = null)
		where TEvent : IEvent;
}

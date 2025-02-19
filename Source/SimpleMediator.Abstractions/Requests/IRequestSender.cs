namespace NocturnalGroup.SimpleMediator.Abstractions.Requests;

/// <summary>
/// Service that publishes requests through the mediator.
/// </summary>
public interface IRequestSender
{
	/// <summary>
	/// Sends a request through the mediator.
	/// </summary>
	/// <param name="request">The request to send.</param>
	/// <param name="ct">The <see cref="CancellationToken"/> to provide to the handler.</param>
	/// <typeparam name="TRequest">The type of request being sent.</typeparam>
	/// <typeparam name="TResponse">The request return type.</typeparam>
	/// <exception cref="InvalidOperationException">Thrown if no handler is registered for the request type.</exception>
	Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, CancellationToken? ct = null)
		where TRequest : IRequest<TResponse>;
}

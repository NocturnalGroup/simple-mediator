using Microsoft.Extensions.DependencyInjection;

namespace NocturnalGroup.SimpleMediator.Abstractions.Requests;

/// <summary>
/// A handler that responds to requests sent through the mediator.
/// </summary>
/// <typeparam name="TRequest">The type of request this handler accepts.</typeparam>
/// <typeparam name="TResponse">The request return type.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	/// <summary>
	/// Handles a request.
	/// </summary>
	/// <param name="request">The request data.</param>
	/// <param name="ct">The cancellation token provided by the sender.</param>
	Task<TResponse> HandleRequestAsync(TRequest request, CancellationToken ct);
}

/// <summary>
/// Request handler extension methods.
/// </summary>
public static class RequestHandlerExtensions
{
	/// <summary>
	/// Registers a request handler into the <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The service collection to register the handler into.</param>
	/// <param name="lifetime">The lifetime of the handler.</param>
	/// <typeparam name="TRequest">The type of request the handler accepts.</typeparam>
	/// <typeparam name="TResponse">The request return type.</typeparam>
	/// <typeparam name="THandler">The handler to register.</typeparam>
	public static IServiceCollection AddRequestHandler<TRequest, TResponse, THandler>(
		this IServiceCollection services,
		ServiceLifetime lifetime = ServiceLifetime.Transient
	)
		where THandler : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		services.Add(new ServiceDescriptor(typeof(IRequestHandler<TRequest, TResponse>), typeof(THandler), lifetime));
		return services;
	}
}

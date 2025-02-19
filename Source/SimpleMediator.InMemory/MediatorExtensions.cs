using Microsoft.Extensions.DependencyInjection;
using NocturnalGroup.SimpleMediator.Abstractions;
using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;
using NocturnalGroup.SimpleMediator.InMemory.Events;
using NocturnalGroup.SimpleMediator.InMemory.Requests;

namespace NocturnalGroup.SimpleMediator.InMemory;

/// <summary>
/// In-Memory mediator extension methods.
/// </summary>
public static class MediatorExtensions
{
	/// <summary>
	/// Registers the in-memory mediator implementations into the <see cref="IServiceCollection"/>.
	/// </summary>
	/// <param name="services">The service collection to register the mediator into.</param>
	/// <param name="lifetime">The lifetime of the handler.</param>
	public static IServiceCollection AddInMemoryMediator(
		this IServiceCollection services,
		ServiceLifetime lifetime = ServiceLifetime.Transient
	)
	{
		services.Add(new ServiceDescriptor(typeof(IEventSender), typeof(EventSender), lifetime));
		services.Add(new ServiceDescriptor(typeof(IRequestSender), typeof(RequestSender), lifetime));
		services.Add(new ServiceDescriptor(typeof(IMediator), typeof(Mediator), lifetime));
		return services;
	}
}

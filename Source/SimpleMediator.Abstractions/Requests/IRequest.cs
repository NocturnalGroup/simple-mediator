namespace NocturnalGroup.SimpleMediator.Abstractions.Requests;

/// <summary>
/// A request that is sent through the mediator.
/// </summary>
/// <typeparam name="TResponse">The request return type.</typeparam>
public interface IRequest<out TResponse>;

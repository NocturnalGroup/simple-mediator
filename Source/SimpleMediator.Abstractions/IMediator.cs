using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;

namespace NocturnalGroup.SimpleMediator.Abstractions;

/// <summary>
/// Convenience wrapper around <see cref="IEventSender"/> and <see cref="IRequestSender"/>.
/// </summary>
public interface IMediator : IEventSender, IRequestSender;

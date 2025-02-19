<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleMediator</h1>
    </summary>
  </ul>
</div>

### A dead simple mediator implementation

[About](readme.md) · Getting Started · [License](license.txt) · [Contributing](contributing.md)

---

## Installing

Please see the installation instructions [here](readme.md#Installing).

## Walkthrough

SimpleMediator is an implementation of the mediator pattern.
Essentially, your types communicate through a common object.
This decreases coupling, and can make your systems more flexible and testable.

SimpleMediator only has two concepts: Events and Requests.
Events are a one-to-many communication type, whereas requests are one-to-one.
We feel this covers most use cases. If not, please open an issue!

### Events

An event is a message that informs consumers that something has happened.

> [!WARNING]
> The provided implementation is an in-memory system.
> If your application crashes, there's no way to recover events.
> If your events needs to be consumed reliably, please use a message bus!

To define an event, create a type which implements the `IEvent` interface.

```csharp
public sealed record UserCreatedEvent(Guid Id, string Username) : IEvent;
```

Next you need to create an event handler.
To create one, create a type that implements the `IEventHandler<TEvent>` interface.

```csharp
public sealed class UserCreatedHandler : IEventHandler<UserCreatedEvent>
{
  public async Task HandleEventAsync(UserCreatedEvent data, CancellationToken _)
  {
    await Console.Out.WriteLineAsync($"User {data.Username} was created");
  }
}
```

Lastly, register your handler into the dependency injector container.

By default, event handlers have a `Transient` lifetime.
This can be changed by specifying the required lifetime when registering the handler.

```csharp
var services = new ServiceCollection();

// 👇 Add the in-memory mediator implementation.
services.AddInMemoryMediator();

// 👇 Add the event handler.
services.AddEventHandler<UserCreatedEvent, UserCreatedHandler>();
services.AddEventHandler<UserCreatedEvent, UserCreatedHandler>(ServiceLifetime.Scoped); // Custom lifetime
```

That's it! You're ready to publish events.

To publish an event, request the `IEventSender` or `IMediator` type from your dependency injection container.
Then call the `PublishEventAsync` method and let the mediator handle the rest.

```csharp
public sealed class UserService
{
  private readonly IEventSender _sender;

  public UserService(IEventSender sender)
  {
    _sender = sender;
  }

  public async Task CreateUserAsync(string username)
  {
    // ... User Creation Logic ...

    await _sender.PublishEventAsync(new UserCreatedEvent(username));
  }
}
```

### Requests

A request is a way to query for some information, or perform an action.

> [!WARNING]
> The provided implementation requires a request handler is registered.
> Sending a request when no handler is registered will throw an exception!

To define a request, create a type which implements the `IRequest<TResponse>` interface.
Your response type can be anything you want, but we recommend you make a dedicated type!

```csharp
public sealed record User(Guid Id, string Username); // Example domain model

public sealed record GetUserRequest(Guid Id) : IRequest<GetUserResponse>;

public sealed record GetUserResponse(User User);
```

Next you need to create a request handler.
To create one, create a type that implements the `IRequestHandler<TRequest, TResponse>` interface.

```csharp
public sealed class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
{
  public async Task<GetUserResponse> HandleRequestAsync(GetUserRequest request, CancellationToken _)
  {
    // ... User Query Logic ...

    return new GetUserResponse(user);
  }
}
```

Lastly, register your handler into the dependency injector container.

By default, event handlers have a `Transient` lifetime.
This can be changed by specifying the required lifetime when registering the handler.

```csharp
var services = new ServiceCollection();

// 👇 Add the in-memory mediator implementation.
services.AddInMemoryMediator();

// 👇 Add the request handler.
services.AddRequestHandler<GetUserRequest, GetUserResponse, GetUserHandler>();
services.AddRequestHandler<GetUserRequest, GetUserResponse, GetUserHandler>(ServiceLifetime.Scoped); // Custom lifetime
```

That's it! You're ready to send requests.

To send a request, request the `IRequestSender` or `IMediator` type from your dependency injection container.
Then call the `SendRequestAsync` method and let the mediator handle the rest.

```csharp
public sealed class FriendsListButton(IRequestSender sender)
{
  public async Task HandleClickAsync(Guid userId)
  {
    var request = new GetUserRequest(userId);
    var response = await sender.SendRequestAsync<GetUserRequest, GetUserResponse>(request);
    await Console.Out.WriteLineAsync(response.User.Username);
  }
}
```

Due to the limitations of generics, `SendRequestAsync` requires both the `TRequest` and `TResponse` types.
To get back some of your screen space, you can install the source generator.
The source generator creates an extension method for all of your `IRequest<TResponse>` types.
Making your code a lot cleaner:

```csharp
public sealed class FriendsListButton(IRequestSender sender)
{
  public async Task HandleClickAsync(Guid userId)
  {
    var request = new GetUserRequest(userId);
    var response = await sender.SendRequestAsync(request);
    await Console.Out.WriteLineAsync(response.User.Username);
  }
}
```

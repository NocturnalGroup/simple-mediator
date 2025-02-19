<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="center" style="list-style: none;">
    <summary>
      <h1>SimpleMediator</h1>
    </summary>
  </ul>
</div>

### A dead simple mediator implementation

About · [Getting Started](tutorial.md) · [License](license.txt) · [Contributing](contributing.md)

---

SimpleMediator is an implementation of the mediator pattern.
Out the box it supports events and requests, with an in-memory implementation.

## Installing

How you install SimpleMediator depends on the needs of your projects.

Every project that interacts with SimpleMediator should install the abstractions via [NuGet](https://www.nuget.org/packages/NocturnalGroup.SimpleMediator.Abstractions):

```shell
dotnet add package NocturnalGroup.SimpleMediator.Abstractions
```

Optionally, install the source generator via [NuGet](https://www.nuget.org/packages/NocturnalGroup.SimpleMediator.SourceGenerator):

```shell
dotnet add package NocturnalGroup.SimpleMediator.SourceGenerator
```

Then in your host project, either implement the interfaces or install the In-Memory implementations via [NuGet](https://www.nuget.org/packages/NocturnalGroup.SimpleMediator.InMemory):

```shell
dotnet add package NocturnalGroup.SimpleMediator.InMemory
```

## Quickstart

For a detailed walkthrough of SimpleMediator, check out our [tutorial](tutorial.md).

### Events

```csharp
var services = new ServiceCollection();

// 👇 Add the in-memory mediator implementation.
services.AddInMemoryMediator();

// 👇 Add the event handler.
services.AddEventHandler<UserCreatedEvent, UserCreatedHandler>(); // Transient lifetime
services.AddEventHandler<UserCreatedEvent, UserCreatedHandler>(ServiceLifetime.Scoped); // Custom lifetime

// 👇 Create an event type.
public sealed record UserCreatedEvent(string Username) : IEvent;

// 👇 Create an event handler.
public sealed class UserCreatedHandler : IEventHandler<UserCreatedEvent>
{
  public async Task HandleEventAsync(UserCreatedEvent data, CancellationToken _)
  {
    await Console.Out.WriteLineAsync($"User {data.Username} was created");
  }
}

// 👇 Publish an event.
public sealed class UserService(IEventSender sender)
{
  public async Task CreateUserAsync(string username)
  {
    // ... User Creation Logic ...

    await sender.PublishEventAsync(new UserCreatedEvent(username));
  }
}
```

### Requests

```csharp
var services = new ServiceCollection();

// 👇 Add the in-memory mediator implementation.
services.AddInMemoryMediator();

// 👇 Add the request handler.
services.AddRequestHandler<GetUserRequest, GetUserResponse, GetUserHandler>(); // Transient lifetime
services.AddRequestHandler<GetUserRequest, GetUserResponse, GetUserHandler>(ServiceLifetime.Scoped); // Custom lifetime

// 👇 Create a request type.
public sealed record GetUserRequest(Guid Id) : IRequest<GetUserResponse>;

// 👇 Create a response type.
public sealed record GetUserResponse(User User);

// 👇 Create a request handler.
public sealed class GetUserHandler : IRequestHandler<GetUserRequest, GetUserResponse>
{
  public async Task<GetUserResponse> HandleRequestAsync(GetUserRequest request, CancellationToken _)
  {
    // ... User Query Logic ...

    return new GetUserResponse(user);
  }
}

// 👇 Send a request.
public sealed class RequestSender(IRequestSender sender)
{
  public async Task HandleClickAsync(Guid userId)
  {
    // Without Source Generator
    var request = new GetUserRequest(userId);
    var response = await sender.SendRequestAsync<GetUserRequest, GetUserResponse>(request);
    await Console.Out.WriteLineAsync(response.User.Username);

    // With Source Generator
    var request = new GetUserRequest(userId);
    var response = await sender.SendRequestAsync(request);
    await Console.Out.WriteLineAsync(response.User.Username);
  }
}
```

## Versioning

We use [Semantic Versioning](https://semver.org/) to clearly communicate changes:

- Major version changes indicate breaking updates
- Minor version changes add features in a backward-compatible way
- Patch version changes include backward-compatible bug fixes

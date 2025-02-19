using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.InMemory.Events;
using Shouldly;

namespace NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Events;

public class EventSenderTests
{
	[Fact]
	public async Task PublishEventAsync_Should_DoNothing_When_NoHandlersRegistered()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		var sender = new EventSender(services.BuildServiceProvider(), new NullLogger<EventSender>());
		var act = () => sender.PublishEventAsync(new TestEvent(), null);

		// Assert
		await act.ShouldNotThrowAsync();
	}

	[Fact]
	public async Task PublishEventAsync_Should_ThrowException_When_HandlerThrowsException()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddTransient<IEventHandler<TestEvent>>(_ => new TestEventHandler(shouldThrow: true));
		services.AddTransient<IEventHandler<TestEvent>>(_ => new TestEventHandler(shouldThrow: true));

		// Act
		var mediator = new EventSender(services.BuildServiceProvider(), new NullLogger<EventSender>());
		var act = () => mediator.PublishEventAsync(new TestEvent(), null);

		// Assert
		await act.ShouldThrowAsync<AggregateException>();
	}

	[Fact]
	public async Task PublishEventAsync_Should_InvokeEventHandlers()
	{
		// Arrange
		var services = new ServiceCollection();
		var handlerA = new TestEventHandler();
		var handlerB = new TestEventHandler();
		services.AddTransient<IEventHandler<TestEvent>>(_ => handlerA);
		services.AddTransient<IEventHandler<TestEvent>>(_ => handlerB);

		// Act
		var mediator = new EventSender(services.BuildServiceProvider(), new NullLogger<EventSender>());
		await mediator.PublishEventAsync(new TestEvent(), null);

		// Assert
		handlerA.Invoked.ShouldBeTrue();
		handlerB.Invoked.ShouldBeTrue();
	}
}

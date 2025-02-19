using Microsoft.Extensions.DependencyInjection;
using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;
using NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Events;
using NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Requests;
using NSubstitute;

namespace NocturnalGroup.SimpleMediator.InMemory.Tests.Unit;

public class MediatorTests
{
	[Fact]
	public async Task PublishEventAsync_Should_InvokeEventSender()
	{
		// Arrange
		var testEvent = new TestEvent();
		var eventSender = Substitute.For<IEventSender>();
		var services = new ServiceCollection().AddSingleton(eventSender);

		// Act
		var mediator = new Mediator(services.BuildServiceProvider());
		await mediator.PublishEventAsync(testEvent);

		// Assert
		await eventSender.Received(1).PublishEventAsync(testEvent);
	}

	[Fact]
	public async Task SendRequestAsync_Should_InvokeRequestSender()
	{
		// Arrange
		var testRequest = new TestRequest();
		var requestSender = Substitute.For<IRequestSender>();
		var services = new ServiceCollection().AddSingleton(requestSender);

		// Act
		var mediator = new Mediator(services.BuildServiceProvider());
		await mediator.SendRequestAsync<TestRequest, TestResponse>(testRequest);

		// Assert
		await requestSender.Received(1).SendRequestAsync<TestRequest, TestResponse>(testRequest);
	}
}

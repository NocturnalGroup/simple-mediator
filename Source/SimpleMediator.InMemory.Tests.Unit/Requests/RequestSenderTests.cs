using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;
using NocturnalGroup.SimpleMediator.InMemory.Requests;
using Shouldly;

namespace NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Requests;

public class RequestSenderTests
{
	[Fact]
	public async Task SendRequestAsync_Should_ThrowException_When_NoHandlersRegistered()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		var sender = new RequestSender(services.BuildServiceProvider(), new NullLogger<RequestSender>());
		var act = () => sender.SendRequestAsync<TestRequest, TestResponse>(new TestRequest(), null);

		// Assert
		await act.ShouldThrowAsync<InvalidOperationException>();
	}

	[Fact]
	public async Task SendRequestAsync_Should_InvokeRequestHandler()
	{
		// Arrange
		var services = new ServiceCollection();
		var dummyHandler = new TestRequestHandler(); // Testing only a single handler is invoked.
		var actualHandler = new TestRequestHandler();
		services.AddTransient<IRequestHandler<TestRequest, TestResponse>>(_ => dummyHandler);
		services.AddTransient<IRequestHandler<TestRequest, TestResponse>>(_ => actualHandler);

		// Act
		var sender = new RequestSender(services.BuildServiceProvider(), new NullLogger<RequestSender>());
		var result = await sender.SendRequestAsync<TestRequest, TestResponse>(new TestRequest(), null);

		// Assert
		result.ShouldNotBeNull();
		dummyHandler.Invoked.ShouldBeFalse();
		actualHandler.Invoked.ShouldBeTrue();
		result.Id.ShouldBe(actualHandler.Response.Id);
	}
}

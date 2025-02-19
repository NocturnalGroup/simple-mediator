using Microsoft.Extensions.DependencyInjection;
using NocturnalGroup.SimpleMediator.Abstractions.Events;
using NocturnalGroup.SimpleMediator.Abstractions.Requests;
using NSubstitute;
using Shouldly;

namespace NocturnalGroup.SimpleMediator.SourceGenerator.Tests.Unit;

public class TestRequest : IRequest<TestResponse>;

public class TestResponse;

public class GeneratorTests
{
	[Fact]
	public async Task SendRequestAsync_Should_InvokeRequestSender()
	{
		// Arrange
		var testRequest = new TestRequest();
		var testResponse = new TestResponse();
		var requestSender = Substitute.For<IRequestSender>();
		requestSender.SendRequestAsync<TestRequest, TestResponse>(testRequest).Returns(Task.FromResult(testResponse));

		// Act
		var response = await requestSender.SendRequestAsync(testRequest);

		// Assert
		await requestSender.Received(1).SendRequestAsync<TestRequest, TestResponse>(testRequest);
		response.ShouldBe(testResponse);
	}
}

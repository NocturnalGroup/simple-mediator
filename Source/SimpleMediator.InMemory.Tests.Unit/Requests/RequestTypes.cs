using NocturnalGroup.SimpleMediator.Abstractions.Requests;

namespace NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Requests;

public record TestRequest : IRequest<TestResponse>;

public record TestResponse(Guid Id);

public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
	public bool Invoked { get; private set; }
	public TestResponse Response { get; } = new(Guid.NewGuid());

	public Task<TestResponse> HandleRequestAsync(TestRequest request, CancellationToken ct)
	{
		Invoked = true;
		return Task.FromResult(Response);
	}
}

using NocturnalGroup.SimpleMediator.Abstractions.Events;

namespace NocturnalGroup.SimpleMediator.InMemory.Tests.Unit.Events;

public class TestEvent : IEvent;

public class TestEventHandler : IEventHandler<TestEvent>
{
	public bool Invoked { get; private set; }
	private readonly bool _shouldThrow;

	public TestEventHandler(bool shouldThrow = false)
	{
		_shouldThrow = shouldThrow;
	}

	public Task HandleEventAsync(TestEvent _, CancellationToken __)
	{
		Invoked = true;
		if (_shouldThrow)
			throw new Exception("Test exception!");
		return Task.CompletedTask;
	}
}

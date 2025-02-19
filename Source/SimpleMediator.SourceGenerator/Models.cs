namespace NocturnalGroup.SimpleMediator.SourceGenerator;

/// <summary>
/// A request to generate an extension method for.
/// </summary>
internal sealed class RequestDefinition
{
	/// <summary>
	/// The fully qualified request type.
	/// </summary>
	public string RequestType { get; }

	/// <summary>
	/// The fully qualified response type.
	/// </summary>
	public string ResponseType { get; }

	public RequestDefinition(string requestType, string responseType)
	{
		RequestType = requestType;
		ResponseType = responseType;
	}
}

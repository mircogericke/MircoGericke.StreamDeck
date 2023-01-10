namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

public abstract class StreamDeckMessage
{
	public abstract string Event { get; }
}

public abstract class ContextMessage : StreamDeckMessage
{
	[JsonPropertyName("context")]
	public required ContextId ContextId { get; init; }
}

public abstract class StreamDeckMessage<T> : StreamDeckMessage
{
	public required T Payload { get; init; }
}

public abstract class ContextMessage<T> : ContextMessage
{
	public required T Payload { get; init; }
}

namespace MircoGericke.StreamDeck.Connection.Messages;

public abstract class StreamDeckMessage
{
	public abstract string Event { get; }
}

public abstract class PluginMessage : StreamDeckMessage
{
	public required string Context { get; init; }
}

public abstract class StreamDeckMessage<T> : StreamDeckMessage
{
	public required T Payload { get; init; }
}

public abstract class PluginMessage<T> : PluginMessage
{
	public required T Payload { get; init; }
}

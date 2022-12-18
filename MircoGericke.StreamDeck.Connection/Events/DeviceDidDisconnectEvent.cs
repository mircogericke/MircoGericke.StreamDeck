namespace MircoGericke.StreamDeck.Connection.Events;

public class DeviceDidDisconnectEvent : StreamDeckEvent
{
	public required string Device { get; init; }
}

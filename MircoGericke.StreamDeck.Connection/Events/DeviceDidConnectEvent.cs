namespace MircoGericke.StreamDeck.Connection.Events;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;

public class DeviceDidConnectEvent : StreamDeckEvent
{
	[JsonPropertyName("device")]
	public required DeviceId DeviceId { get; init; }
	public required DeviceInfoPayload DeviceInfo { get; init; }
}

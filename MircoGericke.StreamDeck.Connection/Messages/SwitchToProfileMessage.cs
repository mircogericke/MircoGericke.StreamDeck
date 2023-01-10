using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;

namespace MircoGericke.StreamDeck.Connection.Messages;
public class SwitchToProfileMessage : ContextMessage<SwitchToProfilePayload>
{
	public override string Event => "switchToProfile";

	[JsonPropertyName("device")]
	public required DeviceId DeviceId { get; init; }
}

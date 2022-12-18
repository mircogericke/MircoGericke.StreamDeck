using MircoGericke.StreamDeck.Connection.Payloads;

namespace MircoGericke.StreamDeck.Connection.Messages;
internal class SwitchToProfileMessage : PluginMessage<SwitchToProfilePayload>
{
	public override string Event => "switchToProfile";
	public required string Device { get; init; }
}

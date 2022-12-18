using MircoGericke.StreamDeck.Connection.Payloads;

namespace MircoGericke.StreamDeck.Connection.Messages;
internal class SetStateMessage : PluginMessage<SetStatePayload>
{
	public override string Event => "setState";
}

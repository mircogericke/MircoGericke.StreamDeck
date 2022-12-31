namespace MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Payloads;

internal class SetImageMessage : PluginMessage<SetImagePayload>
{
	public override string Event => "setImage";
}

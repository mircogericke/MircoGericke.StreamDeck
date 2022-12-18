namespace MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Payloads;

internal class SetTitleMessage : PluginMessage<SetTitlePayload>
{
	public override string Event => "setTitle";
}

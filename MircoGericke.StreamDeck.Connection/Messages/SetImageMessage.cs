namespace MircoGericke.StreamDeck.Connection.Messages;

using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;
using MircoGericke.StreamDeck.Connection.Payloads;

internal class SetImageMessage : PluginMessage<SetImagePayload>
{
	public override string Event => "setImage";
}

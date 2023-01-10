namespace MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Payloads;

public class SetImageMessage : ContextMessage<SetImagePayload>
{
	public override string Event => "setImage";
}

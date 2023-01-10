namespace MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Payloads;

public class SetTitleMessage : ContextMessage<SetTitlePayload>
{
	public override string Event => "setTitle";
}

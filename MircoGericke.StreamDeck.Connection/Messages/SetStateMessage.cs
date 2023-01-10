using MircoGericke.StreamDeck.Connection.Payloads;

namespace MircoGericke.StreamDeck.Connection.Messages;
public class SetStateMessage : ContextMessage<SetStatePayload>
{
	public override string Event => "setState";
}

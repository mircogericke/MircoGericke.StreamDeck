namespace MircoGericke.StreamDeck.Connection.Messages;
using System.Collections.Generic;

internal class SetFeedbackMessage : PluginMessage<Dictionary<string, string>>
{
	public override string Event => "setFeedback";
}

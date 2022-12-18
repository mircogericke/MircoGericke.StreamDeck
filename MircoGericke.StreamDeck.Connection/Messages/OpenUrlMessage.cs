namespace MircoGericke.StreamDeck.Connection.Messages;
using System;
using System.Diagnostics.CodeAnalysis;

using MircoGericke.StreamDeck.Connection.Payloads;

internal class OpenUrlMessage : StreamDeckMessage<OpenUrlPayload>
{
	public override string Event => "openUrl";

	[SetsRequiredMembers]
	public OpenUrlMessage(Uri uri)
	{
		Payload = new() { Url = uri.ToString() };
	}
}

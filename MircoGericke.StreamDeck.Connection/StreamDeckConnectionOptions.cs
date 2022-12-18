namespace MircoGericke.StreamDeck.Connection;

using System.Diagnostics;

[DebuggerDisplay("localost:{Port} [{Uuid}.{RegisterEvent}]")]
public class StreamDeckConnectionOptions
{
	/// <summary>
	/// The port used to connect to the StreamDeck websocket
	/// </summary>
	public required int Port { get; init; }

	/// <summary>
	/// This is the unique identifier used to communicate with the register StreamDeck plugin.
	/// </summary>
	public required string Uuid { get; init; }

	/// <summary>
	/// Name of the event we should pass to the StreamDeck app to register
	/// </summary>
	public required string RegisterEvent { get; init; }
}

namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload received when a dial is pressed or unpressed
/// </summary>
public class DialPressPayload
{
	/// <summary>
	/// Controller which issued the event
	/// </summary>
	public required string Controller { get; init; }

	/// <summary>
	/// Current event settings
	/// </summary>
	public required JsonObject Settings { get; init; }

	/// <summary>
	/// Coordinates of key on the stream deck
	/// </summary>
	public required KeyCoordinate Coordinates { get; init; }

	/// <summary>
	/// Boolean whether the dial is currently pressed or not
	/// </summary>
	[JsonPropertyName("pressed")]
	public required bool IsPressed { get; init; }
}

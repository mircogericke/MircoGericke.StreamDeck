namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload received when a dial is rotated
/// </summary>
public class DialRotatePayload
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
    /// Number of ticks rotated. Positive is to the right, negative to the left
    /// </summary>
    public required int Ticks { get; init; }

    /// <summary>
    /// Boolean whether the dial is currently pressed or not
    /// </summary>
    [JsonPropertyName("pressed")]
    public required bool IsDialPressed { get; init; }
}

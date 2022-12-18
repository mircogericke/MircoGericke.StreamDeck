namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload received when the touchpad (above the dials) is pressed
/// </summary>
public class TouchTapPayload
{
    /// <summary>
    /// Controller which issued the event
    /// </summary>
    public required string Controller { get; set; }

    /// <summary>
    /// Current event settings
    /// </summary>
    public JsonObject? Settings { get; set; }

    /// <summary>
    /// Coordinates of key on the stream deck
    /// </summary>
    public KeyCoordinate Coordinates { get; set; }

    /// <summary>
    /// Boolean whether it was a long press or not
    /// </summary>
    [JsonPropertyName("hold")]
    public bool IsLongPress { get; set; }

    /// <summary>
    /// Position on touchpad which was pressed
    /// </summary>
    [JsonPropertyName("tapPos")]
    public required int[] TapPosition { get; set; }
}

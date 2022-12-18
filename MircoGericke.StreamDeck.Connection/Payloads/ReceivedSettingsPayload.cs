namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Runtime.InteropServices.JavaScript;
using MircoGericke.StreamDeck.Connection.Model;

/// <summary>
/// Payload that holds all the settings in the ReceivedSettings event
/// </summary>
public class ReceivedSettingsPayload
{
    /// <summary>
    /// Action's settings
    /// </summary>
    public JSObject? Settings { get; init; }
    public required KeyCoordinate Coordinates { get; init; }
    public required bool IsInMultiAction { get; init; }
}

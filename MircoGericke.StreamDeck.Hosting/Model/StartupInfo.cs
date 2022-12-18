namespace MircoGericke.StreamDeck.Hosting.Model;

using System.Collections.Generic;

using MircoGericke.StreamDeck.Connection.Payloads;

/// <summary>
/// Class which holds information on the StreamDeck app, platform, devices and plugin.
/// </summary>
public class StartupInfo
{
    public required StartupPluginInfo Plugin { get; init; }
    public required StartupApplication Application { get; init; }
    public required int DevicePixelRatio { get; init; }
    public required IReadOnlyList<DeviceInfoPayload> Devices { get; init; }
}

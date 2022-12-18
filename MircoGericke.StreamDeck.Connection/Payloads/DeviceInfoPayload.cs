
namespace MircoGericke.StreamDeck.Connection.Payloads;

using System.Diagnostics;

using MircoGericke.StreamDeck.Connection.Model;

[DebuggerDisplay("{Name}[{Id}, {Type}] = {Size}")]
public class DeviceInfoPayload
{
    public required string Name { get; init; }
    public required DeviceSize Size { get; init; }
    public required DeviceType Type { get; init; }
}

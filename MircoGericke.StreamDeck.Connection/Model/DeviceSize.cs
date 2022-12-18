namespace MircoGericke.StreamDeck.Connection.Model;

using System.Diagnostics;

[DebuggerDisplay("{Columns}x{Rows}")]
public readonly record struct DeviceSize(int Columns, int Rows);

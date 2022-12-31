namespace MircoGericke.StreamDeck.Connection.Util;
using System.Text.Json;

// TODO: will be replaced with generated JsonSerializerContext once required and init-only properties land in the source generator
internal static class Constants
{
	public static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};
}

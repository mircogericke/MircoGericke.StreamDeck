namespace MircoGericke.StreamDeck.Connection.Util;
using System.Text.Json;

internal static class Constants
{
	public static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};
}

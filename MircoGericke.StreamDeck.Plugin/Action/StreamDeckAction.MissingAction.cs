using MircoGericke.StreamDeck.Plugin.Context;

namespace MircoGericke.StreamDeck.Plugin.Action;

public abstract partial class StreamDeckAction
{
	internal static StreamDeckAction Missing => MissingAction.Instance;

	private class MissingAction : StreamDeckAction
	{
		public static readonly StreamDeckAction Instance = new MissingAction(null!);

		private MissingAction(IActionContext context)
			: base(context) { }
	}
}

namespace MircoGericke.StreamDeck.Plugin;
using MircoGericke.StreamDeck.Connection.Model;

internal class ActionContext : IActionContext
{
	public ActionId ActionId { get; set; }
	public ContextId ContextId { get; set; }
	public DeviceId DeviceId { get; set; }
}

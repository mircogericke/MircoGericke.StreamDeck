namespace MircoGericke.StreamDeck.Plugin;
using MircoGericke.StreamDeck.Connection.Model;

public interface IActionContext
{
	public ActionId ActionId { get; }
	public ContextId ContextId { get; }
	public DeviceId DeviceId { get; }
}

namespace MircoGericke.StreamDeck.Plugin.Action;

using System.Threading.Tasks;

using MircoGericke.StreamDeck.Connection.Payloads;

public interface IKeypadAction : IStreamDeckAction
{
	Task OnKeyDown(KeyPayload payload, CancellationToken cancellationToken);
	Task OnKeyUp(KeyPayload payload, CancellationToken cancellationToken);
}

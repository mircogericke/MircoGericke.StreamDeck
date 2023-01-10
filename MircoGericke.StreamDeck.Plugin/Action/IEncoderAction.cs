namespace MircoGericke.StreamDeck.Plugin.Action;

using System.Threading.Tasks;

using MircoGericke.StreamDeck.Connection.Payloads;

public interface IEncoderAction : IStreamDeckAction
{
	Task OnDialRotate(DialRotatePayload payload, CancellationToken cancellationToken);
	Task OnDialPress(DialPressPayload payload, CancellationToken cancellationToken);
	Task OnTouchTap(TouchTapPayload payload, CancellationToken cancellationToken);
}
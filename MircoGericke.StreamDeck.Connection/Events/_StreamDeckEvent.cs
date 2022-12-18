namespace MircoGericke.StreamDeck.Connection.Events;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using MircoGericke.StreamDeck.Connection.Model;

public class StreamDeckEvent
{
	internal static Type TypeOf(string eventName) => eventName switch
	{
		"keyDown" => typeof(KeyDownEvent),
		"keyUp" => typeof(KeyUpEvent),
		"willAppear" => typeof(WillAppearEvent),
		"willDisappear" => typeof(WillDisappearEvent),
		"titleParametersDidChange" => typeof(TitleParametersDidChangeEvent),
		"deviceDidConnect" => typeof(DeviceDidConnectEvent),
		"deviceDidDisconnect" => typeof(DeviceDidDisconnectEvent),
		"applicationDidLaunch" => typeof(ApplicationDidLaunchEvent),
		"applicationDidTerminate" => typeof(ApplicationDidTerminateEvent),
		"systemDidWakeUp" => typeof(SystemDidWakeUpEvent),
		"didReceiveSettings" => typeof(DidReceiveSettingsEvent),
		"didReceiveGlobalSettings" => typeof(DidReceiveGlobalSettingsEvent),
		"propertyInspectorDidAppear" => typeof(PropertyInspectorDidAppearEvent),
		"propertyInspectorDidDisappear" => typeof(PropertyInspectorDidDisappearEvent),
		"sendToPlugin" => typeof(SendToPluginEvent),
		"dialRotate" => typeof(DialRotateEvent),
		"dialPress" => typeof(DialPressEvent),
		"touchTap" => typeof(TouchTapEvent),
		_ => typeof(StreamDeckEvent),
	};

	public required string Event { get; set; }

	[JsonExtensionData]
	public Dictionary<string, JsonElement>? ExtensionData { get; }
}

public class ActionEvent : StreamDeckEvent
{
	[JsonPropertyName("action")]
	public required ActionId ActionId { get; set; }

	[JsonPropertyName("context")]
	public required ContextId ContextId { get; set; }
}

public class ContextEvent : ActionEvent
{
	[JsonPropertyName("device")]
	public required DeviceId DeviceId { get; set; }
}

public class StreamDeckEvent<T> : StreamDeckEvent
{
	public required T Payload { get; set; }
}

public class ActionEvent<T> : ActionEvent
{
	public required T Payload { get; set; }
}

public class ContextEvent<T> : ContextEvent
{
	public required T Payload { get; set; }
}

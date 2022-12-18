﻿namespace MircoGericke.StreamDeck.Connection;
using System;
using System.Collections.Generic;

using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MircoGericke.StreamDeck.Connection.Events;
using MircoGericke.StreamDeck.Connection.Messages;
using MircoGericke.StreamDeck.Connection.Model;

public class StreamDeckConnection : IHostedService, IDisposable
{
	private readonly StreamDeckConnectionOptions options;
	private readonly ILogger<StreamDeckConnection> logger;

	private CancellationTokenSource? cts;
	private Task<StreamDeckSocket>? keepAliveTask;
	private bool disposedValue;

	private readonly Channel<StreamDeckMessage> sendingChannel = Channel.CreateUnbounded<StreamDeckMessage>(new()
	{
		AllowSynchronousContinuations = true,
		SingleReader = true,
		SingleWriter = false,
	});

	public StreamDeckConnection(ILogger<StreamDeckConnection> logger, StreamDeckConnectionOptions options)
	{
		this.logger = logger;
		this.options = options;
	}

	public virtual async Task StartAsync(CancellationToken cancellationToken)
	{
		logger.LogDebug("Starting");
		var socket = new StreamDeckSocket(options, sendingChannel, logger, DispatchEvent);
		await socket.ConnectAsync(cancellationToken);

		cts = new CancellationTokenSource();
		keepAliveTask = KeepAliveAsync(socket, cts.Token);
	}

	private async Task<StreamDeckSocket> KeepAliveAsync(StreamDeckSocket socket, CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			await SendAsync(new RegisterEventMessage(options.RegisterEvent, options.Uuid), cancellationToken);
			await OnConnected(EventArgs.Empty, cancellationToken);
			try
			{
				await socket.Connection.WaitAsync(cancellationToken);
				if (!cancellationToken.IsCancellationRequested)
				{
					await socket.DisconnectAsync(cancellationToken);
					await OnDisconnected(EventArgs.Empty, cancellationToken);
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Socket connection failed.");
			}
			if (!cancellationToken.IsCancellationRequested)
			{
				logger.LogDebug("Connection dropped unexpectedly, reconnecting.");
				socket.Dispose();
				socket = new StreamDeckSocket(options, sendingChannel, logger, DispatchEvent);
				await socket.ConnectAsync(cancellationToken);
			}
		}
		return socket;
	}

	public virtual async Task StopAsync(CancellationToken cancellationToken)
	{
		logger.LogDebug("Stopping");
		cts?.Cancel();
		if (keepAliveTask != null)
		{
			try
			{
				var socket = await keepAliveTask.WaitAsync(cancellationToken);
				try
				{
					if (!cancellationToken.IsCancellationRequested)
					{
						await socket.DisconnectAsync(cancellationToken);
						await OnDisconnected(EventArgs.Empty, cancellationToken);
					}
				}
				finally
				{
					socket.Dispose();
				}
			}
			catch (OperationCanceledException) { }
		}
	}

	#region events
	private Task DispatchEvent(StreamDeckEvent evt, CancellationToken cancellationToken) => evt switch
	{
		KeyDownEvent e => OnKeyDown(e, cancellationToken),
		KeyUpEvent e => OnKeyUp(e, cancellationToken),
		WillAppearEvent e => OnWillAppear(e, cancellationToken),
		WillDisappearEvent e => OnWillDisappear(e, cancellationToken),
		TitleParametersDidChangeEvent e => OnTitleParametersDidChange(e, cancellationToken),
		DeviceDidConnectEvent e => OnDeviceDidConnect(e, cancellationToken),
		DeviceDidDisconnectEvent e => OnDeviceDidDisconnect(e, cancellationToken),
		ApplicationDidLaunchEvent e => OnApplicationDidLaunch(e, cancellationToken),
		ApplicationDidTerminateEvent e => OnApplicationDidTerminate(e, cancellationToken),
		SystemDidWakeUpEvent e => OnSystemDidWakeUp(e, cancellationToken),
		DidReceiveSettingsEvent e => OnDidReceiveSettings(e, cancellationToken),
		DidReceiveGlobalSettingsEvent e => OnDidReceiveGlobalSettings(e, cancellationToken),
		PropertyInspectorDidAppearEvent e => OnPropertyInspectorDidAppear(e, cancellationToken),
		PropertyInspectorDidDisappearEvent e => OnPropertyInspectorDidDisappear(e, cancellationToken),
		SendToPluginEvent e => OnSendToPlugin(e, cancellationToken),
		DialRotateEvent e => OnDialRotate(e, cancellationToken),
		DialPressEvent e => OnDialPress(e, cancellationToken),
		TouchTapEvent e => OnTouchTap(e, cancellationToken),
		_ => OnUnknownEvent(evt, cancellationToken),
	};

	/// <summary>
	/// Called when plugin is connected to stream deck app
	/// </summary>
	protected virtual Task OnConnected(EventArgs e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when plugin is disconnected from stream deck app
	/// </summary>
	protected virtual Task OnDisconnected(EventArgs e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when key is pushed down
	/// </summary>
	protected virtual Task OnKeyDown(KeyDownEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when key is released
	/// </summary>
	protected virtual Task OnKeyUp(KeyUpEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when the action is shown, main trigger for a PluginAction
	/// </summary>
	protected virtual Task OnWillAppear(WillAppearEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when the action is no longer shown, main trigger for Dispose of PluginAction
	/// </summary>
	protected virtual Task OnWillDisappear(WillDisappearEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Contains information on the Title and its style
	/// </summary>
	protected virtual Task OnTitleParametersDidChange(TitleParametersDidChangeEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a Stream Deck device is connected to the PC
	/// </summary>
	protected virtual Task OnDeviceDidConnect(DeviceDidConnectEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a Stream Deck device has disconnected from the PC
	/// </summary>
	protected virtual Task OnDeviceDidDisconnect(DeviceDidDisconnectEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a monitored app is launched/active
	/// </summary>
	protected virtual Task OnApplicationDidLaunch(ApplicationDidLaunchEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a monitored app is terminated
	/// </summary>
	protected virtual Task OnApplicationDidTerminate(ApplicationDidTerminateEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called after the PC wakes up from sleep
	/// </summary>
	protected virtual Task OnSystemDidWakeUp(SystemDidWakeUpEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when settings for the action are received
	/// </summary>
	protected virtual Task OnDidReceiveSettings(DidReceiveSettingsEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when global settings for the entire plugin are received
	/// </summary>
	protected virtual Task OnDidReceiveGlobalSettings(DidReceiveGlobalSettingsEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when the user is viewing the settings in the Stream Deck app
	/// </summary>
	protected virtual Task OnPropertyInspectorDidAppear(PropertyInspectorDidAppearEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when the user stops viewing the settings in the Stream Deck app
	/// </summary>
	protected virtual Task OnPropertyInspectorDidDisappear(PropertyInspectorDidDisappearEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a payload is sent to the plugin from the PI
	/// </summary>
	protected virtual Task OnSendToPlugin(SendToPluginEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a dial is rotated
	/// </summary>
	protected virtual Task OnDialRotate(DialRotateEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when a dial is pressed or unpressed
	/// </summary>
	protected virtual Task OnDialPress(DialPressEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when the touch pad is pressed
	/// </summary>
	protected virtual Task OnTouchTap(TouchTapEvent e, CancellationToken cancellationToken) => Task.CompletedTask;

	/// <summary>
	/// Called when this library isn't aware of an event type
	/// </summary>
	protected virtual Task OnUnknownEvent(StreamDeckEvent e, CancellationToken cancellationToken)
	{
		logger.LogWarning("Unsupported Stream Deck event: {@event}", e);
		return Task.CompletedTask;
	}
	#endregion

	#region Requests
	protected ValueTask SendAsync(StreamDeckMessage message, CancellationToken cancellationToken = default)
		=> sendingChannel.Writer.WriteAsync(message, cancellationToken);

	public ValueTask SetTitleAsync(string title, string context, SdkTarget target, int? state)
	{
		return SendAsync(new SetTitleMessage
		{
			Context = context,
			Payload = new()
			{
				Title = title,
				Target = target,
				State = state,
			}
		});
	}

	public ValueTask LogMessageAsync(string message) => SendAsync(new LogMessage(message));

	public ValueTask SetImageAsync(string base64Image, string context, SdkTarget target, int? state)
	{
		return SendAsync(new SetImageMessage()
		{
			Context = context,
			Payload = new()
			{
				Image = base64Image,
				Target = target,
				State = state,
			}
		});
	}

	public ValueTask ShowAlertAsync(string context)
	{
		return SendAsync(new ShowAlertMessage
		{
			Context = context,
		});
	}

	public ValueTask ShowOkAsync(string context)
	{
		return SendAsync(new ShowOkMessage
		{
			Context = context
		});
	}

	public ValueTask SetGlobalSettingsAsync(JsonObject settings)
	{
		return SendAsync(new SetGlobalSettingsMessage
		{
			Context = options.Uuid,
			Payload = settings,
		});
	}

	public ValueTask GetGlobalSettingsAsync()
	{
		return SendAsync(new GetGlobalSettingsMessage
		{
			Context = options.Uuid,
		});
	}

	public ValueTask SetSettingsAsync(JsonObject settings, string context)
	{
		return SendAsync(new SetSettingsMessage
		{
			Context = context,
			Payload = settings,
		});
	}

	public ValueTask GetSettingsAsync(string context)
	{
		return SendAsync(new GetSettingsMessage
		{
			Context = context,
		});
	}

	public ValueTask SetStateAsync(uint state, string context)
	{
		return SendAsync(new SetStateMessage
		{
			Context = context,
			Payload = new() { State = state },
		});
	}

	public ValueTask SendToPropertyInspectorAsync(string action, JsonObject data, string context)
	{
		return SendAsync(new SendToPropertyInspectorMessage
		{
			Context = context,
			Action = action,
			Payload = data,
		});
	}

	public ValueTask SwitchToProfileAsync(string device, string profileName, string context)
	{
		return SendAsync(new SwitchToProfileMessage
		{
			Context = context,
			Device = device,
			Payload = new() { Profile = profileName }
		});
	}

	public ValueTask OpenUrlAsync(Uri uri) => SendAsync(new OpenUrlMessage(uri));

	public ValueTask SetFeedbackAsync(Dictionary<string, string> dictKeyValues, string context)
	{
		return SendAsync(new SetFeedbackMessage
		{
			Context = context,
			Payload = dictKeyValues,
		});
	}

	#endregion

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				cts?.Dispose();
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
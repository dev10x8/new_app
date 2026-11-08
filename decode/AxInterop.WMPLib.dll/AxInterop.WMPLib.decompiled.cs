using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WMPLib;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: TypeLibraryTimeStamp("03/26/2026 16:43:40")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyVersion("1.0.0.0")]
namespace AxWMPLib;

[DesignTimeVisible(true)]
[Clsid("{6bf52a52-394a-11d3-b153-00c04f79faa6}")]
public class AxWindowsMediaPlayer : AxHost
{
	private IWMPPlayer4 ocx;

	private AxWindowsMediaPlayerEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DispId(1)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string URL
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("URL", (ActiveXInvokeKind)1);
			}
			return ocx.URL;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("URL", (ActiveXInvokeKind)2);
			}
			ocx.URL = value;
		}
	}

	[DispId(2)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual WMPOpenState openState
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("openState", (ActiveXInvokeKind)1);
			}
			return ocx.openState;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(10)]
	public virtual WMPPlayState playState
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("playState", (ActiveXInvokeKind)1);
			}
			return ocx.playState;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(4)]
	public virtual IWMPControls Ctlcontrols
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Ctlcontrols", (ActiveXInvokeKind)1);
			}
			return ocx.controls;
		}
	}

	[DispId(5)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPSettings settings
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("settings", (ActiveXInvokeKind)1);
			}
			return ocx.settings;
		}
	}

	[DispId(6)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPMedia currentMedia
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("currentMedia", (ActiveXInvokeKind)1);
			}
			return ocx.currentMedia;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("currentMedia", (ActiveXInvokeKind)2);
			}
			ocx.currentMedia = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(8)]
	public virtual IWMPMediaCollection mediaCollection
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("mediaCollection", (ActiveXInvokeKind)1);
			}
			return ocx.mediaCollection;
		}
	}

	[DispId(9)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPPlaylistCollection playlistCollection
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("playlistCollection", (ActiveXInvokeKind)1);
			}
			return ocx.playlistCollection;
		}
	}

	[DispId(11)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string versionInfo
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("versionInfo", (ActiveXInvokeKind)1);
			}
			return ocx.versionInfo;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(7)]
	public virtual IWMPNetwork network
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("network", (ActiveXInvokeKind)1);
			}
			return ocx.network;
		}
	}

	[DispId(13)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPPlaylist currentPlaylist
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("currentPlaylist", (ActiveXInvokeKind)1);
			}
			return ocx.currentPlaylist;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("currentPlaylist", (ActiveXInvokeKind)2);
			}
			ocx.currentPlaylist = value;
		}
	}

	[DispId(14)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPCdromCollection cdromCollection
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("cdromCollection", (ActiveXInvokeKind)1);
			}
			return ocx.cdromCollection;
		}
	}

	[DispId(15)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public virtual IWMPClosedCaption closedCaption
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("closedCaption", (ActiveXInvokeKind)1);
			}
			return ocx.closedCaption;
		}
	}

	[DispId(16)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool isOnline
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("isOnline", (ActiveXInvokeKind)1);
			}
			return ocx.isOnline;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[DispId(17)]
	public virtual IWMPError Error
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Error", (ActiveXInvokeKind)1);
			}
			return ocx.Error;
		}
	}

	[DispId(18)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual string status
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("status", (ActiveXInvokeKind)1);
			}
			return ocx.status;
		}
	}

	[Browsable(false)]
	[DispId(40)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual IWMPDVD dvd
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("dvd", (ActiveXInvokeKind)1);
			}
			return ocx.dvd;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(19)]
	public virtual bool Ctlenabled
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Ctlenabled", (ActiveXInvokeKind)1);
			}
			return ocx.enabled;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Ctlenabled", (ActiveXInvokeKind)2);
			}
			ocx.enabled = value;
		}
	}

	[DispId(21)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool fullScreen
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("fullScreen", (ActiveXInvokeKind)1);
			}
			return ocx.fullScreen;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("fullScreen", (ActiveXInvokeKind)2);
			}
			ocx.fullScreen = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(22)]
	public virtual bool enableContextMenu
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("enableContextMenu", (ActiveXInvokeKind)1);
			}
			return ocx.enableContextMenu;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("enableContextMenu", (ActiveXInvokeKind)2);
			}
			ocx.enableContextMenu = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(23)]
	public virtual string uiMode
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("uiMode", (ActiveXInvokeKind)1);
			}
			return ocx.uiMode;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("uiMode", (ActiveXInvokeKind)2);
			}
			ocx.uiMode = value;
		}
	}

	[DispId(24)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool stretchToFit
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("stretchToFit", (ActiveXInvokeKind)1);
			}
			return ocx.stretchToFit;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("stretchToFit", (ActiveXInvokeKind)2);
			}
			ocx.stretchToFit = value;
		}
	}

	[DispId(25)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual bool windowlessVideo
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("windowlessVideo", (ActiveXInvokeKind)1);
			}
			return ocx.windowlessVideo;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("windowlessVideo", (ActiveXInvokeKind)2);
			}
			ocx.windowlessVideo = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(26)]
	public virtual bool isRemote
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("isRemote", (ActiveXInvokeKind)1);
			}
			return ocx.isRemote;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(27)]
	public virtual IWMPPlayerApplication playerApplication
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("playerApplication", (ActiveXInvokeKind)1);
			}
			return ocx.playerApplication;
		}
	}

	public event _WMPOCXEvents_OpenStateChangeEventHandler OpenStateChange;

	public event _WMPOCXEvents_PlayStateChangeEventHandler PlayStateChange;

	public event _WMPOCXEvents_AudioLanguageChangeEventHandler AudioLanguageChange;

	public event EventHandler StatusChange;

	public event _WMPOCXEvents_ScriptCommandEventHandler ScriptCommand;

	public event EventHandler NewStream;

	public event _WMPOCXEvents_DisconnectEventHandler Disconnect;

	public event _WMPOCXEvents_BufferingEventHandler Buffering;

	public event EventHandler ErrorEvent;

	public event _WMPOCXEvents_WarningEventHandler Warning;

	public event _WMPOCXEvents_EndOfStreamEventHandler EndOfStream;

	public event _WMPOCXEvents_PositionChangeEventHandler PositionChange;

	public event _WMPOCXEvents_MarkerHitEventHandler MarkerHit;

	public event _WMPOCXEvents_DurationUnitChangeEventHandler DurationUnitChange;

	public event _WMPOCXEvents_CdromMediaChangeEventHandler CdromMediaChange;

	public event _WMPOCXEvents_PlaylistChangeEventHandler PlaylistChange;

	public event _WMPOCXEvents_CurrentPlaylistChangeEventHandler CurrentPlaylistChange;

	public event _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler CurrentPlaylistItemAvailable;

	public event _WMPOCXEvents_MediaChangeEventHandler MediaChange;

	public event _WMPOCXEvents_CurrentMediaItemAvailableEventHandler CurrentMediaItemAvailable;

	public event _WMPOCXEvents_CurrentItemChangeEventHandler CurrentItemChange;

	public event EventHandler MediaCollectionChange;

	public event _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler MediaCollectionAttributeStringAdded;

	public event _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler MediaCollectionAttributeStringRemoved;

	public event _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler MediaCollectionAttributeStringChanged;

	public event EventHandler PlaylistCollectionChange;

	public event _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler PlaylistCollectionPlaylistAdded;

	public event _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler PlaylistCollectionPlaylistRemoved;

	public event _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler PlaylistCollectionPlaylistSetAsDeleted;

	public event _WMPOCXEvents_ModeChangeEventHandler ModeChange;

	public event _WMPOCXEvents_MediaErrorEventHandler MediaError;

	public event _WMPOCXEvents_OpenPlaylistSwitchEventHandler OpenPlaylistSwitch;

	public event _WMPOCXEvents_DomainChangeEventHandler DomainChange;

	public event EventHandler SwitchedToPlayerApplication;

	public event EventHandler SwitchedToControl;

	public event EventHandler PlayerDockedStateChange;

	public event EventHandler PlayerReconnect;

	public event _WMPOCXEvents_ClickEventHandler ClickEvent;

	public event _WMPOCXEvents_DoubleClickEventHandler DoubleClickEvent;

	public event _WMPOCXEvents_KeyDownEventHandler KeyDownEvent;

	public event _WMPOCXEvents_KeyPressEventHandler KeyPressEvent;

	public event _WMPOCXEvents_KeyUpEventHandler KeyUpEvent;

	public event _WMPOCXEvents_MouseDownEventHandler MouseDownEvent;

	public event _WMPOCXEvents_MouseMoveEventHandler MouseMoveEvent;

	public event _WMPOCXEvents_MouseUpEventHandler MouseUpEvent;

	public event _WMPOCXEvents_DeviceConnectEventHandler DeviceConnect;

	public event _WMPOCXEvents_DeviceDisconnectEventHandler DeviceDisconnect;

	public event _WMPOCXEvents_DeviceStatusChangeEventHandler DeviceStatusChange;

	public event _WMPOCXEvents_DeviceSyncStateChangeEventHandler DeviceSyncStateChange;

	public event _WMPOCXEvents_DeviceSyncErrorEventHandler DeviceSyncError;

	public event _WMPOCXEvents_CreatePartnershipCompleteEventHandler CreatePartnershipComplete;

	public event _WMPOCXEvents_DeviceEstimationEventHandler DeviceEstimation;

	public event _WMPOCXEvents_CdromRipStateChangeEventHandler CdromRipStateChange;

	public event _WMPOCXEvents_CdromRipMediaErrorEventHandler CdromRipMediaError;

	public event _WMPOCXEvents_CdromBurnStateChangeEventHandler CdromBurnStateChange;

	public event _WMPOCXEvents_CdromBurnMediaErrorEventHandler CdromBurnMediaError;

	public event _WMPOCXEvents_CdromBurnErrorEventHandler CdromBurnError;

	public event _WMPOCXEvents_LibraryConnectEventHandler LibraryConnect;

	public event _WMPOCXEvents_LibraryDisconnectEventHandler LibraryDisconnect;

	public event _WMPOCXEvents_FolderScanStateChangeEventHandler FolderScanStateChange;

	public event _WMPOCXEvents_StringCollectionChangeEventHandler StringCollectionChange;

	public event _WMPOCXEvents_MediaCollectionMediaAddedEventHandler MediaCollectionMediaAdded;

	public event _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler MediaCollectionMediaRemoved;

	public AxWindowsMediaPlayer()
		: base("6bf52a52-394a-11d3-b153-00c04f79faa6")
	{
	}

	public virtual void close()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("close", (ActiveXInvokeKind)0);
		}
		ocx.close();
	}

	public virtual void launchURL(string bstrURL)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("launchURL", (ActiveXInvokeKind)0);
		}
		ocx.launchURL(bstrURL);
	}

	public virtual IWMPPlaylist newPlaylist(string bstrName, string bstrURL)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("newPlaylist", (ActiveXInvokeKind)0);
		}
		return ocx.newPlaylist(bstrName, bstrURL);
	}

	public virtual IWMPMedia newMedia(string bstrURL)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("newMedia", (ActiveXInvokeKind)0);
		}
		return ocx.newMedia(bstrURL);
	}

	public virtual void openPlayer(string bstrURL)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("openPlayer", (ActiveXInvokeKind)0);
		}
		ocx.openPlayer(bstrURL);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxWindowsMediaPlayerEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(_WMPOCXEvents));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		try
		{
			ocx = (IWMPPlayer4)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnOpenStateChange(object sender, _WMPOCXEvents_OpenStateChangeEvent e)
	{
		if (this.OpenStateChange != null)
		{
			this.OpenStateChange(sender, e);
		}
	}

	internal void RaiseOnPlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
	{
		if (this.PlayStateChange != null)
		{
			this.PlayStateChange(sender, e);
		}
	}

	internal void RaiseOnAudioLanguageChange(object sender, _WMPOCXEvents_AudioLanguageChangeEvent e)
	{
		if (this.AudioLanguageChange != null)
		{
			this.AudioLanguageChange(sender, e);
		}
	}

	internal void RaiseOnStatusChange(object sender, EventArgs e)
	{
		if (this.StatusChange != null)
		{
			this.StatusChange(sender, e);
		}
	}

	internal void RaiseOnScriptCommand(object sender, _WMPOCXEvents_ScriptCommandEvent e)
	{
		if (this.ScriptCommand != null)
		{
			this.ScriptCommand(sender, e);
		}
	}

	internal void RaiseOnNewStream(object sender, EventArgs e)
	{
		if (this.NewStream != null)
		{
			this.NewStream(sender, e);
		}
	}

	internal void RaiseOnDisconnect(object sender, _WMPOCXEvents_DisconnectEvent e)
	{
		if (this.Disconnect != null)
		{
			this.Disconnect(sender, e);
		}
	}

	internal void RaiseOnBuffering(object sender, _WMPOCXEvents_BufferingEvent e)
	{
		if (this.Buffering != null)
		{
			this.Buffering(sender, e);
		}
	}

	internal void RaiseOnErrorEvent(object sender, EventArgs e)
	{
		if (this.ErrorEvent != null)
		{
			this.ErrorEvent(sender, e);
		}
	}

	internal void RaiseOnWarning(object sender, _WMPOCXEvents_WarningEvent e)
	{
		if (this.Warning != null)
		{
			this.Warning(sender, e);
		}
	}

	internal void RaiseOnEndOfStream(object sender, _WMPOCXEvents_EndOfStreamEvent e)
	{
		if (this.EndOfStream != null)
		{
			this.EndOfStream(sender, e);
		}
	}

	internal void RaiseOnPositionChange(object sender, _WMPOCXEvents_PositionChangeEvent e)
	{
		if (this.PositionChange != null)
		{
			this.PositionChange(sender, e);
		}
	}

	internal void RaiseOnMarkerHit(object sender, _WMPOCXEvents_MarkerHitEvent e)
	{
		if (this.MarkerHit != null)
		{
			this.MarkerHit(sender, e);
		}
	}

	internal void RaiseOnDurationUnitChange(object sender, _WMPOCXEvents_DurationUnitChangeEvent e)
	{
		if (this.DurationUnitChange != null)
		{
			this.DurationUnitChange(sender, e);
		}
	}

	internal void RaiseOnCdromMediaChange(object sender, _WMPOCXEvents_CdromMediaChangeEvent e)
	{
		if (this.CdromMediaChange != null)
		{
			this.CdromMediaChange(sender, e);
		}
	}

	internal void RaiseOnPlaylistChange(object sender, _WMPOCXEvents_PlaylistChangeEvent e)
	{
		if (this.PlaylistChange != null)
		{
			this.PlaylistChange(sender, e);
		}
	}

	internal void RaiseOnCurrentPlaylistChange(object sender, _WMPOCXEvents_CurrentPlaylistChangeEvent e)
	{
		if (this.CurrentPlaylistChange != null)
		{
			this.CurrentPlaylistChange(sender, e);
		}
	}

	internal void RaiseOnCurrentPlaylistItemAvailable(object sender, _WMPOCXEvents_CurrentPlaylistItemAvailableEvent e)
	{
		if (this.CurrentPlaylistItemAvailable != null)
		{
			this.CurrentPlaylistItemAvailable(sender, e);
		}
	}

	internal void RaiseOnMediaChange(object sender, _WMPOCXEvents_MediaChangeEvent e)
	{
		if (this.MediaChange != null)
		{
			this.MediaChange(sender, e);
		}
	}

	internal void RaiseOnCurrentMediaItemAvailable(object sender, _WMPOCXEvents_CurrentMediaItemAvailableEvent e)
	{
		if (this.CurrentMediaItemAvailable != null)
		{
			this.CurrentMediaItemAvailable(sender, e);
		}
	}

	internal void RaiseOnCurrentItemChange(object sender, _WMPOCXEvents_CurrentItemChangeEvent e)
	{
		if (this.CurrentItemChange != null)
		{
			this.CurrentItemChange(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionChange(object sender, EventArgs e)
	{
		if (this.MediaCollectionChange != null)
		{
			this.MediaCollectionChange(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionAttributeStringAdded(object sender, _WMPOCXEvents_MediaCollectionAttributeStringAddedEvent e)
	{
		if (this.MediaCollectionAttributeStringAdded != null)
		{
			this.MediaCollectionAttributeStringAdded(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionAttributeStringRemoved(object sender, _WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent e)
	{
		if (this.MediaCollectionAttributeStringRemoved != null)
		{
			this.MediaCollectionAttributeStringRemoved(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionAttributeStringChanged(object sender, _WMPOCXEvents_MediaCollectionAttributeStringChangedEvent e)
	{
		if (this.MediaCollectionAttributeStringChanged != null)
		{
			this.MediaCollectionAttributeStringChanged(sender, e);
		}
	}

	internal void RaiseOnPlaylistCollectionChange(object sender, EventArgs e)
	{
		if (this.PlaylistCollectionChange != null)
		{
			this.PlaylistCollectionChange(sender, e);
		}
	}

	internal void RaiseOnPlaylistCollectionPlaylistAdded(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent e)
	{
		if (this.PlaylistCollectionPlaylistAdded != null)
		{
			this.PlaylistCollectionPlaylistAdded(sender, e);
		}
	}

	internal void RaiseOnPlaylistCollectionPlaylistRemoved(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent e)
	{
		if (this.PlaylistCollectionPlaylistRemoved != null)
		{
			this.PlaylistCollectionPlaylistRemoved(sender, e);
		}
	}

	internal void RaiseOnPlaylistCollectionPlaylistSetAsDeleted(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent e)
	{
		if (this.PlaylistCollectionPlaylistSetAsDeleted != null)
		{
			this.PlaylistCollectionPlaylistSetAsDeleted(sender, e);
		}
	}

	internal void RaiseOnModeChange(object sender, _WMPOCXEvents_ModeChangeEvent e)
	{
		if (this.ModeChange != null)
		{
			this.ModeChange(sender, e);
		}
	}

	internal void RaiseOnMediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
	{
		if (this.MediaError != null)
		{
			this.MediaError(sender, e);
		}
	}

	internal void RaiseOnOpenPlaylistSwitch(object sender, _WMPOCXEvents_OpenPlaylistSwitchEvent e)
	{
		if (this.OpenPlaylistSwitch != null)
		{
			this.OpenPlaylistSwitch(sender, e);
		}
	}

	internal void RaiseOnDomainChange(object sender, _WMPOCXEvents_DomainChangeEvent e)
	{
		if (this.DomainChange != null)
		{
			this.DomainChange(sender, e);
		}
	}

	internal void RaiseOnSwitchedToPlayerApplication(object sender, EventArgs e)
	{
		if (this.SwitchedToPlayerApplication != null)
		{
			this.SwitchedToPlayerApplication(sender, e);
		}
	}

	internal void RaiseOnSwitchedToControl(object sender, EventArgs e)
	{
		if (this.SwitchedToControl != null)
		{
			this.SwitchedToControl(sender, e);
		}
	}

	internal void RaiseOnPlayerDockedStateChange(object sender, EventArgs e)
	{
		if (this.PlayerDockedStateChange != null)
		{
			this.PlayerDockedStateChange(sender, e);
		}
	}

	internal void RaiseOnPlayerReconnect(object sender, EventArgs e)
	{
		if (this.PlayerReconnect != null)
		{
			this.PlayerReconnect(sender, e);
		}
	}

	internal void RaiseOnClickEvent(object sender, _WMPOCXEvents_ClickEvent e)
	{
		if (this.ClickEvent != null)
		{
			this.ClickEvent(sender, e);
		}
	}

	internal void RaiseOnDoubleClickEvent(object sender, _WMPOCXEvents_DoubleClickEvent e)
	{
		if (this.DoubleClickEvent != null)
		{
			this.DoubleClickEvent(sender, e);
		}
	}

	internal void RaiseOnKeyDownEvent(object sender, _WMPOCXEvents_KeyDownEvent e)
	{
		if (this.KeyDownEvent != null)
		{
			this.KeyDownEvent(sender, e);
		}
	}

	internal void RaiseOnKeyPressEvent(object sender, _WMPOCXEvents_KeyPressEvent e)
	{
		if (this.KeyPressEvent != null)
		{
			this.KeyPressEvent(sender, e);
		}
	}

	internal void RaiseOnKeyUpEvent(object sender, _WMPOCXEvents_KeyUpEvent e)
	{
		if (this.KeyUpEvent != null)
		{
			this.KeyUpEvent(sender, e);
		}
	}

	internal void RaiseOnMouseDownEvent(object sender, _WMPOCXEvents_MouseDownEvent e)
	{
		if (this.MouseDownEvent != null)
		{
			this.MouseDownEvent(sender, e);
		}
	}

	internal void RaiseOnMouseMoveEvent(object sender, _WMPOCXEvents_MouseMoveEvent e)
	{
		if (this.MouseMoveEvent != null)
		{
			this.MouseMoveEvent(sender, e);
		}
	}

	internal void RaiseOnMouseUpEvent(object sender, _WMPOCXEvents_MouseUpEvent e)
	{
		if (this.MouseUpEvent != null)
		{
			this.MouseUpEvent(sender, e);
		}
	}

	internal void RaiseOnDeviceConnect(object sender, _WMPOCXEvents_DeviceConnectEvent e)
	{
		if (this.DeviceConnect != null)
		{
			this.DeviceConnect(sender, e);
		}
	}

	internal void RaiseOnDeviceDisconnect(object sender, _WMPOCXEvents_DeviceDisconnectEvent e)
	{
		if (this.DeviceDisconnect != null)
		{
			this.DeviceDisconnect(sender, e);
		}
	}

	internal void RaiseOnDeviceStatusChange(object sender, _WMPOCXEvents_DeviceStatusChangeEvent e)
	{
		if (this.DeviceStatusChange != null)
		{
			this.DeviceStatusChange(sender, e);
		}
	}

	internal void RaiseOnDeviceSyncStateChange(object sender, _WMPOCXEvents_DeviceSyncStateChangeEvent e)
	{
		if (this.DeviceSyncStateChange != null)
		{
			this.DeviceSyncStateChange(sender, e);
		}
	}

	internal void RaiseOnDeviceSyncError(object sender, _WMPOCXEvents_DeviceSyncErrorEvent e)
	{
		if (this.DeviceSyncError != null)
		{
			this.DeviceSyncError(sender, e);
		}
	}

	internal void RaiseOnCreatePartnershipComplete(object sender, _WMPOCXEvents_CreatePartnershipCompleteEvent e)
	{
		if (this.CreatePartnershipComplete != null)
		{
			this.CreatePartnershipComplete(sender, e);
		}
	}

	internal void RaiseOnDeviceEstimation(object sender, _WMPOCXEvents_DeviceEstimationEvent e)
	{
		if (this.DeviceEstimation != null)
		{
			this.DeviceEstimation(sender, e);
		}
	}

	internal void RaiseOnCdromRipStateChange(object sender, _WMPOCXEvents_CdromRipStateChangeEvent e)
	{
		if (this.CdromRipStateChange != null)
		{
			this.CdromRipStateChange(sender, e);
		}
	}

	internal void RaiseOnCdromRipMediaError(object sender, _WMPOCXEvents_CdromRipMediaErrorEvent e)
	{
		if (this.CdromRipMediaError != null)
		{
			this.CdromRipMediaError(sender, e);
		}
	}

	internal void RaiseOnCdromBurnStateChange(object sender, _WMPOCXEvents_CdromBurnStateChangeEvent e)
	{
		if (this.CdromBurnStateChange != null)
		{
			this.CdromBurnStateChange(sender, e);
		}
	}

	internal void RaiseOnCdromBurnMediaError(object sender, _WMPOCXEvents_CdromBurnMediaErrorEvent e)
	{
		if (this.CdromBurnMediaError != null)
		{
			this.CdromBurnMediaError(sender, e);
		}
	}

	internal void RaiseOnCdromBurnError(object sender, _WMPOCXEvents_CdromBurnErrorEvent e)
	{
		if (this.CdromBurnError != null)
		{
			this.CdromBurnError(sender, e);
		}
	}

	internal void RaiseOnLibraryConnect(object sender, _WMPOCXEvents_LibraryConnectEvent e)
	{
		if (this.LibraryConnect != null)
		{
			this.LibraryConnect(sender, e);
		}
	}

	internal void RaiseOnLibraryDisconnect(object sender, _WMPOCXEvents_LibraryDisconnectEvent e)
	{
		if (this.LibraryDisconnect != null)
		{
			this.LibraryDisconnect(sender, e);
		}
	}

	internal void RaiseOnFolderScanStateChange(object sender, _WMPOCXEvents_FolderScanStateChangeEvent e)
	{
		if (this.FolderScanStateChange != null)
		{
			this.FolderScanStateChange(sender, e);
		}
	}

	internal void RaiseOnStringCollectionChange(object sender, _WMPOCXEvents_StringCollectionChangeEvent e)
	{
		if (this.StringCollectionChange != null)
		{
			this.StringCollectionChange(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionMediaAdded(object sender, _WMPOCXEvents_MediaCollectionMediaAddedEvent e)
	{
		if (this.MediaCollectionMediaAdded != null)
		{
			this.MediaCollectionMediaAdded(sender, e);
		}
	}

	internal void RaiseOnMediaCollectionMediaRemoved(object sender, _WMPOCXEvents_MediaCollectionMediaRemovedEvent e)
	{
		if (this.MediaCollectionMediaRemoved != null)
		{
			this.MediaCollectionMediaRemoved(sender, e);
		}
	}
}
public delegate void _WMPOCXEvents_OpenStateChangeEventHandler(object sender, _WMPOCXEvents_OpenStateChangeEvent e);
public class _WMPOCXEvents_OpenStateChangeEvent
{
	public int newState;

	public _WMPOCXEvents_OpenStateChangeEvent(int newState)
	{
		this.newState = newState;
	}
}
public delegate void _WMPOCXEvents_PlayStateChangeEventHandler(object sender, _WMPOCXEvents_PlayStateChangeEvent e);
public class _WMPOCXEvents_PlayStateChangeEvent
{
	public int newState;

	public _WMPOCXEvents_PlayStateChangeEvent(int newState)
	{
		this.newState = newState;
	}
}
public delegate void _WMPOCXEvents_AudioLanguageChangeEventHandler(object sender, _WMPOCXEvents_AudioLanguageChangeEvent e);
public class _WMPOCXEvents_AudioLanguageChangeEvent
{
	public int langID;

	public _WMPOCXEvents_AudioLanguageChangeEvent(int langID)
	{
		this.langID = langID;
	}
}
public delegate void _WMPOCXEvents_ScriptCommandEventHandler(object sender, _WMPOCXEvents_ScriptCommandEvent e);
public class _WMPOCXEvents_ScriptCommandEvent
{
	public string scType;

	public string param;

	public _WMPOCXEvents_ScriptCommandEvent(string scType, string param)
	{
		this.scType = scType;
		this.param = param;
	}
}
public delegate void _WMPOCXEvents_DisconnectEventHandler(object sender, _WMPOCXEvents_DisconnectEvent e);
public class _WMPOCXEvents_DisconnectEvent
{
	public int result;

	public _WMPOCXEvents_DisconnectEvent(int result)
	{
		this.result = result;
	}
}
public delegate void _WMPOCXEvents_BufferingEventHandler(object sender, _WMPOCXEvents_BufferingEvent e);
public class _WMPOCXEvents_BufferingEvent
{
	public bool start;

	public _WMPOCXEvents_BufferingEvent(bool start)
	{
		this.start = start;
	}
}
public delegate void _WMPOCXEvents_WarningEventHandler(object sender, _WMPOCXEvents_WarningEvent e);
public class _WMPOCXEvents_WarningEvent
{
	public int warningType;

	public int param;

	public string description;

	public _WMPOCXEvents_WarningEvent(int warningType, int param, string description)
	{
		this.warningType = warningType;
		this.param = param;
		this.description = description;
	}
}
public delegate void _WMPOCXEvents_EndOfStreamEventHandler(object sender, _WMPOCXEvents_EndOfStreamEvent e);
public class _WMPOCXEvents_EndOfStreamEvent
{
	public int result;

	public _WMPOCXEvents_EndOfStreamEvent(int result)
	{
		this.result = result;
	}
}
public delegate void _WMPOCXEvents_PositionChangeEventHandler(object sender, _WMPOCXEvents_PositionChangeEvent e);
public class _WMPOCXEvents_PositionChangeEvent
{
	public double oldPosition;

	public double newPosition;

	public _WMPOCXEvents_PositionChangeEvent(double oldPosition, double newPosition)
	{
		this.oldPosition = oldPosition;
		this.newPosition = newPosition;
	}
}
public delegate void _WMPOCXEvents_MarkerHitEventHandler(object sender, _WMPOCXEvents_MarkerHitEvent e);
public class _WMPOCXEvents_MarkerHitEvent
{
	public int markerNum;

	public _WMPOCXEvents_MarkerHitEvent(int markerNum)
	{
		this.markerNum = markerNum;
	}
}
public delegate void _WMPOCXEvents_DurationUnitChangeEventHandler(object sender, _WMPOCXEvents_DurationUnitChangeEvent e);
public class _WMPOCXEvents_DurationUnitChangeEvent
{
	public int newDurationUnit;

	public _WMPOCXEvents_DurationUnitChangeEvent(int newDurationUnit)
	{
		this.newDurationUnit = newDurationUnit;
	}
}
public delegate void _WMPOCXEvents_CdromMediaChangeEventHandler(object sender, _WMPOCXEvents_CdromMediaChangeEvent e);
public class _WMPOCXEvents_CdromMediaChangeEvent
{
	public int cdromNum;

	public _WMPOCXEvents_CdromMediaChangeEvent(int cdromNum)
	{
		this.cdromNum = cdromNum;
	}
}
public delegate void _WMPOCXEvents_PlaylistChangeEventHandler(object sender, _WMPOCXEvents_PlaylistChangeEvent e);
public class _WMPOCXEvents_PlaylistChangeEvent
{
	public object playlist;

	public WMPPlaylistChangeEventType change;

	public _WMPOCXEvents_PlaylistChangeEvent(object playlist, WMPPlaylistChangeEventType change)
	{
		this.playlist = playlist;
		this.change = change;
	}
}
public delegate void _WMPOCXEvents_CurrentPlaylistChangeEventHandler(object sender, _WMPOCXEvents_CurrentPlaylistChangeEvent e);
public class _WMPOCXEvents_CurrentPlaylistChangeEvent
{
	public WMPPlaylistChangeEventType change;

	public _WMPOCXEvents_CurrentPlaylistChangeEvent(WMPPlaylistChangeEventType change)
	{
		this.change = change;
	}
}
public delegate void _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler(object sender, _WMPOCXEvents_CurrentPlaylistItemAvailableEvent e);
public class _WMPOCXEvents_CurrentPlaylistItemAvailableEvent
{
	public string bstrItemName;

	public _WMPOCXEvents_CurrentPlaylistItemAvailableEvent(string bstrItemName)
	{
		this.bstrItemName = bstrItemName;
	}
}
public delegate void _WMPOCXEvents_MediaChangeEventHandler(object sender, _WMPOCXEvents_MediaChangeEvent e);
public class _WMPOCXEvents_MediaChangeEvent
{
	public object item;

	public _WMPOCXEvents_MediaChangeEvent(object item)
	{
		this.item = item;
	}
}
public delegate void _WMPOCXEvents_CurrentMediaItemAvailableEventHandler(object sender, _WMPOCXEvents_CurrentMediaItemAvailableEvent e);
public class _WMPOCXEvents_CurrentMediaItemAvailableEvent
{
	public string bstrItemName;

	public _WMPOCXEvents_CurrentMediaItemAvailableEvent(string bstrItemName)
	{
		this.bstrItemName = bstrItemName;
	}
}
public delegate void _WMPOCXEvents_CurrentItemChangeEventHandler(object sender, _WMPOCXEvents_CurrentItemChangeEvent e);
public class _WMPOCXEvents_CurrentItemChangeEvent
{
	public object pdispMedia;

	public _WMPOCXEvents_CurrentItemChangeEvent(object pdispMedia)
	{
		this.pdispMedia = pdispMedia;
	}
}
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler(object sender, _WMPOCXEvents_MediaCollectionAttributeStringAddedEvent e);
public class _WMPOCXEvents_MediaCollectionAttributeStringAddedEvent
{
	public string bstrAttribName;

	public string bstrAttribVal;

	public _WMPOCXEvents_MediaCollectionAttributeStringAddedEvent(string bstrAttribName, string bstrAttribVal)
	{
		this.bstrAttribName = bstrAttribName;
		this.bstrAttribVal = bstrAttribVal;
	}
}
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler(object sender, _WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent e);
public class _WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent
{
	public string bstrAttribName;

	public string bstrAttribVal;

	public _WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent(string bstrAttribName, string bstrAttribVal)
	{
		this.bstrAttribName = bstrAttribName;
		this.bstrAttribVal = bstrAttribVal;
	}
}
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler(object sender, _WMPOCXEvents_MediaCollectionAttributeStringChangedEvent e);
public class _WMPOCXEvents_MediaCollectionAttributeStringChangedEvent
{
	public string bstrAttribName;

	public string bstrOldAttribVal;

	public string bstrNewAttribVal;

	public _WMPOCXEvents_MediaCollectionAttributeStringChangedEvent(string bstrAttribName, string bstrOldAttribVal, string bstrNewAttribVal)
	{
		this.bstrAttribName = bstrAttribName;
		this.bstrOldAttribVal = bstrOldAttribVal;
		this.bstrNewAttribVal = bstrNewAttribVal;
	}
}
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent e);
public class _WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent
{
	public string bstrPlaylistName;

	public _WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent(string bstrPlaylistName)
	{
		this.bstrPlaylistName = bstrPlaylistName;
	}
}
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent e);
public class _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent
{
	public string bstrPlaylistName;

	public _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent(string bstrPlaylistName)
	{
		this.bstrPlaylistName = bstrPlaylistName;
	}
}
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler(object sender, _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent e);
public class _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent
{
	public string bstrPlaylistName;

	public bool varfIsDeleted;

	public _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent(string bstrPlaylistName, bool varfIsDeleted)
	{
		this.bstrPlaylistName = bstrPlaylistName;
		this.varfIsDeleted = varfIsDeleted;
	}
}
public delegate void _WMPOCXEvents_ModeChangeEventHandler(object sender, _WMPOCXEvents_ModeChangeEvent e);
public class _WMPOCXEvents_ModeChangeEvent
{
	public string modeName;

	public bool newValue;

	public _WMPOCXEvents_ModeChangeEvent(string modeName, bool newValue)
	{
		this.modeName = modeName;
		this.newValue = newValue;
	}
}
public delegate void _WMPOCXEvents_MediaErrorEventHandler(object sender, _WMPOCXEvents_MediaErrorEvent e);
public class _WMPOCXEvents_MediaErrorEvent
{
	public object pMediaObject;

	public _WMPOCXEvents_MediaErrorEvent(object pMediaObject)
	{
		this.pMediaObject = pMediaObject;
	}
}
public delegate void _WMPOCXEvents_OpenPlaylistSwitchEventHandler(object sender, _WMPOCXEvents_OpenPlaylistSwitchEvent e);
public class _WMPOCXEvents_OpenPlaylistSwitchEvent
{
	public object pItem;

	public _WMPOCXEvents_OpenPlaylistSwitchEvent(object pItem)
	{
		this.pItem = pItem;
	}
}
public delegate void _WMPOCXEvents_DomainChangeEventHandler(object sender, _WMPOCXEvents_DomainChangeEvent e);
public class _WMPOCXEvents_DomainChangeEvent
{
	public string strDomain;

	public _WMPOCXEvents_DomainChangeEvent(string strDomain)
	{
		this.strDomain = strDomain;
	}
}
public delegate void _WMPOCXEvents_ClickEventHandler(object sender, _WMPOCXEvents_ClickEvent e);
public class _WMPOCXEvents_ClickEvent
{
	public short nButton;

	public short nShiftState;

	public int fX;

	public int fY;

	public _WMPOCXEvents_ClickEvent(short nButton, short nShiftState, int fX, int fY)
	{
		this.nButton = nButton;
		this.nShiftState = nShiftState;
		this.fX = fX;
		this.fY = fY;
	}
}
public delegate void _WMPOCXEvents_DoubleClickEventHandler(object sender, _WMPOCXEvents_DoubleClickEvent e);
public class _WMPOCXEvents_DoubleClickEvent
{
	public short nButton;

	public short nShiftState;

	public int fX;

	public int fY;

	public _WMPOCXEvents_DoubleClickEvent(short nButton, short nShiftState, int fX, int fY)
	{
		this.nButton = nButton;
		this.nShiftState = nShiftState;
		this.fX = fX;
		this.fY = fY;
	}
}
public delegate void _WMPOCXEvents_KeyDownEventHandler(object sender, _WMPOCXEvents_KeyDownEvent e);
public class _WMPOCXEvents_KeyDownEvent
{
	public short nKeyCode;

	public short nShiftState;

	public _WMPOCXEvents_KeyDownEvent(short nKeyCode, short nShiftState)
	{
		this.nKeyCode = nKeyCode;
		this.nShiftState = nShiftState;
	}
}
public delegate void _WMPOCXEvents_KeyPressEventHandler(object sender, _WMPOCXEvents_KeyPressEvent e);
public class _WMPOCXEvents_KeyPressEvent
{
	public short nKeyAscii;

	public _WMPOCXEvents_KeyPressEvent(short nKeyAscii)
	{
		this.nKeyAscii = nKeyAscii;
	}
}
public delegate void _WMPOCXEvents_KeyUpEventHandler(object sender, _WMPOCXEvents_KeyUpEvent e);
public class _WMPOCXEvents_KeyUpEvent
{
	public short nKeyCode;

	public short nShiftState;

	public _WMPOCXEvents_KeyUpEvent(short nKeyCode, short nShiftState)
	{
		this.nKeyCode = nKeyCode;
		this.nShiftState = nShiftState;
	}
}
public delegate void _WMPOCXEvents_MouseDownEventHandler(object sender, _WMPOCXEvents_MouseDownEvent e);
public class _WMPOCXEvents_MouseDownEvent
{
	public short nButton;

	public short nShiftState;

	public int fX;

	public int fY;

	public _WMPOCXEvents_MouseDownEvent(short nButton, short nShiftState, int fX, int fY)
	{
		this.nButton = nButton;
		this.nShiftState = nShiftState;
		this.fX = fX;
		this.fY = fY;
	}
}
public delegate void _WMPOCXEvents_MouseMoveEventHandler(object sender, _WMPOCXEvents_MouseMoveEvent e);
public class _WMPOCXEvents_MouseMoveEvent
{
	public short nButton;

	public short nShiftState;

	public int fX;

	public int fY;

	public _WMPOCXEvents_MouseMoveEvent(short nButton, short nShiftState, int fX, int fY)
	{
		this.nButton = nButton;
		this.nShiftState = nShiftState;
		this.fX = fX;
		this.fY = fY;
	}
}
public delegate void _WMPOCXEvents_MouseUpEventHandler(object sender, _WMPOCXEvents_MouseUpEvent e);
public class _WMPOCXEvents_MouseUpEvent
{
	public short nButton;

	public short nShiftState;

	public int fX;

	public int fY;

	public _WMPOCXEvents_MouseUpEvent(short nButton, short nShiftState, int fX, int fY)
	{
		this.nButton = nButton;
		this.nShiftState = nShiftState;
		this.fX = fX;
		this.fY = fY;
	}
}
public delegate void _WMPOCXEvents_DeviceConnectEventHandler(object sender, _WMPOCXEvents_DeviceConnectEvent e);
public class _WMPOCXEvents_DeviceConnectEvent
{
	public IWMPSyncDevice pDevice;

	public _WMPOCXEvents_DeviceConnectEvent(IWMPSyncDevice pDevice)
	{
		this.pDevice = pDevice;
	}
}
public delegate void _WMPOCXEvents_DeviceDisconnectEventHandler(object sender, _WMPOCXEvents_DeviceDisconnectEvent e);
public class _WMPOCXEvents_DeviceDisconnectEvent
{
	public IWMPSyncDevice pDevice;

	public _WMPOCXEvents_DeviceDisconnectEvent(IWMPSyncDevice pDevice)
	{
		this.pDevice = pDevice;
	}
}
public delegate void _WMPOCXEvents_DeviceStatusChangeEventHandler(object sender, _WMPOCXEvents_DeviceStatusChangeEvent e);
public class _WMPOCXEvents_DeviceStatusChangeEvent
{
	public IWMPSyncDevice pDevice;

	public WMPDeviceStatus newStatus;

	public _WMPOCXEvents_DeviceStatusChangeEvent(IWMPSyncDevice pDevice, WMPDeviceStatus newStatus)
	{
		this.pDevice = pDevice;
		this.newStatus = newStatus;
	}
}
public delegate void _WMPOCXEvents_DeviceSyncStateChangeEventHandler(object sender, _WMPOCXEvents_DeviceSyncStateChangeEvent e);
public class _WMPOCXEvents_DeviceSyncStateChangeEvent
{
	public IWMPSyncDevice pDevice;

	public WMPSyncState newState;

	public _WMPOCXEvents_DeviceSyncStateChangeEvent(IWMPSyncDevice pDevice, WMPSyncState newState)
	{
		this.pDevice = pDevice;
		this.newState = newState;
	}
}
public delegate void _WMPOCXEvents_DeviceSyncErrorEventHandler(object sender, _WMPOCXEvents_DeviceSyncErrorEvent e);
public class _WMPOCXEvents_DeviceSyncErrorEvent
{
	public IWMPSyncDevice pDevice;

	public object pMedia;

	public _WMPOCXEvents_DeviceSyncErrorEvent(IWMPSyncDevice pDevice, object pMedia)
	{
		this.pDevice = pDevice;
		this.pMedia = pMedia;
	}
}
public delegate void _WMPOCXEvents_CreatePartnershipCompleteEventHandler(object sender, _WMPOCXEvents_CreatePartnershipCompleteEvent e);
public class _WMPOCXEvents_CreatePartnershipCompleteEvent
{
	public IWMPSyncDevice pDevice;

	public int hrResult;

	public _WMPOCXEvents_CreatePartnershipCompleteEvent(IWMPSyncDevice pDevice, int hrResult)
	{
		this.pDevice = pDevice;
		this.hrResult = hrResult;
	}
}
public delegate void _WMPOCXEvents_DeviceEstimationEventHandler(object sender, _WMPOCXEvents_DeviceEstimationEvent e);
public class _WMPOCXEvents_DeviceEstimationEvent
{
	public IWMPSyncDevice pDevice;

	public int hrResult;

	public long qwEstimatedUsedSpace;

	public long qwEstimatedSpace;

	public _WMPOCXEvents_DeviceEstimationEvent(IWMPSyncDevice pDevice, int hrResult, long qwEstimatedUsedSpace, long qwEstimatedSpace)
	{
		this.pDevice = pDevice;
		this.hrResult = hrResult;
		this.qwEstimatedUsedSpace = qwEstimatedUsedSpace;
		this.qwEstimatedSpace = qwEstimatedSpace;
	}
}
public delegate void _WMPOCXEvents_CdromRipStateChangeEventHandler(object sender, _WMPOCXEvents_CdromRipStateChangeEvent e);
public class _WMPOCXEvents_CdromRipStateChangeEvent
{
	public IWMPCdromRip pCdromRip;

	public WMPRipState wmprs;

	public _WMPOCXEvents_CdromRipStateChangeEvent(IWMPCdromRip pCdromRip, WMPRipState wmprs)
	{
		this.pCdromRip = pCdromRip;
		this.wmprs = wmprs;
	}
}
public delegate void _WMPOCXEvents_CdromRipMediaErrorEventHandler(object sender, _WMPOCXEvents_CdromRipMediaErrorEvent e);
public class _WMPOCXEvents_CdromRipMediaErrorEvent
{
	public IWMPCdromRip pCdromRip;

	public object pMedia;

	public _WMPOCXEvents_CdromRipMediaErrorEvent(IWMPCdromRip pCdromRip, object pMedia)
	{
		this.pCdromRip = pCdromRip;
		this.pMedia = pMedia;
	}
}
public delegate void _WMPOCXEvents_CdromBurnStateChangeEventHandler(object sender, _WMPOCXEvents_CdromBurnStateChangeEvent e);
public class _WMPOCXEvents_CdromBurnStateChangeEvent
{
	public IWMPCdromBurn pCdromBurn;

	public WMPBurnState wmpbs;

	public _WMPOCXEvents_CdromBurnStateChangeEvent(IWMPCdromBurn pCdromBurn, WMPBurnState wmpbs)
	{
		this.pCdromBurn = pCdromBurn;
		this.wmpbs = wmpbs;
	}
}
public delegate void _WMPOCXEvents_CdromBurnMediaErrorEventHandler(object sender, _WMPOCXEvents_CdromBurnMediaErrorEvent e);
public class _WMPOCXEvents_CdromBurnMediaErrorEvent
{
	public IWMPCdromBurn pCdromBurn;

	public object pMedia;

	public _WMPOCXEvents_CdromBurnMediaErrorEvent(IWMPCdromBurn pCdromBurn, object pMedia)
	{
		this.pCdromBurn = pCdromBurn;
		this.pMedia = pMedia;
	}
}
public delegate void _WMPOCXEvents_CdromBurnErrorEventHandler(object sender, _WMPOCXEvents_CdromBurnErrorEvent e);
public class _WMPOCXEvents_CdromBurnErrorEvent
{
	public IWMPCdromBurn pCdromBurn;

	public int hrError;

	public _WMPOCXEvents_CdromBurnErrorEvent(IWMPCdromBurn pCdromBurn, int hrError)
	{
		this.pCdromBurn = pCdromBurn;
		this.hrError = hrError;
	}
}
public delegate void _WMPOCXEvents_LibraryConnectEventHandler(object sender, _WMPOCXEvents_LibraryConnectEvent e);
public class _WMPOCXEvents_LibraryConnectEvent
{
	public IWMPLibrary pLibrary;

	public _WMPOCXEvents_LibraryConnectEvent(IWMPLibrary pLibrary)
	{
		this.pLibrary = pLibrary;
	}
}
public delegate void _WMPOCXEvents_LibraryDisconnectEventHandler(object sender, _WMPOCXEvents_LibraryDisconnectEvent e);
public class _WMPOCXEvents_LibraryDisconnectEvent
{
	public IWMPLibrary pLibrary;

	public _WMPOCXEvents_LibraryDisconnectEvent(IWMPLibrary pLibrary)
	{
		this.pLibrary = pLibrary;
	}
}
public delegate void _WMPOCXEvents_FolderScanStateChangeEventHandler(object sender, _WMPOCXEvents_FolderScanStateChangeEvent e);
public class _WMPOCXEvents_FolderScanStateChangeEvent
{
	public WMPFolderScanState wmpfss;

	public _WMPOCXEvents_FolderScanStateChangeEvent(WMPFolderScanState wmpfss)
	{
		this.wmpfss = wmpfss;
	}
}
public delegate void _WMPOCXEvents_StringCollectionChangeEventHandler(object sender, _WMPOCXEvents_StringCollectionChangeEvent e);
public class _WMPOCXEvents_StringCollectionChangeEvent
{
	public object pdispStringCollection;

	public WMPStringCollectionChangeEventType change;

	public int lCollectionIndex;

	public _WMPOCXEvents_StringCollectionChangeEvent(object pdispStringCollection, WMPStringCollectionChangeEventType change, int lCollectionIndex)
	{
		this.pdispStringCollection = pdispStringCollection;
		this.change = change;
		this.lCollectionIndex = lCollectionIndex;
	}
}
public delegate void _WMPOCXEvents_MediaCollectionMediaAddedEventHandler(object sender, _WMPOCXEvents_MediaCollectionMediaAddedEvent e);
public class _WMPOCXEvents_MediaCollectionMediaAddedEvent
{
	public object pdispMedia;

	public _WMPOCXEvents_MediaCollectionMediaAddedEvent(object pdispMedia)
	{
		this.pdispMedia = pdispMedia;
	}
}
public delegate void _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler(object sender, _WMPOCXEvents_MediaCollectionMediaRemovedEvent e);
public class _WMPOCXEvents_MediaCollectionMediaRemovedEvent
{
	public object pdispMedia;

	public _WMPOCXEvents_MediaCollectionMediaRemovedEvent(object pdispMedia)
	{
		this.pdispMedia = pdispMedia;
	}
}
[ClassInterface(ClassInterfaceType.None)]
public class AxWindowsMediaPlayerEventMulticaster : _WMPOCXEvents
{
	private AxWindowsMediaPlayer parent;

	public AxWindowsMediaPlayerEventMulticaster(AxWindowsMediaPlayer parent)
	{
		this.parent = parent;
	}

	public virtual void OpenStateChange(int newState)
	{
		_WMPOCXEvents_OpenStateChangeEvent e = new _WMPOCXEvents_OpenStateChangeEvent(newState);
		parent.RaiseOnOpenStateChange(parent, e);
	}

	public virtual void PlayStateChange(int newState)
	{
		_WMPOCXEvents_PlayStateChangeEvent e = new _WMPOCXEvents_PlayStateChangeEvent(newState);
		parent.RaiseOnPlayStateChange(parent, e);
	}

	public virtual void AudioLanguageChange(int langID)
	{
		_WMPOCXEvents_AudioLanguageChangeEvent e = new _WMPOCXEvents_AudioLanguageChangeEvent(langID);
		parent.RaiseOnAudioLanguageChange(parent, e);
	}

	public virtual void StatusChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnStatusChange(parent, e);
	}

	public virtual void ScriptCommand(string scType, string param)
	{
		_WMPOCXEvents_ScriptCommandEvent e = new _WMPOCXEvents_ScriptCommandEvent(scType, param);
		parent.RaiseOnScriptCommand(parent, e);
	}

	public virtual void NewStream()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnNewStream(parent, e);
	}

	public virtual void Disconnect(int result)
	{
		_WMPOCXEvents_DisconnectEvent e = new _WMPOCXEvents_DisconnectEvent(result);
		parent.RaiseOnDisconnect(parent, e);
	}

	public virtual void Buffering(bool start)
	{
		_WMPOCXEvents_BufferingEvent e = new _WMPOCXEvents_BufferingEvent(start);
		parent.RaiseOnBuffering(parent, e);
	}

	public virtual void Error()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnErrorEvent(parent, e);
	}

	public virtual void Warning(int warningType, int param, string description)
	{
		_WMPOCXEvents_WarningEvent e = new _WMPOCXEvents_WarningEvent(warningType, param, description);
		parent.RaiseOnWarning(parent, e);
	}

	public virtual void EndOfStream(int result)
	{
		_WMPOCXEvents_EndOfStreamEvent e = new _WMPOCXEvents_EndOfStreamEvent(result);
		parent.RaiseOnEndOfStream(parent, e);
	}

	public virtual void PositionChange(double oldPosition, double newPosition)
	{
		_WMPOCXEvents_PositionChangeEvent e = new _WMPOCXEvents_PositionChangeEvent(oldPosition, newPosition);
		parent.RaiseOnPositionChange(parent, e);
	}

	public virtual void MarkerHit(int markerNum)
	{
		_WMPOCXEvents_MarkerHitEvent e = new _WMPOCXEvents_MarkerHitEvent(markerNum);
		parent.RaiseOnMarkerHit(parent, e);
	}

	public virtual void DurationUnitChange(int newDurationUnit)
	{
		_WMPOCXEvents_DurationUnitChangeEvent e = new _WMPOCXEvents_DurationUnitChangeEvent(newDurationUnit);
		parent.RaiseOnDurationUnitChange(parent, e);
	}

	public virtual void CdromMediaChange(int cdromNum)
	{
		_WMPOCXEvents_CdromMediaChangeEvent e = new _WMPOCXEvents_CdromMediaChangeEvent(cdromNum);
		parent.RaiseOnCdromMediaChange(parent, e);
	}

	public virtual void PlaylistChange(object playlist, WMPPlaylistChangeEventType change)
	{
		_WMPOCXEvents_PlaylistChangeEvent e = new _WMPOCXEvents_PlaylistChangeEvent(playlist, change);
		parent.RaiseOnPlaylistChange(parent, e);
	}

	public virtual void CurrentPlaylistChange(WMPPlaylistChangeEventType change)
	{
		_WMPOCXEvents_CurrentPlaylistChangeEvent e = new _WMPOCXEvents_CurrentPlaylistChangeEvent(change);
		parent.RaiseOnCurrentPlaylistChange(parent, e);
	}

	public virtual void CurrentPlaylistItemAvailable(string bstrItemName)
	{
		_WMPOCXEvents_CurrentPlaylistItemAvailableEvent e = new _WMPOCXEvents_CurrentPlaylistItemAvailableEvent(bstrItemName);
		parent.RaiseOnCurrentPlaylistItemAvailable(parent, e);
	}

	public virtual void MediaChange(object item)
	{
		_WMPOCXEvents_MediaChangeEvent e = new _WMPOCXEvents_MediaChangeEvent(item);
		parent.RaiseOnMediaChange(parent, e);
	}

	public virtual void CurrentMediaItemAvailable(string bstrItemName)
	{
		_WMPOCXEvents_CurrentMediaItemAvailableEvent e = new _WMPOCXEvents_CurrentMediaItemAvailableEvent(bstrItemName);
		parent.RaiseOnCurrentMediaItemAvailable(parent, e);
	}

	public virtual void CurrentItemChange(object pdispMedia)
	{
		_WMPOCXEvents_CurrentItemChangeEvent e = new _WMPOCXEvents_CurrentItemChangeEvent(pdispMedia);
		parent.RaiseOnCurrentItemChange(parent, e);
	}

	public virtual void MediaCollectionChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnMediaCollectionChange(parent, e);
	}

	public virtual void MediaCollectionAttributeStringAdded(string bstrAttribName, string bstrAttribVal)
	{
		_WMPOCXEvents_MediaCollectionAttributeStringAddedEvent e = new _WMPOCXEvents_MediaCollectionAttributeStringAddedEvent(bstrAttribName, bstrAttribVal);
		parent.RaiseOnMediaCollectionAttributeStringAdded(parent, e);
	}

	public virtual void MediaCollectionAttributeStringRemoved(string bstrAttribName, string bstrAttribVal)
	{
		_WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent e = new _WMPOCXEvents_MediaCollectionAttributeStringRemovedEvent(bstrAttribName, bstrAttribVal);
		parent.RaiseOnMediaCollectionAttributeStringRemoved(parent, e);
	}

	public virtual void MediaCollectionAttributeStringChanged(string bstrAttribName, string bstrOldAttribVal, string bstrNewAttribVal)
	{
		_WMPOCXEvents_MediaCollectionAttributeStringChangedEvent e = new _WMPOCXEvents_MediaCollectionAttributeStringChangedEvent(bstrAttribName, bstrOldAttribVal, bstrNewAttribVal);
		parent.RaiseOnMediaCollectionAttributeStringChanged(parent, e);
	}

	public virtual void PlaylistCollectionChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnPlaylistCollectionChange(parent, e);
	}

	public virtual void PlaylistCollectionPlaylistAdded(string bstrPlaylistName)
	{
		_WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent e = new _WMPOCXEvents_PlaylistCollectionPlaylistAddedEvent(bstrPlaylistName);
		parent.RaiseOnPlaylistCollectionPlaylistAdded(parent, e);
	}

	public virtual void PlaylistCollectionPlaylistRemoved(string bstrPlaylistName)
	{
		_WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent e = new _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEvent(bstrPlaylistName);
		parent.RaiseOnPlaylistCollectionPlaylistRemoved(parent, e);
	}

	public virtual void PlaylistCollectionPlaylistSetAsDeleted(string bstrPlaylistName, bool varfIsDeleted)
	{
		_WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent e = new _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEvent(bstrPlaylistName, varfIsDeleted);
		parent.RaiseOnPlaylistCollectionPlaylistSetAsDeleted(parent, e);
	}

	public virtual void ModeChange(string modeName, bool newValue)
	{
		_WMPOCXEvents_ModeChangeEvent e = new _WMPOCXEvents_ModeChangeEvent(modeName, newValue);
		parent.RaiseOnModeChange(parent, e);
	}

	public virtual void MediaError(object pMediaObject)
	{
		_WMPOCXEvents_MediaErrorEvent e = new _WMPOCXEvents_MediaErrorEvent(pMediaObject);
		parent.RaiseOnMediaError(parent, e);
	}

	public virtual void OpenPlaylistSwitch(object pItem)
	{
		_WMPOCXEvents_OpenPlaylistSwitchEvent e = new _WMPOCXEvents_OpenPlaylistSwitchEvent(pItem);
		parent.RaiseOnOpenPlaylistSwitch(parent, e);
	}

	public virtual void DomainChange(string strDomain)
	{
		_WMPOCXEvents_DomainChangeEvent e = new _WMPOCXEvents_DomainChangeEvent(strDomain);
		parent.RaiseOnDomainChange(parent, e);
	}

	public virtual void SwitchedToPlayerApplication()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnSwitchedToPlayerApplication(parent, e);
	}

	public virtual void SwitchedToControl()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnSwitchedToControl(parent, e);
	}

	public virtual void PlayerDockedStateChange()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnPlayerDockedStateChange(parent, e);
	}

	public virtual void PlayerReconnect()
	{
		EventArgs e = new EventArgs();
		parent.RaiseOnPlayerReconnect(parent, e);
	}

	public virtual void Click(short nButton, short nShiftState, int fX, int fY)
	{
		_WMPOCXEvents_ClickEvent e = new _WMPOCXEvents_ClickEvent(nButton, nShiftState, fX, fY);
		parent.RaiseOnClickEvent(parent, e);
	}

	public virtual void DoubleClick(short nButton, short nShiftState, int fX, int fY)
	{
		_WMPOCXEvents_DoubleClickEvent e = new _WMPOCXEvents_DoubleClickEvent(nButton, nShiftState, fX, fY);
		parent.RaiseOnDoubleClickEvent(parent, e);
	}

	public virtual void KeyDown(short nKeyCode, short nShiftState)
	{
		_WMPOCXEvents_KeyDownEvent e = new _WMPOCXEvents_KeyDownEvent(nKeyCode, nShiftState);
		parent.RaiseOnKeyDownEvent(parent, e);
	}

	public virtual void KeyPress(short nKeyAscii)
	{
		_WMPOCXEvents_KeyPressEvent e = new _WMPOCXEvents_KeyPressEvent(nKeyAscii);
		parent.RaiseOnKeyPressEvent(parent, e);
	}

	public virtual void KeyUp(short nKeyCode, short nShiftState)
	{
		_WMPOCXEvents_KeyUpEvent e = new _WMPOCXEvents_KeyUpEvent(nKeyCode, nShiftState);
		parent.RaiseOnKeyUpEvent(parent, e);
	}

	public virtual void MouseDown(short nButton, short nShiftState, int fX, int fY)
	{
		_WMPOCXEvents_MouseDownEvent e = new _WMPOCXEvents_MouseDownEvent(nButton, nShiftState, fX, fY);
		parent.RaiseOnMouseDownEvent(parent, e);
	}

	public virtual void MouseMove(short nButton, short nShiftState, int fX, int fY)
	{
		_WMPOCXEvents_MouseMoveEvent e = new _WMPOCXEvents_MouseMoveEvent(nButton, nShiftState, fX, fY);
		parent.RaiseOnMouseMoveEvent(parent, e);
	}

	public virtual void MouseUp(short nButton, short nShiftState, int fX, int fY)
	{
		_WMPOCXEvents_MouseUpEvent e = new _WMPOCXEvents_MouseUpEvent(nButton, nShiftState, fX, fY);
		parent.RaiseOnMouseUpEvent(parent, e);
	}

	public virtual void DeviceConnect(IWMPSyncDevice pDevice)
	{
		_WMPOCXEvents_DeviceConnectEvent e = new _WMPOCXEvents_DeviceConnectEvent(pDevice);
		parent.RaiseOnDeviceConnect(parent, e);
	}

	public virtual void DeviceDisconnect(IWMPSyncDevice pDevice)
	{
		_WMPOCXEvents_DeviceDisconnectEvent e = new _WMPOCXEvents_DeviceDisconnectEvent(pDevice);
		parent.RaiseOnDeviceDisconnect(parent, e);
	}

	public virtual void DeviceStatusChange(IWMPSyncDevice pDevice, WMPDeviceStatus newStatus)
	{
		_WMPOCXEvents_DeviceStatusChangeEvent e = new _WMPOCXEvents_DeviceStatusChangeEvent(pDevice, newStatus);
		parent.RaiseOnDeviceStatusChange(parent, e);
	}

	public virtual void DeviceSyncStateChange(IWMPSyncDevice pDevice, WMPSyncState newState)
	{
		_WMPOCXEvents_DeviceSyncStateChangeEvent e = new _WMPOCXEvents_DeviceSyncStateChangeEvent(pDevice, newState);
		parent.RaiseOnDeviceSyncStateChange(parent, e);
	}

	public virtual void DeviceSyncError(IWMPSyncDevice pDevice, object pMedia)
	{
		_WMPOCXEvents_DeviceSyncErrorEvent e = new _WMPOCXEvents_DeviceSyncErrorEvent(pDevice, pMedia);
		parent.RaiseOnDeviceSyncError(parent, e);
	}

	public virtual void CreatePartnershipComplete(IWMPSyncDevice pDevice, int hrResult)
	{
		_WMPOCXEvents_CreatePartnershipCompleteEvent e = new _WMPOCXEvents_CreatePartnershipCompleteEvent(pDevice, hrResult);
		parent.RaiseOnCreatePartnershipComplete(parent, e);
	}

	public virtual void DeviceEstimation(IWMPSyncDevice pDevice, int hrResult, long qwEstimatedUsedSpace, long qwEstimatedSpace)
	{
		_WMPOCXEvents_DeviceEstimationEvent e = new _WMPOCXEvents_DeviceEstimationEvent(pDevice, hrResult, qwEstimatedUsedSpace, qwEstimatedSpace);
		parent.RaiseOnDeviceEstimation(parent, e);
	}

	public virtual void CdromRipStateChange(IWMPCdromRip pCdromRip, WMPRipState wmprs)
	{
		_WMPOCXEvents_CdromRipStateChangeEvent e = new _WMPOCXEvents_CdromRipStateChangeEvent(pCdromRip, wmprs);
		parent.RaiseOnCdromRipStateChange(parent, e);
	}

	public virtual void CdromRipMediaError(IWMPCdromRip pCdromRip, object pMedia)
	{
		_WMPOCXEvents_CdromRipMediaErrorEvent e = new _WMPOCXEvents_CdromRipMediaErrorEvent(pCdromRip, pMedia);
		parent.RaiseOnCdromRipMediaError(parent, e);
	}

	public virtual void CdromBurnStateChange(IWMPCdromBurn pCdromBurn, WMPBurnState wmpbs)
	{
		_WMPOCXEvents_CdromBurnStateChangeEvent e = new _WMPOCXEvents_CdromBurnStateChangeEvent(pCdromBurn, wmpbs);
		parent.RaiseOnCdromBurnStateChange(parent, e);
	}

	public virtual void CdromBurnMediaError(IWMPCdromBurn pCdromBurn, object pMedia)
	{
		_WMPOCXEvents_CdromBurnMediaErrorEvent e = new _WMPOCXEvents_CdromBurnMediaErrorEvent(pCdromBurn, pMedia);
		parent.RaiseOnCdromBurnMediaError(parent, e);
	}

	public virtual void CdromBurnError(IWMPCdromBurn pCdromBurn, int hrError)
	{
		_WMPOCXEvents_CdromBurnErrorEvent e = new _WMPOCXEvents_CdromBurnErrorEvent(pCdromBurn, hrError);
		parent.RaiseOnCdromBurnError(parent, e);
	}

	public virtual void LibraryConnect(IWMPLibrary pLibrary)
	{
		_WMPOCXEvents_LibraryConnectEvent e = new _WMPOCXEvents_LibraryConnectEvent(pLibrary);
		parent.RaiseOnLibraryConnect(parent, e);
	}

	public virtual void LibraryDisconnect(IWMPLibrary pLibrary)
	{
		_WMPOCXEvents_LibraryDisconnectEvent e = new _WMPOCXEvents_LibraryDisconnectEvent(pLibrary);
		parent.RaiseOnLibraryDisconnect(parent, e);
	}

	public virtual void FolderScanStateChange(WMPFolderScanState wmpfss)
	{
		_WMPOCXEvents_FolderScanStateChangeEvent e = new _WMPOCXEvents_FolderScanStateChangeEvent(wmpfss);
		parent.RaiseOnFolderScanStateChange(parent, e);
	}

	public virtual void StringCollectionChange(object pdispStringCollection, WMPStringCollectionChangeEventType change, int lCollectionIndex)
	{
		_WMPOCXEvents_StringCollectionChangeEvent e = new _WMPOCXEvents_StringCollectionChangeEvent(pdispStringCollection, change, lCollectionIndex);
		parent.RaiseOnStringCollectionChange(parent, e);
	}

	public virtual void MediaCollectionMediaAdded(object pdispMedia)
	{
		_WMPOCXEvents_MediaCollectionMediaAddedEvent e = new _WMPOCXEvents_MediaCollectionMediaAddedEvent(pdispMedia);
		parent.RaiseOnMediaCollectionMediaAdded(parent, e);
	}

	public virtual void MediaCollectionMediaRemoved(object pdispMedia)
	{
		_WMPOCXEvents_MediaCollectionMediaRemovedEvent e = new _WMPOCXEvents_MediaCollectionMediaRemovedEvent(pdispMedia);
		parent.RaiseOnMediaCollectionMediaRemoved(parent, e);
	}
}

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Threading;

[assembly: ImportedFromTypeLib("WMPLib")]
[assembly: TypeLibVersion(1, 0)]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: Guid("6bf52a50-394a-11d3-b153-00c04f79faa6")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace WMPLib;

[ComImport]
[InterfaceType(1)]
[Guid("19A6627B-DA9E-47C1-BB23-00B5E668236A")]
public interface IWMPEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SwitchedToPlayerApplication();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SwitchedToControl();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlayerDockedStateChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlayerReconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Click([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DoubleClick([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void KeyDown([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void KeyPress([In] short nKeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void KeyUp([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MouseDown([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MouseMove([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MouseUp([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
}
public enum WMPPlaylistChangeEventType
{
	wmplcUnknown,
	wmplcClear,
	wmplcInfoChange,
	wmplcMove,
	wmplcDelete,
	wmplcInsert,
	wmplcAppend,
	wmplcPrivate,
	wmplcNameChange,
	wmplcMorph,
	wmplcSort,
	wmplcLast
}
[ComImport]
[Guid("1E7601FA-47EA-4107-9EA9-9004ED9684FF")]
[InterfaceType(1)]
public interface IWMPEvents2 : IWMPEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToPlayerApplication();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToControl();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerDockedStateChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerReconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Click([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DoubleClick([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyDown([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyPress([In] short nKeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyUp([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseDown([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseMove([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseUp([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceStatusChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPDeviceStatus NewStatus);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceSyncStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPSyncState NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceSyncError([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CreatePartnershipComplete([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult);
}
[ComImport]
[Guid("82A2986C-0293-4FD0-B279-B21B86C058BE")]
[TypeLibType(256)]
[InterfaceType(1)]
public interface IWMPSyncDevice
{
	[DispId(1610678272)]
	string friendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1610678274)]
	string deviceName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678275)]
	string deviceId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678276)]
	int partnershipIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678277)]
	bool connected
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678278)]
	WMPDeviceStatus status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678279)]
	WMPSyncState syncState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678280)]
	int progress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void createPartnership([In] bool vbShowUI);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void deletePartnership();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Start();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void showSettings();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);
}
public enum WMPDeviceStatus
{
	wmpdsUnknown,
	wmpdsPartnershipExists,
	wmpdsPartnershipDeclined,
	wmpdsPartnershipAnother,
	wmpdsManualDevice,
	wmpdsNewDevice,
	wmpdsLast
}
public enum WMPSyncState
{
	wmpssUnknown,
	wmpssSynchronizing,
	wmpssStopped,
	wmpssEstimating,
	wmpssLast
}
[ComImport]
[Guid("1F504270-A66B-4223-8E96-26A06C63D69F")]
[InterfaceType(1)]
public interface IWMPEvents3 : IWMPEvents2
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToPlayerApplication();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToControl();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerDockedStateChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerReconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Click([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DoubleClick([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyDown([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyPress([In] short nKeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyUp([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseDown([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseMove([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseUp([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceStatusChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPDeviceStatus NewStatus);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceSyncStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPSyncState NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceSyncError([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CreatePartnershipComplete([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromRipStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In] WMPRipState wmprs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromRipMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromBurnStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In] WMPBurnState wmpbs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromBurnMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CdromBurnError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.Error)] int hrError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void LibraryConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void LibraryDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void FolderScanStateChange([In] WMPFolderScanState wmpfss);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void StringCollectionChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionMediaAdded([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void MediaCollectionMediaRemoved([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
}
[ComImport]
[InterfaceType(1)]
[TypeLibType(256)]
[Guid("56E2294F-69ED-4629-A869-AEA72C0DCC2C")]
public interface IWMPCdromRip
{
	[DispId(1610678272)]
	WMPRipState ripState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678273)]
	int ripProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void startRip();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void stopRip();
}
public enum WMPRipState
{
	wmprsUnknown,
	wmprsRipping,
	wmprsStopped
}
[ComImport]
[Guid("BD94DBEB-417F-4928-AA06-087D56ED9B59")]
[TypeLibType(256)]
[InterfaceType(1)]
public interface IWMPCdromBurn
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	bool isAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItem);

	[DispId(1610678274)]
	string label
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1610678276)]
	WMPBurnFormat burnFormat
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	[DispId(1610678278)]
	IWMPPlaylist burnPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void refreshStatus();

	[DispId(1610678281)]
	WMPBurnState burnState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678282)]
	int burnProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void startBurn();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void stopBurn();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void erase();
}
public enum WMPBurnFormat
{
	wmpbfAudioCD,
	wmpbfDataCD
}
[ComImport]
[TypeLibType(4160)]
[Guid("D5F0F4F1-130C-11D3-B14E-00C04F79FAA6")]
public interface IWMPPlaylist
{
	[DispId(201)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[DispId(202)]
	string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(210)]
	int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(210)]
		get;
	}

	[DispId(211)]
	string attributeName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(211)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(212)]
	IWMPMedia Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(212)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(203)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(204)]
	void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[DispId(213)]
	bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(213)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(205)]
	void clear();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(206)]
	void insertItem([In] int lIndex, [In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(207)]
	void appendItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(208)]
	void removeItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(209)]
	void moveItem(int lIndexOld, int lIndexNew);
}
[ComImport]
[Guid("94D55E95-3FAC-11D3-B155-00C04F79FAA6")]
[TypeLibType(4160)]
public interface IWMPMedia
{
	[DispId(763)]
	bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(763)]
		get;
	}

	[DispId(751)]
	string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(751)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(764)]
	string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(752)]
	int imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(752)]
		get;
	}

	[DispId(753)]
	int imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(753)]
		get;
	}

	[DispId(754)]
	int markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(754)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(755)]
	double getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(756)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getMarkerName([In] int MarkerNum);

	[DispId(757)]
	double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(757)]
		get;
	}

	[DispId(758)]
	string durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(758)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(759)]
	int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(759)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(760)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(761)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(762)]
	void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(765)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(766)]
	bool isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(767)]
	bool isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
}
public enum WMPBurnState
{
	wmpbsUnknown,
	wmpbsBusy,
	wmpbsReady,
	wmpbsWaitingForDisc,
	wmpbsRefreshStatusPending,
	wmpbsPreparingToBurn,
	wmpbsBurning,
	wmpbsStopped,
	wmpbsErasing,
	wmpbsDownloading
}
[ComImport]
[InterfaceType(1)]
[TypeLibType(256)]
[Guid("3DF47861-7DF1-4C1F-A81B-4C26F0F7A7C6")]
public interface IWMPLibrary
{
	[DispId(1610678272)]
	string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678273)]
	WMPLibraryType type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678274)]
	IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pIWMPLibrary);
}
public enum WMPLibraryType
{
	wmpltUnknown,
	wmpltAll,
	wmpltLocal,
	wmpltRemote,
	wmpltDisc,
	wmpltPortableDevice
}
[ComImport]
[TypeLibType(4160)]
[Guid("8363BC22-B4B4-4B19-989D-1CD765749DD1")]
public interface IWMPMediaCollection
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(452)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPMedia add([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(453)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(454)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(455)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByGenre([In][MarshalAs(UnmanagedType.BStr)] string bstrGenre);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(456)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByAuthor([In][MarshalAs(UnmanagedType.BStr)] string bstrAuthor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(457)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByAlbum([In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(458)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByAttribute([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(459)]
	void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfDeleteFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(461)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPStringCollection getAttributeStringCollection([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(470)]
	int getMediaAtom([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(471)]
	void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(472)]
	bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem);
}
[ComImport]
[TypeLibType(4160)]
[Guid("4A976298-8C0D-11D3-B389-00C04F68574B")]
public interface IWMPStringCollection
{
	[DispId(401)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(401)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(402)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Item([In] int lIndex);
}
public enum WMPFolderScanState
{
	wmpfssUnknown,
	wmpfssScanning,
	wmpfssUpdating,
	wmpfssStopped
}
public enum WMPStringCollectionChangeEventType
{
	wmpsccetUnknown,
	wmpsccetInsert,
	wmpsccetChange,
	wmpsccetDelete,
	wmpsccetClear,
	wmpsccetBeginUpdates,
	wmpsccetEndUpdates
}
[ComImport]
[Guid("26DABCFA-306B-404D-9A6F-630A8405048D")]
[InterfaceType(1)]
public interface IWMPEvents4 : IWMPEvents3
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToPlayerApplication();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void SwitchedToControl();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerDockedStateChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void PlayerReconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Click([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DoubleClick([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyDown([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyPress([In] short nKeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void KeyUp([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseDown([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseMove([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MouseUp([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceStatusChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPDeviceStatus NewStatus);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceSyncStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPSyncState NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void DeviceSyncError([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CreatePartnershipComplete([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromRipStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In] WMPRipState wmprs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromRipMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromBurnStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In] WMPBurnState wmpbs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromBurnMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void CdromBurnError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.Error)] int hrError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void LibraryConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void LibraryDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void FolderScanStateChange([In] WMPFolderScanState wmpfss);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void StringCollectionChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionMediaAdded([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void MediaCollectionMediaRemoved([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DeviceEstimation([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult, [In] long qwEstimatedUsedSpace, [In] long qwEstimatedSpace);
}
[ComImport]
[Guid("6BF52A51-394A-11D3-B153-00C04F79FAA6")]
[TypeLibType(4112)]
[InterfaceType(2)]
public interface _WMPOCXEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5001)]
	void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5101)]
	void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5102)]
	void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5002)]
	void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5301)]
	void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5403)]
	void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5401)]
	void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5402)]
	void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5501)]
	void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5601)]
	void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5201)]
	void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5202)]
	void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5203)]
	void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5204)]
	void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5701)]
	void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5801)]
	void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5804)]
	void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5805)]
	void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5802)]
	void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5803)]
	void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5806)]
	void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5807)]
	void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5808)]
	void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5809)]
	void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5820)]
	void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5810)]
	void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5811)]
	void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5812)]
	void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5818)]
	void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5819)]
	void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5821)]
	void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5823)]
	void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5822)]
	void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6501)]
	void SwitchedToPlayerApplication();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6502)]
	void SwitchedToControl();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6503)]
	void PlayerDockedStateChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6504)]
	void PlayerReconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6505)]
	void Click([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6506)]
	void DoubleClick([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6507)]
	void KeyDown([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6508)]
	void KeyPress([In] short nKeyAscii);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6509)]
	void KeyUp([In] short nKeyCode, [In] short nShiftState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6510)]
	void MouseDown([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6511)]
	void MouseMove([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6512)]
	void MouseUp([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6513)]
	void DeviceConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6514)]
	void DeviceDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6515)]
	void DeviceStatusChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPDeviceStatus NewStatus);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6516)]
	void DeviceSyncStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPSyncState NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6517)]
	void DeviceSyncError([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6518)]
	void CreatePartnershipComplete([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6527)]
	void DeviceEstimation([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult, [In] long qwEstimatedUsedSpace, [In] long qwEstimatedSpace);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6519)]
	void CdromRipStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In] WMPRipState wmprs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6520)]
	void CdromRipMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6521)]
	void CdromBurnStateChange([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In] WMPBurnState wmpbs);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6522)]
	void CdromBurnMediaError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6523)]
	void CdromBurnError([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.Error)] int hrError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6524)]
	void LibraryConnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6525)]
	void LibraryDisconnect([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6526)]
	void FolderScanStateChange([In] WMPFolderScanState wmpfss);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5824)]
	void StringCollectionChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5825)]
	void MediaCollectionMediaAdded([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5826)]
	void MediaCollectionMediaRemoved([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
}
[ComImport]
[Guid("6BF52A52-394A-11D3-B153-00C04F79FAA6")]
[TypeLibType(2)]
[ClassInterface(0)]
[ComSourceInterfaces("WMPLib._WMPOCXEvents\0\0")]
public class WindowsMediaPlayerClass : IWMPPlayer4, WindowsMediaPlayer, _WMPOCXEvents_Event, IWMPPlayer3, IWMPPlayer2, IWMPPlayer, IWMPControls, IWMPSettings, IWMPPlaylist, IWMPMedia, IWMPMediaCollection, IWMPPlaylistCollection, IWMPCdromCollection, IWMPError, IWMPErrorItem, IWMPErrorItem2, IWMPClosedCaption, IWMPDVD, IWMPControls2, IWMPMedia2, IWMPMedia3, IWMPMetadataPicture, IWMPMetadataText, IWMPSettings2, IWMPControls3, IWMPClosedCaption2, IWMPMediaCollection2, IWMPStringCollection2
{
	[DispId(1)]
	public virtual extern string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	public virtual extern WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	public virtual extern WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	public virtual extern IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	public virtual extern IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	public virtual extern IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	public virtual extern IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	public virtual extern IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	public virtual extern string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(7)]
	public virtual extern IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	public virtual extern IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	public virtual extern IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	public virtual extern IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	public virtual extern bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	public virtual extern IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	public virtual extern string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	public virtual extern IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(19)]
	public virtual extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	[DispId(21)]
	public virtual extern bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		set;
	}

	[DispId(22)]
	public virtual extern bool enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		set;
	}

	[DispId(23)]
	public virtual extern string uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(24)]
	public virtual extern bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		[param: In]
		set;
	}

	[DispId(25)]
	public virtual extern bool windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		[param: In]
		set;
	}

	[DispId(26)]
	public virtual extern bool isRemote
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(26)]
		get;
	}

	[DispId(27)]
	public virtual extern IWMPPlayerApplication playerApplication
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(27)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer3_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern WMPOpenState IWMPPlayer3_openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern WMPPlayState IWMPPlayer3_playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPControls IWMPPlayer3_controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPSettings IWMPPlayer3_settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPMedia IWMPPlayer3_currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPMediaCollection IWMPPlayer3_mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylistCollection IWMPPlayer3_playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer3_versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPNetwork IWMPPlayer3_network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylist IWMPPlayer3_currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPCdromCollection IWMPPlayer3_cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPClosedCaption IWMPPlayer3_closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPPlayer3_isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPError IWMPPlayer3_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer3_status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPDVD IWMPPlayer3_dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPPlayer3_enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPPlayer3_fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern bool IWMPPlayer3_enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern string IWMPPlayer3_uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool IWMPPlayer3_stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPPlayer3_windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPPlayer2_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern WMPOpenState IWMPPlayer2_openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern WMPPlayState IWMPPlayer2_playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPControls IWMPPlayer2_controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPSettings IWMPPlayer2_settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPMedia IWMPPlayer2_currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPMediaCollection IWMPPlayer2_mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylistCollection IWMPPlayer2_playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer2_versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPNetwork IWMPPlayer2_network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylist IWMPPlayer2_currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPCdromCollection IWMPPlayer2_cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPClosedCaption IWMPPlayer2_closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPPlayer2_isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPError IWMPPlayer2_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer2_status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPPlayer2_enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPPlayer2_fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern bool IWMPPlayer2_enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern string IWMPPlayer2_uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool IWMPPlayer2_stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPPlayer2_windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPPlayer_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern WMPOpenState IWMPPlayer_openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern WMPPlayState IWMPPlayer_playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPControls IWMPPlayer_controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPSettings IWMPPlayer_settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPMedia IWMPPlayer_currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPMediaCollection IWMPPlayer_mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylistCollection IWMPPlayer_playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer_versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPNetwork IWMPPlayer_network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylist IWMPPlayer_currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPCdromCollection IWMPPlayer_cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPClosedCaption IWMPPlayer_closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPPlayer_isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPError IWMPPlayer_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPPlayer_status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPPlayer_enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPPlayer_fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern bool IWMPPlayer_enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		set;
	}

	public virtual extern string IWMPPlayer_uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPSettings_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern double rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string attributeName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool IWMPMedia_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPCdromCollection_count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int errorCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPError_Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern int errorCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string errorDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern object errorContext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	public virtual extern int remedy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string customUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPErrorItem2_errorCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPErrorItem2_errorDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern object IWMPErrorItem2_errorContext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	public virtual extern int IWMPErrorItem2_remedy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPErrorItem2_customUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int condition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool IWMPDVD_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string domain
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPControls2_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPControls2_currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPControls2_currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia IWMPControls2_currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int IWMPControls2_currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPMedia2_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia2_sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia2_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int IWMPMedia2_imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia2_imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia2_markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPMedia2_duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia2_durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia2_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPMedia2_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPMedia3_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia3_sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia3_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int IWMPMedia3_imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia3_imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia3_markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPMedia3_duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia3_durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia3_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPMedia3_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string mimeType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string pictureType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMetadataPicture_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMetadataText_Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string text
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPSettings2_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool IWMPSettings2_autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPSettings2_baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPSettings2_defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool IWMPSettings2_invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPSettings2_mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern double IWMPSettings2_rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPSettings2_enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int defaultAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string mediaAccessRights
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPControls3_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPControls3_currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPControls3_currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia IWMPControls3_currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int IWMPControls3_currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int audioLanguageCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int currentAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int currentAudioLanguageIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string currentPositionTimecode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int SAMILangCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int SAMIStyleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPStringCollection2_count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern event _WMPOCXEvents_OpenStateChangeEventHandler OpenStateChange;

	public virtual extern event _WMPOCXEvents_PlayStateChangeEventHandler PlayStateChange;

	public virtual extern event _WMPOCXEvents_AudioLanguageChangeEventHandler AudioLanguageChange;

	public virtual extern event _WMPOCXEvents_StatusChangeEventHandler StatusChange;

	public virtual extern event _WMPOCXEvents_ScriptCommandEventHandler ScriptCommand;

	public virtual extern event _WMPOCXEvents_NewStreamEventHandler NewStream;

	public virtual extern event _WMPOCXEvents_DisconnectEventHandler Disconnect;

	public virtual extern event _WMPOCXEvents_BufferingEventHandler Buffering;

	public virtual extern event _WMPOCXEvents_ErrorEventHandler _WMPOCXEvents_Event_Error;

	public virtual extern event _WMPOCXEvents_WarningEventHandler Warning;

	public virtual extern event _WMPOCXEvents_EndOfStreamEventHandler EndOfStream;

	public virtual extern event _WMPOCXEvents_PositionChangeEventHandler PositionChange;

	public virtual extern event _WMPOCXEvents_MarkerHitEventHandler MarkerHit;

	public virtual extern event _WMPOCXEvents_DurationUnitChangeEventHandler DurationUnitChange;

	public virtual extern event _WMPOCXEvents_CdromMediaChangeEventHandler CdromMediaChange;

	public virtual extern event _WMPOCXEvents_PlaylistChangeEventHandler PlaylistChange;

	public virtual extern event _WMPOCXEvents_CurrentPlaylistChangeEventHandler CurrentPlaylistChange;

	public virtual extern event _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler CurrentPlaylistItemAvailable;

	public virtual extern event _WMPOCXEvents_MediaChangeEventHandler MediaChange;

	public virtual extern event _WMPOCXEvents_CurrentMediaItemAvailableEventHandler CurrentMediaItemAvailable;

	public virtual extern event _WMPOCXEvents_CurrentItemChangeEventHandler CurrentItemChange;

	public virtual extern event _WMPOCXEvents_MediaCollectionChangeEventHandler MediaCollectionChange;

	public virtual extern event _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler MediaCollectionAttributeStringAdded;

	public virtual extern event _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler MediaCollectionAttributeStringRemoved;

	public virtual extern event _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler MediaCollectionAttributeStringChanged;

	public virtual extern event _WMPOCXEvents_PlaylistCollectionChangeEventHandler PlaylistCollectionChange;

	public virtual extern event _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler PlaylistCollectionPlaylistAdded;

	public virtual extern event _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler PlaylistCollectionPlaylistRemoved;

	public virtual extern event _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler PlaylistCollectionPlaylistSetAsDeleted;

	public virtual extern event _WMPOCXEvents_ModeChangeEventHandler ModeChange;

	public virtual extern event _WMPOCXEvents_MediaErrorEventHandler MediaError;

	public virtual extern event _WMPOCXEvents_OpenPlaylistSwitchEventHandler OpenPlaylistSwitch;

	public virtual extern event _WMPOCXEvents_DomainChangeEventHandler DomainChange;

	public virtual extern event _WMPOCXEvents_SwitchedToPlayerApplicationEventHandler SwitchedToPlayerApplication;

	public virtual extern event _WMPOCXEvents_SwitchedToControlEventHandler SwitchedToControl;

	public virtual extern event _WMPOCXEvents_PlayerDockedStateChangeEventHandler PlayerDockedStateChange;

	public virtual extern event _WMPOCXEvents_PlayerReconnectEventHandler PlayerReconnect;

	public virtual extern event _WMPOCXEvents_ClickEventHandler Click;

	public virtual extern event _WMPOCXEvents_DoubleClickEventHandler DoubleClick;

	public virtual extern event _WMPOCXEvents_KeyDownEventHandler KeyDown;

	public virtual extern event _WMPOCXEvents_KeyPressEventHandler KeyPress;

	public virtual extern event _WMPOCXEvents_KeyUpEventHandler KeyUp;

	public virtual extern event _WMPOCXEvents_MouseDownEventHandler MouseDown;

	public virtual extern event _WMPOCXEvents_MouseMoveEventHandler MouseMove;

	public virtual extern event _WMPOCXEvents_MouseUpEventHandler MouseUp;

	public virtual extern event _WMPOCXEvents_DeviceConnectEventHandler DeviceConnect;

	public virtual extern event _WMPOCXEvents_DeviceDisconnectEventHandler DeviceDisconnect;

	public virtual extern event _WMPOCXEvents_DeviceStatusChangeEventHandler DeviceStatusChange;

	public virtual extern event _WMPOCXEvents_DeviceSyncStateChangeEventHandler DeviceSyncStateChange;

	public virtual extern event _WMPOCXEvents_DeviceSyncErrorEventHandler DeviceSyncError;

	public virtual extern event _WMPOCXEvents_CreatePartnershipCompleteEventHandler CreatePartnershipComplete;

	public virtual extern event _WMPOCXEvents_DeviceEstimationEventHandler DeviceEstimation;

	public virtual extern event _WMPOCXEvents_CdromRipStateChangeEventHandler CdromRipStateChange;

	public virtual extern event _WMPOCXEvents_CdromRipMediaErrorEventHandler CdromRipMediaError;

	public virtual extern event _WMPOCXEvents_CdromBurnStateChangeEventHandler CdromBurnStateChange;

	public virtual extern event _WMPOCXEvents_CdromBurnMediaErrorEventHandler CdromBurnMediaError;

	public virtual extern event _WMPOCXEvents_CdromBurnErrorEventHandler CdromBurnError;

	public virtual extern event _WMPOCXEvents_LibraryConnectEventHandler LibraryConnect;

	public virtual extern event _WMPOCXEvents_LibraryDisconnectEventHandler LibraryDisconnect;

	public virtual extern event _WMPOCXEvents_FolderScanStateChangeEventHandler FolderScanStateChange;

	public virtual extern event _WMPOCXEvents_StringCollectionChangeEventHandler StringCollectionChange;

	public virtual extern event _WMPOCXEvents_MediaCollectionMediaAddedEventHandler MediaCollectionMediaAdded;

	public virtual extern event _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler MediaCollectionMediaRemoved;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	public virtual extern void close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	public virtual extern void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPMedia newMedia([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(28)]
	public virtual extern void openPlayer([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer3_close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer3_launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer2_close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer2_launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer_close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPPlayer_launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void clear();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void insertItem([In] int lIndex, [In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void appendItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void removeItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void moveItem(int lIndexOld, int lIndexNew);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPMedia add([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByGenre([In][MarshalAs(UnmanagedType.BStr)] string bstrGenre);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAuthor([In][MarshalAs(UnmanagedType.BStr)] string bstrAuthor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAlbum([In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAttribute([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfDeleteFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPStringCollection getAttributeStringCollection([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getMediaAtom([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylistArray IWMPPlaylistCollection_getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylistArray IWMPPlaylistCollection_getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist importPlaylist([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPCdrom IWMPCdromCollection_Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPCdrom getByDriveSpecifier([In][MarshalAs(UnmanagedType.BStr)] string bstrDriveSpecifier);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void clearErrorQueue();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void webHelp();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void topMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void titleMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void back();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void resume();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void step([In] int lStep);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double IWMPMedia2_getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia2_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia2_isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia2_isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double IWMPMedia3_getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia3_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia3_isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia3_isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getAttributeCountByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object getItemInfoByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage, [In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPSettings2_getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPSettings2_setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool requestMediaAccessRights([In][MarshalAs(UnmanagedType.BStr)] string bstrDesiredAccess);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_step([In] int lStep);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getAudioLanguageID([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getAudioLanguageDescription([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getLanguageName([In] int lLangID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getSAMILangName([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getSAMILangID([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getSAMIStyleName([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPMedia IWMPMediaCollection2_add([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getByGenre([In][MarshalAs(UnmanagedType.BStr)] string bstrGenre);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getByAuthor([In][MarshalAs(UnmanagedType.BStr)] string bstrAuthor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getByAlbum([In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist IWMPMediaCollection2_getByAttribute([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMediaCollection2_remove([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfDeleteFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPStringCollection IWMPMediaCollection2_getAttributeStringCollection([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int IWMPMediaCollection2_getMediaAtom([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMediaCollection2_setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMediaCollection2_isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPQuery createQuery();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getPlaylistByQuery([In][MarshalAs(UnmanagedType.Interface)] IWMPQuery pQuery, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType, [In][MarshalAs(UnmanagedType.BStr)] string bstrSortAttribute, [In] bool fSortAscending);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPStringCollection getStringCollectionByQuery([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.Interface)] IWMPQuery pQuery, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType, [In][MarshalAs(UnmanagedType.BStr)] string bstrSortAttribute, [In] bool fSortAscending);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAttributeAndMediaType([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPStringCollection2_Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPStringCollection2_isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPStringCollection2 pIWMPStringCollection2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItemInfo([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getAttributeCountByType([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object getItemInfoByType([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage, [In] int lAttributeIndex);
}
[ComVisible(false)]
[TypeLibType(16)]
[ComEventInterface(typeof(_WMPOCXEvents\u0000), typeof(_WMPOCXEvents_EventProvider\u0000))]
public interface _WMPOCXEvents_Event
{
	event _WMPOCXEvents_OpenStateChangeEventHandler OpenStateChange;

	event _WMPOCXEvents_PlayStateChangeEventHandler PlayStateChange;

	event _WMPOCXEvents_AudioLanguageChangeEventHandler AudioLanguageChange;

	event _WMPOCXEvents_StatusChangeEventHandler StatusChange;

	event _WMPOCXEvents_ScriptCommandEventHandler ScriptCommand;

	event _WMPOCXEvents_NewStreamEventHandler NewStream;

	event _WMPOCXEvents_DisconnectEventHandler Disconnect;

	event _WMPOCXEvents_BufferingEventHandler Buffering;

	event _WMPOCXEvents_ErrorEventHandler Error;

	event _WMPOCXEvents_WarningEventHandler Warning;

	event _WMPOCXEvents_EndOfStreamEventHandler EndOfStream;

	event _WMPOCXEvents_PositionChangeEventHandler PositionChange;

	event _WMPOCXEvents_MarkerHitEventHandler MarkerHit;

	event _WMPOCXEvents_DurationUnitChangeEventHandler DurationUnitChange;

	event _WMPOCXEvents_CdromMediaChangeEventHandler CdromMediaChange;

	event _WMPOCXEvents_PlaylistChangeEventHandler PlaylistChange;

	event _WMPOCXEvents_CurrentPlaylistChangeEventHandler CurrentPlaylistChange;

	event _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler CurrentPlaylistItemAvailable;

	event _WMPOCXEvents_MediaChangeEventHandler MediaChange;

	event _WMPOCXEvents_CurrentMediaItemAvailableEventHandler CurrentMediaItemAvailable;

	event _WMPOCXEvents_CurrentItemChangeEventHandler CurrentItemChange;

	event _WMPOCXEvents_MediaCollectionChangeEventHandler MediaCollectionChange;

	event _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler MediaCollectionAttributeStringAdded;

	event _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler MediaCollectionAttributeStringRemoved;

	event _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler MediaCollectionAttributeStringChanged;

	event _WMPOCXEvents_PlaylistCollectionChangeEventHandler PlaylistCollectionChange;

	event _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler PlaylistCollectionPlaylistAdded;

	event _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler PlaylistCollectionPlaylistRemoved;

	event _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler PlaylistCollectionPlaylistSetAsDeleted;

	event _WMPOCXEvents_ModeChangeEventHandler ModeChange;

	event _WMPOCXEvents_MediaErrorEventHandler MediaError;

	event _WMPOCXEvents_OpenPlaylistSwitchEventHandler OpenPlaylistSwitch;

	event _WMPOCXEvents_DomainChangeEventHandler DomainChange;

	event _WMPOCXEvents_SwitchedToPlayerApplicationEventHandler SwitchedToPlayerApplication;

	event _WMPOCXEvents_SwitchedToControlEventHandler SwitchedToControl;

	event _WMPOCXEvents_PlayerDockedStateChangeEventHandler PlayerDockedStateChange;

	event _WMPOCXEvents_PlayerReconnectEventHandler PlayerReconnect;

	event _WMPOCXEvents_ClickEventHandler Click;

	event _WMPOCXEvents_DoubleClickEventHandler DoubleClick;

	event _WMPOCXEvents_KeyDownEventHandler KeyDown;

	event _WMPOCXEvents_KeyPressEventHandler KeyPress;

	event _WMPOCXEvents_KeyUpEventHandler KeyUp;

	event _WMPOCXEvents_MouseDownEventHandler MouseDown;

	event _WMPOCXEvents_MouseMoveEventHandler MouseMove;

	event _WMPOCXEvents_MouseUpEventHandler MouseUp;

	event _WMPOCXEvents_DeviceConnectEventHandler DeviceConnect;

	event _WMPOCXEvents_DeviceDisconnectEventHandler DeviceDisconnect;

	event _WMPOCXEvents_DeviceStatusChangeEventHandler DeviceStatusChange;

	event _WMPOCXEvents_DeviceSyncStateChangeEventHandler DeviceSyncStateChange;

	event _WMPOCXEvents_DeviceSyncErrorEventHandler DeviceSyncError;

	event _WMPOCXEvents_CreatePartnershipCompleteEventHandler CreatePartnershipComplete;

	event _WMPOCXEvents_DeviceEstimationEventHandler DeviceEstimation;

	event _WMPOCXEvents_CdromRipStateChangeEventHandler CdromRipStateChange;

	event _WMPOCXEvents_CdromRipMediaErrorEventHandler CdromRipMediaError;

	event _WMPOCXEvents_CdromBurnStateChangeEventHandler CdromBurnStateChange;

	event _WMPOCXEvents_CdromBurnMediaErrorEventHandler CdromBurnMediaError;

	event _WMPOCXEvents_CdromBurnErrorEventHandler CdromBurnError;

	event _WMPOCXEvents_LibraryConnectEventHandler LibraryConnect;

	event _WMPOCXEvents_LibraryDisconnectEventHandler LibraryDisconnect;

	event _WMPOCXEvents_FolderScanStateChangeEventHandler FolderScanStateChange;

	event _WMPOCXEvents_StringCollectionChangeEventHandler StringCollectionChange;

	event _WMPOCXEvents_MediaCollectionMediaAddedEventHandler MediaCollectionMediaAdded;

	event _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler MediaCollectionMediaRemoved;
}
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_OpenStateChangeEventHandler([In] int NewState);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_PlayStateChangeEventHandler([In] int NewState);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_AudioLanguageChangeEventHandler([In] int LangID);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_StatusChangeEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_ScriptCommandEventHandler([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_NewStreamEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_DisconnectEventHandler([In] int Result);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_BufferingEventHandler([In] bool Start);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_ErrorEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_WarningEventHandler([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_EndOfStreamEventHandler([In] int Result);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_PositionChangeEventHandler([In] double oldPosition, [In] double newPosition);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MarkerHitEventHandler([In] int MarkerNum);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_DurationUnitChangeEventHandler([In] int NewDurationUnit);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_CdromMediaChangeEventHandler([In] int CdromNum);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_PlaylistChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CurrentPlaylistChangeEventHandler([In] WMPPlaylistChangeEventType change);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object Item);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CurrentMediaItemAvailableEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CurrentItemChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaCollectionChangeEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_PlaylistCollectionChangeEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_ModeChangeEventHandler([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_MediaErrorEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_OpenPlaylistSwitchEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_DomainChangeEventHandler([In][MarshalAs(UnmanagedType.BStr)] string strDomain);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_SwitchedToPlayerApplicationEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_SwitchedToControlEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_PlayerDockedStateChangeEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_PlayerReconnectEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_ClickEventHandler([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_DoubleClickEventHandler([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_KeyDownEventHandler([In] short nKeyCode, [In] short nShiftState);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_KeyPressEventHandler([In] short nKeyAscii);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_KeyUpEventHandler([In] short nKeyCode, [In] short nShiftState);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MouseDownEventHandler([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MouseMoveEventHandler([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MouseUpEventHandler([In] short nButton, [In] short nShiftState, [In] int fX, [In] int fY);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_DeviceConnectEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_DeviceDisconnectEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_DeviceStatusChangeEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPDeviceStatus NewStatus);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_DeviceSyncStateChangeEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In] WMPSyncState NewState);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_DeviceSyncErrorEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_CreatePartnershipCompleteEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_DeviceEstimationEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice, [In][MarshalAs(UnmanagedType.Error)] int hrResult, [In] long qwEstimatedUsedSpace, [In] long qwEstimatedSpace);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CdromRipStateChangeEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In] WMPRipState wmprs);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CdromRipMediaErrorEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromRip pCdromRip, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_CdromBurnStateChangeEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In] WMPBurnState wmpbs);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CdromBurnMediaErrorEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.IDispatch)] object pMedia);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_CdromBurnErrorEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPCdromBurn pCdromBurn, [In][MarshalAs(UnmanagedType.Error)] int hrError);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_LibraryConnectEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_LibraryDisconnectEventHandler([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pLibrary);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_FolderScanStateChangeEventHandler([In] WMPFolderScanState wmpfss);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_StringCollectionChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPOCXEvents_MediaCollectionMediaAddedEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[ComImport]
[Guid("6C497D62-8919-413C-82DB-E935FB3EC584")]
[CoClass(typeof(WindowsMediaPlayerClass))]
public interface WindowsMediaPlayer : IWMPPlayer4, _WMPOCXEvents_Event
{
}
[ComImport]
[Guid("6C497D62-8919-413C-82DB-E935FB3EC584")]
[TypeLibType(4160)]
public interface IWMPPlayer4 : IWMPCore3
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	new IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPMedia newMedia([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(19)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	[DispId(21)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		set;
	}

	[DispId(22)]
	bool enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		set;
	}

	[DispId(23)]
	string uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(24)]
	bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		[param: In]
		set;
	}

	[DispId(25)]
	bool windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		[param: In]
		set;
	}

	[DispId(26)]
	bool isRemote
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(26)]
		get;
	}

	[DispId(27)]
	IWMPPlayerApplication playerApplication
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(27)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(28)]
	void openPlayer([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);
}
[ComImport]
[Guid("7587C667-628F-499F-88E7-6A6F4E888464")]
[TypeLibType(4160)]
public interface IWMPCore3 : IWMPCore2
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	new IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPMedia newMedia([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);
}
[ComImport]
[TypeLibType(4160)]
[Guid("BC17E5B7-7561-4C18-BB90-17D485775659")]
public interface IWMPCore2 : IWMPCore
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("D84CCA99-CCE2-11D2-9ECC-0000F8085981")]
public interface IWMPCore
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void close();

	[DispId(1)]
	string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
public enum WMPOpenState
{
	wmposUndefined,
	wmposPlaylistChanging,
	wmposPlaylistLocating,
	wmposPlaylistConnecting,
	wmposPlaylistLoading,
	wmposPlaylistOpening,
	wmposPlaylistOpenNoMedia,
	wmposPlaylistChanged,
	wmposMediaChanging,
	wmposMediaLocating,
	wmposMediaConnecting,
	wmposMediaLoading,
	wmposMediaOpening,
	wmposMediaOpen,
	wmposBeginCodecAcquisition,
	wmposEndCodecAcquisition,
	wmposBeginLicenseAcquisition,
	wmposEndLicenseAcquisition,
	wmposBeginIndividualization,
	wmposEndIndividualization,
	wmposMediaWaiting,
	wmposOpeningUnknownURL
}
public enum WMPPlayState
{
	wmppsUndefined,
	wmppsStopped,
	wmppsPaused,
	wmppsPlaying,
	wmppsScanForward,
	wmppsScanReverse,
	wmppsBuffering,
	wmppsWaiting,
	wmppsMediaEnded,
	wmppsTransitioning,
	wmppsReady,
	wmppsReconnecting,
	wmppsLast
}
[ComImport]
[TypeLibType(4160)]
[Guid("74C09E02-F828-11D2-A74B-00A0C905F36E")]
public interface IWMPControls
{
	[DispId(62)]
	bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(62)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	void play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	void fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	void fastReverse();

	[DispId(56)]
	double currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[param: In]
		set;
	}

	[DispId(57)]
	string currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(57)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	void previous();

	[DispId(60)]
	IWMPMedia currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(61)]
	int currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	void playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);
}
[ComImport]
[Guid("9104D1AB-80C9-4FED-ABF0-2E6417A6DF14")]
[TypeLibType(4160)]
public interface IWMPSettings
{
	[DispId(113)]
	bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		get;
	}

	[DispId(101)]
	bool autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[param: In]
		set;
	}

	[DispId(108)]
	string baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(109)]
	string defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(103)]
	bool invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		[param: In]
		set;
	}

	[DispId(104)]
	bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		[param: In]
		set;
	}

	[DispId(105)]
	int playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		[param: In]
		set;
	}

	[DispId(106)]
	double rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		[param: In]
		set;
	}

	[DispId(102)]
	int balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		[param: In]
		set;
	}

	[DispId(107)]
	int volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(110)]
	bool getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(111)]
	void setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[DispId(112)]
	bool enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(112)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(112)]
		[param: In]
		set;
	}
}
[ComImport]
[Guid("10A13217-23A7-439B-B1C0-D847C79B7774")]
[TypeLibType(4160)]
public interface IWMPPlaylistCollection
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(552)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(553)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylistArray getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(554)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylistArray getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(556)]
	void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(560)]
	void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(561)]
	bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(562)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist importPlaylist([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);
}
[ComImport]
[Guid("679409C0-99F7-11D3-9FB7-00105AA620BB")]
[TypeLibType(4160)]
public interface IWMPPlaylistArray
{
	[DispId(501)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(501)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(502)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist Item([In] int lIndex);
}
[ComImport]
[TypeLibType(4160)]
[Guid("EC21B779-EDEF-462D-BBA4-AD9DDE2B29A7")]
public interface IWMPNetwork
{
	[DispId(801)]
	int bandWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(801)]
		get;
	}

	[DispId(802)]
	int recoveredPackets
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(802)]
		get;
	}

	[DispId(803)]
	string sourceProtocol
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(803)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(804)]
	int receivedPackets
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(804)]
		get;
	}

	[DispId(805)]
	int lostPackets
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(805)]
		get;
	}

	[DispId(806)]
	int receptionQuality
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(806)]
		get;
	}

	[DispId(807)]
	int bufferingCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(807)]
		get;
	}

	[DispId(808)]
	int bufferingProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(808)]
		get;
	}

	[DispId(809)]
	int bufferingTime
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(809)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(809)]
		[param: In]
		set;
	}

	[DispId(810)]
	int frameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(810)]
		get;
	}

	[DispId(811)]
	int maxBitRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(811)]
		get;
	}

	[DispId(812)]
	int bitRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(812)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(813)]
	int getProxySettings([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(814)]
	void setProxySettings([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol, [In] int lProxySetting);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(815)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getProxyName([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(816)]
	void setProxyName([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol, [In][MarshalAs(UnmanagedType.BStr)] string bstrProxyName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(817)]
	int getProxyPort([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(818)]
	void setProxyPort([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol, [In] int lProxyPort);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(819)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getProxyExceptionList([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(820)]
	void setProxyExceptionList([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol, [In][MarshalAs(UnmanagedType.BStr)] string pbstrExceptionList);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(821)]
	bool getProxyBypassForLocal([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(822)]
	void setProxyBypassForLocal([In][MarshalAs(UnmanagedType.BStr)] string bstrProtocol, [In] bool fBypassForLocal);

	[DispId(823)]
	int maxBandwidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(823)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(823)]
		[param: In]
		set;
	}

	[DispId(824)]
	int downloadProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(824)]
		get;
	}

	[DispId(825)]
	int encodedFrameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(825)]
		get;
	}

	[DispId(826)]
	int framesSkipped
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(826)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("EE4C8FE2-34B2-11D3-A3BF-006097C9B344")]
public interface IWMPCdromCollection
{
	[DispId(301)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(301)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(302)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPCdrom Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(303)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPCdrom getByDriveSpecifier([In][MarshalAs(UnmanagedType.BStr)] string bstrDriveSpecifier);
}
[ComImport]
[TypeLibType(4160)]
[Guid("CFAB6E98-8730-11D3-B388-00C04F68574B")]
public interface IWMPCdrom
{
	[DispId(251)]
	string driveSpecifier
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(251)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(252)]
	IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(252)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(253)]
	void eject();
}
[ComImport]
[TypeLibType(4160)]
[Guid("4F2DF574-C588-11D3-9ED0-00C04FB6E937")]
public interface IWMPClosedCaption
{
	[DispId(951)]
	string SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(951)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(951)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(952)]
	string SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(952)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(952)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(953)]
	string SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(953)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(953)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(954)]
	string captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(954)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(954)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("A12DCF7D-14AB-4C1B-A8CD-63909F06025B")]
public interface IWMPError
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(851)]
	void clearErrorQueue();

	[DispId(852)]
	int errorCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(852)]
		get;
	}

	[DispId(853)]
	IWMPErrorItem Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(853)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(854)]
	void webHelp();
}
[ComImport]
[Guid("3614C646-3B3B-4DE7-A81E-930E3F2127B3")]
[TypeLibType(4160)]
public interface IWMPErrorItem
{
	[DispId(901)]
	int errorCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(901)]
		get;
	}

	[DispId(902)]
	string errorDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(902)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(903)]
	object errorContext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(903)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	[DispId(904)]
	int remedy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(904)]
		get;
	}

	[DispId(905)]
	string customUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(905)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("8DA61686-4668-4A5C-AE5D-803193293DBE")]
public interface IWMPDVD
{
	[DispId(1001)]
	bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1001)]
		get;
	}

	[DispId(1002)]
	string domain
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1003)]
	void topMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1004)]
	void titleMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1005)]
	void back();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1006)]
	void resume();
}
[ComImport]
[Guid("40897764-CEAB-47BE-AD4A-8E28537F9BBF")]
[TypeLibType(4160)]
public interface IWMPPlayerApplication
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1101)]
	void switchToPlayerApplication();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1102)]
	void switchToControl();

	[DispId(1103)]
	bool playerDocked
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1103)]
		get;
	}

	[DispId(1104)]
	bool hasDisplay
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1104)]
		get;
	}
}
[ComImport]
[Guid("54062B68-052A-4C25-A39F-8B63346511D4")]
[TypeLibType(4160)]
public interface IWMPPlayer3 : IWMPCore2
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	new IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(19)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	[DispId(21)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		set;
	}

	[DispId(22)]
	bool enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		set;
	}

	[DispId(23)]
	string uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(24)]
	bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		[param: In]
		set;
	}

	[DispId(25)]
	bool windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		[param: In]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("0E6B01D1-D407-4C85-BF5F-1C01F6150280")]
public interface IWMPPlayer2 : IWMPCore
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(19)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	[DispId(21)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		set;
	}

	[DispId(22)]
	bool enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		set;
	}

	[DispId(23)]
	string uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(24)]
	bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		[param: In]
		set;
	}

	[DispId(25)]
	bool windowlessVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		[param: In]
		set;
	}
}
[ComImport]
[Guid("6BF52A4F-394A-11D3-B153-00C04F79FAA6")]
[TypeLibType(4160)]
public interface IWMPPlayer : IWMPCore
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	new void close();

	[DispId(1)]
	new string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	new WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	new WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	new IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	new IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	new IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	new IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	new string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	new void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(7)]
	new IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	new IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	new IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	new IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	new bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	new IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	new string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(19)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[param: In]
		set;
	}

	[DispId(21)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		set;
	}

	[DispId(22)]
	bool enableContextMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		set;
	}

	[DispId(23)]
	string uiMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("F75CCEC0-C67C-475C-931E-8719870BEE7D")]
public interface IWMPErrorItem2 : IWMPErrorItem
{
	[DispId(901)]
	new int errorCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(901)]
		get;
	}

	[DispId(902)]
	new string errorDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(902)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(903)]
	new object errorContext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(903)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	[DispId(904)]
	new int remedy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(904)]
		get;
	}

	[DispId(905)]
	new string customUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(905)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(906)]
	int condition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(906)]
		get;
	}
}
[ComImport]
[Guid("6F030D25-0890-480F-9775-1F7E40AB5B8E")]
[TypeLibType(4160)]
public interface IWMPControls2 : IWMPControls
{
	[DispId(62)]
	new bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(62)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	new void play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	new void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	new void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	new void fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	new void fastReverse();

	[DispId(56)]
	new double currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[param: In]
		set;
	}

	[DispId(57)]
	new string currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(57)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	new void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	new void previous();

	[DispId(60)]
	new IWMPMedia currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(61)]
	new int currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	new void playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(64)]
	void step([In] int lStep);
}
[ComImport]
[TypeLibType(4160)]
[Guid("AB7C88BB-143E-4EA4-ACC3-E4350B2106C3")]
public interface IWMPMedia2 : IWMPMedia
{
	[DispId(763)]
	new bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(763)]
		get;
	}

	[DispId(751)]
	new string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(751)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(764)]
	new string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(752)]
	new int imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(752)]
		get;
	}

	[DispId(753)]
	new int imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(753)]
		get;
	}

	[DispId(754)]
	new int markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(754)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(755)]
	new double getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(756)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getMarkerName([In] int MarkerNum);

	[DispId(757)]
	new double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(757)]
		get;
	}

	[DispId(758)]
	new string durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(758)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(759)]
	new int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(759)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(760)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(761)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(762)]
	new void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(765)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(766)]
	new bool isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(767)]
	new bool isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[DispId(768)]
	IWMPErrorItem Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(768)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[Guid("F118EFC7-F03A-4FB4-99C9-1C02A5C1065B")]
[TypeLibType(4160)]
public interface IWMPMedia3 : IWMPMedia2
{
	[DispId(763)]
	new bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(763)]
		get;
	}

	[DispId(751)]
	new string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(751)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(764)]
	new string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(764)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(752)]
	new int imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(752)]
		get;
	}

	[DispId(753)]
	new int imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(753)]
		get;
	}

	[DispId(754)]
	new int markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(754)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(755)]
	new double getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(756)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getMarkerName([In] int MarkerNum);

	[DispId(757)]
	new double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(757)]
		get;
	}

	[DispId(758)]
	new string durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(758)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(759)]
	new int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(759)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(760)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(761)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(762)]
	new void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(765)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(766)]
	new bool isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(767)]
	new bool isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[DispId(768)]
	new IWMPErrorItem Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(768)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(769)]
	int getAttributeCountByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(770)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object getItemInfoByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage, [In] int lIndex);
}
[ComImport]
[TypeLibType(4160)]
[Guid("5C29BBE0-F87D-4C45-AA28-A70F0230FFA9")]
public interface IWMPMetadataPicture
{
	[DispId(1051)]
	string mimeType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1051)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1052)]
	string pictureType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1052)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1053)]
	string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1053)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1054)]
	string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1054)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("769A72DB-13D2-45E2-9C48-53CA9D5B7450")]
public interface IWMPMetadataText
{
	[DispId(1056)]
	string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1056)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1055)]
	string text
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1055)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("FDA937A4-EECE-4DA5-A0B6-39BF89ADE2C2")]
public interface IWMPSettings2 : IWMPSettings
{
	[DispId(113)]
	new bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		get;
	}

	[DispId(101)]
	new bool autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[param: In]
		set;
	}

	[DispId(108)]
	new string baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(109)]
	new string defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(103)]
	new bool invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		[param: In]
		set;
	}

	[DispId(104)]
	new bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		[param: In]
		set;
	}

	[DispId(105)]
	new int playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		[param: In]
		set;
	}

	[DispId(106)]
	new double rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		[param: In]
		set;
	}

	[DispId(102)]
	new int balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		[param: In]
		set;
	}

	[DispId(107)]
	new int volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(110)]
	new bool getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(111)]
	new void setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[DispId(112)]
	new bool enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(112)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(112)]
		[param: In]
		set;
	}

	[DispId(114)]
	int defaultAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(114)]
		get;
	}

	[DispId(115)]
	string mediaAccessRights
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(115)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(116)]
	bool requestMediaAccessRights([In][MarshalAs(UnmanagedType.BStr)] string bstrDesiredAccess);
}
[ComImport]
[Guid("A1D1110E-D545-476A-9A78-AC3E4CB1E6BD")]
[TypeLibType(4160)]
public interface IWMPControls3 : IWMPControls2
{
	[DispId(62)]
	new bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(62)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	new void play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	new void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	new void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	new void fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	new void fastReverse();

	[DispId(56)]
	new double currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[param: In]
		set;
	}

	[DispId(57)]
	new string currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(57)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	new void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	new void previous();

	[DispId(60)]
	new IWMPMedia currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(60)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(61)]
	new int currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(61)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	new void playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(64)]
	new void step([In] int lStep);

	[DispId(65)]
	int audioLanguageCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(65)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(66)]
	int getAudioLanguageID([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(67)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getAudioLanguageDescription([In] int lIndex);

	[DispId(68)]
	int currentAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(68)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(68)]
		[param: In]
		set;
	}

	[DispId(69)]
	int currentAudioLanguageIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(69)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(69)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getLanguageName([In] int lLangID);

	[DispId(71)]
	string currentPositionTimecode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(71)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(71)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("350BA78B-6BC8-4113-A5F5-312056934EB6")]
public interface IWMPClosedCaption2 : IWMPClosedCaption
{
	[DispId(951)]
	new string SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(951)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(951)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(952)]
	new string SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(952)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(952)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(953)]
	new string SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(953)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(953)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(954)]
	new string captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(954)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(954)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(955)]
	int SAMILangCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(955)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(956)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getSAMILangName([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(957)]
	int getSAMILangID([In] int nIndex);

	[DispId(958)]
	int SAMIStyleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(958)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(959)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getSAMIStyleName([In] int nIndex);
}
[ComImport]
[TypeLibType(4160)]
[Guid("8BA957F5-FD8C-4791-B82D-F840401EE474")]
public interface IWMPMediaCollection2 : IWMPMediaCollection
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(452)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPMedia add([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(453)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(454)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(455)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getByGenre([In][MarshalAs(UnmanagedType.BStr)] string bstrGenre);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(456)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getByAuthor([In][MarshalAs(UnmanagedType.BStr)] string bstrAuthor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(457)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getByAlbum([In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(458)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPPlaylist getByAttribute([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(459)]
	new void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfDeleteFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(461)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new IWMPStringCollection getAttributeStringCollection([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(470)]
	new int getMediaAtom([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(471)]
	new void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(472)]
	new bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1401)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPQuery createQuery();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1402)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getPlaylistByQuery([In][MarshalAs(UnmanagedType.Interface)] IWMPQuery pQuery, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType, [In][MarshalAs(UnmanagedType.BStr)] string bstrSortAttribute, [In] bool fSortAscending);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1403)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPStringCollection getStringCollectionByQuery([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.Interface)] IWMPQuery pQuery, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType, [In][MarshalAs(UnmanagedType.BStr)] string bstrSortAttribute, [In] bool fSortAscending);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1404)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPPlaylist getByAttributeAndMediaType([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);
}
[ComImport]
[TypeLibType(4160)]
[Guid("A00918F3-A6B0-4BFB-9189-FD834C7BC5A5")]
public interface IWMPQuery
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1351)]
	void addCondition([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrOperator, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1352)]
	void beginNextGroup();
}
[ComImport]
[Guid("46AD648D-53F1-4A74-92E2-2A1B68D63FD4")]
[TypeLibType(4160)]
public interface IWMPStringCollection2 : IWMPStringCollection
{
	[DispId(401)]
	new int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(401)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(402)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1451)]
	bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPStringCollection2 pIWMPStringCollection2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1452)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1453)]
	int getAttributeCountByType([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1454)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object getItemInfoByType([In] int lCollectionIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage, [In] int lAttributeIndex);
}
[ComImport]
[InterfaceType(1)]
[Guid("1D01FBDB-ADE2-4C8D-9842-C190B95C3306")]
public interface IWMPPlayerServices
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void activateUIPlugin([In][MarshalAs(UnmanagedType.BStr)] string bstrPlugin);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void setTaskPane([In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void setTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL, [In][MarshalAs(UnmanagedType.BStr)] string bstrFriendlyName);
}
[ComImport]
[Guid("1BB1592F-F040-418A-9F71-17C7512B4D70")]
[InterfaceType(1)]
public interface IWMPPlayerServices2 : IWMPPlayerServices
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void activateUIPlugin([In][MarshalAs(UnmanagedType.BStr)] string bstrPlugin);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void setTaskPane([In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void setTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL, [In][MarshalAs(UnmanagedType.BStr)] string bstrFriendlyName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void setBackgroundProcessingPriority([In][MarshalAs(UnmanagedType.BStr)] string bstrPriority);
}
[ComImport]
[Guid("CBB92747-741F-44FE-AB5B-F1A48F3B2A59")]
[InterfaceType(1)]
[TypeLibType(256)]
public interface IWMPRemoteMediaServices
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetServiceType([MarshalAs(UnmanagedType.BStr)] out string pbstrType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetApplicationName([MarshalAs(UnmanagedType.BStr)] out string pbstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetScriptableObject([MarshalAs(UnmanagedType.BStr)] out string pbstrName, [MarshalAs(UnmanagedType.IDispatch)] out object ppDispatch);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetCustomUIMode([MarshalAs(UnmanagedType.BStr)] out string pbstrFile);
}
[ComImport]
[InterfaceType(1)]
[Guid("8B5050FF-E0A4-4808-B3A8-893A9E1ED894")]
[TypeLibType(256)]
public interface IWMPSyncServices
{
	[DispId(1610678272)]
	int deviceCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPSyncDevice getDevice([In] int lIndex);
}
[ComImport]
[TypeLibType(256)]
[InterfaceType(1)]
[Guid("39C2F8D5-1CF2-4D5E-AE09-D73492CF9EAA")]
public interface IWMPLibraryServices
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	int getCountByType([In] WMPLibraryType wmplt);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPLibrary getLibraryByType([In] WMPLibraryType wmplt, [In] int lIndex);
}
[ComImport]
[InterfaceType(1)]
[Guid("82CBA86B-9F04-474B-A365-D6DD1466E541")]
[TypeLibType(256)]
public interface IWMPLibrarySharingServices
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	bool isLibraryShared();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	bool isLibrarySharingEnabled();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void showLibrarySharing();
}
[ComImport]
[Guid("DD578A4E-79B1-426C-BF8F-3ADD9072500B")]
[InterfaceType(1)]
[TypeLibType(256)]
public interface IWMPLibrary2 : IWMPLibrary
{
	[DispId(1610678272)]
	new string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678273)]
	new WMPLibraryType type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678274)]
	new IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPLibrary pIWMPLibrary);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
}
[ComImport]
[InterfaceType(1)]
[Guid("788C8743-E57F-439D-A468-5BC77F2E59C6")]
[TypeLibType(256)]
public interface IWMPFolderMonitorServices
{
	[DispId(1610678272)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void add([In][MarshalAs(UnmanagedType.BStr)] string bstrFolder);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void remove([In] int lIndex);

	[DispId(1610678276)]
	WMPFolderScanState scanState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678277)]
	string currentFolder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678278)]
	int scannedFilesCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678279)]
	int addedFilesCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678280)]
	int updateProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void startScan();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void stopScan();
}
[ComImport]
[Guid("88AFB4B2-140A-44D2-91E6-4543DA467CD1")]
[InterfaceType(1)]
[TypeLibType(256)]
public interface IWMPSyncDevice2 : IWMPSyncDevice
{
	[DispId(1610678272)]
	new string friendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1610678274)]
	new string deviceName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678275)]
	new string deviceId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678276)]
	new int partnershipIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678277)]
	new bool connected
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678278)]
	new WMPDeviceStatus status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678279)]
	new WMPSyncState syncState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678280)]
	new int progress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void createPartnership([In] bool vbShowUI);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void deletePartnership();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Start();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void showSettings();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);
}
[ComImport]
[InterfaceType(1)]
[Guid("B22C85F9-263C-4372-A0DA-B518DB9B4098")]
[TypeLibType(256)]
public interface IWMPSyncDevice3 : IWMPSyncDevice2
{
	[DispId(1610678272)]
	new string friendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1610678274)]
	new string deviceName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678275)]
	new string deviceId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610678276)]
	new int partnershipIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678277)]
	new bool connected
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678278)]
	new WMPDeviceStatus status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678279)]
	new WMPSyncState syncState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[DispId(1610678280)]
	new int progress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	new string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void createPartnership([In] bool vbShowUI);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void deletePartnership();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void Start();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void showSettings();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new bool isIdentical([In][MarshalAs(UnmanagedType.Interface)] IWMPSyncDevice pDevice);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void estimateSyncSize([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pNonRulePlaylist, [In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pRulesPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void cancelEstimation();
}
[ComImport]
[Guid("5F9CFD92-8CAD-11D3-9A7E-00C04F8EFB70")]
[TypeLibType(4160)]
public interface IWMPPlaylistCtrl
{
	[DispId(5601)]
	IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5601)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5601)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(5602)]
	string columns
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5602)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5602)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5603)]
	int columnCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5603)]
		get;
	}

	[DispId(5604)]
	string columnOrder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5604)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5604)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5605)]
	bool columnsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5605)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5605)]
		[param: In]
		set;
	}

	[DispId(5607)]
	bool dropDownVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5607)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5607)]
		[param: In]
		set;
	}

	[DispId(5608)]
	bool playlistItemsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5608)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5608)]
		[param: In]
		set;
	}

	[DispId(5609)]
	bool checkboxesVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5609)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5609)]
		[param: In]
		set;
	}

	[DispId(5612)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5612)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5612)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5613)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5613)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5613)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5614)]
	string disabledItemColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5614)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5614)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5615)]
	string itemPlayingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5615)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5615)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5616)]
	string itemPlayingBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5616)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5616)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5617)]
	string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5617)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5617)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5618)]
	bool allowItemEditing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5618)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5618)]
		[param: In]
		set;
	}

	[DispId(5619)]
	bool allowColumnSorting
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5619)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5619)]
		[param: In]
		set;
	}

	[DispId(5620)]
	string dropDownList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5620)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5620)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5621)]
	string dropDownToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5621)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5621)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5622)]
	bool copying
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5622)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5622)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5623)]
	void copy();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5624)]
	void abortCopy();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5625)]
	void deleteSelected();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5626)]
	void deleteSelectedFromLibrary();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5628)]
	void moveSelectedUp();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5629)]
	void moveSelectedDown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5630)]
	void addSelectedToPlaylist([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pdispPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5631)]
	int getNextSelectedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5632)]
	int getNextCheckedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5633)]
	void setSelectedState([In] int nIndex, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5634)]
	void setCheckedState([In] int nIndex, [In] bool vbChecked);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5635)]
	void sortColumn([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5636)]
	void setColumnResizeMode([In] int nIndex, [In][MarshalAs(UnmanagedType.BStr)] string newMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5637)]
	void setColumnWidth([In] int nIndex, [In] int nWidth);

	[DispId(5642)]
	string itemErrorColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5642)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5642)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5643)]
	int itemCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5643)]
		get;
	}

	[DispId(5644)]
	IWMPMedia itemMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5644)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5645)]
	IWMPPlaylist itemPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5645)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5646)]
	int getNextSelectedItem2([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5647)]
	int getNextCheckedItem2([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5648)]
	void setSelectedState2([In] int nIndex, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5649)]
	void setCheckedState2([In] int nIndex, [In] bool vbChecked);

	[DispId(5650)]
	string leftStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5650)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5650)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5651)]
	string rightStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5651)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5651)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5652)]
	bool editButtonVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5652)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5652)]
		[param: In]
		set;
	}

	[DispId(5653)]
	string dropDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5653)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5653)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5654)]
	string dropDownBackgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5654)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5654)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5655)]
	float hueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5655)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5655)]
		[param: In]
		set;
	}

	[DispId(5656)]
	float saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5656)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5656)]
		[param: In]
		set;
	}

	[DispId(5658)]
	string statusColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5658)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5658)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5660)]
	bool toolbarVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5660)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5660)]
		[param: In]
		set;
	}

	[DispId(5662)]
	string itemSelectedColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5662)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5662)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5663)]
	string itemSelectedFocusLostColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5663)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5663)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5664)]
	string itemSelectedBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5664)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5664)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5665)]
	string itemSelectedBackgroundFocusLostColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5665)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5665)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5666)]
	string backgroundSplitColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5666)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5666)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5667)]
	string statusTextColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5667)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5667)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("E41C88DD-2364-4FF7-A0F5-CA9859AF783F")]
public interface IAppDispatch
{
	[DispId(100)]
	bool titlebarVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[param: In]
		set;
	}

	[DispId(101)]
	bool titlebarAutoHide
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[param: In]
		set;
	}

	[DispId(102)]
	string currentTask
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(145)]
	int libraryBasketMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(145)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(145)]
		[param: In]
		set;
	}

	[DispId(146)]
	int libraryBasketWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(146)]
		get;
	}

	[DispId(147)]
	int breadcrumbItemCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(147)]
		get;
	}

	[DispId(148)]
	string breadcrumbItemName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(148)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(149)]
	bool breadcrumbItemHasMenu
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(149)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(150)]
	void breadcrumbItemClick([In] int lIndex);

	[DispId(103)]
	bool settingsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		[param: In]
		set;
	}

	[DispId(104)]
	bool playlistVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(104)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(105)]
	void gotoSkinMode();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(143)]
	void gotoPlayerMode();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(144)]
	void gotoLibraryMode([In] int lButton);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(125)]
	void navigatePrevious();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(126)]
	void navigateNext();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(142)]
	void goFullScreen();

	[DispId(141)]
	bool fullScreenEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(141)]
		get;
	}

	[DispId(132)]
	bool serviceLoginVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(132)]
		get;
	}

	[DispId(133)]
	bool serviceLoginSignedIn
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(133)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(134)]
	void serviceLogin();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(135)]
	void serviceLogout();

	[DispId(140)]
	object serviceGetInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(140)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	[DispId(123)]
	bool navigatePreviousEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		get;
	}

	[DispId(124)]
	bool navigateNextEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(124)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(130)]
	void navigateToAddress([In][MarshalAs(UnmanagedType.BStr)] string address);

	[DispId(131)]
	bool glassEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(131)]
		get;
	}

	[DispId(136)]
	bool inVistaPlus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(136)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(106)]
	void adjustLeft([In] int nDistance);

	[DispId(107)]
	bool taskbarVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		[param: In]
		set;
	}

	[DispId(116)]
	int DPI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		get;
	}

	[DispId(114)]
	bool previousEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(114)]
		get;
	}

	[DispId(139)]
	bool playLibraryItemEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(139)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(115)]
	void previous();

	[DispId(108)]
	bool titlebarCurrentlyVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		get;
	}

	[DispId(137)]
	bool menubarCurrentlyVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(137)]
		get;
	}

	[DispId(109)]
	bool bgPluginRunning
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(110)]
	void configurePlugins([In] int nType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(111)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getTimeString([In] double dTime);

	[DispId(113)]
	bool maximized
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(113)]
		get;
	}

	[DispId(117)]
	int top
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(117)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(117)]
		[param: In]
		set;
	}

	[DispId(118)]
	int left
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[param: In]
		set;
	}

	[DispId(119)]
	int width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(119)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(119)]
		[param: In]
		set;
	}

	[DispId(120)]
	int height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(120)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(120)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(121)]
	void setWindowPos([In] int lTop, [In] int lLeft, [In] int lWidth, [In] int lHeight);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(122)]
	void logData([In][MarshalAs(UnmanagedType.BStr)] string ID, [In][MarshalAs(UnmanagedType.BStr)] string data);

	[DispId(127)]
	string powerPersonality
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(127)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(128)]
	void navigateNamespace([In][MarshalAs(UnmanagedType.BStr)] string address);

	[DispId(129)]
	string exclusiveService
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(129)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(138)]
	string windowText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(138)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(151)]
	int resourceIdForDpi
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(151)]
		get;
	}
}
[ComImport]
[Guid("EF870383-83AB-4EA9-BE48-56FA4251AF10")]
[TypeLibType(4160)]
public interface IWMPSafeBrowser
{
	[DispId(8400)]
	string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8400)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8400)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(8401)]
	int status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8401)]
		get;
	}

	[DispId(8402)]
	int pendingDownloads
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8402)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8403)]
	void showSAMIText([In][MarshalAs(UnmanagedType.BStr)] string samiText);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8404)]
	void showLyrics([In][MarshalAs(UnmanagedType.BStr)] string lyrics);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8405)]
	void loadSpecialPage([In][MarshalAs(UnmanagedType.BStr)] string pageName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8406)]
	void goBack();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8407)]
	void goForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8408)]
	void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8409)]
	void refresh();

	[DispId(8410)]
	string baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8410)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(8414)]
	string fullURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8414)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(8411)]
	int secureLock
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8411)]
		get;
	}

	[DispId(8412)]
	bool busy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8412)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8413)]
	void showCert();
}
[ComImport]
[TypeLibType(4160)]
[Guid("21D077C1-4BAA-11D3-BD45-00C04F6EA5AE")]
public interface IWMPObjectExtendedProps
{
	[DispId(2000)]
	string ID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2000)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2001)]
	string elementType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2002)]
	int left
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		[param: In]
		set;
	}

	[DispId(2003)]
	int top
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		[param: In]
		set;
	}

	[DispId(2022)]
	int right
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		[param: In]
		set;
	}

	[DispId(2023)]
	int bottom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		[param: In]
		set;
	}

	[DispId(2004)]
	int width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		[param: In]
		set;
	}

	[DispId(2005)]
	int height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		[param: In]
		set;
	}

	[DispId(2006)]
	int zIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		[param: In]
		set;
	}

	[DispId(2007)]
	string clippingImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2008)]
	string clippingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2009)]
	bool visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		[param: In]
		set;
	}

	[DispId(2010)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		[param: In]
		set;
	}

	[DispId(2011)]
	bool tabStop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		[param: In]
		set;
	}

	[DispId(2012)]
	bool passThrough
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		[param: In]
		set;
	}

	[DispId(2013)]
	string horizontalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2014)]
	string verticalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2015)]
	void moveTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2021)]
	void slideTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2026)]
	void moveSizeTo([In] int newX, [In] int newY, [In] int newWidth, [In] int newHeight, [In] int moveTime, [In] bool fSlide);

	[DispId(2016)]
	int alphaBlend
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2017)]
	void alphaBlendTo([In] int newVal, [In] int alphaTime);

	[DispId(2018)]
	string accName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2019)]
	string accDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2020)]
	string accKeyboardShortcut
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2024)]
	bool resizeImages
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		[param: In]
		set;
	}

	[DispId(2025)]
	string nineGridMargins
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2027)]
	string resizeOptimize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2028)]
	float rotation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		[param: In]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("72F486B1-0D43-11D3-BD3F-00C04F6EA5AE")]
public interface IWMPLayoutSubView : IWMPObjectExtendedProps
{
	[DispId(2000)]
	new string ID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2000)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2001)]
	new string elementType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2002)]
	new int left
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		[param: In]
		set;
	}

	[DispId(2003)]
	new int top
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		[param: In]
		set;
	}

	[DispId(2022)]
	new int right
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		[param: In]
		set;
	}

	[DispId(2023)]
	new int bottom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		[param: In]
		set;
	}

	[DispId(2004)]
	new int width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		[param: In]
		set;
	}

	[DispId(2005)]
	new int height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		[param: In]
		set;
	}

	[DispId(2006)]
	new int zIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		[param: In]
		set;
	}

	[DispId(2007)]
	new string clippingImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2008)]
	new string clippingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2009)]
	new bool visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		[param: In]
		set;
	}

	[DispId(2010)]
	new bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		[param: In]
		set;
	}

	[DispId(2011)]
	new bool tabStop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		[param: In]
		set;
	}

	[DispId(2012)]
	new bool passThrough
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		[param: In]
		set;
	}

	[DispId(2013)]
	new string horizontalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2014)]
	new string verticalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2015)]
	new void moveTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2021)]
	new void slideTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2026)]
	new void moveSizeTo([In] int newX, [In] int newY, [In] int newWidth, [In] int newHeight, [In] int moveTime, [In] bool fSlide);

	[DispId(2016)]
	new int alphaBlend
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2017)]
	new void alphaBlendTo([In] int newVal, [In] int alphaTime);

	[DispId(2018)]
	new string accName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2019)]
	new string accDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2020)]
	new string accKeyboardShortcut
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2024)]
	new bool resizeImages
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		[param: In]
		set;
	}

	[DispId(2025)]
	new string nineGridMargins
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2027)]
	new string resizeOptimize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2028)]
	new float rotation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		[param: In]
		set;
	}

	[DispId(2300)]
	string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2300)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2300)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2301)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2301)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2301)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2302)]
	string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2302)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2302)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2303)]
	bool backgroundTiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2303)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2303)]
		[param: In]
		set;
	}

	[DispId(2304)]
	float backgroundImageHueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2304)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2304)]
		[param: In]
		set;
	}

	[DispId(2305)]
	float backgroundImageSaturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2305)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2305)]
		[param: In]
		set;
	}

	[DispId(2306)]
	bool resizeBackgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2306)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2306)]
		[param: In]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("172E905D-80D9-4C2F-B7CE-2CCB771787A2")]
public interface IWMPLayoutView : IWMPLayoutSubView
{
	[DispId(2000)]
	new string ID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2000)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2001)]
	new string elementType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2002)]
	new int left
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2002)]
		[param: In]
		set;
	}

	[DispId(2003)]
	new int top
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2003)]
		[param: In]
		set;
	}

	[DispId(2022)]
	new int right
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2022)]
		[param: In]
		set;
	}

	[DispId(2023)]
	new int bottom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2023)]
		[param: In]
		set;
	}

	[DispId(2004)]
	new int width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2004)]
		[param: In]
		set;
	}

	[DispId(2005)]
	new int height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2005)]
		[param: In]
		set;
	}

	[DispId(2006)]
	new int zIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2006)]
		[param: In]
		set;
	}

	[DispId(2007)]
	new string clippingImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2007)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2008)]
	new string clippingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2009)]
	new bool visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2009)]
		[param: In]
		set;
	}

	[DispId(2010)]
	new bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2010)]
		[param: In]
		set;
	}

	[DispId(2011)]
	new bool tabStop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2011)]
		[param: In]
		set;
	}

	[DispId(2012)]
	new bool passThrough
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2012)]
		[param: In]
		set;
	}

	[DispId(2013)]
	new string horizontalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2013)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2014)]
	new string verticalAlignment
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2014)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2015)]
	new void moveTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2021)]
	new void slideTo([In] int newX, [In] int newY, [In] int moveTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2026)]
	new void moveSizeTo([In] int newX, [In] int newY, [In] int newWidth, [In] int newHeight, [In] int moveTime, [In] bool fSlide);

	[DispId(2016)]
	new int alphaBlend
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2016)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2017)]
	new void alphaBlendTo([In] int newVal, [In] int alphaTime);

	[DispId(2018)]
	new string accName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2019)]
	new string accDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2019)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2020)]
	new string accKeyboardShortcut
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2020)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2024)]
	new bool resizeImages
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2024)]
		[param: In]
		set;
	}

	[DispId(2025)]
	new string nineGridMargins
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2025)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2027)]
	new string resizeOptimize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2028)]
	new float rotation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2028)]
		[param: In]
		set;
	}

	[DispId(2300)]
	new string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2300)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2300)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2301)]
	new string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2301)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2301)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2302)]
	new string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2302)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2302)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2303)]
	new bool backgroundTiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2303)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2303)]
		[param: In]
		set;
	}

	[DispId(2304)]
	new float backgroundImageHueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2304)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2304)]
		[param: In]
		set;
	}

	[DispId(2305)]
	new float backgroundImageSaturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2305)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2305)]
		[param: In]
		set;
	}

	[DispId(2306)]
	new bool resizeBackgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2306)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2306)]
		[param: In]
		set;
	}

	[DispId(2307)]
	string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2307)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2307)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2308)]
	string category
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2308)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2308)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2309)]
	string focusObjectID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2309)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2309)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2311)]
	bool titleBar
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2311)]
		get;
	}

	[DispId(2312)]
	bool resizable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2312)]
		get;
	}

	[DispId(2313)]
	int timerInterval
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2313)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2313)]
		[param: In]
		set;
	}

	[DispId(2314)]
	int minWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2314)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2314)]
		[param: In]
		set;
	}

	[DispId(2315)]
	int maxWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2315)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2315)]
		[param: In]
		set;
	}

	[DispId(2316)]
	int minHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2316)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2316)]
		[param: In]
		set;
	}

	[DispId(2317)]
	int maxHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2317)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2317)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2318)]
	void close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2319)]
	void minimize();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2320)]
	void maximize();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2321)]
	void restore();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2322)]
	void size([In][MarshalAs(UnmanagedType.BStr)] string bstrDirection);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2323)]
	void returnToMediaCenter();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2324)]
	void updateWindow();

	[DispId(2326)]
	bool maximized
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2326)]
		get;
	}

	[DispId(2327)]
	bool minimized
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2327)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("5AF0BEC1-46AA-11D3-BD45-00C04F6EA5AE")]
public interface IWMPEventObject
{
	[DispId(2200)]
	object srcElement
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2200)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(2201)]
	bool altKey
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2201)]
		get;
	}

	[DispId(2202)]
	bool ctrlKey
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2202)]
		get;
	}

	[DispId(2203)]
	bool shiftKey
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2203)]
		get;
	}

	[DispId(2204)]
	object fromElement
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2204)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(2205)]
	object toElement
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2205)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(2206)]
	int keyCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2206)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2206)]
		[param: In]
		set;
	}

	[DispId(2207)]
	int button
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2207)]
		get;
	}

	[DispId(2208)]
	int x
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2208)]
		get;
	}

	[DispId(2209)]
	int y
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2209)]
		get;
	}

	[DispId(2210)]
	int clientX
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2210)]
		get;
	}

	[DispId(2211)]
	int clientY
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2211)]
		get;
	}

	[DispId(2212)]
	int offsetX
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2212)]
		get;
	}

	[DispId(2213)]
	int offsetY
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2213)]
		get;
	}

	[DispId(2214)]
	int screenX
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2214)]
		get;
	}

	[DispId(2215)]
	int screenY
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2215)]
		get;
	}

	[DispId(2216)]
	int screenWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2216)]
		get;
	}

	[DispId(2217)]
	int screenHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2217)]
		get;
	}

	[DispId(2218)]
	bool penOrTouch
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2218)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("6FCAE13D-E492-4584-9C21-D2C052A2A33A")]
public interface IWMPTheme
{
	[DispId(2500)]
	string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2500)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2501)]
	float version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2501)]
		get;
	}

	[DispId(2502)]
	string authorVersion
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2502)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2503)]
	string author
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2503)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2504)]
	string copyright
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2504)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2505)]
	string currentViewID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2505)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2505)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2506)]
	void showErrorDialog();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2507)]
	void logString([In][MarshalAs(UnmanagedType.BStr)] string stringVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2508)]
	void openView([In][MarshalAs(UnmanagedType.BStr)] string viewID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2515)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object openViewRelative([In][MarshalAs(UnmanagedType.BStr)] string viewID, [In] int x, [In] int y);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2509)]
	void closeView([In][MarshalAs(UnmanagedType.BStr)] string viewID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2510)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string openDialog([In][MarshalAs(UnmanagedType.BStr)] string dialogType, [In][MarshalAs(UnmanagedType.BStr)] string parameters);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2511)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string loadString([In][MarshalAs(UnmanagedType.BStr)] string bstrString);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2512)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string loadPreference([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2513)]
	void savePreference([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2514)]
	void playSound([MarshalAs(UnmanagedType.BStr)] string bstrFilename);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2516)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object openViewRelativeInternal([In][MarshalAs(UnmanagedType.BStr)] string viewID, [In] int nIndex, [In] int x, [In] int y, [In] int nWidth, [In] int nHeight, [In][MarshalAs(UnmanagedType.BStr)] string bstrHorizontalAlignment, [In][MarshalAs(UnmanagedType.BStr)] string bstrVerticalAlignment);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2518)]
	void setViewPosition([In][MarshalAs(UnmanagedType.BStr)] string viewID, [In] int nIndex, [In] int x, [In] int y, [In] int nWidth, [In] int nHeight, [In][MarshalAs(UnmanagedType.BStr)] string bstrHorizontalAlignment, [In][MarshalAs(UnmanagedType.BStr)] string bstrVerticalAlignment);
}
[ComImport]
[Guid("B2C2D18E-97AF-4B6A-A56B-2FFFF470FB81")]
[TypeLibType(4160)]
public interface IWMPLayoutSettingsDispatch
{
	[DispId(2800)]
	string effectType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2800)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2800)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2801)]
	int effectPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2801)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2801)]
		[param: In]
		set;
	}

	[DispId(2802)]
	string settingsView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2802)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2802)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2803)]
	int videoZoom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2803)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2803)]
		[param: In]
		set;
	}

	[DispId(2804)]
	bool videoShrinkToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2804)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2804)]
		[param: In]
		set;
	}

	[DispId(2805)]
	bool videoStretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2805)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2805)]
		[param: In]
		set;
	}

	[DispId(2868)]
	bool userVideoStretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2868)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2868)]
		[param: In]
		set;
	}

	[DispId(2807)]
	bool showCaptions
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2807)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2807)]
		[param: In]
		set;
	}

	[DispId(2808)]
	bool showTitles
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2808)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2808)]
		[param: In]
		set;
	}

	[DispId(2809)]
	bool showEffects
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2809)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2809)]
		[param: In]
		set;
	}

	[DispId(2811)]
	bool showFullScreenPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2811)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2811)]
		[param: In]
		set;
	}

	[DispId(2813)]
	string contrastMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2813)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2810)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getNamedString([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2815)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getDurationStringFromSeconds([In] int lDurationVal);

	[DispId(2816)]
	string displayView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2816)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2816)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2817)]
	string metadataView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2817)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2817)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2818)]
	bool showSettings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2818)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2818)]
		[param: In]
		set;
	}

	[DispId(2819)]
	bool showResizeBars
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2819)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2819)]
		[param: In]
		set;
	}

	[DispId(2820)]
	bool showPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2820)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2820)]
		[param: In]
		set;
	}

	[DispId(2821)]
	bool showMetadata
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2821)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2821)]
		[param: In]
		set;
	}

	[DispId(2822)]
	int settingsWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2822)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2822)]
		[param: In]
		set;
	}

	[DispId(2823)]
	int settingsHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2823)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2823)]
		[param: In]
		set;
	}

	[DispId(2824)]
	int playlistWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2824)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2824)]
		[param: In]
		set;
	}

	[DispId(2825)]
	int playlistHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2825)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2825)]
		[param: In]
		set;
	}

	[DispId(2826)]
	int metadataWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2826)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2826)]
		[param: In]
		set;
	}

	[DispId(2827)]
	int metadataHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2827)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2827)]
		[param: In]
		set;
	}

	[DispId(2828)]
	bool fullScreenAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2828)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2828)]
		[param: In]
		set;
	}

	[DispId(2829)]
	bool fullScreenRequest
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2829)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2829)]
		[param: In]
		set;
	}

	[DispId(2830)]
	bool quickHide
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2830)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2830)]
		[param: In]
		set;
	}

	[DispId(2831)]
	int displayPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2831)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2831)]
		[param: In]
		set;
	}

	[DispId(2832)]
	int settingsPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2832)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2832)]
		[param: In]
		set;
	}

	[DispId(2833)]
	int metadataPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2833)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2833)]
		[param: In]
		set;
	}

	[DispId(2834)]
	string userDisplayView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2834)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2835)]
	string userWMPDisplayView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2835)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2836)]
	int userDisplayPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2836)]
		get;
	}

	[DispId(2837)]
	int userWMPDisplayPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2837)]
		get;
	}

	[DispId(2838)]
	int dynamicRangeControl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2838)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2838)]
		[param: In]
		set;
	}

	[DispId(2839)]
	float slowRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2839)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2839)]
		[param: In]
		set;
	}

	[DispId(2840)]
	float fastRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2840)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2840)]
		[param: In]
		set;
	}

	[DispId(2841)]
	float buttonHueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2841)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2841)]
		[param: In]
		set;
	}

	[DispId(2842)]
	float buttonSaturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2842)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2842)]
		[param: In]
		set;
	}

	[DispId(2843)]
	float backHueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2843)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2843)]
		[param: In]
		set;
	}

	[DispId(2844)]
	float backSaturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2844)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2844)]
		[param: In]
		set;
	}

	[DispId(2845)]
	int vizRequest
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2845)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2845)]
		[param: In]
		set;
	}

	[DispId(2847)]
	string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2847)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2848)]
	string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2848)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2849)]
	string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2849)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2856)]
	string toolbarButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2856)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2857)]
	string toolbarButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2857)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2858)]
	string toolbarButtonFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2858)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2850)]
	string itemPlayingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2850)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2851)]
	string itemPlayingBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2851)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2852)]
	string itemErrorColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2852)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2853)]
	bool appColorLimited
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2853)]
		get;
	}

	[DispId(2854)]
	bool appColorBlackBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2854)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2854)]
		[param: In]
		set;
	}

	[DispId(2855)]
	string appColorVideoBorder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2855)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2855)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2869)]
	string appColorAux1
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2869)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2870)]
	string appColorAux2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2870)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2871)]
	string appColorAux3
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2871)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2872)]
	string appColorAux4
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2872)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2873)]
	string appColorAux5
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2873)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2874)]
	string appColorAux6
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2874)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2875)]
	string appColorAux7
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2875)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2876)]
	string appColorAux8
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2876)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2877)]
	string appColorAux9
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2877)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2878)]
	string appColorAux10
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2878)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2879)]
	string appColorAux11
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2879)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2880)]
	string appColorAux12
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2880)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2881)]
	string appColorAux13
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2881)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2882)]
	string appColorAux14
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2882)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2883)]
	string appColorAux15
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2883)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2884)]
	string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2884)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2884)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2859)]
	string userWMPSettingsView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2859)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2860)]
	int userWMPSettingsPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2860)]
		get;
	}

	[DispId(2861)]
	bool userWMPShowSettings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2861)]
		get;
	}

	[DispId(2862)]
	string userWMPMetadataView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2862)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2863)]
	int userWMPMetadataPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2863)]
		get;
	}

	[DispId(2864)]
	bool userWMPShowMetadata
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2864)]
		get;
	}

	[DispId(2865)]
	int captionsHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2865)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2865)]
		[param: In]
		set;
	}

	[DispId(2866)]
	bool snapToVideo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2866)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2866)]
		[param: In]
		set;
	}

	[DispId(2867)]
	bool pinFullScreenControls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2867)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2867)]
		[param: In]
		set;
	}

	[DispId(2887)]
	bool isMultiMon
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2887)]
		get;
	}

	[DispId(2888)]
	float exclusiveHueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2888)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2888)]
		[param: In]
		set;
	}

	[DispId(2889)]
	float exclusiveSaturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2889)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2889)]
		[param: In]
		set;
	}

	[DispId(2892)]
	bool themeBkgColorIsActive
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2892)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2892)]
		[param: In]
		set;
	}

	[DispId(2890)]
	string themeBkgColorActive
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2890)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2891)]
	string themeBkgColorInactive
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2891)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("43D5AE92-4332-477C-8883-E0B3B063C5D2")]
public interface IWMPWindow
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3300)]
	void setWindowPos([In] int x, [In] int y, [In] int height, [In] int width);

	[DispId(3301)]
	int frameRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3301)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3301)]
		[param: In]
		set;
	}

	[DispId(3302)]
	int mouseX
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3302)]
		get;
	}

	[DispId(3303)]
	int mouseY
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3303)]
		get;
	}

	[DispId(3304)]
	object onsizing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3304)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3305)]
	void openViewAlwaysOnTop([MarshalAs(UnmanagedType.BStr)] string bstrViewID);
}
[ComImport]
[TypeLibType(4160)]
[Guid("98BB02D4-ED74-43CC-AD6A-45888F2E0DCC")]
public interface IWMPBrandDispatch
{
	[DispId(3040)]
	string fullServiceName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3040)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3000)]
	string friendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3000)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3001)]
	string guideButtonText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3002)]
	string guideButtonTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3003)]
	string guideMenuText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3003)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3004)]
	string guideAccText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3004)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3005)]
	string task1ButtonText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3006)]
	string task1ButtonTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3006)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3007)]
	string task1MenuText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3007)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3008)]
	string task1AccText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3017)]
	string guideUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3018)]
	string task1Url
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3018)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3021)]
	string imageLargeUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3021)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3022)]
	string imageSmallUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3022)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3023)]
	string imageMenuUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3023)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3024)]
	string infoCenterUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3024)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3025)]
	string albumInfoUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3025)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3026)]
	string buyCDUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3026)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3027)]
	string htmlViewUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3028)]
	string navigateUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3028)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3029)]
	string cookieUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3029)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3030)]
	string downloadStatusUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3030)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3031)]
	string colorPlayer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3031)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3032)]
	string colorPlayerText
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3032)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3035)]
	int navigateDispid
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3035)]
		get;
	}

	[DispId(3036)]
	string navigateParams
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3036)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3037)]
	string navigatePane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3037)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3038)]
	string selectedPane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3038)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3038)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3041)]
	void setNavigateProps([In][MarshalAs(UnmanagedType.BStr)] string bstrPane, [In] int lDispid, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3042)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getMediaParams([In][MarshalAs(UnmanagedType.IUnknown)] object pObject, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[DispId(3039)]
	int selectedTask
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3039)]
		[param: In]
		set;
	}

	[DispId(3043)]
	bool contentPartnerSelected
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3043)]
		get;
	}
}
[ComImport]
[Guid("504F112E-77CC-4E3C-A073-5371B31D9B36")]
[TypeLibType(4160)]
public interface IWMPNowPlayingHelperDispatch
{
	[DispId(2901)]
	string viewFriendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2901)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2902)]
	int viewPresetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2902)]
		get;
	}

	[DispId(2903)]
	string viewPresetName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2903)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2904)]
	string effectFriendlyName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2904)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2905)]
	string effectPresetName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2905)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2909)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string resolveDisplayView([In] bool fSafe);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2910)]
	bool isValidDisplayView([In][MarshalAs(UnmanagedType.BStr)] string bstrView);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2911)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getSkinFile();

	[DispId(2912)]
	bool captionsAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2912)]
		get;
	}

	[DispId(2913)]
	int linkAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2913)]
		get;
	}

	[DispId(2914)]
	int linkRequest
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2914)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2914)]
		[param: In]
		set;
	}

	[DispId(2915)]
	string linkRequestParams
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2915)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2915)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2917)]
	int getCurrentArtID([In] bool fLargeArt);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2918)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getTimeString([In] double dTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2919)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getCurrentScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string bstrType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2920)]
	void calcLayout([In] int lWidth, [In] int lHeight, [In] bool vbCaptions, [In] bool vbBanner);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2921)]
	int getLayoutSize([In] int nProp);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2922)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object getRootPlaylist([In][MarshalAs(UnmanagedType.IDispatch)] object pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2923)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getHTMLViewURL();

	[DispId(2926)]
	object editObj
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2926)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2926)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IUnknown)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2927)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getStatusString([In][MarshalAs(UnmanagedType.BStr)] string bstrStatusId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2939)]
	int getStatusPct([In][MarshalAs(UnmanagedType.BStr)] string bstrStatusId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2940)]
	int getStatusResult([In][MarshalAs(UnmanagedType.BStr)] string bstrStatusId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2941)]
	int getStatusIcon([In][MarshalAs(UnmanagedType.BStr)] string bstrStatusId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2942)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getStatusIdList();

	[DispId(2928)]
	string notificationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2928)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2930)]
	string htmlViewBaseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2930)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2930)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2933)]
	string htmlViewFullURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2933)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2933)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2929)]
	int htmlViewSecureLock
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2929)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2929)]
		[param: In]
		set;
	}

	[DispId(2931)]
	bool htmlViewBusy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2931)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2931)]
		[param: In]
		set;
	}

	[DispId(2932)]
	bool htmlViewShowCert
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2932)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2932)]
		[param: In]
		set;
	}

	[DispId(2934)]
	bool previousEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2934)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2934)]
		[param: In]
		set;
	}

	[DispId(2935)]
	bool doPreviousNow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2935)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2935)]
		[param: In]
		set;
	}

	[DispId(2936)]
	int DPI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2936)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2937)]
	void clearColors();

	[DispId(2938)]
	string lastMessage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2938)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2938)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2943)]
	bool inVistaPlus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2943)]
		get;
	}

	[DispId(2944)]
	bool isBidi
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2944)]
		get;
	}

	[DispId(2945)]
	bool isOCX
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2945)]
		get;
	}

	[DispId(2946)]
	bool hoverTransportsEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2946)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2947)]
	void initRipHelper();

	[DispId(2948)]
	bool isAudioCD
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2948)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2948)]
		[param: In]
		set;
	}

	[DispId(2949)]
	bool canRip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2949)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2949)]
		[param: In]
		set;
	}

	[DispId(2950)]
	bool isRipping
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2950)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2950)]
		[param: In]
		set;
	}

	[DispId(2951)]
	string currentDrive
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2951)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2951)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2952)]
	void startRip();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2953)]
	void stopRip();

	[DispId(2954)]
	bool showMMO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2954)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2954)]
		[param: In]
		set;
	}

	[DispId(2971)]
	bool MMOVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2971)]
		get;
	}

	[DispId(2955)]
	bool suggestionsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2955)]
		get;
	}

	[DispId(2956)]
	string suggestionsTextColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2956)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2964)]
	string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2964)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2965)]
	int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2965)]
		get;
	}

	[DispId(2966)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2966)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2957)]
	int doubleClickTime
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2957)]
		get;
	}

	[DispId(2958)]
	bool playAgain
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2958)]
		get;
	}

	[DispId(2959)]
	bool previousPlaylistAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2959)]
		get;
	}

	[DispId(2960)]
	bool nextPlaylistAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2960)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2961)]
	void nextPlaylist();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2962)]
	void previousPlaylist();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2972)]
	void playOffsetMedia([In] int iOffset);

	[DispId(2963)]
	bool basketVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2963)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2963)]
		[param: In]
		set;
	}

	[DispId(2967)]
	string mmoTextColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2967)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2968)]
	bool backgroundVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2968)]
		get;
	}

	[DispId(2969)]
	bool backgroundEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2969)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2969)]
		[param: In]
		set;
	}

	[DispId(2970)]
	int backgroundIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2970)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2970)]
		[param: In]
		set;
	}

	[DispId(2973)]
	string upNext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2973)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2974)]
	bool playbackOverlayVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2974)]
		get;
	}

	[DispId(2975)]
	bool remoted
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2975)]
		get;
	}

	[DispId(2976)]
	bool glassEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2976)]
		get;
	}

	[DispId(2977)]
	bool highContrast
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2977)]
		get;
	}

	[DispId(2978)]
	string testHighContrast
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2978)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2979)]
	void sessionPlaylistCount(out int pVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2980)]
	void setGestureStatus([In][MarshalAs(UnmanagedType.IDispatch)] object pObject, [In] int newVal);

	[DispId(2981)]
	string metadataString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2981)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2981)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2982)]
	int albumArtAlpha
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2982)]
		get;
	}

	[DispId(2983)]
	bool playerModeAlbumArtSelected
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2983)]
		get;
	}

	[DispId(2984)]
	bool inFullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2984)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2985)]
	void syncToAlbumArt([In][MarshalAs(UnmanagedType.IDispatch)] object pObject, [In] int iOffsetFromCurrentMedia, [In][MarshalAs(UnmanagedType.BStr)] string bstrFallbackImage);

	[DispId(2986)]
	int resourceIdForDpi
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2986)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("2A2E0DA3-19FA-4F82-BE18-CD7D7A3B977F")]
public interface IWMPNowDoingDispatch
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3222)]
	void hideBasket();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3211)]
	void burnNavigateToStatus();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3220)]
	void syncNavigateToStatus();

	[DispId(3223)]
	int DPI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3223)]
		get;
	}

	[DispId(3200)]
	string mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3200)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3206)]
	int burn_selectedDrive
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3206)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3206)]
		[param: In]
		set;
	}

	[DispId(3216)]
	int sync_selectedDevice
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3216)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3216)]
		[param: In]
		set;
	}

	[DispId(3208)]
	int burn_numDiscsSpanned
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3208)]
		get;
	}

	[DispId(3225)]
	object editPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3225)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(3227)]
	string basketPlaylistName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3227)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3228)]
	bool isHighContrastMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3228)]
		get;
	}

	[DispId(3229)]
	bool allowRating
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3229)]
		get;
	}

	[DispId(3201)]
	string burn_mediaType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3201)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3202)]
	string burn_contentType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3202)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3203)]
	int burn_freeSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3203)]
		get;
	}

	[DispId(3204)]
	int burn_totalSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3204)]
		get;
	}

	[DispId(3205)]
	string burn_driveName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3205)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3207)]
	int burn_numDevices
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3207)]
		get;
	}

	[DispId(3209)]
	int burn_spaceToUse
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3209)]
		get;
	}

	[DispId(3210)]
	int burn_percentComplete
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3210)]
		get;
	}

	[DispId(3212)]
	int sync_spaceToUse
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3212)]
		get;
	}

	[DispId(3213)]
	int sync_spaceUsed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3213)]
		get;
	}

	[DispId(3214)]
	int sync_totalSpace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3214)]
		get;
	}

	[DispId(3215)]
	string sync_deviceName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3215)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3217)]
	int sync_numDevices
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3217)]
		get;
	}

	[DispId(3218)]
	string sync_oemName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3218)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3219)]
	int sync_percentComplete
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3219)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3224)]
	void logData([In][MarshalAs(UnmanagedType.BStr)] string ID, [In][MarshalAs(UnmanagedType.BStr)] string data);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3226)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string formatTime([In] int value);
}
[ComImport]
[Guid("946B023E-044C-4473-8018-74954F09DC7E")]
[TypeLibType(4160)]
public interface IWMPHoverPreviewDispatch
{
	[DispId(3150)]
	string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3150)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3151)]
	string album
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3151)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3153)]
	string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3153)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3152)]
	object image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3152)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(3155)]
	bool autoClick
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3155)]
		[param: In]
		set;
	}

	[DispId(3154)]
	bool previewClick
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3154)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3156)]
	void dismiss();
}
[ComImport]
[TypeLibType(4096)]
[InterfaceType(2)]
[Guid("BB17FFF7-1692-4555-918A-6AF7BFACEDD2")]
public interface IWMPButtonCtrlEvents
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5120)]
	void onclick();
}
[ComImport]
[ClassInterface(0)]
[Guid("87291B51-0C8E-11D3-BB2A-00A0C93CA73A")]
[ComSourceInterfaces("WMPLib.IWMPButtonCtrlEvents\0\0")]
public class WMPButtonCtrlClass : IWMPButtonCtrl, WMPButtonCtrl, IWMPButtonCtrlEvents_Event
{
	[DispId(5102)]
	public virtual extern string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5102)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5102)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5103)]
	public virtual extern string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5103)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5103)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5104)]
	public virtual extern string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5104)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5104)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5105)]
	public virtual extern string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5105)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5105)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5106)]
	public virtual extern string hoverDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5106)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5106)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5107)]
	public virtual extern bool tiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5107)]
		[param: In]
		set;
	}

	[DispId(5108)]
	public virtual extern string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5108)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5108)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5109)]
	public virtual extern bool down
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5109)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5109)]
		[param: In]
		set;
	}

	[DispId(5110)]
	public virtual extern bool sticky
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5110)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5110)]
		[param: In]
		set;
	}

	[DispId(5112)]
	public virtual extern string upToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5112)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5112)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5113)]
	public virtual extern string downToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5113)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5113)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5114)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5114)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5114)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern event IWMPButtonCtrlEvents_onclickEventHandler onclick;
}
[ComVisible(false)]
[TypeLibType(16)]
[ComEventInterface(typeof(IWMPButtonCtrlEvents\u0000), typeof(IWMPButtonCtrlEvents_EventProvider\u0000))]
public interface IWMPButtonCtrlEvents_Event
{
	event IWMPButtonCtrlEvents_onclickEventHandler onclick;
}
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPButtonCtrlEvents_onclickEventHandler();
[ComImport]
[Guid("87291B50-0C8E-11D3-BB2A-00A0C93CA73A")]
[CoClass(typeof(WMPButtonCtrlClass))]
public interface WMPButtonCtrl : IWMPButtonCtrl, IWMPButtonCtrlEvents_Event
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("87291B50-0C8E-11D3-BB2A-00A0C93CA73A")]
public interface IWMPButtonCtrl
{
	[DispId(5102)]
	string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5102)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5102)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5103)]
	string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5103)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5103)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5104)]
	string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5104)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5104)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5105)]
	string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5105)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5105)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5106)]
	string hoverDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5106)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5106)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5107)]
	bool tiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5107)]
		[param: In]
		set;
	}

	[DispId(5108)]
	string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5108)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5108)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5109)]
	bool down
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5109)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5109)]
		[param: In]
		set;
	}

	[DispId(5110)]
	bool sticky
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5110)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5110)]
		[param: In]
		set;
	}

	[DispId(5112)]
	string upToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5112)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5112)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5113)]
	string downToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5113)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5113)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5114)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5114)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5114)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("FC1880CF-83B9-43A7-A066-C44CE8C82583")]
[ClassInterface(0)]
public class WMPListBoxCtrlClass : IWMPListBoxCtrl, WMPListBoxCtrl
{
	[DispId(6108)]
	public virtual extern int selectedItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6108)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6108)]
		[param: In]
		set;
	}

	[DispId(6100)]
	public virtual extern bool sorted
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6100)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6100)]
		[param: In]
		set;
	}

	[DispId(6101)]
	public virtual extern bool multiselect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6101)]
		[param: In]
		set;
	}

	[DispId(6102)]
	public virtual extern bool readOnly
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6102)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6102)]
		[param: In]
		set;
	}

	[DispId(6103)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6103)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6103)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6104)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6104)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6104)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6105)]
	public virtual extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6105)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6105)]
		[param: In]
		set;
	}

	[DispId(6106)]
	public virtual extern string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6106)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6106)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6107)]
	public virtual extern string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6107)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6107)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6109)]
	public virtual extern int itemCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6109)]
		get;
	}

	[DispId(6110)]
	public virtual extern int firstVisibleItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6110)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6110)]
		[param: In]
		set;
	}

	[DispId(6120)]
	public virtual extern bool popUp
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6120)]
		[param: In]
		set;
	}

	[DispId(6121)]
	public virtual extern int focusItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6121)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6121)]
		[param: In]
		set;
	}

	[DispId(6125)]
	public virtual extern bool border
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6125)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6125)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6111)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItem([In] int nPos);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6112)]
	public virtual extern void insertItem([In] int nPos, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6113)]
	public virtual extern void appendItem([In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6114)]
	public virtual extern void replaceItem([In] int nPos, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6115)]
	public virtual extern void deleteItem([In] int nPos);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6116)]
	public virtual extern void deleteAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6117)]
	public virtual extern int findItem([In] int nStartIndex, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6118)]
	public virtual extern int getNextSelectedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6122)]
	public virtual extern void setSelectedState([In] int nPos, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6123)]
	public virtual extern void show();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6124)]
	public virtual extern void dismiss();
}
[ComImport]
[Guid("FC1880CE-83B9-43A7-A066-C44CE8C82583")]
[CoClass(typeof(WMPListBoxCtrlClass))]
public interface WMPListBoxCtrl : IWMPListBoxCtrl
{
}
[ComImport]
[Guid("FC1880CE-83B9-43A7-A066-C44CE8C82583")]
[TypeLibType(4160)]
public interface IWMPListBoxCtrl
{
	[DispId(6108)]
	int selectedItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6108)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6108)]
		[param: In]
		set;
	}

	[DispId(6100)]
	bool sorted
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6100)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6100)]
		[param: In]
		set;
	}

	[DispId(6101)]
	bool multiselect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6101)]
		[param: In]
		set;
	}

	[DispId(6102)]
	bool readOnly
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6102)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6102)]
		[param: In]
		set;
	}

	[DispId(6103)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6103)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6103)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6104)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6104)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6104)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6105)]
	int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6105)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6105)]
		[param: In]
		set;
	}

	[DispId(6106)]
	string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6106)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6106)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6107)]
	string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6107)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6107)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6109)]
	int itemCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6109)]
		get;
	}

	[DispId(6110)]
	int firstVisibleItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6110)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6110)]
		[param: In]
		set;
	}

	[DispId(6120)]
	bool popUp
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6120)]
		[param: In]
		set;
	}

	[DispId(6121)]
	int focusItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6121)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6121)]
		[param: In]
		set;
	}

	[DispId(6125)]
	bool border
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6125)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6125)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6111)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItem([In] int nPos);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6112)]
	void insertItem([In] int nPos, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6113)]
	void appendItem([In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6114)]
	void replaceItem([In] int nPos, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6115)]
	void deleteItem([In] int nPos);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6116)]
	void deleteAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6117)]
	int findItem([In] int nStartIndex, [In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6118)]
	int getNextSelectedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6122)]
	void setSelectedState([In] int nPos, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6123)]
	void show();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6124)]
	void dismiss();
}
[ComImport]
[TypeLibType(4160)]
[Guid("D255DFB8-C22A-42CF-B8B7-F15D7BCF65D6")]
public interface IWMPListBoxItem
{
	[DispId(6119)]
	string value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6119)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("63D9D30F-AE4C-4678-8CA8-5720F4FE4419")]
[TypeLibType(4160)]
public interface IWMPPlaylistCtrlColumn
{
	[DispId(5670)]
	string columnName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5670)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5670)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5671)]
	string columnID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5671)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5671)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5672)]
	string columnResizeMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5672)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5672)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5673)]
	int columnWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5673)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5673)]
		[param: In]
		set;
	}
}
[ComImport]
[InterfaceType(2)]
[Guid("CDAC14D2-8BE4-11D3-BB48-00A0C93CA73A")]
[TypeLibType(4096)]
public interface IWMPSliderCtrlEvents
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5430)]
	void ondragbegin();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5431)]
	void ondragend();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5432)]
	void onpositionchange();
}
[ComImport]
[Guid("F2BF2C90-405F-11D3-BB39-00A0C93CA73A")]
[ClassInterface(0)]
[ComSourceInterfaces("WMPLib.IWMPSliderCtrlEvents\0\0")]
public class WMPSliderCtrlClass : IWMPSliderCtrl, WMPSliderCtrl, IWMPSliderCtrlEvents_Event
{
	[DispId(5400)]
	public virtual extern string direction
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5400)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5400)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5402)]
	public virtual extern bool slide
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5402)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5402)]
		[param: In]
		set;
	}

	[DispId(5403)]
	public virtual extern bool tiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5403)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5403)]
		[param: In]
		set;
	}

	[DispId(5404)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5404)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5404)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5405)]
	public virtual extern string foregroundEndColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5405)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5405)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5406)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5406)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5406)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5407)]
	public virtual extern string backgroundEndColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5407)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5407)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5408)]
	public virtual extern string disabledColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5408)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5408)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5409)]
	public virtual extern string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5409)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5409)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5410)]
	public virtual extern string foregroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5410)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5410)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5411)]
	public virtual extern string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5411)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5411)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5412)]
	public virtual extern string backgroundHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5412)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5412)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5413)]
	public virtual extern string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5413)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5413)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5414)]
	public virtual extern string thumbImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5414)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5414)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5415)]
	public virtual extern string thumbHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5415)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5415)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5416)]
	public virtual extern string thumbDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5416)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5416)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5417)]
	public virtual extern string thumbDisabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5417)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5417)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5418)]
	public virtual extern float min
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5418)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5418)]
		[param: In]
		set;
	}

	[DispId(5419)]
	public virtual extern float max
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5419)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5419)]
		[param: In]
		set;
	}

	[DispId(5420)]
	public virtual extern float value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5420)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5420)]
		[param: In]
		set;
	}

	[DispId(5421)]
	public virtual extern string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5421)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5421)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5422)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5422)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5422)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5423)]
	public virtual extern int borderSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5423)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5423)]
		[param: In]
		set;
	}

	[DispId(5424)]
	public virtual extern string foregroundHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5424)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5424)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5425)]
	public virtual extern float foregroundProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5425)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5425)]
		[param: In]
		set;
	}

	[DispId(5426)]
	public virtual extern bool useForegroundProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5426)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5426)]
		[param: In]
		set;
	}

	public virtual extern event IWMPSliderCtrlEvents_ondragbeginEventHandler ondragbegin;

	public virtual extern event IWMPSliderCtrlEvents_ondragendEventHandler ondragend;

	public virtual extern event IWMPSliderCtrlEvents_onpositionchangeEventHandler onpositionchange;
}
[ComVisible(false)]
[ComEventInterface(typeof(IWMPSliderCtrlEvents\u0000), typeof(IWMPSliderCtrlEvents_EventProvider\u0000))]
[TypeLibType(16)]
public interface IWMPSliderCtrlEvents_Event
{
	event IWMPSliderCtrlEvents_ondragbeginEventHandler ondragbegin;

	event IWMPSliderCtrlEvents_ondragendEventHandler ondragend;

	event IWMPSliderCtrlEvents_onpositionchangeEventHandler onpositionchange;
}
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPSliderCtrlEvents_ondragbeginEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPSliderCtrlEvents_ondragendEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPSliderCtrlEvents_onpositionchangeEventHandler();
[ComImport]
[Guid("F2BF2C8F-405F-11D3-BB39-00A0C93CA73A")]
[CoClass(typeof(WMPSliderCtrlClass))]
public interface WMPSliderCtrl : IWMPSliderCtrl, IWMPSliderCtrlEvents_Event
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("F2BF2C8F-405F-11D3-BB39-00A0C93CA73A")]
public interface IWMPSliderCtrl
{
	[DispId(5400)]
	string direction
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5400)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5400)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5402)]
	bool slide
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5402)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5402)]
		[param: In]
		set;
	}

	[DispId(5403)]
	bool tiled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5403)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5403)]
		[param: In]
		set;
	}

	[DispId(5404)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5404)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5404)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5405)]
	string foregroundEndColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5405)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5405)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5406)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5406)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5406)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5407)]
	string backgroundEndColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5407)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5407)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5408)]
	string disabledColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5408)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5408)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5409)]
	string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5409)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5409)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5410)]
	string foregroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5410)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5410)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5411)]
	string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5411)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5411)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5412)]
	string backgroundHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5412)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5412)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5413)]
	string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5413)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5413)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5414)]
	string thumbImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5414)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5414)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5415)]
	string thumbHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5415)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5415)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5416)]
	string thumbDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5416)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5416)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5417)]
	string thumbDisabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5417)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5417)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5418)]
	float min
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5418)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5418)]
		[param: In]
		set;
	}

	[DispId(5419)]
	float max
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5419)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5419)]
		[param: In]
		set;
	}

	[DispId(5420)]
	float value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5420)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5420)]
		[param: In]
		set;
	}

	[DispId(5421)]
	string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5421)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5421)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5422)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5422)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5422)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5423)]
	int borderSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5423)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5423)]
		[param: In]
		set;
	}

	[DispId(5424)]
	string foregroundHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5424)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5424)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5425)]
	float foregroundProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5425)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5425)]
		[param: In]
		set;
	}

	[DispId(5426)]
	bool useForegroundProgress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5426)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5426)]
		[param: In]
		set;
	}
}
[ComImport]
[Guid("A85C0477-714C-4A06-B9F6-7C8CA38B45DC")]
[InterfaceType(2)]
[TypeLibType(4096)]
public interface IWMPVideoCtrlEvents
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5720)]
	void onvideostart();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5721)]
	void onvideoend();
}
[ComImport]
[ClassInterface(0)]
[Guid("61CECF11-FC3A-11D2-A1CD-005004602752")]
[ComSourceInterfaces("WMPLib.IWMPVideoCtrlEvents\0\0")]
public class WMPVideoCtrlClass : IWMPVideoCtrl, WMPVideoCtrl, IWMPVideoCtrlEvents_Event
{
	[DispId(5700)]
	public virtual extern bool windowless
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5700)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5700)]
		[param: In]
		set;
	}

	[DispId(5701)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5701)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5701)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5702)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5702)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5702)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5704)]
	public virtual extern bool maintainAspectRatio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5704)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5704)]
		[param: In]
		set;
	}

	[DispId(5706)]
	public virtual extern string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5706)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5706)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5707)]
	public virtual extern bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5707)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5707)]
		[param: In]
		set;
	}

	[DispId(5703)]
	public virtual extern bool shrinkToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5703)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5703)]
		[param: In]
		set;
	}

	[DispId(5708)]
	public virtual extern bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5708)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5708)]
		[param: In]
		set;
	}

	[DispId(5709)]
	public virtual extern int zoom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5709)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5709)]
		[param: In]
		set;
	}

	public virtual extern event IWMPVideoCtrlEvents_onvideostartEventHandler onvideostart;

	public virtual extern event IWMPVideoCtrlEvents_onvideoendEventHandler onvideoend;
}
[TypeLibType(16)]
[ComVisible(false)]
[ComEventInterface(typeof(IWMPVideoCtrlEvents\u0000), typeof(IWMPVideoCtrlEvents_EventProvider\u0000))]
public interface IWMPVideoCtrlEvents_Event
{
	event IWMPVideoCtrlEvents_onvideostartEventHandler onvideostart;

	event IWMPVideoCtrlEvents_onvideoendEventHandler onvideoend;
}
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPVideoCtrlEvents_onvideostartEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPVideoCtrlEvents_onvideoendEventHandler();
[ComImport]
[Guid("61CECF10-FC3A-11D2-A1CD-005004602752")]
[CoClass(typeof(WMPVideoCtrlClass))]
public interface WMPVideoCtrl : IWMPVideoCtrl, IWMPVideoCtrlEvents_Event
{
}
[ComImport]
[Guid("61CECF10-FC3A-11D2-A1CD-005004602752")]
[TypeLibType(4160)]
public interface IWMPVideoCtrl
{
	[DispId(5700)]
	bool windowless
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5700)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5700)]
		[param: In]
		set;
	}

	[DispId(5701)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5701)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5701)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5702)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5702)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5702)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5704)]
	bool maintainAspectRatio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5704)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5704)]
		[param: In]
		set;
	}

	[DispId(5706)]
	string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5706)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5706)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5707)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5707)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5707)]
		[param: In]
		set;
	}

	[DispId(5703)]
	bool shrinkToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5703)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5703)]
		[param: In]
		set;
	}

	[DispId(5708)]
	bool stretchToFit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5708)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5708)]
		[param: In]
		set;
	}

	[DispId(5709)]
	int zoom
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5709)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5709)]
		[param: In]
		set;
	}
}
[ComImport]
[ClassInterface(0)]
[Guid("47DEA830-D619-4154-B8D8-6B74845D6A2D")]
public class WMPEffectsClass : IWMPEffectsCtrl, WMPEffects
{
	[DispId(5500)]
	public virtual extern bool windowed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5500)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5500)]
		[param: In]
		set;
	}

	[DispId(5501)]
	public virtual extern bool allowAll
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5501)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5501)]
		[param: In]
		set;
	}

	[DispId(5507)]
	public virtual extern string currentEffectType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5507)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5507)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5506)]
	public virtual extern string currentEffectTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5506)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5505)]
	public virtual extern object currentEffect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5505)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5505)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(5513)]
	public virtual extern int currentPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5513)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5513)]
		[param: In]
		set;
	}

	[DispId(5514)]
	public virtual extern string currentPresetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5514)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5515)]
	public virtual extern int currentEffectPresetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5515)]
		get;
	}

	[DispId(5516)]
	public virtual extern bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5516)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5516)]
		[param: In]
		set;
	}

	[DispId(5517)]
	public virtual extern bool effectCanGoFullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5517)]
		get;
	}

	[DispId(5518)]
	public virtual extern bool effectHasPropertyPage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5518)]
		get;
	}

	[DispId(5520)]
	public virtual extern int effectCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5520)]
		get;
	}

	[DispId(5521)]
	public virtual extern string effectTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5521)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5522)]
	public virtual extern string effectType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5522)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5502)]
	public virtual extern void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5503)]
	public virtual extern void previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5504)]
	public virtual extern void settings();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5509)]
	public virtual extern void nextEffect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5510)]
	public virtual extern void previousEffect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5511)]
	public virtual extern void nextPreset();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5512)]
	public virtual extern void previousPreset();
}
[ComImport]
[CoClass(typeof(WMPEffectsClass))]
[Guid("A9EFAB80-0A60-4C3F-BBD1-4558DD2A9769")]
public interface WMPEffects : IWMPEffectsCtrl
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("A9EFAB80-0A60-4C3F-BBD1-4558DD2A9769")]
public interface IWMPEffectsCtrl
{
	[DispId(5500)]
	bool windowed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5500)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5500)]
		[param: In]
		set;
	}

	[DispId(5501)]
	bool allowAll
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5501)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5501)]
		[param: In]
		set;
	}

	[DispId(5507)]
	string currentEffectType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5507)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5507)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5506)]
	string currentEffectTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5506)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5502)]
	void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5503)]
	void previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5504)]
	void settings();

	[DispId(5505)]
	object currentEffect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5505)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5505)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5509)]
	void nextEffect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5510)]
	void previousEffect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5511)]
	void nextPreset();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5512)]
	void previousPreset();

	[DispId(5513)]
	int currentPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5513)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5513)]
		[param: In]
		set;
	}

	[DispId(5514)]
	string currentPresetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5514)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5515)]
	int currentEffectPresetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5515)]
		get;
	}

	[DispId(5516)]
	bool fullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5516)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5516)]
		[param: In]
		set;
	}

	[DispId(5517)]
	bool effectCanGoFullScreen
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5517)]
		get;
	}

	[DispId(5518)]
	bool effectHasPropertyPage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5518)]
		get;
	}

	[DispId(5520)]
	int effectCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5520)]
		get;
	}

	[DispId(5521)]
	string effectTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5521)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5522)]
	string effectType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5522)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[Guid("93EB32F5-87B1-45AD-ACC6-0F2483DB83BB")]
[ClassInterface(0)]
public class WMPEqualizerSettingsCtrlClass : IWMPEqualizerSettingsCtrl, WMPEqualizerSettingsCtrl
{
	[DispId(5800)]
	public virtual extern bool bypass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5800)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5800)]
		[param: In]
		set;
	}

	[DispId(5804)]
	public virtual extern float gainLevel1
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5804)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5804)]
		[param: In]
		set;
	}

	[DispId(5805)]
	public virtual extern float gainLevel2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5805)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5805)]
		[param: In]
		set;
	}

	[DispId(5806)]
	public virtual extern float gainLevel3
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5806)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5806)]
		[param: In]
		set;
	}

	[DispId(5807)]
	public virtual extern float gainLevel4
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5807)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5807)]
		[param: In]
		set;
	}

	[DispId(5808)]
	public virtual extern float gainLevel5
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5808)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5808)]
		[param: In]
		set;
	}

	[DispId(5809)]
	public virtual extern float gainLevel6
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5809)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5809)]
		[param: In]
		set;
	}

	[DispId(5810)]
	public virtual extern float gainLevel7
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5810)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5810)]
		[param: In]
		set;
	}

	[DispId(5811)]
	public virtual extern float gainLevel8
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5811)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5811)]
		[param: In]
		set;
	}

	[DispId(5812)]
	public virtual extern float gainLevel9
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5812)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5812)]
		[param: In]
		set;
	}

	[DispId(5813)]
	public virtual extern float gainLevel10
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5813)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5813)]
		[param: In]
		set;
	}

	[DispId(5815)]
	public virtual extern float gainLevels
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5815)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5815)]
		[param: In]
		set;
	}

	[DispId(5801)]
	public virtual extern int bands
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5801)]
		get;
	}

	[DispId(5818)]
	public virtual extern int currentPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5818)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5818)]
		[param: In]
		set;
	}

	[DispId(5819)]
	public virtual extern string currentPresetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5819)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5820)]
	public virtual extern int presetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5820)]
		get;
	}

	[DispId(5821)]
	public virtual extern bool enhancedAudio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5821)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5821)]
		[param: In]
		set;
	}

	[DispId(5822)]
	public virtual extern int speakerSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5822)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5822)]
		[param: In]
		set;
	}

	[DispId(5823)]
	public virtual extern string currentSpeakerName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5823)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5824)]
	public virtual extern int truBassLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5824)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5824)]
		[param: In]
		set;
	}

	[DispId(5825)]
	public virtual extern int wowLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5825)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5825)]
		[param: In]
		set;
	}

	[DispId(5827)]
	public virtual extern float splineTension
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5827)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5827)]
		[param: In]
		set;
	}

	[DispId(5826)]
	public virtual extern bool enableSplineTension
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5826)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5826)]
		[param: In]
		set;
	}

	[DispId(5828)]
	public virtual extern string presetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5828)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5829)]
	public virtual extern bool normalization
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5829)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5829)]
		[param: In]
		set;
	}

	[DispId(5830)]
	public virtual extern float normalizationAverage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5830)]
		get;
	}

	[DispId(5831)]
	public virtual extern float normalizationPeak
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5831)]
		get;
	}

	[DispId(5832)]
	public virtual extern bool crossFade
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5832)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5832)]
		[param: In]
		set;
	}

	[DispId(5833)]
	public virtual extern int crossFadeWindow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5833)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5833)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5814)]
	public virtual extern void reset();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5816)]
	public virtual extern void nextPreset();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5817)]
	public virtual extern void previousPreset();
}
[ComImport]
[Guid("2BD3716F-A914-49FB-8655-996D5F495498")]
[CoClass(typeof(WMPEqualizerSettingsCtrlClass))]
public interface WMPEqualizerSettingsCtrl : IWMPEqualizerSettingsCtrl
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("2BD3716F-A914-49FB-8655-996D5F495498")]
public interface IWMPEqualizerSettingsCtrl
{
	[DispId(5800)]
	bool bypass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5800)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5800)]
		[param: In]
		set;
	}

	[DispId(5804)]
	float gainLevel1
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5804)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5804)]
		[param: In]
		set;
	}

	[DispId(5805)]
	float gainLevel2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5805)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5805)]
		[param: In]
		set;
	}

	[DispId(5806)]
	float gainLevel3
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5806)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5806)]
		[param: In]
		set;
	}

	[DispId(5807)]
	float gainLevel4
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5807)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5807)]
		[param: In]
		set;
	}

	[DispId(5808)]
	float gainLevel5
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5808)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5808)]
		[param: In]
		set;
	}

	[DispId(5809)]
	float gainLevel6
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5809)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5809)]
		[param: In]
		set;
	}

	[DispId(5810)]
	float gainLevel7
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5810)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5810)]
		[param: In]
		set;
	}

	[DispId(5811)]
	float gainLevel8
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5811)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5811)]
		[param: In]
		set;
	}

	[DispId(5812)]
	float gainLevel9
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5812)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5812)]
		[param: In]
		set;
	}

	[DispId(5813)]
	float gainLevel10
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5813)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5813)]
		[param: In]
		set;
	}

	[DispId(5815)]
	float gainLevels
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5815)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5815)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5814)]
	void reset();

	[DispId(5801)]
	int bands
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5801)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5816)]
	void nextPreset();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5817)]
	void previousPreset();

	[DispId(5818)]
	int currentPreset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5818)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5818)]
		[param: In]
		set;
	}

	[DispId(5819)]
	string currentPresetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5819)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5820)]
	int presetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5820)]
		get;
	}

	[DispId(5821)]
	bool enhancedAudio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5821)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5821)]
		[param: In]
		set;
	}

	[DispId(5822)]
	int speakerSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5822)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5822)]
		[param: In]
		set;
	}

	[DispId(5823)]
	string currentSpeakerName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5823)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5824)]
	int truBassLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5824)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5824)]
		[param: In]
		set;
	}

	[DispId(5825)]
	int wowLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5825)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5825)]
		[param: In]
		set;
	}

	[DispId(5827)]
	float splineTension
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5827)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5827)]
		[param: In]
		set;
	}

	[DispId(5826)]
	bool enableSplineTension
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5826)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5826)]
		[param: In]
		set;
	}

	[DispId(5828)]
	string presetTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5828)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5829)]
	bool normalization
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5829)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5829)]
		[param: In]
		set;
	}

	[DispId(5830)]
	float normalizationAverage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5830)]
		get;
	}

	[DispId(5831)]
	float normalizationPeak
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5831)]
		get;
	}

	[DispId(5832)]
	bool crossFade
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5832)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5832)]
		[param: In]
		set;
	}

	[DispId(5833)]
	int crossFadeWindow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5833)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5833)]
		[param: In]
		set;
	}
}
[ComImport]
[ClassInterface(0)]
[Guid("AE7BFAFE-DCC8-4A73-92C8-CC300CA88859")]
public class WMPVideoSettingsCtrlClass : IWMPVideoSettingsCtrl, WMPVideoSettingsCtrl
{
	[DispId(5900)]
	public virtual extern int brightness
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5900)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5900)]
		[param: In]
		set;
	}

	[DispId(5901)]
	public virtual extern int contrast
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5901)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5901)]
		[param: In]
		set;
	}

	[DispId(5902)]
	public virtual extern int hue
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5902)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5902)]
		[param: In]
		set;
	}

	[DispId(5903)]
	public virtual extern int saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5903)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5903)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5904)]
	public virtual extern void reset();
}
[ComImport]
[Guid("07EC23DA-EF73-4BDE-A40F-F269E0B7AFD6")]
[CoClass(typeof(WMPVideoSettingsCtrlClass))]
public interface WMPVideoSettingsCtrl : IWMPVideoSettingsCtrl
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("07EC23DA-EF73-4BDE-A40F-F269E0B7AFD6")]
public interface IWMPVideoSettingsCtrl
{
	[DispId(5900)]
	int brightness
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5900)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5900)]
		[param: In]
		set;
	}

	[DispId(5901)]
	int contrast
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5901)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5901)]
		[param: In]
		set;
	}

	[DispId(5902)]
	int hue
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5902)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5902)]
		[param: In]
		set;
	}

	[DispId(5903)]
	int saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5903)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5903)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5904)]
	void reset();
}
[ComImport]
[ClassInterface(0)]
[Guid("D9DE732A-AEE9-4503-9D11-5605589977A8")]
public class WMPLibraryTreeCtrlClass : IWMPLibraryTreeCtrl, WMPLibraryTreeCtrl
{
	[DispId(6401)]
	public virtual extern bool dropDownVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6401)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6401)]
		[param: In]
		set;
	}

	[DispId(6402)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6402)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6402)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6403)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6403)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6403)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6404)]
	public virtual extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6404)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6404)]
		[param: In]
		set;
	}

	[DispId(6405)]
	public virtual extern string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6405)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6405)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6406)]
	public virtual extern string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6406)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6406)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6407)]
	public virtual extern string filter
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6407)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6407)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6408)]
	public virtual extern string expandState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6408)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6408)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6409)]
	public virtual extern IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6409)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6409)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(6410)]
	public virtual extern IWMPPlaylist selectedPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6410)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6411)]
	public virtual extern IWMPMedia selectedMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6411)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[CoClass(typeof(WMPLibraryTreeCtrlClass))]
[Guid("B738FCAE-F089-45DF-AED6-034B9E7DB632")]
public interface WMPLibraryTreeCtrl : IWMPLibraryTreeCtrl
{
}
[ComImport]
[Guid("B738FCAE-F089-45DF-AED6-034B9E7DB632")]
[TypeLibType(4160)]
public interface IWMPLibraryTreeCtrl
{
	[DispId(6401)]
	bool dropDownVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6401)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6401)]
		[param: In]
		set;
	}

	[DispId(6402)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6402)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6402)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6403)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6403)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6403)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6404)]
	int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6404)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6404)]
		[param: In]
		set;
	}

	[DispId(6405)]
	string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6405)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6405)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6406)]
	string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6406)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6406)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6407)]
	string filter
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6407)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6407)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6408)]
	string expandState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6408)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6408)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6409)]
	IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6409)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6409)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(6410)]
	IWMPPlaylist selectedPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6410)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6411)]
	IWMPMedia selectedMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6411)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[DefaultMember("value")]
[Guid("6342FCED-25EA-4033-BDDB-D049A14382D3")]
[ClassInterface(0)]
public class WMPEditCtrlClass : IWMPEditCtrl, WMPEditCtrl
{
	[DispId(0)]
	public virtual extern string value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6000)]
	public virtual extern bool border
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6000)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6000)]
		[param: In]
		set;
	}

	[DispId(6001)]
	public virtual extern string justification
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6001)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6002)]
	public virtual extern string editStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6002)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6003)]
	public virtual extern bool wordWrap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6003)]
		[param: In]
		set;
	}

	[DispId(6004)]
	public virtual extern bool readOnly
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6004)]
		[param: In]
		set;
	}

	[DispId(6005)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6005)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6006)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6006)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6006)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6007)]
	public virtual extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6007)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6007)]
		[param: In]
		set;
	}

	[DispId(6008)]
	public virtual extern string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6009)]
	public virtual extern string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6009)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6009)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6010)]
	public virtual extern int textLimit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6010)]
		[param: In]
		set;
	}

	[DispId(6011)]
	public virtual extern int lineCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6011)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6012)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getLine([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6013)]
	public virtual extern int getSelectionStart();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6014)]
	public virtual extern int getSelectionEnd();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6015)]
	public virtual extern void setSelection([In] int nStart, [In] int nEnd);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6016)]
	public virtual extern void replaceSelection([In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6017)]
	public virtual extern int getLineIndex([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6018)]
	public virtual extern int getLineFromChar([In] int nPosition);
}
[ComImport]
[CoClass(typeof(WMPEditCtrlClass))]
[Guid("70E1217C-C617-4CFD-BD8A-69CA2043E70B")]
public interface WMPEditCtrl : IWMPEditCtrl
{
}
[ComImport]
[Guid("70E1217C-C617-4CFD-BD8A-69CA2043E70B")]
[TypeLibType(4160)]
[DefaultMember("value")]
public interface IWMPEditCtrl
{
	[DispId(0)]
	string value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6000)]
	bool border
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6000)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6000)]
		[param: In]
		set;
	}

	[DispId(6001)]
	string justification
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6001)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6002)]
	string editStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6002)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6003)]
	bool wordWrap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6003)]
		[param: In]
		set;
	}

	[DispId(6004)]
	bool readOnly
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6004)]
		[param: In]
		set;
	}

	[DispId(6005)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6005)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6006)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6006)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6006)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6007)]
	int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6007)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6007)]
		[param: In]
		set;
	}

	[DispId(6008)]
	string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6009)]
	string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6009)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6009)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6010)]
	int textLimit
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6010)]
		[param: In]
		set;
	}

	[DispId(6011)]
	int lineCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6011)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6012)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getLine([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6013)]
	int getSelectionStart();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6014)]
	int getSelectionEnd();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6015)]
	void setSelection([In] int nStart, [In] int nEnd);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6016)]
	void replaceSelection([In][MarshalAs(UnmanagedType.BStr)] string newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6017)]
	int getLineIndex([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6018)]
	int getLineFromChar([In] int nPosition);
}
[ComImport]
[ClassInterface(0)]
[Guid("A8A55FAC-82EA-4BD7-BD7B-11586A4D99E4")]
public class WMPSkinListClass : IWMPSkinList, WMPSkinList
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6050)]
	public virtual extern void updateBasketColumns();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6051)]
	public virtual extern void highContrastChange();
}
[ComImport]
[CoClass(typeof(WMPSkinListClass))]
[Guid("8CEA03A2-D0C5-4E97-9C38-A676A639A51D")]
public interface WMPSkinList : IWMPSkinList
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("8CEA03A2-D0C5-4E97-9C38-A676A639A51D")]
public interface IWMPSkinList
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6050)]
	void updateBasketColumns();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6051)]
	void highContrastChange();
}
[ComImport]
[Guid("5D0AD945-289E-45C5-A9C6-F301F0152108")]
[TypeLibType(4160)]
public interface IWMPPluginUIHost
{
	[DispId(6201)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6201)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6201)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(6202)]
	string objectID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6202)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6202)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6203)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object getProperty([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6204)]
	void setProperty([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.Struct)] object newVal);
}
[ComImport]
[Guid("BAB3768B-8883-4AEC-9F9B-E14C947913EF")]
[ClassInterface(0)]
public class WMPMenuCtrlClass : IWMPMenuCtrl, WMPMenuCtrl
{
	[DispId(6307)]
	public virtual extern int showFlags
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6307)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6307)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6301)]
	public virtual extern void deleteAllItems();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6302)]
	public virtual extern void appendItem([In] int nID, [In][MarshalAs(UnmanagedType.BStr)] string bstrItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6303)]
	public virtual extern void appendSeparator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6304)]
	public virtual extern void enableItem([In] int nID, [In] bool newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6305)]
	public virtual extern void checkItem([In] int nID, [In] bool newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6306)]
	public virtual extern void checkRadioItem([In] int nID, [In] bool newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6308)]
	public virtual extern int show();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6309)]
	public virtual extern void showEx([In] int nID);
}
[ComImport]
[Guid("158A7ADC-33DA-4039-A553-BDDBBE389F5C")]
[CoClass(typeof(WMPMenuCtrlClass))]
public interface WMPMenuCtrl : IWMPMenuCtrl
{
}
[ComImport]
[Guid("158A7ADC-33DA-4039-A553-BDDBBE389F5C")]
[TypeLibType(4160)]
public interface IWMPMenuCtrl
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6301)]
	void deleteAllItems();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6302)]
	void appendItem([In] int nID, [In][MarshalAs(UnmanagedType.BStr)] string bstrItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6303)]
	void appendSeparator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6304)]
	void enableItem([In] int nID, [In] bool newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6305)]
	void checkItem([In] int nID, [In] bool newVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6306)]
	void checkRadioItem([In] int nID, [In] bool newVal);

	[DispId(6307)]
	int showFlags
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6307)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6307)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6308)]
	int show();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6309)]
	void showEx([In] int nID);
}
[ComImport]
[ClassInterface(0)]
[Guid("6B28F900-8D64-4B80-9963-CC52DDD1FBB4")]
public class WMPAutoMenuCtrlClass : IWMPAutoMenuCtrl, WMPAutoMenuCtrl
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6501)]
	public virtual extern void show([In][MarshalAs(UnmanagedType.BStr)] string newVal);
}
[ComImport]
[CoClass(typeof(WMPAutoMenuCtrlClass))]
[Guid("1AD13E0B-4F3A-41DF-9BE2-F9E6FE0A7875")]
public interface WMPAutoMenuCtrl : IWMPAutoMenuCtrl
{
}
[ComImport]
[Guid("1AD13E0B-4F3A-41DF-9BE2-F9E6FE0A7875")]
[TypeLibType(4160)]
public interface IWMPAutoMenuCtrl
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6501)]
	void show([In][MarshalAs(UnmanagedType.BStr)] string newVal);
}
[ComImport]
[ClassInterface(0)]
[Guid("AE3B6831-25A9-11D3-BD41-00C04F6EA5AE")]
public class WMPRegionalButtonCtrlClass : IWMPRegionalButtonCtrl, WMPRegionalButtonCtrl
{
	[DispId(5300)]
	public virtual extern string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5300)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5300)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5301)]
	public virtual extern string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5301)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5301)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5302)]
	public virtual extern string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5302)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5302)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5303)]
	public virtual extern string hoverDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5303)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5303)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5317)]
	public virtual extern string hoverHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5317)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5317)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5304)]
	public virtual extern string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5304)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5304)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5305)]
	public virtual extern string mappingImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5305)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5305)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5306)]
	public virtual extern string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5306)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5306)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5308)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5308)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5308)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5309)]
	public virtual extern bool showBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5309)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5309)]
		[param: In]
		set;
	}

	[DispId(5310)]
	public virtual extern bool radio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5310)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5310)]
		[param: In]
		set;
	}

	[DispId(5311)]
	public virtual extern int buttonCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5311)]
		get;
	}

	[DispId(5315)]
	public virtual extern float hueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5315)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5315)]
		[param: In]
		set;
	}

	[DispId(5316)]
	public virtual extern float saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5316)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5316)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5312)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	public virtual extern object createButton();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5313)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	public virtual extern object getButton([In] int nButton);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5314)]
	public virtual extern void Click([In] int nButton);
}
[ComImport]
[Guid("58D507B1-2354-11D3-BD41-00C04F6EA5AE")]
[CoClass(typeof(WMPRegionalButtonCtrlClass))]
public interface WMPRegionalButtonCtrl : IWMPRegionalButtonCtrl
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("58D507B1-2354-11D3-BD41-00C04F6EA5AE")]
public interface IWMPRegionalButtonCtrl
{
	[DispId(5300)]
	string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5300)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5300)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5301)]
	string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5301)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5301)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5302)]
	string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5302)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5302)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5303)]
	string hoverDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5303)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5303)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5317)]
	string hoverHoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5317)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5317)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5304)]
	string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5304)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5304)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5305)]
	string mappingImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5305)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5305)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5306)]
	string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5306)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5306)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5308)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5308)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5308)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5309)]
	bool showBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5309)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5309)]
		[param: In]
		set;
	}

	[DispId(5310)]
	bool radio
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5310)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5310)]
		[param: In]
		set;
	}

	[DispId(5311)]
	int buttonCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5311)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5312)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object createButton();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5313)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object getButton([In] int nButton);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5314)]
	void Click([In] int nButton);

	[DispId(5315)]
	float hueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5315)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5315)]
		[param: In]
		set;
	}

	[DispId(5316)]
	float saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5316)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5316)]
		[param: In]
		set;
	}
}
[ComImport]
[InterfaceType(2)]
[Guid("50FC8D31-67AC-11D3-BD4C-00C04F6EA5AE")]
[TypeLibType(4096)]
public interface IWMPRegionalButtonEvents
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5360)]
	void onblur();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5361)]
	void onfocus();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5362)]
	void onclick();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5363)]
	void ondblclick();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5364)]
	void onmousedown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5365)]
	void onmouseup();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5366)]
	void onmousemove();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5367)]
	void onmouseover();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5368)]
	void onmouseout();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5369)]
	void onkeypress();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5370)]
	void onkeydown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5371)]
	void onkeyup();
}
[ComImport]
[ComSourceInterfaces("WMPLib.IWMPRegionalButtonEvents\0\0")]
[ClassInterface(0)]
[Guid("09AEFF11-69EF-11D3-BD4D-00C04F6EA5AE")]
public class WMPRegionalButtonClass : IWMPRegionalButton, WMPRegionalButton, IWMPRegionalButtonEvents_Event
{
	[DispId(5330)]
	public virtual extern string upToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5330)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5330)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5331)]
	public virtual extern string downToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5331)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5331)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5332)]
	public virtual extern string mappingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5332)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5332)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5333)]
	public virtual extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5333)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5333)]
		[param: In]
		set;
	}

	[DispId(5339)]
	public virtual extern bool sticky
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5339)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5339)]
		[param: In]
		set;
	}

	[DispId(5340)]
	public virtual extern bool down
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5340)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5340)]
		[param: In]
		set;
	}

	[DispId(5341)]
	public virtual extern int index
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5341)]
		get;
	}

	[DispId(5342)]
	public virtual extern bool tabStop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5342)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5342)]
		[param: In]
		set;
	}

	[DispId(5343)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5343)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5343)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5345)]
	public virtual extern string accName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5345)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5345)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5346)]
	public virtual extern string accDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5346)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5346)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5347)]
	public virtual extern string accKeyboardShortcut
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5347)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5347)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern event IWMPRegionalButtonEvents_onblurEventHandler onblur;

	public virtual extern event IWMPRegionalButtonEvents_onfocusEventHandler onfocus;

	public virtual extern event IWMPRegionalButtonEvents_onclickEventHandler onclick;

	public virtual extern event IWMPRegionalButtonEvents_ondblclickEventHandler ondblclick;

	public virtual extern event IWMPRegionalButtonEvents_onmousedownEventHandler onmousedown;

	public virtual extern event IWMPRegionalButtonEvents_onmouseupEventHandler onmouseup;

	public virtual extern event IWMPRegionalButtonEvents_onmousemoveEventHandler onmousemove;

	public virtual extern event IWMPRegionalButtonEvents_onmouseoverEventHandler onmouseover;

	public virtual extern event IWMPRegionalButtonEvents_onmouseoutEventHandler onmouseout;

	public virtual extern event IWMPRegionalButtonEvents_onkeypressEventHandler onkeypress;

	public virtual extern event IWMPRegionalButtonEvents_onkeydownEventHandler onkeydown;

	public virtual extern event IWMPRegionalButtonEvents_onkeyupEventHandler onkeyup;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5344)]
	public virtual extern void Click();
}
[ComVisible(false)]
[ComEventInterface(typeof(IWMPRegionalButtonEvents\u0000), typeof(IWMPRegionalButtonEvents_EventProvider\u0000))]
[TypeLibType(16)]
public interface IWMPRegionalButtonEvents_Event
{
	event IWMPRegionalButtonEvents_onblurEventHandler onblur;

	event IWMPRegionalButtonEvents_onfocusEventHandler onfocus;

	event IWMPRegionalButtonEvents_onclickEventHandler onclick;

	event IWMPRegionalButtonEvents_ondblclickEventHandler ondblclick;

	event IWMPRegionalButtonEvents_onmousedownEventHandler onmousedown;

	event IWMPRegionalButtonEvents_onmouseupEventHandler onmouseup;

	event IWMPRegionalButtonEvents_onmousemoveEventHandler onmousemove;

	event IWMPRegionalButtonEvents_onmouseoverEventHandler onmouseover;

	event IWMPRegionalButtonEvents_onmouseoutEventHandler onmouseout;

	event IWMPRegionalButtonEvents_onkeypressEventHandler onkeypress;

	event IWMPRegionalButtonEvents_onkeydownEventHandler onkeydown;

	event IWMPRegionalButtonEvents_onkeyupEventHandler onkeyup;
}
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onblurEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onfocusEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onclickEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_ondblclickEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onmousedownEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPRegionalButtonEvents_onmouseupEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onmousemoveEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPRegionalButtonEvents_onmouseoverEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPRegionalButtonEvents_onmouseoutEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPRegionalButtonEvents_onkeypressEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPRegionalButtonEvents_onkeydownEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPRegionalButtonEvents_onkeyupEventHandler();
[ComImport]
[Guid("58D507B2-2354-11D3-BD41-00C04F6EA5AE")]
[CoClass(typeof(WMPRegionalButtonClass))]
public interface WMPRegionalButton : IWMPRegionalButton, IWMPRegionalButtonEvents_Event
{
}
[ComImport]
[Guid("58D507B2-2354-11D3-BD41-00C04F6EA5AE")]
[TypeLibType(4160)]
public interface IWMPRegionalButton
{
	[DispId(5330)]
	string upToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5330)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5330)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5331)]
	string downToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5331)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5331)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5332)]
	string mappingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5332)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5332)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5333)]
	bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5333)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5333)]
		[param: In]
		set;
	}

	[DispId(5339)]
	bool sticky
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5339)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5339)]
		[param: In]
		set;
	}

	[DispId(5340)]
	bool down
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5340)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5340)]
		[param: In]
		set;
	}

	[DispId(5341)]
	int index
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5341)]
		get;
	}

	[DispId(5342)]
	bool tabStop
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5342)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5342)]
		[param: In]
		set;
	}

	[DispId(5343)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5343)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5343)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5344)]
	void Click();

	[DispId(5345)]
	string accName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5345)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5345)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5346)]
	string accDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5346)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5346)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5347)]
	string accKeyboardShortcut
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5347)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5347)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[InterfaceType(2)]
[Guid("95F45AA4-ED0A-11D2-BA67-0000F80855E6")]
[TypeLibType(4096)]
public interface IWMPCustomSliderCtrlEvents
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5020)]
	void ondragbegin();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5021)]
	void ondragend();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5022)]
	void onpositionchange();
}
[ComImport]
[ComSourceInterfaces("WMPLib.IWMPCustomSliderCtrlEvents\0\0")]
[ClassInterface(0)]
[Guid("95F45AA3-ED0A-11D2-BA67-0000F80855E6")]
public class WMPCustomSliderCtrlClass : IWMPCustomSlider, WMPCustomSliderCtrl, IWMPCustomSliderCtrlEvents_Event
{
	[DispId(5009)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5009)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5009)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5005)]
	public virtual extern float min
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5005)]
		[param: In]
		set;
	}

	[DispId(5006)]
	public virtual extern float max
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5006)]
		[param: In]
		set;
	}

	[DispId(5010)]
	public virtual extern float value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5010)]
		[param: In]
		set;
	}

	[DispId(5011)]
	public virtual extern string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5011)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5011)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5002)]
	public virtual extern string positionImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5002)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5001)]
	public virtual extern string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5001)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5003)]
	public virtual extern string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5003)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5003)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5004)]
	public virtual extern string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5004)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5004)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5012)]
	public virtual extern string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5012)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5008)]
	public virtual extern string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern event IWMPCustomSliderCtrlEvents_ondragbeginEventHandler ondragbegin;

	public virtual extern event IWMPCustomSliderCtrlEvents_ondragendEventHandler ondragend;

	public virtual extern event IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler onpositionchange;
}
[TypeLibType(16)]
[ComVisible(false)]
[ComEventInterface(typeof(IWMPCustomSliderCtrlEvents\u0000), typeof(IWMPCustomSliderCtrlEvents_EventProvider\u0000))]
public interface IWMPCustomSliderCtrlEvents_Event
{
	event IWMPCustomSliderCtrlEvents_ondragbeginEventHandler ondragbegin;

	event IWMPCustomSliderCtrlEvents_ondragendEventHandler ondragend;

	event IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler onpositionchange;
}
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPCustomSliderCtrlEvents_ondragbeginEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void IWMPCustomSliderCtrlEvents_ondragendEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler();
[ComImport]
[CoClass(typeof(WMPCustomSliderCtrlClass))]
[Guid("95F45AA2-ED0A-11D2-BA67-0000F80855E6")]
public interface WMPCustomSliderCtrl : IWMPCustomSlider, IWMPCustomSliderCtrlEvents_Event
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("95F45AA2-ED0A-11D2-BA67-0000F80855E6")]
public interface IWMPCustomSlider
{
	[DispId(5009)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5009)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5009)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5005)]
	float min
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5005)]
		[param: In]
		set;
	}

	[DispId(5006)]
	float max
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5006)]
		[param: In]
		set;
	}

	[DispId(5010)]
	float value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5010)]
		[param: In]
		set;
	}

	[DispId(5011)]
	string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5011)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5011)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5002)]
	string positionImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5002)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5002)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5001)]
	string image
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5001)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5001)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5003)]
	string hoverImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5003)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5003)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5004)]
	string disabledImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5004)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5004)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5012)]
	string downImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5012)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5008)]
	string transparencyColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("DDDA102E-0E17-11D3-A2E2-00C04F79F88E")]
[ClassInterface(0)]
public class WMPTextCtrlClass : IWMPTextCtrl, WMPTextCtrl
{
	[DispId(5201)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5201)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5201)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5206)]
	public virtual extern string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5206)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5206)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5207)]
	public virtual extern string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5207)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5207)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5208)]
	public virtual extern int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5208)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5208)]
		[param: In]
		set;
	}

	[DispId(5209)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5209)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5209)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5210)]
	public virtual extern string hoverBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5210)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5210)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5211)]
	public virtual extern string hoverForegroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5211)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5211)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5212)]
	public virtual extern string hoverFontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5212)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5212)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5213)]
	public virtual extern string value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5213)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5213)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5214)]
	public virtual extern string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5214)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5214)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5215)]
	public virtual extern string disabledFontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5215)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5215)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5216)]
	public virtual extern string disabledForegroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5216)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5216)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5217)]
	public virtual extern string disabledBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5217)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5217)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5221)]
	public virtual extern bool fontSmoothing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5221)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5221)]
		[param: In]
		set;
	}

	[DispId(5222)]
	public virtual extern string justification
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5222)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5222)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5223)]
	public virtual extern bool wordWrap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5223)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5223)]
		[param: In]
		set;
	}

	[DispId(5224)]
	public virtual extern string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5224)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5224)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5225)]
	public virtual extern bool scrolling
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5225)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5225)]
		[param: In]
		set;
	}

	[DispId(5226)]
	public virtual extern string scrollingDirection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5226)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5226)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5227)]
	public virtual extern int scrollingDelay
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5227)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5227)]
		[param: In]
		set;
	}

	[DispId(5228)]
	public virtual extern int scrollingAmount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5228)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5228)]
		[param: In]
		set;
	}

	[DispId(5229)]
	public virtual extern int textWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5229)]
		get;
	}

	[DispId(5230)]
	public virtual extern bool onGlass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5230)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5230)]
		[param: In]
		set;
	}

	[DispId(5231)]
	public virtual extern bool disableGlassBlurBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5231)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5231)]
		[param: In]
		set;
	}
}
[ComImport]
[Guid("237DAC8E-0E32-11D3-A2E2-00C04F79F88E")]
[CoClass(typeof(WMPTextCtrlClass))]
public interface WMPTextCtrl : IWMPTextCtrl
{
}
[ComImport]
[TypeLibType(4160)]
[Guid("237DAC8E-0E32-11D3-A2E2-00C04F79F88E")]
public interface IWMPTextCtrl
{
	[DispId(5201)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5201)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5201)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5206)]
	string fontFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5206)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5206)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5207)]
	string fontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5207)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5207)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5208)]
	int fontSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5208)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5208)]
		[param: In]
		set;
	}

	[DispId(5209)]
	string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5209)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5209)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5210)]
	string hoverBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5210)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5210)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5211)]
	string hoverForegroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5211)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5211)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5212)]
	string hoverFontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5212)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5212)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5213)]
	string value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5213)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5213)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5214)]
	string toolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5214)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5214)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5215)]
	string disabledFontStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5215)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5215)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5216)]
	string disabledForegroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5216)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5216)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5217)]
	string disabledBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5217)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5217)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5221)]
	bool fontSmoothing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5221)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5221)]
		[param: In]
		set;
	}

	[DispId(5222)]
	string justification
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5222)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5222)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5223)]
	bool wordWrap
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5223)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5223)]
		[param: In]
		set;
	}

	[DispId(5224)]
	string cursor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5224)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5224)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5225)]
	bool scrolling
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5225)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5225)]
		[param: In]
		set;
	}

	[DispId(5226)]
	string scrollingDirection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5226)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5226)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5227)]
	int scrollingDelay
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5227)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5227)]
		[param: In]
		set;
	}

	[DispId(5228)]
	int scrollingAmount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5228)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5228)]
		[param: In]
		set;
	}

	[DispId(5229)]
	int textWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5229)]
		get;
	}

	[DispId(5230)]
	bool onGlass
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5230)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5230)]
		[param: In]
		set;
	}

	[DispId(5231)]
	bool disableGlassBlurBackground
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5231)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5231)]
		[param: In]
		set;
	}
}
[ComImport]
[Guid("5F9CFD93-8CAD-11D3-9A7E-00C04F8EFB70")]
[ClassInterface(0)]
public class WMPPlaylistCtrlClass : IWMPPlaylistCtrl, WMPPlaylistCtrl
{
	[DispId(5601)]
	public virtual extern IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5601)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5601)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(5602)]
	public virtual extern string columns
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5602)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5602)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5603)]
	public virtual extern int columnCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5603)]
		get;
	}

	[DispId(5604)]
	public virtual extern string columnOrder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5604)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5604)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5605)]
	public virtual extern bool columnsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5605)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5605)]
		[param: In]
		set;
	}

	[DispId(5607)]
	public virtual extern bool dropDownVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5607)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5607)]
		[param: In]
		set;
	}

	[DispId(5608)]
	public virtual extern bool playlistItemsVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5608)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5608)]
		[param: In]
		set;
	}

	[DispId(5609)]
	public virtual extern bool checkboxesVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5609)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5609)]
		[param: In]
		set;
	}

	[DispId(5612)]
	public virtual extern string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5612)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5612)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5613)]
	public virtual extern string foregroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5613)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5613)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5614)]
	public virtual extern string disabledItemColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5614)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5614)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5615)]
	public virtual extern string itemPlayingColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5615)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5615)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5616)]
	public virtual extern string itemPlayingBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5616)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5616)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5617)]
	public virtual extern string backgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5617)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5617)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5618)]
	public virtual extern bool allowItemEditing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5618)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5618)]
		[param: In]
		set;
	}

	[DispId(5619)]
	public virtual extern bool allowColumnSorting
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5619)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5619)]
		[param: In]
		set;
	}

	[DispId(5620)]
	public virtual extern string dropDownList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5620)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5620)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5621)]
	public virtual extern string dropDownToolTip
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5621)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5621)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5622)]
	public virtual extern bool copying
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5622)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5622)]
		[param: In]
		set;
	}

	[DispId(5642)]
	public virtual extern string itemErrorColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5642)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5642)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5643)]
	public virtual extern int itemCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5643)]
		get;
	}

	[DispId(5644)]
	public virtual extern IWMPMedia itemMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5644)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5645)]
	public virtual extern IWMPPlaylist itemPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5645)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5650)]
	public virtual extern string leftStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5650)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5650)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5651)]
	public virtual extern string rightStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5651)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5651)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5652)]
	public virtual extern bool editButtonVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5652)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5652)]
		[param: In]
		set;
	}

	[DispId(5653)]
	public virtual extern string dropDownImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5653)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5653)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5654)]
	public virtual extern string dropDownBackgroundImage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5654)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5654)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5655)]
	public virtual extern float hueShift
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5655)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5655)]
		[param: In]
		set;
	}

	[DispId(5656)]
	public virtual extern float saturation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5656)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5656)]
		[param: In]
		set;
	}

	[DispId(5658)]
	public virtual extern string statusColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5658)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5658)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5660)]
	public virtual extern bool toolbarVisible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5660)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5660)]
		[param: In]
		set;
	}

	[DispId(5662)]
	public virtual extern string itemSelectedColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5662)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5662)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5663)]
	public virtual extern string itemSelectedFocusLostColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5663)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5663)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5664)]
	public virtual extern string itemSelectedBackgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5664)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5664)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5665)]
	public virtual extern string itemSelectedBackgroundFocusLostColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5665)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5665)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5666)]
	public virtual extern string backgroundSplitColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5666)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5666)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(5667)]
	public virtual extern string statusTextColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5667)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5667)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5623)]
	public virtual extern void copy();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5624)]
	public virtual extern void abortCopy();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5625)]
	public virtual extern void deleteSelected();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5626)]
	public virtual extern void deleteSelectedFromLibrary();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5628)]
	public virtual extern void moveSelectedUp();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5629)]
	public virtual extern void moveSelectedDown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5630)]
	public virtual extern void addSelectedToPlaylist([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pdispPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5631)]
	public virtual extern int getNextSelectedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5632)]
	public virtual extern int getNextCheckedItem([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5633)]
	public virtual extern void setSelectedState([In] int nIndex, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5634)]
	public virtual extern void setCheckedState([In] int nIndex, [In] bool vbChecked);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5635)]
	public virtual extern void sortColumn([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5636)]
	public virtual extern void setColumnResizeMode([In] int nIndex, [In][MarshalAs(UnmanagedType.BStr)] string newMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5637)]
	public virtual extern void setColumnWidth([In] int nIndex, [In] int nWidth);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5646)]
	public virtual extern int getNextSelectedItem2([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5647)]
	public virtual extern int getNextCheckedItem2([In] int nStartIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5648)]
	public virtual extern void setSelectedState2([In] int nIndex, [In] bool vbSelected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5649)]
	public virtual extern void setCheckedState2([In] int nIndex, [In] bool vbChecked);
}
[ComImport]
[CoClass(typeof(WMPPlaylistCtrlClass))]
[Guid("5F9CFD92-8CAD-11D3-9A7E-00C04F8EFB70")]
public interface WMPPlaylistCtrl : IWMPPlaylistCtrl
{
}
[ComImport]
[Guid("891EADB1-1C45-48B0-B704-49A888DA98C4")]
[TypeLibType(4160)]
public interface ITaskCntrCtrl
{
	[DispId(1610743808)]
	object CurrentContainer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IUnknown)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743810)]
	void Activate();
}
[ComImport]
[Guid("D84CCA96-CCE2-11D2-9ECC-0000F8085981")]
[TypeLibType(4112)]
[InterfaceType(2)]
public interface _WMPCoreEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5001)]
	void OpenStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5101)]
	void PlayStateChange([In] int NewState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5102)]
	void AudioLanguageChange([In] int LangID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5002)]
	void StatusChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5301)]
	void ScriptCommand([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5403)]
	void NewStream();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5401)]
	void Disconnect([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5402)]
	void Buffering([In] bool Start);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5501)]
	void Error();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5601)]
	void Warning([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5201)]
	void EndOfStream([In] int Result);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5202)]
	void PositionChange([In] double oldPosition, [In] double newPosition);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5203)]
	void MarkerHit([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5204)]
	void DurationUnitChange([In] int NewDurationUnit);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5701)]
	void CdromMediaChange([In] int CdromNum);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5801)]
	void PlaylistChange([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5804)]
	void CurrentPlaylistChange([In] WMPPlaylistChangeEventType change);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5805)]
	void CurrentPlaylistItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5802)]
	void MediaChange([In][MarshalAs(UnmanagedType.IDispatch)] object Item);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5803)]
	void CurrentMediaItemAvailable([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5806)]
	void CurrentItemChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5807)]
	void MediaCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5808)]
	void MediaCollectionAttributeStringAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5809)]
	void MediaCollectionAttributeStringRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5820)]
	void MediaCollectionAttributeStringChanged([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5810)]
	void PlaylistCollectionChange();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5811)]
	void PlaylistCollectionPlaylistAdded([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5812)]
	void PlaylistCollectionPlaylistRemoved([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5818)]
	void PlaylistCollectionPlaylistSetAsDeleted([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5819)]
	void ModeChange([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5821)]
	void MediaError([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5823)]
	void OpenPlaylistSwitch([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5822)]
	void DomainChange([In][MarshalAs(UnmanagedType.BStr)] string strDomain);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5824)]
	void StringCollectionChange([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5825)]
	void MediaCollectionMediaAdded([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5826)]
	void MediaCollectionMediaRemoved([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
}
[ComImport]
[ComSourceInterfaces("WMPLib._WMPCoreEvents\0\0")]
[Guid("09428D37-E0B9-11D2-B147-00C04F79FAA6")]
[ClassInterface(0)]
public class WMPCoreClass : IWMPCore3, WMPCore, _WMPCoreEvents_Event, IWMPCore2, IWMPCore, IWMPControls, IWMPPlaylist, IWMPSettings, IWMPMedia, IWMPStringCollection, IWMPMediaCollection, IWMPPlaylistCollection, IWMPCdromCollection, IWMPCdrom, IWMPErrorItem, IWMPClosedCaption, IWMPError, IWMPDVD, IWMPControls2, IWMPMedia2, IWMPMedia3, IWMPMetadataPicture, IWMPMetadataText, IWMPSettings2, IWMPControls3, IWMPClosedCaption2
{
	[DispId(1)]
	public virtual extern string URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	public virtual extern WMPOpenState openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
	}

	[DispId(10)]
	public virtual extern WMPPlayState playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(4)]
	public virtual extern IWMPControls controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(5)]
	public virtual extern IWMPSettings settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(6)]
	public virtual extern IWMPMedia currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(8)]
	public virtual extern IWMPMediaCollection mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(9)]
	public virtual extern IWMPPlaylistCollection playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(11)]
	public virtual extern string versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(7)]
	public virtual extern IWMPNetwork network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(13)]
	public virtual extern IWMPPlaylist currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(14)]
	public virtual extern IWMPCdromCollection cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(15)]
	public virtual extern IWMPClosedCaption closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(16)]
	public virtual extern bool isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
	}

	[DispId(17)]
	public virtual extern IWMPError Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(18)]
	public virtual extern string status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(40)]
	public virtual extern IWMPDVD dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore2_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern WMPOpenState IWMPCore2_openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern WMPPlayState IWMPCore2_playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPControls IWMPCore2_controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPSettings IWMPCore2_settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPMedia IWMPCore2_currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPMediaCollection IWMPCore2_mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylistCollection IWMPCore2_playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore2_versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPNetwork IWMPCore2_network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylist IWMPCore2_currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPCdromCollection IWMPCore2_cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPClosedCaption IWMPCore2_closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPCore2_isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPError IWMPCore2_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore2_status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPDVD IWMPCore2_dvd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern WMPOpenState IWMPCore_openState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern WMPPlayState IWMPCore_playState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPControls IWMPCore_controls
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPSettings IWMPCore_settings
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPMedia IWMPCore_currentMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPMediaCollection IWMPCore_mediaCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylistCollection IWMPCore_playlistCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore_versionInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPNetwork IWMPCore_network
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPPlaylist IWMPCore_currentPlaylist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern IWMPCdromCollection IWMPCore_cdromCollection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern IWMPClosedCaption IWMPCore_closedCaption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPCore_isOnline
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPError IWMPCore_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string IWMPCore_status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string attributeName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool IWMPSettings_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern double rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPMedia_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPStringCollection_count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPCdromCollection_count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string driveSpecifier
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPPlaylist Playlist
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern int errorCode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string errorDescription
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern object errorContext
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
	}

	public virtual extern int remedy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string customUrl
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int errorCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPError_Item
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPDVD_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string domain
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPControls2_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPControls2_currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPControls2_currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia IWMPControls2_currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int IWMPControls2_currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPMedia2_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia2_sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia2_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int IWMPMedia2_imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia2_imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia2_markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPMedia2_duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia2_durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia2_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPMedia2_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern bool IWMPMedia3_isIdentical
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia3_sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMedia3_name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int IWMPMedia3_imageSourceWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia3_imageSourceHeight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int IWMPMedia3_markerCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPMedia3_duration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string IWMPMedia3_durationString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern int IWMPMedia3_attributeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern IWMPErrorItem IWMPMedia3_Error
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	public virtual extern string mimeType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string pictureType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMetadataPicture_URL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string IWMPMetadataText_Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern string text
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPSettings2_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern bool IWMPSettings2_autoStart
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPSettings2_baseURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPSettings2_defaultFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern bool IWMPSettings2_invokeURLs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPSettings2_mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_playCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern double IWMPSettings2_rate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_balance
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int IWMPSettings2_volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern bool IWMPSettings2_enableErrorDialogs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int defaultAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern string mediaAccessRights
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern bool IWMPControls3_isAvailable
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern double IWMPControls3_currentPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string IWMPControls3_currentPositionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	public virtual extern IWMPMedia IWMPControls3_currentItem
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	public virtual extern int IWMPControls3_currentMarker
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int audioLanguageCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int currentAudioLanguage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern int currentAudioLanguageIndex
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		set;
	}

	public virtual extern string currentPositionTimecode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMIStyle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMILang
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_SAMIFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern string IWMPClosedCaption2_captioningId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern int SAMILangCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern int SAMIStyleCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		get;
	}

	public virtual extern event _WMPCoreEvents_OpenStateChangeEventHandler OpenStateChange;

	public virtual extern event _WMPCoreEvents_PlayStateChangeEventHandler PlayStateChange;

	public virtual extern event _WMPCoreEvents_AudioLanguageChangeEventHandler AudioLanguageChange;

	public virtual extern event _WMPCoreEvents_StatusChangeEventHandler StatusChange;

	public virtual extern event _WMPCoreEvents_ScriptCommandEventHandler ScriptCommand;

	public virtual extern event _WMPCoreEvents_NewStreamEventHandler NewStream;

	public virtual extern event _WMPCoreEvents_DisconnectEventHandler Disconnect;

	public virtual extern event _WMPCoreEvents_BufferingEventHandler Buffering;

	public virtual extern event _WMPCoreEvents_ErrorEventHandler _WMPCoreEvents_Event_Error;

	public virtual extern event _WMPCoreEvents_WarningEventHandler Warning;

	public virtual extern event _WMPCoreEvents_EndOfStreamEventHandler EndOfStream;

	public virtual extern event _WMPCoreEvents_PositionChangeEventHandler PositionChange;

	public virtual extern event _WMPCoreEvents_MarkerHitEventHandler MarkerHit;

	public virtual extern event _WMPCoreEvents_DurationUnitChangeEventHandler DurationUnitChange;

	public virtual extern event _WMPCoreEvents_CdromMediaChangeEventHandler CdromMediaChange;

	public virtual extern event _WMPCoreEvents_PlaylistChangeEventHandler PlaylistChange;

	public virtual extern event _WMPCoreEvents_CurrentPlaylistChangeEventHandler CurrentPlaylistChange;

	public virtual extern event _WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler CurrentPlaylistItemAvailable;

	public virtual extern event _WMPCoreEvents_MediaChangeEventHandler MediaChange;

	public virtual extern event _WMPCoreEvents_CurrentMediaItemAvailableEventHandler CurrentMediaItemAvailable;

	public virtual extern event _WMPCoreEvents_CurrentItemChangeEventHandler CurrentItemChange;

	public virtual extern event _WMPCoreEvents_MediaCollectionChangeEventHandler MediaCollectionChange;

	public virtual extern event _WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler MediaCollectionAttributeStringAdded;

	public virtual extern event _WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler MediaCollectionAttributeStringRemoved;

	public virtual extern event _WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler MediaCollectionAttributeStringChanged;

	public virtual extern event _WMPCoreEvents_PlaylistCollectionChangeEventHandler PlaylistCollectionChange;

	public virtual extern event _WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler PlaylistCollectionPlaylistAdded;

	public virtual extern event _WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler PlaylistCollectionPlaylistRemoved;

	public virtual extern event _WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler PlaylistCollectionPlaylistSetAsDeleted;

	public virtual extern event _WMPCoreEvents_ModeChangeEventHandler ModeChange;

	public virtual extern event _WMPCoreEvents_MediaErrorEventHandler MediaError;

	public virtual extern event _WMPCoreEvents_OpenPlaylistSwitchEventHandler OpenPlaylistSwitch;

	public virtual extern event _WMPCoreEvents_DomainChangeEventHandler DomainChange;

	public virtual extern event _WMPCoreEvents_StringCollectionChangeEventHandler StringCollectionChange;

	public virtual extern event _WMPCoreEvents_MediaCollectionMediaAddedEventHandler MediaCollectionMediaAdded;

	public virtual extern event _WMPCoreEvents_MediaCollectionMediaRemovedEventHandler MediaCollectionMediaRemoved;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	public virtual extern void close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	public virtual extern void launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPMedia newMedia([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPCore2_close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPCore2_launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPCore_close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPCore_launchURL([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrName, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void clear();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void insertItem([In] int lIndex, [In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void appendItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void removeItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void moveItem(int lIndexOld, int lIndexNew);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPStringCollection_Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPMedia add([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByGenre([In][MarshalAs(UnmanagedType.BStr)] string bstrGenre);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAuthor([In][MarshalAs(UnmanagedType.BStr)] string bstrAuthor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAlbum([In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist getByAttribute([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfDeleteFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPStringCollection getAttributeStringCollection([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribute, [In][MarshalAs(UnmanagedType.BStr)] string bstrMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getMediaAtom([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist newPlaylist([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylistArray IWMPPlaylistCollection_getAll();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylistArray IWMPPlaylistCollection_getByName([In][MarshalAs(UnmanagedType.BStr)] string bstrName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void remove([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void setDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem, [In] bool varfIsDeleted);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool isDeleted([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPPlaylist importPlaylist([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPCdrom IWMPCdromCollection_Item([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern IWMPCdrom getByDriveSpecifier([In][MarshalAs(UnmanagedType.BStr)] string bstrDriveSpecifier);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void eject();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void clearErrorQueue();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void webHelp();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void topMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void titleMenu();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void back();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void resume();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls2_playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void step([In] int lStep);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double IWMPMedia2_getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia2_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia2_getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia2_isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia2_isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern double IWMPMedia3_getMarkerTime([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getMarkerName([In] int MarkerNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getAttributeName([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPMedia3_setItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName, [In][MarshalAs(UnmanagedType.BStr)] string bstrVal);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string IWMPMedia3_getItemInfoByAtom([In] int lAtom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia3_isMemberOf([In][MarshalAs(UnmanagedType.Interface)] IWMPPlaylist pPlaylist);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPMedia3_isReadOnlyItem([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getAttributeCountByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object getItemInfoByType([In][MarshalAs(UnmanagedType.BStr)] string bstrType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLanguage, [In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool IWMPSettings2_getMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPSettings2_setMode([In][MarshalAs(UnmanagedType.BStr)] string bstrMode, [In] bool varfMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern bool requestMediaAccessRights([In][MarshalAs(UnmanagedType.BStr)] string bstrDesiredAccess);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_play();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_fastForward();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_fastReverse();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_next();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_previous();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_playItem([In][MarshalAs(UnmanagedType.Interface)] IWMPMedia pIWMPMedia);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern void IWMPControls3_step([In] int lStep);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getAudioLanguageID([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getAudioLanguageDescription([In] int lIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getLanguageName([In] int lLangID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getSAMILangName([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	public virtual extern int getSAMILangID([In] int nIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string getSAMIStyleName([In] int nIndex);
}
[ComEventInterface(typeof(_WMPCoreEvents\u0000), typeof(_WMPCoreEvents_EventProvider\u0000))]
[TypeLibType(16)]
[ComVisible(false)]
public interface _WMPCoreEvents_Event
{
	event _WMPCoreEvents_OpenStateChangeEventHandler OpenStateChange;

	event _WMPCoreEvents_PlayStateChangeEventHandler PlayStateChange;

	event _WMPCoreEvents_AudioLanguageChangeEventHandler AudioLanguageChange;

	event _WMPCoreEvents_StatusChangeEventHandler StatusChange;

	event _WMPCoreEvents_ScriptCommandEventHandler ScriptCommand;

	event _WMPCoreEvents_NewStreamEventHandler NewStream;

	event _WMPCoreEvents_DisconnectEventHandler Disconnect;

	event _WMPCoreEvents_BufferingEventHandler Buffering;

	event _WMPCoreEvents_ErrorEventHandler Error;

	event _WMPCoreEvents_WarningEventHandler Warning;

	event _WMPCoreEvents_EndOfStreamEventHandler EndOfStream;

	event _WMPCoreEvents_PositionChangeEventHandler PositionChange;

	event _WMPCoreEvents_MarkerHitEventHandler MarkerHit;

	event _WMPCoreEvents_DurationUnitChangeEventHandler DurationUnitChange;

	event _WMPCoreEvents_CdromMediaChangeEventHandler CdromMediaChange;

	event _WMPCoreEvents_PlaylistChangeEventHandler PlaylistChange;

	event _WMPCoreEvents_CurrentPlaylistChangeEventHandler CurrentPlaylistChange;

	event _WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler CurrentPlaylistItemAvailable;

	event _WMPCoreEvents_MediaChangeEventHandler MediaChange;

	event _WMPCoreEvents_CurrentMediaItemAvailableEventHandler CurrentMediaItemAvailable;

	event _WMPCoreEvents_CurrentItemChangeEventHandler CurrentItemChange;

	event _WMPCoreEvents_MediaCollectionChangeEventHandler MediaCollectionChange;

	event _WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler MediaCollectionAttributeStringAdded;

	event _WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler MediaCollectionAttributeStringRemoved;

	event _WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler MediaCollectionAttributeStringChanged;

	event _WMPCoreEvents_PlaylistCollectionChangeEventHandler PlaylistCollectionChange;

	event _WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler PlaylistCollectionPlaylistAdded;

	event _WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler PlaylistCollectionPlaylistRemoved;

	event _WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler PlaylistCollectionPlaylistSetAsDeleted;

	event _WMPCoreEvents_ModeChangeEventHandler ModeChange;

	event _WMPCoreEvents_MediaErrorEventHandler MediaError;

	event _WMPCoreEvents_OpenPlaylistSwitchEventHandler OpenPlaylistSwitch;

	event _WMPCoreEvents_DomainChangeEventHandler DomainChange;

	event _WMPCoreEvents_StringCollectionChangeEventHandler StringCollectionChange;

	event _WMPCoreEvents_MediaCollectionMediaAddedEventHandler MediaCollectionMediaAdded;

	event _WMPCoreEvents_MediaCollectionMediaRemovedEventHandler MediaCollectionMediaRemoved;
}
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_OpenStateChangeEventHandler([In] int NewState);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_PlayStateChangeEventHandler([In] int NewState);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_AudioLanguageChangeEventHandler([In] int LangID);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_StatusChangeEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_ScriptCommandEventHandler([In][MarshalAs(UnmanagedType.BStr)] string scType, [In][MarshalAs(UnmanagedType.BStr)] string Param);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_NewStreamEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_DisconnectEventHandler([In] int Result);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_BufferingEventHandler([In] bool Start);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_ErrorEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_WarningEventHandler([In] int WarningType, [In] int Param, [In][MarshalAs(UnmanagedType.BStr)] string Description);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_EndOfStreamEventHandler([In] int Result);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_PositionChangeEventHandler([In] double oldPosition, [In] double newPosition);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_MarkerHitEventHandler([In] int MarkerNum);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_DurationUnitChangeEventHandler([In] int NewDurationUnit);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_CdromMediaChangeEventHandler([In] int CdromNum);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_PlaylistChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object Playlist, [In] WMPPlaylistChangeEventType change);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_CurrentPlaylistChangeEventHandler([In] WMPPlaylistChangeEventType change);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_MediaChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object Item);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_CurrentMediaItemAvailableEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_CurrentItemChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_MediaCollectionChangeEventHandler();
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrAttribVal);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrAttribName, [In][MarshalAs(UnmanagedType.BStr)] string bstrOldAttribVal, [In][MarshalAs(UnmanagedType.BStr)] string bstrNewAttribVal);
[ComVisible(false)]
[TypeLibType(16)]
public delegate void _WMPCoreEvents_PlaylistCollectionChangeEventHandler();
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler([In][MarshalAs(UnmanagedType.BStr)] string bstrPlaylistName, [In] bool varfIsDeleted);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_ModeChangeEventHandler([In][MarshalAs(UnmanagedType.BStr)] string ModeName, [In] bool NewValue);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_MediaErrorEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pMediaObject);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_OpenPlaylistSwitchEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pItem);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_DomainChangeEventHandler([In][MarshalAs(UnmanagedType.BStr)] string strDomain);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_StringCollectionChangeEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispStringCollection, [In] WMPStringCollectionChangeEventType change, [In] int lCollectionIndex);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_MediaCollectionMediaAddedEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _WMPCoreEvents_MediaCollectionMediaRemovedEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object pdispMedia);
[ComImport]
[Guid("7587C667-628F-499F-88E7-6A6F4E888464")]
[CoClass(typeof(WMPCoreClass))]
public interface WMPCore : IWMPCore3, _WMPCoreEvents_Event
{
}
[ComImport]
[ComConversionLoss]
[Guid("6B550945-018F-11D3-B14A-00C04F79FAA6")]
[TypeLibType(4160)]
public interface IWMPGraphEventHandler
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8151)]
	void NotifyGraphStateChange([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lGraphState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8173)]
	void AsyncNotifyGraphStateChange([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lGraphState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8153)]
	void NotifyRateChange([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] double dRate);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8157)]
	void NotifyPlaybackEnd([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [MarshalAs(UnmanagedType.BStr)] string bstrQueuedUrl, [ComAliasName("WMPLib.ULONG_PTR")] uint dwCurrentContext);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8156)]
	void NotifyStreamEnd([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8158)]
	void NotifyScriptCommand([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][MarshalAs(UnmanagedType.BStr)] string bstrCommand, [In][MarshalAs(UnmanagedType.BStr)] string bstrParam);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8172)]
	void NotifyEarlyScriptCommand([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][MarshalAs(UnmanagedType.BStr)] string bstrCommand, [In][MarshalAs(UnmanagedType.BStr)] string bstrParam, [In] double dTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8159)]
	void NotifyMarkerHit([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lMarker);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8160)]
	void NotifyGraphError([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lErrMajor, [In] int lErrMinor, [In] int lCondition, [In][MarshalAs(UnmanagedType.BStr)] string bstrInfo, [In][MarshalAs(UnmanagedType.IUnknown)] object punkGraphData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8161)]
	void NotifyAcquireCredentials([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][MarshalAs(UnmanagedType.BStr)] string bstrRealm, [In][MarshalAs(UnmanagedType.BStr)] string bstrSite, [In][Out] IntPtr bstrUser, [In][Out] IntPtr bstrPassword, [In][Out] ref uint pdwFlags, out bool pfCancel);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8178)]
	void NotifyUntrustedLicense([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL, out bool pfCancel);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8162)]
	void NotifyLicenseDialog([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][MarshalAs(UnmanagedType.BStr)] string bstrURL, [In][MarshalAs(UnmanagedType.BStr)] string bstrContent, [In] ref byte pPostData, [In] uint dwPostDataSize, [In] int lResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8163)]
	void NotifyNeedsIndividualization([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, out bool pfResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8165)]
	void NotifyNewMetadata([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8166)]
	void NotifyNewMediaCaps([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8167)]
	void NotifyDisconnect([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8168)]
	void NotifySave([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int fStarted, [In] int lResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8169)]
	void NotifyDelayClose([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] bool fDelay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8170)]
	void NotifyDVD([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int lEventCode, [In] int lParam1, [In] int lParam2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8171)]
	void NotifyRequestAppThreadAction([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] uint dwAction);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8174)]
	void NotifyPrerollReady([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8177)]
	void NotifyNewIcons([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8179)]
	void NotifyStepComplete([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8180)]
	void NotifyNewBitrate([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] uint dwBitrate);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8181)]
	void NotifyGraphCreationPreRender([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][ComAliasName("WMPLib.ULONG_PTR")] uint punkFilterGraph, [In][ComAliasName("WMPLib.ULONG_PTR")] uint punkCardeaEncConfig, [In][ComAliasName("WMPLib.ULONG_PTR")] uint phrContinue, [In][ComAliasName("WMPLib.ULONG_PTR")] uint hEventToSet);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8182)]
	void NotifyGraphCreationPostRender([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][ComAliasName("WMPLib.ULONG_PTR")] uint punkFilterGraph, [In][ComAliasName("WMPLib.ULONG_PTR")] uint phrContinue, [In][ComAliasName("WMPLib.ULONG_PTR")] uint hEventToSet);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8186)]
	void NotifyGraphUserEvent([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In] int EventCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8183)]
	void NotifyRevocation([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, out bool pfResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8184)]
	void NotifyNeedsWMGraphIndividualization([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph, [In][ComAliasName("WMPLib.ULONG_PTR")] uint phWnd, [In][ComAliasName("WMPLib.ULONG_PTR")] uint hIndivEvent, out bool pfCancel, out bool pfResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8185)]
	void NotifyNeedsFullscreen([In][ComAliasName("WMPLib.ULONG_PTR")] uint punkGraph);
}
[ComImport]
[TypeLibType(4160)]
[Guid("F8578BFA-CD8F-4CE1-A684-5B7E85FCA7DC")]
public interface IBattery
{
	[DispId(1)]
	int presetCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
	}

	[DispId(2)]
	object preset
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("40C6BDE7-9C90-49D4-AD20-BEF81A6C5F22")]
public interface IBatteryPreset
{
	[DispId(1)]
	string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("F85E2D65-207D-48DB-84B1-915E1735DB17")]
public interface IBatteryRandomPreset : IBatteryPreset
{
	[DispId(1)]
	new string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("876E7208-0172-4EBB-B08B-2E1D30DFE44C")]
[TypeLibType(4160)]
public interface IBatterySavedPreset : IBatteryPreset
{
	[DispId(1)]
	new string title
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("33E9291A-F6A9-11D2-9435-00A0C92A2F2D")]
[TypeLibType(4160)]
public interface IBarsEffect
{
	[DispId(8000)]
	int displayMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8000)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8000)]
		[param: In]
		set;
	}

	[DispId(8001)]
	bool showPeaks
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8001)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8001)]
		[param: In]
		set;
	}

	[DispId(8002)]
	int peakHangTime
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8002)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8002)]
		[param: In]
		set;
	}

	[DispId(8003)]
	float peakFallbackAcceleration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8003)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8003)]
		[param: In]
		set;
	}

	[DispId(8004)]
	float peakFallbackSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8004)]
		[param: In]
		set;
	}

	[DispId(8005)]
	float levelFallbackAcceleration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8005)]
		[param: In]
		set;
	}

	[DispId(8006)]
	float levelFallbackSpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8006)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8006)]
		[param: In]
		set;
	}

	[DispId(8007)]
	string backgroundColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8007)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8007)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(8008)]
	string levelColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8008)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8008)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(8009)]
	string peakColor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8009)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8009)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(8010)]
	int horizontalSpacing
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8010)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8010)]
		[param: In]
		set;
	}

	[DispId(8012)]
	int levelWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8012)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8012)]
		[param: In]
		set;
	}

	[DispId(8013)]
	float levelScale
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8013)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8013)]
		[param: In]
		set;
	}

	[DispId(8014)]
	int fadeRate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8014)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8014)]
		[param: In]
		set;
	}

	[DispId(8015)]
	int fadeMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8015)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8015)]
		[param: In]
		set;
	}

	[DispId(8016)]
	bool transparent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8016)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8016)]
		[param: In]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("E2CC638C-FD2C-409B-A1EA-5DDB72DC8E84")]
public interface IWMPExternal
{
	[DispId(10005)]
	string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("D10CCDFF-472D-498C-B5FE-3630E5405E0A")]
public interface IWMPExternalColors : IWMPExternal
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[Guid("54DF358E-CF38-4010-99F1-F44B0E9000E5")]
[TypeLibType(4160)]
public interface IWMPSubscriptionServiceLimited : IWMPExternalColors
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	new string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	new string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	new string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	new string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	new string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10026)]
	void NavigateTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrKeyName, [In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams);

	[DispId(10027)]
	string SelectedTaskPane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("2E922378-EE70-4CEB-BBAB-CE7CE4A04816")]
[TypeLibType(4160)]
public interface IWMPSubscriptionServiceExternal : IWMPSubscriptionServiceLimited
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	new string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	new string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	new string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	new string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	new string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10026)]
	new void NavigateTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrKeyName, [In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams);

	[DispId(10027)]
	new string SelectedTaskPane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(10009)]
	IWMPDownloadManager DownloadManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10009)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[Guid("E15E9AD1-8F20-4CC4-9EC7-1A328CA86A0D")]
[TypeLibType(4160)]
public interface IWMPDownloadManager
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1151)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPDownloadCollection getDownloadCollection([In] int lCollectionId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1152)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPDownloadCollection createDownloadCollection();
}
[ComImport]
[Guid("0A319C7F-85F9-436C-B88E-82FD88000E1C")]
[TypeLibType(4160)]
public interface IWMPDownloadCollection
{
	[DispId(1201)]
	int ID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1201)]
		get;
	}

	[DispId(1202)]
	int count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1202)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1203)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPDownloadItem2 Item([In] int lItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1204)]
	[return: MarshalAs(UnmanagedType.Interface)]
	IWMPDownloadItem2 startDownload([In][MarshalAs(UnmanagedType.BStr)] string bstrSourceURL, [In][MarshalAs(UnmanagedType.BStr)] string bstrType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1205)]
	void removeItem([In] int lItem);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1206)]
	void clear();
}
[ComImport]
[TypeLibType(4160)]
[Guid("9FBB3336-6DA3-479D-B8FF-67D46E20A987")]
public interface IWMPDownloadItem2 : IWMPDownloadItem
{
	[DispId(1251)]
	new string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1251)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1252)]
	new int size
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1252)]
		get;
	}

	[DispId(1253)]
	new string type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1253)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1254)]
	new int progress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1254)]
		get;
	}

	[DispId(1255)]
	new WMPSubscriptionDownloadState downloadState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1255)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1256)]
	new void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1257)]
	new void resume();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1258)]
	new void cancel();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1301)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string getItemInfo([In][MarshalAs(UnmanagedType.BStr)] string bstrItemName);
}
[ComImport]
[TypeLibType(4160)]
[Guid("C9470E8E-3F6B-46A9-A0A9-452815C34297")]
public interface IWMPDownloadItem
{
	[DispId(1251)]
	string sourceURL
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1251)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1252)]
	int size
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1252)]
		get;
	}

	[DispId(1253)]
	string type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1253)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1254)]
	int progress
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1254)]
		get;
	}

	[DispId(1255)]
	WMPSubscriptionDownloadState downloadState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1255)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1256)]
	void pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1257)]
	void resume();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1258)]
	void cancel();
}
public enum WMPSubscriptionDownloadState
{
	wmpsdlsDownloading,
	wmpsdlsPaused,
	wmpsdlsProcessing,
	wmpsdlsCompleted,
	wmpsdlsCancelled
}
[ComImport]
[Guid("5F0248C1-62B3-42D7-B927-029119E6AD14")]
[TypeLibType(4160)]
public interface IWMPSubscriptionServicePlayMedia : IWMPSubscriptionServiceLimited
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	new string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	new string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	new string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	new string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	new string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10026)]
	new void NavigateTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrKeyName, [In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams);

	[DispId(10027)]
	new string SelectedTaskPane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10004)]
	void playMedia([In][MarshalAs(UnmanagedType.BStr)] string bstrURL);
}
[ComImport]
[TypeLibType(4160)]
[Guid("A915CEA2-72DF-41E1-A576-EF0BAE5E5169")]
public interface IWMPDiscoExternal : IWMPSubscriptionServiceExternal
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	new string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	new string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	new string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	new string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	new string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10026)]
	new void NavigateTaskPaneURL([In][MarshalAs(UnmanagedType.BStr)] string bstrKeyName, [In][MarshalAs(UnmanagedType.BStr)] string bstrTaskPane, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams);

	[DispId(10027)]
	new string SelectedTaskPane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10027)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(10009)]
	new IWMPDownloadManager DownloadManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10009)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10028)]
	object OnLoginChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10028)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10029)]
	bool userLoggedIn
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10029)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10030)]
	void attemptLogin();

	[DispId(10031)]
	string accountType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10031)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10032)]
	object OnViewChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10032)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10033)]
	void changeView([In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationID, [In][MarshalAs(UnmanagedType.BStr)] string bstrFilter, [In][MarshalAs(UnmanagedType.BStr)] string bstrViewParams);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10034)]
	void changeViewOnlineList([In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationID, [In][MarshalAs(UnmanagedType.BStr)] string bstrParams, [In][MarshalAs(UnmanagedType.BStr)] string bstrFriendlyName, [In][MarshalAs(UnmanagedType.BStr)] string bstrListType, [In][MarshalAs(UnmanagedType.BStr)] string bstrViewMode);

	[DispId(10035)]
	string libraryLocationType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10035)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10036)]
	string libraryLocationID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10036)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10037)]
	string selectedItemType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10037)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10038)]
	string selectedItemID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10038)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10039)]
	string filter
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10039)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10040)]
	string task
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10040)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10041)]
	string viewParameters
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10041)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10042)]
	void cancelNavigate();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10043)]
	void showPopup([In] int lPopupIndex, [In][MarshalAs(UnmanagedType.BStr)] string bstrParameters);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10044)]
	void addToBasket([In][MarshalAs(UnmanagedType.BStr)] string bstrViewType, [In][MarshalAs(UnmanagedType.BStr)] string bstrViewIDs);

	[DispId(10045)]
	string basketTitle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10045)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10046)]
	void play([In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationType, [In][MarshalAs(UnmanagedType.BStr)] string bstrLibraryLocationIDs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10047)]
	void download([In][MarshalAs(UnmanagedType.BStr)] string bstrViewType, [In][MarshalAs(UnmanagedType.BStr)] string bstrViewIDs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10048)]
	void buy([In][MarshalAs(UnmanagedType.BStr)] string bstrViewType, [In][MarshalAs(UnmanagedType.BStr)] string bstrViewIDs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10049)]
	void saveCurrentViewToLibrary([In][MarshalAs(UnmanagedType.BStr)] string bstrFriendlyListType, [In] bool fDynamic);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10050)]
	void authenticate([In] int lAuthenticationIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10051)]
	void sendMessage([In][MarshalAs(UnmanagedType.BStr)] string bstrMsg, [In][MarshalAs(UnmanagedType.BStr)] string bstrParam);

	[DispId(10052)]
	object OnSendMessageComplete
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10052)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10053)]
	bool ignoreIEHistory
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10053)]
		[param: In]
		set;
	}

	[DispId(10054)]
	bool pluginRunning
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10054)]
		get;
	}

	[DispId(10055)]
	bool templateBeingDisplayedInLocalLibrary
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10055)]
		get;
	}

	[DispId(10056)]
	object OnChangeViewError
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10056)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10057)]
	object OnChangeViewOnlineListError
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10057)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}
}
[ComImport]
[TypeLibType(4160)]
[Guid("2D7EF888-1D3C-484A-A906-9F49D99BB344")]
public interface IWMPCDDVDWizardExternal : IWMPExternalColors
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(10013)]
	new string appColorMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10013)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10014)]
	new string appColorDark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10014)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10015)]
	new string appColorButtonHighlight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10015)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10016)]
	new string appColorButtonShadow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10016)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10017)]
	new string appColorButtonHoverFace
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10017)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10001)]
	void WriteNames([In][MarshalAs(UnmanagedType.BStr)] string bstrTOC, [In][MarshalAs(UnmanagedType.BStr)] string bstrMetadata);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10002)]
	void ReturnToMainTask();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10007)]
	void WriteNamesEx([In] WMP_WRITENAMESEX_TYPE type, [In][MarshalAs(UnmanagedType.BStr)] string bstrTypeId, [In][MarshalAs(UnmanagedType.BStr)] string bstrMetadata, [In] bool fRenameRegroupFiles);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10008)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string GetMDQByRequestID([In][MarshalAs(UnmanagedType.BStr)] string bstrRequestID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10011)]
	void EditMetadata();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10010)]
	bool IsMetadataAvailableForEdit();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10023)]
	void BuyCD([In][MarshalAs(UnmanagedType.BStr)] string bstrTitle, [In][MarshalAs(UnmanagedType.BStr)] string bstrArtist, [In][MarshalAs(UnmanagedType.BStr)] string bstrAlbum, [In][MarshalAs(UnmanagedType.BStr)] string bstrUFID, [In][MarshalAs(UnmanagedType.BStr)] string bstrWMID);
}
public enum WMP_WRITENAMESEX_TYPE
{
	WMP_WRITENAMES_TYPE_CD_BY_TOC,
	WMP_WRITENAMES_TYPE_CD_BY_CONTENT_ID,
	WMP_WRITENAMES_TYPE_CD_BY_MDQCD,
	WMP_WRITENAMES_TYPE_DVD_BY_DVDID
}
[ComImport]
[TypeLibType(4160)]
[Guid("F81B2A59-02BC-4003-8B2F-C124AF66FC66")]
public interface IWMPBaseExternal : IWMPExternal
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}
}
[ComImport]
[Guid("3148E685-B243-423D-8341-8480D6EFF674")]
[TypeLibType(4160)]
public interface IWMPOfflineExternal : IWMPExternal
{
	[DispId(10005)]
	new string version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10005)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10012)]
	new string appColorLight
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10012)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10018)]
	new object OnColorChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10018)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10025)]
	void forceOnline();
}
[ComImport]
[TypeLibType(4160)]
[Guid("4E195DB1-9E29-47FC-9CE1-DE9937D32925")]
public interface IWMPDMRAVTransportService
{
	[DispId(1)]
	string TransportState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2)]
	string TransportStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3)]
	string PlaybackStorageMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(4)]
	string RecordStorageMedium
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5)]
	string PossiblePlaybackStorageMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(6)]
	string PossibleRecordStorageMedia
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(7)]
	string CurrentPlayMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(8)]
	string TransportPlaySpeed
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(9)]
	string RecordMediumWriteStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(10)]
	string CurrentRecordQualityMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(11)]
	string PossibleRecordQualityModes
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(12)]
	uint NumberOfTracks
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(12)]
		get;
	}

	[DispId(13)]
	uint CurrentTrack
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		get;
	}

	[DispId(14)]
	string CurrentTrackDuration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(15)]
	string CurrentMediaDuration
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(16)]
	string CurrentTrackMetaData
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(17)]
	string CurrentTrackURI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(18)]
	string AVTransportURI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(19)]
	string AVTransportURIMetaData
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(19)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(20)]
	string NextAVTransportURI
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(21)]
	string NextAVTransportURIMetaData
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(21)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(22)]
	string RelativeTimePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(22)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(23)]
	string AbsoluteTimePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(23)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(24)]
	int RelativeCounterPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(24)]
		get;
	}

	[DispId(25)]
	int AbsoluteCounterPosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(25)]
		get;
	}

	[DispId(26)]
	string CurrentTransportActions
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(26)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(27)]
	string LastChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(27)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(28)]
	string A_ARG_TYPE_SeekMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(28)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(29)]
	string A_ARG_TYPE_SeekTarget
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(29)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(30)]
	uint A_ARG_TYPE_InstanceID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(30)]
		get;
	}

	[DispId(45)]
	string CurrentProtocolInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(45)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(31)]
	void SetAVTransportURI([In][MarshalAs(UnmanagedType.IUnknown)] object punkRemoteEndpointInfo, [In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrCurrentURI, [In][MarshalAs(UnmanagedType.BStr)] string bstrCurrentURIMetaData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(33)]
	void GetMediaInfo([In] uint ulInstanceID, out uint pulNumTracks, [MarshalAs(UnmanagedType.BStr)] out string pbstrMediaDuration, [MarshalAs(UnmanagedType.BStr)] out string pbstrCurrentURI, [MarshalAs(UnmanagedType.BStr)] out string pbstrCurrentURIMetaData, [MarshalAs(UnmanagedType.BStr)] out string pbstrNextURI, [MarshalAs(UnmanagedType.BStr)] out string pNextURIMetaData, [MarshalAs(UnmanagedType.BStr)] out string pbstrPlayMedium, [MarshalAs(UnmanagedType.BStr)] out string pbstrRecordMedium, [MarshalAs(UnmanagedType.BStr)] out string pbstrWriteStatus);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(34)]
	void GetTransportInfo([In] uint ulInstanceID, [MarshalAs(UnmanagedType.BStr)] out string pbstrCurrentTransportState, [MarshalAs(UnmanagedType.BStr)] out string pbstrCurrentTransportStatus, [MarshalAs(UnmanagedType.BStr)] out string pbstrCurrentSpeed);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(35)]
	void GetPositionInfo([In] uint ulInstanceID, out uint pTrack, [MarshalAs(UnmanagedType.BStr)] out string pbstrTrackDuration, [MarshalAs(UnmanagedType.BStr)] out string pbstrTrackMetaData, [MarshalAs(UnmanagedType.BStr)] out string pbstrTrackURI, [MarshalAs(UnmanagedType.BStr)] out string pbstrRelTime, [MarshalAs(UnmanagedType.BStr)] out string pbstrAbsTime, out int plRelCount, out int plAbsCount);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(36)]
	void GetDeviceCapabilities([In] uint ulInstanceID, [MarshalAs(UnmanagedType.BStr)] out string pbstrPlayMedia, [MarshalAs(UnmanagedType.BStr)] out string pbstrRecMedia, [MarshalAs(UnmanagedType.BStr)] out string pbstrRecQualityModes);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(37)]
	void GetTransportSettings([In] uint ulInstanceID, [MarshalAs(UnmanagedType.BStr)] out string pbstrPlayMode, [MarshalAs(UnmanagedType.BStr)] out string pbstrRecQualityMode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(38)]
	void stop([In] uint ulInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(39)]
	void play([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrSpeed);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(40)]
	void pause([In] uint ulInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	void Seek([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrUnit, [In][MarshalAs(UnmanagedType.BStr)] string bstrTarget);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	void next([In] uint ulInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(43)]
	void previous([In] uint ulInstanceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(44)]
	void GetCurrentTransportActions([In] uint ulInstanceID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrActions);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(32)]
	void SetNextAVTransportURI([In][MarshalAs(UnmanagedType.IUnknown)] object punkRemoteEndpointInfo, [In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrNextURI, [In][MarshalAs(UnmanagedType.BStr)] string bstrNextURIMetaData);
}
[ComImport]
[Guid("FB61CD38-8DE7-4479-8B76-A8D097C20C70")]
[TypeLibType(4160)]
public interface IWMPDMRConnectionManagerService
{
	[DispId(1)]
	string SourceProtocolInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2)]
	string SinkProtocolInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3)]
	string CurrentConnectionIDs
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(4)]
	string A_ARG_TYPE_ConnectionStatus
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5)]
	string A_ARG_TYPE_ConnectionManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(6)]
	string A_ARG_TYPE_Direction
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(7)]
	string A_ARG_TYPE_ProtocolInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(8)]
	int A_ARG_TYPE_ConnectionID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		get;
	}

	[DispId(9)]
	int A_ARG_TYPE_AVTransportID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		get;
	}

	[DispId(10)]
	int A_ARG_TYPE_RcsID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	void GetProtocolInfo([In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrSource, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrSink);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	void GetCurrentConnectionIDs([In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrConnectionIDs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	void GetCurrentConnectionInfo([In] int lConnectionID, [In][Out] ref int plResID, [In][Out] ref int plAVTransportID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrProtocolInfo, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrPeerConnectionManager, [In][Out] ref int plPeerConnectionID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrDirection, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrStatus);
}
[ComImport]
[TypeLibType(4160)]
[Guid("FF4B1BDA-19F0-42CF-8DDA-19162950C543")]
public interface IWMPDMRRenderingControlService
{
	[DispId(1)]
	string LastChange
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(2)]
	string PresetNameList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(3)]
	bool mute
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		get;
	}

	[DispId(4)]
	ushort volume
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		get;
	}

	[DispId(5)]
	string A_ARG_TYPE_Channel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(6)]
	uint A_ARG_TYPE_InstanceID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		get;
	}

	[DispId(7)]
	string A_ARG_TYPE_PresetName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	void ListPresets([In] uint ulInstanceID, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string pbstrCurrentPresetList);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	void SelectPreset([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrPresetName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	void GetMute([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrChannel, [In][Out] ref bool pbCurrentMute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	void SetMute([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrChannel, [In] bool bDesiredMute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	void GetVolume([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrChannel, [In][Out] ref ushort puiCurrentVolume);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	void SetVolume([In] uint ulInstanceID, [In][MarshalAs(UnmanagedType.BStr)] string bstrChannel, [In] ushort uiDesiredVolume);
}
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class IWMPRegionalButtonEvents_SinkHelper : IWMPRegionalButtonEvents
{
	public IWMPRegionalButtonEvents_onblurEventHandler m_onblurDelegate;

	public IWMPRegionalButtonEvents_onfocusEventHandler m_onfocusDelegate;

	public IWMPRegionalButtonEvents_onclickEventHandler m_onclickDelegate;

	public IWMPRegionalButtonEvents_ondblclickEventHandler m_ondblclickDelegate;

	public IWMPRegionalButtonEvents_onmousedownEventHandler m_onmousedownDelegate;

	public IWMPRegionalButtonEvents_onmouseupEventHandler m_onmouseupDelegate;

	public IWMPRegionalButtonEvents_onmousemoveEventHandler m_onmousemoveDelegate;

	public IWMPRegionalButtonEvents_onmouseoverEventHandler m_onmouseoverDelegate;

	public IWMPRegionalButtonEvents_onmouseoutEventHandler m_onmouseoutDelegate;

	public IWMPRegionalButtonEvents_onkeypressEventHandler m_onkeypressDelegate;

	public IWMPRegionalButtonEvents_onkeydownEventHandler m_onkeydownDelegate;

	public IWMPRegionalButtonEvents_onkeyupEventHandler m_onkeyupDelegate;

	public int m_dwCookie;

	public void onblur()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onblurDelegate != null)
		{
			m_onblurDelegate();
		}
	}

	public void onfocus()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onfocusDelegate != null)
		{
			m_onfocusDelegate();
		}
	}

	public void onclick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onclickDelegate != null)
		{
			m_onclickDelegate();
		}
	}

	public void ondblclick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ondblclickDelegate != null)
		{
			m_ondblclickDelegate();
		}
	}

	public void onmousedown()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onmousedownDelegate != null)
		{
			m_onmousedownDelegate();
		}
	}

	public void onmouseup()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onmouseupDelegate != null)
		{
			m_onmouseupDelegate();
		}
	}

	public void onmousemove()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onmousemoveDelegate != null)
		{
			m_onmousemoveDelegate();
		}
	}

	public void onmouseover()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onmouseoverDelegate != null)
		{
			m_onmouseoverDelegate();
		}
	}

	public void onmouseout()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onmouseoutDelegate != null)
		{
			m_onmouseoutDelegate();
		}
	}

	public void onkeypress()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onkeypressDelegate != null)
		{
			m_onkeypressDelegate();
		}
	}

	public void onkeydown()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onkeydownDelegate != null)
		{
			m_onkeydownDelegate();
		}
	}

	public void onkeyup()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onkeyupDelegate != null)
		{
			m_onkeyupDelegate();
		}
	}

	internal IWMPRegionalButtonEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_onblurDelegate = null;
		m_onfocusDelegate = null;
		m_onclickDelegate = null;
		m_ondblclickDelegate = null;
		m_onmousedownDelegate = null;
		m_onmouseupDelegate = null;
		m_onmousemoveDelegate = null;
		m_onmouseoverDelegate = null;
		m_onmouseoutDelegate = null;
		m_onkeypressDelegate = null;
		m_onkeydownDelegate = null;
		m_onkeyupDelegate = null;
	}
}
internal sealed class IWMPRegionalButtonEvents_EventProvider : IWMPRegionalButtonEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			49, 141, 252, 80, 172, 103, 211, 17, 189, 76,
			0, 192, 79, 110, 165, 174
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_onblur(IWMPRegionalButtonEvents_onblurEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onblurDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onblur(IWMPRegionalButtonEvents_onblurEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onblurDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onblurDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onfocus(IWMPRegionalButtonEvents_onfocusEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onfocusDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onfocus(IWMPRegionalButtonEvents_onfocusEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onfocusDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onfocusDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onclick(IWMPRegionalButtonEvents_onclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onclickDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onclick(IWMPRegionalButtonEvents_onclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onclickDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onclickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ondblclick(IWMPRegionalButtonEvents_ondblclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_ondblclickDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ondblclick(IWMPRegionalButtonEvents_ondblclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_ondblclickDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_ondblclickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onmousedown(IWMPRegionalButtonEvents_onmousedownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onmousedownDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onmousedown(IWMPRegionalButtonEvents_onmousedownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onmousedownDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onmousedownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onmouseup(IWMPRegionalButtonEvents_onmouseupEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onmouseupDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onmouseup(IWMPRegionalButtonEvents_onmouseupEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onmouseupDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onmouseupDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onmousemove(IWMPRegionalButtonEvents_onmousemoveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onmousemoveDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onmousemove(IWMPRegionalButtonEvents_onmousemoveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onmousemoveDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onmousemoveDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onmouseover(IWMPRegionalButtonEvents_onmouseoverEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onmouseoverDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onmouseover(IWMPRegionalButtonEvents_onmouseoverEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onmouseoverDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onmouseoverDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onmouseout(IWMPRegionalButtonEvents_onmouseoutEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onmouseoutDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onmouseout(IWMPRegionalButtonEvents_onmouseoutEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onmouseoutDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onmouseoutDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onkeypress(IWMPRegionalButtonEvents_onkeypressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onkeypressDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onkeypress(IWMPRegionalButtonEvents_onkeypressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onkeypressDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onkeypressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onkeydown(IWMPRegionalButtonEvents_onkeydownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onkeydownDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onkeydown(IWMPRegionalButtonEvents_onkeydownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onkeydownDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onkeydownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onkeyup(IWMPRegionalButtonEvents_onkeyupEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = new IWMPRegionalButtonEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPRegionalButtonEvents_SinkHelper, out pdwCookie);
			iWMPRegionalButtonEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPRegionalButtonEvents_SinkHelper.m_onkeyupDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPRegionalButtonEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onkeyup(IWMPRegionalButtonEvents_onkeyupEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPRegionalButtonEvents_SinkHelper.m_onkeyupDelegate != null && ((iWMPRegionalButtonEvents_SinkHelper.m_onkeyupDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IWMPRegionalButtonEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					IWMPRegionalButtonEvents_SinkHelper iWMPRegionalButtonEvents_SinkHelper = (IWMPRegionalButtonEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iWMPRegionalButtonEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class IWMPButtonCtrlEvents_SinkHelper : IWMPButtonCtrlEvents
{
	public IWMPButtonCtrlEvents_onclickEventHandler m_onclickDelegate;

	public int m_dwCookie;

	public void onclick()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onclickDelegate != null)
		{
			m_onclickDelegate();
		}
	}

	internal IWMPButtonCtrlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_onclickDelegate = null;
	}
}
internal sealed class IWMPButtonCtrlEvents_EventProvider : IWMPButtonCtrlEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			247, 255, 23, 187, 146, 22, 85, 69, 145, 138,
			106, 247, 191, 172, 237, 210
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_onclick(IWMPButtonCtrlEvents_onclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPButtonCtrlEvents_SinkHelper iWMPButtonCtrlEvents_SinkHelper = new IWMPButtonCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPButtonCtrlEvents_SinkHelper, out pdwCookie);
			iWMPButtonCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPButtonCtrlEvents_SinkHelper.m_onclickDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPButtonCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onclick(IWMPButtonCtrlEvents_onclickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPButtonCtrlEvents_SinkHelper iWMPButtonCtrlEvents_SinkHelper = (IWMPButtonCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPButtonCtrlEvents_SinkHelper.m_onclickDelegate != null && ((iWMPButtonCtrlEvents_SinkHelper.m_onclickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPButtonCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IWMPButtonCtrlEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					IWMPButtonCtrlEvents_SinkHelper iWMPButtonCtrlEvents_SinkHelper = (IWMPButtonCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iWMPButtonCtrlEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class IWMPVideoCtrlEvents_SinkHelper : IWMPVideoCtrlEvents
{
	public IWMPVideoCtrlEvents_onvideostartEventHandler m_onvideostartDelegate;

	public IWMPVideoCtrlEvents_onvideoendEventHandler m_onvideoendDelegate;

	public int m_dwCookie;

	public void onvideostart()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onvideostartDelegate != null)
		{
			m_onvideostartDelegate();
		}
	}

	public void onvideoend()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onvideoendDelegate != null)
		{
			m_onvideoendDelegate();
		}
	}

	internal IWMPVideoCtrlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_onvideostartDelegate = null;
		m_onvideoendDelegate = null;
	}
}
internal sealed class IWMPVideoCtrlEvents_EventProvider : IWMPVideoCtrlEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			119, 4, 92, 168, 76, 113, 6, 74, 185, 246,
			124, 140, 163, 139, 69, 220
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_onvideostart(IWMPVideoCtrlEvents_onvideostartEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPVideoCtrlEvents_SinkHelper iWMPVideoCtrlEvents_SinkHelper = new IWMPVideoCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPVideoCtrlEvents_SinkHelper, out pdwCookie);
			iWMPVideoCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPVideoCtrlEvents_SinkHelper.m_onvideostartDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPVideoCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onvideostart(IWMPVideoCtrlEvents_onvideostartEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPVideoCtrlEvents_SinkHelper iWMPVideoCtrlEvents_SinkHelper = (IWMPVideoCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPVideoCtrlEvents_SinkHelper.m_onvideostartDelegate != null && ((iWMPVideoCtrlEvents_SinkHelper.m_onvideostartDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPVideoCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onvideoend(IWMPVideoCtrlEvents_onvideoendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPVideoCtrlEvents_SinkHelper iWMPVideoCtrlEvents_SinkHelper = new IWMPVideoCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPVideoCtrlEvents_SinkHelper, out pdwCookie);
			iWMPVideoCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPVideoCtrlEvents_SinkHelper.m_onvideoendDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPVideoCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onvideoend(IWMPVideoCtrlEvents_onvideoendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPVideoCtrlEvents_SinkHelper iWMPVideoCtrlEvents_SinkHelper = (IWMPVideoCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPVideoCtrlEvents_SinkHelper.m_onvideoendDelegate != null && ((iWMPVideoCtrlEvents_SinkHelper.m_onvideoendDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPVideoCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IWMPVideoCtrlEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					IWMPVideoCtrlEvents_SinkHelper iWMPVideoCtrlEvents_SinkHelper = (IWMPVideoCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iWMPVideoCtrlEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class IWMPSliderCtrlEvents_SinkHelper : IWMPSliderCtrlEvents
{
	public IWMPSliderCtrlEvents_ondragbeginEventHandler m_ondragbeginDelegate;

	public IWMPSliderCtrlEvents_ondragendEventHandler m_ondragendDelegate;

	public IWMPSliderCtrlEvents_onpositionchangeEventHandler m_onpositionchangeDelegate;

	public int m_dwCookie;

	public void ondragbegin()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ondragbeginDelegate != null)
		{
			m_ondragbeginDelegate();
		}
	}

	public void ondragend()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ondragendDelegate != null)
		{
			m_ondragendDelegate();
		}
	}

	public void onpositionchange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onpositionchangeDelegate != null)
		{
			m_onpositionchangeDelegate();
		}
	}

	internal IWMPSliderCtrlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ondragbeginDelegate = null;
		m_ondragendDelegate = null;
		m_onpositionchangeDelegate = null;
	}
}
internal sealed class IWMPSliderCtrlEvents_EventProvider : IWMPSliderCtrlEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			210, 20, 172, 205, 228, 139, 211, 17, 187, 72,
			0, 160, 201, 60, 167, 58
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_ondragbegin(IWMPSliderCtrlEvents_ondragbeginEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = new IWMPSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ondragbegin(IWMPSliderCtrlEvents_ondragbeginEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = (IWMPSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate != null && ((iWMPSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ondragend(IWMPSliderCtrlEvents_ondragendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = new IWMPSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPSliderCtrlEvents_SinkHelper.m_ondragendDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ondragend(IWMPSliderCtrlEvents_ondragendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = (IWMPSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPSliderCtrlEvents_SinkHelper.m_ondragendDelegate != null && ((iWMPSliderCtrlEvents_SinkHelper.m_ondragendDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onpositionchange(IWMPSliderCtrlEvents_onpositionchangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = new IWMPSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onpositionchange(IWMPSliderCtrlEvents_onpositionchangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = (IWMPSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate != null && ((iWMPSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IWMPSliderCtrlEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					IWMPSliderCtrlEvents_SinkHelper iWMPSliderCtrlEvents_SinkHelper = (IWMPSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iWMPSliderCtrlEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class _WMPOCXEvents_SinkHelper : _WMPOCXEvents
{
	public _WMPOCXEvents_OpenStateChangeEventHandler m_OpenStateChangeDelegate;

	public _WMPOCXEvents_PlayStateChangeEventHandler m_PlayStateChangeDelegate;

	public _WMPOCXEvents_AudioLanguageChangeEventHandler m_AudioLanguageChangeDelegate;

	public _WMPOCXEvents_StatusChangeEventHandler m_StatusChangeDelegate;

	public _WMPOCXEvents_ScriptCommandEventHandler m_ScriptCommandDelegate;

	public _WMPOCXEvents_NewStreamEventHandler m_NewStreamDelegate;

	public _WMPOCXEvents_DisconnectEventHandler m_DisconnectDelegate;

	public _WMPOCXEvents_BufferingEventHandler m_BufferingDelegate;

	public _WMPOCXEvents_ErrorEventHandler m_ErrorDelegate;

	public _WMPOCXEvents_WarningEventHandler m_WarningDelegate;

	public _WMPOCXEvents_EndOfStreamEventHandler m_EndOfStreamDelegate;

	public _WMPOCXEvents_PositionChangeEventHandler m_PositionChangeDelegate;

	public _WMPOCXEvents_MarkerHitEventHandler m_MarkerHitDelegate;

	public _WMPOCXEvents_DurationUnitChangeEventHandler m_DurationUnitChangeDelegate;

	public _WMPOCXEvents_CdromMediaChangeEventHandler m_CdromMediaChangeDelegate;

	public _WMPOCXEvents_PlaylistChangeEventHandler m_PlaylistChangeDelegate;

	public _WMPOCXEvents_CurrentPlaylistChangeEventHandler m_CurrentPlaylistChangeDelegate;

	public _WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler m_CurrentPlaylistItemAvailableDelegate;

	public _WMPOCXEvents_MediaChangeEventHandler m_MediaChangeDelegate;

	public _WMPOCXEvents_CurrentMediaItemAvailableEventHandler m_CurrentMediaItemAvailableDelegate;

	public _WMPOCXEvents_CurrentItemChangeEventHandler m_CurrentItemChangeDelegate;

	public _WMPOCXEvents_MediaCollectionChangeEventHandler m_MediaCollectionChangeDelegate;

	public _WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler m_MediaCollectionAttributeStringAddedDelegate;

	public _WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler m_MediaCollectionAttributeStringRemovedDelegate;

	public _WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler m_MediaCollectionAttributeStringChangedDelegate;

	public _WMPOCXEvents_PlaylistCollectionChangeEventHandler m_PlaylistCollectionChangeDelegate;

	public _WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler m_PlaylistCollectionPlaylistAddedDelegate;

	public _WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler m_PlaylistCollectionPlaylistRemovedDelegate;

	public _WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler m_PlaylistCollectionPlaylistSetAsDeletedDelegate;

	public _WMPOCXEvents_ModeChangeEventHandler m_ModeChangeDelegate;

	public _WMPOCXEvents_MediaErrorEventHandler m_MediaErrorDelegate;

	public _WMPOCXEvents_OpenPlaylistSwitchEventHandler m_OpenPlaylistSwitchDelegate;

	public _WMPOCXEvents_DomainChangeEventHandler m_DomainChangeDelegate;

	public _WMPOCXEvents_SwitchedToPlayerApplicationEventHandler m_SwitchedToPlayerApplicationDelegate;

	public _WMPOCXEvents_SwitchedToControlEventHandler m_SwitchedToControlDelegate;

	public _WMPOCXEvents_PlayerDockedStateChangeEventHandler m_PlayerDockedStateChangeDelegate;

	public _WMPOCXEvents_PlayerReconnectEventHandler m_PlayerReconnectDelegate;

	public _WMPOCXEvents_ClickEventHandler m_ClickDelegate;

	public _WMPOCXEvents_DoubleClickEventHandler m_DoubleClickDelegate;

	public _WMPOCXEvents_KeyDownEventHandler m_KeyDownDelegate;

	public _WMPOCXEvents_KeyPressEventHandler m_KeyPressDelegate;

	public _WMPOCXEvents_KeyUpEventHandler m_KeyUpDelegate;

	public _WMPOCXEvents_MouseDownEventHandler m_MouseDownDelegate;

	public _WMPOCXEvents_MouseMoveEventHandler m_MouseMoveDelegate;

	public _WMPOCXEvents_MouseUpEventHandler m_MouseUpDelegate;

	public _WMPOCXEvents_DeviceConnectEventHandler m_DeviceConnectDelegate;

	public _WMPOCXEvents_DeviceDisconnectEventHandler m_DeviceDisconnectDelegate;

	public _WMPOCXEvents_DeviceStatusChangeEventHandler m_DeviceStatusChangeDelegate;

	public _WMPOCXEvents_DeviceSyncStateChangeEventHandler m_DeviceSyncStateChangeDelegate;

	public _WMPOCXEvents_DeviceSyncErrorEventHandler m_DeviceSyncErrorDelegate;

	public _WMPOCXEvents_CreatePartnershipCompleteEventHandler m_CreatePartnershipCompleteDelegate;

	public _WMPOCXEvents_DeviceEstimationEventHandler m_DeviceEstimationDelegate;

	public _WMPOCXEvents_CdromRipStateChangeEventHandler m_CdromRipStateChangeDelegate;

	public _WMPOCXEvents_CdromRipMediaErrorEventHandler m_CdromRipMediaErrorDelegate;

	public _WMPOCXEvents_CdromBurnStateChangeEventHandler m_CdromBurnStateChangeDelegate;

	public _WMPOCXEvents_CdromBurnMediaErrorEventHandler m_CdromBurnMediaErrorDelegate;

	public _WMPOCXEvents_CdromBurnErrorEventHandler m_CdromBurnErrorDelegate;

	public _WMPOCXEvents_LibraryConnectEventHandler m_LibraryConnectDelegate;

	public _WMPOCXEvents_LibraryDisconnectEventHandler m_LibraryDisconnectDelegate;

	public _WMPOCXEvents_FolderScanStateChangeEventHandler m_FolderScanStateChangeDelegate;

	public _WMPOCXEvents_StringCollectionChangeEventHandler m_StringCollectionChangeDelegate;

	public _WMPOCXEvents_MediaCollectionMediaAddedEventHandler m_MediaCollectionMediaAddedDelegate;

	public _WMPOCXEvents_MediaCollectionMediaRemovedEventHandler m_MediaCollectionMediaRemovedDelegate;

	public int m_dwCookie;

	public void OpenStateChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OpenStateChangeDelegate != null)
		{
			m_OpenStateChangeDelegate(P_0);
		}
	}

	public void PlayStateChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlayStateChangeDelegate != null)
		{
			m_PlayStateChangeDelegate(P_0);
		}
	}

	public void AudioLanguageChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_AudioLanguageChangeDelegate != null)
		{
			m_AudioLanguageChangeDelegate(P_0);
		}
	}

	public void StatusChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_StatusChangeDelegate != null)
		{
			m_StatusChangeDelegate();
		}
	}

	public void ScriptCommand(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ScriptCommandDelegate != null)
		{
			m_ScriptCommandDelegate(P_0, P_1);
		}
	}

	public void NewStream()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_NewStreamDelegate != null)
		{
			m_NewStreamDelegate();
		}
	}

	public void Disconnect(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DisconnectDelegate != null)
		{
			m_DisconnectDelegate(P_0);
		}
	}

	public void Buffering(bool P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_BufferingDelegate != null)
		{
			m_BufferingDelegate(P_0);
		}
	}

	public void Error()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ErrorDelegate != null)
		{
			m_ErrorDelegate();
		}
	}

	public void Warning(int P_0, int P_1, string P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_WarningDelegate != null)
		{
			m_WarningDelegate(P_0, P_1, P_2);
		}
	}

	public void EndOfStream(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_EndOfStreamDelegate != null)
		{
			m_EndOfStreamDelegate(P_0);
		}
	}

	public void PositionChange(double P_0, double P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PositionChangeDelegate != null)
		{
			m_PositionChangeDelegate(P_0, P_1);
		}
	}

	public void MarkerHit(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MarkerHitDelegate != null)
		{
			m_MarkerHitDelegate(P_0);
		}
	}

	public void DurationUnitChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DurationUnitChangeDelegate != null)
		{
			m_DurationUnitChangeDelegate(P_0);
		}
	}

	public void CdromMediaChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromMediaChangeDelegate != null)
		{
			m_CdromMediaChangeDelegate(P_0);
		}
	}

	public void PlaylistChange(object P_0, WMPPlaylistChangeEventType P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistChangeDelegate != null)
		{
			m_PlaylistChangeDelegate(P_0, P_1);
		}
	}

	public void CurrentPlaylistChange(WMPPlaylistChangeEventType P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentPlaylistChangeDelegate != null)
		{
			m_CurrentPlaylistChangeDelegate(P_0);
		}
	}

	public void CurrentPlaylistItemAvailable(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentPlaylistItemAvailableDelegate != null)
		{
			m_CurrentPlaylistItemAvailableDelegate(P_0);
		}
	}

	public void MediaChange(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaChangeDelegate != null)
		{
			m_MediaChangeDelegate(P_0);
		}
	}

	public void CurrentMediaItemAvailable(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentMediaItemAvailableDelegate != null)
		{
			m_CurrentMediaItemAvailableDelegate(P_0);
		}
	}

	public void CurrentItemChange(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentItemChangeDelegate != null)
		{
			m_CurrentItemChangeDelegate(P_0);
		}
	}

	public void MediaCollectionChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionChangeDelegate != null)
		{
			m_MediaCollectionChangeDelegate();
		}
	}

	public void MediaCollectionAttributeStringAdded(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringAddedDelegate != null)
		{
			m_MediaCollectionAttributeStringAddedDelegate(P_0, P_1);
		}
	}

	public void MediaCollectionAttributeStringRemoved(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringRemovedDelegate != null)
		{
			m_MediaCollectionAttributeStringRemovedDelegate(P_0, P_1);
		}
	}

	public void MediaCollectionAttributeStringChanged(string P_0, string P_1, string P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringChangedDelegate != null)
		{
			m_MediaCollectionAttributeStringChangedDelegate(P_0, P_1, P_2);
		}
	}

	public void PlaylistCollectionChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionChangeDelegate != null)
		{
			m_PlaylistCollectionChangeDelegate();
		}
	}

	public void PlaylistCollectionPlaylistAdded(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistAddedDelegate != null)
		{
			m_PlaylistCollectionPlaylistAddedDelegate(P_0);
		}
	}

	public void PlaylistCollectionPlaylistRemoved(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistRemovedDelegate != null)
		{
			m_PlaylistCollectionPlaylistRemovedDelegate(P_0);
		}
	}

	public void PlaylistCollectionPlaylistSetAsDeleted(string P_0, bool P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistSetAsDeletedDelegate != null)
		{
			m_PlaylistCollectionPlaylistSetAsDeletedDelegate(P_0, P_1);
		}
	}

	public void ModeChange(string P_0, bool P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ModeChangeDelegate != null)
		{
			m_ModeChangeDelegate(P_0, P_1);
		}
	}

	public void MediaError(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaErrorDelegate != null)
		{
			m_MediaErrorDelegate(P_0);
		}
	}

	public void OpenPlaylistSwitch(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OpenPlaylistSwitchDelegate != null)
		{
			m_OpenPlaylistSwitchDelegate(P_0);
		}
	}

	public void DomainChange(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DomainChangeDelegate != null)
		{
			m_DomainChangeDelegate(P_0);
		}
	}

	public void SwitchedToPlayerApplication()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_SwitchedToPlayerApplicationDelegate != null)
		{
			m_SwitchedToPlayerApplicationDelegate();
		}
	}

	public void SwitchedToControl()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_SwitchedToControlDelegate != null)
		{
			m_SwitchedToControlDelegate();
		}
	}

	public void PlayerDockedStateChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlayerDockedStateChangeDelegate != null)
		{
			m_PlayerDockedStateChangeDelegate();
		}
	}

	public void PlayerReconnect()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlayerReconnectDelegate != null)
		{
			m_PlayerReconnectDelegate();
		}
	}

	public void Click(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ClickDelegate != null)
		{
			m_ClickDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void DoubleClick(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DoubleClickDelegate != null)
		{
			m_DoubleClickDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void KeyDown(short P_0, short P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyDownDelegate != null)
		{
			m_KeyDownDelegate(P_0, P_1);
		}
	}

	public void KeyPress(short P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyPressDelegate != null)
		{
			m_KeyPressDelegate(P_0);
		}
	}

	public void KeyUp(short P_0, short P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_KeyUpDelegate != null)
		{
			m_KeyUpDelegate(P_0, P_1);
		}
	}

	public void MouseDown(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseDownDelegate != null)
		{
			m_MouseDownDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void MouseMove(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseMoveDelegate != null)
		{
			m_MouseMoveDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void MouseUp(short P_0, short P_1, int P_2, int P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MouseUpDelegate != null)
		{
			m_MouseUpDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void DeviceConnect(IWMPSyncDevice P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceConnectDelegate != null)
		{
			m_DeviceConnectDelegate(P_0);
		}
	}

	public void DeviceDisconnect(IWMPSyncDevice P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceDisconnectDelegate != null)
		{
			m_DeviceDisconnectDelegate(P_0);
		}
	}

	public void DeviceStatusChange(IWMPSyncDevice P_0, WMPDeviceStatus P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceStatusChangeDelegate != null)
		{
			m_DeviceStatusChangeDelegate(P_0, P_1);
		}
	}

	public void DeviceSyncStateChange(IWMPSyncDevice P_0, WMPSyncState P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceSyncStateChangeDelegate != null)
		{
			m_DeviceSyncStateChangeDelegate(P_0, P_1);
		}
	}

	public void DeviceSyncError(IWMPSyncDevice P_0, object P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceSyncErrorDelegate != null)
		{
			m_DeviceSyncErrorDelegate(P_0, P_1);
		}
	}

	public void CreatePartnershipComplete(IWMPSyncDevice P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CreatePartnershipCompleteDelegate != null)
		{
			m_CreatePartnershipCompleteDelegate(P_0, P_1);
		}
	}

	public void DeviceEstimation(IWMPSyncDevice P_0, int P_1, long P_2, long P_3)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DeviceEstimationDelegate != null)
		{
			m_DeviceEstimationDelegate(P_0, P_1, P_2, P_3);
		}
	}

	public void CdromRipStateChange(IWMPCdromRip P_0, WMPRipState P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromRipStateChangeDelegate != null)
		{
			m_CdromRipStateChangeDelegate(P_0, P_1);
		}
	}

	public void CdromRipMediaError(IWMPCdromRip P_0, object P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromRipMediaErrorDelegate != null)
		{
			m_CdromRipMediaErrorDelegate(P_0, P_1);
		}
	}

	public void CdromBurnStateChange(IWMPCdromBurn P_0, WMPBurnState P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromBurnStateChangeDelegate != null)
		{
			m_CdromBurnStateChangeDelegate(P_0, P_1);
		}
	}

	public void CdromBurnMediaError(IWMPCdromBurn P_0, object P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromBurnMediaErrorDelegate != null)
		{
			m_CdromBurnMediaErrorDelegate(P_0, P_1);
		}
	}

	public void CdromBurnError(IWMPCdromBurn P_0, int P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromBurnErrorDelegate != null)
		{
			m_CdromBurnErrorDelegate(P_0, P_1);
		}
	}

	public void LibraryConnect(IWMPLibrary P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_LibraryConnectDelegate != null)
		{
			m_LibraryConnectDelegate(P_0);
		}
	}

	public void LibraryDisconnect(IWMPLibrary P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_LibraryDisconnectDelegate != null)
		{
			m_LibraryDisconnectDelegate(P_0);
		}
	}

	public void FolderScanStateChange(WMPFolderScanState P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_FolderScanStateChangeDelegate != null)
		{
			m_FolderScanStateChangeDelegate(P_0);
		}
	}

	public void StringCollectionChange(object P_0, WMPStringCollectionChangeEventType P_1, int P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_StringCollectionChangeDelegate != null)
		{
			m_StringCollectionChangeDelegate(P_0, P_1, P_2);
		}
	}

	public void MediaCollectionMediaAdded(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionMediaAddedDelegate != null)
		{
			m_MediaCollectionMediaAddedDelegate(P_0);
		}
	}

	public void MediaCollectionMediaRemoved(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionMediaRemovedDelegate != null)
		{
			m_MediaCollectionMediaRemovedDelegate(P_0);
		}
	}

	internal _WMPOCXEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OpenStateChangeDelegate = null;
		m_PlayStateChangeDelegate = null;
		m_AudioLanguageChangeDelegate = null;
		m_StatusChangeDelegate = null;
		m_ScriptCommandDelegate = null;
		m_NewStreamDelegate = null;
		m_DisconnectDelegate = null;
		m_BufferingDelegate = null;
		m_ErrorDelegate = null;
		m_WarningDelegate = null;
		m_EndOfStreamDelegate = null;
		m_PositionChangeDelegate = null;
		m_MarkerHitDelegate = null;
		m_DurationUnitChangeDelegate = null;
		m_CdromMediaChangeDelegate = null;
		m_PlaylistChangeDelegate = null;
		m_CurrentPlaylistChangeDelegate = null;
		m_CurrentPlaylistItemAvailableDelegate = null;
		m_MediaChangeDelegate = null;
		m_CurrentMediaItemAvailableDelegate = null;
		m_CurrentItemChangeDelegate = null;
		m_MediaCollectionChangeDelegate = null;
		m_MediaCollectionAttributeStringAddedDelegate = null;
		m_MediaCollectionAttributeStringRemovedDelegate = null;
		m_MediaCollectionAttributeStringChangedDelegate = null;
		m_PlaylistCollectionChangeDelegate = null;
		m_PlaylistCollectionPlaylistAddedDelegate = null;
		m_PlaylistCollectionPlaylistRemovedDelegate = null;
		m_PlaylistCollectionPlaylistSetAsDeletedDelegate = null;
		m_ModeChangeDelegate = null;
		m_MediaErrorDelegate = null;
		m_OpenPlaylistSwitchDelegate = null;
		m_DomainChangeDelegate = null;
		m_SwitchedToPlayerApplicationDelegate = null;
		m_SwitchedToControlDelegate = null;
		m_PlayerDockedStateChangeDelegate = null;
		m_PlayerReconnectDelegate = null;
		m_ClickDelegate = null;
		m_DoubleClickDelegate = null;
		m_KeyDownDelegate = null;
		m_KeyPressDelegate = null;
		m_KeyUpDelegate = null;
		m_MouseDownDelegate = null;
		m_MouseMoveDelegate = null;
		m_MouseUpDelegate = null;
		m_DeviceConnectDelegate = null;
		m_DeviceDisconnectDelegate = null;
		m_DeviceStatusChangeDelegate = null;
		m_DeviceSyncStateChangeDelegate = null;
		m_DeviceSyncErrorDelegate = null;
		m_CreatePartnershipCompleteDelegate = null;
		m_DeviceEstimationDelegate = null;
		m_CdromRipStateChangeDelegate = null;
		m_CdromRipMediaErrorDelegate = null;
		m_CdromBurnStateChangeDelegate = null;
		m_CdromBurnMediaErrorDelegate = null;
		m_CdromBurnErrorDelegate = null;
		m_LibraryConnectDelegate = null;
		m_LibraryDisconnectDelegate = null;
		m_FolderScanStateChangeDelegate = null;
		m_StringCollectionChangeDelegate = null;
		m_MediaCollectionMediaAddedDelegate = null;
		m_MediaCollectionMediaRemovedDelegate = null;
	}
}
internal sealed class _WMPOCXEvents_EventProvider : _WMPOCXEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			81, 42, 245, 107, 74, 57, 211, 17, 177, 83,
			0, 192, 79, 121, 250, 166
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OpenStateChange(_WMPOCXEvents_OpenStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_OpenStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OpenStateChange(_WMPOCXEvents_OpenStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_OpenStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_OpenStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlayStateChange(_WMPOCXEvents_PlayStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlayStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlayStateChange(_WMPOCXEvents_PlayStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlayStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlayStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_AudioLanguageChange(_WMPOCXEvents_AudioLanguageChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_AudioLanguageChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_AudioLanguageChange(_WMPOCXEvents_AudioLanguageChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_AudioLanguageChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_AudioLanguageChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_StatusChange(_WMPOCXEvents_StatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_StatusChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_StatusChange(_WMPOCXEvents_StatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_StatusChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_StatusChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ScriptCommand(_WMPOCXEvents_ScriptCommandEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_ScriptCommandDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ScriptCommand(_WMPOCXEvents_ScriptCommandEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_ScriptCommandDelegate != null && ((wMPOCXEvents_SinkHelper.m_ScriptCommandDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_NewStream(_WMPOCXEvents_NewStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_NewStreamDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_NewStream(_WMPOCXEvents_NewStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_NewStreamDelegate != null && ((wMPOCXEvents_SinkHelper.m_NewStreamDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Disconnect(_WMPOCXEvents_DisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DisconnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Disconnect(_WMPOCXEvents_DisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DisconnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_DisconnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Buffering(_WMPOCXEvents_BufferingEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_BufferingDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Buffering(_WMPOCXEvents_BufferingEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_BufferingDelegate != null && ((wMPOCXEvents_SinkHelper.m_BufferingDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Error(_WMPOCXEvents_ErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_ErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Error(_WMPOCXEvents_ErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_ErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_ErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Warning(_WMPOCXEvents_WarningEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_WarningDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Warning(_WMPOCXEvents_WarningEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_WarningDelegate != null && ((wMPOCXEvents_SinkHelper.m_WarningDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_EndOfStream(_WMPOCXEvents_EndOfStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_EndOfStreamDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_EndOfStream(_WMPOCXEvents_EndOfStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_EndOfStreamDelegate != null && ((wMPOCXEvents_SinkHelper.m_EndOfStreamDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PositionChange(_WMPOCXEvents_PositionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PositionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PositionChange(_WMPOCXEvents_PositionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PositionChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_PositionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MarkerHit(_WMPOCXEvents_MarkerHitEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MarkerHitDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MarkerHit(_WMPOCXEvents_MarkerHitEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MarkerHitDelegate != null && ((wMPOCXEvents_SinkHelper.m_MarkerHitDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DurationUnitChange(_WMPOCXEvents_DurationUnitChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DurationUnitChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DurationUnitChange(_WMPOCXEvents_DurationUnitChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DurationUnitChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_DurationUnitChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromMediaChange(_WMPOCXEvents_CdromMediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromMediaChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromMediaChange(_WMPOCXEvents_CdromMediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromMediaChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromMediaChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistChange(_WMPOCXEvents_PlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlaylistChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistChange(_WMPOCXEvents_PlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlaylistChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlaylistChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentPlaylistChange(_WMPOCXEvents_CurrentPlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CurrentPlaylistChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentPlaylistChange(_WMPOCXEvents_CurrentPlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CurrentPlaylistChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_CurrentPlaylistChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentPlaylistItemAvailable(_WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentPlaylistItemAvailable(_WMPOCXEvents_CurrentPlaylistItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate != null && ((wMPOCXEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaChange(_WMPOCXEvents_MediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaChange(_WMPOCXEvents_MediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentMediaItemAvailable(_WMPOCXEvents_CurrentMediaItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentMediaItemAvailable(_WMPOCXEvents_CurrentMediaItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate != null && ((wMPOCXEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentItemChange(_WMPOCXEvents_CurrentItemChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CurrentItemChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentItemChange(_WMPOCXEvents_CurrentItemChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CurrentItemChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_CurrentItemChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionChange(_WMPOCXEvents_MediaCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionChange(_WMPOCXEvents_MediaCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringAdded(_WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringAdded(_WMPOCXEvents_MediaCollectionAttributeStringAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringRemoved(_WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringRemoved(_WMPOCXEvents_MediaCollectionAttributeStringRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringChanged(_WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringChanged(_WMPOCXEvents_MediaCollectionAttributeStringChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionChange(_WMPOCXEvents_PlaylistCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlaylistCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionChange(_WMPOCXEvents_PlaylistCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlaylistCollectionChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlaylistCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistAdded(_WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistAdded(_WMPOCXEvents_PlaylistCollectionPlaylistAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistRemoved(_WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistRemoved(_WMPOCXEvents_PlaylistCollectionPlaylistRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistSetAsDeleted(_WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistSetAsDeleted(_WMPOCXEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ModeChange(_WMPOCXEvents_ModeChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_ModeChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ModeChange(_WMPOCXEvents_ModeChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_ModeChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_ModeChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaError(_WMPOCXEvents_MediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaError(_WMPOCXEvents_MediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OpenPlaylistSwitch(_WMPOCXEvents_OpenPlaylistSwitchEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_OpenPlaylistSwitchDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OpenPlaylistSwitch(_WMPOCXEvents_OpenPlaylistSwitchEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_OpenPlaylistSwitchDelegate != null && ((wMPOCXEvents_SinkHelper.m_OpenPlaylistSwitchDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DomainChange(_WMPOCXEvents_DomainChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DomainChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DomainChange(_WMPOCXEvents_DomainChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DomainChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_DomainChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_SwitchedToPlayerApplication(_WMPOCXEvents_SwitchedToPlayerApplicationEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_SwitchedToPlayerApplicationDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_SwitchedToPlayerApplication(_WMPOCXEvents_SwitchedToPlayerApplicationEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_SwitchedToPlayerApplicationDelegate != null && ((wMPOCXEvents_SinkHelper.m_SwitchedToPlayerApplicationDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_SwitchedToControl(_WMPOCXEvents_SwitchedToControlEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_SwitchedToControlDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_SwitchedToControl(_WMPOCXEvents_SwitchedToControlEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_SwitchedToControlDelegate != null && ((wMPOCXEvents_SinkHelper.m_SwitchedToControlDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlayerDockedStateChange(_WMPOCXEvents_PlayerDockedStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlayerDockedStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlayerDockedStateChange(_WMPOCXEvents_PlayerDockedStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlayerDockedStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlayerDockedStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlayerReconnect(_WMPOCXEvents_PlayerReconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_PlayerReconnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlayerReconnect(_WMPOCXEvents_PlayerReconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_PlayerReconnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_PlayerReconnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Click(_WMPOCXEvents_ClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_ClickDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Click(_WMPOCXEvents_ClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_ClickDelegate != null && ((wMPOCXEvents_SinkHelper.m_ClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DoubleClick(_WMPOCXEvents_DoubleClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DoubleClickDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DoubleClick(_WMPOCXEvents_DoubleClickEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DoubleClickDelegate != null && ((wMPOCXEvents_SinkHelper.m_DoubleClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_KeyDown(_WMPOCXEvents_KeyDownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_KeyDownDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_KeyDown(_WMPOCXEvents_KeyDownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_KeyDownDelegate != null && ((wMPOCXEvents_SinkHelper.m_KeyDownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_KeyPress(_WMPOCXEvents_KeyPressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_KeyPressDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_KeyPress(_WMPOCXEvents_KeyPressEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_KeyPressDelegate != null && ((wMPOCXEvents_SinkHelper.m_KeyPressDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_KeyUp(_WMPOCXEvents_KeyUpEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_KeyUpDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_KeyUp(_WMPOCXEvents_KeyUpEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_KeyUpDelegate != null && ((wMPOCXEvents_SinkHelper.m_KeyUpDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MouseDown(_WMPOCXEvents_MouseDownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MouseDownDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MouseDown(_WMPOCXEvents_MouseDownEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MouseDownDelegate != null && ((wMPOCXEvents_SinkHelper.m_MouseDownDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MouseMove(_WMPOCXEvents_MouseMoveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MouseMoveDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MouseMove(_WMPOCXEvents_MouseMoveEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MouseMoveDelegate != null && ((wMPOCXEvents_SinkHelper.m_MouseMoveDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MouseUp(_WMPOCXEvents_MouseUpEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MouseUpDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MouseUp(_WMPOCXEvents_MouseUpEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MouseUpDelegate != null && ((wMPOCXEvents_SinkHelper.m_MouseUpDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceConnect(_WMPOCXEvents_DeviceConnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceConnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceConnect(_WMPOCXEvents_DeviceConnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceConnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceConnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceDisconnect(_WMPOCXEvents_DeviceDisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceDisconnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceDisconnect(_WMPOCXEvents_DeviceDisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceDisconnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceDisconnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceStatusChange(_WMPOCXEvents_DeviceStatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceStatusChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceStatusChange(_WMPOCXEvents_DeviceStatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceStatusChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceStatusChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceSyncStateChange(_WMPOCXEvents_DeviceSyncStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceSyncStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceSyncStateChange(_WMPOCXEvents_DeviceSyncStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceSyncStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceSyncStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceSyncError(_WMPOCXEvents_DeviceSyncErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceSyncErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceSyncError(_WMPOCXEvents_DeviceSyncErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceSyncErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceSyncErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CreatePartnershipComplete(_WMPOCXEvents_CreatePartnershipCompleteEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CreatePartnershipCompleteDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CreatePartnershipComplete(_WMPOCXEvents_CreatePartnershipCompleteEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CreatePartnershipCompleteDelegate != null && ((wMPOCXEvents_SinkHelper.m_CreatePartnershipCompleteDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DeviceEstimation(_WMPOCXEvents_DeviceEstimationEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_DeviceEstimationDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DeviceEstimation(_WMPOCXEvents_DeviceEstimationEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_DeviceEstimationDelegate != null && ((wMPOCXEvents_SinkHelper.m_DeviceEstimationDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromRipStateChange(_WMPOCXEvents_CdromRipStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromRipStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromRipStateChange(_WMPOCXEvents_CdromRipStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromRipStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromRipStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromRipMediaError(_WMPOCXEvents_CdromRipMediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromRipMediaErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromRipMediaError(_WMPOCXEvents_CdromRipMediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromRipMediaErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromRipMediaErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromBurnStateChange(_WMPOCXEvents_CdromBurnStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromBurnStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromBurnStateChange(_WMPOCXEvents_CdromBurnStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromBurnStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromBurnStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromBurnMediaError(_WMPOCXEvents_CdromBurnMediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromBurnMediaErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromBurnMediaError(_WMPOCXEvents_CdromBurnMediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromBurnMediaErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromBurnMediaErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromBurnError(_WMPOCXEvents_CdromBurnErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_CdromBurnErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromBurnError(_WMPOCXEvents_CdromBurnErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_CdromBurnErrorDelegate != null && ((wMPOCXEvents_SinkHelper.m_CdromBurnErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_LibraryConnect(_WMPOCXEvents_LibraryConnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_LibraryConnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_LibraryConnect(_WMPOCXEvents_LibraryConnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_LibraryConnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_LibraryConnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_LibraryDisconnect(_WMPOCXEvents_LibraryDisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_LibraryDisconnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_LibraryDisconnect(_WMPOCXEvents_LibraryDisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_LibraryDisconnectDelegate != null && ((wMPOCXEvents_SinkHelper.m_LibraryDisconnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_FolderScanStateChange(_WMPOCXEvents_FolderScanStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_FolderScanStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_FolderScanStateChange(_WMPOCXEvents_FolderScanStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_FolderScanStateChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_FolderScanStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_StringCollectionChange(_WMPOCXEvents_StringCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_StringCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_StringCollectionChange(_WMPOCXEvents_StringCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_StringCollectionChangeDelegate != null && ((wMPOCXEvents_SinkHelper.m_StringCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionMediaAdded(_WMPOCXEvents_MediaCollectionMediaAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionMediaAdded(_WMPOCXEvents_MediaCollectionMediaAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionMediaRemoved(_WMPOCXEvents_MediaCollectionMediaRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = new _WMPOCXEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPOCXEvents_SinkHelper, out pdwCookie);
			wMPOCXEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPOCXEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPOCXEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionMediaRemoved(_WMPOCXEvents_MediaCollectionMediaRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPOCXEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate != null && ((wMPOCXEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public _WMPOCXEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					_WMPOCXEvents_SinkHelper wMPOCXEvents_SinkHelper = (_WMPOCXEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(wMPOCXEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[TypeLibType(TypeLibTypeFlags.FHidden)]
[ClassInterface(ClassInterfaceType.None)]
public sealed class IWMPCustomSliderCtrlEvents_SinkHelper : IWMPCustomSliderCtrlEvents
{
	public IWMPCustomSliderCtrlEvents_ondragbeginEventHandler m_ondragbeginDelegate;

	public IWMPCustomSliderCtrlEvents_ondragendEventHandler m_ondragendDelegate;

	public IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler m_onpositionchangeDelegate;

	public int m_dwCookie;

	public void ondragbegin()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ondragbeginDelegate != null)
		{
			m_ondragbeginDelegate();
		}
	}

	public void ondragend()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ondragendDelegate != null)
		{
			m_ondragendDelegate();
		}
	}

	public void onpositionchange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_onpositionchangeDelegate != null)
		{
			m_onpositionchangeDelegate();
		}
	}

	internal IWMPCustomSliderCtrlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ondragbeginDelegate = null;
		m_ondragendDelegate = null;
		m_onpositionchangeDelegate = null;
	}
}
internal sealed class IWMPCustomSliderCtrlEvents_EventProvider : IWMPCustomSliderCtrlEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			164, 90, 244, 149, 10, 237, 210, 17, 186, 103,
			0, 0, 248, 8, 85, 230
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_ondragbegin(IWMPCustomSliderCtrlEvents_ondragbeginEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = new IWMPCustomSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPCustomSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPCustomSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ondragbegin(IWMPCustomSliderCtrlEvents_ondragbeginEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = (IWMPCustomSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate != null && ((iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragbeginDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ondragend(IWMPCustomSliderCtrlEvents_ondragendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = new IWMPCustomSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPCustomSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragendDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPCustomSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ondragend(IWMPCustomSliderCtrlEvents_ondragendEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = (IWMPCustomSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragendDelegate != null && ((iWMPCustomSliderCtrlEvents_SinkHelper.m_ondragendDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_onpositionchange(IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = new IWMPCustomSliderCtrlEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(iWMPCustomSliderCtrlEvents_SinkHelper, out pdwCookie);
			iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie = pdwCookie;
			iWMPCustomSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate = P_0;
			m_aEventSinkHelpers.Add(iWMPCustomSliderCtrlEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_onpositionchange(IWMPCustomSliderCtrlEvents_onpositionchangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = (IWMPCustomSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (iWMPCustomSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate != null && ((iWMPCustomSliderCtrlEvents_SinkHelper.m_onpositionchangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public IWMPCustomSliderCtrlEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					IWMPCustomSliderCtrlEvents_SinkHelper iWMPCustomSliderCtrlEvents_SinkHelper = (IWMPCustomSliderCtrlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(iWMPCustomSliderCtrlEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}
[ClassInterface(ClassInterfaceType.None)]
[TypeLibType(TypeLibTypeFlags.FHidden)]
public sealed class _WMPCoreEvents_SinkHelper : _WMPCoreEvents
{
	public _WMPCoreEvents_OpenStateChangeEventHandler m_OpenStateChangeDelegate;

	public _WMPCoreEvents_PlayStateChangeEventHandler m_PlayStateChangeDelegate;

	public _WMPCoreEvents_AudioLanguageChangeEventHandler m_AudioLanguageChangeDelegate;

	public _WMPCoreEvents_StatusChangeEventHandler m_StatusChangeDelegate;

	public _WMPCoreEvents_ScriptCommandEventHandler m_ScriptCommandDelegate;

	public _WMPCoreEvents_NewStreamEventHandler m_NewStreamDelegate;

	public _WMPCoreEvents_DisconnectEventHandler m_DisconnectDelegate;

	public _WMPCoreEvents_BufferingEventHandler m_BufferingDelegate;

	public _WMPCoreEvents_ErrorEventHandler m_ErrorDelegate;

	public _WMPCoreEvents_WarningEventHandler m_WarningDelegate;

	public _WMPCoreEvents_EndOfStreamEventHandler m_EndOfStreamDelegate;

	public _WMPCoreEvents_PositionChangeEventHandler m_PositionChangeDelegate;

	public _WMPCoreEvents_MarkerHitEventHandler m_MarkerHitDelegate;

	public _WMPCoreEvents_DurationUnitChangeEventHandler m_DurationUnitChangeDelegate;

	public _WMPCoreEvents_CdromMediaChangeEventHandler m_CdromMediaChangeDelegate;

	public _WMPCoreEvents_PlaylistChangeEventHandler m_PlaylistChangeDelegate;

	public _WMPCoreEvents_CurrentPlaylistChangeEventHandler m_CurrentPlaylistChangeDelegate;

	public _WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler m_CurrentPlaylistItemAvailableDelegate;

	public _WMPCoreEvents_MediaChangeEventHandler m_MediaChangeDelegate;

	public _WMPCoreEvents_CurrentMediaItemAvailableEventHandler m_CurrentMediaItemAvailableDelegate;

	public _WMPCoreEvents_CurrentItemChangeEventHandler m_CurrentItemChangeDelegate;

	public _WMPCoreEvents_MediaCollectionChangeEventHandler m_MediaCollectionChangeDelegate;

	public _WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler m_MediaCollectionAttributeStringAddedDelegate;

	public _WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler m_MediaCollectionAttributeStringRemovedDelegate;

	public _WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler m_MediaCollectionAttributeStringChangedDelegate;

	public _WMPCoreEvents_PlaylistCollectionChangeEventHandler m_PlaylistCollectionChangeDelegate;

	public _WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler m_PlaylistCollectionPlaylistAddedDelegate;

	public _WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler m_PlaylistCollectionPlaylistRemovedDelegate;

	public _WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler m_PlaylistCollectionPlaylistSetAsDeletedDelegate;

	public _WMPCoreEvents_ModeChangeEventHandler m_ModeChangeDelegate;

	public _WMPCoreEvents_MediaErrorEventHandler m_MediaErrorDelegate;

	public _WMPCoreEvents_OpenPlaylistSwitchEventHandler m_OpenPlaylistSwitchDelegate;

	public _WMPCoreEvents_DomainChangeEventHandler m_DomainChangeDelegate;

	public _WMPCoreEvents_StringCollectionChangeEventHandler m_StringCollectionChangeDelegate;

	public _WMPCoreEvents_MediaCollectionMediaAddedEventHandler m_MediaCollectionMediaAddedDelegate;

	public _WMPCoreEvents_MediaCollectionMediaRemovedEventHandler m_MediaCollectionMediaRemovedDelegate;

	public int m_dwCookie;

	public void OpenStateChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OpenStateChangeDelegate != null)
		{
			m_OpenStateChangeDelegate(P_0);
		}
	}

	public void PlayStateChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlayStateChangeDelegate != null)
		{
			m_PlayStateChangeDelegate(P_0);
		}
	}

	public void AudioLanguageChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_AudioLanguageChangeDelegate != null)
		{
			m_AudioLanguageChangeDelegate(P_0);
		}
	}

	public void StatusChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_StatusChangeDelegate != null)
		{
			m_StatusChangeDelegate();
		}
	}

	public void ScriptCommand(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ScriptCommandDelegate != null)
		{
			m_ScriptCommandDelegate(P_0, P_1);
		}
	}

	public void NewStream()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_NewStreamDelegate != null)
		{
			m_NewStreamDelegate();
		}
	}

	public void Disconnect(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DisconnectDelegate != null)
		{
			m_DisconnectDelegate(P_0);
		}
	}

	public void Buffering(bool P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_BufferingDelegate != null)
		{
			m_BufferingDelegate(P_0);
		}
	}

	public void Error()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ErrorDelegate != null)
		{
			m_ErrorDelegate();
		}
	}

	public void Warning(int P_0, int P_1, string P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_WarningDelegate != null)
		{
			m_WarningDelegate(P_0, P_1, P_2);
		}
	}

	public void EndOfStream(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_EndOfStreamDelegate != null)
		{
			m_EndOfStreamDelegate(P_0);
		}
	}

	public void PositionChange(double P_0, double P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PositionChangeDelegate != null)
		{
			m_PositionChangeDelegate(P_0, P_1);
		}
	}

	public void MarkerHit(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MarkerHitDelegate != null)
		{
			m_MarkerHitDelegate(P_0);
		}
	}

	public void DurationUnitChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DurationUnitChangeDelegate != null)
		{
			m_DurationUnitChangeDelegate(P_0);
		}
	}

	public void CdromMediaChange(int P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CdromMediaChangeDelegate != null)
		{
			m_CdromMediaChangeDelegate(P_0);
		}
	}

	public void PlaylistChange(object P_0, WMPPlaylistChangeEventType P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistChangeDelegate != null)
		{
			m_PlaylistChangeDelegate(P_0, P_1);
		}
	}

	public void CurrentPlaylistChange(WMPPlaylistChangeEventType P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentPlaylistChangeDelegate != null)
		{
			m_CurrentPlaylistChangeDelegate(P_0);
		}
	}

	public void CurrentPlaylistItemAvailable(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentPlaylistItemAvailableDelegate != null)
		{
			m_CurrentPlaylistItemAvailableDelegate(P_0);
		}
	}

	public void MediaChange(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaChangeDelegate != null)
		{
			m_MediaChangeDelegate(P_0);
		}
	}

	public void CurrentMediaItemAvailable(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentMediaItemAvailableDelegate != null)
		{
			m_CurrentMediaItemAvailableDelegate(P_0);
		}
	}

	public void CurrentItemChange(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_CurrentItemChangeDelegate != null)
		{
			m_CurrentItemChangeDelegate(P_0);
		}
	}

	public void MediaCollectionChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionChangeDelegate != null)
		{
			m_MediaCollectionChangeDelegate();
		}
	}

	public void MediaCollectionAttributeStringAdded(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringAddedDelegate != null)
		{
			m_MediaCollectionAttributeStringAddedDelegate(P_0, P_1);
		}
	}

	public void MediaCollectionAttributeStringRemoved(string P_0, string P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringRemovedDelegate != null)
		{
			m_MediaCollectionAttributeStringRemovedDelegate(P_0, P_1);
		}
	}

	public void MediaCollectionAttributeStringChanged(string P_0, string P_1, string P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionAttributeStringChangedDelegate != null)
		{
			m_MediaCollectionAttributeStringChangedDelegate(P_0, P_1, P_2);
		}
	}

	public void PlaylistCollectionChange()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionChangeDelegate != null)
		{
			m_PlaylistCollectionChangeDelegate();
		}
	}

	public void PlaylistCollectionPlaylistAdded(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistAddedDelegate != null)
		{
			m_PlaylistCollectionPlaylistAddedDelegate(P_0);
		}
	}

	public void PlaylistCollectionPlaylistRemoved(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistRemovedDelegate != null)
		{
			m_PlaylistCollectionPlaylistRemovedDelegate(P_0);
		}
	}

	public void PlaylistCollectionPlaylistSetAsDeleted(string P_0, bool P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_PlaylistCollectionPlaylistSetAsDeletedDelegate != null)
		{
			m_PlaylistCollectionPlaylistSetAsDeletedDelegate(P_0, P_1);
		}
	}

	public void ModeChange(string P_0, bool P_1)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ModeChangeDelegate != null)
		{
			m_ModeChangeDelegate(P_0, P_1);
		}
	}

	public void MediaError(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaErrorDelegate != null)
		{
			m_MediaErrorDelegate(P_0);
		}
	}

	public void OpenPlaylistSwitch(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_OpenPlaylistSwitchDelegate != null)
		{
			m_OpenPlaylistSwitchDelegate(P_0);
		}
	}

	public void DomainChange(string P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_DomainChangeDelegate != null)
		{
			m_DomainChangeDelegate(P_0);
		}
	}

	public void StringCollectionChange(object P_0, WMPStringCollectionChangeEventType P_1, int P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_StringCollectionChangeDelegate != null)
		{
			m_StringCollectionChangeDelegate(P_0, P_1, P_2);
		}
	}

	public void MediaCollectionMediaAdded(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionMediaAddedDelegate != null)
		{
			m_MediaCollectionMediaAddedDelegate(P_0);
		}
	}

	public void MediaCollectionMediaRemoved(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_MediaCollectionMediaRemovedDelegate != null)
		{
			m_MediaCollectionMediaRemovedDelegate(P_0);
		}
	}

	internal _WMPCoreEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_OpenStateChangeDelegate = null;
		m_PlayStateChangeDelegate = null;
		m_AudioLanguageChangeDelegate = null;
		m_StatusChangeDelegate = null;
		m_ScriptCommandDelegate = null;
		m_NewStreamDelegate = null;
		m_DisconnectDelegate = null;
		m_BufferingDelegate = null;
		m_ErrorDelegate = null;
		m_WarningDelegate = null;
		m_EndOfStreamDelegate = null;
		m_PositionChangeDelegate = null;
		m_MarkerHitDelegate = null;
		m_DurationUnitChangeDelegate = null;
		m_CdromMediaChangeDelegate = null;
		m_PlaylistChangeDelegate = null;
		m_CurrentPlaylistChangeDelegate = null;
		m_CurrentPlaylistItemAvailableDelegate = null;
		m_MediaChangeDelegate = null;
		m_CurrentMediaItemAvailableDelegate = null;
		m_CurrentItemChangeDelegate = null;
		m_MediaCollectionChangeDelegate = null;
		m_MediaCollectionAttributeStringAddedDelegate = null;
		m_MediaCollectionAttributeStringRemovedDelegate = null;
		m_MediaCollectionAttributeStringChangedDelegate = null;
		m_PlaylistCollectionChangeDelegate = null;
		m_PlaylistCollectionPlaylistAddedDelegate = null;
		m_PlaylistCollectionPlaylistRemovedDelegate = null;
		m_PlaylistCollectionPlaylistSetAsDeletedDelegate = null;
		m_ModeChangeDelegate = null;
		m_MediaErrorDelegate = null;
		m_OpenPlaylistSwitchDelegate = null;
		m_DomainChangeDelegate = null;
		m_StringCollectionChangeDelegate = null;
		m_MediaCollectionMediaAddedDelegate = null;
		m_MediaCollectionMediaRemovedDelegate = null;
	}
}
internal sealed class _WMPCoreEvents_EventProvider : _WMPCoreEvents_Event, IDisposable
{
	private IConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private IConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		IConnectionPoint ppCP = null;
		Guid riid = new Guid(new byte[16]
		{
			150, 202, 76, 216, 226, 204, 210, 17, 158, 204,
			0, 0, 248, 8, 89, 129
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref riid, out ppCP);
		m_ConnectionPoint = ppCP;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_OpenStateChange(_WMPCoreEvents_OpenStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_OpenStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OpenStateChange(_WMPCoreEvents_OpenStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_OpenStateChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_OpenStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlayStateChange(_WMPCoreEvents_PlayStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlayStateChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlayStateChange(_WMPCoreEvents_PlayStateChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlayStateChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlayStateChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_AudioLanguageChange(_WMPCoreEvents_AudioLanguageChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_AudioLanguageChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_AudioLanguageChange(_WMPCoreEvents_AudioLanguageChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_AudioLanguageChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_AudioLanguageChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_StatusChange(_WMPCoreEvents_StatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_StatusChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_StatusChange(_WMPCoreEvents_StatusChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_StatusChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_StatusChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ScriptCommand(_WMPCoreEvents_ScriptCommandEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_ScriptCommandDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ScriptCommand(_WMPCoreEvents_ScriptCommandEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_ScriptCommandDelegate != null && ((wMPCoreEvents_SinkHelper.m_ScriptCommandDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_NewStream(_WMPCoreEvents_NewStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_NewStreamDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_NewStream(_WMPCoreEvents_NewStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_NewStreamDelegate != null && ((wMPCoreEvents_SinkHelper.m_NewStreamDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Disconnect(_WMPCoreEvents_DisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_DisconnectDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Disconnect(_WMPCoreEvents_DisconnectEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_DisconnectDelegate != null && ((wMPCoreEvents_SinkHelper.m_DisconnectDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Buffering(_WMPCoreEvents_BufferingEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_BufferingDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Buffering(_WMPCoreEvents_BufferingEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_BufferingDelegate != null && ((wMPCoreEvents_SinkHelper.m_BufferingDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Error(_WMPCoreEvents_ErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_ErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Error(_WMPCoreEvents_ErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_ErrorDelegate != null && ((wMPCoreEvents_SinkHelper.m_ErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_Warning(_WMPCoreEvents_WarningEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_WarningDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_Warning(_WMPCoreEvents_WarningEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_WarningDelegate != null && ((wMPCoreEvents_SinkHelper.m_WarningDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_EndOfStream(_WMPCoreEvents_EndOfStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_EndOfStreamDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_EndOfStream(_WMPCoreEvents_EndOfStreamEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_EndOfStreamDelegate != null && ((wMPCoreEvents_SinkHelper.m_EndOfStreamDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PositionChange(_WMPCoreEvents_PositionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PositionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PositionChange(_WMPCoreEvents_PositionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PositionChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_PositionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MarkerHit(_WMPCoreEvents_MarkerHitEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MarkerHitDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MarkerHit(_WMPCoreEvents_MarkerHitEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MarkerHitDelegate != null && ((wMPCoreEvents_SinkHelper.m_MarkerHitDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DurationUnitChange(_WMPCoreEvents_DurationUnitChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_DurationUnitChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DurationUnitChange(_WMPCoreEvents_DurationUnitChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_DurationUnitChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_DurationUnitChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CdromMediaChange(_WMPCoreEvents_CdromMediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_CdromMediaChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CdromMediaChange(_WMPCoreEvents_CdromMediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_CdromMediaChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_CdromMediaChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistChange(_WMPCoreEvents_PlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlaylistChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistChange(_WMPCoreEvents_PlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlaylistChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlaylistChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentPlaylistChange(_WMPCoreEvents_CurrentPlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_CurrentPlaylistChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentPlaylistChange(_WMPCoreEvents_CurrentPlaylistChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_CurrentPlaylistChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_CurrentPlaylistChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentPlaylistItemAvailable(_WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentPlaylistItemAvailable(_WMPCoreEvents_CurrentPlaylistItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate != null && ((wMPCoreEvents_SinkHelper.m_CurrentPlaylistItemAvailableDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaChange(_WMPCoreEvents_MediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaChange(_WMPCoreEvents_MediaChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentMediaItemAvailable(_WMPCoreEvents_CurrentMediaItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentMediaItemAvailable(_WMPCoreEvents_CurrentMediaItemAvailableEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate != null && ((wMPCoreEvents_SinkHelper.m_CurrentMediaItemAvailableDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_CurrentItemChange(_WMPCoreEvents_CurrentItemChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_CurrentItemChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_CurrentItemChange(_WMPCoreEvents_CurrentItemChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_CurrentItemChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_CurrentItemChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionChange(_WMPCoreEvents_MediaCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionChange(_WMPCoreEvents_MediaCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringAdded(_WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringAdded(_WMPCoreEvents_MediaCollectionAttributeStringAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringRemoved(_WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringRemoved(_WMPCoreEvents_MediaCollectionAttributeStringRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionAttributeStringChanged(_WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionAttributeStringChanged(_WMPCoreEvents_MediaCollectionAttributeStringChangedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionAttributeStringChangedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionChange(_WMPCoreEvents_PlaylistCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlaylistCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionChange(_WMPCoreEvents_PlaylistCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlaylistCollectionChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlaylistCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistAdded(_WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistAdded(_WMPCoreEvents_PlaylistCollectionPlaylistAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistRemoved(_WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistRemoved(_WMPCoreEvents_PlaylistCollectionPlaylistRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_PlaylistCollectionPlaylistSetAsDeleted(_WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_PlaylistCollectionPlaylistSetAsDeleted(_WMPCoreEvents_PlaylistCollectionPlaylistSetAsDeletedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate != null && ((wMPCoreEvents_SinkHelper.m_PlaylistCollectionPlaylistSetAsDeletedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_ModeChange(_WMPCoreEvents_ModeChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_ModeChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_ModeChange(_WMPCoreEvents_ModeChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_ModeChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_ModeChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaError(_WMPCoreEvents_MediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaErrorDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaError(_WMPCoreEvents_MediaErrorEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaErrorDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaErrorDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_OpenPlaylistSwitch(_WMPCoreEvents_OpenPlaylistSwitchEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_OpenPlaylistSwitchDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_OpenPlaylistSwitch(_WMPCoreEvents_OpenPlaylistSwitchEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_OpenPlaylistSwitchDelegate != null && ((wMPCoreEvents_SinkHelper.m_OpenPlaylistSwitchDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_DomainChange(_WMPCoreEvents_DomainChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_DomainChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_DomainChange(_WMPCoreEvents_DomainChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_DomainChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_DomainChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_StringCollectionChange(_WMPCoreEvents_StringCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_StringCollectionChangeDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_StringCollectionChange(_WMPCoreEvents_StringCollectionChangeEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_StringCollectionChangeDelegate != null && ((wMPCoreEvents_SinkHelper.m_StringCollectionChangeDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionMediaAdded(_WMPCoreEvents_MediaCollectionMediaAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionMediaAdded(_WMPCoreEvents_MediaCollectionMediaAddedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionMediaAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void add_MediaCollectionMediaRemoved(_WMPCoreEvents_MediaCollectionMediaRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = new _WMPCoreEvents_SinkHelper();
			int pdwCookie = 0;
			m_ConnectionPoint.Advise(wMPCoreEvents_SinkHelper, out pdwCookie);
			wMPCoreEvents_SinkHelper.m_dwCookie = pdwCookie;
			wMPCoreEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(wMPCoreEvents_SinkHelper);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void remove_MediaCollectionMediaRemoved(_WMPCoreEvents_MediaCollectionMediaRemovedEventHandler P_0)
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_aEventSinkHelpers == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (wMPCoreEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate != null && ((wMPCoreEvents_SinkHelper.m_MediaCollectionMediaRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					if (count <= 1)
					{
						Marshal.ReleaseComObject(m_ConnectionPoint);
						m_ConnectionPoint = null;
						m_aEventSinkHelpers = null;
					}
					break;
				}
				num++;
			}
			while (num < count);
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public _WMPCoreEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_ConnectionPointContainer = (IConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		bool lockTaken = default(bool);
		try
		{
			Monitor.Enter(this, ref lockTaken);
			if (m_ConnectionPoint == null)
			{
				return;
			}
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 < count)
			{
				do
				{
					_WMPCoreEvents_SinkHelper wMPCoreEvents_SinkHelper = (_WMPCoreEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(wMPCoreEvents_SinkHelper.m_dwCookie);
					num++;
				}
				while (num < count);
			}
			Marshal.ReleaseComObject(m_ConnectionPoint);
		}
		catch (Exception)
		{
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}

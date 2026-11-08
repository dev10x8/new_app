using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Office.Core;

[assembly: PrimaryInteropAssembly(5, 3)]
[assembly: Guid("0002e157-0000-0000-c000-000000000046")]
[assembly: ImportedFromTypeLib("VBIDE")]
[assembly: AssemblyVersion("12.0.0.0")]
namespace Microsoft.Vbe.Interop;

[ComImport]
[TypeLibType(4176)]
[Guid("0002E158-0000-0000-C000-000000000046")]
public interface Application
{
	[DispId(100)]
	string Version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[TypeLibType(16)]
[Guid("06A03650-2369-11CE-BFDC-08002B2B8CDA")]
public enum vbextFileTypes
{
	vbextFileTypeForm,
	vbextFileTypeModule,
	vbextFileTypeClass,
	vbextFileTypeProject,
	vbextFileTypeExe,
	vbextFileTypeFrx,
	vbextFileTypeRes,
	vbextFileTypeUserControl,
	vbextFileTypePropertyPage,
	vbextFileTypeDocObject,
	vbextFileTypeBinary,
	vbextFileTypeGroupProject,
	vbextFileTypeDesigners
}
[ComImport]
[TypeLibType(4160)]
[Guid("0002E166-0000-0000-C000-000000000046")]
public interface VBE : Application
{
	[DispId(100)]
	new string Version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(107)]
	VBProjects VBProjects
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(108)]
	CommandBars CommandBars
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(108)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(109)]
	CodePanes CodePanes
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(110)]
	Windows Windows
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(110)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(111)]
	Events Events
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(111)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(201)]
	VBProject ActiveVBProject
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(202)]
	VBComponent SelectedVBComponent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(204)]
	Window MainWindow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(204)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(205)]
	Window ActiveWindow
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(205)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(206)]
	CodePane ActiveCodePane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(206)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(206)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[DispId(209)]
	Addins Addins
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(209)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[Guid("BE39F3DB-1B13-11D0-887F-00A0C90F2744")]
public enum vbext_WindowType
{
	vbext_wt_CodeWindow = 0,
	vbext_wt_Designer = 1,
	vbext_wt_Browser = 2,
	vbext_wt_Watch = 3,
	vbext_wt_Locals = 4,
	vbext_wt_Immediate = 5,
	vbext_wt_ProjectWindow = 6,
	vbext_wt_PropertyWindow = 7,
	vbext_wt_Find = 8,
	vbext_wt_FindReplace = 9,
	vbext_wt_Toolbox = 10,
	vbext_wt_LinkedWindowFrame = 11,
	vbext_wt_MainWindow = 12,
	vbext_wt_ToolWindow = 15
}
[Guid("BE39F3DC-1B13-11D0-887F-00A0C90F2744")]
public enum vbext_WindowState
{
	vbext_ws_Normal,
	vbext_ws_Minimize,
	vbext_ws_Maximize
}
[ComImport]
[Guid("0002E16B-0000-0000-C000-000000000046")]
[TypeLibType(4160)]
public interface Window
{
	[DispId(1)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Windows Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(99)]
	void Close();

	[DispId(100)]
	string Caption
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(100)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(106)]
	bool Visible
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(106)]
		[param: In]
		set;
	}

	[DispId(101)]
	int Left
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(101)]
		[param: In]
		set;
	}

	[DispId(103)]
	int Top
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(103)]
		[param: In]
		set;
	}

	[DispId(105)]
	int Width
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(105)]
		[param: In]
		set;
	}

	[DispId(107)]
	int Height
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(107)]
		[param: In]
		set;
	}

	[DispId(109)]
	vbext_WindowState WindowState
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(109)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(111)]
	void SetFocus();

	[DispId(112)]
	vbext_WindowType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(112)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(65)]
	[DispId(113)]
	void SetKind([In] vbext_WindowType eKind);

	[DispId(116)]
	LinkedWindows LinkedWindows
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(117)]
	Window LinkedWindowFrame
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(117)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(65)]
	[DispId(118)]
	void Detach();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(65)]
	[DispId(119)]
	void Attach([In] int lWindowHandle);

	[DispId(120)]
	int HWnd
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(120)]
		get;
	}
}
[ComImport]
[TypeLibType(4176)]
[Guid("0002E16A-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
public interface _Windows_old : IEnumerable
{
	[DispId(1)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Window Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(201)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();
}
[ComImport]
[DefaultMember("Item")]
[Guid("F57B7ED0-D8AB-11D1-85DF-00C04F98F42C")]
[TypeLibType(4160)]
public interface _Windows : _Windows_old
{
	[DispId(1)]
	new VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	new Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new Window Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(201)]
	new int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(300)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Window CreateToolWindow([In][MarshalAs(UnmanagedType.Interface)] AddIn AddInInst, [In][MarshalAs(UnmanagedType.BStr)] string ProgId, [In][MarshalAs(UnmanagedType.BStr)] string Caption, [In][MarshalAs(UnmanagedType.BStr)] string GuidPosition, [In][Out][MarshalAs(UnmanagedType.IDispatch)] ref object DocObj);
}
[ComImport]
[ClassInterface(0)]
[DefaultMember("Item")]
[Guid("0002E185-0000-0000-C000-000000000046")]
[TypeLibType(2)]
public class WindowsClass : _Windows, Windows
{
	[DispId(1)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(201)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Window Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(300)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Window CreateToolWindow([In][MarshalAs(UnmanagedType.Interface)] AddIn AddInInst, [In][MarshalAs(UnmanagedType.BStr)] string ProgId, [In][MarshalAs(UnmanagedType.BStr)] string Caption, [In][MarshalAs(UnmanagedType.BStr)] string GuidPosition, [In][Out][MarshalAs(UnmanagedType.IDispatch)] ref object DocObj);
}
[ComImport]
[Guid("F57B7ED0-D8AB-11D1-85DF-00C04F98F42C")]
[CoClass(typeof(WindowsClass))]
public interface Windows : _Windows
{
}
[ComImport]
[TypeLibType(4160)]
[DefaultMember("Item")]
[Guid("0002E16C-0000-0000-C000-000000000046")]
public interface _LinkedWindows : IEnumerable
{
	[DispId(1)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Window Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Window Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(201)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(202)]
	void Remove([In][MarshalAs(UnmanagedType.Interface)] Window Window);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(203)]
	void Add([In][MarshalAs(UnmanagedType.Interface)] Window Window);
}
[ComImport]
[Guid("0002E187-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
[TypeLibType(2)]
[ClassInterface(0)]
public class LinkedWindowsClass : _LinkedWindows, LinkedWindows, IEnumerable
{
	[DispId(1)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern Window Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(201)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(201)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Window Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(202)]
	public virtual extern void Remove([In][MarshalAs(UnmanagedType.Interface)] Window Window);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(203)]
	public virtual extern void Add([In][MarshalAs(UnmanagedType.Interface)] Window Window);
}
[ComImport]
[Guid("0002E16C-0000-0000-C000-000000000046")]
[CoClass(typeof(LinkedWindowsClass))]
public interface LinkedWindows : _LinkedWindows
{
}
[ComImport]
[Guid("0002E167-0000-0000-C000-000000000046")]
[TypeLibType(4160)]
public interface Events
{
	[DispId(202)]
	ReferencesEvents ReferencesEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(202)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(205)]
	CommandBarEvents CommandBarEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(205)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[Guid("0002E113-0000-0000-C000-000000000046")]
[TypeLibType(256)]
[InterfaceType(1)]
public interface _VBProjectsEvents
{
}
[ComImport]
[TypeLibType(4096)]
[InterfaceType(2)]
[Guid("0002E103-0000-0000-C000-000000000046")]
public interface _dispVBProjectsEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void ItemAdded([In][MarshalAs(UnmanagedType.Interface)] VBProject VBProject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void ItemRemoved([In][MarshalAs(UnmanagedType.Interface)] VBProject VBProject);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void ItemRenamed([In][MarshalAs(UnmanagedType.Interface)] VBProject VBProject, [In][MarshalAs(UnmanagedType.BStr)] string OldName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void ItemActivated([In][MarshalAs(UnmanagedType.Interface)] VBProject VBProject);
}
[ComImport]
[TypeLibType(256)]
[Guid("0002E115-0000-0000-C000-000000000046")]
[InterfaceType(1)]
public interface _VBComponentsEvents
{
}
[ComImport]
[TypeLibType(4096)]
[InterfaceType(2)]
[Guid("0002E116-0000-0000-C000-000000000046")]
public interface _dispVBComponentsEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void ItemAdded([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void ItemRemoved([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void ItemRenamed([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent, [In][MarshalAs(UnmanagedType.BStr)] string OldName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void ItemSelected([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void ItemActivated([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	void ItemReloaded([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);
}
[ComImport]
[Guid("0002E11A-0000-0000-C000-000000000046")]
[TypeLibType(256)]
[InterfaceType(1)]
public interface _ReferencesEvents
{
}
[ComImport]
[InterfaceType(2)]
[Guid("0002E118-0000-0000-C000-000000000046")]
[TypeLibType(4096)]
public interface _dispReferencesEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void ItemAdded([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void ItemRemoved([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);
}
[ComImport]
[ClassInterface(0)]
[ComSourceInterfaces("Microsoft.Vbe.Interop._dispReferencesEvents\0\0")]
[Guid("0002E119-0000-0000-C000-000000000046")]
[TypeLibType(2)]
public class ReferencesEventsClass : _ReferencesEvents, ReferencesEvents, _dispReferencesEvents_Event
{
	public virtual extern event _dispReferencesEvents_ItemAddedEventHandler ItemAdded;

	public virtual extern event _dispReferencesEvents_ItemRemovedEventHandler ItemRemoved;
}
[TypeLibType(16)]
[ComVisible(false)]
[ComEventInterface(typeof(_dispReferencesEvents\u0000), typeof(_dispReferencesEvents_EventProvider\u0000))]
public interface _dispReferencesEvents_Event
{
	event _dispReferencesEvents_ItemAddedEventHandler ItemAdded;

	event _dispReferencesEvents_ItemRemovedEventHandler ItemRemoved;
}
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _dispReferencesEvents_ItemAddedEventHandler([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _dispReferencesEvents_ItemRemovedEventHandler([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);
[ComImport]
[CoClass(typeof(ReferencesEventsClass))]
[Guid("0002E11A-0000-0000-C000-000000000046")]
public interface ReferencesEvents : _ReferencesEvents, _dispReferencesEvents_Event
{
}
[ComImport]
[Guid("0002E130-0000-0000-C000-000000000046")]
[InterfaceType(1)]
[TypeLibType(256)]
public interface _CommandBarControlEvents
{
}
[ComImport]
[Guid("0002E131-0000-0000-C000-000000000046")]
[InterfaceType(2)]
[TypeLibType(4096)]
public interface _dispCommandBarControlEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void Click([In][MarshalAs(UnmanagedType.IDispatch)] object CommandBarControl, [In] ref bool handled, [In] ref bool CancelDefault);
}
[ComImport]
[TypeLibType(2)]
[Guid("0002E132-0000-0000-C000-000000000046")]
[ClassInterface(0)]
[ComSourceInterfaces("Microsoft.Vbe.Interop._dispCommandBarControlEvents\0\0")]
public class CommandBarEventsClass : _CommandBarControlEvents, CommandBarEvents, _dispCommandBarControlEvents_Event
{
	public virtual extern event _dispCommandBarControlEvents_ClickEventHandler Click;
}
[TypeLibType(16)]
[ComEventInterface(typeof(_dispCommandBarControlEvents\u0000), typeof(_dispCommandBarControlEvents_EventProvider\u0000))]
[ComVisible(false)]
public interface _dispCommandBarControlEvents_Event
{
	event _dispCommandBarControlEvents_ClickEventHandler Click;
}
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _dispCommandBarControlEvents_ClickEventHandler([In][MarshalAs(UnmanagedType.IDispatch)] object CommandBarControl, [In] ref bool handled, [In] ref bool CancelDefault);
[ComImport]
[Guid("0002E130-0000-0000-C000-000000000046")]
[CoClass(typeof(CommandBarEventsClass))]
public interface CommandBarEvents : _CommandBarControlEvents, _dispCommandBarControlEvents_Event
{
}
[ComImport]
[TypeLibType(4176)]
[Guid("0002E159-0000-0000-C000-000000000046")]
public interface _ProjectTemplate
{
	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[TypeLibType(18)]
[ClassInterface(0)]
[Guid("32CDF9E0-1602-11CE-BFDC-08002B2B8CDA")]
public class ProjectTemplateClass : _ProjectTemplate, ProjectTemplate
{
	[DispId(1)]
	public virtual extern Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[Guid("0002E159-0000-0000-C000-000000000046")]
[CoClass(typeof(ProjectTemplateClass))]
public interface ProjectTemplate : _ProjectTemplate
{
}
[Guid("FFCF3247-DEBF-11D1-BAFF-00C04FB6C4A6")]
public enum vbext_ProjectType
{
	vbext_pt_HostProject = 100,
	vbext_pt_StandAlone
}
[Guid("0002E129-0000-0000-C000-000000000046")]
public enum vbext_ProjectProtection
{
	vbext_pp_none,
	vbext_pp_locked
}
[Guid("BE39F3D2-1B13-11D0-887F-00A0C90F2744")]
public enum vbext_VBAMode
{
	vbext_vm_Run,
	vbext_vm_Break,
	vbext_vm_Design
}
[ComImport]
[Guid("0002E160-0000-0000-C000-000000000046")]
[TypeLibType(4176)]
public interface _VBProject_Old : _ProjectTemplate
{
	[DispId(1)]
	new Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	new Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(116)]
	string HelpFile
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(117)]
	int HelpContextID
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
	string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(119)]
	vbext_VBAMode Mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(119)]
		get;
	}

	[DispId(120)]
	References References
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(120)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(121)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(122)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(122)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(123)]
	VBProjects Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(131)]
	vbext_ProjectProtection Protection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(131)]
		get;
	}

	[DispId(133)]
	bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(133)]
		get;
	}

	[DispId(135)]
	VBComponents VBComponents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(135)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[TypeLibType(4176)]
[Guid("EEE00915-E393-11D1-BB03-00C04FB6C4A6")]
public interface _VBProject : _VBProject_Old
{
	[DispId(1)]
	new Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	new Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(116)]
	new string HelpFile
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(117)]
	new int HelpContextID
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
	new string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(119)]
	new vbext_VBAMode Mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(119)]
		get;
	}

	[DispId(120)]
	new References References
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(120)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(121)]
	new string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(122)]
	new VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(122)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(123)]
	new VBProjects Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(131)]
	new vbext_ProjectProtection Protection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(131)]
		get;
	}

	[DispId(133)]
	new bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(133)]
		get;
	}

	[DispId(135)]
	new VBComponents VBComponents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(135)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(138)]
	void SaveAs([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(139)]
	void MakeCompiledFile();

	[DispId(140)]
	vbext_ProjectType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(140)]
		get;
	}

	[DispId(141)]
	string FileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(141)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(142)]
	string BuildFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(142)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(142)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[TypeLibType(2)]
[ClassInterface(0)]
[Guid("0002E169-0000-0000-C000-000000000046")]
public class VBProjectClass : _VBProject, VBProject
{
	[DispId(1)]
	public virtual extern Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern Application Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(116)]
	public virtual extern string HelpFile
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(116)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(117)]
	public virtual extern int HelpContextID
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
	public virtual extern string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(118)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(119)]
	public virtual extern vbext_VBAMode Mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(119)]
		get;
	}

	[DispId(120)]
	public virtual extern References References
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(120)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(121)]
	public virtual extern string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(121)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(122)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(122)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(123)]
	public virtual extern VBProjects Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(131)]
	public virtual extern vbext_ProjectProtection Protection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(131)]
		get;
	}

	[DispId(133)]
	public virtual extern bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(133)]
		get;
	}

	[DispId(135)]
	public virtual extern VBComponents VBComponents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(135)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(140)]
	public virtual extern vbext_ProjectType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(140)]
		get;
	}

	[DispId(141)]
	public virtual extern string FileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(141)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(142)]
	public virtual extern string BuildFileName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(142)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(142)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(138)]
	public virtual extern void SaveAs([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(139)]
	public virtual extern void MakeCompiledFile();
}
[ComImport]
[CoClass(typeof(VBProjectClass))]
[Guid("EEE00915-E393-11D1-BB03-00C04FB6C4A6")]
public interface VBProject : _VBProject
{
}
[ComImport]
[DefaultMember("Item")]
[TypeLibType(4176)]
[Guid("0002E165-0000-0000-C000-000000000046")]
public interface _VBProjects_Old : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBProject Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(20)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	VBE Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();
}
[ComImport]
[Guid("EEE00919-E393-11D1-BB03-00C04FB6C4A6")]
[TypeLibType(4160)]
[DefaultMember("Item")]
public interface _VBProjects : _VBProjects_Old
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new VBProject Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(20)]
	new VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	new VBE Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	new int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(137)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBProject Add([In] vbext_ProjectType Type);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(138)]
	void Remove([In][MarshalAs(UnmanagedType.Interface)] VBProject lpc);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(139)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBProject Open([In][MarshalAs(UnmanagedType.BStr)] string bstrPath);
}
[ComImport]
[Guid("0002E101-0000-0000-C000-000000000046")]
[ClassInterface(0)]
[DefaultMember("Item")]
[TypeLibType(2)]
public class VBProjectsClass : _VBProjects, VBProjects
{
	[DispId(20)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern VBE Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBProject Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(137)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBProject Add([In] vbext_ProjectType Type);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(138)]
	public virtual extern void Remove([In][MarshalAs(UnmanagedType.Interface)] VBProject lpc);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(139)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBProject Open([In][MarshalAs(UnmanagedType.BStr)] string bstrPath);
}
[ComImport]
[CoClass(typeof(VBProjectsClass))]
[Guid("EEE00919-E393-11D1-BB03-00C04FB6C4A6")]
public interface VBProjects : _VBProjects
{
}
[ComImport]
[Guid("BE39F3D4-1B13-11D0-887F-00A0C90F2744")]
[DefaultMember("Item")]
[TypeLibType(4176)]
public interface SelectedComponents : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Component Item([In] int index);

	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();
}
[Guid("BE39F3D5-1B13-11D0-887F-00A0C90F2744")]
public enum vbext_ComponentType
{
	vbext_ct_StdModule = 1,
	vbext_ct_ClassModule = 2,
	vbext_ct_MSForm = 3,
	vbext_ct_ActiveXDesigner = 11,
	vbext_ct_Document = 100
}
[ComImport]
[TypeLibType(4176)]
[Guid("0002E161-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
public interface _Components : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Component Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	void Remove([In][MarshalAs(UnmanagedType.Interface)] Component Component);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Component Add([In] vbext_ComponentType ComponentType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Component Import([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(20)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[Guid("BE39F3D6-1B13-11D0-887F-00A0C90F2744")]
[DefaultMember("Item")]
[TypeLibType(18)]
[ClassInterface(0)]
public class ComponentsClass : _Components, Components, IEnumerable
{
	[DispId(1)]
	public virtual extern Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(20)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Component Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	public virtual extern void Remove([In][MarshalAs(UnmanagedType.Interface)] Component Component);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Component Add([In] vbext_ComponentType ComponentType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Component Import([In][MarshalAs(UnmanagedType.BStr)] string FileName);
}
[ComImport]
[Guid("0002E161-0000-0000-C000-000000000046")]
[CoClass(typeof(ComponentsClass))]
public interface Components : _Components
{
}
[ComImport]
[TypeLibType(4176)]
[Guid("0002E162-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
public interface _VBComponents_Old : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBComponent Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(2)]
	VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	void Remove([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBComponent Add([In] vbext_ComponentType ComponentType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBComponent Import([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(20)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[DefaultMember("Item")]
[Guid("EEE0091C-E393-11D1-BB03-00C04FB6C4A6")]
[TypeLibType(4160)]
public interface _VBComponents : _VBComponents_Old
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new VBComponent Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(2)]
	new VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	new int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	new void Remove([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new VBComponent Add([In] vbext_ComponentType ComponentType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new VBComponent Import([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(20)]
	new VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBComponent AddCustom([In][MarshalAs(UnmanagedType.BStr)] string ProgId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(26)]
	[return: MarshalAs(UnmanagedType.Interface)]
	VBComponent AddMTDesigner([In] int index = 0);
}
[ComImport]
[Guid("BE39F3D7-1B13-11D0-887F-00A0C90F2744")]
[TypeLibType(2)]
[ClassInterface(0)]
[DefaultMember("Item")]
public class VBComponentsClass : _VBComponents, VBComponents
{
	[DispId(2)]
	public virtual extern VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(20)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(20)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBComponent Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	public virtual extern void Remove([In][MarshalAs(UnmanagedType.Interface)] VBComponent VBComponent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBComponent Add([In] vbext_ComponentType ComponentType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBComponent Import([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBComponent AddCustom([In][MarshalAs(UnmanagedType.BStr)] string ProgId);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(26)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern VBComponent AddMTDesigner([In] int index = 0);
}
[ComImport]
[Guid("EEE0091C-E393-11D1-BB03-00C04FB6C4A6")]
[CoClass(typeof(VBComponentsClass))]
public interface VBComponents : _VBComponents
{
}
[ComImport]
[Guid("0002E163-0000-0000-C000-000000000046")]
[TypeLibType(4176)]
public interface _Component
{
	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Components Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	bool IsDirty
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[param: In]
		set;
	}

	[DispId(48)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[Guid("BE39F3D8-1B13-11D0-887F-00A0C90F2744")]
[ClassInterface(0)]
[TypeLibType(18)]
public class ComponentClass : _Component, Component
{
	[DispId(1)]
	public virtual extern Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern Components Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(10)]
	public virtual extern bool IsDirty
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[param: In]
		set;
	}

	[DispId(48)]
	public virtual extern string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}
}
[ComImport]
[CoClass(typeof(ComponentClass))]
[Guid("0002E163-0000-0000-C000-000000000046")]
public interface Component : _Component
{
}
[ComImport]
[TypeLibType(4176)]
[Guid("0002E164-0000-0000-C000-000000000046")]
public interface _VBComponent_Old
{
	[DispId(10)]
	bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(48)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(49)]
	object Designer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(49)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(50)]
	CodeModule CodeModule
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(50)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(51)]
	vbext_ComponentType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(51)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	void Export([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(53)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(53)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(54)]
	VBComponents Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(54)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(55)]
	bool HasOpenDesigner
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(55)]
		get;
	}

	[DispId(56)]
	Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Window DesignerWindow();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	void Activate();
}
[ComImport]
[Guid("EEE00921-E393-11D1-BB03-00C04FB6C4A6")]
[TypeLibType(4160)]
public interface _VBComponent : _VBComponent_Old
{
	[DispId(10)]
	new bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(48)]
	new string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(49)]
	new object Designer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(49)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(50)]
	new CodeModule CodeModule
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(50)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(51)]
	new vbext_ComponentType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(51)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	new void Export([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(53)]
	new VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(53)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(54)]
	new VBComponents Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(54)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(55)]
	new bool HasOpenDesigner
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(55)]
		get;
	}

	[DispId(56)]
	new Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	[return: MarshalAs(UnmanagedType.Interface)]
	new Window DesignerWindow();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	new void Activate();

	[DispId(64)]
	string DesignerID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(64)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[ClassInterface(0)]
[TypeLibType(2)]
[Guid("BE39F3DA-1B13-11D0-887F-00A0C90F2744")]
public class VBComponentClass : _VBComponent, VBComponent
{
	[DispId(10)]
	public virtual extern bool Saved
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		get;
	}

	[DispId(48)]
	public virtual extern string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(48)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(49)]
	public virtual extern object Designer
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(49)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(50)]
	public virtual extern CodeModule CodeModule
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(50)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(51)]
	public virtual extern vbext_ComponentType Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(51)]
		get;
	}

	[DispId(53)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(53)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(54)]
	public virtual extern VBComponents Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(54)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(55)]
	public virtual extern bool HasOpenDesigner
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(55)]
		get;
	}

	[DispId(56)]
	public virtual extern Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(56)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(64)]
	public virtual extern string DesignerID
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(64)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	public virtual extern void Export([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Window DesignerWindow();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	public virtual extern void Activate();
}
[ComImport]
[Guid("EEE00921-E393-11D1-BB03-00C04FB6C4A6")]
[CoClass(typeof(VBComponentClass))]
public interface VBComponent : _VBComponent
{
}
[ComImport]
[Guid("0002E18C-0000-0000-C000-000000000046")]
[DefaultMember("Value")]
[TypeLibType(4160)]
public interface Property
{
	[DispId(0)]
	object Value
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[DispId(3)]
	object IndexedValue
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[param: Optional]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[DispId(4)]
	short NumIndices
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		get;
	}

	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Properties Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(40)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(41)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(41)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(42)]
	Properties Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(42)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(45)]
	object Object
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(45)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(45)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IUnknown)]
		set;
	}
}
[ComImport]
[Guid("0002E188-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
[TypeLibType(4160)]
public interface _Properties : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Property Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(1)]
	Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	object Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(40)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(1)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[DispId(10)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[DefaultMember("Item")]
[Guid("0002E18B-0000-0000-C000-000000000046")]
[TypeLibType(2)]
[ClassInterface(0)]
public class PropertiesClass : _Properties, Properties, IEnumerable
{
	[DispId(1)]
	public virtual extern Application Application
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern object Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(40)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		get;
	}

	[DispId(10)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Property Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();
}
[ComImport]
[Guid("0002E188-0000-0000-C000-000000000046")]
[CoClass(typeof(PropertiesClass))]
public interface Properties : _Properties
{
}
[ComImport]
[Guid("DA936B62-AC8B-11D1-B6E5-00A0C90F2744")]
[TypeLibType(4160)]
[DefaultMember("Item")]
public interface _AddIns : IEnumerable
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	AddIn Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(1)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	object Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(40)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	void Update();
}
[ComImport]
[TypeLibType(2)]
[DefaultMember("Item")]
[ClassInterface(0)]
[Guid("DA936B63-AC8B-11D1-B6E5-00A0C90F2744")]
public class AddinsClass : _AddIns, Addins, IEnumerable
{
	[DispId(1)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	public virtual extern object Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(40)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(40)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern AddIn Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[TypeLibFunc(1)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	public virtual extern void Update();
}
[ComImport]
[Guid("DA936B62-AC8B-11D1-B6E5-00A0C90F2744")]
[CoClass(typeof(AddinsClass))]
public interface Addins : _AddIns
{
}
[ComImport]
[DefaultMember("Description")]
[Guid("DA936B64-AC8B-11D1-B6E5-00A0C90F2744")]
[TypeLibType(4160)]
public interface AddIn
{
	[DispId(0)]
	string Description
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

	[DispId(1)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(2)]
	Addins Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(3)]
	string ProgId
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(4)]
	string Guid
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(6)]
	bool Connect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[param: In]
		set;
	}

	[DispId(7)]
	object Object
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}
}
public enum vbext_ProcKind
{
	vbext_pk_Proc,
	vbext_pk_Let,
	vbext_pk_Set,
	vbext_pk_Get
}
[ComImport]
[DefaultMember("Name")]
[Guid("0002E16E-0000-0000-C000-000000000046")]
[TypeLibType(4304)]
public interface _CodeModule
{
	[DispId(1610743808)]
	VBComponent Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(0)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[TypeLibFunc(64)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743812)]
	void AddFromString([In][MarshalAs(UnmanagedType.BStr)] string String);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743813)]
	void AddFromFile([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[DispId(1610743814)]
	string Lines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743814)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743815)]
	int CountOfLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743815)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743816)]
	void InsertLines([In] int Line, [In][MarshalAs(UnmanagedType.BStr)] string String);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743817)]
	void DeleteLines([In] int StartLine, [In] int Count = 1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743818)]
	void ReplaceLine([In] int Line, [In][MarshalAs(UnmanagedType.BStr)] string String);

	[DispId(1610743819)]
	int ProcStartLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743819)]
		get;
	}

	[DispId(1610743820)]
	int ProcCountLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743820)]
		get;
	}

	[DispId(1610743821)]
	int ProcBodyLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743821)]
		get;
	}

	[DispId(1610743822)]
	string ProcOfLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743822)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743823)]
	int CountOfDeclarationLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743823)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743824)]
	int CreateEventProc([In][MarshalAs(UnmanagedType.BStr)] string EventName, [In][MarshalAs(UnmanagedType.BStr)] string ObjectName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743825)]
	bool Find([In][MarshalAs(UnmanagedType.BStr)] string Target, [In][Out] ref int StartLine, [In][Out] ref int StartColumn, [In][Out] ref int EndLine, [In][Out] ref int EndColumn, [In] bool WholeWord = false, [In] bool MatchCase = false, [In] bool PatternSearch = false);

	[DispId(1610743826)]
	CodePane CodePane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743826)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
[ComImport]
[ClassInterface(0)]
[DefaultMember("Name")]
[Guid("0002E170-0000-0000-C000-000000000046")]
[TypeLibType(2)]
public class CodeModuleClass : _CodeModule, CodeModule
{
	[DispId(1610743808)]
	public virtual extern VBComponent Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(0)]
	public virtual extern string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1610743814)]
	public virtual extern string Lines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743814)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743815)]
	public virtual extern int CountOfLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743815)]
		get;
	}

	[DispId(1610743819)]
	public virtual extern int ProcStartLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743819)]
		get;
	}

	[DispId(1610743820)]
	public virtual extern int ProcCountLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743820)]
		get;
	}

	[DispId(1610743821)]
	public virtual extern int ProcBodyLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743821)]
		get;
	}

	[DispId(1610743822)]
	public virtual extern string ProcOfLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743822)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743823)]
	public virtual extern int CountOfDeclarationLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743823)]
		get;
	}

	[DispId(1610743826)]
	public virtual extern CodePane CodePane
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743826)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743812)]
	public virtual extern void AddFromString([In][MarshalAs(UnmanagedType.BStr)] string String);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743813)]
	public virtual extern void AddFromFile([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743816)]
	public virtual extern void InsertLines([In] int Line, [In][MarshalAs(UnmanagedType.BStr)] string String);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743817)]
	public virtual extern void DeleteLines([In] int StartLine, [In] int Count = 1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743818)]
	public virtual extern void ReplaceLine([In] int Line, [In][MarshalAs(UnmanagedType.BStr)] string String);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743824)]
	public virtual extern int CreateEventProc([In][MarshalAs(UnmanagedType.BStr)] string EventName, [In][MarshalAs(UnmanagedType.BStr)] string ObjectName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743825)]
	public virtual extern bool Find([In][MarshalAs(UnmanagedType.BStr)] string Target, [In][Out] ref int StartLine, [In][Out] ref int StartColumn, [In][Out] ref int EndLine, [In][Out] ref int EndColumn, [In] bool WholeWord = false, [In] bool MatchCase = false, [In] bool PatternSearch = false);
}
[ComImport]
[CoClass(typeof(CodeModuleClass))]
[Guid("0002E16E-0000-0000-C000-000000000046")]
public interface CodeModule : _CodeModule
{
}
[ComImport]
[Guid("0002E172-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
[TypeLibType(4288)]
public interface _CodePanes : IEnumerable
{
	[DispId(1610743808)]
	VBE Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	CodePane Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(1610743811)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743811)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[DispId(1610743813)]
	CodePane Current
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1610743813)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		[TypeLibFunc(64)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}
}
[ComImport]
[Guid("0002E174-0000-0000-C000-000000000046")]
[ClassInterface(0)]
[DefaultMember("Item")]
[TypeLibType(2)]
public class CodePanesClass : _CodePanes, CodePanes, IEnumerable
{
	[DispId(1610743808)]
	public virtual extern VBE Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743811)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743811)]
		get;
	}

	[DispId(1610743813)]
	public virtual extern CodePane Current
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		[TypeLibFunc(64)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(1610743813)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern CodePane Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();
}
[ComImport]
[Guid("0002E172-0000-0000-C000-000000000046")]
[CoClass(typeof(CodePanesClass))]
public interface CodePanes : _CodePanes
{
}
public enum vbext_CodePaneview
{
	vbext_cv_ProcedureView,
	vbext_cv_FullModuleView
}
[ComImport]
[TypeLibType(4288)]
[Guid("0002E176-0000-0000-C000-000000000046")]
public interface _CodePane
{
	[DispId(1610743808)]
	CodePanes Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743810)]
	Window Window
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743810)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743811)]
	void GetSelection(out int StartLine, out int StartColumn, out int EndLine, out int EndColumn);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743812)]
	void SetSelection([In] int StartLine, [In] int StartColumn, [In] int EndLine, [In] int EndColumn);

	[DispId(1610743813)]
	int TopLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		[param: In]
		set;
	}

	[DispId(1610743815)]
	int CountOfVisibleLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743815)]
		get;
	}

	[DispId(1610743816)]
	CodeModule CodeModule
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743816)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743817)]
	void Show();

	[DispId(1610743818)]
	vbext_CodePaneview CodePaneView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743818)]
		get;
	}
}
[ComImport]
[TypeLibType(2)]
[ClassInterface(0)]
[Guid("0002E178-0000-0000-C000-000000000046")]
public class CodePaneClass : _CodePane, CodePane
{
	[DispId(1610743808)]
	public virtual extern CodePanes Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743810)]
	public virtual extern Window Window
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743810)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743813)]
	public virtual extern int TopLine
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		[param: In]
		set;
	}

	[DispId(1610743815)]
	public virtual extern int CountOfVisibleLines
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743815)]
		get;
	}

	[DispId(1610743816)]
	public virtual extern CodeModule CodeModule
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743816)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743818)]
	public virtual extern vbext_CodePaneview CodePaneView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743818)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743811)]
	public virtual extern void GetSelection(out int StartLine, out int StartColumn, out int EndLine, out int EndColumn);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743812)]
	public virtual extern void SetSelection([In] int StartLine, [In] int StartColumn, [In] int EndLine, [In] int EndColumn);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743817)]
	public virtual extern void Show();
}
[ComImport]
[Guid("0002E176-0000-0000-C000-000000000046")]
[CoClass(typeof(CodePaneClass))]
public interface CodePane : _CodePane
{
}
[ComImport]
[DefaultMember("Item")]
[TypeLibType(4304)]
[Guid("0002E17A-0000-0000-C000-000000000046")]
public interface _References : IEnumerable
{
	[DispId(1610743808)]
	VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Reference Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[DispId(1610743811)]
	int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743811)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	new IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743813)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Reference AddFromGuid([In][MarshalAs(UnmanagedType.BStr)] string Guid, [In] int Major, [In] int Minor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743814)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Reference AddFromFile([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743815)]
	void Remove([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);
}
public enum vbext_RefKind
{
	vbext_rk_TypeLib,
	vbext_rk_Project
}
[ComImport]
[TypeLibType(4288)]
[Guid("0002E17E-0000-0000-C000-000000000046")]
public interface Reference
{
	[DispId(1610743808)]
	References Collection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743810)]
	string Name
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743810)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743811)]
	string Guid
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743811)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743812)]
	int Major
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743812)]
		get;
	}

	[DispId(1610743813)]
	int Minor
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743813)]
		get;
	}

	[DispId(1610743814)]
	string FullPath
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743814)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(1610743815)]
	bool BuiltIn
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743815)]
		get;
	}

	[DispId(1610743816)]
	bool IsBroken
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743816)]
		get;
	}

	[DispId(1610743817)]
	vbext_RefKind Type
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743817)]
		get;
	}

	[DispId(1610743818)]
	string Description
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743818)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}
}
[ComImport]
[InterfaceType(2)]
[DefaultMember("ItemAdded")]
[Guid("CDDE3804-2064-11CF-867F-00AA005FF34A")]
[TypeLibType(4240)]
public interface _dispReferences_Events
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	void ItemAdded([MarshalAs(UnmanagedType.Interface)] Reference Reference);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void ItemRemoved([MarshalAs(UnmanagedType.Interface)] Reference Reference);
}
[ComImport]
[Guid("0002E17C-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
[TypeLibType(2)]
[ComSourceInterfaces("Microsoft.Vbe.Interop._dispReferences_Events\0\0")]
[ClassInterface(0)]
public class ReferencesClass : _References, References, _dispReferences_Events_Event, IEnumerable
{
	[DispId(1610743808)]
	public virtual extern VBProject Parent
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743808)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743809)]
	public virtual extern VBE VBE
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743809)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[DispId(1610743811)]
	public virtual extern int Count
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1610743811)]
		get;
	}

	public virtual extern event _dispReferences_Events_ItemAddedEventHandler ItemAdded;

	public virtual extern event _dispReferences_Events_ItemRemovedEventHandler ItemRemoved;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Reference Item([In][MarshalAs(UnmanagedType.Struct)] object index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-4)]
	[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public virtual extern IEnumerator GetEnumerator();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743813)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Reference AddFromGuid([In][MarshalAs(UnmanagedType.BStr)] string Guid, [In] int Major, [In] int Minor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743814)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Reference AddFromFile([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610743815)]
	public virtual extern void Remove([In][MarshalAs(UnmanagedType.Interface)] Reference Reference);
}
[ComVisible(false)]
[ComEventInterface(typeof(_dispReferences_Events\u0000), typeof(_dispReferences_Events_EventProvider\u0000))]
[TypeLibType(16)]
public interface _dispReferences_Events_Event
{
	event _dispReferences_Events_ItemAddedEventHandler ItemAdded;

	event _dispReferences_Events_ItemRemovedEventHandler ItemRemoved;
}
[ComVisible(false)]
[TypeLibType(16)]
[DefaultMember("Invoke")]
public delegate void _dispReferences_Events_ItemAddedEventHandler([MarshalAs(UnmanagedType.Interface)] Reference Reference);
[TypeLibType(16)]
[ComVisible(false)]
public delegate void _dispReferences_Events_ItemRemovedEventHandler([MarshalAs(UnmanagedType.Interface)] Reference Reference);
[ComImport]
[Guid("0002E17A-0000-0000-C000-000000000046")]
[CoClass(typeof(ReferencesClass))]
public interface References : _References, _dispReferences_Events_Event
{
}
[ClassInterface(ClassInterfaceType.None)]
public sealed class _dispReferences_Events_SinkHelper : _dispReferences_Events
{
	public _dispReferences_Events_ItemRemovedEventHandler m_ItemRemovedDelegate;

	public _dispReferences_Events_ItemAddedEventHandler m_ItemAddedDelegate;

	public int m_dwCookie;

	public void ItemRemoved(Reference P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ItemRemovedDelegate != null)
		{
			m_ItemRemovedDelegate(P_0);
		}
	}

	public void ItemAdded(Reference P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ItemAddedDelegate != null)
		{
			m_ItemAddedDelegate(P_0);
		}
	}

	internal _dispReferences_Events_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ItemRemovedDelegate = null;
		m_ItemAddedDelegate = null;
	}
}
internal sealed class _dispReferences_Events_EventProvider : _dispReferences_Events_Event, IDisposable
{
	private UCOMIConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private UCOMIConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UCOMIConnectionPoint val = null;
		Guid guid = new Guid(new byte[16]
		{
			4, 56, 222, 205, 100, 32, 207, 17, 134, 127,
			0, 170, 0, 95, 243, 74
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref guid, ref val);
		m_ConnectionPoint = val;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_ItemRemoved(_dispReferences_Events_ItemRemovedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_dispReferences_Events_SinkHelper dispReferences_Events_SinkHelper = new _dispReferences_Events_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)dispReferences_Events_SinkHelper, ref dwCookie);
			dispReferences_Events_SinkHelper.m_dwCookie = dwCookie;
			dispReferences_Events_SinkHelper.m_ItemRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(dispReferences_Events_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ItemRemoved(_dispReferences_Events_ItemRemovedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_dispReferences_Events_SinkHelper dispReferences_Events_SinkHelper = (_dispReferences_Events_SinkHelper)m_aEventSinkHelpers[num];
				if (dispReferences_Events_SinkHelper.m_ItemRemovedDelegate != null && ((dispReferences_Events_SinkHelper.m_ItemRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dispReferences_Events_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public void add_ItemAdded(_dispReferences_Events_ItemAddedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_dispReferences_Events_SinkHelper dispReferences_Events_SinkHelper = new _dispReferences_Events_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)dispReferences_Events_SinkHelper, ref dwCookie);
			dispReferences_Events_SinkHelper.m_dwCookie = dwCookie;
			dispReferences_Events_SinkHelper.m_ItemAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(dispReferences_Events_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ItemAdded(_dispReferences_Events_ItemAddedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_dispReferences_Events_SinkHelper dispReferences_Events_SinkHelper = (_dispReferences_Events_SinkHelper)m_aEventSinkHelpers[num];
				if (dispReferences_Events_SinkHelper.m_ItemAddedDelegate != null && ((dispReferences_Events_SinkHelper.m_ItemAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dispReferences_Events_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public _dispReferences_Events_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		m_ConnectionPointContainer = (UCOMIConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		Monitor.Enter(this);
		try
		{
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
					_dispReferences_Events_SinkHelper dispReferences_Events_SinkHelper = (_dispReferences_Events_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dispReferences_Events_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
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
public sealed class _dispReferencesEvents_SinkHelper : _dispReferencesEvents
{
	public _dispReferencesEvents_ItemRemovedEventHandler m_ItemRemovedDelegate;

	public _dispReferencesEvents_ItemAddedEventHandler m_ItemAddedDelegate;

	public int m_dwCookie;

	public void ItemRemoved(Reference P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ItemRemovedDelegate != null)
		{
			m_ItemRemovedDelegate(P_0);
		}
	}

	public void ItemAdded(Reference P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ItemAddedDelegate != null)
		{
			m_ItemAddedDelegate(P_0);
		}
	}

	internal _dispReferencesEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ItemRemovedDelegate = null;
		m_ItemAddedDelegate = null;
	}
}
internal sealed class _dispReferencesEvents_EventProvider : _dispReferencesEvents_Event, IDisposable
{
	private UCOMIConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private UCOMIConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UCOMIConnectionPoint val = null;
		Guid guid = new Guid(new byte[16]
		{
			24, 225, 2, 0, 0, 0, 0, 0, 192, 0,
			0, 0, 0, 0, 0, 70
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref guid, ref val);
		m_ConnectionPoint = val;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_ItemRemoved(_dispReferencesEvents_ItemRemovedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_dispReferencesEvents_SinkHelper dispReferencesEvents_SinkHelper = new _dispReferencesEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)dispReferencesEvents_SinkHelper, ref dwCookie);
			dispReferencesEvents_SinkHelper.m_dwCookie = dwCookie;
			dispReferencesEvents_SinkHelper.m_ItemRemovedDelegate = P_0;
			m_aEventSinkHelpers.Add(dispReferencesEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ItemRemoved(_dispReferencesEvents_ItemRemovedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_dispReferencesEvents_SinkHelper dispReferencesEvents_SinkHelper = (_dispReferencesEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dispReferencesEvents_SinkHelper.m_ItemRemovedDelegate != null && ((dispReferencesEvents_SinkHelper.m_ItemRemovedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dispReferencesEvents_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public void add_ItemAdded(_dispReferencesEvents_ItemAddedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_dispReferencesEvents_SinkHelper dispReferencesEvents_SinkHelper = new _dispReferencesEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)dispReferencesEvents_SinkHelper, ref dwCookie);
			dispReferencesEvents_SinkHelper.m_dwCookie = dwCookie;
			dispReferencesEvents_SinkHelper.m_ItemAddedDelegate = P_0;
			m_aEventSinkHelpers.Add(dispReferencesEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_ItemAdded(_dispReferencesEvents_ItemAddedEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_dispReferencesEvents_SinkHelper dispReferencesEvents_SinkHelper = (_dispReferencesEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dispReferencesEvents_SinkHelper.m_ItemAddedDelegate != null && ((dispReferencesEvents_SinkHelper.m_ItemAddedDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dispReferencesEvents_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public _dispReferencesEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		m_ConnectionPointContainer = (UCOMIConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		Monitor.Enter(this);
		try
		{
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
					_dispReferencesEvents_SinkHelper dispReferencesEvents_SinkHelper = (_dispReferencesEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dispReferencesEvents_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
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
public sealed class _dispCommandBarControlEvents_SinkHelper : _dispCommandBarControlEvents
{
	public _dispCommandBarControlEvents_ClickEventHandler m_ClickDelegate;

	public int m_dwCookie;

	public void Click(object P_0, ref bool P_1, ref bool P_2)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		if (m_ClickDelegate != null)
		{
			m_ClickDelegate(P_0, ref P_1, ref P_2);
		}
	}

	internal _dispCommandBarControlEvents_SinkHelper()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		m_dwCookie = 0;
		m_ClickDelegate = null;
	}
}
internal sealed class _dispCommandBarControlEvents_EventProvider : _dispCommandBarControlEvents_Event, IDisposable
{
	private UCOMIConnectionPointContainer m_ConnectionPointContainer;

	private ArrayList m_aEventSinkHelpers;

	private UCOMIConnectionPoint m_ConnectionPoint;

	private void Init()
	{
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		UCOMIConnectionPoint val = null;
		Guid guid = new Guid(new byte[16]
		{
			49, 225, 2, 0, 0, 0, 0, 0, 192, 0,
			0, 0, 0, 0, 0, 70
		});
		m_ConnectionPointContainer.FindConnectionPoint(ref guid, ref val);
		m_ConnectionPoint = val;
		m_aEventSinkHelpers = new ArrayList();
	}

	public void add_Click(_dispCommandBarControlEvents_ClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			if (m_ConnectionPoint == null)
			{
				Init();
			}
			_dispCommandBarControlEvents_SinkHelper dispCommandBarControlEvents_SinkHelper = new _dispCommandBarControlEvents_SinkHelper();
			int dwCookie = 0;
			m_ConnectionPoint.Advise((object)dispCommandBarControlEvents_SinkHelper, ref dwCookie);
			dispCommandBarControlEvents_SinkHelper.m_dwCookie = dwCookie;
			dispCommandBarControlEvents_SinkHelper.m_ClickDelegate = P_0;
			m_aEventSinkHelpers.Add(dispCommandBarControlEvents_SinkHelper);
		}
		finally
		{
			Monitor.Exit(this);
		}
	}

	public void remove_Click(_dispCommandBarControlEvents_ClickEventHandler P_0)
	{
		Monitor.Enter(this);
		try
		{
			int count = m_aEventSinkHelpers.Count;
			int num = 0;
			if (0 >= count)
			{
				return;
			}
			do
			{
				_dispCommandBarControlEvents_SinkHelper dispCommandBarControlEvents_SinkHelper = (_dispCommandBarControlEvents_SinkHelper)m_aEventSinkHelpers[num];
				if (dispCommandBarControlEvents_SinkHelper.m_ClickDelegate != null && ((dispCommandBarControlEvents_SinkHelper.m_ClickDelegate.Equals(P_0) ? 1u : 0u) & 0xFFu) != 0)
				{
					m_aEventSinkHelpers.RemoveAt(num);
					m_ConnectionPoint.Unadvise(dispCommandBarControlEvents_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public _dispCommandBarControlEvents_EventProvider(object P_0)
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		m_ConnectionPointContainer = (UCOMIConnectionPointContainer)P_0;
	}

	public void Finalize()
	{
		Monitor.Enter(this);
		try
		{
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
					_dispCommandBarControlEvents_SinkHelper dispCommandBarControlEvents_SinkHelper = (_dispCommandBarControlEvents_SinkHelper)m_aEventSinkHelpers[num];
					m_ConnectionPoint.Unadvise(dispCommandBarControlEvents_SinkHelper.m_dwCookie);
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
			Monitor.Exit(this);
		}
	}

	public void Dispose()
	{
		//Error decoding local variables: Signature type sequence must have at least one element.
		Finalize();
		GC.SuppressFinalize(this);
	}
}

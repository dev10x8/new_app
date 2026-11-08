using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.MyServices.Internal;

[assembly: Guid("49ad2a1b-c572-4a4a-a5e0-8adeca0c92f9")]
[assembly: AssemblyCopyright("Copyright ©  2008")]
[assembly: AssemblyFileVersion("3.0.6.0")]
[assembly: AssemblyProduct("CButton")]
[assembly: AssemblyCompany("SSDiver2112")]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: ComVisible(false)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyTitle("CButton")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyVersion("3.0.6.0")]
namespace CButtonLib.My
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	internal class MyApplication : ApplicationBase
	{
		[DebuggerNonUserCode]
		public MyApplication()
		{
		}
	}
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class MyComputer : Computer
	{
		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public MyComputer()
		{
		}
	}
	[StandardModule]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	[HideModuleName]
	internal sealed class MyProject
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms")]
		internal sealed class MyForms
		{
			[ThreadStatic]
			private static Hashtable m_FormBeingCreated;

			[DebuggerHidden]
			private static T Create__Instance__<T>(T Instance) where T : Form, new()
			{
				if (Instance == null || (((Control)Instance).IsDisposed ? true : false))
				{
					if (m_FormBeingCreated != null)
					{
						if (m_FormBeingCreated.ContainsKey(typeof(T)))
						{
							throw new InvalidOperationException(Utils.GetResourceString("WinForms_RecursiveFormCreate", new string[0]));
						}
					}
					else
					{
						m_FormBeingCreated = new Hashtable();
					}
					m_FormBeingCreated.Add(typeof(T), null);
					try
					{
						return new T();
					}
					catch (TargetInvocationException ex) when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						ProjectData.SetProjectError((Exception)ex);
						return ex.InnerException != null;
					}).Invoke())
					{
						string resourceString = Utils.GetResourceString("WinForms_SeeInnerException", new string[1] { ex.InnerException.Message });
						throw new InvalidOperationException(resourceString, ex.InnerException);
					}
					finally
					{
						m_FormBeingCreated.Remove(typeof(T));
					}
				}
				return Instance;
			}

			[DebuggerHidden]
			private void Dispose__Instance__<T>(ref T instance) where T : Form
			{
				((Component)instance).Dispose();
				instance = default(T);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public MyForms()
			{
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyForms);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			public override string ToString()
			{
				return base.ToString();
			}
		}

		[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class MyWebServices
		{
			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			internal new Type GetType()
			{
				return typeof(MyWebServices);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public override string ToString()
			{
				return base.ToString();
			}

			[DebuggerHidden]
			private static T Create__Instance__<T>(T instance) where T : new()
			{
				if (instance == null)
				{
					return new T();
				}
				return instance;
			}

			[DebuggerHidden]
			private void Dispose__Instance__<T>(ref T instance)
			{
				instance = default(T);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public MyWebServices()
			{
			}
		}

		[ComVisible(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			private readonly ContextValue<T> m_Context;

			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					T val = m_Context.Value;
					if (val == null)
					{
						val = new T();
						m_Context.Value = val;
					}
					return val;
				}
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public ThreadSafeObjectProvider()
			{
				m_Context = new ContextValue<T>();
			}
		}

		private static readonly ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new ThreadSafeObjectProvider<MyComputer>();

		private static readonly ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new ThreadSafeObjectProvider<MyApplication>();

		private static readonly ThreadSafeObjectProvider<User> m_UserObjectProvider = new ThreadSafeObjectProvider<User>();

		private static ThreadSafeObjectProvider<MyForms> m_MyFormsObjectProvider = new ThreadSafeObjectProvider<MyForms>();

		private static readonly ThreadSafeObjectProvider<MyWebServices> m_MyWebServicesObjectProvider = new ThreadSafeObjectProvider<MyWebServices>();

		[HelpKeyword("My.Computer")]
		internal static MyComputer Computer
		{
			[DebuggerHidden]
			get
			{
				return m_ComputerObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.Application")]
		internal static MyApplication Application
		{
			[DebuggerHidden]
			get
			{
				return m_AppObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.User")]
		internal static User User
		{
			[DebuggerHidden]
			get
			{
				return m_UserObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.Forms")]
		internal static MyForms Forms
		{
			[DebuggerHidden]
			get
			{
				return m_MyFormsObjectProvider.GetInstance;
			}
		}

		[HelpKeyword("My.WebServices")]
		internal static MyWebServices WebServices
		{
			[DebuggerHidden]
			get
			{
				return m_MyWebServicesObjectProvider.GetInstance;
			}
		}
	}
}
namespace CButtonLib
{
	[DefaultEvent("ClickButtonArea")]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(CButton), "CButtonLib.CButton.bmp")]
	[DebuggerStepThrough]
	[DesignerGenerated]
	[Designer(typeof(CButtonDesigner))]
	public class CButton : Control, IButtonControl
	{
		public delegate void ClickButtonAreaEventHandler(object Sender, MouseEventArgs e);

		public delegate void ButtonAreaUPEventHandler(object Sender, MouseEventArgs e);

		public delegate void ButtonAreaDownEventHandler(object Sender, MouseEventArgs e);

		public delegate void SideImageClickedEventHandler(object Sender, MouseEventArgs e);

		public enum eMouseDrawState
		{
			Over,
			Down,
			Up
		}

		public enum eShape
		{
			Ellipse,
			Rectangle,
			TriangleUp,
			TriangleDown,
			TriangleLeft,
			TriangleRight
		}

		public enum eFocus
		{
			Dot,
			Dim,
			None
		}

		public enum eFillType
		{
			Solid,
			GradientLinear,
			GradientPath
		}

		private static List<WeakReference> __ENCList = new List<WeakReference>();

		private IContainer components;

		private DialogResult m_DialogResult;

		private bool m_IsDefault;

		private eMouseDrawState MouseDrawState;

		private int PressedOffset;

		private PointF Imagept;

		public RectangleF ButtonArea;

		public RectangleF TextArea;

		private RectangleF ImageArea;

		private Size ImageSizeUse;

		private Color[] _HoverColorBlend;

		private Color[] _ClickColorBlend;

		private Color[] _DisabledBlend;

		private Color _HoverColorSolid;

		private Color _ClickColorSolid;

		private Color _DisabledSolid;

		private Rectangle rectSideImage;

		private const int WM_LBUTTONDBLCLK = 515;

		private const int WM_LBUTTONDOWN = 513;

		private const int WM_LBUTTONUP = 514;

		private const int WM_MBUTTONDBLCLK = 521;

		private const int WM_MBUTTONDOWN = 519;

		private const int WM_MBUTTONUP = 520;

		private const int WM_MOUSEMOVE = 512;

		private const int WM_RBUTTONDBLCLK = 518;

		private const int WM_RBUTTONDOWN = 516;

		private const int WM_RBUTTONUP = 517;

		private CornersProperty _Corners;

		private eShape _Shape;

		private int _DimFactorHover;

		private int _DimFactorClick;

		private Color _BorderColor;

		private bool _BorderShow;

		private eFocus _ShowFocus;

		private eFillType _FillType;

		private LinearGradientMode _FillTypeLinear;

		private Color _ColorFillSolid;

		private cBlendItems _ColorFillBlend;

		private string _Text;

		private TextRenderingHint _TextSmoothingMode;

		private ContentAlignment _TextAlign;

		private Padding _TextMargin;

		private TextImageRelation _TextImageRelation;

		private bool _TextShadowShow;

		private Color _TextShadow;

		private Image _Image;

		private ContentAlignment _ImageAlign;

		private Size _ImageSize;

		private ImageList _Imagelist;

		private int _ImageIndex;

		private Image _SideImage;

		private Size _SideImageSize;

		private bool _SideImageIsClickable;

		private bool _SideImageBehindText;

		private ContentAlignment _SideImageAlign;

		private cFocalPoints _FocalPoints;

		private bool _UseMnemonic;

		[Browsable(false)]
		public bool IsDefault => m_IsDefault;

		[DefaultValue(typeof(DialogResult), "None")]
		[Description("The dialog result produced in a modal form by clicking the button.")]
		[Category("Behavior")]
		public DialogResult DialogResult
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return m_DialogResult;
			}
			set
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				if (Enum.IsDefined(typeof(DialogResult), value))
				{
					m_DialogResult = value;
				}
			}
		}

		[DefaultValue(typeof(Font), "Arial, 10pt, style=Bold")]
		public override Font Font
		{
			get
			{
				return ((Control)this).Font;
			}
			set
			{
				((Control)this).Font = value;
			}
		}

		[DefaultValue(typeof(Color), "White")]
		public override Color ForeColor
		{
			get
			{
				return ((Control)this).ForeColor;
			}
			set
			{
				((Control)this).ForeColor = value;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool DesignerSelected
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("Get or Set the Corner Radii")]
		[Category("Appearance CButton")]
		public CornersProperty Corners
		{
			get
			{
				return _Corners;
			}
			set
			{
				_Corners = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(eShape), "Rectangle")]
		[Description("The Shape of the Button as eShape choices (Ellipse, Rectangle, and Triangles)")]
		[Category("Appearance CButton")]
		public eShape Shape
		{
			get
			{
				return _Shape;
			}
			set
			{
				_Shape = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set how much to dim the color on mouse rollover. Positive to Lighten and negative to Darken")]
		[DefaultValue(50)]
		[Category("Appearance CButton")]
		public int DimFactorHover
		{
			get
			{
				return _DimFactorHover;
			}
			set
			{
				_DimFactorHover = value;
				UpdateDimBlends();
				UpdateDimColors();
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[DefaultValue(-25)]
		[Description("Get or Set how much to dim the color on mouse down. Positive to Lighten and negative to Darken")]
		public int DimFactorClick
		{
			get
			{
				return _DimFactorClick;
			}
			set
			{
				_DimFactorClick = value;
				UpdateDimBlends();
				UpdateDimColors();
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "SteelBlue")]
		[Description("Get or Set to show the Border")]
		[Category("Appearance CButton")]
		public Color BorderColor
		{
			get
			{
				return _BorderColor;
			}
			set
			{
				_BorderColor = value;
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[Description("Get or Set whether to show the Border")]
		[DefaultValue(true)]
		public bool BorderShow
		{
			get
			{
				return _BorderShow;
			}
			set
			{
				_BorderShow = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(eFocus), "Dot")]
		[Category("Appearance CButton")]
		[Description("Get or Set the Focus queue")]
		public eFocus ShowFocus
		{
			get
			{
				return _ShowFocus;
			}
			set
			{
				_ShowFocus = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(eFillType), "GradientLinear")]
		[Description("The Fill Type to apply to the CButton")]
		[Category("Appearance CButton")]
		public eFillType FillType
		{
			get
			{
				return _FillType;
			}
			set
			{
				_FillType = value;
				((Control)this).Invalidate();
			}
		}

		[Description("The Linear Blend type")]
		[DefaultValue(typeof(LinearGradientMode), "Vertical")]
		[Category("Appearance CButton")]
		public LinearGradientMode FillTypeLinear
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _FillTypeLinear;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_FillTypeLinear = value;
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[Description("The Solid Color to fill the CButton")]
		[DefaultValue(typeof(Color), "Control")]
		public Color ColorFillSolid
		{
			get
			{
				return _ColorFillSolid;
			}
			set
			{
				_ColorFillSolid = value;
				UpdateDimColors();
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[RefreshProperties(RefreshProperties.All)]
		[Description("The ColorBlend used to fill the CButton")]
		[Editor(typeof(BlendTypeEditor), typeof(UITypeEditor))]
		public cBlendItems ColorFillBlend
		{
			get
			{
				return _ColorFillBlend;
			}
			set
			{
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				if (value.iColor.Length == value.iPoint.Length)
				{
					value.iPoint[0] = 0f;
					value.iPoint[checked(value.iPoint.Length - 1)] = 1f;
					_ColorFillBlend = value;
					UpdateDimBlends();
					((Control)this).Invalidate();
				}
				else
				{
					Interaction.MsgBox((object)"The number of colors must match the number of positions. The blend will be reset", (MsgBoxStyle)0, (object)null);
				}
			}
		}

		[Description("Get or Set the Button Text")]
		[DefaultValue("CButton")]
		[Category("Appearance CButton")]
		public override string Text
		{
			get
			{
				return _Text;
			}
			set
			{
				_Text = value;
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[DefaultValue(typeof(TextRenderingHint), "ClearTypeGridFit")]
		[Description("Get or Set if the text is rendered with a Smoothing technique")]
		public TextRenderingHint TextSmoothingMode
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _TextSmoothingMode;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_TextSmoothingMode = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
		[Category("Appearance CButton")]
		[Description("Get or Set the alignment for the text")]
		public ContentAlignment TextAlign
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _TextAlign;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_TextAlign = value;
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[Description("Get or Set the margion between the text and the button edge")]
		[DefaultValue(typeof(Padding), "2,2,2,2")]
		public Padding TextMargin
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _TextMargin;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_TextMargin = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(TextImageRelation), "Overlay")]
		[Description("Get or Set the Relationship of the Text to the Image")]
		[Category("Appearance CButton")]
		public TextImageRelation TextImageRelation
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _TextImageRelation;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_TextImageRelation = value;
				((Control)this).Invalidate();
			}
		}

		[Category("Appearance CButton")]
		[DefaultValue(true)]
		[Description("Get or Set if the Text has a shadow")]
		public bool TextShadowShow
		{
			get
			{
				return _TextShadowShow;
			}
			set
			{
				_TextShadowShow = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "MidnightBlue")]
		[Description("Get or Set the color of the Shadow Text")]
		[Category("Appearance CButton")]
		public Color TextShadow
		{
			get
			{
				return _TextShadow;
			}
			set
			{
				_TextShadow = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set the small Image next to text")]
		[Category("Appearance CButton")]
		[DefaultValue(typeof(Image), "none")]
		public Image Image
		{
			get
			{
				return _Image;
			}
			set
			{
				_Image = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set the placement of the Image")]
		[Category("Appearance CButton")]
		[DefaultValue(typeof(ContentAlignment), "MiddleCenter")]
		public ContentAlignment ImageAlign
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _ImageAlign;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_ImageAlign = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(Size), "16, 16")]
		[Description("Get or Set the Size of the Image")]
		[Category("Appearance CButton")]
		public Size ImageSize
		{
			get
			{
				return _ImageSize;
			}
			set
			{
				_ImageSize = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set the ImageList control")]
		[Category("Appearance CButton")]
		public ImageList Imagelist
		{
			get
			{
				return _Imagelist;
			}
			set
			{
				_Imagelist = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set the ImageList control")]
		[Category("Appearance CButton")]
		public int ImageIndex
		{
			get
			{
				return _ImageIndex;
			}
			set
			{
				if (Imagelist.Images.Count > 0 && ((value >= 0) & (value < Imagelist.Images.Count)))
				{
					_ImageIndex = value;
					Image = Imagelist.Images[value];
					((Control)this).Invalidate();
				}
			}
		}

		[Category("Appearance CButton")]
		[Description("Get or Set the Side Image")]
		[DefaultValue(typeof(Image), "none")]
		public Image SideImage
		{
			get
			{
				return _SideImage;
			}
			set
			{
				_SideImage = value;
				if (value != null)
				{
					_SideImageSize = value.Size;
				}
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(Size), "48, 48")]
		[Description("Get or Set the Size of the Side Image")]
		[Category("Appearance CButton")]
		public Size SideImageSize
		{
			get
			{
				return _SideImageSize;
			}
			set
			{
				_SideImageSize = value;
				((Control)this).Invalidate();
			}
		}

		[Description("Get or Set if the Side Image raises its own click event")]
		[Category("Appearance CButton")]
		[DefaultValue(false)]
		public bool SideImageIsClickable
		{
			get
			{
				return _SideImageIsClickable;
			}
			set
			{
				_SideImageIsClickable = value;
			}
		}

		[Category("Appearance CButton")]
		[DefaultValue(true)]
		[Description("Get or Set if the Side Image is in front or behind the Text")]
		public bool SideImageBehindText
		{
			get
			{
				return _SideImageBehindText;
			}
			set
			{
				_SideImageBehindText = value;
				((Control)this).Invalidate();
			}
		}

		[DefaultValue(typeof(ContentAlignment), "MiddleLeft")]
		[Description("Get or Set the Side Image Alignment")]
		[Category("Appearance CButton")]
		public ContentAlignment SideImageAlign
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _SideImageAlign;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_SideImageAlign = value;
				((Control)this).Invalidate();
			}
		}

		[Description("The CenterPoint and FocusScales for the ColorBlend")]
		[Category("Appearance CButton")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public cFocalPoints FocalPoints
		{
			get
			{
				return _FocalPoints;
			}
			set
			{
				_FocalPoints = value;
				((Control)this).Invalidate();
			}
		}

		[Description("If true, the first character proceeded by an ampersand (&) will be used as the button's mnemonic key.")]
		[Category("Appearance CButton")]
		[DefaultValue(true)]
		public bool UseMnemonic
		{
			get
			{
				return _UseMnemonic;
			}
			set
			{
				_UseMnemonic = value;
				((Control)this).Invalidate();
			}
		}

		[method: DebuggerNonUserCode]
		public event ClickButtonAreaEventHandler ClickButtonArea;

		[method: DebuggerNonUserCode]
		public event ButtonAreaUPEventHandler ButtonAreaUP;

		[method: DebuggerNonUserCode]
		public event ButtonAreaDownEventHandler ButtonAreaDown;

		[method: DebuggerNonUserCode]
		public event SideImageClickedEventHandler SideImageClicked;

		[DebuggerNonUserCode]
		private static void __ENCAddToList(object value)
		{
			checked
			{
				lock (__ENCList)
				{
					if (__ENCList.Count == __ENCList.Capacity)
					{
						int num = 0;
						int num2 = __ENCList.Count - 1;
						int num3 = 0;
						while (true)
						{
							int num4 = num3;
							int num5 = num2;
							if (num4 > num5)
							{
								break;
							}
							WeakReference weakReference = __ENCList[num3];
							if (weakReference.IsAlive)
							{
								if (num3 != num)
								{
									__ENCList[num] = __ENCList[num3];
								}
								num++;
							}
							num3++;
						}
						__ENCList.RemoveRange(num, __ENCList.Count - num);
						__ENCList.Capacity = __ENCList.Count;
					}
					__ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
				}
			}
		}

		[DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if ((disposing && components != null) ? true : false)
				{
					components.Dispose();
				}
			}
			finally
			{
				((Control)this).Dispose(disposing);
			}
		}

		[System.Diagnostics.DebuggerStepThrough]
		private void InitializeComponent()
		{
			((Control)this).SuspendLayout();
			System.Drawing.Size size = new System.Drawing.Size(75, 25);
			((Control)this).Size = size;
			((Control)this).ResumeLayout(false);
		}

		public void NotifyDefault(bool value)
		{
			if (m_IsDefault != value)
			{
				m_IsDefault = value;
			}
			((Control)this).Invalidate();
		}

		public void PerformClick()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			if (((Control)this).CanSelect)
			{
				PerformClickButtonArea(this, checked(new MouseEventArgs((MouseButtons)1048576, 1, (int)Math.Round(ButtonArea.X), (int)Math.Round(ButtonArea.Y), 0)));
			}
		}

		private void PerformClickButtonArea(object Sender, MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if ((int)m_DialogResult != 0)
			{
				((Control)this).FindForm().DialogResult = m_DialogResult;
			}
			ClickButtonArea?.Invoke(RuntimeHelpers.GetObjectValue(Sender), e);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public CButton()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Expected O, but got Unknown
			((Control)this).MouseMove += new MouseEventHandler(CButton_MouseMove);
			((Control)this).MouseLeave += CButton_MouseLeave;
			((Control)this).GotFocus += CButton_GotFocus;
			((Control)this).LostFocus += CButton_LostFocus;
			((Control)this).KeyDown += new KeyEventHandler(CButton_KeyDown);
			((Control)this).KeyUp += new KeyEventHandler(CButton_KeyUp);
			__ENCAddToList(this);
			MouseDrawState = eMouseDrawState.Up;
			_Corners = DefaultCorners();
			_Shape = eShape.Rectangle;
			_DimFactorHover = 50;
			_DimFactorClick = -25;
			_BorderColor = Color.SteelBlue;
			_BorderShow = true;
			_ShowFocus = eFocus.Dot;
			_FillType = eFillType.GradientLinear;
			_FillTypeLinear = (LinearGradientMode)1;
			_ColorFillSolid = SystemColors.Control;
			_ColorFillBlend = DefaultColorFillBlend();
			_Text = "CButton";
			_TextSmoothingMode = (TextRenderingHint)5;
			_TextAlign = (ContentAlignment)32;
			((Padding)(ref _TextMargin))..ctor(2);
			_TextImageRelation = (TextImageRelation)0;
			_TextShadowShow = true;
			_TextShadow = Color.MidnightBlue;
			_ImageAlign = (ContentAlignment)32;
			ref Size imageSize = ref _ImageSize;
			imageSize = new Size(16, 16);
			_Imagelist = new ImageList();
			ref Size sideImageSize = ref _SideImageSize;
			sideImageSize = new Size(48, 48);
			_SideImageBehindText = true;
			_SideImageAlign = (ContentAlignment)16;
			_FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
			_UseMnemonic = true;
			InitializeComponent();
			((Control)this).SetStyle((ControlStyles)76306, true);
			Size size = new Size(90, 25);
			((Control)this).Size = size;
			ForeColor = Color.White;
			Font = new Font("Arial", 10f, (FontStyle)1);
			UpdateDimColors();
			UpdateDimBlends();
		}

		public CButton(string text)
			: base(text)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Expected O, but got Unknown
			((Control)this).MouseMove += new MouseEventHandler(CButton_MouseMove);
			((Control)this).MouseLeave += CButton_MouseLeave;
			((Control)this).GotFocus += CButton_GotFocus;
			((Control)this).LostFocus += CButton_LostFocus;
			((Control)this).KeyDown += new KeyEventHandler(CButton_KeyDown);
			((Control)this).KeyUp += new KeyEventHandler(CButton_KeyUp);
			__ENCAddToList(this);
			MouseDrawState = eMouseDrawState.Up;
			_Corners = DefaultCorners();
			_Shape = eShape.Rectangle;
			_DimFactorHover = 50;
			_DimFactorClick = -25;
			_BorderColor = Color.SteelBlue;
			_BorderShow = true;
			_ShowFocus = eFocus.Dot;
			_FillType = eFillType.GradientLinear;
			_FillTypeLinear = (LinearGradientMode)1;
			_ColorFillSolid = SystemColors.Control;
			_ColorFillBlend = DefaultColorFillBlend();
			_Text = "CButton";
			_TextSmoothingMode = (TextRenderingHint)5;
			_TextAlign = (ContentAlignment)32;
			((Padding)(ref _TextMargin))..ctor(2);
			_TextImageRelation = (TextImageRelation)0;
			_TextShadowShow = true;
			_TextShadow = Color.MidnightBlue;
			_ImageAlign = (ContentAlignment)32;
			ref Size imageSize = ref _ImageSize;
			imageSize = new Size(16, 16);
			_Imagelist = new ImageList();
			ref Size sideImageSize = ref _SideImageSize;
			sideImageSize = new Size(48, 48);
			_SideImageBehindText = true;
			_SideImageAlign = (ContentAlignment)16;
			_FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
			_UseMnemonic = true;
			InitializeComponent();
			((Control)this).SetStyle((ControlStyles)76306, true);
			Size size = new Size(90, 25);
			((Control)this).Size = size;
			ForeColor = Color.White;
			Font = new Font("Arial", 10f, (FontStyle)1);
			UpdateDimColors();
			UpdateDimBlends();
		}

		public CButton(string text, int left, int top, int width, int height)
			: base(text, left, top, width, height)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Expected O, but got Unknown
			((Control)this).MouseMove += new MouseEventHandler(CButton_MouseMove);
			((Control)this).MouseLeave += CButton_MouseLeave;
			((Control)this).GotFocus += CButton_GotFocus;
			((Control)this).LostFocus += CButton_LostFocus;
			((Control)this).KeyDown += new KeyEventHandler(CButton_KeyDown);
			((Control)this).KeyUp += new KeyEventHandler(CButton_KeyUp);
			__ENCAddToList(this);
			MouseDrawState = eMouseDrawState.Up;
			_Corners = DefaultCorners();
			_Shape = eShape.Rectangle;
			_DimFactorHover = 50;
			_DimFactorClick = -25;
			_BorderColor = Color.SteelBlue;
			_BorderShow = true;
			_ShowFocus = eFocus.Dot;
			_FillType = eFillType.GradientLinear;
			_FillTypeLinear = (LinearGradientMode)1;
			_ColorFillSolid = SystemColors.Control;
			_ColorFillBlend = DefaultColorFillBlend();
			_Text = "CButton";
			_TextSmoothingMode = (TextRenderingHint)5;
			_TextAlign = (ContentAlignment)32;
			((Padding)(ref _TextMargin))..ctor(2);
			_TextImageRelation = (TextImageRelation)0;
			_TextShadowShow = true;
			_TextShadow = Color.MidnightBlue;
			_ImageAlign = (ContentAlignment)32;
			ref Size imageSize = ref _ImageSize;
			imageSize = new Size(16, 16);
			_Imagelist = new ImageList();
			ref Size sideImageSize = ref _SideImageSize;
			sideImageSize = new Size(48, 48);
			_SideImageBehindText = true;
			_SideImageAlign = (ContentAlignment)16;
			_FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
			_UseMnemonic = true;
			InitializeComponent();
			((Control)this).SetStyle((ControlStyles)76306, true);
			Size size = new Size(90, 25);
			((Control)this).Size = size;
			ForeColor = Color.White;
			Font = new Font("Arial", 10f, (FontStyle)1);
			UpdateDimColors();
			UpdateDimBlends();
		}

		public CButton(Control parent, string text)
			: base(parent, text)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Expected O, but got Unknown
			((Control)this).MouseMove += new MouseEventHandler(CButton_MouseMove);
			((Control)this).MouseLeave += CButton_MouseLeave;
			((Control)this).GotFocus += CButton_GotFocus;
			((Control)this).LostFocus += CButton_LostFocus;
			((Control)this).KeyDown += new KeyEventHandler(CButton_KeyDown);
			((Control)this).KeyUp += new KeyEventHandler(CButton_KeyUp);
			__ENCAddToList(this);
			MouseDrawState = eMouseDrawState.Up;
			_Corners = DefaultCorners();
			_Shape = eShape.Rectangle;
			_DimFactorHover = 50;
			_DimFactorClick = -25;
			_BorderColor = Color.SteelBlue;
			_BorderShow = true;
			_ShowFocus = eFocus.Dot;
			_FillType = eFillType.GradientLinear;
			_FillTypeLinear = (LinearGradientMode)1;
			_ColorFillSolid = SystemColors.Control;
			_ColorFillBlend = DefaultColorFillBlend();
			_Text = "CButton";
			_TextSmoothingMode = (TextRenderingHint)5;
			_TextAlign = (ContentAlignment)32;
			((Padding)(ref _TextMargin))..ctor(2);
			_TextImageRelation = (TextImageRelation)0;
			_TextShadowShow = true;
			_TextShadow = Color.MidnightBlue;
			_ImageAlign = (ContentAlignment)32;
			ref Size imageSize = ref _ImageSize;
			imageSize = new Size(16, 16);
			_Imagelist = new ImageList();
			ref Size sideImageSize = ref _SideImageSize;
			sideImageSize = new Size(48, 48);
			_SideImageBehindText = true;
			_SideImageAlign = (ContentAlignment)16;
			_FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
			_UseMnemonic = true;
			InitializeComponent();
			((Control)this).SetStyle((ControlStyles)76306, true);
			Size size = new Size(90, 25);
			((Control)this).Size = size;
			ForeColor = Color.White;
			Font = new Font("Arial", 10f, (FontStyle)1);
			UpdateDimColors();
			UpdateDimBlends();
		}

		public CButton(Control parent, string text, int left, int top, int width, int height)
			: base(parent, text, left, top, width, height)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Expected O, but got Unknown
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			((Control)this).MouseMove += new MouseEventHandler(CButton_MouseMove);
			((Control)this).MouseLeave += CButton_MouseLeave;
			((Control)this).GotFocus += CButton_GotFocus;
			((Control)this).LostFocus += CButton_LostFocus;
			((Control)this).KeyDown += new KeyEventHandler(CButton_KeyDown);
			((Control)this).KeyUp += new KeyEventHandler(CButton_KeyUp);
			__ENCAddToList(this);
			MouseDrawState = eMouseDrawState.Up;
			_Corners = DefaultCorners();
			_Shape = eShape.Rectangle;
			_DimFactorHover = 50;
			_DimFactorClick = -25;
			_BorderColor = Color.SteelBlue;
			_BorderShow = true;
			_ShowFocus = eFocus.Dot;
			_FillType = eFillType.GradientLinear;
			_FillTypeLinear = (LinearGradientMode)1;
			_ColorFillSolid = SystemColors.Control;
			_ColorFillBlend = DefaultColorFillBlend();
			_Text = "CButton";
			_TextSmoothingMode = (TextRenderingHint)5;
			_TextAlign = (ContentAlignment)32;
			((Padding)(ref _TextMargin))..ctor(2);
			_TextImageRelation = (TextImageRelation)0;
			_TextShadowShow = true;
			_TextShadow = Color.MidnightBlue;
			_ImageAlign = (ContentAlignment)32;
			ref Size imageSize = ref _ImageSize;
			imageSize = new Size(16, 16);
			_Imagelist = new ImageList();
			ref Size sideImageSize = ref _SideImageSize;
			sideImageSize = new Size(48, 48);
			_SideImageBehindText = true;
			_SideImageAlign = (ContentAlignment)16;
			_FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
			_UseMnemonic = true;
			InitializeComponent();
			((Control)this).SetStyle((ControlStyles)76306, true);
			Size size = new Size(90, 25);
			((Control)this).Size = size;
			ForeColor = Color.White;
			Font = new Font("Arial", 10f, (FontStyle)1);
			UpdateDimColors();
			UpdateDimBlends();
		}

		private CornersProperty DefaultCorners()
		{
			return new CornersProperty(0);
		}

		public void ResetCorners()
		{
			Corners = DefaultCorners();
		}

		private bool ShouldSerializeCorners()
		{
			return !_Corners.Equals(DefaultCorners());
		}

		private cBlendItems DefaultColorFillBlend()
		{
			return new cBlendItems(new Color[3]
			{
				Color.AliceBlue,
				Color.RoyalBlue,
				Color.Navy
			}, new float[3] { 0f, 0.5f, 1f });
		}

		public void ResetColorFillBlend()
		{
			ColorFillBlend = DefaultColorFillBlend();
		}

		private bool ShouldSerializeColorFillBlend()
		{
			return !_ColorFillBlend.Equals(DefaultColorFillBlend());
		}

		private cFocalPoints DefaultFocalPoints()
		{
			return new cFocalPoints(0.5f, 0.5f, 0f, 0f);
		}

		public void ResetFocalPoints()
		{
			FocalPoints = new cFocalPoints(0.5f, 0.5f, 0f, 0f);
		}

		private bool ShouldSerializeFocalPoints()
		{
			return !_FocalPoints.Equals(new cFocalPoints(0.5f, 0.5f, 0f, 0f));
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Expected O, but got Unknown
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Expected O, but got Unknown
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Expected O, but got Unknown
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Expected O, but got Unknown
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0470: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Expected O, but got Unknown
			//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0602: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Expected O, but got Unknown
			//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Expected O, but got Unknown
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_0576: Expected O, but got Unknown
			//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e2: Expected O, but got Unknown
			e.Graphics.SmoothingMode = (SmoothingMode)4;
			e.Graphics.TextRenderingHint = _TextSmoothingMode;
			Color color;
			Color color2;
			Color color3;
			if (((Control)this).Enabled)
			{
				color = _BorderColor;
				color2 = ForeColor;
				color3 = _TextShadow;
			}
			else
			{
				color = GrayTheColor(_BorderColor);
				color2 = GrayTheColor(ForeColor);
				color3 = GrayTheColor(_TextShadow);
			}
			Pen val = new Pen(color);
			checked
			{
				try
				{
					val.Alignment = (PenAlignment)1;
					RectangleF baseRect = new RectangleF(0f, 0f, ((Control)this).Size.Width - 1, ((Control)this).Size.Height - 1);
					ButtonArea = AdjustRect(baseRect, ((Control)this).Padding);
					GraphicsPath path = GetPath();
					try
					{
						if (((Control)this).BackgroundImage == null)
						{
							switch (unchecked((int)FillType))
							{
							case 0:
							{
								Brush val6 = (Brush)new SolidBrush(GetFill());
								try
								{
									e.Graphics.FillPath(val6, path);
								}
								finally
								{
									((IDisposable)val6)?.Dispose();
								}
								break;
							}
							case 2:
							{
								PathGradientBrush val4 = new PathGradientBrush(path);
								try
								{
									ColorBlend val5 = new ColorBlend();
									val5.Colors = GetFillBlend();
									val5.Positions = ColorFillBlend.iPoint;
									val4.FocusScales = FocalPoints.FocusScales();
									Padding padding = ((Control)this).Padding;
									float x = (float)((Padding)(ref padding)).Left + ButtonArea.Width * FocalPoints.CenterPoint().X;
									Padding padding2 = ((Control)this).Padding;
									PointF centerPoint = new PointF(x, (float)((Padding)(ref padding2)).Top + ButtonArea.Height * FocalPoints.CenterPoint().Y);
									val4.CenterPoint = centerPoint;
									val4.InterpolationColors = val5;
									e.Graphics.FillPath((Brush)(object)val4, path);
								}
								finally
								{
									((IDisposable)val4)?.Dispose();
								}
								break;
							}
							case 1:
							{
								LinearGradientBrush val2 = new LinearGradientBrush(ButtonArea, Color.White, Color.White, FillTypeLinear);
								try
								{
									ColorBlend val3 = new ColorBlend();
									val3.Colors = GetFillBlend();
									val3.Positions = ColorFillBlend.iPoint;
									val2.InterpolationColors = val3;
									e.Graphics.FillPath((Brush)(object)val2, path);
								}
								finally
								{
									((IDisposable)val2)?.Dispose();
								}
								break;
							}
							}
						}
						if ((((Control)this).Focused && ShowFocus == eFocus.Dot) ? true : false)
						{
							Pen val7 = new Pen(Brushes.Black, 1f);
							val7.DashStyle = (DashStyle)2;
							Pen val8 = val7;
							try
							{
								e.Graphics.DrawPath(val8, GetPath(-1, -1));
							}
							finally
							{
								((IDisposable)val8)?.Dispose();
							}
						}
						if (BorderShow)
						{
							e.Graphics.DrawPath(val, path);
						}
					}
					finally
					{
						((IDisposable)path)?.Dispose();
					}
					PointF pointF = ImageLocation(GetStringFormat(SideImageAlign), ((Control)this).Size, SideImageSize);
					ref Rectangle reference = ref rectSideImage;
					reference = new Rectangle((int)Math.Round(pointF.X), (int)Math.Round(pointF.Y), SideImageSize.Width, SideImageSize.Height);
					if ((SideImageBehindText && SideImage != null) ? true : false)
					{
						if (((Control)this).Enabled)
						{
							e.Graphics.SmoothingMode = (SmoothingMode)3;
							e.Graphics.DrawImage(SideImage, pointF.X, pointF.Y, (float)SideImageSize.Width, (float)SideImageSize.Height);
						}
						else
						{
							ControlPaint.DrawImageDisabled(e.Graphics, (Image)new Bitmap(SideImage, SideImageSize.Width, SideImageSize.Height), (int)Math.Round(pointF.X), (int)Math.Round(pointF.Y), ((Control)this).BackColor);
						}
					}
					SetImageAndText(e.Graphics);
					if (Image != null)
					{
						if (((Control)this).Enabled)
						{
							e.Graphics.DrawImage(Image, Imagept.X, Imagept.Y, (float)ImageSize.Width, (float)ImageSize.Height);
						}
						else
						{
							ControlPaint.DrawImageDisabled(e.Graphics, (Image)new Bitmap(Image, ImageSize.Width, ImageSize.Height), (int)Math.Round(Imagept.X), (int)Math.Round(Imagept.Y), ((Control)this).BackColor);
						}
					}
					if (TextShadowShow)
					{
						TextArea.Offset(1f, 1f);
						e.Graphics.DrawString(Text, Font, (Brush)new SolidBrush(color3), TextArea, GetStringFormat(TextAlign));
						TextArea.Offset(-1f, -1f);
					}
					e.Graphics.DrawString(Text, Font, (Brush)new SolidBrush(color2), TextArea, GetStringFormat(TextAlign));
					if ((!SideImageBehindText && SideImage != null) ? true : false)
					{
						if (((Control)this).Enabled)
						{
							e.Graphics.SmoothingMode = (SmoothingMode)3;
							e.Graphics.DrawImage(SideImage, pointF.X, pointF.Y, (float)SideImageSize.Width, (float)SideImageSize.Height);
						}
						else
						{
							ControlPaint.DrawImageDisabled(e.Graphics, (Image)new Bitmap(SideImage, SideImageSize.Width, SideImageSize.Height), (int)Math.Round(pointF.X), (int)Math.Round(pointF.Y), ((Control)this).BackColor);
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}

		private GraphicsPath GetPath(int infX = 0, int infY = 0)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			RectangleF buttonArea = ButtonArea;
			if ((infX != 0 && infY != 0) ? true : false)
			{
				buttonArea.Inflate(infX, infY);
			}
			switch ((int)_Shape)
			{
			case 0:
				val.AddEllipse(buttonArea);
				break;
			case 1:
				val = GetRoundedRectPath(buttonArea);
				break;
			case 2:
			{
				PointF[] array = new PointF[3];
				ref PointF reference10 = ref array[0];
				PointF pointF3 = new PointF(buttonArea.Width / 2f, buttonArea.Y);
				reference10 = pointF3;
				ref PointF reference11 = ref array[1];
				PointF pointF2 = new PointF(buttonArea.Width, buttonArea.Y + buttonArea.Height);
				reference11 = pointF2;
				ref PointF reference12 = ref array[2];
				PointF pointF = new PointF(buttonArea.X, buttonArea.Y + buttonArea.Height);
				reference12 = pointF;
				PointF[] array5 = array;
				val.AddPolygon(array5);
				break;
			}
			case 3:
			{
				PointF[] array = new PointF[3];
				ref PointF reference7 = ref array[0];
				PointF pointF = new PointF(buttonArea.X, buttonArea.Y);
				reference7 = pointF;
				ref PointF reference8 = ref array[1];
				PointF pointF2 = new PointF(buttonArea.Width / 2f, buttonArea.Y + buttonArea.Height);
				reference8 = pointF2;
				ref PointF reference9 = ref array[2];
				PointF pointF3 = new PointF(buttonArea.X + buttonArea.Width, buttonArea.Y);
				reference9 = pointF3;
				PointF[] array4 = array;
				val.AddPolygon(array4);
				break;
			}
			case 4:
			{
				PointF[] array = new PointF[3];
				ref PointF reference4 = ref array[0];
				PointF pointF = new PointF(buttonArea.X, buttonArea.Y + buttonArea.Height / 2f);
				reference4 = pointF;
				ref PointF reference5 = ref array[1];
				PointF pointF2 = new PointF(buttonArea.Width, buttonArea.Y);
				reference5 = pointF2;
				ref PointF reference6 = ref array[2];
				PointF pointF3 = new PointF(buttonArea.Width, buttonArea.Y + buttonArea.Height);
				reference6 = pointF3;
				PointF[] array3 = array;
				val.AddPolygon(array3);
				break;
			}
			case 5:
			{
				PointF[] array = new PointF[3];
				ref PointF reference = ref array[0];
				PointF pointF = new PointF(buttonArea.X, buttonArea.Y);
				reference = pointF;
				ref PointF reference2 = ref array[1];
				PointF pointF2 = new PointF(buttonArea.Width, buttonArea.Y + buttonArea.Height / 2f);
				reference2 = pointF2;
				ref PointF reference3 = ref array[2];
				PointF pointF3 = new PointF(buttonArea.X, buttonArea.Y + buttonArea.Height);
				reference3 = pointF3;
				PointF[] array2 = array;
				val.AddPolygon(array2);
				break;
			}
			}
			return val;
		}

		private StringFormat GetStringFormat(ContentAlignment ctrlalign)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Invalid comparison between Unknown and I4
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Invalid comparison between Unknown and I4
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Invalid comparison between Unknown and I4
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Invalid comparison between Unknown and I4
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Invalid comparison between Unknown and I4
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Invalid comparison between Unknown and I4
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Invalid comparison between Unknown and I4
			StringFormat val = new StringFormat();
			if ((int)ctrlalign == 32)
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)1;
			}
			else if ((int)ctrlalign == 16)
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)0;
			}
			else if ((int)ctrlalign == 64)
			{
				val.LineAlignment = (StringAlignment)1;
				val.Alignment = (StringAlignment)2;
			}
			else if ((int)ctrlalign == 2)
			{
				val.LineAlignment = (StringAlignment)0;
				val.Alignment = (StringAlignment)1;
			}
			else if ((int)ctrlalign == 1)
			{
				val.LineAlignment = (StringAlignment)0;
				val.Alignment = (StringAlignment)0;
			}
			else if ((int)ctrlalign == 4)
			{
				val.LineAlignment = (StringAlignment)0;
				val.Alignment = (StringAlignment)2;
			}
			else if ((int)ctrlalign == 512)
			{
				val.LineAlignment = (StringAlignment)2;
				val.Alignment = (StringAlignment)1;
			}
			else if ((int)ctrlalign == 256)
			{
				val.LineAlignment = (StringAlignment)2;
				val.Alignment = (StringAlignment)0;
			}
			else if ((int)ctrlalign == 1024)
			{
				val.LineAlignment = (StringAlignment)2;
				val.Alignment = (StringAlignment)2;
			}
			if (_UseMnemonic)
			{
				val.HotkeyPrefix = (HotkeyPrefix)1;
			}
			else
			{
				val.HotkeyPrefix = (HotkeyPrefix)0;
			}
			return val;
		}

		private void SetImageAndText(Graphics g)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected I4, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Expected I4, but got Unknown
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0421: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Expected I4, but got Unknown
			if (MouseDrawState == eMouseDrawState.Down)
			{
				PressedOffset = 1;
			}
			else
			{
				PressedOffset = 0;
			}
			if (Image != null)
			{
				ImageSizeUse = ImageSize;
			}
			else
			{
				ref Size imageSizeUse = ref ImageSizeUse;
				imageSizeUse = new Size(0, 0);
			}
			switch ((int)TextImageRelation)
			{
			case 0:
			case 1:
			case 2:
				TextArea = AdjustRect(ButtonArea, TextMargin);
				ImageArea = ButtonArea;
				TextArea.Y += (float)PressedOffset;
				Imagept = ImageLocation(GetStringFormat(ImageAlign), ButtonArea.Size, ImageSizeUse);
				Imagept.X += ButtonArea.X;
				break;
			case 4:
			{
				SizeF sizeF2 = g.MeasureString(Text, Font);
				TextArea = AdjustRect(ButtonArea, TextMargin);
				TextArea.Width -= (float)checked(ImageSizeUse.Width - 4);
				TextArea.Y += (float)PressedOffset;
				ref RectangleF imageArea2 = ref ImageArea;
				imageArea2 = new RectangleF(TextArea.X - (float)ImageSizeUse.Width, ButtonArea.Y, ImageSizeUse.Width, ImageSizeUse.Height);
				Imagept = ImageLocation(GetStringFormat(ImageAlign), ButtonArea.Size, ImageArea.Size);
				switch ((int)GetStringFormat(TextAlign).Alignment)
				{
				case 1:
					Imagept.X = ButtonArea.X + (ButtonArea.Width - sizeF2.Width - (float)ImageSizeUse.Width) / 2f;
					TextArea.X = ButtonArea.X + (float)ImageSizeUse.Width;
					break;
				case 0:
					Imagept.X = ButtonArea.X + 4f;
					TextArea.X = ButtonArea.X + (float)ImageSizeUse.Width + 4f;
					break;
				case 2:
					Imagept.X = ButtonArea.X + TextArea.Width - sizeF2.Width - 12f;
					TextArea.X = ButtonArea.X + (float)ImageSizeUse.Width - 8f;
					break;
				}
				break;
			}
			case 8:
			{
				SizeF sizeF = g.MeasureString(Text, Font);
				TextArea = AdjustRect(ButtonArea, TextMargin);
				TextArea.Width -= (float)checked(ImageSizeUse.Width - 8);
				TextArea.Y += (float)PressedOffset;
				ref RectangleF imageArea = ref ImageArea;
				imageArea = new RectangleF(TextArea.X, ButtonArea.Y, ImageSizeUse.Width, ImageSizeUse.Height);
				Imagept = ImageLocation(GetStringFormat(ImageAlign), ButtonArea.Size, ImageArea.Size);
				switch ((int)GetStringFormat(TextAlign).Alignment)
				{
				case 1:
					Imagept.X = (TextArea.Width - sizeF.Width) / 2f + sizeF.Width;
					TextArea.X = -4f;
					break;
				case 0:
					Imagept.X = sizeF.Width + 8f;
					TextArea.X = 4f;
					break;
				case 2:
					Imagept.X = TextArea.Width - 12f;
					TextArea.X = -16f;
					break;
				}
				break;
			}
			}
			Imagept.Y += (float)PressedOffset + ButtonArea.Y;
		}

		private static PointF ImageLocation(StringFormat sf, SizeF Area, SizeF ImageArea)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected I4, but got Unknown
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected I4, but got Unknown
			PointF result = default(PointF);
			switch ((int)sf.Alignment)
			{
			case 1:
				result.X = (Area.Width - ImageArea.Width) / 2f;
				break;
			case 0:
				result.X = 2f;
				break;
			case 2:
				result.X = Area.Width - ImageArea.Width - 2f;
				break;
			}
			switch ((int)sf.LineAlignment)
			{
			case 1:
				result.Y = (Area.Height - ImageArea.Height) / 2f;
				break;
			case 0:
				result.Y = 2f;
				break;
			case 2:
				result.Y = Area.Height - ImageArea.Height - 2f;
				break;
			}
			return result;
		}

		private void UpdateDimBlends()
		{
			_ClickColorBlend = (Color[])_ColorFillBlend.iColor.Clone();
			_HoverColorBlend = (Color[])_ColorFillBlend.iColor.Clone();
			_DisabledBlend = (Color[])_ColorFillBlend.iColor.Clone();
			checked
			{
				int num = _ColorFillBlend.iColor.Length - 1;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 <= num4)
					{
						Color color = _ColorFillBlend.iColor[num2];
						ref Color reference = ref _ClickColorBlend[num2];
						reference = DimTheColor(color, _DimFactorClick);
						ref Color reference2 = ref _HoverColorBlend[num2];
						reference2 = DimTheColor(color, _DimFactorHover);
						ref Color reference3 = ref _DisabledBlend[num2];
						reference3 = GrayTheColor(color);
						num2++;
						continue;
					}
					break;
				}
			}
		}

		private void UpdateDimColors()
		{
			_ClickColorSolid = DimTheColor(_ColorFillSolid, _DimFactorClick);
			_HoverColorSolid = DimTheColor(_ColorFillSolid, _DimFactorHover);
			_DisabledSolid = GrayTheColor(_ColorFillSolid);
		}

		public static Color DimTheColor(Color DimColor, int DimDegree)
		{
			if (DimColor == Color.Transparent || DimDegree == 0)
			{
				return DimColor;
			}
			checked
			{
				int num = DimColor.R + DimDegree;
				int num2 = DimColor.G + DimDegree;
				int num3 = DimColor.B + DimDegree;
				if (num > 255)
				{
					num = 255;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				if (num < 0)
				{
					num = 0;
				}
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num3 < 0)
				{
					num3 = 0;
				}
				return Color.FromArgb(num, num2, num3);
			}
		}

		public static Color GrayTheColor(Color GrayColor)
		{
			checked
			{
				int num = (int)Math.Round(unchecked((double)(int)GrayColor.R * 0.3 + (double)(int)GrayColor.G * 0.59 + (double)(int)GrayColor.B * 0.11));
				return Color.FromArgb(GrayColor.A, num, num, num);
			}
		}

		private Color[] GetFillBlend()
		{
			if (((Control)this).Enabled)
			{
				if (MouseDrawState == eMouseDrawState.Down)
				{
					return _ClickColorBlend;
				}
				if (MouseDrawState == eMouseDrawState.Over || (((ShowFocus == eFocus.Dim) & ((Control)this).Focused) ? true : false))
				{
					return _HoverColorBlend;
				}
				return _ColorFillBlend.iColor;
			}
			return _DisabledBlend;
		}

		private Color GetFill()
		{
			if (((Control)this).Enabled)
			{
				if (MouseDrawState == eMouseDrawState.Down)
				{
					return _ClickColorSolid;
				}
				if (MouseDrawState == eMouseDrawState.Over || (((ShowFocus == eFocus.Dim) & ((Control)this).Focused) ? true : false))
				{
					return _HoverColorSolid;
				}
				return _ColorFillSolid;
			}
			return _DisabledSolid;
		}

		private static RectangleF AdjustRect(RectangleF BaseRect, Padding Pad)
		{
			BaseRect.Width = Math.Max(BaseRect.Width - (float)((Padding)(ref Pad)).Horizontal, 5f);
			BaseRect.Height = Math.Max(BaseRect.Height - (float)((Padding)(ref Pad)).Vertical, 5f);
			BaseRect.Offset(((Padding)(ref Pad)).Left, ((Padding)(ref Pad)).Top);
			return BaseRect;
		}

		private static Rectangle AdjustRect(Rectangle BaseRect, Padding Pad)
		{
			checked
			{
				BaseRect.Width = Math.Max(BaseRect.Width - ((Padding)(ref Pad)).Horizontal, 5);
				BaseRect.Height = Math.Max(BaseRect.Height - ((Padding)(ref Pad)).Vertical, 5);
				BaseRect.Offset(((Padding)(ref Pad)).Left, ((Padding)(ref Pad)).Top);
				return BaseRect;
			}
		}

		private GraphicsPath GetRoundedRectPath(RectangleF BaseRect)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			checked
			{
				RectangleF rectangleF;
				if (Corners.All == -1)
				{
					GraphicsPath val2 = val;
					if (Corners.UpperLeft == 0)
					{
						val2.AddLine(BaseRect.X, BaseRect.Y, BaseRect.X, BaseRect.Y);
					}
					else
					{
						PointF location = BaseRect.Location;
						SizeF size = new SizeF(Corners.UpperLeft * 2, Corners.UpperLeft * 2);
						rectangleF = new RectangleF(location, size);
						val2.AddArc(rectangleF, 180f, 90f);
					}
					if (Corners.UpperRight == 0)
					{
						val2.AddLine(BaseRect.X + (float)Corners.UpperLeft, BaseRect.Y, BaseRect.Right - (float)Corners.UpperRight, BaseRect.Top);
					}
					else
					{
						PointF location2 = BaseRect.Location;
						SizeF size = new SizeF(Corners.UpperRight * 2, Corners.UpperRight * 2);
						rectangleF = new RectangleF(location2, size);
						rectangleF.X = BaseRect.Right - (float)(Corners.UpperRight * 2);
						val2.AddArc(rectangleF, 270f, 90f);
					}
					if (Corners.LowerRight == 0)
					{
						val2.AddLine(BaseRect.Right, BaseRect.Top + (float)Corners.UpperRight, BaseRect.Right, BaseRect.Bottom - (float)Corners.LowerRight);
					}
					else
					{
						PointF location3 = BaseRect.Location;
						SizeF size = new SizeF(Corners.LowerRight * 2, Corners.LowerRight * 2);
						rectangleF = new RectangleF(location3, size);
						rectangleF.Y = BaseRect.Bottom - (float)(Corners.LowerRight * 2);
						rectangleF.X = BaseRect.Right - (float)(Corners.LowerRight * 2);
						val2.AddArc(rectangleF, 0f, 90f);
					}
					if (Corners.LowerLeft == 0)
					{
						val2.AddLine(BaseRect.Right - (float)Corners.LowerRight, BaseRect.Bottom, BaseRect.X - (float)Corners.LowerLeft, BaseRect.Bottom);
					}
					else
					{
						PointF location4 = BaseRect.Location;
						SizeF size = new SizeF(Corners.LowerLeft * 2, Corners.LowerLeft * 2);
						rectangleF = new RectangleF(location4, size);
						rectangleF.Y = BaseRect.Bottom - (float)(Corners.LowerLeft * 2);
						val2.AddArc(rectangleF, 90f, 90f);
					}
					val2.CloseFigure();
					val2 = null;
				}
				else
				{
					GraphicsPath val3 = val;
					if (Corners.All == 0)
					{
						val3.AddRectangle(BaseRect);
					}
					else
					{
						PointF location5 = BaseRect.Location;
						SizeF size = new SizeF(Corners.All * 2, Corners.All * 2);
						rectangleF = new RectangleF(location5, size);
						val3.AddArc(rectangleF, 180f, 90f);
						rectangleF.X = BaseRect.Right - (float)(Corners.All * 2);
						val3.AddArc(rectangleF, 270f, 90f);
						rectangleF.Y = BaseRect.Bottom - (float)(Corners.All * 2);
						val3.AddArc(rectangleF, 0f, 90f);
						rectangleF.X = BaseRect.Left;
						val3.AddArc(rectangleF, 90f, 90f);
					}
					val3.CloseFigure();
					val3 = null;
				}
				return val;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Invalid comparison between Unknown and I4
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Invalid comparison between Unknown and I4
			((Control)this).OnMouseDown(e);
			if (!((Control)this).Enabled || ((_SideImageIsClickable && rectSideImage.Contains(e.Location)) ? true : false))
			{
				return;
			}
			if (GetPath().IsVisible(e.X, e.Y))
			{
				((Control)this).Focus();
				MouseDrawState = eMouseDrawState.Down;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
				ButtonAreaDown?.Invoke(this, e);
				return;
			}
			uint msg = default(uint);
			if ((int)e.Button == 1048576)
			{
				if (e.Clicks == 1)
				{
					msg = 513u;
				}
				else if (e.Clicks > 1)
				{
					msg = 515u;
				}
			}
			else if ((int)e.Button == 2097152)
			{
				if (e.Clicks == 1)
				{
					msg = 516u;
				}
				else if (e.Clicks > 1)
				{
					msg = 518u;
				}
			}
			SendMessage(((Control)this).Parent.Handle, msg, IntPtr.Zero, IntPtr.Zero);
		}

		private void CButton_MouseMove(object sender, MouseEventArgs e)
		{
			if ((_SideImageIsClickable && rectSideImage.Contains(e.Location)) ? true : false)
			{
				MouseDrawState = eMouseDrawState.Up;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
			else if (GetPath().IsVisible(e.X, e.Y))
			{
				if (MouseDrawState != eMouseDrawState.Down)
				{
					MouseDrawState = eMouseDrawState.Over;
				}
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
			else if (MouseDrawState != eMouseDrawState.Up)
			{
				MouseDrawState = eMouseDrawState.Up;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
		}

		private void CButton_MouseLeave(object sender, EventArgs e)
		{
			if (MouseDrawState != eMouseDrawState.Up)
			{
				MouseDrawState = eMouseDrawState.Up;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
		}

		protected override void OnMouseClick(MouseEventArgs e)
		{
			((Control)this).OnMouseClick(e);
			if ((_SideImageIsClickable && rectSideImage.Contains(e.Location)) ? true : false)
			{
				SideImageClicked?.Invoke(this, e);
			}
			else if (GetPath().IsVisible(e.X, e.Y))
			{
				PerformClickButtonArea(this, e);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			((Control)this).OnMouseUp(e);
			if ((!_SideImageIsClickable || !rectSideImage.Contains(e.Location)) && 0 == 0 && GetPath().IsVisible(e.X, e.Y))
			{
				if (MouseDrawState == eMouseDrawState.Down)
				{
					ButtonAreaUP?.Invoke(this, e);
				}
				MouseDrawState = eMouseDrawState.Up;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
		}

		private void CButton_GotFocus(object sender, EventArgs e)
		{
			if (ShowFocus == eFocus.Dim)
			{
				MouseDrawState = eMouseDrawState.Over;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
		}

		private void CButton_LostFocus(object sender, EventArgs e)
		{
			if (ShowFocus == eFocus.Dim)
			{
				MouseDrawState = eMouseDrawState.Up;
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
			}
		}

		private void CButton_KeyDown(object sender, KeyEventArgs e)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Invalid comparison between Unknown and I4
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			if (((Control)this).Enabled)
			{
				Keys keyCode = e.KeyCode;
				if (((int)keyCode == 13 || (int)keyCode == 32 || (int)keyCode == 13) ? true : false)
				{
					MouseDrawState = eMouseDrawState.Down;
					((Control)this).Invalidate(Rectangle.Round(ButtonArea));
				}
			}
		}

		private void CButton_KeyUp(object sender, KeyEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Invalid comparison between Unknown and I4
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			Keys keyCode = e.KeyCode;
			if (((int)keyCode != 13 && (int)keyCode != 32 && (int)keyCode != 13) || 1 == 0)
			{
				return;
			}
			e.Handled = true;
			if (MouseDrawState == eMouseDrawState.Down)
			{
				((Control)this).Parent.Text = Cursor.Position.ToString();
				if (ButtonArea.Contains(((Control)this).PointToClient(Cursor.Position)))
				{
					MouseDrawState = eMouseDrawState.Over;
				}
				else
				{
					MouseDrawState = eMouseDrawState.Up;
				}
				((Control)this).Invalidate(Rectangle.Round(ButtonArea));
				PerformClickButtonArea(this, checked(new MouseEventArgs((MouseButtons)1048576, 1, (int)Math.Round(ButtonArea.X), (int)Math.Round(ButtonArea.Y), 0)));
			}
		}

		protected override bool ProcessMnemonic(char charCode)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			if (((Control)this).CanSelect & Control.IsMnemonic(charCode, Text))
			{
				((Control)this).Select();
				PerformClickButtonArea(this, checked(new MouseEventArgs((MouseButtons)1048576, 1, (int)Math.Round(ButtonArea.X), (int)Math.Round(ButtonArea.Y), 0)));
				return true;
			}
			return false;
		}

		protected override void Finalize()
		{
			((Component)this).Finalize();
		}
	}
	[TypeConverter(typeof(BlendItemsConverter))]
	[DebuggerStepThrough]
	public class cBlendItems
	{
		private Color[] _iColor;

		private float[] _iPoint;

		[Description("The Color for the Point")]
		[Category("Appearance")]
		public Color[] iColor
		{
			get
			{
				return _iColor;
			}
			set
			{
				_iColor = value;
			}
		}

		[Description("The Color for the Point")]
		[Category("Appearance")]
		public float[] iPoint
		{
			get
			{
				return _iPoint;
			}
			set
			{
				_iPoint = value;
			}
		}

		public cBlendItems()
		{
		}

		public cBlendItems(Color[] Color, float[] Pt)
		{
			iColor = Color;
			iPoint = Pt;
		}

		public override string ToString()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			Color[] array = _iColor;
			for (int i = 0; i < array.Length; i = checked(i + 1))
			{
				Color color = array[i];
				if (color.IsNamedColor)
				{
					arrayList.Add(color.Name);
					continue;
				}
				arrayList.Add($"{color.A},{color.R},{color.G},{color.B}");
			}
			float[] array2 = _iPoint;
			foreach (float num in array2)
			{
				arrayList2.Add(num.ToString());
			}
			return string.Format("{0}|{1}", Strings.Join(arrayList.ToArray(), ";"), Strings.Join(arrayList2.ToArray(), ";"));
		}

		public override bool Equals(object obj)
		{
			cBlendItems cBlendItems2 = (cBlendItems)obj;
			if (iColor.Length != cBlendItems2.iColor.Length || ((iPoint.Length != cBlendItems2.iPoint.Length) ? true : false))
			{
				return false;
			}
			checked
			{
				int num = iColor.Length - 1;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 > num4)
					{
						break;
					}
					if (iColor[num2] != cBlendItems2.iColor[num2] || ((iPoint[num2] != cBlendItems2.iPoint[num2]) ? true : false))
					{
						return false;
					}
					num2++;
				}
				return true;
			}
		}
	}
	[DebuggerStepThrough]
	internal class BlendItemsConverter : ExpandableObjectConverter
	{
		[DebuggerNonUserCode]
		public BlendItemsConverter()
		{
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			cBlendItems cBlendItems2 = new cBlendItems();
			cBlendItems2.iColor = (Color[])propertyValues["iColor"];
			cBlendItems2.iPoint = (float[])propertyValues["iPoint"];
			return cBlendItems2;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if ((object)sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				try
				{
					string[] array = Strings.Split(Conversions.ToString(value), "|", -1, (CompareMethod)0);
					List<Color> list = new List<Color>();
					List<float> list2 = new List<float>();
					string[] array2 = Strings.Split(array[0], ";", -1, (CompareMethod)0);
					Color color = default(Color);
					foreach (string text in array2)
					{
						object? obj = TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(text);
						list.Add((obj != null) ? ((Color)obj) : color);
					}
					string[] array3 = Strings.Split(array[1], ";", -1, (CompareMethod)0);
					foreach (string text2 in array3)
					{
						list2.Add(Conversions.ToSingle(TypeDescriptor.GetConverter(typeof(float)).ConvertFromString(text2)));
					}
					if ((!Information.IsNothing((object)list) && !Information.IsNothing((object)list2)) ? true : false)
					{
						if (list.Count != list2.Count)
						{
							throw new ArgumentException($"Can not convert '{Conversions.ToString(value)}' to type cBlendItem");
						}
						return new cBlendItems(list.ToArray(), list2.ToArray());
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					throw new ArgumentException($"Can not convert '{Conversions.ToString(value)}' to type cBlendItem");
				}
				return base.ConvertFrom(context, culture, RuntimeHelpers.GetObjectValue(value));
			}
			return new cBlendItems();
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (((object)destinationType == typeof(string) && value is cBlendItems) ? true : false)
			{
				cBlendItems cBlendItems2 = (cBlendItems)value;
				ArrayList arrayList = new ArrayList();
				ArrayList arrayList2 = new ArrayList();
				Color[] iColor = cBlendItems2.iColor;
				for (int i = 0; i < iColor.Length; i = checked(i + 1))
				{
					Color color = iColor[i];
					if (color.IsNamedColor)
					{
						arrayList.Add(color.Name);
						continue;
					}
					arrayList.Add($"{color.A},{color.R},{color.G},{color.B}");
				}
				float[] iPoint = cBlendItems2.iPoint;
				foreach (float num in iPoint)
				{
					arrayList2.Add(num.ToString());
				}
				return string.Format("{0}|{1}", Strings.Join(arrayList.ToArray(), ";"), Strings.Join(arrayList2.ToArray(), ";"));
			}
			return base.ConvertTo(context, culture, RuntimeHelpers.GetObjectValue(value), destinationType);
		}
	}
	[DebuggerStepThrough]
	public class BlendTypeEditor : UITypeEditor
	{
		[DebuggerNonUserCode]
		public BlendTypeEditor()
		{
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (context != null)
			{
				return (UITypeEditorEditStyle)3;
			}
			return ((UITypeEditor)this).GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0333: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_040b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Expected O, but got Unknown
			if (context != null && provider != null)
			{
				IWindowsFormsEditorService val = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				checked
				{
					if (val != null)
					{
						DropdownColorBlender dropdownColorBlender = new DropdownColorBlender(val);
						try
						{
							CButton cButton = (((object)context.Instance.GetType() != typeof(CButton)) ? ((CButtonActionList)context.Instance).CurrControl : ((CButton)context.Instance));
							DropdownColorBlender dropdownColorBlender2 = dropdownColorBlender;
							float num;
							if (((Control)cButton).Width > ((Control)cButton).Height)
							{
								((Control)dropdownColorBlender2.TheSample).Height = (int)Math.Round((double)((Control)dropdownColorBlender2.TheSample).Width * ((double)((Control)cButton).Height / (double)((Control)cButton).Width));
								((Control)dropdownColorBlender2.TheSample).Top = (int)Math.Round((double)(((Control)dropdownColorBlender2.panSampleHolder).Height - ((Control)dropdownColorBlender2.TheSample).Height) / 2.0);
								num = (float)((double)((Control)dropdownColorBlender2.TheSample).Height / (double)((Control)cButton).Height);
							}
							else
							{
								((Control)dropdownColorBlender2.TheSample).Width = (int)Math.Round((double)((Control)dropdownColorBlender2.TheSample).Height * ((double)((Control)cButton).Width / (double)((Control)cButton).Height));
								((Control)dropdownColorBlender2.TheSample).Left = (int)Math.Round((double)(((Control)dropdownColorBlender2.panSampleHolder).Width - ((Control)dropdownColorBlender2.TheSample).Width) / 2.0);
								num = (float)((double)((Control)dropdownColorBlender2.TheSample).Width / (double)((Control)cButton).Width);
							}
							dropdownColorBlender2.TheSample.Shape = cButton.Shape;
							dropdownColorBlender2.TheSample.FillType = cButton.FillType;
							dropdownColorBlender2.TheSample.FillTypeLinear = cButton.FillTypeLinear;
							dropdownColorBlender2.TheSample.ColorFillSolid = cButton.ColorFillSolid;
							dropdownColorBlender2.TheSample.BorderColor = cButton.BorderColor;
							dropdownColorBlender2.TheSample.FocalPoints = cButton.FocalPoints;
							dropdownColorBlender2.TheSample.ColorFillBlend = cButton.ColorFillBlend;
							dropdownColorBlender2.TheSample.Corners = new CornersProperty((int)Math.Round((float)cButton.Corners.LowerLeft * num), (int)Math.Round((float)cButton.Corners.LowerRight * num), (int)Math.Round((float)cButton.Corners.UpperLeft * num), (int)Math.Round((float)cButton.Corners.UpperRight * num));
							dropdownColorBlender2.LoadABlend(cButton.ColorFillBlend);
							CButton theSample = dropdownColorBlender2.TheSample;
							Padding textMargin = cButton.TextMargin;
							int num2 = (int)Math.Round((float)((Padding)(ref textMargin)).Left * num);
							Padding val2 = cButton.TextMargin;
							int num3 = (int)Math.Round((float)((Padding)(ref val2)).Top * num);
							Padding val3 = cButton.TextMargin;
							int num4 = (int)Math.Round((float)((Padding)(ref val3)).Right * num);
							Padding val4 = cButton.TextMargin;
							Padding padding = default(Padding);
							((Padding)(ref padding))..ctor(num2, num3, num4, (int)Math.Round((float)((Padding)(ref val4)).Bottom * num));
							theSample.TextMargin = padding;
							CButton theSample2 = dropdownColorBlender2.TheSample;
							padding = ((Control)cButton).Padding;
							int num5 = (int)Math.Round((float)((Padding)(ref padding)).Left * num);
							val4 = ((Control)cButton).Padding;
							int num6 = (int)Math.Round((float)((Padding)(ref val4)).Top * num);
							val3 = ((Control)cButton).Padding;
							int num7 = (int)Math.Round((float)((Padding)(ref val3)).Right * num);
							val2 = ((Control)cButton).Padding;
							((Padding)(ref textMargin))..ctor(num5, num6, num7, (int)Math.Round((float)((Padding)(ref val2)).Bottom * num));
							((Control)theSample2).Padding = textMargin;
							dropdownColorBlender2.TheSample.Text = cButton.Text;
							dropdownColorBlender2.TheSample.ForeColor = cButton.ForeColor;
							dropdownColorBlender2.TheSample.TextAlign = cButton.TextAlign;
							dropdownColorBlender2.TheSample.Font = new Font(cButton.Font.FontFamily, cButton.Font.Size * num, cButton.Font.Style);
							dropdownColorBlender2.TheSample.TextShadow = cButton.TextShadow;
							dropdownColorBlender2.TheSample.TextShadowShow = cButton.TextShadowShow;
							dropdownColorBlender2 = null;
							val.DropDownControl((Control)(object)dropdownColorBlender);
							return dropdownColorBlender.TheSample.ColorFillBlend;
						}
						finally
						{
							((IDisposable)dropdownColorBlender)?.Dispose();
						}
					}
				}
			}
			return ((UITypeEditor)this).EditValue(context, provider, RuntimeHelpers.GetObjectValue(value));
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			cBlendItems cBlendItems2 = (cBlendItems)e.Value;
			LinearGradientBrush val = new LinearGradientBrush(e.Bounds, Color.Black, Color.Black, (LinearGradientMode)0);
			try
			{
				ColorBlend val2 = new ColorBlend();
				val2.Colors = cBlendItems2.iColor;
				val2.Positions = cBlendItems2.iPoint;
				val.InterpolationColors = val2;
				e.Graphics.FillRectangle((Brush)(object)val, e.Bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
	[TypeConverter(typeof(FocalPointsConverter))]
	public class cFocalPoints
	{
		private float _CenterPtX;

		private float _CenterPtY;

		private float _FocusPtX;

		private float _FocusPtY;

		[DefaultValue(0.5f)]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public float CenterPtX
		{
			get
			{
				return _CenterPtX;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 1f)
				{
					value = 1f;
				}
				_CenterPtX = value;
			}
		}

		[DefaultValue(0.5f)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[NotifyParentProperty(true)]
		public float CenterPtY
		{
			get
			{
				return _CenterPtY;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 1f)
				{
					value = 1f;
				}
				_CenterPtY = value;
			}
		}

		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(0f)]
		[NotifyParentProperty(true)]
		public float FocusPtX
		{
			get
			{
				return _FocusPtX;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 1f)
				{
					value = 1f;
				}
				_FocusPtX = value;
			}
		}

		[NotifyParentProperty(true)]
		[DefaultValue(0f)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public float FocusPtY
		{
			get
			{
				return _FocusPtY;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 1f)
				{
					value = 1f;
				}
				_FocusPtY = value;
			}
		}

		public PointF CenterPoint()
		{
			return new PointF(CenterPtX, CenterPtY);
		}

		public PointF FocusScales()
		{
			return new PointF(FocusPtX, FocusPtY);
		}

		public void SetCenterPoint(PointF centerPoint)
		{
			CenterPtX = centerPoint.X;
			CenterPtY = centerPoint.Y;
		}

		public void SetFocusScales(PointF focusScale)
		{
			FocusPtX = focusScale.X;
			FocusPtY = focusScale.Y;
		}

		public cFocalPoints()
		{
			_CenterPtX = 0.5f;
			_CenterPtY = 0.5f;
			_FocusPtX = 0f;
			_FocusPtY = 0f;
			CenterPtX = 0.5f;
			CenterPtY = 0.5f;
			FocusPtX = 0f;
			FocusPtY = 0f;
		}

		public cFocalPoints(float Cx, float Cy, float Fx, float Fy)
		{
			_CenterPtX = 0.5f;
			_CenterPtY = 0.5f;
			_FocusPtX = 0f;
			_FocusPtY = 0f;
			CenterPtX = Cx;
			CenterPtY = Cy;
			FocusPtX = Fx;
			FocusPtY = Fy;
		}

		public cFocalPoints(PointF ptC, PointF ptF)
		{
			_CenterPtX = 0.5f;
			_CenterPtY = 0.5f;
			_FocusPtX = 0f;
			_FocusPtY = 0f;
			CenterPtX = ptC.X;
			CenterPtY = ptC.Y;
			FocusPtX = ptF.X;
			FocusPtY = ptF.Y;
		}

		public override string ToString()
		{
			return $"{_CenterPtX}, {_CenterPtY}, {_FocusPtX}, {_FocusPtY}";
		}

		public override bool Equals(object obj)
		{
			cFocalPoints cFocalPoints2 = (cFocalPoints)obj;
			return (CenterPtX == cFocalPoints2.CenterPtX && CenterPtY == cFocalPoints2.CenterPtY && FocusPtX == cFocalPoints2.FocusPtX && FocusPtY == cFocalPoints2.FocusPtY) ? true : false;
		}
	}
	internal class FocalPointsConverter : ExpandableObjectConverter
	{
		[DebuggerNonUserCode]
		public FocalPointsConverter()
		{
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			cFocalPoints cFocalPoints2 = new cFocalPoints();
			cFocalPoints2.CenterPtX = Conversions.ToSingle(propertyValues["CenterPtX"]);
			cFocalPoints2.CenterPtY = Conversions.ToSingle(propertyValues["CenterPtY"]);
			cFocalPoints2.FocusPtX = Conversions.ToSingle(propertyValues["FocusPtX"]);
			cFocalPoints2.FocusPtY = Conversions.ToSingle(propertyValues["FocusPtY"]);
			return cFocalPoints2;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if ((object)sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				try
				{
					string text = Conversions.ToString(value);
					string[] array = new string[5];
					array = Strings.Split(text, ",", -1, (CompareMethod)0);
					if (!Information.IsNothing((object)array))
					{
						if (Information.IsNothing((object)array[0]))
						{
							array[0] = "0.5";
						}
						if (Information.IsNothing((object)array[1]))
						{
							array[1] = "0.5";
						}
						if (Information.IsNothing((object)array[2]))
						{
							array[2] = "0";
						}
						if (Information.IsNothing((object)array[3]))
						{
							array[3] = "0";
						}
						return new cFocalPoints(Conversions.ToSingle(array[0].Trim()), Conversions.ToSingle(array[1].Trim()), Conversions.ToSingle(array[2].Trim()), Conversions.ToSingle(array[3].Trim()));
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					throw new ArgumentException($"Can not convert '{Conversions.ToString(value)}' to type FocalPoints");
				}
				return base.ConvertFrom(context, culture, RuntimeHelpers.GetObjectValue(value));
			}
			return new cFocalPoints();
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (((object)destinationType == typeof(string) && value is cFocalPoints) ? true : false)
			{
				cFocalPoints cFocalPoints2 = (cFocalPoints)value;
				return $"{cFocalPoints2.CenterPtX}, {cFocalPoints2.CenterPtY}, {cFocalPoints2.FocusPtX}, {cFocalPoints2.FocusPtY}";
			}
			return base.ConvertTo(context, culture, RuntimeHelpers.GetObjectValue(value), destinationType);
		}
	}
	[TypeConverter(typeof(CornerConverter))]
	public class CornersProperty
	{
		private int _All;

		private int _UpperLeft;

		private int _UpperRight;

		private int _LowerLeft;

		private int _LowerRight;

		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("Set the Radius of the All four Corners the same")]
		[DefaultValue(-1)]
		[NotifyParentProperty(true)]
		public int All
		{
			get
			{
				return _All;
			}
			set
			{
				_All = value;
				if (value > -1)
				{
					_LowerLeft = value;
					_LowerRight = value;
					_UpperLeft = value;
					_UpperRight = value;
				}
			}
		}

		[NotifyParentProperty(true)]
		[Description("Set the Radius of the Upper Left Corner")]
		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int UpperLeft
		{
			get
			{
				return _UpperLeft;
			}
			set
			{
				_UpperLeft = value;
				CheckForAll(value);
			}
		}

		[Description("Set the Radius of the Upper Right Corner")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[DefaultValue(0)]
		[NotifyParentProperty(true)]
		public int UpperRight
		{
			get
			{
				return _UpperRight;
			}
			set
			{
				_UpperRight = value;
				CheckForAll(value);
			}
		}

		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("Set the Radius of the Lower Left Corner")]
		[NotifyParentProperty(true)]
		public int LowerLeft
		{
			get
			{
				return _LowerLeft;
			}
			set
			{
				_LowerLeft = value;
				CheckForAll(value);
			}
		}

		[DefaultValue(0)]
		[RefreshProperties(RefreshProperties.Repaint)]
		[Description("Set the Radius of the Lower Right Corner")]
		[NotifyParentProperty(true)]
		public int LowerRight
		{
			get
			{
				return _LowerRight;
			}
			set
			{
				_LowerRight = value;
				CheckForAll(value);
			}
		}

		public CornersProperty(int LowerLeft, int LowerRight, int UpperLeft, int UpperRight)
		{
			_All = -1;
			_UpperLeft = 0;
			_UpperRight = 0;
			_LowerLeft = 0;
			_LowerRight = 0;
			this.LowerLeft = LowerLeft;
			this.LowerRight = LowerRight;
			this.UpperLeft = UpperLeft;
			this.UpperRight = UpperRight;
		}

		public CornersProperty(int All)
		{
			_All = -1;
			_UpperLeft = 0;
			_UpperRight = 0;
			_LowerLeft = 0;
			_LowerRight = 0;
			this.All = All;
		}

		public CornersProperty()
		{
			_All = -1;
			_UpperLeft = 0;
			_UpperRight = 0;
			_LowerLeft = 0;
			_LowerRight = 0;
			LowerLeft = 0;
			LowerRight = 0;
			UpperLeft = 0;
			UpperRight = 0;
		}

		private void CheckForAll(int val)
		{
			if ((val == LowerLeft && val == LowerRight && val == UpperLeft && val == UpperRight) ? true : false)
			{
				if (_All != val)
				{
					_All = val;
				}
			}
			else if (All != -1)
			{
				All = -1;
			}
		}

		public override bool Equals(object obj)
		{
			CornersProperty cornersProperty = (CornersProperty)obj;
			return (All == cornersProperty.All && LowerLeft == cornersProperty.LowerLeft && LowerRight == cornersProperty.LowerRight && UpperLeft == cornersProperty.UpperLeft && UpperRight == cornersProperty.UpperRight) ? true : false;
		}
	}
	internal class CornerConverter : ExpandableObjectConverter
	{
		[DebuggerNonUserCode]
		public CornerConverter()
		{
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
		{
			CornersProperty cornersProperty = new CornersProperty();
			int num = Conversions.ToInteger(propertyValues["All"]);
			int lowerLeft = Conversions.ToInteger(propertyValues["LowerLeft"]);
			int lowerRight = Conversions.ToInteger(propertyValues["LowerRight"]);
			int upperLeft = Conversions.ToInteger(propertyValues["UpperLeft"]);
			int upperRight = Conversions.ToInteger(propertyValues["UpperRight"]);
			int all = ((CButton)context.Instance).Corners.All;
			if (all != num && num > -1)
			{
				cornersProperty.All = num;
			}
			else
			{
				cornersProperty.LowerLeft = lowerLeft;
				cornersProperty.LowerRight = lowerRight;
				cornersProperty.UpperLeft = upperLeft;
				cornersProperty.UpperRight = upperRight;
			}
			return cornersProperty;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if ((object)sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				try
				{
					string text = Conversions.ToString(value);
					string[] array = new string[5];
					array = Strings.Split(text, ",", -1, (CompareMethod)0);
					if (!Information.IsNothing((object)array))
					{
						if (Information.IsNothing((object)array[0]))
						{
							array[0] = "0";
						}
						if (Information.IsNothing((object)array[1]))
						{
							array[1] = "0";
						}
						if (Information.IsNothing((object)array[2]))
						{
							array[2] = "0";
						}
						if (Information.IsNothing((object)array[3]))
						{
							array[3] = "0";
						}
						return new CornersProperty(Conversions.ToInteger(array[0].Trim()), Conversions.ToInteger(array[1].Trim()), Conversions.ToInteger(array[2].Trim()), Conversions.ToInteger(array[3].Trim()));
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					throw new ArgumentException($"Can not convert '{Conversions.ToString(value)}' to type Corners");
				}
				return base.ConvertFrom(context, culture, RuntimeHelpers.GetObjectValue(value));
			}
			return new CornersProperty();
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			CornersProperty cornersProperty = (CornersProperty)value;
			if (((object)destinationType == typeof(string) && value is CornersProperty) ? true : false)
			{
				return $"{cornersProperty.LowerLeft}, {cornersProperty.LowerRight}, {cornersProperty.UpperLeft}, {cornersProperty.UpperRight}";
			}
			return base.ConvertTo(context, culture, RuntimeHelpers.GetObjectValue(value), destinationType);
		}
	}
	public class CButtonDesigner : ControlDesigner
	{
		public Adorner PadAdorner;

		public Adorner CornAdorner;

		public Adorner FocalPtAdorner;

		public Adorner ChooseAdorner;

		private ISelectionService selectionSvc;

		private BehaviorService behaviorSvc;

		private CButton _CButton;

		private DesignerActionListCollection _Lists;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				if (_Lists == null)
				{
					_Lists = new DesignerActionListCollection();
					_Lists.Add((DesignerActionList)(object)new CButtonActionList(((ComponentDesigner)this).Component));
				}
				return _Lists;
			}
		}

		public CButtonDesigner()
		{
			PadAdorner = null;
			CornAdorner = null;
			FocalPtAdorner = null;
			ChooseAdorner = null;
			selectionSvc = null;
			behaviorSvc = null;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && behaviorSvc != null)
			{
				behaviorSvc.Adorners.Remove(PadAdorner);
				behaviorSvc.Adorners.Remove(CornAdorner);
				behaviorSvc.Adorners.Remove(FocalPtAdorner);
				behaviorSvc.Adorners.Remove(ChooseAdorner);
			}
			((ControlDesigner)this).Dispose(disposing);
		}

		public override void Initialize(IComponent component)
		{
			((ControlDesigner)this).Initialize(component);
			InitializeServices();
			InitializePadAdorner();
			InitializeCornerAdorner();
			InitializeFocalPtAdorner();
			InitializeChooseAdorner();
			_CButton = (CButton)component;
		}

		private void InitializeServices()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			selectionSvc = (ISelectionService)((ComponentDesigner)this).GetService(typeof(ISelectionService));
			behaviorSvc = (BehaviorService)((ComponentDesigner)this).GetService(typeof(BehaviorService));
		}

		private void InitializePadAdorner()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (PadAdorner != null)
			{
				((CollectionBase)(object)PadAdorner.Glyphs).Clear();
				return;
			}
			PadAdorner = new Adorner();
			behaviorSvc.Adorners.Add(PadAdorner);
			PadAdorner.Glyphs.Add((Glyph)(object)new PadGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, PadAdorner, PadGlyph.eAdjWhat.Button));
			PadAdorner.Glyphs.Add((Glyph)(object)new PadGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, PadAdorner, PadGlyph.eAdjWhat.Text));
		}

		private void InitializeCornerAdorner()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (CornAdorner != null)
			{
				((CollectionBase)(object)CornAdorner.Glyphs).Clear();
				return;
			}
			CornAdorner = new Adorner();
			behaviorSvc.Adorners.Add(CornAdorner);
			CornAdorner.Glyphs.Add((Glyph)(object)new CornerGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, CornAdorner, CornerGlyph.eCorner.All));
			CornAdorner.Glyphs.Add((Glyph)(object)new CornerGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, CornAdorner, CornerGlyph.eCorner.UpperLeft));
			CornAdorner.Glyphs.Add((Glyph)(object)new CornerGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, CornAdorner, CornerGlyph.eCorner.UpperRight));
			CornAdorner.Glyphs.Add((Glyph)(object)new CornerGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, CornAdorner, CornerGlyph.eCorner.LowerLeft));
			CornAdorner.Glyphs.Add((Glyph)(object)new CornerGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, CornAdorner, CornerGlyph.eCorner.LowerRight));
		}

		private void InitializeFocalPtAdorner()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			if (FocalPtAdorner != null)
			{
				((CollectionBase)(object)FocalPtAdorner.Glyphs).Clear();
				return;
			}
			FocalPtAdorner = new Adorner();
			behaviorSvc.Adorners.Add(FocalPtAdorner);
			FocalPtAdorner.Glyphs.Add((Glyph)(object)new FocalPtGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, FocalPtAdorner, FocalPtGlyph.eAdjWhat.FocalScale));
			FocalPtAdorner.Glyphs.Add((Glyph)(object)new FocalPtGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, FocalPtAdorner, FocalPtGlyph.eAdjWhat.Center));
		}

		private void InitializeChooseAdorner()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			if (ChooseAdorner != null)
			{
				((CollectionBase)(object)ChooseAdorner.Glyphs).Clear();
				return;
			}
			ChooseAdorner = new Adorner();
			behaviorSvc.Adorners.Add(ChooseAdorner);
			ChooseAdorner.Glyphs.Add((Glyph)(object)new ChooseGlyph(behaviorSvc, (CButton)(object)((ControlDesigner)this).Control, selectionSvc, (IDesigner)this, ChooseAdorner));
		}
	}
	public class CButtonActionList : DesignerActionList
	{
		private CButton _CButton;

		private DesignerActionUIService _DesignerService;

		public CButton CurrControl => _CButton;

		public CButton.eShape Shape
		{
			get
			{
				return _CButton.Shape;
			}
			set
			{
				SetControlProperty("Shape", value);
			}
		}

		public Color BorderColor
		{
			get
			{
				return _CButton.BorderColor;
			}
			set
			{
				SetControlProperty("BorderColor", value);
			}
		}

		public bool BorderShow
		{
			get
			{
				return _CButton.BorderShow;
			}
			set
			{
				SetControlProperty("BorderShow", value);
			}
		}

		[Editor(typeof(BlendTypeEditor), typeof(UITypeEditor))]
		public cBlendItems ColorFillBlend
		{
			get
			{
				return _CButton.ColorFillBlend;
			}
			set
			{
				SetControlProperty("ColorFillBlend", value);
			}
		}

		public CButton.eFillType FillType
		{
			get
			{
				return _CButton.FillType;
			}
			set
			{
				SetControlProperty("FillType", value);
			}
		}

		public LinearGradientMode FillTypeLinear
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return _CButton.FillTypeLinear;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetControlProperty("FillTypeLinear", value);
			}
		}

		public Color ColorFillSolid
		{
			get
			{
				return _CButton.ColorFillSolid;
			}
			set
			{
				SetControlProperty("ColorFillSolid", value);
			}
		}

		public string Text
		{
			get
			{
				return _CButton.Text;
			}
			set
			{
				SetControlProperty("Text", value);
			}
		}

		public ContentAlignment TextAlign
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return _CButton.TextAlign;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetControlProperty("TextAlign", value);
			}
		}

		public TextImageRelation TextImageRelation
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return _CButton.TextImageRelation;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetControlProperty("TextImageRelation", value);
			}
		}

		public bool TextShadowShow
		{
			get
			{
				return _CButton.TextShadowShow;
			}
			set
			{
				SetControlProperty("TextShadowShow", value);
			}
		}

		public Color ForeColor
		{
			get
			{
				return _CButton.ForeColor;
			}
			set
			{
				SetControlProperty("ForeColor", value);
			}
		}

		public Color TextShadow
		{
			get
			{
				return _CButton.TextShadow;
			}
			set
			{
				SetControlProperty("TextShadow", value);
			}
		}

		public ContentAlignment ImageAlign
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return _CButton.ImageAlign;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetControlProperty("ImageAlign", value);
			}
		}

		public ContentAlignment SideImageAlign
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return _CButton.SideImageAlign;
			}
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				SetControlProperty("SideImageAlign", value);
			}
		}

		public bool SideImageBehindText
		{
			get
			{
				return _CButton.SideImageBehindText;
			}
			set
			{
				SetControlProperty("SideImageBehindText", value);
			}
		}

		public CButtonActionList(IComponent component)
			: base(component)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			_CButton = (CButton)component;
			_DesignerService = (DesignerActionUIService)((DesignerActionList)this).GetService(typeof(DesignerActionUIService));
			((DesignerActionList)this).AutoShow = true;
		}

		private void SetControlProperty(string property_name, object value)
		{
			TypeDescriptor.GetProperties(_CButton)[property_name].SetValue(_CButton, RuntimeHelpers.GetObjectValue(value));
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Expected O, but got Unknown
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Expected O, but got Unknown
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Expected O, but got Unknown
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Expected O, but got Unknown
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Expected O, but got Unknown
			DesignerActionItemCollection val = new DesignerActionItemCollection();
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("Shape", "Shape", "", "The Shape of the Control"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Fill"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("FillType", "Fill Type", "Fill", "Fill Solid or Gradient"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("FillTypeLinear", "Linear Fill Type", "Fill", "Gradient Fill Type"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ColorFillBlend", "Blend Colors", "Fill", "Color and Position Arrays for the ColorBlend"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ColorFillSolid", "Solid Fill Color", "Fill", "The Color for Solid Fills"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Text"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("Text", "Text", "Text", "The Text on the Button"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("TextAlign", "Text Alignment", "Text", "The Alignment to use on the Text"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ForeColor", "Text Color", "Text", "The Color of the Text"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("TextShadow", "Shadow Color", "Text", "The Color of the Shadow Text"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("TextImageRelation", "Text to Image Relation", "Text", "The Relationship of the Text to the Image"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("TextShadowShow", "Use Text Shadow", "Text", "Turn the Text Shadow On and Off"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Border"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("BorderColor", "Border Color", "Border", "The color of the button's Border"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("BorderShow", "Show Border", "Border", "The show or not show the border"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Images"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ImageAlign", "Image Alignment", "Images", "The Alignment for the Image"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("SideImageAlign", "SideImage Alignment", "Images", "The Alignment for the SideImage"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("SideImageBehindText", "Is SideImage Behind the Text", "Images", ""));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Corners"));
			string text = $"UpperLeft={_CButton.Corners.UpperLeft.ToString()}, UpperRight={_CButton.Corners.UpperRight.ToString()}, LowerRight={_CButton.Corners.LowerRight.ToString()}, LowerLeft={_CButton.Corners.LowerLeft.ToString()}";
			val.Add((DesignerActionItem)new DesignerActionTextItem(text, "Corners"));
			return val;
		}
	}
	public class ChooseGlyph : Glyph
	{
		public enum eChooseWhat
		{
			Pad,
			Corner,
			FocalPt,
			None
		}

		public class CornBehavior : Behavior
		{
			private IDesigner relatedDesigner;

			private CButton relatedControl;

			internal CornBehavior(IDesigner relatedDesigner)
			{
				this.relatedDesigner = null;
				relatedControl = null;
				this.relatedDesigner = relatedDesigner;
				relatedControl = (CButton)relatedDesigner.Component;
			}

			public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
			{
				ChooseGlyph chooseGlyph = (ChooseGlyph)(object)g;
				chooseGlyph.chooseWhat = chooseGlyph.sendWhat;
				switch ((int)chooseGlyph.chooseWhat)
				{
				case 0:
					((CButtonDesigner)relatedDesigner).PadAdorner.Invalidate();
					break;
				case 1:
					((CButtonDesigner)relatedDesigner).CornAdorner.Invalidate();
					break;
				case 2:
					((CButtonDesigner)relatedDesigner).FocalPtAdorner.Invalidate();
					break;
				}
				return true;
			}
		}

		private CButton relatedControl;

		private BehaviorService behaviorSvc;

		private ISelectionService selectionSvc;

		private IDesigner relatedDesigner;

		private Adorner ChooseAdorner;

		private Rectangle selectBox;

		private Rectangle padBox;

		private Rectangle cornBox;

		private Rectangle focalPtBox;

		private Brush boxFillColor;

		public eChooseWhat chooseWhat
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		public eChooseWhat sendWhat
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		public override Rectangle Bounds => selectBox;

		public ChooseGlyph(BehaviorService behaviorService, CButton control, ISelectionService selectionService, IDesigner relatedDesigner, Adorner myAdorner)
			: base((Behavior)(object)new CornBehavior(relatedDesigner))
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			selectionSvc = null;
			this.relatedDesigner = null;
			boxFillColor = (Brush)new SolidBrush(Color.FromArgb(120, Color.White));
			eChooseWhat eChooseWhat = eChooseWhat.None;
			chooseWhat = eChooseWhat;
			eChooseWhat = eChooseWhat.None;
			sendWhat = eChooseWhat;
			behaviorSvc = behaviorService;
			selectionSvc = selectionService;
			this.relatedDesigner = relatedDesigner;
			ChooseAdorner = myAdorner;
			relatedControl = (CButton)this.relatedDesigner.Component;
			selectionSvc.SelectionChanged += selectionService_SelectionChanged;
			((Control)relatedControl).Move += relatedControl_Move;
			((Control)relatedControl).Resize += relatedControl_Resize;
		}

		private void relatedControl_Move(object sender, EventArgs e)
		{
			if (object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl))
			{
				SetBoxes();
				ChooseAdorner.Invalidate();
			}
		}

		private void relatedControl_Resize(object sender, EventArgs e)
		{
			if (object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl))
			{
				SetBoxes();
				ChooseAdorner.Invalidate();
			}
		}

		private void selectionService_SelectionChanged(object sender, EventArgs e)
		{
			if (((object)relatedControl).GetType() == typeof(CButton))
			{
				if (object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl))
				{
					SetBoxes();
					ChooseAdorner.Enabled = true;
					relatedControl.DesignerSelected = true;
				}
				else if (relatedControl.DesignerSelected)
				{
					ChooseAdorner.Enabled = false;
					relatedControl.DesignerSelected = false;
				}
			}
			chooseWhat = eChooseWhat.None;
		}

		public override Cursor GetHitTest(Point p)
		{
			if (object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl))
			{
				if (padBox.Contains(p))
				{
					sendWhat = eChooseWhat.Pad;
					return Cursors.Hand;
				}
				if (cornBox.Contains(p))
				{
					sendWhat = eChooseWhat.Corner;
					return Cursors.Hand;
				}
				if (focalPtBox.Contains(p))
				{
					sendWhat = eChooseWhat.FocalPt;
					return Cursors.Hand;
				}
				if (selectBox.Contains(p))
				{
					sendWhat = eChooseWhat.None;
					return Cursors.Arrow;
				}
				return null;
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			if (object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl))
			{
				Pen val = new Pen(Brushes.DarkGray, 1f);
				val.DashStyle = (DashStyle)0;
				Pen val2 = val;
				try
				{
					Font val3 = new Font("Arial", 8f);
					StringFormat val4 = new StringFormat();
					val4.Alignment = (StringAlignment)1;
					val4.LineAlignment = (StringAlignment)1;
					StringFormat val5 = val4;
					pe.Graphics.FillRectangle(boxFillColor, selectBox);
					pe.Graphics.DrawRectangle(val2, selectBox);
					val2.DashStyle = (DashStyle)2;
					pe.Graphics.DrawRectangle(val2, padBox);
					pe.Graphics.DrawRectangle(val2, cornBox);
					pe.Graphics.DrawRectangle(val2, focalPtBox);
					padBox.Offset(1, 1);
					cornBox.Offset(1, 1);
					focalPtBox.Offset(1, 1);
					pe.Graphics.DrawString("P", val3, Brushes.Black, (RectangleF)padBox, val5);
					pe.Graphics.DrawString("C", val3, Brushes.Black, (RectangleF)cornBox, val5);
					pe.Graphics.DrawString("F", val3, Brushes.Black, (RectangleF)focalPtBox, val5);
					padBox.Offset(-1, -1);
					cornBox.Offset(-1, -1);
					focalPtBox.Offset(-1, -1);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}

		private void SetBoxes()
		{
			Point point = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
			checked
			{
				if (((Control)relatedControl).Height < 40)
				{
					ref Rectangle reference = ref selectBox;
					reference = new Rectangle(point.X + ((Control)relatedControl).Width + 5, point.Y + unchecked(((Control)relatedControl).Height / 2) - 8, 42, 16);
					ref Rectangle reference2 = ref padBox;
					reference2 = new Rectangle(point.X + ((Control)relatedControl).Width + 8, point.Y + unchecked(((Control)relatedControl).Height / 2) - 5, 10, 10);
					ref Rectangle reference3 = ref cornBox;
					reference3 = new Rectangle(point.X + ((Control)relatedControl).Width + 21, point.Y + unchecked(((Control)relatedControl).Height / 2) - 5, 10, 10);
					ref Rectangle reference4 = ref focalPtBox;
					reference4 = new Rectangle(point.X + ((Control)relatedControl).Width + 34, point.Y + unchecked(((Control)relatedControl).Height / 2) - 5, 10, 10);
				}
				else
				{
					ref Rectangle reference5 = ref selectBox;
					reference5 = new Rectangle(point.X + ((Control)relatedControl).Width + 5, point.Y, 16, 42);
					ref Rectangle reference6 = ref padBox;
					reference6 = new Rectangle(point.X + ((Control)relatedControl).Width + 8, point.Y + 3, 10, 10);
					ref Rectangle reference7 = ref cornBox;
					reference7 = new Rectangle(point.X + ((Control)relatedControl).Width + 8, point.Y + 16, 10, 10);
					ref Rectangle reference8 = ref focalPtBox;
					reference8 = new Rectangle(point.X + ((Control)relatedControl).Width + 8, point.Y + 29, 10, 10);
				}
			}
		}
	}
	public class CornerGlyph : Glyph
	{
		public enum eCorner
		{
			All,
			UpperLeft,
			UpperRight,
			LowerLeft,
			LowerRight
		}

		public class CornBehavior : Behavior
		{
			private IDesigner relatedDesigner;

			private CButton relatedControl;

			private float sngSliderPos;

			private Point _dragStart;

			public bool _dragging;

			private eCorner whatCorner;

			internal CornBehavior(IDesigner relatedDesigner, eCorner whatCorner)
			{
				this.relatedDesigner = null;
				relatedControl = null;
				this.relatedDesigner = relatedDesigner;
				relatedControl = (CButton)relatedDesigner.Component;
				this.whatCorner = whatCorner;
			}

			public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
			{
				CornerGlyph cornerGlyph = (CornerGlyph)(object)g;
				switch ((int)whatCorner)
				{
				case 2:
				case 4:
					sngSliderPos = checked((int)Math.Round((double)(cornerGlyph.CornerTrack.Width - 10) * ((double)(cornerGlyph.maxCorner - cornerGlyph.GetValue()) / (double)cornerGlyph.maxCorner)));
					break;
				case 0:
				case 1:
				case 3:
					sngSliderPos = checked((int)Math.Round((double)(cornerGlyph.CornerTrack.Width - 10) * ((double)cornerGlyph.GetValue() / (double)cornerGlyph.maxCorner)));
					break;
				}
				_dragStart = mouseLoc;
				_dragging = true;
				return true;
			}

			public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b1: Expected O, but got Unknown
				if ((int)button != 1048576)
				{
					_dragging = false;
				}
				checked
				{
					if (_dragging)
					{
						CornerGlyph cornerGlyph = (CornerGlyph)(object)g;
						int num = mouseLoc.X - _dragStart.X;
						if (num == 0)
						{
							return true;
						}
						_dragStart = mouseLoc;
						switch (unchecked((int)whatCorner))
						{
						case 2:
						case 4:
							sngSliderPos += num;
							break;
						case 0:
						case 1:
						case 3:
							sngSliderPos += num;
							break;
						}
						if (sngSliderPos < 0f)
						{
							sngSliderPos = 0f;
						}
						if (sngSliderPos > (float)(cornerGlyph.CornerTrack.Width - 10))
						{
							sngSliderPos = cornerGlyph.CornerTrack.Width - 10;
						}
						switch (unchecked((int)whatCorner))
						{
						case 2:
						case 4:
							cornerGlyph.SetValue((int)Math.Round((double)cornerGlyph.maxCorner - (double)sngSliderPos / ((double)(cornerGlyph.CornerTrack.Width - 10) / (double)cornerGlyph.maxCorner)));
							break;
						case 0:
						case 1:
						case 3:
							cornerGlyph.SetValue((int)Math.Round((double)sngSliderPos / ((double)(cornerGlyph.CornerTrack.Width - 10) / (double)cornerGlyph.maxCorner)));
							break;
						}
						if (whatCorner == eCorner.All)
						{
							foreach (Glyph item in (CollectionBase)(object)cornerGlyph.CornAdorner.Glyphs)
							{
								Glyph val = item;
								cornerGlyph.CornAdorner.Invalidate(val.Bounds);
							}
						}
						else
						{
							cornerGlyph.CornAdorner.Invalidate(cornerGlyph.Bounds);
						}
						((Control)relatedControl).Refresh();
					}
					return true;
				}
			}

			public override bool OnMouseEnter(Glyph g)
			{
				_dragging = true;
				return true;
			}

			public override bool OnMouseLeave(Glyph g)
			{
				if (_dragging)
				{
					SetCorners();
					_dragging = false;
				}
				return true;
			}

			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				_dragging = false;
				SetCorners();
				return true;
			}

			private void SetCorners()
			{
				if (whatCorner == eCorner.All)
				{
					SetControlProperty("Corners", new CornersProperty(relatedControl.Corners.All));
				}
				else
				{
					SetControlProperty("Corners", new CornersProperty(relatedControl.Corners.LowerLeft, relatedControl.Corners.LowerRight, relatedControl.Corners.UpperLeft, relatedControl.Corners.UpperRight));
				}
			}

			private void SetControlProperty(string property_name, object value)
			{
				TypeDescriptor.GetProperties(relatedControl)[property_name].SetValue(relatedControl, RuntimeHelpers.GetObjectValue(value));
			}
		}

		private CButton relatedControl;

		private BehaviorService behaviorSvc;

		private ISelectionService selectionSvc;

		private IDesigner relatedDesigner;

		private Adorner CornAdorner;

		private Brush boxFillColor;

		public Rectangle CornerTrack;

		public int maxCorner;

		public eCorner WhatCorner
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		public override Rectangle Bounds => CornerTrack;

		public CornerGlyph(BehaviorService behaviorService, CButton control, ISelectionService selectionService, IDesigner relatedDesigner, Adorner myAdorner, eCorner whatCorner)
			: base((Behavior)(object)new CornBehavior(relatedDesigner, whatCorner))
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			selectionSvc = null;
			this.relatedDesigner = null;
			boxFillColor = (Brush)new SolidBrush(Color.FromArgb(120, Color.White));
			behaviorSvc = behaviorService;
			selectionSvc = selectionService;
			this.relatedDesigner = relatedDesigner;
			CornAdorner = myAdorner;
			WhatCorner = whatCorner;
			relatedControl = (CButton)this.relatedDesigner.Component;
			((Control)relatedControl).Resize += relatedControl_Resize;
		}

		private void relatedControl_Resize(object sender, EventArgs e)
		{
			SizeCornerTrack();
			((Control)relatedControl).Refresh();
			CornAdorner.Invalidate(Bounds);
		}

		public Rectangle GetCorn()
		{
			Point point = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
			int num;
			int num2;
			if (((Control)relatedControl).Width / 2 < 30)
			{
				checked
				{
					num = unchecked(((Control)relatedControl).Width / 2) - 30;
					num2 = 30;
				}
			}
			else
			{
				num = 0;
				num2 = Math.Min(((Control)relatedControl).Width / 2, 80);
			}
			checked
			{
				switch (unchecked((int)WhatCorner))
				{
				case 0:
					num2 = Math.Min(((Control)relatedControl).Width, 100);
					return new Rectangle(point.X + unchecked(checked(((Control)relatedControl).Width - num2) / 2), point.Y + unchecked(((Control)relatedControl).Height / 2) - 5, num2, 10);
				case 1:
					return new Rectangle(point.X + num, point.Y - 15, num2, 10);
				case 2:
					return new Rectangle(point.X - num + ((Control)relatedControl).Width - num2, point.Y - 15, num2, 10);
				case 3:
					return new Rectangle(point.X + num, point.Y + ((Control)relatedControl).Height + 5, num2, 10);
				case 4:
					return new Rectangle(point.X - num + ((Control)relatedControl).Width - num2, point.Y + ((Control)relatedControl).Height + 5, num2, 10);
				default:
				{
					Rectangle result = default(Rectangle);
					return result;
				}
				}
			}
		}

		public void SizeCornerTrack()
		{
			Point point = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
			CornerTrack = GetCorn();
			checked
			{
				maxCorner = (int)Math.Max(unchecked(checked((long)Math.Round(Math.Min(relatedControl.ButtonArea.Width, relatedControl.ButtonArea.Height))) / 2), 0L);
				if (maxCorner <= 0)
				{
					return;
				}
				if (WhatCorner == eCorner.All)
				{
					if ((relatedControl.Corners.All != -1 && relatedControl.Corners.All > maxCorner) ? true : false)
					{
						relatedControl.Corners.All = maxCorner;
					}
				}
				else if (GetValue() > maxCorner)
				{
					SetValue(maxCorner);
				}
			}
		}

		private int GetValue()
		{
			int num = default(int);
			return (int)WhatCorner switch
			{
				0 => relatedControl.Corners.All, 
				1 => relatedControl.Corners.UpperLeft, 
				2 => relatedControl.Corners.UpperRight, 
				3 => relatedControl.Corners.LowerLeft, 
				4 => relatedControl.Corners.LowerRight, 
				_ => num, 
			};
		}

		private void SetValue(int val)
		{
			switch ((int)WhatCorner)
			{
			case 0:
				relatedControl.Corners.All = val;
				break;
			case 1:
				relatedControl.Corners.UpperLeft = val;
				break;
			case 2:
				relatedControl.Corners.UpperRight = val;
				break;
			case 3:
				relatedControl.Corners.LowerLeft = val;
				break;
			case 4:
				relatedControl.Corners.LowerRight = val;
				break;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			if ((object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) && ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat == ChooseGlyph.eChooseWhat.Corner) ? true : false)
			{
				Rectangle rectangle = new Rectangle(checked(CornerTrack.X + GetPos()), CornerTrack.Y, 10, 10);
				if (((CornBehavior)(object)((Glyph)this).Behavior)._dragging)
				{
					rectangle.Inflate(10, 10);
				}
				if (rectangle.Contains(p))
				{
					return Cursors.VSplit;
				}
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			checked
			{
				if ((object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) && ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat == ChooseGlyph.eChooseWhat.Corner) ? true : false)
				{
					Pen val = new Pen(Brushes.DarkGray, 1f);
					val.DashStyle = (DashStyle)0;
					Pen val2 = val;
					try
					{
						StringFormat val3 = new StringFormat();
						val3.Alignment = (StringAlignment)1;
						val3.LineAlignment = (StringAlignment)1;
						StringFormat val4 = val3;
						SizeCornerTrack();
						int pos = GetPos();
						Graphics graphics = pe.Graphics;
						Brush cornflowerBlue = Brushes.CornflowerBlue;
						Rectangle rectangle = new Rectangle(CornerTrack.X + pos, CornerTrack.Y, 10, 10);
						graphics.FillRectangle(cornflowerBlue, rectangle);
						val2.Color = Color.White;
						Graphics graphics2 = pe.Graphics;
						rectangle = new Rectangle(CornerTrack.X + pos, CornerTrack.Y, 9, 9);
						graphics2.DrawRectangle(val2, rectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
		}

		private int GetPos()
		{
			switch ((int)WhatCorner)
			{
			case 2:
			case 4:
				return checked((int)Math.Round((double)(CornerTrack.Width - 10) * ((double)(maxCorner - GetValue()) / (double)maxCorner)));
			case 0:
			case 1:
			case 3:
				return checked((int)Math.Round((double)(CornerTrack.Width - 10) * ((double)GetValue() / (double)maxCorner)));
			default:
			{
				int result = default(int);
				return result;
			}
			}
		}
	}
	[ToolboxItemFilter("Prevent", ToolboxItemFilterType.Prevent)]
	[DesignerGenerated]
	[ToolboxItem(false)]
	public class DropdownColorBlender : UserControl
	{
		private static List<WeakReference> __ENCList = new List<WeakReference>();

		private IContainer components;

		[AccessedThroughProperty("panProps")]
		private Panel _panProps;

		[AccessedThroughProperty("Label4")]
		private Label _Label4;

		[AccessedThroughProperty("Label3")]
		private Label _Label3;

		[AccessedThroughProperty("Label2")]
		private Label _Label2;

		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		[AccessedThroughProperty("nudBlue")]
		private NumericUpDown _nudBlue;

		[AccessedThroughProperty("txbCurrColor")]
		private TextBox _txbCurrColor;

		[AccessedThroughProperty("panCurrColor")]
		private Panel _panCurrColor;

		[AccessedThroughProperty("nudGreen")]
		private NumericUpDown _nudGreen;

		[AccessedThroughProperty("nudRed")]
		private NumericUpDown _nudRed;

		[AccessedThroughProperty("Panel20")]
		private Panel _Panel20;

		[AccessedThroughProperty("Panel22")]
		private Panel _Panel22;

		[AccessedThroughProperty("ColorBox")]
		private ComboBox _ColorBox;

		[AccessedThroughProperty("Panel23")]
		private Panel _Panel23;

		[AccessedThroughProperty("Panel24")]
		private Panel _Panel24;

		[AccessedThroughProperty("Panel25")]
		private Panel _Panel25;

		[AccessedThroughProperty("Panel26")]
		private Panel _Panel26;

		[AccessedThroughProperty("Panel27")]
		private Panel _Panel27;

		[AccessedThroughProperty("Panel28")]
		private Panel _Panel28;

		[AccessedThroughProperty("Panel18")]
		private Panel _Panel18;

		[AccessedThroughProperty("Panel21")]
		private Panel _Panel21;

		[AccessedThroughProperty("Panel17")]
		private Panel _Panel17;

		[AccessedThroughProperty("Panel16")]
		private Panel _Panel16;

		[AccessedThroughProperty("Panel15")]
		private Panel _Panel15;

		[AccessedThroughProperty("Panel14")]
		private Panel _Panel14;

		[AccessedThroughProperty("Panel19")]
		private Panel _Panel19;

		[AccessedThroughProperty("Panel13")]
		private Panel _Panel13;

		[AccessedThroughProperty("Panel12")]
		private Panel _Panel12;

		[AccessedThroughProperty("Panel11")]
		private Panel _Panel11;

		[AccessedThroughProperty("Panel10")]
		private Panel _Panel10;

		[AccessedThroughProperty("Panel9")]
		private Panel _Panel9;

		[AccessedThroughProperty("Panel8")]
		private Panel _Panel8;

		[AccessedThroughProperty("Panel29")]
		private Panel _Panel29;

		[AccessedThroughProperty("Panel7")]
		private Panel _Panel7;

		[AccessedThroughProperty("Panel6")]
		private Panel _Panel6;

		[AccessedThroughProperty("TabControl1")]
		private TabControl _TabControl1;

		[AccessedThroughProperty("TabPage1")]
		private TabPage _TabPage1;

		[AccessedThroughProperty("TabPage3")]
		private TabPage _TabPage3;

		[AccessedThroughProperty("tbarAlpha")]
		private TrackBar _tbarAlpha;

		[AccessedThroughProperty("txbAlpha")]
		private TextBox _txbAlpha;

		[AccessedThroughProperty("TabPage2")]
		private TabPage _TabPage2;

		[AccessedThroughProperty("butClear")]
		private Button _butClear;

		[AccessedThroughProperty("butApply")]
		private Button _butApply;

		[AccessedThroughProperty("panSampleHolder")]
		private Panel _panSampleHolder;

		[AccessedThroughProperty("TheSample")]
		private CButton _TheSample;

		private IWindowsFormsEditorService _editorService;

		private cblPointer StartPointer;

		private cblPointer EndPointer;

		private Collection MiddlePointers;

		private bool MouseMoving;

		private int CurrPointer;

		private int TopMargin;

		private Color[] _BlendColors;

		private float[] _BlendPositions;

		private int _BarHeight;

		private Panel CurrSwatch;

		internal virtual Panel panProps
		{
			[DebuggerNonUserCode]
			get
			{
				return _panProps;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = panProps_SizeChanged;
				if (_panProps != null)
				{
					((Control)_panProps).SizeChanged -= eventHandler;
				}
				_panProps = value;
				if (_panProps != null)
				{
					((Control)_panProps).SizeChanged += eventHandler;
				}
			}
		}

		internal virtual Label Label4
		{
			[DebuggerNonUserCode]
			get
			{
				return _Label4;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_Label4 = value;
			}
		}

		internal virtual Label Label3
		{
			[DebuggerNonUserCode]
			get
			{
				return _Label3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_Label3 = value;
			}
		}

		internal virtual Label Label2
		{
			[DebuggerNonUserCode]
			get
			{
				return _Label2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_Label2 = value;
			}
		}

		internal virtual Label Label1
		{
			[DebuggerNonUserCode]
			get
			{
				return _Label1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_Label1 = value;
			}
		}

		internal virtual NumericUpDown nudBlue
		{
			[DebuggerNonUserCode]
			get
			{
				return _nudBlue;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = nud_ValueChanged;
				if (_nudBlue != null)
				{
					_nudBlue.ValueChanged -= eventHandler;
				}
				_nudBlue = value;
				if (_nudBlue != null)
				{
					_nudBlue.ValueChanged += eventHandler;
				}
			}
		}

		internal virtual TextBox txbCurrColor
		{
			[DebuggerNonUserCode]
			get
			{
				return _txbCurrColor;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_txbCurrColor = value;
			}
		}

		internal virtual Panel panCurrColor
		{
			[DebuggerNonUserCode]
			get
			{
				return _panCurrColor;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_panCurrColor = value;
			}
		}

		internal virtual NumericUpDown nudGreen
		{
			[DebuggerNonUserCode]
			get
			{
				return _nudGreen;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = nud_ValueChanged;
				if (_nudGreen != null)
				{
					_nudGreen.ValueChanged -= eventHandler;
				}
				_nudGreen = value;
				if (_nudGreen != null)
				{
					_nudGreen.ValueChanged += eventHandler;
				}
			}
		}

		internal virtual NumericUpDown nudRed
		{
			[DebuggerNonUserCode]
			get
			{
				return _nudRed;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = nud_ValueChanged;
				if (_nudRed != null)
				{
					_nudRed.ValueChanged -= eventHandler;
				}
				_nudRed = value;
				if (_nudRed != null)
				{
					_nudRed.ValueChanged += eventHandler;
				}
			}
		}

		internal virtual Panel Panel20
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel20;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel20 != null)
				{
					((Control)_Panel20).Click -= eventHandler;
					((Control)_Panel20).MouseEnter -= eventHandler2;
				}
				_Panel20 = value;
				if (_Panel20 != null)
				{
					((Control)_Panel20).Click += eventHandler;
					((Control)_Panel20).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel22
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel22;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel22 != null)
				{
					((Control)_Panel22).Click -= eventHandler;
					((Control)_Panel22).MouseEnter -= eventHandler2;
				}
				_Panel22 = value;
				if (_Panel22 != null)
				{
					((Control)_Panel22).Click += eventHandler;
					((Control)_Panel22).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual ComboBox ColorBox
		{
			[DebuggerNonUserCode]
			get
			{
				return _ColorBox;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = ColorBox_SelectedIndexChanged;
				if (_ColorBox != null)
				{
					_ColorBox.SelectedIndexChanged -= eventHandler;
				}
				_ColorBox = value;
				if (_ColorBox != null)
				{
					_ColorBox.SelectedIndexChanged += eventHandler;
				}
			}
		}

		internal virtual Panel Panel23
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel23;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel23 != null)
				{
					((Control)_Panel23).Click -= eventHandler;
					((Control)_Panel23).MouseEnter -= eventHandler2;
				}
				_Panel23 = value;
				if (_Panel23 != null)
				{
					((Control)_Panel23).Click += eventHandler;
					((Control)_Panel23).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel24
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel24;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel24 != null)
				{
					((Control)_Panel24).Click -= eventHandler;
					((Control)_Panel24).MouseEnter -= eventHandler2;
				}
				_Panel24 = value;
				if (_Panel24 != null)
				{
					((Control)_Panel24).Click += eventHandler;
					((Control)_Panel24).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel25
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel25;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel25 != null)
				{
					((Control)_Panel25).Click -= eventHandler;
					((Control)_Panel25).MouseEnter -= eventHandler2;
				}
				_Panel25 = value;
				if (_Panel25 != null)
				{
					((Control)_Panel25).Click += eventHandler;
					((Control)_Panel25).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel26
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel26;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel26 != null)
				{
					((Control)_Panel26).Click -= eventHandler;
					((Control)_Panel26).MouseEnter -= eventHandler2;
				}
				_Panel26 = value;
				if (_Panel26 != null)
				{
					((Control)_Panel26).Click += eventHandler;
					((Control)_Panel26).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel27
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel27;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel27 != null)
				{
					((Control)_Panel27).Click -= eventHandler;
					((Control)_Panel27).MouseEnter -= eventHandler2;
				}
				_Panel27 = value;
				if (_Panel27 != null)
				{
					((Control)_Panel27).Click += eventHandler;
					((Control)_Panel27).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel28
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel28;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel28 != null)
				{
					((Control)_Panel28).Click -= eventHandler;
					((Control)_Panel28).MouseEnter -= eventHandler2;
				}
				_Panel28 = value;
				if (_Panel28 != null)
				{
					((Control)_Panel28).Click += eventHandler;
					((Control)_Panel28).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel18
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel18;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel18 != null)
				{
					((Control)_Panel18).Click -= eventHandler;
					((Control)_Panel18).MouseEnter -= eventHandler2;
				}
				_Panel18 = value;
				if (_Panel18 != null)
				{
					((Control)_Panel18).Click += eventHandler;
					((Control)_Panel18).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel21
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel21;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel21 != null)
				{
					((Control)_Panel21).Click -= eventHandler;
					((Control)_Panel21).MouseEnter -= eventHandler2;
				}
				_Panel21 = value;
				if (_Panel21 != null)
				{
					((Control)_Panel21).Click += eventHandler;
					((Control)_Panel21).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel17
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel17;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel17 != null)
				{
					((Control)_Panel17).Click -= eventHandler;
					((Control)_Panel17).MouseEnter -= eventHandler2;
				}
				_Panel17 = value;
				if (_Panel17 != null)
				{
					((Control)_Panel17).Click += eventHandler;
					((Control)_Panel17).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel16
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel16;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel16 != null)
				{
					((Control)_Panel16).Click -= eventHandler;
					((Control)_Panel16).MouseEnter -= eventHandler2;
				}
				_Panel16 = value;
				if (_Panel16 != null)
				{
					((Control)_Panel16).Click += eventHandler;
					((Control)_Panel16).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel15
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel15;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel15 != null)
				{
					((Control)_Panel15).Click -= eventHandler;
					((Control)_Panel15).MouseEnter -= eventHandler2;
				}
				_Panel15 = value;
				if (_Panel15 != null)
				{
					((Control)_Panel15).Click += eventHandler;
					((Control)_Panel15).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel14
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel14;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel14 != null)
				{
					((Control)_Panel14).Click -= eventHandler;
					((Control)_Panel14).MouseEnter -= eventHandler2;
				}
				_Panel14 = value;
				if (_Panel14 != null)
				{
					((Control)_Panel14).Click += eventHandler;
					((Control)_Panel14).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel19
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel19;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel19 != null)
				{
					((Control)_Panel19).Click -= eventHandler;
					((Control)_Panel19).MouseEnter -= eventHandler2;
				}
				_Panel19 = value;
				if (_Panel19 != null)
				{
					((Control)_Panel19).Click += eventHandler;
					((Control)_Panel19).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel13
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel13;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel13 != null)
				{
					((Control)_Panel13).Click -= eventHandler;
					((Control)_Panel13).MouseEnter -= eventHandler2;
				}
				_Panel13 = value;
				if (_Panel13 != null)
				{
					((Control)_Panel13).Click += eventHandler;
					((Control)_Panel13).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel12
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel12;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel12 != null)
				{
					((Control)_Panel12).Click -= eventHandler;
					((Control)_Panel12).MouseEnter -= eventHandler2;
				}
				_Panel12 = value;
				if (_Panel12 != null)
				{
					((Control)_Panel12).Click += eventHandler;
					((Control)_Panel12).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel11
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel11;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel11 != null)
				{
					((Control)_Panel11).Click -= eventHandler;
					((Control)_Panel11).MouseEnter -= eventHandler2;
				}
				_Panel11 = value;
				if (_Panel11 != null)
				{
					((Control)_Panel11).Click += eventHandler;
					((Control)_Panel11).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel10
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel10;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel10 != null)
				{
					((Control)_Panel10).Click -= eventHandler;
					((Control)_Panel10).MouseEnter -= eventHandler2;
				}
				_Panel10 = value;
				if (_Panel10 != null)
				{
					((Control)_Panel10).Click += eventHandler;
					((Control)_Panel10).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel9
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel9;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel9 != null)
				{
					((Control)_Panel9).Click -= eventHandler;
					((Control)_Panel9).MouseEnter -= eventHandler2;
				}
				_Panel9 = value;
				if (_Panel9 != null)
				{
					((Control)_Panel9).Click += eventHandler;
					((Control)_Panel9).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel8
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel8;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel8 != null)
				{
					((Control)_Panel8).Click -= eventHandler;
					((Control)_Panel8).MouseEnter -= eventHandler2;
				}
				_Panel8 = value;
				if (_Panel8 != null)
				{
					((Control)_Panel8).Click += eventHandler;
					((Control)_Panel8).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel29
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel29;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel29 != null)
				{
					((Control)_Panel29).Click -= eventHandler;
					((Control)_Panel29).MouseEnter -= eventHandler2;
				}
				_Panel29 = value;
				if (_Panel29 != null)
				{
					((Control)_Panel29).Click += eventHandler;
					((Control)_Panel29).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel7
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel7;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel7 != null)
				{
					((Control)_Panel7).Click -= eventHandler;
					((Control)_Panel7).MouseEnter -= eventHandler2;
				}
				_Panel7 = value;
				if (_Panel7 != null)
				{
					((Control)_Panel7).Click += eventHandler;
					((Control)_Panel7).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual Panel Panel6
		{
			[DebuggerNonUserCode]
			get
			{
				return _Panel6;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Panel7_Click;
				EventHandler eventHandler2 = Panel10_MouseEnter;
				if (_Panel6 != null)
				{
					((Control)_Panel6).Click -= eventHandler;
					((Control)_Panel6).MouseEnter -= eventHandler2;
				}
				_Panel6 = value;
				if (_Panel6 != null)
				{
					((Control)_Panel6).Click += eventHandler;
					((Control)_Panel6).MouseEnter += eventHandler2;
				}
			}
		}

		internal virtual TabControl TabControl1
		{
			[DebuggerNonUserCode]
			get
			{
				return _TabControl1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = TabControl1_MouseLeave;
				if (_TabControl1 != null)
				{
					((Control)_TabControl1).MouseLeave -= eventHandler;
				}
				_TabControl1 = value;
				if (_TabControl1 != null)
				{
					((Control)_TabControl1).MouseLeave += eventHandler;
				}
			}
		}

		internal virtual TabPage TabPage1
		{
			[DebuggerNonUserCode]
			get
			{
				return _TabPage1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_TabPage1 = value;
			}
		}

		internal virtual TabPage TabPage3
		{
			[DebuggerNonUserCode]
			get
			{
				return _TabPage3;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_TabPage3 = value;
			}
		}

		internal virtual TrackBar tbarAlpha
		{
			[DebuggerNonUserCode]
			get
			{
				return _tbarAlpha;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = nud_ValueChanged;
				if (_tbarAlpha != null)
				{
					_tbarAlpha.ValueChanged -= eventHandler;
				}
				_tbarAlpha = value;
				if (_tbarAlpha != null)
				{
					_tbarAlpha.ValueChanged += eventHandler;
				}
			}
		}

		internal virtual TextBox txbAlpha
		{
			[DebuggerNonUserCode]
			get
			{
				return _txbAlpha;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = txbAlpha_TextChanged;
				if (_txbAlpha != null)
				{
					((Control)_txbAlpha).TextChanged -= eventHandler;
				}
				_txbAlpha = value;
				if (_txbAlpha != null)
				{
					((Control)_txbAlpha).TextChanged += eventHandler;
				}
			}
		}

		internal virtual TabPage TabPage2
		{
			[DebuggerNonUserCode]
			get
			{
				return _TabPage2;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_TabPage2 = value;
			}
		}

		internal virtual Button butClear
		{
			[DebuggerNonUserCode]
			get
			{
				return _butClear;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = butClear_Click;
				if (_butClear != null)
				{
					((Control)_butClear).Click -= eventHandler;
				}
				_butClear = value;
				if (_butClear != null)
				{
					((Control)_butClear).Click += eventHandler;
				}
			}
		}

		internal virtual Button butApply
		{
			[DebuggerNonUserCode]
			get
			{
				return _butApply;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = butApply_Click;
				if (_butApply != null)
				{
					((Control)_butApply).Click -= eventHandler;
				}
				_butApply = value;
				if (_butApply != null)
				{
					((Control)_butApply).Click += eventHandler;
				}
			}
		}

		internal virtual Panel panSampleHolder
		{
			[DebuggerNonUserCode]
			get
			{
				return _panSampleHolder;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = panSampleHolder_SizeChanged;
				if (_panSampleHolder != null)
				{
					((Control)_panSampleHolder).SizeChanged -= eventHandler;
				}
				_panSampleHolder = value;
				if (_panSampleHolder != null)
				{
					((Control)_panSampleHolder).SizeChanged += eventHandler;
				}
			}
		}

		internal virtual CButton TheSample
		{
			[DebuggerNonUserCode]
			get
			{
				return _TheSample;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_TheSample = value;
			}
		}

		[Category("ColorBlender")]
		[Description("Array of Colors used in ColorBlend")]
		public Color[] BlendColors
		{
			get
			{
				return _BlendColors;
			}
			set
			{
				_BlendColors = value;
			}
		}

		[Category("ColorBlender")]
		[Description("Array of Color Positions used in ColorBlend")]
		public float[] BlendPositions
		{
			get
			{
				return _BlendPositions;
			}
			set
			{
				_BlendPositions = value;
			}
		}

		[Description("Height of color blender bar")]
		[Category("ColorBlender")]
		public int BarHeight
		{
			get
			{
				return _BarHeight;
			}
			set
			{
				_BarHeight = value;
				Panel obj = panProps;
				Point location = new Point(10, checked(value + 20));
				((Control)obj).Location = location;
				((Control)this).Invalidate();
			}
		}

		[DebuggerNonUserCode]
		private static void __ENCAddToList(object value)
		{
			checked
			{
				lock (__ENCList)
				{
					if (__ENCList.Count == __ENCList.Capacity)
					{
						int num = 0;
						int num2 = __ENCList.Count - 1;
						int num3 = 0;
						while (true)
						{
							int num4 = num3;
							int num5 = num2;
							if (num4 > num5)
							{
								break;
							}
							WeakReference weakReference = __ENCList[num3];
							if (weakReference.IsAlive)
							{
								if (num3 != num)
								{
									__ENCList[num] = __ENCList[num3];
								}
								num++;
							}
							num3++;
						}
						__ENCList.RemoveRange(num, __ENCList.Count - num);
						__ENCList.Capacity = __ENCList.Count;
					}
					__ENCList.Add(new WeakReference(RuntimeHelpers.GetObjectValue(value)));
				}
			}
		}

		[DebuggerNonUserCode]
		protected override void Dispose(bool disposing)
		{
			try
			{
				if ((disposing && components != null) ? true : false)
				{
					components.Dispose();
				}
			}
			finally
			{
				((ContainerControl)this).Dispose(disposing);
			}
		}

		[DebuggerStepThrough]
		private void InitializeComponent()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Expected O, but got Unknown
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Expected O, but got Unknown
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Expected O, but got Unknown
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Expected O, but got Unknown
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Expected O, but got Unknown
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Expected O, but got Unknown
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Expected O, but got Unknown
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Expected O, but got Unknown
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Expected O, but got Unknown
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Expected O, but got Unknown
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Expected O, but got Unknown
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Expected O, but got Unknown
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Expected O, but got Unknown
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Expected O, but got Unknown
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Expected O, but got Unknown
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b7: Expected O, but got Unknown
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_1570: Unknown result type (might be due to invalid IL or missing references)
			//IL_157a: Expected O, but got Unknown
			//IL_15ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_15f7: Expected O, but got Unknown
			//IL_168d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1697: Expected O, but got Unknown
			//IL_172d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1737: Expected O, but got Unknown
			//IL_1848: Unknown result type (might be due to invalid IL or missing references)
			//IL_1901: Unknown result type (might be due to invalid IL or missing references)
			//IL_190b: Expected O, but got Unknown
			//IL_1d78: Unknown result type (might be due to invalid IL or missing references)
			components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DropdownColorBlender));
			cBlendItems cBlendItems2 = new cBlendItems();
			panProps = new Panel();
			TabControl1 = new TabControl();
			TabPage1 = new TabPage();
			Panel20 = new Panel();
			Panel7 = new Panel();
			Panel22 = new Panel();
			Panel6 = new Panel();
			Panel29 = new Panel();
			Panel23 = new Panel();
			Panel8 = new Panel();
			Panel24 = new Panel();
			Panel9 = new Panel();
			Panel25 = new Panel();
			Panel10 = new Panel();
			Panel26 = new Panel();
			Panel11 = new Panel();
			Panel27 = new Panel();
			Panel12 = new Panel();
			Panel28 = new Panel();
			Panel13 = new Panel();
			Panel18 = new Panel();
			Panel19 = new Panel();
			Panel21 = new Panel();
			Panel14 = new Panel();
			Panel17 = new Panel();
			Panel15 = new Panel();
			Panel16 = new Panel();
			TabPage3 = new TabPage();
			nudRed = new NumericUpDown();
			nudGreen = new NumericUpDown();
			nudBlue = new NumericUpDown();
			ColorBox = new ComboBox();
			Label2 = new Label();
			Label4 = new Label();
			Label3 = new Label();
			TabPage2 = new TabPage();
			txbAlpha = new TextBox();
			Label1 = new Label();
			tbarAlpha = new TrackBar();
			txbCurrColor = new TextBox();
			panCurrColor = new Panel();
			butClear = new Button();
			butApply = new Button();
			panSampleHolder = new Panel();
			TheSample = new CButton();
			((Control)panProps).SuspendLayout();
			((Control)TabControl1).SuspendLayout();
			((Control)TabPage1).SuspendLayout();
			((Control)TabPage3).SuspendLayout();
			((ISupportInitialize)nudRed).BeginInit();
			((ISupportInitialize)nudGreen).BeginInit();
			((ISupportInitialize)nudBlue).BeginInit();
			((Control)TabPage2).SuspendLayout();
			((ISupportInitialize)tbarAlpha).BeginInit();
			((Control)panSampleHolder).SuspendLayout();
			((Control)this).SuspendLayout();
			((Control)panProps).Controls.Add((Control)(object)TabControl1);
			((Control)panProps).Controls.Add((Control)(object)txbCurrColor);
			((Control)panProps).Controls.Add((Control)(object)panCurrColor);
			Panel obj = panProps;
			Point location = new Point(3, 48);
			((Control)obj).Location = location;
			((Control)panProps).Name = "panProps";
			Panel obj2 = panProps;
			Size size = new Size(186, 115);
			((Control)obj2).Size = size;
			((Control)panProps).TabIndex = 6;
			((Control)TabControl1).Controls.Add((Control)(object)TabPage1);
			((Control)TabControl1).Controls.Add((Control)(object)TabPage3);
			((Control)TabControl1).Controls.Add((Control)(object)TabPage2);
			((Control)TabControl1).Font = new Font("Microsoft Sans Serif", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			TabControl tabControl = TabControl1;
			size = new Size(59, 18);
			tabControl.ItemSize = size;
			TabControl tabControl2 = TabControl1;
			location = new Point(4, 24);
			((Control)tabControl2).Location = location;
			((Control)TabControl1).Name = "TabControl1";
			TabControl1.SelectedIndex = 0;
			TabControl tabControl3 = TabControl1;
			size = new Size(182, 90);
			((Control)tabControl3).Size = size;
			((Control)TabControl1).TabIndex = 11;
			((Control)TabPage1).Controls.Add((Control)(object)Panel20);
			((Control)TabPage1).Controls.Add((Control)(object)Panel7);
			((Control)TabPage1).Controls.Add((Control)(object)Panel22);
			((Control)TabPage1).Controls.Add((Control)(object)Panel6);
			((Control)TabPage1).Controls.Add((Control)(object)Panel29);
			((Control)TabPage1).Controls.Add((Control)(object)Panel23);
			((Control)TabPage1).Controls.Add((Control)(object)Panel8);
			((Control)TabPage1).Controls.Add((Control)(object)Panel24);
			((Control)TabPage1).Controls.Add((Control)(object)Panel9);
			((Control)TabPage1).Controls.Add((Control)(object)Panel25);
			((Control)TabPage1).Controls.Add((Control)(object)Panel10);
			((Control)TabPage1).Controls.Add((Control)(object)Panel26);
			((Control)TabPage1).Controls.Add((Control)(object)Panel11);
			((Control)TabPage1).Controls.Add((Control)(object)Panel27);
			((Control)TabPage1).Controls.Add((Control)(object)Panel12);
			((Control)TabPage1).Controls.Add((Control)(object)Panel28);
			((Control)TabPage1).Controls.Add((Control)(object)Panel13);
			((Control)TabPage1).Controls.Add((Control)(object)Panel18);
			((Control)TabPage1).Controls.Add((Control)(object)Panel19);
			((Control)TabPage1).Controls.Add((Control)(object)Panel21);
			((Control)TabPage1).Controls.Add((Control)(object)Panel14);
			((Control)TabPage1).Controls.Add((Control)(object)Panel17);
			((Control)TabPage1).Controls.Add((Control)(object)Panel15);
			((Control)TabPage1).Controls.Add((Control)(object)Panel16);
			TabPage tabPage = TabPage1;
			location = new Point(4, 22);
			tabPage.Location = location;
			((Control)TabPage1).Name = "TabPage1";
			TabPage tabPage2 = TabPage1;
			Padding val = default(Padding);
			((Padding)(ref val))..ctor(3);
			((Control)tabPage2).Padding = val;
			TabPage tabPage3 = TabPage1;
			size = new Size(174, 64);
			((Control)tabPage3).Size = size;
			TabPage1.TabIndex = 0;
			TabPage1.Text = "Swatches";
			TabPage1.UseVisualStyleBackColor = true;
			((Control)Panel20).BackColor = Color.FromArgb(255, 128, 0);
			Panel20.BorderStyle = (BorderStyle)2;
			Panel panel = Panel20;
			location = new Point(48, 6);
			((Control)panel).Location = location;
			((Control)Panel20).Name = "Panel20";
			Panel panel2 = Panel20;
			size = new Size(17, 17);
			((Control)panel2).Size = size;
			((Control)Panel20).TabIndex = 35;
			((Control)Panel7).BackColor = Color.White;
			Panel7.BorderStyle = (BorderStyle)2;
			Panel panel3 = Panel7;
			location = new Point(6, 6);
			((Control)panel3).Location = location;
			((Control)Panel7).Name = "Panel7";
			Panel panel4 = Panel7;
			size = new Size(17, 17);
			((Control)panel4).Size = size;
			((Control)Panel7).TabIndex = 29;
			((Control)Panel22).BackColor = Color.FromArgb(255, 192, 255);
			Panel22.BorderStyle = (BorderStyle)2;
			Panel panel5 = Panel22;
			location = new Point(153, 44);
			((Control)panel5).Location = location;
			((Control)Panel22).Name = "Panel22";
			Panel panel6 = Panel22;
			size = new Size(17, 17);
			((Control)panel6).Size = size;
			((Control)Panel22).TabIndex = 43;
			((Control)Panel6).BackColor = Color.Black;
			Panel6.BorderStyle = (BorderStyle)2;
			Panel panel7 = Panel6;
			location = new Point(6, 44);
			((Control)panel7).Location = location;
			((Control)Panel6).Name = "Panel6";
			Panel panel8 = Panel6;
			size = new Size(17, 17);
			((Control)panel8).Size = size;
			((Control)Panel6).TabIndex = 30;
			((Control)Panel29).BackColor = Color.Silver;
			Panel29.BorderStyle = (BorderStyle)2;
			Panel panel9 = Panel29;
			location = new Point(6, 25);
			((Control)panel9).Location = location;
			((Control)Panel29).Name = "Panel29";
			Panel panel10 = Panel29;
			size = new Size(17, 17);
			((Control)panel10).Size = size;
			((Control)Panel29).TabIndex = 28;
			((Control)Panel23).BackColor = Color.FromArgb(192, 255, 255);
			Panel23.BorderStyle = (BorderStyle)2;
			Panel panel11 = Panel23;
			location = new Point(111, 44);
			((Control)panel11).Location = location;
			((Control)Panel23).Name = "Panel23";
			Panel panel12 = Panel23;
			size = new Size(17, 17);
			((Control)panel12).Size = size;
			((Control)Panel23).TabIndex = 44;
			((Control)Panel8).BackColor = Color.Red;
			Panel8.BorderStyle = (BorderStyle)2;
			Panel panel13 = Panel8;
			location = new Point(27, 6);
			((Control)panel13).Location = location;
			((Control)Panel8).Name = "Panel8";
			Panel panel14 = Panel8;
			size = new Size(17, 17);
			((Control)panel14).Size = size;
			((Control)Panel8).TabIndex = 33;
			((Control)Panel24).BackColor = Color.FromArgb(192, 192, 255);
			Panel24.BorderStyle = (BorderStyle)2;
			Panel panel15 = Panel24;
			location = new Point(132, 44);
			((Control)panel15).Location = location;
			((Control)Panel24).Name = "Panel24";
			Panel panel16 = Panel24;
			size = new Size(17, 17);
			((Control)panel16).Size = size;
			((Control)Panel24).TabIndex = 45;
			((Control)Panel9).BackColor = Color.Lime;
			Panel9.BorderStyle = (BorderStyle)2;
			Panel panel17 = Panel9;
			location = new Point(90, 6);
			((Control)panel17).Location = location;
			((Control)Panel9).Name = "Panel9";
			Panel panel18 = Panel9;
			size = new Size(17, 17);
			((Control)panel18).Size = size;
			((Control)Panel9).TabIndex = 32;
			((Control)Panel25).BackColor = Color.FromArgb(192, 255, 192);
			Panel25.BorderStyle = (BorderStyle)2;
			Panel panel19 = Panel25;
			location = new Point(90, 44);
			((Control)panel19).Location = location;
			((Control)Panel25).Name = "Panel25";
			Panel panel20 = Panel25;
			size = new Size(17, 17);
			((Control)panel20).Size = size;
			((Control)Panel25).TabIndex = 42;
			((Control)Panel10).BackColor = Color.Blue;
			Panel10.BorderStyle = (BorderStyle)2;
			Panel panel21 = Panel10;
			location = new Point(132, 6);
			((Control)panel21).Location = location;
			((Control)Panel10).Name = "Panel10";
			Panel panel22 = Panel10;
			size = new Size(17, 17);
			((Control)panel22).Size = size;
			((Control)Panel10).TabIndex = 31;
			((Control)Panel26).BackColor = Color.FromArgb(255, 255, 192);
			Panel26.BorderStyle = (BorderStyle)2;
			Panel panel23 = Panel26;
			location = new Point(69, 44);
			((Control)panel23).Location = location;
			((Control)Panel26).Name = "Panel26";
			Panel panel24 = Panel26;
			size = new Size(17, 17);
			((Control)panel24).Size = size;
			((Control)Panel26).TabIndex = 39;
			((Control)Panel11).BackColor = Color.Cyan;
			Panel11.BorderStyle = (BorderStyle)2;
			Panel panel25 = Panel11;
			location = new Point(111, 6);
			((Control)panel25).Location = location;
			((Control)Panel11).Name = "Panel11";
			Panel panel26 = Panel11;
			size = new Size(17, 17);
			((Control)panel26).Size = size;
			((Control)Panel11).TabIndex = 24;
			((Control)Panel27).BackColor = Color.FromArgb(255, 224, 192);
			Panel27.BorderStyle = (BorderStyle)2;
			Panel panel27 = Panel27;
			location = new Point(48, 44);
			((Control)panel27).Location = location;
			((Control)Panel27).Name = "Panel27";
			Panel panel28 = Panel27;
			size = new Size(17, 17);
			((Control)panel28).Size = size;
			((Control)Panel27).TabIndex = 40;
			((Control)Panel12).BackColor = Color.Fuchsia;
			Panel12.BorderStyle = (BorderStyle)2;
			Panel panel29 = Panel12;
			location = new Point(153, 6);
			((Control)panel29).Location = location;
			((Control)Panel12).Name = "Panel12";
			Panel panel30 = Panel12;
			size = new Size(17, 17);
			((Control)panel30).Size = size;
			((Control)Panel12).TabIndex = 23;
			((Control)Panel28).BackColor = Color.FromArgb(255, 192, 192);
			Panel28.BorderStyle = (BorderStyle)2;
			Panel panel31 = Panel28;
			location = new Point(27, 44);
			((Control)panel31).Location = location;
			((Control)Panel28).Name = "Panel28";
			Panel panel32 = Panel28;
			size = new Size(17, 17);
			((Control)panel32).Size = size;
			((Control)Panel28).TabIndex = 41;
			((Control)Panel13).BackColor = Color.Maroon;
			Panel13.BorderStyle = (BorderStyle)2;
			Panel panel33 = Panel13;
			location = new Point(27, 25);
			((Control)panel33).Location = location;
			((Control)Panel13).Name = "Panel13";
			Panel panel34 = Panel13;
			size = new Size(17, 17);
			((Control)panel34).Size = size;
			((Control)Panel13).TabIndex = 22;
			((Control)Panel18).BackColor = Color.Purple;
			Panel18.BorderStyle = (BorderStyle)2;
			Panel panel35 = Panel18;
			location = new Point(153, 25);
			((Control)panel35).Location = location;
			((Control)Panel18).Name = "Panel18";
			Panel panel36 = Panel18;
			size = new Size(17, 17);
			((Control)panel36).Size = size;
			((Control)Panel18).TabIndex = 34;
			((Control)Panel19).BackColor = Color.Yellow;
			Panel19.BorderStyle = (BorderStyle)2;
			Panel panel37 = Panel19;
			location = new Point(69, 6);
			((Control)panel37).Location = location;
			((Control)Panel19).Name = "Panel19";
			Panel panel38 = Panel19;
			size = new Size(17, 17);
			((Control)panel38).Size = size;
			((Control)Panel19).TabIndex = 27;
			((Control)Panel21).BackColor = Color.Teal;
			Panel21.BorderStyle = (BorderStyle)2;
			Panel panel39 = Panel21;
			location = new Point(111, 25);
			((Control)panel39).Location = location;
			((Control)Panel21).Name = "Panel21";
			Panel panel40 = Panel21;
			size = new Size(17, 17);
			((Control)panel40).Size = size;
			((Control)Panel21).TabIndex = 36;
			((Control)Panel14).BackColor = Color.FromArgb(128, 64, 0);
			Panel14.BorderStyle = (BorderStyle)2;
			Panel panel41 = Panel14;
			location = new Point(48, 25);
			((Control)panel41).Location = location;
			((Control)Panel14).Name = "Panel14";
			Panel panel42 = Panel14;
			size = new Size(17, 17);
			((Control)panel42).Size = size;
			((Control)Panel14).TabIndex = 26;
			((Control)Panel17).BackColor = Color.Navy;
			Panel17.BorderStyle = (BorderStyle)2;
			Panel panel43 = Panel17;
			location = new Point(132, 25);
			((Control)panel43).Location = location;
			((Control)Panel17).Name = "Panel17";
			Panel panel44 = Panel17;
			size = new Size(17, 17);
			((Control)panel44).Size = size;
			((Control)Panel17).TabIndex = 38;
			((Control)Panel15).BackColor = Color.FromArgb(192, 192, 0);
			Panel15.BorderStyle = (BorderStyle)2;
			Panel panel45 = Panel15;
			location = new Point(69, 25);
			((Control)panel45).Location = location;
			((Control)Panel15).Name = "Panel15";
			Panel panel46 = Panel15;
			size = new Size(17, 17);
			((Control)panel46).Size = size;
			((Control)Panel15).TabIndex = 25;
			((Control)Panel16).BackColor = Color.Green;
			Panel16.BorderStyle = (BorderStyle)2;
			Panel panel47 = Panel16;
			location = new Point(90, 25);
			((Control)panel47).Location = location;
			((Control)Panel16).Name = "Panel16";
			Panel panel48 = Panel16;
			size = new Size(17, 17);
			((Control)panel48).Size = size;
			((Control)Panel16).TabIndex = 37;
			((Control)TabPage3).Controls.Add((Control)(object)nudRed);
			((Control)TabPage3).Controls.Add((Control)(object)nudGreen);
			((Control)TabPage3).Controls.Add((Control)(object)nudBlue);
			((Control)TabPage3).Controls.Add((Control)(object)ColorBox);
			((Control)TabPage3).Controls.Add((Control)(object)Label2);
			((Control)TabPage3).Controls.Add((Control)(object)Label4);
			((Control)TabPage3).Controls.Add((Control)(object)Label3);
			TabPage tabPage4 = TabPage3;
			location = new Point(4, 22);
			tabPage4.Location = location;
			((Control)TabPage3).Name = "TabPage3";
			TabPage tabPage5 = TabPage3;
			((Padding)(ref val))..ctor(3);
			((Control)tabPage5).Padding = val;
			TabPage tabPage6 = TabPage3;
			size = new Size(174, 64);
			((Control)tabPage6).Size = size;
			TabPage3.TabIndex = 2;
			TabPage3.Text = "Color";
			TabPage3.UseVisualStyleBackColor = true;
			NumericUpDown obj3 = nudRed;
			location = new Point(21, 41);
			((Control)obj3).Location = location;
			NumericUpDown obj4 = nudRed;
			decimal maximum = new decimal(new int[4] { 255, 0, 0, 0 });
			obj4.Maximum = maximum;
			((Control)nudRed).Name = "nudRed";
			NumericUpDown obj5 = nudRed;
			size = new Size(40, 20);
			((Control)obj5).Size = size;
			((Control)nudRed).TabIndex = 8;
			maximum = (nudRed.Value = new decimal(new int[4] { 255, 0, 0, 0 }));
			NumericUpDown obj6 = nudGreen;
			location = new Point(67, 41);
			((Control)obj6).Location = location;
			maximum = (nudGreen.Maximum = new decimal(new int[4] { 255, 0, 0, 0 }));
			((Control)nudGreen).Name = "nudGreen";
			NumericUpDown obj7 = nudGreen;
			size = new Size(40, 20);
			((Control)obj7).Size = size;
			((Control)nudGreen).TabIndex = 8;
			maximum = (nudGreen.Value = new decimal(new int[4] { 255, 0, 0, 0 }));
			NumericUpDown obj8 = nudBlue;
			location = new Point(113, 41);
			((Control)obj8).Location = location;
			maximum = (nudBlue.Maximum = new decimal(new int[4] { 255, 0, 0, 0 }));
			((Control)nudBlue).Name = "nudBlue";
			NumericUpDown obj9 = nudBlue;
			size = new Size(40, 20);
			((Control)obj9).Size = size;
			((Control)nudBlue).TabIndex = 8;
			maximum = (nudBlue.Value = new decimal(new int[4] { 255, 0, 0, 0 }));
			((Control)ColorBox).Font = new Font("Tahoma", 10f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((ListControl)ColorBox).FormattingEnabled = true;
			ComboBox colorBox = ColorBox;
			location = new Point(5, 3);
			((Control)colorBox).Location = location;
			((Control)ColorBox).Name = "ColorBox";
			ComboBox colorBox2 = ColorBox;
			size = new Size(164, 24);
			((Control)colorBox2).Size = size;
			((Control)ColorBox).TabIndex = 4;
			((Control)Label2).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Control)Label2).ForeColor = Color.Red;
			Label label = Label2;
			location = new Point(18, 28);
			((Control)label).Location = location;
			((Control)Label2).Name = "Label2";
			Label label2 = Label2;
			size = new Size(42, 14);
			((Control)label2).Size = size;
			((Control)Label2).TabIndex = 10;
			Label2.Text = "Red";
			Label2.TextAlign = (ContentAlignment)32;
			((Control)Label4).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Control)Label4).ForeColor = Color.Blue;
			Label label3 = Label4;
			location = new Point(110, 28);
			((Control)label3).Location = location;
			((Control)Label4).Name = "Label4";
			Label label4 = Label4;
			size = new Size(42, 14);
			((Control)label4).Size = size;
			((Control)Label4).TabIndex = 10;
			Label4.Text = "Blue";
			Label4.TextAlign = (ContentAlignment)32;
			((Control)Label3).Font = new Font("Arial", 8f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Control)Label3).ForeColor = Color.Green;
			Label label5 = Label3;
			location = new Point(64, 28);
			((Control)label5).Location = location;
			((Control)Label3).Name = "Label3";
			Label label6 = Label3;
			size = new Size(42, 14);
			((Control)label6).Size = size;
			((Control)Label3).TabIndex = 10;
			Label3.Text = "Green";
			Label3.TextAlign = (ContentAlignment)32;
			TabPage2.BackColor = Color.White;
			((Control)TabPage2).Controls.Add((Control)(object)txbAlpha);
			((Control)TabPage2).Controls.Add((Control)(object)Label1);
			((Control)TabPage2).Controls.Add((Control)(object)tbarAlpha);
			TabPage tabPage7 = TabPage2;
			location = new Point(4, 22);
			tabPage7.Location = location;
			((Control)TabPage2).Name = "TabPage2";
			TabPage tabPage8 = TabPage2;
			((Padding)(ref val))..ctor(3);
			((Control)tabPage8).Padding = val;
			TabPage tabPage9 = TabPage2;
			size = new Size(174, 64);
			((Control)tabPage9).Size = size;
			TabPage2.TabIndex = 3;
			TabPage2.Text = "Transparency";
			TextBox obj10 = txbAlpha;
			location = new Point(115, 12);
			((Control)obj10).Location = location;
			((Control)txbAlpha).Name = "txbAlpha";
			TextBox obj11 = txbAlpha;
			size = new Size(27, 20);
			((Control)obj11).Size = size;
			((Control)txbAlpha).TabIndex = 13;
			txbAlpha.Text = "255";
			((Control)Label1).Font = new Font("Arial", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			Label label7 = Label1;
			location = new Point(29, 14);
			((Control)label7).Location = location;
			((Control)Label1).Name = "Label1";
			Label label8 = Label1;
			size = new Size(81, 17);
			((Control)label8).Size = size;
			((Control)Label1).TabIndex = 9;
			Label1.Text = "Alpha Value";
			Label1.TextAlign = (ContentAlignment)32;
			tbarAlpha.AutoSize = false;
			((Control)tbarAlpha).BackColor = Color.White;
			TrackBar obj12 = tbarAlpha;
			location = new Point(6, 35);
			((Control)obj12).Location = location;
			tbarAlpha.Maximum = 255;
			((Control)tbarAlpha).Name = "tbarAlpha";
			TrackBar obj13 = tbarAlpha;
			size = new Size(164, 21);
			((Control)obj13).Size = size;
			((Control)tbarAlpha).TabIndex = 12;
			tbarAlpha.TickStyle = (TickStyle)0;
			tbarAlpha.Value = 255;
			TextBox obj14 = txbCurrColor;
			location = new Point(26, 2);
			((Control)obj14).Location = location;
			((Control)txbCurrColor).Name = "txbCurrColor";
			TextBox obj15 = txbCurrColor;
			size = new Size(156, 20);
			((Control)obj15).Size = size;
			((Control)txbCurrColor).TabIndex = 7;
			panCurrColor.BorderStyle = (BorderStyle)2;
			Panel obj16 = panCurrColor;
			location = new Point(4, 2);
			((Control)obj16).Location = location;
			((Control)panCurrColor).Name = "panCurrColor";
			Panel obj17 = panCurrColor;
			size = new Size(21, 21);
			((Control)obj17).Size = size;
			((Control)panCurrColor).TabIndex = 7;
			Button obj18 = butClear;
			location = new Point(200, 150);
			((Control)obj18).Location = location;
			((Control)butClear).Name = "butClear";
			Button obj19 = butClear;
			size = new Size(42, 23);
			((Control)obj19).Size = size;
			((Control)butClear).TabIndex = 8;
			((ButtonBase)butClear).Text = "Clear";
			((ButtonBase)butClear).UseVisualStyleBackColor = true;
			Button obj20 = butApply;
			location = new Point(248, 150);
			((Control)obj20).Location = location;
			((Control)butApply).Name = "butApply";
			Button obj21 = butApply;
			size = new Size(42, 23);
			((Control)obj21).Size = size;
			((Control)butApply).TabIndex = 8;
			((ButtonBase)butApply).Text = "Apply";
			((ButtonBase)butApply).UseVisualStyleBackColor = true;
			((Control)panSampleHolder).Controls.Add((Control)(object)TheSample);
			Panel obj22 = panSampleHolder;
			location = new Point(195, 44);
			((Control)obj22).Location = location;
			((Control)panSampleHolder).Name = "panSampleHolder";
			Panel obj23 = panSampleHolder;
			size = new Size(100, 100);
			((Control)obj23).Size = size;
			((Control)panSampleHolder).TabIndex = 9;
			cBlendItems2.iColor = new Color[2]
			{
				Color.White,
				Color.White
			};
			cBlendItems2.iPoint = new float[2] { 0f, 1f };
			TheSample.ColorFillBlend = cBlendItems2;
			TheSample.Corners.All = 0;
			TheSample.Corners.LowerLeft = 0;
			TheSample.Corners.LowerRight = 0;
			TheSample.Corners.UpperLeft = 0;
			TheSample.Corners.UpperRight = 0;
			TheSample.FillType = CButton.eFillType.Solid;
			TheSample.FillTypeLinear = (LinearGradientMode)0;
			TheSample.ImageIndex = 0;
			CButton theSample = TheSample;
			location = new Point(0, 0);
			((Control)theSample).Location = location;
			((Control)TheSample).Name = "TheSample";
			CButton theSample2 = TheSample;
			size = new Size(100, 100);
			((Control)theSample2).Size = size;
			((Control)TheSample).TabIndex = 0;
			TheSample.Text = "CButton1";
			CButton theSample3 = TheSample;
			((Padding)(ref val))..ctor(0);
			theSample3.TextMargin = val;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleDimensions = autoScaleDimensions;
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Control)this).BackColor = Color.White;
			((Control)this).Controls.Add((Control)(object)panSampleHolder);
			((Control)this).Controls.Add((Control)(object)butApply);
			((Control)this).Controls.Add((Control)(object)butClear);
			((Control)this).Controls.Add((Control)(object)panProps);
			((Control)this).Name = "DropdownColorBlender";
			size = new Size(301, 176);
			((Control)this).Size = size;
			((Control)panProps).ResumeLayout(false);
			((Control)panProps).PerformLayout();
			((Control)TabControl1).ResumeLayout(false);
			((Control)TabPage1).ResumeLayout(false);
			((Control)TabPage3).ResumeLayout(false);
			((ISupportInitialize)nudRed).EndInit();
			((ISupportInitialize)nudGreen).EndInit();
			((ISupportInitialize)nudBlue).EndInit();
			((Control)TabPage2).ResumeLayout(false);
			((Control)TabPage2).PerformLayout();
			((ISupportInitialize)tbarAlpha).EndInit();
			((Control)panSampleHolder).ResumeLayout(false);
			((Control)this).ResumeLayout(false);
		}

		public DropdownColorBlender(IWindowsFormsEditorService editorService)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			((Control)this).MouseDown += new MouseEventHandler(ColorBlender_MouseDown);
			((UserControl)this).Load += ColorBlender_Load;
			((Control)this).MouseMove += new MouseEventHandler(ColorBlender_MouseMove);
			((Control)this).MouseUp += new MouseEventHandler(ColorBlender_MouseUp);
			__ENCAddToList(this);
			StartPointer = new cblPointer(0f, Color.White, IsCurr: false);
			EndPointer = new cblPointer(1f, Color.White, IsCurr: false);
			MiddlePointers = new Collection();
			MouseMoving = false;
			TopMargin = 8;
			_BlendColors = new Color[2]
			{
				Color.White,
				Color.Black
			};
			_BlendPositions = new float[2] { 0f, 1f };
			_BarHeight = 20;
			InitializeComponent();
			_editorService = editorService;
		}

		private void ColorBlender_Load(object sender, EventArgs e)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			ColorBox.DrawMode = (DrawMode)1;
			ColorBox.DropDownStyle = (ComboBoxStyle)2;
			ColorBox.DrawItem += new DrawItemEventHandler(ColorList_DrawItem);
			if (!((Component)(object)this).DesignMode)
			{
				ColorBox.Items.Clear();
				List<Color> list = new List<Color>();
				string[] names = Enum.GetNames(typeof(KnownColor));
				foreach (string name in names)
				{
					if (!Color.FromName(name).IsSystemColor)
					{
						list.Add(Color.FromName(name));
					}
				}
				list.Sort(SortColors);
				foreach (Color item in list)
				{
					ColorBox.Items.Add((object)item.Name);
				}
			}
			ColorBox.SelectedIndex = 1;
		}

		private int SortColors(Color x, Color y)
		{
			int num = x.GetHue().CompareTo(y.GetHue());
			int num2 = x.GetSaturation().CompareTo(y.GetSaturation());
			int num3 = x.GetBrightness().CompareTo(y.GetBrightness());
			if (num != 0)
			{
				return num;
			}
			if (num2 != 0)
			{
				return num2;
			}
			if (num3 != 0)
			{
				return num3;
			}
			return 0;
		}

		private void panProps_SizeChanged(object sender, EventArgs e)
		{
			checked
			{
				((Control)this).Width = ((Control)panSampleHolder).Left + ((Control)panSampleHolder).Width;
				((Control)this).Height = ((Control)butApply).Top + ((Control)butApply).Height;
			}
		}

		private void panSampleHolder_SizeChanged(object sender, EventArgs e)
		{
			checked
			{
				((Control)this).Width = ((Control)panSampleHolder).Left + ((Control)panSampleHolder).Width;
				((Control)this).Height = ((Control)butApply).Top + ((Control)butApply).Height;
			}
		}

		private void ColorBlender_MouseDown(object sender, MouseEventArgs e)
		{
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Invalid comparison between Unknown and I4
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Invalid comparison between Unknown and I4
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Invalid comparison between Unknown and I4
			if (!checked((e.Y > TopMargin + BarHeight - 10) & (e.Y < TopMargin + BarHeight + 20) & (e.X > 5) & (e.X < ((Control)this).Width - 5)))
			{
				return;
			}
			int num = IsMouseOverPointer(e.X, e.Y);
			if (num > -1)
			{
				if (CurrPointer != num)
				{
					CurrPointer = num;
					ClearCurrPointer();
					((cblPointer)MiddlePointers[CurrPointer]).pIsCurr = true;
					UpdateRGBnuds(((cblPointer)MiddlePointers[CurrPointer]).pColor);
				}
				if ((int)e.Button == 1048576)
				{
					MouseMoving = true;
				}
				else if ((int)e.Button == 2097152)
				{
					MiddlePointers.Remove(CurrPointer);
				}
			}
			else if (IsMouseOverStartPointer(e.X, e.Y))
			{
				ClearCurrPointer();
				CurrPointer = -1;
				StartPointer.pIsCurr = true;
				UpdateRGBnuds(StartPointer.pColor);
			}
			else if (IsMouseOverEndPointer(e.X, e.Y))
			{
				ClearCurrPointer();
				CurrPointer = -1;
				EndPointer.pIsCurr = true;
				UpdateRGBnuds(EndPointer.pColor);
			}
			else if ((int)e.Button == 1048576)
			{
				ClearCurrPointer();
				MiddlePointers.Add((object)new cblPointer((float)checked((double)(e.X - 10) / (double)(((Control)this).Width - 20)), Color.FromArgb(tbarAlpha.Value, Convert.ToInt32(nudRed.Value), Convert.ToInt32(nudGreen.Value), Convert.ToInt32(nudBlue.Value)), IsCurr: true), (string)null, (object)null, (object)null);
				SortCollection(MiddlePointers, "pPos", pbAscending: true);
				CurrPointer = FindCurr();
				((Control)this).Invalidate();
				MouseMoving = true;
			}
		}

		private void ColorBlender_MouseMove(object sender, MouseEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			if ((int)e.Button == 1048576 && MouseMoving && ((e.X >= 11) & (e.X <= checked(((Control)this).Width - 11))))
			{
				((cblPointer)MiddlePointers[CurrPointer]).pPos = (float)checked((double)(e.X - 10) / (double)(((Control)this).Width - 20));
				SortCollection(MiddlePointers, "pPos", pbAscending: true);
				CurrPointer = FindCurr();
				((Control)this).Invalidate();
			}
		}

		private void ColorBlender_MouseUp(object sender, MouseEventArgs e)
		{
			MouseMoving = false;
			SortCollection(MiddlePointers, "pPos", pbAscending: true);
			CurrPointer = FindCurr();
			((Control)this).Invalidate();
		}

		private bool IsMouseOverStartPointer(int X, int Y)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			Region val = new Region(BuildPointer(GetpX(0f)));
			try
			{
				return val.IsVisible((float)X, (float)Y);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private bool IsMouseOverEndPointer(int X, int Y)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			Region val = new Region(BuildPointer(GetpX(1f)));
			try
			{
				return val.IsVisible((float)X, (float)Y);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private int IsMouseOverPointer(int X, int Y)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			if (MiddlePointers != null)
			{
				int count = MiddlePointers.Count;
				int num = 1;
				while (true)
				{
					int num2 = num;
					int num3 = count;
					if (num2 > num3)
					{
						break;
					}
					Region val = new Region(BuildPointer(GetpX(((cblPointer)MiddlePointers[num]).pPos)));
					try
					{
						if (val.IsVisible((float)X, (float)Y))
						{
							return num;
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					num = checked(num + 1);
				}
				return -1;
			}
			int result = default(int);
			return result;
		}

		private void ClearCurrPointer()
		{
			foreach (cblPointer middlePointer in MiddlePointers)
			{
				middlePointer.pIsCurr = false;
			}
			StartPointer.pIsCurr = false;
			EndPointer.pIsCurr = false;
		}

		private int FindCurr()
		{
			int count = MiddlePointers.Count;
			int num = 1;
			while (true)
			{
				int num2 = num;
				int num3 = count;
				if (num2 > num3)
				{
					break;
				}
				if (((cblPointer)MiddlePointers[num]).pIsCurr)
				{
					return num;
				}
				num = checked(num + 1);
			}
			return -1;
		}

		private void DrawPointer(ref Graphics g, Color bColor, float pt, bool IsCurr)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Expected O, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			Brush val = (Brush)new SolidBrush(bColor);
			try
			{
				Pen val2 = new Pen(Color.LightGray, 2f);
				try
				{
					float num = GetpX(pt);
					g.FillPath(val, BuildPointer(num));
					g.DrawPath(val2, BuildPointer(num));
					val2.Width = 1f;
					val2.Color = Color.Black;
					g.DrawPath(val2, BuildPointer(num));
					if (IsCurr)
					{
						g.FillEllipse(Brushes.Red, num + 10f, (float)checked(TopMargin + BarHeight + 8), 10f, 4f);
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private float GetpX(float pos)
		{
			return (float)checked(((Control)this).Width - 20) * pos - 5f;
		}

		private GraphicsPath BuildPointer(float CenterPtX)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			CenterPtX += 10f;
			GraphicsPath val = new GraphicsPath();
			checked
			{
				val.AddLine(CenterPtX + 5f, (float)(TopMargin + BarHeight - 3), CenterPtX + 10f, (float)(TopMargin + BarHeight));
				val.AddLine(CenterPtX + 10f, (float)(TopMargin + BarHeight), CenterPtX + 10f, (float)(TopMargin + BarHeight + 7));
				val.AddLine(CenterPtX + 10f, (float)(TopMargin + BarHeight + 7), CenterPtX, (float)(TopMargin + BarHeight + 7));
				val.AddLine(CenterPtX, (float)(TopMargin + BarHeight + 7), CenterPtX, (float)(TopMargin + BarHeight));
				val.CloseFigure();
				return val;
			}
		}

		public LinearGradientBrush LinearBrush(Rectangle BaseRect, LinearGradientMode Mode)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			Rectangle rectangle = checked(new Rectangle(BaseRect.X - 1, BaseRect.Y - 1, BaseRect.Width + 2, BaseRect.Height + 2));
			LinearGradientBrush val = new LinearGradientBrush(rectangle, Color.AliceBlue, Color.Blue, Mode);
			ColorBlend val2 = new ColorBlend();
			val2.Colors = BlendColors;
			val2.Positions = BlendPositions;
			val.InterpolationColors = val2;
			return val;
		}

		private void BuildABlend()
		{
			List<Color> list = new List<Color>();
			list.Add(StartPointer.pColor);
			if (MiddlePointers != null)
			{
				foreach (cblPointer middlePointer in MiddlePointers)
				{
					list.Add(middlePointer.pColor);
				}
			}
			list.Add(EndPointer.pColor);
			BlendColors = list.ToArray();
			list = null;
			List<float> list2 = new List<float>();
			list2.Add(StartPointer.pPos);
			if (MiddlePointers != null)
			{
				foreach (cblPointer middlePointer2 in MiddlePointers)
				{
					list2.Add(middlePointer2.pPos);
				}
			}
			list2.Add(EndPointer.pPos);
			BlendPositions = list2.ToArray();
			list2 = null;
			TheSample.ColorFillBlend = new cBlendItems(BlendColors, BlendPositions);
		}

		public void LoadABlend(cBlendItems cb)
		{
			StartPointer.pColor = cb.iColor[0];
			StartPointer.pPos = cb.iPoint[0];
			checked
			{
				EndPointer.pColor = cb.iColor[cb.iColor.Length - 1];
				EndPointer.pPos = cb.iPoint[cb.iColor.Length - 1];
				if (cb.iColor.Length <= 2)
				{
					return;
				}
				int num = cb.iColor.Length - 2;
				int num2 = 1;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 <= num4)
					{
						MiddlePointers.Add((object)new cblPointer(cb.iPoint[num2], cb.iColor[num2], IsCurr: false), (string)null, (object)null, (object)null);
						num2++;
						continue;
					}
					break;
				}
			}
		}

		private void UpdatePointerColor()
		{
			Color color = Color.FromArgb(tbarAlpha.Value, Convert.ToInt32(nudRed.Value), Convert.ToInt32(nudGreen.Value), Convert.ToInt32(nudBlue.Value));
			if (StartPointer.pIsCurr)
			{
				StartPointer.pColor = color;
			}
			else if (EndPointer.pIsCurr)
			{
				EndPointer.pColor = color;
			}
			else
			{
				int num = FindCurr();
				if (num > -1)
				{
					((cblPointer)MiddlePointers[num]).pColor = color;
				}
			}
			((Control)panCurrColor).BackColor = color;
			txbCurrColor.Text = GetColorName(color);
			((Control)this).Invalidate();
		}

		public string GetColorName(Color c)
		{
			string[] names = Enum.GetNames(typeof(KnownColor));
			foreach (string text in names)
			{
				if (!Color.FromName(text).IsSystemColor && ColorTranslator.ToWin32(Color.FromName(text)) == ColorTranslator.ToWin32(c))
				{
					return Conversions.ToString(Interaction.IIf(Operators.CompareString(c.Name, "ffffffff", false) == 0, (object)"White- ffffffff", (object)$"{text}- {c.Name}"));
				}
			}
			return Conversions.ToString(Interaction.IIf(Operators.CompareString(c.Name, "ff7f007f", false) == 0, (object)"Transparent- ff7f007f", (object)c.Name));
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Expected O, but got Unknown
			BuildABlend();
			Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height);
			Graphics g = Graphics.FromImage((Image)(object)val);
			g.Clear(((Control)this).BackColor);
			g.SmoothingMode = (SmoothingMode)4;
			checked
			{
				Rectangle rectangle = new Rectangle(10, TopMargin, ((Control)this).ClientSize.Width - 20, BarHeight);
				Brush val2 = (Brush)(object)LinearBrush(rectangle, (LinearGradientMode)0);
				g.FillRectangle(val2, rectangle);
				Pen val3 = new Pen(Color.Gray, 1f);
				try
				{
					val3.DashStyle = (DashStyle)1;
					g.DrawLine(val3, 10, TopMargin + BarHeight + 7, ((Control)this).ClientSize.Width - 15, TopMargin + BarHeight + 7);
					val3.Color = Color.Black;
					val3.DashStyle = (DashStyle)0;
					DrawPointer(ref g, StartPointer.pColor, 0f, StartPointer.pIsCurr);
					DrawPointer(ref g, EndPointer.pColor, 1f, EndPointer.pIsCurr);
					if (MiddlePointers != null)
					{
						int count = MiddlePointers.Count;
						int num = 1;
						while (true)
						{
							int num2 = num;
							int num3 = count;
							if (num2 <= num3)
							{
								DrawPointer(ref g, ((cblPointer)MiddlePointers[num]).pColor, ((cblPointer)MiddlePointers[num]).pPos, num == CurrPointer);
								num++;
								continue;
							}
							break;
						}
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				e.Graphics.DrawImage((Image)(Bitmap)((Image)val).Clone(), 0, 0);
				((Image)val).Dispose();
				val2.Dispose();
				g.Dispose();
				if (TheSample.FillType == CButton.eFillType.GradientPath)
				{
					((Control)TheSample).Refresh();
				}
			}
		}

		private static void SortCollection(Collection col, string psSortPropertyName, bool pbAscending, string psKeyPropertyName = "")
		{
			bool flag = Operators.CompareString(psKeyPropertyName, "", false) != 0;
			checked
			{
				int num = col.Count - 1;
				int num2 = 1;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 > num4)
					{
						break;
					}
					object objectValue = RuntimeHelpers.GetObjectValue(col[num2]);
					float num5 = Conversions.ToSingle(Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), psSortPropertyName, (CallType)2, new object[0]));
					int num6 = num2;
					int num7 = num2 + 1;
					int count = col.Count;
					int num8 = num7;
					while (true)
					{
						int num9 = num8;
						num4 = count;
						if (num9 > num4)
						{
							break;
						}
						objectValue = RuntimeHelpers.GetObjectValue(col[num8]);
						float num10 = Conversions.ToSingle(Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), psSortPropertyName, (CallType)2, new object[0]));
						if ((!pbAscending) ? (num10 > num5) : (num10 < num5))
						{
							num5 = num10;
							num6 = num8;
						}
						objectValue = null;
						num8++;
					}
					if (num6 != num2)
					{
						objectValue = RuntimeHelpers.GetObjectValue(col[num6]);
						col.Remove(num6);
						if (flag)
						{
							string text = Conversions.ToString(Versioned.CallByName(RuntimeHelpers.GetObjectValue(objectValue), psKeyPropertyName, (CallType)2, new object[0]));
							col.Add(RuntimeHelpers.GetObjectValue(objectValue), text, (object)num2, (object)null);
						}
						else
						{
							col.Add(RuntimeHelpers.GetObjectValue(objectValue), (string)null, (object)num2, (object)null);
						}
						objectValue = null;
					}
					objectValue = null;
					num2++;
				}
			}
		}

		private void ColorBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateRGBnuds(Color.FromName(ColorBox.Text));
			((Control)this).Invalidate();
		}

		private void Panel7_Click(object sender, EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			UpdateRGBnuds(((Control)(Panel)sender).BackColor);
			((Control)this).Invalidate();
		}

		private void Panel10_MouseEnter(object sender, EventArgs e)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			try
			{
				CurrSwatch.BorderStyle = (BorderStyle)2;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			CurrSwatch = (Panel)sender;
			CurrSwatch.BorderStyle = (BorderStyle)1;
		}

		private void TabControl1_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				CurrSwatch.BorderStyle = (BorderStyle)2;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		private void UpdateRGBnuds(Color c)
		{
			tbarAlpha.Value = c.A;
			nudRed.Value = new decimal(c.R);
			nudGreen.Value = new decimal(c.G);
			nudBlue.Value = new decimal(c.B);
			UpdatePointerColor();
		}

		private void nud_ValueChanged(object sender, EventArgs e)
		{
			txbAlpha.Text = tbarAlpha.Value.ToString();
			UpdatePointerColor();
		}

		private void txbAlpha_TextChanged(object sender, EventArgs e)
		{
			if (Operators.CompareString(txbAlpha.Text, "", false) != 0)
			{
				int num = Conversions.ToInteger(txbAlpha.Text);
				if (num < 0)
				{
					txbAlpha.Text = "0";
				}
				else if (num > 255)
				{
					txbAlpha.Text = "255";
				}
				tbarAlpha.Value = Conversions.ToInteger(txbAlpha.Text);
			}
		}

		private void butApply_Click(object sender, EventArgs e)
		{
			_editorService.CloseDropDown();
		}

		private void butClear_Click(object sender, EventArgs e)
		{
			StartPointer.pColor = Color.White;
			EndPointer.pColor = Color.White;
			MiddlePointers.Clear();
			((Control)this).Invalidate();
		}

		private void ColorList_DrawItem(object sender, DrawItemEventArgs e)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Invalid comparison between Unknown and I4
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			if ((e.State & 0x1000) == 4096)
			{
				e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			}
			else if ((e.State & 1) == 1)
			{
				e.Graphics.FillRectangle(Brushes.CornflowerBlue, e.Bounds);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.Beige, e.Bounds);
			}
			ComboBox val = (ComboBox)sender;
			string text = Conversions.ToString(val.Items[e.Index]);
			Font val2 = new Font("Tahoma", 10f);
			checked
			{
				try
				{
					SolidBrush val3 = new SolidBrush(Color.FromName(text));
					e.Graphics.FillRectangle((Brush)(object)val3, e.Bounds.X + 3, e.Bounds.Y + 3, 20, e.Bounds.Height - 5);
					e.Graphics.DrawRectangle(Pens.Black, e.Bounds.X + 3, e.Bounds.Y + 3, 20, e.Bounds.Height - 5);
					e.Graphics.DrawString(text, val2, Brushes.Black, (float)(e.Bounds.X + 25), (float)(e.Bounds.Y + 1));
					e.DrawFocusRectangle();
					((Brush)val3).Dispose();
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
	}
	internal class cblPointer
	{
		private float _pPos;

		private Color _pColor;

		private bool _pIsCurr;

		public float pPos
		{
			get
			{
				return _pPos;
			}
			set
			{
				_pPos = value;
			}
		}

		public Color pColor
		{
			get
			{
				return _pColor;
			}
			set
			{
				_pColor = value;
			}
		}

		public bool pIsCurr
		{
			get
			{
				return _pIsCurr;
			}
			set
			{
				_pIsCurr = value;
			}
		}

		public cblPointer(float pt, Color c, bool IsCurr)
		{
			pPos = pt;
			pColor = c;
			pIsCurr = IsCurr;
		}
	}
	public class FocalPtGlyph : Glyph
	{
		public enum eAdjWhat
		{
			FocalScale,
			Center,
			None
		}

		public class FocalPtBehavior : Behavior
		{
			private IDesigner relatedDesigner;

			private CButton relatedControl;

			private eAdjWhat adjWhat;

			private Point _dragStart;

			public bool _dragging;

			internal FocalPtBehavior(IDesigner relatedDesigner, eAdjWhat adjWhat)
			{
				this.relatedDesigner = null;
				relatedControl = null;
				this.relatedDesigner = relatedDesigner;
				relatedControl = (CButton)relatedDesigner.Component;
				this.adjWhat = adjWhat;
			}

			public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Invalid comparison between Unknown and I4
				if ((int)button == 1048576)
				{
					if ((int)Control.ModifierKeys == 65536)
					{
						if (adjWhat == eAdjWhat.FocalScale)
						{
							cFocalPoints focalPoints = relatedControl.FocalPoints;
							PointF focusScales = new PointF(0f, 0f);
							focalPoints.SetFocusScales(focusScales);
						}
						else if (adjWhat == eAdjWhat.Center)
						{
							cFocalPoints focalPoints2 = relatedControl.FocalPoints;
							PointF focusScales = new PointF(0.5f, 0.5f);
							focalPoints2.SetCenterPoint(focusScales);
						}
						((Control)relatedControl).Refresh();
					}
					else
					{
						_dragStart = mouseLoc;
						_dragging = true;
					}
				}
				return true;
			}

			public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				if ((int)button != 1048576)
				{
					_dragging = false;
				}
				checked
				{
					if (_dragging)
					{
						FocalPtGlyph focalPtGlyph = (FocalPtGlyph)(object)g;
						Point point = focalPtGlyph.behaviorSvc.ControlToAdornerWindow((Control)(object)focalPtGlyph.relatedControl);
						int num = mouseLoc.X - _dragStart.X;
						int num2 = mouseLoc.Y - _dragStart.Y;
						if ((num == 0 && num2 == 0) ? true : false)
						{
							return true;
						}
						_dragStart = mouseLoc;
						SizeF sizeF = new SizeF((float)Math.Max(Math.Min((float)num / relatedControl.ButtonArea.Width, 1.0), 0.0), (float)Math.Max(Math.Min((float)num2 / relatedControl.ButtonArea.Height, 1.0), 0.0));
						if (focalPtGlyph.AdjWhat == eAdjWhat.FocalScale)
						{
							cFocalPoints focalPoints = relatedControl.FocalPoints;
							PointF pt = relatedControl.FocalPoints.FocusScales();
							SizeF sz = new SizeF((float)num / relatedControl.ButtonArea.Width, (float)num2 / relatedControl.ButtonArea.Height);
							focalPoints.SetFocusScales(PointF.Add(pt, sz));
						}
						else if (focalPtGlyph.AdjWhat == eAdjWhat.Center)
						{
							cFocalPoints focalPoints2 = relatedControl.FocalPoints;
							PointF pt2 = relatedControl.FocalPoints.CenterPoint();
							SizeF sz = new SizeF((float)num / relatedControl.ButtonArea.Width, (float)num2 / relatedControl.ButtonArea.Height);
							focalPoints2.SetCenterPoint(PointF.Add(pt2, sz));
						}
						focalPtGlyph.FocalPtAdorner.Invalidate();
					}
					return true;
				}
			}

			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				_dragging = false;
				return true;
			}
		}

		private CButton relatedControl;

		private BehaviorService behaviorSvc;

		private ISelectionService selectionSvc;

		private IDesigner relatedDesigner;

		private Adorner FocalPtAdorner;

		private Rectangle FocalBox;

		private Brush boxFillColor;

		public eAdjWhat AdjWhat
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		public override Rectangle Bounds
		{
			get
			{
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_011b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0120: Unknown result type (might be due to invalid IL or missing references)
				//IL_0169: Unknown result type (might be due to invalid IL or missing references)
				//IL_016e: Unknown result type (might be due to invalid IL or missing references)
				Point point = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
				checked
				{
					if (relatedControl.FillType == CButton.eFillType.GradientPath)
					{
						Padding padding;
						Padding padding2;
						RectangleF value;
						if (AdjWhat == eAdjWhat.Center)
						{
							int x = point.X;
							padding = ((Control)relatedControl).Padding;
							float x2 = (float)(x + ((Padding)(ref padding)).Left) + relatedControl.FocalPoints.CenterPoint().X * relatedControl.ButtonArea.Width - 5f;
							int y = point.Y;
							padding2 = ((Control)relatedControl).Padding;
							value = new RectangleF(x2, (float)(y + ((Padding)(ref padding2)).Top) + relatedControl.FocalPoints.CenterPoint().Y * relatedControl.ButtonArea.Height - 5f, 10f, 10f);
							return Rectangle.Round(value);
						}
						if (AdjWhat == eAdjWhat.FocalScale)
						{
							int x3 = point.X;
							padding2 = ((Control)relatedControl).Padding;
							float x4 = (float)(x3 + ((Padding)(ref padding2)).Left) + relatedControl.FocalPoints.FocusScales().X * relatedControl.ButtonArea.Width - 5f;
							int y2 = point.Y;
							padding = ((Control)relatedControl).Padding;
							value = new RectangleF(x4, (float)(y2 + ((Padding)(ref padding)).Top) + relatedControl.FocalPoints.FocusScales().Y * relatedControl.ButtonArea.Height - 5f, 10f, 10f);
							return Rectangle.Round(value);
						}
					}
					return new Rectangle(point.X, point.Y, ((Control)relatedControl).Width, ((Control)relatedControl).Height);
				}
			}
		}

		public FocalPtGlyph(BehaviorService behaviorService, CButton control, ISelectionService selectionService, IDesigner relatedDesigner, Adorner myAdorner, eAdjWhat adjWhat)
			: base((Behavior)(object)new FocalPtBehavior(relatedDesigner, adjWhat))
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			selectionSvc = null;
			this.relatedDesigner = null;
			boxFillColor = (Brush)new SolidBrush(Color.FromArgb(120, Color.White));
			behaviorSvc = behaviorService;
			selectionSvc = selectionService;
			this.relatedDesigner = relatedDesigner;
			FocalPtAdorner = myAdorner;
			AdjWhat = adjWhat;
			relatedControl = (CButton)this.relatedDesigner.Component;
		}

		public override Cursor GetHitTest(Point p)
		{
			if ((object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) && ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat == ChooseGlyph.eChooseWhat.FocalPt) ? true : false)
			{
				if (((FocalPtBehavior)(object)((Glyph)this).Behavior)._dragging)
				{
					return Cursors.Cross;
				}
				if (Bounds.Contains(p))
				{
					return Cursors.Cross;
				}
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			if (!object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) || ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat != ChooseGlyph.eChooseWhat.FocalPt || 1 == 0)
			{
				return;
			}
			Point point = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
			if (relatedControl.FillType != CButton.eFillType.GradientPath)
			{
				return;
			}
			Pen val = new Pen(Color.Gray, 1f);
			try
			{
				val.DashStyle = (DashStyle)2;
				if (AdjWhat == eAdjWhat.Center)
				{
					pe.Graphics.FillEllipse(boxFillColor, Bounds);
					pe.Graphics.DrawEllipse(val, Bounds);
				}
				else if (AdjWhat == eAdjWhat.FocalScale)
				{
					pe.Graphics.FillRectangle(boxFillColor, Bounds);
					pe.Graphics.DrawRectangle(val, Rectangle.Round(Bounds));
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
namespace CButtonLib.My.Resources
{
	[StandardModule]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[HideModuleName]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal sealed class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("CButtonLib.Resources", typeof(Resources).Assembly);
					resourceMan = resourceManager;
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}
	}
}
namespace CButtonLib.My
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal sealed class MySettings : ApplicationSettingsBase
	{
		private static MySettings defaultInstance = (MySettings)(object)SettingsBase.Synchronized((SettingsBase)(object)new MySettings());

		public static MySettings Default => defaultInstance;

		[DebuggerNonUserCode]
		public MySettings()
		{
		}
	}
	[CompilerGenerated]
	[HideModuleName]
	[StandardModule]
	[DebuggerNonUserCode]
	internal sealed class MySettingsProperty
	{
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings => MySettings.Default;
	}
}
namespace CButtonLib
{
	public class PadGlyph : Glyph
	{
		public enum eWhatBorder
		{
			Top,
			Left,
			Right,
			Bottom
		}

		public enum eAdjWhat
		{
			Button,
			Text
		}

		public class PadBehavior : Behavior
		{
			private IDesigner relatedDesigner;

			private CButton relatedControl;

			private eAdjWhat AdjWhat;

			private Point _dragStart;

			public bool _dragging;

			internal PadBehavior(IDesigner relatedDesigner, eAdjWhat adjWhat)
			{
				this.relatedDesigner = null;
				relatedControl = null;
				this.relatedDesigner = relatedDesigner;
				relatedControl = (CButton)relatedDesigner.Component;
				AdjWhat = adjWhat;
			}

			public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
			{
				_dragStart = mouseLoc;
				_dragging = true;
				return true;
			}

			public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLoc)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Invalid comparison between Unknown and I4
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
				//IL_019c: Unknown result type (might be due to invalid IL or missing references)
				if ((int)button != 1048576)
				{
					_dragging = false;
				}
				checked
				{
					if (_dragging)
					{
						PadGlyph padGlyph = (PadGlyph)(object)g;
						int num = mouseLoc.X - _dragStart.X;
						int num2 = mouseLoc.Y - _dragStart.Y;
						if ((num == 0 && num2 == 0) ? true : false)
						{
							return true;
						}
						_dragStart = mouseLoc;
						Padding val = ((AdjWhat != 0) ? relatedControl.TextMargin : ((Control)relatedControl).Padding);
						switch (unchecked((int)padGlyph.WhatBorder))
						{
						case 0:
						{
							int bottom = ((Padding)(ref val)).Top;
							bottom += num2;
							((Padding)(ref val))..ctor(((Padding)(ref val)).Left, bottom, ((Padding)(ref val)).Right, ((Padding)(ref val)).Bottom);
							break;
						}
						case 1:
						{
							int bottom = ((Padding)(ref val)).Left;
							bottom += num;
							((Padding)(ref val))..ctor(bottom, ((Padding)(ref val)).Top, ((Padding)(ref val)).Right, ((Padding)(ref val)).Bottom);
							break;
						}
						case 2:
						{
							int bottom = ((Padding)(ref val)).Right;
							bottom -= num;
							((Padding)(ref val))..ctor(((Padding)(ref val)).Left, ((Padding)(ref val)).Top, bottom, ((Padding)(ref val)).Bottom);
							break;
						}
						case 3:
						{
							int bottom = ((Padding)(ref val)).Bottom;
							bottom -= num2;
							((Padding)(ref val))..ctor(((Padding)(ref val)).Left, ((Padding)(ref val)).Top, ((Padding)(ref val)).Right, bottom);
							break;
						}
						}
						if (AdjWhat == eAdjWhat.Button)
						{
							((Control)relatedControl).Padding = val;
						}
						else
						{
							relatedControl.TextMargin = val;
						}
						padGlyph.PadAdorner.Invalidate();
					}
					return true;
				}
			}

			public override bool OnMouseUp(Glyph g, MouseButtons button)
			{
				_dragging = false;
				return true;
			}
		}

		private CButton relatedControl;

		private BehaviorService behaviorSvc;

		private ISelectionService selectionSvc;

		private IDesigner relatedDesigner;

		private Adorner PadAdorner;

		private eWhatBorder WhatBorder;

		private bool isOver;

		public eAdjWhat AdjWhat
		{
			[DebuggerNonUserCode]
			get;
			[DebuggerNonUserCode]
			set;
		}

		public override Rectangle Bounds
		{
			get
			{
				Point pos = behaviorSvc.ControlToAdornerWindow((Control)(object)relatedControl);
				Rectangle result = ((AdjWhat != 0) ? Rectangle.Round(relatedControl.TextArea) : Rectangle.Round(relatedControl.ButtonArea));
				result.Offset(pos);
				return result;
			}
		}

		public PadGlyph(BehaviorService behaviorService, CButton control, ISelectionService selectionService, IDesigner relatedDesigner, Adorner myAdorner, eAdjWhat adjWhat)
			: base((Behavior)(object)new PadBehavior(relatedDesigner, adjWhat))
		{
			selectionSvc = null;
			this.relatedDesigner = null;
			isOver = false;
			behaviorSvc = behaviorService;
			selectionSvc = selectionService;
			this.relatedDesigner = relatedDesigner;
			PadAdorner = myAdorner;
			AdjWhat = adjWhat;
			relatedControl = (CButton)this.relatedDesigner.Component;
		}

		public override Cursor GetHitTest(Point p)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Invalid comparison between Unknown and I4
			if ((object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) && ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat == ChooseGlyph.eChooseWhat.Pad) ? true : false)
			{
				GraphicsPath val = new GraphicsPath();
				try
				{
					Pen val2 = new Pen(Brushes.Black, 7f);
					try
					{
						if (((PadBehavior)(object)((Glyph)this).Behavior)._dragging)
						{
							val2.Width = 20f;
						}
						else
						{
							val2.Width = 3f;
						}
						if (((int)Control.ModifierKeys == 65536) & (AdjWhat == eAdjWhat.Button))
						{
							return null;
						}
						val.AddLine(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y);
						if (val.IsOutlineVisible(p, val2))
						{
							WhatBorder = eWhatBorder.Top;
							if (!isOver)
							{
								isOver = true;
								PadAdorner.Invalidate();
							}
							return Cursors.HSplit;
						}
						val.Reset();
						val.AddLine(Bounds.X, Bounds.Y, Bounds.X, Bounds.Bottom);
						if (val.IsOutlineVisible(p, val2))
						{
							WhatBorder = eWhatBorder.Left;
							if (!isOver)
							{
								isOver = true;
								PadAdorner.Invalidate();
							}
							return Cursors.VSplit;
						}
						val.Reset();
						val.AddLine(Bounds.Right, Bounds.Y, Bounds.Right, Bounds.Bottom);
						if (val.IsOutlineVisible(p, val2))
						{
							WhatBorder = eWhatBorder.Right;
							if (!isOver)
							{
								isOver = true;
								PadAdorner.Invalidate();
							}
							return Cursors.VSplit;
						}
						val.Reset();
						val.AddLine(Bounds.X, Bounds.Bottom, Bounds.Right, Bounds.Bottom);
						if (val.IsOutlineVisible(p, val2))
						{
							WhatBorder = eWhatBorder.Bottom;
							if (!isOver)
							{
								isOver = true;
								PadAdorner.Invalidate();
							}
							return Cursors.HSplit;
						}
						if (isOver)
						{
							isOver = false;
							PadAdorner.Invalidate();
						}
						return null;
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			if (!object.ReferenceEquals(RuntimeHelpers.GetObjectValue(selectionSvc.PrimarySelection), relatedControl) || ((ChooseGlyph)(object)((CButtonDesigner)relatedDesigner).ChooseAdorner.Glyphs[0]).chooseWhat != 0 || 1 == 0)
			{
				return;
			}
			Pen val = new Pen(Brushes.DarkGray, 1.5f);
			val.DashStyle = (DashStyle)1;
			Pen val2 = val;
			try
			{
				if (isOver)
				{
					val2.Color = Color.Black;
				}
				Graphics graphics = pe.Graphics;
				Rectangle rectangle = checked(new Rectangle(Bounds.X, Bounds.Y, Bounds.Width - 1, Bounds.Height - 1));
				graphics.DrawRectangle(val2, rectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}
}

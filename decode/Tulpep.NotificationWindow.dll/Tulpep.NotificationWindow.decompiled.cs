using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Tulpep.NotificationWindow.Properties;

[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyProduct("Tulpep.NotificationWindow")]
[assembly: CompilationRelaxations(8)]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyTitle("Tulpep.NotificationWindow")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("718bace5-a97f-4a78-ad50-5730eba79995")]
[assembly: AssemblyFileVersion("1.1.38")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyVersion("1.1.38.0")]
namespace Tulpep.NotificationWindow
{
	[ToolboxBitmap(typeof(PopupNotifier), "Icon.ico")]
	[DefaultEvent("Click")]
	public class PopupNotifier : Component
	{
		private const int SW_SHOWNOACTIVATE = 4;

		private const int HWND_TOPMOST = -1;

		private const uint SWP_NOACTIVATE = 16u;

		private bool disposed;

		private PopupNotifierForm frmPopup;

		private Timer tmrAnimation;

		private Timer tmrWait;

		private bool isAppearing = true;

		private bool markedForDisposed;

		private bool mouseIsOn;

		private int maxPosition;

		private double maxOpacity;

		private int posStart;

		private int posStop;

		private double opacityStart;

		private double opacityStop;

		private int realAnimationDuration;

		private Stopwatch sw;

		private Size imageSize = new Size(0, 0);

		[Description("Color of the window header.")]
		[Category("Header")]
		[DefaultValue(typeof(Color), "ControlDark")]
		public Color HeaderColor { get; set; }

		[Description("Color of the window background.")]
		[DefaultValue(typeof(Color), "Control")]
		[Category("Appearance")]
		public Color BodyColor { get; set; }

		[Category("Title")]
		[Description("Color of the title text.")]
		[DefaultValue(typeof(Color), "Gray")]
		public Color TitleColor { get; set; }

		[Category("Content")]
		[Description("Color of the content text.")]
		[DefaultValue(typeof(Color), "ControlText")]
		public Color ContentColor { get; set; }

		[Category("Appearance")]
		[DefaultValue(typeof(Color), "WindowFrame")]
		[Description("Color of the window border.")]
		public Color BorderColor { get; set; }

		[Category("Buttons")]
		[Description("Border color of the close and options buttons when the mouse is over them.")]
		[DefaultValue(typeof(Color), "WindowFrame")]
		public Color ButtonBorderColor { get; set; }

		[Description("Background color of the close and options buttons when the mouse is over them.")]
		[Category("Buttons")]
		[DefaultValue(typeof(Color), "Highlight")]
		public Color ButtonHoverColor { get; set; }

		[DefaultValue(typeof(Color), "HotTrack")]
		[Description("Color of the content text when the mouse is hovering over it.")]
		[Category("Content")]
		public Color ContentHoverColor { get; set; }

		[DefaultValue(50)]
		[Description("Gradient of window background color.")]
		[Category("Appearance")]
		public int GradientPower { get; set; }

		[Category("Content")]
		[Description("Font of the content text.")]
		public Font ContentFont { get; set; }

		[Category("Title")]
		[Description("Font of the title.")]
		public Font TitleFont { get; set; }

		[Description("Size of the icon image.")]
		[Category("Image")]
		public Size ImageSize
		{
			get
			{
				if (imageSize.Width == 0)
				{
					if (Image != null)
					{
						return Image.Size;
					}
					return new Size(0, 0);
				}
				return imageSize;
			}
			set
			{
				imageSize = value;
			}
		}

		[Category("Image")]
		[Description("Icon image to display.")]
		public Image Image { get; set; }

		[Category("Header")]
		[DefaultValue(true)]
		[Description("Whether to show a 'grip' image within the window header.")]
		public bool ShowGrip { get; set; }

		[Category("Behavior")]
		[DefaultValue(true)]
		[Description("Whether to scroll the window or only fade it.")]
		public bool Scroll { get; set; }

		[Description("Content text to display.")]
		[Category("Content")]
		public string ContentText { get; set; }

		[Description("Title text to display.")]
		[Category("Title")]
		public string TitleText { get; set; }

		[Category("Title")]
		[Description("Padding of title text.")]
		public Padding TitlePadding { get; set; }

		[Category("Content")]
		[Description("Padding of content text.")]
		public Padding ContentPadding { get; set; }

		[Category("Image")]
		[Description("Padding of icon image.")]
		public Padding ImagePadding { get; set; }

		[DefaultValue(9)]
		[Category("Header")]
		[Description("Height of window header.")]
		public int HeaderHeight { get; set; }

		[Category("Buttons")]
		[DefaultValue(true)]
		[Description("Whether to show the close button.")]
		public bool ShowCloseButton { get; set; }

		[Category("Buttons")]
		[DefaultValue(false)]
		[Description("Whether to show the options button.")]
		public bool ShowOptionsButton { get; set; }

		[Description("Context menu to open when clicking on the options button.")]
		[Category("Behavior")]
		public ContextMenuStrip OptionsMenu { get; set; }

		[Category("Behavior")]
		[Description("Time in milliseconds the window is displayed.")]
		[DefaultValue(3000)]
		public int Delay { get; set; }

		[DefaultValue(1000)]
		[Category("Behavior")]
		[Description("Time in milliseconds needed to make the window appear or disappear.")]
		public int AnimationDuration { get; set; }

		[Description("Interval in milliseconds used to draw the animation.")]
		[Category("Behavior")]
		[DefaultValue(10)]
		public int AnimationInterval { get; set; }

		[Category("Appearance")]
		[Description("Size of the window.")]
		public Size Size { get; set; }

		[Category("Content")]
		[Description("Show Content Right To Left,نمایش پیغام چپ به راست فعال شود")]
		public bool IsRightToLeft { get; set; }

		public event EventHandler Click;

		public event EventHandler Close;

		public event EventHandler Appear;

		public event EventHandler Disappear;

		[DllImport("user32.dll")]
		private static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		private static void ShowInactiveTopmost(Form frm)
		{
			ShowWindow(((Control)frm).Handle, 4);
			SetWindowPos(((Control)frm).Handle.ToInt32(), -1, ((Control)frm).Left, ((Control)frm).Top, ((Control)frm).Width, ((Control)frm).Height, 16u);
		}

		public void ResetImageSize()
		{
			imageSize = Size.Empty;
		}

		private bool ShouldSerializeImageSize()
		{
			return !imageSize.Equals((object?)Size.Empty);
		}

		private void ResetTitlePadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			TitlePadding = Padding.Empty;
		}

		private bool ShouldSerializeTitlePadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Padding titlePadding = TitlePadding;
			return !((object)(Padding)(ref titlePadding)).Equals((object?)Padding.Empty);
		}

		private void ResetContentPadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ContentPadding = Padding.Empty;
		}

		private bool ShouldSerializeContentPadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Padding contentPadding = ContentPadding;
			return !((object)(Padding)(ref contentPadding)).Equals((object?)Padding.Empty);
		}

		private void ResetImagePadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ImagePadding = Padding.Empty;
		}

		private bool ShouldSerializeImagePadding()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Padding imagePadding = ImagePadding;
			return !((object)(Padding)(ref imagePadding)).Equals((object?)Padding.Empty);
		}

		public PopupNotifier()
		{
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Expected O, but got Unknown
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Expected O, but got Unknown
			HeaderColor = SystemColors.ControlDark;
			BodyColor = SystemColors.Control;
			TitleColor = Color.Gray;
			ContentColor = SystemColors.ControlText;
			BorderColor = SystemColors.WindowFrame;
			ButtonBorderColor = SystemColors.WindowFrame;
			ButtonHoverColor = SystemColors.Highlight;
			ContentHoverColor = SystemColors.HotTrack;
			GradientPower = 50;
			ContentFont = SystemFonts.DialogFont;
			TitleFont = SystemFonts.CaptionFont;
			ShowGrip = true;
			Scroll = true;
			TitlePadding = new Padding(0);
			ContentPadding = new Padding(0);
			ImagePadding = new Padding(0);
			HeaderHeight = 9;
			ShowCloseButton = true;
			ShowOptionsButton = false;
			Delay = 3000;
			AnimationInterval = 10;
			AnimationDuration = 1000;
			Size = new Size(400, 100);
			frmPopup = new PopupNotifierForm(this);
			((Form)frmPopup).FormBorderStyle = (FormBorderStyle)0;
			((Form)frmPopup).StartPosition = (FormStartPosition)0;
			((Form)frmPopup).FormBorderStyle = (FormBorderStyle)0;
			((Control)frmPopup).MouseEnter += frmPopup_MouseEnter;
			((Control)frmPopup).MouseLeave += frmPopup_MouseLeave;
			frmPopup.CloseClick += frmPopup_CloseClick;
			frmPopup.LinkClick += frmPopup_LinkClick;
			frmPopup.ContextMenuOpened += frmPopup_ContextMenuOpened;
			frmPopup.ContextMenuClosed += frmPopup_ContextMenuClosed;
			((Control)frmPopup).VisibleChanged += frmPopup_VisibleChanged;
			tmrAnimation = new Timer();
			tmrAnimation.Tick += tmAnimation_Tick;
			tmrWait = new Timer();
			tmrWait.Tick += tmWait_Tick;
		}

		public void Popup()
		{
			if (disposed)
			{
				return;
			}
			if (!((Control)frmPopup).Visible)
			{
				((Form)frmPopup).Size = Size;
				if (Scroll)
				{
					posStart = Screen.PrimaryScreen.WorkingArea.Bottom;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				}
				else
				{
					posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				}
				opacityStart = 0.0;
				opacityStop = 1.0;
				((Form)frmPopup).Opacity = opacityStart;
				((Form)frmPopup).Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - ((Form)frmPopup).Size.Width - 1, posStart);
				ShowInactiveTopmost((Form)(object)frmPopup);
				isAppearing = true;
				tmrWait.Interval = Delay;
				tmrAnimation.Interval = AnimationInterval;
				realAnimationDuration = AnimationDuration;
				tmrAnimation.Start();
				sw = Stopwatch.StartNew();
				return;
			}
			if (!isAppearing)
			{
				((Form)frmPopup).Size = Size;
				if (Scroll)
				{
					posStart = ((Control)frmPopup).Top;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				}
				else
				{
					posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				}
				opacityStart = ((Form)frmPopup).Opacity;
				opacityStop = 1.0;
				isAppearing = true;
				realAnimationDuration = Math.Max((int)sw.ElapsedMilliseconds, 1);
				sw.Restart();
			}
			((Control)frmPopup).Invalidate();
		}

		public void Hide()
		{
			tmrAnimation.Stop();
			tmrWait.Stop();
			((Control)frmPopup).Hide();
			if (markedForDisposed)
			{
				Dispose();
			}
		}

		private void frmPopup_ContextMenuClosed(object sender, EventArgs e)
		{
			if (!mouseIsOn)
			{
				tmrWait.Interval = Delay;
				tmrWait.Start();
			}
		}

		private void frmPopup_ContextMenuOpened(object sender, EventArgs e)
		{
			tmrWait.Stop();
		}

		private void frmPopup_LinkClick(object sender, EventArgs e)
		{
			if (this.Click != null)
			{
				this.Click(this, EventArgs.Empty);
			}
		}

		private void frmPopup_CloseClick(object sender, EventArgs e)
		{
			Hide();
			if (this.Close != null)
			{
				this.Close(this, EventArgs.Empty);
			}
		}

		private void frmPopup_VisibleChanged(object sender, EventArgs e)
		{
			if (((Control)frmPopup).Visible)
			{
				if (this.Appear != null)
				{
					this.Appear(this, EventArgs.Empty);
				}
			}
			else if (this.Disappear != null)
			{
				this.Disappear(this, EventArgs.Empty);
			}
		}

		private void tmAnimation_Tick(object sender, EventArgs e)
		{
			long elapsedMilliseconds = sw.ElapsedMilliseconds;
			int num = (int)(posStart + (posStop - posStart) * elapsedMilliseconds / realAnimationDuration);
			bool flag = posStop - posStart < 0;
			if ((flag && num < posStop) || (!flag && num > posStop))
			{
				num = posStop;
			}
			double num2 = opacityStart + (opacityStop - opacityStart) * (double)elapsedMilliseconds / (double)realAnimationDuration;
			flag = opacityStop - opacityStart < 0.0;
			if ((flag && num2 < opacityStop) || (!flag && num2 > opacityStop))
			{
				num2 = opacityStop;
			}
			((Control)frmPopup).Top = num;
			((Form)frmPopup).Opacity = num2;
			if (elapsedMilliseconds <= realAnimationDuration)
			{
				return;
			}
			sw.Reset();
			tmrAnimation.Stop();
			if (isAppearing)
			{
				if (Scroll)
				{
					posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom;
				}
				else
				{
					posStart = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
					posStop = Screen.PrimaryScreen.WorkingArea.Bottom - ((Control)frmPopup).Height;
				}
				opacityStart = 1.0;
				opacityStop = 0.0;
				realAnimationDuration = AnimationDuration;
				isAppearing = false;
				maxPosition = ((Control)frmPopup).Top;
				maxOpacity = ((Form)frmPopup).Opacity;
				if (!mouseIsOn)
				{
					tmrWait.Stop();
					tmrWait.Start();
				}
			}
			else
			{
				((Control)frmPopup).Hide();
				if (markedForDisposed)
				{
					Dispose();
				}
			}
		}

		private void tmWait_Tick(object sender, EventArgs e)
		{
			tmrWait.Stop();
			tmrAnimation.Interval = AnimationInterval;
			tmrAnimation.Start();
			sw.Restart();
		}

		private void frmPopup_MouseLeave(object sender, EventArgs e)
		{
			if (((Control)frmPopup).Visible && (OptionsMenu == null || !((ToolStripDropDown)OptionsMenu).Visible))
			{
				tmrWait.Interval = Delay;
				tmrWait.Start();
			}
			mouseIsOn = false;
		}

		private void frmPopup_MouseEnter(object sender, EventArgs e)
		{
			if (!isAppearing)
			{
				((Control)frmPopup).Top = maxPosition;
				((Form)frmPopup).Opacity = maxOpacity;
				tmrAnimation.Stop();
			}
			tmrWait.Stop();
			mouseIsOn = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (isAppearing)
				{
					markedForDisposed = true;
					return;
				}
				if (disposing)
				{
					if (frmPopup != null)
					{
						((Component)(object)frmPopup).Dispose();
					}
					tmrAnimation.Tick -= tmAnimation_Tick;
					tmrWait.Tick -= tmWait_Tick;
					((Component)(object)tmrAnimation).Dispose();
					((Component)(object)tmrWait).Dispose();
				}
				disposed = true;
			}
			base.Dispose(disposing);
		}
	}
	internal class PopupNotifierForm : Form
	{
		private bool mouseOnClose;

		private bool mouseOnLink;

		private bool mouseOnOptions;

		private int heightOfTitle;

		private bool gdiInitialized;

		private Rectangle rcBody;

		private Rectangle rcHeader;

		private Rectangle rcForm;

		private LinearGradientBrush brushBody;

		private LinearGradientBrush brushHeader;

		private Brush brushButtonHover;

		private Pen penButtonBorder;

		private Pen penContent;

		private Pen penBorder;

		private Brush brushForeColor;

		private Brush brushLinkHover;

		private Brush brushContent;

		private Brush brushTitle;

		public PopupNotifier Parent { get; set; }

		private RectangleF RectContentText
		{
			get
			{
				//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01df: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0210: Unknown result type (might be due to invalid IL or missing references)
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_0225: Unknown result type (might be due to invalid IL or missing references)
				//IL_022a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0252: Unknown result type (might be due to invalid IL or missing references)
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_026e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0273: Unknown result type (might be due to invalid IL or missing references)
				//IL_0283: Unknown result type (might be due to invalid IL or missing references)
				//IL_0288: Unknown result type (might be due to invalid IL or missing references)
				//IL_0298: Unknown result type (might be due to invalid IL or missing references)
				//IL_029d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0160: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0175: Unknown result type (might be due to invalid IL or missing references)
				//IL_0185: Unknown result type (might be due to invalid IL or missing references)
				//IL_018a: Unknown result type (might be due to invalid IL or missing references)
				if (Parent.Image != null)
				{
					Padding imagePadding = Parent.ImagePadding;
					int num = ((Padding)(ref imagePadding)).Left + Parent.ImageSize.Width;
					Padding imagePadding2 = Parent.ImagePadding;
					int num2 = num + ((Padding)(ref imagePadding2)).Right;
					Padding contentPadding = Parent.ContentPadding;
					float x = num2 + ((Padding)(ref contentPadding)).Left;
					int headerHeight = Parent.HeaderHeight;
					Padding titlePadding = Parent.TitlePadding;
					int num3 = headerHeight + ((Padding)(ref titlePadding)).Top + heightOfTitle;
					Padding titlePadding2 = Parent.TitlePadding;
					int num4 = num3 + ((Padding)(ref titlePadding2)).Bottom;
					Padding contentPadding2 = Parent.ContentPadding;
					float y = num4 + ((Padding)(ref contentPadding2)).Top;
					int width = ((Control)this).Width;
					Padding imagePadding3 = Parent.ImagePadding;
					int num5 = width - ((Padding)(ref imagePadding3)).Left - Parent.ImageSize.Width;
					Padding imagePadding4 = Parent.ImagePadding;
					int num6 = num5 - ((Padding)(ref imagePadding4)).Right;
					Padding contentPadding3 = Parent.ContentPadding;
					int num7 = num6 - ((Padding)(ref contentPadding3)).Left;
					Padding contentPadding4 = Parent.ContentPadding;
					float width2 = num7 - ((Padding)(ref contentPadding4)).Right - 16 - 5;
					int num8 = ((Control)this).Height - Parent.HeaderHeight;
					Padding titlePadding3 = Parent.TitlePadding;
					int num9 = num8 - ((Padding)(ref titlePadding3)).Top - heightOfTitle;
					Padding titlePadding4 = Parent.TitlePadding;
					int num10 = num9 - ((Padding)(ref titlePadding4)).Bottom;
					Padding contentPadding5 = Parent.ContentPadding;
					int num11 = num10 - ((Padding)(ref contentPadding5)).Top;
					Padding contentPadding6 = Parent.ContentPadding;
					return new RectangleF(x, y, width2, num11 - ((Padding)(ref contentPadding6)).Bottom - 1);
				}
				Padding contentPadding7 = Parent.ContentPadding;
				float x2 = ((Padding)(ref contentPadding7)).Left;
				int headerHeight2 = Parent.HeaderHeight;
				Padding titlePadding5 = Parent.TitlePadding;
				int num12 = headerHeight2 + ((Padding)(ref titlePadding5)).Top + heightOfTitle;
				Padding titlePadding6 = Parent.TitlePadding;
				int num13 = num12 + ((Padding)(ref titlePadding6)).Bottom;
				Padding contentPadding8 = Parent.ContentPadding;
				float y2 = num13 + ((Padding)(ref contentPadding8)).Top;
				int width3 = ((Control)this).Width;
				Padding contentPadding9 = Parent.ContentPadding;
				int num14 = width3 - ((Padding)(ref contentPadding9)).Left;
				Padding contentPadding10 = Parent.ContentPadding;
				float width4 = num14 - ((Padding)(ref contentPadding10)).Right - 16 - 5;
				int num15 = ((Control)this).Height - Parent.HeaderHeight;
				Padding titlePadding7 = Parent.TitlePadding;
				int num16 = num15 - ((Padding)(ref titlePadding7)).Top - heightOfTitle;
				Padding titlePadding8 = Parent.TitlePadding;
				int num17 = num16 - ((Padding)(ref titlePadding8)).Bottom;
				Padding contentPadding11 = Parent.ContentPadding;
				int num18 = num17 - ((Padding)(ref contentPadding11)).Top;
				Padding contentPadding12 = Parent.ContentPadding;
				return new RectangleF(x2, y2, width4, num18 - ((Padding)(ref contentPadding12)).Bottom - 1);
			}
		}

		private Rectangle RectClose => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3, 16, 16);

		private Rectangle RectOptions => new Rectangle(((Control)this).Width - 5 - 16, Parent.HeaderHeight + 3 + 16 + 5, 16, 16);

		public event EventHandler LinkClick;

		public event EventHandler CloseClick;

		internal event EventHandler ContextMenuOpened;

		internal event EventHandler ContextMenuClosed;

		public PopupNotifierForm(PopupNotifier parent)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			Parent = parent;
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Form)this).ShowInTaskbar = false;
			((Control)this).VisibleChanged += PopupNotifierForm_VisibleChanged;
			((Control)this).MouseMove += new MouseEventHandler(PopupNotifierForm_MouseMove);
			((Control)this).MouseUp += new MouseEventHandler(PopupNotifierForm_MouseUp);
			((Control)this).Paint += new PaintEventHandler(PopupNotifierForm_Paint);
		}

		private void PopupNotifierForm_VisibleChanged(object sender, EventArgs e)
		{
			if (((Control)this).Visible)
			{
				mouseOnClose = false;
				mouseOnLink = false;
				mouseOnOptions = false;
			}
		}

		private void InitializeComponent()
		{
			((Control)this).SuspendLayout();
			((Form)this).ClientSize = new Size(392, 66);
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			((Control)this).Name = "PopupNotifierForm";
			((Form)this).StartPosition = (FormStartPosition)0;
			((Control)this).ResumeLayout(false);
		}

		private int AddValueMax255(int input, int add)
		{
			if (input + add >= 256)
			{
				return 255;
			}
			return input + add;
		}

		private int DedValueMin0(int input, int ded)
		{
			if (input - ded <= 0)
			{
				return 0;
			}
			return input - ded;
		}

		private Color GetDarkerColor(Color color)
		{
			return Color.FromArgb(255, DedValueMin0(color.R, Parent.GradientPower), DedValueMin0(color.G, Parent.GradientPower), DedValueMin0(color.B, Parent.GradientPower));
		}

		private Color GetLighterColor(Color color)
		{
			return Color.FromArgb(255, AddValueMax255(color.R, Parent.GradientPower), AddValueMax255(color.G, Parent.GradientPower), AddValueMax255(color.B, Parent.GradientPower));
		}

		private void PopupNotifierForm_MouseMove(object sender, MouseEventArgs e)
		{
			if (Parent.ShowCloseButton)
			{
				mouseOnClose = RectClose.Contains(e.X, e.Y);
			}
			if (Parent.ShowOptionsButton)
			{
				mouseOnOptions = RectOptions.Contains(e.X, e.Y);
			}
			mouseOnLink = RectContentText.Contains(e.X, e.Y);
			((Control)this).Invalidate();
		}

		private void PopupNotifierForm_MouseUp(object sender, MouseEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Invalid comparison between Unknown and I4
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Expected O, but got Unknown
			if ((int)e.Button != 1048576)
			{
				return;
			}
			if (RectClose.Contains(e.X, e.Y) && this.CloseClick != null)
			{
				this.CloseClick(this, EventArgs.Empty);
			}
			if (RectContentText.Contains(e.X, e.Y) && this.LinkClick != null)
			{
				this.LinkClick(this, EventArgs.Empty);
			}
			if (RectOptions.Contains(e.X, e.Y) && Parent.OptionsMenu != null)
			{
				if (this.ContextMenuOpened != null)
				{
					this.ContextMenuOpened(this, EventArgs.Empty);
				}
				((ToolStripDropDown)Parent.OptionsMenu).Show((Control)(object)this, new Point(RectOptions.Right - ((Control)Parent.OptionsMenu).Width, RectOptions.Bottom));
				((ToolStripDropDown)Parent.OptionsMenu).Closed += new ToolStripDropDownClosedEventHandler(OptionsMenu_Closed);
			}
		}

		private void OptionsMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			((ToolStripDropDown)Parent.OptionsMenu).Closed -= new ToolStripDropDownClosedEventHandler(OptionsMenu_Closed);
			if (this.ContextMenuClosed != null)
			{
				this.ContextMenuClosed(this, EventArgs.Empty);
			}
		}

		private void AllocateGDIObjects()
		{
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Expected O, but got Unknown
			rcBody = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			rcHeader = new Rectangle(0, 0, ((Control)this).Width, Parent.HeaderHeight);
			rcForm = new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
			brushBody = new LinearGradientBrush(rcBody, Parent.BodyColor, GetLighterColor(Parent.BodyColor), (LinearGradientMode)1);
			brushHeader = new LinearGradientBrush(rcHeader, Parent.HeaderColor, GetDarkerColor(Parent.HeaderColor), (LinearGradientMode)1);
			brushButtonHover = (Brush)new SolidBrush(Parent.ButtonHoverColor);
			penButtonBorder = new Pen(Parent.ButtonBorderColor);
			penContent = new Pen(Parent.ContentColor, 2f);
			penBorder = new Pen(Parent.BorderColor);
			brushForeColor = (Brush)new SolidBrush(((Control)this).ForeColor);
			brushLinkHover = (Brush)new SolidBrush(Parent.ContentHoverColor);
			brushContent = (Brush)new SolidBrush(Parent.ContentColor);
			brushTitle = (Brush)new SolidBrush(Parent.TitleColor);
			gdiInitialized = true;
		}

		private void DisposeGDIObjects()
		{
			if (gdiInitialized)
			{
				((Brush)brushBody).Dispose();
				((Brush)brushHeader).Dispose();
				brushButtonHover.Dispose();
				penButtonBorder.Dispose();
				penContent.Dispose();
				penBorder.Dispose();
				brushForeColor.Dispose();
				brushLinkHover.Dispose();
				brushContent.Dispose();
				brushTitle.Dispose();
			}
		}

		private void PopupNotifierForm_Paint(object sender, PaintEventArgs e)
		{
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Expected O, but got Unknown
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_0455: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Expected O, but got Unknown
			if (!gdiInitialized)
			{
				AllocateGDIObjects();
			}
			e.Graphics.FillRectangle((Brush)(object)brushBody, rcBody);
			e.Graphics.FillRectangle((Brush)(object)brushHeader, rcHeader);
			e.Graphics.DrawRectangle(penBorder, rcForm);
			if (Parent.ShowGrip)
			{
				e.Graphics.DrawImage((Image)(object)Resources.Grip, (((Control)this).Width - ((Image)Resources.Grip).Width) / 2, (Parent.HeaderHeight - 3) / 2);
			}
			if (Parent.ShowCloseButton)
			{
				if (mouseOnClose)
				{
					e.Graphics.FillRectangle(brushButtonHover, RectClose);
					e.Graphics.DrawRectangle(penButtonBorder, RectClose);
				}
				e.Graphics.DrawLine(penContent, RectClose.Left + 4, RectClose.Top + 4, RectClose.Right - 4, RectClose.Bottom - 4);
				e.Graphics.DrawLine(penContent, RectClose.Left + 4, RectClose.Bottom - 4, RectClose.Right - 4, RectClose.Top + 4);
			}
			if (Parent.ShowOptionsButton)
			{
				if (mouseOnOptions)
				{
					e.Graphics.FillRectangle(brushButtonHover, RectOptions);
					e.Graphics.DrawRectangle(penButtonBorder, RectOptions);
				}
				e.Graphics.FillPolygon(brushForeColor, new Point[3]
				{
					new Point(RectOptions.Left + 4, RectOptions.Top + 6),
					new Point(RectOptions.Left + 12, RectOptions.Top + 6),
					new Point(RectOptions.Left + 8, RectOptions.Top + 4 + 6)
				});
			}
			if (Parent.Image != null)
			{
				Graphics graphics = e.Graphics;
				Image image = Parent.Image;
				Padding imagePadding = Parent.ImagePadding;
				int left = ((Padding)(ref imagePadding)).Left;
				int headerHeight = Parent.HeaderHeight;
				Padding imagePadding2 = Parent.ImagePadding;
				graphics.DrawImage(image, left, headerHeight + ((Padding)(ref imagePadding2)).Top, Parent.ImageSize.Width, Parent.ImageSize.Height);
			}
			if (Parent.IsRightToLeft)
			{
				heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
				int num = ((Control)this).Width - 30;
				StringFormat val = new StringFormat((StringFormatFlags)1);
				Graphics graphics2 = e.Graphics;
				string titleText = Parent.TitleText;
				Font titleFont = Parent.TitleFont;
				Brush obj = brushTitle;
				float num2 = num;
				int headerHeight2 = Parent.HeaderHeight;
				Padding titlePadding = Parent.TitlePadding;
				graphics2.DrawString(titleText, titleFont, obj, num2, (float)(headerHeight2 + ((Padding)(ref titlePadding)).Top), val);
				((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
				Brush val2 = (mouseOnLink ? brushLinkHover : brushContent);
				StringFormat val3 = new StringFormat((StringFormatFlags)1);
				e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val2, RectContentText, val3);
				return;
			}
			heightOfTitle = (int)e.Graphics.MeasureString("A", Parent.TitleFont).Height;
			Padding titlePadding2 = Parent.TitlePadding;
			int num3 = ((Padding)(ref titlePadding2)).Left;
			if (Parent.Image != null)
			{
				int num4 = num3;
				Padding imagePadding3 = Parent.ImagePadding;
				int num5 = ((Padding)(ref imagePadding3)).Left + Parent.ImageSize.Width;
				Padding imagePadding4 = Parent.ImagePadding;
				num3 = num4 + (num5 + ((Padding)(ref imagePadding4)).Right);
			}
			Graphics graphics3 = e.Graphics;
			string titleText2 = Parent.TitleText;
			Font titleFont2 = Parent.TitleFont;
			Brush obj2 = brushTitle;
			float num6 = num3;
			int headerHeight3 = Parent.HeaderHeight;
			Padding titlePadding3 = Parent.TitlePadding;
			graphics3.DrawString(titleText2, titleFont2, obj2, num6, (float)(headerHeight3 + ((Padding)(ref titlePadding3)).Top));
			((Control)this).Cursor = (mouseOnLink ? Cursors.Hand : Cursors.Default);
			Brush val4 = (mouseOnLink ? brushLinkHover : brushContent);
			e.Graphics.DrawString(Parent.ContentText, Parent.ContentFont, val4, RectContentText);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				DisposeGDIObjects();
			}
			((Form)this).Dispose(disposing);
		}
	}
}
namespace Tulpep.NotificationWindow.Properties
{
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resources
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
					ResourceManager resourceManager = new ResourceManager("Tulpep.NotificationWindow.Properties.Resources", typeof(Resources).Assembly);
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

		internal static Bitmap Grip
		{
			get
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Expected O, but got Unknown
				object @object = ResourceManager.GetObject("Grip", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal Resources()
		{
		}
	}
}

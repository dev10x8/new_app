using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;

[assembly: AssemblyDescription("")]
[assembly: AssemblyTitle("Madhushala Downloader")]
[assembly: AssemblyCompany("")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: TargetFramework(".NETFramework,Version=v4.0,Profile=Client", FrameworkDisplayName = ".NET Framework 4 Client Profile")]
[assembly: AssemblyProduct("Madhushala Downloader")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("30a1d54f-9e69-4225-b149-946d618aeb7f")]
[assembly: ComVisible(false)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCopyright("Copyright ©  2025")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace Madhushala_Downloader.My
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	internal class MyApplication : WindowsFormsApplicationBase
	{
		private static List<WeakReference> __ENCList = new List<WeakReference>();

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

		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		[STAThread]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerHidden]
		internal static void Main(string[] Args)
		{
			try
			{
				Application.SetCompatibleTextRenderingDefault(WindowsFormsApplicationBase.UseCompatibleTextRendering);
			}
			finally
			{
			}
			((WindowsFormsApplicationBase)MyProject.Application).Run(Args);
		}

		[DebuggerStepThrough]
		public MyApplication()
			: base((AuthenticationMode)0)
		{
			__ENCAddToList(this);
			((WindowsFormsApplicationBase)this).IsSingleInstance = false;
			((WindowsFormsApplicationBase)this).EnableVisualStyles = true;
			((WindowsFormsApplicationBase)this).SaveMySettingsOnExit = true;
			((WindowsFormsApplicationBase)this).ShutdownStyle = (ShutdownMode)0;
		}

		[DebuggerStepThrough]
		protected override void OnCreateMainForm()
		{
			((WindowsFormsApplicationBase)this).MainForm = (Form)(object)MyProject.Forms.Form1;
		}
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	internal class MyComputer : Computer
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
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
			public Form1 m_Form1;

			[ThreadStatic]
			private static Hashtable m_FormBeingCreated;

			public Form1 Form1
			{
				[DebuggerNonUserCode]
				get
				{
					m_Form1 = Create__Instance__(m_Form1);
					return m_Form1;
				}
				[DebuggerNonUserCode]
				set
				{
					if (value != m_Form1)
					{
						if (value != null)
						{
							throw new ArgumentException("Property can only be set to Nothing");
						}
						Dispose__Instance__(ref m_Form1);
					}
				}
			}

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

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
		internal sealed class MyWebServices
		{
			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public override bool Equals(object o)
			{
				return base.Equals(RuntimeHelpers.GetObjectValue(o));
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
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

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
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

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public MyWebServices()
			{
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[ComVisible(false)]
		internal sealed class ThreadSafeObjectProvider<T> where T : new()
		{
			[ThreadStatic]
			[CompilerGenerated]
			private static T m_ThreadStaticValue;

			internal T GetInstance
			{
				[DebuggerHidden]
				get
				{
					if (m_ThreadStaticValue == null)
					{
						m_ThreadStaticValue = new T();
					}
					return m_ThreadStaticValue;
				}
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public ThreadSafeObjectProvider()
			{
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
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class InternalXmlHelper
	{
		[CompilerGenerated]
		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		private sealed class RemoveNamespaceAttributesClosure
		{
			private readonly string[] m_inScopePrefixes;

			private readonly XNamespace[] m_inScopeNs;

			private readonly List<XAttribute> m_attributes;

			[EditorBrowsable(EditorBrowsableState.Never)]
			internal RemoveNamespaceAttributesClosure(string[] inScopePrefixes, XNamespace[] inScopeNs, List<XAttribute> attributes)
			{
				m_inScopePrefixes = inScopePrefixes;
				m_inScopeNs = inScopeNs;
				m_attributes = attributes;
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			internal XElement ProcessXElement(XElement elem)
			{
				return RemoveNamespaceAttributes(m_inScopePrefixes, m_inScopeNs, m_attributes, elem);
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			internal object ProcessObject(object obj)
			{
				XElement val = (XElement)((obj is XElement) ? obj : null);
				if (val != null)
				{
					return RemoveNamespaceAttributes(m_inScopePrefixes, m_inScopeNs, m_attributes, val);
				}
				return obj;
			}
		}

		public static string Value
		{
			get
			{
				using (IEnumerator<XElement> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						XElement current = enumerator.Current;
						return current.Value;
					}
				}
				return null;
			}
			set
			{
				using IEnumerator<XElement> enumerator = source.GetEnumerator();
				if (enumerator.MoveNext())
				{
					XElement current = enumerator.Current;
					current.Value = value;
				}
			}
		}

		public static string AttributeValue
		{
			get
			{
				using (IEnumerator<XElement> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						XElement current = enumerator.Current;
						return (string)current.Attribute(name);
					}
				}
				return null;
			}
			set
			{
				using IEnumerator<XElement> enumerator = source.GetEnumerator();
				if (enumerator.MoveNext())
				{
					XElement current = enumerator.Current;
					current.SetAttributeValue(name, (object)value);
				}
			}
		}

		public static string AttributeValue
		{
			get
			{
				return (string)source.Attribute(name);
			}
			set
			{
				source.SetAttributeValue(name, (object)value);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		private InternalXmlHelper()
		{
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static XAttribute CreateAttribute(XName name, object value)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			if (value == null)
			{
				return null;
			}
			return new XAttribute(name, RuntimeHelpers.GetObjectValue(value));
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static XAttribute CreateNamespaceAttribute(XName name, XNamespace ns)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			XAttribute val = new XAttribute(name, (object)ns.NamespaceName);
			((XObject)val).AddAnnotation((object)ns);
			return val;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static object RemoveNamespaceAttributes(string[] inScopePrefixes, XNamespace[] inScopeNs, List<XAttribute> attributes, object obj)
		{
			if (obj != null)
			{
				XElement val = (XElement)((obj is XElement) ? obj : null);
				if (val != null)
				{
					return RemoveNamespaceAttributes(inScopePrefixes, inScopeNs, attributes, val);
				}
				if (obj is IEnumerable obj2)
				{
					return RemoveNamespaceAttributes(inScopePrefixes, inScopeNs, attributes, obj2);
				}
			}
			return obj;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IEnumerable RemoveNamespaceAttributes(string[] inScopePrefixes, XNamespace[] inScopeNs, List<XAttribute> attributes, IEnumerable obj)
		{
			if (obj != null)
			{
				if (obj is IEnumerable<XElement> source)
				{
					return source.Select(new RemoveNamespaceAttributesClosure(inScopePrefixes, inScopeNs, attributes).ProcessXElement);
				}
				return obj.Cast<object>().Select(new RemoveNamespaceAttributesClosure(inScopePrefixes, inScopeNs, attributes).ProcessObject);
			}
			return obj;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static XElement RemoveNamespaceAttributes(string[] inScopePrefixes, XNamespace[] inScopeNs, List<XAttribute> attributes, XElement e)
		{
			checked
			{
				if (e != null)
				{
					XAttribute val = e.FirstAttribute;
					while (val != null)
					{
						XAttribute nextAttribute = val.NextAttribute;
						if (val.IsNamespaceDeclaration)
						{
							XNamespace val2 = ((XObject)val).Annotation<XNamespace>();
							string localName = val.Name.LocalName;
							if (val2 != null)
							{
								if ((inScopePrefixes != null && inScopeNs != null) ? true : false)
								{
									int num = inScopePrefixes.Length - 1;
									int num2 = num;
									int num3 = 0;
									while (true)
									{
										int num4 = num3;
										int num5 = num2;
										if (num4 > num5)
										{
											break;
										}
										string value = inScopePrefixes[num3];
										XNamespace val3 = inScopeNs[num3];
										if (localName.Equals(value))
										{
											if (val2 == val3)
											{
												val.Remove();
											}
											val = null;
											break;
										}
										num3++;
									}
								}
								if (val != null)
								{
									if (attributes != null)
									{
										int num6 = attributes.Count - 1;
										int num7 = num6;
										int num8 = 0;
										while (true)
										{
											int num9 = num8;
											int num5 = num7;
											if (num9 > num5)
											{
												break;
											}
											XAttribute val4 = attributes[num8];
											string localName2 = val4.Name.LocalName;
											XNamespace val5 = ((XObject)val4).Annotation<XNamespace>();
											if (val5 != null && localName.Equals(localName2))
											{
												if (val2 == val5)
												{
													val.Remove();
												}
												val = null;
												break;
											}
											num8++;
										}
									}
									if (val != null)
									{
										val.Remove();
										attributes.Add(val);
									}
								}
							}
						}
						val = nextAttribute;
					}
				}
				return e;
			}
		}
	}
}
namespace Madhushala_Downloader
{
	[DesignerGenerated]
	public class Form1 : Form
	{
		private static List<WeakReference> __ENCList = new List<WeakReference>();

		private IContainer components;

		private float angle;

		private Timer timer;

		private int loaderDots;

		private int loaderRadius;

		public Form1()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			((Form)this).Load += Form1_Load;
			__ENCAddToList(this);
			angle = 0f;
			Timer val = new Timer();
			val.Interval = 50;
			timer = val;
			loaderDots = 12;
			loaderRadius = 40;
			InitializeComponent();
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
				((Form)this).Dispose(disposing);
			}
		}

		[DebuggerStepThrough]
		private void InitializeComponent()
		{
			((Control)this).SuspendLayout();
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleDimensions = autoScaleDimensions;
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			Size clientSize = new Size(284, 261);
			((Form)this).ClientSize = clientSize;
			((Control)this).Name = "Form1";
			((Form)this).StartPosition = (FormStartPosition)1;
			((Form)this).Text = "Form1";
			((Control)this).ResumeLayout(false);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			((Form)this).FormBorderStyle = (FormBorderStyle)0;
			((Form)this).StartPosition = (FormStartPosition)1;
			((Form)this).BackColor = Color.White;
			((Control)this).Width = 320;
			((Control)this).Height = 200;
			((Form)this).ShowInTaskbar = false;
			((Form)this).Text = "Updating...";
			timer.Tick += OnTick;
			timer.Start();
			Thread thread = new Thread(SafeUpdate);
			thread.IsBackground = true;
			thread.Start();
		}

		private void OnTick(object sender, EventArgs e)
		{
			angle += 10f;
			if (angle >= 360f)
			{
				angle = 0f;
			}
			((Control)this).Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Expected O, but got Unknown
			((Form)this).OnPaint(e);
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = (SmoothingMode)4;
			Font val = new Font("Segoe UI", 12f, (FontStyle)1);
			string text = "Please wait...";
			SizeF sizeF = graphics.MeasureString(text, val);
			graphics.DrawString(text, val, Brushes.Black, ((float)((Control)this).Width - sizeF.Width) / 2f, (float)((double)((Control)this).Height / 2.0 - 30.0));
			checked
			{
				int num = (int)Math.Round((double)((Control)this).Width / 2.0);
				int num2 = (int)Math.Round((double)((Control)this).Height / 2.0 + 20.0);
				int num3 = loaderDots - 1;
				int num4 = 0;
				while (true)
				{
					int num5 = num4;
					int num6 = num3;
					if (num5 <= num6)
					{
						float num7 = (float)((double)(num4 * 360) / (double)loaderDots + (double)angle);
						double num8 = (double)num7 * Math.PI / 180.0;
						float num9 = (float)num + (float)loaderRadius * (float)Math.Cos(num8);
						float num10 = (float)num2 + (float)loaderRadius * (float)Math.Sin(num8);
						int alpha = (int)Math.Round((double)(255 * (num4 + 1)) / (double)loaderDots);
						SolidBrush val2 = new SolidBrush(Color.FromArgb(alpha, Color.FromArgb(30, 144, 255)));
						graphics.FillEllipse((Brush)(object)val2, num9 - 6f, num10 - 6f, 12f, 12f);
						num4++;
						continue;
					}
					break;
				}
			}
		}

		private void SafeUpdate()
		{
			string text = "Madhushala Ultimate";
			string address = "http://13.235.37.46:85/New%20folder/POS%20Exe/Madhushala%20Ultimate.exe";
			string address2 = "http://13.235.37.46:85/New%20folder/West%20Bengal/M_B.bmp";
			string text2 = Path.Combine(Application.StartupPath, "M_B.bmp");
			string text3 = Path.Combine(Application.StartupPath, "M_B.NEW");
			string text4 = Path.Combine(Application.StartupPath, text + ".exe");
			string text5 = Path.Combine(Application.StartupPath, text + ".NEW");
			Process[] processesByName = Process.GetProcessesByName(text);
			if (processesByName.Length > 0)
			{
				ShowError("Software is already running. Close it and try again.");
				return;
			}
			try
			{
				if (File.Exists(text5))
				{
					File.Delete(text5);
				}
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
				WebClient webClient = new WebClient();
				webClient.DownloadFile(address, text5);
				if (!File.Exists(text5))
				{
					ShowError("Download failed: EXE missing after download.");
					return;
				}
				if (File.Exists(text4))
				{
					File.Delete(text4);
				}
				File.Move(text5, text4);
				webClient.DownloadFile(address2, text3);
				if (!File.Exists(text3))
				{
					ShowError("Download failed: BMP file missing after download.");
					return;
				}
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text3, text2);
				((Control)this).Invoke((Delegate)(VB$AnonymousDelegate_0)([SpecialName] () =>
				{
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					timer.Stop();
					Interaction.MsgBox((object)"Update completed successfully!", (MsgBoxStyle)64, (object)null);
					((Form)this).Close();
				}));
			}
			catch (WebException ex)
			{
				ProjectData.SetProjectError((Exception)ex);
				WebException ex2 = ex;
				string text6 = "";
				switch ((int)ex2.Status)
				{
				case 2:
					text6 = "Cannot connect to server. Check internet or server offline.";
					break;
				case 14:
					text6 = "Server timeout. Slow internet or busy server.";
					break;
				case 1:
					text6 = "Server address invalid or DNS issue.";
					break;
				case 7:
				{
					HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
					text6 = "Server error: " + Conversions.ToString((int)httpWebResponse.StatusCode) + " - " + httpWebResponse.StatusDescription;
					break;
				}
				default:
					text6 = "Network error: " + ex2.Message;
					break;
				}
				ShowError(text6);
				ProjectData.ClearProjectError();
			}
			catch (UnauthorizedAccessException ex3)
			{
				ProjectData.SetProjectError((Exception)ex3);
				UnauthorizedAccessException ex4 = ex3;
				ShowError("Access denied. Run application as Administrator.");
				ProjectData.ClearProjectError();
			}
			catch (IOException ex5)
			{
				ProjectData.SetProjectError((Exception)ex5);
				IOException ex6 = ex5;
				ShowError("File system error. Possible reasons:\r\n• Disk full\r\n• File in use by another process\r\n• Antivirus blocking");
				ProjectData.ClearProjectError();
			}
			catch (Exception ex7)
			{
				ProjectData.SetProjectError(ex7);
				Exception ex8 = ex7;
				ShowError("Unexpected error: " + ex8.Message);
				ProjectData.ClearProjectError();
			}
		}

		private void ShowError(string msg)
		{
			((Control)this).Invoke((Delegate)(VB$AnonymousDelegate_0)([SpecialName] () =>
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				timer.Stop();
				Interaction.MsgBox((object)("Update failed!\r\n" + msg), (MsgBoxStyle)16, (object)null);
				((Form)this).Close();
			}));
		}
	}
}
namespace Madhushala_Downloader.My.Resources
{
	[HideModuleName]
	[StandardModule]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
					ResourceManager resourceManager = new ResourceManager("Madhushala_Downloader.Resources", typeof(Resources).Assembly);
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
namespace Madhushala_Downloader.My
{
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal sealed class MySettings : ApplicationSettingsBase
	{
		private static MySettings defaultInstance = (MySettings)(object)SettingsBase.Synchronized((SettingsBase)(object)new MySettings());

		private static bool addedHandler;

		private static object addedHandlerLockObject = RuntimeHelpers.GetObjectValue(new object());

		public static MySettings Default
		{
			get
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Expected O, but got Unknown
				if (!addedHandler)
				{
					object obj = addedHandlerLockObject;
					ObjectFlowControl.CheckForSyncLockOnValueType(obj);
					bool lockTaken = false;
					try
					{
						Monitor.Enter(obj, ref lockTaken);
						if (!addedHandler)
						{
							((WindowsFormsApplicationBase)MyProject.Application).Shutdown += (ShutdownEventHandler)([EditorBrowsable(EditorBrowsableState.Advanced)] [DebuggerNonUserCode] (object sender, EventArgs e) =>
							{
								if (((WindowsFormsApplicationBase)MyProject.Application).SaveMySettingsOnExit)
								{
									((ApplicationSettingsBase)MySettingsProperty.Settings).Save();
								}
							});
							addedHandler = true;
						}
					}
					finally
					{
						if (lockTaken)
						{
							Monitor.Exit(obj);
						}
					}
				}
				return defaultInstance;
			}
		}

		[DebuggerNonUserCode]
		public MySettings()
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DebuggerNonUserCode]
		private static void AutoSaveSettings(object sender, EventArgs e)
		{
			if (((WindowsFormsApplicationBase)MyProject.Application).SaveMySettingsOnExit)
			{
				((ApplicationSettingsBase)MySettingsProperty.Settings).Save();
			}
		}
	}
	[StandardModule]
	[HideModuleName]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	internal sealed class MySettingsProperty
	{
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings => MySettings.Default;
	}
}
[DebuggerDisplay("<generated method>", Type = "<generated method>")]
[CompilerGenerated]
internal delegate void VB$AnonymousDelegate_0();

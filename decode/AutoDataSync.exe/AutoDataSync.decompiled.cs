using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
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

[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("AutoDataSync")]
[assembly: AssemblyTitle("AutoDataSync")]
[assembly: ComVisible(false)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyTrademark("")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: CompilationRelaxations(8)]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("f9a9f4e3-4089-4c1c-a8c5-6089017a244b")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace AutoDataSync.My
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
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class MyComputer : Computer
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
		public MyComputer()
		{
		}
	}
	[StandardModule]
	[HideModuleName]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	internal sealed class MyProject
	{
		[MyGroupCollection("System.Windows.Forms.Form", "Create__Instance__", "Dispose__Instance__", "My.MyProject.Forms")]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			[EditorBrowsable(EditorBrowsableState.Never)]
			[DebuggerHidden]
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

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
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
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal sealed class InternalXmlHelper
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[CompilerGenerated]
		[DebuggerNonUserCode]
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
namespace AutoDataSync
{
	public class cls_getconnFrAutoUpdate
	{
		private SqlCommand lcmd;

		private SqlCommand scmd;

		public string lclConnectionString;

		public string srvrConnectionString;

		private SqlConnection lcnn;

		private SqlConnection scnn;

		public cls_getconnFrAutoUpdate()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			lcmd = new SqlCommand();
			scmd = new SqlCommand();
			lclConnectionString = "";
			srvrConnectionString = "";
			lcnn = new SqlConnection();
			scnn = new SqlConnection();
		}

		public void getcon1()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			string text10 = "";
			string text11 = "";
			string text12 = "";
			string text13 = "";
			string text14 = "";
			string text15 = "";
			string text16 = "";
			string path = Application.StartupPath + "\\connlcl.txt";
			string path2 = Application.StartupPath + "\\connsrvr.txt";
			StreamReader streamReader = new StreamReader(path);
			string[] array = File.ReadAllLines(path);
			checked
			{
				int num = array.Length - 1;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 > num4)
					{
						break;
					}
					string text17 = array[num2];
					if (num2 == 0)
					{
						text = text17;
					}
					if (num2 == 1)
					{
						text2 = text17;
					}
					if (num2 == 2)
					{
						text3 = text17;
					}
					if (num2 == 3)
					{
						text4 = text17;
					}
					num2++;
				}
				streamReader.Close();
				StreamReader streamReader2 = new StreamReader(path2);
				string[] array2 = File.ReadAllLines(path2);
				int num5 = array.Length - 1;
				int num6 = 0;
				while (true)
				{
					int num7 = num6;
					int num4 = num5;
					if (num7 > num4)
					{
						break;
					}
					string text18 = array2[num6];
					if (num6 == 0)
					{
						text5 = text18;
					}
					if (num6 == 1)
					{
						text6 = text18;
					}
					if (num6 == 2)
					{
						text7 = text18;
					}
					if (num6 == 3)
					{
						text8 = text18;
					}
					num6++;
				}
				streamReader2.Close();
				if (Operators.CompareString(text3, "WINDOWS AUTHENTICATION", false) != 0)
				{
					text9 = text;
					text10 = text2;
					text11 = text3;
					text12 = text4;
				}
				else
				{
					text9 = text;
					text10 = text2;
					text11 = "WINDOWS AUTHENTICATION";
					text12 = "WINDOWS AUTHENTICATION";
				}
				if (Operators.CompareString(text7, "WINDOWS AUTHENTICATION", false) != 0)
				{
					text13 = text5;
					text14 = text6;
					text15 = text7;
					text16 = text8;
				}
				else
				{
					text13 = text5;
					text14 = text6;
					text15 = "WINDOWS AUTHENTICATION";
					text16 = "WINDOWS AUTHENTICATION";
				}
				if (Operators.CompareString(text3, "WINDOWS AUTHENTICATION", false) == 0)
				{
					lclConnectionString = "Data Source=" + text9.Trim() + ";Initial Catalog=" + text2.Trim() + ";Trusted_Connection=True";
				}
				else
				{
					lclConnectionString = "Data Source=" + text9.Trim() + ";Initial Catalog=" + text2.Trim() + ";User ID=" + text11.Trim() + ";password=" + text12.Trim();
				}
				lcnn.ConnectionString = lclConnectionString;
				if (Operators.CompareString(text7, "WINDOWS AUTHENTICATION", false) == 0)
				{
					srvrConnectionString = "Data Source=" + text13.Trim() + ";Initial Catalog=" + text14.Trim() + ";Trusted_Connection=True";
				}
				else
				{
					srvrConnectionString = "Data Source=" + text13.Trim() + ";Initial Catalog=" + text14.Trim() + ";User ID=" + text15.Trim() + ";password=" + text16.Trim();
				}
				scnn.ConnectionString = srvrConnectionString;
				try
				{
					lcnn.Open();
					scnn.Open();
					if (lcnn.State == ConnectionState.Open)
					{
						lcnn.Close();
					}
					if (scnn.State == ConnectionState.Open)
					{
						scnn.Close();
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
				lcmd.Connection = lcnn;
				lcmd.CommandType = CommandType.StoredProcedure;
				lcmd.CommandTimeout = 360;
				scmd.Connection = scnn;
				scmd.CommandType = CommandType.StoredProcedure;
				scmd.CommandTimeout = 360;
			}
		}
	}
	[DesignerGenerated]
	public class Form1 : Form
	{
		private static List<WeakReference> __ENCList = new List<WeakReference>();

		private IContainer components;

		private bool _dataUploadErrorCheck;

		private bool workWithServer;

		private List<string> ListString;

		public Form1()
		{
			((Form)this).Load += Form1_Load;
			__ENCAddToList(this);
			_dataUploadErrorCheck = false;
			workWithServer = false;
			ListString = new List<string>();
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
			((Form)this).Text = "Form1";
			((Control)this).ResumeLayout(false);
		}

		private bool CheckForInternetConnectionDLL()
		{
			bool result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com");
				httpWebRequest.Timeout = 3000;
				httpWebRequest.Method = "HEAD";
				using HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				result = httpWebResponse.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public void Sale_Add2_FL()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Expected O, but got Unknown
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Expected O, but got Unknown
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Sales_Add_FL_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtsalesbillmain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtsalesbilldetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text7 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtsales_tax";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text8 = stringWriter6.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Sales_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text3);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text4);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text5);
					val4.Parameters.AddWithValue("@salesbillmain", (object)text6);
					val4.Parameters.AddWithValue("@salesbilldetail", (object)text7);
					val4.Parameters.AddWithValue("@sales_tax", (object)text8);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text9 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text9, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtSale";
						StringWriter stringWriter7 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter7, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text10 = stringWriter7.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateSaleStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtSale", (object)text10);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading Sale data ", (MsgBoxStyle)0, (object)null);
					}
				}
				Sale_Add2_CS();
				Sale_Add2_NAI();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading Sale data \r\n\r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Sale_Add2_NAI()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Expected O, but got Unknown
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Expected O, but got Unknown
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Sales_Add_NAI_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtsalesbillmain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtsalesbilldetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text7 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtsales_tax";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text8 = stringWriter6.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Sales_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text3);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text4);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text5);
					val4.Parameters.AddWithValue("@salesbillmain", (object)text6);
					val4.Parameters.AddWithValue("@salesbilldetail", (object)text7);
					val4.Parameters.AddWithValue("@sales_tax", (object)text8);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text9 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text9, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtSale";
						StringWriter stringWriter7 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter7, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text10 = stringWriter7.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateSaleStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtSale", (object)text10);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading Sale data ", (MsgBoxStyle)0, (object)null);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading Sale data \r\n\r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Sale_Add2_CS()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Expected O, but got Unknown
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Expected O, but got Unknown
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Sales_Add_CS_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtsalesbillmain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtsalesbilldetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text7 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtsales_tax";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text8 = stringWriter6.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Sales_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text3);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text4);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text5);
					val4.Parameters.AddWithValue("@salesbillmain", (object)text6);
					val4.Parameters.AddWithValue("@salesbilldetail", (object)text7);
					val4.Parameters.AddWithValue("@sales_tax", (object)text8);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text9 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text9, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtSale";
						StringWriter stringWriter7 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter7, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text10 = stringWriter7.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateSaleStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtSale", (object)text10);
						val6.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Purchase_Add2()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Expected O, but got Unknown
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Expected O, but got Unknown
			//IL_0418: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e2: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Purchase_Add_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtPurchaseMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtpurchasedetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtPurchaseTaxDetail";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtTransactionMain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtTransactiondetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text7 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtTransactionMatch";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text8 = stringWriter6.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Purchase_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@purchasemain", (object)text3);
					val4.Parameters.AddWithValue("@purchasedetail", (object)text4);
					val4.Parameters.AddWithValue("@PurchaseTaxDetail", (object)text5);
					val4.Parameters.AddWithValue("@TransactionMain", (object)text6);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text7);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text8);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text9 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text9, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtPur";
						StringWriter stringWriter7 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter7, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text10 = stringWriter7.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdatePurchaseStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtPur", (object)text10);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading purchase data ", (MsgBoxStyle)0, (object)null);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading purchase data \r\n\r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Transfer_Add2()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Expected O, but got Unknown
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Transfer_Add_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtstk_transfer_main";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtstk_transfer_detail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Transfer_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@stk_transfer_main", (object)text3);
					val4.Parameters.AddWithValue("@stk_transfer_detail", (object)text4);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text5 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text5, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtStkTrnsfr";
						StringWriter stringWriter3 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text6 = stringWriter3.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateStockTransferStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtStkTrnsfr", (object)text6);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading transfer data ", (MsgBoxStyle)0, (object)null);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading transfer data \r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Breakage_Add2()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Expected O, but got Unknown
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Expected O, but got Unknown
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Breakage_Add_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtbreakagemain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtbreakagedetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Breakage_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@breakagemain", (object)text3);
					val4.Parameters.AddWithValue("@breakagedetail", (object)text4);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text5 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text5, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtBrkg";
						StringWriter stringWriter3 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text6 = stringWriter3.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateBreakageStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtBrkg", (object)text6);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading breakage data ", (MsgBoxStyle)0, (object)null);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading breakage data \r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void PAYREC_Add()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0357: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Expected O, but got Unknown
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_PAYREC_ADD_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter3.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_PAYREC_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text3);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text4);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text5);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text6 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text6, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtPayRec";
						StringWriter stringWriter4 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text7 = stringWriter4.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdatePayRecStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtPayRec", (object)text7);
						val6.ExecuteNonQuery();
					}
					else
					{
						_dataUploadErrorCheck = true;
						Interaction.MsgBox((object)"Error in uploading data ", (MsgBoxStyle)0, (object)null);
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading data \r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Trndelete2()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Trndelete_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "DTTrndelete";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_delete_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@Trndelete", (object)text3);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text4 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text4, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtTrnid";
						StringWriter stringWriter2 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text5 = stringWriter2.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateDeleteStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtTrnid", (object)text5);
						val6.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading data \r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		public void Trnid_Add2()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Expected O, but got Unknown
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			SqlConnection val = new SqlConnection();
			SqlConnection val2 = new SqlConnection();
			try
			{
				if (!CheckForInternetConnectionDLL())
				{
					return;
				}
				cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
				cls_getconnFrAutoUpdate2.getcon1();
				string text = default(string);
				string text2 = default(string);
				if (!workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.lclConnectionString;
					text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
				}
				else if (workWithServer)
				{
					text = cls_getconnFrAutoUpdate2.srvrConnectionString;
					text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
				}
				val = new SqlConnection(text);
				val2 = new SqlConnection(text2);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Trnid_Add_NEW ", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dttrnidmst";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter.ToString();
				if (dataSet.Tables[0].Rows.Count != 0 && CheckForInternetConnectionDLL())
				{
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					SqlCommand val4 = new SqlCommand("SP_Trnid_Upload_NEW", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@trnidmst", (object)text3);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text4 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text4, "Y", false) == 0)
					{
						dataSet2.Tables[1].TableName = "dtTrnidMst";
						StringWriter stringWriter2 = new StringWriter();
						dataSet2.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
						string text5 = stringWriter2.ToString();
						SqlCommand val6 = new SqlCommand("SP_UpdateTrnidMstStatus", val);
						val6.CommandType = CommandType.StoredProcedure;
						val6.Parameters.AddWithValue("@dtTrnidMst", (object)text5);
						val6.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_dataUploadErrorCheck = true;
				Interaction.MsgBox((object)("Error in uploading Trnid data \r\n\r\n" + ex2.ToString()), (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				((Component)(object)val).Dispose();
				if (val2.State == ConnectionState.Open)
				{
					val2.Close();
				}
				((Component)(object)val2).Dispose();
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				workWithServer = false;
				Trndelete2();
				Sale_Add2_FL();
				Purchase_Add2();
				Transfer_Add2();
				Breakage_Add2();
				PAYREC_Add();
				Trnid_Add2();
				UploadMasters();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			finally
			{
				((Form)this).Close();
			}
		}

		private void UploadMasters()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c7e: Expected O, but got Unknown
			//IL_5ece: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ed5: Expected O, but got Unknown
			//IL_0eed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ef4: Expected O, but got Unknown
			//IL_6444: Unknown result type (might be due to invalid IL or missing references)
			//IL_644b: Expected O, but got Unknown
			//IL_1106: Unknown result type (might be due to invalid IL or missing references)
			//IL_110d: Expected O, but got Unknown
			//IL_6714: Unknown result type (might be due to invalid IL or missing references)
			//IL_671b: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_137c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1383: Expected O, but got Unknown
			//IL_6985: Unknown result type (might be due to invalid IL or missing references)
			//IL_698c: Expected O, but got Unknown
			//IL_1563: Unknown result type (might be due to invalid IL or missing references)
			//IL_156a: Expected O, but got Unknown
			//IL_6bf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_6bfd: Expected O, but got Unknown
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Expected O, but got Unknown
			//IL_177c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1783: Expected O, but got Unknown
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
			//IL_1c86: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c8d: Expected O, but got Unknown
			//IL_0564: Unknown result type (might be due to invalid IL or missing references)
			//IL_056b: Expected O, but got Unknown
			//IL_2276: Unknown result type (might be due to invalid IL or missing references)
			//IL_227d: Expected O, but got Unknown
			//IL_71eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2498: Unknown result type (might be due to invalid IL or missing references)
			//IL_249f: Expected O, but got Unknown
			//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7c: Expected O, but got Unknown
			//IL_2687: Unknown result type (might be due to invalid IL or missing references)
			//IL_268e: Expected O, but got Unknown
			//IL_2ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ab7: Expected O, but got Unknown
			//IL_32ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_32f4: Expected O, but got Unknown
			//IL_36b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_36bf: Expected O, but got Unknown
			//IL_392b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3932: Expected O, but got Unknown
			//IL_6ef6: Unknown result type (might be due to invalid IL or missing references)
			//IL_6efd: Expected O, but got Unknown
			//IL_3c53: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c5a: Expected O, but got Unknown
			//IL_4d24: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d2b: Expected O, but got Unknown
			//IL_5006: Unknown result type (might be due to invalid IL or missing references)
			//IL_500d: Expected O, but got Unknown
			//IL_7152: Unknown result type (might be due to invalid IL or missing references)
			//IL_7159: Expected O, but got Unknown
			//IL_53e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_53f0: Expected O, but got Unknown
			//IL_5721: Unknown result type (might be due to invalid IL or missing references)
			//IL_5728: Expected O, but got Unknown
			//IL_5d98: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d9f: Expected O, but got Unknown
			DataSet dataSet = new DataSet();
			DataSet dataSet2 = new DataSet();
			DataSet dataSet3 = new DataSet();
			SqlDataAdapter val = new SqlDataAdapter();
			SqlConnection val2 = new SqlConnection();
			SqlConnection val3 = new SqlConnection();
			checked
			{
				try
				{
					if (!CheckForInternetConnectionDLL())
					{
						return;
					}
					cls_getconnFrAutoUpdate cls_getconnFrAutoUpdate2 = new cls_getconnFrAutoUpdate();
					cls_getconnFrAutoUpdate2.getcon1();
					string text = default(string);
					string text2 = default(string);
					if (!workWithServer)
					{
						text = cls_getconnFrAutoUpdate2.lclConnectionString;
						text2 = cls_getconnFrAutoUpdate2.srvrConnectionString;
					}
					else if (workWithServer)
					{
						text = cls_getconnFrAutoUpdate2.srvrConnectionString;
						text2 = cls_getconnFrAutoUpdate2.lclConnectionString;
					}
					val2 = new SqlConnection(text);
					val3 = new SqlConnection(text2);
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					if (val2.State == ConnectionState.Closed)
					{
						val2.Open();
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from itemrateinfo where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE itemrateinfo NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE salesbillmain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num = dataSet.Tables[0].Rows.Count - 1;
						int num2 = 0;
						while (true)
						{
							int num3 = num2;
							int num4 = num;
							if (num3 > num4)
							{
								break;
							}
							item = "Delete from itemrateinfo where CompanyCode='" + dataSet.Tables[0].Rows[num2][2].ToString() + "' and ratecode= '" + dataSet.Tables[0].Rows[num2][0].ToString() + "' ";
							ListString.Add(item);
							item = "insert into itemrateinfo (ratecode,ratename,companycode) values ('" + dataSet.Tables[0].Rows[num2][0].ToString() + "','" + dataSet.Tables[0].Rows[num2][1].ToString() + "','" + dataSet.Tables[0].Rows[num2][2].ToString() + "') ";
							ListString.Add(item);
							num2++;
						}
						item = "ALTER TABLE salesbillmain CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemrateinfo CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from itemratemst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE itemratemst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num5 = dataSet.Tables[0].Rows.Count - 1;
						int num6 = 0;
						while (true)
						{
							int num7 = num6;
							int num4 = num5;
							if (num7 > num4)
							{
								break;
							}
							item = "Delete from itemratemst where CompanyCode='" + dataSet.Tables[0].Rows[num6][0].ToString() + "' and ratecode= '" + dataSet.Tables[0].Rows[num6][1].ToString() + "' and itemcode='" + dataSet.Tables[0].Rows[num6][2].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into itemratemst (companycode,ratecode,itemcode,salesrate,itemType) values ('" + dataSet.Tables[0].Rows[num6][0].ToString(), "','"), dataSet.Tables[0].Rows[num6][1].ToString()), "','"), dataSet.Tables[0].Rows[num6][2].ToString()), "','"), dataSet.Tables[0].Rows[num6][3]), (object)"','"), (object)dataSet.Tables[0].Rows[num6][4].ToString()), (object)"') "));
							ListString.Add(item);
							num6++;
						}
						item = "ALTER TABLE itemratemst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from itemmst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num8 = dataSet.Tables[0].Rows.Count - 1;
						int num9 = 0;
						while (true)
						{
							int num10 = num9;
							int num4 = num8;
							if (num10 > num4)
							{
								break;
							}
							item = "Delete from itemmst where itemcode='" + dataSet.Tables[0].Rows[num9][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num9][8].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into itemmst (groupcode,itemcode,itemname,categoryCode,kflcode,packing,strengthname,ml,companycode,barcode,salesrate,purchaserate,purchase_discount_amount,purchase_salestaxonmrp_amount,purchase_taxscheme_amount,min_qnty,barcode2,itemImgUrl,barcode3,shortCode) values ('" + dataSet.Tables[0].Rows[num9][0].ToString(), "','"), dataSet.Tables[0].Rows[num9][1].ToString()), "','"), dataSet.Tables[0].Rows[num9][2].ToString()), "','"), dataSet.Tables[0].Rows[num9][3].ToString()), "','"), dataSet.Tables[0].Rows[num9][4].ToString()), "','"), dataSet.Tables[0].Rows[num9][5]), (object)"','"), (object)dataSet.Tables[0].Rows[num9][6].ToString()), (object)"','"), dataSet.Tables[0].Rows[num9][7]), (object)"','"), (object)dataSet.Tables[0].Rows[num9][8].ToString()), (object)"','"), dataSet.Tables[0].Rows[num9][9]), (object)"','"), dataSet.Tables[0].Rows[num9][10]), (object)"','"), dataSet.Tables[0].Rows[num9][11]), (object)"','"), dataSet.Tables[0].Rows[num9][12]), (object)"','"), dataSet.Tables[0].Rows[num9][13]), (object)"','"), dataSet.Tables[0].Rows[num9][14]), (object)"','"), dataSet.Tables[0].Rows[num9][15]), (object)"','"), (object)dataSet.Tables[0].Rows[num9][16].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num9][17].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num9][18].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num9][19].ToString()), (object)"') "));
							ListString.Add(item);
							num9++;
						}
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					try
					{
						dataSet = new DataSet();
						val = new SqlDataAdapter("select * from ItemMerging where Sync = 'N' ", val2);
						((DbDataAdapter)(object)val).Fill(dataSet);
						if (dataSet.Tables[0].Rows.Count != 0)
						{
							string item = "ALTER TABLE ItemMerging NOCHECK CONSTRAINT ALL";
							ListString.Add(item);
							int num11 = dataSet.Tables[0].Rows.Count - 1;
							int num12 = 0;
							while (true)
							{
								int num13 = num12;
								int num4 = num11;
								if (num13 > num4)
								{
									break;
								}
								item = "Delete from ItemMerging where itemcode='" + dataSet.Tables[0].Rows[num12][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num12][0].ToString() + "' ";
								ListString.Add(item);
								item = "insert into ItemMerging (companycode,itemcode,merging) values ('" + dataSet.Tables[0].Rows[num12][0].ToString() + "','" + dataSet.Tables[0].Rows[num12][1].ToString() + "','" + dataSet.Tables[0].Rows[num12][2].ToString() + "') ";
								ListString.Add(item);
								num12++;
							}
							item = "ALTER TABLE ItemMerging CHECK CONSTRAINT ALL";
							ListString.Add(item);
						}
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from categorymst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE categorymst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE ManufacturerMaster NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num14 = dataSet.Tables[0].Rows.Count - 1;
						int num15 = 0;
						while (true)
						{
							int num16 = num15;
							int num4 = num14;
							if (num16 > num4)
							{
								break;
							}
							item = "Delete from categorymst where categoryCode='" + dataSet.Tables[0].Rows[num15][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num15][0].ToString() + "' ";
							ListString.Add(item);
							item = "insert into categorymst (companycode,categorycode,categoryname,ManufacturerCode) values ('" + dataSet.Tables[0].Rows[num15][0].ToString() + "','" + dataSet.Tables[0].Rows[num15][1].ToString() + "','" + dataSet.Tables[0].Rows[num15][2].ToString() + "','" + dataSet.Tables[0].Rows[num15][3].ToString() + "') ";
							ListString.Add(item);
							num15++;
						}
						item = "ALTER TABLE ManufacturerMaster CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE categorymst CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from groupmst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE groupmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num17 = dataSet.Tables[0].Rows.Count - 1;
						int num18 = 0;
						while (true)
						{
							int num19 = num18;
							int num4 = num17;
							if (num19 > num4)
							{
								break;
							}
							item = "Delete from groupmst where groupcode='" + dataSet.Tables[0].Rows[num18][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num18][0].ToString() + "' ";
							ListString.Add(item);
							item = "insert into groupmst (companycode,groupcode,groupname) values ('" + dataSet.Tables[0].Rows[num18][0].ToString() + "','" + dataSet.Tables[0].Rows[num18][1].ToString() + "','" + dataSet.Tables[0].Rows[num18][2].ToString() + "') ";
							ListString.Add(item);
							num18++;
						}
						item = "ALTER TABLE groupmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from   ManufacturerMaster where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE ManufacturerMaster NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE categorymst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num20 = dataSet.Tables[0].Rows.Count - 1;
						int num21 = 0;
						while (true)
						{
							int num22 = num21;
							int num4 = num20;
							if (num22 > num4)
							{
								break;
							}
							item = "Delete from ManufacturerMaster where manufacturercode ='" + dataSet.Tables[0].Rows[num21][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num21][0].ToString() + "' ";
							ListString.Add(item);
							item = "insert into ManufacturerMaster (Companycode,ManufacturerCode,ManufacturerName,ManufacturerDescription) values ('" + dataSet.Tables[0].Rows[num21][0].ToString() + "','" + dataSet.Tables[0].Rows[num21][1].ToString() + "','" + dataSet.Tables[0].Rows[num21][2].ToString() + "','" + dataSet.Tables[0].Rows[num21][3].ToString() + "') ";
							ListString.Add(item);
							num21++;
						}
						item = "ALTER TABLE ManufacturerMaster CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE categorymst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from   strength where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE strength NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num23 = dataSet.Tables[0].Rows.Count - 1;
						int num24 = 0;
						while (true)
						{
							int num25 = num24;
							int num4 = num23;
							if (num25 > num4)
							{
								break;
							}
							item = "Delete from strength where strengthname ='" + dataSet.Tables[0].Rows[num24][0].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num24][1].ToString() + "' ";
							ListString.Add(item);
							item = "insert into strength (strengthname,Companycode) values ('" + dataSet.Tables[0].Rows[num24][0].ToString() + "','" + dataSet.Tables[0].Rows[num24][1].ToString() + "') ";
							ListString.Add(item);
							num24++;
						}
						item = "ALTER TABLE strength CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from kflmst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE kflmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num26 = dataSet.Tables[0].Rows.Count - 1;
						int num27 = 0;
						while (true)
						{
							int num28 = num27;
							int num4 = num26;
							if (num28 > num4)
							{
								break;
							}
							item = "Delete from kflmst where kflcode='" + dataSet.Tables[0].Rows[num27][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num27][0].ToString() + "' ";
							ListString.Add(item);
							item = "insert into kflmst (companycode,kflcode,kflname) values ('" + dataSet.Tables[0].Rows[num27][0].ToString() + "','" + dataSet.Tables[0].Rows[num27][1].ToString() + "','" + dataSet.Tables[0].Rows[num27][2].ToString() + "') ";
							ListString.Add(item);
							num27++;
						}
						item = "ALTER TABLE kflmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE itemmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from openingstockmst  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE openingstockmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num29 = dataSet.Tables[0].Rows.Count - 1;
						int num30 = 0;
						while (true)
						{
							int num31 = num30;
							int num4 = num29;
							if (num31 > num4)
							{
								break;
							}
							DateTime dateTime = Conversions.ToDate(Convert.ToDateTime(RuntimeHelpers.GetObjectValue(dataSet.Tables[0].Rows[num30][7])).ToString("yyyy-MM-dd hh:mm:ss tt"));
							item = "Delete from openingStockDetail where shopcode='" + dataSet.Tables[0].Rows[num30][0].ToString() + "' and companyCode= '" + dataSet.Tables[0].Rows[num30][1].ToString() + "' and itemCode= '" + dataSet.Tables[0].Rows[num30][2].ToString() + "' ";
							ListString.Add(item);
							item = "Delete from openingstockmst where shopcode='" + dataSet.Tables[0].Rows[num30][0].ToString() + "' and companyCode= '" + dataSet.Tables[0].Rows[num30][1].ToString() + "' and itemCode= '" + dataSet.Tables[0].Rows[num30][2].ToString() + "' ";
							ListString.Add(item);
							item = "INSERT INTO openingstockmst (shopcode,companyCode,itemCode, loose, box, qnty, yearcode,trndate, purchaseRate, totalAmount,purchaseRate_WithTax,MRP,AvgRate) values ('" + dataSet.Tables[0].Rows[num30][0].ToString() + "','" + dataSet.Tables[0].Rows[num30][1].ToString() + "','" + dataSet.Tables[0].Rows[num30][2].ToString() + "','" + dataSet.Tables[0].Rows[num30][3].ToString() + "','" + dataSet.Tables[0].Rows[num30][4].ToString() + "','" + dataSet.Tables[0].Rows[num30][5].ToString() + "','" + dataSet.Tables[0].Rows[num30][6].ToString() + "','" + Conversions.ToString(dateTime) + "','" + dataSet.Tables[0].Rows[num30][8].ToString() + "','" + dataSet.Tables[0].Rows[num30][9].ToString() + "','" + dataSet.Tables[0].Rows[num30][11].ToString() + "','" + dataSet.Tables[0].Rows[num30][12].ToString() + "','" + dataSet.Tables[0].Rows[num30][13].ToString() + "') ";
							ListString.Add(item);
							num30++;
						}
						item = "ALTER TABLE openingstockmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from openingStockDetail  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE openingStockDetail NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num32 = dataSet.Tables[0].Rows.Count - 1;
						int num33 = 0;
						while (true)
						{
							int num34 = num33;
							int num4 = num32;
							if (num34 > num4)
							{
								break;
							}
							DateTime dateTime = Conversions.ToDate(Convert.ToDateTime(RuntimeHelpers.GetObjectValue(dataSet.Tables[0].Rows[num33][2])).ToString("yyyy-MM-dd hh:mm:ss tt"));
							item = "Delete from openingStockDetail where companyCode='" + dataSet.Tables[0].Rows[num33][0].ToString() + "' and yearcode= '" + dataSet.Tables[0].Rows[num33][1].ToString() + "' and shopcode= '" + dataSet.Tables[0].Rows[num33][3].ToString() + "' and itemCode= '" + dataSet.Tables[0].Rows[num33][5].ToString() + "' AND MRP =  '" + dataSet.Tables[0].Rows[num33][13].ToString() + "' AND purchaseTrnno = '" + dataSet.Tables[0].Rows[num33][17].ToString() + "' AND batchNo =  '" + dataSet.Tables[0].Rows[num33][4].ToString() + "'";
							ListString.Add(item);
							item = "INSERT INTO openingStockDetail (companyCode,yearcode,trndate,shopcode, batchNo, itemCode, loose, box, qnty,  purchaseRate, totalAmount,purchaseRate_WithTax,MRP,AvgRate,saleqty,closing,purchaseTrnno) values ('" + dataSet.Tables[0].Rows[num33][0].ToString() + "','" + dataSet.Tables[0].Rows[num33][1].ToString() + "','" + Conversions.ToString(dateTime) + "','" + dataSet.Tables[0].Rows[num33][3].ToString() + "','" + dataSet.Tables[0].Rows[num33][4].ToString() + "','" + dataSet.Tables[0].Rows[num33][5].ToString() + "','" + dataSet.Tables[0].Rows[num33][6].ToString() + "','" + dataSet.Tables[0].Rows[num33][7].ToString() + "','" + dataSet.Tables[0].Rows[num33][8].ToString() + "','" + dataSet.Tables[0].Rows[num33][9].ToString() + "','" + dataSet.Tables[0].Rows[num33][10].ToString() + "','" + dataSet.Tables[0].Rows[num33][12].ToString() + "','" + dataSet.Tables[0].Rows[num33][13].ToString() + "','" + dataSet.Tables[0].Rows[num33][14].ToString() + "','" + dataSet.Tables[0].Rows[num33][15].ToString() + "','" + dataSet.Tables[0].Rows[num33][16].ToString() + "','" + dataSet.Tables[0].Rows[num33][17].ToString() + "') ";
							ListString.Add(item);
							num33++;
						}
						item = "ALTER TABLE openingStockDetail CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from reportTaggingDetail  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE reportTaggingDetail NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num35 = dataSet.Tables[0].Rows.Count - 1;
						int num36 = 0;
						while (true)
						{
							int num37 = num36;
							int num4 = num35;
							if (num37 > num4)
							{
								break;
							}
							item = "Delete from reportTaggingDetail where companycode = '" + dataSet.Tables[0].Rows[num36][2].ToString() + "' and reportCategory = '" + dataSet.Tables[0].Rows[num36][1].ToString() + "' ";
							ListString.Add(item);
							item = "insert into reportTaggingDetail (reportname,ReportCategory,groupname,companycode) values ('" + dataSet.Tables[0].Rows[num36][0].ToString() + "','" + dataSet.Tables[0].Rows[num36][1].ToString() + "','" + dataSet.Tables[0].Rows[num36][2].ToString() + "','" + dataSet.Tables[0].Rows[num36][3].ToString() + "')";
							ListString.Add(item);
							num36++;
						}
						item = "ALTER TABLE reportTaggingDetail CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from reportTaggingMain  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE reportTaggingMain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num38 = dataSet.Tables[0].Rows.Count - 1;
						int num39 = 0;
						while (true)
						{
							int num40 = num39;
							int num4 = num38;
							if (num40 > num4)
							{
								break;
							}
							item = "Delete from reportTaggingMain where companycode = '" + dataSet.Tables[0].Rows[num39][2].ToString() + "' and reportCategory = '" + dataSet.Tables[0].Rows[num39][1].ToString() + "' ";
							ListString.Add(item);
							item = "insert into reportTaggingmain (reportname,ReportCategory,companycode) values ('" + dataSet.Tables[0].Rows[num39][0].ToString() + "','" + dataSet.Tables[0].Rows[num39][1].ToString() + "','" + dataSet.Tables[0].Rows[num39][2].ToString() + "')";
							ListString.Add(item);
							num39++;
						}
						item = "ALTER TABLE reportTaggingMain CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from taxdetail  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE taxdetail NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE PurchaseTaxDetail NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num41 = dataSet.Tables[0].Rows.Count - 1;
						int num42 = 0;
						while (true)
						{
							int num43 = num42;
							int num4 = num41;
							if (num43 > num4)
							{
								break;
							}
							item = "Delete from taxdetail where SchemeCode = '" + dataSet.Tables[0].Rows[num42][1].ToString() + "' ";
							ListString.Add(item);
							num42++;
						}
						int num44 = dataSet.Tables[0].Rows.Count - 1;
						int num45 = 0;
						while (true)
						{
							int num46 = num45;
							int num4 = num44;
							if (num46 > num4)
							{
								break;
							}
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into taxdetail(companycode,schemecode,schemename,srno,taxcode,taxname,ledcode,sig,taxrate,taxamount,calc_on,after_discount,after_saletax,base) values('" + dataSet.Tables[0].Rows[num45][0].ToString(), "','"), dataSet.Tables[0].Rows[num45][1].ToString()), "','"), dataSet.Tables[0].Rows[num45][2].ToString()), "','"), dataSet.Tables[0].Rows[num45][3].ToString()), "','"), dataSet.Tables[0].Rows[num45][4].ToString()), "','"), dataSet.Tables[0].Rows[num45][5].ToString()), "','"), dataSet.Tables[0].Rows[num45][6].ToString()), "','"), dataSet.Tables[0].Rows[num45][7].ToString()), "','"), dataSet.Tables[0].Rows[num45][8].ToString()), "','"), dataSet.Tables[0].Rows[num45][9]), (object)"','"), (object)dataSet.Tables[0].Rows[num45][10].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num45][11].ToString()), (object)"','"), (object)Convert.ToInt16(RuntimeHelpers.GetObjectValue(dataSet.Tables[0].Rows[num45][12]))), (object)"','"), (object)Convert.ToInt16(RuntimeHelpers.GetObjectValue(dataSet.Tables[0].Rows[num45][13]))), (object)"')"));
							ListString.Add(item);
							num45++;
						}
						item = "ALTER TABLE PurchaseTaxDetail CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE taxdetail CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from masteraccountsLedger where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE masteraccountsLedger NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE salesbillmain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE purchasemain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num47 = dataSet.Tables[0].Rows.Count - 1;
						int num48 = 0;
						while (true)
						{
							int num49 = num48;
							int num4 = num47;
							if (num49 > num4)
							{
								break;
							}
							item = "Delete from MasterAccountsLedger where CompanyCode='" + dataSet.Tables[0].Rows[num48][0].ToString() + "'  and ledgerCode= '" + dataSet.Tables[0].Rows[num48][2].ToString() + "' ";
							ListString.Add(item);
							item = "INSERT INTO MasterAccountsLedger(companyCode, AccGroupCode, ledgerCode, ledgerName, contactPerson, contactNo, address1, address2, city, district, state, pin, phn, faxNo, email, webSite, vat, pan, ledgerNameAlias, CreditEffects, LockAmount, LockEffect, Sequence, Hidden,ratecode,BankName,BranchName,AccountNo,IfcCode,SalesRateName,SalesRatecode,Areacode)  values ('" + dataSet.Tables[0].Rows[num48][0].ToString() + "','" + dataSet.Tables[0].Rows[num48][1].ToString() + "','" + dataSet.Tables[0].Rows[num48][2].ToString() + "','" + dataSet.Tables[0].Rows[num48][3].ToString() + "','" + dataSet.Tables[0].Rows[num48][4].ToString() + "','" + dataSet.Tables[0].Rows[num48][5].ToString() + "','" + dataSet.Tables[0].Rows[num48][6].ToString() + "','" + dataSet.Tables[0].Rows[num48][7].ToString() + "','" + dataSet.Tables[0].Rows[num48][8].ToString() + "','" + dataSet.Tables[0].Rows[num48][9].ToString() + "','" + dataSet.Tables[0].Rows[num48][10].ToString() + "','" + dataSet.Tables[0].Rows[num48][11].ToString() + "','" + dataSet.Tables[0].Rows[num48][12].ToString() + "','" + dataSet.Tables[0].Rows[num48][13].ToString() + "','" + dataSet.Tables[0].Rows[num48][14].ToString() + "','" + dataSet.Tables[0].Rows[num48][15].ToString() + "','" + dataSet.Tables[0].Rows[num48][16].ToString() + "','" + dataSet.Tables[0].Rows[num48][17].ToString() + "','" + dataSet.Tables[0].Rows[num48][18].ToString() + "','" + dataSet.Tables[0].Rows[num48][19].ToString() + "', '" + dataSet.Tables[0].Rows[num48][20].ToString() + "','" + dataSet.Tables[0].Rows[num48][21].ToString() + "','" + dataSet.Tables[0].Rows[num48][22].ToString() + "','" + dataSet.Tables[0].Rows[num48][23].ToString() + "','" + dataSet.Tables[0].Rows[num48][24].ToString() + "','" + dataSet.Tables[0].Rows[num48][25].ToString() + "','" + dataSet.Tables[0].Rows[num48][26].ToString() + "','" + dataSet.Tables[0].Rows[num48][27].ToString() + "','" + dataSet.Tables[0].Rows[num48][28].ToString() + "','" + dataSet.Tables[0].Rows[num48][29].ToString() + "','" + dataSet.Tables[0].Rows[num48][30].ToString() + "','" + dataSet.Tables[0].Rows[num48][31].ToString() + "') ";
							ListString.Add(item);
							num48++;
						}
						item = "ALTER TABLE purchasemain CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE salesbillmain CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE masteraccountsLedger CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from Masteraccountsgroups where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE Masteraccountsgroups NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num50 = dataSet.Tables[0].Rows.Count - 1;
						int num51 = 0;
						while (true)
						{
							int num52 = num51;
							int num4 = num50;
							if (num52 > num4)
							{
								break;
							}
							item = "Delete from MasterAccountsGroups where CompanyCode='" + dataSet.Tables[0].Rows[num51][0].ToString() + "'  and AccGroupCode= '" + dataSet.Tables[0].Rows[num51][2].ToString() + "' ";
							ListString.Add(item);
							item = "INSERT INTO MasterAccountsGroups (companyCode, AccMainCode, AccGroupCode, AccGroupName, ShortCode, PrimaryGroup, UnderAccGroupCode, PLEffect, CreditEffects, AccGroupAlias, Sequence, Hidden) values ('" + dataSet.Tables[0].Rows[num51][0].ToString() + "','" + dataSet.Tables[0].Rows[num51][1].ToString() + "','" + dataSet.Tables[0].Rows[num51][2].ToString() + "','" + dataSet.Tables[0].Rows[num51][3].ToString() + "','" + dataSet.Tables[0].Rows[num51][4].ToString() + "','" + dataSet.Tables[0].Rows[num51][5].ToString() + "','" + dataSet.Tables[0].Rows[num51][6].ToString() + "','" + dataSet.Tables[0].Rows[num51][7].ToString() + "','" + dataSet.Tables[0].Rows[num51][8].ToString() + "','" + dataSet.Tables[0].Rows[num51][9].ToString() + "','" + dataSet.Tables[0].Rows[num51][10].ToString() + "','" + dataSet.Tables[0].Rows[num51][11].ToString() + "') ";
							ListString.Add(item);
							num51++;
						}
						item = "ALTER TABLE Masteraccountsgroups CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from masteraccountsmain where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE masteraccountsmain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num53 = dataSet.Tables[0].Rows.Count - 1;
						int num54 = 0;
						while (true)
						{
							int num55 = num54;
							int num4 = num53;
							if (num55 > num4)
							{
								break;
							}
							item = "Delete from masteraccountsmain where  AccMainCode= '" + dataSet.Tables[0].Rows[num54][1].ToString() + "' ";
							ListString.Add(item);
							item = "INSERT INTO masteraccountsmain (Sequence, AccMainCode, AccMainName, CreditEffects,PLEffect, AccMainAlias, Hidden) values ('" + dataSet.Tables[0].Rows[num54][0].ToString() + "','" + dataSet.Tables[0].Rows[num54][1].ToString() + "','" + dataSet.Tables[0].Rows[num54][2].ToString() + "','" + dataSet.Tables[0].Rows[num54][3].ToString() + "','" + dataSet.Tables[0].Rows[num54][4].ToString() + "','" + dataSet.Tables[0].Rows[num54][5].ToString() + "','" + dataSet.Tables[0].Rows[num54][6].ToString() + "') ";
							ListString.Add(item);
							num54++;
						}
						item = "ALTER TABLE masteraccountsmain CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from masteraccountsopening  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE masteraccountsopening NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num56 = dataSet.Tables[0].Rows.Count - 1;
						int num57 = 0;
						while (true)
						{
							int num58 = num57;
							int num4 = num56;
							if (num58 > num4)
							{
								break;
							}
							item = "Delete from masteraccountsopening where ledgercode = '" + dataSet.Tables[0].Rows[num57][3].ToString() + "' and yearcode = '" + dataSet.Tables[0].Rows[num57][1].ToString() + "' ";
							ListString.Add(item);
							num57++;
						}
						int num59 = dataSet.Tables[0].Rows.Count - 1;
						int num60 = 0;
						while (true)
						{
							int num61 = num60;
							int num4 = num59;
							if (num61 > num4)
							{
								break;
							}
							item = "insert into masteraccountsopening(companyCode, yearCode, restaurantCode,ledgerCode, TrnAmount, TrnEffect, EffectDate,Sync) values('" + dataSet.Tables[0].Rows[num60][0].ToString() + "','" + dataSet.Tables[0].Rows[num60][1].ToString() + "','" + dataSet.Tables[0].Rows[num60][2].ToString() + "','" + dataSet.Tables[0].Rows[num60][3].ToString() + "','" + dataSet.Tables[0].Rows[num60][4].ToString() + "','" + dataSet.Tables[0].Rows[num60][5].ToString() + "','" + dataSet.Tables[0].Rows[num60][6].ToString() + "','" + dataSet.Tables[0].Rows[num60][7].ToString() + "')";
							ListString.Add(item);
							num60++;
						}
						item = "ALTER TABLE masteraccountsopening CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from companyparamtr  where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE companyparamtr NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE salesbillmain NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num62 = dataSet.Tables[0].Rows.Count - 1;
						int num63 = 0;
						while (true)
						{
							int num64 = num63;
							int num4 = num62;
							if (num64 > num4)
							{
								break;
							}
							item = "Delete from companyparamtr where companycode = '" + dataSet.Tables[0].Rows[num63][0].ToString() + "' ";
							ListString.Add(item);
							num63++;
						}
						int num65 = dataSet.Tables[0].Rows.Count - 1;
						int num66 = 0;
						while (true)
						{
							int num67 = num66;
							int num4 = num65;
							if (num67 > num4)
							{
								break;
							}
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)"insert into companyparamtr (companycode,csalebillno,saleaccount_head,bankacc,cashparty,crcashparty,breakacc,saleretrnacc,csalelmt,printbill,printer,purchaseacchd,cashhd,discntacc,defshpgdn,ratecode,expenceacc,rndoffacc,autorefcurrsal,actposmode,bill_footer,bill_footer2,bill_footer3,bill_footer4,on_off_shop_type,back_up_restore_path,print_mode,surcharge_account_head,Trade_discount_head,surcharge_percent,phone_number1,email_id,sender_id,password,zone,screen_time,status_show,saletax_including_free,backup_upload,sms_on,purchase_discount_type,language,message,sales_taxon_mrp,sales_taxon_mrp_rate,mrp_change,sms_password,excisePassword,exciseId,batchSale,negativeSale,batchEdit,sms_billEditDelete,sms_PartySale,sms_creditCardSale,sms_payment,sms_receipt,companyId,manualBatchSale,csLimit,OnOff,printTimeInBill,printCustomerNameInBill,CsStartBillNo,active,path1,UserWiseSaleStatement,savebill,maxdegitFrQty,billFormat,senderSMSid,billLeftMargin,HideCurntSale_Stock,PrntPrevw,isPoleDisplay,poleDisplayPortNo,naiStartBillNo,RestrictBackDateEntry,IsAutoSave,AutoSaveafter) values ('", dataSet.Tables[0].Rows[num66]["companycode"]), (object)"','"), dataSet.Tables[0].Rows[num66]["csalebillno"]), (object)"','"), dataSet.Tables[0].Rows[num66]["saleaccount_head"]), (object)"','"), dataSet.Tables[0].Rows[num66]["bankacc"]), (object)"','"), dataSet.Tables[0].Rows[num66]["cashparty"]), (object)"','"), dataSet.Tables[0].Rows[num66]["crcashparty"]), (object)"','"), dataSet.Tables[0].Rows[num66]["breakacc"]), (object)"','"), dataSet.Tables[0].Rows[num66]["saleretrnacc"]), (object)"','"), dataSet.Tables[0].Rows[num66]["csalelmt"]), (object)"','"), dataSet.Tables[0].Rows[num66]["printbill"]), (object)"','"), dataSet.Tables[0].Rows[num66]["printer"]), (object)"','"), dataSet.Tables[0].Rows[num66]["purchaseacchd"]), (object)"','"), dataSet.Tables[0].Rows[num66]["cashhd"]), (object)"','"), dataSet.Tables[0].Rows[num66]["discntacc"]), (object)"','"), dataSet.Tables[0].Rows[num66]["defshpgdn"]), (object)"','"), dataSet.Tables[0].Rows[num66]["ratecode"]), (object)"','"), dataSet.Tables[0].Rows[num66]["expenceacc"]), (object)"', "), (object)"'"), dataSet.Tables[0].Rows[num66]["rndoffacc"]), (object)"','"), dataSet.Tables[0].Rows[num66]["autorefcurrsal"]), (object)"','"), dataSet.Tables[0].Rows[num66]["actposmode"]), (object)"','"), dataSet.Tables[0].Rows[num66]["bill_footer"]), (object)"','"), dataSet.Tables[0].Rows[num66]["bill_footer2"]), (object)"','"), dataSet.Tables[0].Rows[num66]["bill_footer3"]), (object)"','"), dataSet.Tables[0].Rows[num66]["bill_footer4"]), (object)"','"), dataSet.Tables[0].Rows[num66]["on_off_shop_type"]), (object)"','"), dataSet.Tables[0].Rows[num66]["back_up_restore_path"]), (object)"','"), dataSet.Tables[0].Rows[num66]["print_mode"]), (object)"','"), dataSet.Tables[0].Rows[num66]["surcharge_account_head"]), (object)"','"), dataSet.Tables[0].Rows[num66]["Trade_discount_head"]), (object)"','"), dataSet.Tables[0].Rows[num66]["surcharge_percent"]), (object)"','"), dataSet.Tables[0].Rows[num66]["phone_number1"]), (object)"','"), dataSet.Tables[0].Rows[num66]["email_id"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sender_id"]), (object)"','"), dataSet.Tables[0].Rows[num66]["password"]), (object)"', "), (object)"'"), dataSet.Tables[0].Rows[num66]["zone"]), (object)"','"), dataSet.Tables[0].Rows[num66]["screen_time"]), (object)"','"), dataSet.Tables[0].Rows[num66]["status_show"]), (object)"','"), dataSet.Tables[0].Rows[num66]["saletax_including_free"]), (object)"','"), dataSet.Tables[0].Rows[num66]["backup_upload"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_on"]), (object)"','"), dataSet.Tables[0].Rows[num66]["purchase_discount_type"]), (object)"','"), dataSet.Tables[0].Rows[num66]["language"]), (object)"','"), dataSet.Tables[0].Rows[num66]["message"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sales_taxon_mrp"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sales_taxon_mrp_rate"]), (object)"','"), dataSet.Tables[0].Rows[num66]["mrp_change"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_password"]), (object)"','"), dataSet.Tables[0].Rows[num66]["excisePassword"]), (object)"','"), dataSet.Tables[0].Rows[num66]["exciseId"]), (object)"','"), dataSet.Tables[0].Rows[num66]["batchSale"]), (object)"','"), dataSet.Tables[0].Rows[num66]["negativeSale"]), (object)"', "), (object)"'"), dataSet.Tables[0].Rows[num66]["batchEdit"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_billEditDelete"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_PartySale"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_creditCardSale"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_payment"]), (object)"','"), dataSet.Tables[0].Rows[num66]["sms_receipt"]), (object)"','"), dataSet.Tables[0].Rows[num66]["companyId"]), (object)"','"), dataSet.Tables[0].Rows[num66]["manualBatchSale"]), (object)"','"), dataSet.Tables[0].Rows[num66]["csLimit"]), (object)"','"), dataSet.Tables[0].Rows[num66]["OnOff"]), (object)"','"), dataSet.Tables[0].Rows[num66]["printTimeInBill"]), (object)"','"), dataSet.Tables[0].Rows[num66]["printCustomerNameInBill"]), (object)"','"), dataSet.Tables[0].Rows[num66]["CsStartBillNo"]), (object)"','"), dataSet.Tables[0].Rows[num66]["active"]), (object)"','"), dataSet.Tables[0].Rows[num66]["path1"]), (object)"','"), dataSet.Tables[0].Rows[num66]["UserWiseSaleStatement"]), (object)"','"), dataSet.Tables[0].Rows[num66]["savebill"]), (object)"', "), (object)"'"), dataSet.Tables[0].Rows[num66]["maxdegitFrQty"]), (object)"','"), dataSet.Tables[0].Rows[num66]["billFormat"]), (object)"','"), dataSet.Tables[0].Rows[num66]["senderSMSid"]), (object)"','"), dataSet.Tables[0].Rows[num66]["billLeftMargin"]), (object)"','"), dataSet.Tables[0].Rows[num66]["HideCurntSale_Stock"]), (object)"','"), dataSet.Tables[0].Rows[num66]["PrntPrevw"]), (object)"','"), dataSet.Tables[0].Rows[num66]["isPoleDisplay"]), (object)"','"), dataSet.Tables[0].Rows[num66]["poleDisplayPortNo"]), (object)"','"), dataSet.Tables[0].Rows[num66]["naiStartBillNo"]), (object)"','"), dataSet.Tables[0].Rows[num66]["RestrictBackDateEntry"]), (object)"','"), dataSet.Tables[0].Rows[num66]["IsAutoSave"]), (object)"','"), dataSet.Tables[0].Rows[num66]["AutoSaveafter"]), (object)"') "));
							ListString.Add(item);
							num66++;
						}
						item = "ALTER TABLE salesbillmain CHECK CONSTRAINT ALL";
						ListString.Add(item);
						item = "ALTER TABLE companyparamtr CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from countermst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE countermst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num68 = dataSet.Tables[0].Rows.Count - 1;
						int num69 = 0;
						while (true)
						{
							int num70 = num69;
							int num4 = num68;
							if (num70 > num4)
							{
								break;
							}
							item = "Delete from countermst where countercode='" + dataSet.Tables[0].Rows[num69]["countercode"].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num69]["companycode"].ToString() + "' ";
							ListString.Add(item);
							item = "insert into countermst (countercode,countername,companycode,ledgercode,ledgername,counteropening,IScustomerDisplay)  values ('" + dataSet.Tables[0].Rows[num69]["countercode"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["countername"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["companycode"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["ledgercode"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["ledgername"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["counteropening"].ToString() + "','" + dataSet.Tables[0].Rows[num69]["IScustomerDisplay"].ToString() + "') ";
							ListString.Add(item);
							num69++;
						}
						item = "ALTER TABLE countermst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("SELECT [usercode],[username],[accesslevel],[password],[Sync],[Discount],[isActive],[useractive],[isBlock],[MacName],[isNew],[isUpdate],[saledatasync],[newupdate]  FROM [dbo].[users] where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE users NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num71 = dataSet.Tables[0].Rows.Count - 1;
						int num72 = 0;
						while (true)
						{
							int num73 = num72;
							int num4 = num71;
							if (num73 > num4)
							{
								break;
							}
							item = "Delete from users where usercode='" + dataSet.Tables[0].Rows[num72]["usercode"].ToString() + "' ";
							ListString.Add(item);
							item = "insert into users ( [usercode],[username],[accesslevel],[password],[Discount],[isActive],[useractive],[isBlock],[MacName],[isNew],[isUpdate],[saledatasync],[newupdate])  values ('" + dataSet.Tables[0].Rows[num72]["usercode"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["username"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["accesslevel"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["password"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["Discount"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["isActive"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["useractive"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["isBlock"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["MacName"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["isNew"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["isUpdate"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["saledatasync"].ToString() + "','" + dataSet.Tables[0].Rows[num72]["newupdate"].ToString() + "') ";
							ListString.Add(item);
							num72++;
						}
						item = "ALTER TABLE users CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from offermst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE offermst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num74 = dataSet.Tables[0].Rows.Count - 1;
						int num75 = 0;
						while (true)
						{
							int num76 = num75;
							int num4 = num74;
							if (num76 > num4)
							{
								break;
							}
							item = "Delete from offermst where itemcode='" + dataSet.Tables[0].Rows[num75]["itemcode"].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num75]["companycode"].ToString() + "' ";
							ListString.Add(item);
							item = "insert into offermst (itemcode,itemname,frm_date,to_date,qnty,rate,companycode,blockchk)  values ('" + dataSet.Tables[0].Rows[num75]["itemcode"].ToString() + "','" + dataSet.Tables[0].Rows[num75]["itemname"].ToString() + "','" + dataSet.Tables[0].Rows[num75]["frm_date"].ToString() + "','" + dataSet.Tables[0].Rows[num75]["to_date"].ToString() + "'," + Conversions.ToString(Conversion.Val(dataSet.Tables[0].Rows[num75]["qnty"].ToString())) + "," + Conversions.ToString(Conversion.Val(dataSet.Tables[0].Rows[num75]["rate"].ToString())) + ",'" + dataSet.Tables[0].Rows[num75]["companycode"].ToString() + "'," + Conversions.ToString(Conversion.Val(dataSet.Tables[0].Rows[num75]["blockchk"].ToString())) + ") ";
							ListString.Add(item);
							num75++;
						}
						item = "ALTER TABLE offermst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet2 = new DataSet();
					val = new SqlDataAdapter("select * from customerDetails where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet2);
					if (dataSet2.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE customerDetails NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num77 = dataSet2.Tables[0].Rows.Count - 1;
						int num78 = 0;
						while (true)
						{
							int num79 = num78;
							int num4 = num77;
							if (num79 > num4)
							{
								break;
							}
							item = "Delete from customerDetails where customerCode='" + dataSet2.Tables[0].Rows[num78]["customerCode"].ToString() + "' and companycode= '" + dataSet2.Tables[0].Rows[num78]["companycode"].ToString() + "' ";
							ListString.Add(item);
							item = "insert into customerDetails (companyCode,customerCode,customerName,contactNo,cardNo,address1,address2,city,state,pin,phn,email,sex,dob,aniverseryDate,firstVisitDate,lastVisitDate,visitCount,district,discountRate,userCode,pcName,cust_licence)  values ('" + dataSet2.Tables[0].Rows[num78]["companyCode"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["customerCode"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["customerName"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["contactNo"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["cardNo"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["address1"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["address2"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["city"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["state"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["pin"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["phn"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["email"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["sex"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["dob"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["aniverseryDate"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["firstVisitDate"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["lastVisitDate"].ToString() + "'," + Conversions.ToString(Conversion.Val(dataSet2.Tables[0].Rows[num78]["visitCount"].ToString())) + ",'" + dataSet2.Tables[0].Rows[num78]["district"].ToString() + "'," + Conversions.ToString(Conversion.Val(dataSet2.Tables[0].Rows[num78]["discountRate"].ToString())) + ",'" + dataSet2.Tables[0].Rows[num78]["userCode"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["pcName"].ToString() + "','" + dataSet2.Tables[0].Rows[num78]["cust_licence"].ToString() + "') ";
							ListString.Add(item);
							num78++;
						}
						item = "ALTER TABLE customerDetails CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					try
					{
						dataSet = new DataSet();
						val = new SqlDataAdapter("select * from tblArea", val2);
						((DbDataAdapter)(object)val).Fill(dataSet);
						if (dataSet.Tables[0].Rows.Count != 0)
						{
							string item = "Delete from tblArea";
							ListString.Add(item);
							int num80 = dataSet.Tables[0].Rows.Count - 1;
							int num81 = 0;
							while (true)
							{
								int num82 = num81;
								int num4 = num80;
								if (num82 <= num4)
								{
									item = "insert into tblarea values('" + dataSet.Tables[0].Rows[num81]["AreaCode"].ToString() + "','" + dataSet.Tables[0].Rows[num81]["AreaName"].ToString() + "')";
									ListString.Add(item);
									num81++;
									continue;
								}
								break;
							}
						}
					}
					catch (Exception ex3)
					{
						ProjectData.SetProjectError(ex3);
						Exception ex4 = ex3;
						ProjectData.ClearProjectError();
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from naiItemMaster where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE naiItemMaster NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num83 = dataSet.Tables[0].Rows.Count - 1;
						int num84 = 0;
						while (true)
						{
							int num85 = num84;
							int num4 = num83;
							if (num85 > num4)
							{
								break;
							}
							item = "Delete from naiItemMaster where itemcode='" + dataSet.Tables[0].Rows[num84][3].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num84][0].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into naiItemMaster (companyCode,mainGroupCode,subGroupCode,itemCode,itemName,itemType,unitCode,kflCode,strengthName,salesRate,purchaseRate,purchaseBulkRate,barCode,shortCode,allowBillDiscountTax,taxSchemeCode,isBarItem,minQnty,isCommonItem,userCode,barCode2,barCode3) values ('" + dataSet.Tables[0].Rows[num84][0].ToString(), "','"), dataSet.Tables[0].Rows[num84][1].ToString()), "','"), dataSet.Tables[0].Rows[num84][2].ToString()), "','"), dataSet.Tables[0].Rows[num84][3].ToString()), "','"), dataSet.Tables[0].Rows[num84][4].ToString()), "','"), dataSet.Tables[0].Rows[num84][5]), (object)"','"), (object)dataSet.Tables[0].Rows[num84][6].ToString()), (object)"','"), dataSet.Tables[0].Rows[num84][7]), (object)"','"), (object)dataSet.Tables[0].Rows[num84][8].ToString()), (object)"','"), dataSet.Tables[0].Rows[num84][9]), (object)"','"), dataSet.Tables[0].Rows[num84][10]), (object)"','"), dataSet.Tables[0].Rows[num84][11]), (object)"','"), dataSet.Tables[0].Rows[num84][12]), (object)"','"), dataSet.Tables[0].Rows[num84][13]), (object)"','"), dataSet.Tables[0].Rows[num84][14]), (object)"','"), dataSet.Tables[0].Rows[num84][15]), (object)"','"), (object)dataSet.Tables[0].Rows[num84][16].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num84][17].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num84][18].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num84][19].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num84][20].ToString()), (object)"','"), (object)dataSet.Tables[0].Rows[num84][21].ToString()), (object)"') "));
							ListString.Add(item);
							num84++;
						}
						item = "ALTER TABLE naiItemMaster CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from naiUnitMaster where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE naiUnitMaster NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num86 = dataSet.Tables[0].Rows.Count - 1;
						int num87 = 0;
						while (true)
						{
							int num88 = num87;
							int num4 = num86;
							if (num88 > num4)
							{
								break;
							}
							item = "Delete from naiUnitMaster where unitCode='" + dataSet.Tables[0].Rows[num87][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num87][0].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into naiUnitMaster (companyCode,unitCode,basicUnitName,basicQnty,bulkUnitName,bulkQnty,type,userCode) values ('" + dataSet.Tables[0].Rows[num87][0].ToString(), "','"), dataSet.Tables[0].Rows[num87][1].ToString()), "','"), dataSet.Tables[0].Rows[num87][2].ToString()), "','"), dataSet.Tables[0].Rows[num87][3].ToString()), "','"), dataSet.Tables[0].Rows[num87][4].ToString()), "','"), dataSet.Tables[0].Rows[num87][5]), (object)"','"), (object)dataSet.Tables[0].Rows[num87][6].ToString()), (object)"','"), dataSet.Tables[0].Rows[num87][7]), (object)"') "));
							ListString.Add(item);
							num87++;
						}
						item = "ALTER TABLE naiUnitMaster CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from naiItemMainGroup where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE naiItemMainGroup NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num89 = dataSet.Tables[0].Rows.Count - 1;
						int num90 = 0;
						while (true)
						{
							int num91 = num90;
							int num4 = num89;
							if (num91 > num4)
							{
								break;
							}
							item = "Delete from naiItemMainGroup where mainGroupCode='" + dataSet.Tables[0].Rows[num90][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num90][0].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into naiItemMainGroup (companyCode,mainGroupCode,mainGroupName,isRawMeterial,isBarItem,userCode) values ('" + dataSet.Tables[0].Rows[num90][0].ToString(), "','"), dataSet.Tables[0].Rows[num90][1].ToString()), "','"), dataSet.Tables[0].Rows[num90][2].ToString()), "','"), dataSet.Tables[0].Rows[num90][3].ToString()), "','"), dataSet.Tables[0].Rows[num90][4].ToString()), "','"), dataSet.Tables[0].Rows[num90][5]), (object)"') "));
							ListString.Add(item);
							num90++;
						}
						item = "ALTER TABLE naiItemMainGroup CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from naiItemSubGroup where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE naiItemSubGroup NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num92 = dataSet.Tables[0].Rows.Count - 1;
						int num93 = 0;
						while (true)
						{
							int num94 = num93;
							int num4 = num92;
							if (num94 > num4)
							{
								break;
							}
							item = "Delete from naiItemSubGroup where subGroupCode='" + dataSet.Tables[0].Rows[num93][1].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num93][0].ToString() + "' ";
							ListString.Add(item);
							item = Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject((object)string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("insert into naiItemSubGroup (companyCode,mainGroupCode,subGroupCode,subGroupName,ManufacturerCode,userCode) values ('" + dataSet.Tables[0].Rows[num93][0].ToString(), "','"), dataSet.Tables[0].Rows[num93][1].ToString()), "','"), dataSet.Tables[0].Rows[num93][2].ToString()), "','"), dataSet.Tables[0].Rows[num93][3].ToString()), "','"), dataSet.Tables[0].Rows[num93][4].ToString()), "','"), dataSet.Tables[0].Rows[num93][5]), (object)"') "));
							ListString.Add(item);
							num93++;
						}
						item = "ALTER TABLE naiItemSubGroup CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					dataSet = new DataSet();
					val = new SqlDataAdapter("select * from yearmst where Sync = 'N' ", val2);
					((DbDataAdapter)(object)val).Fill(dataSet);
					if (dataSet.Tables[0].Rows.Count != 0)
					{
						string item = "ALTER TABLE yearmst NOCHECK CONSTRAINT ALL";
						ListString.Add(item);
						int num95 = dataSet.Tables[0].Rows.Count - 1;
						int num96 = 0;
						while (true)
						{
							int num97 = num96;
							int num4 = num95;
							if (num97 > num4)
							{
								break;
							}
							item = "Delete from yearmst where yearcode ='" + dataSet.Tables[0].Rows[num96][0].ToString() + "' and companycode= '" + dataSet.Tables[0].Rows[num96][4].ToString() + "' ";
							ListString.Add(item);
							item = "insert into yearmst (yearcode,yearrange,stdate,enddate,companycode) values ('" + dataSet.Tables[0].Rows[num96][0].ToString() + "','" + dataSet.Tables[0].Rows[num96][1].ToString() + "','" + dataSet.Tables[0].Rows[num96][2].ToString() + "','" + dataSet.Tables[0].Rows[num96][3].ToString() + "','" + dataSet.Tables[0].Rows[num96][4].ToString() + "') ";
							ListString.Add(item);
							num96++;
						}
						item = "ALTER TABLE yearmst CHECK CONSTRAINT ALL";
						ListString.Add(item);
					}
					if (ListString.Count == 0)
					{
						return;
					}
					if (CheckForInternetConnectionDLL())
					{
						if (val3.State == ConnectionState.Open)
						{
							val3.Close();
						}
						if (val3.State == ConnectionState.Closed)
						{
							val3.Open();
						}
						string text3 = string.Empty;
						SqlTransaction val4 = val3.BeginTransaction("TransactionUpload");
						try
						{
							int num98 = ListString.Count - 1;
							int num99 = 0;
							while (true)
							{
								int num100 = num99;
								int num4 = num98;
								if (num100 > num4)
								{
									break;
								}
								text3 = text3 + ListString[num99] + "\r\n";
								num99++;
							}
							SqlCommand val5 = new SqlCommand(text3, val3);
							val5.Transaction = val4;
							val5.CommandTimeout = 0;
							if (val5.ExecuteNonQuery() == 0)
							{
								throw new Exception("Exception");
							}
							val4.Commit();
							text3 = "";
							ListString.Clear();
							string item = "update itemmst set Sync = 'Y' where Sync = 'N'  ";
							ListString.Add(item);
							item = "update ItemMerging set Sync = 'Y' where Sync = 'N'  ";
							ListString.Add(item);
							item = "update kflmst set Sync = 'Y' where Sync = 'N'  ";
							ListString.Add(item);
							item = "update categorymst set Sync = 'Y' where Sync = 'N'  ";
							ListString.Add(item);
							item = "update groupmst set Sync = 'Y' where Sync = 'N'  ";
							ListString.Add(item);
							item = "update itemrateinfo set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update itemratemst set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update openingstockmst set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update openingStockDetail set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update MasterAccountsopening set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update MasterAccountsLedger set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update companyparamtr set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update MasterAccountsGroups set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update masteraccountsmain set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update masteraccountsopening set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update countermst set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update offermst set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update customerDetails set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update users set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update ManufacturerMaster set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							item = "update yearmst set Sync = 'Y' where Sync = 'N' ";
							ListString.Add(item);
							SqlTransaction val6 = val2.BeginTransaction("TransactionUpload2");
							int num101 = ListString.Count - 1;
							int num102 = 0;
							while (true)
							{
								int num103 = num102;
								int num4 = num101;
								if (num103 > num4)
								{
									break;
								}
								text3 = text3 + ListString[num102] + "\r\n";
								num102++;
							}
							val5 = new SqlCommand(text3, val2);
							val5.Transaction = val6;
							val5.CommandTimeout = 0;
							if (val5.ExecuteNonQuery() == 0)
							{
								throw new Exception("Exception");
							}
							val6.Commit();
							text3 = "";
							ListString.Clear();
							return;
						}
						catch (Exception ex5)
						{
							ProjectData.SetProjectError(ex5);
							Exception ex6 = ex5;
							text3 = "";
							val4.Rollback();
							ProjectData.ClearProjectError();
							return;
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					Interaction.MsgBox((object)"Check your Net Connection", (MsgBoxStyle)64, (object)null);
					ListString = null;
				}
				catch (Exception ex7)
				{
					ProjectData.SetProjectError(ex7);
					Exception ex8 = ex7;
					ProjectData.ClearProjectError();
				}
				finally
				{
					ListString.Clear();
					if (val2.State == ConnectionState.Open)
					{
						val2.Close();
					}
					((Component)(object)val2).Dispose();
					if (val3.State == ConnectionState.Open)
					{
						val3.Close();
					}
					((Component)(object)val3).Dispose();
				}
			}
		}
	}
}
namespace AutoDataSync.My.Resources
{
	/// <summary>
	///   A strongly-typed resource class, for looking up localized strings, etc.
	/// </summary>
	[CompilerGenerated]
	[StandardModule]
	[HideModuleName]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal sealed class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		/// <summary>
		///   Returns the cached ResourceManager instance used by this class.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("AutoDataSync.Resources", typeof(Resources).Assembly);
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
namespace AutoDataSync.My
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
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

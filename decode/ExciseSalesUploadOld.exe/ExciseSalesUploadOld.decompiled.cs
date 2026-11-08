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
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("ExciseSalesUpload")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCopyright("Copyright ©  2024")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyTitle("ExciseSalesUpload")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("4a5e9db5-e8f5-434d-a40c-a1b811f4fc33")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
namespace ExciseSalesUpload.My
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
		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[STAThread]
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
		[DebuggerHidden]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public MyComputer()
		{
		}
	}
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	[HideModuleName]
	[StandardModule]
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
			[CompilerGenerated]
			[ThreadStatic]
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
	[CompilerGenerated]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[DebuggerNonUserCode]
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
namespace ExciseSalesUpload
{
	public class cls_getconn
	{
		private SqlCommand lcmd;

		private SqlCommand scmd;

		public string lclConnectionString;

		public string srvrConnectionString;

		private SqlConnection lcnn;

		private SqlConnection scnn;

		private string srt;

		public cls_getconn()
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
			srt = "";
		}

		public void getconfrExcise()
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = Application.StartupPath + "\\connlcl.txt";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			string exeConfigFilename = Application.StartupPath + "\\Madhushala Ultimate.exe.config";
			ExeConfigurationFileMap val = new ExeConfigurationFileMap();
			val.ExeConfigFilename = exeConfigFilename;
			Configuration val2 = ConfigurationManager.OpenMappedExeConfiguration(val, (ConfigurationUserLevel)0);
			text3 = val2.AppSettings.Settings["authentication"].Value;
			if (Operators.CompareString(text3, "WINDOWS AUTHENTICATION", false) != 0)
			{
				text6 = val2.AppSettings.Settings["server_name"].Value;
				text7 = val2.AppSettings.Settings["db"].Value;
				text8 = val2.AppSettings.Settings["User"].Value;
				text9 = val2.AppSettings.Settings["Password"].Value;
			}
			else
			{
				text6 = val2.AppSettings.Settings["server_name"].Value;
				text7 = val2.AppSettings.Settings["db"].Value;
				text8 = "WINDOWS AUTHENTICATION";
				text9 = "WINDOWS AUTHENTICATION";
			}
			if (Operators.CompareString(text3, "WINDOWS AUTHENTICATION", false) == 0)
			{
				lclConnectionString = "Data Source=" + text6.Trim() + ";Initial Catalog=" + text7.Trim() + ";Trusted_Connection=True";
			}
			else
			{
				lclConnectionString = "Data Source=" + text6.Trim() + ";Initial Catalog=" + text7.Trim() + ";User ID=" + text8.Trim() + ";password=" + text9.Trim();
			}
			lcnn.ConnectionString = lclConnectionString;
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
	[DesignerGenerated]
	public class Form1 : Form
	{
		public class StockPostModel
		{
			public string ShopCode
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string CompanyCode
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string ShopItemCode
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public int qty
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public int Stock
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string TimeStamp
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			[DebuggerNonUserCode]
			public StockPostModel()
			{
			}
		}

		public class StockPostModel2
		{
			public string Licensee_Id_No
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string Datetime_Stamp
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string GTIN
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string Pack_Size
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public double MRP
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public int Quantity
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			[DebuggerNonUserCode]
			public StockPostModel2()
			{
			}
		}

		public class Res
		{
			public string TokenCode
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			[DebuggerNonUserCode]
			public Res()
			{
			}
		}

		public class Res2
		{
			public bool success
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public int Data
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public bool Status
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string Message
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			public string ErrorType
			{
				[DebuggerNonUserCode]
				get;
				[DebuggerNonUserCode]
				set;
			}

			[DebuggerNonUserCode]
			public Res2()
			{
			}
		}

		private static List<WeakReference> __ENCList = new List<WeakReference>();

		private IContainer components;

		[AccessedThroughProperty("Label1")]
		private Label _Label1;

		[AccessedThroughProperty("Button1")]
		private Button _Button1;

		[AccessedThroughProperty("WebBrowser1")]
		private WebBrowser _WebBrowser1;

		[AccessedThroughProperty("ProgressBar1")]
		private ProgressBar _ProgressBar1;

		[AccessedThroughProperty("lbl_loader")]
		private Label _lbl_loader;

		public SqlConnection con;

		private cls_getconn clsCon;

		private Font verdana10Font;

		private StreamReader reader;

		private string tkey;

		private string gtin;

		private string szExc;

		private double mrpExc;

		private int qtyExc;

		private DataSet dsExcz;

		private bool rmv;

		private string companyLicenseNo2;

		private string companyname;

		private string username;

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
				EventHandler eventHandler = Label1_Click;
				if (_Label1 != null)
				{
					((Control)_Label1).Click -= eventHandler;
				}
				_Label1 = value;
				if (_Label1 != null)
				{
					((Control)_Label1).Click += eventHandler;
				}
			}
		}

		internal virtual Button Button1
		{
			[DebuggerNonUserCode]
			get
			{
				return _Button1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				EventHandler eventHandler = Button1_Click;
				if (_Button1 != null)
				{
					((Control)_Button1).Click -= eventHandler;
				}
				_Button1 = value;
				if (_Button1 != null)
				{
					((Control)_Button1).Click += eventHandler;
				}
			}
		}

		internal virtual WebBrowser WebBrowser1
		{
			[DebuggerNonUserCode]
			get
			{
				return _WebBrowser1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_WebBrowser1 = value;
			}
		}

		internal virtual ProgressBar ProgressBar1
		{
			[DebuggerNonUserCode]
			get
			{
				return _ProgressBar1;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_ProgressBar1 = value;
			}
		}

		internal virtual Label lbl_loader
		{
			[DebuggerNonUserCode]
			get
			{
				return _lbl_loader;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			[DebuggerNonUserCode]
			set
			{
				_lbl_loader = value;
			}
		}

		public Form1()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			((Form)this).Load += Form1_Load;
			__ENCAddToList(this);
			con = new SqlConnection();
			clsCon = new cls_getconn();
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
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			//IL_0295: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Expected O, but got Unknown
			Label1 = new Label();
			Button1 = new Button();
			WebBrowser1 = new WebBrowser();
			ProgressBar1 = new ProgressBar();
			lbl_loader = new Label();
			((Control)this).SuspendLayout();
			Label1.AutoSize = true;
			((Control)Label1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)Label1).ForeColor = Color.Navy;
			Label label = Label1;
			Point location = new Point(237, 21);
			((Control)label).Location = location;
			((Control)Label1).Name = "Label1";
			Label label2 = Label1;
			Size size = new Size(224, 20);
			((Control)label2).Size = size;
			((Control)Label1).TabIndex = 0;
			Label1.Text = "Excise Server Data Upload";
			((Control)Button1).Font = new Font("Microsoft Sans Serif", 10f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)Button1).ForeColor = Color.Navy;
			Button button = Button1;
			location = new Point(699, 21);
			((Control)button).Location = location;
			((Control)Button1).Name = "Button1";
			Button button2 = Button1;
			size = new Size(130, 28);
			((Control)button2).Size = size;
			((Control)Button1).TabIndex = 1;
			((ButtonBase)Button1).Text = "Upload Records";
			((ButtonBase)Button1).UseVisualStyleBackColor = true;
			WebBrowser webBrowser = WebBrowser1;
			location = new Point(12, 55);
			((Control)webBrowser).Location = location;
			WebBrowser webBrowser2 = WebBrowser1;
			size = new Size(20, 20);
			((Control)webBrowser2).MinimumSize = size;
			((Control)WebBrowser1).Name = "WebBrowser1";
			WebBrowser webBrowser3 = WebBrowser1;
			size = new Size(817, 430);
			((Control)webBrowser3).Size = size;
			((Control)WebBrowser1).TabIndex = 2;
			((Control)ProgressBar1).BackColor = Color.Blue;
			ProgressBar progressBar = ProgressBar1;
			location = new Point(253, 288);
			((Control)progressBar).Location = location;
			((Control)ProgressBar1).Name = "ProgressBar1";
			ProgressBar progressBar2 = ProgressBar1;
			size = new Size(303, 23);
			((Control)progressBar2).Size = size;
			((Control)ProgressBar1).TabIndex = 3;
			((Control)ProgressBar1).Visible = false;
			lbl_loader.AutoSize = true;
			((Control)lbl_loader).Font = new Font("Times New Roman", 18f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)lbl_loader).ForeColor = Color.DarkBlue;
			Label obj = lbl_loader;
			location = new Point(256, 259);
			((Control)obj).Location = location;
			((Control)lbl_loader).Name = "lbl_loader";
			Label obj2 = lbl_loader;
			size = new Size(83, 26);
			((Control)obj2).Size = size;
			((Control)lbl_loader).TabIndex = 4;
			lbl_loader.Text = "Label2";
			((Control)lbl_loader).Visible = false;
			SizeF autoScaleDimensions = new SizeF(6f, 13f);
			((ContainerControl)this).AutoScaleDimensions = autoScaleDimensions;
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			size = new Size(865, 568);
			((Form)this).ClientSize = size;
			((Control)this).Controls.Add((Control)(object)lbl_loader);
			((Control)this).Controls.Add((Control)(object)ProgressBar1);
			((Control)this).Controls.Add((Control)(object)WebBrowser1);
			((Control)this).Controls.Add((Control)(object)Button1);
			((Control)this).Controls.Add((Control)(object)Label1);
			((Form)this).FormBorderStyle = (FormBorderStyle)1;
			((Control)this).Name = "Form1";
			((Form)this).Text = "Excise Data Upload";
			((Control)this).ResumeLayout(false);
			((Control)this).PerformLayout();
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			try
			{
				WebBrowser1.ScriptErrorsSuppressed = true;
				WebBrowser1.Navigate(new Uri("https://excise.wb.gov.in"), false);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			Excise_Upload();
		}

		private bool CheckForInternetConnectionDLL()
		{
			bool result;
			try
			{
				using WebClient webClient = new WebClient();
				using (webClient.OpenRead("http://www.google.com"))
				{
					result = true;
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public string Excise_Upload()
		{
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0972: Unknown result type (might be due to invalid IL or missing references)
			//IL_0941: Unknown result type (might be due to invalid IL or missing references)
			DataTable dataTable = new DataTable();
			string s = "select top 1 LicenseNo,faxno from companymast";
			DataSet dataSet = populate(s);
			companyLicenseNo2 = Strings.Trim(Conversions.ToString(dataSet.Tables[0].Rows[0]["LicenseNo"]));
			((Control)lbl_loader).Visible = true;
			lbl_loader.Text = "Uploading Data 0%";
			((Control)ProgressBar1).Visible = true;
			ProgressBar1.Minimum = 0;
			ProgressBar1.Value = 1;
			Application.DoEvents();
			do
			{
				tkey = SendRequest(Strings.Trim(Conversions.ToString(dataSet.Tables[0].Rows[0]["LicenseNo"])), Strings.Trim(Conversions.ToString(dataSet.Tables[0].Rows[0]["faxno"])));
			}
			while (Operators.CompareString(tkey, (string)null, false) == 0);
			if (Operators.CompareString(tkey, "Error", false) == 0)
			{
				((Control)lbl_loader).Visible = false;
				((Control)ProgressBar1).Visible = false;
				Interaction.MsgBox((object)"E-abgari UserId or Password Wrong", (MsgBoxStyle)0, (object)null);
			}
			else
			{
				s = "select companyname from companymast";
				dataSet = populate(s);
				companyname = Strings.Trim(Conversions.ToString(dataSet.Tables[0].Rows[0]["companyname"]));
				object obj = new DataSet();
				object obj2 = new DataSet();
				obj = new List<StockPostModel2>();
				s = "select * from tblTempSaleRecordExcise";
				obj2 = populate(s);
				if (Operators.ConditionalCompareObjectGreater(NewLateBinding.LateGet(NewLateBinding.LateGet(NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null), (Type)null, "Rows", new object[0], (string[])null, (Type[])null, (bool[])null), (Type)null, "Count", new object[0], (string[])null, (Type[])null, (bool[])null), (object)0, false))
				{
					ProgressBar1.Maximum = Conversions.ToInteger(NewLateBinding.LateGet(NewLateBinding.LateGet(NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null), (Type)null, "Rows", new object[0], (string[])null, (Type[])null, (bool[])null), (Type)null, "Count", new object[0], (string[])null, (Type[])null, (bool[])null));
					object objectValue = default(object);
					object obj3 = default(object);
					int num = default(int);
					if (ForLoopControl.ForLoopInitObj(objectValue, (object)0, Operators.SubtractObject(NewLateBinding.LateGet(NewLateBinding.LateGet(NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null), (Type)null, "Rows", new object[0], (string[])null, (Type[])null, (bool[])null), (Type)null, "Count", new object[0], (string[])null, (Type[])null, (bool[])null), (object)1), (object)1, ref obj3, ref objectValue))
					{
						do
						{
							ProgressBar1.Value = Conversions.ToInteger(Operators.AddObject(objectValue, (object)1));
							lbl_loader.Text = "Uploading Data " + checked((int)Math.Round((double)ProgressBar1.Value / (double)ProgressBar1.Maximum * 100.0)) + "%";
							Application.DoEvents();
							while (true)
							{
								WebClient webClient = new WebClient();
								string address = "https://excise.wb.gov.in/APIEODB/EODBPLA/InsertPosData";
								ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
								webClient.Headers.Add("Tokencode", tkey);
								webClient.Headers.Add("Accept", "application/json");
								object obj4 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								object[] array = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array2 = array;
								bool[] array3 = new bool[1] { true };
								object obj5 = NewLateBinding.LateGet(obj4, (Type)null, "Rows", array2, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array[0]);
								}
								string text = NewLateBinding.LateGet(obj5, (Type)null, "Item", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null).ToString();
								object obj6 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								object[] array4 = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array5 = array4;
								array3 = new bool[1] { true };
								object obj7 = NewLateBinding.LateGet(obj6, (Type)null, "Rows", array5, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array4[0]);
								}
								string text2 = NewLateBinding.LateGet(obj7, (Type)null, "Item", new object[1] { 2 }, (string[])null, (Type[])null, (bool[])null).ToString();
								object obj8 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								array4 = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array6 = array4;
								array3 = new bool[1] { true };
								object obj9 = NewLateBinding.LateGet(obj8, (Type)null, "Rows", array6, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array4[0]);
								}
								string text3 = NewLateBinding.LateGet(obj9, (Type)null, "Item", new object[1] { 3 }, (string[])null, (Type[])null, (bool[])null).ToString();
								object obj10 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								array4 = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array7 = array4;
								array3 = new bool[1] { true };
								object obj11 = NewLateBinding.LateGet(obj10, (Type)null, "Rows", array7, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array4[0]);
								}
								string text4 = NewLateBinding.LateGet(obj11, (Type)null, "Item", new object[1] { 4 }, (string[])null, (Type[])null, (bool[])null).ToString();
								object obj12 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								array4 = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array8 = array4;
								array3 = new bool[1] { true };
								object obj13 = NewLateBinding.LateGet(obj12, (Type)null, "Rows", array8, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array4[0]);
								}
								string text5 = NewLateBinding.LateGet(obj13, (Type)null, "Item", new object[1] { 5 }, (string[])null, (Type[])null, (bool[])null).ToString();
								object obj14 = NewLateBinding.LateGet(obj2, (Type)null, "Tables", new object[1] { 0 }, (string[])null, (Type[])null, (bool[])null);
								array4 = new object[1] { RuntimeHelpers.GetObjectValue(objectValue) };
								object[] array9 = array4;
								array3 = new bool[1] { true };
								object obj15 = NewLateBinding.LateGet(obj14, (Type)null, "Rows", array9, (string[])null, (Type[])null, array3);
								if (array3[0])
								{
									objectValue = RuntimeHelpers.GetObjectValue(array4[0]);
								}
								string text6 = NewLateBinding.LateGet(obj15, (Type)null, "Item", new object[1] { 1 }, (string[])null, (Type[])null, (bool[])null).ToString();
								string data = "{\"Licensee_Id_No\":\"" + text + "\",\"Datetime_Stamp\":\"" + text6 + "\",\"GTIN\":\"" + text2 + "\",\"Pack_Size\":\"" + text3 + "\",\"MRP\":\"" + text4 + "\",\"Quantity\":\"" + text5 + "\"}";
								webClient.Headers.Add("Content-Type", "application/json");
								try
								{
									string text7 = webClient.UploadString(address, "POST", data);
									if (Operators.CompareString(text7.Substring(1, 7), "success", false) == 0)
									{
										string s2 = "delete from tblTempSaleRecordExcise where GTIN = '" + text2 + "' and Pack_Size = '" + text3 + "' and MRP = '" + text4 + "' and Licensee_Id_No = '" + text + "' ";
										populate(s2);
									}
									object objectValue2 = RuntimeHelpers.GetObjectValue(JsonConvert.DeserializeObject(text7));
								}
								catch (WebException ex)
								{
									ProjectData.SetProjectError((Exception)ex);
									WebException ex2 = ex;
									HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
									if (httpWebResponse == null)
									{
										ProjectData.ClearProjectError();
										continue;
									}
									if (httpWebResponse.StatusCode == HttpStatusCode.TooManyRequests)
									{
										ProjectData.ClearProjectError();
										continue;
									}
									if (httpWebResponse.StatusCode == HttpStatusCode.BadRequest)
									{
										num = 1;
										ProjectData.ClearProjectError();
										break;
									}
									ProjectData.ClearProjectError();
									continue;
								}
								break;
							}
						}
						while (ForLoopControl.ForNextCheckObj(objectValue, obj3, ref objectValue));
					}
					if (num == 1)
					{
						((Control)lbl_loader).Visible = false;
						((Control)ProgressBar1).Visible = false;
						Interaction.MsgBox((object)"Upload Process Completed with some failed records !", (MsgBoxStyle)0, (object)null);
						updateCRM();
					}
					else
					{
						((Control)lbl_loader).Visible = false;
						((Control)ProgressBar1).Visible = false;
						Interaction.MsgBox((object)"Upload Process Completed", (MsgBoxStyle)0, (object)null);
						updateCRM();
					}
				}
				else
				{
					((Control)lbl_loader).Visible = false;
					((Control)ProgressBar1).Visible = false;
					Interaction.MsgBox((object)"All Recordes Are updated", (MsgBoxStyle)0, (object)null);
				}
			}
			string result = default(string);
			return result;
		}

		private void updateCRM()
		{
			string requestUriString = "http://13.235.37.46:90/api/post/postlicenseetrackdataforexcise";
			try
			{
				username = Strings.Trim(companyLicenseNo2) + "/01";
				string text = companyname;
				string text2 = DateTime.Now.ToShortDateString();
				WebRequest webRequest = WebRequest.Create(requestUriString);
				webRequest.Method = "POST";
				webRequest.ContentType = "application/json";
				string s = "{\"licenseeName\":\"" + text + "\",\"licenseeCode\":\"" + username + "\",\"trndates\":\"" + text2 + "\"}";
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				webRequest.ContentLength = bytes.Length;
				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(bytes, 0, bytes.Length);
				}
				WebResponse response = webRequest.GetResponse();
				using StreamReader streamReader = new StreamReader(response.GetResponseStream());
				string text3 = streamReader.ReadToEnd();
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		public string SendRequest(string username, string password)
		{
			int num = 0;
			WebClient webClient = new WebClient();
			string text2 = default(string);
			string result;
			while (true)
			{
				string address = "https://excise.wb.gov.in/APIEODB/EODBPLA/CheckAuthorization";
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				string text = "";
				text = username;
				text = Strings.Trim(text) + "/01";
				string data = "{\"User_Name\":\"" + text + "\",\"Password\":\"" + password + "\"}";
				webClient.Headers.Add("Content-Type", "application/json");
				try
				{
					string json = webClient.UploadString(address, "POST", data);
					JArray jArray = JArray.Parse(json);
					foreach (JObject item in jArray)
					{
						text2 = item["TokenCode"].ToString();
					}
					result = text2;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					if (Operators.CompareString(ex2.Message, "The remote server returned an error: (400) Bad Request.", false) == 0)
					{
						result = "Error";
						ProjectData.ClearProjectError();
						break;
					}
					ProjectData.ClearProjectError();
					continue;
				}
				break;
			}
			return result;
		}

		public DataSet populate(string s)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			try
			{
				DataSet dataSet = new DataSet();
				openCon("barmanager");
				SqlDataAdapter val = new SqlDataAdapter(s, con);
				val.SelectCommand.CommandTimeout = 0;
				((DbDataAdapter)(object)val).Fill(dataSet);
				closecon();
				return dataSet;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				throw ex2;
			}
			finally
			{
			}
		}

		public void closecon()
		{
			con.Close();
			((Component)(object)con).Dispose();
		}

		public bool openCon(string catlog)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			cls_getconn cls_getconn2 = new cls_getconn();
			cls_getconn2.getconfrExcise();
			string lclConnectionString = cls_getconn2.lclConnectionString;
			con.ConnectionString = lclConnectionString;
			bool result;
			try
			{
				con.Open();
				result = true;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox((object)"Sorry connection can not open", (MsgBoxStyle)0, (object)null);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		private void Label1_Click(object sender, EventArgs e)
		{
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			((Form)this).WindowState = (FormWindowState)0;
		}
	}
	[StandardModule]
	internal sealed class mdl_public_variables
	{
		public class DgbCursorLocation
		{
			public int Row;

			public int Col;

			[DebuggerNonUserCode]
			public DgbCursorLocation()
			{
			}
		}

		public static DateTime sms_date;

		public static string Wsms_typeS = "";

		public static string Wsms_type = "";

		public static bool sms_SaleStatement = false;

		public static bool sms_stock = false;

		public static int slwhats = 0;

		public static int iswhatsapp = 0;

		public static DateTime bldte;

		public static bool isratechange;

		public static string manufactcomapany;

		public static bool isdosprint;

		public static bool ishdsale;

		public static bool salesDashBool = false;

		public static bool isOpeningEdit;

		public static int printBillMargin = 0;

		public static SqlConnection con = new SqlConnection();

		public static SqlConnection con2 = new SqlConnection();

		public static string ExciseItemGlobalData = "";

		public static string ExciseItemCode;

		public static string server;

		public static string server_authentication;

		public static string server_id;

		public static string server_password;

		public static string companycode;

		public static string SalesTextstr;

		public static string tblstock = "tbl";

		public static SqlConnection conServer = new SqlConnection();

		public static string server_id_Sr;

		public static string server_password_Sr;

		public static string server_authentication_Sr;

		public static string server_Sr;

		public static bool ServerSync;

		public static string ServerDB;

		public static string LocalDB = ConfigurationManager.OpenExeConfiguration((ConfigurationUserLevel)0).AppSettings.Settings["db"].Value;

		public static bool ServerWork = false;

		public static bool serval = false;

		public static string psrd;

		public static string partyname;

		public static string partycode;

		public static string salespartycode;

		public static double grss;

		public static double dsrt;

		public static double dsct;

		public static double nett;

		public static int dStatus = 0;

		public static bool isAMC;

		public static DateTime AMCdate;

		public static string updatelink;

		public static bool isUpdatestatus;

		public static string memberID;

		public static Form frm = new Form();

		public static DataSet dslogin = new DataSet();

		public static string dllMessage = "";

		public static bool mrpchange;

		public static string shopcode;

		public static string shopname;

		public static int count;

		public static string companyname;

		public static string groupcode;

		public static string col1;

		public static string col2;

		public static string col3;

		public static string col4;

		public static string ratename;

		public static string ratecode;

		public static int salesrateload;

		public static int flag;

		public static string transactionType;

		public static int flag1;

		public static string usercode;

		public static string username;

		public static string accesslevel;

		public static string yearcode;

		public static string yearrange;

		public static string comname;

		public static string comcode;

		public static string address1;

		public static string address2;

		public static string city;

		public static string district;

		public static string state;

		public static string pin;

		public static string phone;

		public static string email;

		public static string www;

		public static string fax;

		public static string lstno;

		public static string cstno;

		public static string panno;

		public static string vatno;

		public static string stno;

		public static string companyLicenseNo;

		public static string ltype;

		public static string ledgercode;

		public static string ledgername;

		public static string acountname;

		public static string debit;

		public static string credit;

		public static string contactperson;

		public static string comemail;

		public static string area;

		public static string acountcode;

		public static string transaction;

		public static bool uadmin;

		public static bool pos;

		public static DataSet dataset = new DataSet();

		public static int counter;

		public static string[] deleted_itemcode = new string[1];

		public static string[] itemcode_array = new string[1];

		public static int searchflag;

		public static int position;

		public static int arraycount;

		public static string salestype;

		public static double amount;

		public static string discount;

		public static string recv;

		public static string due;

		public static int row;

		public static string voucher_number;

		public static string surcharge_amount;

		public static string discountreason;

		public static int salestrnid;

		public static int billtype;

		public static bool FLCS;

		public static bool printTimeInBill;

		public static int csStartBillNo;

		public static string referencebillNo;

		public static string referencebilltype;

		public static int maxdegitFrQty = 2;

		public static object billFormat;

		public static string comaddress1;

		public static string comaddress2;

		public static string comcity;

		public static string comdistrict;

		public static string comstate;

		public static string compin;

		public static string comphone;

		public static string combillno;

		public static string naiStartBillNO;

		public static string comsaleacc;

		public static string combankacc;

		public static string combreakacc;

		public static string comsaleretacc;

		public static double comlimit = 0.0;

		public static double csComLimit;

		public static string comsaveprint;

		public static string comprintsave;

		public static string comprinter;

		public static string compuracc;

		public static string comcashacc;

		public static string comupi;

		public static string comdiscacc;

		public static string com_trade_discount;

		public static string comdefstore;

		public static string comdefrate;

		public static string comexpenceacc;

		public static string comrndacc;

		public static string comsaleref;

		public static string compos;

		public static DateTime comstdate;

		public static DateTime comenddate;

		public static string combillfooter;

		public static string combillfooter2;

		public static string combillfooter3;

		public static string combillfooter4;

		public static string back_up_path;

		public static string comprintmode;

		public static string com_surcharge_acc;

		public static string com_surcharge_percent;

		public static string com_salesonmrp_acc;

		public static double com_salesonmrp_percent;

		public static string com_mobile_number1;

		public static string com_mail_id;

		public static string com_sender;

		public static string com_password;

		public static string comonoff;

		public static string comzone;

		public static int screen_time;

		public static bool status_show;

		public static string shop_type;

		public static string purchase_discount_type;

		public static bool sms_on;

		public static bool backupupload;

		public static string sms_password;

		public static bool saletaxincludingfree;

		public static bool batchSale;

		public static bool manualBatchSale;

		public static bool negativeSale;

		public static bool batchEdit;

		public static string exciseID;

		public static string excisePassword;

		public static bool printCustomerNameInBill;

		public static string storecode1;

		public static string storecode2;

		public static string storename1;

		public static string storename2;

		public static string breakage_bill_no;

		public static string breakage_tp_pass_no;

		public static DateTime transaction_date;

		public static string vch_head_account;

		public static string vch_client_account;

		public static string vchno;

		public static string vch_cheque_no;

		public static double vch_due;

		public static double vch_amt_paid;

		public static double vch_discount;

		public static double vch_net_due;

		public static string vch_narration;

		public static string vch_head_name;

		public static string vch_client_name;

		public static string breakage_trn;

		public static DateTime breakage_trndate;

		public static string breakage_store_name;

		public static string breakage_party_name;

		public static string breakage_narration;

		public static int breakage_row;

		public static int breakage_free_row;

		public static string account_code;

		public static string account_name;

		public static string subgroupname;

		public static bool account_type;

		public static bool _isPurchaseImport = false;

		public static string sales_rate_code;

		public static string sales_rate_name;

		public static string jrlno;

		public static DateTime jrldate;

		public static double fineAmtount;

		public static int fineTrnid;

		public static string fineTrnno;

		public static bool editCs = false;

		public static bool editNai = false;

		public static Bitmap imagePath;

		public static bool sms_billEditDelete;

		public static bool sms_PartySale;

		public static bool sms_creditCardSale;

		public static bool sms_payment;

		public static bool sms_receipt;

		public static bool VoucharDateLock;

		public static string senderSMSid = "";

		public static bool iswtsdb = false;

		public static bool isAutoNumberingVoucher;

		public static bool IsUserWiseSaleStatement = false;

		public static bool isPoleDisplay;

		public static string poleDisplayPortNo;

		public static string PrevYearCode;

		public static DateTime PrevComStDate;

		public static DateTime PrevComEndDate;

		public static string currentyearcode;

		public static bool isPaid = false;

		public static int billno;

		public static bool HideCurntSale_Stock = false;

		public static bool PrntPrevw = false;

		public static bool RestrictBackDateEntry = false;

		public static bool isAutoSave = false;

		public static int AutoSaveLimit;

		public static bool isReceipt = false;

		public static DateTime bookBigningFrm = DateTime.Now;

		public static bool isSaleWindowMinimize = false;

		public static List<string> queryList = new List<string>();

		public static DataTable XMLDataTable = new DataTable("SaveSale");

		public static object ABMDllAssembly;

		public static string schemetrnno;

		public static string schemecode;

		public static int control_selection
		{
			get
			{
				int result = default(int);
				return result;
			}
			set
			{
			}
		}

		public static string convertDateFormat(DateTime convertDate)
		{
			string text = null;
			return Strings.Format((object)convertDate, "MM/dd/yyyy HH:mm:ss tt");
		}

		public static string ConvertDate(DateTime changeDate)
		{
			return changeDate.Year + "-" + changeDate.Month + "-" + changeDate.Day;
		}

		public static string ConvertDateTime(DateTime changedate)
		{
			return changedate.Year + "-" + changedate.Month + "-" + changedate.Day + " " + changedate.Hour + ":" + changedate.Minute + ":" + changedate.Second;
		}

		public static string DateIndiaConvert(DateTime GetDate)
		{
			string text = "";
			text = ((Strings.Len(GetDate.Day.ToString()) != 1) ? GetDate.Day.ToString() : ("0" + GetDate.Day));
			text += "/";
			text = ((Strings.Len(GetDate.Month.ToString()) != 1) ? (text + GetDate.Month) : (text + "0" + GetDate.Month));
			text += "/";
			return text + GetDate.Year;
		}

		public static string DtpDate(string DtpDateVal)
		{
			string text = "";
			string[] array = DtpDateVal.Split(new char[1] { '/' });
			return array[1] + "/" + array[0] + "/" + array[2];
		}

		private static void createconnectionstring(string catlog)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			closecon();
			server = ConfigurationManager.AppSettings["server_name"];
			if (Operators.CompareString(server_authentication, "WINDOWS AUTHENTICATION", false) == 0)
			{
				con = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";Trusted_Connection=True");
				con.Open();
			}
			else if ((Operators.CompareString(server_authentication, "SQL SERVER AUTHENTICATION", false) == 0) | (Operators.CompareString(server_authentication, "SQL AUTHENTICATION", false) == 0))
			{
				con = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";User ID=" + server_id + " ;password=" + server_password);
				con.Open();
			}
		}

		private static void createconnectionstring2(string catlog)
		{
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			closecon2();
			if (!ServerWork)
			{
				server_id = ConfigurationManager.AppSettings["User"];
				server_password = ConfigurationManager.AppSettings["Password"];
			}
			else
			{
				server_id = ConfigurationManager.AppSettings["User_Sr"];
				server_password = ConfigurationManager.AppSettings["Password_Sr"];
			}
			if (Operators.CompareString(server_authentication, "WINDOWS AUTHENTICATION", false) == 0)
			{
				con2 = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";Trusted_Connection=True");
				con2.Open();
			}
			else if ((Operators.CompareString(server_authentication, "SQL SERVER AUTHENTICATION", false) == 0) | (Operators.CompareString(server_authentication, "SQL AUTHENTICATION", false) == 0))
			{
				con2 = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";User ID=" + server_id + " ;password=" + server_password);
				con2.Open();
			}
		}

		public static string getConnectionString(string catlog)
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			closecon();
			server_authentication = ConfigurationManager.AppSettings["Authentication_sr"];
			server_id = ConfigurationManager.AppSettings["User_Sr"];
			server_password = ConfigurationManager.AppSettings["Password_Sr"];
			if (Operators.CompareString(server_authentication, "WINDOWS AUTHENTICATION", false) == 0)
			{
				con = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";Trusted_Connection=True");
			}
			else if ((Operators.CompareString(server_authentication, "SQL SERVER AUTHENTICATION", false) == 0) | (Operators.CompareString(server_authentication, "SQL AUTHENTICATION", false) == 0))
			{
				con = new SqlConnection("Data Source=" + server + ";Initial Catalog=" + catlog + ";User ID=" + server_id + ";password=" + server_password);
			}
			return con.ConnectionString;
		}

		public static void opencon()
		{
			createconnectionstring(LocalDB);
		}

		public static void opencon2()
		{
			createconnectionstring2(LocalDB);
		}

		public static void openmaster()
		{
			createconnectionstring("master");
		}

		public static void closecon()
		{
			con.Close();
			((Component)(object)con).Dispose();
		}

		public static void closecon2()
		{
			con2.Close();
			((Component)(object)con2).Dispose();
		}

		public static string lft(string @string, int length)
		{
			return Strings.Left(@string, length);
		}

		public static string rght(string @string, int length)
		{
			return Strings.Right(@string, length);
		}

		public static object clear_back(Bitmap bmp)
		{
			bmp.MakeTransparent(Color.White);
			return bmp;
		}

		private static void InsertMobileAppData()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			try
			{
				SqlConnection val = new SqlConnection("Data Source=13.235.37.46,6281;Initial Catalog=ABMCRM;User ID=ABM-Dev2;Password=Z27wCUM9WCtn;");
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		}

		private static bool checkMobileAppNoExists(string empMb, string remkk, string empName)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Expected O, but got Unknown
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Expected O, but got Unknown
			bool result = default(bool);
			try
			{
				SqlConnection val = new SqlConnection("Data Source=13.235.37.46,6281;Initial Catalog=ABMCRM;User ID=ABM-Dev2;Password=Z27wCUM9WCtn;");
				SqlCommand val2 = new SqlCommand("select top 1 * from  tbl_EmployeeDetails_MobileApp where empMobileNo='" + empMb + "'", val);
				SqlDataAdapter val3 = new SqlDataAdapter(val2);
				DataSet dataSet = new DataSet();
				((DbDataAdapter)(object)val3).Fill(dataSet);
				if (dataSet.Tables[0].Rows.Count > 0)
				{
					SqlDataAdapter val4 = new SqlDataAdapter("select isnull(CallOff,0) as CallOff from tbl_ComplaintReq_MobileApp where EmployeeCode='" + dataSet.Tables[0].Rows[0]["ID"].ToString() + "' and cast(CreatedOn as Date)=cast(getDate() as date)", val);
					DataSet dataSet2 = new DataSet();
					((DbDataAdapter)(object)val4).Fill(dataSet2);
					if (dataSet2.Tables[0].Rows.Count > 0 && Convert.ToBoolean(dataSet2.Tables[0].Rows[0]["CallOff"].ToString()))
					{
						result = false;
						return result;
					}
					if (val.State == ConnectionState.Open)
					{
						val.Close();
					}
					val.Open();
					SqlCommand val5 = new SqlCommand("Insert Into tbl_ComplaintReq_MobileApp values(1,'Madhushala Ultimate',0,'" + dataSet.Tables[0].Rows[0]["ID"].ToString() + "','" + remkk + "',getDate(),1,null,0,null,null)", val);
					val5.ExecuteNonQuery();
					val.Close();
					result = true;
					return result;
				}
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				val.Open();
				SqlCommand val6 = new SqlCommand("Insert Into tbl_EmployeeDetails_MobileApp values(0,'Madhushala Ultimate',0,'" + dataSet.Tables[0].Rows[0]["ID"].ToString() + "','" + remkk + "',getDate(),1,null,0,null,null)", val);
				val6.ExecuteNonQuery();
				val.Close();
				val.Close();
				result = false;
				return result;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public static string encription(int encriptionNo)
		{
			string text = "madhushala";
			if (encriptionNo == 0)
			{
				return text + "0000000000";
			}
			string text2 = encriptionNo.ToString();
			string text3 = "";
			checked
			{
				int num = text2.Length - 1;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 > num4)
					{
						break;
					}
					text3 = ((Operators.CompareString(Conversions.ToString(text2[num2]), "0", false) != 0) ? (text3 + (10 - int.Parse(Conversions.ToString(text2[num2])))) : (text3 + "-"));
					num2++;
				}
				if (text3.Length < 10)
				{
					int num5 = text3.Length + 1;
					while (true)
					{
						int num6 = num5;
						int num4 = 10;
						if (num6 > num4)
						{
							break;
						}
						text += "0";
						num5++;
					}
				}
				return text + text3;
			}
		}

		public static int decription(string decriptionNo)
		{
			string text = "";
			decriptionNo = decriptionNo.Remove(0, 10);
			if (Operators.CompareString(decriptionNo, "0000000000", false) == 0)
			{
				return 0;
			}
			checked
			{
				int num = decriptionNo.Length - 1;
				int num2 = 0;
				while (true)
				{
					int num3 = num2;
					int num4 = num;
					if (num3 > num4)
					{
						break;
					}
					if (Operators.CompareString(Conversions.ToString(decriptionNo[num2]), "-", false) == 0)
					{
						text += "0";
					}
					else if (Operators.CompareString(Conversions.ToString(decriptionNo[num2]), "0", false) != 0)
					{
						text += 10 - int.Parse(Conversions.ToString(decriptionNo[num2]));
					}
					num2++;
				}
				return int.Parse(text);
			}
		}

		public static string GetDayOfWeek(DateTime dateForDay)
		{
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			DayOfWeek dayOfWeek = currentCulture.Calendar.GetDayOfWeek(dateForDay.Date);
			return currentCulture.DateTimeFormat.GetDayName(dayOfWeek);
		}

		public static void PoleDisplayImportText(string line1, string line2)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			SerialPort val = new SerialPort();
			val.WriteTimeout = -1;
			val.ReadTimeout = -1;
			val.PortName = poleDisplayPortNo;
			val.BaudRate = 9600;
			val.Parity = (Parity)0;
			val.DataBits = 8;
			val.StopBits = (StopBits)1;
			val.Open();
			val.Write(Convert.ToString('\f'));
			val.WriteLine(line1);
			val.WriteLine("\r" + line2);
			val.Close();
			((Component)(object)val).Dispose();
			val = null;
		}

		public static Bitmap ImageResize(Image ImageSource, double ImageScale)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			Bitmap val = new Bitmap(ImageSource);
			checked
			{
				Bitmap val2 = new Bitmap((int)Math.Round((double)((Image)val).Width * ImageScale), (int)Math.Round((double)((Image)val).Height * ImageScale));
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				val3.DrawImage((Image)(object)val, 0, 0, ((Image)val2).Width + 1, ((Image)val2).Height + 1);
				return val2;
			}
		}

		private static void CreateServerString(string catlog_Sr)
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			try
			{
				if ((Operators.CompareString(server_authentication_Sr, "SQL SERVER AUTHENTICATION", false) == 0) | (Operators.CompareString(server_authentication, "SQL AUTHENTICATION", false) == 0))
				{
					conServer = new SqlConnection("Data Source=" + server_Sr + ";Initial Catalog=" + catlog_Sr + ";User ID=" + server_id_Sr + " ;password=" + server_password_Sr);
					conServer.Open();
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				Interaction.MsgBox((object)ex2.Message, (MsgBoxStyle)0, (object)null);
				ProjectData.ClearProjectError();
			}
		}

		public static void txtconnection()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			FileStream stream = new FileStream(Application.StartupPath + "\\ServerName.txt", FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(stream);
			text2 = Convert.ToString(server_Sr);
			text = Convert.ToString(server);
			text3 = Convert.ToString(ServerDB);
			string text4 = "";
			text4 = text + Environment.NewLine + text2 + Environment.NewLine + text3;
			streamWriter.WriteLine(text4);
			streamWriter.Flush();
			streamWriter.Close();
			closecon();
			closeconServer();
		}

		public static void openconSer()
		{
			if (iswtsdb)
			{
				CreateServerString("ABMCRM");
			}
			else
			{
				CreateServerString(ServerDB);
			}
		}

		public static void closeconServer()
		{
			conServer.Close();
			((Component)(object)conServer).Dispose();
		}

		public static bool CheckForInternetConnection()
		{
			bool result;
			try
			{
				using WebClient webClient = new WebClient();
				using (webClient.OpenRead("http://www.google.com"))
				{
					result = true;
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				result = false;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public static bool CheckHomeDelivery()
		{
			if (Operators.CompareString(ConfigurationManager.AppSettings["homedelivery"], "full", false) == 0)
			{
				return true;
			}
			return false;
		}

		public static string SetKotPrinter()
		{
			return ConfigurationManager.AppSettings["kotprinter"].ToString();
		}
	}
}
namespace ExciseSalesUpload.My.Resources
{
	[HideModuleName]
	[DebuggerNonUserCode]
	[StandardModule]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[CompilerGenerated]
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
					ResourceManager resourceManager = new ResourceManager("ExciseSalesUpload.Resources", typeof(Resources).Assembly);
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
namespace ExciseSalesUpload.My
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
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
							((WindowsFormsApplicationBase)MyProject.Application).Shutdown += (ShutdownEventHandler)([DebuggerNonUserCode] [EditorBrowsable(EditorBrowsableState.Advanced)] (object sender, EventArgs e) =>
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

		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		private static void AutoSaveSettings(object sender, EventArgs e)
		{
			if (((WindowsFormsApplicationBase)MyProject.Application).SaveMySettingsOnExit)
			{
				((ApplicationSettingsBase)MySettingsProperty.Settings).Save();
			}
		}
	}
	[DebuggerNonUserCode]
	[HideModuleName]
	[StandardModule]
	[CompilerGenerated]
	internal sealed class MySettingsProperty
	{
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings => MySettings.Default;
	}
}

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
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.MyServices.Internal;

[assembly: AssemblyProduct("MadhushalaDataSync")]
[assembly: AssemblyTitle("MadhushalaDataSync")]
[assembly: AssemblyTrademark("")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: CompilationRelaxations(8)]
[assembly: ComVisible(false)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyCopyright("Copyright ©  2019")]
[assembly: AssemblyCompany("")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("30456d07-9cee-4cca-b61e-f5ed575eaddf")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace MadhushalaDataSync.My
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
	[HideModuleName]
	[StandardModule]
	[GeneratedCode("MyTemplate", "10.0.0.0")]
	internal sealed class MyProject
	{
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

			[DebuggerHidden]
			[EditorBrowsable(EditorBrowsableState.Never)]
			public ThreadSafeObjectProvider()
			{
				m_Context = new ContextValue<T>();
			}
		}

		private static readonly ThreadSafeObjectProvider<MyComputer> m_ComputerObjectProvider = new ThreadSafeObjectProvider<MyComputer>();

		private static readonly ThreadSafeObjectProvider<MyApplication> m_AppObjectProvider = new ThreadSafeObjectProvider<MyApplication>();

		private static readonly ThreadSafeObjectProvider<User> m_UserObjectProvider = new ThreadSafeObjectProvider<User>();

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
namespace MadhushalaDataSync
{
	public class cls_getconn
	{
		private SqlCommand lcmd;

		private SqlCommand scmd;

		public string lclConnectionString;

		public string srvrConnectionString;

		private SqlConnection lcnn;

		private SqlConnection scnn;

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
		}

		public void getcon1()
		{
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			string text = "";
			string text2 = "";
			string text3 = "";
			string path = Application.StartupPath + "\\ServerName.txt";
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
					string text4 = array[num2];
					if (num2 == 0)
					{
						text = text4;
					}
					if (num2 == 1)
					{
						text2 = text4;
					}
					if (num2 == 2)
					{
						text3 = text4;
					}
					num2++;
				}
				streamReader.Close();
				string text5 = text;
				string text6 = text2;
				string text7 = text3;
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				Uri uri = new Uri(codeBase);
				string localPath = uri.LocalPath;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(localPath);
				string text8 = "applicationSettings";
				string text9 = fileNameWithoutExtension + ".Properties.Settings";
				string exeConfigFilename = Application.StartupPath + "\\Madhushala Ultimate.exe.config";
				ExeConfigurationFileMap val = new ExeConfigurationFileMap();
				val.ExeConfigFilename = exeConfigFilename;
				Configuration val2 = ConfigurationManager.OpenMappedExeConfiguration(val, (ConfigurationUserLevel)0);
				AppSettingsSection val3 = (AppSettingsSection)val2.GetSection("appSettings");
				string value = val3.Settings["AUTHENTICATION"].Value;
				string value2 = val3.Settings["AUTHENTICATION_SR"].Value;
				string value3 = val3.Settings["User"].Value;
				string value4 = val3.Settings["Password"].Value;
				string value5 = val3.Settings["User_Sr"].Value;
				string value6 = val3.Settings["Password_Sr"].Value;
				if (Operators.CompareString(value, "WINDOWS AUTHENTICATION", false) == 0)
				{
					lclConnectionString = "Data Source=" + text5.Trim() + ";Initial Catalog=barmanager;Trusted_Connection=True";
				}
				else if (Operators.CompareString(value, "SQL SERVER AUTHENTICATION", false) == 0)
				{
					lclConnectionString = "Data Source=" + text5.Trim() + ";Initial Catalog=barmanager;User ID=" + value3.Trim() + ";password=" + value4.Trim();
				}
				lcnn.ConnectionString = lclConnectionString;
				if (Operators.CompareString(value2, "WINDOWS AUTHENTICATION", false) == 0)
				{
					srvrConnectionString = "Data Source=" + text6.Trim() + ";Initial Catalog=" + text7.Trim() + ";Trusted_Connection=True";
				}
				else if (Operators.CompareString(value2, "SQL SERVER AUTHENTICATION", false) == 0)
				{
					srvrConnectionString = "Data Source=" + text6.Trim() + ";Initial Catalog=" + text7.Trim() + ";User ID=" + value5.Trim() + ";password=" + value6.Trim();
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

		public SqlConnection localsqlcon;

		public SqlConnection serversqlcon;

		public Form1()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			((Form)this).Load += Form1_Load;
			__ENCAddToList(this);
			localsqlcon = new SqlConnection();
			serversqlcon = new SqlConnection();
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

		private void Form1_Load(object sender, EventArgs e)
		{
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

		public void FrmCompParameter(int compcodeComp, int yearcodeComp)
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				Sale_Add2_FL(compcodeComp, yearcodeComp);
				Purchase_Add2(compcodeComp, yearcodeComp);
				Transfer_Add2(compcodeComp, yearcodeComp);
				Breakage_Add2(compcodeComp, yearcodeComp);
				PAYREC_Add(compcodeComp, yearcodeComp);
				Trndelete2(compcodeComp, yearcodeComp);
				Trnid_Add2(compcodeComp, yearcodeComp);
				MessageBox.Show("Data Uploaded Successfully");
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				MessageBox.Show("Data Upload Failed");
				ProjectData.ClearProjectError();
			}
			finally
			{
			}
		}

		public void TempSaleAdd(int compcode00, int yearcode00, string lcode)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Expected O, but got Unknown
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec sp_SaleNew '" + DateAndTime.Today.ToString("yyyy-MM-dd") + "','" + DateAndTime.Today.ToString("yyyy-MM-dd") + "', '" + Conversions.ToString(compcode00) + "','" + Conversions.ToString(yearcode00) + "','" + lcode + "',''", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTmpdata";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_LiveTempDataUpload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@Tmpdata", (object)text);
					val4.Parameters.AddWithValue("@companycode", (object)compcode00);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					try
					{
						((DbDataAdapter)(object)val5).Fill(dataSet2);
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						ProjectData.ClearProjectError();
					}
					string text2 = dataSet2.Tables[0].Rows[0][0].ToString();
				}
			}
			catch (Exception ex3)
			{
				ProjectData.SetProjectError(ex3);
				Exception ex4 = ex3;
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

		public void Sale_Add2_FL(int compcode0, int yearcode0)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Expected O, but got Unknown
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Sales_Add_FL '" + Conversions.ToString(compcode0) + "','" + Conversions.ToString(yearcode0) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtsalesbillmain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtsalesbilldetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtsales_tax";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter6.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Sales_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text2);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text3);
					val4.Parameters.AddWithValue("@salesbillmain", (object)text4);
					val4.Parameters.AddWithValue("@salesbilldetail", (object)text5);
					val4.Parameters.AddWithValue("@sales_tax", (object)text6);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text7 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text7, "Y", false) == 0)
					{
						val4 = new SqlCommand("update salesbillmain set Sync = 'Y' where billType = '0' and companycode = '" + Conversions.ToString(compcode0) + "' and yearcode = '" + Conversions.ToString(yearcode0) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
						val4 = new SqlCommand("UPDATE TransactionMain set sync = 'Y' where ReceiptType = 'FL' and trnType IN('SALE','RECEIPT','CREDIT NOTE') and companycode = '" + Conversions.ToString(compcode0) + "' and yearcode = '" + Conversions.ToString(yearcode0) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
					}
				}
				Sale_Add2_CS(compcode0, yearcode0);
				Trnid_Add2(compcode0, yearcode0);
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

		public void Sale_Add2_CS(int compcode1, int yearcode1)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Expected O, but got Unknown
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Sales_Add_CS '" + Conversions.ToString(compcode1) + "','" + Conversions.ToString(yearcode1) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtsalesbillmain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtsalesbilldetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtsales_tax";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter6.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Sales_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text2);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text3);
					val4.Parameters.AddWithValue("@salesbillmain", (object)text4);
					val4.Parameters.AddWithValue("@salesbilldetail", (object)text5);
					val4.Parameters.AddWithValue("@sales_tax", (object)text6);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text7 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text7, "Y", false) == 0)
					{
						val4 = new SqlCommand("update salesbillmain set Sync = 'Y' where billType  = '1' and companycode = '" + Conversions.ToString(compcode1) + "' and yearcode = '" + Conversions.ToString(yearcode1) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
						val4 = new SqlCommand("UPDATE TransactionMain set sync = 'Y' where  ReceiptType = 'CS' and trnType IN('SALE','RECEIPT','CREDIT NOTE') and companycode = '" + Conversions.ToString(compcode1) + "' and yearcode = '" + Conversions.ToString(yearcode1) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
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

		public void Purchase_Add2(int compcode2, int yearcode2)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Expected O, but got Unknown
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Purchase_Add '" + Conversions.ToString(compcode2) + "','" + Conversions.ToString(yearcode2) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtPurchaseMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtpurchasedetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtPurchaseTaxDetail";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter3.ToString();
				dataSet.Tables[3].TableName = "dtTransactionMain";
				StringWriter stringWriter4 = new StringWriter();
				dataSet.Tables[3].WriteXml((TextWriter?)stringWriter4, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text4 = stringWriter4.ToString();
				dataSet.Tables[4].TableName = "dtTransactiondetail";
				StringWriter stringWriter5 = new StringWriter();
				dataSet.Tables[4].WriteXml((TextWriter?)stringWriter5, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text5 = stringWriter5.ToString();
				dataSet.Tables[5].TableName = "dtTransactionMatch";
				StringWriter stringWriter6 = new StringWriter();
				dataSet.Tables[5].WriteXml((TextWriter?)stringWriter6, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text6 = stringWriter6.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Purchase_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@purchasemain", (object)text);
					val4.Parameters.AddWithValue("@purchasedetail", (object)text2);
					val4.Parameters.AddWithValue("@PurchaseTaxDetail", (object)text3);
					val4.Parameters.AddWithValue("@TransactionMain", (object)text4);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text5);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text6);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text7 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text7, "Y", false) == 0)
					{
						val4 = new SqlCommand("update purchasemain set Sync = 'Y' where ptype = 'PURCHASE' and companycode = '" + Conversions.ToString(compcode2) + "' and yearcode = '" + Conversions.ToString(yearcode2) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
						val4 = new SqlCommand("UPDATE TransactionMain set sync = 'Y' where trnType = 'PURCHASE' and companycode = '" + Conversions.ToString(compcode2) + "' and yearcode = '" + Conversions.ToString(yearcode2) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
					}
				}
				Trnid_Add2(compcode2, yearcode2);
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

		public void Transfer_Add2(int compcode3, int yearcode3)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Transfer_Add '" + Conversions.ToString(compcode3) + "','" + Conversions.ToString(yearcode3) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtstk_transfer_main";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtstk_transfer_detail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Transfer_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@stk_transfer_main", (object)text);
					val4.Parameters.AddWithValue("@stk_transfer_detail", (object)text2);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text3 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text3, "Y", false) == 0)
					{
						val4 = new SqlCommand("update stk_transfer_main set Sync = 'Y' where companycode = '" + Conversions.ToString(compcode3) + "' and yearcode = '" + Conversions.ToString(yearcode3) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
					}
				}
				Trnid_Add2(compcode3, yearcode3);
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

		public void Breakage_Add2(int compcode4, int yearcode4)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Expected O, but got Unknown
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Breakage_Add '" + Conversions.ToString(compcode4) + "','" + Conversions.ToString(yearcode4) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtbreakagemain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtbreakagedetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Breakage_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@breakagemain", (object)text);
					val4.Parameters.AddWithValue("@breakagedetail", (object)text2);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text3 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text3, "Y", false) == 0)
					{
						val4 = new SqlCommand("update breakagemain set Sync = 'Y' where companycode = '" + Conversions.ToString(compcode4) + "' and yearcode = '" + Conversions.ToString(yearcode4) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
					}
				}
				Trnid_Add2(compcode4, yearcode4);
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

		public void PAYREC_Add(int compcode5, int yearcode5)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Expected O, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Expected O, but got Unknown
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_PAYREC_ADD '" + Conversions.ToString(compcode5) + "','" + Conversions.ToString(yearcode5) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dtTransactionMain";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
				dataSet.Tables[1].TableName = "dtTransactiondetail";
				StringWriter stringWriter2 = new StringWriter();
				dataSet.Tables[1].WriteXml((TextWriter?)stringWriter2, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text2 = stringWriter2.ToString();
				dataSet.Tables[2].TableName = "dtTransactionMatch";
				StringWriter stringWriter3 = new StringWriter();
				dataSet.Tables[2].WriteXml((TextWriter?)stringWriter3, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text3 = stringWriter3.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_PAYREC_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@TransactionMain", (object)text);
					val4.Parameters.AddWithValue("@Transactiondetail", (object)text2);
					val4.Parameters.AddWithValue("@TransactionMatch", (object)text3);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text4 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text4, "Y", false) == 0)
					{
						val4 = new SqlCommand("UPDATE TransactionMain set sync = 'Y' where trnType IN('RECEIPT','PAYMENT') and companycode = '" + Conversions.ToString(compcode5) + "' and yearcode = '" + Conversions.ToString(yearcode5) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
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

		public void Trndelete2(int compcode6, int yearcode6)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Expected O, but got Unknown
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Trndelete '" + Conversions.ToString(compcode6) + "','" + Conversions.ToString(yearcode6) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "DTTrndelete";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_delete_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@Trndelete", (object)text);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text2 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text2, "Y", false) == 0)
					{
						val4 = new SqlCommand("UPDATE Trnidmst set sync = 'Y' WHERE companycode = '" + Conversions.ToString(compcode6) + "' and yearcode = '" + Conversions.ToString(yearcode6) + "' and Sync = 'D' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
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

		public void Trnid_Add2(int compcode7, int yearcode7)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Expected O, but got Unknown
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Expected O, but got Unknown
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Expected O, but got Unknown
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
				cls_getconn cls_getconn2 = new cls_getconn();
				cls_getconn2.getcon1();
				string lclConnectionString = cls_getconn2.lclConnectionString;
				string srvrConnectionString = cls_getconn2.srvrConnectionString;
				val = new SqlConnection(lclConnectionString);
				val2 = new SqlConnection(srvrConnectionString);
				if (val.State == ConnectionState.Open)
				{
					val.Close();
				}
				if (val.State == ConnectionState.Closed)
				{
					val.Open();
				}
				SqlDataAdapter val3 = new SqlDataAdapter("exec SP_Trnid_Add '" + Conversions.ToString(compcode7) + "','" + Conversions.ToString(yearcode7) + "'", val);
				((DbDataAdapter)(object)val3).Fill(dataSet);
				dataSet.Tables[0].TableName = "dttrnidmst";
				StringWriter stringWriter = new StringWriter();
				dataSet.Tables[0].WriteXml((TextWriter?)stringWriter, XmlWriteMode.WriteSchema, writeHierarchy: false);
				string text = stringWriter.ToString();
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
					SqlCommand val4 = new SqlCommand("SP_Trnid_Upload", val2);
					val4.CommandType = CommandType.StoredProcedure;
					val4.Parameters.AddWithValue("@trnidmst", (object)text);
					SqlDataAdapter val5 = new SqlDataAdapter();
					val5.SelectCommand = val4;
					dataSet2 = new DataSet();
					val5.SelectCommand.CommandTimeout = 0;
					((DbDataAdapter)(object)val5).Fill(dataSet2);
					string text2 = dataSet2.Tables[0].Rows[0][0].ToString();
					if (Operators.CompareString(text2, "Y", false) == 0)
					{
						val4 = new SqlCommand("UPDATE Trnidmst set sync = 'Y' WHERE companycode = '" + Conversions.ToString(compcode7) + "' and yearcode = '" + Conversions.ToString(yearcode7) + "' ", val);
						val4.CommandType = CommandType.Text;
						val4.ExecuteNonQuery();
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
	}
}
namespace MadhushalaDataSync.My.Resources
{
	[DebuggerNonUserCode]
	[HideModuleName]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[StandardModule]
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
					ResourceManager resourceManager = new ResourceManager("MadhushalaDataSync.Resources", typeof(Resources).Assembly);
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
namespace MadhushalaDataSync.My
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
	internal sealed class MySettings : ApplicationSettingsBase
	{
		private static MySettings defaultInstance = (MySettings)(object)SettingsBase.Synchronized((SettingsBase)(object)new MySettings());

		public static MySettings Default => defaultInstance;

		[DebuggerNonUserCode]
		public MySettings()
		{
		}
	}
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[HideModuleName]
	[StandardModule]
	internal sealed class MySettingsProperty
	{
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings => MySettings.Default;
	}
}

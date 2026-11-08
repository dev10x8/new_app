using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.MyServices.Internal;

[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("NumberToWord")]
[assembly: AssemblyTitle("NumberToWord")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyDescription("")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("020730f0-eec8-4bdc-88d9-97a7276fe6a2")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.0.0")]
namespace NumberToWord.My
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[GeneratedCode("MyTemplate", "8.0.0.0")]
	internal class MyApplication : ApplicationBase
	{
		[DebuggerNonUserCode]
		public MyApplication()
		{
		}
	}
	[GeneratedCode("MyTemplate", "8.0.0.0")]
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
	[GeneratedCode("MyTemplate", "8.0.0.0")]
	internal sealed class MyProject
	{
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[ComVisible(false)]
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
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class InternalXmlHelper
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerNonUserCode]
		[CompilerGenerated]
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
namespace NumberToWord
{
	public class NumeriCon
	{
		[DebuggerNonUserCode]
		public NumeriCon()
		{
		}

		public string ConvertNum(double Input, string str1, string str2)
		{
			string text = "";
			long num = 0L;
			string text2 = Input.ToString();
			string text3 = Conversions.ToString(Input);
			long num2 = Input.ToString().Length;
			string[] array = text2.Split(new char[1] { '.' });
			if (array.Length == 2)
			{
				num = ((array.ElementAt(1).Length != 1) ? int.Parse(array.ElementAt(1)) : int.Parse(array.ElementAt(1) + "0"));
				Input -= double.Parse("." + array.ElementAt(1));
				text3 = Conversions.ToString(Input);
				num2 = Input.ToString().Length;
			}
			checked
			{
				if (num2 > 9)
				{
					text3 = text3.Insert((int)(num2 - 7), ",");
				}
				string[] array2 = text3.Split(new char[1] { ',' });
				int num3 = 0;
				string[] array3 = array2;
				foreach (string s in array3)
				{
					num3++;
					Input = double.Parse(s);
					text3 = Conversions.ToString(Input);
					num2 = Input.ToString().Length;
					if (Input < 1000.0)
					{
						if ((Input > 0.0) & (array2.Length > 0))
						{
							text += FindNumber((long)Math.Round(Input));
						}
					}
					else
					{
						int num4 = 0;
						num4 = ((num2 <= 3) ? 2 : 3);
						while (num2 - num4 > 0)
						{
							text3 = text3.Insert((int)(num2 - num4), ",");
							num2 -= num4;
							num4 = 2;
						}
						string[] array4 = text3.Split(new char[1] { ',' });
						num2 = Input.ToString().Length;
						int num5 = 0;
						num4 = 3;
						string[] array5 = array4;
						foreach (string text4 in array5)
						{
							long num6 = Conversions.ToLong(text4);
							num5++;
							if (num5 == array4.Length)
							{
								if (num6 != 0)
								{
									text = ((Conversions.ToLong(text4) >= 100) ? (text + " " + FindNumber(Conversions.ToLong(text4))) : (text + " " + FindNumber(Conversions.ToLong(text4))));
								}
							}
							else if (num6 != 0)
							{
								text = ((Operators.CompareString(text, (string)null, false) != 0) ? (text + " " + FindNumber(Conversions.ToLong(text4)) + " " + FindSuffix(num2, Conversions.ToLong(text4))) : (text + FindNumber(Conversions.ToLong(text4)) + " " + FindSuffix(num2, Conversions.ToLong(text4))));
							}
							num2 -= text4.Length;
						}
					}
					if ((num3 == 1) & (array2.Length > 1))
					{
						text += " Crore ";
					}
				}
				text = text + " " + str1;
				if (num > 0)
				{
					text = text + " And " + FindNumber(num) + " " + str2;
				}
				return text;
			}
		}

		private string FindNumber(long Number)
		{
			string text = null;
			string[] array = new string[11]
			{
				"Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
				"Ten"
			};
			string[] array2 = new string[10] { "", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
			checked
			{
				if (Number < 11)
				{
					text = array[(int)Number];
				}
				else if (Number < 20)
				{
					text = array2[(int)(Number - 10)];
				}
				else if (Number == 20)
				{
					text = "Twenty";
				}
				else if (Number < 30)
				{
					text = "Twenty " + array[(int)(Number - 20)];
				}
				else if (Number == 30)
				{
					text = "Thirty";
				}
				else if (Number < 40)
				{
					text = "Thirty " + array[(int)(Number - 30)];
				}
				else if (Number == 40)
				{
					text = "Fourty";
				}
				else if (Number < 50)
				{
					text = "Fourty " + array[(int)(Number - 40)];
				}
				else if (Number == 50)
				{
					text = "Fifty";
				}
				else if (Number < 60)
				{
					text = "Fifty " + array[(int)(Number - 50)];
				}
				else if (Number == 60)
				{
					text = "Sixty";
				}
				else if (Number < 70)
				{
					text = "Sixty " + array[(int)(Number - 60)];
				}
				else if (Number == 70)
				{
					text = "Seventy";
				}
				else if (Number < 80)
				{
					text = "Seventy " + array[(int)(Number - 70)];
				}
				else if (Number == 80)
				{
					text = "Eighty";
				}
				else if (Number < 90)
				{
					text = "Eighty " + array[(int)(Number - 80)];
				}
				else if (Number == 90)
				{
					text = "Ninety";
				}
				else if (Number < 100)
				{
					text = "Ninety " + array[(int)(Number - 90)];
				}
				else if (Number < 1000)
				{
					text = Number.ToString();
					text = text.Insert(1, ",");
					string[] array3 = text.Split(new char[1] { ',' });
					text = FindNumber(Conversions.ToLong(array3[0])) + " Hundred";
					string text2 = FindNumber(Conversions.ToLong(array3[1]));
					if (Conversions.ToLong(array3[1]) != 0)
					{
						text = text + " " + text2;
					}
				}
				return text;
			}
		}

		private string FindSuffix(long Length, long l)
		{
			if (l != 0)
			{
				if (Length > 7)
				{
					return "Crore";
				}
				if (Length > 5)
				{
					return "Lakh";
				}
				if (Length > 3)
				{
					return "Thousand";
				}
				if (Length > 2)
				{
					return "Hundred";
				}
				return "";
			}
			return "";
		}
	}
}
namespace NumberToWord.My.Resources
{
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[StandardModule]
	[HideModuleName]
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
					ResourceManager resourceManager = new ResourceManager("NumberToWord.Resources", typeof(Resources).Assembly);
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
namespace NumberToWord.My
{
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
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
	[StandardModule]
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[HideModuleName]
	internal sealed class MySettingsProperty
	{
		[HelpKeyword("My.Settings")]
		internal static MySettings Settings => MySettings.Default;
	}
}

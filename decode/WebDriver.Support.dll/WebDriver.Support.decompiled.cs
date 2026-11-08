using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Remote;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: CLSCompliant(true)]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyCompany("Selenium Committers")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © Software Freedom Conservancy 2018")]
[assembly: AssemblyDescription("Selenium WebDriver .NET Bindings support classes")]
[assembly: AssemblyFileVersion("3.141.0.0")]
[assembly: AssemblyInformationalVersion("3.141.0")]
[assembly: AssemblyProduct("Selenium")]
[assembly: AssemblyTitle("Selenium WebDriver Support")]
[assembly: AssemblyVersion("3.141.0.0")]
namespace OpenQA.Selenium.Support.UI
{
	[Obsolete("The ExpectedConditions implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)")]
	public sealed class ExpectedConditions
	{
		private ExpectedConditions()
		{
		}

		public static Func<IWebDriver, bool> TitleIs(string title)
		{
			return (IWebDriver driver) => title == driver.Title;
		}

		public static Func<IWebDriver, bool> TitleContains(string title)
		{
			return (IWebDriver driver) => driver.Title.Contains(title);
		}

		public static Func<IWebDriver, bool> UrlToBe(string url)
		{
			return (IWebDriver driver) => driver.Url.ToLowerInvariant().Equals(url.ToLowerInvariant());
		}

		public static Func<IWebDriver, bool> UrlContains(string fraction)
		{
			return (IWebDriver driver) => driver.Url.ToLowerInvariant().Contains(fraction.ToLowerInvariant());
		}

		public static Func<IWebDriver, bool> UrlMatches(string regex)
		{
			return delegate(IWebDriver driver)
			{
				string url = driver.Url;
				return new Regex(regex, RegexOptions.IgnoreCase).Match(url).Success;
			};
		}

		public static Func<IWebDriver, IWebElement> ElementExists(By locator)
		{
			return (IWebDriver driver) => driver.FindElement(locator);
		}

		public static Func<IWebDriver, IWebElement> ElementIsVisible(By locator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return ElementIfVisible(driver.FindElement(locator));
				}
				catch (StaleElementReferenceException)
				{
					return (IWebElement)null;
				}
			};
		}

		public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> VisibilityOfAllElementsLocatedBy(By locator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(locator);
					if (readOnlyCollection.Any((IWebElement element) => !element.Displayed))
					{
						return (ReadOnlyCollection<IWebElement>)null;
					}
					return readOnlyCollection.Any() ? readOnlyCollection : null;
				}
				catch (StaleElementReferenceException)
				{
					return (ReadOnlyCollection<IWebElement>)null;
				}
			};
		}

		public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> VisibilityOfAllElementsLocatedBy(ReadOnlyCollection<IWebElement> elements)
		{
			return delegate
			{
				try
				{
					if (elements.Any((IWebElement element) => !element.Displayed))
					{
						return (ReadOnlyCollection<IWebElement>)null;
					}
					return elements.Any() ? elements : null;
				}
				catch (StaleElementReferenceException)
				{
					return (ReadOnlyCollection<IWebElement>)null;
				}
			};
		}

		public static Func<IWebDriver, ReadOnlyCollection<IWebElement>> PresenceOfAllElementsLocatedBy(By locator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(locator);
					return readOnlyCollection.Any() ? readOnlyCollection : null;
				}
				catch (StaleElementReferenceException)
				{
					return (ReadOnlyCollection<IWebElement>)null;
				}
			};
		}

		public static Func<IWebDriver, bool> TextToBePresentInElement(IWebElement element, string text)
		{
			return delegate
			{
				try
				{
					return element.Text.Contains(text);
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			};
		}

		public static Func<IWebDriver, bool> TextToBePresentInElementLocated(By locator, string text)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return driver.FindElement(locator).Text.Contains(text);
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			};
		}

		public static Func<IWebDriver, bool> TextToBePresentInElementValue(IWebElement element, string text)
		{
			return delegate
			{
				try
				{
					return element.GetAttribute("value")?.Contains(text) ?? false;
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			};
		}

		public static Func<IWebDriver, bool> TextToBePresentInElementValue(By locator, string text)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return driver.FindElement(locator).GetAttribute("value")?.Contains(text) ?? false;
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			};
		}

		public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(string frameLocator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return driver.SwitchTo().Frame(frameLocator);
				}
				catch (NoSuchFrameException)
				{
					return (IWebDriver)null;
				}
			};
		}

		public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(By locator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					IWebElement frameElement = driver.FindElement(locator);
					return driver.SwitchTo().Frame(frameElement);
				}
				catch (NoSuchFrameException)
				{
					return (IWebDriver)null;
				}
			};
		}

		public static Func<IWebDriver, bool> InvisibilityOfElementLocated(By locator)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return !driver.FindElement(locator).Displayed;
				}
				catch (NoSuchElementException)
				{
					return true;
				}
				catch (StaleElementReferenceException)
				{
					return true;
				}
			};
		}

		public static Func<IWebDriver, bool> InvisibilityOfElementWithText(By locator, string text)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					string text2 = driver.FindElement(locator).Text;
					if (string.IsNullOrEmpty(text2))
					{
						return true;
					}
					return !text2.Equals(text);
				}
				catch (NoSuchElementException)
				{
					return true;
				}
				catch (StaleElementReferenceException)
				{
					return true;
				}
			};
		}

		public static Func<IWebDriver, IWebElement> ElementToBeClickable(By locator)
		{
			return delegate(IWebDriver driver)
			{
				IWebElement webElement = ElementIfVisible(driver.FindElement(locator));
				try
				{
					if (webElement != null && webElement.Enabled)
					{
						return webElement;
					}
					return (IWebElement)null;
				}
				catch (StaleElementReferenceException)
				{
					return (IWebElement)null;
				}
			};
		}

		public static Func<IWebDriver, IWebElement> ElementToBeClickable(IWebElement element)
		{
			return delegate
			{
				try
				{
					if (element != null && element.Displayed && element.Enabled)
					{
						return element;
					}
					return (IWebElement)null;
				}
				catch (StaleElementReferenceException)
				{
					return (IWebElement)null;
				}
			};
		}

		public static Func<IWebDriver, bool> StalenessOf(IWebElement element)
		{
			return delegate
			{
				try
				{
					return element == null || !element.Enabled;
				}
				catch (StaleElementReferenceException)
				{
					return true;
				}
			};
		}

		public static Func<IWebDriver, bool> ElementToBeSelected(IWebElement element)
		{
			return ElementSelectionStateToBe(element, selected: true);
		}

		public static Func<IWebDriver, bool> ElementToBeSelected(IWebElement element, bool selected)
		{
			return (IWebDriver driver) => element.Selected == selected;
		}

		public static Func<IWebDriver, bool> ElementSelectionStateToBe(IWebElement element, bool selected)
		{
			return (IWebDriver driver) => element.Selected == selected;
		}

		public static Func<IWebDriver, bool> ElementToBeSelected(By locator)
		{
			return ElementSelectionStateToBe(locator, selected: true);
		}

		public static Func<IWebDriver, bool> ElementSelectionStateToBe(By locator, bool selected)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return driver.FindElement(locator).Selected == selected;
				}
				catch (StaleElementReferenceException)
				{
					return false;
				}
			};
		}

		public static Func<IWebDriver, IAlert> AlertIsPresent()
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					return driver.SwitchTo().Alert();
				}
				catch (NoAlertPresentException)
				{
					return (IAlert)null;
				}
			};
		}

		public static Func<IWebDriver, bool> AlertState(bool state)
		{
			return delegate(IWebDriver driver)
			{
				try
				{
					driver.SwitchTo().Alert();
					return state;
				}
				catch (NoAlertPresentException)
				{
					return !state;
				}
			};
		}

		private static IWebElement ElementIfVisible(IWebElement element)
		{
			if (!element.Displayed)
			{
				return null;
			}
			return element;
		}
	}
	public interface ILoadableComponent
	{
		ILoadableComponent Load();
	}
	[Serializable]
	public class LoadableComponentException : WebDriverException
	{
		public LoadableComponentException()
		{
		}

		public LoadableComponentException(string message)
			: base(message)
		{
		}

		public LoadableComponentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected LoadableComponentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
	public abstract class LoadableComponent<T> : ILoadableComponent where T : LoadableComponent<T>
	{
		public virtual string UnableToLoadMessage { get; set; }

		protected bool IsLoaded
		{
			get
			{
				bool flag = false;
				try
				{
					return EvaluateLoadedStatus();
				}
				catch (WebDriverException)
				{
					return false;
				}
			}
		}

		public virtual T Load()
		{
			if (IsLoaded)
			{
				return (T)this;
			}
			TryLoad();
			if (!IsLoaded)
			{
				throw new LoadableComponentException(UnableToLoadMessage);
			}
			return (T)this;
		}

		ILoadableComponent ILoadableComponent.Load()
		{
			return Load();
		}

		protected virtual void HandleLoadError(WebDriverException ex)
		{
		}

		protected abstract void ExecuteLoad();

		protected abstract bool EvaluateLoadedStatus();

		protected T TryLoad()
		{
			try
			{
				ExecuteLoad();
			}
			catch (WebDriverException ex)
			{
				HandleLoadError(ex);
			}
			return (T)this;
		}
	}
	public class PopupWindowFinder
	{
		private readonly IWebDriver driver;

		private readonly TimeSpan timeout;

		private readonly TimeSpan sleepInterval;

		private static TimeSpan DefaultTimeout => TimeSpan.FromSeconds(5.0);

		private static TimeSpan DefaultSleepInterval => TimeSpan.FromMilliseconds(250.0);

		public PopupWindowFinder(IWebDriver driver)
			: this(driver, DefaultTimeout, DefaultSleepInterval)
		{
		}

		public PopupWindowFinder(IWebDriver driver, TimeSpan timeout)
			: this(driver, timeout, DefaultSleepInterval)
		{
		}

		public PopupWindowFinder(IWebDriver driver, TimeSpan timeout, TimeSpan sleepInterval)
		{
			this.driver = driver;
			this.timeout = timeout;
			this.sleepInterval = sleepInterval;
		}

		public string Click(IWebElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element", "element cannot be null");
			}
			return Invoke(delegate
			{
				element.Click();
			});
		}

		public string Invoke(Action popupMethod)
		{
			if (popupMethod == null)
			{
				throw new ArgumentNullException("popupMethod", "popupMethod cannot be null");
			}
			IList<string> existingHandles = driver.WindowHandles;
			popupMethod();
			return new WebDriverWait(new SystemClock(), driver, timeout, sleepInterval).Until(delegate
			{
				string result = null;
				IList<string> difference = GetDifference(existingHandles, driver.WindowHandles);
				if (difference.Count > 0)
				{
					result = difference[0];
				}
				return result;
			});
		}

		private static IList<string> GetDifference(IList<string> existingHandles, IList<string> currentHandles)
		{
			return currentHandles.Except(existingHandles).ToList();
		}
	}
	public class SelectElement : IWrapsElement
	{
		private readonly IWebElement element;

		public IWebElement WrappedElement => element;

		public bool IsMultiple { get; private set; }

		public IList<IWebElement> Options => element.FindElements(By.TagName("option"));

		public IWebElement SelectedOption
		{
			get
			{
				foreach (IWebElement option in Options)
				{
					if (option.Selected)
					{
						return option;
					}
				}
				throw new NoSuchElementException("No option is selected");
			}
		}

		public IList<IWebElement> AllSelectedOptions
		{
			get
			{
				List<IWebElement> list = new List<IWebElement>();
				foreach (IWebElement option in Options)
				{
					if (option.Selected)
					{
						list.Add(option);
					}
				}
				return list;
			}
		}

		public SelectElement(IWebElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element", "element cannot be null");
			}
			if (string.IsNullOrEmpty(element.TagName) || string.Compare(element.TagName, "select", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new UnexpectedTagNameException("select", element.TagName);
			}
			this.element = element;
			string attribute = element.GetAttribute("multiple");
			IsMultiple = attribute != null && attribute.ToLowerInvariant() != "false";
		}

		public void SelectByText(string text, bool partialMatch = false)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text", "text must not be null");
			}
			bool flag = false;
			IList<IWebElement> list = (partialMatch ? element.FindElements(By.XPath(".//option[contains(normalize-space(.),  " + EscapeQuotes(text) + ")]")) : element.FindElements(By.XPath(".//option[normalize-space(.) = " + EscapeQuotes(text) + "]")));
			foreach (IWebElement item in list)
			{
				SetSelected(item, select: true);
				if (!IsMultiple)
				{
					return;
				}
				flag = true;
			}
			if (list.Count == 0 && text.Contains(" "))
			{
				string longestSubstringWithoutSpace = GetLongestSubstringWithoutSpace(text);
				IList<IWebElement> list2 = ((!string.IsNullOrEmpty(longestSubstringWithoutSpace)) ? element.FindElements(By.XPath(".//option[contains(., " + EscapeQuotes(longestSubstringWithoutSpace) + ")]")) : element.FindElements(By.TagName("option")));
				foreach (IWebElement item2 in list2)
				{
					if (text == item2.Text)
					{
						SetSelected(item2, select: true);
						if (!IsMultiple)
						{
							return;
						}
						flag = true;
					}
				}
			}
			if (!flag)
			{
				throw new NoSuchElementException("Cannot locate element with text: " + text);
			}
		}

		public void SelectByValue(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(".//option[@value = ");
			stringBuilder.Append(EscapeQuotes(value));
			stringBuilder.Append("]");
			ReadOnlyCollection<IWebElement> readOnlyCollection = element.FindElements(By.XPath(stringBuilder.ToString()));
			bool flag = false;
			foreach (IWebElement item in (IEnumerable<IWebElement>)readOnlyCollection)
			{
				SetSelected(item, select: true);
				if (!IsMultiple)
				{
					return;
				}
				flag = true;
			}
			if (!flag)
			{
				throw new NoSuchElementException("Cannot locate option with value: " + value);
			}
		}

		public void SelectByIndex(int index)
		{
			string text = index.ToString(CultureInfo.InvariantCulture);
			foreach (IWebElement option in Options)
			{
				if (option.GetAttribute("index") == text)
				{
					SetSelected(option, select: true);
					return;
				}
			}
			throw new NoSuchElementException("Cannot locate option with index: " + index);
		}

		public void DeselectAll()
		{
			if (!IsMultiple)
			{
				throw new InvalidOperationException("You may only deselect all options if multi-select is supported");
			}
			foreach (IWebElement option in Options)
			{
				SetSelected(option, select: false);
			}
		}

		public void DeselectByText(string text)
		{
			if (!IsMultiple)
			{
				throw new InvalidOperationException("You may only deselect option if multi-select is supported");
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder(".//option[normalize-space(.) = ");
			stringBuilder.Append(EscapeQuotes(text));
			stringBuilder.Append("]");
			foreach (IWebElement item in (IEnumerable<IWebElement>)element.FindElements(By.XPath(stringBuilder.ToString())))
			{
				SetSelected(item, select: false);
				flag = true;
			}
			if (!flag)
			{
				throw new NoSuchElementException("Cannot locate option with text: " + text);
			}
		}

		public void DeselectByValue(string value)
		{
			if (!IsMultiple)
			{
				throw new InvalidOperationException("You may only deselect option if multi-select is supported");
			}
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder(".//option[@value = ");
			stringBuilder.Append(EscapeQuotes(value));
			stringBuilder.Append("]");
			foreach (IWebElement item in (IEnumerable<IWebElement>)element.FindElements(By.XPath(stringBuilder.ToString())))
			{
				SetSelected(item, select: false);
				flag = true;
			}
			if (!flag)
			{
				throw new NoSuchElementException("Cannot locate option with value: " + value);
			}
		}

		public void DeselectByIndex(int index)
		{
			if (!IsMultiple)
			{
				throw new InvalidOperationException("You may only deselect option if multi-select is supported");
			}
			string text = index.ToString(CultureInfo.InvariantCulture);
			foreach (IWebElement option in Options)
			{
				if (text == option.GetAttribute("index"))
				{
					SetSelected(option, select: false);
					return;
				}
			}
			throw new NoSuchElementException("Cannot locate option with index: " + index);
		}

		private static string EscapeQuotes(string toEscape)
		{
			if (toEscape.IndexOf("\"", StringComparison.OrdinalIgnoreCase) > -1 && toEscape.IndexOf("'", StringComparison.OrdinalIgnoreCase) > -1)
			{
				bool flag = false;
				if (toEscape.LastIndexOf("\"", StringComparison.OrdinalIgnoreCase) == toEscape.Length - 1)
				{
					flag = true;
				}
				List<string> list = new List<string>(toEscape.Split(new char[1] { '"' }));
				if (flag && string.IsNullOrEmpty(list[list.Count - 1]))
				{
					list.RemoveAt(list.Count - 1);
				}
				StringBuilder stringBuilder = new StringBuilder("concat(");
				for (int i = 0; i < list.Count; i++)
				{
					stringBuilder.Append("\"").Append(list[i]).Append("\"");
					if (i == list.Count - 1)
					{
						if (flag)
						{
							stringBuilder.Append(", '\"')");
						}
						else
						{
							stringBuilder.Append(")");
						}
					}
					else
					{
						stringBuilder.Append(", '\"', ");
					}
				}
				return stringBuilder.ToString();
			}
			if (toEscape.IndexOf("\"", StringComparison.OrdinalIgnoreCase) > -1)
			{
				return string.Format(CultureInfo.InvariantCulture, "'{0}'", new object[1] { toEscape });
			}
			return string.Format(CultureInfo.InvariantCulture, "\"{0}\"", new object[1] { toEscape });
		}

		private static string GetLongestSubstringWithoutSpace(string s)
		{
			string text = string.Empty;
			string[] array = s.Split(new char[1] { ' ' });
			foreach (string text2 in array)
			{
				if (text2.Length > text.Length)
				{
					text = text2;
				}
			}
			return text;
		}

		private static void SetSelected(IWebElement option, bool select)
		{
			bool selected = option.Selected;
			if ((!selected && select) || (selected && !select))
			{
				option.Click();
			}
		}
	}
	public abstract class SlowLoadableComponent<T> : LoadableComponent<T> where T : SlowLoadableComponent<T>
	{
		private readonly IClock clock;

		private readonly TimeSpan timeout;

		private TimeSpan sleepInterval = TimeSpan.FromMilliseconds(200.0);

		public TimeSpan SleepInterval
		{
			get
			{
				return sleepInterval;
			}
			set
			{
				sleepInterval = value;
			}
		}

		protected SlowLoadableComponent(TimeSpan timeout)
			: this(timeout, (IClock)new SystemClock())
		{
		}

		protected SlowLoadableComponent(TimeSpan timeout, IClock clock)
		{
			this.clock = clock;
			this.timeout = timeout;
		}

		public override T Load()
		{
			if (base.IsLoaded)
			{
				return (T)this;
			}
			TryLoad();
			DateTime otherDateTime = clock.LaterBy(timeout);
			while (clock.IsNowBefore(otherDateTime))
			{
				if (base.IsLoaded)
				{
					return (T)this;
				}
				HandleErrors();
				Wait();
			}
			if (base.IsLoaded)
			{
				return (T)this;
			}
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			object[] array = new object[1];
			TimeSpan timeSpan = timeout;
			array[0] = timeSpan.TotalSeconds;
			throw new WebDriverTimeoutException(string.Format(invariantCulture, "Timed out after {0} seconds.", array));
		}

		protected virtual void HandleErrors()
		{
		}

		private void Wait()
		{
			Thread.Sleep(sleepInterval);
		}
	}
	[Serializable]
	public class UnexpectedTagNameException : WebDriverException
	{
		public UnexpectedTagNameException(string expected, string actual)
			: base(string.Format(CultureInfo.InvariantCulture, "Element should have been {0} but was {1}", new object[2] { expected, actual }))
		{
		}

		public UnexpectedTagNameException()
		{
		}

		public UnexpectedTagNameException(string message)
			: base(message)
		{
		}

		public UnexpectedTagNameException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected UnexpectedTagNameException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
namespace OpenQA.Selenium.Support.PageObjects
{
	public class ByAll : By
	{
		private readonly By[] bys;

		public ByAll(params By[] bys)
		{
			this.bys = bys;
		}

		public override IWebElement FindElement(ISearchContext context)
		{
			ReadOnlyCollection<IWebElement> readOnlyCollection = FindElements(context);
			if (readOnlyCollection.Count == 0)
			{
				throw new NoSuchElementException("Cannot locate an element using " + ToString());
			}
			return readOnlyCollection[0];
		}

		public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
		{
			if (bys.Length == 0)
			{
				return new List<IWebElement>().AsReadOnly();
			}
			IEnumerable<IWebElement> enumerable = null;
			By[] array = bys;
			foreach (By by in array)
			{
				ReadOnlyCollection<IWebElement> readOnlyCollection = by.FindElements(context);
				if (readOnlyCollection.Count == 0)
				{
					return new List<IWebElement>().AsReadOnly();
				}
				enumerable = ((enumerable != null) ? enumerable.Intersect(by.FindElements(context)) : readOnlyCollection);
			}
			return enumerable.ToList().AsReadOnly();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			By[] array = bys;
			foreach (By value in array)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(value);
			}
			return string.Format(CultureInfo.InvariantCulture, "By.All([{0}])", new object[1] { stringBuilder.ToString() });
		}
	}
	public class ByChained : By
	{
		private readonly By[] bys;

		public ByChained(params By[] bys)
		{
			this.bys = bys;
		}

		public override IWebElement FindElement(ISearchContext context)
		{
			ReadOnlyCollection<IWebElement> readOnlyCollection = FindElements(context);
			if (readOnlyCollection.Count == 0)
			{
				throw new NoSuchElementException("Cannot locate an element using " + ToString());
			}
			return readOnlyCollection[0];
		}

		public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
		{
			if (bys.Length == 0)
			{
				return new List<IWebElement>().AsReadOnly();
			}
			List<IWebElement> list = null;
			By[] array = bys;
			foreach (By by in array)
			{
				List<IWebElement> list2 = new List<IWebElement>();
				if (list == null)
				{
					list2.AddRange(by.FindElements(context));
				}
				else
				{
					foreach (IWebElement item in list)
					{
						list2.AddRange(item.FindElements(by));
					}
				}
				list = list2;
			}
			return list.AsReadOnly();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			By[] array = bys;
			foreach (By value in array)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(value);
			}
			return string.Format(CultureInfo.InvariantCulture, "By.Chained([{0}])", new object[1] { stringBuilder.ToString() });
		}
	}
	internal static class ByFactory
	{
		public static By From(FindsByAttribute attribute)
		{
			How how = attribute.How;
			string @using = attribute.Using;
			switch (how)
			{
			case How.Id:
				return By.Id(@using);
			case How.Name:
				return By.Name(@using);
			case How.TagName:
				return By.TagName(@using);
			case How.ClassName:
				return By.ClassName(@using);
			case How.CssSelector:
				return By.CssSelector(@using);
			case How.LinkText:
				return By.LinkText(@using);
			case How.PartialLinkText:
				return By.PartialLinkText(@using);
			case How.XPath:
				return By.XPath(@using);
			case How.Custom:
			{
				if (attribute.CustomFinderType == null)
				{
					throw new ArgumentException("Cannot use How.Custom without supplying a custom finder type");
				}
				if (!attribute.CustomFinderType.IsSubclassOf(typeof(By)))
				{
					throw new ArgumentException("Custom finder type must be a descendent of the By class");
				}
				ConstructorInfo? constructor = attribute.CustomFinderType.GetConstructor(new Type[1] { typeof(string) });
				if (constructor == null)
				{
					throw new ArgumentException("Custom finder type must expose a public constructor with a string argument");
				}
				return constructor.Invoke(new object[1] { @using }) as By;
			}
			default:
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Did not know how to construct How from how {0}, using {1}", new object[2] { how, @using }));
			}
		}
	}
	public class ByIdOrName : By
	{
		private string elementIdentifier = string.Empty;

		private By idFinder;

		private By nameFinder;

		public ByIdOrName(string elementIdentifier)
		{
			if (string.IsNullOrEmpty(elementIdentifier))
			{
				throw new ArgumentException("element identifier cannot be null or the empty string", "elementIdentifier");
			}
			this.elementIdentifier = elementIdentifier;
			idFinder = By.Id(this.elementIdentifier);
			nameFinder = By.Name(this.elementIdentifier);
		}

		public override IWebElement FindElement(ISearchContext context)
		{
			try
			{
				return idFinder.FindElement(context);
			}
			catch (NoSuchElementException)
			{
				return nameFinder.FindElement(context);
			}
		}

		public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
		{
			List<IWebElement> list = new List<IWebElement>();
			list.AddRange(idFinder.FindElements(context));
			list.AddRange(nameFinder.FindElements(context));
			return list.AsReadOnly();
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "ByIdOrName([{0}])", new object[1] { elementIdentifier });
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class CacheLookupAttribute : Attribute
	{
	}
	public class DefaultElementLocator : IElementLocator
	{
		private ISearchContext searchContext;

		public ISearchContext SearchContext => searchContext;

		public DefaultElementLocator(ISearchContext searchContext)
		{
			this.searchContext = searchContext;
		}

		public IWebElement LocateElement(IEnumerable<By> bys)
		{
			if (bys == null)
			{
				throw new ArgumentNullException("bys", "List of criteria may not be null");
			}
			string text = null;
			foreach (By by in bys)
			{
				try
				{
					return searchContext.FindElement(by);
				}
				catch (NoSuchElementException)
				{
					text = ((text == null) ? "Could not find element by: " : (text + ", or: ")) + by;
				}
			}
			throw new NoSuchElementException(text);
		}

		public ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
		{
			if (bys == null)
			{
				throw new ArgumentNullException("bys", "List of criteria may not be null");
			}
			List<IWebElement> list = new List<IWebElement>();
			foreach (By by in bys)
			{
				ReadOnlyCollection<IWebElement> collection = searchContext.FindElements(by);
				list.AddRange(collection);
			}
			return list.AsReadOnly();
		}
	}
	public class DefaultPageObjectMemberDecorator : IPageObjectMemberDecorator
	{
		private static List<Type> interfacesToBeProxied;

		private static Type interfaceProxyType;

		private static List<Type> InterfacesToBeProxied
		{
			get
			{
				if (interfacesToBeProxied == null)
				{
					interfacesToBeProxied = new List<Type>();
					interfacesToBeProxied.Add(typeof(IWebElement));
					interfacesToBeProxied.Add(typeof(ILocatable));
					interfacesToBeProxied.Add(typeof(IWrapsElement));
				}
				return interfacesToBeProxied;
			}
		}

		private static Type InterfaceProxyType
		{
			get
			{
				if (interfaceProxyType == null)
				{
					interfaceProxyType = CreateTypeForASingleElement();
				}
				return interfaceProxyType;
			}
		}

		public object Decorate(MemberInfo member, IElementLocator locator)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			PropertyInfo propertyInfo = member as PropertyInfo;
			Type memberType = null;
			if (fieldInfo != null)
			{
				memberType = fieldInfo.FieldType;
			}
			bool flag = false;
			if (propertyInfo != null)
			{
				flag = propertyInfo.CanWrite;
				memberType = propertyInfo.PropertyType;
			}
			if ((fieldInfo == null) & (propertyInfo == null || !flag))
			{
				return null;
			}
			IList<By> list = CreateLocatorList(member);
			if (list.Count > 0)
			{
				bool cache = ShouldCacheLookup(member);
				return CreateProxyObject(memberType, locator, list, cache);
			}
			return null;
		}

		protected static bool ShouldCacheLookup(MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member", "memeber cannot be null");
			}
			Type typeFromHandle = typeof(CacheLookupAttribute);
			if (member.GetCustomAttributes(typeFromHandle, inherit: true).Length == 0)
			{
				return member.DeclaringType.GetCustomAttributes(typeFromHandle, inherit: true).Length != 0;
			}
			return true;
		}

		protected static ReadOnlyCollection<By> CreateLocatorList(MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member", "memeber cannot be null");
			}
			bool flag = Attribute.GetCustomAttributes(member, typeof(FindsBySequenceAttribute), inherit: true).Length != 0;
			bool flag2 = Attribute.GetCustomAttributes(member, typeof(FindsByAllAttribute), inherit: true).Length != 0;
			if (flag && flag2)
			{
				throw new ArgumentException("Cannot specify FindsBySequence and FindsByAll on the same member");
			}
			List<By> list = new List<By>();
			Attribute[] customAttributes = Attribute.GetCustomAttributes(member, typeof(FindsByAttribute), inherit: true);
			if (customAttributes.Length != 0)
			{
				Array.Sort(customAttributes);
				Attribute[] array = customAttributes;
				for (int i = 0; i < array.Length; i++)
				{
					FindsByAttribute findsByAttribute = (FindsByAttribute)array[i];
					if (findsByAttribute.Using == null)
					{
						findsByAttribute.Using = member.Name;
					}
					list.Add(findsByAttribute.Finder);
				}
				if (flag)
				{
					ByChained item = new ByChained(list.ToArray());
					list.Clear();
					list.Add(item);
				}
				if (flag2)
				{
					ByAll item2 = new ByAll(list.ToArray());
					list.Clear();
					list.Add(item2);
				}
			}
			return list.AsReadOnly();
		}

		private static object CreateProxyObject(Type memberType, IElementLocator locator, IEnumerable<By> bys, bool cache)
		{
			object result = null;
			if (memberType == typeof(IList<IWebElement>))
			{
				foreach (Type item in InterfacesToBeProxied)
				{
					if (typeof(IList<>).MakeGenericType(item).Equals(memberType))
					{
						result = WebElementListProxy.CreateProxy(memberType, locator, bys, cache);
						break;
					}
				}
			}
			else
			{
				if (!(memberType == typeof(IWebElement)))
				{
					throw new ArgumentException("Type of member '" + memberType.Name + "' is not IWebElement or IList<IWebElement>");
				}
				result = WebElementProxy.CreateProxy(InterfaceProxyType, locator, bys, cache);
			}
			return result;
		}

		private static Type CreateTypeForASingleElement()
		{
			AssemblyName assemblyName = new AssemblyName(Guid.NewGuid().ToString());
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name).DefineType(typeof(IWebElement).FullName, TypeAttributes.Public | TypeAttributes.ClassSemanticsMask | TypeAttributes.Abstract);
			foreach (Type item in InterfacesToBeProxied)
			{
				typeBuilder.AddInterfaceImplementation(item);
			}
			return typeBuilder.CreateType();
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class FindsByAllAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class FindsByAttribute : Attribute, IComparable
	{
		private By finder;

		[DefaultValue(How.Id)]
		public How How { get; set; }

		public string Using { get; set; }

		[DefaultValue(0)]
		public int Priority { get; set; }

		public Type CustomFinderType { get; set; }

		internal By Finder
		{
			get
			{
				if (finder == null)
				{
					finder = ByFactory.From(this);
				}
				return finder;
			}
			set
			{
				finder = value;
			}
		}

		public static bool operator ==(FindsByAttribute one, FindsByAttribute two)
		{
			if ((object)one == two)
			{
				return true;
			}
			if ((object)one == null || (object)two == null)
			{
				return false;
			}
			return one.Equals(two);
		}

		public static bool operator !=(FindsByAttribute one, FindsByAttribute two)
		{
			return !(one == two);
		}

		public static bool operator >(FindsByAttribute one, FindsByAttribute two)
		{
			if (one == null)
			{
				throw new ArgumentNullException("one", "Object to compare cannot be null");
			}
			return one.CompareTo(two) > 0;
		}

		public static bool operator <(FindsByAttribute one, FindsByAttribute two)
		{
			if (one == null)
			{
				throw new ArgumentNullException("one", "Object to compare cannot be null");
			}
			return one.CompareTo(two) < 0;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj", "Object to compare cannot be null");
			}
			FindsByAttribute findsByAttribute = obj as FindsByAttribute;
			if (findsByAttribute == null)
			{
				throw new ArgumentException("Object to compare must be a FindsByAttribute", "obj");
			}
			if (Priority != findsByAttribute.Priority)
			{
				return Priority - findsByAttribute.Priority;
			}
			return 0;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			FindsByAttribute findsByAttribute = obj as FindsByAttribute;
			if (findsByAttribute == null)
			{
				return false;
			}
			if (findsByAttribute.Priority != Priority)
			{
				return false;
			}
			if (findsByAttribute.Finder != Finder)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return Finder.GetHashCode();
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class FindsBySequenceAttribute : Attribute
	{
	}
	public enum How
	{
		Id,
		Name,
		TagName,
		ClassName,
		CssSelector,
		LinkText,
		PartialLinkText,
		XPath,
		Custom
	}
	public interface IElementLocator
	{
		ISearchContext SearchContext { get; }

		IWebElement LocateElement(IEnumerable<By> bys);

		ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys);
	}
	public interface IPageObjectMemberDecorator
	{
		object Decorate(MemberInfo member, IElementLocator locator);
	}
	[Obsolete("The PageFactory implementation in the .NET bindings is deprecated and will be removed in a future release. This portion of the code has been migrated to the DotNetSeleniumExtras repository on GitHub (https://github.com/DotNetSeleniumTools/DotNetSeleniumExtras)")]
	public sealed class PageFactory
	{
		private PageFactory()
		{
		}

		public static T InitElements<T>(IWebDriver driver)
		{
			return InitElements<T>(new DefaultElementLocator(driver));
		}

		public static T InitElements<T>(IElementLocator locator)
		{
			ConstructorInfo? constructor = typeof(T).GetConstructor(new Type[1] { typeof(IWebDriver) });
			if (constructor == null)
			{
				throw new ArgumentException("No constructor for the specified class containing a single argument of type IWebDriver can be found");
			}
			if (locator == null)
			{
				throw new ArgumentNullException("locator", "locator cannot be null");
			}
			if (!(locator.SearchContext is IWebDriver))
			{
				throw new ArgumentException("The search context of the element locator must implement IWebDriver", "locator");
			}
			T obj = (T)constructor.Invoke(new object[1] { locator.SearchContext as IWebDriver });
			InitElements(obj, locator);
			return obj;
		}

		public static void InitElements(ISearchContext driver, object page)
		{
			InitElements(page, new DefaultElementLocator(driver));
		}

		public static void InitElements(ISearchContext driver, object page, IPageObjectMemberDecorator decorator)
		{
			InitElements(page, new DefaultElementLocator(driver), decorator);
		}

		public static void InitElements(object page, IElementLocator locator)
		{
			InitElements(page, locator, new DefaultPageObjectMemberDecorator());
		}

		public static void InitElements(object page, IElementLocator locator, IPageObjectMemberDecorator decorator)
		{
			if (page == null)
			{
				throw new ArgumentNullException("page", "page cannot be null");
			}
			if (locator == null)
			{
				throw new ArgumentNullException("locator", "locator cannot be null");
			}
			if (decorator == null)
			{
				throw new ArgumentNullException("locator", "decorator cannot be null");
			}
			if (locator.SearchContext == null)
			{
				throw new ArgumentException("The SearchContext of the locator object cannot be null", "locator");
			}
			Type type = page.GetType();
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.Public));
			list.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
			while (type != null)
			{
				list.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic));
				list.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic));
				type = type.BaseType;
			}
			foreach (MemberInfo item in list)
			{
				object obj = decorator.Decorate(item, locator);
				if (obj != null)
				{
					FieldInfo fieldInfo = item as FieldInfo;
					PropertyInfo propertyInfo = item as PropertyInfo;
					if (fieldInfo != null)
					{
						fieldInfo.SetValue(page, obj);
					}
					else if (propertyInfo != null)
					{
						propertyInfo.SetValue(page, obj, null);
					}
				}
			}
		}
	}
	public class RetryingElementLocator : IElementLocator
	{
		private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5.0);

		private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromMilliseconds(500.0);

		private ISearchContext searchContext;

		private TimeSpan timeout;

		private TimeSpan pollingInterval;

		public ISearchContext SearchContext => searchContext;

		public RetryingElementLocator(ISearchContext searchContext)
			: this(searchContext, DefaultTimeout)
		{
		}

		public RetryingElementLocator(ISearchContext searchContext, TimeSpan timeout)
			: this(searchContext, timeout, DefaultPollingInterval)
		{
		}

		public RetryingElementLocator(ISearchContext searchContext, TimeSpan timeout, TimeSpan pollingInterval)
		{
			this.searchContext = searchContext;
			this.timeout = timeout;
			this.pollingInterval = pollingInterval;
		}

		public IWebElement LocateElement(IEnumerable<By> bys)
		{
			if (bys == null)
			{
				throw new ArgumentNullException("bys", "List of criteria may not be null");
			}
			string text = null;
			DateTime dateTime = DateTime.Now.Add(timeout);
			bool flag = DateTime.Now > dateTime;
			while (!flag)
			{
				foreach (By by in bys)
				{
					try
					{
						return SearchContext.FindElement(by);
					}
					catch (NoSuchElementException)
					{
						text = ((text == null) ? "Could not find element by: " : (text + ", or: ")) + by;
					}
				}
				flag = DateTime.Now > dateTime;
				if (!flag)
				{
					Thread.Sleep(pollingInterval);
				}
			}
			throw new NoSuchElementException(text);
		}

		public ReadOnlyCollection<IWebElement> LocateElements(IEnumerable<By> bys)
		{
			if (bys == null)
			{
				throw new ArgumentNullException("bys", "List of criteria may not be null");
			}
			List<IWebElement> list = new List<IWebElement>();
			DateTime dateTime = DateTime.Now.Add(timeout);
			bool flag = DateTime.Now > dateTime;
			while (!flag)
			{
				foreach (By by in bys)
				{
					ReadOnlyCollection<IWebElement> collection = SearchContext.FindElements(by);
					list.AddRange(collection);
				}
				flag = list.Count != 0 || DateTime.Now > dateTime;
				if (!flag)
				{
					Thread.Sleep(pollingInterval);
				}
			}
			return list.AsReadOnly();
		}
	}
	internal abstract class WebDriverObjectProxy : RealProxy
	{
		private readonly IElementLocator locator;

		private readonly IEnumerable<By> bys;

		private readonly bool cache;

		protected IElementLocator Locator => locator;

		protected IEnumerable<By> Bys => bys;

		protected bool Cache => cache;

		protected WebDriverObjectProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, bool cache)
			: base(classToProxy)
		{
			this.locator = locator;
			this.bys = bys;
			this.cache = cache;
		}

		protected static ReturnMessage InvokeMethod(IMethodCallMessage msg, object representedValue)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			if (msg == null)
			{
				throw new ArgumentNullException("msg", "The message containing invocation information cannot be null");
			}
			return new ReturnMessage((((IMethodMessage)msg).MethodBase as MethodInfo).Invoke(representedValue, ((IMethodMessage)msg).Args), (object[])null, 0, ((IMethodMessage)msg).LogicalCallContext, msg);
		}
	}
	internal class WebElementListProxy : WebDriverObjectProxy
	{
		private List<IWebElement> collection;

		private List<IWebElement> ElementList
		{
			get
			{
				if (!base.Cache || collection == null)
				{
					collection = new List<IWebElement>();
					collection.AddRange(base.Locator.LocateElements(base.Bys));
				}
				return collection;
			}
		}

		private WebElementListProxy(Type typeToBeProxied, IElementLocator locator, IEnumerable<By> bys, bool cache)
			: base(typeToBeProxied, locator, bys, cache)
		{
		}

		public static object CreateProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
		{
			return ((RealProxy)new WebElementListProxy(classToProxy, locator, bys, cacheLookups)).GetTransparentProxy();
		}

		public override IMessage Invoke(IMessage msg)
		{
			List<IWebElement> elementList = ElementList;
			return (IMessage)(object)WebDriverObjectProxy.InvokeMethod((IMethodCallMessage)(object)((msg is IMethodCallMessage) ? msg : null), elementList);
		}
	}
	internal sealed class WebElementProxy : WebDriverObjectProxy, IWrapsElement
	{
		private IWebElement cachedElement;

		public IWebElement WrappedElement => Element;

		private IWebElement Element
		{
			get
			{
				if (!base.Cache || cachedElement == null)
				{
					cachedElement = base.Locator.LocateElement(base.Bys);
				}
				return cachedElement;
			}
		}

		private WebElementProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, bool cache)
			: base(classToProxy, locator, bys, cache)
		{
		}

		public static object CreateProxy(Type classToProxy, IElementLocator locator, IEnumerable<By> bys, bool cacheLookups)
		{
			return ((RealProxy)new WebElementProxy(classToProxy, locator, bys, cacheLookups)).GetTransparentProxy();
		}

		public override IMessage Invoke(IMessage msg)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			IWebElement element = Element;
			IMethodCallMessage val = (IMethodCallMessage)(object)((msg is IMethodCallMessage) ? msg : null);
			if (typeof(IWrapsElement).IsAssignableFrom((((IMethodMessage)val).MethodBase as MethodInfo).DeclaringType))
			{
				return (IMessage)new ReturnMessage((object)element, (object[])null, 0, ((IMethodMessage)val).LogicalCallContext, val);
			}
			return (IMessage)(object)WebDriverObjectProxy.InvokeMethod(val, element);
		}
	}
}
namespace OpenQA.Selenium.Support.Extensions
{
	public static class WebDriverExtensions
	{
		public static Screenshot TakeScreenshot(this IWebDriver driver)
		{
			if (!(driver is ITakesScreenshot takesScreenshot))
			{
				if (!(driver is IHasCapabilities hasCapabilities))
				{
					throw new WebDriverException("Driver does not implement ITakesScreenshot or IHasCapabilities");
				}
				if (!hasCapabilities.Capabilities.HasCapability(CapabilityType.TakesScreenshot) || !(bool)hasCapabilities.Capabilities.GetCapability(CapabilityType.TakesScreenshot))
				{
					throw new WebDriverException("Driver capabilities do not support taking screenshots");
				}
				return new Screenshot(((driver.GetType().GetMethod("Execute", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(driver, new object[2]
				{
					DriverCommand.Screenshot,
					null
				}) as Response) ?? throw new WebDriverException("Unexpected failure getting screenshot; response was not in the proper format.")).Value.ToString());
			}
			return takesScreenshot.GetScreenshot();
		}

		public static void ExecuteJavaScript(this IWebDriver driver, string script, params object[] args)
		{
			ExecuteJavaScriptInternal(driver, script, args);
		}

		public static T ExecuteJavaScript<T>(this IWebDriver driver, string script, params object[] args)
		{
			object obj = ExecuteJavaScriptInternal(driver, script, args);
			T result = default(T);
			Type typeFromHandle = typeof(T);
			if (obj == null)
			{
				if (typeFromHandle.IsValueType && Nullable.GetUnderlyingType(typeFromHandle) == null)
				{
					throw new WebDriverException("Script returned null, but desired type is a value type");
				}
				return result;
			}
			if (!typeFromHandle.IsInstanceOfType(obj))
			{
				throw new WebDriverException("Script returned a value, but the result could not be cast to the desired type");
			}
			return (T)obj;
		}

		private static object ExecuteJavaScriptInternal(IWebDriver driver, string script, object[] args)
		{
			return ((driver as IJavaScriptExecutor) ?? throw new WebDriverException("Driver does not implement IJavaScriptExecutor")).ExecuteScript(script, args);
		}
	}
}
namespace OpenQA.Selenium.Support.Events
{
	public class EventFiringWebDriver : IWebDriver, ISearchContext, IDisposable, IJavaScriptExecutor, ITakesScreenshot, IWrapsDriver
	{
		private class EventFiringNavigation : INavigation
		{
			private EventFiringWebDriver parentDriver;

			private INavigation wrappedNavigation;

			public EventFiringNavigation(EventFiringWebDriver driver)
			{
				parentDriver = driver;
				wrappedNavigation = parentDriver.WrappedDriver.Navigate();
			}

			public void Back()
			{
				try
				{
					WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(parentDriver);
					parentDriver.OnNavigatingBack(e);
					wrappedNavigation.Back();
					parentDriver.OnNavigatedBack(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void Forward()
			{
				try
				{
					WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(parentDriver);
					parentDriver.OnNavigatingForward(e);
					wrappedNavigation.Forward();
					parentDriver.OnNavigatedForward(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void GoToUrl(string url)
			{
				try
				{
					WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(parentDriver, url);
					parentDriver.OnNavigating(e);
					wrappedNavigation.GoToUrl(url);
					parentDriver.OnNavigated(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void GoToUrl(Uri url)
			{
				if (url == null)
				{
					throw new ArgumentNullException("url", "url cannot be null");
				}
				try
				{
					WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(parentDriver, url.ToString());
					parentDriver.OnNavigating(e);
					wrappedNavigation.GoToUrl(url);
					parentDriver.OnNavigated(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void Refresh()
			{
				try
				{
					wrappedNavigation.Refresh();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}
		}

		private class EventFiringOptions : IOptions
		{
			private IOptions wrappedOptions;

			public ICookieJar Cookies => wrappedOptions.Cookies;

			public IWindow Window => wrappedOptions.Window;

			public ILogs Logs => wrappedOptions.Logs;

			public EventFiringOptions(EventFiringWebDriver driver)
			{
				wrappedOptions = driver.WrappedDriver.Manage();
			}

			public ITimeouts Timeouts()
			{
				return new EventFiringTimeouts(wrappedOptions);
			}
		}

		private class EventFiringTargetLocator : ITargetLocator
		{
			private ITargetLocator wrappedLocator;

			private EventFiringWebDriver parentDriver;

			public EventFiringTargetLocator(EventFiringWebDriver driver)
			{
				parentDriver = driver;
				wrappedLocator = parentDriver.WrappedDriver.SwitchTo();
			}

			public IWebDriver Frame(int frameIndex)
			{
				IWebDriver webDriver = null;
				try
				{
					return wrappedLocator.Frame(frameIndex);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebDriver Frame(string frameName)
			{
				IWebDriver webDriver = null;
				try
				{
					return wrappedLocator.Frame(frameName);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebDriver Frame(IWebElement frameElement)
			{
				IWebDriver webDriver = null;
				try
				{
					IWrapsElement wrapsElement = frameElement as IWrapsElement;
					return wrappedLocator.Frame(wrapsElement.WrappedElement);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebDriver ParentFrame()
			{
				IWebDriver webDriver = null;
				try
				{
					return wrappedLocator.ParentFrame();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebDriver Window(string windowName)
			{
				IWebDriver webDriver = null;
				try
				{
					return wrappedLocator.Window(windowName);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebDriver DefaultContent()
			{
				IWebDriver webDriver = null;
				try
				{
					return wrappedLocator.DefaultContent();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebElement ActiveElement()
			{
				IWebElement webElement = null;
				try
				{
					return wrappedLocator.ActiveElement();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IAlert Alert()
			{
				IAlert alert = null;
				try
				{
					return wrappedLocator.Alert();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}
		}

		private class EventFiringTimeouts : ITimeouts
		{
			private ITimeouts wrappedTimeouts;

			public TimeSpan ImplicitWait
			{
				get
				{
					return wrappedTimeouts.ImplicitWait;
				}
				set
				{
					wrappedTimeouts.ImplicitWait = value;
				}
			}

			public TimeSpan AsynchronousJavaScript
			{
				get
				{
					return wrappedTimeouts.AsynchronousJavaScript;
				}
				set
				{
					wrappedTimeouts.AsynchronousJavaScript = value;
				}
			}

			public TimeSpan PageLoad
			{
				get
				{
					return wrappedTimeouts.PageLoad;
				}
				set
				{
					wrappedTimeouts.PageLoad = value;
				}
			}

			public EventFiringTimeouts(IOptions options)
			{
				wrappedTimeouts = options.Timeouts();
			}
		}

		private class EventFiringWebElement : IWebElement, ISearchContext, IWrapsElement
		{
			private IWebElement underlyingElement;

			private EventFiringWebDriver parentDriver;

			public IWebElement WrappedElement => underlyingElement;

			public string TagName
			{
				get
				{
					string empty = string.Empty;
					try
					{
						return underlyingElement.TagName;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public string Text
			{
				get
				{
					string empty = string.Empty;
					try
					{
						return underlyingElement.Text;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public bool Enabled
			{
				get
				{
					bool flag = false;
					try
					{
						return underlyingElement.Enabled;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public bool Selected
			{
				get
				{
					bool flag = false;
					try
					{
						return underlyingElement.Selected;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public Point Location
			{
				get
				{
					Point point = default(Point);
					try
					{
						return underlyingElement.Location;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public Size Size
			{
				get
				{
					Size size = default(Size);
					try
					{
						return underlyingElement.Size;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			public bool Displayed
			{
				get
				{
					bool flag = false;
					try
					{
						return underlyingElement.Displayed;
					}
					catch (Exception thrownException)
					{
						parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
						throw;
					}
				}
			}

			protected EventFiringWebDriver ParentDriver => parentDriver;

			public EventFiringWebElement(EventFiringWebDriver driver, IWebElement element)
			{
				underlyingElement = element;
				parentDriver = driver;
			}

			public void Clear()
			{
				try
				{
					WebElementValueEventArgs e = new WebElementValueEventArgs(parentDriver.WrappedDriver, underlyingElement, null);
					parentDriver.OnElementValueChanging(e);
					underlyingElement.Clear();
					parentDriver.OnElementValueChanged(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void SendKeys(string text)
			{
				try
				{
					WebElementValueEventArgs e = new WebElementValueEventArgs(parentDriver.WrappedDriver, underlyingElement, text);
					parentDriver.OnElementValueChanging(e);
					underlyingElement.SendKeys(text);
					parentDriver.OnElementValueChanged(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void Submit()
			{
				try
				{
					underlyingElement.Submit();
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public void Click()
			{
				try
				{
					WebElementEventArgs e = new WebElementEventArgs(parentDriver.WrappedDriver, underlyingElement);
					parentDriver.OnElementClicking(e);
					underlyingElement.Click();
					parentDriver.OnElementClicked(e);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public string GetAttribute(string attributeName)
			{
				string empty = string.Empty;
				try
				{
					return underlyingElement.GetAttribute(attributeName);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public string GetProperty(string propertyName)
			{
				string empty = string.Empty;
				try
				{
					return underlyingElement.GetProperty(propertyName);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public string GetCssValue(string propertyName)
			{
				string empty = string.Empty;
				try
				{
					return underlyingElement.GetCssValue(propertyName);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public IWebElement FindElement(By by)
			{
				IWebElement webElement = null;
				try
				{
					FindElementEventArgs e = new FindElementEventArgs(parentDriver.WrappedDriver, underlyingElement, by);
					parentDriver.OnFindingElement(e);
					IWebElement webElement2 = underlyingElement.FindElement(by);
					parentDriver.OnFindElementCompleted(e);
					return parentDriver.WrapElement(webElement2);
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
			}

			public ReadOnlyCollection<IWebElement> FindElements(By by)
			{
				List<IWebElement> list = new List<IWebElement>();
				try
				{
					FindElementEventArgs e = new FindElementEventArgs(parentDriver.WrappedDriver, underlyingElement, by);
					parentDriver.OnFindingElement(e);
					ReadOnlyCollection<IWebElement> readOnlyCollection = underlyingElement.FindElements(by);
					parentDriver.OnFindElementCompleted(e);
					foreach (IWebElement item2 in readOnlyCollection)
					{
						IWebElement item = parentDriver.WrapElement(item2);
						list.Add(item);
					}
				}
				catch (Exception thrownException)
				{
					parentDriver.OnException(new WebDriverExceptionEventArgs(parentDriver, thrownException));
					throw;
				}
				return list.AsReadOnly();
			}
		}

		private IWebDriver driver;

		public IWebDriver WrappedDriver => driver;

		public string Url
		{
			get
			{
				string empty = string.Empty;
				try
				{
					return driver.Url;
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
			set
			{
				try
				{
					WebDriverNavigationEventArgs e = new WebDriverNavigationEventArgs(driver, value);
					OnNavigating(e);
					driver.Url = value;
					OnNavigated(e);
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
		}

		public string Title
		{
			get
			{
				string empty = string.Empty;
				try
				{
					return driver.Title;
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
		}

		public string PageSource
		{
			get
			{
				string empty = string.Empty;
				try
				{
					return driver.PageSource;
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
		}

		public string CurrentWindowHandle
		{
			get
			{
				string empty = string.Empty;
				try
				{
					return driver.CurrentWindowHandle;
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
		}

		public ReadOnlyCollection<string> WindowHandles
		{
			get
			{
				ReadOnlyCollection<string> readOnlyCollection = null;
				try
				{
					return driver.WindowHandles;
				}
				catch (Exception thrownException)
				{
					OnException(new WebDriverExceptionEventArgs(driver, thrownException));
					throw;
				}
			}
		}

		public event EventHandler<WebDriverNavigationEventArgs> Navigating;

		public event EventHandler<WebDriverNavigationEventArgs> Navigated;

		public event EventHandler<WebDriverNavigationEventArgs> NavigatingBack;

		public event EventHandler<WebDriverNavigationEventArgs> NavigatedBack;

		public event EventHandler<WebDriverNavigationEventArgs> NavigatingForward;

		public event EventHandler<WebDriverNavigationEventArgs> NavigatedForward;

		public event EventHandler<WebElementEventArgs> ElementClicking;

		public event EventHandler<WebElementEventArgs> ElementClicked;

		public event EventHandler<WebElementValueEventArgs> ElementValueChanging;

		public event EventHandler<WebElementValueEventArgs> ElementValueChanged;

		public event EventHandler<FindElementEventArgs> FindingElement;

		public event EventHandler<FindElementEventArgs> FindElementCompleted;

		public event EventHandler<WebDriverScriptEventArgs> ScriptExecuting;

		public event EventHandler<WebDriverScriptEventArgs> ScriptExecuted;

		public event EventHandler<WebDriverExceptionEventArgs> ExceptionThrown;

		public EventFiringWebDriver(IWebDriver parentDriver)
		{
			driver = parentDriver;
		}

		public void Close()
		{
			try
			{
				driver.Close();
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
		}

		public void Quit()
		{
			try
			{
				driver.Quit();
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
		}

		public IOptions Manage()
		{
			return new EventFiringOptions(this);
		}

		public INavigation Navigate()
		{
			return new EventFiringNavigation(this);
		}

		public ITargetLocator SwitchTo()
		{
			return new EventFiringTargetLocator(this);
		}

		public IWebElement FindElement(By by)
		{
			IWebElement webElement = null;
			try
			{
				FindElementEventArgs e = new FindElementEventArgs(driver, by);
				OnFindingElement(e);
				IWebElement underlyingElement = driver.FindElement(by);
				OnFindElementCompleted(e);
				return WrapElement(underlyingElement);
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
		}

		public ReadOnlyCollection<IWebElement> FindElements(By by)
		{
			List<IWebElement> list = new List<IWebElement>();
			try
			{
				FindElementEventArgs e = new FindElementEventArgs(driver, by);
				OnFindingElement(e);
				ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(by);
				OnFindElementCompleted(e);
				foreach (IWebElement item2 in readOnlyCollection)
				{
					IWebElement item = WrapElement(item2);
					list.Add(item);
				}
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
			return list.AsReadOnly();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		public object ExecuteScript(string script, params object[] args)
		{
			if (!(driver is IJavaScriptExecutor javaScriptExecutor))
			{
				throw new NotSupportedException("Underlying driver instance does not support executing JavaScript");
			}
			object obj = null;
			try
			{
				object[] args2 = UnwrapElementArguments(args);
				WebDriverScriptEventArgs e = new WebDriverScriptEventArgs(driver, script);
				OnScriptExecuting(e);
				obj = javaScriptExecutor.ExecuteScript(script, args2);
				OnScriptExecuted(e);
				return obj;
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
		}

		public object ExecuteAsyncScript(string script, params object[] args)
		{
			object obj = null;
			if (!(driver is IJavaScriptExecutor javaScriptExecutor))
			{
				throw new NotSupportedException("Underlying driver instance does not support executing JavaScript");
			}
			try
			{
				object[] args2 = UnwrapElementArguments(args);
				WebDriverScriptEventArgs e = new WebDriverScriptEventArgs(driver, script);
				OnScriptExecuting(e);
				obj = javaScriptExecutor.ExecuteAsyncScript(script, args2);
				OnScriptExecuted(e);
				return obj;
			}
			catch (Exception thrownException)
			{
				OnException(new WebDriverExceptionEventArgs(driver, thrownException));
				throw;
			}
		}

		public Screenshot GetScreenshot()
		{
			ITakesScreenshot obj = driver as ITakesScreenshot;
			if (driver == null)
			{
				throw new NotSupportedException("Underlying driver instance does not support taking screenshots");
			}
			return obj.GetScreenshot();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				driver.Dispose();
			}
		}

		protected virtual void OnNavigating(WebDriverNavigationEventArgs e)
		{
			if (this.Navigating != null)
			{
				this.Navigating(this, e);
			}
		}

		protected virtual void OnNavigated(WebDriverNavigationEventArgs e)
		{
			if (this.Navigated != null)
			{
				this.Navigated(this, e);
			}
		}

		protected virtual void OnNavigatingBack(WebDriverNavigationEventArgs e)
		{
			if (this.NavigatingBack != null)
			{
				this.NavigatingBack(this, e);
			}
		}

		protected virtual void OnNavigatedBack(WebDriverNavigationEventArgs e)
		{
			if (this.NavigatedBack != null)
			{
				this.NavigatedBack(this, e);
			}
		}

		protected virtual void OnNavigatingForward(WebDriverNavigationEventArgs e)
		{
			if (this.NavigatingForward != null)
			{
				this.NavigatingForward(this, e);
			}
		}

		protected virtual void OnNavigatedForward(WebDriverNavigationEventArgs e)
		{
			if (this.NavigatedForward != null)
			{
				this.NavigatedForward(this, e);
			}
		}

		protected virtual void OnElementClicking(WebElementEventArgs e)
		{
			if (this.ElementClicking != null)
			{
				this.ElementClicking(this, e);
			}
		}

		protected virtual void OnElementClicked(WebElementEventArgs e)
		{
			if (this.ElementClicked != null)
			{
				this.ElementClicked(this, e);
			}
		}

		protected virtual void OnElementValueChanging(WebElementValueEventArgs e)
		{
			if (this.ElementValueChanging != null)
			{
				this.ElementValueChanging(this, e);
			}
		}

		protected virtual void OnElementValueChanged(WebElementValueEventArgs e)
		{
			if (this.ElementValueChanged != null)
			{
				this.ElementValueChanged(this, e);
			}
		}

		protected virtual void OnFindingElement(FindElementEventArgs e)
		{
			if (this.FindingElement != null)
			{
				this.FindingElement(this, e);
			}
		}

		protected virtual void OnFindElementCompleted(FindElementEventArgs e)
		{
			if (this.FindElementCompleted != null)
			{
				this.FindElementCompleted(this, e);
			}
		}

		protected virtual void OnScriptExecuting(WebDriverScriptEventArgs e)
		{
			if (this.ScriptExecuting != null)
			{
				this.ScriptExecuting(this, e);
			}
		}

		protected virtual void OnScriptExecuted(WebDriverScriptEventArgs e)
		{
			if (this.ScriptExecuted != null)
			{
				this.ScriptExecuted(this, e);
			}
		}

		protected virtual void OnException(WebDriverExceptionEventArgs e)
		{
			if (this.ExceptionThrown != null)
			{
				this.ExceptionThrown(this, e);
			}
		}

		private static object[] UnwrapElementArguments(object[] args)
		{
			List<object> list = new List<object>();
			foreach (object obj in args)
			{
				if (obj is IWrapsElement wrapsElement)
				{
					list.Add(wrapsElement.WrappedElement);
				}
				else
				{
					list.Add(obj);
				}
			}
			return list.ToArray();
		}

		private IWebElement WrapElement(IWebElement underlyingElement)
		{
			return new EventFiringWebElement(this, underlyingElement);
		}
	}
	public class FindElementEventArgs : EventArgs
	{
		private IWebDriver driver;

		private IWebElement element;

		private By method;

		public IWebDriver Driver => driver;

		public IWebElement Element => element;

		public By FindMethod => method;

		public FindElementEventArgs(IWebDriver driver, By method)
			: this(driver, null, method)
		{
		}

		public FindElementEventArgs(IWebDriver driver, IWebElement element, By method)
		{
			this.driver = driver;
			this.element = element;
			this.method = method;
		}
	}
	public class WebDriverExceptionEventArgs : EventArgs
	{
		private Exception thrownException;

		private IWebDriver driver;

		public Exception ThrownException => thrownException;

		public IWebDriver Driver => driver;

		public WebDriverExceptionEventArgs(IWebDriver driver, Exception thrownException)
		{
			this.driver = driver;
			this.thrownException = thrownException;
		}
	}
	public class WebDriverNavigationEventArgs : EventArgs
	{
		private string url;

		private IWebDriver driver;

		public string Url => url;

		public IWebDriver Driver => driver;

		public WebDriverNavigationEventArgs(IWebDriver driver)
			: this(driver, null)
		{
		}

		public WebDriverNavigationEventArgs(IWebDriver driver, string url)
		{
			this.url = url;
			this.driver = driver;
		}
	}
	public class WebDriverScriptEventArgs : EventArgs
	{
		private IWebDriver driver;

		private string script;

		public IWebDriver Driver => driver;

		public string Script => script;

		public WebDriverScriptEventArgs(IWebDriver driver, string script)
		{
			this.driver = driver;
			this.script = script;
		}
	}
	public class WebElementEventArgs : EventArgs
	{
		private IWebDriver driver;

		private IWebElement element;

		public IWebDriver Driver => driver;

		public IWebElement Element => element;

		public WebElementEventArgs(IWebDriver driver, IWebElement element)
		{
			this.driver = driver;
			this.element = element;
		}
	}
	public class WebElementValueEventArgs : WebElementEventArgs
	{
		public string Value { get; private set; }

		public WebElementValueEventArgs(IWebDriver driver, IWebElement element, string value)
			: base(driver, element)
		{
			Value = value;
		}
	}
}

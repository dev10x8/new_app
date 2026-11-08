using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using Selenium.Internal;
using Selenium.Internal.SeleniumEmulation;

[assembly: AssemblyCopyright("Copyright © Software Freedom Conservancy 2014")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyTitle("Selenium WebDriver WebDriverBackedSelenium")]
[assembly: AssemblyDescription("Selenium RC API using WebDriver .NET Bindings")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Selenium Committers")]
[assembly: AssemblyProduct("Selenium")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("ec585f6c-1adb-4018-84fa-e2136e0b5115")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyFileVersion("2.52.0.0")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyVersion("2.52.0.0")]
namespace Selenium
{
	public class WebDriverBackedSelenium : DefaultSelenium
	{
		public IWebDriver UnderlyingWebDriver => ((WebDriverCommandProcessor)commandProcessor).UnderlyingWebDriver;

		public WebDriverBackedSelenium(IWebDriver baseDriver, string baseUrl)
			: this(baseDriver, new Uri(baseUrl))
		{
		}

		public WebDriverBackedSelenium(IWebDriver baseDriver, Uri baseUrl)
			: base(new WebDriverCommandProcessor(baseUrl, baseDriver))
		{
		}
	}
	public class WebDriverCommandProcessor : ICommandProcessor
	{
		private IWebDriver driver;

		private Uri baseUrl;

		private Dictionary<string, SeleneseCommand> seleneseMethods = new Dictionary<string, SeleneseCommand>();

		private ElementFinder elementFinder = new ElementFinder();

		private CommandTimer timer;

		private AlertOverride alertOverride;

		private IScriptMutator mutator;

		public IWebDriver UnderlyingWebDriver => driver;

		public WebDriverCommandProcessor(string baseUrl, IWebDriver baseDriver)
			: this(new Uri(baseUrl), baseDriver)
		{
		}

		public WebDriverCommandProcessor(Uri baseUrl, IWebDriver baseDriver)
		{
			if (baseUrl == null)
			{
				throw new ArgumentNullException("baseUrl", "baseUrl cannot be null");
			}
			driver = baseDriver;
			this.baseUrl = baseUrl;
			mutator = new CompoundMutator(baseUrl.ToString());
			timer = new CommandTimer(30000);
			alertOverride = new AlertOverride(baseDriver);
		}

		public string DoCommand(string command, string[] args)
		{
			return Execute(command, args)?.ToString();
		}

		public void SetExtensionJs(string extensionJs)
		{
			throw new NotImplementedException();
		}

		public void Start()
		{
			PopulateSeleneseMethods();
		}

		public void Start(string optionsString)
		{
			throw new NotImplementedException("This is not been ported to WebDriverBackedSelenium");
		}

		public void Start(object optionsObject)
		{
			throw new NotImplementedException("This is not been ported to WebDriverBackedSelenium");
		}

		public void Stop()
		{
			if (driver != null)
			{
				driver.Quit();
			}
			driver = null;
		}

		public string GetString(string command, string[] args)
		{
			return (string)Execute(command, args);
		}

		public string[] GetStringArray(string command, string[] args)
		{
			return (string[])Execute(command, args);
		}

		public decimal GetNumber(string command, string[] args)
		{
			return Convert.ToDecimal(Execute(command, args), CultureInfo.InvariantCulture);
		}

		public decimal[] GetNumberArray(string command, string[] args)
		{
			throw new NotImplementedException();
		}

		public bool GetBoolean(string command, string[] args)
		{
			return (bool)Execute(command, args);
		}

		public bool[] GetBooleanArray(string command, string[] args)
		{
			throw new NotImplementedException();
		}

		private object Execute(string commandName, string[] args)
		{
			if (!seleneseMethods.TryGetValue(commandName, out var value))
			{
				if (seleneseMethods.Count == 0)
				{
					throw new NotSupportedException(commandName + " is not supported\nNote: Start() must be called before any other methods may be called - make sure you've called Start().");
				}
				throw new NotSupportedException(commandName);
			}
			return timer.Execute(value, driver, args);
		}

		private void PopulateSeleneseMethods()
		{
			KeyState keyState = new KeyState();
			WindowSelector windowSelector = new WindowSelector(driver);
			seleneseMethods.Add("addLocationStrategy", new AddLocationStrategy(elementFinder));
			seleneseMethods.Add("addSelection", new AddSelection(elementFinder));
			seleneseMethods.Add("altKeyDown", new AltKeyDown(keyState));
			seleneseMethods.Add("altKeyUp", new AltKeyUp(keyState));
			seleneseMethods.Add("assignId", new AssignId(elementFinder));
			seleneseMethods.Add("attachFile", new AttachFile(elementFinder));
			seleneseMethods.Add("captureScreenshotToString", new CaptureScreenshotToString());
			seleneseMethods.Add("click", new Click(alertOverride, elementFinder));
			seleneseMethods.Add("clickAt", new ClickAt(alertOverride, elementFinder));
			seleneseMethods.Add("check", new Check(alertOverride, elementFinder));
			seleneseMethods.Add("chooseCancelOnNextConfirmation", new SetNextConfirmationState(result: false));
			seleneseMethods.Add("chooseOkOnNextConfirmation", new SetNextConfirmationState(result: true));
			seleneseMethods.Add("close", new Close());
			seleneseMethods.Add("createCookie", new CreateCookie());
			seleneseMethods.Add("controlKeyDown", new ControlKeyDown(keyState));
			seleneseMethods.Add("controlKeyUp", new ControlKeyUp(keyState));
			seleneseMethods.Add("deleteAllVisibleCookies", new DeleteAllVisibleCookies());
			seleneseMethods.Add("deleteCookie", new DeleteCookie());
			seleneseMethods.Add("doubleClick", new DoubleClick(elementFinder));
			seleneseMethods.Add("dragdrop", new DragAndDrop(elementFinder));
			seleneseMethods.Add("dragAndDrop", new DragAndDrop(elementFinder));
			seleneseMethods.Add("dragAndDropToObject", new DragAndDropToObject(elementFinder));
			seleneseMethods.Add("fireEvent", new FireEvent(elementFinder));
			seleneseMethods.Add("focus", new FireNamedEvent(elementFinder, "focus"));
			seleneseMethods.Add("getAlert", new GetAlert(alertOverride));
			seleneseMethods.Add("getAllButtons", new GetAllButtons());
			seleneseMethods.Add("getAllFields", new GetAllFields());
			seleneseMethods.Add("getAllLinks", new GetAllLinks());
			seleneseMethods.Add("getAllWindowTitles", new GetAllWindowTitles());
			seleneseMethods.Add("getAttribute", new GetAttribute(elementFinder));
			seleneseMethods.Add("getAttributeFromAllWindows", new GetAttributeFromAllWindows());
			seleneseMethods.Add("getBodyText", new GetBodyText());
			seleneseMethods.Add("getConfirmation", new GetConfirmation(alertOverride));
			seleneseMethods.Add("getCookie", new GetCookie());
			seleneseMethods.Add("getCookieByName", new GetCookieByName());
			seleneseMethods.Add("getElementHeight", new GetElementHeight(elementFinder));
			seleneseMethods.Add("getElementIndex", new GetElementIndex(elementFinder));
			seleneseMethods.Add("getElementPositionLeft", new GetElementPositionLeft(elementFinder));
			seleneseMethods.Add("getElementPositionTop", new GetElementPositionTop(elementFinder));
			seleneseMethods.Add("getElementWidth", new GetElementWidth(elementFinder));
			seleneseMethods.Add("getEval", new GetEval(mutator));
			seleneseMethods.Add("getHtmlSource", new GetHtmlSource());
			seleneseMethods.Add("getLocation", new GetLocation());
			seleneseMethods.Add("getSelectedId", new FindFirstSelectedOptionProperty(elementFinder, "id"));
			seleneseMethods.Add("getSelectedIds", new FindSelectedOptionProperties(elementFinder, "id"));
			seleneseMethods.Add("getSelectedIndex", new FindFirstSelectedOptionProperty(elementFinder, "index"));
			seleneseMethods.Add("getSelectedIndexes", new FindSelectedOptionProperties(elementFinder, "index"));
			seleneseMethods.Add("getSelectedLabel", new FindFirstSelectedOptionProperty(elementFinder, "text"));
			seleneseMethods.Add("getSelectedLabels", new FindSelectedOptionProperties(elementFinder, "text"));
			seleneseMethods.Add("getSelectedValue", new FindFirstSelectedOptionProperty(elementFinder, "value"));
			seleneseMethods.Add("getSelectedValues", new FindSelectedOptionProperties(elementFinder, "value"));
			seleneseMethods.Add("getSelectOptions", new GetSelectOptions(elementFinder));
			seleneseMethods.Add("getSpeed", new NoOp("0"));
			seleneseMethods.Add("getTable", new GetTable(elementFinder));
			seleneseMethods.Add("getText", new GetText(elementFinder));
			seleneseMethods.Add("getTitle", new GetTitle());
			seleneseMethods.Add("getValue", new GetValue(elementFinder));
			seleneseMethods.Add("getXpathCount", new GetXpathCount());
			seleneseMethods.Add("getCssCount", new GetCssCount());
			seleneseMethods.Add("goBack", new GoBack());
			seleneseMethods.Add("highlight", new Highlight(elementFinder));
			seleneseMethods.Add("isAlertPresent", new IsAlertPresent(alertOverride));
			seleneseMethods.Add("isChecked", new IsChecked(elementFinder));
			seleneseMethods.Add("isConfirmationPresent", new IsConfirmationPresent(alertOverride));
			seleneseMethods.Add("isCookiePresent", new IsCookiePresent());
			seleneseMethods.Add("isEditable", new IsEditable(elementFinder));
			seleneseMethods.Add("isElementPresent", new IsElementPresent(elementFinder));
			seleneseMethods.Add("isOrdered", new IsOrdered(elementFinder));
			seleneseMethods.Add("isSomethingSelected", new IsSomethingSelected());
			seleneseMethods.Add("isTextPresent", new IsTextPresent());
			seleneseMethods.Add("isVisible", new IsVisible(elementFinder));
			seleneseMethods.Add("keyDown", new KeyEvent(elementFinder, keyState, "doKeyDown"));
			seleneseMethods.Add("keyPress", new TypeKeys(alertOverride, elementFinder));
			seleneseMethods.Add("keyUp", new KeyEvent(elementFinder, keyState, "doKeyUp"));
			seleneseMethods.Add("metaKeyDown", new MetaKeyDown(keyState));
			seleneseMethods.Add("metaKeyUp", new MetaKeyUp(keyState));
			seleneseMethods.Add("mouseOver", new MouseEvent(elementFinder, "mouseover"));
			seleneseMethods.Add("mouseOut", new MouseEvent(elementFinder, "mouseout"));
			seleneseMethods.Add("mouseDown", new MouseEvent(elementFinder, "mousedown"));
			seleneseMethods.Add("mouseDownAt", new MouseEventAt(elementFinder, "mousedown"));
			seleneseMethods.Add("mouseMove", new MouseEvent(elementFinder, "mousemove"));
			seleneseMethods.Add("mouseMoveAt", new MouseEventAt(elementFinder, "mousemove"));
			seleneseMethods.Add("mouseUp", new MouseEvent(elementFinder, "mouseup"));
			seleneseMethods.Add("mouseUpAt", new MouseEventAt(elementFinder, "mouseup"));
			seleneseMethods.Add("open", new Open(baseUrl));
			seleneseMethods.Add("openWindow", new OpenWindow(new GetEval(mutator)));
			seleneseMethods.Add("refresh", new Refresh());
			seleneseMethods.Add("removeAllSelections", new RemoveAllSelections(elementFinder));
			seleneseMethods.Add("removeSelection", new RemoveSelection(elementFinder));
			seleneseMethods.Add("runScript", new RunScript(mutator));
			seleneseMethods.Add("select", new SelectOption(alertOverride, elementFinder));
			seleneseMethods.Add("selectFrame", new SelectFrame(windowSelector));
			seleneseMethods.Add("selectWindow", new SelectWindow(windowSelector));
			seleneseMethods.Add("setBrowserLogLevel", new NoOp(null));
			seleneseMethods.Add("setContext", new NoOp(null));
			seleneseMethods.Add("setSpeed", new NoOp(null));
			seleneseMethods.Add("setTimeout", new SetTimeout(timer));
			seleneseMethods.Add("shiftKeyDown", new ShiftKeyDown(keyState));
			seleneseMethods.Add("shiftKeyUp", new ShiftKeyUp(keyState));
			seleneseMethods.Add("submit", new Submit(alertOverride, elementFinder));
			seleneseMethods.Add("type", new Selenium.Internal.SeleniumEmulation.Type(alertOverride, elementFinder, keyState));
			seleneseMethods.Add("typeKeys", new TypeKeys(alertOverride, elementFinder));
			seleneseMethods.Add("uncheck", new Uncheck(elementFinder));
			seleneseMethods.Add("useXpathLibrary", new NoOp(null));
			seleneseMethods.Add("waitForCondition", new WaitForCondition(mutator));
			seleneseMethods.Add("waitForFrameToLoad", new NoOp(null));
			seleneseMethods.Add("waitForPageToLoad", new WaitForPageToLoad());
			seleneseMethods.Add("waitForPopUp", new WaitForPopup(windowSelector));
			seleneseMethods.Add("windowFocus", new WindowFocus());
			seleneseMethods.Add("windowMaximize", new WindowMaximize());
		}
	}
}
namespace Selenium.Internal
{
	internal class AlertOverride
	{
		private IWebDriver driver;

		public AlertOverride(IWebDriver driver)
		{
			this.driver = driver;
		}

		public void ReplaceAlertMethod()
		{
			((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   window.localStorage.setItem('__webdriverAlerts', JSON.stringify([]));   window.alert = function(msg) {     var alerts = JSON.parse(window.localStorage.getItem('__webdriverAlerts'));     alerts.push(msg);     window.localStorage.setItem('__webdriverAlerts', JSON.stringify(alerts));   };   window.localStorage.setItem('__webdriverConfirms', JSON.stringify([]));   if (!('__webdriverNextConfirm' in window.localStorage)) {     window.localStorage.setItem('__webdriverNextConfirm', JSON.stringify(true));   }   window.confirm = function(msg) {     var confirms = JSON.parse(window.localStorage.getItem('__webdriverConfirms'));     confirms.push(msg);     window.localStorage.setItem('__webdriverConfirms', JSON.stringify(confirms));     var res = JSON.parse(window.localStorage.getItem('__webdriverNextConfirm'));     window.localStorage.setItem('__webdriverNextConfirm', JSON.stringify(true));     return res;   }; } else {   if (window.__webdriverAlerts) { return; }   window.__webdriverAlerts = [];   window.alert = function(msg) { window.__webdriverAlerts.push(msg); };   window.__webdriverConfirms = [];   window.__webdriverNextConfirm = true;   window.confirm = function(msg) {     window.__webdriverConfirms.push(msg);     var res = window.__webdriverNextConfirm;     window.__webdriverNextConfirm = true;     return res;   }; }");
		}

		public string GetNextAlert()
		{
			string text = (string)((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   if (!('__webdriverAlerts' in window.localStorage)) { return null }   var alerts = JSON.parse(window.localStorage.getItem('__webdriverAlerts'));   if (! alerts) { return null }   var t = alerts.shift();   window.localStorage.setItem('__webdriverAlerts', JSON.stringify(alerts));   if (t) { t = t.replace(/\\n/g, ' '); }   return t; } else {   if (!window.__webdriverAlerts) { return null }   var t = window.__webdriverAlerts.shift();   if (t) { t = t.replace(/\\n/g, ' '); }   return t; }");
			if (text == null)
			{
				throw new SeleniumException("There were no alerts");
			}
			return text;
		}

		public bool IsAlertPresent()
		{
			bool result = false;
			object obj = ((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   if (!('__webdriverAlerts' in window.localStorage)) { return false }   var alerts = JSON.parse(window.localStorage.getItem('__webdriverAlerts'));   return alerts && alerts.length > 0; } else {   return window.__webdriverAlerts && window.__webdriverAlerts.length > 0; }");
			if (obj != null)
			{
				result = (bool)obj;
			}
			return result;
		}

		public string GetNextConfirmation()
		{
			string text = (string)((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   if (!('__webdriverConfirms' in window.localStorage)) { return null }   var confirms = JSON.parse(window.localStorage.getItem('__webdriverConfirms'));   if (! confirms) { return null }   var t = confirms.shift();   window.localStorage.setItem('__webdriverConfirms', JSON.stringify(confirms));   if (t) { t = t.replace(/\\n/g, ' '); }   return t; } else {   if (!window.__webdriverConfirms) { return null; }   return window.__webdriverConfirms.shift(); }");
			if (text == null)
			{
				throw new SeleniumException("There were no confirmations");
			}
			return text;
		}

		public bool IsConfirmationPresent()
		{
			bool result = false;
			object obj = ((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   if (!('__webdriverConfirms' in window.localStorage)) { return false }   var confirms = JSON.parse(window.localStorage.getItem('__webdriverConfirms'));   return confirms && confirms.length > 0; } else {   return window.__webdriverConfirms && window.__webdriverConfirms.length > 0; }");
			if (obj != null)
			{
				result = (bool)obj;
			}
			return result;
		}
	}
	internal class CommandTimer
	{
		private int timeout;

		private object commandResult;

		private SeleneseCommand command;

		private IWebDriver driver;

		private string[] args;

		private SeleniumException thrownException;

		public int Timeout
		{
			get
			{
				return timeout;
			}
			set
			{
				timeout = value;
			}
		}

		public CommandTimer(int timeout)
		{
			this.timeout = timeout;
		}

		public object Execute(SeleneseCommand commandToExecute, IWebDriver commandDriver, string[] commandArguments)
		{
			thrownException = null;
			command = commandToExecute;
			driver = commandDriver;
			args = commandArguments;
			Thread thread = new Thread(RunCommand);
			thread.Start();
			thread.Join(timeout);
			if (thread.IsAlive)
			{
				throw new SeleniumException("Timed out running command");
			}
			if (thrownException != null)
			{
				throw thrownException;
			}
			return commandResult;
		}

		private void RunCommand()
		{
			try
			{
				commandResult = command.Apply(driver, args);
			}
			catch (SeleniumException ex)
			{
				thrownException = ex;
			}
			catch (WebDriverException innerException)
			{
				thrownException = new SeleniumException("WebDriver exception thrown", innerException);
			}
			catch (InvalidOperationException innerException2)
			{
				thrownException = new SeleniumException("WebDriver exception thrown", innerException2);
			}
		}
	}
}
namespace Selenium.Internal.SeleniumEmulation
{
	internal abstract class SeleneseCommand
	{
		internal object Apply(IWebDriver driver, string[] args)
		{
			return args.Length switch
			{
				0 => HandleSeleneseCommand(driver, null, null), 
				1 => HandleSeleneseCommand(driver, args[0], null), 
				2 => HandleSeleneseCommand(driver, args[0], args[1]), 
				_ => throw new SeleniumException("Too many arguments! " + args.Length), 
			};
		}

		protected abstract object HandleSeleneseCommand(IWebDriver driver, string locator, string value);
	}
	internal class AddLocationStrategy : SeleneseCommand
	{
		private ElementFinder finder;

		public AddLocationStrategy(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string strategy = string.Format(CultureInfo.InvariantCulture, "return (function(locator, inWindow, inDocument) {{ {0} }}).call(null, arguments[0], window, document)", new object[1] { value });
			finder.AddStrategy(locator, strategy);
			return null;
		}
	}
	internal class AddSelection : SeleneseCommand
	{
		private ElementFinder finder;

		public AddSelection(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			seleniumSelect.AddSelection(value);
			return null;
		}
	}
	internal class AltKeyDown : SeleneseCommand
	{
		private KeyState keyState;

		public AltKeyDown(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.AltKeyDown = true;
			return null;
		}
	}
	internal class AltKeyUp : SeleneseCommand
	{
		private KeyState keyState;

		public AltKeyUp(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.AltKeyDown = false;
			return null;
		}
	}
	internal class AssignId : SeleneseCommand
	{
		private ElementFinder finder;

		public AssignId(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			JavaScriptLibrary.ExecuteScript(driver, "arguments[0].id = arguments[1]", finder.FindElement(driver, locator), value);
			return null;
		}
	}
	internal class AttachFile : SeleneseCommand
	{
		private ElementFinder finder;

		public AttachFile(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			webElement.Clear();
			webElement.SendKeys(value);
			return null;
		}
	}
	internal class CaptureScreenshotToString : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string result = string.Empty;
			if (driver is ITakesScreenshot takesScreenshot)
			{
				result = takesScreenshot.GetScreenshot().AsBase64EncodedString;
			}
			return result;
		}
	}
	internal class Check : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alert;

		public Check(AlertOverride alert, ElementFinder elementFinder)
		{
			finder = elementFinder;
			this.alert = alert;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alert.ReplaceAlertMethod();
			IWebElement webElement = finder.FindElement(driver, locator);
			webElement.Click();
			return null;
		}
	}
	internal class Click : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alert;

		public Click(AlertOverride alert, ElementFinder finder)
		{
			this.finder = finder;
			this.alert = alert;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alert.ReplaceAlertMethod();
			IWebElement webElement = finder.FindElement(driver, locator);
			webElement.Click();
			return null;
		}
	}
	internal class ClickAt : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alert;

		public ClickAt(AlertOverride alert, ElementFinder finder)
		{
			this.finder = finder;
			this.alert = alert;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alert.ReplaceAlertMethod();
			IWebElement toElement = finder.FindElement(driver, locator);
			string[] array = value.Split(new char[1] { ',' });
			int offsetX = int.Parse(array[0], CultureInfo.InvariantCulture);
			int offsetY = int.Parse(array[1], CultureInfo.InvariantCulture);
			new Actions(driver).MoveToElement(toElement, offsetX, offsetY).Click().Perform();
			return null;
		}
	}
	internal class Close : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			driver.Close();
			return null;
		}
	}
	public interface IScriptMutator
	{
		void Mutate(string script, StringBuilder outputTo);
	}
	public class CompoundMutator : IScriptMutator
	{
		private List<IScriptMutator> mutators = new List<IScriptMutator>();

		public CompoundMutator(string basePath)
		{
			AddMutator(new VariableDeclaration("selenium", "var selenium = {};"));
			AddMutator(new VariableDeclaration("selenium.browserbot", "selenium.browserbot = {};"));
			AddMutator(new VariableDeclaration("selenium.browserbot.baseUrl", "selenium.browserbot.baseUrl = '" + basePath + "';"));
			AddMutator(new FunctionDeclaration("selenium.page", "if (!selenium.browserbot) { selenium.browserbot = {} }; return selenium.browserbot;"));
			AddMutator(new FunctionDeclaration("selenium.browserbot.getCurrentWindow", "return window;"));
			AddMutator(new FunctionDeclaration("selenium.page().getCurrentWindow", "return window;"));
			AddMutator(new FunctionDeclaration("selenium.browserbot.getDocument", "return document;"));
			AddMutator(new FunctionDeclaration("selenium.page().getDocument", "return document;"));
			AddMutator(new SeleniumMutator("selenium.isElementPresent", JavaScriptLibrary.GetSeleniumScript("isElementPresent.js")));
			AddMutator(new SeleniumMutator("selenium.isTextPresent", JavaScriptLibrary.GetSeleniumScript("isTextPresent.js")));
			AddMutator(new SeleniumMutator("selenium.isVisible", JavaScriptLibrary.GetSeleniumScript("isVisible.js")));
			AddMutator(new SeleniumMutator("selenium.browserbot.findElement", JavaScriptLibrary.GetSeleniumScript("findElement.js")));
		}

		public void Mutate(string script, StringBuilder outputTo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IScriptMutator mutator in mutators)
			{
				mutator.Mutate(script, stringBuilder);
			}
			stringBuilder.Append(script);
			if (outputTo != null)
			{
				outputTo.Append("return eval('");
				outputTo.Append(Escape(stringBuilder.ToString()));
				outputTo.Append("');");
			}
		}

		internal void AddMutator(IScriptMutator mutator)
		{
			mutators.Add(mutator);
		}

		private static string Escape(string escapee)
		{
			return escapee.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("'", "\\'");
		}
	}
	internal class ControlKeyDown : SeleneseCommand
	{
		private KeyState keyState;

		public ControlKeyDown(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.ControlKeyDown = true;
			return null;
		}
	}
	internal class ControlKeyUp : SeleneseCommand
	{
		private KeyState keyState;

		public ControlKeyUp(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.ControlKeyDown = false;
			return null;
		}
	}
	internal class CreateCookie : SeleneseCommand
	{
		private readonly Regex nameValuePairRegex = new Regex("([^\\s=\\[\\]\\(\\),\"\\/\\?@:;]+)=([^=\\[\\]\\(\\),\"\\/\\?@:;]*)");

		private readonly Regex maxAgeRegex = new Regex("max_age=(\\d+)");

		private readonly Regex pathRegex = new Regex("path=([^\\s,]+)[,]?");

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			if (!nameValuePairRegex.IsMatch(locator))
			{
				throw new SeleniumException("Invalid parameter: " + locator);
			}
			Match match = nameValuePairRegex.Match(locator);
			string value2 = match.Groups[1].Value;
			string value3 = match.Groups[2].Value;
			DateTime? expiry = null;
			if (maxAgeRegex.IsMatch(value))
			{
				Match match2 = maxAgeRegex.Match(value);
				expiry = DateTime.Now.AddSeconds(int.Parse(match2.Groups[1].Value, CultureInfo.InvariantCulture));
			}
			string empty = string.Empty;
			if (pathRegex.IsMatch(value))
			{
				Match match3 = pathRegex.Match(value);
				empty = match3.Groups[1].Value;
				try
				{
					if (empty.StartsWith("http", StringComparison.Ordinal))
					{
						empty = new Uri(empty).AbsolutePath;
					}
				}
				catch (UriFormatException)
				{
				}
			}
			else
			{
				empty = new Uri(driver.Url).AbsolutePath;
			}
			Cookie cookie = new Cookie(value2, value3, empty, expiry);
			driver.Manage().Cookies.AddCookie(cookie);
			return null;
		}
	}
	internal class DeleteAllVisibleCookies : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string ignored, string alsoIgnored)
		{
			driver.Manage().Cookies.DeleteAllCookies();
			return null;
		}
	}
	internal class DeleteCookie : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			driver.Manage().Cookies.DeleteCookieNamed(locator);
			return null;
		}
	}
	internal class DoubleClick : SeleneseCommand
	{
		private ElementFinder finder;

		public DoubleClick(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			webElement.Click();
			webElement.Click();
			return null;
		}
	}
	internal class DragAndDrop : SeleneseCommand
	{
		private ElementFinder finder;

		public DragAndDrop(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string[] array = value.Split(new string[1] { "," }, 2, StringSplitOptions.None);
			int offsetX = int.Parse(array[0], CultureInfo.InvariantCulture);
			int offsetY = int.Parse(array[1], CultureInfo.InvariantCulture);
			IWebElement source = finder.FindElement(driver, locator);
			Actions actions = new Actions(driver);
			actions.DragAndDropToOffset(source, offsetX, offsetY).Perform();
			return null;
		}
	}
	internal class DragAndDropToObject : SeleneseCommand
	{
		private ElementFinder finder;

		public DragAndDropToObject(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement source = finder.FindElement(driver, locator);
			IWebElement target = finder.FindElement(driver, value);
			Actions actions = new Actions(driver);
			actions.DragAndDrop(source, target).Perform();
			return null;
		}
	}
	internal class ElementFinder
	{
		private string findElement;

		private Dictionary<string, string> lookupStrategies = new Dictionary<string, string>();

		public ElementFinder()
		{
			string seleniumScript = JavaScriptLibrary.GetSeleniumScript("findElement.js");
			findElement = "return (" + seleniumScript + ")(arguments[0]);";
			string strategy = "return (" + JavaScriptLibrary.GetSeleniumScript("linkLocator.js") + ").call(null, arguments[0], document)";
			AddStrategy("link", strategy);
		}

		internal IWebElement FindElement(IWebDriver driver, string locator)
		{
			IJavaScriptExecutor javaScriptExecutor = driver as IJavaScriptExecutor;
			string text = FindStrategy(locator);
			if (!string.IsNullOrEmpty(text))
			{
				string text2 = locator.Substring(locator.IndexOf('=') + 1);
				try
				{
					if (!(javaScriptExecutor.ExecuteScript(text, text2) is IWebElement result))
					{
						throw new SeleniumException("Element " + locator + " not found");
					}
					return result;
				}
				catch (WebDriverException)
				{
					throw new SeleniumException("Element " + locator + " not found");
				}
			}
			try
			{
				IWebElement webElement = FindElementDirectly(driver, locator);
				if (webElement != null)
				{
					return webElement;
				}
				return javaScriptExecutor.ExecuteScript(findElement, locator) as IWebElement;
			}
			catch (WebDriverException)
			{
				throw new SeleniumException("Element " + locator + " not found");
			}
			catch (InvalidOperationException)
			{
				throw new SeleniumException("Element " + locator + " not found");
			}
		}

		internal string FindStrategy(string locator)
		{
			string value = string.Empty;
			int num = locator.IndexOf('=');
			if (num == -1)
			{
				return null;
			}
			string key = locator.Substring(0, num);
			if (!lookupStrategies.TryGetValue(key, out value))
			{
				return null;
			}
			return value;
		}

		internal void AddStrategy(string strategyName, string strategy)
		{
			lookupStrategies.Add(strategyName, strategy);
		}

		private static IWebElement FindElementDirectly(IWebDriver driver, string locator)
		{
			if (locator.StartsWith("xpath=", StringComparison.Ordinal))
			{
				return FindUsingXPath(driver, locator.Substring("xpath=".Length));
			}
			if (locator.StartsWith("//", StringComparison.Ordinal))
			{
				return FindUsingXPath(driver, locator);
			}
			if (locator.StartsWith("css=", StringComparison.Ordinal))
			{
				return driver.FindElement(By.CssSelector(locator.Substring("css=".Length)));
			}
			return null;
		}

		private static IWebElement FindUsingXPath(IWebDriver driver, string xpath)
		{
			try
			{
				return driver.FindElement(By.XPath(xpath));
			}
			catch (WebDriverException)
			{
				if (xpath.EndsWith("/"))
				{
					xpath = xpath.Substring(0, xpath.Length - 1);
					return driver.FindElement(By.XPath(xpath));
				}
			}
			throw new NoSuchElementException(string.Format(CultureInfo.InvariantCulture, "Cannot find element by XPath {0}", new object[1] { xpath }));
		}
	}
	internal class FindFirstSelectedOptionProperty : SeleneseCommand
	{
		private ElementFinder finder;

		private string property;

		public FindFirstSelectedOptionProperty(ElementFinder finder, string property)
		{
			this.finder = finder;
			this.property = property;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			ReadOnlyCollection<IWebElement> selectedOptions = seleniumSelect.SelectedOptions;
			if (selectedOptions.Count == 0)
			{
				throw new SeleniumException("No options are selected: " + locator);
			}
			return selectedOptions[0].GetAttribute(property);
		}
	}
	internal class FindSelectedOptionProperties : SeleneseCommand
	{
		private ElementFinder finder;

		private string property;

		public FindSelectedOptionProperties(ElementFinder finder, string property)
		{
			this.finder = finder;
			this.property = property;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			ReadOnlyCollection<IWebElement> selectedOptions = seleniumSelect.SelectedOptions;
			List<string> list = new List<string>();
			foreach (IWebElement item in selectedOptions)
			{
				list.Add(item.GetAttribute(property));
			}
			return list.ToArray();
		}
	}
	internal class FireEvent : SeleneseCommand
	{
		private ElementFinder finder;

		private string fire;

		public FireEvent(ElementFinder elementFinder)
		{
			finder = elementFinder;
			fire = "return (" + JavaScriptLibrary.GetSeleniumScript("fireEvent.js") + ").apply(null, arguments);";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			JavaScriptLibrary.ExecuteScript(driver, fire, webElement, value);
			return null;
		}
	}
	internal class FireNamedEvent : SeleneseCommand
	{
		private ElementFinder finder;

		private string fire;

		private string name;

		public FireNamedEvent(ElementFinder elementFinder, string eventName)
		{
			finder = elementFinder;
			name = eventName;
			fire = "return (" + JavaScriptLibrary.GetSeleniumScript("fireEvent.js") + ").apply(null, arguments);";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			JavaScriptLibrary.ExecuteScript(driver, fire, webElement, name);
			return null;
		}
	}
	public class FunctionDeclaration : IScriptMutator
	{
		private Regex pattern;

		private string function;

		public FunctionDeclaration(string raw, string result)
		{
			if (raw == null)
			{
				raw = string.Empty;
			}
			string text = raw.Replace(".", "\\s*\\.\\s*");
			pattern = new Regex(".*" + text + "\\s*\\(\\s*\\).*");
			function = raw + " = function() { " + result + " }; ";
		}

		public void Mutate(string script, StringBuilder outputTo)
		{
			if (outputTo != null && pattern.IsMatch(script))
			{
				outputTo.Append(function);
			}
		}
	}
	internal class GetAlert : SeleneseCommand
	{
		private AlertOverride alertOverride;

		public GetAlert(AlertOverride alertOverride)
		{
			this.alertOverride = alertOverride;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return alertOverride.GetNextAlert();
		}
	}
	internal class GetAllButtons : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(By.XPath("//input"));
			List<string> list = new List<string>();
			foreach (IWebElement item in readOnlyCollection)
			{
				switch (item.GetAttribute("type").ToUpperInvariant())
				{
				case "BUTTON":
				case "SUBMIT":
				case "RESET":
					list.Add(item.GetAttribute("id"));
					break;
				}
			}
			return list.ToArray();
		}
	}
	internal class GetAllFields : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(By.XPath("//input"));
			List<string> list = new List<string>();
			foreach (IWebElement item in readOnlyCollection)
			{
				string text = item.GetAttribute("type").ToUpperInvariant();
				if (text == "TEXT")
				{
					list.Add(item.GetAttribute("id"));
				}
			}
			return list.ToArray();
		}
	}
	internal class GetAllLinks : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			ReadOnlyCollection<IWebElement> readOnlyCollection = driver.FindElements(By.XPath("//a"));
			List<string> list = new List<string>();
			foreach (IWebElement item in readOnlyCollection)
			{
				string attribute = item.GetAttribute("id");
				if (string.IsNullOrEmpty(attribute))
				{
					list.Add(string.Empty);
				}
				else
				{
					list.Add(attribute);
				}
			}
			return list.ToArray();
		}
	}
	internal class GetAllWindowTitles : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string currentWindowHandle = driver.CurrentWindowHandle;
			List<string> list = new List<string>();
			foreach (string windowHandle in driver.WindowHandles)
			{
				driver.SwitchTo().Window(windowHandle);
				list.Add(driver.Title);
			}
			driver.SwitchTo().Window(currentWindowHandle);
			return list.ToArray();
		}
	}
	internal class GetAttribute : SeleneseCommand
	{
		private ElementFinder finder;

		public GetAttribute(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			int num = locator.LastIndexOf("@", StringComparison.Ordinal);
			string locator2 = locator.Substring(0, num);
			string attributeName = locator.Substring(num + 1);
			IWebElement webElement = finder.FindElement(driver, locator2);
			return webElement.GetAttribute(attributeName);
		}
	}
	internal class GetAttributeFromAllWindows : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string currentWindowHandle = driver.CurrentWindowHandle;
			List<string> list = new List<string>();
			foreach (string windowHandle in driver.WindowHandles)
			{
				driver.SwitchTo().Window(windowHandle);
				string item = (string)((IJavaScriptExecutor)driver).ExecuteScript("return '' + window[arguments[0]];", value);
				list.Add(item);
			}
			driver.SwitchTo().Window(currentWindowHandle);
			return list.ToArray();
		}
	}
	internal class GetBodyText : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.FindElement(By.XPath("//body")).Text;
		}
	}
	internal class GetConfirmation : SeleneseCommand
	{
		private AlertOverride alertOverride;

		public GetConfirmation(AlertOverride alertOverride)
		{
			this.alertOverride = alertOverride;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return alertOverride.GetNextConfirmation();
		}
	}
	internal class GetCookie : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Cookie allCookie in driver.Manage().Cookies.AllCookies)
			{
				stringBuilder.Append(allCookie.ToString());
				stringBuilder.Append("; ");
			}
			return stringBuilder.ToString();
		}
	}
	internal class GetCookieByName : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.Manage().Cookies.GetCookieNamed(locator)?.Value;
		}
	}
	internal class GetCssCount : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.FindElements(By.CssSelector(locator)).Count;
		}
	}
	internal class GetElementHeight : SeleneseCommand
	{
		private ElementFinder finder;

		public GetElementHeight(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Size.Height;
		}
	}
	internal class GetElementIndex : SeleneseCommand
	{
		private const string ElementIndexFinderScript = "var _isCommentOrEmptyTextNode = function(node) {\r\n    return node.nodeType == 8 || ((node.nodeType == 3) && !(/[^\\\\t\\\\n\\\\r ]/.test(node.data)));\r\n}\r\nvar element = arguments[0];\r\nvar previousSibling;\r\nvar index = 0;\r\nwhile ((previousSibling = element.previousSibling) != null) {\r\n    if (!_isCommentOrEmptyTextNode(previousSibling)) {\r\n        index++;\r\n    }\r\n    element = previousSibling;\r\n}\r\nreturn index;";

		private ElementFinder finder;

		public GetElementIndex(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			return JavaScriptLibrary.ExecuteScript(driver, "var _isCommentOrEmptyTextNode = function(node) {\r\n    return node.nodeType == 8 || ((node.nodeType == 3) && !(/[^\\\\t\\\\n\\\\r ]/.test(node.data)));\r\n}\r\nvar element = arguments[0];\r\nvar previousSibling;\r\nvar index = 0;\r\nwhile ((previousSibling = element.previousSibling) != null) {\r\n    if (!_isCommentOrEmptyTextNode(previousSibling)) {\r\n        index++;\r\n    }\r\n    element = previousSibling;\r\n}\r\nreturn index;", webElement);
		}
	}
	internal class GetElementPositionLeft : SeleneseCommand
	{
		private ElementFinder finder;

		public GetElementPositionLeft(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Location.X;
		}
	}
	internal class GetElementPositionTop : SeleneseCommand
	{
		private ElementFinder finder;

		public GetElementPositionTop(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Location.Y;
		}
	}
	internal class GetElementWidth : SeleneseCommand
	{
		private ElementFinder finder;

		public GetElementWidth(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Size.Width;
		}
	}
	internal class GetEval : SeleneseCommand
	{
		private IScriptMutator mutator;

		public GetEval(IScriptMutator mutator)
		{
			this.mutator = mutator;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			mutator.Mutate(locator, stringBuilder);
			object obj = ((IJavaScriptExecutor)driver).ExecuteScript(stringBuilder.ToString());
			if (obj != null)
			{
				return obj.ToString();
			}
			return string.Empty;
		}
	}
	internal class GetExpression : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return locator;
		}
	}
	internal class GetHtmlSource : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string ignored, string alsoIgnored)
		{
			return driver.PageSource;
		}
	}
	internal class GetLocation : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.Url;
		}
	}
	internal class GetSelectOptions : SeleneseCommand
	{
		private ElementFinder finder;

		public GetSelectOptions(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			ReadOnlyCollection<IWebElement> allOptions = seleniumSelect.AllOptions;
			List<string> list = new List<string>();
			foreach (IWebElement item in allOptions)
			{
				list.Add(item.Text);
			}
			return list.ToArray();
		}
	}
	internal class GetTable : SeleneseCommand
	{
		private ElementFinder finder;

		private static Regex TableParts => new Regex("(.*)\\.(\\d+)\\.(\\d+)");

		public GetTable(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string empty = string.Empty;
			if (!TableParts.IsMatch(locator))
			{
				throw new SeleniumException("Invalid target format. Correct format is tableName.rowNum.columnNum");
			}
			Match match = TableParts.Match(locator);
			string value2 = match.Groups[1].Value;
			long num = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
			long num2 = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
			IWebElement webElement = finder.FindElement(driver, value2);
			string script = "var table = arguments[0]; var row = arguments[1]; var col = arguments[2];if (row > table.rows.length) { return \"Cannot access row \" + row + \" - table has \" + table.rows.length + \" rows\"; }if (col > table.rows[row].cells.length) { return \"Cannot access column \" + col + \" - table row has \" + table.rows[row].cells.length + \" columns\"; }return table.rows[row].cells[col];";
			object obj = JavaScriptLibrary.ExecuteScript(driver, script, webElement, num, num2);
			if (obj is IWebElement webElement2)
			{
				return webElement2.Text.Trim();
			}
			throw new SeleniumException(obj.ToString());
		}
	}
	internal class GetText : SeleneseCommand
	{
		private ElementFinder finder;

		public GetText(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string seleniumScript = JavaScriptLibrary.GetSeleniumScript("getText.js");
			try
			{
				return (string)((IJavaScriptExecutor)driver).ExecuteScript("return (" + seleniumScript + ")(arguments[0]);", locator);
			}
			catch (WebDriverException)
			{
				IWebElement webElement = finder.FindElement(driver, locator);
				return webElement.Text;
			}
			catch (InvalidOperationException)
			{
				IWebElement webElement2 = finder.FindElement(driver, locator);
				return webElement2.Text;
			}
		}
	}
	internal class GetTitle : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.Title;
		}
	}
	internal class GetValue : SeleneseCommand
	{
		private ElementFinder finder;

		public GetValue(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			if (webElement.TagName == "input" && webElement.GetAttribute("type") == "checkbox")
			{
				if (webElement.GetAttribute("checked") == null)
				{
					return "off";
				}
				return "on";
			}
			return webElement.GetAttribute("value");
		}
	}
	internal class GetXpathCount : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.FindElements(By.XPath(locator)).Count;
		}
	}
	internal class GoBack : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string ignored, string alsoIgnored)
		{
			driver.Navigate().Back();
			return null;
		}
	}
	internal class Highlight : SeleneseCommand
	{
		private ElementFinder finder;

		public Highlight(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			JavaScriptLibrary.CallEmbeddedHtmlUtilities(driver, "highlight", finder.FindElement(driver, locator));
			return null;
		}
	}
	public interface IFilterFunction
	{
		IList<IWebElement> FilterElements(IList<IWebElement> allElements, string filterValue);
	}
}
namespace Selenium
{
	internal interface ILookupStrategy
	{
		IWebElement Find(IWebDriver driver, string use);
	}
}
namespace Selenium.Internal.SeleniumEmulation
{
	internal interface IOptionSelectStrategy
	{
		bool SelectOption(ReadOnlyCollection<IWebElement> fromOptions, string selectThis, bool setSelected, bool allowMultipleSelect);
	}
	internal class IsAlertPresent : SeleneseCommand
	{
		private AlertOverride alertOverride;

		public IsAlertPresent(AlertOverride alertOverride)
		{
			this.alertOverride = alertOverride;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return alertOverride.IsAlertPresent();
		}
	}
	internal class IsChecked : SeleneseCommand
	{
		private ElementFinder finder;

		public IsChecked(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Selected;
		}
	}
	internal class IsConfirmationPresent : SeleneseCommand
	{
		private AlertOverride alertOverride;

		public IsConfirmationPresent(AlertOverride alertOverride)
		{
			this.alertOverride = alertOverride;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return alertOverride.IsConfirmationPresent();
		}
	}
	internal class IsCookiePresent : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return driver.Manage().Cookies.GetCookieNamed(locator) != null;
		}
	}
	internal class IsEditable : SeleneseCommand
	{
		private ElementFinder finder;

		public IsEditable(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			string text = webElement.TagName.ToUpperInvariant();
			bool flag = text == "INPUT" || text == "SELECT";
			string text2 = string.Empty;
			if (text == "INPUT")
			{
				text2 = webElement.GetAttribute("readonly");
				if (text2 != null && text2 == "false")
				{
					text2 = string.Empty;
				}
			}
			return webElement.Enabled && flag && string.IsNullOrEmpty(text2);
		}
	}
	internal class IsElementPresent : SeleneseCommand
	{
		private ElementFinder finder;

		public IsElementPresent(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			try
			{
				finder.FindElement(driver, locator);
				return true;
			}
			catch (SeleniumException)
			{
				return false;
			}
		}
	}
	internal class IsOrdered : SeleneseCommand
	{
		private ElementFinder finder;

		public IsOrdered(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			IWebElement webElement2 = finder.FindElement(driver, value);
			string script = "    if (arguments[0] === arguments[1]) return false;\n\n    var previousSibling;\n    while ((previousSibling = arguments[1].previousSibling) != null) {\n        if (previousSibling === arguments[0]) {\n            return true;\n        }\n        arguments[1] = previousSibling;\n    }\n    return false;\n";
			bool? flag = (bool)JavaScriptLibrary.ExecuteScript(driver, script, webElement, webElement2);
			return flag.HasValue && flag.Value;
		}
	}
	internal class IsSomethingSelected : SeleneseCommand
	{
		private string script;

		public IsSomethingSelected()
		{
			script = "return (" + JavaScriptLibrary.GetSeleniumScript("isSomethingSelected.js") + ").apply(null, arguments)";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			object obj = JavaScriptLibrary.ExecuteScript(driver, script, locator);
			return obj is bool && (bool)obj;
		}
	}
	internal class IsTextPresent : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string seleniumScript = JavaScriptLibrary.GetSeleniumScript("isTextPresent.js");
			object obj = ((IJavaScriptExecutor)driver).ExecuteScript("return (" + seleniumScript + ")(arguments[0]);", locator);
			return obj is bool && (bool)obj;
		}
	}
	internal class IsVisible : SeleneseCommand
	{
		private ElementFinder finder;

		public IsVisible(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return finder.FindElement(driver, locator).Displayed;
		}
	}
	internal interface ITextMatchingStrategy
	{
		bool IsAMatch(string compareThis, string compareTo);
	}
	public static class JavaScriptLibrary
	{
		private const string InjectableSeleniumResourceName = "injectableSelenium.js";

		private const string HtmlUtilsResourceName = "htmlutils.js";

		public static void CallEmbeddedSelenium(IWebDriver driver, string functionName, IWebElement element, params object[] values)
		{
			StringBuilder stringBuilder = new StringBuilder(ReadScript("injectableSelenium.js"));
			stringBuilder.Append("return browserbot.").Append(functionName).Append(".apply(browserbot, arguments);");
			List<object> list = new List<object>();
			list.Add(element);
			list.AddRange(values);
			((IJavaScriptExecutor)driver).ExecuteScript(stringBuilder.ToString(), list.ToArray());
		}

		public static object CallEmbeddedHtmlUtilities(IWebDriver driver, string functionName, IWebElement element, params object[] values)
		{
			StringBuilder stringBuilder = new StringBuilder(ReadScript("htmlutils.js"));
			stringBuilder.Append("return htmlutils.").Append(functionName).Append(".apply(htmlutils, arguments);");
			List<object> list = new List<object>();
			list.Add(element);
			list.AddRange(values);
			return ((IJavaScriptExecutor)driver).ExecuteScript(stringBuilder.ToString(), list.ToArray());
		}

		public static object ExecuteScript(IWebDriver driver, string script, params object[] args)
		{
			if (!(driver is IJavaScriptExecutor javaScriptExecutor))
			{
				throw new InvalidOperationException("The underlying WebDriver instance does not support executing javascript");
			}
			return javaScriptExecutor.ExecuteScript(script, args);
		}

		public static string GetSeleniumScript(string libraryName)
		{
			string text = ReadScript(libraryName);
			return string.Format(CultureInfo.InvariantCulture, "function() {{ return ({0}).apply(null, arguments);}}", new object[1] { text });
		}

		private static string ReadScript(string script)
		{
			string empty = string.Empty;
			Stream resourceStream = ResourceUtilities.GetResourceStream(script, script);
			using TextReader textReader = new StreamReader(resourceStream);
			return textReader.ReadToEnd();
		}
	}
	internal class KeyEvent : SeleneseCommand
	{
		private ElementFinder finder;

		private KeyState keyState;

		private string eventName;

		public KeyEvent(ElementFinder elementFinder, KeyState state, string eventName)
		{
			finder = elementFinder;
			keyState = state;
			this.eventName = eventName;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			object[] values = new object[5] { value, keyState.ControlKeyDown, keyState.AltKeyDown, keyState.ShiftKeyDown, keyState.MetaKeyDown };
			JavaScriptLibrary.CallEmbeddedSelenium(driver, eventName, finder.FindElement(driver, locator), values);
			return null;
		}
	}
	internal class KeyState
	{
		private bool altKeyDown;

		private bool controlKeyDown;

		private bool shiftKeyDown;

		private bool metaKeyDown;

		public bool AltKeyDown
		{
			get
			{
				return altKeyDown;
			}
			set
			{
				altKeyDown = value;
			}
		}

		public bool ControlKeyDown
		{
			get
			{
				return controlKeyDown;
			}
			set
			{
				controlKeyDown = value;
			}
		}

		public bool ShiftKeyDown
		{
			get
			{
				return shiftKeyDown;
			}
			set
			{
				shiftKeyDown = value;
			}
		}

		public bool MetaKeyDown
		{
			get
			{
				return metaKeyDown;
			}
			set
			{
				metaKeyDown = value;
			}
		}
	}
	internal class MetaKeyDown : SeleneseCommand
	{
		private KeyState keyState;

		public MetaKeyDown(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.MetaKeyDown = true;
			return null;
		}
	}
	internal class MetaKeyUp : SeleneseCommand
	{
		private KeyState keyState;

		public MetaKeyUp(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.MetaKeyDown = false;
			return null;
		}
	}
	internal class MouseEvent : SeleneseCommand
	{
		private ElementFinder finder;

		private string type;

		private string fire;

		public MouseEvent(ElementFinder elementFinder, string eventType)
		{
			finder = elementFinder;
			type = eventType;
			fire = "return (" + JavaScriptLibrary.GetSeleniumScript("fireEvent.js") + ").apply(null, arguments);";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			JavaScriptLibrary.ExecuteScript(driver, fire, webElement, type);
			return null;
		}
	}
	internal class MouseEventAt : SeleneseCommand
	{
		private ElementFinder finder;

		private string type;

		private string fire;

		public MouseEventAt(ElementFinder elementFinder, string eventType)
		{
			finder = elementFinder;
			type = eventType;
			fire = "return (" + JavaScriptLibrary.GetSeleniumScript("fireEventAt.js") + ").apply(null, arguments);";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			JavaScriptLibrary.ExecuteScript(driver, fire, webElement, type, value);
			return null;
		}
	}
	internal class NoOp : SeleneseCommand
	{
		private object returnValue;

		public NoOp(object toReturn)
		{
			returnValue = toReturn;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			return returnValue;
		}
	}
	internal class Open : SeleneseCommand
	{
		private Uri baseUrl;

		public Open(Uri baseUrl)
		{
			this.baseUrl = baseUrl;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string url = ConstructUrl(locator);
			driver.Url = url;
			return null;
		}

		private string ConstructUrl(string path)
		{
			return path.Contains("://") ? path : (baseUrl.ToString().TrimEnd(new char[1] { '/' }) + ((!path.StartsWith("/", StringComparison.Ordinal)) ? "/" : string.Empty) + path);
		}
	}
	internal class OpenWindow : SeleneseCommand
	{
		private GetEval opener;

		public OpenWindow(GetEval windowOpener)
		{
			opener = windowOpener;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string[] args = new string[1] { string.Format(CultureInfo.InvariantCulture, "window.open('{0}', '{1}'); null;", new object[2] { locator, value }) };
			opener.Apply(driver, args);
			return null;
		}
	}
	internal class Refresh : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			driver.Navigate().Refresh();
			return null;
		}
	}
	internal class RemoveAllSelections : SeleneseCommand
	{
		private ElementFinder finder;

		public RemoveAllSelections(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			ReadOnlyCollection<IWebElement> readOnlyCollection = webElement.FindElements(By.TagName("option"));
			string attribute = webElement.GetAttribute("multiple");
			if (string.IsNullOrEmpty(attribute))
			{
				return null;
			}
			foreach (IWebElement item in readOnlyCollection)
			{
				if (item.Selected)
				{
					item.Click();
				}
			}
			return null;
		}
	}
	internal class RemoveSelection : SeleneseCommand
	{
		private ElementFinder finder;

		public RemoveSelection(ElementFinder elementFinder)
		{
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			seleniumSelect.RemoveSelection(value);
			return null;
		}
	}
	internal class RunScript : SeleneseCommand
	{
		private IScriptMutator mutator;

		public RunScript(IScriptMutator mutator)
		{
			this.mutator = mutator;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			mutator.Mutate(locator, stringBuilder);
			((IJavaScriptExecutor)driver).ExecuteScript(stringBuilder.ToString());
			return null;
		}
	}
	internal class SelectFrame : SeleneseCommand
	{
		private WindowSelector windows;

		public SelectFrame(WindowSelector windowSelector)
		{
			windows = windowSelector;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			windows.SelectFrame(driver, locator);
			return null;
		}
	}
	internal class SelectOption : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alertOverride;

		public SelectOption(AlertOverride alertOverride, ElementFinder finder)
		{
			this.finder = finder;
			this.alertOverride = alertOverride;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alertOverride.ReplaceAlertMethod();
			SeleniumSelect seleniumSelect = new SeleniumSelect(finder, driver, locator);
			seleniumSelect.SetSelected(value);
			return null;
		}
	}
	internal class SelectWindow : SeleneseCommand
	{
		private WindowSelector windows;

		public SelectWindow(WindowSelector windowSelector)
		{
			windows = windowSelector;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			windows.SelectWindow(driver, locator);
			return null;
		}
	}
	public class SeleniumMutator : IScriptMutator
	{
		private Regex pattern;

		private string method;

		private string atom;

		public SeleniumMutator(string method, string atom)
		{
			if (method == null)
			{
				method = string.Empty;
			}
			string text = ".*" + method.Replace(".", "\\s*\\.\\s*") + ".*";
			pattern = new Regex(text);
			this.method = method;
			this.atom = atom;
		}

		public void Mutate(string script, StringBuilder outputTo)
		{
			if (outputTo != null && pattern.IsMatch(script))
			{
				outputTo.Append(string.Format(CultureInfo.InvariantCulture, "{0} = function() {{ return ({1}).apply(null, arguments);}};", new object[2] { method, atom }));
			}
		}
	}
	internal class SeleniumSelect
	{
		private string findOption;

		private IWebDriver driver;

		private IWebElement select;

		public ReadOnlyCollection<IWebElement> AllOptions => select.FindElements(By.TagName("option"));

		public ReadOnlyCollection<IWebElement> SelectedOptions
		{
			get
			{
				List<IWebElement> list = new List<IWebElement>();
				foreach (IWebElement item in select.FindElements(By.TagName("option")))
				{
					if (item.Selected)
					{
						list.Add(item);
					}
				}
				return list.AsReadOnly();
			}
		}

		private bool IsMultiple
		{
			get
			{
				string attribute = select.GetAttribute("multiple");
				return attribute == "true" || attribute == "multiple";
			}
		}

		public SeleniumSelect(ElementFinder finder, IWebDriver driver, string locator)
		{
			this.driver = driver;
			findOption = "return (" + JavaScriptLibrary.GetSeleniumScript("findOption.js") + ").apply(null, arguments)";
			select = finder.FindElement(driver, locator);
			if (select.TagName.ToLowerInvariant() != "select")
			{
				throw new SeleniumException("Element is not a select element: " + locator);
			}
		}

		public void SetSelected(string optionLocator)
		{
			if (IsMultiple)
			{
				foreach (IWebElement item in select.FindElements(By.TagName("option")))
				{
					if (item.Selected)
					{
						item.Click();
					}
				}
			}
			LocateOption(optionLocator)?.Click();
		}

		public void AddSelection(string optionLocator)
		{
			AssertSupportsMultipleSelections();
			LocateOption(optionLocator)?.Click();
		}

		public void RemoveSelection(string optionLocator)
		{
			AssertSupportsMultipleSelections();
			IWebElement webElement = LocateOption(optionLocator);
			if (webElement != null && webElement.Selected)
			{
				webElement.Click();
			}
		}

		private IWebElement LocateOption(string optionLocator)
		{
			IWebElement result = null;
			try
			{
				result = ((IJavaScriptExecutor)driver).ExecuteScript(findOption, select, optionLocator) as IWebElement;
			}
			catch (InvalidOperationException)
			{
			}
			return result;
		}

		private void AssertSupportsMultipleSelections()
		{
			if (!IsMultiple)
			{
				throw new SeleniumException("You may only add a selection to a select that supports multiple selections");
			}
		}
	}
	internal class SetNextConfirmationState : SeleneseCommand
	{
		private bool result;

		public SetNextConfirmationState(bool result)
		{
			this.result = result;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			((IJavaScriptExecutor)driver).ExecuteScript("var canUseLocalStorage = false; try { canUseLocalStorage = !!window.localStorage; } catch(ex) { /* probe failed */ } var canUseJSON = false; try { canUseJSON = !!JSON; } catch(ex) { /* probe failed */ } if (canUseLocalStorage && canUseJSON) {   window.localStorage.setItem('__webdriverNextConfirm', JSON.stringify(arguments[0])); } else {   window.__webdriverNextConfirm = arguments[0];}", result);
			return null;
		}
	}
	internal class SetTimeout : SeleneseCommand
	{
		private CommandTimer timer;

		public SetTimeout(CommandTimer timer)
		{
			this.timer = timer;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			if (locator == "0")
			{
				timer.Timeout = int.MaxValue;
			}
			else
			{
				timer.Timeout = int.Parse(locator, CultureInfo.InvariantCulture);
			}
			return null;
		}
	}
	internal class ShiftKeyDown : SeleneseCommand
	{
		private KeyState keyState;

		public ShiftKeyDown(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.ShiftKeyDown = true;
			return null;
		}
	}
	internal class ShiftKeyUp : SeleneseCommand
	{
		private KeyState keyState;

		public ShiftKeyUp(KeyState keyState)
		{
			this.keyState = keyState;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			keyState.ShiftKeyDown = false;
			return null;
		}
	}
	internal class Submit : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alert;

		public Submit(AlertOverride alert, ElementFinder finder)
		{
			this.alert = alert;
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alert.ReplaceAlertMethod();
			finder.FindElement(driver, locator).Submit();
			return null;
		}
	}
	internal class Type : SeleneseCommand
	{
		private AlertOverride alertOverride;

		private ElementFinder finder;

		private KeyState state;

		private string type;

		public Type(AlertOverride alertOverride, ElementFinder elementFinder, KeyState keyState)
		{
			this.alertOverride = alertOverride;
			finder = elementFinder;
			state = keyState;
			type = "return (" + JavaScriptLibrary.GetSeleniumScript("type.js") + ").apply(null, arguments);";
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alertOverride.ReplaceAlertMethod();
			if (state.ControlKeyDown || state.AltKeyDown || state.MetaKeyDown)
			{
				throw new SeleniumException("type not supported immediately after call to controlKeyDown() or altKeyDown() or metaKeyDown()");
			}
			string text = (state.ShiftKeyDown ? value.ToUpperInvariant() : value);
			IWebElement webElement = finder.FindElement(driver, locator);
			string tagName = webElement.TagName;
			string attribute = webElement.GetAttribute("type");
			if (tagName.ToLowerInvariant() == "input" && attribute != null && attribute.ToLowerInvariant() == "file")
			{
				webElement.SendKeys(text);
				return null;
			}
			if (driver is IJavaScriptExecutor)
			{
				JavaScriptLibrary.ExecuteScript(driver, type, webElement, text);
			}
			else
			{
				webElement.SendKeys(text);
			}
			return null;
		}
	}
	internal class TypeKeys : SeleneseCommand
	{
		private ElementFinder finder;

		private AlertOverride alertOverride;

		public TypeKeys(AlertOverride alertOverride, ElementFinder elementFinder)
		{
			this.alertOverride = alertOverride;
			finder = elementFinder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			alertOverride.ReplaceAlertMethod();
			value = value.Replace("\\10", Keys.Enter);
			value = value.Replace("\\13", Keys.Return);
			value = value.Replace("\\27", Keys.Escape);
			value = value.Replace("\\38", Keys.ArrowUp);
			value = value.Replace("\\40", Keys.ArrowDown);
			value = value.Replace("\\37", Keys.ArrowLeft);
			value = value.Replace("\\39", Keys.ArrowRight);
			finder.FindElement(driver, locator).SendKeys(value);
			return null;
		}
	}
	internal class Uncheck : SeleneseCommand
	{
		private ElementFinder finder;

		public Uncheck(ElementFinder finder)
		{
			this.finder = finder;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			IWebElement webElement = finder.FindElement(driver, locator);
			if (webElement.Selected)
			{
				webElement.Click();
			}
			return null;
		}
	}
	public class VariableDeclaration : IScriptMutator
	{
		private Regex pattern;

		private string declaration;

		public VariableDeclaration(string raw, string declaration)
		{
			if (raw == null)
			{
				raw = string.Empty;
			}
			this.declaration = declaration;
			raw = raw.Replace(".", "\\s*\\.\\s*").Replace("(", "\\(").Replace(")", "\\)");
			pattern = new Regex(".*" + raw + ".*");
		}

		public void Mutate(string script, StringBuilder outputTo)
		{
			if (outputTo != null && pattern.IsMatch(script))
			{
				outputTo.Append(declaration);
			}
		}
	}
	internal abstract class Waiter
	{
		[Serializable]
		public class WaitTimedOutException : Exception
		{
			public WaitTimedOutException(string message)
				: base(message)
			{
			}

			protected WaitTimedOutException(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}

		private const long DefaultTimeout = 30000L;

		private const long DefaultInterval = 500L;

		public Waiter()
		{
		}

		public void Wait(string message)
		{
			Wait(message, 30000L, 500L);
		}

		public void Wait(string message, long timeoutInMilliseconds)
		{
			Wait(message, timeoutInMilliseconds, 500L);
		}

		public void Wait(string message, long timeoutInMilliseconds, long intervalInMilliseconds)
		{
			ParameterizedThreadStart start = CheckForConditionUntilTimeout;
			Thread thread = new Thread(start);
			thread.Name = "WebDriver";
			thread.Start(intervalInMilliseconds);
			thread.Join((int)timeoutInMilliseconds);
			if (thread.IsAlive)
			{
				thread.Abort();
				throw new WaitTimedOutException(message);
			}
		}

		public abstract bool Until();

		private void CheckForConditionUntilTimeout(object intervalInMilliseconds)
		{
			long num = (long)intervalInMilliseconds;
			while (!Until())
			{
				Thread.Sleep(TimeSpan.FromMilliseconds(num));
			}
		}
	}
	internal class WaitForCondition : SeleneseCommand
	{
		private class ConditionWaiter : Waiter
		{
			private IWebDriver driver;

			private string script;

			public ConditionWaiter(IWebDriver driver, string script)
			{
				this.driver = driver;
				this.script = script;
			}

			public override bool Until()
			{
				object obj = ((IJavaScriptExecutor)driver).ExecuteScript(script);
				if (obj == null)
				{
					return false;
				}
				if (obj is string)
				{
					return !string.IsNullOrEmpty(obj.ToString());
				}
				if (obj is bool)
				{
					return (bool)obj;
				}
				return true;
			}
		}

		private IScriptMutator mutator;

		public WaitForCondition(IScriptMutator mutator)
		{
			this.mutator = mutator;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			mutator.Mutate(locator, stringBuilder);
			string script = stringBuilder.ToString();
			string message = "Failed to resolve " + locator;
			ConditionWaiter conditionWaiter = new ConditionWaiter(driver, script);
			if (!string.IsNullOrEmpty(value))
			{
				conditionWaiter.Wait(message, long.Parse(value, CultureInfo.InvariantCulture));
			}
			else
			{
				conditionWaiter.Wait(message);
			}
			return null;
		}
	}
	internal class WaitForPageToLoad : SeleneseCommand
	{
		private class ReadyStateWaiter : Waiter
		{
			private IWebDriver driver;

			public ReadyStateWaiter(IWebDriver driver)
			{
				this.driver = driver;
			}

			public override bool Until()
			{
				try
				{
					object obj = ((IJavaScriptExecutor)driver).ExecuteScript("return 'complete' == document.readyState;");
					if (obj != null && obj is bool && (bool)obj)
					{
						return true;
					}
				}
				catch (Exception)
				{
				}
				return false;
			}
		}

		private class LengthCheckingWaiter : Waiter
		{
			private IWebDriver driver;

			private int length;

			private DateTime seenAt;

			public LengthCheckingWaiter(IWebDriver driver)
			{
				this.driver = driver;
			}

			public override bool Until()
			{
				try
				{
					int num = driver.FindElement(By.TagName("body")).Text.Length;
					if (num != length)
					{
						seenAt = DateTime.Now;
						length = num;
						return false;
					}
					return DateTime.Now.Subtract(seenAt).TotalMilliseconds > 1000.0;
				}
				catch (NoSuchElementException)
				{
				}
				return false;
			}
		}

		private int timeToWaitAfterPageLoad = 100;

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			if (!(driver is IJavaScriptExecutor javaScriptExecutor))
			{
				return null;
			}
			long timeoutInMilliseconds = long.Parse(locator, CultureInfo.InvariantCulture);
			Pause();
			object obj = javaScriptExecutor.ExecuteScript("return !!document['readyState'];");
			Waiter waiter = ((obj != null && (bool)obj) ? ((Waiter)new ReadyStateWaiter(driver)) : ((Waiter)new LengthCheckingWaiter(driver)));
			waiter.Wait(string.Format(CultureInfo.InvariantCulture, "Failed to load page within {0} ms", new object[1] { locator }), timeoutInMilliseconds);
			Pause();
			return null;
		}

		private void Pause()
		{
			Thread.Sleep(timeToWaitAfterPageLoad);
		}
	}
	internal class WaitForPopup : SeleneseCommand
	{
		private class PopupWaiter : Waiter
		{
			private string windowId;

			private WindowSelector windows;

			private IWebDriver driver;

			public PopupWaiter(IWebDriver driver, string windowId, WindowSelector windows)
			{
				this.driver = driver;
				this.windowId = windowId;
				this.windows = windows;
			}

			public override bool Until()
			{
				try
				{
					if (windowId == "_blank")
					{
						windows.SelectBlankWindow(driver);
					}
					else
					{
						try
						{
							driver.SwitchTo().Window(windowId);
						}
						catch (NoSuchWindowException)
						{
							return false;
						}
					}
					return driver.Url != "about:blank";
				}
				catch (SeleniumException)
				{
				}
				return false;
			}
		}

		private WindowSelector windows;

		public WaitForPopup(WindowSelector windowSelector)
		{
			windows = windowSelector;
		}

		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			string message = string.Format(CultureInfo.InvariantCulture, "Timed out waiting for {0}. Waited {1}", new object[2] { locator, value });
			PopupWaiter popupWaiter = new PopupWaiter(driver, locator, windows);
			if (!string.IsNullOrEmpty(value))
			{
				long timeoutInMilliseconds = long.Parse(value, CultureInfo.InvariantCulture);
				popupWaiter.Wait(message, timeoutInMilliseconds);
			}
			else
			{
				popupWaiter.Wait(message);
			}
			return null;
		}
	}
	internal class WindowFocus : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			JavaScriptLibrary.ExecuteScript(driver, "window.focus()");
			return null;
		}
	}
	internal class WindowMaximize : SeleneseCommand
	{
		protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
		{
			JavaScriptLibrary.ExecuteScript(driver, "if (window.screen) { window.moveTo(0, 0); window.resizeTo(window.screen.availWidth, window.screen.availHeight);};");
			return null;
		}
	}
	internal class WindowSelector
	{
		private Dictionary<string, string> lastFrame = new Dictionary<string, string>();

		private string originalWindowHandle;

		public WindowSelector(IWebDriver driver)
		{
			originalWindowHandle = driver.CurrentWindowHandle;
		}

		public void SelectWindow(IWebDriver driver, string windowId)
		{
			if (windowId == "null")
			{
				driver.SwitchTo().Window(originalWindowHandle);
			}
			else if (windowId == "_blank")
			{
				SelectBlankWindow(driver);
			}
			else
			{
				if (windowId.StartsWith("title=", StringComparison.Ordinal))
				{
					SelectWindowWithTitle(driver, windowId.Substring("title=".Length));
					return;
				}
				if (windowId.StartsWith("name=", StringComparison.Ordinal))
				{
					windowId = windowId.Substring("name=".Length);
				}
				try
				{
					driver.SwitchTo().Window(windowId);
				}
				catch (NoSuchWindowException)
				{
					SelectWindowWithTitle(driver, windowId);
				}
			}
			if (lastFrame.ContainsKey(driver.CurrentWindowHandle))
			{
				try
				{
					SelectFrame(driver, lastFrame[driver.CurrentWindowHandle]);
				}
				catch (SeleniumException)
				{
					lastFrame.Remove(driver.CurrentWindowHandle);
				}
			}
		}

		public void SelectFrame(IWebDriver driver, string locator)
		{
			if (locator == "relative=top")
			{
				driver.SwitchTo().DefaultContent();
				lastFrame.Remove(driver.CurrentWindowHandle);
				return;
			}
			try
			{
				if (!lastFrame.ContainsKey(driver.CurrentWindowHandle))
				{
					lastFrame.Add(driver.CurrentWindowHandle, locator);
				}
				driver.SwitchTo().Frame(locator);
			}
			catch (NoSuchFrameException ex)
			{
				throw new SeleniumException(ex.Message);
			}
		}

		public void SelectBlankWindow(IWebDriver driver)
		{
			string currentWindowHandle = driver.CurrentWindowHandle;
			ReadOnlyCollection<string> windowHandles = driver.WindowHandles;
			foreach (string item in windowHandles)
			{
				if (!(item == originalWindowHandle))
				{
					driver.SwitchTo().Window(item);
					string value = ((IJavaScriptExecutor)driver).ExecuteScript("return window.name;").ToString();
					if (string.IsNullOrEmpty(value))
					{
						return;
					}
				}
			}
			driver.SwitchTo().Window(currentWindowHandle);
			throw new SeleniumException("Unable to select window _blank");
		}

		private static void SelectWindowWithTitle(IWebDriver driver, string title)
		{
			bool flag = false;
			string currentWindowHandle = driver.CurrentWindowHandle;
			foreach (string windowHandle in driver.WindowHandles)
			{
				driver.SwitchTo().Window(windowHandle);
				if (title == driver.Title)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				driver.SwitchTo().Window(currentWindowHandle);
				throw new SeleniumException("Unable to select window with title: " + title);
			}
		}
	}
}

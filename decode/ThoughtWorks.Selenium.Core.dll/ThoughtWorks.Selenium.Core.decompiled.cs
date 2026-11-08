using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

[assembly: AssemblyTitle("Selenium .NET Client Driver")]
[assembly: AssemblyDescription("Selenium RC API .NET Bindings")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Selenium Committers")]
[assembly: AssemblyProduct("Selenium")]
[assembly: AssemblyCopyright("Copyright © Software Freedom Conservancy 2014")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyFileVersion("2.52.0.0")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyVersion("2.52.0.0")]
namespace Selenium;

public interface IRemoteCommand
{
	string CommandString { get; }
}
public class DefaultRemoteCommand : IRemoteCommand
{
	private static readonly string PARSE_ERROR_MESSAGE = "Command string must contain 4 pipe characters and should start with a '|'. Unable to parse command string";

	private readonly string[] args;

	private readonly string command;

	public string CommandString
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder("cmd=");
			stringBuilder.Append(HttpUtility.UrlEncode(command));
			if (args == null)
			{
				return stringBuilder.ToString();
			}
			for (int i = 0; i < args.Length; i++)
			{
				stringBuilder.Append('&').Append((i + 1).ToString()).Append('=')
					.Append(HttpUtility.UrlEncode(args[i]));
			}
			return stringBuilder.ToString();
		}
	}

	public string Command => command;

	public string[] Args => args;

	public DefaultRemoteCommand(string command, string[] args)
	{
		this.command = command;
		this.args = args;
	}

	public static DefaultRemoteCommand Parse(string commandString)
	{
		if (commandString == null || commandString.Trim().Length == 0 || !commandString.StartsWith("|"))
		{
			throw new ArgumentException(PARSE_ERROR_MESSAGE + "'" + commandString + "'.");
		}
		string[] array = commandString.Split(new char[1] { '|' });
		if (array.Length != 5)
		{
			throw new ArgumentException(PARSE_ERROR_MESSAGE + "'" + commandString + "'.");
		}
		return new DefaultRemoteCommand(array[1], new string[2]
		{
			array[2],
			array[3]
		});
	}
}
public interface ISelenium
{
	void SetExtensionJs(string extensionJs);

	void Start();

	void Stop();

	void Click(string locator);

	void DoubleClick(string locator);

	void ContextMenu(string locator);

	void ClickAt(string locator, string coordString);

	void DoubleClickAt(string locator, string coordString);

	void ContextMenuAt(string locator, string coordString);

	void FireEvent(string locator, string eventName);

	void Focus(string locator);

	void KeyPress(string locator, string keySequence);

	void ShiftKeyDown();

	void ShiftKeyUp();

	void MetaKeyDown();

	void MetaKeyUp();

	void AltKeyDown();

	void AltKeyUp();

	void ControlKeyDown();

	void ControlKeyUp();

	void KeyDown(string locator, string keySequence);

	void KeyUp(string locator, string keySequence);

	void MouseOver(string locator);

	void MouseOut(string locator);

	void MouseDown(string locator);

	void MouseDownRight(string locator);

	void MouseDownAt(string locator, string coordString);

	void MouseDownRightAt(string locator, string coordString);

	void MouseUp(string locator);

	void MouseUpRight(string locator);

	void MouseUpAt(string locator, string coordString);

	void MouseUpRightAt(string locator, string coordString);

	void MouseMove(string locator);

	void MouseMoveAt(string locator, string coordString);

	void Type(string locator, string value);

	void TypeKeys(string locator, string value);

	void SetSpeed(string value);

	string GetSpeed();

	void Check(string locator);

	void Uncheck(string locator);

	void Select(string selectLocator, string optionLocator);

	void AddSelection(string locator, string optionLocator);

	void RemoveSelection(string locator, string optionLocator);

	void RemoveAllSelections(string locator);

	void Submit(string formLocator);

	void Open(string url);

	void OpenWindow(string url, string windowID);

	void SelectWindow(string windowID);

	void SelectPopUp(string windowID);

	void DeselectPopUp();

	void SelectFrame(string locator);

	bool GetWhetherThisFrameMatchFrameExpression(string currentFrameString, string target);

	bool GetWhetherThisWindowMatchWindowExpression(string currentWindowString, string target);

	void WaitForPopUp(string windowID, string timeout);

	void ChooseCancelOnNextConfirmation();

	void ChooseOkOnNextConfirmation();

	void AnswerOnNextPrompt(string answer);

	void GoBack();

	void Refresh();

	void Close();

	bool IsAlertPresent();

	bool IsPromptPresent();

	bool IsConfirmationPresent();

	string GetAlert();

	string GetConfirmation();

	string GetPrompt();

	string GetLocation();

	string GetTitle();

	string GetBodyText();

	string GetValue(string locator);

	string GetText(string locator);

	void Highlight(string locator);

	string GetEval(string script);

	bool IsChecked(string locator);

	string GetTable(string tableCellAddress);

	string[] GetSelectedLabels(string selectLocator);

	string GetSelectedLabel(string selectLocator);

	string[] GetSelectedValues(string selectLocator);

	string GetSelectedValue(string selectLocator);

	string[] GetSelectedIndexes(string selectLocator);

	string GetSelectedIndex(string selectLocator);

	string[] GetSelectedIds(string selectLocator);

	string GetSelectedId(string selectLocator);

	bool IsSomethingSelected(string selectLocator);

	string[] GetSelectOptions(string selectLocator);

	string GetAttribute(string attributeLocator);

	bool IsTextPresent(string pattern);

	bool IsElementPresent(string locator);

	bool IsVisible(string locator);

	bool IsEditable(string locator);

	string[] GetAllButtons();

	string[] GetAllLinks();

	string[] GetAllFields();

	string[] GetAttributeFromAllWindows(string attributeName);

	void Dragdrop(string locator, string movementsString);

	void SetMouseSpeed(string pixels);

	decimal GetMouseSpeed();

	void DragAndDrop(string locator, string movementsString);

	void DragAndDropToObject(string locatorOfObjectToBeDragged, string locatorOfDragDestinationObject);

	void WindowFocus();

	void WindowMaximize();

	string[] GetAllWindowIds();

	string[] GetAllWindowNames();

	string[] GetAllWindowTitles();

	string GetHtmlSource();

	void SetCursorPosition(string locator, string position);

	decimal GetElementIndex(string locator);

	bool IsOrdered(string locator1, string locator2);

	decimal GetElementPositionLeft(string locator);

	decimal GetElementPositionTop(string locator);

	decimal GetElementWidth(string locator);

	decimal GetElementHeight(string locator);

	decimal GetCursorPosition(string locator);

	string GetExpression(string expression);

	decimal GetXpathCount(string xpath);

	decimal GetCSSCount(string cssLocator);

	void AssignId(string locator, string identifier);

	void AllowNativeXpath(string allow);

	void IgnoreAttributesWithoutValue(string ignore);

	void WaitForCondition(string script, string timeout);

	void SetTimeout(string timeout);

	void WaitForPageToLoad(string timeout);

	void WaitForFrameToLoad(string frameAddress, string timeout);

	string GetCookie();

	string GetCookieByName(string name);

	bool IsCookiePresent(string name);

	void CreateCookie(string nameValuePair, string optionsString);

	void DeleteCookie(string name, string optionsString);

	void DeleteAllVisibleCookies();

	void SetBrowserLogLevel(string logLevel);

	void RunScript(string script);

	void AddLocationStrategy(string strategyName, string functionDefinition);

	void CaptureEntirePageScreenshot(string filename, string kwargs);

	void Rollup(string rollupName, string kwargs);

	void AddScript(string scriptContent, string scriptTagId);

	void RemoveScript(string scriptTagId);

	void UseXpathLibrary(string libraryName);

	void SetContext(string context);

	void AttachFile(string fieldLocator, string fileLocator);

	void CaptureScreenshot(string filename);

	string CaptureScreenshotToString();

	string CaptureEntirePageScreenshotToString(string kwargs);

	void ShutDownSeleniumServer();

	string RetrieveLastRemoteControlLogs();

	void KeyDownNative(string keycode);

	void KeyUpNative(string keycode);

	void KeyPressNative(string keycode);
}
public class DefaultSelenium : ISelenium
{
	protected ICommandProcessor commandProcessor;

	public ICommandProcessor Processor => commandProcessor;

	public DefaultSelenium(string serverHost, int serverPort, string browserString, string browserURL)
	{
		commandProcessor = new HttpCommandProcessor(serverHost, serverPort, browserString, browserURL);
	}

	public DefaultSelenium(ICommandProcessor processor)
	{
		commandProcessor = processor;
	}

	public void SetExtensionJs(string extensionJs)
	{
		commandProcessor.SetExtensionJs(extensionJs);
	}

	public void Start()
	{
		commandProcessor.Start();
	}

	public void Start(string optionsString)
	{
		commandProcessor.Start(optionsString);
	}

	public void Stop()
	{
		commandProcessor.Stop();
	}

	public void Click(string locator)
	{
		commandProcessor.DoCommand("click", new string[1] { locator });
	}

	public void DoubleClick(string locator)
	{
		commandProcessor.DoCommand("doubleClick", new string[1] { locator });
	}

	public void ContextMenu(string locator)
	{
		commandProcessor.DoCommand("contextMenu", new string[1] { locator });
	}

	public void ClickAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("clickAt", new string[2] { locator, coordString });
	}

	public void DoubleClickAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("doubleClickAt", new string[2] { locator, coordString });
	}

	public void ContextMenuAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("contextMenuAt", new string[2] { locator, coordString });
	}

	public void FireEvent(string locator, string eventName)
	{
		commandProcessor.DoCommand("fireEvent", new string[2] { locator, eventName });
	}

	public void Focus(string locator)
	{
		commandProcessor.DoCommand("focus", new string[1] { locator });
	}

	public void KeyPress(string locator, string keySequence)
	{
		commandProcessor.DoCommand("keyPress", new string[2] { locator, keySequence });
	}

	public void ShiftKeyDown()
	{
		commandProcessor.DoCommand("shiftKeyDown", new string[0]);
	}

	public void ShiftKeyUp()
	{
		commandProcessor.DoCommand("shiftKeyUp", new string[0]);
	}

	public void MetaKeyDown()
	{
		commandProcessor.DoCommand("metaKeyDown", new string[0]);
	}

	public void MetaKeyUp()
	{
		commandProcessor.DoCommand("metaKeyUp", new string[0]);
	}

	public void AltKeyDown()
	{
		commandProcessor.DoCommand("altKeyDown", new string[0]);
	}

	public void AltKeyUp()
	{
		commandProcessor.DoCommand("altKeyUp", new string[0]);
	}

	public void ControlKeyDown()
	{
		commandProcessor.DoCommand("controlKeyDown", new string[0]);
	}

	public void ControlKeyUp()
	{
		commandProcessor.DoCommand("controlKeyUp", new string[0]);
	}

	public void KeyDown(string locator, string keySequence)
	{
		commandProcessor.DoCommand("keyDown", new string[2] { locator, keySequence });
	}

	public void KeyUp(string locator, string keySequence)
	{
		commandProcessor.DoCommand("keyUp", new string[2] { locator, keySequence });
	}

	public void MouseOver(string locator)
	{
		commandProcessor.DoCommand("mouseOver", new string[1] { locator });
	}

	public void MouseOut(string locator)
	{
		commandProcessor.DoCommand("mouseOut", new string[1] { locator });
	}

	public void MouseDown(string locator)
	{
		commandProcessor.DoCommand("mouseDown", new string[1] { locator });
	}

	public void MouseDownRight(string locator)
	{
		commandProcessor.DoCommand("mouseDownRight", new string[1] { locator });
	}

	public void MouseDownAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("mouseDownAt", new string[2] { locator, coordString });
	}

	public void MouseDownRightAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("mouseDownRightAt", new string[2] { locator, coordString });
	}

	public void MouseUp(string locator)
	{
		commandProcessor.DoCommand("mouseUp", new string[1] { locator });
	}

	public void MouseUpRight(string locator)
	{
		commandProcessor.DoCommand("mouseUpRight", new string[1] { locator });
	}

	public void MouseUpAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("mouseUpAt", new string[2] { locator, coordString });
	}

	public void MouseUpRightAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("mouseUpRightAt", new string[2] { locator, coordString });
	}

	public void MouseMove(string locator)
	{
		commandProcessor.DoCommand("mouseMove", new string[1] { locator });
	}

	public void MouseMoveAt(string locator, string coordString)
	{
		commandProcessor.DoCommand("mouseMoveAt", new string[2] { locator, coordString });
	}

	public void Type(string locator, string value)
	{
		commandProcessor.DoCommand("type", new string[2] { locator, value });
	}

	public void TypeKeys(string locator, string value)
	{
		commandProcessor.DoCommand("typeKeys", new string[2] { locator, value });
	}

	public void SetSpeed(string value)
	{
		commandProcessor.DoCommand("setSpeed", new string[1] { value });
	}

	public string GetSpeed()
	{
		return commandProcessor.GetString("getSpeed", new string[0]);
	}

	public void Check(string locator)
	{
		commandProcessor.DoCommand("check", new string[1] { locator });
	}

	public void Uncheck(string locator)
	{
		commandProcessor.DoCommand("uncheck", new string[1] { locator });
	}

	public void Select(string selectLocator, string optionLocator)
	{
		commandProcessor.DoCommand("select", new string[2] { selectLocator, optionLocator });
	}

	public void AddSelection(string locator, string optionLocator)
	{
		commandProcessor.DoCommand("addSelection", new string[2] { locator, optionLocator });
	}

	public void RemoveSelection(string locator, string optionLocator)
	{
		commandProcessor.DoCommand("removeSelection", new string[2] { locator, optionLocator });
	}

	public void RemoveAllSelections(string locator)
	{
		commandProcessor.DoCommand("removeAllSelections", new string[1] { locator });
	}

	public void Submit(string formLocator)
	{
		commandProcessor.DoCommand("submit", new string[1] { formLocator });
	}

	public void Open(string url)
	{
		commandProcessor.DoCommand("open", new string[1] { url });
	}

	public void OpenWindow(string url, string windowID)
	{
		commandProcessor.DoCommand("openWindow", new string[2] { url, windowID });
	}

	public void SelectWindow(string windowID)
	{
		commandProcessor.DoCommand("selectWindow", new string[1] { windowID });
	}

	public void SelectPopUp(string windowID)
	{
		commandProcessor.DoCommand("selectPopUp", new string[1] { windowID });
	}

	public void DeselectPopUp()
	{
		commandProcessor.DoCommand("deselectPopUp", new string[0]);
	}

	public void SelectFrame(string locator)
	{
		commandProcessor.DoCommand("selectFrame", new string[1] { locator });
	}

	public bool GetWhetherThisFrameMatchFrameExpression(string currentFrameString, string target)
	{
		return commandProcessor.GetBoolean("getWhetherThisFrameMatchFrameExpression", new string[2] { currentFrameString, target });
	}

	public bool GetWhetherThisWindowMatchWindowExpression(string currentWindowString, string target)
	{
		return commandProcessor.GetBoolean("getWhetherThisWindowMatchWindowExpression", new string[2] { currentWindowString, target });
	}

	public void WaitForPopUp(string windowID, string timeout)
	{
		commandProcessor.DoCommand("waitForPopUp", new string[2] { windowID, timeout });
	}

	public void ChooseCancelOnNextConfirmation()
	{
		commandProcessor.DoCommand("chooseCancelOnNextConfirmation", new string[0]);
	}

	public void ChooseOkOnNextConfirmation()
	{
		commandProcessor.DoCommand("chooseOkOnNextConfirmation", new string[0]);
	}

	public void AnswerOnNextPrompt(string answer)
	{
		commandProcessor.DoCommand("answerOnNextPrompt", new string[1] { answer });
	}

	public void GoBack()
	{
		commandProcessor.DoCommand("goBack", new string[0]);
	}

	public void Refresh()
	{
		commandProcessor.DoCommand("refresh", new string[0]);
	}

	public void Close()
	{
		commandProcessor.DoCommand("close", new string[0]);
	}

	public bool IsAlertPresent()
	{
		return commandProcessor.GetBoolean("isAlertPresent", new string[0]);
	}

	public bool IsPromptPresent()
	{
		return commandProcessor.GetBoolean("isPromptPresent", new string[0]);
	}

	public bool IsConfirmationPresent()
	{
		return commandProcessor.GetBoolean("isConfirmationPresent", new string[0]);
	}

	public string GetAlert()
	{
		return commandProcessor.GetString("getAlert", new string[0]);
	}

	public string GetConfirmation()
	{
		return commandProcessor.GetString("getConfirmation", new string[0]);
	}

	public string GetPrompt()
	{
		return commandProcessor.GetString("getPrompt", new string[0]);
	}

	public string GetLocation()
	{
		return commandProcessor.GetString("getLocation", new string[0]);
	}

	public string GetTitle()
	{
		return commandProcessor.GetString("getTitle", new string[0]);
	}

	public string GetBodyText()
	{
		return commandProcessor.GetString("getBodyText", new string[0]);
	}

	public string GetValue(string locator)
	{
		return commandProcessor.GetString("getValue", new string[1] { locator });
	}

	public string GetText(string locator)
	{
		return commandProcessor.GetString("getText", new string[1] { locator });
	}

	public void Highlight(string locator)
	{
		commandProcessor.DoCommand("highlight", new string[1] { locator });
	}

	public string GetEval(string script)
	{
		return commandProcessor.GetString("getEval", new string[1] { script });
	}

	public bool IsChecked(string locator)
	{
		return commandProcessor.GetBoolean("isChecked", new string[1] { locator });
	}

	public string GetTable(string tableCellAddress)
	{
		return commandProcessor.GetString("getTable", new string[1] { tableCellAddress });
	}

	public string[] GetSelectedLabels(string selectLocator)
	{
		return commandProcessor.GetStringArray("getSelectedLabels", new string[1] { selectLocator });
	}

	public string GetSelectedLabel(string selectLocator)
	{
		return commandProcessor.GetString("getSelectedLabel", new string[1] { selectLocator });
	}

	public string[] GetSelectedValues(string selectLocator)
	{
		return commandProcessor.GetStringArray("getSelectedValues", new string[1] { selectLocator });
	}

	public string GetSelectedValue(string selectLocator)
	{
		return commandProcessor.GetString("getSelectedValue", new string[1] { selectLocator });
	}

	public string[] GetSelectedIndexes(string selectLocator)
	{
		return commandProcessor.GetStringArray("getSelectedIndexes", new string[1] { selectLocator });
	}

	public string GetSelectedIndex(string selectLocator)
	{
		return commandProcessor.GetString("getSelectedIndex", new string[1] { selectLocator });
	}

	public string[] GetSelectedIds(string selectLocator)
	{
		return commandProcessor.GetStringArray("getSelectedIds", new string[1] { selectLocator });
	}

	public string GetSelectedId(string selectLocator)
	{
		return commandProcessor.GetString("getSelectedId", new string[1] { selectLocator });
	}

	public bool IsSomethingSelected(string selectLocator)
	{
		return commandProcessor.GetBoolean("isSomethingSelected", new string[1] { selectLocator });
	}

	public string[] GetSelectOptions(string selectLocator)
	{
		return commandProcessor.GetStringArray("getSelectOptions", new string[1] { selectLocator });
	}

	public string GetAttribute(string attributeLocator)
	{
		return commandProcessor.GetString("getAttribute", new string[1] { attributeLocator });
	}

	public bool IsTextPresent(string pattern)
	{
		return commandProcessor.GetBoolean("isTextPresent", new string[1] { pattern });
	}

	public bool IsElementPresent(string locator)
	{
		return commandProcessor.GetBoolean("isElementPresent", new string[1] { locator });
	}

	public bool IsVisible(string locator)
	{
		return commandProcessor.GetBoolean("isVisible", new string[1] { locator });
	}

	public bool IsEditable(string locator)
	{
		return commandProcessor.GetBoolean("isEditable", new string[1] { locator });
	}

	public string[] GetAllButtons()
	{
		return commandProcessor.GetStringArray("getAllButtons", new string[0]);
	}

	public string[] GetAllLinks()
	{
		return commandProcessor.GetStringArray("getAllLinks", new string[0]);
	}

	public string[] GetAllFields()
	{
		return commandProcessor.GetStringArray("getAllFields", new string[0]);
	}

	public string[] GetAttributeFromAllWindows(string attributeName)
	{
		return commandProcessor.GetStringArray("getAttributeFromAllWindows", new string[1] { attributeName });
	}

	public void Dragdrop(string locator, string movementsString)
	{
		commandProcessor.DoCommand("dragdrop", new string[2] { locator, movementsString });
	}

	public void SetMouseSpeed(string pixels)
	{
		commandProcessor.DoCommand("setMouseSpeed", new string[1] { pixels });
	}

	public decimal GetMouseSpeed()
	{
		return commandProcessor.GetNumber("getMouseSpeed", new string[0]);
	}

	public void DragAndDrop(string locator, string movementsString)
	{
		commandProcessor.DoCommand("dragAndDrop", new string[2] { locator, movementsString });
	}

	public void DragAndDropToObject(string locatorOfObjectToBeDragged, string locatorOfDragDestinationObject)
	{
		commandProcessor.DoCommand("dragAndDropToObject", new string[2] { locatorOfObjectToBeDragged, locatorOfDragDestinationObject });
	}

	public void WindowFocus()
	{
		commandProcessor.DoCommand("windowFocus", new string[0]);
	}

	public void WindowMaximize()
	{
		commandProcessor.DoCommand("windowMaximize", new string[0]);
	}

	public string[] GetAllWindowIds()
	{
		return commandProcessor.GetStringArray("getAllWindowIds", new string[0]);
	}

	public string[] GetAllWindowNames()
	{
		return commandProcessor.GetStringArray("getAllWindowNames", new string[0]);
	}

	public string[] GetAllWindowTitles()
	{
		return commandProcessor.GetStringArray("getAllWindowTitles", new string[0]);
	}

	public string GetHtmlSource()
	{
		return commandProcessor.GetString("getHtmlSource", new string[0]);
	}

	public void SetCursorPosition(string locator, string position)
	{
		commandProcessor.DoCommand("setCursorPosition", new string[2] { locator, position });
	}

	public decimal GetElementIndex(string locator)
	{
		return commandProcessor.GetNumber("getElementIndex", new string[1] { locator });
	}

	public bool IsOrdered(string locator1, string locator2)
	{
		return commandProcessor.GetBoolean("isOrdered", new string[2] { locator1, locator2 });
	}

	public decimal GetElementPositionLeft(string locator)
	{
		return commandProcessor.GetNumber("getElementPositionLeft", new string[1] { locator });
	}

	public decimal GetElementPositionTop(string locator)
	{
		return commandProcessor.GetNumber("getElementPositionTop", new string[1] { locator });
	}

	public decimal GetElementWidth(string locator)
	{
		return commandProcessor.GetNumber("getElementWidth", new string[1] { locator });
	}

	public decimal GetElementHeight(string locator)
	{
		return commandProcessor.GetNumber("getElementHeight", new string[1] { locator });
	}

	public decimal GetCursorPosition(string locator)
	{
		return commandProcessor.GetNumber("getCursorPosition", new string[1] { locator });
	}

	public string GetExpression(string expression)
	{
		return commandProcessor.GetString("getExpression", new string[1] { expression });
	}

	public decimal GetXpathCount(string xpath)
	{
		return commandProcessor.GetNumber("getXpathCount", new string[1] { xpath });
	}

	public decimal GetCSSCount(string cssLocator)
	{
		return commandProcessor.GetNumber("getCssCount", new string[1] { cssLocator });
	}

	public void AssignId(string locator, string identifier)
	{
		commandProcessor.DoCommand("assignId", new string[2] { locator, identifier });
	}

	public void AllowNativeXpath(string allow)
	{
		commandProcessor.DoCommand("allowNativeXpath", new string[1] { allow });
	}

	public void IgnoreAttributesWithoutValue(string ignore)
	{
		commandProcessor.DoCommand("ignoreAttributesWithoutValue", new string[1] { ignore });
	}

	public void WaitForCondition(string script, string timeout)
	{
		commandProcessor.DoCommand("waitForCondition", new string[2] { script, timeout });
	}

	public void SetTimeout(string timeout)
	{
		commandProcessor.DoCommand("setTimeout", new string[1] { timeout });
	}

	public void WaitForPageToLoad(string timeout)
	{
		commandProcessor.DoCommand("waitForPageToLoad", new string[1] { timeout });
	}

	public void WaitForFrameToLoad(string frameAddress, string timeout)
	{
		commandProcessor.DoCommand("waitForFrameToLoad", new string[2] { frameAddress, timeout });
	}

	public string GetCookie()
	{
		return commandProcessor.GetString("getCookie", new string[0]);
	}

	public string GetCookieByName(string name)
	{
		return commandProcessor.GetString("getCookieByName", new string[1] { name });
	}

	public bool IsCookiePresent(string name)
	{
		return commandProcessor.GetBoolean("isCookiePresent", new string[1] { name });
	}

	public void CreateCookie(string nameValuePair, string optionsString)
	{
		commandProcessor.DoCommand("createCookie", new string[2] { nameValuePair, optionsString });
	}

	public void DeleteCookie(string name, string optionsString)
	{
		commandProcessor.DoCommand("deleteCookie", new string[2] { name, optionsString });
	}

	public void DeleteAllVisibleCookies()
	{
		commandProcessor.DoCommand("deleteAllVisibleCookies", new string[0]);
	}

	public void SetBrowserLogLevel(string logLevel)
	{
		commandProcessor.DoCommand("setBrowserLogLevel", new string[1] { logLevel });
	}

	public void RunScript(string script)
	{
		commandProcessor.DoCommand("runScript", new string[1] { script });
	}

	public void AddLocationStrategy(string strategyName, string functionDefinition)
	{
		commandProcessor.DoCommand("addLocationStrategy", new string[2] { strategyName, functionDefinition });
	}

	public void CaptureEntirePageScreenshot(string filename, string kwargs)
	{
		commandProcessor.DoCommand("captureEntirePageScreenshot", new string[2] { filename, kwargs });
	}

	public void Rollup(string rollupName, string kwargs)
	{
		commandProcessor.DoCommand("rollup", new string[2] { rollupName, kwargs });
	}

	public void AddScript(string scriptContent, string scriptTagId)
	{
		commandProcessor.DoCommand("addScript", new string[2] { scriptContent, scriptTagId });
	}

	public void RemoveScript(string scriptTagId)
	{
		commandProcessor.DoCommand("removeScript", new string[1] { scriptTagId });
	}

	public void UseXpathLibrary(string libraryName)
	{
		commandProcessor.DoCommand("useXpathLibrary", new string[1] { libraryName });
	}

	public void SetContext(string context)
	{
		commandProcessor.DoCommand("setContext", new string[1] { context });
	}

	public void AttachFile(string fieldLocator, string fileLocator)
	{
		commandProcessor.DoCommand("attachFile", new string[2] { fieldLocator, fileLocator });
	}

	public void CaptureScreenshot(string filename)
	{
		commandProcessor.DoCommand("captureScreenshot", new string[1] { filename });
	}

	public string CaptureScreenshotToString()
	{
		return commandProcessor.GetString("captureScreenshotToString", new string[0]);
	}

	public string CaptureNetworkTraffic(string type)
	{
		return commandProcessor.GetString("captureNetworkTraffic", new string[1] { type });
	}

	public string CaptureEntirePageScreenshotToString(string kwargs)
	{
		return commandProcessor.GetString("captureEntirePageScreenshotToString", new string[1] { kwargs });
	}

	public void ShutDownSeleniumServer()
	{
		commandProcessor.DoCommand("shutDownSeleniumServer", new string[0]);
	}

	public string RetrieveLastRemoteControlLogs()
	{
		return commandProcessor.GetString("retrieveLastRemoteControlLogs", new string[0]);
	}

	public void KeyDownNative(string keycode)
	{
		commandProcessor.DoCommand("keyDownNative", new string[1] { keycode });
	}

	public void KeyUpNative(string keycode)
	{
		commandProcessor.DoCommand("keyUpNative", new string[1] { keycode });
	}

	public void KeyPressNative(string keycode)
	{
		commandProcessor.DoCommand("keyPressNative", new string[1] { keycode });
	}
}
public interface ICommandProcessor
{
	string DoCommand(string command, string[] args);

	void SetExtensionJs(string extensionJs);

	void Start();

	void Start(string optionsString);

	void Start(object optionsObject);

	void Stop();

	string GetString(string command, string[] args);

	string[] GetStringArray(string command, string[] args);

	decimal GetNumber(string command, string[] args);

	decimal[] GetNumberArray(string command, string[] args);

	bool GetBoolean(string command, string[] args);

	bool[] GetBooleanArray(string command, string[] args);
}
public class HttpCommandProcessor : ICommandProcessor
{
	private readonly string url;

	private string sessionId;

	private string browserStartCommand;

	private string browserURL;

	private string extensionJs;

	public string Url => url;

	public HttpCommandProcessor(string serverHost, int serverPort, string browserStartCommand, string browserURL)
	{
		url = "http://" + serverHost + ":" + serverPort + "/selenium-server/driver/";
		this.browserStartCommand = browserStartCommand;
		this.browserURL = browserURL;
		extensionJs = "";
	}

	public HttpCommandProcessor(string serverURL, string browserStartCommand, string browserURL)
	{
		url = serverURL;
		this.browserStartCommand = browserStartCommand;
		this.browserURL = browserURL;
		extensionJs = "";
	}

	public string DoCommand(string command, string[] args)
	{
		IRemoteCommand command2 = new DefaultRemoteCommand(command, args);
		using HttpWebResponse httpWebResponse = (HttpWebResponse)CreateWebRequest(command2).GetResponse();
		if (httpWebResponse.StatusCode != HttpStatusCode.OK)
		{
			throw new SeleniumException(httpWebResponse.StatusDescription);
		}
		string text = ReadResponse(httpWebResponse);
		if (!text.StartsWith("OK"))
		{
			throw new SeleniumException(text);
		}
		return text;
	}

	public virtual string ReadResponse(HttpWebResponse response)
	{
		using StreamReader streamReader = new StreamReader(response.GetResponseStream());
		return streamReader.ReadToEnd();
	}

	public virtual WebRequest CreateWebRequest(IRemoteCommand command)
	{
		byte[] array = BuildCommandPostData(command.CommandString);
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
		httpWebRequest.Method = "POST";
		httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
		httpWebRequest.Timeout = -1;
		httpWebRequest.ServicePoint.ConnectionLimit = 2000;
		Stream requestStream = httpWebRequest.GetRequestStream();
		requestStream.Write(array, 0, array.Length);
		requestStream.Close();
		return httpWebRequest;
	}

	private byte[] BuildCommandPostData(string commandString)
	{
		string text = commandString;
		if (sessionId != null)
		{
			text = text + "&sessionId=" + sessionId;
		}
		return new UTF8Encoding().GetBytes(text);
	}

	public void SetExtensionJs(string extensionJs)
	{
		this.extensionJs = extensionJs;
	}

	public void Start()
	{
		string @string = GetString("getNewBrowserSession", new string[3] { browserStartCommand, browserURL, extensionJs });
		sessionId = @string;
	}

	public void Start(string optionString)
	{
		string @string = GetString("getNewBrowserSession", new string[4] { browserStartCommand, browserURL, extensionJs, optionString });
		sessionId = @string;
	}

	public void Start(object optionsObject)
	{
		Start(optionsObject.ToString());
	}

	public void Stop()
	{
		DoCommand("testComplete", null);
		sessionId = null;
	}

	public string GetString(string commandName, string[] args)
	{
		return DoCommand(commandName, args).Substring(3);
	}

	public string[] GetStringArray(string commandName, string[] args)
	{
		string @string = GetString(commandName, args);
		return parseCSV(@string);
	}

	public static string[] parseCSV(string input)
	{
		ArrayList arrayList = new ArrayList();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < input.Length; i++)
		{
			char c = input.ToCharArray()[i];
			switch (c)
			{
			case ',':
				arrayList.Add(stringBuilder.ToString());
				stringBuilder = new StringBuilder();
				break;
			case '\\':
				i++;
				c = input.ToCharArray()[i];
				stringBuilder.Append(c);
				break;
			default:
				stringBuilder.Append(c);
				break;
			}
		}
		arrayList.Add(stringBuilder.ToString());
		return (string[])arrayList.ToArray(typeof(string));
	}

	public decimal GetNumber(string commandName, string[] args)
	{
		string @string = GetString(commandName, args);
		return decimal.Parse(@string);
	}

	public decimal[] GetNumberArray(string commandName, string[] args)
	{
		string[] stringArray = GetStringArray(commandName, args);
		decimal[] array = new decimal[stringArray.Length];
		for (int i = 0; i < stringArray.Length; i++)
		{
			ref decimal reference = ref array[i];
			reference = decimal.Parse(stringArray[i]);
		}
		return array;
	}

	public bool GetBoolean(string commandName, string[] args)
	{
		string @string = GetString(commandName, args);
		if ("true".Equals(@string))
		{
			return true;
		}
		if ("false".Equals(@string))
		{
			return false;
		}
		throw new Exception("result was neither 'true' nor 'false': " + @string);
	}

	public bool[] GetBooleanArray(string commandName, string[] args)
	{
		string[] stringArray = GetStringArray(commandName, args);
		bool[] array = new bool[stringArray.Length];
		for (int i = 0; i < stringArray.Length; i++)
		{
			if ("true".Equals(stringArray))
			{
				array[i] = true;
				continue;
			}
			if ("false".Equals(stringArray))
			{
				array[i] = false;
				continue;
			}
			throw new Exception("result was neither 'true' nor 'false': " + stringArray);
		}
		return array;
	}
}
public class SeleniumException : Exception
{
	public SeleniumException()
	{
	}

	public SeleniumException(string message)
		: base(message)
	{
	}

	public SeleniumException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}

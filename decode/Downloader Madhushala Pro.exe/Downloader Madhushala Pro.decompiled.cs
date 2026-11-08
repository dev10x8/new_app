using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;

[assembly: AssemblyProduct("Madhushala Pro")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyFileVersion("1.0.0.4")]
[assembly: AssemblyTitle("Downloader Madhushala Pro")]
[assembly: AssemblyVersion("1.0.0.4")]
namespace My;

[GeneratedCode("MyTemplate", "14.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class MyApplication : ApplicationBase
{
}
[EditorBrowsable(EditorBrowsableState.Never)]
[GeneratedCode("MyTemplate", "14.0.0.0")]
internal class MyComputer : Computer
{
	[DebuggerHidden]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public MyComputer()
	{
	}
}
[GeneratedCode("MyTemplate", "14.0.0.0")]
[HideModuleName]
[StandardModule]
internal sealed class MyProject
{
	[MyGroupCollection("System.Web.Services.Protocols.SoapHttpClientProtocol", "Create__Instance__", "Dispose__Instance__", "")]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
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

		[EditorBrowsable(EditorBrowsableState.Never)]
		[DebuggerHidden]
		public ThreadSafeObjectProvider()
		{
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
[StandardModule]
internal sealed class DownloaderProgram
{
	[STAThread]
	public static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run((Form)(object)new DownloaderForm());
	}
}
public class DownloaderForm : Form
{
	private const string BaseUrl = "https://fileuploader.madhushalasoftware.com/PRO/";

	private const string RegKey = "HKEY_CURRENT_USER\\Software\\MadhushalaUltimate";

	private readonly string[][] _files;

	private const int StallSeconds = 60;

	private string _folder;

	private int _index;

	private WebClient _client;

	private bool _busy;

	private bool _cancelled;

	private bool _stalled;

	private const int MaxRetries = 2;

	private int _retries;

	private bool _finished;

	private DateTime _lastProgress;

	private readonly List<string> _done;

	private readonly List<string> _failed;

	[AccessedThroughProperty("_watchdog")]
	private Timer __watchdog;

	private Label lblTitle;

	private Label lblWait;

	private Label lblFile;

	private Label lblPercent;

	private Label lblSize;

	private ProgressBar bar;

	[AccessedThroughProperty("btnCancel")]
	private Button _btnCancel;

	private bool _updateMode;

	private virtual Timer _watchdog
	{
		get
		{
			return __watchdog;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			EventHandler eventHandler = _watchdog_Tick;
			if (__watchdog != null)
			{
				__watchdog.Tick -= eventHandler;
			}
			__watchdog = value;
			if (__watchdog != null)
			{
				__watchdog.Tick += eventHandler;
			}
		}
	}

	private virtual Button btnCancel
	{
		get
		{
			return _btnCancel;
		}
		[MethodImpl(MethodImplOptions.Synchronized)]
		set
		{
			EventHandler eventHandler = btnCancel_Click;
			if (_btnCancel != null)
			{
				((Control)_btnCancel).Click -= eventHandler;
			}
			_btnCancel = value;
			if (_btnCancel != null)
			{
				((Control)_btnCancel).Click += eventHandler;
			}
		}
	}

	public DownloaderForm()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Expected O, but got Unknown
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		_files = new string[2][]
		{
			new string[2] { "Madhushala Pro.exe", "LastExeUpdateDate" },
			new string[2] { "M_B.bmp", "LastImageUpdateDate" }
		};
		_index = 0;
		_busy = false;
		_cancelled = false;
		_stalled = false;
		_retries = 0;
		_finished = false;
		_done = new List<string>();
		_failed = new List<string>();
		_watchdog = new Timer();
		_updateMode = false;
		_folder = AppDomain.CurrentDomain.BaseDirectory;
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		foreach (string a in commandLineArgs)
		{
			if (string.Equals(a, "/update", StringComparison.OrdinalIgnoreCase))
			{
				_updateMode = true;
			}
		}
		((Form)this).Text = "Downloader Madhushala Pro";
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		Size clientSize = new Size(480, 236);
		((Form)this).ClientSize = clientSize;
		((Form)this).BackColor = Color.White;
		((Control)this).Font = new Font("Segoe UI", 9f);
		try
		{
			((Form)this).Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
		Panel val = new Panel();
		((Control)val).Dock = (DockStyle)1;
		((Control)val).Height = 56;
		((Control)val).BackColor = Color.FromArgb(30, 30, 30);
		((Control)this).Controls.Add((Control)(object)val);
		lblTitle = new Label();
		lblTitle.Text = "Downloader Madhushala Pro";
		((Control)lblTitle).ForeColor = Color.FromArgb(255, 213, 79);
		((Control)lblTitle).Font = new Font("Segoe UI", 13f, (FontStyle)1);
		lblTitle.AutoSize = true;
		Label obj = lblTitle;
		Point location = new Point(18, 14);
		((Control)obj).Location = location;
		((Control)val).Controls.Add((Control)(object)lblTitle);
		lblWait = new Label();
		lblWait.Text = "Please wait while the latest files are being downloaded...";
		((Control)lblWait).ForeColor = Color.FromArgb(80, 80, 80);
		lblWait.AutoSize = true;
		Label obj2 = lblWait;
		location = new Point(20, 70);
		((Control)obj2).Location = location;
		((Control)this).Controls.Add((Control)(object)lblWait);
		lblFile = new Label();
		lblFile.Text = "Preparing download...";
		((Control)lblFile).Font = new Font("Segoe UI", 10f, (FontStyle)1);
		((Control)lblFile).ForeColor = Color.FromArgb(30, 30, 30);
		lblFile.AutoSize = false;
		Label obj3 = lblFile;
		clientSize = new Size(440, 22);
		((Control)obj3).Size = clientSize;
		Label obj4 = lblFile;
		location = new Point(20, 94);
		((Control)obj4).Location = location;
		((Control)this).Controls.Add((Control)(object)lblFile);
		bar = new ProgressBar();
		ProgressBar obj5 = bar;
		location = new Point(20, 122);
		((Control)obj5).Location = location;
		ProgressBar obj6 = bar;
		clientSize = new Size(440, 24);
		((Control)obj6).Size = clientSize;
		bar.Minimum = 0;
		bar.Maximum = 100;
		bar.Style = (ProgressBarStyle)1;
		((Control)this).Controls.Add((Control)(object)bar);
		lblSize = new Label();
		lblSize.Text = "";
		((Control)lblSize).ForeColor = Color.FromArgb(90, 90, 90);
		lblSize.AutoSize = false;
		Label obj7 = lblSize;
		clientSize = new Size(300, 20);
		((Control)obj7).Size = clientSize;
		Label obj8 = lblSize;
		location = new Point(20, 152);
		((Control)obj8).Location = location;
		((Control)this).Controls.Add((Control)(object)lblSize);
		lblPercent = new Label();
		lblPercent.Text = "0 %";
		((Control)lblPercent).Font = new Font("Segoe UI", 10f, (FontStyle)1);
		lblPercent.TextAlign = (ContentAlignment)64;
		lblPercent.AutoSize = false;
		Label obj9 = lblPercent;
		clientSize = new Size(120, 20);
		((Control)obj9).Size = clientSize;
		Label obj10 = lblPercent;
		location = new Point(340, 152);
		((Control)obj10).Location = location;
		((Control)this).Controls.Add((Control)(object)lblPercent);
		btnCancel = new Button();
		((ButtonBase)btnCancel).Text = "Cancel";
		Button obj11 = btnCancel;
		clientSize = new Size(100, 30);
		((Control)obj11).Size = clientSize;
		Button obj12 = btnCancel;
		location = new Point(360, 190);
		((Control)obj12).Location = location;
		((Control)this).Controls.Add((Control)(object)btnCancel);
		_watchdog.Interval = 1000;
	}

	protected override void OnShown(EventArgs e)
	{
		((Form)this).OnShown(e);
		StartAll();
	}

	private void StartAll()
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Invalid comparison between Unknown and I4
		try
		{
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
		if (_updateMode)
		{
			lblFile.Text = "Closing the old Madhushala Pro...";
			DateTime now = DateTime.Now;
			while (IsMainAppRunning() && (DateTime.Now - now).TotalSeconds < 60.0)
			{
				Application.DoEvents();
				Thread.Sleep(250);
			}
		}
		while (IsMainAppRunning())
		{
			DialogResult val = MessageBox.Show((IWin32Window)(object)this, "Madhushala Pro is currently running.\r\n\r\nPlease close Madhushala Pro and click Retry to continue the download.", ((Form)this).Text, (MessageBoxButtons)5, (MessageBoxIcon)48);
			if ((int)val != 4)
			{
				_cancelled = true;
				ShowResultAndClose();
				return;
			}
		}
		_index = 0;
		StartNext();
	}

	private bool IsMainAppRunning()
	{
		Process[] processesByName = Process.GetProcessesByName("Madhushala Pro");
		foreach (Process process in processesByName)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(process.MainModule.FileName);
				if (string.Equals(directoryName.TrimEnd(new char[1] { '\\' }), _folder.TrimEnd(new char[1] { '\\' }), StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}
		return false;
	}

	private void StartNext()
	{
		if (_index >= _files.Length)
		{
			ShowResultAndClose();
			return;
		}
		string text = _files[_index][0];
		checked
		{
			lblFile.Text = "Downloading " + text + "   (" + (_index + 1) + " of " + _files.Length + ")";
			lblSize.Text = "Connecting to the update server...";
			string text2 = Path.Combine(_folder, text + ".download");
			try
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			_stalled = false;
			_client = new WebClient();
			_client.DownloadProgressChanged += OnProgress;
			_client.DownloadFileCompleted += OnCompleted;
			_lastProgress = DateTime.Now;
			_busy = true;
			_watchdog.Start();
			try
			{
				_client.DownloadFileAsync(new Uri("https://fileuploader.madhushalasoftware.com/PRO/" + Uri.EscapeDataString(text)), text2);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				_watchdog.Stop();
				_busy = false;
				_failed.Add(text + " - " + Describe(ex2));
				_index++;
				StartNext();
				ProjectData.ClearProjectError();
			}
		}
	}

	private void OnProgress(object sender, DownloadProgressChangedEventArgs e)
	{
		_lastProgress = DateTime.Now;
		int num = ((e.TotalBytesToReceive > 0) ? e.ProgressPercentage : 0);
		int num2 = checked((int)Math.Round((double)(_index * 100 + num) / (double)_files.Length));
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 100)
		{
			num2 = 100;
		}
		bar.Value = num2;
		lblPercent.Text = num2 + " %";
		lblSize.Text = FormatSize(e.BytesReceived) + ((e.TotalBytesToReceive > 0) ? (" of " + FormatSize(e.TotalBytesToReceive)) : "") + " downloaded";
	}

	private void OnCompleted(object sender, AsyncCompletedEventArgs e)
	{
		_watchdog.Stop();
		_busy = false;
		string text = _files[_index][0];
		string valueName = _files[_index][1];
		string text2 = Path.Combine(_folder, text + ".download");
		string text3 = Path.Combine(_folder, text);
		string text4 = "";
		try
		{
			if (_client.ResponseHeaders != null)
			{
				text4 = _client.ResponseHeaders["Last-Modified"];
			}
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
		try
		{
			_client.Dispose();
		}
		catch (Exception projectError2)
		{
			ProjectData.SetProjectError(projectError2);
			ProjectData.ClearProjectError();
		}
		if (_cancelled)
		{
			SafeDelete(text2);
			ShowResultAndClose();
			return;
		}
		checked
		{
			if (e.Cancelled)
			{
				SafeDelete(text2);
				if (_stalled)
				{
					_failed.Add(text + " - The download stopped responding (no data received for " + 60 + " seconds). Please check the internet connection.");
				}
				else
				{
					_failed.Add(text + " - The download was cancelled.");
				}
			}
			else if (e.Error != null)
			{
				SafeDelete(text2);
				if (_retries < 2)
				{
					_retries++;
					lblSize.Text = "Connection lost - retrying (" + _retries + " of " + 2 + ")...";
					StartNext();
					return;
				}
				_failed.Add(text + " - " + Describe(e.Error));
			}
			else
			{
				try
				{
					long length = new FileInfo(text2).Length;
					if (length == 0)
					{
						throw new IOException("The update server returned an empty file.");
					}
					string text5 = text3 + ".bak";
					if (File.Exists(text3))
					{
						if (File.Exists(text5))
						{
							File.Delete(text5);
						}
						File.Move(text3, text5);
					}
					try
					{
						File.Move(text2, text3);
					}
					catch (Exception projectError3)
					{
						ProjectData.SetProjectError(projectError3);
						if (!File.Exists(text3) && File.Exists(text5))
						{
							File.Move(text5, text3);
						}
						throw;
					}
					try
					{
						if (Operators.CompareString(text4, "", false) != 0 && DateTime.TryParse(text4, out var result))
						{
							Registry.SetValue("HKEY_CURRENT_USER\\Software\\MadhushalaUltimate", valueName, result.ToString("o"));
						}
					}
					catch (Exception projectError4)
					{
						ProjectData.SetProjectError(projectError4);
						ProjectData.ClearProjectError();
					}
					_done.Add(text + "   (" + FormatSize(length) + ")");
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					SafeDelete(text2);
					_failed.Add(text + " - " + Describe(ex2));
					ProjectData.ClearProjectError();
				}
			}
			_index++;
			_retries = 0;
			StartNext();
		}
	}

	private void _watchdog_Tick(object sender, EventArgs e)
	{
		if (_busy && (DateTime.Now - _lastProgress).TotalSeconds > 60.0)
		{
			_stalled = true;
			try
			{
				_client.CancelAsync();
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		if (_busy)
		{
			_cancelled = true;
			((Control)btnCancel).Enabled = false;
			lblFile.Text = "Cancelling...";
			try
			{
				_client.CancelAsync();
				return;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
				return;
			}
		}
		((Form)this).Close();
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		if (_busy && !_finished)
		{
			((CancelEventArgs)(object)e).Cancel = true;
			_cancelled = true;
			try
			{
				_client.CancelAsync();
				return;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
				return;
			}
		}
		((Form)this).OnFormClosing(e);
	}

	private void ShowResultAndClose()
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		if (_finished)
		{
			return;
		}
		_finished = true;
		_watchdog.Stop();
		bar.Value = ((_failed.Count == 0 && !_cancelled) ? 100 : bar.Value);
		lblPercent.Text = bar.Value + " %";
		((Control)btnCancel).Enabled = false;
		if (_updateMode && _failed.Count == 0 && !_cancelled)
		{
			lblFile.Text = "Update complete - starting Madhushala Pro...";
			((Control)lblFile).Refresh();
			try
			{
				Process.Start(Path.Combine(_folder, "Madhushala Pro.exe"));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				MessageBox.Show((IWin32Window)(object)this, "Madhushala Pro has been updated, but it could not be started automatically.\r\n\r\nPlease start Madhushala Pro from the desktop.", "Download Complete", (MessageBoxButtons)0, (MessageBoxIcon)64);
				ProjectData.ClearProjectError();
			}
			((Form)this).Close();
			return;
		}
		string text;
		MessageBoxIcon val;
		string text2;
		if (_cancelled && _done.Count == 0)
		{
			text = "Download Cancelled";
			text2 = "The download was cancelled.\r\n\r\nYour existing Madhushala Pro files have not been changed.";
			val = (MessageBoxIcon)64;
		}
		else if (_failed.Count == 0 && !_cancelled)
		{
			text = "Download Complete";
			text2 = "Madhushala Pro has been updated successfully.\r\n\r\nDownloaded files:\r\n" + ListLines(_done) + "\r\nLocation: " + _folder + "\r\n\r\nYou can now start Madhushala Pro.";
			val = (MessageBoxIcon)64;
		}
		else
		{
			text = ((_done.Count > 0) ? "Download Partly Completed" : "Download Failed");
			text2 = "";
			if (_done.Count > 0)
			{
				text2 = text2 + "Downloaded files:\r\n" + ListLines(_done) + "\r\n";
			}
			if (_failed.Count > 0)
			{
				text2 = text2 + "Not downloaded:\r\n" + ListLines(_failed) + "\r\n";
			}
			if (_cancelled)
			{
				text2 += "The download was cancelled.\r\n\r\n";
			}
			text2 += "Files that were not downloaded have been left unchanged.\r\nPlease try again later. If the problem continues, contact your software vendor.";
			val = (MessageBoxIcon)((_done.Count > 0) ? 48 : 16);
		}
		MessageBox.Show((IWin32Window)(object)this, text2, text, (MessageBoxButtons)0, val);
		((Form)this).Close();
	}

	private static string ListLines(List<string> items)
	{
		string text = "";
		foreach (string item in items)
		{
			text = text + "   - " + item + "\r\n";
		}
		return text;
	}

	private static string Describe(Exception ex)
	{
		if (ex is WebException ex2)
		{
			switch (ex2.Status)
			{
			case WebExceptionStatus.NameResolutionFailure:
			case WebExceptionStatus.ConnectFailure:
			case WebExceptionStatus.ConnectionClosed:
			case WebExceptionStatus.ProxyNameResolutionFailure:
				return "Could not connect to the update server. Please check the internet connection.";
			case WebExceptionStatus.Timeout:
				return "The update server did not respond in time. Please try again later.";
			case WebExceptionStatus.TrustFailure:
			case WebExceptionStatus.SecureChannelFailure:
				return "A secure connection to the update server could not be established.";
			case WebExceptionStatus.ProtocolError:
			{
				HttpWebResponse httpWebResponse = ex2.Response as HttpWebResponse;
				if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.NotFound)
				{
					return "The file is not available on the update server.";
				}
				if (httpWebResponse != null)
				{
					return "The update server returned an error (" + (int)httpWebResponse.StatusCode + " " + httpWebResponse.StatusDescription + ").";
				}
				break;
			}
			}
			if (ex2.InnerException != null)
			{
				return "Network error: " + ex2.Message + " (" + Describe(ex2.InnerException) + ")";
			}
			return "Network error: " + ex2.Message;
		}
		if (ex is UnauthorizedAccessException)
		{
			return "Access was denied while saving the file. Please run the downloader as Administrator.";
		}
		if (ex is IOException)
		{
			return "The file could not be saved (" + ex.Message + "). Please make sure Madhushala Pro is closed.";
		}
		if (ex.InnerException != null)
		{
			return Describe(ex.InnerException);
		}
		return ex.Message;
	}

	private static string FormatSize(long bytes)
	{
		if (bytes >= 1048576)
		{
			return ((double)bytes / 1048576.0).ToString("0.0") + " MB";
		}
		return Math.Max(1L, checked((long)Math.Round((double)bytes / 1024.0))) + " KB";
	}

	private static void SafeDelete(string p)
	{
		try
		{
			if (File.Exists(p))
			{
				File.Delete(p);
			}
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
	}
}

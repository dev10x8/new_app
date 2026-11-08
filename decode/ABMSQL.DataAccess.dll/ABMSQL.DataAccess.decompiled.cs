using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using ABMSQL.DataAccess.Properties;
using Microsoft.Win32;
using SQLDMO;

[assembly: AssemblyCompany("ABM")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyTitle("ABMSQL.DataAccess")]
[assembly: AssemblyDescription("ABMSQL.DataAccess")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyProduct("ABMSQL.DataAccess")]
[assembly: AssemblyCopyright("Copyright © ABM 2008")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("9b8518a5-f4ae-4cb2-9e9d-a036082c8ed8")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyVersion("1.0.0.0")]
namespace ABMSQL.DataAccess
{
	public class Installation : Form
	{
		protected string _ApplicationName = "";

		protected string _CompName = "";

		protected string _InstanceName = "";

		protected string _DatabaseName = "";

		protected string _OptionalInstallPassword = "";

		protected string _MDFFilename = "";

		protected BusinessMSDEInstaller _Installer = null;

		public bool IsDone = false;

		public bool IsRestart = false;

		public Configuration _myconfig = ConfigurationManager.OpenExeConfiguration((ConfigurationUserLevel)0);

		private IContainer components = null;

		private TextBox txtPath;

		private FolderBrowserDialog folderBrowserDialog1;

		private Label label1;

		private Panel pnlFirst;

		private Button btnFolder;

		private Panel pnlSecond;

		private Label label2;

		private Panel panel2;

		private Button btnNext;

		private Button btnAttach;

		private PictureBox pictureBox1;

		private Label lblInstall;

		public Installation(string _appName, string _compName, string _instanceName, string _databaseName, string _optionalInstallPassword, string _mdfFileName)
		{
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Invalid comparison between Unknown and I4
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			_ApplicationName = _appName;
			_CompName = _compName;
			_InstanceName = _instanceName;
			_DatabaseName = _databaseName;
			_OptionalInstallPassword = _optionalInstallPassword;
			_MDFFilename = _mdfFileName;
			_Installer = new BusinessMSDEInstaller(_CompName, _InstanceName, _DatabaseName);
			InitializeComponent();
			((Control)this).Text = "Welcome to " + _ApplicationName + " Configuration Wizard";
			((Control)lblInstall).Text = _ApplicationName + " Configuration";
			((Control)txtPath).Text = _Installer.MSDEInstallPath;
			if (GetAppSettingKey("IsInstall") == "0" && GetAppSettingKey("IsRestart") == "0")
			{
				IsDone = true;
				IsRestart = false;
				((Form)this).Close();
			}
			else if (GetAppSettingKey("IsInstall") == "1" && _Installer.IsMSDEInstanceExist())
			{
				DialogResult val = MessageBox.Show("Application has detected that an instance of this application is already exist. Do you want to delete it?", "Application Exist", (MessageBoxButtons)4, (MessageBoxIcon)32);
				if ((int)val == 6)
				{
					_Installer.Uninstall();
					SetAppSettingKey("IsInstall", "1");
					SetAppSettingKey("IsRestart", "0");
					_myconfig.Save();
					MessageBox.Show("Please restart your computer and re-run this application", "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
					IsDone = true;
					IsRestart = true;
					((Form)this).Close();
				}
				else
				{
					SetAppSettingKey("IsInstall", "0");
					_myconfig.Save();
				}
			}
		}

		public string GetAppSettingKey(string _keyname)
		{
			if (_myconfig.AppSettings.Settings[_keyname] == null)
			{
				return "";
			}
			return _myconfig.AppSettings.Settings[_keyname].Value;
		}

		public void SetAppSettingKey(string _keyname, string _value)
		{
			if (_myconfig.AppSettings.Settings[_keyname] == null)
			{
				_myconfig.AppSettings.Settings.Add(_keyname, _value);
			}
			else
			{
				_myconfig.AppSettings.Settings[_keyname].Value = _value;
			}
		}

		private void btnFolder_Click(object sender, EventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			DialogResult val = ((CommonDialog)folderBrowserDialog1).ShowDialog();
			if ((int)val == 1)
			{
				((Control)txtPath).Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				try
				{
					if (Directory.Exists(((Control)txtPath).Text))
					{
						Directory.Delete(((Control)txtPath).Text, recursive: true);
					}
				}
				catch
				{
				}
				if (_Installer.Install(_OptionalInstallPassword, ((Control)txtPath).Text) == 0)
				{
					SetAppSettingKey("IsInstall", "0");
					SetAppSettingKey("IsRestart", "1");
					IsDone = true;
					IsRestart = true;
					_myconfig.Save();
				}
			}
			catch
			{
			}
			MessageBox.Show("Please restart your computer and re-run this application", "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Form)this).Close();
			Application.Exit();
		}

		private void Installation_Load(object sender, EventArgs e)
		{
			if (GetAppSettingKey("IsInstall") == "1")
			{
				((Control)pnlFirst).Visible = true;
			}
			else
			{
				((Control)pnlSecond).Visible = true;
			}
		}

		private void btnAttach_Click(object sender, EventArgs e)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			if (GetAppSettingKey("IsRestart") == "1" || !_Installer.IsDatabaseExist())
			{
				try
				{
					_Installer.AttachDatabase(_MDFFilename, "sa", _OptionalInstallPassword);
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf(_MDFFilename) > 0)
					{
						MessageBox.Show("MDF File in MSDE2000\\" + _MDFFilename + " does not exist, please re-extract application file");
						Application.Exit();
						IsRestart = true;
						IsDone = true;
					}
					else
					{
						MessageBox.Show("Please restart your computer and re-run this application.\r\n" + ex.Message, "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
						IsDone = true;
						IsRestart = true;
						Application.Exit();
						((Form)this).Close();
					}
					return;
				}
				SetAppSettingKey("IsRestart", "0");
				_myconfig.Save();
				IsRestart = false;
				MessageBox.Show("Application is successfully installed", "Success", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			IsDone = true;
			((Form)this).Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			((Form)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Expected O, but got Unknown
			//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f4: Expected O, but got Unknown
			//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
			txtPath = new TextBox();
			folderBrowserDialog1 = new FolderBrowserDialog();
			label1 = new Label();
			pnlFirst = new Panel();
			btnNext = new Button();
			btnFolder = new Button();
			pnlSecond = new Panel();
			btnAttach = new Button();
			label2 = new Label();
			panel2 = new Panel();
			lblInstall = new Label();
			pictureBox1 = new PictureBox();
			((Control)pnlFirst).SuspendLayout();
			((Control)pnlSecond).SuspendLayout();
			((Control)panel2).SuspendLayout();
			((ISupportInitialize)pictureBox1).BeginInit();
			((Control)this).SuspendLayout();
			((Control)txtPath).Location = new Point(128, 9);
			((Control)txtPath).Margin = new Padding(4);
			((Control)txtPath).Name = "txtPath";
			((TextBoxBase)txtPath).ReadOnly = true;
			((Control)txtPath).Size = new Size(291, 23);
			((Control)txtPath).TabIndex = 0;
			((Control)label1).AutoSize = true;
			((Control)label1).Location = new Point(4, 12);
			((Control)label1).Margin = new Padding(4, 0, 4, 0);
			((Control)label1).Name = "label1";
			((Control)label1).Size = new Size(116, 16);
			((Control)label1).TabIndex = 1;
			((Control)label1).Text = "Installation Path";
			((Control)pnlFirst).Controls.Add((Control)(object)btnNext);
			((Control)pnlFirst).Controls.Add((Control)(object)btnFolder);
			((Control)pnlFirst).Controls.Add((Control)(object)label1);
			((Control)pnlFirst).Controls.Add((Control)(object)txtPath);
			((Control)pnlFirst).Location = new Point(12, 121);
			((Control)pnlFirst).Name = "pnlFirst";
			((Control)pnlFirst).Size = new Size(461, 99);
			((Control)pnlFirst).TabIndex = 3;
			((Control)pnlFirst).Visible = false;
			((Control)btnNext).Anchor = (AnchorStyles)10;
			((Control)btnNext).Location = new Point(369, 55);
			((Control)btnNext).Name = "btnNext";
			((Control)btnNext).Size = new Size(88, 34);
			((Control)btnNext).TabIndex = 4;
			((Control)btnNext).Text = "Next";
			((ButtonBase)btnNext).UseVisualStyleBackColor = true;
			((Control)btnNext).Click += btnNext_Click;
			((Control)btnFolder).Location = new Point(426, 9);
			((Control)btnFolder).Name = "btnFolder";
			((Control)btnFolder).Size = new Size(32, 24);
			((Control)btnFolder).TabIndex = 3;
			((Control)btnFolder).Text = "...";
			((ButtonBase)btnFolder).UseVisualStyleBackColor = true;
			((Control)btnFolder).Click += btnFolder_Click;
			((Control)pnlSecond).Controls.Add((Control)(object)btnAttach);
			((Control)pnlSecond).Controls.Add((Control)(object)label2);
			((Control)pnlSecond).Location = new Point(12, 121);
			((Control)pnlSecond).Name = "pnlSecond";
			((Control)pnlSecond).Size = new Size(461, 99);
			((Control)pnlSecond).TabIndex = 4;
			((Control)pnlSecond).Visible = false;
			((Control)btnAttach).Anchor = (AnchorStyles)10;
			((Control)btnAttach).Location = new Point(370, 55);
			((Control)btnAttach).Name = "btnAttach";
			((Control)btnAttach).Size = new Size(88, 34);
			((Control)btnAttach).TabIndex = 5;
			((Control)btnAttach).Text = "Next";
			((ButtonBase)btnAttach).UseVisualStyleBackColor = true;
			((Control)btnAttach).Click += btnAttach_Click;
			((Control)label2).AutoSize = true;
			((Control)label2).Location = new Point(4, 9);
			((Control)label2).Margin = new Padding(4, 0, 4, 0);
			((Control)label2).Name = "label2";
			((Control)label2).Size = new Size(426, 16);
			((Control)label2).TabIndex = 2;
			((Control)label2).Text = "Application is now configuring database. Click Next to continue";
			((Control)panel2).BackColor = Color.White;
			((Control)panel2).Controls.Add((Control)(object)lblInstall);
			((Control)panel2).Controls.Add((Control)(object)pictureBox1);
			((Control)panel2).Location = new Point(-2, -1);
			((Control)panel2).Name = "panel2";
			((Control)panel2).Size = new Size(484, 106);
			((Control)panel2).TabIndex = 5;
			((Control)lblInstall).Font = new Font("Verdana", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)lblInstall).Location = new Point(136, 13);
			((Control)lblInstall).Name = "lblInstall";
			((Control)lblInstall).Size = new Size(335, 74);
			((Control)lblInstall).TabIndex = 1;
			((Control)lblInstall).Text = "Food Installation";
			pictureBox1.Image = (Image)(object)Resources.bxp58369;
			((Control)pictureBox1).Location = new Point(14, 13);
			((Control)pictureBox1).Name = "pictureBox1";
			((Control)pictureBox1).Size = new Size(105, 74);
			pictureBox1.SizeMode = (PictureBoxSizeMode)1;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Form)this).ClientSize = new Size(481, 231);
			((Control)this).Controls.Add((Control)(object)panel2);
			((Control)this).Controls.Add((Control)(object)pnlSecond);
			((Control)this).Controls.Add((Control)(object)pnlFirst);
			((Control)this).Font = new Font("Verdana", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Form)this).FormBorderStyle = (FormBorderStyle)1;
			((Form)this).Margin = new Padding(4);
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Control)this).Name = "Installation";
			((Form)this).StartPosition = (FormStartPosition)1;
			((Control)this).Text = "Welcome to ";
			((Form)this).Load += Installation_Load;
			((Control)pnlFirst).ResumeLayout(false);
			((Control)pnlFirst).PerformLayout();
			((Control)pnlSecond).ResumeLayout(false);
			((Control)pnlSecond).PerformLayout();
			((Control)panel2).ResumeLayout(false);
			((ISupportInitialize)pictureBox1).EndInit();
			((Control)this).ResumeLayout(false);
		}
	}
	public class BusinessSQLEXPRInstaller
	{
		protected string _ComputerInstanceName = "";

		protected string _ComputerName = "";

		protected string _InstanceName = "";

		protected string _SQLEXPRSourcePath = "SQLEXPR2005";

		protected string _SQLEXPRInstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Trim() + "\\SQLEXPR2005";

		protected string _UserName = "sa";

		protected string _SAPassword = "sa";

		protected string _DatabaseName = "MyDatabase";

		public string SQLEXPRInstallPath
		{
			get
			{
				return _SQLEXPRInstallPath;
			}
			set
			{
				_SQLEXPRInstallPath = value;
			}
		}

		public BusinessSQLEXPRInstaller(string _computerName, string _instanceName, string _databaseName)
		{
			if (_instanceName == "")
			{
				_ComputerInstanceName = "";
			}
			else
			{
				_ComputerInstanceName = _computerName + "\\" + _instanceName;
				_InstanceName = _instanceName;
			}
			_DatabaseName = _databaseName;
		}

		protected string GetSQLEXPRSourceFullPath()
		{
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _SQLEXPRSourcePath);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		protected string GetInstalltionProductName()
		{
			string[] array = _ComputerInstanceName.Split(new char[1] { '\\' });
			if (array.Length != 2)
			{
				return "";
			}
			return "Microsoft SQL Server 2005 Express Edition (" + array[1] + ")";
		}

		protected RegistryKey GetRegistryBySubFolderKeyName(RegistryKey _registryKey, string _keyName, string _keyvalue)
		{
			string[] subKeyNames = _registryKey.GetSubKeyNames();
			if (subKeyNames.Length <= 0)
			{
				return null;
			}
			string[] array = subKeyNames;
			foreach (string name in array)
			{
				RegistryKey registryKey = _registryKey.OpenSubKey(name, writable: true);
				try
				{
					if (registryKey.GetValue(_keyName).ToString().ToLower() == _keyvalue.ToLower())
					{
						return registryKey;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		protected string GetRegistryValueNameByValue(RegistryKey _registryKey, string _value)
		{
			string[] valueNames = _registryKey.GetValueNames();
			string[] array = valueNames;
			foreach (string text in array)
			{
				if (_registryKey.GetValue(text).ToString().ToLower() == _value.ToLower())
				{
					return text;
				}
			}
			return "";
		}

		protected string GetRegistryShortName(string _name)
		{
			int num = _name.LastIndexOf('\\');
			return _name.Substring(num + 1);
		}

		protected string GetInstallationPath(string _path)
		{
			int num = _path.LastIndexOf(".exe");
			string path = _path;
			if (num >= 0)
			{
				path = _path.Substring(0, num + 3);
			}
			DirectoryInfo parent = Directory.GetParent(path);
			return parent.FullName;
		}

		public bool IsDatabaseExist()
		{
			Application application = new ApplicationClass();
			SQLServer sQLServer = new SQLServerClass();
			sQLServer.Connect(_InstanceName, _UserName, _SAPassword);
			foreach (Database database in sQLServer.Databases)
			{
				if (database.Name != null && database.Name.Trim().ToLower() == _DatabaseName.Trim().ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public bool IsSQLEXPRInstanceExist()
		{
			string installtionProductName = GetInstalltionProductName();
			RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Products");
			RegistryKey registryBySubFolderKeyName = GetRegistryBySubFolderKeyName(registryKey, "ProductName", installtionProductName);
			if (registryBySubFolderKeyName != null)
			{
				return true;
			}
			return false;
		}

		public string GetCurrentSQLInstallationPath()
		{
			string installtionProductName = GetInstalltionProductName();
			string[] array = null;
			if (_ComputerInstanceName != "")
			{
				array = _ComputerInstanceName.Split(new char[1] { '\\' });
			}
			string result = "";
			RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft", writable: true);
			RegistryKey registryKey2 = null;
			if (_ComputerInstanceName == "")
			{
				registryKey2 = registryKey.OpenSubKey("MSSQLServer", writable: true);
				result = GetInstallationPath(registryKey2.GetValue("ImagePath").ToString());
			}
			else if (array.Length == 2)
			{
				registryKey = registryKey.OpenSubKey("Microsoft SQL Server", writable: true);
				registryKey2 = registryKey.OpenSubKey(array[1], writable: true);
				RegistryKey registryKey3 = registryKey2.OpenSubKey("Setup", writable: true);
				result = GetInstallationPath(registryKey3.GetValue("SQLPath").ToString());
			}
			return result;
		}

		public void Uninstall()
		{
			string installtionProductName = GetInstalltionProductName();
			string[] array = null;
			if (_ComputerInstanceName != "")
			{
				array = _ComputerInstanceName.Split(new char[1] { '\\' });
			}
			string path = "";
			RegistryKey registryKey = null;
			try
			{
				RegistryKey registryKey2 = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Products", writable: true);
				registryKey = GetRegistryBySubFolderKeyName(registryKey2, "ProductName", installtionProductName);
				if (registryKey != null)
				{
					registryKey2.DeleteSubKeyTree(GetRegistryShortName(registryKey.Name));
				}
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey3 = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Patches", writable: true);
				registryKey3.DeleteSubKeyTree(GetRegistryShortName(registryKey.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey4 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft", writable: true);
				RegistryKey registryKey5 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey5 = registryKey4.OpenSubKey("MSSQLServer", writable: true);
					path = GetInstallationPath(registryKey5.GetValue("ImagePath").ToString());
				}
				else if (array.Length == 2)
				{
					registryKey4 = registryKey4.OpenSubKey("Microsoft SQL Server", writable: true);
					registryKey5 = registryKey4.OpenSubKey(array[1], writable: true);
					RegistryKey registryKey6 = registryKey5.OpenSubKey("Setup", writable: true);
					path = GetInstallationPath(registryKey6.GetValue("SQLDataRoot").ToString());
				}
				registryKey4.DeleteSubKeyTree(GetRegistryShortName(registryKey5.Name));
			}
			catch
			{
			}
			RegistryKey registryKey7 = null;
			try
			{
				RegistryKey registryKey8 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows")
					.OpenSubKey("CurrentVersion")
					.OpenSubKey("Uninstall", writable: true);
				registryKey7 = GetRegistryBySubFolderKeyName(registryKey8, "DisplayName", installtionProductName);
				registryKey8.DeleteSubKeyTree(GetRegistryShortName(registryKey7.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey9 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Microsoft SQL Server")
					.OpenSubKey("Component Set", writable: true);
				string registryValueNameByValue = GetRegistryValueNameByValue(registryKey9, GetRegistryShortName(registryKey7.Name));
				if (registryValueNameByValue != "")
				{
					registryKey9.DeleteValue(registryValueNameByValue);
				}
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey10 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services", writable: true);
				RegistryKey registryKey11 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey11 = registryKey10.OpenSubKey("MSSQLServer", writable: true);
				}
				else if (array.Length == 2)
				{
					registryKey11 = registryKey10.OpenSubKey("MSSQL$" + array[1], writable: true);
				}
				registryKey10.DeleteSubKeyTree(GetRegistryShortName(registryKey11.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey12 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services", writable: true);
				RegistryKey registryKey13 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey13 = registryKey12.OpenSubKey("SQLServerAgent", writable: true);
				}
				else if (array.Length == 2)
				{
					registryKey13 = registryKey12.OpenSubKey("SQLAgent$" + array[1], writable: true);
				}
				registryKey12.DeleteSubKeyTree(GetRegistryShortName(registryKey13.Name));
			}
			catch
			{
			}
			Directory.Delete(path, recursive: true);
		}

		public int Install(string _optionalSAPassword, string _optionalInstallPath)
		{
			if (IsSQLEXPRInstanceExist())
			{
				throw new Exception("SQL EXPRRESS Instance name " + _InstanceName + "already exist");
			}
			if (_optionalSAPassword != "")
			{
				_SAPassword = _optionalSAPassword;
			}
			if (_optionalInstallPath != "")
			{
				_SQLEXPRInstallPath = _optionalInstallPath;
			}
			string sQLEXPRSourceFullPath = GetSQLEXPRSourceFullPath();
			if (!File.Exists(Path.Combine(sQLEXPRSourceFullPath, "Setup.Exe")))
			{
				throw new Exception("SQL EXPRESS Installer does not exist in " + Path.Combine(sQLEXPRSourceFullPath, "Setup.exe"));
			}
			string path = Path.Combine(sQLEXPRSourceFullPath, "setup.ini");
			string text = "\"" + _SQLEXPRInstallPath + "\\Binn\"";
			string text2 = "\"" + _SQLEXPRInstallPath + "\\Data\"";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			string text3 = "[Options] DISABLENETWORKPROTOCOLS=0 SECURITYMODE=SQL TARGETDIR=" + text + " DATADIR=" + text2;
			streamWriter.WriteLine(text3.ToString());
			streamWriter.Close();
			fileStream.Close();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = Path.Combine(sQLEXPRSourceFullPath, "setup.exe");
			processStartInfo.Arguments = "-q /norebootchk /qb reboot=ReallySuppress addlocal=all INSTANCENAME=" + _InstanceName + " SECURITYMODE=SQL SAPWD=" + _SAPassword + " DISABLENETWORKPROTOCOLS=0 SQLAUTOSTART=1";
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
			process.WaitForExit();
			return process.ExitCode;
		}

		public void BackupDatabase(string _dbname, string _backupName, string _optionalSAUsername, string _optionalSAPassword)
		{
			try
			{
				if (_optionalSAUsername != "")
				{
					_UserName = _optionalSAUsername;
				}
				if (_optionalSAPassword != "")
				{
					_SAPassword = _optionalSAPassword;
				}
				string sQLEXPRSourceFullPath = GetSQLEXPRSourceFullPath();
				string text = Path.Combine(sQLEXPRSourceFullPath, _backupName);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				Backup backup = new BackupClass();
				_SQLServer sQLServer = new SQLServerClass();
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				backup.Action = SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
				backup.Database = _dbname;
				backup.Files = "[" + text + "]";
				backup.BackupSetName = _dbname;
				backup.BackupSetDescription = "Database backup";
				backup.Initialize = true;
				backup.SQLBackup(sQLServer);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}

		public void RestoreDatabase(string _optionalMDFFileName, string _optionalSAUsername, string _optionalSAPassword)
		{
			try
			{
				if (_optionalSAUsername != "")
				{
					_UserName = _optionalSAUsername;
				}
				if (_optionalSAPassword != "")
				{
					_SAPassword = _optionalSAPassword;
				}
				string sQLEXPRSourceFullPath = GetSQLEXPRSourceFullPath();
				string text = Path.Combine(sQLEXPRSourceFullPath, "Application_Data.MDF");
				if (_optionalMDFFileName != "")
				{
					text = Path.Combine(sQLEXPRSourceFullPath, _optionalMDFFileName);
				}
				if (!File.Exists(text))
				{
					throw new Exception("Cannot attach Database File '" + text + "'. Database file does not exist");
				}
				SQLServer sQLServer = new SQLServerClass();
				Registry2 registry = null;
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				registry = (Registry2)sQLServer.Registry;
				string directoryName = Path.GetDirectoryName(text);
				registry.BackupDirectory = directoryName;
				string currentSQLInstallationPath = GetCurrentSQLInstallationPath();
				currentSQLInstallationPath = Path.Combine(currentSQLInstallationPath, "MSSQL\\Data");
				if (Directory.Exists(currentSQLInstallationPath))
				{
					Directory.CreateDirectory(currentSQLInstallationPath);
				}
				currentSQLInstallationPath = Path.Combine(currentSQLInstallationPath, _DatabaseName + "_Data.MDF");
				string text2 = currentSQLInstallationPath.Replace("_Data.MDF", "_Log.LDF");
				Restore restore = new RestoreClass();
				restore.Action = SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
				restore.Database = _DatabaseName;
				restore.Files = "[" + Path.GetFileName(text) + "]";
				QueryResults queryResults = restore.ReadFileList(sQLServer);
				string columnString = queryResults.GetColumnString(1, 1);
				string columnString2 = queryResults.GetColumnString(1, 2);
				string columnString3 = queryResults.GetColumnString(2, 1);
				string columnString4 = queryResults.GetColumnString(2, 2);
				string text3 = "[" + columnString + "],[" + columnString2 + "],[" + columnString + "],[" + currentSQLInstallationPath + "]";
				string text4 = text3;
				text3 = text4 + ",[" + columnString3 + "],[" + columnString4 + "],[" + columnString3 + "],[" + text2 + "]";
				restore.RelocateFiles = text3;
				restore.FileNumber = 1;
				restore.ReplaceDatabase = true;
				restore.SQLRestore(sQLServer);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}

		public void AttachDatabase(string _optionalMDFFileName, string _optionalSAUsername, string _optionalSAPassword)
		{
			if (_optionalSAUsername != "")
			{
				_UserName = _optionalSAUsername;
			}
			if (_optionalSAPassword != "")
			{
				_SAPassword = _optionalSAPassword;
			}
			string sQLEXPRSourceFullPath = GetSQLEXPRSourceFullPath();
			string text = Path.Combine(sQLEXPRSourceFullPath, "Application_Data.MDF");
			if (_optionalMDFFileName != "")
			{
				text = Path.Combine(sQLEXPRSourceFullPath, _optionalMDFFileName);
			}
			if (!File.Exists(text))
			{
				throw new Exception("Cannot attach Database File '" + text + "'. Database file does not exist");
			}
			text = "[" + text + "]";
			Application application = new ApplicationClass();
			SQLServer sQLServer = new SQLServerClass();
			try
			{
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				sQLServer.AttachDB(_DatabaseName, text);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}
	}
	public enum ServiceProviderEnum
	{
		SHA1,
		SHA256,
		SHA384,
		SHA512,
		MD5
	}
	public class BusinessHash
	{
		private string mSalt;

		private HashAlgorithm mCryptoService;

		public string Salt
		{
			get
			{
				return mSalt;
			}
			set
			{
				mSalt = value;
			}
		}

		public BusinessHash()
		{
			mCryptoService = new SHA1Managed();
		}

		public BusinessHash(ServiceProviderEnum serviceProvider)
		{
			switch (serviceProvider)
			{
			case ServiceProviderEnum.MD5:
				mCryptoService = new MD5CryptoServiceProvider();
				break;
			case ServiceProviderEnum.SHA1:
				mCryptoService = new SHA1Managed();
				break;
			case ServiceProviderEnum.SHA256:
				mCryptoService = new SHA256Managed();
				break;
			case ServiceProviderEnum.SHA384:
				mCryptoService = new SHA384Managed();
				break;
			case ServiceProviderEnum.SHA512:
				mCryptoService = new SHA512Managed();
				break;
			}
		}

		public BusinessHash(string serviceProviderName)
		{
			try
			{
				mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
			}
			catch
			{
				throw;
			}
		}

		public virtual string Encrypt(string plainText)
		{
			byte[] array = mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + mSalt));
			return Convert.ToBase64String(array, 0, array.Length);
		}

		public virtual byte[] EncryptByte(string plainText)
		{
			return mCryptoService.ComputeHash(Encoding.ASCII.GetBytes(plainText + mSalt));
		}
	}
	public class BusinessMSDEInstaller
	{
		protected string _ComputerInstanceName = "";

		protected string _ComputerName = "";

		protected string _InstanceName = "";

		protected string _MSDESourcePath = "MSDE2000";

		protected string _MSDEInstallPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Trim() + "\\MSDE2000";

		protected string _UserName = "sa";

		protected string _SAPassword = "sa";

		protected string _DatabaseName = "MyDatabase";

		public string MSDEInstallPath
		{
			get
			{
				return _MSDEInstallPath;
			}
			set
			{
				_MSDEInstallPath = value;
			}
		}

		public BusinessMSDEInstaller(string _computerName, string _instanceName, string _databaseName)
		{
			if (_instanceName == "")
			{
				_ComputerInstanceName = "";
			}
			else
			{
				_ComputerInstanceName = _computerName + "\\" + _instanceName;
				_InstanceName = _instanceName;
			}
			_DatabaseName = _databaseName;
		}

		protected string GetMSDESourceFullPath()
		{
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _MSDESourcePath);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		protected string GetInstalltionProductName()
		{
			string[] array = _ComputerInstanceName.Split(new char[1] { '\\' });
			if (array.Length != 2)
			{
				return "";
			}
			return "Microsoft SQL Server Desktop Engine (" + array[1] + ")";
		}

		protected RegistryKey GetRegistryBySubFolderKeyName(RegistryKey _registryKey, string _keyName, string _keyvalue)
		{
			string[] subKeyNames = _registryKey.GetSubKeyNames();
			if (subKeyNames.Length <= 0)
			{
				return null;
			}
			string[] array = subKeyNames;
			foreach (string name in array)
			{
				RegistryKey registryKey = _registryKey.OpenSubKey(name, writable: true);
				try
				{
					if (registryKey.GetValue(_keyName).ToString().ToLower() == _keyvalue.ToLower())
					{
						return registryKey;
					}
				}
				catch
				{
				}
			}
			return null;
		}

		protected string GetRegistryValueNameByValue(RegistryKey _registryKey, string _value)
		{
			string[] valueNames = _registryKey.GetValueNames();
			string[] array = valueNames;
			foreach (string text in array)
			{
				if (_registryKey.GetValue(text).ToString().ToLower() == _value.ToLower())
				{
					return text;
				}
			}
			return "";
		}

		protected string GetRegistryShortName(string _name)
		{
			int num = _name.LastIndexOf('\\');
			return _name.Substring(num + 1);
		}

		protected string GetInstallationPath(string _path)
		{
			int num = _path.LastIndexOf(".exe");
			string path = _path;
			if (num >= 0)
			{
				path = _path.Substring(0, num + 3);
			}
			DirectoryInfo parent = Directory.GetParent(path);
			return parent.FullName;
		}

		public string GetCurrentSQLInstallationPath()
		{
			string installtionProductName = GetInstalltionProductName();
			string[] array = null;
			if (_ComputerInstanceName != "")
			{
				array = _ComputerInstanceName.Split(new char[1] { '\\' });
			}
			string result = "";
			RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft", writable: true);
			RegistryKey registryKey2 = null;
			if (_ComputerInstanceName == "")
			{
				registryKey2 = registryKey.OpenSubKey("MSSQLServer", writable: true);
				result = GetInstallationPath(registryKey2.GetValue("ImagePath").ToString());
			}
			else if (array.Length == 2)
			{
				registryKey = registryKey.OpenSubKey("Microsoft SQL Server", writable: true);
				registryKey2 = registryKey.OpenSubKey(array[1], writable: true);
				RegistryKey registryKey3 = registryKey2.OpenSubKey("Setup", writable: true);
				result = GetInstallationPath(registryKey3.GetValue("SQLPath").ToString());
			}
			return result;
		}

		public bool IsDatabaseExist()
		{
			Application application = new ApplicationClass();
			SQLServer sQLServer = new SQLServerClass();
			sQLServer.Connect(_InstanceName, _UserName, _SAPassword);
			foreach (Database database in sQLServer.Databases)
			{
				if (database.Name != null && database.Name.Trim().ToLower() == _DatabaseName.Trim().ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public bool IsMSDEInstanceExist()
		{
			string installtionProductName = GetInstalltionProductName();
			RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Products");
			RegistryKey registryBySubFolderKeyName = GetRegistryBySubFolderKeyName(registryKey, "ProductName", installtionProductName);
			if (registryBySubFolderKeyName != null)
			{
				return true;
			}
			return false;
		}

		public void Uninstall()
		{
			string installtionProductName = GetInstalltionProductName();
			string[] array = null;
			if (_ComputerInstanceName != "")
			{
				array = _ComputerInstanceName.Split(new char[1] { '\\' });
			}
			string path = "";
			RegistryKey registryKey = null;
			try
			{
				RegistryKey registryKey2 = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Products", writable: true);
				registryKey = GetRegistryBySubFolderKeyName(registryKey2, "ProductName", installtionProductName);
				if (registryKey != null)
				{
					registryKey2.DeleteSubKeyTree(GetRegistryShortName(registryKey.Name));
				}
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey3 = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey("Installer").OpenSubKey("Patches", writable: true);
				registryKey3.DeleteSubKeyTree(GetRegistryShortName(registryKey.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey4 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft", writable: true);
				RegistryKey registryKey5 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey5 = registryKey4.OpenSubKey("MSSQLServer", writable: true);
					path = GetInstallationPath(registryKey5.GetValue("ImagePath").ToString());
				}
				else if (array.Length == 2)
				{
					registryKey4 = registryKey4.OpenSubKey("Microsoft SQL Server", writable: true);
					registryKey5 = registryKey4.OpenSubKey(array[1], writable: true);
					RegistryKey registryKey6 = registryKey5.OpenSubKey("Setup", writable: true);
					path = GetInstallationPath(registryKey6.GetValue("SQLDataRoot").ToString());
				}
				registryKey4.DeleteSubKeyTree(GetRegistryShortName(registryKey5.Name));
			}
			catch
			{
			}
			RegistryKey registryKey7 = null;
			try
			{
				RegistryKey registryKey8 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Windows")
					.OpenSubKey("CurrentVersion")
					.OpenSubKey("Uninstall", writable: true);
				registryKey7 = GetRegistryBySubFolderKeyName(registryKey8, "DisplayName", installtionProductName);
				registryKey8.DeleteSubKeyTree(GetRegistryShortName(registryKey7.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey9 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("Microsoft").OpenSubKey("Microsoft SQL Server")
					.OpenSubKey("Component Set", writable: true);
				string registryValueNameByValue = GetRegistryValueNameByValue(registryKey9, GetRegistryShortName(registryKey7.Name));
				if (registryValueNameByValue != "")
				{
					registryKey9.DeleteValue(registryValueNameByValue);
				}
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey10 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services", writable: true);
				RegistryKey registryKey11 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey11 = registryKey10.OpenSubKey("MSSQLServer", writable: true);
				}
				else if (array.Length == 2)
				{
					registryKey11 = registryKey10.OpenSubKey("MSSQL$" + array[1], writable: true);
				}
				registryKey10.DeleteSubKeyTree(GetRegistryShortName(registryKey11.Name));
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey12 = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM").OpenSubKey("CurrentControlSet").OpenSubKey("Services", writable: true);
				RegistryKey registryKey13 = null;
				if (_ComputerInstanceName == "")
				{
					registryKey13 = registryKey12.OpenSubKey("SQLServerAgent", writable: true);
				}
				else if (array.Length == 2)
				{
					registryKey13 = registryKey12.OpenSubKey("SQLAgent$" + array[1], writable: true);
				}
				registryKey12.DeleteSubKeyTree(GetRegistryShortName(registryKey13.Name));
			}
			catch
			{
			}
			Directory.Delete(path, recursive: true);
		}

		public int Install(string _optionalSAPassword, string _optionalInstallPath)
		{
			if (IsMSDEInstanceExist())
			{
				throw new Exception("MSDE Instance name " + _InstanceName + "already exist");
			}
			if (_optionalSAPassword != "")
			{
				_SAPassword = _optionalSAPassword;
			}
			if (_optionalInstallPath != "")
			{
				_MSDEInstallPath = _optionalInstallPath;
			}
			string mSDESourceFullPath = GetMSDESourceFullPath();
			if (!File.Exists(Path.Combine(mSDESourceFullPath, "Setup.Exe")))
			{
				throw new Exception("MSDE Installer does not exist in " + Path.Combine(mSDESourceFullPath, "Setup.exe"));
			}
			string path = Path.Combine(mSDESourceFullPath, "setup.ini");
			string text = "\"" + _MSDEInstallPath + "\\Binn\"";
			string text2 = "\"" + _MSDEInstallPath + "\\Data\"";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			string text3 = "[Options] DISABLENETWORKPROTOCOLS=0 SECURITYMODE=SQL TARGETDIR=" + text + " DATADIR=" + text2;
			streamWriter.WriteLine(text3.ToString());
			streamWriter.Close();
			fileStream.Close();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.FileName = Path.Combine(mSDESourceFullPath, "setup.exe");
			processStartInfo.Arguments = "INSTANCENAME=" + _InstanceName + " SAPWD=" + _SAPassword + " DISABLENETWORKPROTOCOLS=0 SECURITYMODE=SQL TARGETDIR=" + text + " DATADIR=" + text2 + "/qb";
			Process process = new Process();
			process.StartInfo = processStartInfo;
			process.Start();
			process.WaitForExit();
			return process.ExitCode;
		}

		public void BackupDatabase(string _dbname, string _backupName, string _optionalSAUsername, string _optionalSAPassword)
		{
			try
			{
				if (_optionalSAUsername != "")
				{
					_UserName = _optionalSAUsername;
				}
				if (_optionalSAPassword != "")
				{
					_SAPassword = _optionalSAPassword;
				}
				string mSDESourceFullPath = GetMSDESourceFullPath();
				string text = Path.Combine(mSDESourceFullPath, _backupName);
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				Backup backup = new BackupClass();
				_SQLServer sQLServer = new SQLServerClass();
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				backup.Action = SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
				backup.Database = _dbname;
				backup.Files = "[" + text + "]";
				backup.BackupSetName = _dbname;
				backup.BackupSetDescription = "Database backup";
				backup.Initialize = true;
				backup.SQLBackup(sQLServer);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}

		public void RestoreDatabase(string _optionalMDFFileName, string _optionalSAUsername, string _optionalSAPassword)
		{
			try
			{
				if (_optionalSAUsername != "")
				{
					_UserName = _optionalSAUsername;
				}
				if (_optionalSAPassword != "")
				{
					_SAPassword = _optionalSAPassword;
				}
				string mSDESourceFullPath = GetMSDESourceFullPath();
				string text = Path.Combine(mSDESourceFullPath, "Application_Data.MDF");
				if (_optionalMDFFileName != "")
				{
					text = Path.Combine(mSDESourceFullPath, _optionalMDFFileName);
				}
				if (!File.Exists(text))
				{
					throw new Exception("Cannot attach Database File '" + text + "'. Database file does not exist");
				}
				SQLServer sQLServer = new SQLServerClass();
				Registry2 registry = null;
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				registry = (Registry2)sQLServer.Registry;
				string directoryName = Path.GetDirectoryName(text);
				registry.BackupDirectory = directoryName;
				string currentSQLInstallationPath = GetCurrentSQLInstallationPath();
				currentSQLInstallationPath = Path.Combine(currentSQLInstallationPath, "MSSQL\\Data");
				if (Directory.Exists(currentSQLInstallationPath))
				{
					Directory.CreateDirectory(currentSQLInstallationPath);
				}
				currentSQLInstallationPath = Path.Combine(currentSQLInstallationPath, _DatabaseName + "_Data.MDF");
				string text2 = currentSQLInstallationPath.Replace("_Data.MDF", "_Log.LDF");
				Restore restore = new RestoreClass();
				restore.Action = SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
				restore.Database = _DatabaseName;
				restore.Files = "[" + Path.GetFileName(text) + "]";
				QueryResults queryResults = restore.ReadFileList(sQLServer);
				string columnString = queryResults.GetColumnString(1, 1);
				string columnString2 = queryResults.GetColumnString(1, 2);
				string columnString3 = queryResults.GetColumnString(2, 1);
				string columnString4 = queryResults.GetColumnString(2, 2);
				string text3 = "[" + columnString + "],[" + columnString2 + "],[" + columnString + "],[" + currentSQLInstallationPath + "]";
				string text4 = text3;
				text3 = text4 + ",[" + columnString3 + "],[" + columnString4 + "],[" + columnString3 + "],[" + text2 + "]";
				restore.RelocateFiles = text3;
				restore.FileNumber = 1;
				restore.ReplaceDatabase = true;
				restore.SQLRestore(sQLServer);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}

		public void AttachDatabase(string _optionalMDFFileName, string _optionalSAUsername, string _optionalSAPassword)
		{
			if (_optionalSAUsername != "")
			{
				_UserName = _optionalSAUsername;
			}
			if (_optionalSAPassword != "")
			{
				_SAPassword = _optionalSAPassword;
			}
			string mSDESourceFullPath = GetMSDESourceFullPath();
			string text = Path.Combine(mSDESourceFullPath, "Application_Data.MDF");
			if (_optionalMDFFileName != "")
			{
				text = Path.Combine(mSDESourceFullPath, _optionalMDFFileName);
			}
			if (!File.Exists(text))
			{
				throw new Exception("Cannot attach Database File '" + text + "'. Database file does not exist");
			}
			text = "[" + text + "]";
			Application application = new ApplicationClass();
			SQLServer sQLServer = new SQLServerClass();
			try
			{
				sQLServer.Connect(_ComputerInstanceName, _UserName, _SAPassword);
				sQLServer.AttachDB(_DatabaseName, text);
				sQLServer.DisConnect();
			}
			catch (Exception ex)
			{
				throw new Exception("Library unhandle exception : " + ex.Message);
			}
		}
	}
	public class DataAccessTableFiller
	{
		private DataAccessTableDefinition _Definition = null;

		private IDbCommand _LastCommand = null;

		private IDictionary<string, object> _ColumnValue = new Dictionary<string, object>();

		private bool _autoTruncateString = false;

		private string _AuditNote = "";

		protected DataRow _RowData;

		public DataAccessTableDefinition Definition => _Definition;

		public IDbCommand LastCommand
		{
			get
			{
				return _LastCommand;
			}
			set
			{
				_LastCommand = value;
			}
		}

		public IDictionary<string, object> ColumnValue => _ColumnValue;

		public bool AutoTruncateString
		{
			get
			{
				return _autoTruncateString;
			}
			set
			{
				_autoTruncateString = value;
			}
		}

		public string AuditNote
		{
			get
			{
				return _AuditNote;
			}
			set
			{
				_AuditNote = value;
			}
		}

		public DataRow RowData => _RowData;

		public DataAccessTableFiller(DataAccessTableDefinition _definition)
		{
			_Definition = _definition;
		}

		public string GetSortFilterCustom(string _ascending, IList<string> _list)
		{
			string text = "";
			foreach (string item in _list)
			{
				if (text != "")
				{
					text += ",";
				}
				text = text + item + " " + _ascending;
			}
			return text;
		}

		public string GetWhereFilterByAll(ref IList<IDbDataParameter> _filterParameters)
		{
			string text = "";
			int num = 1;
			_filterParameters = new List<IDbDataParameter>();
			foreach (DataRow row in _Definition.ColumnDefinition.Rows)
			{
				string text2 = row["COLUMN_NAME"].ToString().ToLower();
				if (_ColumnValue.ContainsKey(text2))
				{
					if (text != "")
					{
						text += " AND ";
					}
					string text3 = "@FilterParam" + num;
					text = text2 + "=" + text3;
					IDbDataParameter dbDataParameter = _Definition.DALLayer.GetCommand().CreateParameter();
					dbDataParameter.ParameterName = text3;
					dbDataParameter.Value = GetColumnValue(text2.ToLower());
					_filterParameters.Add(dbDataParameter);
					num++;
				}
			}
			return text;
		}

		public string GetWhereFilterByPrimaryKey(ref IDbDataParameter[] _filterParameters)
		{
			string text = "";
			int num = 1;
			_filterParameters = new IDbDataParameter[_Definition.PrimaryKeyColumns.Count];
			foreach (string primaryKeyColumn in _Definition.PrimaryKeyColumns)
			{
				if (text != "")
				{
					text += " AND ";
				}
				string text2 = "@FilterParam" + num;
				text = primaryKeyColumn + "=" + text2;
				if (!_ColumnValue.ContainsKey(primaryKeyColumn.ToLower()))
				{
					throw new Exception("Filler for table name " + _Definition.TableName + " does not fill value for column " + primaryKeyColumn);
				}
				_filterParameters[num - 1] = _Definition.DALLayer.GetCommand().CreateParameter();
				_filterParameters[num - 1].ParameterName = text2;
				_filterParameters[num - 1].Value = GetColumnValue(primaryKeyColumn.ToLower());
				num++;
			}
			return text;
		}

		public string GetWhereFilterByCustom(IList<string> _columns, ref IDbDataParameter[] _filterParameters)
		{
			string text = "";
			int num = 1;
			_filterParameters = new IDbDataParameter[_Definition.PrimaryKeyColumns.Count];
			foreach (string _column in _columns)
			{
				if (text != "")
				{
					text += " AND ";
				}
				if (_ColumnValue.ContainsKey(_column.ToLower()))
				{
					string text2 = "@FilterParam" + num;
					text = _column + "=" + text2;
					_filterParameters[num - 1] = _Definition.DALLayer.GetCommand().CreateParameter();
					_filterParameters[num - 1].ParameterName = text2;
					_filterParameters[num - 1].Value = GetColumnValue(_column.ToLower());
					num++;
				}
			}
			return text;
		}

		public string GetWhereFilterByCustomReplace(IList<string> _columns, IList<string> _replaceColumns, ref IDbDataParameter[] _filterParameters)
		{
			string text = "";
			int num = 1;
			_filterParameters = new IDbDataParameter[_columns.Count];
			for (int i = 0; i < _columns.Count; i++)
			{
				if (text != "")
				{
					text += " AND ";
				}
				if (_ColumnValue.ContainsKey(_columns[i].ToLower()))
				{
					string text2 = "@FilterParam" + num;
					text = _replaceColumns[i] + "=" + text2;
					_filterParameters[num - 1] = _Definition.DALLayer.GetCommand().CreateParameter();
					_filterParameters[num - 1].ParameterName = text2;
					_filterParameters[num - 1].Value = GetColumnValue(_columns[i].ToLower());
					num++;
				}
			}
			return text;
		}

		public void AddDefaultColumnValueForOtherNotNullableColumn()
		{
			foreach (DataRow row in _Definition.ColumnDefinition.Rows)
			{
				string text = row["COLUMN_NAME"].ToString().ToLower();
				if (!_ColumnValue.ContainsKey(text) && !_Definition.NullAbleColumns.Contains(text))
				{
					AddColumnValue(text, _Definition.GetDefaultValueFor(text));
					if (_AuditNote != "")
					{
						_AuditNote += ",";
					}
					_AuditNote = _AuditNote + "[DefaultValueForNotNullableColumn,ColumnName:'" + text + "']";
				}
			}
		}

		public void ClearColumnValue()
		{
			_ColumnValue.Clear();
			_AuditNote = "";
		}

		public void AddColumnValue(string _columnName, object _value)
		{
			_columnName = _columnName.ToLower();
			if (_ColumnValue.ContainsKey(_columnName))
			{
				if (!_Definition.IsStringType(_columnName))
				{
					_ColumnValue[_columnName] = _value;
				}
				else
				{
					_ColumnValue[_columnName] = CheckAndTruncateParam(_columnName, _value);
				}
			}
			else if (!_Definition.IsStringType(_columnName))
			{
				_ColumnValue.Add(_columnName, _value);
			}
			else
			{
				_ColumnValue.Add(_columnName, CheckAndTruncateParam(_columnName, _value));
			}
		}

		public void AddColumnValueCustomByFiller(IList<string> _listColumns, DataAccessTableFiller _filler)
		{
			for (int i = 0; i < _listColumns.Count; i++)
			{
				string text = _listColumns[i].ToLower();
				if (_filler.ColumnValue.ContainsKey(text))
				{
					AddColumnValue(text, _filler.ColumnValue[text]);
				}
			}
		}

		public void AddColumnValueCustomReplaceByFiller(IList<string> _listColumns, IList<string> _listReplaceColumn, DataAccessTableFiller _filler)
		{
			if (_listColumns.Count != _listReplaceColumn.Count)
			{
				return;
			}
			for (int i = 0; i < _listColumns.Count; i++)
			{
				string text = _listColumns[i].ToLower();
				string columnName = _listReplaceColumn[i].ToLower();
				if (_filler.ColumnValue.ContainsKey(text))
				{
					AddColumnValue(columnName, _filler.ColumnValue[text]);
					continue;
				}
				throw new Exception("Current Filler with table name '" + _filler._Definition.TableName + "' does not contain value for column '" + text + "'");
			}
		}

		public object GetColumnValue(string _columnName)
		{
			_columnName = _columnName.ToLower();
			if (!_ColumnValue.ContainsKey(_columnName))
			{
				return null;
			}
			if (_ColumnValue[_columnName] == Convert.DBNull)
			{
				return null;
			}
			return _ColumnValue[_columnName];
		}

		public object GetColumnValueNotNull(string _columnName)
		{
			_columnName = _columnName.ToLower();
			if (!_ColumnValue.ContainsKey(_columnName) || _ColumnValue[_columnName] == Convert.DBNull)
			{
				return _Definition.GetDefaultValueFor(_columnName);
			}
			return _ColumnValue[_columnName];
		}

		public void SetColumnValueByDataRow(DataRow _row)
		{
			_RowData = _row;
			_ColumnValue.Clear();
			foreach (DataColumn column in _row.Table.Columns)
			{
				AddColumnValue(column.ColumnName, _row[column.ColumnName]);
			}
		}

		public string GetStringColumnValue(string _columnName)
		{
			object columnValue = GetColumnValue(_columnName);
			if (!(columnValue is string))
			{
				return "";
			}
			return (string)columnValue;
		}

		public int GetIntColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is int result))
			{
				return 0;
			}
			return result;
		}

		public long GetLongColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is long result))
			{
				return 0L;
			}
			return result;
		}

		public decimal GetDecimalColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is decimal result))
			{
				return 0m;
			}
			return result;
		}

		public float GetFloatColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is float result))
			{
				return 0f;
			}
			return result;
		}

		public short GetShortColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is short result))
			{
				return 0;
			}
			return result;
		}

		public byte[] GetByteColumnValue(string _columnName)
		{
			object columnValue = GetColumnValue(_columnName);
			if (!(columnValue is byte[]))
			{
				return null;
			}
			return (byte[])columnValue;
		}

		public DateTime GetDateTimeColumnValue(string _columnName)
		{
			if (!(GetColumnValue(_columnName) is DateTime result))
			{
				return DateTime.MinValue;
			}
			return result;
		}

		public IDbCommand GetUpdateStatementNoFilter()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "UPDATE {0} SET ";
			format = string.Format(format, _Definition.TableName);
			string text = "";
			IDbDataParameter[] _filterParameters = null;
			string text2 = " WHERE " + GetWhereFilterByPrimaryKey(ref _filterParameters);
			int num = 1;
			foreach (KeyValuePair<string, object> item in _ColumnValue)
			{
				if (text != "")
				{
					text += ",";
				}
				text = text + item.Key + "=@Param" + num;
				IDbDataParameter dbDataParameter = command.CreateParameter();
				dbDataParameter.ParameterName = "@Param" + num;
				dbDataParameter.Value = item.Value;
				command.Parameters.Add(dbDataParameter);
				num++;
			}
			command.CommandText = format + text + text2;
			IDbDataParameter[] array = _filterParameters;
			foreach (IDbDataParameter value in array)
			{
				command.Parameters.Add(value);
			}
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetUpdateStatementFilterIdentity()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "UPDATE {0} SET ";
			format = string.Format(format, _Definition.TableName);
			string text = "";
			IDbDataParameter[] _filterParameters = null;
			string text2 = " WHERE " + GetWhereFilterByPrimaryKey(ref _filterParameters);
			int num = 1;
			foreach (KeyValuePair<string, object> item in _ColumnValue)
			{
				if (!_Definition.IdentityColumns.Contains(item.Key))
				{
					if (text != "")
					{
						text += ",";
					}
					text = text + item.Key + "=@Param" + num;
					IDbDataParameter dbDataParameter = command.CreateParameter();
					dbDataParameter.ParameterName = "@Param" + num;
					dbDataParameter.Value = item.Value;
					command.Parameters.Add(dbDataParameter);
					num++;
				}
			}
			command.CommandText = format + text + text2;
			IDbDataParameter[] array = _filterParameters;
			foreach (IDbDataParameter value in array)
			{
				command.Parameters.Add(value);
			}
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetInsertStatementNoFilter()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "INSERT INTO {0} ";
			format = string.Format(format, _Definition.TableName);
			string text = "";
			string text2 = "";
			int num = 1;
			foreach (KeyValuePair<string, object> item in _ColumnValue)
			{
				if (text != "")
				{
					text += ",";
				}
				text += item.Key;
				if (text2 != "")
				{
					text2 += ",";
				}
				text2 = text2 + "@PARAM" + num;
				IDbDataParameter dbDataParameter = command.CreateParameter();
				dbDataParameter.ParameterName = "@PARAM" + num;
				dbDataParameter.Value = item.Value;
				command.Parameters.Add(dbDataParameter);
				num++;
			}
			text = "(" + text + ") ";
			text2 = "(" + text2 + ") ";
			command.CommandText = format + text + "VALUES " + text2;
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetInsertStatementFilterIdentity()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "INSERT INTO {0} ";
			format = string.Format(format, _Definition.TableName);
			string text = "";
			string text2 = "";
			int num = 1;
			bool flag = false;
			foreach (KeyValuePair<string, object> item in _ColumnValue)
			{
				if (_Definition.IdentityColumns.Contains(item.Key))
				{
					flag = true;
					continue;
				}
				if (text != "")
				{
					text += ",";
				}
				text += item.Key;
				if (text2 != "")
				{
					text2 += ",";
				}
				text2 = text2 + "@PARAM" + num;
				IDbDataParameter dbDataParameter = command.CreateParameter();
				dbDataParameter.ParameterName = "@PARAM" + num;
				dbDataParameter.Value = item.Value;
				command.Parameters.Add(dbDataParameter);
				num++;
			}
			text = "(" + text + ") ";
			text2 = "(" + text2 + ") ";
			command.CommandText = format + text + "VALUES " + text2;
			if ((_Definition.IdentityColumns != null && _Definition.IdentityColumns.Count > 0) || flag)
			{
				command.CommandText += "; SELECT @@IDENTITY as [Identity]";
			}
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetDeleteStatementForPrimaryKey()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "DELETE FROM {0}";
			format = string.Format(format, _Definition.TableName);
			IDbDataParameter[] _filterParameters = null;
			string whereFilterByPrimaryKey = GetWhereFilterByPrimaryKey(ref _filterParameters);
			if (whereFilterByPrimaryKey == "" || _filterParameters.Length <= 0)
			{
				command.CommandText = format;
				_LastCommand = command;
				return command;
			}
			command.CommandText = format + " WHERE " + whereFilterByPrimaryKey;
			IDbDataParameter[] array = _filterParameters;
			foreach (IDbDataParameter value in array)
			{
				command.Parameters.Add(value);
			}
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetDeleteStatementForAllFilter()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "DELETE FROM {0}";
			format = string.Format(format, _Definition.TableName);
			IList<IDbDataParameter> _filterParameters = new List<IDbDataParameter>();
			string whereFilterByAll = GetWhereFilterByAll(ref _filterParameters);
			if (whereFilterByAll == "" || _filterParameters.Count <= 0)
			{
				command.CommandText = format;
				_LastCommand = command;
				return command;
			}
			command.CommandText = format + " WHERE " + whereFilterByAll;
			foreach (IDbDataParameter item in _filterParameters)
			{
				command.Parameters.Add(item);
			}
			_LastCommand = command;
			return command;
		}

		public IDbCommand GetDeleteStatementForAll()
		{
			IDbCommand command = _Definition.DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string format = "DELETE FROM {0}";
			format = string.Format(format, _Definition.TableName);
			command.CommandText = format;
			_LastCommand = command;
			return command;
		}

		public object CheckAndTruncateParam(string _columnName, object _valuex)
		{
			if (!_Definition.IsStringType(_columnName) || !(_valuex is string))
			{
				return _valuex;
			}
			string text = _valuex.ToString();
			int stringColumnMaxLength = _Definition.GetStringColumnMaxLength(_columnName);
			if (_Definition.IsNonFixStringType(_columnName))
			{
				if (text.Length > stringColumnMaxLength)
				{
					text = text.Substring(0, stringColumnMaxLength);
					if (_AuditNote != "")
					{
						_AuditNote += ",";
					}
					string auditNote = _AuditNote;
					_AuditNote = auditNote + "[StringTruncated,ColumnName:'" + _columnName + "',OriginalValue:'" + _valuex.ToString() + "']";
				}
			}
			else if (_Definition.IsFixStringType(_columnName) && text.Length != stringColumnMaxLength)
			{
				if (text.Length > stringColumnMaxLength)
				{
					text = text.Substring(0, stringColumnMaxLength);
					if (_AuditNote != "")
					{
						_AuditNote += ",";
					}
					string auditNote = _AuditNote;
					_AuditNote = auditNote + "[StringTruncated,ColumnName:'" + _columnName + "',OriginalValue:'" + _valuex.ToString() + "']";
				}
				else
				{
					text = text.PadRight(stringColumnMaxLength, ' ');
					if (_AuditNote != "")
					{
						_AuditNote += ",";
					}
					string auditNote = _AuditNote;
					_AuditNote = auditNote + "[StringTruncated,ColumnName:'" + _columnName + "',OriginalValue:'" + _valuex.ToString() + "']";
				}
			}
			return text;
		}
	}
	public enum SqlServerType
	{
		MSSQL,
		MYSQL,
		ORACLE,
		OLEDB
	}
	public class DataAccessLayer
	{
		private string _ConnectionString = "";

		private IDbConnection _Connection = null;

		private SqlServerType _ConnectionType = SqlServerType.MSSQL;

		private IDbTransaction _Transaction = null;

		private static DataAccessTableManager _Manager;

		private static DataAccessAudit _Audit;

		public string ConnectionString => _ConnectionString;

		public IDbConnection Connection => _Connection;

		public SqlServerType ConnectionType => _ConnectionType;

		public IDbTransaction Transaction
		{
			get
			{
				return _Transaction;
			}
			set
			{
				_Transaction = value;
			}
		}

		public static DataAccessTableManager Manager
		{
			get
			{
				return _Manager;
			}
			set
			{
				_Manager = value;
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session[DataAccessStringAndDefault.ManagerSessionName] = value;
				}
			}
		}

		public static DataAccessAudit Audit
		{
			get
			{
				return _Audit;
			}
			set
			{
				_Audit = value;
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session[DataAccessStringAndDefault.AuditSessionName] = value;
				}
			}
		}

		public DataAccessTableManager GetDataAccessTableManager()
		{
			if (_Manager == null && HttpContext.Current != null && HttpContext.Current.Session[DataAccessStringAndDefault.ManagerSessionName] != null)
			{
				_Manager = (DataAccessTableManager)HttpContext.Current.Session[DataAccessStringAndDefault.ManagerSessionName];
			}
			if (_Manager == null)
			{
				_Manager = new DataAccessTableManager();
				_Manager.DALLayer = this;
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session[DataAccessStringAndDefault.ManagerSessionName] = _Manager;
				}
			}
			return _Manager;
		}

		public DataAccessAudit GetAudit()
		{
			if (_Audit == null && HttpContext.Current != null && HttpContext.Current.Session[DataAccessStringAndDefault.AuditSessionName] != null)
			{
				_Audit = (DataAccessAudit)HttpContext.Current.Session[DataAccessStringAndDefault.AuditSessionName];
			}
			if (_Audit == null)
			{
				_Audit = DataAccessStringAndDefault.GetDefaultAudit();
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Session[DataAccessStringAndDefault.AuditSessionName] = _Audit;
				}
			}
			return _Audit;
		}

		public DataAccessLayer(SqlServerType _connectionType, string _connectionString)
		{
			_ConnectionType = _connectionType;
			_ConnectionString = _connectionString;
			ResetConnection();
		}

		public void BeginTransaction()
		{
			if (_Transaction == null)
			{
				_Transaction = _Connection.BeginTransaction();
			}
		}

		public void CommitTransaction()
		{
			if (_Transaction != null)
			{
				_Transaction.Commit();
				_Transaction = null;
			}
		}

		public void RollbackTransaction()
		{
			if (_Transaction != null)
			{
				_Transaction.Rollback();
				_Transaction = null;
			}
		}

		public void OpenConnection()
		{
			if (_Connection != null)
			{
				_Connection.Open();
			}
		}

		public void OpenConnectionTryClose()
		{
			if (_Connection != null)
			{
				try
				{
					_Connection.Close();
				}
				catch
				{
				}
				_Connection.Open();
			}
		}

		public void CloseConnection()
		{
			if (_Connection != null)
			{
				Audit.FlushAudit(Manager);
				_Connection.Close();
			}
		}

		private void TerminateConnection()
		{
			if (_Connection == null || _Connection.State == ConnectionState.Closed)
			{
				return;
			}
			try
			{
				_Connection.Close();
			}
			catch
			{
			}
		}

		public void ResetConnection()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			if (_Connection != null)
			{
				TerminateConnection();
			}
			if (_ConnectionType == SqlServerType.MSSQL)
			{
				_Connection = (IDbConnection)new SqlConnection(_ConnectionString);
			}
			else if (_ConnectionType != SqlServerType.MYSQL)
			{
				if (_ConnectionType == SqlServerType.OLEDB)
				{
					_Connection = (IDbConnection)new OleDbConnection(_ConnectionString);
				}
				else if (_ConnectionType != SqlServerType.ORACLE)
				{
				}
			}
		}

		public void CheckConnection()
		{
			if (_Connection == null)
			{
				throw new Exception("Connection is not initialized");
			}
			if (_Connection.State != ConnectionState.Open)
			{
				throw new Exception("Connection is closed");
			}
		}

		public IDbCommand GetCommand()
		{
			CheckConnection();
			IDbCommand dbCommand = _Connection.CreateCommand();
			dbCommand.Transaction = _Transaction;
			return dbCommand;
		}

		public IDbCommand GetCommandText(string _text)
		{
			CheckConnection();
			IDbCommand dbCommand = _Connection.CreateCommand();
			dbCommand.CommandText = _text;
			dbCommand.CommandType = CommandType.Text;
			dbCommand.Transaction = _Transaction;
			return dbCommand;
		}

		public IDbDataAdapter GetDataAdapter(IDbCommand _com)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			IDbDataAdapter result = null;
			if (_ConnectionType == SqlServerType.MSSQL)
			{
				result = (IDbDataAdapter)new SqlDataAdapter((SqlCommand)_com);
			}
			else if (_ConnectionType != SqlServerType.MYSQL)
			{
				if (_ConnectionType == SqlServerType.OLEDB)
				{
					result = (IDbDataAdapter)new OleDbDataAdapter((OleDbCommand)_com);
				}
				else if (_ConnectionType != SqlServerType.ORACLE)
				{
				}
			}
			return result;
		}

		public IDbDataAdapter GetDataAdapter(string _text)
		{
			CheckConnection();
			IDbCommand commandText = GetCommandText(_text);
			return GetDataAdapter(commandText);
		}

		public IDbDataAdapter GetDataAdapterProcedure(string _text)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			CheckConnection();
			IDbCommand commandText = GetCommandText(_text);
			commandText.CommandType = CommandType.StoredProcedure;
			IDbDataAdapter result = null;
			if (_ConnectionType == SqlServerType.MSSQL)
			{
				result = (IDbDataAdapter)new SqlDataAdapter((SqlCommand)commandText);
			}
			else if (_ConnectionType != SqlServerType.MYSQL)
			{
				if (_ConnectionType == SqlServerType.OLEDB)
				{
					result = (IDbDataAdapter)new OleDbDataAdapter((OleDbCommand)commandText);
				}
				else if (_ConnectionType != SqlServerType.ORACLE)
				{
				}
			}
			return result;
		}

		public DataSet ExecuteDataSetProcedure(string _text)
		{
			DataSet dataSet = new DataSet();
			IDataAdapter dataAdapterProcedure = GetDataAdapterProcedure(_text);
			dataAdapterProcedure.Fill(dataSet);
			return dataSet;
		}

		public DataSet ExecuteDataSetText(string _text)
		{
			DataSet dataSet = new DataSet();
			IDataAdapter dataAdapter = GetDataAdapter(_text);
			dataAdapter.Fill(dataSet);
			return dataSet;
		}

		public int ExecuteNonQueryText(string _text)
		{
			IDbCommand commandText = GetCommandText(_text);
			int num = 0;
			num = commandText.ExecuteNonQuery();
			Audit.AddAuditForSessionUser(DataAccessStringAndDefault.DirectDALAccessUserName, commandText.CommandText, null);
			return num;
		}

		public IDataReader ExecuteDataReaderText(string _text)
		{
			IDbCommand commandText = GetCommandText(_text);
			return commandText.ExecuteReader();
		}
	}
	public class InstallationSQLEXPR : Form
	{
		protected string _ApplicationName = "";

		protected string _CompName = "";

		protected string _InstanceName = "";

		protected string _DatabaseName = "";

		protected string _OptionalInstallPassword = "";

		protected string _BackupFileName = "";

		protected BusinessSQLEXPRInstaller _Installer = null;

		public bool IsDone = false;

		public bool IsRestart = false;

		public Configuration _myconfig = ConfigurationManager.OpenExeConfiguration((ConfigurationUserLevel)0);

		private IContainer components = null;

		private TextBox txtPath;

		private FolderBrowserDialog folderBrowserDialog1;

		private Label label1;

		private Panel pnlFirst;

		private Button btnFolder;

		private Panel pnlSecond;

		private Label label2;

		private Panel panel2;

		private Button btnNext;

		private Button btnAttach;

		private PictureBox pictureBox1;

		private Label lblInstall;

		private Button button1;

		public InstallationSQLEXPR(string _appName, string _compName, string _instanceName, string _databaseName, string _optionalInstallPassword, string _backupFileName)
		{
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Invalid comparison between Unknown and I4
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			_ApplicationName = _appName;
			_CompName = _compName;
			_InstanceName = _instanceName;
			_DatabaseName = _databaseName;
			_OptionalInstallPassword = _optionalInstallPassword;
			_BackupFileName = _backupFileName;
			_Installer = new BusinessSQLEXPRInstaller(_CompName, _InstanceName, _DatabaseName);
			InitializeComponent();
			((Control)this).Text = "Welcome to " + _ApplicationName + " Configuration Wizard";
			((Control)lblInstall).Text = _ApplicationName + " Configuration";
			((Control)txtPath).Text = _Installer.SQLEXPRInstallPath;
			if (GetAppSettingKey("IsInstall") == "0" && GetAppSettingKey("IsRestart") == "0")
			{
				IsDone = true;
				IsRestart = false;
				((Form)this).Close();
			}
			else if (GetAppSettingKey("IsInstall") == "1" && _Installer.IsSQLEXPRInstanceExist())
			{
				DialogResult val = MessageBox.Show("Application has detected that an instance of this application is already exist. Do you want to delete it?", "Application Exist", (MessageBoxButtons)4, (MessageBoxIcon)32);
				if ((int)val == 6)
				{
					_Installer.Uninstall();
					SetAppSettingKey("IsInstall", "1");
					SetAppSettingKey("IsRestart", "0");
					_myconfig.Save();
					MessageBox.Show("Please restart your computer and re-run this application", "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
					IsDone = true;
					IsRestart = true;
					((Form)this).Close();
				}
				else
				{
					SetAppSettingKey("IsInstall", "0");
					_myconfig.Save();
				}
			}
		}

		public string GetAppSettingKey(string _keyname)
		{
			if (_myconfig.AppSettings.Settings[_keyname] == null)
			{
				return "";
			}
			return _myconfig.AppSettings.Settings[_keyname].Value;
		}

		public void SetAppSettingKey(string _keyname, string _value)
		{
			if (_myconfig.AppSettings.Settings[_keyname] == null)
			{
				_myconfig.AppSettings.Settings.Add(_keyname, _value);
			}
			else
			{
				_myconfig.AppSettings.Settings[_keyname].Value = _value;
			}
		}

		private void btnFolder_Click(object sender, EventArgs e)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			DialogResult val = ((CommonDialog)folderBrowserDialog1).ShowDialog();
			if ((int)val == 1)
			{
				((Control)txtPath).Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				try
				{
					if (Directory.Exists(((Control)txtPath).Text))
					{
						Directory.Delete(((Control)txtPath).Text, recursive: true);
					}
				}
				catch
				{
				}
				if (_Installer.Install(_OptionalInstallPassword, ((Control)txtPath).Text) == 0)
				{
					SetAppSettingKey("IsInstall", "0");
					SetAppSettingKey("IsRestart", "1");
					IsDone = true;
					IsRestart = true;
					_myconfig.Save();
				}
			}
			catch
			{
			}
			MessageBox.Show("Please restart your computer and re-run this application", "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
			((Form)this).Close();
			Application.Exit();
		}

		private void Installation_Load(object sender, EventArgs e)
		{
			if (GetAppSettingKey("IsInstall") == "1")
			{
				((Control)pnlFirst).Visible = true;
			}
			else
			{
				((Control)pnlSecond).Visible = true;
			}
		}

		private void btnAttach_Click(object sender, EventArgs e)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			if (GetAppSettingKey("IsRestart") == "1" || !_Installer.IsDatabaseExist())
			{
				try
				{
					_Installer.RestoreDatabase(_BackupFileName, "sa", _OptionalInstallPassword);
				}
				catch (Exception ex)
				{
					if (ex.Message.IndexOf(_BackupFileName) > 0)
					{
						MessageBox.Show("MDF File in MSDE2000\\" + _BackupFileName + " does not exist, please re-extract application file");
						Application.Exit();
						IsRestart = true;
						IsDone = true;
					}
					else
					{
						MessageBox.Show("Please restart your computer and re-run this application.\r\n" + ex.Message, "Restart", (MessageBoxButtons)0, (MessageBoxIcon)64);
						IsDone = true;
						IsRestart = true;
						Application.Exit();
						((Form)this).Close();
					}
					return;
				}
				SetAppSettingKey("IsRestart", "0");
				_myconfig.Save();
				IsRestart = false;
				MessageBox.Show("Application is successfully installed", "Success", (MessageBoxButtons)0, (MessageBoxIcon)64);
			}
			IsDone = true;
			((Form)this).Close();
		}

		public void BackupDatabase(string _dbname, string _backupname)
		{
			_Installer.BackupDatabase(_dbname, _backupname, "sa", _OptionalInstallPassword);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			((Form)this).Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			((Form)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Expected O, but got Unknown
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0473: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Expected O, but got Unknown
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Expected O, but got Unknown
			//IL_0556: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Expected O, but got Unknown
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0598: Expected O, but got Unknown
			//IL_0646: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Expected O, but got Unknown
			//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c9: Expected O, but got Unknown
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_08e8: Expected O, but got Unknown
			//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(InstallationSQLEXPR));
			txtPath = new TextBox();
			folderBrowserDialog1 = new FolderBrowserDialog();
			label1 = new Label();
			pnlFirst = new Panel();
			btnNext = new Button();
			btnFolder = new Button();
			pnlSecond = new Panel();
			button1 = new Button();
			btnAttach = new Button();
			label2 = new Label();
			panel2 = new Panel();
			lblInstall = new Label();
			pictureBox1 = new PictureBox();
			((Control)pnlFirst).SuspendLayout();
			((Control)pnlSecond).SuspendLayout();
			((Control)panel2).SuspendLayout();
			((ISupportInitialize)pictureBox1).BeginInit();
			((Control)this).SuspendLayout();
			((Control)txtPath).Location = new Point(128, 9);
			((Control)txtPath).Margin = new Padding(4);
			((Control)txtPath).Name = "txtPath";
			((TextBoxBase)txtPath).ReadOnly = true;
			((Control)txtPath).Size = new Size(291, 23);
			((Control)txtPath).TabIndex = 0;
			((Control)label1).AutoSize = true;
			((Control)label1).Location = new Point(4, 12);
			((Control)label1).Margin = new Padding(4, 0, 4, 0);
			((Control)label1).Name = "label1";
			((Control)label1).Size = new Size(116, 16);
			((Control)label1).TabIndex = 1;
			((Control)label1).Text = "Installation Path";
			((Control)pnlFirst).Controls.Add((Control)(object)btnNext);
			((Control)pnlFirst).Controls.Add((Control)(object)btnFolder);
			((Control)pnlFirst).Controls.Add((Control)(object)label1);
			((Control)pnlFirst).Controls.Add((Control)(object)txtPath);
			((Control)pnlFirst).Location = new Point(10, 150);
			((Control)pnlFirst).Name = "pnlFirst";
			((Control)pnlFirst).Size = new Size(461, 73);
			((Control)pnlFirst).TabIndex = 3;
			((Control)pnlFirst).Visible = false;
			((Control)btnNext).Anchor = (AnchorStyles)10;
			((Control)btnNext).BackgroundImage = (Image)(object)Resources.bar1;
			((Control)btnNext).BackgroundImageLayout = (ImageLayout)3;
			((ButtonBase)btnNext).FlatStyle = (FlatStyle)1;
			((Control)btnNext).Location = new Point(370, 35);
			((Control)btnNext).Name = "btnNext";
			((Control)btnNext).Size = new Size(88, 30);
			((Control)btnNext).TabIndex = 4;
			((Control)btnNext).Text = "&Next";
			((ButtonBase)btnNext).UseVisualStyleBackColor = true;
			((Control)btnNext).Click += btnNext_Click;
			((Control)btnFolder).Location = new Point(426, 9);
			((Control)btnFolder).Name = "btnFolder";
			((Control)btnFolder).Size = new Size(32, 24);
			((Control)btnFolder).TabIndex = 3;
			((Control)btnFolder).Text = "...";
			((ButtonBase)btnFolder).UseVisualStyleBackColor = true;
			((Control)btnFolder).Click += btnFolder_Click;
			((Control)pnlSecond).BackColor = Color.Transparent;
			((Control)pnlSecond).Controls.Add((Control)(object)button1);
			((Control)pnlSecond).Controls.Add((Control)(object)btnAttach);
			((Control)pnlSecond).Controls.Add((Control)(object)label2);
			((Control)pnlSecond).Location = new Point(10, 136);
			((Control)pnlSecond).Name = "pnlSecond";
			((Control)pnlSecond).Size = new Size(461, 90);
			((Control)pnlSecond).TabIndex = 4;
			((Control)button1).Anchor = (AnchorStyles)10;
			((Control)button1).BackgroundImage = (Image)componentResourceManager.GetObject("button1.BackgroundImage");
			((Control)button1).BackgroundImageLayout = (ImageLayout)3;
			((ButtonBase)button1).FlatStyle = (FlatStyle)1;
			((Control)button1).Font = new Font("Verdana", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Control)button1).Location = new Point(351, 51);
			((Control)button1).Name = "button1";
			((Control)button1).Size = new Size(88, 30);
			((Control)button1).TabIndex = 6;
			((Control)button1).Text = "&Cancel";
			((ButtonBase)button1).UseVisualStyleBackColor = true;
			((Control)button1).Click += button1_Click;
			((Control)btnAttach).Anchor = (AnchorStyles)10;
			((Control)btnAttach).BackgroundImage = (Image)componentResourceManager.GetObject("btnAttach.BackgroundImage");
			((Control)btnAttach).BackgroundImageLayout = (ImageLayout)3;
			((ButtonBase)btnAttach).FlatStyle = (FlatStyle)1;
			((Control)btnAttach).Font = new Font("Verdana", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Control)btnAttach).Location = new Point(239, 51);
			((Control)btnAttach).Name = "btnAttach";
			((Control)btnAttach).Size = new Size(88, 30);
			((Control)btnAttach).TabIndex = 5;
			((Control)btnAttach).Text = "&Next";
			((ButtonBase)btnAttach).UseVisualStyleBackColor = true;
			((Control)btnAttach).Click += btnAttach_Click;
			((Control)label2).AutoSize = true;
			((Control)label2).Location = new Point(16, 26);
			((Control)label2).Margin = new Padding(4, 0, 4, 0);
			((Control)label2).Name = "label2";
			((Control)label2).Size = new Size(426, 16);
			((Control)label2).TabIndex = 2;
			((Control)label2).Text = "Application is now configuring database. Click Next to continue";
			((Control)panel2).BackColor = Color.Transparent;
			((Control)panel2).Controls.Add((Control)(object)lblInstall);
			((Control)panel2).Controls.Add((Control)(object)pictureBox1);
			((Control)panel2).Location = new Point(-2, -1);
			((Control)panel2).Name = "panel2";
			((Control)panel2).Size = new Size(484, 145);
			((Control)panel2).TabIndex = 5;
			((Control)lblInstall).Font = new Font("Verdana", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)lblInstall).Location = new Point(182, 13);
			((Control)lblInstall).Name = "lblInstall";
			((Control)lblInstall).Size = new Size(292, 74);
			((Control)lblInstall).TabIndex = 1;
			((Control)lblInstall).Text = "Food Installation";
			lblInstall.TextAlign = (ContentAlignment)2;
			pictureBox1.Image = (Image)(object)Resources.Madhushala_Background;
			pictureBox1.InitialImage = null;
			((Control)pictureBox1).Location = new Point(14, 8);
			((Control)pictureBox1).Name = "pictureBox1";
			((Control)pictureBox1).Size = new Size(162, 131);
			pictureBox1.SizeMode = (PictureBoxSizeMode)1;
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			((Control)this).BackgroundImage = (Image)(object)Resources.Bar;
			((Control)this).BackgroundImageLayout = (ImageLayout)3;
			((Form)this).ClientSize = new Size(481, 231);
			((Control)this).Controls.Add((Control)(object)panel2);
			((Control)this).Controls.Add((Control)(object)pnlSecond);
			((Control)this).Controls.Add((Control)(object)pnlFirst);
			((Control)this).Font = new Font("Verdana", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
			((Form)this).FormBorderStyle = (FormBorderStyle)1;
			((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			((Form)this).Margin = new Padding(4);
			((Form)this).MaximizeBox = false;
			((Form)this).MinimizeBox = false;
			((Control)this).Name = "InstallationSQLEXPR";
			((Form)this).StartPosition = (FormStartPosition)1;
			((Control)this).Text = "Welcome to ";
			((Form)this).Load += Installation_Load;
			((Control)pnlFirst).ResumeLayout(false);
			((Control)pnlFirst).PerformLayout();
			((Control)pnlSecond).ResumeLayout(false);
			((Control)pnlSecond).PerformLayout();
			((Control)panel2).ResumeLayout(false);
			((ISupportInitialize)pictureBox1).EndInit();
			((Control)this).ResumeLayout(false);
		}
	}
	[Serializable]
	public class DataAccessTableManager
	{
		private DataAccessLayer _DALLayer = null;

		private IDictionary<string, DataAccessTableDefinition> _TableDefinition = new Dictionary<string, DataAccessTableDefinition>();

		public DataAccessLayer DALLayer
		{
			get
			{
				return _DALLayer;
			}
			set
			{
				_DALLayer = value;
			}
		}

		public DataAccessTableDefinition this[string _tableName]
		{
			get
			{
				if (_TableDefinition.ContainsKey(_tableName.ToLower()))
				{
					return _TableDefinition[_tableName.ToLower()];
				}
				DataAccessTableDefinition dataAccessTableDefinition = new DataAccessTableDefinition(_DALLayer, _tableName.ToLower());
				_TableDefinition.Add(_tableName.ToLower(), dataAccessTableDefinition);
				return dataAccessTableDefinition;
			}
		}

		public static long LongParse(object _obj)
		{
			long result = 0L;
			try
			{
				result = long.Parse(_obj.ToString());
			}
			catch
			{
			}
			return result;
		}

		public static int IntParse(object _obj)
		{
			int result = 0;
			try
			{
				result = int.Parse(_obj.ToString());
			}
			catch
			{
			}
			return result;
		}
	}
	public class DataAccessStringAndDefault
	{
		public static string DateTimeFormat => "MM/dd/yyyy";

		public static string NullString => "null";

		public static string DirectDALAccessUserName => "[Unknown:DirectDAL]";

		public static string ManagerSessionName => "ABMSQLManager";

		public static string AuditSessionName => "ABMSQLAudit";

		public static string CurrenUserSessionName => "CurrentUser";

		public static DataAccessAudit GetDefaultAudit()
		{
			if (HttpContext.Current != null)
			{
				return new DataAccessAudit("AuditTrail", "UserLogin", "TableName", "Statement", "Note", "~\\AuditTrail.txt");
			}
			return new DataAccessAudit("AuditTrail", "UserLogin", "TableName", "Statement", "Note", "AuditTrail.txt");
		}
	}
	public class DataAccessAudit
	{
		private string _Filename = "";

		private string _TableName = "";

		private string _AuditColumnTable = "";

		private string _AuditColumnStatement = "";

		private string _AuditColumnUser = "";

		private string _AutitColumnNote = "";

		public IList<AuditEntity> _AuditList = new List<AuditEntity>();

		private DataAccessTableManager _AutoFlushManager = null;

		public DataAccessTableManager AutoFlushManager
		{
			get
			{
				return _AutoFlushManager;
			}
			set
			{
				_AutoFlushManager = value;
			}
		}

		public DataAccessAudit(string _tableName, string _auditcolumnUser, string _auditcolumntable, string _auditcolumnstatement, string _auditcolumnnote, string _filename)
		{
			if (HttpContext.Current != null)
			{
				_Filename = HttpContext.Current.Server.MapPath(_filename);
			}
			else
			{
				_Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _filename);
			}
			_TableName = _tableName;
			_AuditColumnTable = _auditcolumntable;
			_AuditColumnStatement = _auditcolumnstatement;
			_AuditColumnUser = _auditcolumnUser;
			_AutitColumnNote = _auditcolumnnote;
		}

		public static string GetCommandAuditStatement(IDbCommand _com)
		{
			string text = "'" + _com.CommandText + "'";
			string text2 = "   " + GetParametersInfo(_com);
			if (text2.Trim() != "")
			{
				return text + "\r\n" + text2;
			}
			return text;
		}

		private static string GetParametersInfo(IDbCommand _com)
		{
			string text = "";
			foreach (IDbDataParameter parameter in _com.Parameters)
			{
				if (text != "")
				{
					text += ",";
				}
				text = text + parameter.ParameterName + "=" + FormatObjectToSQL(parameter.Value);
			}
			return text;
		}

		private static string FormatObjectToSQL(object _obj)
		{
			string text = "";
			if (_obj == null)
			{
				return DataAccessStringAndDefault.NullString;
			}
			if (_obj is string)
			{
				return "'" + _obj.ToString() + "'";
			}
			if (_obj is DateTime)
			{
				return "'" + ((DateTime)_obj).ToString(DataAccessStringAndDefault.DateTimeFormat) + "'";
			}
			if (_obj is byte)
			{
				return "new byte[]{" + (byte)_obj + "}";
			}
			if (_obj is byte[])
			{
				return "new byte[]{" + GetByteString((byte[])_obj) + "}";
			}
			if (_obj is int)
			{
				return _obj.ToString();
			}
			if (_obj is long)
			{
				return _obj.ToString();
			}
			if (_obj is decimal)
			{
				return _obj.ToString();
			}
			if (_obj is float)
			{
				return _obj.ToString();
			}
			return "'" + _obj.ToString() + "'";
		}

		private static string GetByteString(byte[] _byte)
		{
			string text = "";
			foreach (byte b in _byte)
			{
				if (text != "")
				{
					text += ",";
				}
				text += b;
			}
			return text;
		}

		public void AddAuditForSessionUser(string _table, string _statement, string _note)
		{
			string user = "";
			if (HttpContext.Current != null && HttpContext.Current.Session["CurrentUser"] != null)
			{
				user = HttpContext.Current.Session["CurrentUser"].ToString();
			}
			AuditEntity item = new AuditEntity(user, _table, _statement, _note);
			_AuditList.Add(item);
			if (_AutoFlushManager != null)
			{
				FlushAudit(_AutoFlushManager);
			}
		}

		public void AddAudit(string _user, string _table, string _statement, string _note)
		{
			AuditEntity item = new AuditEntity(_user, _table, _statement, _note);
			_AuditList.Add(item);
			if (_AutoFlushManager != null)
			{
				FlushAudit(_AutoFlushManager);
			}
		}

		public void ClearAllAudit(DataAccessTableManager _manager)
		{
			if (_Filename != "" && File.Exists(_Filename))
			{
				File.Delete(_Filename);
			}
			if (_TableName != "" && _manager != null && _manager[_TableName] != null)
			{
				DataAccessTableFiller filler = _manager[_TableName].GetFiller();
				IDbCommand deleteStatementForAll = filler.GetDeleteStatementForAll();
				_manager[_TableName].ExecuteNonQueryDeleteFromCommand(filler);
			}
		}

		public int FlushAudit(DataAccessTableManager _manager)
		{
			if (_AuditList.Count <= 0)
			{
				return 0;
			}
			AuditEntity[] array = new AuditEntity[_AuditList.Count];
			_AuditList.CopyTo(array, 0);
			if (_Filename != "")
			{
				if (!File.Exists(_Filename))
				{
					FileStream fileStream = File.Create(_Filename);
					fileStream.Close();
				}
				StreamWriter streamWriter = new StreamWriter(_Filename, append: true);
				string text = "";
				foreach (AuditEntity audit in _AuditList)
				{
					text = audit.GetAuditRecordForFile();
					streamWriter.WriteLine(text);
				}
				streamWriter.Flush();
				streamWriter.Close();
			}
			if (_TableName != "" && _manager != null && _manager[_TableName] != null)
			{
				AuditEntity[] array2 = array;
				foreach (AuditEntity current in array2)
				{
					DataAccessTableFiller filler = _manager[_TableName].GetFiller();
					filler.AddColumnValue(_AuditColumnUser, current.User);
					filler.AddColumnValue(_AuditColumnTable, current.Table);
					filler.AddColumnValue(_AuditColumnStatement, current.Statement);
					filler.AddColumnValue(_AutitColumnNote, current.Note);
					IDbCommand insertStatementFilterIdentity = filler.GetInsertStatementFilterIdentity();
					_manager[_TableName].ExecuteNonQueryInsertIdentityValueFromCommandWithoutAudit(filler);
				}
			}
			_AuditList.Clear();
			return _AuditList.Count;
		}
	}
	public class AuditEntity
	{
		private string _User = "";

		private string _Table = "";

		private string _Statement = "";

		private string _Note = "";

		public string User
		{
			get
			{
				return _User;
			}
			set
			{
				_User = value;
			}
		}

		public string Table
		{
			get
			{
				return _Table;
			}
			set
			{
				_Table = value;
			}
		}

		public string Statement
		{
			get
			{
				return _Statement;
			}
			set
			{
				_Statement = value;
			}
		}

		public string Note
		{
			get
			{
				return _Note;
			}
			set
			{
				_Note = value;
			}
		}

		public AuditEntity(string _user, string _table, string _statement, string _note)
		{
			_User = _user;
			_Table = _table;
			_Statement = _statement;
			_Note = _note;
		}

		public string GetAuditRecordForFile()
		{
			string text = "";
			if (_Note != "")
			{
				text = "-  User = '{0}' : Table = '{1}' : Statement = {2}\r\n   {3}";
				return string.Format(text, _User, _Table, _Statement, _Note);
			}
			text = "-  User = '{0}' : Table = '{1}' : Statement = {2}";
			return string.Format(text, _User, _Table, _Statement);
		}
	}
	public class DataAccessTableDefinition
	{
		private DataAccessLayer _DALLayer = null;

		private string _TableName = "";

		private DataTable _ColumnDefinition = null;

		private List<string> _PrimaryKeyColumns = new List<string>();

		private List<string> _IdentityColumns = new List<string>();

		private List<string> _NullAbleColumns = new List<string>();

		public DataAccessLayer DALLayer => _DALLayer;

		public string TableName => _TableName;

		public DataTable ColumnDefinition => _ColumnDefinition;

		public List<string> PrimaryKeyColumns => _PrimaryKeyColumns;

		public List<string> IdentityColumns => _IdentityColumns;

		public List<string> NullAbleColumns => _NullAbleColumns;

		public DataAccessTableDefinition(DataAccessLayer _dALLayer, string _tableName)
		{
			_DALLayer = _dALLayer;
			_TableName = _tableName;
			GetColumnDefinition();
		}

		private void GetColumnDefinition()
		{
			string text = "";
			if (_DALLayer.ConnectionType == SqlServerType.MSSQL)
			{
				text = "select a.COLUMN_NAME, a.DATA_TYPE, a.CHARACTER_MAXIMUM_LENGTH, b.IsPrimaryKey, a.IsIdentity, CASE WHEN a.IS_NULLABLE = 'YES' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNullAble\r\n                            FROM\r\n                            (\r\n                                select *, COLUMNPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), COLUMN_NAME, 'IsIdentity') AS IsIdentity from information_schema.columns where table_name = '{0}'\r\n                            ) a\r\n                            LEFT JOIN\r\n                            (\r\n                                select a.column_name, CAST(1 AS BIT) as ISPrimaryKey from information_schema.key_column_usage a\r\n                                    left join information_schema.table_constraints b\r\n                                    on a.constraint_name = b.constraint_name AND a.table_name = b.table_name\r\n                                    where a.table_name = '{0}' and b.constraint_type = 'PRIMARY KEY'\r\n                            ) b\r\n                            on a.column_name = b.column_name\r\n                            ORDER BY a.Ordinal_Position";
				text = string.Format(text, _TableName);
				_ColumnDefinition = _DALLayer.ExecuteDataSetText(text).Tables[0];
			}
			else if (_DALLayer.ConnectionType == SqlServerType.MYSQL)
			{
				text = "select a.COLUMN_NAME, a.DATA_TYPE, a.CHARACTER_MAXIMUM_LENGTH, b.IsPrimaryKey, a.IsIdentity, CASE WHEN a.IS_NULLABLE = 'YES' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNullAble\r\n                            FROM\r\n                            (\r\n                                select *, COLUMNPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), COLUMN_NAME, 'IsIdentity') AS IsIdentity from information_schema.columns where table_name = '{0}'\r\n                            ) a\r\n                            LEFT JOIN\r\n                            (\r\n                                select a.column_name, CAST(1 AS BIT) as ISPrimaryKey from information_schema.key_column_usage a\r\n                                    left join information_schema.table_constraints b\r\n                                    on a.constraint_name = b.constraint_name AND a.table_name = b.table_name\r\n                                    where a.table_name = '{0}' and b.constraint_type = 'PRIMARY KEY'\r\n                            ) b\r\n                            on a.column_name = b.column_name\r\n                            ORDER BY a.Ordinal_Position";
				text = string.Format(text, _TableName);
				_ColumnDefinition = _DALLayer.ExecuteDataSetText(text).Tables[0];
			}
			else if (_DALLayer.ConnectionType == SqlServerType.OLEDB)
			{
				text = "select a.COLUMN_NAME, a.DATA_TYPE, a.CHARACTER_MAXIMUM_LENGTH, b.IsPrimaryKey, a.IsIdentity, CASE WHEN a.IS_NULLABLE = 'YES' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsNullAble\r\n                            FROM\r\n                            (\r\n                                select *, COLUMNPROPERTY(OBJECT_ID(QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)), COLUMN_NAME, 'IsIdentity') AS IsIdentity from information_schema.columns where table_name = '{0}'\r\n                            ) a\r\n                            LEFT JOIN\r\n                            (\r\n                                select a.column_name, CAST(1 AS BIT) as ISPrimaryKey from information_schema.key_column_usage a\r\n                                    left join information_schema.table_constraints b\r\n                                    on a.constraint_name = b.constraint_name AND a.table_name = b.table_name\r\n                                    where a.table_name = '{0}' and b.constraint_type = 'PRIMARY KEY'\r\n                            ) b\r\n                            on a.column_name = b.column_name\r\n                            ORDER BY a.Ordinal_Position";
				text = string.Format(text, _TableName);
				_ColumnDefinition = _DALLayer.ExecuteDataSetText(text).Tables[0];
			}
			else if (_DALLayer.ConnectionType != SqlServerType.ORACLE)
			{
			}
			_PrimaryKeyColumns.Clear();
			foreach (DataRow row in _ColumnDefinition.Rows)
			{
				if (row["IsPrimaryKey"].ToString() == "1" || row["IsPrimaryKey"].ToString().ToLower() == "true")
				{
					_PrimaryKeyColumns.Add(row["Column_Name"].ToString().ToLower());
				}
				if (row["IsIdentity"].ToString() == "1" || row["IsIdentity"].ToString().ToLower() == "true")
				{
					_IdentityColumns.Add(row["Column_Name"].ToString().ToLower());
				}
				if (row["IsNullAble"].ToString() == "1" || row["IsNullAble"].ToString().ToLower() == "true")
				{
					_NullAbleColumns.Add(row["Column_Name"].ToString().ToLower());
				}
			}
		}

		private bool IsColumnPrimaryKey(string _columnName)
		{
			DataRow[] array = _ColumnDefinition.Select("COLUMN_NAME = '" + _columnName + "'");
			if (array == null || array.Length <= 0)
			{
				return false;
			}
			if (array[0]["IsPrimaryKey"].ToString() == "1")
			{
				return true;
			}
			return false;
		}

		private bool IsColumnIdentity(string _columnName)
		{
			DataRow[] array = _ColumnDefinition.Select("COLUMN_NAME = '" + _columnName + "'");
			if (array == null || array.Length <= 0)
			{
				return false;
			}
			if (array[0]["IsIdentity"].ToString() == "1")
			{
				return true;
			}
			return false;
		}

		private bool IsColumnExist(string _columnName)
		{
			DataRow[] array = _ColumnDefinition.Select("COLUMN_NAME = '" + _columnName + "'");
			if (array == null || array.Length <= 0)
			{
				return false;
			}
			return true;
		}

		public int GetStringColumnMaxLength(string _columnName)
		{
			DataRow[] array = _ColumnDefinition.Select("COLUMN_NAME = '" + _columnName + "'");
			if (array == null || array.Length <= 0)
			{
				return 0;
			}
			return DataAccessTableManager.IntParse(array[0]["CHARACTER_MAXIMUM_LENGTH"]);
		}

		public DataAccessTableFiller GetFiller()
		{
			return new DataAccessTableFiller(this);
		}

		public IDbCommand GetDataAll(string _sort)
		{
			IDbCommand command = _DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT * FROM " + _TableName;
			if (_sort != null && _sort != "")
			{
				command.CommandText = " ORDER BY " + _sort;
			}
			return command;
		}

		public IDbCommand GetDataByWhere(IList<string> _selectColumn, string _where, IDbDataParameter[] _params, string _sort)
		{
			IDbCommand command = _DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string text = "*";
			if (_selectColumn != null && _selectColumn.Count > 0)
			{
				text = "";
				foreach (string item in _selectColumn)
				{
					if (text != "")
					{
						text += ",";
					}
					text += item;
				}
			}
			string text2 = "";
			if (_where == "")
			{
				text2 = "SELECT {0} FROM {1}";
				text2 = string.Format(text2, text, _TableName);
			}
			else
			{
				text2 = "SELECT {0} FROM {1} WHERE {2}";
				text2 = string.Format(text2, text, _TableName, _where);
			}
			if (_sort != null && _sort != "")
			{
				text2 = text2 + " ORDER BY " + _sort;
			}
			command.CommandText = text2;
			foreach (IDbDataParameter value in _params)
			{
				command.Parameters.Add(value);
			}
			return command;
		}

		public IDbCommand GetDataBy(IList<string> _selectColumn, IList<KeyValuePair<string, object>> _pairFilter, string _sort)
		{
			IDbCommand command = _DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			string text = "*";
			if (_selectColumn != null && _selectColumn.Count > 0)
			{
				text = "";
				foreach (string item in _selectColumn)
				{
					if (text != "")
					{
						text += ",";
					}
					text += item;
				}
			}
			string format = "SELECT {0} FROM {1} ";
			format = string.Format(format, text, _TableName);
			string text2 = " WHERE ";
			string text3 = "";
			int num = 1;
			foreach (KeyValuePair<string, object> item2 in _pairFilter)
			{
				if (text3 != "")
				{
					text3 += " AND ";
				}
				text3 = text3 + item2.Key + "=@Param" + num;
				IDbDataParameter dbDataParameter = command.CreateParameter();
				dbDataParameter.ParameterName = "@Param" + num;
				dbDataParameter.Value = item2.Value;
				command.Parameters.Add(dbDataParameter);
				num++;
			}
			if (text3 != "")
			{
				command.CommandText = format + text2 + text3;
			}
			else
			{
				command.CommandText = format;
			}
			if (_sort != null && _sort != "")
			{
				command.CommandText = command.CommandText + " ORDER BY " + _sort;
			}
			return command;
		}

		public IDbCommand GetDataByManual(string _sql, IDbDataParameter[] _params)
		{
			IDbCommand command = _DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = _sql;
			if (_params != null)
			{
				foreach (IDbDataParameter value in _params)
				{
					command.Parameters.Add(value);
				}
			}
			return command;
		}

		public DataSet ExecuteDataSetFromCommand(IDbCommand _com)
		{
			DataSet dataSet = new DataSet();
			IDataAdapter dataAdapter = _DALLayer.GetDataAdapter(_com);
			dataAdapter.Fill(dataSet);
			return dataSet;
		}

		public DataTable ExecuteDataTableFromCommand(IDbCommand _com)
		{
			DataTable result = null;
			DataSet dataSet = new DataSet();
			IDataAdapter dataAdapter = _DALLayer.GetDataAdapter(_com);
			dataAdapter.Fill(dataSet);
			if (dataSet.Tables.Count > 0)
			{
				result = dataSet.Tables[0];
			}
			return result;
		}

		public IDataReader ExecuteReaderFromCommand(IDbCommand _com)
		{
			return _com.ExecuteReader();
		}

		public long ExecuteNonQueryInsertIdentityValueFromCommandWithoutAudit(DataAccessTableFiller _filler)
		{
			object obj = _filler.LastCommand.ExecuteScalar();
			return DataAccessTableManager.LongParse(obj);
		}

		public long ExecuteNonQueryInsertIdentityValueFromCommand(DataAccessTableFiller _filler)
		{
			object obj = _filler.LastCommand.ExecuteScalar();
			long result = DataAccessTableManager.LongParse(obj);
			DataAccessLayer.Audit.AddAuditForSessionUser(_TableName, DataAccessAudit.GetCommandAuditStatement(_filler.LastCommand), _filler.AuditNote);
			if (_filler.Definition.PrimaryKeyColumns.Count == 1 && obj != null)
			{
				_filler.AddColumnValue(_filler.Definition.PrimaryKeyColumns[0], obj);
				if (_filler.RowData != null)
				{
					_filler.RowData[_filler.Definition.PrimaryKeyColumns[0]] = obj;
					_filler.RowData.AcceptChanges();
				}
			}
			return result;
		}

		public int ExecuteNonQueryInsertFromCommand(DataAccessTableFiller _filler)
		{
			int result = _filler.LastCommand.ExecuteNonQuery();
			DataAccessLayer.Audit.AddAuditForSessionUser(_TableName, DataAccessAudit.GetCommandAuditStatement(_filler.LastCommand), _filler.AuditNote);
			return result;
		}

		public int ExecuteNonQueryUpdateFromCommand(DataAccessTableFiller _filler)
		{
			int result = _filler.LastCommand.ExecuteNonQuery();
			DataAccessLayer.Audit.AddAuditForSessionUser(_TableName, DataAccessAudit.GetCommandAuditStatement(_filler.LastCommand), _filler.AuditNote);
			return result;
		}

		public int ExecuteNonQueryDeleteFromCommand(DataAccessTableFiller _filler)
		{
			int result = _filler.LastCommand.ExecuteNonQuery();
			DataAccessLayer.Audit.AddAuditForSessionUser(_TableName, DataAccessAudit.GetCommandAuditStatement(_filler.LastCommand), _filler.AuditNote);
			return result;
		}

		public int ExecuteNonQueryDeleteFromWhere(string _sql, IDbDataParameter[] _params)
		{
			string format = "DELETE FROM {0} WHERE " + _sql;
			format = string.Format(format, _TableName);
			IDbCommand command = _DALLayer.GetCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = format;
			if (_params != null)
			{
				foreach (IDbDataParameter value in _params)
				{
					command.Parameters.Add(value);
				}
			}
			int result = command.ExecuteNonQuery();
			DataAccessLayer.Audit.AddAuditForSessionUser(_TableName, DataAccessAudit.GetCommandAuditStatement(command), "");
			return result;
		}

		public string GetTypeName(string _typename)
		{
			if (_ColumnDefinition == null || _ColumnDefinition.Rows.Count <= 0)
			{
				return "";
			}
			DataRow[] array = _ColumnDefinition.Select("Column_Name='" + _typename + "'");
			if (array == null || array.Length <= 0)
			{
				return "";
			}
			return array[0]["Data_Type"].ToString();
		}

		public bool IsDateTimeType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			switch (typeName)
			{
			default:
				if (!(typeName == "date"))
				{
					return false;
				}
				goto case "datetime";
			case "datetime":
			case "date":
			case "smalldatetime":
				return true;
			}
		}

		public bool IsFixStringType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "varchar" || typeName == "char")
			{
				return true;
			}
			return false;
		}

		public bool IsNonFixStringType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "nvarchar" || typeName == "nchar")
			{
				return true;
			}
			return false;
		}

		public bool IsStringType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			switch (typeName)
			{
			default:
				if (!(typeName == "nchar"))
				{
					return false;
				}
				goto case "varchar";
			case "varchar":
			case "nvarchar":
			case "char":
				return true;
			}
		}

		public bool IsImageType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "image" || typeName == "byte")
			{
				return true;
			}
			return false;
		}

		public bool IsValueType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			switch (typeName)
			{
			default:
				if (!(typeName == "float"))
				{
					return false;
				}
				goto case "int";
			case "int":
			case "bigint":
			case "tinyint":
			case "smallint":
			case "decimal":
				return true;
			}
		}

		public bool IsIntValueType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "int")
			{
				return true;
			}
			return false;
		}

		public bool IsBigIntValueType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "bigint")
			{
				return true;
			}
			return false;
		}

		public bool IsDecimalValueType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "decimal")
			{
				return true;
			}
			return false;
		}

		public bool IsShortValueType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "tinyint" || typeName == "smallint")
			{
				return true;
			}
			return false;
		}

		public bool IsBitType(string _columnName)
		{
			string typeName = GetTypeName(_columnName);
			typeName = typeName.ToLower();
			if (typeName == "bit")
			{
				return true;
			}
			return false;
		}

		public object GetDefaultValueFor(string _columnName)
		{
			if (IsValueType(_columnName))
			{
				return 0;
			}
			if (IsImageType(_columnName))
			{
				return new byte[1];
			}
			if (IsNonFixStringType(_columnName))
			{
				return "";
			}
			if (IsFixStringType(_columnName))
			{
				return "".PadLeft(GetStringColumnMaxLength(_columnName), ' ');
			}
			if (IsDateTimeType(_columnName))
			{
				return DateTime.MinValue;
			}
			if (IsBitType(_columnName))
			{
				return false;
			}
			return 0;
		}
	}
}
namespace ABMSQL.DataAccess.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
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
					ResourceManager resourceManager = new ResourceManager("ABMSQL.DataAccess.Properties.Resources", typeof(Resources).Assembly);
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

		internal static Bitmap Bar
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected O, but got Unknown
				object @object = ResourceManager.GetObject("Bar", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap bar1
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected O, but got Unknown
				object @object = ResourceManager.GetObject("bar1", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap bxp58369
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected O, but got Unknown
				object @object = ResourceManager.GetObject("bxp58369", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal static Bitmap Madhushala_Background
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected O, but got Unknown
				object @object = ResourceManager.GetObject("Madhushala_Background", resourceCulture);
				return (Bitmap)@object;
			}
		}

		internal Resources()
		{
		}
	}
}

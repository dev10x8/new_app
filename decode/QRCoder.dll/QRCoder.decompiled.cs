using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using QRCoder.Framework4._0Methods;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyTitle("QRCoder")]
[assembly: AssemblyDescription("A free-to-use QR code library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("www.code-bude.net")]
[assembly: AssemblyProduct("QRCoder")]
[assembly: AssemblyCopyright("Free to use (MIT license)")]
[assembly: AssemblyTrademark("written by Raffael Herrmann")]
[assembly: ComVisible(true)]
[assembly: Guid("e668b98b-83bb-4e60-b33c-4fd5ed9c0156")]
[assembly: AssemblyFileVersion("1.3.5.0")]
[assembly: AssemblyInformationalVersion("1.3.5-CI00008")]
[assembly: AssemblyVersion("1.3.5.0")]
namespace QRCoder
{
	public abstract class AbstractQRCode
	{
		protected QRCodeData QrCodeData { get; set; }

		protected AbstractQRCode()
		{
		}

		protected AbstractQRCode(QRCodeData data)
		{
			QrCodeData = data;
		}

		public virtual void SetQRCodeData(QRCodeData data)
		{
			QrCodeData = data;
		}

		public void Dispose()
		{
			QrCodeData = null;
		}
	}
	public class AsciiQRCode : AbstractQRCode, IDisposable
	{
		public AsciiQRCode()
		{
		}

		public AsciiQRCode(QRCodeData data)
			: base(data)
		{
		}

		public string GetGraphic(int repeatPerModule)
		{
			return string.Join("\n", GetLineByLineGraphic(repeatPerModule));
		}

		public string GetGraphic(int repeatPerModule, string darkColorString, string whiteSpaceString, string endOfLine = "\n")
		{
			return string.Join(endOfLine, GetLineByLineGraphic(repeatPerModule, darkColorString, whiteSpaceString));
		}

		public string[] GetLineByLineGraphic(int repeatPerModule)
		{
			return GetLineByLineGraphic(repeatPerModule, "██", "  ");
		}

		public string[] GetLineByLineGraphic(int repeatPerModule, string darkColorString, string whiteSpaceString)
		{
			List<string> list = new List<string>();
			int num = ((darkColorString.Length / 2 != 1) ? (darkColorString.Length / 2) : 0);
			int num2 = repeatPerModule + num;
			int num3 = base.QrCodeData.ModuleMatrix.Count * num2;
			for (int i = 0; i < num3; i++)
			{
				bool flag = true;
				StringBuilder stringBuilder = new StringBuilder();
				for (int j = 0; j < base.QrCodeData.ModuleMatrix.Count; j++)
				{
					bool flag2 = base.QrCodeData.ModuleMatrix[j][(i + num2) / num2 - 1];
					for (int k = 0; k < repeatPerModule; k++)
					{
						stringBuilder.Append(flag2 ? darkColorString : whiteSpaceString);
					}
					if (flag2)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					list.Add(stringBuilder.ToString());
				}
			}
			return list.ToArray();
		}
	}
	public class Base64QRCode : AbstractQRCode, IDisposable
	{
		public enum ImageType
		{
			Gif,
			Jpeg,
			Png
		}

		private QRCode qr;

		public Base64QRCode()
		{
			qr = new QRCode();
		}

		public Base64QRCode(QRCodeData data)
			: base(data)
		{
			qr = new QRCode(data);
		}

		public override void SetQRCodeData(QRCodeData data)
		{
			qr.SetQRCodeData(data);
		}

		public string GetGraphic(int pixelsPerModule)
		{
			return GetGraphic(pixelsPerModule, Color.Black, Color.White);
		}

		public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
		{
			return GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);
		}

		public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
		{
			string empty = string.Empty;
			Bitmap graphic = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones);
			try
			{
				return BitmapToBase64(graphic, imgType);
			}
			finally
			{
				((IDisposable)graphic)?.Dispose();
			}
		}

		public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
		{
			string empty = string.Empty;
			Bitmap graphic = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones);
			try
			{
				return BitmapToBase64(graphic, imgType);
			}
			finally
			{
				((IDisposable)graphic)?.Dispose();
			}
		}

		private string BitmapToBase64(Bitmap bmp, ImageType imgType)
		{
			string empty = string.Empty;
			ImageFormat val = (ImageFormat)(imgType switch
			{
				ImageType.Png => ImageFormat.Png, 
				ImageType.Jpeg => ImageFormat.Jpeg, 
				ImageType.Gif => ImageFormat.Gif, 
				_ => ImageFormat.Png, 
			});
			using MemoryStream memoryStream = new MemoryStream();
			((Image)bmp).Save((Stream)memoryStream, val);
			return Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
		}
	}
	public class BitmapByteQRCode : AbstractQRCode, IDisposable
	{
		public BitmapByteQRCode()
		{
		}

		public BitmapByteQRCode(QRCodeData data)
			: base(data)
		{
		}

		public byte[] GetGraphic(int pixelsPerModule)
		{
			return GetGraphic(pixelsPerModule, new byte[3], new byte[3] { 255, 255, 255 });
		}

		public byte[] GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex)
		{
			return GetGraphic(pixelsPerModule, HexColorToByteArray(darkColorHtmlHex), HexColorToByteArray(lightColorHtmlHex));
		}

		public byte[] GetGraphic(int pixelsPerModule, byte[] darkColorRgb, byte[] lightColorRgb)
		{
			int num = base.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
			IEnumerable<byte> enumerable = darkColorRgb.Reverse();
			IEnumerable<byte> enumerable2 = lightColorRgb.Reverse();
			List<byte> list = new List<byte>();
			list.AddRange(new byte[18]
			{
				66, 77, 76, 0, 0, 0, 0, 0, 0, 0,
				26, 0, 0, 0, 12, 0, 0, 0
			});
			list.AddRange(IntTo4Byte(num));
			list.AddRange(IntTo4Byte(num));
			list.AddRange(new byte[4] { 1, 0, 24, 0 });
			for (int num2 = num - 1; num2 >= 0; num2 -= pixelsPerModule)
			{
				for (int i = 0; i < pixelsPerModule; i++)
				{
					for (int j = 0; j < num; j += pixelsPerModule)
					{
						bool flag = base.QrCodeData.ModuleMatrix[(num2 + pixelsPerModule) / pixelsPerModule - 1][(j + pixelsPerModule) / pixelsPerModule - 1];
						for (int k = 0; k < pixelsPerModule; k++)
						{
							list.AddRange(flag ? enumerable : enumerable2);
						}
					}
					if (num % 4 != 0)
					{
						for (int l = 0; l < num % 4; l++)
						{
							list.Add(0);
						}
					}
				}
			}
			list.AddRange(new byte[2]);
			return list.ToArray();
		}

		private byte[] HexColorToByteArray(string colorString)
		{
			if (colorString.StartsWith("#"))
			{
				colorString = colorString.Substring(1);
			}
			byte[] array = new byte[colorString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = byte.Parse(colorString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return array;
		}

		private byte[] IntTo4Byte(int inp)
		{
			byte[] array = new byte[2];
			array[1] = (byte)(inp >> 8);
			array[0] = (byte)inp;
			return array;
		}
	}
	public static class PayloadGenerator
	{
		public abstract class Payload
		{
			public virtual int Version => -1;

			public virtual QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

			public virtual QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Default;

			public abstract override string ToString();
		}

		public class WiFi : Payload
		{
			public enum Authentication
			{
				WEP,
				WPA,
				nopass
			}

			private readonly string ssid;

			private readonly string password;

			private readonly string authenticationMode;

			private readonly bool isHiddenSsid;

			public WiFi(string ssid, string password, Authentication authenticationMode, bool isHiddenSSID = false)
			{
				this.ssid = EscapeInput(ssid);
				this.ssid = (isHexStyle(this.ssid) ? ("\"" + this.ssid + "\"") : this.ssid);
				this.password = EscapeInput(password);
				this.password = (isHexStyle(this.password) ? ("\"" + this.password + "\"") : this.password);
				this.authenticationMode = authenticationMode.ToString();
				isHiddenSsid = isHiddenSSID;
			}

			public override string ToString()
			{
				return string.Format("WIFI:T:{0};S:{1};P:{2};{3};", authenticationMode, ssid, password, isHiddenSsid ? "H:true" : string.Empty);
			}
		}

		public class Mail : Payload
		{
			public enum MailEncoding
			{
				MAILTO,
				MATMSG,
				SMTP
			}

			private readonly string mailReceiver;

			private readonly string subject;

			private readonly string message;

			private readonly MailEncoding encoding;

			public Mail(string mailReceiver, MailEncoding encoding = MailEncoding.MAILTO)
			{
				this.mailReceiver = mailReceiver;
				subject = (message = string.Empty);
				this.encoding = encoding;
			}

			public Mail(string mailReceiver, string subject, MailEncoding encoding = MailEncoding.MAILTO)
			{
				this.mailReceiver = mailReceiver;
				this.subject = subject;
				message = string.Empty;
				this.encoding = encoding;
			}

			public Mail(string mailReceiver, string subject, string message, MailEncoding encoding = MailEncoding.MAILTO)
			{
				this.mailReceiver = mailReceiver;
				this.subject = subject;
				this.message = message;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				return encoding switch
				{
					MailEncoding.MAILTO => $"mailto:{mailReceiver}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(message)}", 
					MailEncoding.MATMSG => $"MATMSG:TO:{mailReceiver};SUB:{EscapeInput(subject)};BODY:{EscapeInput(message)};;", 
					MailEncoding.SMTP => $"SMTP:{mailReceiver}:{EscapeInput(subject, simple: true)}:{EscapeInput(message, simple: true)}", 
					_ => mailReceiver, 
				};
			}
		}

		public class SMS : Payload
		{
			public enum SMSEncoding
			{
				SMS,
				SMSTO,
				SMS_iOS
			}

			private readonly string number;

			private readonly string subject;

			private readonly SMSEncoding encoding;

			public SMS(string number, SMSEncoding encoding = SMSEncoding.SMS)
			{
				this.number = number;
				subject = string.Empty;
				this.encoding = encoding;
			}

			public SMS(string number, string subject, SMSEncoding encoding = SMSEncoding.SMS)
			{
				this.number = number;
				this.subject = subject;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				return encoding switch
				{
					SMSEncoding.SMS => $"sms:{number}?body={Uri.EscapeDataString(subject)}", 
					SMSEncoding.SMS_iOS => $"sms:{number};body={Uri.EscapeDataString(subject)}", 
					SMSEncoding.SMSTO => $"SMSTO:{number}:{subject}", 
					_ => "sms:", 
				};
			}
		}

		public class MMS : Payload
		{
			public enum MMSEncoding
			{
				MMS,
				MMSTO
			}

			private readonly string number;

			private readonly string subject;

			private readonly MMSEncoding encoding;

			public MMS(string number, MMSEncoding encoding = MMSEncoding.MMS)
			{
				this.number = number;
				subject = string.Empty;
				this.encoding = encoding;
			}

			public MMS(string number, string subject, MMSEncoding encoding = MMSEncoding.MMS)
			{
				this.number = number;
				this.subject = subject;
				this.encoding = encoding;
			}

			public override string ToString()
			{
				return encoding switch
				{
					MMSEncoding.MMSTO => $"mmsto:{number}?subject={Uri.EscapeDataString(subject)}", 
					MMSEncoding.MMS => $"mms:{number}?body={Uri.EscapeDataString(subject)}", 
					_ => "mms:", 
				};
			}
		}

		public class Geolocation : Payload
		{
			public enum GeolocationEncoding
			{
				GEO,
				GoogleMaps
			}

			private readonly string latitude;

			private readonly string longitude;

			private readonly GeolocationEncoding encoding;

			public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.GEO)
			{
				this.latitude = latitude.Replace(",", ".");
				this.longitude = longitude.Replace(",", ".");
				this.encoding = encoding;
			}

			public override string ToString()
			{
				return encoding switch
				{
					GeolocationEncoding.GEO => $"geo:{latitude},{longitude}", 
					GeolocationEncoding.GoogleMaps => $"http://maps.google.com/maps?q={latitude},{longitude}", 
					_ => "geo:", 
				};
			}
		}

		public class PhoneNumber : Payload
		{
			private readonly string number;

			public PhoneNumber(string number)
			{
				this.number = number;
			}

			public override string ToString()
			{
				return $"tel:{number}";
			}
		}

		public class SkypeCall : Payload
		{
			private readonly string skypeUsername;

			public SkypeCall(string skypeUsername)
			{
				this.skypeUsername = skypeUsername;
			}

			public override string ToString()
			{
				return $"skype:{skypeUsername}?call";
			}
		}

		public class Url : Payload
		{
			private readonly string url;

			public Url(string url)
			{
				this.url = url;
			}

			public override string ToString()
			{
				if (url.StartsWith("http"))
				{
					return url;
				}
				return "http://" + url;
			}
		}

		public class WhatsAppMessage : Payload
		{
			private readonly string message;

			public WhatsAppMessage(string message)
			{
				this.message = message;
			}

			public override string ToString()
			{
				return $"whatsapp://send?text={Uri.EscapeDataString(message)}";
			}
		}

		public class Bookmark : Payload
		{
			private readonly string url;

			private readonly string title;

			public Bookmark(string url, string title)
			{
				this.url = EscapeInput(url);
				this.title = EscapeInput(title);
			}

			public override string ToString()
			{
				return $"MEBKM:TITLE:{title};URL:{url};;";
			}
		}

		public class ContactData : Payload
		{
			public enum ContactOutputType
			{
				MeCard,
				VCard21,
				VCard3,
				VCard4
			}

			public enum AddressOrder
			{
				Default,
				Reversed
			}

			private readonly string firstname;

			private readonly string lastname;

			private readonly string nickname;

			private readonly string phone;

			private readonly string mobilePhone;

			private readonly string workPhone;

			private readonly string email;

			private readonly DateTime? birthday;

			private readonly string website;

			private readonly string street;

			private readonly string houseNumber;

			private readonly string city;

			private readonly string zipCode;

			private readonly string stateRegion;

			private readonly string country;

			private readonly string note;

			private readonly ContactOutputType outputType;

			private readonly AddressOrder addressOrder;

			public ContactData(ContactOutputType outputType, string firstname, string lastname, string nickname = null, string phone = null, string mobilePhone = null, string workPhone = null, string email = null, DateTime? birthday = null, string website = null, string street = null, string houseNumber = null, string city = null, string zipCode = null, string country = null, string note = null, string stateRegion = null, AddressOrder addressOrder = AddressOrder.Default)
			{
				this.firstname = firstname;
				this.lastname = lastname;
				this.nickname = nickname;
				this.phone = phone;
				this.mobilePhone = mobilePhone;
				this.workPhone = workPhone;
				this.email = email;
				this.birthday = birthday;
				this.website = website;
				this.street = street;
				this.houseNumber = houseNumber;
				this.city = city;
				this.stateRegion = stateRegion;
				this.zipCode = zipCode;
				this.country = country;
				this.addressOrder = addressOrder;
				this.note = note;
				this.outputType = outputType;
			}

			public override string ToString()
			{
				string empty = string.Empty;
				DateTime? dateTime;
				if (outputType.Equals(ContactOutputType.MeCard))
				{
					empty += "MECARD+\r\n";
					if (!string.IsNullOrEmpty(firstname) && !string.IsNullOrEmpty(lastname))
					{
						empty += $"N:{lastname}, {firstname}\r\n";
					}
					else if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname))
					{
						empty += $"N:{firstname}{lastname}\r\n";
					}
					if (!string.IsNullOrEmpty(phone))
					{
						empty += $"TEL:{phone}\r\n";
					}
					if (!string.IsNullOrEmpty(mobilePhone))
					{
						empty += $"TEL:{mobilePhone}\r\n";
					}
					if (!string.IsNullOrEmpty(workPhone))
					{
						empty += $"TEL:{workPhone}\r\n";
					}
					if (!string.IsNullOrEmpty(email))
					{
						empty += $"EMAIL:{email}\r\n";
					}
					if (!string.IsNullOrEmpty(note))
					{
						empty += $"NOTE:{note}\r\n";
					}
					dateTime = birthday;
					if (dateTime.HasValue)
					{
						string text = empty;
						dateTime = birthday;
						empty = text + string.Format("BDAY:{0}\r\n", dateTime.Value.ToString("yyyyMMdd"));
					}
					string empty2 = string.Empty;
					empty2 = ((addressOrder != 0) ? string.Format("ADR:,,{0}{1},{2},{3},{4},{5}\r\n", (!string.IsNullOrEmpty(houseNumber)) ? (houseNumber + " ") : "", (!string.IsNullOrEmpty(street)) ? street : "", (!string.IsNullOrEmpty(city)) ? city : "", (!string.IsNullOrEmpty(stateRegion)) ? stateRegion : "", (!string.IsNullOrEmpty(zipCode)) ? zipCode : "", (!string.IsNullOrEmpty(country)) ? country : "") : string.Format("ADR:,,{0}{1},{2},{3},{4},{5}\r\n", (!string.IsNullOrEmpty(street)) ? (street + " ") : "", (!string.IsNullOrEmpty(houseNumber)) ? houseNumber : "", (!string.IsNullOrEmpty(zipCode)) ? zipCode : "", (!string.IsNullOrEmpty(city)) ? city : "", (!string.IsNullOrEmpty(stateRegion)) ? stateRegion : "", (!string.IsNullOrEmpty(country)) ? country : ""));
					empty += empty2;
					if (!string.IsNullOrEmpty(website))
					{
						empty += $"URL:{website}\r\n";
					}
					if (!string.IsNullOrEmpty(nickname))
					{
						empty += $"NICKNAME:{nickname}\r\n";
					}
					return empty.Trim('\r', '\n');
				}
				string text2 = outputType.ToString().Substring(5);
				text2 = ((text2.Length <= 1) ? (text2 + ".0") : text2.Insert(1, "."));
				empty += "BEGIN:VCARD\r\n";
				empty += $"VERSION:{text2}\r\n";
				empty += string.Format("N:{0};{1};;;\r\n", (!string.IsNullOrEmpty(lastname)) ? lastname : "", (!string.IsNullOrEmpty(firstname)) ? firstname : "");
				empty += string.Format("FN:{0}{1}\r\n", (!string.IsNullOrEmpty(firstname)) ? (firstname + " ") : "", (!string.IsNullOrEmpty(lastname)) ? lastname : "");
				if (!string.IsNullOrEmpty(phone))
				{
					empty += "TEL;";
					empty = (outputType.Equals(ContactOutputType.VCard21) ? (empty + $"HOME;VOICE:{phone}") : ((!outputType.Equals(ContactOutputType.VCard3)) ? (empty + $"TYPE=home,voice;VALUE=uri:tel:{phone}") : (empty + $"TYPE=HOME,VOICE:{phone}")));
					empty += "\r\n";
				}
				if (!string.IsNullOrEmpty(mobilePhone))
				{
					empty += "TEL;";
					empty = (outputType.Equals(ContactOutputType.VCard21) ? (empty + $"HOME;CELL:{mobilePhone}") : ((!outputType.Equals(ContactOutputType.VCard3)) ? (empty + $"TYPE=home,cell;VALUE=uri:tel:{mobilePhone}") : (empty + $"TYPE=HOME,CELL:{mobilePhone}")));
					empty += "\r\n";
				}
				if (!string.IsNullOrEmpty(workPhone))
				{
					empty += "TEL;";
					empty = (outputType.Equals(ContactOutputType.VCard21) ? (empty + $"WORK;VOICE:{workPhone}") : ((!outputType.Equals(ContactOutputType.VCard3)) ? (empty + $"TYPE=work,voice;VALUE=uri:tel:{workPhone}") : (empty + $"TYPE=WORK,VOICE:{workPhone}")));
					empty += "\r\n";
				}
				empty += "ADR;";
				empty = (outputType.Equals(ContactOutputType.VCard21) ? (empty + "HOME;PREF:") : ((!outputType.Equals(ContactOutputType.VCard3)) ? (empty + "TYPE=home,pref:") : (empty + "TYPE=HOME,PREF:")));
				string empty3 = string.Empty;
				empty3 = ((addressOrder != 0) ? string.Format(";;{0}{1};{2};{3};{4};{5}\r\n", (!string.IsNullOrEmpty(houseNumber)) ? (houseNumber + " ") : "", (!string.IsNullOrEmpty(street)) ? street : "", (!string.IsNullOrEmpty(city)) ? city : "", (!string.IsNullOrEmpty(stateRegion)) ? stateRegion : "", (!string.IsNullOrEmpty(zipCode)) ? zipCode : "", (!string.IsNullOrEmpty(country)) ? country : "") : string.Format(";;{0}{1};{2};{3};{4};{5}\r\n", (!string.IsNullOrEmpty(street)) ? (street + " ") : "", (!string.IsNullOrEmpty(houseNumber)) ? houseNumber : "", (!string.IsNullOrEmpty(zipCode)) ? zipCode : "", (!string.IsNullOrEmpty(city)) ? city : "", (!string.IsNullOrEmpty(stateRegion)) ? stateRegion : "", (!string.IsNullOrEmpty(country)) ? country : ""));
				empty += empty3;
				dateTime = birthday;
				if (dateTime.HasValue)
				{
					string text3 = empty;
					dateTime = birthday;
					empty = text3 + string.Format("BDAY:{0}\r\n", dateTime.Value.ToString("yyyyMMdd"));
				}
				if (!string.IsNullOrEmpty(phone))
				{
					empty += $"URL:{website}\r\n";
				}
				if (!string.IsNullOrEmpty(email))
				{
					empty += $"EMAIL:{email}\r\n";
				}
				if (!string.IsNullOrEmpty(note))
				{
					empty += $"NOTE:{note}\r\n";
				}
				if (!outputType.Equals(ContactOutputType.VCard21) && !string.IsNullOrEmpty(nickname))
				{
					empty += $"NICKNAME:{nickname}\r\n";
				}
				return empty + "END:VCARD";
			}
		}

		public class BitcoinAddress : Payload
		{
			private readonly string address;

			private readonly string label;

			private readonly string message;

			private readonly double? amount;

			public BitcoinAddress(string address, double? amount, string label = null, string message = null)
			{
				this.address = address;
				if (!string.IsNullOrEmpty(label))
				{
					this.label = Uri.EscapeUriString(label);
				}
				if (!string.IsNullOrEmpty(message))
				{
					this.message = Uri.EscapeUriString(message);
				}
				this.amount = amount;
			}

			public override string ToString()
			{
				string arg = null;
				List<KeyValuePair<string, string>> obj = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("label", label),
					new KeyValuePair<string, string>("message", message)
				};
				double? num = amount;
				object value;
				if (!num.HasValue)
				{
					value = null;
				}
				else
				{
					num = amount;
					value = num.Value.ToString("#.########", CultureInfo.InvariantCulture);
				}
				obj.Add(new KeyValuePair<string, string>("amount", (string)value));
				List<KeyValuePair<string, string>> source = obj;
				if (source.Any((KeyValuePair<string, string> keyPair) => !string.IsNullOrEmpty(keyPair.Value)))
				{
					arg = "?" + string.Join("&", (from keyPair in source
						where !string.IsNullOrEmpty(keyPair.Value)
						select $"{keyPair.Key}={keyPair.Value}").ToArray());
				}
				return $"bitcoin:{address}{arg}";
			}
		}

		public class SwissQrCode : Payload
		{
			public class Reference
			{
				public enum ReferenceType
				{
					QRR,
					SCOR,
					NON
				}

				public enum ReferenceTextType
				{
					QrReference,
					CreditorReferenceIso11649
				}

				public class SwissQrCodeReferenceException : Exception
				{
					public SwissQrCodeReferenceException()
					{
					}

					public SwissQrCodeReferenceException(string message)
						: base(message)
					{
					}

					public SwissQrCodeReferenceException(string message, Exception inner)
						: base(message, inner)
					{
					}
				}

				private readonly ReferenceType referenceType;

				private readonly string reference;

				private readonly string unstructuredMessage;

				private readonly ReferenceTextType? referenceTextType;

				public ReferenceType RefType => referenceType;

				public string ReferenceText
				{
					get
					{
						if (string.IsNullOrEmpty(reference))
						{
							return null;
						}
						return reference.Replace("\n", "");
					}
				}

				public string UnstructureMessage
				{
					get
					{
						if (string.IsNullOrEmpty(unstructuredMessage))
						{
							return null;
						}
						return unstructuredMessage.Replace("\n", "");
					}
				}

				public Reference(ReferenceType referenceType, string reference = null, ReferenceTextType? referenceTextType = null, string unstructuredMessage = null)
				{
					this.referenceType = referenceType;
					this.referenceTextType = referenceTextType;
					if (referenceType.Equals(ReferenceType.NON) && reference != null)
					{
						throw new SwissQrCodeReferenceException("Reference is only allowed when referenceType not equals \"NON\"");
					}
					if (!referenceType.Equals(ReferenceType.NON) && reference != null && !referenceTextType.HasValue)
					{
						throw new SwissQrCodeReferenceException("You have to set an ReferenceTextType when using the reference text.");
					}
					if (referenceTextType.Equals(ReferenceTextType.QrReference) && reference != null && reference.Length > 27)
					{
						throw new SwissQrCodeReferenceException("QR-references have to be shorter than 28 chars.");
					}
					if (referenceTextType.Equals(ReferenceTextType.QrReference) && reference != null && !Regex.IsMatch(reference, "^[0-9]+$"))
					{
						throw new SwissQrCodeReferenceException("QR-reference must exist out of digits only.");
					}
					if (referenceTextType.Equals(ReferenceTextType.QrReference) && reference != null && !ChecksumMod10(reference))
					{
						throw new SwissQrCodeReferenceException("QR-references is invalid. Checksum error.");
					}
					if (referenceTextType.Equals(ReferenceTextType.CreditorReferenceIso11649) && reference != null && reference.Length > 25)
					{
						throw new SwissQrCodeReferenceException("Creditor references (ISO 11649) have to be shorter than 26 chars.");
					}
					this.reference = reference;
					if (unstructuredMessage != null && unstructuredMessage.Length > 140)
					{
						throw new SwissQrCodeReferenceException("The unstructured message must be shorter than 141 chars.");
					}
					this.unstructuredMessage = unstructuredMessage;
				}
			}

			public class Iban
			{
				public enum IbanType
				{
					Iban,
					QrIban
				}

				public class SwissQrCodeIbanException : Exception
				{
					public SwissQrCodeIbanException()
					{
					}

					public SwissQrCodeIbanException(string message)
						: base(message)
					{
					}

					public SwissQrCodeIbanException(string message, Exception inner)
						: base(message, inner)
					{
					}
				}

				private string iban;

				private IbanType ibanType;

				public bool IsQrIban => ibanType.Equals(IbanType.QrIban);

				public Iban(string iban, IbanType ibanType)
				{
					if (!IsValidIban(iban))
					{
						throw new SwissQrCodeIbanException("The IBAN entered isn't valid.");
					}
					if (!iban.StartsWith("CH") && !iban.StartsWith("LI"))
					{
						throw new SwissQrCodeIbanException("The IBAN must start with \"CH\" or \"LI\".");
					}
					this.iban = iban;
					this.ibanType = ibanType;
				}

				public override string ToString()
				{
					return iban.Replace("\n", "").Replace(" ", "");
				}
			}

			public class Contact
			{
				public class SwissQrCodeContactException : Exception
				{
					public SwissQrCodeContactException()
					{
					}

					public SwissQrCodeContactException(string message)
						: base(message)
					{
					}

					public SwissQrCodeContactException(string message, Exception inner)
						: base(message, inner)
					{
					}
				}

				private string br = "\r\n";

				private string name;

				private string street;

				private string houseNumber;

				private string zipCode;

				private string city;

				private string country;

				public Contact(string name, string zipCode, string city, string country, string street = null, string houseNumber = null)
				{
					string text = "^([a-zA-Z0-9\\.,;:'\\ \\-/\\(\\)?\\*\\[\\]\\{\\}\\\\`\u00b4~ ]|[!\"#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$";
					if (string.IsNullOrEmpty(name))
					{
						throw new SwissQrCodeContactException("Name must not be empty.");
					}
					if (name.Length > 70)
					{
						throw new SwissQrCodeContactException("Name must be shorter than 71 chars.");
					}
					if (!Regex.IsMatch(name, text))
					{
						throw new SwissQrCodeContactException($"Name must match the following pattern as defined in pain.001: {text}");
					}
					this.name = name;
					if (!string.IsNullOrEmpty(street) && street.Length > 70)
					{
						throw new SwissQrCodeContactException("Street must be shorter than 71 chars.");
					}
					if (!string.IsNullOrEmpty(street) && !Regex.IsMatch(street, text))
					{
						throw new SwissQrCodeContactException($"Street must match the following pattern as defined in pain.001: {text}");
					}
					this.street = street;
					if (!string.IsNullOrEmpty(houseNumber) && houseNumber.Length > 16)
					{
						throw new SwissQrCodeContactException("House number must be shorter than 17 chars.");
					}
					this.houseNumber = houseNumber;
					if (string.IsNullOrEmpty(zipCode))
					{
						throw new SwissQrCodeContactException("Zip code must not be empty.");
					}
					if (zipCode.Length > 16)
					{
						throw new SwissQrCodeContactException("Zip code must be shorter than 17 chars.");
					}
					if (!Regex.IsMatch(zipCode, text))
					{
						throw new SwissQrCodeContactException($"Zip code must match the following pattern as defined in pain.001: {text}");
					}
					this.zipCode = zipCode;
					if (string.IsNullOrEmpty(city))
					{
						throw new SwissQrCodeContactException("City must not be empty.");
					}
					if (city.Length > 35)
					{
						throw new SwissQrCodeContactException("City name must be shorter than 36 chars.");
					}
					if (!Regex.IsMatch(city, text))
					{
						throw new SwissQrCodeContactException($"City name must match the following pattern as defined in pain.001: {text}");
					}
					this.city = city;
					if (!(from x in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
						where new RegionInfo(x.LCID).TwoLetterISORegionName.ToUpper() == country.ToUpper()
						select x).Any())
					{
						throw new SwissQrCodeContactException("Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");
					}
					this.country = country;
				}

				public override string ToString()
				{
					return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(name.Replace("\n", "") + br, (!string.IsNullOrEmpty(street)) ? street.Replace("\n", "") : string.Empty, br), (!string.IsNullOrEmpty(houseNumber)) ? houseNumber.Replace("\n", "") : string.Empty, br), zipCode.Replace("\n", ""), br), city.Replace("\n", ""), br), country, br);
				}
			}

			public enum Currency
			{
				CHF = 756,
				EUR = 978
			}

			public class SwissQrCodeException : Exception
			{
				public SwissQrCodeException()
				{
				}

				public SwissQrCodeException(string message)
					: base(message)
				{
				}

				public SwissQrCodeException(string message, Exception inner)
					: base(message, inner)
				{
				}
			}

			private readonly string br = "\r\n";

			private readonly string alternativeProcedure1;

			private readonly string alternativeProcedure2;

			private readonly Iban iban;

			private readonly decimal? amount;

			private readonly Contact creditor;

			private readonly Contact ultimateCreditor;

			private readonly Contact debitor;

			private readonly Currency currency;

			private readonly DateTime? requestedDateOfPayment;

			private readonly Reference reference;

			public SwissQrCode(Iban iban, Currency currency, Contact creditor, Reference reference, Contact debitor = null, decimal? amount = null, DateTime? requestedDateOfPayment = null, Contact ultimateCreditor = null, string alternativeProcedure1 = null, string alternativeProcedure2 = null)
			{
				this.iban = iban;
				this.creditor = creditor;
				this.ultimateCreditor = ultimateCreditor;
				if (amount.HasValue && amount.ToString().Length > 12)
				{
					throw new SwissQrCodeException("Amount (including decimals) must be shorter than 13 places.");
				}
				this.amount = amount;
				this.currency = currency;
				this.requestedDateOfPayment = requestedDateOfPayment;
				this.debitor = debitor;
				if (iban.IsQrIban && reference.RefType.Equals(Reference.ReferenceType.NON))
				{
					throw new SwissQrCodeException("If QR-IBAN is used, you have to choose \"QRR\" or \"SCOR\" as reference type!");
				}
				this.reference = reference;
				if (alternativeProcedure1 != null && alternativeProcedure1.Length > 100)
				{
					throw new SwissQrCodeException("Alternative procedure information block 1 must be shorter than 101 chars.");
				}
				this.alternativeProcedure1 = alternativeProcedure1;
				if (alternativeProcedure2 != null && alternativeProcedure2.Length > 100)
				{
					throw new SwissQrCodeException("Alternative procedure information block 2 must be shorter than 101 chars.");
				}
				this.alternativeProcedure2 = alternativeProcedure2;
			}

			public override string ToString()
			{
				string text = "SPC" + br;
				text = text + "0100" + br;
				text = text + "1" + br;
				text = text + iban.ToString() + br;
				text += creditor.ToString();
				text = ((ultimateCreditor == null) ? string.Concat(text, string.Concat(Enumerable.Repeat(br, 6).ToArray())) : (text + ultimateCreditor.ToString()));
				string text2 = text;
				decimal? num = amount;
				text = text2 + (num.HasValue ? $"{amount:0.00}" : string.Empty) + br;
				text = string.Concat(text, currency, br);
				string text3 = text;
				DateTime? dateTime = requestedDateOfPayment;
				string text4;
				if (!dateTime.HasValue)
				{
					text4 = string.Empty;
				}
				else
				{
					dateTime = requestedDateOfPayment;
					text4 = dateTime.Value.ToString("yyyy-MM-dd");
				}
				text = text3 + text4 + br;
				text = ((debitor == null) ? string.Concat(text, string.Concat(Enumerable.Repeat(br, 6).ToArray())) : (text + debitor.ToString()));
				text = text + reference.RefType.ToString() + br;
				text = text + ((!string.IsNullOrEmpty(reference.ReferenceText)) ? reference.ReferenceText : string.Empty) + br;
				text = text + ((!string.IsNullOrEmpty(reference.UnstructureMessage)) ? reference.UnstructureMessage : string.Empty) + br;
				if (!string.IsNullOrEmpty(alternativeProcedure1))
				{
					text = text + alternativeProcedure1.Replace("\n", "") + br;
				}
				if (!string.IsNullOrEmpty(alternativeProcedure2))
				{
					text = text + alternativeProcedure2.Replace("\n", "") + br;
				}
				return text;
			}
		}

		public class Girocode : Payload
		{
			public enum GirocodeVersion
			{
				Version1,
				Version2
			}

			public enum TypeOfRemittance
			{
				Structured,
				Unstructured
			}

			public enum GirocodeEncoding
			{
				UTF_8,
				ISO_8859_1,
				ISO_8859_2,
				ISO_8859_4,
				ISO_8859_5,
				ISO_8859_7,
				ISO_8859_10,
				ISO_8859_15
			}

			public class GirocodeException : Exception
			{
				public GirocodeException()
				{
				}

				public GirocodeException(string message)
					: base(message)
				{
				}

				public GirocodeException(string message, Exception inner)
					: base(message, inner)
				{
				}
			}

			private string br = "\n";

			private readonly string iban;

			private readonly string bic;

			private readonly string name;

			private readonly string purposeOfCreditTransfer;

			private readonly string remittanceInformation;

			private readonly string messageToGirocodeUser;

			private readonly decimal amount;

			private readonly GirocodeVersion version;

			private readonly GirocodeEncoding encoding;

			private readonly TypeOfRemittance typeOfRemittance;

			public Girocode(string iban, string bic, string name, decimal amount, string remittanceInformation = "", TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured, string purposeOfCreditTransfer = "", string messageToGirocodeUser = "", GirocodeVersion version = GirocodeVersion.Version1, GirocodeEncoding encoding = GirocodeEncoding.ISO_8859_1)
			{
				this.version = version;
				this.encoding = encoding;
				if (!IsValidIban(iban))
				{
					throw new GirocodeException("The IBAN entered isn't valid.");
				}
				this.iban = iban.Replace(" ", "").ToUpper();
				if (!IsValidBic(bic))
				{
					throw new GirocodeException("The BIC entered isn't valid.");
				}
				this.bic = bic.Replace(" ", "").ToUpper();
				if (name.Length > 70)
				{
					throw new GirocodeException("(Payee-)Name must be shorter than 71 chars.");
				}
				this.name = name;
				if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",", ".").Split(new char[1] { '.' })[1].TrimEnd(new char[1] { '0' }).Length > 2)
				{
					throw new GirocodeException("Amount must have less than 3 digits after decimal point.");
				}
				if (amount < 0.01m || amount > 999999999.99m)
				{
					throw new GirocodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
				}
				this.amount = amount;
				if (purposeOfCreditTransfer.Length > 4)
				{
					throw new GirocodeException("Purpose of credit transfer can only have 4 chars at maximum.");
				}
				this.purposeOfCreditTransfer = purposeOfCreditTransfer;
				if (typeOfRemittance.Equals(TypeOfRemittance.Unstructured) && remittanceInformation.Length > 140)
				{
					throw new GirocodeException("Unstructured reference texts have to shorter than 141 chars.");
				}
				if (typeOfRemittance.Equals(TypeOfRemittance.Structured) && remittanceInformation.Length > 35)
				{
					throw new GirocodeException("Structured reference texts have to shorter than 36 chars.");
				}
				this.typeOfRemittance = typeOfRemittance;
				this.remittanceInformation = remittanceInformation;
				if (messageToGirocodeUser.Length > 70)
				{
					throw new GirocodeException("Message to the Girocode-User reader texts have to shorter than 71 chars.");
				}
				this.messageToGirocodeUser = messageToGirocodeUser;
			}

			public override string ToString()
			{
				return ConvertStringToEncoding(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("BCD" + br, version.Equals(GirocodeVersion.Version1) ? "001" : "002", br), (int)(encoding + 1), br), "SCT", br), bic, br), name, br), iban, br), $"EUR{amount:0.00}".Replace(",", "."), br), purposeOfCreditTransfer, br), typeOfRemittance.Equals(TypeOfRemittance.Structured) ? remittanceInformation : string.Empty, br), typeOfRemittance.Equals(TypeOfRemittance.Unstructured) ? remittanceInformation : string.Empty, br), messageToGirocodeUser), encoding.ToString().Replace("_", "-"));
			}
		}

		public class BezahlCode : Payload
		{
			public enum Currency
			{
				AED = 784,
				AFN = 971,
				ALL = 8,
				AMD = 51,
				ANG = 532,
				AOA = 973,
				ARS = 32,
				AUD = 36,
				AWG = 533,
				AZN = 944,
				BAM = 977,
				BBD = 52,
				BDT = 50,
				BGN = 975,
				BHD = 48,
				BIF = 108,
				BMD = 60,
				BND = 96,
				BOB = 68,
				BOV = 984,
				BRL = 986,
				BSD = 44,
				BTN = 64,
				BWP = 72,
				BYR = 974,
				BZD = 84,
				CAD = 124,
				CDF = 976,
				CHE = 947,
				CHF = 756,
				CHW = 948,
				CLF = 990,
				CLP = 152,
				CNY = 156,
				COP = 170,
				COU = 970,
				CRC = 188,
				CUC = 931,
				CUP = 192,
				CVE = 132,
				CZK = 203,
				DJF = 262,
				DKK = 208,
				DOP = 214,
				DZD = 12,
				EGP = 818,
				ERN = 232,
				ETB = 230,
				EUR = 978,
				FJD = 242,
				FKP = 238,
				GBP = 826,
				GEL = 981,
				GHS = 936,
				GIP = 292,
				GMD = 270,
				GNF = 324,
				GTQ = 320,
				GYD = 328,
				HKD = 344,
				HNL = 340,
				HRK = 191,
				HTG = 332,
				HUF = 348,
				IDR = 360,
				ILS = 376,
				INR = 356,
				IQD = 368,
				IRR = 364,
				ISK = 352,
				JMD = 388,
				JOD = 400,
				JPY = 392,
				KES = 404,
				KGS = 417,
				KHR = 116,
				KMF = 174,
				KPW = 408,
				KRW = 410,
				KWD = 414,
				KYD = 136,
				KZT = 398,
				LAK = 418,
				LBP = 422,
				LKR = 144,
				LRD = 430,
				LSL = 426,
				LYD = 434,
				MAD = 504,
				MDL = 498,
				MGA = 969,
				MKD = 807,
				MMK = 104,
				MNT = 496,
				MOP = 446,
				MRO = 478,
				MUR = 480,
				MVR = 462,
				MWK = 454,
				MXN = 484,
				MXV = 979,
				MYR = 458,
				MZN = 943,
				NAD = 516,
				NGN = 566,
				NIO = 558,
				NOK = 578,
				NPR = 524,
				NZD = 554,
				OMR = 512,
				PAB = 590,
				PEN = 604,
				PGK = 598,
				PHP = 608,
				PKR = 586,
				PLN = 985,
				PYG = 600,
				QAR = 634,
				RON = 946,
				RSD = 941,
				RUB = 643,
				RWF = 646,
				SAR = 682,
				SBD = 90,
				SCR = 690,
				SDG = 938,
				SEK = 752,
				SGD = 702,
				SHP = 654,
				SLL = 694,
				SOS = 706,
				SRD = 968,
				SSP = 728,
				STD = 678,
				SVC = 222,
				SYP = 760,
				SZL = 748,
				THB = 764,
				TJS = 972,
				TMT = 934,
				TND = 788,
				TOP = 776,
				TRY = 949,
				TTD = 780,
				TWD = 901,
				TZS = 834,
				UAH = 980,
				UGX = 800,
				USD = 840,
				USN = 997,
				UYI = 940,
				UYU = 858,
				UZS = 860,
				VEF = 937,
				VND = 704,
				VUV = 548,
				WST = 882,
				XAF = 950,
				XAG = 961,
				XAU = 959,
				XBA = 955,
				XBB = 956,
				XBC = 957,
				XBD = 958,
				XCD = 951,
				XDR = 960,
				XOF = 952,
				XPD = 964,
				XPF = 953,
				XPT = 962,
				XSU = 994,
				XTS = 963,
				XUA = 965,
				XXX = 999,
				YER = 886,
				ZAR = 710,
				ZMW = 967,
				ZWL = 932
			}

			public enum AuthorityType
			{
				[Obsolete]
				singlepayment,
				singlepaymentsepa,
				[Obsolete]
				singledirectdebit,
				singledirectdebitsepa,
				[Obsolete]
				periodicsinglepayment,
				periodicsinglepaymentsepa,
				contact,
				contact_v2
			}

			public class BezahlCodeException : Exception
			{
				public BezahlCodeException()
				{
				}

				public BezahlCodeException(string message)
					: base(message)
				{
				}

				public BezahlCodeException(string message, Exception inner)
					: base(message, inner)
				{
				}
			}

			private readonly string name;

			private readonly string iban;

			private readonly string bic;

			private readonly string account;

			private readonly string bnc;

			private readonly string sepaReference;

			private readonly string reason;

			private readonly string creditorId;

			private readonly string mandateId;

			private readonly string periodicTimeunit;

			private readonly decimal amount;

			private readonly int postingKey;

			private readonly int periodicTimeunitRotation;

			private readonly Currency currency;

			private readonly AuthorityType authority;

			private readonly DateTime executionDate;

			private readonly DateTime dateOfSignature;

			private readonly DateTime periodicFirstExecutionDate;

			private readonly DateTime periodicLastExecutionDate;

			public BezahlCode(AuthorityType authority, string name, string account = "", string bnc = "", string iban = "", string bic = "", string reason = "")
				: this(authority, name, account, bnc, iban, bic, 0m, string.Empty, 0, null, null, string.Empty, string.Empty, null, reason, 0, string.Empty, Currency.EUR, null, 1)
			{
			}

			public BezahlCode(AuthorityType authority, string name, string account, string bnc, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string reason = "", int postingKey = 0, Currency currency = Currency.EUR, DateTime? executionDate = null)
				: this(authority, name, account, bnc, string.Empty, string.Empty, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, string.Empty, string.Empty, null, reason, postingKey, string.Empty, currency, executionDate, 2)
			{
			}

			public BezahlCode(AuthorityType authority, string name, string iban, string bic, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string creditorId = "", string mandateId = "", DateTime? dateOfSignature = null, string reason = "", string sepaReference = "", Currency currency = Currency.EUR, DateTime? executionDate = null)
				: this(authority, name, string.Empty, string.Empty, iban, bic, amount, periodicTimeunit, periodicTimeunitRotation, periodicFirstExecutionDate, periodicLastExecutionDate, creditorId, mandateId, dateOfSignature, reason, 0, sepaReference, currency, executionDate, 3)
			{
			}

			public BezahlCode(AuthorityType authority, string name, string account, string bnc, string iban, string bic, decimal amount, string periodicTimeunit = "", int periodicTimeunitRotation = 0, DateTime? periodicFirstExecutionDate = null, DateTime? periodicLastExecutionDate = null, string creditorId = "", string mandateId = "", DateTime? dateOfSignature = null, string reason = "", int postingKey = 0, string sepaReference = "", Currency currency = Currency.EUR, DateTime? executionDate = null, int internalMode = 0)
			{
				switch (internalMode)
				{
				case 1:
					if (!authority.Equals(AuthorityType.contact) && !authority.Equals(AuthorityType.contact_v2))
					{
						throw new BezahlCodeException("The constructor without an amount may only ne used with authority types 'contact' and 'contact_v2'.");
					}
					if (authority.Equals(AuthorityType.contact) && (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(bnc)))
					{
						throw new BezahlCodeException("When using authority type 'contact' the parameters 'account' and 'bnc' must be set.");
					}
					if (!authority.Equals(AuthorityType.contact_v2))
					{
						bool flag = !string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc);
						bool flag2 = !string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic);
						if ((!flag && !flag2) || (flag && flag2))
						{
							throw new BezahlCodeException("When using authority type 'contact_v2' either the parameters 'account' and 'bnc' or the parameters 'iban' and 'bic' must be set. Leave the other parameter pair empty.");
						}
					}
					break;
				case 2:
					if (!authority.Equals(AuthorityType.periodicsinglepayment) && !authority.Equals(AuthorityType.singledirectdebit) && !authority.Equals(AuthorityType.singlepayment))
					{
						throw new BezahlCodeException("The constructor with 'account' and 'bnc' may only be used with 'non SEPA' authority types. Either choose another authority type or switch constructor.");
					}
					if (authority.Equals(AuthorityType.periodicsinglepayment) && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
					{
						throw new BezahlCodeException("When using 'periodicsinglepayment' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
					}
					break;
				case 3:
					if (!authority.Equals(AuthorityType.periodicsinglepaymentsepa) && !authority.Equals(AuthorityType.singledirectdebitsepa) && !authority.Equals(AuthorityType.singlepaymentsepa))
					{
						throw new BezahlCodeException("The constructor with 'iban' and 'bic' may only be used with 'SEPA' authority types. Either choose another authority type or switch constructor.");
					}
					if (authority.Equals(AuthorityType.periodicsinglepaymentsepa) && (string.IsNullOrEmpty(periodicTimeunit) || periodicTimeunitRotation == 0))
					{
						throw new BezahlCodeException("When using 'periodicsinglepaymentsepa' as authority type, the parameters 'periodicTimeunit' and 'periodicTimeunitRotation' must be set.");
					}
					break;
				}
				this.authority = authority;
				if (name.Length > 70)
				{
					throw new BezahlCodeException("(Payee-)Name must be shorter than 71 chars.");
				}
				this.name = name;
				if (reason.Length > 27)
				{
					throw new BezahlCodeException("Reasons texts have to be shorter than 28 chars.");
				}
				this.reason = reason;
				bool flag3 = !string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc);
				bool flag4 = !string.IsNullOrEmpty(iban) && !string.IsNullOrEmpty(bic);
				if (authority.Equals(AuthorityType.periodicsinglepayment) || authority.Equals(AuthorityType.singledirectdebit) || authority.Equals(AuthorityType.singlepayment) || authority.Equals(AuthorityType.contact) || (authority.Equals(AuthorityType.contact_v2) && flag3))
				{
					if (!Regex.IsMatch(account.Replace(" ", ""), "^[0-9]{1,9}$"))
					{
						throw new BezahlCodeException("The account entered isn't valid.");
					}
					this.account = account.Replace(" ", "").ToUpper();
					if (!Regex.IsMatch(bnc.Replace(" ", ""), "^[0-9]{1,9}$"))
					{
						throw new BezahlCodeException("The bnc entered isn't valid.");
					}
					this.bnc = bnc.Replace(" ", "").ToUpper();
					if (!authority.Equals(AuthorityType.contact) && !authority.Equals(AuthorityType.contact_v2))
					{
						if (postingKey < 0 || postingKey >= 100)
						{
							throw new BezahlCodeException("PostingKey must be within 0 and 99.");
						}
						this.postingKey = postingKey;
					}
				}
				if (authority.Equals(AuthorityType.periodicsinglepaymentsepa) || authority.Equals(AuthorityType.singledirectdebitsepa) || authority.Equals(AuthorityType.singlepaymentsepa) || (authority.Equals(AuthorityType.contact_v2) && flag4))
				{
					if (!IsValidIban(iban))
					{
						throw new BezahlCodeException("The IBAN entered isn't valid.");
					}
					this.iban = iban.Replace(" ", "").ToUpper();
					if (!IsValidBic(bic))
					{
						throw new BezahlCodeException("The BIC entered isn't valid.");
					}
					this.bic = bic.Replace(" ", "").ToUpper();
					if (!authority.Equals(AuthorityType.contact_v2))
					{
						if (sepaReference.Length > 35)
						{
							throw new BezahlCodeException("SEPA reference texts have to be shorter than 36 chars.");
						}
						this.sepaReference = sepaReference;
						if (!string.IsNullOrEmpty(creditorId) && !Regex.IsMatch(creditorId.Replace(" ", ""), "^[a-zA-Z]{2,2}[0-9]{2,2}([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){3,3}([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){1,28}$"))
						{
							throw new BezahlCodeException("The creditorId entered isn't valid.");
						}
						this.creditorId = creditorId;
						if (!string.IsNullOrEmpty(mandateId) && !Regex.IsMatch(mandateId.Replace(" ", ""), "^([A-Za-z0-9]|[\\+|\\?|/|\\-|:|\\(|\\)|\\.|,|']){1,35}$"))
						{
							throw new BezahlCodeException("The mandateId entered isn't valid.");
						}
						this.mandateId = mandateId;
						if (dateOfSignature.HasValue)
						{
							this.dateOfSignature = dateOfSignature.Value;
						}
					}
				}
				if (authority.Equals(AuthorityType.contact) || authority.Equals(AuthorityType.contact_v2))
				{
					return;
				}
				if (amount.ToString().Replace(",", ".").Contains(".") && amount.ToString().Replace(",", ".").Split(new char[1] { '.' })[1].TrimEnd(new char[1] { '0' }).Length > 2)
				{
					throw new BezahlCodeException("Amount must have less than 3 digits after decimal point.");
				}
				if (amount < 0.01m || amount > 999999999.99m)
				{
					throw new BezahlCodeException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
				}
				this.amount = amount;
				this.currency = currency;
				if (!executionDate.HasValue)
				{
					this.executionDate = DateTime.Now;
				}
				else
				{
					if (DateTime.Today.Ticks > executionDate.Value.Ticks)
					{
						throw new BezahlCodeException("Execution date must be today or in future.");
					}
					this.executionDate = executionDate.Value;
				}
				if (authority.Equals(AuthorityType.periodicsinglepayment) || authority.Equals(AuthorityType.periodicsinglepaymentsepa))
				{
					if (periodicTimeunit.ToUpper() != "M" && periodicTimeunit.ToUpper() != "W")
					{
						throw new BezahlCodeException("The periodicTimeunit must be either 'M' (monthly) or 'W' (weekly).");
					}
					this.periodicTimeunit = periodicTimeunit;
					if (periodicTimeunitRotation < 1 || periodicTimeunitRotation > 52)
					{
						throw new BezahlCodeException("The periodicTimeunitRotation must be 1 or greater. (It means repeat the payment every 'periodicTimeunitRotation' weeks/months.");
					}
					this.periodicTimeunitRotation = periodicTimeunitRotation;
					if (periodicFirstExecutionDate.HasValue)
					{
						this.periodicFirstExecutionDate = periodicFirstExecutionDate.Value;
					}
					if (periodicLastExecutionDate.HasValue)
					{
						this.periodicLastExecutionDate = periodicLastExecutionDate.Value;
					}
				}
			}

			public override string ToString()
			{
				string text = $"bank://{authority}?";
				text += $"name={Uri.EscapeDataString(name)}&";
				if (!authority.Equals(AuthorityType.contact) && !authority.Equals(AuthorityType.contact_v2))
				{
					DateTime dateTime;
					if (authority.Equals(AuthorityType.periodicsinglepayment) || authority.Equals(AuthorityType.singledirectdebit) || authority.Equals(AuthorityType.singlepayment))
					{
						text += $"account={account}&";
						text += $"bnc={bnc}&";
						if (postingKey > 0)
						{
							text += $"postingkey={postingKey}&";
						}
					}
					else
					{
						text += $"iban={iban}&";
						text += $"bic={bic}&";
						if (!string.IsNullOrEmpty(sepaReference))
						{
							text += $"separeference={Uri.EscapeDataString(sepaReference)}&";
						}
						if (authority.Equals(AuthorityType.singledirectdebitsepa))
						{
							if (!string.IsNullOrEmpty(creditorId))
							{
								text += $"creditorid={Uri.EscapeDataString(creditorId)}&";
							}
							if (!string.IsNullOrEmpty(mandateId))
							{
								text += $"mandateid={Uri.EscapeDataString(mandateId)}&";
							}
							_ = dateOfSignature;
							string text2 = text;
							dateTime = dateOfSignature;
							text = text2 + string.Format("dateofsignature={0}&", dateTime.ToString("ddMMyyyy"));
						}
					}
					text += $"amount={amount:0.00}&".Replace(".", ",");
					if (!string.IsNullOrEmpty(reason))
					{
						text += $"reason={Uri.EscapeDataString(reason)}&";
					}
					text += $"currency={currency}&";
					string text3 = text;
					dateTime = executionDate;
					text = text3 + string.Format("executiondate={0}&", dateTime.ToString("ddMMyyyy"));
					if (authority.Equals(AuthorityType.periodicsinglepayment) || authority.Equals(AuthorityType.periodicsinglepaymentsepa))
					{
						text += $"periodictimeunit={periodicTimeunit}&";
						text += $"periodictimeunitrotation={periodicTimeunitRotation}&";
						_ = periodicFirstExecutionDate;
						string text4 = text;
						dateTime = periodicFirstExecutionDate;
						text = text4 + string.Format("periodicfirstexecutiondate={0}&", dateTime.ToString("ddMMyyyy"));
						_ = periodicLastExecutionDate;
						string text5 = text;
						dateTime = periodicLastExecutionDate;
						text = text5 + string.Format("periodiclastexecutiondate={0}&", dateTime.ToString("ddMMyyyy"));
					}
				}
				else
				{
					if (authority.Equals(AuthorityType.contact))
					{
						text += $"account={account}&";
						text += $"bnc={bnc}&";
					}
					else if (authority.Equals(AuthorityType.contact_v2))
					{
						if (!string.IsNullOrEmpty(account) && !string.IsNullOrEmpty(bnc))
						{
							text += $"account={account}&";
							text += $"bnc={bnc}&";
						}
						else
						{
							text += $"iban={iban}&";
							text += $"bic={bic}&";
						}
					}
					if (!string.IsNullOrEmpty(reason))
					{
						text += $"reason={Uri.EscapeDataString(reason)}&";
					}
				}
				return text.Trim(new char[1] { '&' });
			}
		}

		public class CalendarEvent : Payload
		{
			public enum EventEncoding
			{
				iCalComplete,
				Universal
			}

			private readonly string subject;

			private readonly string description;

			private readonly string location;

			private readonly string start;

			private readonly string end;

			private readonly EventEncoding encoding;

			public CalendarEvent(string subject, string description, string location, DateTime start, DateTime end, bool allDayEvent, EventEncoding encoding = EventEncoding.Universal)
			{
				this.subject = subject;
				this.description = description;
				this.location = location;
				this.encoding = encoding;
				string text = (allDayEvent ? "yyyyMMdd" : "yyyyMMddTHHmmss");
				this.start = start.ToString(text);
				this.end = end.ToString(text);
			}

			public override string ToString()
			{
				string text = $"BEGIN:VEVENT{Environment.NewLine}";
				text += $"SUMMARY:{subject}{Environment.NewLine}";
				text += ((!string.IsNullOrEmpty(description)) ? $"DESCRIPTION:{description}{Environment.NewLine}" : "");
				text += ((!string.IsNullOrEmpty(location)) ? $"LOCATION:{location}{Environment.NewLine}" : "");
				text += $"DTSTART:{start}{Environment.NewLine}";
				text += $"DTEND:{end}{Environment.NewLine}";
				text += "END:VEVENT";
				if (encoding.Equals(EventEncoding.iCalComplete))
				{
					text = $"BEGIN:VCALENDAR{Environment.NewLine}VERSION:2.0{Environment.NewLine}{text}{Environment.NewLine}END:VCALENDAR";
				}
				return text;
			}
		}

		public class OneTimePassword : Payload
		{
			public enum OneTimePasswordAuthType
			{
				TOTP,
				HOTP
			}

			public enum OneTimePasswordAuthAlgorithm
			{
				SHA1,
				SHA256,
				SHA512
			}

			[Obsolete("This enum is obsolete, use OneTimePasswordAuthAlgorithm instead", false)]
			public enum OoneTimePasswordAuthAlgorithm
			{
				SHA1,
				SHA256,
				SHA512
			}

			public OneTimePasswordAuthType Type { get; set; }

			public string Secret { get; set; }

			public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; }

			[Obsolete("This property is obsolete, use AuthAlgorithm instead", false)]
			public OoneTimePasswordAuthAlgorithm Algorithm
			{
				get
				{
					return (OoneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OoneTimePasswordAuthAlgorithm), AuthAlgorithm.ToString());
				}
				set
				{
					AuthAlgorithm = (OneTimePasswordAuthAlgorithm)Enum.Parse(typeof(OneTimePasswordAuthAlgorithm), value.ToString());
				}
			}

			public string Issuer { get; set; }

			public string Label { get; set; }

			public int Digits { get; set; } = 6;


			public int? Counter { get; set; }

			public int? Period { get; set; } = 30;


			public override string ToString()
			{
				return Type switch
				{
					OneTimePasswordAuthType.TOTP => TimeToString(), 
					OneTimePasswordAuthType.HOTP => HMACToString(), 
					_ => throw new ArgumentOutOfRangeException(), 
				};
			}

			private string HMACToString()
			{
				StringBuilder stringBuilder = new StringBuilder("otpauth://hotp/");
				ProcessCommonFields(stringBuilder);
				int num = Counter ?? 1;
				stringBuilder.Append("&counter=" + num);
				return stringBuilder.ToString();
			}

			private string TimeToString()
			{
				if (!Period.HasValue)
				{
					throw new Exception("Period must be set when using OneTimePasswordAuthType.TOTP");
				}
				StringBuilder stringBuilder = new StringBuilder("otpauth://totp/");
				ProcessCommonFields(stringBuilder);
				if (Period != 30)
				{
					stringBuilder.Append("&period=" + Period);
				}
				return stringBuilder.ToString();
			}

			private void ProcessCommonFields(StringBuilder sb)
			{
				if (String40Methods.IsNullOrWhiteSpace(Secret))
				{
					throw new Exception("Secret must be a filled out base32 encoded string");
				}
				string text = Secret.Replace(" ", "");
				string text2 = null;
				string text3 = null;
				if (!String40Methods.IsNullOrWhiteSpace(Issuer))
				{
					if (Issuer.Contains(":"))
					{
						throw new Exception("Issuer must not have a ':'");
					}
					text2 = Uri.EscapeUriString(Issuer);
				}
				if (!String40Methods.IsNullOrWhiteSpace(Label))
				{
					if (Label.Contains(":"))
					{
						throw new Exception("Label must not have a ':'");
					}
					text3 = Uri.EscapeUriString(Label);
				}
				if (text3 != null)
				{
					if (text2 != null)
					{
						text3 = text2 + ":" + text3;
					}
				}
				else if (text2 != null)
				{
					text3 = text2;
				}
				if (text3 != null)
				{
					sb.Append(text3);
				}
				sb.Append("?secret=" + text);
				if (text2 != null)
				{
					sb.Append("&issuer=" + text2);
				}
				if (Digits != 6)
				{
					sb.Append("&digits=" + Digits);
				}
			}
		}

		public class ShadowSocksConfig : Payload
		{
			public enum Method
			{
				Aes128Cfb,
				Aes128Cfb1,
				Aes128Cfb8,
				Aes128Ctr,
				Aes128Ofb,
				Aes192Cfb,
				Aes192Cfb1,
				Aes192Cfb8,
				Aes192Ctr,
				Aes192Ofb,
				Aes256Cb,
				Aes256Cfb1,
				Aes256Cfb8,
				Aes256Ctr,
				Aes256Ofb,
				BfCfb,
				Camellia128Cfb,
				Camellia192Cfb,
				Camellia256Cfb,
				Cast5Cfb,
				Chacha20,
				DesCfb,
				IdeaCfb,
				Rc2Cfb,
				Rc4,
				Rc4Md5,
				Salsa20,
				Salsa20Ctr,
				SeedCfb,
				Table
			}

			public class ShadowSocksConfigException : Exception
			{
				public ShadowSocksConfigException()
				{
				}

				public ShadowSocksConfigException(string message)
					: base(message)
				{
				}

				public ShadowSocksConfigException(string message, Exception inner)
					: base(message, inner)
				{
				}
			}

			private readonly string hostname;

			private readonly string password;

			private readonly string tag;

			private readonly string methodStr;

			private readonly Method method;

			private readonly int port;

			private Dictionary<string, string> encryptionTexts = new Dictionary<string, string>
			{
				{ "Aes128Cfb", "aes-128-cfb" },
				{ "Aes128Cfb1", "aes-128-cfb1" },
				{ "Aes128Cfb8", "aes-128-cfb8" },
				{ "Aes128Ctr", "aes-128-ctr" },
				{ "Aes128Ofb", "aes-128-ofb" },
				{ "Aes192Cfb", "aes-192-cfb" },
				{ "Aes192Cfb1", "aes-192-cfb1" },
				{ "Aes192Cfb8", "aes-192-cfb8" },
				{ "Aes192Ctr", "aes-192-ctr" },
				{ "Aes192Ofb", "aes-192-ofb" },
				{ "Aes256Cb", "aes-256-cfb" },
				{ "Aes256Cfb1", "aes-256-cfb1" },
				{ "Aes256Cfb8", "aes-256-cfb8" },
				{ "Aes256Ctr", "aes-256-ctr" },
				{ "Aes256Ofb", "aes-256-ofb" },
				{ "BfCfb", "bf-cfb" },
				{ "Camellia128Cfb", "camellia-128-cfb" },
				{ "Camellia192Cfb", "camellia-192-cfb" },
				{ "Camellia256Cfb", "camellia-256-cfb" },
				{ "Cast5Cfb", "cast5-cfb" },
				{ "Chacha20", "chacha20" },
				{ "DesCfb", "des-cfb" },
				{ "IdeaCfb", "idea-cfb" },
				{ "Rc2Cfb", "rc2-cfb" },
				{ "Rc4", "rc4" },
				{ "Rc4Md5", "rc4-md5" },
				{ "Salsa20", "salsa20" },
				{ "Salsa20Ctr", "salsa20-ctr" },
				{ "SeedCfb", "seed-cfb" },
				{ "Table", "table" }
			};

			public ShadowSocksConfig(string hostname, int port, string password, Method method, string tag = null)
			{
				this.hostname = hostname;
				if (port < 1 || port > 65535)
				{
					throw new ShadowSocksConfigException("Value of 'port' must be within 0 and 65535.");
				}
				this.port = port;
				this.password = password;
				this.method = method;
				methodStr = encryptionTexts[method.ToString()];
				this.tag = tag;
			}

			public override string ToString()
			{
				string s = $"{methodStr}:{password}@{hostname}:{port}";
				string arg = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
				return $"ss://{arg}{((!string.IsNullOrEmpty(tag)) ? $"#{tag}" : string.Empty)}";
			}
		}

		public class MoneroTransaction : Payload
		{
			public class MoneroTransactionException : Exception
			{
				public MoneroTransactionException()
				{
				}

				public MoneroTransactionException(string message)
					: base(message)
				{
				}

				public MoneroTransactionException(string message, Exception inner)
					: base(message, inner)
				{
				}
			}

			private readonly string address;

			private readonly string txPaymentId;

			private readonly string recipientName;

			private readonly string txDescription;

			private readonly float? txAmount;

			public MoneroTransaction(string address, float? txAmount = null, string txPaymentId = null, string recipientName = null, string txDescription = null)
			{
				if (string.IsNullOrEmpty(address))
				{
					throw new MoneroTransactionException("The address is mandatory and has to be set.");
				}
				this.address = address;
				if (txAmount.HasValue && txAmount <= 0f)
				{
					throw new MoneroTransactionException("Value of 'txAmount' must be greater than 0.");
				}
				this.txAmount = txAmount;
				this.txPaymentId = txPaymentId;
				this.recipientName = recipientName;
				this.txDescription = txDescription;
			}

			public override string ToString()
			{
				string arg = address;
				object arg2;
				float? num;
				if (string.IsNullOrEmpty(txPaymentId) && string.IsNullOrEmpty(recipientName) && string.IsNullOrEmpty(txDescription))
				{
					num = txAmount;
					if (!num.HasValue)
					{
						arg2 = string.Empty;
						goto IL_004e;
					}
				}
				arg2 = "?";
				goto IL_004e;
				IL_004e:
				string text = string.Concat($"monero://{arg}{arg2}" + ((!string.IsNullOrEmpty(txPaymentId)) ? $"tx_payment_id={Uri.EscapeDataString(txPaymentId)}&" : string.Empty), (!string.IsNullOrEmpty(recipientName)) ? $"recipient_name={Uri.EscapeDataString(recipientName)}&" : string.Empty);
				num = txAmount;
				return string.Concat(text + (num.HasValue ? string.Format("tx_amount={0}&", txAmount.ToString().Replace(",", ".")) : string.Empty), (!string.IsNullOrEmpty(txDescription)) ? $"tx_description={Uri.EscapeDataString(txDescription)}" : string.Empty).TrimEnd(new char[1] { '&' });
			}
		}

		public class SlovenianUpnQr : Payload
		{
			private string _payerName = "";

			private string _payerAddress = "";

			private string _payerPlace = "";

			private string _amount = "";

			private string _code = "";

			private string _purpose = "";

			private string _deadLine = "";

			private string _recipientIban = "";

			private string _recipientName = "";

			private string _recipientAddress = "";

			private string _recipientPlace = "";

			private string _recipientSiModel = "";

			private string _recipientSiReference = "";

			public override int Version => 15;

			public override QRCodeGenerator.ECCLevel EccLevel => QRCodeGenerator.ECCLevel.M;

			public override QRCodeGenerator.EciMode EciMode => QRCodeGenerator.EciMode.Iso8859_2;

			private string LimitLength(string value, int maxLength)
			{
				if (value.Length > maxLength)
				{
					return value.Substring(0, maxLength);
				}
				return value;
			}

			public SlovenianUpnQr(string payerName, string payerAddress, string payerPlace, string recipientName, string recipientAddress, string recipientPlace, string recipientIban, string description, double amount, string recipientSiModel = "SI00", string recipientSiReference = "", string code = "OTHR")
				: this(payerName, payerAddress, payerPlace, recipientName, recipientAddress, recipientPlace, recipientIban, description, amount, null, recipientSiModel, recipientSiReference, code)
			{
			}

			public SlovenianUpnQr(string payerName, string payerAddress, string payerPlace, string recipientName, string recipientAddress, string recipientPlace, string recipientIban, string description, double amount, DateTime? deadline, string recipientSiModel = "SI99", string recipientSiReference = "", string code = "OTHR")
			{
				_payerName = LimitLength(payerName.Trim(), 33);
				_payerAddress = LimitLength(payerAddress.Trim(), 33);
				_payerPlace = LimitLength(payerPlace.Trim(), 33);
				_amount = FormatAmount(amount);
				_code = LimitLength(code.Trim().ToUpper(), 4);
				_purpose = LimitLength(description.Trim(), 42);
				_deadLine = ((!deadline.HasValue) ? "" : deadline?.ToString("dd.MM.yyyy"));
				_recipientIban = LimitLength(recipientIban.Trim(), 34);
				_recipientName = LimitLength(recipientName.Trim(), 33);
				_recipientAddress = LimitLength(recipientAddress.Trim(), 33);
				_recipientPlace = LimitLength(recipientPlace.Trim(), 33);
				_recipientSiModel = LimitLength(recipientSiModel.Trim().ToUpper(), 4);
				_recipientSiReference = LimitLength(recipientSiReference.Trim(), 22);
			}

			private string FormatAmount(double amount)
			{
				int num = (int)Math.Round(amount * 100.0);
				return $"{num:00000000000}";
			}

			private int CalculateChecksum()
			{
				return 5 + _payerName.Length + _payerAddress.Length + _payerPlace.Length + _amount.Length + _code.Length + _purpose.Length + _deadLine.Length + _recipientIban.Length + _recipientName.Length + _recipientAddress.Length + _recipientPlace.Length + _recipientSiModel.Length + _recipientSiReference.Length + 19;
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("UPNQR");
				stringBuilder.Append('\n').Append('\n').Append('\n')
					.Append('\n')
					.Append('\n');
				stringBuilder.Append(_payerName).Append('\n');
				stringBuilder.Append(_payerAddress).Append('\n');
				stringBuilder.Append(_payerPlace).Append('\n');
				stringBuilder.Append(_amount).Append('\n').Append('\n')
					.Append('\n');
				stringBuilder.Append(_code.ToUpper()).Append('\n');
				stringBuilder.Append(_purpose).Append('\n');
				stringBuilder.Append(_deadLine).Append('\n');
				stringBuilder.Append(_recipientIban.ToUpper()).Append('\n');
				stringBuilder.Append(_recipientSiModel).Append(_recipientSiReference).Append('\n');
				stringBuilder.Append(_recipientName).Append('\n');
				stringBuilder.Append(_recipientAddress).Append('\n');
				stringBuilder.Append(_recipientPlace).Append('\n');
				stringBuilder.AppendFormat("{0:000}", CalculateChecksum()).Append('\n');
				return stringBuilder.ToString();
			}
		}

		private static bool IsValidIban(string iban)
		{
			return Regex.IsMatch(iban.Replace(" ", ""), "^[a-zA-Z]{2}[0-9]{2}[a-zA-Z0-9]{4}[0-9]{7}([a-zA-Z0-9]?){0,16}$");
		}

		private static bool IsValidBic(string bic)
		{
			return Regex.IsMatch(bic.Replace(" ", ""), "^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
		}

		private static string ConvertStringToEncoding(string message, string encoding)
		{
			Encoding encoding2 = Encoding.GetEncoding(encoding);
			Encoding uTF = Encoding.UTF8;
			byte[] bytes = uTF.GetBytes(message);
			byte[] bytes2 = Encoding.Convert(uTF, encoding2, bytes);
			return encoding2.GetString(bytes2);
		}

		private static string EscapeInput(string inp, bool simple = false)
		{
			char[] array = new char[4] { '\\', ';', ',', ':' };
			if (simple)
			{
				array = new char[1] { ':' };
			}
			char[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char c = array2[i];
				inp = inp.Replace(c.ToString(), "\\" + c);
			}
			return inp;
		}

		public static bool ChecksumMod10(string digits)
		{
			if (string.IsNullOrEmpty(digits) || digits.Length < 2)
			{
				return false;
			}
			int[] array = new int[10] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };
			int num = 0;
			for (int i = 0; i < digits.Length - 1; i++)
			{
				int num2 = Convert.ToInt32(digits[i]) - 48;
				num = array[(num2 + num) % 10];
			}
			return (10 - num) % 10 == Convert.ToInt32(digits[digits.Length - 1]) - 48;
		}

		private static bool isHexStyle(string inp)
		{
			if (!Regex.IsMatch(inp, "\\A\\b[0-9a-fA-F]+\\b\\Z"))
			{
				return Regex.IsMatch(inp, "\\A\\b(0[xX])?[0-9a-fA-F]+\\b\\Z");
			}
			return true;
		}
	}
	public sealed class PngByteQRCode : AbstractQRCode, IDisposable
	{
		private sealed class PngBuilder : IDisposable
		{
			public enum ColorType : byte
			{
				Greyscale = 0,
				Indexed = 3
			}

			private static readonly byte[] PngSignature = new byte[8] { 137, 80, 78, 71, 13, 10, 26, 10 };

			private static readonly uint[] CrcTable = new uint[256]
			{
				0u, 1996959894u, 3993919788u, 2567524794u, 124634137u, 1886057615u, 3915621685u, 2657392035u, 249268274u, 2044508324u,
				3772115230u, 2547177864u, 162941995u, 2125561021u, 3887607047u, 2428444049u, 498536548u, 1789927666u, 4089016648u, 2227061214u,
				450548861u, 1843258603u, 4107580753u, 2211677639u, 325883990u, 1684777152u, 4251122042u, 2321926636u, 335633487u, 1661365465u,
				4195302755u, 2366115317u, 997073096u, 1281953886u, 3579855332u, 2724688242u, 1006888145u, 1258607687u, 3524101629u, 2768942443u,
				901097722u, 1119000684u, 3686517206u, 2898065728u, 853044451u, 1172266101u, 3705015759u, 2882616665u, 651767980u, 1373503546u,
				3369554304u, 3218104598u, 565507253u, 1454621731u, 3485111705u, 3099436303u, 671266974u, 1594198024u, 3322730930u, 2970347812u,
				795835527u, 1483230225u, 3244367275u, 3060149565u, 1994146192u, 31158534u, 2563907772u, 4023717930u, 1907459465u, 112637215u,
				2680153253u, 3904427059u, 2013776290u, 251722036u, 2517215374u, 3775830040u, 2137656763u, 141376813u, 2439277719u, 3865271297u,
				1802195444u, 476864866u, 2238001368u, 4066508878u, 1812370925u, 453092731u, 2181625025u, 4111451223u, 1706088902u, 314042704u,
				2344532202u, 4240017532u, 1658658271u, 366619977u, 2362670323u, 4224994405u, 1303535960u, 984961486u, 2747007092u, 3569037538u,
				1256170817u, 1037604311u, 2765210733u, 3554079995u, 1131014506u, 879679996u, 2909243462u, 3663771856u, 1141124467u, 855842277u,
				2852801631u, 3708648649u, 1342533948u, 654459306u, 3188396048u, 3373015174u, 1466479909u, 544179635u, 3110523913u, 3462522015u,
				1591671054u, 702138776u, 2966460450u, 3352799412u, 1504918807u, 783551873u, 3082640443u, 3233442989u, 3988292384u, 2596254646u,
				62317068u, 1957810842u, 3939845945u, 2647816111u, 81470997u, 1943803523u, 3814918930u, 2489596804u, 225274430u, 2053790376u,
				3826175755u, 2466906013u, 167816743u, 2097651377u, 4027552580u, 2265490386u, 503444072u, 1762050814u, 4150417245u, 2154129355u,
				426522225u, 1852507879u, 4275313526u, 2312317920u, 282753626u, 1742555852u, 4189708143u, 2394877945u, 397917763u, 1622183637u,
				3604390888u, 2714866558u, 953729732u, 1340076626u, 3518719985u, 2797360999u, 1068828381u, 1219638859u, 3624741850u, 2936675148u,
				906185462u, 1090812512u, 3747672003u, 2825379669u, 829329135u, 1181335161u, 3412177804u, 3160834842u, 628085408u, 1382605366u,
				3423369109u, 3138078467u, 570562233u, 1426400815u, 3317316542u, 2998733608u, 733239954u, 1555261956u, 3268935591u, 3050360625u,
				752459403u, 1541320221u, 2607071920u, 3965973030u, 1969922972u, 40735498u, 2617837225u, 3943577151u, 1913087877u, 83908371u,
				2512341634u, 3803740692u, 2075208622u, 213261112u, 2463272603u, 3855990285u, 2094854071u, 198958881u, 2262029012u, 4057260610u,
				1759359992u, 534414190u, 2176718541u, 4139329115u, 1873836001u, 414664567u, 2282248934u, 4279200368u, 1711684554u, 285281116u,
				2405801727u, 4167216745u, 1634467795u, 376229701u, 2685067896u, 3608007406u, 1308918612u, 956543938u, 2808555105u, 3495958263u,
				1231636301u, 1047427035u, 2932959818u, 3654703836u, 1088359270u, 936918000u, 2847714899u, 3736837829u, 1202900863u, 817233897u,
				3183342108u, 3401237130u, 1404277552u, 615818150u, 3134207493u, 3453421203u, 1423857449u, 601450431u, 3009837614u, 3294710456u,
				1567103746u, 711928724u, 3020668471u, 3272380065u, 1510334235u, 755167117u
			};

			private static readonly byte[] IHDR = new byte[4] { 73, 72, 68, 82 };

			private static readonly byte[] IDAT = new byte[4] { 73, 68, 65, 84 };

			private static readonly byte[] IEND = new byte[4] { 73, 69, 78, 68 };

			private static readonly byte[] PLTE = new byte[4] { 80, 76, 84, 69 };

			private static readonly byte[] tRNS = new byte[4] { 116, 82, 78, 83 };

			private MemoryStream stream = new MemoryStream();

			public void Dispose()
			{
				stream?.Dispose();
				stream = null;
			}

			public byte[] GetBytes()
			{
				byte[] array = stream.ToArray();
				int num = PngSignature.Length;
				while (num < array.Length)
				{
					int num2 = (array[num] << 24) | (array[num + 1] << 16) | (array[num + 2] << 8) | array[num + 3];
					uint num3 = Crc32(array, num + 4, num2 + 4);
					int num4 = num + 8 + num2;
					array[num4] = (byte)(num3 >> 24);
					array[num4 + 1] = (byte)(num3 >> 16);
					array[num4 + 2] = (byte)(num3 >> 8);
					array[num4 + 3] = (byte)num3;
					num = num4 + 4;
				}
				return array;
			}

			public void WriteHeader(int width, int height, byte bitDepth, ColorType colorType)
			{
				stream.Write(PngSignature, 0, PngSignature.Length);
				WriteChunkStart(IHDR, 13);
				WriteIntBigEndian((uint)width);
				WriteIntBigEndian((uint)height);
				stream.WriteByte(bitDepth);
				stream.WriteByte((byte)colorType);
				stream.WriteByte(0);
				stream.WriteByte(0);
				stream.WriteByte(0);
				WriteChunkEnd();
			}

			public void WritePalette(params byte[][] rgbaColors)
			{
				bool flag = false;
				WriteChunkStart(PLTE, 3 * rgbaColors.Length);
				byte[][] array = rgbaColors;
				foreach (byte[] array2 in array)
				{
					flag |= array2.Length > 3 && array2[3] < byte.MaxValue;
					stream.WriteByte(array2[0]);
					stream.WriteByte(array2[1]);
					stream.WriteByte(array2[2]);
				}
				WriteChunkEnd();
				if (flag)
				{
					WriteChunkStart(tRNS, rgbaColors.Length);
					array = rgbaColors;
					foreach (byte[] array3 in array)
					{
						stream.WriteByte((array3.Length > 3) ? array3[3] : byte.MaxValue);
					}
					WriteChunkEnd();
				}
			}

			public void WriteScanlines(byte[] scanlines)
			{
				using MemoryStream memoryStream = new MemoryStream();
				Deflate(memoryStream, scanlines);
				WriteChunkStart(IDAT, (int)(memoryStream.Length + 6));
				stream.WriteByte(120);
				stream.WriteByte(156);
				memoryStream.Position = 0L;
				memoryStream.WriteTo(stream);
				uint value = Adler32(scanlines, 0, scanlines.Length);
				WriteIntBigEndian(value);
				WriteChunkEnd();
			}

			public void WriteEnd()
			{
				WriteChunkStart(IEND, 0);
				WriteChunkEnd();
			}

			private void WriteChunkStart(byte[] type, int length)
			{
				WriteIntBigEndian((uint)length);
				stream.Write(type, 0, 4);
			}

			private void WriteChunkEnd()
			{
				stream.SetLength(stream.Length + 4);
				stream.Position += 4L;
			}

			private void WriteIntBigEndian(uint value)
			{
				stream.WriteByte((byte)(value >> 24));
				stream.WriteByte((byte)(value >> 16));
				stream.WriteByte((byte)(value >> 8));
				stream.WriteByte((byte)value);
			}

			private static void Deflate(Stream output, byte[] bytes)
			{
				using DeflateStream deflateStream = new DeflateStream(output, CompressionMode.Compress, leaveOpen: true);
				deflateStream.Write(bytes, 0, bytes.Length);
			}

			private static uint Adler32(byte[] data, int index, int length)
			{
				uint num = 1u;
				uint num2 = 0u;
				int num3 = index + length;
				for (int i = index; i < num3; i++)
				{
					num = (num + data[i]) % 65521;
					num2 = (num2 + num) % 65521;
				}
				return (num2 << 16) + num;
			}

			private static uint Crc32(byte[] data, int index, int length)
			{
				uint num = uint.MaxValue;
				int num2 = index + length;
				for (int i = index; i < num2; i++)
				{
					num = CrcTable[(num ^ data[i]) & 0xFF] ^ (num >> 8);
				}
				return num ^ 0xFFFFFFFFu;
			}
		}

		public PngByteQRCode()
		{
		}

		public PngByteQRCode(QRCodeData data)
			: base(data)
		{
		}

		public byte[] GetGraphic(int pixelsPerModule)
		{
			using PngBuilder pngBuilder = new PngBuilder();
			int num = base.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
			pngBuilder.WriteHeader(num, num, 1, PngBuilder.ColorType.Greyscale);
			pngBuilder.WriteScanlines(DrawScanlines(pixelsPerModule));
			pngBuilder.WriteEnd();
			return pngBuilder.GetBytes();
		}

		public byte[] GetGraphic(int pixelsPerModule, byte[] darkColorRgba, byte[] lightColorRgba)
		{
			using PngBuilder pngBuilder = new PngBuilder();
			int num = base.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
			pngBuilder.WriteHeader(num, num, 1, PngBuilder.ColorType.Indexed);
			pngBuilder.WritePalette(darkColorRgba, lightColorRgba);
			pngBuilder.WriteScanlines(DrawScanlines(pixelsPerModule));
			pngBuilder.WriteEnd();
			return pngBuilder.GetBytes();
		}

		private byte[] DrawScanlines(int pixelsPerModule)
		{
			List<BitArray> moduleMatrix = base.QrCodeData.ModuleMatrix;
			int count = moduleMatrix.Count;
			int num = (count * pixelsPerModule + 7) / 8 + 1;
			byte[] array = new byte[num * count * pixelsPerModule];
			for (int i = 0; i < count; i++)
			{
				BitArray bitArray = moduleMatrix[i];
				int num2 = i * pixelsPerModule * num;
				for (int j = 0; j < count; j++)
				{
					if (!bitArray[j])
					{
						int k = j * pixelsPerModule;
						for (int num3 = k + pixelsPerModule; k < num3; k++)
						{
							array[num2 + 1 + k / 8] |= (byte)(128 >> k % 8);
						}
					}
				}
				for (int l = 1; l < pixelsPerModule; l++)
				{
					Array.Copy(array, num2, array, num2 + l * num, num);
				}
			}
			return array;
		}
	}
	public class PostscriptQRCode : AbstractQRCode, IDisposable
	{
		private const string psHeader = "%!PS-Adobe-3.0 {3}\r\n%%Creator: QRCoder.NET\r\n%%Title: QRCode\r\n%%CreationDate: {0}\r\n%%DocumentData: Clean7Bit\r\n%%Origin: 0\r\n%%DocumentMedia: Default {1} {1} 0 () ()\r\n%%BoundingBox: 0 0 {1} {1}\r\n%%LanguageLevel: 2 \r\n%%Pages: 1\r\n%%Page: 1 1\r\n%%EndComments\r\n%%BeginConstants\r\n/sz {1} def\r\n/sc {2} def\r\n%%EndConstants\r\n%%BeginFeature: *PageSize Default\r\n<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice\r\n%%EndFeature\r\n";

		private const string psFunctions = "%%BeginFunctions \r\n/csquare {{\r\n    newpath\r\n    0 0 moveto\r\n    0 1 rlineto\r\n    1 0 rlineto\r\n    0 -1 rlineto\r\n    closepath\r\n    setrgbcolor\r\n    fill\r\n}} def\r\n/f {{ \r\n    {0} {1} {2} csquare\r\n    1 0 translate\r\n}} def\r\n/b {{ \r\n    1 0 translate\r\n}} def \r\n/background {{ \r\n    {3} {4} {5} csquare \r\n}} def\r\n/nl {{\r\n    -{6} -1 translate\r\n}} def\r\n%%EndFunctions\r\n%%BeginBody\r\n0 0 moveto\r\ngsave\r\nsz sz scale\r\nbackground\r\ngrestore\r\ngsave\r\nsc sc scale\r\n0 {6} 1 sub translate\r\n";

		private const string psFooter = "%%EndBody\r\ngrestore\r\nshowpage   \r\n%%EOF\r\n";

		public PostscriptQRCode()
		{
		}

		public PostscriptQRCode(QRCodeData data)
			: base(data)
		{
		}

		public string GetGraphic(int pointsPerModule, bool epsFormat = false)
		{
			Size viewBox = new Size(pointsPerModule * base.QrCodeData.ModuleMatrix.Count, pointsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones: true, epsFormat);
		}

		public string GetGraphic(int pointsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, bool epsFormat = false)
		{
			Size viewBox = new Size(pointsPerModule * base.QrCodeData.ModuleMatrix.Count, pointsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, darkColor, lightColor, drawQuietZones, epsFormat);
		}

		public string GetGraphic(int pointsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true, bool epsFormat = false)
		{
			Size viewBox = new Size(pointsPerModule * base.QrCodeData.ModuleMatrix.Count, pointsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, darkColorHex, lightColorHex, drawQuietZones, epsFormat);
		}

		public string GetGraphic(Size viewBox, bool drawQuietZones = true, bool epsFormat = false)
		{
			return GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones, epsFormat);
		}

		public string GetGraphic(Size viewBox, string darkColorHex, string lightColorHex, bool drawQuietZones = true, bool epsFormat = false)
		{
			return GetGraphic(viewBox, ColorTranslator.FromHtml(darkColorHex), ColorTranslator.FromHtml(lightColorHex), drawQuietZones, epsFormat);
		}

		public string GetGraphic(Size viewBox, Color darkColor, Color lightColor, bool drawQuietZones = true, bool epsFormat = false)
		{
			int num = ((!drawQuietZones) ? 4 : 0);
			int num2 = base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? (num * 2) : 0);
			double input = (double)Math.Min(viewBox.Width, viewBox.Height) / (double)num2;
			string text = string.Format("%!PS-Adobe-3.0 {3}\r\n%%Creator: QRCoder.NET\r\n%%Title: QRCode\r\n%%CreationDate: {0}\r\n%%DocumentData: Clean7Bit\r\n%%Origin: 0\r\n%%DocumentMedia: Default {1} {1} 0 () ()\r\n%%BoundingBox: 0 0 {1} {1}\r\n%%LanguageLevel: 2 \r\n%%Pages: 1\r\n%%Page: 1 1\r\n%%EndComments\r\n%%BeginConstants\r\n/sz {1} def\r\n/sc {2} def\r\n%%EndConstants\r\n%%BeginFeature: *PageSize Default\r\n<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice\r\n%%EndFeature\r\n", DateTime.Now.ToString("s"), CleanSvgVal(viewBox.Width), CleanSvgVal(input), epsFormat ? "EPSF-3.0" : string.Empty);
			text += string.Format("%%BeginFunctions \r\n/csquare {{\r\n    newpath\r\n    0 0 moveto\r\n    0 1 rlineto\r\n    1 0 rlineto\r\n    0 -1 rlineto\r\n    closepath\r\n    setrgbcolor\r\n    fill\r\n}} def\r\n/f {{ \r\n    {0} {1} {2} csquare\r\n    1 0 translate\r\n}} def\r\n/b {{ \r\n    1 0 translate\r\n}} def \r\n/background {{ \r\n    {3} {4} {5} csquare \r\n}} def\r\n/nl {{\r\n    -{6} -1 translate\r\n}} def\r\n%%EndFunctions\r\n%%BeginBody\r\n0 0 moveto\r\ngsave\r\nsz sz scale\r\nbackground\r\ngrestore\r\ngsave\r\nsc sc scale\r\n0 {6} 1 sub translate\r\n", CleanSvgVal((double)(int)darkColor.R / 255.0), CleanSvgVal((double)(int)darkColor.G / 255.0), CleanSvgVal((double)(int)darkColor.B / 255.0), CleanSvgVal((double)(int)lightColor.R / 255.0), CleanSvgVal((double)(int)lightColor.G / 255.0), CleanSvgVal((double)(int)lightColor.B / 255.0), num2);
			for (int i = num; i < num + num2; i++)
			{
				if (i > num)
				{
					text += "nl\n";
				}
				for (int j = num; j < num + num2; j++)
				{
					text += (base.QrCodeData.ModuleMatrix[i][j] ? "f " : "b ");
				}
				text += "\n";
			}
			return text + "%%EndBody\r\ngrestore\r\nshowpage   \r\n%%EOF\r\n";
		}

		private string CleanSvgVal(double input)
		{
			return input.ToString(CultureInfo.InvariantCulture);
		}
	}
	public class QRCodeData : IDisposable
	{
		public enum Compression
		{
			Uncompressed,
			Deflate,
			GZip
		}

		public List<BitArray> ModuleMatrix { get; set; }

		public int Version { get; private set; }

		public QRCodeData(int version)
		{
			Version = version;
			int num = ModulesPerSideFromVersion(version);
			ModuleMatrix = new List<BitArray>();
			for (int i = 0; i < num; i++)
			{
				ModuleMatrix.Add(new BitArray(num));
			}
		}

		public QRCodeData(string pathToRawData, Compression compressMode)
			: this(File.ReadAllBytes(pathToRawData), compressMode)
		{
		}

		public QRCodeData(byte[] rawData, Compression compressMode)
		{
			List<byte> list = new List<byte>(rawData);
			if (compressMode.Equals(Compression.Deflate))
			{
				using MemoryStream stream = new MemoryStream(list.ToArray());
				using MemoryStream memoryStream = new MemoryStream();
				using (DeflateStream input = new DeflateStream(stream, CompressionMode.Decompress))
				{
					Stream4Methods.CopyTo(input, memoryStream);
				}
				list = new List<byte>(memoryStream.ToArray());
			}
			else if (compressMode.Equals(Compression.GZip))
			{
				using MemoryStream stream2 = new MemoryStream(list.ToArray());
				using MemoryStream memoryStream2 = new MemoryStream();
				using (GZipStream input2 = new GZipStream(stream2, CompressionMode.Decompress))
				{
					Stream4Methods.CopyTo(input2, memoryStream2);
				}
				list = new List<byte>(memoryStream2.ToArray());
			}
			if (list[0] != 81 || list[1] != 82 || list[2] != 82)
			{
				throw new Exception("Invalid raw data file. Filetype doesn't match \"QRR\".");
			}
			int num = list[4];
			list.RemoveRange(0, 5);
			Version = (num - 21 - 8) / 4 + 1;
			Queue<bool> queue = new Queue<bool>();
			foreach (byte item in list)
			{
				new BitArray(new byte[1] { item });
				for (int num2 = 7; num2 >= 0; num2--)
				{
					queue.Enqueue((item & (1 << num2)) != 0);
				}
			}
			ModuleMatrix = new List<BitArray>();
			for (int i = 0; i < num; i++)
			{
				ModuleMatrix.Add(new BitArray(num));
				for (int j = 0; j < num; j++)
				{
					ModuleMatrix[i][j] = queue.Dequeue();
				}
			}
		}

		public byte[] GetRawData(Compression compressMode)
		{
			List<byte> list = new List<byte>();
			list.AddRange(new byte[4] { 81, 82, 82, 0 });
			list.Add((byte)ModuleMatrix.Count);
			Queue<int> queue = new Queue<int>();
			foreach (BitArray item in ModuleMatrix)
			{
				foreach (object item2 in item)
				{
					queue.Enqueue(((bool)item2) ? 1 : 0);
				}
			}
			for (int i = 0; i < 8 - ModuleMatrix.Count * ModuleMatrix.Count % 8; i++)
			{
				queue.Enqueue(0);
			}
			while (queue.Count > 0)
			{
				byte b = 0;
				for (int num = 7; num >= 0; num--)
				{
					b += (byte)(queue.Dequeue() << num);
				}
				list.Add(b);
			}
			byte[] array = list.ToArray();
			if (compressMode.Equals(Compression.Deflate))
			{
				using MemoryStream memoryStream = new MemoryStream();
				using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress))
				{
					deflateStream.Write(array, 0, array.Length);
				}
				array = memoryStream.ToArray();
			}
			else if (compressMode.Equals(Compression.GZip))
			{
				using MemoryStream memoryStream2 = new MemoryStream();
				using (GZipStream gZipStream = new GZipStream(memoryStream2, CompressionMode.Compress, leaveOpen: true))
				{
					gZipStream.Write(array, 0, array.Length);
				}
				array = memoryStream2.ToArray();
			}
			return array;
		}

		public void SaveRawData(string filePath, Compression compressMode)
		{
			File.WriteAllBytes(filePath, GetRawData(compressMode));
		}

		private static int ModulesPerSideFromVersion(int version)
		{
			return 21 + (version - 1) * 4;
		}

		public void Dispose()
		{
			ModuleMatrix = null;
			Version = 0;
		}
	}
	public class QRCode : AbstractQRCode, IDisposable
	{
		public QRCode()
		{
		}

		public QRCode(QRCodeData data)
			: base(data)
		{
		}

		public Bitmap GetGraphic(int pixelsPerModule)
		{
			return GetGraphic(pixelsPerModule, Color.Black, Color.White, drawQuietZones: true);
		}

		public Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
		{
			return GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones);
		}

		public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			int num = (base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? 8 : 0)) * pixelsPerModule;
			int num2 = ((!drawQuietZones) ? (4 * pixelsPerModule) : 0);
			Bitmap val = new Bitmap(num, num);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			for (int i = 0; i < num + num2; i += pixelsPerModule)
			{
				for (int j = 0; j < num + num2; j += pixelsPerModule)
				{
					if (base.QrCodeData.ModuleMatrix[(j + pixelsPerModule) / pixelsPerModule - 1][(i + pixelsPerModule) / pixelsPerModule - 1])
					{
						val2.FillRectangle((Brush)new SolidBrush(darkColor), new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
					}
					else
					{
						val2.FillRectangle((Brush)new SolidBrush(lightColor), new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
					}
				}
			}
			val2.Save();
			return val;
		}

		public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Expected O, but got Unknown
			int num = (base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? 8 : 0)) * pixelsPerModule;
			int num2 = ((!drawQuietZones) ? (4 * pixelsPerModule) : 0);
			Bitmap val = new Bitmap(num, num, (PixelFormat)137224);
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			val2.InterpolationMode = (InterpolationMode)7;
			val2.CompositingQuality = (CompositingQuality)2;
			val2.Clear(lightColor);
			bool flag = icon != null && iconSizePercent > 0 && iconSizePercent <= 100;
			GraphicsPath val3 = null;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			if (flag)
			{
				num3 = (float)(iconSizePercent * ((Image)val).Width) / 100f;
				num4 = (flag ? (num3 * (float)((Image)icon).Height / (float)((Image)icon).Width) : 0f);
				num5 = ((float)((Image)val).Width - num3) / 2f;
				num6 = ((float)((Image)val).Height - num4) / 2f;
				RectangleF rect = new RectangleF(num5 - (float)iconBorderWidth, num6 - (float)iconBorderWidth, num3 + (float)(iconBorderWidth * 2), num4 + (float)(iconBorderWidth * 2));
				val3 = CreateRoundedRectanglePath(rect, iconBorderWidth * 2);
			}
			SolidBrush val4 = new SolidBrush(lightColor);
			SolidBrush val5 = new SolidBrush(darkColor);
			for (int i = 0; i < num + num2; i += pixelsPerModule)
			{
				for (int j = 0; j < num + num2; j += pixelsPerModule)
				{
					if (base.QrCodeData.ModuleMatrix[(j + pixelsPerModule) / pixelsPerModule - 1][(i + pixelsPerModule) / pixelsPerModule - 1])
					{
						Rectangle rectangle = new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule);
						if (flag)
						{
							Region val6 = new Region(rectangle);
							val6.Exclude(val3);
							val2.FillRegion((Brush)(object)val5, val6);
						}
						else
						{
							val2.FillRectangle((Brush)(object)val5, rectangle);
						}
					}
					else
					{
						val2.FillRectangle((Brush)(object)val4, new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
					}
				}
			}
			if (flag)
			{
				RectangleF rectangleF = new RectangleF(num5, num6, num3, num4);
				val2.DrawImage((Image)(object)icon, rectangleF, new RectangleF(0f, 0f, ((Image)icon).Width, ((Image)icon).Height), (GraphicsUnit)2);
			}
			val2.Save();
			return val;
		}

		internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Expected O, but got Unknown
			GraphicsPath val = new GraphicsPath();
			val.AddArc(rect.X, rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 180f, 90f);
			val.AddLine(rect.X + (float)cornerRadius, rect.Y, rect.Right - (float)(cornerRadius * 2), rect.Y);
			val.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y, (float)(cornerRadius * 2), (float)(cornerRadius * 2), 270f, 90f);
			val.AddLine(rect.Right, rect.Y + (float)(cornerRadius * 2), rect.Right, rect.Y + rect.Height - (float)(cornerRadius * 2));
			val.AddArc(rect.X + rect.Width - (float)(cornerRadius * 2), rect.Y + rect.Height - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 0f, 90f);
			val.AddLine(rect.Right - (float)(cornerRadius * 2), rect.Bottom, rect.X + (float)(cornerRadius * 2), rect.Bottom);
			val.AddArc(rect.X, rect.Bottom - (float)(cornerRadius * 2), (float)(cornerRadius * 2), (float)(cornerRadius * 2), 90f, 90f);
			val.AddLine(rect.X, rect.Bottom - (float)(cornerRadius * 2), rect.X, rect.Y + (float)(cornerRadius * 2));
			val.CloseFigure();
			return val;
		}
	}
	public class QRCodeGenerator : IDisposable
	{
		public enum EciMode
		{
			Default = 0,
			Iso8859_1 = 3,
			Iso8859_2 = 4,
			Utf8 = 26
		}

		private static class ModulePlacer
		{
			private static class MaskPattern
			{
				public static bool Pattern1(int x, int y)
				{
					return (x + y) % 2 == 0;
				}

				public static bool Pattern2(int x, int y)
				{
					return y % 2 == 0;
				}

				public static bool Pattern3(int x, int y)
				{
					return x % 3 == 0;
				}

				public static bool Pattern4(int x, int y)
				{
					return (x + y) % 3 == 0;
				}

				public static bool Pattern5(int x, int y)
				{
					return (int)(Math.Floor((double)y / 2.0) + Math.Floor((double)x / 3.0)) % 2 == 0;
				}

				public static bool Pattern6(int x, int y)
				{
					return x * y % 2 + x * y % 3 == 0;
				}

				public static bool Pattern7(int x, int y)
				{
					return (x * y % 2 + x * y % 3) % 2 == 0;
				}

				public static bool Pattern8(int x, int y)
				{
					return ((x + y) % 2 + x * y % 3) % 2 == 0;
				}

				public static int Score(ref QRCodeData qrCode)
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int count = qrCode.ModuleMatrix.Count;
					for (int i = 0; i < count; i++)
					{
						int num5 = 0;
						int num6 = 0;
						bool flag = qrCode.ModuleMatrix[i][0];
						bool flag2 = qrCode.ModuleMatrix[0][i];
						for (int j = 0; j < count; j++)
						{
							num5 = ((qrCode.ModuleMatrix[i][j] != flag) ? 1 : (num5 + 1));
							if (num5 == 5)
							{
								num += 3;
							}
							else if (num5 > 5)
							{
								num++;
							}
							flag = qrCode.ModuleMatrix[i][j];
							num6 = ((qrCode.ModuleMatrix[j][i] != flag2) ? 1 : (num6 + 1));
							if (num6 == 5)
							{
								num += 3;
							}
							else if (num6 > 5)
							{
								num++;
							}
							flag2 = qrCode.ModuleMatrix[j][i];
						}
					}
					for (int k = 0; k < count - 1; k++)
					{
						for (int l = 0; l < count - 1; l++)
						{
							if (qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k][l + 1] && qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k + 1][l] && qrCode.ModuleMatrix[k][l] == qrCode.ModuleMatrix[k + 1][l + 1])
							{
								num2 += 3;
							}
						}
					}
					for (int m = 0; m < count; m++)
					{
						for (int n = 0; n < count - 10; n++)
						{
							if ((qrCode.ModuleMatrix[m][n] && !qrCode.ModuleMatrix[m][n + 1] && qrCode.ModuleMatrix[m][n + 2] && qrCode.ModuleMatrix[m][n + 3] && qrCode.ModuleMatrix[m][n + 4] && !qrCode.ModuleMatrix[m][n + 5] && qrCode.ModuleMatrix[m][n + 6] && !qrCode.ModuleMatrix[m][n + 7] && !qrCode.ModuleMatrix[m][n + 8] && !qrCode.ModuleMatrix[m][n + 9] && !qrCode.ModuleMatrix[m][n + 10]) || (!qrCode.ModuleMatrix[m][n] && !qrCode.ModuleMatrix[m][n + 1] && !qrCode.ModuleMatrix[m][n + 2] && !qrCode.ModuleMatrix[m][n + 3] && qrCode.ModuleMatrix[m][n + 4] && !qrCode.ModuleMatrix[m][n + 5] && qrCode.ModuleMatrix[m][n + 6] && qrCode.ModuleMatrix[m][n + 7] && qrCode.ModuleMatrix[m][n + 8] && !qrCode.ModuleMatrix[m][n + 9] && qrCode.ModuleMatrix[m][n + 10]))
							{
								num3 += 40;
							}
							if ((qrCode.ModuleMatrix[n][m] && !qrCode.ModuleMatrix[n + 1][m] && qrCode.ModuleMatrix[n + 2][m] && qrCode.ModuleMatrix[n + 3][m] && qrCode.ModuleMatrix[n + 4][m] && !qrCode.ModuleMatrix[n + 5][m] && qrCode.ModuleMatrix[n + 6][m] && !qrCode.ModuleMatrix[n + 7][m] && !qrCode.ModuleMatrix[n + 8][m] && !qrCode.ModuleMatrix[n + 9][m] && !qrCode.ModuleMatrix[n + 10][m]) || (!qrCode.ModuleMatrix[n][m] && !qrCode.ModuleMatrix[n + 1][m] && !qrCode.ModuleMatrix[n + 2][m] && !qrCode.ModuleMatrix[n + 3][m] && qrCode.ModuleMatrix[n + 4][m] && !qrCode.ModuleMatrix[n + 5][m] && qrCode.ModuleMatrix[n + 6][m] && qrCode.ModuleMatrix[n + 7][m] && qrCode.ModuleMatrix[n + 8][m] && !qrCode.ModuleMatrix[n + 9][m] && qrCode.ModuleMatrix[n + 10][m]))
							{
								num3 += 40;
							}
						}
					}
					double num7 = 0.0;
					foreach (BitArray item in qrCode.ModuleMatrix)
					{
						foreach (bool item2 in item)
						{
							if (item2)
							{
								num7 += 1.0;
							}
						}
					}
					double num8 = num7 / (double)(qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count) * 100.0;
					int val = Math.Abs((int)Math.Floor(num8 / 5.0) * 5 - 50) / 5;
					int val2 = Math.Abs((int)Math.Floor(num8 / 5.0) * 5 - 45) / 5;
					num4 = Math.Min(val, val2) * 10;
					return num + num2 + num3 + num4;
				}
			}

			public static void AddQuietZone(ref QRCodeData qrCode)
			{
				bool[] array = new bool[qrCode.ModuleMatrix.Count + 8];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = false;
				}
				for (int j = 0; j < 4; j++)
				{
					qrCode.ModuleMatrix.Insert(0, new BitArray(array));
				}
				for (int k = 0; k < 4; k++)
				{
					qrCode.ModuleMatrix.Add(new BitArray(array));
				}
				for (int l = 4; l < qrCode.ModuleMatrix.Count - 4; l++)
				{
					bool[] collection = new bool[4];
					List<bool> list = new List<bool>(collection);
					list.AddRange(qrCode.ModuleMatrix[l].Cast<bool>());
					list.AddRange(collection);
					qrCode.ModuleMatrix[l] = new BitArray(list.ToArray());
				}
			}

			private static string ReverseString(string inp)
			{
				string text = string.Empty;
				if (inp.Length > 0)
				{
					for (int num = inp.Length - 1; num >= 0; num--)
					{
						text += inp[num];
					}
				}
				return text;
			}

			public static void PlaceVersion(ref QRCodeData qrCode, string versionStr)
			{
				int count = qrCode.ModuleMatrix.Count;
				string text = ReverseString(versionStr);
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						qrCode.ModuleMatrix[j + count - 11][i] = text[i * 3 + j] == '1';
						qrCode.ModuleMatrix[i][j + count - 11] = text[i * 3 + j] == '1';
					}
				}
			}

			public static void PlaceFormat(ref QRCodeData qrCode, string formatStr)
			{
				int count = qrCode.ModuleMatrix.Count;
				string text = ReverseString(formatStr);
				int[,] obj = new int[15, 4]
				{
					{ 8, 0, 0, 8 },
					{ 8, 1, 0, 8 },
					{ 8, 2, 0, 8 },
					{ 8, 3, 0, 8 },
					{ 8, 4, 0, 8 },
					{ 8, 5, 0, 8 },
					{ 8, 7, 0, 8 },
					{ 8, 8, 0, 8 },
					{ 7, 8, 8, 0 },
					{ 5, 8, 8, 0 },
					{ 4, 8, 8, 0 },
					{ 3, 8, 8, 0 },
					{ 2, 8, 8, 0 },
					{ 1, 8, 8, 0 },
					{ 0, 8, 8, 0 }
				};
				obj[0, 2] = count - 1;
				obj[1, 2] = count - 2;
				obj[2, 2] = count - 3;
				obj[3, 2] = count - 4;
				obj[4, 2] = count - 5;
				obj[5, 2] = count - 6;
				obj[6, 2] = count - 7;
				obj[7, 2] = count - 8;
				obj[8, 3] = count - 7;
				obj[9, 3] = count - 6;
				obj[10, 3] = count - 5;
				obj[11, 3] = count - 4;
				obj[12, 3] = count - 3;
				obj[13, 3] = count - 2;
				obj[14, 3] = count - 1;
				int[,] array = obj;
				for (int i = 0; i < 15; i++)
				{
					Point point = new Point(array[i, 0], array[i, 1]);
					Point point2 = new Point(array[i, 2], array[i, 3]);
					qrCode.ModuleMatrix[point.Y][point.X] = text[i] == '1';
					qrCode.ModuleMatrix[point2.Y][point2.X] = text[i] == '1';
				}
			}

			public static int MaskCode(ref QRCodeData qrCode, int version, ref List<Rectangle> blockedModules, ECCLevel eccLevel)
			{
				string patternName = string.Empty;
				int num = 0;
				int count = qrCode.ModuleMatrix.Count;
				MethodInfo[] methods = typeof(MaskPattern).GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					if (methodInfo.Name.Length != 8 || !methodInfo.Name.StartsWith("Pattern"))
					{
						continue;
					}
					QRCodeData qrCode2 = new QRCodeData(version);
					for (int j = 0; j < count; j++)
					{
						for (int k = 0; k < count; k++)
						{
							qrCode2.ModuleMatrix[j][k] = qrCode.ModuleMatrix[j][k];
						}
					}
					string formatString = GetFormatString(eccLevel, Convert.ToInt32(methodInfo.Name.Substring(7, 1)) - 1);
					PlaceFormat(ref qrCode2, formatString);
					if (version >= 7)
					{
						string versionString = GetVersionString(version);
						PlaceVersion(ref qrCode2, versionString);
					}
					for (int l = 0; l < count; l++)
					{
						for (int m = 0; m < count; m++)
						{
							if (!IsBlocked(new Rectangle(l, m, 1, 1), blockedModules))
							{
								BitArray bitArray = qrCode2.ModuleMatrix[m];
								int index = l;
								bitArray[index] ^= (bool)methodInfo.Invoke(null, new object[2] { l, m });
							}
						}
					}
					int num2 = MaskPattern.Score(ref qrCode2);
					if (string.IsNullOrEmpty(patternName) || num > num2)
					{
						patternName = methodInfo.Name;
						num = num2;
					}
				}
				MethodInfo methodInfo2 = typeof(MaskPattern).GetMethods().First((MethodInfo x) => x.Name == patternName);
				for (int n = 0; n < count; n++)
				{
					for (int num3 = 0; num3 < count; num3++)
					{
						if (!IsBlocked(new Rectangle(n, num3, 1, 1), blockedModules))
						{
							BitArray bitArray = qrCode.ModuleMatrix[num3];
							int i = n;
							bitArray[i] ^= (bool)methodInfo2.Invoke(null, new object[2] { n, num3 });
						}
					}
				}
				return Convert.ToInt32(methodInfo2.Name.Substring(methodInfo2.Name.Length - 1, 1)) - 1;
			}

			public static void PlaceDataWords(ref QRCodeData qrCode, string data, ref List<Rectangle> blockedModules)
			{
				int count = qrCode.ModuleMatrix.Count;
				bool flag = true;
				Queue<bool> queue = new Queue<bool>();
				for (int i = 0; i < data.Length; i++)
				{
					queue.Enqueue(data[i] != '0');
				}
				for (int num = count - 1; num >= 0; num -= 2)
				{
					if (num == 6)
					{
						num = 5;
					}
					for (int j = 1; j <= count; j++)
					{
						if (flag)
						{
							int num2 = count - j;
							if (queue.Count > 0 && !IsBlocked(new Rectangle(num, num2, 1, 1), blockedModules))
							{
								qrCode.ModuleMatrix[num2][num] = queue.Dequeue();
							}
							if (queue.Count > 0 && num > 0 && !IsBlocked(new Rectangle(num - 1, num2, 1, 1), blockedModules))
							{
								qrCode.ModuleMatrix[num2][num - 1] = queue.Dequeue();
							}
						}
						else
						{
							int num2 = j - 1;
							if (queue.Count > 0 && !IsBlocked(new Rectangle(num, num2, 1, 1), blockedModules))
							{
								qrCode.ModuleMatrix[num2][num] = queue.Dequeue();
							}
							if (queue.Count > 0 && num > 0 && !IsBlocked(new Rectangle(num - 1, num2, 1, 1), blockedModules))
							{
								qrCode.ModuleMatrix[num2][num - 1] = queue.Dequeue();
							}
						}
					}
					flag = !flag;
				}
			}

			public static void ReserveSeperatorAreas(int size, ref List<Rectangle> blockedModules)
			{
				blockedModules.AddRange(new Rectangle[6]
				{
					new Rectangle(7, 0, 1, 8),
					new Rectangle(0, 7, 7, 1),
					new Rectangle(0, size - 8, 8, 1),
					new Rectangle(7, size - 7, 1, 7),
					new Rectangle(size - 8, 0, 1, 8),
					new Rectangle(size - 7, 7, 7, 1)
				});
			}

			public static void ReserveVersionAreas(int size, int version, ref List<Rectangle> blockedModules)
			{
				blockedModules.AddRange(new Rectangle[6]
				{
					new Rectangle(8, 0, 1, 6),
					new Rectangle(8, 7, 1, 1),
					new Rectangle(0, 8, 6, 1),
					new Rectangle(7, 8, 2, 1),
					new Rectangle(size - 8, 8, 8, 1),
					new Rectangle(8, size - 7, 1, 7)
				});
				if (version >= 7)
				{
					blockedModules.AddRange(new Rectangle[2]
					{
						new Rectangle(size - 11, 0, 3, 6),
						new Rectangle(0, size - 11, 6, 3)
					});
				}
			}

			public static void PlaceDarkModule(ref QRCodeData qrCode, int version, ref List<Rectangle> blockedModules)
			{
				qrCode.ModuleMatrix[4 * version + 9][8] = true;
				blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
			}

			public static void PlaceFinderPatterns(ref QRCodeData qrCode, ref List<Rectangle> blockedModules)
			{
				int count = qrCode.ModuleMatrix.Count;
				int[] array = new int[6]
				{
					0,
					0,
					count - 7,
					0,
					0,
					count - 7
				};
				for (int i = 0; i < 6; i += 2)
				{
					for (int j = 0; j < 7; j++)
					{
						for (int k = 0; k < 7; k++)
						{
							if (((j != 1 && j != 5) || k <= 0 || k >= 6) && (j <= 0 || j >= 6 || (k != 1 && k != 5)))
							{
								qrCode.ModuleMatrix[k + array[i + 1]][j + array[i]] = true;
							}
						}
					}
					blockedModules.Add(new Rectangle(array[i], array[i + 1], 7, 7));
				}
			}

			public static void PlaceAlignmentPatterns(ref QRCodeData qrCode, List<Point> alignmentPatternLocations, ref List<Rectangle> blockedModules)
			{
				foreach (Point alignmentPatternLocation in alignmentPatternLocations)
				{
					Rectangle r = new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5);
					bool flag = false;
					foreach (Rectangle blockedModule in blockedModules)
					{
						if (Intersects(r, blockedModule))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					for (int i = 0; i < 5; i++)
					{
						for (int j = 0; j < 5; j++)
						{
							if (j == 0 || j == 4 || i == 0 || i == 4 || (i == 2 && j == 2))
							{
								qrCode.ModuleMatrix[alignmentPatternLocation.Y + j][alignmentPatternLocation.X + i] = true;
							}
						}
					}
					blockedModules.Add(new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5));
				}
			}

			public static void PlaceTimingPatterns(ref QRCodeData qrCode, ref List<Rectangle> blockedModules)
			{
				int count = qrCode.ModuleMatrix.Count;
				for (int i = 8; i < count - 8; i++)
				{
					if (i % 2 == 0)
					{
						qrCode.ModuleMatrix[6][i] = true;
						qrCode.ModuleMatrix[i][6] = true;
					}
				}
				blockedModules.AddRange(new Rectangle[2]
				{
					new Rectangle(6, 8, 1, count - 16),
					new Rectangle(8, 6, count - 16, 1)
				});
			}

			private static bool Intersects(Rectangle r1, Rectangle r2)
			{
				if (r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height)
				{
					return r1.Y < r2.Y + r2.Height;
				}
				return false;
			}

			private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
			{
				bool result = false;
				foreach (Rectangle blockedModule in blockedModules)
				{
					if (Intersects(blockedModule, r1))
					{
						result = true;
					}
				}
				return result;
			}
		}

		public enum ECCLevel
		{
			L,
			M,
			Q,
			H
		}

		private enum EncodingMode
		{
			Numeric = 1,
			Alphanumeric = 2,
			Byte = 4,
			Kanji = 8,
			ECI = 7
		}

		private struct AlignmentPattern
		{
			public int Version;

			public List<Point> PatternPositions;
		}

		private struct CodewordBlock
		{
			public int GroupNumber { get; }

			public int BlockNumber { get; }

			public string BitString { get; }

			public List<string> CodeWords { get; }

			public List<int> CodeWordsInt { get; }

			public List<string> ECCWords { get; }

			public List<int> ECCWordsInt { get; }

			public CodewordBlock(int groupNumber, int blockNumber, string bitString, List<string> codeWords, List<string> eccWords, List<int> codeWordsInt, List<int> eccWordsInt)
			{
				GroupNumber = groupNumber;
				BlockNumber = blockNumber;
				BitString = bitString;
				CodeWords = codeWords;
				ECCWords = eccWords;
				CodeWordsInt = codeWordsInt;
				ECCWordsInt = eccWordsInt;
			}
		}

		private struct ECCInfo
		{
			public int Version { get; }

			public ECCLevel ErrorCorrectionLevel { get; }

			public int TotalDataCodewords { get; }

			public int ECCPerBlock { get; }

			public int BlocksInGroup1 { get; }

			public int CodewordsInGroup1 { get; }

			public int BlocksInGroup2 { get; }

			public int CodewordsInGroup2 { get; }

			public ECCInfo(int version, ECCLevel errorCorrectionLevel, int totalDataCodewords, int eccPerBlock, int blocksInGroup1, int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
			{
				Version = version;
				ErrorCorrectionLevel = errorCorrectionLevel;
				TotalDataCodewords = totalDataCodewords;
				ECCPerBlock = eccPerBlock;
				BlocksInGroup1 = blocksInGroup1;
				CodewordsInGroup1 = codewordsInGroup1;
				BlocksInGroup2 = blocksInGroup2;
				CodewordsInGroup2 = codewordsInGroup2;
			}
		}

		private struct VersionInfo
		{
			public int Version { get; }

			public List<VersionInfoDetails> Details { get; }

			public VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
			{
				Version = version;
				Details = versionInfoDetails;
			}
		}

		private struct VersionInfoDetails
		{
			public ECCLevel ErrorCorrectionLevel { get; }

			public Dictionary<EncodingMode, int> CapacityDict { get; }

			public VersionInfoDetails(ECCLevel errorCorrectionLevel, Dictionary<EncodingMode, int> capacityDict)
			{
				ErrorCorrectionLevel = errorCorrectionLevel;
				CapacityDict = capacityDict;
			}
		}

		private struct Antilog
		{
			public int ExponentAlpha { get; }

			public int IntegerValue { get; }

			public Antilog(int exponentAlpha, int integerValue)
			{
				ExponentAlpha = exponentAlpha;
				IntegerValue = integerValue;
			}
		}

		private struct PolynomItem
		{
			public int Coefficient { get; }

			public int Exponent { get; }

			public PolynomItem(int coefficient, int exponent)
			{
				Coefficient = coefficient;
				Exponent = exponent;
			}
		}

		private class Polynom
		{
			public List<PolynomItem> PolyItems { get; set; }

			public Polynom()
			{
				PolyItems = new List<PolynomItem>();
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (PolynomItem polyItem in PolyItems)
				{
					stringBuilder.Append("a^" + polyItem.Coefficient + "*x^" + polyItem.Exponent + " + ");
				}
				return stringBuilder.ToString().TrimEnd(' ', '+');
			}
		}

		private class Point
		{
			public int X { get; }

			public int Y { get; }

			public Point(int x, int y)
			{
				X = x;
				Y = y;
			}
		}

		private class Rectangle
		{
			public int X { get; }

			public int Y { get; }

			public int Width { get; }

			public int Height { get; }

			public Rectangle(int x, int y, int w, int h)
			{
				X = x;
				Y = y;
				Width = w;
				Height = h;
			}
		}

		private static readonly char[] numTable = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

		private static readonly char[] alphanumEncTable = new char[45]
		{
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
			'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
			'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*',
			'+', '-', '.', '/', ':'
		};

		private int[] capacityBaseValues = new int[640]
		{
			41, 25, 17, 10, 34, 20, 14, 8, 27, 16,
			11, 7, 17, 10, 7, 4, 77, 47, 32, 20,
			63, 38, 26, 16, 48, 29, 20, 12, 34, 20,
			14, 8, 127, 77, 53, 32, 101, 61, 42, 26,
			77, 47, 32, 20, 58, 35, 24, 15, 187, 114,
			78, 48, 149, 90, 62, 38, 111, 67, 46, 28,
			82, 50, 34, 21, 255, 154, 106, 65, 202, 122,
			84, 52, 144, 87, 60, 37, 106, 64, 44, 27,
			322, 195, 134, 82, 255, 154, 106, 65, 178, 108,
			74, 45, 139, 84, 58, 36, 370, 224, 154, 95,
			293, 178, 122, 75, 207, 125, 86, 53, 154, 93,
			64, 39, 461, 279, 192, 118, 365, 221, 152, 93,
			259, 157, 108, 66, 202, 122, 84, 52, 552, 335,
			230, 141, 432, 262, 180, 111, 312, 189, 130, 80,
			235, 143, 98, 60, 652, 395, 271, 167, 513, 311,
			213, 131, 364, 221, 151, 93, 288, 174, 119, 74,
			772, 468, 321, 198, 604, 366, 251, 155, 427, 259,
			177, 109, 331, 200, 137, 85, 883, 535, 367, 226,
			691, 419, 287, 177, 489, 296, 203, 125, 374, 227,
			155, 96, 1022, 619, 425, 262, 796, 483, 331, 204,
			580, 352, 241, 149, 427, 259, 177, 109, 1101, 667,
			458, 282, 871, 528, 362, 223, 621, 376, 258, 159,
			468, 283, 194, 120, 1250, 758, 520, 320, 991, 600,
			412, 254, 703, 426, 292, 180, 530, 321, 220, 136,
			1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470,
			322, 198, 602, 365, 250, 154, 1548, 938, 644, 397,
			1212, 734, 504, 310, 876, 531, 364, 224, 674, 408,
			280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345,
			948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153,
			792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272,
			813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970,
			666, 410, 1159, 702, 482, 297, 919, 557, 382, 235,
			2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742,
			509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618,
			1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640,
			439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528,
			1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704,
			1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407,
			1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451,
			997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330,
			3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094,
			751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902,
			2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910,
			625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732,
			2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369,
			1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559,
			1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994,
			1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457,
			4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499,
			1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201,
			3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226,
			842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002,
			2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183,
			2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756,
			2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632,
			1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605,
			5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966,
			1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577,
			4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591,
			1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292,
			3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087,
			2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972,
			2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391,
			2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784
		};

		private int[] capacityECCBaseValues = new int[960]
		{
			19, 7, 1, 19, 0, 0, 16, 10, 1, 16,
			0, 0, 13, 13, 1, 13, 0, 0, 9, 17,
			1, 9, 0, 0, 34, 10, 1, 34, 0, 0,
			28, 16, 1, 28, 0, 0, 22, 22, 1, 22,
			0, 0, 16, 28, 1, 16, 0, 0, 55, 15,
			1, 55, 0, 0, 44, 26, 1, 44, 0, 0,
			34, 18, 2, 17, 0, 0, 26, 22, 2, 13,
			0, 0, 80, 20, 1, 80, 0, 0, 64, 18,
			2, 32, 0, 0, 48, 26, 2, 24, 0, 0,
			36, 16, 4, 9, 0, 0, 108, 26, 1, 108,
			0, 0, 86, 24, 2, 43, 0, 0, 62, 18,
			2, 15, 2, 16, 46, 22, 2, 11, 2, 12,
			136, 18, 2, 68, 0, 0, 108, 16, 4, 27,
			0, 0, 76, 24, 4, 19, 0, 0, 60, 28,
			4, 15, 0, 0, 156, 20, 2, 78, 0, 0,
			124, 18, 4, 31, 0, 0, 88, 18, 2, 14,
			4, 15, 66, 26, 4, 13, 1, 14, 194, 24,
			2, 97, 0, 0, 154, 22, 2, 38, 2, 39,
			110, 22, 4, 18, 2, 19, 86, 26, 4, 14,
			2, 15, 232, 30, 2, 116, 0, 0, 182, 22,
			3, 36, 2, 37, 132, 20, 4, 16, 4, 17,
			100, 24, 4, 12, 4, 13, 274, 18, 2, 68,
			2, 69, 216, 26, 4, 43, 1, 44, 154, 24,
			6, 19, 2, 20, 122, 28, 6, 15, 2, 16,
			324, 20, 4, 81, 0, 0, 254, 30, 1, 50,
			4, 51, 180, 28, 4, 22, 4, 23, 140, 24,
			3, 12, 8, 13, 370, 24, 2, 92, 2, 93,
			290, 22, 6, 36, 2, 37, 206, 26, 4, 20,
			6, 21, 158, 28, 7, 14, 4, 15, 428, 26,
			4, 107, 0, 0, 334, 22, 8, 37, 1, 38,
			244, 24, 8, 20, 4, 21, 180, 22, 12, 11,
			4, 12, 461, 30, 3, 115, 1, 116, 365, 24,
			4, 40, 5, 41, 261, 20, 11, 16, 5, 17,
			197, 24, 11, 12, 5, 13, 523, 22, 5, 87,
			1, 88, 415, 24, 5, 41, 5, 42, 295, 30,
			5, 24, 7, 25, 223, 24, 11, 12, 7, 13,
			589, 24, 5, 98, 1, 99, 453, 28, 7, 45,
			3, 46, 325, 24, 15, 19, 2, 20, 253, 30,
			3, 15, 13, 16, 647, 28, 1, 107, 5, 108,
			507, 28, 10, 46, 1, 47, 367, 28, 1, 22,
			15, 23, 283, 28, 2, 14, 17, 15, 721, 30,
			5, 120, 1, 121, 563, 26, 9, 43, 4, 44,
			397, 28, 17, 22, 1, 23, 313, 28, 2, 14,
			19, 15, 795, 28, 3, 113, 4, 114, 627, 26,
			3, 44, 11, 45, 445, 26, 17, 21, 4, 22,
			341, 26, 9, 13, 16, 14, 861, 28, 3, 107,
			5, 108, 669, 26, 3, 41, 13, 42, 485, 30,
			15, 24, 5, 25, 385, 28, 15, 15, 10, 16,
			932, 28, 4, 116, 4, 117, 714, 26, 17, 42,
			0, 0, 512, 28, 17, 22, 6, 23, 406, 30,
			19, 16, 6, 17, 1006, 28, 2, 111, 7, 112,
			782, 28, 17, 46, 0, 0, 568, 30, 7, 24,
			16, 25, 442, 24, 34, 13, 0, 0, 1094, 30,
			4, 121, 5, 122, 860, 28, 4, 47, 14, 48,
			614, 30, 11, 24, 14, 25, 464, 30, 16, 15,
			14, 16, 1174, 30, 6, 117, 4, 118, 914, 28,
			6, 45, 14, 46, 664, 30, 11, 24, 16, 25,
			514, 30, 30, 16, 2, 17, 1276, 26, 8, 106,
			4, 107, 1000, 28, 8, 47, 13, 48, 718, 30,
			7, 24, 22, 25, 538, 30, 22, 15, 13, 16,
			1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46,
			4, 47, 754, 28, 28, 22, 6, 23, 596, 30,
			33, 16, 4, 17, 1468, 30, 8, 122, 4, 123,
			1128, 28, 22, 45, 3, 46, 808, 30, 8, 23,
			26, 24, 628, 30, 12, 15, 28, 16, 1531, 30,
			3, 117, 10, 118, 1193, 28, 3, 45, 23, 46,
			871, 30, 4, 24, 31, 25, 661, 30, 11, 15,
			31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28,
			21, 45, 7, 46, 911, 30, 1, 23, 37, 24,
			701, 30, 19, 15, 26, 16, 1735, 30, 5, 115,
			10, 116, 1373, 28, 19, 47, 10, 48, 985, 30,
			15, 24, 25, 25, 745, 30, 23, 15, 25, 16,
			1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46,
			29, 47, 1033, 30, 42, 24, 1, 25, 793, 30,
			23, 15, 28, 16, 1955, 30, 17, 115, 0, 0,
			1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24,
			35, 25, 845, 30, 19, 15, 35, 16, 2071, 30,
			17, 115, 1, 116, 1631, 28, 14, 46, 21, 47,
			1171, 30, 29, 24, 19, 25, 901, 30, 11, 15,
			46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28,
			14, 46, 23, 47, 1231, 30, 44, 24, 7, 25,
			961, 30, 59, 16, 1, 17, 2306, 30, 12, 121,
			7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30,
			39, 24, 14, 25, 986, 30, 22, 15, 41, 16,
			2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47,
			34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30,
			2, 15, 64, 16, 2566, 30, 17, 122, 4, 123,
			1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24,
			10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30,
			4, 122, 18, 123, 2102, 28, 13, 46, 32, 47,
			1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15,
			32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28,
			40, 47, 7, 48, 1582, 30, 43, 24, 22, 25,
			1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118,
			6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30,
			34, 24, 34, 25, 1276, 30, 20, 15, 61, 16
		};

		private int[] alignmentPatternBaseValues = new int[280]
		{
			0, 0, 0, 0, 0, 0, 0, 6, 18, 0,
			0, 0, 0, 0, 6, 22, 0, 0, 0, 0,
			0, 6, 26, 0, 0, 0, 0, 0, 6, 30,
			0, 0, 0, 0, 0, 6, 34, 0, 0, 0,
			0, 0, 6, 22, 38, 0, 0, 0, 0, 6,
			24, 42, 0, 0, 0, 0, 6, 26, 46, 0,
			0, 0, 0, 6, 28, 50, 0, 0, 0, 0,
			6, 30, 54, 0, 0, 0, 0, 6, 32, 58,
			0, 0, 0, 0, 6, 34, 62, 0, 0, 0,
			0, 6, 26, 46, 66, 0, 0, 0, 6, 26,
			48, 70, 0, 0, 0, 6, 26, 50, 74, 0,
			0, 0, 6, 30, 54, 78, 0, 0, 0, 6,
			30, 56, 82, 0, 0, 0, 6, 30, 58, 86,
			0, 0, 0, 6, 34, 62, 90, 0, 0, 0,
			6, 28, 50, 72, 94, 0, 0, 6, 26, 50,
			74, 98, 0, 0, 6, 30, 54, 78, 102, 0,
			0, 6, 28, 54, 80, 106, 0, 0, 6, 32,
			58, 84, 110, 0, 0, 6, 30, 58, 86, 114,
			0, 0, 6, 34, 62, 90, 118, 0, 0, 6,
			26, 50, 74, 98, 122, 0, 6, 30, 54, 78,
			102, 126, 0, 6, 26, 52, 78, 104, 130, 0,
			6, 30, 56, 82, 108, 134, 0, 6, 34, 60,
			86, 112, 138, 0, 6, 30, 58, 86, 114, 142,
			0, 6, 34, 62, 90, 118, 146, 0, 6, 30,
			54, 78, 102, 126, 150, 6, 24, 50, 76, 102,
			128, 154, 6, 28, 54, 80, 106, 132, 158, 6,
			32, 58, 84, 110, 136, 162, 6, 26, 54, 82,
			110, 138, 166, 6, 30, 58, 86, 114, 142, 170
		};

		private int[] remainderBits = new int[40]
		{
			0, 7, 7, 7, 7, 7, 0, 0, 0, 0,
			0, 0, 0, 3, 3, 3, 3, 3, 3, 3,
			4, 4, 4, 4, 4, 4, 4, 3, 3, 3,
			3, 3, 3, 3, 0, 0, 0, 0, 0, 0
		};

		private List<AlignmentPattern> alignmentPatternTable;

		private List<ECCInfo> capacityECCTable;

		private List<VersionInfo> capacityTable;

		private List<Antilog> galoisField;

		private Dictionary<char, int> alphanumEncDict;

		public QRCodeGenerator()
		{
			CreateAntilogTable();
			CreateAlphanumEncDict();
			CreateCapacityTable();
			CreateCapacityECCTable();
			CreateAlignmentPatternTable();
		}

		public QRCodeData CreateQrCode(PayloadGenerator.Payload payload)
		{
			return CreateQrCode(payload.ToString(), payload.EccLevel, forceUtf8: false, utf8BOM: false, payload.EciMode, payload.Version);
		}

		public QRCodeData CreateQrCode(PayloadGenerator.Payload payload, ECCLevel eccLevel)
		{
			return CreateQrCode(payload.ToString(), eccLevel, forceUtf8: false, utf8BOM: false, payload.EciMode, payload.Version);
		}

		public QRCodeData CreateQrCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1)
		{
			EncodingMode encodingFromPlaintext = GetEncodingFromPlaintext(plainText, forceUtf8);
			string text = PlainTextToBinary(plainText, encodingFromPlaintext, eciMode, utf8BOM, forceUtf8);
			int dataLength = GetDataLength(encodingFromPlaintext, plainText, text, forceUtf8);
			int version = requestedVersion;
			if (version == -1)
			{
				version = GetVersion(dataLength, encodingFromPlaintext, eccLevel);
			}
			string text2 = string.Empty;
			if (eciMode != 0)
			{
				text2 = DecToBin(7, 4);
				text2 += DecToBin((int)eciMode, 8);
			}
			text2 += DecToBin((int)encodingFromPlaintext, 4);
			string text3 = DecToBin(dataLength, GetCountIndicatorLength(version, encodingFromPlaintext));
			string text4 = text2 + text3;
			text4 += text;
			ECCInfo eccInfo = capacityECCTable.Single((ECCInfo x) => x.Version == version && x.ErrorCorrectionLevel.Equals(eccLevel));
			int num = eccInfo.TotalDataCodewords * 8;
			int num2 = num - text4.Length;
			if (num2 > 0)
			{
				text4 += new string('0', Math.Min(num2, 4));
			}
			if (text4.Length % 8 != 0)
			{
				text4 += new string('0', 8 - text4.Length % 8);
			}
			while (text4.Length < num)
			{
				text4 += "1110110000010001";
			}
			if (text4.Length > num)
			{
				text4 = text4.Substring(0, num);
			}
			List<CodewordBlock> list = new List<CodewordBlock>();
			for (int i = 0; i < eccInfo.BlocksInGroup1; i++)
			{
				string bitString = text4.Substring(i * eccInfo.CodewordsInGroup1 * 8, eccInfo.CodewordsInGroup1 * 8);
				List<string> list2 = BinaryStringToBitBlockList(bitString);
				List<int> codeWordsInt = BinaryStringListToDecList(list2);
				List<string> list3 = CalculateECCWords(bitString, eccInfo);
				List<int> eccWordsInt = BinaryStringListToDecList(list3);
				list.Add(new CodewordBlock(1, i + 1, bitString, list2, list3, codeWordsInt, eccWordsInt));
			}
			text4 = text4.Substring(eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8);
			for (int j = 0; j < eccInfo.BlocksInGroup2; j++)
			{
				string bitString2 = text4.Substring(j * eccInfo.CodewordsInGroup2 * 8, eccInfo.CodewordsInGroup2 * 8);
				List<string> list4 = BinaryStringToBitBlockList(bitString2);
				List<int> codeWordsInt2 = BinaryStringListToDecList(list4);
				List<string> list5 = CalculateECCWords(bitString2, eccInfo);
				List<int> eccWordsInt2 = BinaryStringListToDecList(list5);
				list.Add(new CodewordBlock(2, j + 1, bitString2, list4, list5, codeWordsInt2, eccWordsInt2));
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int k = 0; k < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); k++)
			{
				foreach (CodewordBlock item in list)
				{
					if (item.CodeWords.Count > k)
					{
						stringBuilder.Append(item.CodeWords[k]);
					}
				}
			}
			for (int l = 0; l < eccInfo.ECCPerBlock; l++)
			{
				foreach (CodewordBlock item2 in list)
				{
					if (item2.ECCWords.Count > l)
					{
						stringBuilder.Append(item2.ECCWords[l]);
					}
				}
			}
			stringBuilder.Append(new string('0', remainderBits[version - 1]));
			string data = stringBuilder.ToString();
			QRCodeData qrCode = new QRCodeData(version);
			List<Rectangle> blockedModules = new List<Rectangle>();
			ModulePlacer.PlaceFinderPatterns(ref qrCode, ref blockedModules);
			ModulePlacer.ReserveSeperatorAreas(qrCode.ModuleMatrix.Count, ref blockedModules);
			ModulePlacer.PlaceAlignmentPatterns(ref qrCode, (from x in alignmentPatternTable
				where x.Version == version
				select x.PatternPositions).First(), ref blockedModules);
			ModulePlacer.PlaceTimingPatterns(ref qrCode, ref blockedModules);
			ModulePlacer.PlaceDarkModule(ref qrCode, version, ref blockedModules);
			ModulePlacer.ReserveVersionAreas(qrCode.ModuleMatrix.Count, version, ref blockedModules);
			ModulePlacer.PlaceDataWords(ref qrCode, data, ref blockedModules);
			int maskVersion = ModulePlacer.MaskCode(ref qrCode, version, ref blockedModules, eccLevel);
			string formatString = GetFormatString(eccLevel, maskVersion);
			ModulePlacer.PlaceFormat(ref qrCode, formatString);
			if (version >= 7)
			{
				string versionString = GetVersionString(version);
				ModulePlacer.PlaceVersion(ref qrCode, versionString);
			}
			ModulePlacer.AddQuietZone(ref qrCode);
			return qrCode;
		}

		private static string GetFormatString(ECCLevel level, int maskVersion)
		{
			string text = "10100110111";
			string text2 = "101010000010010";
			string text3 = level switch
			{
				ECCLevel.Q => "11", 
				ECCLevel.M => "00", 
				ECCLevel.L => "01", 
				_ => "10", 
			} + DecToBin(maskVersion, 3);
			string text4 = text3.PadRight(15, '0').TrimStart(new char[1] { '0' });
			while (text4.Length > 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				text = text.PadRight(text4.Length, '0');
				for (int i = 0; i < text4.Length; i++)
				{
					stringBuilder.Append((Convert.ToInt32(text4[i]) ^ Convert.ToInt32(text[i])).ToString());
				}
				text4 = stringBuilder.ToString().TrimStart(new char[1] { '0' });
			}
			text4 = text4.PadLeft(10, '0');
			text3 += text4;
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < text3.Length; j++)
			{
				stringBuilder2.Append((Convert.ToInt32(text3[j]) ^ Convert.ToInt32(text2[j])).ToString());
			}
			return stringBuilder2.ToString();
		}

		private static string GetVersionString(int version)
		{
			string text = "1111100100101";
			string text2 = DecToBin(version, 6);
			string text3 = text2.PadRight(18, '0').TrimStart(new char[1] { '0' });
			while (text3.Length > 12)
			{
				StringBuilder stringBuilder = new StringBuilder();
				text = text.PadRight(text3.Length, '0');
				for (int i = 0; i < text3.Length; i++)
				{
					stringBuilder.Append((Convert.ToInt32(text3[i]) ^ Convert.ToInt32(text[i])).ToString());
				}
				text3 = stringBuilder.ToString().TrimStart(new char[1] { '0' });
			}
			text3 = text3.PadLeft(12, '0');
			return text2 + text3;
		}

		private List<string> CalculateECCWords(string bitString, ECCInfo eccInfo)
		{
			int eCCPerBlock = eccInfo.ECCPerBlock;
			Polynom polynom = CalculateMessagePolynom(bitString);
			Polynom polynom2 = CalculateGeneratorPolynom(eCCPerBlock);
			for (int i = 0; i < polynom.PolyItems.Count; i++)
			{
				polynom.PolyItems[i] = new PolynomItem(polynom.PolyItems[i].Coefficient, polynom.PolyItems[i].Exponent + eCCPerBlock);
			}
			for (int j = 0; j < polynom2.PolyItems.Count; j++)
			{
				polynom2.PolyItems[j] = new PolynomItem(polynom2.PolyItems[j].Coefficient, polynom2.PolyItems[j].Exponent + (polynom.PolyItems.Count - 1));
			}
			Polynom polynom3 = polynom;
			int num = 0;
			while (polynom3.PolyItems.Count > 0 && polynom3.PolyItems[polynom3.PolyItems.Count - 1].Exponent > 0)
			{
				if (polynom3.PolyItems[0].Coefficient == 0)
				{
					polynom3.PolyItems.RemoveAt(0);
					polynom3.PolyItems.Add(new PolynomItem(0, polynom3.PolyItems[polynom3.PolyItems.Count - 1].Exponent - 1));
				}
				else
				{
					Polynom poly = MultiplyGeneratorPolynomByLeadterm(polynom2, ConvertToAlphaNotation(polynom3).PolyItems[0], num);
					poly = ConvertToDecNotation(poly);
					poly = XORPolynoms(polynom3, poly);
					polynom3 = poly;
				}
				num++;
			}
			return polynom3.PolyItems.Select((PolynomItem x) => DecToBin(x.Coefficient, 8)).ToList();
		}

		private Polynom ConvertToAlphaNotation(Polynom poly)
		{
			Polynom polynom = new Polynom();
			for (int i = 0; i < poly.PolyItems.Count; i++)
			{
				polynom.PolyItems.Add(new PolynomItem((poly.PolyItems[i].Coefficient != 0) ? GetAlphaExpFromIntVal(poly.PolyItems[i].Coefficient) : 0, poly.PolyItems[i].Exponent));
			}
			return polynom;
		}

		private Polynom ConvertToDecNotation(Polynom poly)
		{
			Polynom polynom = new Polynom();
			for (int i = 0; i < poly.PolyItems.Count; i++)
			{
				polynom.PolyItems.Add(new PolynomItem(GetIntValFromAlphaExp(poly.PolyItems[i].Coefficient), poly.PolyItems[i].Exponent));
			}
			return polynom;
		}

		private int GetVersion(int length, EncodingMode encMode, ECCLevel eccLevel)
		{
			return (from x in capacityTable
				where x.Details.Count((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel && y.CapacityDict[encMode] >= Convert.ToInt32(length)) > 0
				select new
				{
					version = x.Version,
					capacity = x.Details.Single((VersionInfoDetails y) => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]
				}).Min(x => x.version);
		}

		private EncodingMode GetEncodingFromPlaintext(string plainText, bool forceUtf8)
		{
			EncodingMode result = EncodingMode.Numeric;
			foreach (char value in plainText)
			{
				if (forceUtf8 || !alphanumEncTable.Contains(value))
				{
					return EncodingMode.Byte;
				}
				if (!numTable.Contains(value))
				{
					result = EncodingMode.Alphanumeric;
				}
			}
			return result;
		}

		private Polynom CalculateMessagePolynom(string bitString)
		{
			Polynom polynom = new Polynom();
			for (int num = bitString.Length / 8 - 1; num >= 0; num--)
			{
				polynom.PolyItems.Add(new PolynomItem(BinToDec(bitString.Substring(0, 8)), num));
				bitString = bitString.Remove(0, 8);
			}
			return polynom;
		}

		private Polynom CalculateGeneratorPolynom(int numEccWords)
		{
			Polynom polynom = new Polynom();
			polynom.PolyItems.AddRange(new PolynomItem[2]
			{
				new PolynomItem(0, 1),
				new PolynomItem(0, 0)
			});
			for (int i = 1; i <= numEccWords - 1; i++)
			{
				Polynom polynom2 = new Polynom();
				polynom2.PolyItems.AddRange(new PolynomItem[2]
				{
					new PolynomItem(0, 1),
					new PolynomItem(i, 0)
				});
				polynom = MultiplyAlphaPolynoms(polynom, polynom2);
			}
			return polynom;
		}

		private List<string> BinaryStringToBitBlockList(string bitString)
		{
			return (from x in new List<char>(bitString.ToCharArray()).Select((char x, int i) => new
				{
					Index = i,
					Value = x
				})
				group x by x.Index / 8 into x
				select string.Join("", x.Select(v => v.Value.ToString()).ToArray())).ToList();
		}

		private List<int> BinaryStringListToDecList(List<string> binaryStringList)
		{
			return binaryStringList.Select((string binaryString) => BinToDec(binaryString)).ToList();
		}

		private int BinToDec(string binStr)
		{
			return Convert.ToInt32(binStr, 2);
		}

		private static string DecToBin(int decNum)
		{
			return Convert.ToString(decNum, 2);
		}

		private static string DecToBin(int decNum, int padLeftUpTo)
		{
			return DecToBin(decNum).PadLeft(padLeftUpTo, '0');
		}

		private int GetCountIndicatorLength(int version, EncodingMode encMode)
		{
			if (version < 10)
			{
				if (encMode.Equals(EncodingMode.Numeric))
				{
					return 10;
				}
				if (encMode.Equals(EncodingMode.Alphanumeric))
				{
					return 9;
				}
				return 8;
			}
			if (version < 27)
			{
				if (encMode.Equals(EncodingMode.Numeric))
				{
					return 12;
				}
				if (encMode.Equals(EncodingMode.Alphanumeric))
				{
					return 11;
				}
				if (encMode.Equals(EncodingMode.Byte))
				{
					return 16;
				}
				return 10;
			}
			if (encMode.Equals(EncodingMode.Numeric))
			{
				return 14;
			}
			if (encMode.Equals(EncodingMode.Alphanumeric))
			{
				return 13;
			}
			if (encMode.Equals(EncodingMode.Byte))
			{
				return 16;
			}
			return 12;
		}

		private int GetDataLength(EncodingMode encoding, string plainText, string codedText, bool forceUtf8)
		{
			if (!forceUtf8 && !IsUtf8(encoding, plainText))
			{
				return plainText.Length;
			}
			return codedText.Length / 8;
		}

		private bool IsUtf8(EncodingMode encoding, string plainText)
		{
			if (encoding == EncodingMode.Byte)
			{
				return !IsValidISO(plainText);
			}
			return false;
		}

		private bool IsValidISO(string input)
		{
			byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
			string @string = Encoding.GetEncoding("ISO-8859-1").GetString(bytes, 0, bytes.Length);
			return string.Equals(input, @string);
		}

		private string PlainTextToBinary(string plainText, EncodingMode encMode, EciMode eciMode, bool utf8BOM, bool forceUtf8)
		{
			return encMode switch
			{
				EncodingMode.Alphanumeric => PlainTextToBinaryAlphanumeric(plainText), 
				EncodingMode.Numeric => PlainTextToBinaryNumeric(plainText), 
				EncodingMode.Byte => PlainTextToBinaryByte(plainText, eciMode, utf8BOM, forceUtf8), 
				EncodingMode.Kanji => string.Empty, 
				_ => string.Empty, 
			};
		}

		private string PlainTextToBinaryNumeric(string plainText)
		{
			string text = string.Empty;
			while (plainText.Length >= 3)
			{
				int decNum = Convert.ToInt32(plainText.Substring(0, 3));
				text += DecToBin(decNum, 10);
				plainText = plainText.Substring(3);
			}
			if (plainText.Length == 2)
			{
				int decNum2 = Convert.ToInt32(plainText);
				text += DecToBin(decNum2, 7);
			}
			else if (plainText.Length == 1)
			{
				int decNum3 = Convert.ToInt32(plainText);
				text += DecToBin(decNum3, 4);
			}
			return text;
		}

		private string PlainTextToBinaryAlphanumeric(string plainText)
		{
			string text = string.Empty;
			while (plainText.Length >= 2)
			{
				string text2 = plainText.Substring(0, 2);
				int decNum = alphanumEncDict[text2[0]] * 45 + alphanumEncDict[text2[1]];
				text += DecToBin(decNum, 11);
				plainText = plainText.Substring(2);
			}
			if (plainText.Length > 0)
			{
				text += DecToBin(alphanumEncDict[plainText[0]], 6);
			}
			return text;
		}

		private string PlainTextToBinaryECI(string plainText)
		{
			string text = string.Empty;
			byte[] bytes = Encoding.GetEncoding("ascii").GetBytes(plainText);
			foreach (byte decNum in bytes)
			{
				text += DecToBin(decNum, 8);
			}
			return text;
		}

		private string ConvertToIso8859(string value, string Iso = "ISO-8859-2")
		{
			Encoding encoding = Encoding.GetEncoding(Iso);
			Encoding uTF = Encoding.UTF8;
			byte[] bytes = uTF.GetBytes(value);
			byte[] bytes2 = Encoding.Convert(uTF, encoding, bytes);
			return encoding.GetString(bytes2);
		}

		private string PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
		{
			string text = string.Empty;
			byte[] array = ((IsValidISO(plainText) && !forceUtf8) ? Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText) : (eciMode switch
			{
				EciMode.Iso8859_1 => Encoding.GetEncoding("ISO-8859-1").GetBytes(ConvertToIso8859(plainText, "ISO-8859-1")), 
				EciMode.Iso8859_2 => Encoding.GetEncoding("ISO-8859-2").GetBytes(ConvertToIso8859(plainText)), 
				_ => utf8BOM ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(plainText)).ToArray() : Encoding.UTF8.GetBytes(plainText), 
			}));
			byte[] array2 = array;
			foreach (byte decNum in array2)
			{
				text += DecToBin(decNum, 8);
			}
			return text;
		}

		private Polynom XORPolynoms(Polynom messagePolynom, Polynom resPolynom)
		{
			Polynom polynom = new Polynom();
			Polynom polynom2;
			Polynom polynom3;
			if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
			{
				polynom2 = messagePolynom;
				polynom3 = resPolynom;
			}
			else
			{
				polynom2 = resPolynom;
				polynom3 = messagePolynom;
			}
			for (int i = 0; i < polynom2.PolyItems.Count; i++)
			{
				PolynomItem item = new PolynomItem(polynom2.PolyItems[i].Coefficient ^ ((polynom3.PolyItems.Count > i) ? polynom3.PolyItems[i].Coefficient : 0), messagePolynom.PolyItems[0].Exponent - i);
				polynom.PolyItems.Add(item);
			}
			polynom.PolyItems.RemoveAt(0);
			return polynom;
		}

		private Polynom MultiplyGeneratorPolynomByLeadterm(Polynom genPolynom, PolynomItem leadTerm, int lowerExponentBy)
		{
			Polynom polynom = new Polynom();
			foreach (PolynomItem polyItem in genPolynom.PolyItems)
			{
				PolynomItem item = new PolynomItem((polyItem.Coefficient + leadTerm.Coefficient) % 255, polyItem.Exponent - lowerExponentBy);
				polynom.PolyItems.Add(item);
			}
			return polynom;
		}

		private Polynom MultiplyAlphaPolynoms(Polynom polynomBase, Polynom polynomMultiplier)
		{
			Polynom polynom = new Polynom();
			foreach (PolynomItem polyItem in polynomMultiplier.PolyItems)
			{
				foreach (PolynomItem polyItem2 in polynomBase.PolyItems)
				{
					PolynomItem item = new PolynomItem(ShrinkAlphaExp(polyItem.Coefficient + polyItem2.Coefficient), polyItem.Exponent + polyItem2.Exponent);
					polynom.PolyItems.Add(item);
				}
			}
			IEnumerable<int> enumerable = from x in polynom.PolyItems
				group x by x.Exponent into x
				where x.Count() > 1
				select x.First().Exponent;
			List<PolynomItem> list = new List<PolynomItem>();
			IList<int> toGlue = (enumerable as IList<int>) ?? enumerable.ToList();
			foreach (int exponent in toGlue)
			{
				int intVal = polynom.PolyItems.Where((PolynomItem x) => x.Exponent == exponent).Aggregate(0, (int current, PolynomItem polynomOld) => current ^ GetIntValFromAlphaExp(polynomOld.Coefficient));
				PolynomItem item2 = new PolynomItem(GetAlphaExpFromIntVal(intVal), exponent);
				list.Add(item2);
			}
			polynom.PolyItems.RemoveAll((PolynomItem x) => toGlue.Contains(x.Exponent));
			polynom.PolyItems.AddRange(list);
			polynom.PolyItems = polynom.PolyItems.OrderByDescending((PolynomItem x) => x.Exponent).ToList();
			return polynom;
		}

		private int GetIntValFromAlphaExp(int exp)
		{
			return (from alog in galoisField
				where alog.ExponentAlpha == exp
				select alog.IntegerValue).First();
		}

		private int GetAlphaExpFromIntVal(int intVal)
		{
			return (from alog in galoisField
				where alog.IntegerValue == intVal
				select alog.ExponentAlpha).First();
		}

		private static int ShrinkAlphaExp(int alphaExp)
		{
			return (int)((double)(alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
		}

		private void CreateAlphanumEncDict()
		{
			alphanumEncDict = new Dictionary<char, int>();
			for (int i = 0; i < alphanumEncTable.Length; i++)
			{
				alphanumEncDict.Add(alphanumEncTable[i], i);
			}
		}

		private void CreateAlignmentPatternTable()
		{
			alignmentPatternTable = new List<AlignmentPattern>();
			for (int i = 0; i < 280; i += 7)
			{
				List<Point> list = new List<Point>();
				for (int j = 0; j < 7; j++)
				{
					if (alignmentPatternBaseValues[i + j] == 0)
					{
						continue;
					}
					for (int k = 0; k < 7; k++)
					{
						if (alignmentPatternBaseValues[i + k] != 0)
						{
							Point item = new Point(alignmentPatternBaseValues[i + j] - 2, alignmentPatternBaseValues[i + k] - 2);
							if (!list.Contains(item))
							{
								list.Add(item);
							}
						}
					}
				}
				alignmentPatternTable.Add(new AlignmentPattern
				{
					Version = (i + 7) / 7,
					PatternPositions = list
				});
			}
		}

		private void CreateCapacityECCTable()
		{
			capacityECCTable = new List<ECCInfo>();
			for (int i = 0; i < 960; i += 24)
			{
				capacityECCTable.AddRange(new ECCInfo[4]
				{
					new ECCInfo((i + 24) / 24, ECCLevel.L, capacityECCBaseValues[i], capacityECCBaseValues[i + 1], capacityECCBaseValues[i + 2], capacityECCBaseValues[i + 3], capacityECCBaseValues[i + 4], capacityECCBaseValues[i + 5]),
					new ECCInfo((i + 24) / 24, ECCLevel.M, capacityECCBaseValues[i + 6], capacityECCBaseValues[i + 7], capacityECCBaseValues[i + 8], capacityECCBaseValues[i + 9], capacityECCBaseValues[i + 10], capacityECCBaseValues[i + 11]),
					new ECCInfo((i + 24) / 24, ECCLevel.Q, capacityECCBaseValues[i + 12], capacityECCBaseValues[i + 13], capacityECCBaseValues[i + 14], capacityECCBaseValues[i + 15], capacityECCBaseValues[i + 16], capacityECCBaseValues[i + 17]),
					new ECCInfo((i + 24) / 24, ECCLevel.H, capacityECCBaseValues[i + 18], capacityECCBaseValues[i + 19], capacityECCBaseValues[i + 20], capacityECCBaseValues[i + 21], capacityECCBaseValues[i + 22], capacityECCBaseValues[i + 23])
				});
			}
		}

		private void CreateCapacityTable()
		{
			capacityTable = new List<VersionInfo>();
			for (int i = 0; i < 640; i += 16)
			{
				capacityTable.Add(new VersionInfo((i + 16) / 16, new List<VersionInfoDetails>
				{
					new VersionInfoDetails(ECCLevel.L, new Dictionary<EncodingMode, int>
					{
						{
							EncodingMode.Numeric,
							capacityBaseValues[i]
						},
						{
							EncodingMode.Alphanumeric,
							capacityBaseValues[i + 1]
						},
						{
							EncodingMode.Byte,
							capacityBaseValues[i + 2]
						},
						{
							EncodingMode.Kanji,
							capacityBaseValues[i + 3]
						}
					}),
					new VersionInfoDetails(ECCLevel.M, new Dictionary<EncodingMode, int>
					{
						{
							EncodingMode.Numeric,
							capacityBaseValues[i + 4]
						},
						{
							EncodingMode.Alphanumeric,
							capacityBaseValues[i + 5]
						},
						{
							EncodingMode.Byte,
							capacityBaseValues[i + 6]
						},
						{
							EncodingMode.Kanji,
							capacityBaseValues[i + 7]
						}
					}),
					new VersionInfoDetails(ECCLevel.Q, new Dictionary<EncodingMode, int>
					{
						{
							EncodingMode.Numeric,
							capacityBaseValues[i + 8]
						},
						{
							EncodingMode.Alphanumeric,
							capacityBaseValues[i + 9]
						},
						{
							EncodingMode.Byte,
							capacityBaseValues[i + 10]
						},
						{
							EncodingMode.Kanji,
							capacityBaseValues[i + 11]
						}
					}),
					new VersionInfoDetails(ECCLevel.H, new Dictionary<EncodingMode, int>
					{
						{
							EncodingMode.Numeric,
							capacityBaseValues[i + 12]
						},
						{
							EncodingMode.Alphanumeric,
							capacityBaseValues[i + 13]
						},
						{
							EncodingMode.Byte,
							capacityBaseValues[i + 14]
						},
						{
							EncodingMode.Kanji,
							capacityBaseValues[i + 15]
						}
					})
				}));
			}
		}

		private void CreateAntilogTable()
		{
			galoisField = new List<Antilog>();
			int num = 1;
			for (int i = 0; i < 256; i++)
			{
				galoisField.Add(new Antilog(i, num));
				num *= 2;
				if (num > 255)
				{
					num ^= 0x11D;
				}
			}
		}

		public void Dispose()
		{
			alignmentPatternBaseValues = null;
			alignmentPatternTable = null;
			alphanumEncDict = null;
			capacityBaseValues = null;
			capacityECCBaseValues = null;
			capacityECCTable = null;
			capacityTable = null;
			galoisField = null;
			remainderBits = null;
		}
	}
	internal static class String40Methods
	{
		public static bool IsNullOrWhiteSpace(string value)
		{
			if (value == null)
			{
				return true;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (!char.IsWhiteSpace(value[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static string ReverseString(string str)
		{
			char[] array = str.ToCharArray();
			char[] array2 = new char[array.Length];
			int num = 0;
			int num2 = str.Length - 1;
			while (num < str.Length)
			{
				array2[num] = array[num2];
				num++;
				num2--;
			}
			return new string(array2);
		}

		public static bool IsAllDigit(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (!char.IsDigit(str[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
	public class SvgQRCode : AbstractQRCode, IDisposable
	{
		public enum SizingMode
		{
			WidthHeightAttribute,
			ViewBoxAttribute
		}

		public SvgQRCode()
		{
		}

		public SvgQRCode(QRCodeData data)
			: base(data)
		{
		}

		public string GetGraphic(int pixelsPerModule)
		{
			Size viewBox = new Size(pixelsPerModule * base.QrCodeData.ModuleMatrix.Count, pixelsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, Color.Black, Color.White);
		}

		public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
		{
			Size viewBox = new Size(pixelsPerModule * base.QrCodeData.ModuleMatrix.Count, pixelsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, darkColor, lightColor, drawQuietZones, sizingMode);
		}

		public string GetGraphic(int pixelsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
		{
			Size viewBox = new Size(pixelsPerModule * base.QrCodeData.ModuleMatrix.Count, pixelsPerModule * base.QrCodeData.ModuleMatrix.Count);
			return GetGraphic(viewBox, darkColorHex, lightColorHex, drawQuietZones, sizingMode);
		}

		public string GetGraphic(Size viewBox, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
		{
			return GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones, sizingMode);
		}

		public string GetGraphic(Size viewBox, Color darkColor, Color lightColor, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
		{
			return GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(darkColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(lightColor.ToArgb())), drawQuietZones, sizingMode);
		}

		public string GetGraphic(Size viewBox, string darkColorHex, string lightColorHex, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
		{
			int num = ((!drawQuietZones) ? 4 : 0);
			int num2 = base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? (num * 2) : 0);
			double num3 = (double)Math.Min(viewBox.Width, viewBox.Height) / (double)num2;
			double input = (double)num2 * num3;
			string arg = (sizingMode.Equals(SizingMode.WidthHeightAttribute) ? $"width=\"{viewBox.Width}\" height=\"{viewBox.Height}\"" : $"viewBox=\"0 0 {viewBox.Width} {viewBox.Height}\"");
			StringBuilder stringBuilder = new StringBuilder($"<svg version=\"1.1\" baseProfile=\"full\" shape-rendering=\"crispEdges\" {arg} xmlns=\"http://www.w3.org/2000/svg\">");
			stringBuilder.AppendLine($"<rect x=\"0\" y=\"0\" width=\"{CleanSvgVal(input)}\" height=\"{CleanSvgVal(input)}\" fill=\"{lightColorHex}\" />");
			for (int i = num; i < num + num2; i++)
			{
				for (int j = num; j < num + num2; j++)
				{
					if (base.QrCodeData.ModuleMatrix[j][i])
					{
						double input2 = (double)(i - num) * num3;
						double input3 = (double)(j - num) * num3;
						stringBuilder.AppendLine($"<rect x=\"{CleanSvgVal(input2)}\" y=\"{CleanSvgVal(input3)}\" width=\"{CleanSvgVal(num3)}\" height=\"{CleanSvgVal(num3)}\" fill=\"{darkColorHex}\" />");
					}
				}
			}
			stringBuilder.Append("</svg>");
			return stringBuilder.ToString();
		}

		private string CleanSvgVal(double input)
		{
			return input.ToString(CultureInfo.InvariantCulture);
		}
	}
	public class XamlQRCode : AbstractQRCode, IDisposable
	{
		public XamlQRCode()
		{
		}

		public XamlQRCode(QRCodeData data)
			: base(data)
		{
		}

		public DrawingImage GetGraphic(int pixelsPerModule)
		{
			return GetGraphic(pixelsPerModule, drawQuietZones: true);
		}

		public DrawingImage GetGraphic(int pixelsPerModule, bool drawQuietZones)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0044: Expected O, but got Unknown
			int num = base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? 8 : 0);
			Size viewBox = default(Size);
			((Size)(ref viewBox))..ctor((double)(pixelsPerModule * num), (double)(pixelsPerModule * num));
			return GetGraphic(viewBox, (Brush)new SolidColorBrush(Colors.Black), (Brush)new SolidColorBrush(Colors.White), drawQuietZones);
		}

		public DrawingImage GetGraphic(Size viewBox, bool drawQuietZones = true)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001c: Expected O, but got Unknown
			return GetGraphic(viewBox, (Brush)new SolidColorBrush(Colors.Black), (Brush)new SolidColorBrush(Colors.White), drawQuietZones);
		}

		public DrawingImage GetGraphic(int pixelsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			//IL_0052: Expected O, but got Unknown
			int num = base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? 8 : 0);
			Size viewBox = default(Size);
			((Size)(ref viewBox))..ctor((double)(pixelsPerModule * num), (double)(pixelsPerModule * num));
			return GetGraphic(viewBox, (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(darkColorHex)), (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString(lightColorHex)), drawQuietZones);
		}

		public DrawingImage GetGraphic(Size viewBox, Brush darkBrush, Brush lightBrush, bool drawQuietZones = true)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			int num = base.QrCodeData.ModuleMatrix.Count - ((!drawQuietZones) ? 8 : 0);
			double num2 = Math.Min(((Size)(ref viewBox)).Width, ((Size)(ref viewBox)).Height);
			double num3 = num2 / (double)num;
			int num4 = ((!drawQuietZones) ? 4 : 0);
			DrawingGroup val = new DrawingGroup();
			val.Children.Add((Drawing)new GeometryDrawing(lightBrush, (Pen)null, (Geometry)new RectangleGeometry(new Rect(new Point(0.0, 0.0), new Size(num2, num2)))));
			GeometryGroup val2 = new GeometryGroup();
			double num5 = 0.0;
			double num6 = 0.0;
			for (int i = num4; i < num; i++)
			{
				num6 = 0.0;
				for (int j = num4; j < num; j++)
				{
					if (base.QrCodeData.ModuleMatrix[j][i])
					{
						val2.Children.Add((Geometry)new RectangleGeometry(new Rect(num5, num6, num3, num3)));
					}
					num6 += num3;
				}
				num5 += num3;
			}
			val.Children.Add((Drawing)new GeometryDrawing(darkBrush, (Pen)null, (Geometry)(object)val2));
			return new DrawingImage((Drawing)(object)val);
		}
	}
}
namespace QRCoder.Framework4._0Methods
{
	internal class Stream4Methods
	{
		public static void CopyTo(Stream input, Stream output)
		{
			byte[] array = new byte[16384];
			int count;
			while ((count = input.Read(array, 0, array.Length)) > 0)
			{
				output.Write(array, 0, count);
			}
		}
	}
}

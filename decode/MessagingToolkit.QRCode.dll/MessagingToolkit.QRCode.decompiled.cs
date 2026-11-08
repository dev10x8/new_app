using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using MessagingToolkit.QRCode.Codec.Data;
using MessagingToolkit.QRCode.Codec.Ecc;
using MessagingToolkit.QRCode.Codec.Reader;
using MessagingToolkit.QRCode.Codec.Reader.Pattern;
using MessagingToolkit.QRCode.Codec.Util;
using MessagingToolkit.QRCode.ExceptionHandler;
using MessagingToolkit.QRCode.Geom;

[assembly: AssemblyProduct("")]
[assembly: CompilationRelaxations(8)]
[assembly: AssemblyTitle("MessagingToolkit.QRCode.NET Component")]
[assembly: AssemblyDescription("A library for QRCode encoding and decoding")]
[assembly: AssemblyCompany("TWIT88.COM")]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.Default | DebuggableAttribute.DebuggingModes.DisableOptimizations | DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints | DebuggableAttribute.DebuggingModes.EnableEditAndContinue)]
[assembly: AssemblyVersion("1.0.3541.23206")]
namespace MessagingToolkit.QRCode.Codec.Util
{
	public interface DebugCanvas
	{
		void println(string str);

		void drawPoint(MessagingToolkit.QRCode.Geom.Point point, int color);

		void drawCross(MessagingToolkit.QRCode.Geom.Point point, int color);

		void drawPoints(MessagingToolkit.QRCode.Geom.Point[] points, int color);

		void drawLine(Line line, int color);

		void drawLines(Line[] lines, int color);

		void drawPolygon(MessagingToolkit.QRCode.Geom.Point[] points, int color);

		void drawMatrix(bool[][] matrix);
	}
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Color_Fields
	{
		public static readonly int GRAY = 11184810;

		public static readonly int LIGHTGRAY = 12303291;

		public static readonly int DARKGRAY = 4473924;

		public static readonly int BLACK = 0;

		public static readonly int WHITE = 16777215;

		public static readonly int BLUE = 8947967;

		public static readonly int GREEN = 8978312;

		public static readonly int LIGHTBLUE = 12303359;

		public static readonly int LIGHTGREEN = 12320699;

		public static readonly int RED = 267946120;

		public static readonly int ORANGE = 16777096;

		public static readonly int LIGHTRED = 16759739;
	}
	public interface Color
	{
	}
}
namespace MessagingToolkit.QRCode.Codec
{
	public class QRCodeEncoder
	{
		public enum ENCODE_MODE
		{
			ALPHA_NUMERIC,
			NUMERIC,
			BYTE
		}

		public enum ERROR_CORRECTION
		{
			L,
			M,
			Q,
			H
		}

		internal static string DATA_PATH = "qrcode_data";

		internal static string QRCODE_DATA_PATH = string.Empty;

		internal ERROR_CORRECTION qrcodeErrorCorrect;

		internal ENCODE_MODE qrcodeEncodeMode;

		internal int qrcodeVersion;

		internal int qrcodeStructureappendN;

		internal int qrcodeStructureappendM;

		internal int qrcodeStructureappendParity;

		internal System.Drawing.Color qrCodeBackgroundColor;

		internal System.Drawing.Color qrCodeForegroundColor;

		internal int qrCodeScale;

		internal string qrcodeStructureappendOriginaldata;

		public virtual ERROR_CORRECTION QRCodeErrorCorrect
		{
			get
			{
				return qrcodeErrorCorrect;
			}
			set
			{
				qrcodeErrorCorrect = value;
			}
		}

		public virtual int QRCodeVersion
		{
			get
			{
				return qrcodeVersion;
			}
			set
			{
				if (value >= 0 && value <= 40)
				{
					qrcodeVersion = value;
				}
			}
		}

		public virtual ENCODE_MODE QRCodeEncodeMode
		{
			get
			{
				return qrcodeEncodeMode;
			}
			set
			{
				qrcodeEncodeMode = value;
			}
		}

		public virtual int QRCodeScale
		{
			get
			{
				return qrCodeScale;
			}
			set
			{
				qrCodeScale = value;
			}
		}

		public virtual System.Drawing.Color QRCodeBackgroundColor
		{
			get
			{
				return qrCodeBackgroundColor;
			}
			set
			{
				qrCodeBackgroundColor = value;
			}
		}

		public virtual System.Drawing.Color QRCodeForegroundColor
		{
			get
			{
				return qrCodeForegroundColor;
			}
			set
			{
				qrCodeForegroundColor = value;
			}
		}

		public QRCodeEncoder()
		{
			qrcodeErrorCorrect = ERROR_CORRECTION.M;
			qrcodeEncodeMode = ENCODE_MODE.BYTE;
			qrcodeVersion = 7;
			qrcodeStructureappendN = 0;
			qrcodeStructureappendM = 0;
			qrcodeStructureappendParity = 0;
			qrcodeStructureappendOriginaldata = "";
			qrCodeScale = 4;
			qrCodeBackgroundColor = System.Drawing.Color.White;
			qrCodeForegroundColor = System.Drawing.Color.Black;
		}

		public virtual void setStructureappend(int m, int n, int p)
		{
			if (n > 1 && n <= 16 && m > 0 && m <= 16 && p >= 0 && p <= 255)
			{
				qrcodeStructureappendM = m;
				qrcodeStructureappendN = n;
				qrcodeStructureappendParity = p;
			}
		}

		public virtual int calStructureappendParity(sbyte[] originaldata)
		{
			int i = 0;
			int num = 0;
			int num2 = originaldata.Length;
			if (num2 > 1)
			{
				num = 0;
				for (; i < num2; i++)
				{
					num ^= originaldata[i] & 0xFF;
				}
			}
			else
			{
				num = -1;
			}
			return num;
		}

		public virtual bool[][] calQrcode(byte[] qrcodeData)
		{
			int num = 0;
			int num2 = qrcodeData.Length;
			int[] array = new int[num2 + 32];
			sbyte[] array2 = new sbyte[num2 + 32];
			if (num2 <= 0)
			{
				bool[][] array3 = new bool[1][];
				bool[] array4 = new bool[1];
				array3[0] = array4;
				return array3;
			}
			if (qrcodeStructureappendN > 1)
			{
				array[0] = 3;
				array2[0] = 4;
				array[1] = qrcodeStructureappendM - 1;
				array2[1] = 4;
				array[2] = qrcodeStructureappendN - 1;
				array2[2] = 4;
				array[3] = qrcodeStructureappendParity;
				array2[3] = 8;
				num = 4;
			}
			array2[num] = 4;
			int[] array5;
			int num3;
			switch (qrcodeEncodeMode)
			{
			case ENCODE_MODE.ALPHA_NUMERIC:
			{
				array5 = new int[41]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2, 2, 2, 2, 2, 2, 4, 4, 4,
					4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
					4
				};
				array[num] = 2;
				num++;
				array[num] = num2;
				array2[num] = 9;
				num3 = num;
				num++;
				for (int i = 0; i < num2; i++)
				{
					char c = (char)qrcodeData[i];
					sbyte b = 0;
					if (c >= '0' && c < ':')
					{
						b = (sbyte)(c - 48);
					}
					else if (c >= 'A' && c < '[')
					{
						b = (sbyte)(c - 55);
					}
					else
					{
						if (c == ' ')
						{
							b = 36;
						}
						if (c == '$')
						{
							b = 37;
						}
						if (c == '%')
						{
							b = 38;
						}
						if (c == '*')
						{
							b = 39;
						}
						if (c == '+')
						{
							b = 40;
						}
						if (c == '-')
						{
							b = 41;
						}
						if (c == '.')
						{
							b = 42;
						}
						if (c == '/')
						{
							b = 43;
						}
						if (c == ':')
						{
							b = 44;
						}
					}
					if (i % 2 == 0)
					{
						array[num] = b;
						array2[num] = 6;
						continue;
					}
					array[num] = array[num] * 45 + b;
					array2[num] = 11;
					if (i < num2 - 1)
					{
						num++;
					}
				}
				num++;
				break;
			}
			case ENCODE_MODE.NUMERIC:
			{
				array5 = new int[41]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2, 2, 2, 2, 2, 2, 4, 4, 4,
					4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
					4
				};
				array[num] = 1;
				num++;
				array[num] = num2;
				array2[num] = 10;
				num3 = num;
				num++;
				for (int i = 0; i < num2; i++)
				{
					if (i % 3 == 0)
					{
						array[num] = qrcodeData[i] - 48;
						array2[num] = 4;
						continue;
					}
					array[num] = array[num] * 10 + (qrcodeData[i] - 48);
					if (i % 3 == 1)
					{
						array2[num] = 7;
						continue;
					}
					array2[num] = 10;
					if (i < num2 - 1)
					{
						num++;
					}
				}
				num++;
				break;
			}
			default:
			{
				array5 = new int[41]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
					8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
					8, 8, 8, 8, 8, 8, 8, 8, 8, 8,
					8
				};
				array[num] = 4;
				num++;
				array[num] = num2;
				array2[num] = 8;
				num3 = num;
				num++;
				for (int i = 0; i < num2; i++)
				{
					array[i + num] = qrcodeData[i] & 0xFF;
					array2[i + num] = 8;
				}
				num += num2;
				break;
			}
			}
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				num4 += array2[i];
			}
			int num5 = qrcodeErrorCorrect switch
			{
				ERROR_CORRECTION.L => 1, 
				ERROR_CORRECTION.Q => 3, 
				ERROR_CORRECTION.H => 2, 
				_ => 0, 
			};
			int[][] array6 = new int[4][]
			{
				new int[41]
				{
					0, 128, 224, 352, 512, 688, 864, 992, 1232, 1456,
					1728, 2032, 2320, 2672, 2920, 3320, 3624, 4056, 4504, 5016,
					5352, 5712, 6256, 6880, 7312, 8000, 8496, 9024, 9544, 10136,
					10984, 11640, 12328, 13048, 13800, 14496, 15312, 15936, 16816, 17728,
					18672
				},
				new int[41]
				{
					0, 152, 272, 440, 640, 864, 1088, 1248, 1552, 1856,
					2192, 2592, 2960, 3424, 3688, 4184, 4712, 5176, 5768, 6360,
					6888, 7456, 8048, 8752, 9392, 10208, 10960, 11744, 12248, 13048,
					13880, 14744, 15640, 16568, 17528, 18448, 19472, 20528, 21616, 22496,
					23648
				},
				new int[41]
				{
					0, 72, 128, 208, 288, 368, 480, 528, 688, 800,
					976, 1120, 1264, 1440, 1576, 1784, 2024, 2264, 2504, 2728,
					3080, 3248, 3536, 3712, 4112, 4304, 4768, 5024, 5288, 5608,
					5960, 6344, 6760, 7208, 7688, 7888, 8432, 8768, 9136, 9776,
					10208
				},
				new int[41]
				{
					0, 104, 176, 272, 384, 496, 608, 704, 880, 1056,
					1232, 1440, 1648, 1952, 2088, 2360, 2600, 2936, 3176, 3560,
					3880, 4096, 4544, 4912, 5312, 5744, 6032, 6464, 6968, 7288,
					7880, 8264, 8920, 9368, 9848, 10288, 10832, 11408, 12016, 12656,
					13328
				}
			};
			int num6 = 0;
			if (qrcodeVersion == 0)
			{
				qrcodeVersion = 1;
				for (int i = 1; i <= 40; i++)
				{
					if (array6[num5][i] >= num4 + array5[qrcodeVersion])
					{
						num6 = array6[num5][i];
						break;
					}
					qrcodeVersion++;
				}
			}
			else
			{
				num6 = array6[num5][qrcodeVersion];
			}
			num4 += array5[qrcodeVersion];
			array2[num3] = (sbyte)(array2[num3] + array5[qrcodeVersion]);
			int[] array7 = new int[41]
			{
				0, 26, 44, 70, 100, 134, 172, 196, 242, 292,
				346, 404, 466, 532, 581, 655, 733, 815, 901, 991,
				1085, 1156, 1258, 1364, 1474, 1588, 1706, 1828, 1921, 2051,
				2185, 2323, 2465, 2611, 2761, 2876, 3034, 3196, 3362, 3532,
				3706
			};
			int num7 = array7[qrcodeVersion];
			int num8 = 17 + (qrcodeVersion << 2);
			int[] array8 = new int[41]
			{
				0, 0, 7, 7, 7, 7, 7, 0, 0, 0,
				0, 0, 0, 0, 3, 3, 3, 3, 3, 3,
				3, 4, 4, 4, 4, 4, 4, 4, 3, 3,
				3, 3, 3, 3, 3, 0, 0, 0, 0, 0,
				0
			};
			int num9 = array8[qrcodeVersion] + (num7 << 3);
			sbyte[] array9 = new sbyte[num9];
			sbyte[] array10 = new sbyte[num9];
			sbyte[] array11 = new sbyte[num9];
			sbyte[] array12 = new sbyte[15];
			sbyte[] array13 = new sbyte[15];
			sbyte[] array14 = new sbyte[1];
			sbyte[] array15 = new sbyte[128];
			try
			{
				string fileName = DATA_PATH + ".qrv" + Convert.ToString(qrcodeVersion) + "_" + Convert.ToString(num5) + ".dat";
				Stream embeddedFile = GetEmbeddedFile(fileName);
				BufferedStream bufferedStream = new BufferedStream(embeddedFile);
				SystemUtils.ReadInput(bufferedStream, array9, 0, array9.Length);
				SystemUtils.ReadInput(bufferedStream, array10, 0, array10.Length);
				SystemUtils.ReadInput(bufferedStream, array11, 0, array11.Length);
				SystemUtils.ReadInput(bufferedStream, array12, 0, array12.Length);
				SystemUtils.ReadInput(bufferedStream, array13, 0, array13.Length);
				SystemUtils.ReadInput(bufferedStream, array14, 0, array14.Length);
				SystemUtils.ReadInput(bufferedStream, array15, 0, array15.Length);
				bufferedStream.Close();
				embeddedFile.Close();
			}
			catch (Exception throwable)
			{
				SystemUtils.WriteStackTrace(throwable, Console.Error);
			}
			sbyte b2 = 1;
			for (sbyte b3 = 1; b3 < 128; b3++)
			{
				if (array15[b3] == 0)
				{
					b2 = b3;
					break;
				}
			}
			sbyte[] array16 = new sbyte[b2];
			Array.Copy(array15, 0, array16, 0, (byte)b2);
			sbyte[] array17 = new sbyte[15]
			{
				0, 1, 2, 3, 4, 5, 7, 8, 8, 8,
				8, 8, 8, 8, 8
			};
			sbyte[] array18 = new sbyte[15]
			{
				8, 8, 8, 8, 8, 8, 8, 8, 7, 5,
				4, 3, 2, 1, 0
			};
			int maxDataCodewords = num6 >> 3;
			int num10 = 4 * qrcodeVersion + 17;
			int num11 = num10 * num10;
			sbyte[] array19 = new sbyte[num11 + num10];
			try
			{
				string fileName = DATA_PATH + ".qrvfr" + Convert.ToString(qrcodeVersion) + ".dat";
				Stream embeddedFile = GetEmbeddedFile(fileName);
				BufferedStream bufferedStream = new BufferedStream(embeddedFile);
				SystemUtils.ReadInput(bufferedStream, array19, 0, array19.Length);
				bufferedStream.Close();
				embeddedFile.Close();
			}
			catch (Exception throwable)
			{
				SystemUtils.WriteStackTrace(throwable, Console.Error);
			}
			if (num4 <= num6 - 4)
			{
				array[num] = 0;
				array2[num] = 4;
			}
			else if (num4 < num6)
			{
				array[num] = 0;
				array2[num] = (sbyte)(num6 - num4);
			}
			else if (num4 > num6)
			{
				Console.Out.WriteLine("overflow");
			}
			sbyte[] codewords = divideDataBy8Bits(array, array2, maxDataCodewords);
			sbyte[] array20 = calculateRSECC(codewords, array14[0], array16, maxDataCodewords, num7);
			sbyte[][] array21 = new sbyte[num10][];
			for (int j = 0; j < num10; j++)
			{
				array21[j] = new sbyte[num10];
			}
			for (int i = 0; i < num10; i++)
			{
				for (int k = 0; k < num10; k++)
				{
					array21[k][i] = 0;
				}
			}
			for (int i = 0; i < num7; i++)
			{
				sbyte b4 = array20[i];
				for (int k = 7; k >= 0; k--)
				{
					int num12 = i * 8 + k;
					array21[array9[num12] & 0xFF][array10[num12] & 0xFF] = (sbyte)((255 * (b4 & 1)) ^ array11[num12]);
					b4 = (sbyte)SystemUtils.URShift(b4 & 0xFF, 1);
				}
			}
			for (int num13 = array8[qrcodeVersion]; num13 > 0; num13--)
			{
				int num14 = num13 + num7 * 8 - 1;
				array21[array9[num14] & 0xFF][array10[num14] & 0xFF] = (sbyte)(0xFF ^ array11[num14]);
			}
			sbyte b5 = selectMask(array21, array8[qrcodeVersion] + num7 * 8);
			sbyte b6 = (sbyte)(1 << (int)b5);
			sbyte b7 = (sbyte)((num5 << 3) | b5);
			string[] array22 = new string[32]
			{
				"101010000010010", "101000100100101", "101111001111100", "101101101001011", "100010111111001", "100000011001110", "100111110010111", "100101010100000", "111011111000100", "111001011110011",
				"111110110101010", "111100010011101", "110011000101111", "110001100011000", "110110001000001", "110100101110110", "001011010001001", "001001110111110", "001110011100111", "001100111010000",
				"000011101100010", "000001001010101", "000110100001100", "000100000111011", "011010101011111", "011000001101000", "011111100110001", "011101000000110", "010010010110100", "010000110000011",
				"010111011011010", "010101111101101"
			};
			for (int i = 0; i < 15; i++)
			{
				sbyte b8 = sbyte.Parse(array22[b7].Substring(i, i + 1 - i));
				array21[array17[i] & 0xFF][array18[i] & 0xFF] = (sbyte)(b8 * 255);
				array21[array12[i] & 0xFF][array13[i] & 0xFF] = (sbyte)(b8 * 255);
			}
			bool[][] array23 = new bool[num10][];
			for (int l = 0; l < num10; l++)
			{
				array23[l] = new bool[num10];
			}
			int num15 = 0;
			for (int i = 0; i < num10; i++)
			{
				for (int k = 0; k < num10; k++)
				{
					if ((array21[k][i] & b6) != 0 || array19[num15] == 49)
					{
						array23[k][i] = true;
					}
					else
					{
						array23[k][i] = false;
					}
					num15++;
				}
				num15++;
			}
			return array23;
		}

		private static sbyte[] divideDataBy8Bits(int[] data, sbyte[] bits, int maxDataCodewords)
		{
			int num = bits.Length;
			int num2 = 0;
			int num3 = 8;
			int num4 = 0;
			if (num != data.Length)
			{
			}
			for (int i = 0; i < num; i++)
			{
				num4 += bits[i];
			}
			int num5 = (num4 - 1) / 8 + 1;
			sbyte[] array = new sbyte[maxDataCodewords];
			for (int i = 0; i < num5; i++)
			{
				array[i] = 0;
			}
			for (int i = 0; i < num; i++)
			{
				int num6 = data[i];
				int num7 = bits[i];
				bool flag = true;
				if (num7 == 0)
				{
					break;
				}
				while (flag)
				{
					if (num3 > num7)
					{
						array[num2] = (sbyte)((array[num2] << num7) | num6);
						num3 -= num7;
						flag = false;
						continue;
					}
					num7 -= num3;
					array[num2] = (sbyte)((array[num2] << num3) | (num6 >> num7));
					if (num7 == 0)
					{
						flag = false;
					}
					else
					{
						num6 &= (1 << num7) - 1;
						flag = true;
					}
					num2++;
					num3 = 8;
				}
			}
			if (num3 != 8)
			{
				array[num2] = (sbyte)(array[num2] << num3);
			}
			else
			{
				num2--;
			}
			if (num2 < maxDataCodewords - 1)
			{
				bool flag = true;
				while (num2 < maxDataCodewords - 1)
				{
					num2++;
					if (flag)
					{
						array[num2] = -20;
					}
					else
					{
						array[num2] = 17;
					}
					flag = !flag;
				}
			}
			return array;
		}

		private static sbyte[] calculateRSECC(sbyte[] codewords, sbyte rsEccCodewords, sbyte[] rsBlockOrder, int maxDataCodewords, int maxCodewords)
		{
			sbyte[][] array = new sbyte[256][];
			for (int i = 0; i < 256; i++)
			{
				array[i] = new sbyte[rsEccCodewords];
			}
			try
			{
				string fileName = DATA_PATH + ".rsc" + rsEccCodewords + ".dat";
				Stream embeddedFile = GetEmbeddedFile(fileName);
				BufferedStream bufferedStream = new BufferedStream(embeddedFile);
				for (int i = 0; i < 256; i++)
				{
					SystemUtils.ReadInput(bufferedStream, array[i], 0, array[i].Length);
				}
				bufferedStream.Close();
				embeddedFile.Close();
			}
			catch (Exception throwable)
			{
				SystemUtils.WriteStackTrace(throwable, Console.Error);
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			sbyte[][] array2 = new sbyte[rsBlockOrder.Length][];
			sbyte[] array3 = new sbyte[maxCodewords];
			Array.Copy(codewords, 0, array3, 0, codewords.Length);
			for (num = 0; num < rsBlockOrder.Length; num++)
			{
				array2[num] = new sbyte[(rsBlockOrder[num] & 0xFF) - rsEccCodewords];
			}
			for (num = 0; num < maxDataCodewords; num++)
			{
				array2[num3][num2] = codewords[num];
				num2++;
				if (num2 >= (rsBlockOrder[num3] & 0xFF) - rsEccCodewords)
				{
					num2 = 0;
					num3++;
				}
			}
			for (num3 = 0; num3 < rsBlockOrder.Length; num3++)
			{
				sbyte[] array4 = new sbyte[array2[num3].Length];
				array2[num3].CopyTo(array4, 0);
				int num4 = rsBlockOrder[num3] & 0xFF;
				int num5 = num4 - rsEccCodewords;
				for (num2 = num5; num2 > 0; num2--)
				{
					sbyte b = array4[0];
					if (b != 0)
					{
						sbyte[] array5 = new sbyte[array4.Length - 1];
						Array.Copy(array4, 1, array5, 0, array4.Length - 1);
						sbyte[] xb = array[b & 0xFF];
						array4 = calculateByteArrayBits(array5, xb, "xor");
					}
					else if (rsEccCodewords < array4.Length)
					{
						sbyte[] array6 = new sbyte[array4.Length - 1];
						Array.Copy(array4, 1, array6, 0, array4.Length - 1);
						array4 = new sbyte[array6.Length];
						array6.CopyTo(array4, 0);
					}
					else
					{
						sbyte[] array6 = new sbyte[rsEccCodewords];
						Array.Copy(array4, 1, array6, 0, array4.Length - 1);
						array6[rsEccCodewords - 1] = 0;
						array4 = new sbyte[array6.Length];
						array6.CopyTo(array4, 0);
					}
				}
				Array.Copy(array4, 0, array3, codewords.Length + num3 * rsEccCodewords, (byte)rsEccCodewords);
			}
			return array3;
		}

		private static sbyte[] calculateByteArrayBits(sbyte[] xa, sbyte[] xb, string ind)
		{
			sbyte[] array;
			sbyte[] array2;
			if (xa.Length > xb.Length)
			{
				array = new sbyte[xa.Length];
				xa.CopyTo(array, 0);
				array2 = new sbyte[xb.Length];
				xb.CopyTo(array2, 0);
			}
			else
			{
				array = new sbyte[xb.Length];
				xb.CopyTo(array, 0);
				array2 = new sbyte[xa.Length];
				xa.CopyTo(array2, 0);
			}
			int num = array.Length;
			int num2 = array2.Length;
			sbyte[] array3 = new sbyte[num];
			for (int i = 0; i < num; i++)
			{
				if (i < num2)
				{
					if ((object)ind == "xor")
					{
						array3[i] = (sbyte)(array[i] ^ array2[i]);
					}
					else
					{
						array3[i] = (sbyte)(array[i] | array2[i]);
					}
				}
				else
				{
					array3[i] = array[i];
				}
			}
			return array3;
		}

		private static sbyte selectMask(sbyte[][] matrixContent, int maxCodewordsBitWithRemain)
		{
			int num = matrixContent.Length;
			int[] array = new int[8];
			int[] array2 = array;
			array = new int[8];
			int[] array3 = array;
			array = new int[8];
			int[] array4 = array;
			array = new int[8];
			int[] array5 = array;
			int num2 = 0;
			int num3 = 0;
			array = new int[8];
			int[] array6 = array;
			for (int i = 0; i < num; i++)
			{
				array = new int[8];
				int[] array7 = array;
				array = new int[8];
				int[] array8 = array;
				bool[] array9 = new bool[8];
				bool[] array10 = array9;
				array9 = new bool[8];
				bool[] array11 = array9;
				for (int j = 0; j < num; j++)
				{
					if (j > 0 && i > 0)
					{
						num2 = matrixContent[j][i] & matrixContent[j - 1][i] & matrixContent[j][i - 1] & matrixContent[j - 1][i - 1] & 0xFF;
						num3 = (matrixContent[j][i] & 0xFF) | (matrixContent[j - 1][i] & 0xFF) | (matrixContent[j][i - 1] & 0xFF) | (matrixContent[j - 1][i - 1] & 0xFF);
					}
					for (int k = 0; k < 8; k++)
					{
						array7[k] = ((array7[k] & 0x3F) << 1) | (SystemUtils.URShift(matrixContent[j][i] & 0xFF, k) & 1);
						array8[k] = ((array8[k] & 0x3F) << 1) | (SystemUtils.URShift(matrixContent[i][j] & 0xFF, k) & 1);
						if ((matrixContent[j][i] & (1 << k)) != 0)
						{
							array6[k]++;
						}
						if (array7[k] == 93)
						{
							array4[k] += 40;
						}
						if (array8[k] == 93)
						{
							array4[k] += 40;
						}
						if (j > 0 && i > 0)
						{
							if (((uint)num2 & (true ? 1u : 0u)) != 0 || (num3 & 1) == 0)
							{
								array3[k] += 3;
							}
							num2 >>= 1;
							num3 >>= 1;
						}
						if ((array7[k] & 0x1F) == 0 || (array7[k] & 0x1F) == 31)
						{
							if (j > 3)
							{
								if (array10[k])
								{
									array2[k]++;
								}
								else
								{
									array2[k] += 3;
									array10[k] = true;
								}
							}
						}
						else
						{
							array10[k] = false;
						}
						if ((array8[k] & 0x1F) == 0 || (array8[k] & 0x1F) == 31)
						{
							if (j > 3)
							{
								if (array11[k])
								{
									array2[k]++;
									continue;
								}
								array2[k] += 3;
								array11[k] = true;
							}
						}
						else
						{
							array11[k] = false;
						}
					}
				}
			}
			int num4 = 0;
			sbyte result = 0;
			int[] array12 = new int[21]
			{
				90, 80, 70, 60, 50, 40, 30, 20, 10, 0,
				0, 10, 20, 30, 40, 50, 60, 70, 80, 90,
				90
			};
			for (int k = 0; k < 8; k++)
			{
				array5[k] = array12[20 * array6[k] / maxCodewordsBitWithRemain];
				int num5 = array2[k] + array3[k] + array4[k] + array5[k];
				if (num5 < num4 || k == 0)
				{
					result = (sbyte)k;
					num4 = num5;
				}
			}
			return result;
		}

		public virtual Bitmap Encode(string content, Encoding encoding)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			bool[][] array = calQrcode(encoding.GetBytes(content));
			SolidBrush val = new SolidBrush(qrCodeBackgroundColor);
			Bitmap val2 = new Bitmap(array.Length * qrCodeScale + 1, array.Length * qrCodeScale + 1);
			Graphics val3 = Graphics.FromImage((Image)(object)val2);
			val3.FillRectangle((Brush)(object)val, new Rectangle(0, 0, ((Image)val2).Width, ((Image)val2).Height));
			val.Color = qrCodeForegroundColor;
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j][i])
					{
						val3.FillRectangle((Brush)(object)val, j * qrCodeScale, i * qrCodeScale, qrCodeScale, qrCodeScale);
					}
				}
			}
			return val2;
		}

		public virtual Bitmap Encode(string content)
		{
			if (QRCodeUtility.IsUniCode(content))
			{
				return Encode(content, Encoding.Unicode);
			}
			return Encode(content, Encoding.ASCII);
		}

		internal static Stream GetEmbeddedFile(string fileName)
		{
			string name = typeof(QRCodeEncoder).Assembly.GetName().Name;
			try
			{
				Assembly assembly = Assembly.Load(name);
				Stream manifestResourceStream = assembly.GetManifestResourceStream(name + "." + fileName);
				if (manifestResourceStream == null)
				{
					throw new Exception("Could not locate embedded resource '" + fileName + "' in assembly '" + name + "'");
				}
				return manifestResourceStream;
			}
			catch (Exception ex)
			{
				throw new Exception(name + ": " + ex.Message);
			}
		}

		internal static XmlDocument GetEmbeddedXml(string fileName)
		{
			Stream embeddedFile = GetEmbeddedFile(fileName);
			XmlTextReader reader = new XmlTextReader(embeddedFile);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
			return xmlDocument;
		}

		internal static Bitmap GetEmbeddedImage(string fileName)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			Stream embeddedFile = GetEmbeddedFile(fileName);
			return new Bitmap(embeddedFile);
		}
	}
}
namespace MessagingToolkit.QRCode.Geom
{
	public class Point
	{
		public const int RIGHT = 1;

		public const int BOTTOM = 2;

		public const int LEFT = 4;

		public const int TOP = 8;

		internal int x;

		internal int y;

		public virtual int X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		public virtual int Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		public Point()
		{
			x = 0;
			y = 0;
		}

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public virtual void translate(int dx, int dy)
		{
			x += dx;
			y += dy;
		}

		public virtual void set_Renamed(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return "(" + Convert.ToString(x) + "," + Convert.ToString(y) + ")";
		}

		public static Point getCenter(Point p1, Point p2)
		{
			return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
		}

		public bool equals(Point compare)
		{
			if (x == compare.x && y == compare.y)
			{
				return true;
			}
			return false;
		}

		public virtual int distanceOf(Point other)
		{
			int num = other.X;
			int num2 = other.Y;
			return QRCodeUtility.sqrt((x - num) * (x - num) + (y - num2) * (y - num2));
		}
	}
	public class Axis
	{
		internal int sin;

		internal int cos;

		internal int modulePitch;

		internal Point origin;

		public virtual Point Origin
		{
			set
			{
				origin = value;
			}
		}

		public virtual int ModulePitch
		{
			set
			{
				modulePitch = value;
			}
		}

		public Axis(int[] angle, int modulePitch)
		{
			sin = angle[0];
			cos = angle[1];
			this.modulePitch = modulePitch;
			origin = new Point();
		}

		public virtual Point translate(Point offset)
		{
			int x = offset.X;
			int y = offset.Y;
			return translate(x, y);
		}

		public virtual Point translate(Point origin, Point offset)
		{
			Origin = origin;
			int x = offset.X;
			int y = offset.Y;
			return translate(x, y);
		}

		public virtual Point translate(Point origin, int moveX, int moveY)
		{
			Origin = origin;
			return translate(moveX, moveY);
		}

		public virtual Point translate(Point origin, int modulePitch, int moveX, int moveY)
		{
			Origin = origin;
			this.modulePitch = modulePitch;
			return translate(moveX, moveY);
		}

		public virtual Point translate(int moveX, int moveY)
		{
			long num = QRCodeImageReader.DECIMAL_POINT;
			Point point = new Point();
			int num2 = ((moveX != 0) ? (modulePitch * moveX >> (int)num) : 0);
			int num3 = ((moveY != 0) ? (modulePitch * moveY >> (int)num) : 0);
			point.translate(num2 * cos - num3 * sin >> (int)num, num2 * sin + num3 * cos >> (int)num);
			point.translate(origin.X, origin.Y);
			return point;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Util
{
	public class ContentConverter
	{
		internal static char n = '\n';

		public static string convert(string targetString)
		{
			if (targetString == null)
			{
				return targetString;
			}
			if (targetString.IndexOf("MEBKM:") > -1)
			{
				targetString = convertDocomoBookmark(targetString);
			}
			if (targetString.IndexOf("MECARD:") > -1)
			{
				targetString = convertDocomoAddressBook(targetString);
			}
			if (targetString.IndexOf("MATMSG:") > -1)
			{
				targetString = convertDocomoMailto(targetString);
			}
			if (targetString.IndexOf("http\\://") > -1)
			{
				targetString = replaceString(targetString, "http\\://", "\nhttp://");
			}
			return targetString;
		}

		private static string convertDocomoBookmark(string targetString)
		{
			targetString = removeString(targetString, "MEBKM:");
			targetString = removeString(targetString, "TITLE:");
			targetString = removeString(targetString, ";");
			targetString = removeString(targetString, "URL:");
			return targetString;
		}

		private static string convertDocomoAddressBook(string targetString)
		{
			targetString = removeString(targetString, "MECARD:");
			targetString = removeString(targetString, ";");
			targetString = replaceString(targetString, "N:", "NAME1:");
			targetString = replaceString(targetString, "SOUND:", n + "NAME2:");
			targetString = replaceString(targetString, "TEL:", n + "TEL1:");
			targetString = replaceString(targetString, "EMAIL:", n + "MAIL1:");
			targetString += n;
			return targetString;
		}

		private static string convertDocomoMailto(string s)
		{
			string s2 = s;
			char c = '\n';
			s2 = removeString(s2, "MATMSG:");
			s2 = removeString(s2, ";");
			s2 = replaceString(s2, "TO:", "MAILTO:");
			s2 = replaceString(s2, "SUB:", c + "SUBJECT:");
			s2 = replaceString(s2, "BODY:", c + "BODY:");
			return s2 + c;
		}

		private static string replaceString(string s, string s1, string s2)
		{
			string text = s;
			for (int num = text.IndexOf(s1, 0); num > -1; num = text.IndexOf(s1, num + s2.Length))
			{
				text = text.Substring(0, num) + s2 + text.Substring(num + s1.Length);
			}
			return text;
		}

		private static string removeString(string s, string s1)
		{
			return replaceString(s, s1, "");
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class VersionInformationException : ArgumentException
	{
	}
	[Serializable]
	public class InvalidVersionException : VersionInformationException
	{
		internal string message;

		public override string Message => message;

		public InvalidVersionException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Reader.Pattern
{
	public class FinderPattern
	{
		public const int UL = 0;

		public const int UR = 1;

		public const int DL = 2;

		internal static readonly int[] VersionInfoBit;

		internal static DebugCanvas canvas;

		internal MessagingToolkit.QRCode.Geom.Point[] center;

		internal int version;

		internal int[] sincos;

		internal int[] width;

		internal int[] moduleSize;

		public virtual int Version => version;

		public virtual int SqrtNumModules => 17 + 4 * version;

		public static FinderPattern findFinderPattern(bool[][] image)
		{
			Line[] lineAcross = findLineAcross(image);
			Line[] crossLines = findLineCross(lineAcross);
			MessagingToolkit.QRCode.Geom.Point[] array = null;
			try
			{
				array = getCenter(crossLines);
			}
			catch (FinderPatternNotFoundException ex)
			{
				throw ex;
			}
			int[] angle = getAngle(array);
			array = sort(array, angle);
			int[] array2 = getWidth(image, array, angle);
			int[] array3 = new int[3]
			{
				(array2[0] << QRCodeImageReader.DECIMAL_POINT) / 7,
				(array2[1] << QRCodeImageReader.DECIMAL_POINT) / 7,
				(array2[2] << QRCodeImageReader.DECIMAL_POINT) / 7
			};
			int num = calcRoughVersion(array, array2);
			if (num > 6)
			{
				try
				{
					num = calcExactVersion(array, angle, array3, image);
				}
				catch (VersionInformationException)
				{
				}
			}
			return new FinderPattern(array, num, angle, array2, array3);
		}

		internal FinderPattern(MessagingToolkit.QRCode.Geom.Point[] center, int version, int[] sincos, int[] width, int[] moduleSize)
		{
			this.center = center;
			this.version = version;
			this.sincos = sincos;
			this.width = width;
			this.moduleSize = moduleSize;
		}

		public virtual MessagingToolkit.QRCode.Geom.Point[] getCenter()
		{
			return center;
		}

		public virtual MessagingToolkit.QRCode.Geom.Point getCenter(int position)
		{
			if (position >= 0 && position <= 2)
			{
				return center[position];
			}
			return null;
		}

		public virtual int getWidth(int position)
		{
			return width[position];
		}

		public virtual int[] getAngle()
		{
			return sincos;
		}

		public virtual int getModuleSize()
		{
			return moduleSize[0];
		}

		public virtual int getModuleSize(int place)
		{
			return moduleSize[place];
		}

		internal static Line[] findLineAcross(bool[][] image)
		{
			int num = 0;
			int num2 = 1;
			int num3 = image.Length;
			int num4 = image[0].Length;
			MessagingToolkit.QRCode.Geom.Point point = new MessagingToolkit.QRCode.Geom.Point();
			ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
			int[] array = new int[5];
			int num5 = 0;
			int num6 = num;
			bool flag = false;
			while (true)
			{
				bool flag2 = true;
				bool flag3 = image[point.X][point.Y];
				if (flag3 == flag)
				{
					array[num5]++;
				}
				else
				{
					if (!flag3 && checkPattern(array, num5))
					{
						int num7;
						int x;
						int num8;
						int y;
						if (num6 == num)
						{
							num7 = point.X;
							for (int i = 0; i < 5; i++)
							{
								num7 -= array[i];
							}
							x = point.X - 1;
							num8 = (y = point.Y);
						}
						else
						{
							num7 = (x = point.X);
							num8 = point.Y;
							for (int i = 0; i < 5; i++)
							{
								num8 -= array[i];
							}
							y = point.Y - 1;
						}
						arrayList.Add(new Line(num7, num8, x, y));
					}
					num5 = (num5 + 1) % 5;
					array[num5] = 1;
					flag = !flag;
				}
				if (num6 == num)
				{
					if (point.X < num3 - 1)
					{
						point.translate(1, 0);
					}
					else if (point.Y < num4 - 1)
					{
						point.set_Renamed(0, point.Y + 1);
						array = new int[5];
					}
					else
					{
						point.set_Renamed(0, 0);
						array = new int[5];
						num6 = num2;
					}
				}
				else if (point.Y < num4 - 1)
				{
					point.translate(0, 1);
				}
				else
				{
					if (point.X >= num3 - 1)
					{
						break;
					}
					point.set_Renamed(point.X + 1, 0);
					array = new int[5];
				}
			}
			Line[] array2 = new Line[arrayList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = (Line)arrayList[j];
			}
			canvas.drawLines(array2, Color_Fields.LIGHTGREEN);
			return array2;
		}

		internal static bool checkPattern(int[] buffer, int pointer)
		{
			int[] array = new int[5] { 1, 1, 3, 1, 1 };
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				num += buffer[i];
			}
			num <<= QRCodeImageReader.DECIMAL_POINT;
			num /= 7;
			for (int j = 0; j < 5; j++)
			{
				int num2 = num * array[j] - num / 2;
				int num3 = num * array[j] + num / 2;
				int num4 = buffer[(pointer + j + 1) % 5] << QRCodeImageReader.DECIMAL_POINT;
				if (num4 < num2 || num4 > num3)
				{
					return false;
				}
			}
			return true;
		}

		internal static Line[] findLineCross(Line[] lineAcross)
		{
			ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
			ArrayList arrayList2 = ArrayList.Synchronized(new ArrayList(10));
			ArrayList arrayList3 = ArrayList.Synchronized(new ArrayList(10));
			for (int i = 0; i < lineAcross.Length; i++)
			{
				arrayList3.Add(lineAcross[i]);
			}
			for (int i = 0; i < arrayList3.Count - 1; i++)
			{
				arrayList2.Clear();
				arrayList2.Add(arrayList3[i]);
				for (int j = i + 1; j < arrayList3.Count; j++)
				{
					if (Line.isNeighbor((Line)arrayList2[arrayList2.Count - 1], (Line)arrayList3[j]))
					{
						arrayList2.Add(arrayList3[j]);
						Line line = (Line)arrayList2[arrayList2.Count - 1];
						if (arrayList2.Count * 5 > line.Length && j == arrayList3.Count - 1)
						{
							arrayList.Add(arrayList2[arrayList2.Count / 2]);
							for (int k = 0; k < arrayList2.Count; k++)
							{
								arrayList3.Remove(arrayList2[k]);
							}
						}
					}
					else
					{
						if (!cantNeighbor((Line)arrayList2[arrayList2.Count - 1], (Line)arrayList3[j]) && j != arrayList3.Count - 1)
						{
							continue;
						}
						Line line = (Line)arrayList2[arrayList2.Count - 1];
						if (arrayList2.Count * 6 > line.Length)
						{
							arrayList.Add(arrayList2[arrayList2.Count / 2]);
							for (int k = 0; k < arrayList2.Count; k++)
							{
								arrayList3.Remove(arrayList2[k]);
							}
						}
						break;
					}
				}
			}
			Line[] array = new Line[arrayList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (Line)arrayList[i];
			}
			return array;
		}

		internal static bool cantNeighbor(Line line1, Line line2)
		{
			if (Line.isCross(line1, line2))
			{
				return true;
			}
			if (line1.Horizontal)
			{
				if (Math.Abs(line1.getP1().Y - line2.getP1().Y) > 1)
				{
					return true;
				}
				return false;
			}
			if (Math.Abs(line1.getP1().X - line2.getP1().X) > 1)
			{
				return true;
			}
			return false;
		}

		internal static int[] getAngle(MessagingToolkit.QRCode.Geom.Point[] centers)
		{
			Line[] array = new Line[3];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Line(centers[i], centers[(i + 1) % array.Length]);
			}
			Line longest = Line.getLongest(array);
			MessagingToolkit.QRCode.Geom.Point point = new MessagingToolkit.QRCode.Geom.Point();
			for (int i = 0; i < centers.Length; i++)
			{
				if (!longest.getP1().equals(centers[i]) && !longest.getP2().equals(centers[i]))
				{
					point = centers[i];
					break;
				}
			}
			canvas.println("originPoint is: " + point);
			MessagingToolkit.QRCode.Geom.Point point2 = new MessagingToolkit.QRCode.Geom.Point();
			point2 = (((point.Y <= longest.getP1().Y) & (point.Y <= longest.getP2().Y)) ? ((longest.getP1().X >= longest.getP2().X) ? longest.getP1() : longest.getP2()) : (((point.X >= longest.getP1().X) & (point.X >= longest.getP2().X)) ? ((longest.getP1().Y >= longest.getP2().Y) ? longest.getP1() : longest.getP2()) : (((point.Y >= longest.getP1().Y) & (point.Y >= longest.getP2().Y)) ? ((longest.getP1().X >= longest.getP2().X) ? longest.getP2() : longest.getP1()) : ((longest.getP1().Y >= longest.getP2().Y) ? longest.getP2() : longest.getP1()))));
			int length = new Line(point, point2).Length;
			return new int[2]
			{
				(point2.Y - point.Y << QRCodeImageReader.DECIMAL_POINT) / length,
				(point2.X - point.X << QRCodeImageReader.DECIMAL_POINT) / length
			};
		}

		internal static MessagingToolkit.QRCode.Geom.Point[] getCenter(Line[] crossLines)
		{
			ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
			for (int i = 0; i < crossLines.Length - 1; i++)
			{
				Line line = crossLines[i];
				for (int j = i + 1; j < crossLines.Length; j++)
				{
					Line line2 = crossLines[j];
					if (Line.isCross(line, line2))
					{
						int num = 0;
						int num2 = 0;
						if (line.Horizontal)
						{
							num = line.Center.X;
							num2 = line2.Center.Y;
						}
						else
						{
							num = line2.Center.X;
							num2 = line.Center.Y;
						}
						arrayList.Add(new MessagingToolkit.QRCode.Geom.Point(num, num2));
					}
				}
			}
			MessagingToolkit.QRCode.Geom.Point[] array = new MessagingToolkit.QRCode.Geom.Point[arrayList.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (MessagingToolkit.QRCode.Geom.Point)arrayList[i];
			}
			if (array.Length == 3)
			{
				canvas.drawPolygon(array, Color_Fields.RED);
				return array;
			}
			throw new FinderPatternNotFoundException("Invalid number of Finder Pattern detected");
		}

		internal static MessagingToolkit.QRCode.Geom.Point[] sort(MessagingToolkit.QRCode.Geom.Point[] centers, int[] angle)
		{
			MessagingToolkit.QRCode.Geom.Point[] array = new MessagingToolkit.QRCode.Geom.Point[3];
			switch (getURQuadant(angle))
			{
			case 1:
				array[1] = getPointAtSide(centers, 1, 2);
				array[2] = getPointAtSide(centers, 2, 4);
				break;
			case 2:
				array[1] = getPointAtSide(centers, 2, 4);
				array[2] = getPointAtSide(centers, 8, 4);
				break;
			case 3:
				array[1] = getPointAtSide(centers, 4, 8);
				array[2] = getPointAtSide(centers, 1, 8);
				break;
			case 4:
				array[1] = getPointAtSide(centers, 8, 1);
				array[2] = getPointAtSide(centers, 2, 1);
				break;
			}
			for (int i = 0; i < centers.Length; i++)
			{
				if (!centers[i].equals(array[1]) && !centers[i].equals(array[2]))
				{
					array[0] = centers[i];
				}
			}
			return array;
		}

		internal static int getURQuadant(int[] angle)
		{
			int num = angle[0];
			int num2 = angle[1];
			if (num >= 0 && num2 > 0)
			{
				return 1;
			}
			if (num > 0 && num2 <= 0)
			{
				return 2;
			}
			if (num <= 0 && num2 < 0)
			{
				return 3;
			}
			if (num < 0 && num2 >= 0)
			{
				return 4;
			}
			return 0;
		}

		internal static MessagingToolkit.QRCode.Geom.Point getPointAtSide(MessagingToolkit.QRCode.Geom.Point[] points, int side1, int side2)
		{
			MessagingToolkit.QRCode.Geom.Point point = new MessagingToolkit.QRCode.Geom.Point();
			int x = ((side1 != 1 && side2 != 1) ? int.MaxValue : 0);
			int y = ((side1 != 2 && side2 != 2) ? int.MaxValue : 0);
			point = new MessagingToolkit.QRCode.Geom.Point(x, y);
			for (int i = 0; i < points.Length; i++)
			{
				switch (side1)
				{
				case 1:
					if (point.X < points[i].X)
					{
						point = points[i];
					}
					else
					{
						if (point.X != points[i].X)
						{
							break;
						}
						if (side2 == 2)
						{
							if (point.Y < points[i].Y)
							{
								point = points[i];
							}
						}
						else if (point.Y > points[i].Y)
						{
							point = points[i];
						}
					}
					break;
				case 2:
					if (point.Y < points[i].Y)
					{
						point = points[i];
					}
					else
					{
						if (point.Y != points[i].Y)
						{
							break;
						}
						if (side2 == 1)
						{
							if (point.X < points[i].X)
							{
								point = points[i];
							}
						}
						else if (point.X > points[i].X)
						{
							point = points[i];
						}
					}
					break;
				case 4:
					if (point.X > points[i].X)
					{
						point = points[i];
					}
					else
					{
						if (point.X != points[i].X)
						{
							break;
						}
						if (side2 == 2)
						{
							if (point.Y < points[i].Y)
							{
								point = points[i];
							}
						}
						else if (point.Y > points[i].Y)
						{
							point = points[i];
						}
					}
					break;
				case 8:
					if (point.Y > points[i].Y)
					{
						point = points[i];
					}
					else
					{
						if (point.Y != points[i].Y)
						{
							break;
						}
						if (side2 == 1)
						{
							if (point.X < points[i].X)
							{
								point = points[i];
							}
						}
						else if (point.X > points[i].X)
						{
							point = points[i];
						}
					}
					break;
				}
			}
			return point;
		}

		internal static int[] getWidth(bool[][] image, MessagingToolkit.QRCode.Geom.Point[] centers, int[] sincos)
		{
			int[] array = new int[3];
			for (int i = 0; i < 3; i++)
			{
				bool flag = false;
				int y = centers[i].Y;
				int num;
				for (num = centers[i].X; num > 0; num--)
				{
					if (image[num][y] && !image[num - 1][y])
					{
						if (flag)
						{
							break;
						}
						flag = true;
					}
				}
				flag = false;
				int j;
				for (j = centers[i].X; j < image.Length; j++)
				{
					if (image[j][y] && !image[j + 1][y])
					{
						if (flag)
						{
							break;
						}
						flag = true;
					}
				}
				array[i] = j - num + 1;
			}
			return array;
		}

		internal static int calcRoughVersion(MessagingToolkit.QRCode.Geom.Point[] center, int[] width)
		{
			int dECIMAL_POINT = QRCodeImageReader.DECIMAL_POINT;
			int num = new Line(center[0], center[1]).Length << dECIMAL_POINT;
			int num2 = (width[0] + width[1] << dECIMAL_POINT) / 14;
			int num3 = (num / num2 - 10) / 4;
			if ((num / num2 - 10) % 4 >= 2)
			{
				num3++;
			}
			return num3;
		}

		internal static int calcExactVersion(MessagingToolkit.QRCode.Geom.Point[] centers, int[] angle, int[] moduleSize, bool[][] image)
		{
			bool[] array = new bool[18];
			MessagingToolkit.QRCode.Geom.Point[] array2 = new MessagingToolkit.QRCode.Geom.Point[18];
			Axis axis = new Axis(angle, moduleSize[1]);
			axis.Origin = centers[1];
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					MessagingToolkit.QRCode.Geom.Point point = axis.translate(j - 7, i - 3);
					array[j + i * 3] = image[point.X][point.Y];
					array2[j + i * 3] = point;
				}
			}
			canvas.drawPoints(array2, Color_Fields.RED);
			int num = 0;
			try
			{
				return checkVersionInfo(array);
			}
			catch (InvalidVersionInfoException)
			{
				canvas.println("Version info error. now retry with other place one.");
				axis.Origin = centers[2];
				axis.ModulePitch = moduleSize[2];
				for (int j = 0; j < 6; j++)
				{
					for (int i = 0; i < 3; i++)
					{
						MessagingToolkit.QRCode.Geom.Point point = axis.translate(j - 3, i - 7);
						array[i + j * 3] = image[point.X][point.Y];
						array2[j + i * 3] = point;
					}
				}
				canvas.drawPoints(array2, Color_Fields.RED);
				try
				{
					return checkVersionInfo(array);
				}
				catch (VersionInformationException ex)
				{
					throw ex;
				}
			}
		}

		internal static int checkVersionInfo(bool[] target)
		{
			int num = 0;
			int i;
			for (i = 0; i < VersionInfoBit.Length; i++)
			{
				num = 0;
				for (int j = 0; j < 18; j++)
				{
					if (target[j] ^ ((VersionInfoBit[i] >> j) % 2 == 1))
					{
						num++;
					}
				}
				if (num <= 3)
				{
					break;
				}
			}
			if (num <= 3)
			{
				return 7 + i;
			}
			throw new InvalidVersionInfoException("Too many errors in version information");
		}

		static FinderPattern()
		{
			VersionInfoBit = new int[34]
			{
				31892, 34236, 39577, 42195, 48118, 51042, 55367, 58893, 63784, 68472,
				70749, 76311, 79154, 84390, 87683, 92361, 96236, 102084, 102881, 110507,
				110734, 117786, 119615, 126325, 127568, 133589, 136944, 141498, 145311, 150283,
				152622, 158308, 161089, 167017
			};
			canvas = QRCodeDecoder.Canvas;
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class DecodingFailedException : ArgumentException
	{
		internal string message = null;

		public override string Message => message;

		public DecodingFailedException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Data
{
	public interface QRCodeImage
	{
		int Width { get; }

		int Height { get; }

		int getPixel(int x, int y);
	}
}
namespace MessagingToolkit.QRCode.Codec.Reader
{
	public class QRCodeImageReader
	{
		private class ModulePitch
		{
			public int top;

			public int left;

			public int bottom;

			public int right;

			private QRCodeImageReader enclosingInstance;

			public QRCodeImageReader Enclosing_Instance => enclosingInstance;

			public ModulePitch(QRCodeImageReader enclosingInstance)
			{
				InitBlock(enclosingInstance);
			}

			private void InitBlock(QRCodeImageReader enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
		}

		public const bool POINT_DARK = true;

		public const bool POINT_LIGHT = false;

		internal DebugCanvas canvas;

		public static int DECIMAL_POINT = 21;

		internal SamplingGrid samplingGrid;

		internal bool[][] bitmap;

		public QRCodeImageReader()
		{
			canvas = QRCodeDecoder.Canvas;
		}

		internal virtual bool[][] applyMedianFilter(bool[][] image, int threshold)
		{
			bool[][] array = new bool[image.Length][];
			for (int i = 0; i < image.Length; i++)
			{
				array[i] = new bool[image[0].Length];
			}
			for (int j = 1; j < image[0].Length - 1; j++)
			{
				for (int k = 1; k < image.Length - 1; k++)
				{
					int num = 0;
					for (int l = -1; l < 2; l++)
					{
						for (int m = -1; m < 2; m++)
						{
							if (image[k + m][j + l])
							{
								num++;
							}
						}
					}
					if (num > threshold)
					{
						array[k][j] = true;
					}
				}
			}
			return array;
		}

		internal virtual bool[][] applyCrossMaskingMedianFilter(bool[][] image, int threshold)
		{
			bool[][] array = new bool[image.Length][];
			for (int i = 0; i < image.Length; i++)
			{
				array[i] = new bool[image[0].Length];
			}
			for (int j = 2; j < image[0].Length - 2; j++)
			{
				for (int k = 2; k < image.Length - 2; k++)
				{
					int num = 0;
					for (int l = -2; l < 3; l++)
					{
						if (image[k + l][j])
						{
							num++;
						}
						if (image[k][j + l])
						{
							num++;
						}
					}
					if (num > threshold)
					{
						array[k][j] = true;
					}
				}
			}
			return array;
		}

		internal virtual bool[][] filterImage(int[][] image)
		{
			imageToGrayScale(image);
			return grayScaleToBitmap(image);
		}

		internal virtual void imageToGrayScale(int[][] image)
		{
			for (int i = 0; i < image[0].Length; i++)
			{
				for (int j = 0; j < image.Length; j++)
				{
					int num = (image[j][i] >> 16) & 0xFF;
					int num2 = (image[j][i] >> 8) & 0xFF;
					int num3 = image[j][i] & 0xFF;
					int num4 = (num * 30 + num2 * 59 + num3 * 11) / 100;
					image[j][i] = num4;
				}
			}
		}

		internal virtual bool[][] grayScaleToBitmap(int[][] grayScale)
		{
			int[][] middleBrightnessPerArea = getMiddleBrightnessPerArea(grayScale);
			int num = middleBrightnessPerArea.Length;
			int num2 = grayScale.Length / num;
			int num3 = grayScale[0].Length / num;
			bool[][] array = new bool[grayScale.Length][];
			for (int i = 0; i < grayScale.Length; i++)
			{
				array[i] = new bool[grayScale[0].Length];
			}
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					for (int l = 0; l < num3; l++)
					{
						for (int m = 0; m < num2; m++)
						{
							array[num2 * k + m][num3 * j + l] = ((grayScale[num2 * k + m][num3 * j + l] < middleBrightnessPerArea[k][j]) ? true : false);
						}
					}
				}
			}
			return array;
		}

		internal virtual int[][] getMiddleBrightnessPerArea(int[][] image)
		{
			int num = 4;
			int num2 = image.Length / num;
			int num3 = image[0].Length / num;
			int[][][] array = new int[num][][];
			for (int i = 0; i < num; i++)
			{
				array[i] = new int[num][];
				for (int j = 0; j < num; j++)
				{
					array[i][j] = new int[2];
				}
			}
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num; l++)
				{
					array[l][k][0] = 255;
					for (int m = 0; m < num3; m++)
					{
						for (int n = 0; n < num2; n++)
						{
							int num4 = image[num2 * l + n][num3 * k + m];
							if (num4 < array[l][k][0])
							{
								array[l][k][0] = num4;
							}
							if (num4 > array[l][k][1])
							{
								array[l][k][1] = num4;
							}
						}
					}
				}
			}
			int[][] array2 = new int[num][];
			for (int num5 = 0; num5 < num; num5++)
			{
				array2[num5] = new int[num];
			}
			for (int k = 0; k < num; k++)
			{
				for (int l = 0; l < num; l++)
				{
					array2[l][k] = (array[l][k][0] + array[l][k][1]) / 2;
				}
			}
			return array2;
		}

		public virtual QRCodeSymbol getQRCodeSymbol(int[][] image)
		{
			int num = ((image.Length < image[0].Length) ? image[0].Length : image.Length);
			DECIMAL_POINT = 23 - QRCodeUtility.sqrt(num / 256);
			bitmap = filterImage(image);
			canvas.println("Drawing matrix.");
			canvas.drawMatrix(bitmap);
			canvas.println("Scanning Finder Pattern.");
			FinderPattern finderPattern = null;
			try
			{
				finderPattern = FinderPattern.findFinderPattern(bitmap);
			}
			catch (FinderPatternNotFoundException)
			{
				canvas.println("Not found, now retrying...");
				bitmap = applyCrossMaskingMedianFilter(bitmap, 5);
				canvas.drawMatrix(bitmap);
				for (int i = 0; i < 1000000000; i++)
				{
				}
				try
				{
					finderPattern = FinderPattern.findFinderPattern(bitmap);
				}
				catch (FinderPatternNotFoundException ex)
				{
					throw new SymbolNotFoundException(ex.Message);
				}
				catch (VersionInformationException ex2)
				{
					throw new SymbolNotFoundException(ex2.Message);
				}
			}
			catch (VersionInformationException ex4)
			{
				throw new SymbolNotFoundException(ex4.Message);
			}
			canvas.println("FinderPattern at");
			string str = finderPattern.getCenter(0).ToString() + finderPattern.getCenter(1).ToString() + finderPattern.getCenter(2).ToString();
			canvas.println(str);
			int[] angle = finderPattern.getAngle();
			canvas.println("Angle*4098: Sin " + Convert.ToString(angle[0]) + "  Cos " + Convert.ToString(angle[1]));
			int version = finderPattern.Version;
			canvas.println("Version: " + Convert.ToString(version));
			if (version < 1 || version > 40)
			{
				throw new InvalidVersionException("Invalid version: " + version);
			}
			AlignmentPattern alignmentPattern = null;
			try
			{
				alignmentPattern = AlignmentPattern.findAlignmentPattern(bitmap, finderPattern);
			}
			catch (AlignmentPatternNotFoundException ex5)
			{
				throw new SymbolNotFoundException(ex5.Message);
			}
			int num2 = alignmentPattern.getCenter().Length;
			canvas.println("AlignmentPatterns at");
			for (int j = 0; j < num2; j++)
			{
				string text = "";
				for (int k = 0; k < num2; k++)
				{
					text += alignmentPattern.getCenter()[k][j].ToString();
				}
				canvas.println(text);
			}
			canvas.println("Creating sampling grid.");
			samplingGrid = getSamplingGrid(finderPattern, alignmentPattern);
			canvas.println("Reading grid.");
			bool[][] array = null;
			try
			{
				array = getQRCodeMatrix(bitmap, samplingGrid);
			}
			catch (IndexOutOfRangeException)
			{
				throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
			}
			return new QRCodeSymbol(array);
		}

		public virtual QRCodeSymbol getQRCodeSymbolWithAdjustedGrid(MessagingToolkit.QRCode.Geom.Point adjust)
		{
			if (bitmap == null || samplingGrid == null)
			{
				throw new SystemException("This method must be called after QRCodeImageReader.getQRCodeSymbol() called");
			}
			samplingGrid.adjust(adjust);
			canvas.println("Sampling grid adjusted d(" + adjust.X + "," + adjust.Y + ")");
			bool[][] array = null;
			try
			{
				array = getQRCodeMatrix(bitmap, samplingGrid);
			}
			catch (IndexOutOfRangeException)
			{
				throw new SymbolNotFoundException("Sampling grid exceeded image boundary");
			}
			return new QRCodeSymbol(array);
		}

		internal virtual SamplingGrid getSamplingGrid(FinderPattern finderPattern, AlignmentPattern alignmentPattern)
		{
			MessagingToolkit.QRCode.Geom.Point[][] center = alignmentPattern.getCenter();
			int version = finderPattern.Version;
			int num = version / 7 + 2;
			center[0][0] = finderPattern.getCenter(0);
			center[num - 1][0] = finderPattern.getCenter(1);
			center[0][num - 1] = finderPattern.getCenter(2);
			int num2 = num - 1;
			SamplingGrid samplingGrid = new SamplingGrid(num2);
			Axis axis = new Axis(finderPattern.getAngle(), finderPattern.getModuleSize());
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					ModulePitch modulePitch = new ModulePitch(this);
					Line line = new Line();
					Line line2 = new Line();
					axis.ModulePitch = finderPattern.getModuleSize();
					MessagingToolkit.QRCode.Geom.Point[][] logicalCenter = AlignmentPattern.getLogicalCenter(finderPattern);
					MessagingToolkit.QRCode.Geom.Point point = center[j][i];
					MessagingToolkit.QRCode.Geom.Point point2 = center[j + 1][i];
					MessagingToolkit.QRCode.Geom.Point point3 = center[j][i + 1];
					MessagingToolkit.QRCode.Geom.Point point4 = center[j + 1][i + 1];
					MessagingToolkit.QRCode.Geom.Point point5 = logicalCenter[j][i];
					MessagingToolkit.QRCode.Geom.Point point6 = logicalCenter[j + 1][i];
					MessagingToolkit.QRCode.Geom.Point point7 = logicalCenter[j][i + 1];
					MessagingToolkit.QRCode.Geom.Point point8 = logicalCenter[j + 1][i + 1];
					if (j == 0 && i == 0)
					{
						if (num2 == 1)
						{
							point = axis.translate(point, -3, -3);
							point2 = axis.translate(point2, 3, -3);
							point3 = axis.translate(point3, -3, 3);
							point4 = axis.translate(point4, 6, 6);
							point5.translate(-6, -6);
							point6.translate(3, -3);
							point7.translate(-3, 3);
							point8.translate(6, 6);
						}
						else
						{
							point = axis.translate(point, -3, -3);
							point2 = axis.translate(point2, 0, -6);
							point3 = axis.translate(point3, -6, 0);
							point5.translate(-6, -6);
							point6.translate(0, -6);
							point7.translate(-6, 0);
						}
					}
					else if (j == 0 && i == num2 - 1)
					{
						point = axis.translate(point, -6, 0);
						point3 = axis.translate(point3, -3, 3);
						point4 = axis.translate(point4, 0, 6);
						point5.translate(-6, 0);
						point7.translate(-6, 6);
						point8.translate(0, 6);
					}
					else if (j == num2 - 1 && i == 0)
					{
						point = axis.translate(point, 0, -6);
						point2 = axis.translate(point2, 3, -3);
						point4 = axis.translate(point4, 6, 0);
						point5.translate(0, -6);
						point6.translate(6, -6);
						point8.translate(6, 0);
					}
					else if (j == num2 - 1 && i == num2 - 1)
					{
						point3 = axis.translate(point3, 0, 6);
						point2 = axis.translate(point2, 6, 0);
						point4 = axis.translate(point4, 6, 6);
						point7.translate(0, 6);
						point6.translate(6, 0);
						point8.translate(6, 6);
					}
					else if (j == 0)
					{
						point = axis.translate(point, -6, 0);
						point3 = axis.translate(point3, -6, 0);
						point5.translate(-6, 0);
						point7.translate(-6, 0);
					}
					else if (j == num2 - 1)
					{
						point2 = axis.translate(point2, 6, 0);
						point4 = axis.translate(point4, 6, 0);
						point6.translate(6, 0);
						point8.translate(6, 0);
					}
					else if (i == 0)
					{
						point = axis.translate(point, 0, -6);
						point2 = axis.translate(point2, 0, -6);
						point5.translate(0, -6);
						point6.translate(0, -6);
					}
					else if (i == num2 - 1)
					{
						point3 = axis.translate(point3, 0, 6);
						point4 = axis.translate(point4, 0, 6);
						point7.translate(0, 6);
						point8.translate(0, 6);
					}
					if (j == 0)
					{
						point6.translate(1, 0);
						point8.translate(1, 0);
					}
					else
					{
						point5.translate(-1, 0);
						point7.translate(-1, 0);
					}
					if (i == 0)
					{
						point7.translate(0, 1);
						point8.translate(0, 1);
					}
					else
					{
						point5.translate(0, -1);
						point6.translate(0, -1);
					}
					int num3 = point6.X - point5.X;
					int num4 = point7.Y - point5.Y;
					if (version < 7)
					{
						num3 += 3;
						num4 += 3;
					}
					modulePitch.top = getAreaModulePitch(point, point2, num3 - 1);
					modulePitch.left = getAreaModulePitch(point, point3, num4 - 1);
					modulePitch.bottom = getAreaModulePitch(point3, point4, num3 - 1);
					modulePitch.right = getAreaModulePitch(point2, point4, num4 - 1);
					line.setP1(point);
					line2.setP1(point);
					line.setP2(point3);
					line2.setP2(point2);
					samplingGrid.initGrid(j, i, num3, num4);
					for (int k = 0; k < num3; k++)
					{
						Line line3 = new Line(line.getP1(), line.getP2());
						axis.Origin = line3.getP1();
						axis.ModulePitch = modulePitch.top;
						line3.setP1(axis.translate(k, 0));
						axis.Origin = line3.getP2();
						axis.ModulePitch = modulePitch.bottom;
						line3.setP2(axis.translate(k, 0));
						samplingGrid.setXLine(j, i, k, line3);
					}
					for (int k = 0; k < num4; k++)
					{
						Line line4 = new Line(line2.getP1(), line2.getP2());
						axis.Origin = line4.getP1();
						axis.ModulePitch = modulePitch.left;
						line4.setP1(axis.translate(0, k));
						axis.Origin = line4.getP2();
						axis.ModulePitch = modulePitch.right;
						line4.setP2(axis.translate(0, k));
						samplingGrid.setYLine(j, i, k, line4);
					}
				}
			}
			return samplingGrid;
		}

		internal virtual int getAreaModulePitch(MessagingToolkit.QRCode.Geom.Point start, MessagingToolkit.QRCode.Geom.Point end, int logicalDistance)
		{
			Line line = new Line(start, end);
			int length = line.Length;
			return (length << DECIMAL_POINT) / logicalDistance;
		}

		internal virtual bool[][] getQRCodeMatrix(bool[][] image, SamplingGrid gridLines)
		{
			int totalWidth = gridLines.TotalWidth;
			canvas.println("gridSize=" + totalWidth);
			MessagingToolkit.QRCode.Geom.Point point = null;
			bool[][] array = new bool[totalWidth][];
			for (int i = 0; i < totalWidth; i++)
			{
				array[i] = new bool[totalWidth];
			}
			for (int j = 0; j < gridLines.getHeight(); j++)
			{
				for (int k = 0; k < gridLines.getWidth(); k++)
				{
					ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
					for (int l = 0; l < gridLines.getHeight(k, j); l++)
					{
						for (int m = 0; m < gridLines.getWidth(k, j); m++)
						{
							int x = gridLines.getXLine(k, j, m).getP1().X;
							int y = gridLines.getXLine(k, j, m).getP1().Y;
							int x2 = gridLines.getXLine(k, j, m).getP2().X;
							int y2 = gridLines.getXLine(k, j, m).getP2().Y;
							int x3 = gridLines.getYLine(k, j, l).getP1().X;
							int y3 = gridLines.getYLine(k, j, l).getP1().Y;
							int x4 = gridLines.getYLine(k, j, l).getP2().X;
							int y4 = gridLines.getYLine(k, j, l).getP2().Y;
							int num = (y2 - y) * (x3 - x4) - (y4 - y3) * (x - x2);
							int num2 = (x * y2 - x2 * y) * (x3 - x4) - (x3 * y4 - x4 * y3) * (x - x2);
							int num3 = (x3 * y4 - x4 * y3) * (y2 - y) - (x * y2 - x2 * y) * (y4 - y3);
							array[gridLines.getX(k, m)][gridLines.getY(j, l)] = image[num2 / num][num3 / num];
							if (j == gridLines.getHeight() - 1 && k == gridLines.getWidth() - 1 && l == gridLines.getHeight(k, j) - 1 && m == gridLines.getWidth(k, j) - 1)
							{
								point = new MessagingToolkit.QRCode.Geom.Point(num2 / num, num3 / num);
							}
						}
					}
				}
			}
			if (point.X > image.Length - 1 || point.Y > image[0].Length - 1)
			{
				throw new IndexOutOfRangeException("Sampling grid pointed out of image");
			}
			canvas.drawPoint(point, Color_Fields.BLUE);
			return array;
		}
	}
}
namespace MessagingToolkit.QRCode.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
					ResourceManager resourceManager = new ResourceManager("MessagingToolkit.QRCode.Properties.Resources", typeof(Resources).Assembly);
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

		internal Resources()
		{
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class SymbolNotFoundException : ArgumentException
	{
		internal string message = null;

		public override string Message => message;

		public SymbolNotFoundException(string message)
		{
			this.message = message;
		}
	}
	[Serializable]
	public class AlignmentPatternNotFoundException : ArgumentException
	{
		internal string message = null;

		public override string Message => message;

		public AlignmentPatternNotFoundException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Reader.Pattern
{
	public class AlignmentPattern
	{
		internal const int RIGHT = 1;

		internal const int BOTTOM = 2;

		internal const int LEFT = 3;

		internal const int TOP = 4;

		internal static DebugCanvas canvas;

		internal MessagingToolkit.QRCode.Geom.Point[][] center;

		internal int patternDistance;

		public virtual int LogicalDistance => patternDistance;

		internal AlignmentPattern(MessagingToolkit.QRCode.Geom.Point[][] center, int patternDistance)
		{
			this.center = center;
			this.patternDistance = patternDistance;
		}

		public static AlignmentPattern findAlignmentPattern(bool[][] image, FinderPattern finderPattern)
		{
			MessagingToolkit.QRCode.Geom.Point[][] logicalCenter = getLogicalCenter(finderPattern);
			int num = logicalCenter[1][0].X - logicalCenter[0][0].X;
			MessagingToolkit.QRCode.Geom.Point[][] array = null;
			array = getCenter(image, finderPattern, logicalCenter);
			return new AlignmentPattern(array, num);
		}

		public virtual MessagingToolkit.QRCode.Geom.Point[][] getCenter()
		{
			return center;
		}

		public virtual void setCenter(MessagingToolkit.QRCode.Geom.Point[][] center)
		{
			this.center = center;
		}

		internal static MessagingToolkit.QRCode.Geom.Point[][] getCenter(bool[][] image, FinderPattern finderPattern, MessagingToolkit.QRCode.Geom.Point[][] logicalCenters)
		{
			int moduleSize = finderPattern.getModuleSize();
			Axis axis = new Axis(finderPattern.getAngle(), moduleSize);
			int num = logicalCenters.Length;
			MessagingToolkit.QRCode.Geom.Point[][] array = new MessagingToolkit.QRCode.Geom.Point[num][];
			for (int i = 0; i < num; i++)
			{
				array[i] = new MessagingToolkit.QRCode.Geom.Point[num];
			}
			axis.Origin = finderPattern.getCenter(0);
			array[0][0] = axis.translate(3, 3);
			canvas.drawCross(array[0][0], Color_Fields.BLUE);
			axis.Origin = finderPattern.getCenter(1);
			array[num - 1][0] = axis.translate(-3, 3);
			canvas.drawCross(array[num - 1][0], Color_Fields.BLUE);
			axis.Origin = finderPattern.getCenter(2);
			array[0][num - 1] = axis.translate(3, -3);
			canvas.drawCross(array[0][num - 1], Color_Fields.BLUE);
			MessagingToolkit.QRCode.Geom.Point p = array[0][0];
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					if ((k == 0 && j == 0) || (k == 0 && j == num - 1) || (k == num - 1 && j == 0))
					{
						continue;
					}
					MessagingToolkit.QRCode.Geom.Point point = null;
					if (j == 0)
					{
						if (k > 0 && k < num - 1)
						{
							point = axis.translate(array[k - 1][j], logicalCenters[k][j].X - logicalCenters[k - 1][j].X, 0);
						}
						array[k][j] = new MessagingToolkit.QRCode.Geom.Point(point.X, point.Y);
						canvas.drawCross(array[k][j], Color_Fields.RED);
					}
					else if (k == 0)
					{
						if (j > 0 && j < num - 1)
						{
							point = axis.translate(array[k][j - 1], 0, logicalCenters[k][j].Y - logicalCenters[k][j - 1].Y);
						}
						array[k][j] = new MessagingToolkit.QRCode.Geom.Point(point.X, point.Y);
						canvas.drawCross(array[k][j], Color_Fields.RED);
					}
					else
					{
						MessagingToolkit.QRCode.Geom.Point point2 = axis.translate(array[k - 1][j], logicalCenters[k][j].X - logicalCenters[k - 1][j].X, 0);
						MessagingToolkit.QRCode.Geom.Point point3 = axis.translate(array[k][j - 1], 0, logicalCenters[k][j].Y - logicalCenters[k][j - 1].Y);
						array[k][j] = new MessagingToolkit.QRCode.Geom.Point((point2.X + point3.X) / 2, (point2.Y + point3.Y) / 2 + 1);
					}
					if (finderPattern.Version > 1)
					{
						MessagingToolkit.QRCode.Geom.Point precisionCenter = getPrecisionCenter(image, array[k][j]);
						if (array[k][j].distanceOf(precisionCenter) < 6)
						{
							canvas.drawCross(array[k][j], Color_Fields.RED);
							int num2 = precisionCenter.X - array[k][j].X;
							int num3 = precisionCenter.Y - array[k][j].Y;
							canvas.println("Adjust AP(" + k + "," + j + ") to d(" + num2 + "," + num3 + ")");
							array[k][j] = precisionCenter;
						}
					}
					canvas.drawCross(array[k][j], Color_Fields.BLUE);
					canvas.drawLine(new Line(p, array[k][j]), Color_Fields.LIGHTBLUE);
					p = array[k][j];
				}
			}
			return array;
		}

		internal static MessagingToolkit.QRCode.Geom.Point getPrecisionCenter(bool[][] image, MessagingToolkit.QRCode.Geom.Point targetPoint)
		{
			int x = targetPoint.X;
			int y = targetPoint.Y;
			if (x < 0 || y < 0 || x > image.Length - 1 || y > image[0].Length - 1)
			{
				throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
			}
			if (!image[targetPoint.X][targetPoint.Y])
			{
				int num = 0;
				bool flag = false;
				while (!flag)
				{
					num++;
					for (int num2 = num; num2 > -num; num2--)
					{
						for (int num3 = num; num3 > -num; num3--)
						{
							int num4 = targetPoint.X + num3;
							int num5 = targetPoint.Y + num2;
							if (num4 < 0 || num5 < 0 || num4 > image.Length - 1 || num5 > image[0].Length - 1)
							{
								throw new AlignmentPatternNotFoundException("Alignment Pattern finder exceeded out of image");
							}
							if (image[num4][num5])
							{
								targetPoint = new MessagingToolkit.QRCode.Geom.Point(targetPoint.X + num3, targetPoint.Y + num2);
								flag = true;
							}
						}
					}
				}
			}
			int num6;
			int i;
			int num7 = (num6 = (i = targetPoint.X));
			int num8;
			int j;
			int num9 = (num8 = (j = targetPoint.Y));
			while (num6 >= 1 && !targetPointOnTheCorner(image, num6, num9, num6 - 1, num9))
			{
				num6--;
			}
			for (; i < image.Length - 1 && !targetPointOnTheCorner(image, i, num9, i + 1, num9); i++)
			{
			}
			while (num8 >= 1 && !targetPointOnTheCorner(image, num7, num8, num7, num8 - 1))
			{
				num8--;
			}
			for (; j < image[0].Length - 1 && !targetPointOnTheCorner(image, num7, j, num7, j + 1); j++)
			{
			}
			return new MessagingToolkit.QRCode.Geom.Point((num6 + i + 1) / 2, (num8 + j + 1) / 2);
		}

		internal static bool targetPointOnTheCorner(bool[][] image, int x, int y, int nx, int ny)
		{
			if (x < 0 || y < 0 || nx < 0 || ny < 0 || x > image.Length || y > image[0].Length || nx > image.Length || ny > image[0].Length)
			{
				throw new AlignmentPatternNotFoundException("Alignment Pattern Finder exceeded image edge");
			}
			return !image[x][y] && image[nx][ny];
		}

		public static MessagingToolkit.QRCode.Geom.Point[][] getLogicalCenter(FinderPattern finderPattern)
		{
			int version = finderPattern.Version;
			MessagingToolkit.QRCode.Geom.Point[][] array = new MessagingToolkit.QRCode.Geom.Point[1][];
			for (int i = 0; i < 1; i++)
			{
				array[i] = new MessagingToolkit.QRCode.Geom.Point[1];
			}
			int[] array2 = new int[1];
			array2 = LogicalSeed.getSeed(version);
			array = new MessagingToolkit.QRCode.Geom.Point[array2.Length][];
			for (int j = 0; j < array2.Length; j++)
			{
				array[j] = new MessagingToolkit.QRCode.Geom.Point[array2.Length];
			}
			for (int k = 0; k < array.Length; k++)
			{
				for (int l = 0; l < array.Length; l++)
				{
					array[l][k] = new MessagingToolkit.QRCode.Geom.Point(array2[l], array2[k]);
				}
			}
			return array;
		}

		static AlignmentPattern()
		{
			canvas = QRCodeDecoder.Canvas;
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class InvalidVersionInfoException : VersionInformationException
	{
		internal string message = null;

		public override string Message => message;

		public InvalidVersionInfoException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Ecc
{
	public class ReedSolomon
	{
		internal int[] y;

		internal int[] gexp = new int[512];

		internal int[] glog = new int[256];

		internal int NPAR;

		internal int MAXDEG;

		internal int[] synBytes;

		internal int[] Lambda;

		internal int[] Omega;

		internal int[] ErrorLocs = new int[256];

		internal int NErrors;

		internal int[] ErasureLocs = new int[256];

		internal int NErasures = 0;

		internal bool correctionSucceeded = true;

		public virtual bool CorrectionSucceeded => correctionSucceeded;

		public virtual int NumCorrectedErrors => NErrors;

		public ReedSolomon(int[] source, int NPAR)
		{
			initializeGaloisTables();
			y = source;
			this.NPAR = NPAR;
			MAXDEG = NPAR * 2;
			synBytes = new int[MAXDEG];
			Lambda = new int[MAXDEG];
			Omega = new int[MAXDEG];
		}

		internal virtual void initializeGaloisTables()
		{
			int num6;
			int num5;
			int num4;
			int num3;
			int num2;
			int num;
			int num7 = (num6 = (num5 = (num4 = (num3 = (num2 = (num = 0))))));
			int num8 = 1;
			gexp[0] = 1;
			gexp[255] = gexp[0];
			glog[0] = 0;
			for (int i = 1; i < 256; i++)
			{
				int num9 = num;
				num = num2;
				num2 = num3;
				num3 = num4;
				num4 = num5 ^ num9;
				num5 = num6 ^ num9;
				num6 = num7 ^ num9;
				num7 = num8;
				num8 = num9;
				gexp[i] = num8 + num7 * 2 + num6 * 4 + num5 * 8 + num4 * 16 + num3 * 32 + num2 * 64 + num * 128;
				gexp[i + 255] = gexp[i];
			}
			for (int i = 1; i < 256; i++)
			{
				for (int j = 0; j < 256; j++)
				{
					if (gexp[j] == i)
					{
						glog[i] = j;
						break;
					}
				}
			}
		}

		internal virtual int gmult(int a, int b)
		{
			if (a == 0 || b == 0)
			{
				return 0;
			}
			int num = glog[a];
			int num2 = glog[b];
			return gexp[num + num2];
		}

		internal virtual int ginv(int elt)
		{
			return gexp[255 - glog[elt]];
		}

		internal virtual void decode_data(int[] data)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				int num = 0;
				for (int j = 0; j < data.Length; j++)
				{
					num = data[j] ^ gmult(gexp[i + 1], num);
				}
				synBytes[i] = num;
			}
		}

		public virtual void correct()
		{
			decode_data(y);
			correctionSucceeded = true;
			bool flag = false;
			for (int i = 0; i < synBytes.Length; i++)
			{
				if (synBytes[i] != 0)
				{
					flag = true;
				}
			}
			if (flag)
			{
				correctionSucceeded = correct_errors_erasures(y, y.Length, 0, new int[1]);
			}
		}

		internal virtual void Modified_Berlekamp_Massey()
		{
			int[] array = new int[MAXDEG];
			int[] array2 = new int[MAXDEG];
			int[] array3 = new int[MAXDEG];
			int[] array4 = new int[MAXDEG];
			init_gamma(array4);
			copy_poly(array3, array4);
			mul_z_poly(array3);
			copy_poly(array, array4);
			int num = -1;
			int num2 = NErasures;
			for (int i = NErasures; i < 8; i++)
			{
				int num3 = compute_discrepancy(array, synBytes, num2, i);
				if (num3 != 0)
				{
					for (int j = 0; j < MAXDEG; j++)
					{
						array2[j] = array[j] ^ gmult(num3, array3[j]);
					}
					if (num2 < i - num)
					{
						int num4 = i - num;
						num = i - num2;
						for (int j = 0; j < MAXDEG; j++)
						{
							array3[j] = gmult(array[j], ginv(num3));
						}
						num2 = num4;
					}
					for (int j = 0; j < MAXDEG; j++)
					{
						array[j] = array2[j];
					}
				}
				mul_z_poly(array3);
			}
			for (int j = 0; j < MAXDEG; j++)
			{
				Lambda[j] = array[j];
			}
			compute_modified_omega();
		}

		internal virtual void compute_modified_omega()
		{
			int[] array = new int[MAXDEG * 2];
			mult_polys(array, Lambda, synBytes);
			zero_poly(Omega);
			for (int i = 0; i < NPAR; i++)
			{
				Omega[i] = array[i];
			}
		}

		internal virtual void mult_polys(int[] dst, int[] p1, int[] p2)
		{
			int[] array = new int[MAXDEG * 2];
			for (int i = 0; i < MAXDEG * 2; i++)
			{
				dst[i] = 0;
			}
			for (int i = 0; i < MAXDEG; i++)
			{
				for (int j = MAXDEG; j < MAXDEG * 2; j++)
				{
					array[j] = 0;
				}
				for (int j = 0; j < MAXDEG; j++)
				{
					array[j] = gmult(p2[j], p1[i]);
				}
				for (int j = MAXDEG * 2 - 1; j >= i; j--)
				{
					array[j] = array[j - i];
				}
				for (int j = 0; j < i; j++)
				{
					array[j] = 0;
				}
				for (int j = 0; j < MAXDEG * 2; j++)
				{
					dst[j] ^= array[j];
				}
			}
		}

		internal virtual void init_gamma(int[] gamma)
		{
			int[] array = new int[MAXDEG];
			zero_poly(gamma);
			zero_poly(array);
			gamma[0] = 1;
			for (int i = 0; i < NErasures; i++)
			{
				copy_poly(array, gamma);
				scale_poly(gexp[ErasureLocs[i]], array);
				mul_z_poly(array);
				add_polys(gamma, array);
			}
		}

		internal virtual void compute_next_omega(int d, int[] A, int[] dst, int[] src)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				dst[i] = src[i] ^ gmult(d, A[i]);
			}
		}

		internal virtual int compute_discrepancy(int[] lambda, int[] S, int L, int n)
		{
			int num = 0;
			for (int i = 0; i <= L; i++)
			{
				num ^= gmult(lambda[i], S[n - i]);
			}
			return num;
		}

		internal virtual void add_polys(int[] dst, int[] src)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				dst[i] ^= src[i];
			}
		}

		internal virtual void copy_poly(int[] dst, int[] src)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				dst[i] = src[i];
			}
		}

		internal virtual void scale_poly(int k, int[] poly)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				poly[i] = gmult(k, poly[i]);
			}
		}

		internal virtual void zero_poly(int[] poly)
		{
			for (int i = 0; i < MAXDEG; i++)
			{
				poly[i] = 0;
			}
		}

		internal virtual void mul_z_poly(int[] src)
		{
			for (int num = MAXDEG - 1; num > 0; num--)
			{
				src[num] = src[num - 1];
			}
			src[0] = 0;
		}

		internal virtual void Find_Roots()
		{
			NErrors = 0;
			for (int i = 1; i < 256; i++)
			{
				int num = 0;
				for (int j = 0; j < NPAR + 1; j++)
				{
					num ^= gmult(gexp[j * i % 255], Lambda[j]);
				}
				if (num == 0)
				{
					ErrorLocs[NErrors] = 255 - i;
					NErrors++;
				}
			}
		}

		internal virtual bool correct_errors_erasures(int[] codeword, int csize, int nerasures, int[] erasures)
		{
			NErasures = nerasures;
			for (int i = 0; i < NErasures; i++)
			{
				ErasureLocs[i] = erasures[i];
			}
			Modified_Berlekamp_Massey();
			Find_Roots();
			if (NErrors <= NPAR || NErrors > 0)
			{
				for (int j = 0; j < NErrors; j++)
				{
					if (ErrorLocs[j] >= csize)
					{
						return false;
					}
				}
				for (int j = 0; j < NErrors; j++)
				{
					int i = ErrorLocs[j];
					int num = 0;
					for (int k = 0; k < MAXDEG; k++)
					{
						num ^= gmult(Omega[k], gexp[(255 - i) * k % 255]);
					}
					int num2 = 0;
					for (int k = 1; k < MAXDEG; k += 2)
					{
						num2 ^= gmult(Lambda[k], gexp[(255 - i) * (k - 1) % 255]);
					}
					int num3 = gmult(num, ginv(num2));
					codeword[csize - i - 1] ^= num3;
				}
				return true;
			}
			return false;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Data
{
	public class QRCodeBitmapImage : QRCodeImage
	{
		private Bitmap image;

		public virtual int Width => ((Image)image).Width;

		public virtual int Height => ((Image)image).Height;

		public QRCodeBitmapImage(Bitmap image)
		{
			this.image = image;
		}

		public virtual int getPixel(int x, int y)
		{
			return image.GetPixel(x, y).ToArgb();
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Util
{
	public class QRCodeUtility
	{
		public static int sqrt(int val)
		{
			int num = 0;
			int num2 = 32768;
			int num3 = 15;
			do
			{
				int num4;
				if (val >= (num4 = (num << 1) + num2 << num3--))
				{
					num += num2;
					val -= num4;
				}
			}
			while ((num2 >>= 1) > 0);
			return num;
		}

		public static bool IsUniCode(string value)
		{
			byte[] characters = AsciiStringToByteArray(value);
			byte[] characters2 = UnicodeStringToByteArray(value);
			string text = FromASCIIByteArray(characters);
			string text2 = FromUnicodeByteArray(characters2);
			if (text != text2)
			{
				return true;
			}
			return false;
		}

		public static bool IsUnicode(byte[] byteData)
		{
			string str = FromASCIIByteArray(byteData);
			string str2 = FromUnicodeByteArray(byteData);
			byte[] array = AsciiStringToByteArray(str);
			byte[] array2 = UnicodeStringToByteArray(str2);
			if (array[0] != array2[0])
			{
				return true;
			}
			return false;
		}

		public static string FromASCIIByteArray(byte[] characters)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			return aSCIIEncoding.GetString(characters);
		}

		public static string FromUnicodeByteArray(byte[] characters)
		{
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			return unicodeEncoding.GetString(characters);
		}

		public static byte[] AsciiStringToByteArray(string str)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			return aSCIIEncoding.GetBytes(str);
		}

		public static byte[] UnicodeStringToByteArray(string str)
		{
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			return unicodeEncoding.GetBytes(str);
		}
	}
}
namespace MessagingToolkit.QRCode.Geom
{
	public class SamplingGrid
	{
		private class AreaGrid
		{
			private SamplingGrid enclosingInstance;

			private Line[] xLine;

			private Line[] yLine;

			public virtual int Width => xLine.Length;

			public virtual int Height => yLine.Length;

			public virtual Line[] XLines => xLine;

			public virtual Line[] YLines => yLine;

			public SamplingGrid Enclosing_Instance => enclosingInstance;

			private void InitBlock(SamplingGrid enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}

			public AreaGrid(SamplingGrid enclosingInstance, int width, int height)
			{
				InitBlock(enclosingInstance);
				xLine = new Line[width];
				yLine = new Line[height];
			}

			public virtual Line getXLine(int x)
			{
				return xLine[x];
			}

			public virtual Line getYLine(int y)
			{
				return yLine[y];
			}

			public virtual void setXLine(int x, Line line)
			{
				xLine[x] = line;
			}

			public virtual void setYLine(int y, Line line)
			{
				yLine[y] = line;
			}
		}

		private AreaGrid[][] grid;

		public virtual int TotalWidth
		{
			get
			{
				int num = 0;
				for (int i = 0; i < grid.Length; i++)
				{
					num += grid[i][0].Width;
					if (i > 0)
					{
						num--;
					}
				}
				return num;
			}
		}

		public virtual int TotalHeight
		{
			get
			{
				int num = 0;
				for (int i = 0; i < grid[0].Length; i++)
				{
					num += grid[0][i].Height;
					if (i > 0)
					{
						num--;
					}
				}
				return num;
			}
		}

		public SamplingGrid(int sqrtNumArea)
		{
			grid = new AreaGrid[sqrtNumArea][];
			for (int i = 0; i < sqrtNumArea; i++)
			{
				grid[i] = new AreaGrid[sqrtNumArea];
			}
		}

		public virtual void initGrid(int ax, int ay, int width, int height)
		{
			grid[ax][ay] = new AreaGrid(this, width, height);
		}

		public virtual void setXLine(int ax, int ay, int x, Line line)
		{
			grid[ax][ay].setXLine(x, line);
		}

		public virtual void setYLine(int ax, int ay, int y, Line line)
		{
			grid[ax][ay].setYLine(y, line);
		}

		public virtual Line getXLine(int ax, int ay, int x)
		{
			return grid[ax][ay].getXLine(x);
		}

		public virtual Line getYLine(int ax, int ay, int y)
		{
			return grid[ax][ay].getYLine(y);
		}

		public virtual Line[] getXLines(int ax, int ay)
		{
			return grid[ax][ay].XLines;
		}

		public virtual Line[] getYLines(int ax, int ay)
		{
			return grid[ax][ay].YLines;
		}

		public virtual int getWidth()
		{
			return grid[0].Length;
		}

		public virtual int getHeight()
		{
			return grid.Length;
		}

		public virtual int getWidth(int ax, int ay)
		{
			return grid[ax][ay].Width;
		}

		public virtual int getHeight(int ax, int ay)
		{
			return grid[ax][ay].Height;
		}

		public virtual int getX(int ax, int x)
		{
			int num = x;
			for (int i = 0; i < ax; i++)
			{
				num += grid[i][0].Width - 1;
			}
			return num;
		}

		public virtual int getY(int ay, int y)
		{
			int num = y;
			for (int i = 0; i < ay; i++)
			{
				num += grid[0][i].Height - 1;
			}
			return num;
		}

		public virtual void adjust(Point adjust)
		{
			int x = adjust.X;
			int y = adjust.Y;
			for (int i = 0; i < grid[0].Length; i++)
			{
				for (int j = 0; j < grid.Length; j++)
				{
					for (int k = 0; k < grid[j][i].XLines.Length; k++)
					{
						grid[j][i].XLines[k].translate(x, y);
					}
					for (int l = 0; l < grid[j][i].YLines.Length; l++)
					{
						grid[j][i].YLines[l].translate(x, y);
					}
				}
			}
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class FinderPatternNotFoundException : Exception
	{
		internal string message = null;

		public override string Message => message;

		public FinderPatternNotFoundException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Util
{
	public class DebugCanvasAdapter : DebugCanvas
	{
		public virtual void println(string string_Renamed)
		{
		}

		public virtual void drawPoint(MessagingToolkit.QRCode.Geom.Point point, int color)
		{
		}

		public virtual void drawCross(MessagingToolkit.QRCode.Geom.Point point, int color)
		{
		}

		public virtual void drawPoints(MessagingToolkit.QRCode.Geom.Point[] points, int color)
		{
		}

		public virtual void drawLine(Line line, int color)
		{
		}

		public virtual void drawLines(Line[] lines, int color)
		{
		}

		public virtual void drawPolygon(MessagingToolkit.QRCode.Geom.Point[] points, int color)
		{
		}

		public virtual void drawMatrix(bool[][] matrix)
		{
		}
	}
}
namespace MessagingToolkit.QRCode.Codec
{
	public class QRCodeDecoder
	{
		internal class DecodeResult
		{
			internal int numCorrections;

			internal bool correctionSucceeded;

			internal sbyte[] decodedBytes;

			private QRCodeDecoder enclosingInstance;

			public virtual sbyte[] DecodedBytes => decodedBytes;

			public virtual int NumErrors => numCorrections;

			public virtual bool CorrectionSucceeded => correctionSucceeded;

			public QRCodeDecoder Enclosing_Instance => enclosingInstance;

			public DecodeResult(QRCodeDecoder enclosingInstance, sbyte[] decodedBytes, int numErrors, bool correctionSucceeded)
			{
				InitBlock(enclosingInstance);
				this.decodedBytes = decodedBytes;
				numCorrections = numErrors;
				this.correctionSucceeded = correctionSucceeded;
			}

			private void InitBlock(QRCodeDecoder enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
		}

		internal QRCodeSymbol qrCodeSymbol;

		internal int numTryDecode;

		internal ArrayList results;

		internal ArrayList lastResults = ArrayList.Synchronized(new ArrayList(10));

		internal static DebugCanvas canvas;

		internal QRCodeImageReader imageReader;

		internal int numLastCorrections;

		internal bool correctionSucceeded;

		public static DebugCanvas Canvas
		{
			get
			{
				return canvas;
			}
			set
			{
				canvas = value;
			}
		}

		internal virtual MessagingToolkit.QRCode.Geom.Point[] AdjustPoints
		{
			get
			{
				ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
				for (int i = 0; i < 4; i++)
				{
					arrayList.Add(new MessagingToolkit.QRCode.Geom.Point(1, 1));
				}
				int num = 0;
				int num2 = 0;
				for (int num3 = 0; num3 > -4; num3--)
				{
					for (int num4 = 0; num4 > -4; num4--)
					{
						if (num4 != num3 && (num4 + num3) % 2 == 0)
						{
							arrayList.Add(new MessagingToolkit.QRCode.Geom.Point(num4 - num, num3 - num2));
							num = num4;
							num2 = num3;
						}
					}
				}
				MessagingToolkit.QRCode.Geom.Point[] array = new MessagingToolkit.QRCode.Geom.Point[arrayList.Count];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = (MessagingToolkit.QRCode.Geom.Point)arrayList[j];
				}
				return array;
			}
		}

		public QRCodeDecoder()
		{
			numTryDecode = 0;
			results = ArrayList.Synchronized(new ArrayList(10));
			canvas = new DebugCanvasAdapter();
		}

		public virtual sbyte[] decodeBytes(QRCodeImage qrCodeImage)
		{
			MessagingToolkit.QRCode.Geom.Point[] adjustPoints = AdjustPoints;
			ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
			while (numTryDecode < adjustPoints.Length)
			{
				try
				{
					DecodeResult decodeResult = decode(qrCodeImage, adjustPoints[numTryDecode]);
					if (decodeResult.CorrectionSucceeded)
					{
						return decodeResult.DecodedBytes;
					}
					arrayList.Add(decodeResult);
					canvas.println("Decoding succeeded but could not correct");
					canvas.println("all errors. Retrying..");
				}
				catch (DecodingFailedException ex)
				{
					if (ex.Message.IndexOf("Finder Pattern") >= 0)
					{
						throw ex;
					}
				}
				finally
				{
					numTryDecode++;
				}
			}
			if (arrayList.Count == 0)
			{
				throw new DecodingFailedException("Give up decoding");
			}
			int num = -1;
			int num2 = int.MaxValue;
			for (int i = 0; i < arrayList.Count; i++)
			{
				DecodeResult decodeResult = (DecodeResult)arrayList[i];
				if (decodeResult.NumErrors < num2)
				{
					num2 = decodeResult.NumErrors;
					num = i;
				}
			}
			canvas.println("All trials need for correct error");
			canvas.println("Reporting #" + num + " that,");
			canvas.println("corrected minimum errors (" + num2 + ")");
			canvas.println("Decoding finished.");
			return ((DecodeResult)arrayList[num]).DecodedBytes;
		}

		public virtual string decode(QRCodeImage qrCodeImage, Encoding encoding)
		{
			sbyte[] array = decodeBytes(qrCodeImage);
			byte[] array2 = new byte[array.Length];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return encoding.GetString(array2);
		}

		public virtual string decode(QRCodeImage qrCodeImage)
		{
			sbyte[] array = decodeBytes(qrCodeImage);
			byte[] array2 = new byte[array.Length];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			Encoding encoding = ((!QRCodeUtility.IsUnicode(array2)) ? Encoding.ASCII : Encoding.Unicode);
			return encoding.GetString(array2);
		}

		internal virtual DecodeResult decode(QRCodeImage qrCodeImage, MessagingToolkit.QRCode.Geom.Point adjust)
		{
			try
			{
				if (numTryDecode == 0)
				{
					canvas.println("Decoding started");
					int[][] image = imageToIntArray(qrCodeImage);
					imageReader = new QRCodeImageReader();
					qrCodeSymbol = imageReader.getQRCodeSymbol(image);
				}
				else
				{
					canvas.println("--");
					canvas.println("Decoding restarted #" + numTryDecode);
					qrCodeSymbol = imageReader.getQRCodeSymbolWithAdjustedGrid(adjust);
				}
			}
			catch (SymbolNotFoundException ex)
			{
				throw new DecodingFailedException(ex.Message);
			}
			canvas.println("Created QRCode symbol.");
			canvas.println("Reading symbol.");
			canvas.println("Version: " + qrCodeSymbol.VersionReference);
			canvas.println("Mask pattern: " + qrCodeSymbol.MaskPatternRefererAsString);
			int[] blocks = qrCodeSymbol.Blocks;
			canvas.println("Correcting data errors.");
			blocks = correctDataBlocks(blocks);
			try
			{
				sbyte[] decodedByteArray = getDecodedByteArray(blocks, qrCodeSymbol.Version, qrCodeSymbol.NumErrorCollectionCode);
				return new DecodeResult(this, decodedByteArray, numLastCorrections, correctionSucceeded);
			}
			catch (InvalidDataBlockException ex2)
			{
				canvas.println(ex2.Message);
				throw new DecodingFailedException(ex2.Message);
			}
		}

		internal virtual int[][] imageToIntArray(QRCodeImage image)
		{
			int width = image.Width;
			int height = image.Height;
			int[][] array = new int[width][];
			for (int i = 0; i < width; i++)
			{
				array[i] = new int[height];
			}
			for (int j = 0; j < height; j++)
			{
				for (int k = 0; k < width; k++)
				{
					array[k][j] = image.getPixel(k, j);
				}
			}
			return array;
		}

		internal virtual int[] correctDataBlocks(int[] blocks)
		{
			int num = 0;
			int dataCapacity = qrCodeSymbol.DataCapacity;
			int[] array = new int[dataCapacity];
			int numErrorCollectionCode = qrCodeSymbol.NumErrorCollectionCode;
			int numRSBlocks = qrCodeSymbol.NumRSBlocks;
			int num2 = numErrorCollectionCode / numRSBlocks;
			if (numRSBlocks == 1)
			{
				ReedSolomon reedSolomon = new ReedSolomon(blocks, num2);
				reedSolomon.correct();
				num += reedSolomon.NumCorrectedErrors;
				if (num > 0)
				{
					canvas.println(Convert.ToString(num) + " data errors corrected.");
				}
				else
				{
					canvas.println("No errors found.");
				}
				numLastCorrections = num;
				correctionSucceeded = reedSolomon.CorrectionSucceeded;
				return blocks;
			}
			int num3 = dataCapacity % numRSBlocks;
			if (num3 == 0)
			{
				int num4 = dataCapacity / numRSBlocks;
				int[][] array2 = new int[numRSBlocks][];
				for (int i = 0; i < numRSBlocks; i++)
				{
					array2[i] = new int[num4];
				}
				int[][] array3 = array2;
				for (int i = 0; i < numRSBlocks; i++)
				{
					for (int j = 0; j < num4; j++)
					{
						array3[i][j] = blocks[j * numRSBlocks + i];
					}
					ReedSolomon reedSolomon = new ReedSolomon(array3[i], num2);
					reedSolomon.correct();
					num += reedSolomon.NumCorrectedErrors;
					correctionSucceeded = reedSolomon.CorrectionSucceeded;
				}
				int num5 = 0;
				for (int i = 0; i < numRSBlocks; i++)
				{
					for (int j = 0; j < num4 - num2; j++)
					{
						array[num5++] = array3[i][j];
					}
				}
			}
			else
			{
				int num6 = dataCapacity / numRSBlocks;
				int num7 = dataCapacity / numRSBlocks + 1;
				int num8 = numRSBlocks - num3;
				int[][] array4 = new int[num8][];
				for (int k = 0; k < num8; k++)
				{
					array4[k] = new int[num6];
				}
				int[][] array5 = array4;
				int[][] array6 = new int[num3][];
				for (int l = 0; l < num3; l++)
				{
					array6[l] = new int[num7];
				}
				int[][] array7 = array6;
				for (int i = 0; i < numRSBlocks; i++)
				{
					int num9;
					ReedSolomon reedSolomon;
					if (i < num8)
					{
						num9 = 0;
						for (int j = 0; j < num6; j++)
						{
							if (j == num6 - num2)
							{
								num9 = num3;
							}
							array5[i][j] = blocks[j * numRSBlocks + i + num9];
						}
						reedSolomon = new ReedSolomon(array5[i], num2);
						reedSolomon.correct();
						num += reedSolomon.NumCorrectedErrors;
						correctionSucceeded = reedSolomon.CorrectionSucceeded;
						continue;
					}
					num9 = 0;
					for (int j = 0; j < num7; j++)
					{
						if (j == num6 - num2)
						{
							num9 = num8;
						}
						array7[i - num8][j] = blocks[j * numRSBlocks + i - num9];
					}
					reedSolomon = new ReedSolomon(array7[i - num8], num2);
					reedSolomon.correct();
					num += reedSolomon.NumCorrectedErrors;
					correctionSucceeded = reedSolomon.CorrectionSucceeded;
				}
				int num5 = 0;
				for (int i = 0; i < numRSBlocks; i++)
				{
					if (i < num8)
					{
						for (int j = 0; j < num6 - num2; j++)
						{
							array[num5++] = array5[i][j];
						}
					}
					else
					{
						for (int j = 0; j < num7 - num2; j++)
						{
							array[num5++] = array7[i - num8][j];
						}
					}
				}
			}
			if (num > 0)
			{
				canvas.println(Convert.ToString(num) + " data errors corrected.");
			}
			else
			{
				canvas.println("No errors found.");
			}
			numLastCorrections = num;
			return array;
		}

		internal virtual sbyte[] getDecodedByteArray(int[] blocks, int version, int numErrorCorrectionCode)
		{
			QRCodeDataBlockReader qRCodeDataBlockReader = new QRCodeDataBlockReader(blocks, version, numErrorCorrectionCode);
			try
			{
				return qRCodeDataBlockReader.DataByte;
			}
			catch (InvalidDataBlockException ex)
			{
				throw ex;
			}
		}

		internal virtual string getDecodedString(int[] blocks, int version, int numErrorCorrectionCode)
		{
			string text = null;
			QRCodeDataBlockReader qRCodeDataBlockReader = new QRCodeDataBlockReader(blocks, version, numErrorCorrectionCode);
			try
			{
				return qRCodeDataBlockReader.DataString;
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new InvalidDataBlockException(ex.Message);
			}
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Data
{
	public class QRCodeSymbol
	{
		internal int version;

		internal int errorCollectionLevel;

		internal int maskPattern;

		internal int dataCapacity;

		internal bool[][] moduleMatrix;

		internal int width;

		internal int height;

		internal MessagingToolkit.QRCode.Geom.Point[][] alignmentPattern;

		internal int[][] numErrorCollectionCode = new int[40][]
		{
			new int[4] { 7, 10, 13, 17 },
			new int[4] { 10, 16, 22, 28 },
			new int[4] { 15, 26, 36, 44 },
			new int[4] { 20, 36, 52, 64 },
			new int[4] { 26, 48, 72, 88 },
			new int[4] { 36, 64, 96, 112 },
			new int[4] { 40, 72, 108, 130 },
			new int[4] { 48, 88, 132, 156 },
			new int[4] { 60, 110, 160, 192 },
			new int[4] { 72, 130, 192, 224 },
			new int[4] { 80, 150, 224, 264 },
			new int[4] { 96, 176, 260, 308 },
			new int[4] { 104, 198, 288, 352 },
			new int[4] { 120, 216, 320, 384 },
			new int[4] { 132, 240, 360, 432 },
			new int[4] { 144, 280, 408, 480 },
			new int[4] { 168, 308, 448, 532 },
			new int[4] { 180, 338, 504, 588 },
			new int[4] { 196, 364, 546, 650 },
			new int[4] { 224, 416, 600, 700 },
			new int[4] { 224, 442, 644, 750 },
			new int[4] { 252, 476, 690, 816 },
			new int[4] { 270, 504, 750, 900 },
			new int[4] { 300, 560, 810, 960 },
			new int[4] { 312, 588, 870, 1050 },
			new int[4] { 336, 644, 952, 1110 },
			new int[4] { 360, 700, 1020, 1200 },
			new int[4] { 390, 728, 1050, 1260 },
			new int[4] { 420, 784, 1140, 1350 },
			new int[4] { 450, 812, 1200, 1440 },
			new int[4] { 480, 868, 1290, 1530 },
			new int[4] { 510, 924, 1350, 1620 },
			new int[4] { 540, 980, 1440, 1710 },
			new int[4] { 570, 1036, 1530, 1800 },
			new int[4] { 570, 1064, 1590, 1890 },
			new int[4] { 600, 1120, 1680, 1980 },
			new int[4] { 630, 1204, 1770, 2100 },
			new int[4] { 660, 1260, 1860, 2220 },
			new int[4] { 720, 1316, 1950, 2310 },
			new int[4] { 750, 1372, 2040, 2430 }
		};

		internal int[][] numRSBlocks = new int[40][]
		{
			new int[4] { 1, 1, 1, 1 },
			new int[4] { 1, 1, 1, 1 },
			new int[4] { 1, 1, 2, 2 },
			new int[4] { 1, 2, 2, 4 },
			new int[4] { 1, 2, 4, 4 },
			new int[4] { 2, 4, 4, 4 },
			new int[4] { 2, 4, 6, 5 },
			new int[4] { 2, 4, 6, 6 },
			new int[4] { 2, 5, 8, 8 },
			new int[4] { 4, 5, 8, 8 },
			new int[4] { 4, 5, 8, 11 },
			new int[4] { 4, 8, 10, 11 },
			new int[4] { 4, 9, 12, 16 },
			new int[4] { 4, 9, 16, 16 },
			new int[4] { 6, 10, 12, 18 },
			new int[4] { 6, 10, 17, 16 },
			new int[4] { 6, 11, 16, 19 },
			new int[4] { 6, 13, 18, 21 },
			new int[4] { 7, 14, 21, 25 },
			new int[4] { 8, 16, 20, 25 },
			new int[4] { 8, 17, 23, 25 },
			new int[4] { 9, 17, 23, 34 },
			new int[4] { 9, 18, 25, 30 },
			new int[4] { 10, 20, 27, 32 },
			new int[4] { 12, 21, 29, 35 },
			new int[4] { 12, 23, 34, 37 },
			new int[4] { 12, 25, 34, 40 },
			new int[4] { 13, 26, 35, 42 },
			new int[4] { 14, 28, 38, 45 },
			new int[4] { 15, 29, 40, 48 },
			new int[4] { 16, 31, 43, 51 },
			new int[4] { 17, 33, 45, 54 },
			new int[4] { 18, 35, 48, 57 },
			new int[4] { 19, 37, 51, 60 },
			new int[4] { 19, 38, 53, 63 },
			new int[4] { 20, 40, 56, 66 },
			new int[4] { 21, 43, 59, 70 },
			new int[4] { 22, 45, 62, 74 },
			new int[4] { 24, 47, 65, 77 },
			new int[4] { 25, 49, 68, 81 }
		};

		public virtual int NumErrorCollectionCode => numErrorCollectionCode[version - 1][errorCollectionLevel];

		public virtual int NumRSBlocks => numRSBlocks[version - 1][errorCollectionLevel];

		public virtual int Version => version;

		public virtual string VersionReference
		{
			get
			{
				char[] array = new char[4] { 'L', 'M', 'Q', 'H' };
				return Convert.ToString(version) + "-" + array[errorCollectionLevel];
			}
		}

		public virtual MessagingToolkit.QRCode.Geom.Point[][] AlignmentPattern => alignmentPattern;

		public virtual int DataCapacity => dataCapacity;

		public virtual int ErrorCollectionLevel => errorCollectionLevel;

		public virtual int MaskPatternReferer => maskPattern;

		public virtual string MaskPatternRefererAsString
		{
			get
			{
				string text = Convert.ToString(MaskPatternReferer, 2);
				int length = text.Length;
				for (int i = 0; i < 3 - length; i++)
				{
					text = "0" + text;
				}
				return text;
			}
		}

		public virtual int Width => width;

		public virtual int Height => height;

		public virtual int[] Blocks
		{
			get
			{
				int num = Width;
				int num2 = Height;
				int num3 = num - 1;
				int num4 = num2 - 1;
				ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
				ArrayList arrayList2 = ArrayList.Synchronized(new ArrayList(10));
				int num5 = 0;
				int num6 = 7;
				int num7 = 0;
				bool flag = true;
				bool flag2 = false;
				bool flag3 = flag;
				do
				{
					arrayList.Add(getElement(num3, num4));
					if (getElement(num3, num4))
					{
						num5 += 1 << num6;
					}
					num6--;
					if (num6 == -1)
					{
						arrayList2.Add(num5);
						num6 = 7;
						num5 = 0;
					}
					do
					{
						if (flag3 == flag)
						{
							if ((num3 + num7) % 2 == 0)
							{
								num3--;
								continue;
							}
							if (num4 > 0)
							{
								num3++;
								num4--;
								continue;
							}
							num3--;
							if (num3 == 6)
							{
								num3--;
								num7 = 1;
							}
							flag3 = flag2;
						}
						else if ((num3 + num7) % 2 == 0)
						{
							num3--;
						}
						else if (num4 < num2 - 1)
						{
							num3++;
							num4++;
						}
						else
						{
							num3--;
							if (num3 == 6)
							{
								num3--;
								num7 = 1;
							}
							flag3 = flag;
						}
					}
					while (isInFunctionPattern(num3, num4));
				}
				while (num3 != -1);
				int[] array = new int[arrayList2.Count];
				for (int i = 0; i < arrayList2.Count; i++)
				{
					int num8 = (int)arrayList2[i];
					array[i] = num8;
				}
				return array;
			}
		}

		public virtual bool getElement(int x, int y)
		{
			return moduleMatrix[x][y];
		}

		public QRCodeSymbol(bool[][] moduleMatrix)
		{
			this.moduleMatrix = moduleMatrix;
			width = moduleMatrix.Length;
			height = moduleMatrix[0].Length;
			initialize();
		}

		internal virtual void initialize()
		{
			version = (width - 17) / 4;
			MessagingToolkit.QRCode.Geom.Point[][] array = new MessagingToolkit.QRCode.Geom.Point[1][];
			for (int i = 0; i < 1; i++)
			{
				array[i] = new MessagingToolkit.QRCode.Geom.Point[1];
			}
			int[] array2 = new int[1];
			if (version >= 2 && version <= 40)
			{
				array2 = LogicalSeed.getSeed(version);
				MessagingToolkit.QRCode.Geom.Point[][] array3 = new MessagingToolkit.QRCode.Geom.Point[array2.Length][];
				for (int j = 0; j < array2.Length; j++)
				{
					array3[j] = new MessagingToolkit.QRCode.Geom.Point[array2.Length];
				}
				array = array3;
			}
			for (int k = 0; k < array2.Length; k++)
			{
				for (int l = 0; l < array2.Length; l++)
				{
					array[l][k] = new MessagingToolkit.QRCode.Geom.Point(array2[l], array2[k]);
				}
			}
			alignmentPattern = array;
			dataCapacity = calcDataCapacity();
			bool[] formatInformation = readFormatInformation();
			decodeFormatInformation(formatInformation);
			unmask();
		}

		internal virtual bool[] readFormatInformation()
		{
			bool[] array = new bool[15];
			for (int i = 0; i <= 5; i++)
			{
				array[i] = getElement(8, i);
			}
			array[6] = getElement(8, 7);
			array[7] = getElement(8, 8);
			array[8] = getElement(7, 8);
			for (int i = 9; i <= 14; i++)
			{
				array[i] = getElement(14 - i, 8);
			}
			int number = 21522;
			for (int i = 0; i <= 14; i++)
			{
				bool flag = false;
				flag = (SystemUtils.URShift(number, i) & 1) == 1;
				if (array[i] == flag)
				{
					array[i] = false;
				}
				else
				{
					array[i] = true;
				}
			}
			BCH15_5 bCH15_ = new BCH15_5(array);
			bool[] array2 = bCH15_.correct();
			bool[] array3 = new bool[5];
			for (int i = 0; i < 5; i++)
			{
				array3[i] = array2[10 + i];
			}
			return array3;
		}

		internal virtual void unmask()
		{
			bool[][] array = generateMaskPattern();
			int num = Width;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if (array[j][i])
					{
						reverseElement(j, i);
					}
				}
			}
		}

		internal virtual bool[][] generateMaskPattern()
		{
			int maskPatternReferer = MaskPatternReferer;
			int num = Width;
			int num2 = Height;
			bool[][] array = new bool[num][];
			for (int i = 0; i < num; i++)
			{
				array[i] = new bool[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				for (int k = 0; k < num; k++)
				{
					if (isInFunctionPattern(k, j))
					{
						continue;
					}
					switch (maskPatternReferer)
					{
					case 0:
						if ((j + k) % 2 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 1:
						if (j % 2 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 2:
						if (k % 3 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 3:
						if ((j + k) % 3 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 4:
						if ((j / 2 + k / 3) % 2 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 5:
						if (j * k % 2 + j * k % 3 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 6:
						if ((j * k % 2 + j * k % 3) % 2 == 0)
						{
							array[k][j] = true;
						}
						break;
					case 7:
						if ((j * k % 3 + (j + k) % 2) % 2 == 0)
						{
							array[k][j] = true;
						}
						break;
					}
				}
			}
			return array;
		}

		private int calcDataCapacity()
		{
			int num = 0;
			int num2 = 0;
			int num3 = Version;
			num2 = ((num3 > 6) ? 67 : 31);
			int num4 = num3 / 7 + 2;
			int num5 = ((num3 == 1) ? 192 : (192 + (num4 * num4 - 3) * 25));
			num = num5 + 8 * num3 + 2 - (num4 - 2) * 10;
			return (width * width - num - num2) / 8;
		}

		internal virtual void decodeFormatInformation(bool[] formatInformation)
		{
			if (!formatInformation[4])
			{
				if (formatInformation[3])
				{
					errorCollectionLevel = 0;
				}
				else
				{
					errorCollectionLevel = 1;
				}
			}
			else if (formatInformation[3])
			{
				errorCollectionLevel = 2;
			}
			else
			{
				errorCollectionLevel = 3;
			}
			for (int num = 2; num >= 0; num--)
			{
				if (formatInformation[num])
				{
					maskPattern += 1 << num;
				}
			}
		}

		public virtual void reverseElement(int x, int y)
		{
			moduleMatrix[x][y] = !moduleMatrix[x][y];
		}

		public virtual bool isInFunctionPattern(int targetX, int targetY)
		{
			if (targetX < 9 && targetY < 9)
			{
				return true;
			}
			if (targetX > Width - 9 && targetY < 9)
			{
				return true;
			}
			if (targetX < 9 && targetY > Height - 9)
			{
				return true;
			}
			if (version >= 7)
			{
				if (targetX > Width - 12 && targetY < 6)
				{
					return true;
				}
				if (targetX < 6 && targetY > Height - 12)
				{
					return true;
				}
			}
			if (targetX == 6 || targetY == 6)
			{
				return true;
			}
			MessagingToolkit.QRCode.Geom.Point[][] array = AlignmentPattern;
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if ((j != 0 || i != 0) && (j != num - 1 || i != 0) && (j != 0 || i != num - 1) && Math.Abs(array[j][i].X - targetX) < 3 && Math.Abs(array[j][i].Y - targetY) < 3)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
namespace MessagingToolkit.QRCode.Geom
{
	public class Line
	{
		internal int x1;

		internal int y1;

		internal int x2;

		internal int y2;

		public virtual bool Horizontal
		{
			get
			{
				if (y1 == y2)
				{
					return true;
				}
				return false;
			}
		}

		public virtual bool Vertical
		{
			get
			{
				if (x1 == x2)
				{
					return true;
				}
				return false;
			}
		}

		public virtual Point Center
		{
			get
			{
				int x = (x1 + x2) / 2;
				int y = (y1 + y2) / 2;
				return new Point(x, y);
			}
		}

		public virtual int Length
		{
			get
			{
				int num = Math.Abs(x2 - x1);
				int num2 = Math.Abs(y2 - y1);
				return QRCodeUtility.sqrt(num * num + num2 * num2);
			}
		}

		public Line()
		{
			x1 = (y1 = (x2 = (y2 = 0)));
		}

		public Line(int x1, int y1, int x2, int y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}

		public Line(Point p1, Point p2)
		{
			x1 = p1.X;
			y1 = p1.Y;
			x2 = p2.X;
			y2 = p2.Y;
		}

		public virtual Point getP1()
		{
			return new Point(x1, y1);
		}

		public virtual Point getP2()
		{
			return new Point(x2, y2);
		}

		public virtual void setLine(int x1, int y1, int x2, int y2)
		{
			this.x1 = x1;
			this.y1 = y1;
			this.x2 = x2;
			this.y2 = y2;
		}

		public virtual void setP1(Point p1)
		{
			x1 = p1.X;
			y1 = p1.Y;
		}

		public virtual void setP1(int x1, int y1)
		{
			this.x1 = x1;
			this.y1 = y1;
		}

		public virtual void setP2(Point p2)
		{
			x2 = p2.X;
			y2 = p2.Y;
		}

		public virtual void setP2(int x2, int y2)
		{
			this.x2 = x2;
			this.y2 = y2;
		}

		public virtual void translate(int dx, int dy)
		{
			x1 += dx;
			y1 += dy;
			x2 += dx;
			y2 += dy;
		}

		public static bool isNeighbor(Line line1, Line line2)
		{
			if (Math.Abs(line1.getP1().X - line2.getP1().X) < 2 && Math.Abs(line1.getP1().Y - line2.getP1().Y) < 2 && Math.Abs(line1.getP2().X - line2.getP2().X) < 2 && Math.Abs(line1.getP2().Y - line2.getP2().Y) < 2)
			{
				return true;
			}
			return false;
		}

		public static bool isCross(Line line1, Line line2)
		{
			if (line1.Horizontal && line2.Vertical)
			{
				if (line1.getP1().Y > line2.getP1().Y && line1.getP1().Y < line2.getP2().Y && line2.getP1().X > line1.getP1().X && line2.getP1().X < line1.getP2().X)
				{
					return true;
				}
			}
			else if (line1.Vertical && line2.Horizontal && line1.getP1().X > line2.getP1().X && line1.getP1().X < line2.getP2().X && line2.getP1().Y > line1.getP1().Y && line2.getP1().Y < line1.getP2().Y)
			{
				return true;
			}
			return false;
		}

		public static Line getLongest(Line[] lines)
		{
			Line line = new Line();
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].Length > line.Length)
				{
					line = lines[i];
				}
			}
			return line;
		}

		public override string ToString()
		{
			return "(" + Convert.ToString(x1) + "," + Convert.ToString(y1) + ")-(" + Convert.ToString(x2) + "," + Convert.ToString(y2) + ")";
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Util
{
	public class SystemUtils
	{
		public static int ReadInput(Stream sourceStream, sbyte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			byte[] array = new byte[target.Length];
			int num = sourceStream.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (sbyte)array[i];
			}
			return num;
		}

		public static int ReadInput(TextReader sourceTextReader, short[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (short)array[i];
			}
			return num;
		}

		public static void WriteStackTrace(Exception throwable, TextWriter stream)
		{
			stream.Write(throwable.StackTrace);
			stream.Flush();
		}

		public static int URShift(int number, int bits)
		{
			if (number >= 0)
			{
				return number >> bits;
			}
			return (number >> bits) + (2 << ~bits);
		}

		public static int URShift(int number, long bits)
		{
			return URShift(number, (int)bits);
		}

		public static long URShift(long number, int bits)
		{
			if (number >= 0)
			{
				return number >> bits;
			}
			return (number >> bits) + (2L << ~bits);
		}

		public static long URShift(long number, long bits)
		{
			return URShift(number, (int)bits);
		}

		public static byte[] ToByteArray(sbyte[] sbyteArray)
		{
			byte[] array = null;
			if (sbyteArray != null)
			{
				array = new byte[sbyteArray.Length];
				for (int i = 0; i < sbyteArray.Length; i++)
				{
					array[i] = (byte)sbyteArray[i];
				}
			}
			return array;
		}

		public static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		public static byte[] ToByteArray(object[] tempObjectArray)
		{
			byte[] array = null;
			if (tempObjectArray != null)
			{
				array = new byte[tempObjectArray.Length];
				for (int i = 0; i < tempObjectArray.Length; i++)
				{
					array[i] = (byte)tempObjectArray[i];
				}
			}
			return array;
		}

		public static sbyte[] ToSByteArray(byte[] byteArray)
		{
			sbyte[] array = null;
			if (byteArray != null)
			{
				array = new sbyte[byteArray.Length];
				for (int i = 0; i < byteArray.Length; i++)
				{
					array[i] = (sbyte)byteArray[i];
				}
			}
			return array;
		}

		public static char[] ToCharArray(sbyte[] sByteArray)
		{
			return Encoding.UTF8.GetChars(ToByteArray(sByteArray));
		}

		public static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Reader
{
	public class QRCodeDataBlockReader
	{
		private const int MODE_NUMBER = 1;

		private const int MODE_ROMAN_AND_NUMBER = 2;

		private const int MODE_8BIT_BYTE = 4;

		private const int MODE_KANJI = 8;

		internal int[] blocks;

		internal int dataLengthMode;

		internal int blockPointer;

		internal int bitPointer;

		internal int dataLength;

		internal int numErrorCorrectionCode;

		internal DebugCanvas canvas;

		private int[][] sizeOfDataLengthInfo = new int[3][]
		{
			new int[4] { 10, 9, 8, 8 },
			new int[4] { 12, 11, 16, 10 },
			new int[4] { 14, 13, 16, 12 }
		};

		internal virtual int NextMode
		{
			get
			{
				if (blockPointer > blocks.Length - numErrorCorrectionCode - 2)
				{
					return 0;
				}
				return getNextBits(4);
			}
		}

		public virtual sbyte[] DataByte
		{
			get
			{
				canvas.println("Reading data blocks.");
				MemoryStream memoryStream = new MemoryStream();
				try
				{
					while (true)
					{
						int nextMode = NextMode;
						switch (nextMode)
						{
						case 0:
							if (memoryStream.Length > 0)
							{
								goto end_IL_0019;
							}
							throw new InvalidDataBlockException("Empty data block");
						default:
							if (nextMode != 8)
							{
								throw new InvalidDataBlockException("Invalid mode: " + nextMode + " in (block:" + blockPointer + " bit:" + bitPointer + ")");
							}
							break;
						case 1:
						case 2:
						case 4:
							break;
						}
						dataLength = getDataLength(nextMode);
						if (dataLength < 1)
						{
							throw new InvalidDataBlockException("Invalid data length: " + dataLength);
						}
						switch (nextMode)
						{
						case 1:
						{
							sbyte[] array4 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getFigureString(dataLength)));
							memoryStream.Write(SystemUtils.ToByteArray(array4), 0, array4.Length);
							break;
						}
						case 2:
						{
							sbyte[] array3 = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getRomanAndFigureString(dataLength)));
							memoryStream.Write(SystemUtils.ToByteArray(array3), 0, array3.Length);
							break;
						}
						case 4:
						{
							sbyte[] array2 = get8bitByteArray(dataLength);
							memoryStream.Write(SystemUtils.ToByteArray(array2), 0, array2.Length);
							break;
						}
						case 8:
						{
							sbyte[] array = SystemUtils.ToSByteArray(SystemUtils.ToByteArray(getKanjiString(dataLength)));
							memoryStream.Write(SystemUtils.ToByteArray(array), 0, array.Length);
							break;
						}
						}
						bool flag = true;
						continue;
						end_IL_0019:
						break;
					}
				}
				catch (IndexOutOfRangeException throwable)
				{
					SystemUtils.WriteStackTrace(throwable, Console.Error);
					throw new InvalidDataBlockException("Data Block Error in (block:" + blockPointer + " bit:" + bitPointer + ")");
				}
				catch (IOException ex)
				{
					throw new InvalidDataBlockException(ex.Message);
				}
				return SystemUtils.ToSByteArray(memoryStream.ToArray());
			}
		}

		public virtual string DataString
		{
			get
			{
				canvas.println("Reading data blocks...");
				string text = "";
				while (true)
				{
					int nextMode = NextMode;
					canvas.println("mode: " + nextMode);
					if (nextMode == 0)
					{
						break;
					}
					if (nextMode == 1 || nextMode == 2 || nextMode == 4 || nextMode != 8)
					{
					}
					dataLength = getDataLength(nextMode);
					canvas.println(Convert.ToString(blocks[blockPointer]));
					Console.Out.WriteLine("length: " + dataLength);
					switch (nextMode)
					{
					case 1:
						text += getFigureString(dataLength);
						break;
					case 2:
						text += getRomanAndFigureString(dataLength);
						break;
					case 4:
						text += get8bitByteString(dataLength);
						break;
					case 8:
						text += getKanjiString(dataLength);
						break;
					}
					bool flag = true;
				}
				Console.Out.WriteLine("");
				return text;
			}
		}

		public QRCodeDataBlockReader(int[] blocks, int version, int numErrorCorrectionCode)
		{
			blockPointer = 0;
			bitPointer = 7;
			dataLength = 0;
			this.blocks = blocks;
			this.numErrorCorrectionCode = numErrorCorrectionCode;
			if (version <= 9)
			{
				dataLengthMode = 0;
			}
			else if (version >= 10 && version <= 26)
			{
				dataLengthMode = 1;
			}
			else if (version >= 27 && version <= 40)
			{
				dataLengthMode = 2;
			}
			canvas = QRCodeDecoder.Canvas;
		}

		internal virtual int getNextBits(int numBits)
		{
			int num = 0;
			if (numBits < bitPointer + 1)
			{
				int num2 = 0;
				for (int i = 0; i < numBits; i++)
				{
					num2 += 1 << i;
				}
				num2 <<= bitPointer - numBits + 1;
				num = (blocks[blockPointer] & num2) >> bitPointer - numBits + 1;
				bitPointer -= numBits;
				return num;
			}
			if (numBits < bitPointer + 1 + 8)
			{
				int num3 = 0;
				for (int i = 0; i < bitPointer + 1; i++)
				{
					num3 += 1 << i;
				}
				num = (blocks[blockPointer] & num3) << numBits - (bitPointer + 1);
				blockPointer++;
				num += blocks[blockPointer] >> 8 - (numBits - (bitPointer + 1));
				bitPointer -= numBits % 8;
				if (bitPointer < 0)
				{
					bitPointer = 8 + bitPointer;
				}
				return num;
			}
			if (numBits < bitPointer + 1 + 16)
			{
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < bitPointer + 1; i++)
				{
					num3 += 1 << i;
				}
				int num5 = (blocks[blockPointer] & num3) << numBits - (bitPointer + 1);
				blockPointer++;
				int num6 = blocks[blockPointer] << numBits - (bitPointer + 1 + 8);
				blockPointer++;
				for (int i = 0; i < numBits - (bitPointer + 1 + 8); i++)
				{
					num4 += 1 << i;
				}
				num4 <<= 8 - (numBits - (bitPointer + 1 + 8));
				int num7 = (blocks[blockPointer] & num4) >> 8 - (numBits - (bitPointer + 1 + 8));
				num = num5 + num6 + num7;
				bitPointer -= (numBits - 8) % 8;
				if (bitPointer < 0)
				{
					bitPointer = 8 + bitPointer;
				}
				return num;
			}
			Console.Out.WriteLine("ERROR!");
			return 0;
		}

		internal virtual int guessMode(int mode)
		{
			return mode switch
			{
				3 => 1, 
				5 => 4, 
				6 => 4, 
				7 => 4, 
				9 => 8, 
				10 => 8, 
				11 => 8, 
				12 => 4, 
				13 => 4, 
				14 => 4, 
				15 => 4, 
				_ => 8, 
			};
		}

		internal virtual int getDataLength(int modeIndicator)
		{
			int num = 0;
			while (true)
			{
				bool flag = true;
				if (modeIndicator >> num == 1)
				{
					break;
				}
				num++;
			}
			return getNextBits(sizeOfDataLengthInfo[dataLengthMode][num]);
		}

		internal virtual string getFigureString(int dataLength)
		{
			int num = dataLength;
			int num2 = 0;
			string text = "";
			do
			{
				if (num >= 3)
				{
					num2 = getNextBits(10);
					if (num2 < 100)
					{
						text += "0";
					}
					if (num2 < 10)
					{
						text += "0";
					}
					num -= 3;
				}
				else
				{
					switch (num)
					{
					case 2:
						num2 = getNextBits(7);
						if (num2 < 10)
						{
							text += "0";
						}
						num -= 2;
						break;
					case 1:
						num2 = getNextBits(4);
						num--;
						break;
					}
				}
				text += Convert.ToString(num2);
			}
			while (num > 0);
			return text;
		}

		internal virtual string getRomanAndFigureString(int dataLength)
		{
			int num = dataLength;
			int num2 = 0;
			string text = "";
			char[] array = new char[45]
			{
				'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
				'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
				'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
				'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*',
				'+', '-', '.', '/', ':'
			};
			do
			{
				if (num > 1)
				{
					num2 = getNextBits(11);
					int num3 = num2 / 45;
					int num4 = num2 % 45;
					text += Convert.ToString(array[num3]);
					text += Convert.ToString(array[num4]);
					num -= 2;
				}
				else if (num == 1)
				{
					num2 = getNextBits(6);
					text += Convert.ToString(array[num2]);
					num--;
				}
			}
			while (num > 0);
			return text;
		}

		public virtual sbyte[] get8bitByteArray(int dataLength)
		{
			int num = dataLength;
			int num2 = 0;
			MemoryStream memoryStream = new MemoryStream();
			do
			{
				canvas.println("Length: " + num);
				num2 = getNextBits(8);
				memoryStream.WriteByte((byte)num2);
				num--;
			}
			while (num > 0);
			return SystemUtils.ToSByteArray(memoryStream.ToArray());
		}

		internal virtual string get8bitByteString(int dataLength)
		{
			int num = dataLength;
			int num2 = 0;
			string text = "";
			do
			{
				num2 = getNextBits(8);
				text += (char)num2;
				num--;
			}
			while (num > 0);
			return text;
		}

		internal virtual string getKanjiString(int dataLength)
		{
			int num = dataLength;
			int num2 = 0;
			string text = "";
			do
			{
				num2 = getNextBits(13);
				int num3 = num2 % 192;
				int num4 = num2 / 192;
				int num5 = (num4 << 8) + num3;
				int num6 = 0;
				num6 = ((num5 + 33088 > 40956) ? (num5 + 49472) : (num5 + 33088));
				text += new string(SystemUtils.ToCharArray(SystemUtils.ToByteArray(new sbyte[2]
				{
					(sbyte)(num6 >> 8),
					(sbyte)(num6 & 0xFF)
				})));
				num--;
			}
			while (num > 0);
			return text;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Reader.Pattern
{
	public class LogicalSeed
	{
		private static int[][] seed;

		public static int[] getSeed(int version)
		{
			return seed[version - 1];
		}

		public static int getSeed(int version, int patternNumber)
		{
			return seed[version - 1][patternNumber];
		}

		static LogicalSeed()
		{
			seed = new int[40][];
			seed[0] = new int[2] { 6, 14 };
			seed[1] = new int[2] { 6, 18 };
			seed[2] = new int[2] { 6, 22 };
			seed[3] = new int[2] { 6, 26 };
			seed[4] = new int[2] { 6, 30 };
			seed[5] = new int[2] { 6, 34 };
			seed[6] = new int[3] { 6, 22, 38 };
			seed[7] = new int[3] { 6, 24, 42 };
			seed[8] = new int[3] { 6, 26, 46 };
			seed[9] = new int[3] { 6, 28, 50 };
			seed[10] = new int[3] { 6, 30, 54 };
			seed[11] = new int[3] { 6, 32, 58 };
			seed[12] = new int[3] { 6, 34, 62 };
			seed[13] = new int[4] { 6, 26, 46, 66 };
			seed[14] = new int[4] { 6, 26, 48, 70 };
			seed[15] = new int[4] { 6, 26, 50, 74 };
			seed[16] = new int[4] { 6, 30, 54, 78 };
			seed[17] = new int[4] { 6, 30, 56, 82 };
			seed[18] = new int[4] { 6, 30, 58, 86 };
			seed[19] = new int[4] { 6, 34, 62, 90 };
			seed[20] = new int[5] { 6, 28, 50, 72, 94 };
			seed[21] = new int[5] { 6, 26, 50, 74, 98 };
			seed[22] = new int[5] { 6, 30, 54, 78, 102 };
			seed[23] = new int[5] { 6, 28, 54, 80, 106 };
			seed[24] = new int[5] { 6, 32, 58, 84, 110 };
			seed[25] = new int[5] { 6, 30, 58, 86, 114 };
			seed[26] = new int[5] { 6, 34, 62, 90, 118 };
			seed[27] = new int[6] { 6, 26, 50, 74, 98, 122 };
			seed[28] = new int[6] { 6, 30, 54, 78, 102, 126 };
			seed[29] = new int[6] { 6, 26, 52, 78, 104, 130 };
			seed[30] = new int[6] { 6, 30, 56, 82, 108, 134 };
			seed[31] = new int[6] { 6, 34, 60, 86, 112, 138 };
			seed[32] = new int[6] { 6, 30, 58, 86, 114, 142 };
			seed[33] = new int[6] { 6, 34, 62, 90, 118, 146 };
			seed[34] = new int[7] { 6, 30, 54, 78, 102, 126, 150 };
			seed[35] = new int[7] { 6, 24, 50, 76, 102, 128, 154 };
			seed[36] = new int[7] { 6, 28, 54, 80, 106, 132, 158 };
			seed[37] = new int[7] { 6, 32, 58, 84, 110, 136, 162 };
			seed[38] = new int[7] { 6, 26, 54, 82, 110, 138, 166 };
			seed[39] = new int[7] { 6, 30, 58, 86, 114, 142, 170 };
		}
	}
}
namespace MessagingToolkit.QRCode.ExceptionHandler
{
	[Serializable]
	public class InvalidDataBlockException : ArgumentException
	{
		internal string message = null;

		public override string Message => message;

		public InvalidDataBlockException(string message)
		{
			this.message = message;
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Util
{
	public class ConsoleCanvas : DebugCanvas
	{
		public void println(string str)
		{
			Console.WriteLine(str);
		}

		public void drawPoint(MessagingToolkit.QRCode.Geom.Point point, int color)
		{
		}

		public void drawCross(MessagingToolkit.QRCode.Geom.Point point, int color)
		{
		}

		public void drawPoints(MessagingToolkit.QRCode.Geom.Point[] points, int color)
		{
		}

		public void drawLine(Line line, int color)
		{
		}

		public void drawLines(Line[] lines, int color)
		{
		}

		public void drawPolygon(MessagingToolkit.QRCode.Geom.Point[] points, int color)
		{
		}

		public void drawMatrix(bool[][] matrix)
		{
		}
	}
}
namespace MessagingToolkit.QRCode.Codec.Ecc
{
	public class BCH15_5
	{
		internal int[][] gf16;

		internal bool[] recieveData;

		internal int numCorrectedError;

		internal static string[] bitName = new string[15]
		{
			"c0", "c1", "c2", "c3", "c4", "c5", "c6", "c7", "c8", "c9",
			"d0", "d1", "d2", "d3", "d4"
		};

		public virtual int NumCorrectedError => numCorrectedError;

		public BCH15_5(bool[] source)
		{
			gf16 = createGF16();
			recieveData = source;
		}

		public virtual bool[] correct()
		{
			int[] s = calcSyndrome(recieveData);
			int[] errorPos = detectErrorBitPosition(s);
			return correctErrorBit(recieveData, errorPos);
		}

		internal virtual int[][] createGF16()
		{
			gf16 = new int[16][];
			for (int i = 0; i < 16; i++)
			{
				gf16[i] = new int[4];
			}
			int[] array = new int[4] { 1, 1, 0, 0 };
			for (int i = 0; i < 4; i++)
			{
				gf16[i][i] = 1;
			}
			for (int i = 0; i < 4; i++)
			{
				gf16[4][i] = array[i];
			}
			for (int i = 5; i < 16; i++)
			{
				for (int j = 1; j < 4; j++)
				{
					gf16[i][j] = gf16[i - 1][j - 1];
				}
				if (gf16[i - 1][3] == 1)
				{
					for (int j = 0; j < 4; j++)
					{
						gf16[i][j] = (gf16[i][j] + array[j]) % 2;
					}
				}
			}
			return gf16;
		}

		internal virtual int searchElement(int[] x)
		{
			int i;
			for (i = 0; i < 15 && (x[0] != gf16[i][0] || x[1] != gf16[i][1] || x[2] != gf16[i][2] || x[3] != gf16[i][3]); i++)
			{
			}
			return i;
		}

		internal virtual int[] getCode(int input)
		{
			int[] array = new int[15];
			int[] array2 = new int[8];
			for (int i = 0; i < 15; i++)
			{
				int num = array2[7];
				int num2;
				int num3;
				if (i < 7)
				{
					num2 = (input >> 6 - i) % 2;
					num3 = (num2 + num) % 2;
				}
				else
				{
					num2 = num;
					num3 = 0;
				}
				array2[7] = (array2[6] + num3) % 2;
				array2[6] = (array2[5] + num3) % 2;
				array2[5] = array2[4];
				array2[4] = (array2[3] + num3) % 2;
				array2[3] = array2[2];
				array2[2] = array2[1];
				array2[1] = array2[0];
				array2[0] = num3;
				array[14 - i] = num2;
			}
			return array;
		}

		internal virtual int addGF(int arg1, int arg2)
		{
			int[] array = new int[4];
			for (int i = 0; i < 4; i++)
			{
				int num = ((arg1 >= 0 && arg1 < 15) ? gf16[arg1][i] : 0);
				int num2 = ((arg2 >= 0 && arg2 < 15) ? gf16[arg2][i] : 0);
				array[i] = (num + num2) % 2;
			}
			return searchElement(array);
		}

		internal virtual int[] calcSyndrome(bool[] y)
		{
			int[] array = new int[5];
			int[] array2 = new int[4];
			int i;
			for (i = 0; i < 15; i++)
			{
				if (y[i])
				{
					for (int j = 0; j < 4; j++)
					{
						array2[j] = (array2[j] + gf16[i][j]) % 2;
					}
				}
			}
			i = searchElement(array2);
			array[0] = ((i >= 15) ? (-1) : i);
			array2 = new int[4];
			for (i = 0; i < 15; i++)
			{
				if (y[i])
				{
					for (int j = 0; j < 4; j++)
					{
						array2[j] = (array2[j] + gf16[i * 3 % 15][j]) % 2;
					}
				}
			}
			i = searchElement(array2);
			array[2] = ((i >= 15) ? (-1) : i);
			array2 = new int[4];
			for (i = 0; i < 15; i++)
			{
				if (y[i])
				{
					for (int j = 0; j < 4; j++)
					{
						array2[j] = (array2[j] + gf16[i * 5 % 15][j]) % 2;
					}
				}
			}
			i = searchElement(array2);
			array[4] = ((i >= 15) ? (-1) : i);
			return array;
		}

		internal virtual int[] calcErrorPositionVariable(int[] s)
		{
			int[] array = new int[4]
			{
				s[0],
				0,
				0,
				0
			};
			int arg = (s[0] + s[1]) % 15;
			int num = addGF(s[2], arg);
			num = ((num >= 15) ? (-1) : num);
			arg = (s[2] + s[1]) % 15;
			int num2 = addGF(s[4], arg);
			num2 = ((num2 >= 15) ? (-1) : num2);
			array[1] = ((num2 < 0 && num < 0) ? (-1) : ((num2 - num + 15) % 15));
			arg = (s[1] + array[0]) % 15;
			int arg2 = addGF(s[2], arg);
			arg = (s[0] + array[1]) % 15;
			array[2] = addGF(arg2, arg);
			return array;
		}

		internal virtual int[] detectErrorBitPosition(int[] s)
		{
			int[] array = calcErrorPositionVariable(s);
			int[] array2 = new int[4];
			if (array[0] == -1)
			{
				return array2;
			}
			if (array[1] == -1)
			{
				array2[0] = 1;
				array2[1] = array[0];
				return array2;
			}
			for (int i = 0; i < 15; i++)
			{
				int arg = i * 3 % 15;
				int num = i * 2 % 15;
				int num2 = i;
				int arg2 = (array[0] + num) % 15;
				int arg3 = addGF(arg, arg2);
				arg2 = (array[1] + num2) % 15;
				int arg4 = addGF(arg2, array[2]);
				int num3 = addGF(arg3, arg4);
				if (num3 >= 15)
				{
					array2[0]++;
					array2[array2[0]] = i;
				}
			}
			return array2;
		}

		internal virtual bool[] correctErrorBit(bool[] y, int[] errorPos)
		{
			for (int i = 1; i <= errorPos[0]; i++)
			{
				y[errorPos[i]] = !y[errorPos[i]];
			}
			numCorrectedError = errorPos[0];
			return y;
		}
	}
}

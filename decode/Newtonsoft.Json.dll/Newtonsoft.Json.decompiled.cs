using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Linq.JsonPath;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

[assembly: Guid("9ca358aa-317b-4925-8ada-4a29e943a363")]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: ComVisible(false)]
[assembly: AssemblyFileVersion("6.0.8.18111")]
[assembly: CLSCompliant(true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: InternalsVisibleTo("Newtonsoft.Json.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f561df277c6c0b497d629032b410cdcf286e537c054724f7ffa0164345f62b3e642029d7a80cc351918955328c4adc8a048823ef90b0cf38ea7db0d729caf2b633c3babe08b0310198c1081995c19029bc675193744eab9d7345b8a67258ec17d112cebdbbb2a281487dceeafb9d83aa930f32103fbe1d2911425bc5744002c7")]
[assembly: TargetFramework(".NETFramework,Version=v4.0,Profile=Client", FrameworkDisplayName = ".NET Framework 4 Client Profile")]
[assembly: AssemblyTitle("Json.NET .NET 4.0")]
[assembly: AllowPartiallyTrustedCallers]
[assembly: InternalsVisibleTo("Newtonsoft.Json.Schema, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f561df277c6c0b497d629032b410cdcf286e537c054724f7ffa0164345f62b3e642029d7a80cc351918955328c4adc8a048823ef90b0cf38ea7db0d729caf2b633c3babe08b0310198c1081995c19029bc675193744eab9d7345b8a67258ec17d112cebdbbb2a281487dceeafb9d83aa930f32103fbe1d2911425bc5744002c7")]
[assembly: AssemblyConfiguration("")]
[assembly: InternalsVisibleTo("Newtonsoft.Json.Dynamic, PublicKey=0024000004800000940000000602000000240000525341310004000001000100cbd8d53b9d7de30f1f1278f636ec462cf9c254991291e66ebb157a885638a517887633b898ccbcf0d5c5ff7be85a6abe9e765d0ac7cd33c68dac67e7e64530e8222101109f154ab14a941c490ac155cd1d4fcba0fabb49016b4ef28593b015cab5937da31172f03f67d09edda404b88a60023f062ae71d0b2e4438b74cc11dc9")]
[assembly: AssemblyDescription("Json.NET is a popular high-performance JSON framework for .NET")]
[assembly: AssemblyProduct("Json.NET")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCompany("Newtonsoft")]
[assembly: AssemblyCopyright("Copyright © James Newton-King 2008")]
[assembly: AssemblyVersion("6.0.0.0")]
namespace Newtonsoft.Json.Bson
{
	internal enum BsonBinaryType : byte
	{
		Binary = 0,
		Function = 1,
		[Obsolete("This type has been deprecated in the BSON specification. Use Binary instead.")]
		BinaryOld = 2,
		[Obsolete("This type has been deprecated in the BSON specification. Use Uuid instead.")]
		UuidOld = 3,
		Uuid = 4,
		Md5 = 5,
		UserDefined = 128
	}
	internal class BsonBinaryWriter
	{
		private static readonly Encoding Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

		private readonly BinaryWriter _writer;

		private byte[] _largeByteBuffer;

		public DateTimeKind DateTimeKindHandling { get; set; }

		public BsonBinaryWriter(BinaryWriter writer)
		{
			DateTimeKindHandling = DateTimeKind.Utc;
			_writer = writer;
		}

		public void Flush()
		{
			_writer.Flush();
		}

		public void Close()
		{
			_writer.Close();
		}

		public void WriteToken(BsonToken t)
		{
			CalculateSize(t);
			WriteTokenInternal(t);
		}

		private void WriteTokenInternal(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				_writer.Write(bsonObject.CalculatedSize);
				foreach (BsonProperty item in bsonObject)
				{
					_writer.Write((sbyte)item.Value.Type);
					WriteString((string)item.Name.Value, item.Name.ByteCount, null);
					WriteTokenInternal(item.Value);
				}
				_writer.Write((byte)0);
				break;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				_writer.Write(bsonArray.CalculatedSize);
				ulong num2 = 0uL;
				foreach (BsonToken item2 in bsonArray)
				{
					_writer.Write((sbyte)item2.Type);
					WriteString(num2.ToString(CultureInfo.InvariantCulture), MathUtils.IntLength(num2), null);
					WriteTokenInternal(item2);
					num2++;
				}
				_writer.Write((byte)0);
				break;
			}
			case BsonType.Integer:
			{
				BsonValue bsonValue3 = (BsonValue)t;
				_writer.Write(Convert.ToInt32(bsonValue3.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.Long:
			{
				BsonValue bsonValue4 = (BsonValue)t;
				_writer.Write(Convert.ToInt64(bsonValue4.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.Number:
			{
				BsonValue bsonValue5 = (BsonValue)t;
				_writer.Write(Convert.ToDouble(bsonValue5.Value, CultureInfo.InvariantCulture));
				break;
			}
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				WriteString((string)bsonString.Value, bsonString.ByteCount, bsonString.CalculatedSize - 4);
				break;
			}
			case BsonType.Boolean:
			{
				BsonValue bsonValue6 = (BsonValue)t;
				_writer.Write((bool)bsonValue6.Value);
				break;
			}
			case BsonType.Date:
			{
				BsonValue bsonValue2 = (BsonValue)t;
				long num = 0L;
				if (bsonValue2.Value is DateTime)
				{
					DateTime dateTime = (DateTime)bsonValue2.Value;
					if (DateTimeKindHandling == DateTimeKind.Utc)
					{
						dateTime = dateTime.ToUniversalTime();
					}
					else if (DateTimeKindHandling == DateTimeKind.Local)
					{
						dateTime = dateTime.ToLocalTime();
					}
					num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime, convertToUtc: false);
				}
				else
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)bsonValue2.Value;
					num = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.UtcDateTime, dateTimeOffset.Offset);
				}
				_writer.Write(num);
				break;
			}
			case BsonType.Binary:
			{
				BsonBinary bsonBinary = (BsonBinary)t;
				byte[] array = (byte[])bsonBinary.Value;
				_writer.Write(array.Length);
				_writer.Write((byte)bsonBinary.BinaryType);
				_writer.Write(array);
				break;
			}
			case BsonType.Oid:
			{
				BsonValue bsonValue = (BsonValue)t;
				byte[] buffer = (byte[])bsonValue.Value;
				_writer.Write(buffer);
				break;
			}
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				WriteString((string)bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, null);
				WriteString((string)bsonRegex.Options.Value, bsonRegex.Options.ByteCount, null);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, t.Type));
			case BsonType.Undefined:
			case BsonType.Null:
				break;
			}
		}

		private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
		{
			if (calculatedlengthPrefix.HasValue)
			{
				_writer.Write(calculatedlengthPrefix.Value);
			}
			WriteUtf8Bytes(s, byteCount);
			_writer.Write((byte)0);
		}

		public void WriteUtf8Bytes(string s, int byteCount)
		{
			if (s != null)
			{
				if (_largeByteBuffer == null)
				{
					_largeByteBuffer = new byte[256];
				}
				if (byteCount <= 256)
				{
					Encoding.GetBytes(s, 0, s.Length, _largeByteBuffer, 0);
					_writer.Write(_largeByteBuffer, 0, byteCount);
				}
				else
				{
					byte[] bytes = Encoding.GetBytes(s);
					_writer.Write(bytes);
				}
			}
		}

		private int CalculateSize(int stringByteCount)
		{
			return stringByteCount + 1;
		}

		private int CalculateSizeWithLength(int stringByteCount, bool includeSize)
		{
			int num = ((!includeSize) ? 1 : 5);
			return num + stringByteCount;
		}

		private int CalculateSize(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				int num4 = 4;
				foreach (BsonProperty item in bsonObject)
				{
					int num5 = 1;
					num5 += CalculateSize(item.Name);
					num5 += CalculateSize(item.Value);
					num4 += num5;
				}
				return bsonObject.CalculatedSize = num4 + 1;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				int num2 = 4;
				ulong num3 = 0uL;
				foreach (BsonToken item2 in bsonArray)
				{
					num2++;
					num2 += CalculateSize(MathUtils.IntLength(num3));
					num2 += CalculateSize(item2);
					num3++;
				}
				num2++;
				bsonArray.CalculatedSize = num2;
				return bsonArray.CalculatedSize;
			}
			case BsonType.Integer:
				return 4;
			case BsonType.Long:
				return 8;
			case BsonType.Number:
				return 8;
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				string text = (string)bsonString.Value;
				bsonString.ByteCount = ((text != null) ? Encoding.GetByteCount(text) : 0);
				bsonString.CalculatedSize = CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
				return bsonString.CalculatedSize;
			}
			case BsonType.Boolean:
				return 1;
			case BsonType.Undefined:
			case BsonType.Null:
				return 0;
			case BsonType.Date:
				return 8;
			case BsonType.Binary:
			{
				BsonBinary bsonBinary = (BsonBinary)t;
				byte[] array = (byte[])bsonBinary.Value;
				bsonBinary.CalculatedSize = 5 + array.Length;
				return bsonBinary.CalculatedSize;
			}
			case BsonType.Oid:
				return 12;
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				int num = 0;
				num += CalculateSize(bsonRegex.Pattern);
				num += CalculateSize(bsonRegex.Options);
				bsonRegex.CalculatedSize = num;
				return bsonRegex.CalculatedSize;
			}
			default:
				throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.InvariantCulture, t.Type));
			}
		}
	}
}
namespace Newtonsoft.Json
{
	public abstract class JsonReader : IDisposable
	{
		protected internal enum State
		{
			Start,
			Complete,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			Closed,
			PostValue,
			ConstructorStart,
			Constructor,
			Error,
			Finished
		}

		private JsonToken _tokenType;

		private object _value;

		internal char _quoteChar;

		internal State _currentState;

		internal ReadType _readType;

		private JsonPosition _currentPosition;

		private CultureInfo _culture;

		private DateTimeZoneHandling _dateTimeZoneHandling;

		private int? _maxDepth;

		private bool _hasExceededMaxDepth;

		internal DateParseHandling _dateParseHandling;

		internal FloatParseHandling _floatParseHandling;

		private string _dateFormatString;

		private readonly List<JsonPosition> _stack;

		protected State CurrentState => _currentState;

		public bool CloseInput { get; set; }

		public bool SupportMultipleContent { get; set; }

		public virtual char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			protected internal set
			{
				_quoteChar = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling;
			}
			set
			{
				_dateTimeZoneHandling = value;
			}
		}

		public DateParseHandling DateParseHandling
		{
			get
			{
				return _dateParseHandling;
			}
			set
			{
				_dateParseHandling = value;
			}
		}

		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return _floatParseHandling;
			}
			set
			{
				_floatParseHandling = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString;
			}
			set
			{
				_dateFormatString = value;
			}
		}

		public int? MaxDepth
		{
			get
			{
				return _maxDepth;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				_maxDepth = value;
			}
		}

		public virtual JsonToken TokenType => _tokenType;

		public virtual object Value => _value;

		public virtual Type ValueType
		{
			get
			{
				if (_value == null)
				{
					return null;
				}
				return _value.GetType();
			}
		}

		public virtual int Depth
		{
			get
			{
				int count = _stack.Count;
				if (JsonTokenUtils.IsStartToken(TokenType) || _currentPosition.Type == JsonContainerType.None)
				{
					return count;
				}
				return count + 1;
			}
		}

		public virtual string Path
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				IEnumerable<JsonPosition> positions = ((_currentState == State.ArrayStart || _currentState == State.ConstructorStart || _currentState == State.ObjectStart) ? _stack : _stack.Concat(new JsonPosition[1] { _currentPosition }));
				return JsonPosition.BuildPath(positions);
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				_culture = value;
			}
		}

		internal JsonPosition GetPosition(int depth)
		{
			if (depth < _stack.Count)
			{
				return _stack[depth];
			}
			return _currentPosition;
		}

		protected JsonReader()
		{
			_currentState = State.Start;
			_stack = new List<JsonPosition>(4);
			_dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			_dateParseHandling = DateParseHandling.DateTime;
			_floatParseHandling = FloatParseHandling.Double;
			CloseInput = true;
		}

		private void Push(JsonContainerType value)
		{
			UpdateScopeWithFinishedValue();
			if (_currentPosition.Type == JsonContainerType.None)
			{
				_currentPosition = new JsonPosition(value);
				return;
			}
			_stack.Add(_currentPosition);
			_currentPosition = new JsonPosition(value);
			if (_maxDepth.HasValue)
			{
				int num = Depth + 1;
				int? maxDepth = _maxDepth;
				if (num > maxDepth.GetValueOrDefault() && maxDepth.HasValue && !_hasExceededMaxDepth)
				{
					_hasExceededMaxDepth = true;
					throw JsonReaderException.Create(this, "The reader's MaxDepth of {0} has been exceeded.".FormatWith(CultureInfo.InvariantCulture, _maxDepth));
				}
			}
		}

		private JsonContainerType Pop()
		{
			JsonPosition currentPosition;
			if (_stack.Count > 0)
			{
				currentPosition = _currentPosition;
				_currentPosition = _stack[_stack.Count - 1];
				_stack.RemoveAt(_stack.Count - 1);
			}
			else
			{
				currentPosition = _currentPosition;
				_currentPosition = default(JsonPosition);
			}
			if (_maxDepth.HasValue && Depth <= _maxDepth)
			{
				_hasExceededMaxDepth = false;
			}
			return currentPosition.Type;
		}

		private JsonContainerType Peek()
		{
			return _currentPosition.Type;
		}

		public abstract bool Read();

		public abstract int? ReadAsInt32();

		public abstract string ReadAsString();

		public abstract byte[] ReadAsBytes();

		public abstract decimal? ReadAsDecimal();

		public abstract DateTime? ReadAsDateTime();

		public abstract DateTimeOffset? ReadAsDateTimeOffset();

		internal virtual bool ReadInternal()
		{
			throw new NotImplementedException();
		}

		internal DateTimeOffset? ReadAsDateTimeOffsetInternal()
		{
			_readType = ReadType.ReadAsDateTimeOffset;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Date:
					if (Value is DateTime)
					{
						SetToken(JsonToken.Date, new DateTimeOffset((DateTime)Value), updateIndex: false);
					}
					return (DateTimeOffset)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					DateTimeOffset dateTimeOffset;
					if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTimeOffset, DateTimeZoneHandling, _dateFormatString, Culture, out var dt))
					{
						dateTimeOffset = (DateTimeOffset)dt;
						SetToken(JsonToken.Date, dateTimeOffset, updateIndex: false);
						return dateTimeOffset;
					}
					if (DateTimeOffset.TryParse(text, Culture, DateTimeStyles.RoundtripKind, out dateTimeOffset))
					{
						SetToken(JsonToken.Date, dateTimeOffset, updateIndex: false);
						return dateTimeOffset;
					}
					throw JsonReaderException.Create(this, "Could not convert string to DateTimeOffset: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal byte[] ReadAsBytesInternal()
		{
			_readType = ReadType.ReadAsBytes;
			JsonToken tokenType;
			do
			{
				if (!ReadInternal())
				{
					SetToken(JsonToken.None);
					return null;
				}
				tokenType = TokenType;
			}
			while (tokenType == JsonToken.Comment);
			if (IsWrappedInTypeObject())
			{
				byte[] array = ReadAsBytes();
				ReadInternal();
				SetToken(JsonToken.Bytes, array, updateIndex: false);
				return array;
			}
			switch (tokenType)
			{
			case JsonToken.String:
			{
				string text = (string)Value;
				Guid g;
				byte[] array3 = ((text.Length == 0) ? new byte[0] : ((!ConvertUtils.TryConvertGuid(text, out g)) ? Convert.FromBase64String(text) : g.ToByteArray()));
				SetToken(JsonToken.Bytes, array3, updateIndex: false);
				return array3;
			}
			case JsonToken.Null:
				return null;
			case JsonToken.Bytes:
				if (ValueType == typeof(Guid))
				{
					byte[] array4 = ((Guid)Value).ToByteArray();
					SetToken(JsonToken.Bytes, array4, updateIndex: false);
					return array4;
				}
				return (byte[])Value;
			case JsonToken.StartArray:
			{
				List<byte> list = new List<byte>();
				while (ReadInternal())
				{
					tokenType = TokenType;
					switch (tokenType)
					{
					case JsonToken.Integer:
						list.Add(Convert.ToByte(Value, CultureInfo.InvariantCulture));
						break;
					case JsonToken.EndArray:
					{
						byte[] array2 = list.ToArray();
						SetToken(JsonToken.Bytes, array2, updateIndex: false);
						return array2;
					}
					default:
						throw JsonReaderException.Create(this, "Unexpected token when reading bytes: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
					case JsonToken.Comment:
						break;
					}
				}
				throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
			}
			case JsonToken.EndArray:
				return null;
			default:
				throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
		}

		internal decimal? ReadAsDecimalInternal()
		{
			_readType = ReadType.ReadAsDecimal;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
					if (!(Value is decimal))
					{
						SetToken(JsonToken.Float, Convert.ToDecimal(Value, CultureInfo.InvariantCulture), updateIndex: false);
					}
					return (decimal)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					if (decimal.TryParse(text, NumberStyles.Number, Culture, out var result))
					{
						SetToken(JsonToken.Float, result, updateIndex: false);
						return result;
					}
					throw JsonReaderException.Create(this, "Could not convert string to decimal: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading decimal. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal int? ReadAsInt32Internal()
		{
			_readType = ReadType.ReadAsInt32;
			while (ReadInternal())
			{
				switch (TokenType)
				{
				case JsonToken.Comment:
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
					if (!(Value is int))
					{
						SetToken(JsonToken.Integer, Convert.ToInt32(Value, CultureInfo.InvariantCulture), updateIndex: false);
					}
					return (int)Value;
				case JsonToken.Null:
					return null;
				case JsonToken.String:
				{
					string text = (string)Value;
					if (string.IsNullOrEmpty(text))
					{
						SetToken(JsonToken.Null);
						return null;
					}
					if (int.TryParse(text, NumberStyles.Integer, Culture, out var result))
					{
						SetToken(JsonToken.Integer, result, updateIndex: false);
						return result;
					}
					throw JsonReaderException.Create(this, "Could not convert string to integer: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
				}
				case JsonToken.EndArray:
					return null;
				default:
					throw JsonReaderException.Create(this, "Error reading integer. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
				}
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal string ReadAsStringInternal()
		{
			_readType = ReadType.ReadAsString;
			while (ReadInternal())
			{
				JsonToken tokenType = TokenType;
				switch (tokenType)
				{
				case JsonToken.Comment:
					continue;
				case JsonToken.String:
					return (string)Value;
				case JsonToken.Null:
					return null;
				}
				if (JsonTokenUtils.IsPrimitiveToken(tokenType) && Value != null)
				{
					string text = ((!(Value is IFormattable)) ? Value.ToString() : ((IFormattable)Value).ToString(null, Culture));
					SetToken(JsonToken.String, text, updateIndex: false);
					return text;
				}
				if (tokenType == JsonToken.EndArray)
				{
					return null;
				}
				throw JsonReaderException.Create(this, "Error reading string. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
			SetToken(JsonToken.None);
			return null;
		}

		internal DateTime? ReadAsDateTimeInternal()
		{
			_readType = ReadType.ReadAsDateTime;
			do
			{
				if (!ReadInternal())
				{
					SetToken(JsonToken.None);
					return null;
				}
			}
			while (TokenType == JsonToken.Comment);
			if (TokenType == JsonToken.Date)
			{
				return (DateTime)Value;
			}
			if (TokenType == JsonToken.Null)
			{
				return null;
			}
			if (TokenType == JsonToken.String)
			{
				string text = (string)Value;
				if (string.IsNullOrEmpty(text))
				{
					SetToken(JsonToken.Null);
					return null;
				}
				DateTime value;
				if (DateTimeUtils.TryParseDateTime(text, DateParseHandling.DateTime, DateTimeZoneHandling, _dateFormatString, Culture, out var dt))
				{
					value = (DateTime)dt;
					value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);
					SetToken(JsonToken.Date, value, updateIndex: false);
					return value;
				}
				if (DateTime.TryParse(text, Culture, DateTimeStyles.RoundtripKind, out value))
				{
					value = DateTimeUtils.EnsureDateTime(value, DateTimeZoneHandling);
					SetToken(JsonToken.Date, value, updateIndex: false);
					return value;
				}
				throw JsonReaderException.Create(this, "Could not convert string to DateTime: {0}.".FormatWith(CultureInfo.InvariantCulture, Value));
			}
			if (TokenType == JsonToken.EndArray)
			{
				return null;
			}
			throw JsonReaderException.Create(this, "Error reading date. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, TokenType));
		}

		private bool IsWrappedInTypeObject()
		{
			_readType = ReadType.Read;
			if (TokenType == JsonToken.StartObject)
			{
				if (!ReadInternal())
				{
					throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
				}
				if (Value.ToString() == "$type")
				{
					ReadInternal();
					if (Value != null && Value.ToString().StartsWith("System.Byte[]", StringComparison.Ordinal))
					{
						ReadInternal();
						if (Value.ToString() == "$value")
						{
							return true;
						}
					}
				}
				throw JsonReaderException.Create(this, "Error reading bytes. Unexpected token: {0}.".FormatWith(CultureInfo.InvariantCulture, JsonToken.StartObject));
			}
			return false;
		}

		public void Skip()
		{
			if (TokenType == JsonToken.PropertyName)
			{
				Read();
			}
			if (JsonTokenUtils.IsStartToken(TokenType))
			{
				int depth = Depth;
				while (Read() && depth < Depth)
				{
				}
			}
		}

		protected void SetToken(JsonToken newToken)
		{
			SetToken(newToken, null, updateIndex: true);
		}

		protected void SetToken(JsonToken newToken, object value)
		{
			SetToken(newToken, value, updateIndex: true);
		}

		internal void SetToken(JsonToken newToken, object value, bool updateIndex)
		{
			_tokenType = newToken;
			_value = value;
			switch (newToken)
			{
			case JsonToken.StartObject:
				_currentState = State.ObjectStart;
				Push(JsonContainerType.Object);
				break;
			case JsonToken.StartArray:
				_currentState = State.ArrayStart;
				Push(JsonContainerType.Array);
				break;
			case JsonToken.StartConstructor:
				_currentState = State.ConstructorStart;
				Push(JsonContainerType.Constructor);
				break;
			case JsonToken.EndObject:
				ValidateEnd(JsonToken.EndObject);
				break;
			case JsonToken.EndArray:
				ValidateEnd(JsonToken.EndArray);
				break;
			case JsonToken.EndConstructor:
				ValidateEnd(JsonToken.EndConstructor);
				break;
			case JsonToken.PropertyName:
				_currentState = State.Property;
				_currentPosition.PropertyName = (string)value;
				break;
			case JsonToken.Raw:
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				SetPostValueState(updateIndex);
				break;
			case JsonToken.Comment:
				break;
			}
		}

		internal void SetPostValueState(bool updateIndex)
		{
			if (Peek() != 0)
			{
				_currentState = State.PostValue;
			}
			else
			{
				SetFinished();
			}
			if (updateIndex)
			{
				UpdateScopeWithFinishedValue();
			}
		}

		private void UpdateScopeWithFinishedValue()
		{
			if (_currentPosition.HasIndex)
			{
				_currentPosition.Position++;
			}
		}

		private void ValidateEnd(JsonToken endToken)
		{
			JsonContainerType jsonContainerType = Pop();
			if (GetTypeForCloseToken(endToken) != jsonContainerType)
			{
				throw JsonReaderException.Create(this, "JsonToken {0} is not valid for closing JsonType {1}.".FormatWith(CultureInfo.InvariantCulture, endToken, jsonContainerType));
			}
			if (Peek() != 0)
			{
				_currentState = State.PostValue;
			}
			else
			{
				SetFinished();
			}
		}

		protected void SetStateBasedOnCurrent()
		{
			JsonContainerType jsonContainerType = Peek();
			switch (jsonContainerType)
			{
			case JsonContainerType.Object:
				_currentState = State.Object;
				break;
			case JsonContainerType.Array:
				_currentState = State.Array;
				break;
			case JsonContainerType.Constructor:
				_currentState = State.Constructor;
				break;
			case JsonContainerType.None:
				SetFinished();
				break;
			default:
				throw JsonReaderException.Create(this, "While setting the reader state back to current object an unexpected JsonType was encountered: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContainerType));
			}
		}

		private void SetFinished()
		{
			if (SupportMultipleContent)
			{
				_currentState = State.Start;
			}
			else
			{
				_currentState = State.Finished;
			}
		}

		private JsonContainerType GetTypeForCloseToken(JsonToken token)
		{
			return token switch
			{
				JsonToken.EndObject => JsonContainerType.Object, 
				JsonToken.EndArray => JsonContainerType.Array, 
				JsonToken.EndConstructor => JsonContainerType.Constructor, 
				_ => throw JsonReaderException.Create(this, "Not a valid close JsonToken: {0}".FormatWith(CultureInfo.InvariantCulture, token)), 
			};
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_currentState != State.Closed && disposing)
			{
				Close();
			}
		}

		public virtual void Close()
		{
			_currentState = State.Closed;
			_tokenType = JsonToken.None;
			_value = null;
		}
	}
}
namespace Newtonsoft.Json.Bson
{
	public class BsonReader : JsonReader
	{
		private enum BsonReaderState
		{
			Normal,
			ReferenceStart,
			ReferenceRef,
			ReferenceId,
			CodeWScopeStart,
			CodeWScopeCode,
			CodeWScopeScope,
			CodeWScopeScopeObject,
			CodeWScopeScopeEnd
		}

		private class ContainerContext
		{
			public readonly BsonType Type;

			public int Length;

			public int Position;

			public ContainerContext(BsonType type)
			{
				Type = type;
			}
		}

		private const int MaxCharBytesSize = 128;

		private static readonly byte[] SeqRange1 = new byte[2] { 0, 127 };

		private static readonly byte[] SeqRange2 = new byte[2] { 194, 223 };

		private static readonly byte[] SeqRange3 = new byte[2] { 224, 239 };

		private static readonly byte[] SeqRange4 = new byte[2] { 240, 244 };

		private readonly BinaryReader _reader;

		private readonly List<ContainerContext> _stack;

		private byte[] _byteBuffer;

		private char[] _charBuffer;

		private BsonType _currentElementType;

		private BsonReaderState _bsonReaderState;

		private ContainerContext _currentContext;

		private bool _readRootValueAsArray;

		private bool _jsonNet35BinaryCompatibility;

		private DateTimeKind _dateTimeKindHandling;

		[Obsolete("JsonNet35BinaryCompatibility will be removed in a future version of Json.NET.")]
		public bool JsonNet35BinaryCompatibility
		{
			get
			{
				return _jsonNet35BinaryCompatibility;
			}
			set
			{
				_jsonNet35BinaryCompatibility = value;
			}
		}

		public bool ReadRootValueAsArray
		{
			get
			{
				return _readRootValueAsArray;
			}
			set
			{
				_readRootValueAsArray = value;
			}
		}

		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return _dateTimeKindHandling;
			}
			set
			{
				_dateTimeKindHandling = value;
			}
		}

		public BsonReader(Stream stream)
			: this(stream, readRootValueAsArray: false, DateTimeKind.Local)
		{
		}

		public BsonReader(BinaryReader reader)
			: this(reader, readRootValueAsArray: false, DateTimeKind.Local)
		{
		}

		public BsonReader(Stream stream, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			_reader = new BinaryReader(stream);
			_stack = new List<ContainerContext>();
			_readRootValueAsArray = readRootValueAsArray;
			_dateTimeKindHandling = dateTimeKindHandling;
		}

		public BsonReader(BinaryReader reader, bool readRootValueAsArray, DateTimeKind dateTimeKindHandling)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			_reader = reader;
			_stack = new List<ContainerContext>();
			_readRootValueAsArray = readRootValueAsArray;
			_dateTimeKindHandling = dateTimeKindHandling;
		}

		private string ReadElement()
		{
			_currentElementType = ReadType();
			return ReadString();
		}

		public override byte[] ReadAsBytes()
		{
			return ReadAsBytesInternal();
		}

		public override decimal? ReadAsDecimal()
		{
			return ReadAsDecimalInternal();
		}

		public override int? ReadAsInt32()
		{
			return ReadAsInt32Internal();
		}

		public override string ReadAsString()
		{
			return ReadAsStringInternal();
		}

		public override DateTime? ReadAsDateTime()
		{
			return ReadAsDateTimeInternal();
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return ReadAsDateTimeOffsetInternal();
		}

		public override bool Read()
		{
			_readType = Newtonsoft.Json.ReadType.Read;
			return ReadInternal();
		}

		internal override bool ReadInternal()
		{
			try
			{
				bool flag;
				switch (_bsonReaderState)
				{
				case BsonReaderState.Normal:
					flag = ReadNormal();
					break;
				case BsonReaderState.ReferenceStart:
				case BsonReaderState.ReferenceRef:
				case BsonReaderState.ReferenceId:
					flag = ReadReference();
					break;
				case BsonReaderState.CodeWScopeStart:
				case BsonReaderState.CodeWScopeCode:
				case BsonReaderState.CodeWScopeScope:
				case BsonReaderState.CodeWScopeScopeObject:
				case BsonReaderState.CodeWScopeScopeEnd:
					flag = ReadCodeWScope();
					break;
				default:
					throw JsonReaderException.Create(this, "Unexpected state: {0}".FormatWith(CultureInfo.InvariantCulture, _bsonReaderState));
				}
				if (!flag)
				{
					SetToken(JsonToken.None);
					return false;
				}
				return true;
			}
			catch (EndOfStreamException)
			{
				SetToken(JsonToken.None);
				return false;
			}
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput && _reader != null)
			{
				_reader.Close();
			}
		}

		private bool ReadCodeWScope()
		{
			switch (_bsonReaderState)
			{
			case BsonReaderState.CodeWScopeStart:
				SetToken(JsonToken.PropertyName, "$code");
				_bsonReaderState = BsonReaderState.CodeWScopeCode;
				return true;
			case BsonReaderState.CodeWScopeCode:
				ReadInt32();
				SetToken(JsonToken.String, ReadLengthString());
				_bsonReaderState = BsonReaderState.CodeWScopeScope;
				return true;
			case BsonReaderState.CodeWScopeScope:
			{
				if (base.CurrentState == State.PostValue)
				{
					SetToken(JsonToken.PropertyName, "$scope");
					return true;
				}
				SetToken(JsonToken.StartObject);
				_bsonReaderState = BsonReaderState.CodeWScopeScopeObject;
				ContainerContext containerContext = new ContainerContext(BsonType.Object);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				return true;
			}
			case BsonReaderState.CodeWScopeScopeObject:
			{
				bool flag = ReadNormal();
				if (flag && TokenType == JsonToken.EndObject)
				{
					_bsonReaderState = BsonReaderState.CodeWScopeScopeEnd;
				}
				return flag;
			}
			case BsonReaderState.CodeWScopeScopeEnd:
				SetToken(JsonToken.EndObject);
				_bsonReaderState = BsonReaderState.Normal;
				return true;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private bool ReadReference()
		{
			switch (base.CurrentState)
			{
			case State.ObjectStart:
				SetToken(JsonToken.PropertyName, "$ref");
				_bsonReaderState = BsonReaderState.ReferenceRef;
				return true;
			case State.Property:
				if (_bsonReaderState == BsonReaderState.ReferenceRef)
				{
					SetToken(JsonToken.String, ReadLengthString());
					return true;
				}
				if (_bsonReaderState == BsonReaderState.ReferenceId)
				{
					SetToken(JsonToken.Bytes, ReadBytes(12));
					return true;
				}
				throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + _bsonReaderState);
			case State.PostValue:
				if (_bsonReaderState == BsonReaderState.ReferenceRef)
				{
					SetToken(JsonToken.PropertyName, "$id");
					_bsonReaderState = BsonReaderState.ReferenceId;
					return true;
				}
				if (_bsonReaderState == BsonReaderState.ReferenceId)
				{
					SetToken(JsonToken.EndObject);
					_bsonReaderState = BsonReaderState.Normal;
					return true;
				}
				throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + _bsonReaderState);
			default:
				throw JsonReaderException.Create(this, "Unexpected state when reading BSON reference: " + base.CurrentState);
			}
		}

		private bool ReadNormal()
		{
			switch (base.CurrentState)
			{
			case State.Start:
			{
				JsonToken token2 = ((!_readRootValueAsArray) ? JsonToken.StartObject : JsonToken.StartArray);
				BsonType type = ((!_readRootValueAsArray) ? BsonType.Object : BsonType.Array);
				SetToken(token2);
				ContainerContext containerContext = new ContainerContext(type);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				return true;
			}
			case State.Complete:
			case State.Closed:
				return false;
			case State.Property:
				ReadType(_currentElementType);
				return true;
			case State.ObjectStart:
			case State.ArrayStart:
			case State.PostValue:
			{
				ContainerContext currentContext = _currentContext;
				if (currentContext == null)
				{
					return false;
				}
				int num = currentContext.Length - 1;
				if (currentContext.Position < num)
				{
					if (currentContext.Type == BsonType.Array)
					{
						ReadElement();
						ReadType(_currentElementType);
						return true;
					}
					SetToken(JsonToken.PropertyName, ReadElement());
					return true;
				}
				if (currentContext.Position == num)
				{
					if (ReadByte() != 0)
					{
						throw JsonReaderException.Create(this, "Unexpected end of object byte value.");
					}
					PopContext();
					if (_currentContext != null)
					{
						MovePosition(currentContext.Length);
					}
					JsonToken token = ((currentContext.Type == BsonType.Object) ? JsonToken.EndObject : JsonToken.EndArray);
					SetToken(token);
					return true;
				}
				throw JsonReaderException.Create(this, "Read past end of current container context.");
			}
			default:
				throw new ArgumentOutOfRangeException();
			case State.ConstructorStart:
			case State.Constructor:
			case State.Error:
			case State.Finished:
				return false;
			}
		}

		private void PopContext()
		{
			_stack.RemoveAt(_stack.Count - 1);
			if (_stack.Count == 0)
			{
				_currentContext = null;
			}
			else
			{
				_currentContext = _stack[_stack.Count - 1];
			}
		}

		private void PushContext(ContainerContext newContext)
		{
			_stack.Add(newContext);
			_currentContext = newContext;
		}

		private byte ReadByte()
		{
			MovePosition(1);
			return _reader.ReadByte();
		}

		private void ReadType(BsonType type)
		{
			switch (type)
			{
			case BsonType.Number:
			{
				double num = ReadDouble();
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					SetToken(JsonToken.Float, Convert.ToDecimal(num, CultureInfo.InvariantCulture));
				}
				else
				{
					SetToken(JsonToken.Float, num);
				}
				break;
			}
			case BsonType.String:
			case BsonType.Symbol:
				SetToken(JsonToken.String, ReadLengthString());
				break;
			case BsonType.Object:
			{
				SetToken(JsonToken.StartObject);
				ContainerContext containerContext2 = new ContainerContext(BsonType.Object);
				PushContext(containerContext2);
				containerContext2.Length = ReadInt32();
				break;
			}
			case BsonType.Array:
			{
				SetToken(JsonToken.StartArray);
				ContainerContext containerContext = new ContainerContext(BsonType.Array);
				PushContext(containerContext);
				containerContext.Length = ReadInt32();
				break;
			}
			case BsonType.Binary:
			{
				BsonBinaryType binaryType;
				byte[] array = ReadBinary(out binaryType);
				object value3 = ((binaryType != BsonBinaryType.Uuid) ? array : ((object)new Guid(array)));
				SetToken(JsonToken.Bytes, value3);
				break;
			}
			case BsonType.Undefined:
				SetToken(JsonToken.Undefined);
				break;
			case BsonType.Oid:
			{
				byte[] value2 = ReadBytes(12);
				SetToken(JsonToken.Bytes, value2);
				break;
			}
			case BsonType.Boolean:
			{
				bool flag = Convert.ToBoolean(ReadByte());
				SetToken(JsonToken.Boolean, flag);
				break;
			}
			case BsonType.Date:
			{
				long javaScriptTicks = ReadInt64();
				DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
				SetToken(JsonToken.Date, DateTimeKindHandling switch
				{
					DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), 
					DateTimeKind.Local => dateTime.ToLocalTime(), 
					_ => dateTime, 
				});
				break;
			}
			case BsonType.Null:
				SetToken(JsonToken.Null);
				break;
			case BsonType.Regex:
			{
				string text = ReadString();
				string text2 = ReadString();
				string value = "/" + text + "/" + text2;
				SetToken(JsonToken.String, value);
				break;
			}
			case BsonType.Reference:
				SetToken(JsonToken.StartObject);
				_bsonReaderState = BsonReaderState.ReferenceStart;
				break;
			case BsonType.Code:
				SetToken(JsonToken.String, ReadLengthString());
				break;
			case BsonType.CodeWScope:
				SetToken(JsonToken.StartObject);
				_bsonReaderState = BsonReaderState.CodeWScopeStart;
				break;
			case BsonType.Integer:
				SetToken(JsonToken.Integer, (long)ReadInt32());
				break;
			case BsonType.TimeStamp:
			case BsonType.Long:
				SetToken(JsonToken.Integer, ReadInt64());
				break;
			default:
				throw new ArgumentOutOfRangeException("type", "Unexpected BsonType value: " + type);
			}
		}

		private byte[] ReadBinary(out BsonBinaryType binaryType)
		{
			int count = ReadInt32();
			binaryType = (BsonBinaryType)ReadByte();
			if (binaryType == BsonBinaryType.BinaryOld && !_jsonNet35BinaryCompatibility)
			{
				count = ReadInt32();
			}
			return ReadBytes(count);
		}

		private string ReadString()
		{
			EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			while (true)
			{
				int num3 = num2;
				byte b;
				while (num3 < 128 && (b = _reader.ReadByte()) > 0)
				{
					_byteBuffer[num3++] = b;
				}
				int num4 = num3 - num2;
				num += num4;
				if (num3 < 128 && stringBuilder == null)
				{
					int chars = Encoding.UTF8.GetChars(_byteBuffer, 0, num4, _charBuffer, 0);
					MovePosition(num + 1);
					return new string(_charBuffer, 0, chars);
				}
				int lastFullCharStop = GetLastFullCharStop(num3 - 1);
				int chars2 = Encoding.UTF8.GetChars(_byteBuffer, 0, lastFullCharStop + 1, _charBuffer, 0);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(256);
				}
				stringBuilder.Append(_charBuffer, 0, chars2);
				if (lastFullCharStop < num4 - 1)
				{
					num2 = num4 - lastFullCharStop - 1;
					Array.Copy(_byteBuffer, lastFullCharStop + 1, _byteBuffer, 0, num2);
					continue;
				}
				if (num3 < 128)
				{
					break;
				}
				num2 = 0;
			}
			MovePosition(num + 1);
			return stringBuilder.ToString();
		}

		private string ReadLengthString()
		{
			int num = ReadInt32();
			MovePosition(num);
			string @string = GetString(num - 1);
			_reader.ReadByte();
			return @string;
		}

		private string GetString(int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			EnsureBuffers();
			StringBuilder stringBuilder = null;
			int num = 0;
			int num2 = 0;
			do
			{
				int count = ((length - num > 128 - num2) ? (128 - num2) : (length - num));
				int num3 = _reader.Read(_byteBuffer, num2, count);
				if (num3 == 0)
				{
					throw new EndOfStreamException("Unable to read beyond the end of the stream.");
				}
				num += num3;
				num3 += num2;
				if (num3 == length)
				{
					int chars = Encoding.UTF8.GetChars(_byteBuffer, 0, num3, _charBuffer, 0);
					return new string(_charBuffer, 0, chars);
				}
				int lastFullCharStop = GetLastFullCharStop(num3 - 1);
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length);
				}
				int chars2 = Encoding.UTF8.GetChars(_byteBuffer, 0, lastFullCharStop + 1, _charBuffer, 0);
				stringBuilder.Append(_charBuffer, 0, chars2);
				if (lastFullCharStop < num3 - 1)
				{
					num2 = num3 - lastFullCharStop - 1;
					Array.Copy(_byteBuffer, lastFullCharStop + 1, _byteBuffer, 0, num2);
				}
				else
				{
					num2 = 0;
				}
			}
			while (num < length);
			return stringBuilder.ToString();
		}

		private int GetLastFullCharStop(int start)
		{
			int num = start;
			int num2 = 0;
			for (; num >= 0; num--)
			{
				num2 = BytesInSequence(_byteBuffer[num]);
				switch (num2)
				{
				case 0:
					continue;
				default:
					num--;
					break;
				case 1:
					break;
				}
				break;
			}
			if (num2 == start - num)
			{
				return start;
			}
			return num;
		}

		private int BytesInSequence(byte b)
		{
			if (b <= SeqRange1[1])
			{
				return 1;
			}
			if (b >= SeqRange2[0] && b <= SeqRange2[1])
			{
				return 2;
			}
			if (b >= SeqRange3[0] && b <= SeqRange3[1])
			{
				return 3;
			}
			if (b >= SeqRange4[0] && b <= SeqRange4[1])
			{
				return 4;
			}
			return 0;
		}

		private void EnsureBuffers()
		{
			if (_byteBuffer == null)
			{
				_byteBuffer = new byte[128];
			}
			if (_charBuffer == null)
			{
				int maxCharCount = Encoding.UTF8.GetMaxCharCount(128);
				_charBuffer = new char[maxCharCount];
			}
		}

		private double ReadDouble()
		{
			MovePosition(8);
			return _reader.ReadDouble();
		}

		private int ReadInt32()
		{
			MovePosition(4);
			return _reader.ReadInt32();
		}

		private long ReadInt64()
		{
			MovePosition(8);
			return _reader.ReadInt64();
		}

		private BsonType ReadType()
		{
			MovePosition(1);
			return (BsonType)_reader.ReadSByte();
		}

		private void MovePosition(int count)
		{
			_currentContext.Position += count;
		}

		private byte[] ReadBytes(int count)
		{
			MovePosition(count);
			return _reader.ReadBytes(count);
		}
	}
	internal abstract class BsonToken
	{
		public abstract BsonType Type { get; }

		public BsonToken Parent { get; set; }

		public int CalculatedSize { get; set; }
	}
	internal class BsonObject : BsonToken, IEnumerable<BsonProperty>, IEnumerable
	{
		private readonly List<BsonProperty> _children = new List<BsonProperty>();

		public override BsonType Type => BsonType.Object;

		public void Add(string name, BsonToken token)
		{
			_children.Add(new BsonProperty
			{
				Name = new BsonString(name, includeLength: false),
				Value = token
			});
			token.Parent = this;
		}

		public IEnumerator<BsonProperty> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	internal class BsonArray : BsonToken, IEnumerable<BsonToken>, IEnumerable
	{
		private readonly List<BsonToken> _children = new List<BsonToken>();

		public override BsonType Type => BsonType.Array;

		public void Add(BsonToken token)
		{
			_children.Add(token);
			token.Parent = this;
		}

		public IEnumerator<BsonToken> GetEnumerator()
		{
			return _children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	internal class BsonValue : BsonToken
	{
		private readonly object _value;

		private readonly BsonType _type;

		public object Value => _value;

		public override BsonType Type => _type;

		public BsonValue(object value, BsonType type)
		{
			_value = value;
			_type = type;
		}
	}
	internal class BsonString : BsonValue
	{
		public int ByteCount { get; set; }

		public bool IncludeLength { get; set; }

		public BsonString(object value, bool includeLength)
			: base(value, BsonType.String)
		{
			IncludeLength = includeLength;
		}
	}
	internal class BsonBinary : BsonValue
	{
		public BsonBinaryType BinaryType { get; set; }

		public BsonBinary(byte[] value, BsonBinaryType binaryType)
			: base(value, BsonType.Binary)
		{
			BinaryType = binaryType;
		}
	}
	internal class BsonRegex : BsonToken
	{
		public BsonString Pattern { get; set; }

		public BsonString Options { get; set; }

		public override BsonType Type => BsonType.Regex;

		public BsonRegex(string pattern, string options)
		{
			Pattern = new BsonString(pattern, includeLength: false);
			Options = new BsonString(options, includeLength: false);
		}
	}
	internal class BsonProperty
	{
		public BsonString Name { get; set; }

		public BsonToken Value { get; set; }
	}
	internal enum BsonType : sbyte
	{
		Number = 1,
		String = 2,
		Object = 3,
		Array = 4,
		Binary = 5,
		Undefined = 6,
		Oid = 7,
		Boolean = 8,
		Date = 9,
		Null = 10,
		Regex = 11,
		Reference = 12,
		Code = 13,
		Symbol = 14,
		CodeWScope = 15,
		Integer = 16,
		TimeStamp = 17,
		Long = 18,
		MinKey = -1,
		MaxKey = sbyte.MaxValue
	}
}
namespace Newtonsoft.Json
{
	public abstract class JsonWriter : IDisposable
	{
		internal enum State
		{
			Start,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			ConstructorStart,
			Constructor,
			Closed,
			Error
		}

		private static readonly State[][] StateArray;

		internal static readonly State[][] StateArrayTempate;

		private readonly List<JsonPosition> _stack;

		private JsonPosition _currentPosition;

		private State _currentState;

		private Formatting _formatting;

		private DateFormatHandling _dateFormatHandling;

		private DateTimeZoneHandling _dateTimeZoneHandling;

		private StringEscapeHandling _stringEscapeHandling;

		private FloatFormatHandling _floatFormatHandling;

		private string _dateFormatString;

		private CultureInfo _culture;

		public bool CloseOutput { get; set; }

		protected internal int Top
		{
			get
			{
				int num = _stack.Count;
				if (Peek() != 0)
				{
					num++;
				}
				return num;
			}
		}

		public WriteState WriteState
		{
			get
			{
				switch (_currentState)
				{
				case State.Error:
					return WriteState.Error;
				case State.Closed:
					return WriteState.Closed;
				case State.ObjectStart:
				case State.Object:
					return WriteState.Object;
				case State.ArrayStart:
				case State.Array:
					return WriteState.Array;
				case State.ConstructorStart:
				case State.Constructor:
					return WriteState.Constructor;
				case State.Property:
					return WriteState.Property;
				case State.Start:
					return WriteState.Start;
				default:
					throw JsonWriterException.Create(this, "Invalid state: " + _currentState, null);
				}
			}
		}

		internal string ContainerPath
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				return JsonPosition.BuildPath(_stack);
			}
		}

		public string Path
		{
			get
			{
				if (_currentPosition.Type == JsonContainerType.None)
				{
					return string.Empty;
				}
				IEnumerable<JsonPosition> positions = ((_currentState == State.ArrayStart || _currentState == State.ConstructorStart || _currentState == State.ObjectStart) ? _stack : _stack.Concat(new JsonPosition[1] { _currentPosition }));
				return JsonPosition.BuildPath(positions);
			}
		}

		public Formatting Formatting
		{
			get
			{
				return _formatting;
			}
			set
			{
				_formatting = value;
			}
		}

		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling;
			}
			set
			{
				_dateFormatHandling = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling;
			}
			set
			{
				_dateTimeZoneHandling = value;
			}
		}

		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _stringEscapeHandling;
			}
			set
			{
				_stringEscapeHandling = value;
				OnStringEscapeHandlingChanged();
			}
		}

		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _floatFormatHandling;
			}
			set
			{
				_floatFormatHandling = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString;
			}
			set
			{
				_dateFormatString = value;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? CultureInfo.InvariantCulture;
			}
			set
			{
				_culture = value;
			}
		}

		internal static State[][] BuildStateArray()
		{
			List<State[]> list = StateArrayTempate.ToList();
			State[] item = StateArrayTempate[0];
			State[] item2 = StateArrayTempate[7];
			foreach (JsonToken value in EnumUtils.GetValues(typeof(JsonToken)))
			{
				if (list.Count <= (int)value)
				{
					switch (value)
					{
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.String:
					case JsonToken.Boolean:
					case JsonToken.Null:
					case JsonToken.Undefined:
					case JsonToken.Date:
					case JsonToken.Bytes:
						list.Add(item2);
						break;
					default:
						list.Add(item);
						break;
					}
				}
			}
			return list.ToArray();
		}

		static JsonWriter()
		{
			StateArrayTempate = new State[8][]
			{
				new State[10]
				{
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ObjectStart,
					State.ObjectStart,
					State.Error,
					State.Error,
					State.ObjectStart,
					State.ObjectStart,
					State.ObjectStart,
					State.ObjectStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ArrayStart,
					State.ArrayStart,
					State.Error,
					State.Error,
					State.ArrayStart,
					State.ArrayStart,
					State.ArrayStart,
					State.ArrayStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.ConstructorStart,
					State.ConstructorStart,
					State.Error,
					State.Error,
					State.ConstructorStart,
					State.ConstructorStart,
					State.ConstructorStart,
					State.ConstructorStart,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Property,
					State.Error,
					State.Property,
					State.Property,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Property,
					State.ObjectStart,
					State.Object,
					State.ArrayStart,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Property,
					State.ObjectStart,
					State.Object,
					State.ArrayStart,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				},
				new State[10]
				{
					State.Start,
					State.Object,
					State.Error,
					State.Error,
					State.Array,
					State.Array,
					State.Constructor,
					State.Constructor,
					State.Error,
					State.Error
				}
			};
			StateArray = BuildStateArray();
		}

		internal virtual void OnStringEscapeHandlingChanged()
		{
		}

		protected JsonWriter()
		{
			_stack = new List<JsonPosition>(4);
			_currentState = State.Start;
			_formatting = Formatting.None;
			_dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
			CloseOutput = true;
		}

		internal void UpdateScopeWithFinishedValue()
		{
			if (_currentPosition.HasIndex)
			{
				_currentPosition.Position++;
			}
		}

		private void Push(JsonContainerType value)
		{
			if (_currentPosition.Type != 0)
			{
				_stack.Add(_currentPosition);
			}
			_currentPosition = new JsonPosition(value);
		}

		private JsonContainerType Pop()
		{
			JsonPosition currentPosition = _currentPosition;
			if (_stack.Count > 0)
			{
				_currentPosition = _stack[_stack.Count - 1];
				_stack.RemoveAt(_stack.Count - 1);
			}
			else
			{
				_currentPosition = default(JsonPosition);
			}
			return currentPosition.Type;
		}

		private JsonContainerType Peek()
		{
			return _currentPosition.Type;
		}

		public abstract void Flush();

		public virtual void Close()
		{
			AutoCompleteAll();
		}

		public virtual void WriteStartObject()
		{
			InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
		}

		public virtual void WriteEndObject()
		{
			InternalWriteEnd(JsonContainerType.Object);
		}

		public virtual void WriteStartArray()
		{
			InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
		}

		public virtual void WriteEndArray()
		{
			InternalWriteEnd(JsonContainerType.Array);
		}

		public virtual void WriteStartConstructor(string name)
		{
			InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
		}

		public virtual void WriteEndConstructor()
		{
			InternalWriteEnd(JsonContainerType.Constructor);
		}

		public virtual void WritePropertyName(string name)
		{
			InternalWritePropertyName(name);
		}

		public virtual void WritePropertyName(string name, bool escape)
		{
			WritePropertyName(name);
		}

		public virtual void WriteEnd()
		{
			WriteEnd(Peek());
		}

		public void WriteToken(JsonReader reader)
		{
			WriteToken(reader, writeChildren: true, writeDateConstructorAsDate: true);
		}

		public void WriteToken(JsonReader reader, bool writeChildren)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			WriteToken(reader, writeChildren, writeDateConstructorAsDate: true);
		}

		public void WriteToken(JsonToken token, object value)
		{
			WriteTokenInternal(token, value);
		}

		public void WriteToken(JsonToken token)
		{
			WriteTokenInternal(token, null);
		}

		internal void WriteToken(JsonReader reader, bool writeChildren, bool writeDateConstructorAsDate)
		{
			int initialDepth = ((reader.TokenType == JsonToken.None) ? (-1) : (JsonTokenUtils.IsStartToken(reader.TokenType) ? reader.Depth : (reader.Depth + 1)));
			WriteToken(reader, initialDepth, writeChildren, writeDateConstructorAsDate);
		}

		internal void WriteToken(JsonReader reader, int initialDepth, bool writeChildren, bool writeDateConstructorAsDate)
		{
			do
			{
				if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
				{
					WriteConstructorDate(reader);
				}
				else
				{
					WriteTokenInternal(reader.TokenType, reader.Value);
				}
			}
			while (initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren && reader.Read());
		}

		private void WriteTokenInternal(JsonToken tokenType, object value)
		{
			switch (tokenType)
			{
			case JsonToken.StartObject:
				WriteStartObject();
				break;
			case JsonToken.StartArray:
				WriteStartArray();
				break;
			case JsonToken.StartConstructor:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteStartConstructor(value.ToString());
				break;
			case JsonToken.PropertyName:
				ValidationUtils.ArgumentNotNull(value, "value");
				WritePropertyName(value.ToString());
				break;
			case JsonToken.Comment:
				WriteComment(value?.ToString());
				break;
			case JsonToken.Integer:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is BigInteger)
				{
					WriteValue((BigInteger)value);
				}
				else
				{
					WriteValue(Convert.ToInt64(value, CultureInfo.InvariantCulture));
				}
				break;
			case JsonToken.Float:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is decimal)
				{
					WriteValue((decimal)value);
				}
				else if (value is double)
				{
					WriteValue((double)value);
				}
				else if (value is float)
				{
					WriteValue((float)value);
				}
				else
				{
					WriteValue(Convert.ToDouble(value, CultureInfo.InvariantCulture));
				}
				break;
			case JsonToken.String:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteValue(value.ToString());
				break;
			case JsonToken.Boolean:
				ValidationUtils.ArgumentNotNull(value, "value");
				WriteValue(Convert.ToBoolean(value, CultureInfo.InvariantCulture));
				break;
			case JsonToken.Null:
				WriteNull();
				break;
			case JsonToken.Undefined:
				WriteUndefined();
				break;
			case JsonToken.EndObject:
				WriteEndObject();
				break;
			case JsonToken.EndArray:
				WriteEndArray();
				break;
			case JsonToken.EndConstructor:
				WriteEndConstructor();
				break;
			case JsonToken.Date:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is DateTimeOffset)
				{
					WriteValue((DateTimeOffset)value);
				}
				else
				{
					WriteValue(Convert.ToDateTime(value, CultureInfo.InvariantCulture));
				}
				break;
			case JsonToken.Raw:
				WriteRawValue(value?.ToString());
				break;
			case JsonToken.Bytes:
				ValidationUtils.ArgumentNotNull(value, "value");
				if (value is Guid)
				{
					WriteValue((Guid)value);
				}
				else
				{
					WriteValue((byte[])value);
				}
				break;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", tokenType, "Unexpected token type.");
			case JsonToken.None:
				break;
			}
		}

		private void WriteConstructorDate(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + reader.TokenType, null);
			}
			long javaScriptTicks = (long)reader.Value;
			DateTime value = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			if (!reader.Read())
			{
				throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", null);
			}
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + reader.TokenType, null);
			}
			WriteValue(value);
		}

		private void WriteEnd(JsonContainerType type)
		{
			switch (type)
			{
			case JsonContainerType.Object:
				WriteEndObject();
				break;
			case JsonContainerType.Array:
				WriteEndArray();
				break;
			case JsonContainerType.Constructor:
				WriteEndConstructor();
				break;
			default:
				throw JsonWriterException.Create(this, "Unexpected type when writing end: " + type, null);
			}
		}

		private void AutoCompleteAll()
		{
			while (Top > 0)
			{
				WriteEnd();
			}
		}

		private JsonToken GetCloseTokenForType(JsonContainerType type)
		{
			return type switch
			{
				JsonContainerType.Object => JsonToken.EndObject, 
				JsonContainerType.Array => JsonToken.EndArray, 
				JsonContainerType.Constructor => JsonToken.EndConstructor, 
				_ => throw JsonWriterException.Create(this, "No close token for type: " + type, null), 
			};
		}

		private void AutoCompleteClose(JsonContainerType type)
		{
			int num = 0;
			if (_currentPosition.Type == type)
			{
				num = 1;
			}
			else
			{
				int num2 = Top - 2;
				for (int num3 = num2; num3 >= 0; num3--)
				{
					int index = num2 - num3;
					if (_stack[index].Type == type)
					{
						num = num3 + 2;
						break;
					}
				}
			}
			if (num == 0)
			{
				throw JsonWriterException.Create(this, "No token to close.", null);
			}
			for (int i = 0; i < num; i++)
			{
				JsonToken closeTokenForType = GetCloseTokenForType(Pop());
				if (_currentState == State.Property)
				{
					WriteNull();
				}
				if (_formatting == Formatting.Indented && _currentState != State.ObjectStart && _currentState != State.ArrayStart)
				{
					WriteIndent();
				}
				WriteEnd(closeTokenForType);
				JsonContainerType jsonContainerType = Peek();
				switch (jsonContainerType)
				{
				case JsonContainerType.Object:
					_currentState = State.Object;
					break;
				case JsonContainerType.Array:
					_currentState = State.Array;
					break;
				case JsonContainerType.Constructor:
					_currentState = State.Array;
					break;
				case JsonContainerType.None:
					_currentState = State.Start;
					break;
				default:
					throw JsonWriterException.Create(this, "Unknown JsonType: " + jsonContainerType, null);
				}
			}
		}

		protected virtual void WriteEnd(JsonToken token)
		{
		}

		protected virtual void WriteIndent()
		{
		}

		protected virtual void WriteValueDelimiter()
		{
		}

		protected virtual void WriteIndentSpace()
		{
		}

		internal void AutoComplete(JsonToken tokenBeingWritten)
		{
			State state = StateArray[(int)tokenBeingWritten][(int)_currentState];
			if (state == State.Error)
			{
				throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith(CultureInfo.InvariantCulture, tokenBeingWritten.ToString(), _currentState.ToString()), null);
			}
			if ((_currentState == State.Object || _currentState == State.Array || _currentState == State.Constructor) && tokenBeingWritten != JsonToken.Comment)
			{
				WriteValueDelimiter();
			}
			if (_formatting == Formatting.Indented)
			{
				if (_currentState == State.Property)
				{
					WriteIndentSpace();
				}
				if (_currentState == State.Array || _currentState == State.ArrayStart || _currentState == State.Constructor || _currentState == State.ConstructorStart || (tokenBeingWritten == JsonToken.PropertyName && _currentState != 0))
				{
					WriteIndent();
				}
			}
			_currentState = state;
		}

		public virtual void WriteNull()
		{
			InternalWriteValue(JsonToken.Null);
		}

		public virtual void WriteUndefined()
		{
			InternalWriteValue(JsonToken.Undefined);
		}

		public virtual void WriteRaw(string json)
		{
			InternalWriteRaw();
		}

		public virtual void WriteRawValue(string json)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(JsonToken.Undefined);
			WriteRaw(json);
		}

		public virtual void WriteValue(string value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(long value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(float value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(double value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(bool value)
		{
			InternalWriteValue(JsonToken.Boolean);
		}

		public virtual void WriteValue(short value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(char value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(byte value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte value)
		{
			InternalWriteValue(JsonToken.Integer);
		}

		public virtual void WriteValue(decimal value)
		{
			InternalWriteValue(JsonToken.Float);
		}

		public virtual void WriteValue(DateTime value)
		{
			InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(DateTimeOffset value)
		{
			InternalWriteValue(JsonToken.Date);
		}

		public virtual void WriteValue(Guid value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(TimeSpan value)
		{
			InternalWriteValue(JsonToken.String);
		}

		public virtual void WriteValue(int? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(uint? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(long? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ulong? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(bool? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(short? value)
		{
			if (!((int?)value).HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(ushort? value)
		{
			if (!((int?)value).HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(char? value)
		{
			if (!((int?)value).HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(byte? value)
		{
			if (!((int?)value).HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		[CLSCompliant(false)]
		public virtual void WriteValue(sbyte? value)
		{
			if (!((int?)value).HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(decimal? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(DateTime? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(DateTimeOffset? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(Guid? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(TimeSpan? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
			}
			else
			{
				WriteValue(value.Value);
			}
		}

		public virtual void WriteValue(byte[] value)
		{
			if (value == null)
			{
				WriteNull();
			}
			else
			{
				InternalWriteValue(JsonToken.Bytes);
			}
		}

		public virtual void WriteValue(Uri value)
		{
			if (value == null)
			{
				WriteNull();
			}
			else
			{
				InternalWriteValue(JsonToken.String);
			}
		}

		public virtual void WriteValue(object value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			if (value is BigInteger)
			{
				throw CreateUnsupportedTypeException(this, value);
			}
			WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
		}

		public virtual void WriteComment(string text)
		{
			InternalWriteComment();
		}

		public virtual void WriteWhitespace(string ws)
		{
			InternalWriteWhitespace(ws);
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
		}

		private void Dispose(bool disposing)
		{
			if (_currentState != State.Closed)
			{
				Close();
			}
		}

		internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
		{
			switch (typeCode)
			{
			case PrimitiveTypeCode.Char:
				writer.WriteValue((char)value);
				return;
			case PrimitiveTypeCode.CharNullable:
				writer.WriteValue((value == null) ? null : new char?((char)value));
				return;
			case PrimitiveTypeCode.Boolean:
				writer.WriteValue((bool)value);
				return;
			case PrimitiveTypeCode.BooleanNullable:
				writer.WriteValue((value == null) ? null : new bool?((bool)value));
				return;
			case PrimitiveTypeCode.SByte:
				writer.WriteValue((sbyte)value);
				return;
			case PrimitiveTypeCode.SByteNullable:
				writer.WriteValue((value == null) ? null : new sbyte?((sbyte)value));
				return;
			case PrimitiveTypeCode.Int16:
				writer.WriteValue((short)value);
				return;
			case PrimitiveTypeCode.Int16Nullable:
				writer.WriteValue((value == null) ? null : new short?((short)value));
				return;
			case PrimitiveTypeCode.UInt16:
				writer.WriteValue((ushort)value);
				return;
			case PrimitiveTypeCode.UInt16Nullable:
				writer.WriteValue((value == null) ? null : new ushort?((ushort)value));
				return;
			case PrimitiveTypeCode.Int32:
				writer.WriteValue((int)value);
				return;
			case PrimitiveTypeCode.Int32Nullable:
				writer.WriteValue((value == null) ? null : new int?((int)value));
				return;
			case PrimitiveTypeCode.Byte:
				writer.WriteValue((byte)value);
				return;
			case PrimitiveTypeCode.ByteNullable:
				writer.WriteValue((value == null) ? null : new byte?((byte)value));
				return;
			case PrimitiveTypeCode.UInt32:
				writer.WriteValue((uint)value);
				return;
			case PrimitiveTypeCode.UInt32Nullable:
				writer.WriteValue((value == null) ? null : new uint?((uint)value));
				return;
			case PrimitiveTypeCode.Int64:
				writer.WriteValue((long)value);
				return;
			case PrimitiveTypeCode.Int64Nullable:
				writer.WriteValue((value == null) ? null : new long?((long)value));
				return;
			case PrimitiveTypeCode.UInt64:
				writer.WriteValue((ulong)value);
				return;
			case PrimitiveTypeCode.UInt64Nullable:
				writer.WriteValue((value == null) ? null : new ulong?((ulong)value));
				return;
			case PrimitiveTypeCode.Single:
				writer.WriteValue((float)value);
				return;
			case PrimitiveTypeCode.SingleNullable:
				writer.WriteValue((value == null) ? null : new float?((float)value));
				return;
			case PrimitiveTypeCode.Double:
				writer.WriteValue((double)value);
				return;
			case PrimitiveTypeCode.DoubleNullable:
				writer.WriteValue((value == null) ? null : new double?((double)value));
				return;
			case PrimitiveTypeCode.DateTime:
				writer.WriteValue((DateTime)value);
				return;
			case PrimitiveTypeCode.DateTimeNullable:
				writer.WriteValue((value == null) ? null : new DateTime?((DateTime)value));
				return;
			case PrimitiveTypeCode.DateTimeOffset:
				writer.WriteValue((DateTimeOffset)value);
				return;
			case PrimitiveTypeCode.DateTimeOffsetNullable:
				writer.WriteValue((value == null) ? null : new DateTimeOffset?((DateTimeOffset)value));
				return;
			case PrimitiveTypeCode.Decimal:
				writer.WriteValue((decimal)value);
				return;
			case PrimitiveTypeCode.DecimalNullable:
				writer.WriteValue((value == null) ? null : new decimal?((decimal)value));
				return;
			case PrimitiveTypeCode.Guid:
				writer.WriteValue((Guid)value);
				return;
			case PrimitiveTypeCode.GuidNullable:
				writer.WriteValue((value == null) ? null : new Guid?((Guid)value));
				return;
			case PrimitiveTypeCode.TimeSpan:
				writer.WriteValue((TimeSpan)value);
				return;
			case PrimitiveTypeCode.TimeSpanNullable:
				writer.WriteValue((value == null) ? null : new TimeSpan?((TimeSpan)value));
				return;
			case PrimitiveTypeCode.BigInteger:
				writer.WriteValue((BigInteger)value);
				return;
			case PrimitiveTypeCode.BigIntegerNullable:
				writer.WriteValue((value == null) ? null : new BigInteger?((BigInteger)value));
				return;
			case PrimitiveTypeCode.Uri:
				writer.WriteValue((Uri)value);
				return;
			case PrimitiveTypeCode.String:
				writer.WriteValue((string)value);
				return;
			case PrimitiveTypeCode.Bytes:
				writer.WriteValue((byte[])value);
				return;
			case PrimitiveTypeCode.DBNull:
				writer.WriteNull();
				return;
			}
			if (value is IConvertible)
			{
				IConvertible convertible = (IConvertible)value;
				TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertible);
				PrimitiveTypeCode typeCode2 = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? PrimitiveTypeCode.String : typeInformation.TypeCode);
				Type conversionType = ((typeInformation.TypeCode == PrimitiveTypeCode.Object) ? typeof(string) : typeInformation.Type);
				object value2 = convertible.ToType(conversionType, CultureInfo.InvariantCulture);
				WriteValue(writer, typeCode2, value2);
				return;
			}
			throw CreateUnsupportedTypeException(writer, value);
		}

		private static JsonWriterException CreateUnsupportedTypeException(JsonWriter writer, object value)
		{
			return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), null);
		}

		protected void SetWriteState(JsonToken token, object value)
		{
			switch (token)
			{
			case JsonToken.StartObject:
				InternalWriteStart(token, JsonContainerType.Object);
				break;
			case JsonToken.StartArray:
				InternalWriteStart(token, JsonContainerType.Array);
				break;
			case JsonToken.StartConstructor:
				InternalWriteStart(token, JsonContainerType.Constructor);
				break;
			case JsonToken.PropertyName:
				if (!(value is string))
				{
					throw new ArgumentException("A name is required when setting property name state.", "value");
				}
				InternalWritePropertyName((string)value);
				break;
			case JsonToken.Comment:
				InternalWriteComment();
				break;
			case JsonToken.Raw:
				InternalWriteRaw();
				break;
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				InternalWriteValue(token);
				break;
			case JsonToken.EndObject:
				InternalWriteEnd(JsonContainerType.Object);
				break;
			case JsonToken.EndArray:
				InternalWriteEnd(JsonContainerType.Array);
				break;
			case JsonToken.EndConstructor:
				InternalWriteEnd(JsonContainerType.Constructor);
				break;
			default:
				throw new ArgumentOutOfRangeException("token");
			}
		}

		internal void InternalWriteEnd(JsonContainerType container)
		{
			AutoCompleteClose(container);
		}

		internal void InternalWritePropertyName(string name)
		{
			_currentPosition.PropertyName = name;
			AutoComplete(JsonToken.PropertyName);
		}

		internal void InternalWriteRaw()
		{
		}

		internal void InternalWriteStart(JsonToken token, JsonContainerType container)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(token);
			Push(container);
		}

		internal void InternalWriteValue(JsonToken token)
		{
			UpdateScopeWithFinishedValue();
			AutoComplete(token);
		}

		internal void InternalWriteWhitespace(string ws)
		{
			if (ws != null && !StringUtils.IsWhiteSpace(ws))
			{
				throw JsonWriterException.Create(this, "Only white space characters should be used.", null);
			}
		}

		internal void InternalWriteComment()
		{
			AutoComplete(JsonToken.Comment);
		}
	}
}
namespace Newtonsoft.Json.Bson
{
	public class BsonWriter : JsonWriter
	{
		private readonly BsonBinaryWriter _writer;

		private BsonToken _root;

		private BsonToken _parent;

		private string _propertyName;

		public DateTimeKind DateTimeKindHandling
		{
			get
			{
				return _writer.DateTimeKindHandling;
			}
			set
			{
				_writer.DateTimeKindHandling = value;
			}
		}

		public BsonWriter(Stream stream)
		{
			ValidationUtils.ArgumentNotNull(stream, "stream");
			_writer = new BsonBinaryWriter(new BinaryWriter(stream));
		}

		public BsonWriter(BinaryWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = new BsonBinaryWriter(writer);
		}

		public override void Flush()
		{
			_writer.Flush();
		}

		protected override void WriteEnd(JsonToken token)
		{
			base.WriteEnd(token);
			RemoveParent();
			if (base.Top == 0)
			{
				_writer.WriteToken(_root);
			}
		}

		public override void WriteComment(string text)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON comment as BSON.", null);
		}

		public override void WriteStartConstructor(string name)
		{
			throw JsonWriterException.Create(this, "Cannot write JSON constructor as BSON.", null);
		}

		public override void WriteRaw(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		public override void WriteRawValue(string json)
		{
			throw JsonWriterException.Create(this, "Cannot write raw JSON as BSON.", null);
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			AddParent(new BsonArray());
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			AddParent(new BsonObject());
		}

		public override void WritePropertyName(string name)
		{
			base.WritePropertyName(name);
			_propertyName = name;
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && _writer != null)
			{
				_writer.Close();
			}
		}

		private void AddParent(BsonToken container)
		{
			AddToken(container);
			_parent = container;
		}

		private void RemoveParent()
		{
			_parent = _parent.Parent;
		}

		private void AddValue(object value, BsonType type)
		{
			AddToken(new BsonValue(value, type));
		}

		internal void AddToken(BsonToken token)
		{
			if (_parent != null)
			{
				if (_parent is BsonObject)
				{
					((BsonObject)_parent).Add(_propertyName, token);
					_propertyName = null;
				}
				else
				{
					((BsonArray)_parent).Add(token);
				}
				return;
			}
			if (token.Type != BsonType.Object && token.Type != BsonType.Array)
			{
				throw JsonWriterException.Create(this, "Error writing {0} value. BSON must start with an Object or Array.".FormatWith(CultureInfo.InvariantCulture, token.Type), null);
			}
			_parent = token;
			_root = token;
		}

		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				InternalWriteValue(JsonToken.Integer);
				AddToken(new BsonBinary(((BigInteger)value).ToByteArray(), BsonBinaryType.Binary));
			}
			else
			{
				base.WriteValue(value);
			}
		}

		public override void WriteNull()
		{
			base.WriteNull();
			AddValue(null, BsonType.Null);
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			AddValue(null, BsonType.Undefined);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			if (value == null)
			{
				AddValue(null, BsonType.Null);
			}
			else
			{
				AddToken(new BsonString(value, includeLength: true));
			}
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			if (value > int.MaxValue)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Long);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			if (value > long.MaxValue)
			{
				throw JsonWriterException.Create(this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", null);
			}
			base.WriteValue(value);
			AddValue(value, BsonType.Long);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Number);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Number);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = null;
			text = value.ToString(CultureInfo.InvariantCulture);
			AddToken(new BsonString(text, includeLength: true));
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Number);
		}

		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			AddValue(value, BsonType.Date);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			AddValue(value, BsonType.Date);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			AddToken(new BsonBinary(value, BsonBinaryType.Binary));
		}

		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			AddToken(new BsonBinary(value.ToByteArray(), BsonBinaryType.Uuid));
		}

		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			AddToken(new BsonString(value.ToString(), includeLength: true));
		}

		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			AddToken(new BsonString(value.ToString(), includeLength: true));
		}

		public void WriteObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw JsonWriterException.Create(this, "An object id must be 12 bytes", null);
			}
			UpdateScopeWithFinishedValue();
			AutoComplete(JsonToken.Undefined);
			AddValue(value, BsonType.Oid);
		}

		public void WriteRegex(string pattern, string options)
		{
			ValidationUtils.ArgumentNotNull(pattern, "pattern");
			UpdateScopeWithFinishedValue();
			AutoComplete(JsonToken.Undefined);
			AddToken(new BsonRegex(pattern, options));
		}
	}
	public class BsonObjectId
	{
		public byte[] Value { get; private set; }

		public BsonObjectId(byte[] value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value.Length != 12)
			{
				throw new ArgumentException("An ObjectId must be 12 bytes", "value");
			}
			Value = value;
		}
	}
}
namespace Newtonsoft.Json
{
	public abstract class JsonConverter
	{
		public virtual bool CanRead => true;

		public virtual bool CanWrite => true;

		public abstract void WriteJson(JsonWriter writer, object value, JsonSerializer serializer);

		public abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer);

		public abstract bool CanConvert(Type objectType);

		public virtual JsonSchema GetSchema()
		{
			return null;
		}
	}
}
namespace Newtonsoft.Json.Converters
{
	public class BinaryConverter : JsonConverter
	{
		private const string BinaryTypeName = "System.Data.Linq.Binary";

		private const string BinaryToArrayName = "ToArray";

		private ReflectionObject _reflectionObject;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			byte[] byteArray = GetByteArray(value);
			writer.WriteValue(byteArray);
		}

		private byte[] GetByteArray(object value)
		{
			if (value.GetType().AssignableToTypeName("System.Data.Linq.Binary"))
			{
				EnsureReflectionObject(value.GetType());
				return (byte[])_reflectionObject.GetValue(value, "ToArray");
			}
			if (value is SqlBinary sqlBinary)
			{
				return sqlBinary.Value;
			}
			throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		private void EnsureReflectionObject(Type t)
		{
			if (_reflectionObject == null)
			{
				_reflectionObject = ReflectionObject.Create(t, t.GetConstructor(new Type[1] { typeof(byte[]) }), "ToArray");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			byte[] array;
			if (reader.TokenType == JsonToken.StartArray)
			{
				array = ReadByteArray(reader);
			}
			else
			{
				if (reader.TokenType != JsonToken.String)
				{
					throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				}
				string s = reader.Value.ToString();
				array = Convert.FromBase64String(s);
			}
			if (type.AssignableToTypeName("System.Data.Linq.Binary"))
			{
				EnsureReflectionObject(type);
				return _reflectionObject.Creator(array);
			}
			if (type == typeof(SqlBinary))
			{
				return new SqlBinary(array);
			}
			throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith(CultureInfo.InvariantCulture, objectType));
		}

		private byte[] ReadByteArray(JsonReader reader)
		{
			List<byte> list = new List<byte>();
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.Integer:
					list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));
					break;
				case JsonToken.EndArray:
					return list.ToArray();
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
				case JsonToken.Comment:
					break;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
		}

		public override bool CanConvert(Type objectType)
		{
			if (objectType.AssignableToTypeName("System.Data.Linq.Binary"))
			{
				return true;
			}
			if (objectType == typeof(SqlBinary) || objectType == typeof(SqlBinary?))
			{
				return true;
			}
			return false;
		}
	}
	public class DataSetConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataSet dataSet = (DataSet)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			DataTableConverter dataTableConverter = new DataTableConverter();
			writer.WriteStartObject();
			foreach (DataTable table in dataSet.Tables)
			{
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(table.TableName) : table.TableName);
				dataTableConverter.WriteJson(writer, table, serializer);
			}
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataSet dataSet = ((objectType == typeof(DataSet)) ? new DataSet() : ((DataSet)Activator.CreateInstance(objectType)));
			DataTableConverter dataTableConverter = new DataTableConverter();
			CheckedRead(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				DataTable dataTable = dataSet.Tables[(string)reader.Value];
				bool flag = dataTable != null;
				dataTable = (DataTable)dataTableConverter.ReadJson(reader, typeof(DataTable), dataTable, serializer);
				if (!flag)
				{
					dataSet.Tables.Add(dataTable);
				}
				CheckedRead(reader);
			}
			return dataSet;
		}

		public override bool CanConvert(Type valueType)
		{
			return typeof(DataSet).IsAssignableFrom(valueType);
		}

		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading DataSet.");
			}
		}
	}
	public class DataTableConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DataTable dataTable = (DataTable)value;
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartArray();
			foreach (DataRow row in dataTable.Rows)
			{
				writer.WriteStartObject();
				foreach (DataColumn column in row.Table.Columns)
				{
					if (serializer.NullValueHandling != NullValueHandling.Ignore || (row[column] != null && row[column] != DBNull.Value))
					{
						writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
						serializer.Serialize(writer, row[column]);
					}
				}
				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			DataTable dataTable = existingValue as DataTable;
			if (dataTable == null)
			{
				dataTable = ((objectType == typeof(DataTable)) ? new DataTable() : ((DataTable)Activator.CreateInstance(objectType)));
			}
			if (reader.TokenType == JsonToken.PropertyName)
			{
				dataTable.TableName = (string)reader.Value;
				CheckedRead(reader);
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable. Expected StartArray, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			CheckedRead(reader);
			while (reader.TokenType != JsonToken.EndArray)
			{
				CreateRow(reader, dataTable);
				CheckedRead(reader);
			}
			return dataTable;
		}

		private static void CreateRow(JsonReader reader, DataTable dt)
		{
			DataRow dataRow = dt.NewRow();
			CheckedRead(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text = (string)reader.Value;
				CheckedRead(reader);
				DataColumn dataColumn = dt.Columns[text];
				if (dataColumn == null)
				{
					Type columnDataType = GetColumnDataType(reader);
					dataColumn = new DataColumn(text, columnDataType);
					dt.Columns.Add(dataColumn);
				}
				if (dataColumn.DataType == typeof(DataTable))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						CheckedRead(reader);
					}
					DataTable dataTable = new DataTable();
					while (reader.TokenType != JsonToken.EndArray)
					{
						CreateRow(reader, dataTable);
						CheckedRead(reader);
					}
					dataRow[text] = dataTable;
				}
				else if (dataColumn.DataType.IsArray && dataColumn.DataType != typeof(byte[]))
				{
					if (reader.TokenType == JsonToken.StartArray)
					{
						CheckedRead(reader);
					}
					List<object> list = new List<object>();
					while (reader.TokenType != JsonToken.EndArray)
					{
						list.Add(reader.Value);
						CheckedRead(reader);
					}
					Array array = Array.CreateInstance(dataColumn.DataType.GetElementType(), list.Count);
					Array.Copy(list.ToArray(), array, list.Count);
					dataRow[text] = array;
				}
				else
				{
					dataRow[text] = reader.Value ?? DBNull.Value;
				}
				CheckedRead(reader);
			}
			dataRow.EndEdit();
			dt.Rows.Add(dataRow);
		}

		private static Type GetColumnDataType(JsonReader reader)
		{
			JsonToken tokenType = reader.TokenType;
			switch (tokenType)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return reader.ValueType;
			case JsonToken.Null:
			case JsonToken.Undefined:
				return typeof(string);
			case JsonToken.StartArray:
			{
				CheckedRead(reader);
				if (reader.TokenType == JsonToken.StartObject)
				{
					return typeof(DataTable);
				}
				Type columnDataType = GetColumnDataType(reader);
				return columnDataType.MakeArrayType();
			}
			default:
				throw JsonSerializationException.Create(reader, "Unexpected JSON token when reading DataTable: {0}".FormatWith(CultureInfo.InvariantCulture, tokenType));
			}
		}

		private static void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading DataTable.");
			}
		}

		public override bool CanConvert(Type valueType)
		{
			return typeof(DataTable).IsAssignableFrom(valueType);
		}
	}
	public abstract class CustomCreationConverter<T> : JsonConverter
	{
		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T val = Create(objectType);
			if (val == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, val);
			return val;
		}

		public abstract T Create(Type objectType);

		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}
	}
	public abstract class DateTimeConverterBase : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
			{
				return true;
			}
			if (objectType == typeof(DateTimeOffset) || objectType == typeof(DateTimeOffset?))
			{
				return true;
			}
			return false;
		}
	}
	public class DiscriminatedUnionConverter : JsonConverter
	{
		private const string CasePropertyName = "Case";

		private const string FieldsPropertyName = "Fields";

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			Type type = value.GetType();
			object arg = FSharpUtils.GetUnionFields(null, value, type, null);
			object arg2 = FSharpUtils.GetUnionCaseInfo(arg);
			object obj = FSharpUtils.GetUnionCaseFields(arg);
			object obj2 = FSharpUtils.GetUnionCaseInfoName(arg2);
			object[] array = obj as object[];
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Case") : "Case");
			writer.WriteValue((string)obj2);
			if (array != null && array.Length > 0)
			{
				writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Fields") : "Fields");
				serializer.Serialize(writer, obj);
			}
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			object obj = null;
			string text = null;
			JArray jArray = null;
			ReadAndAssert(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string text2 = reader.Value.ToString();
				if (string.Equals(text2, "Case", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					IEnumerable enumerable = (IEnumerable)FSharpUtils.GetUnionCases(null, objectType, null);
					text = reader.Value.ToString();
					foreach (object item in enumerable)
					{
						if ((string)FSharpUtils.GetUnionCaseInfoName(item) == text)
						{
							obj = item;
							break;
						}
					}
					if (obj == null)
					{
						throw JsonSerializationException.Create(reader, "No union type found with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
					}
				}
				else
				{
					if (!string.Equals(text2, "Fields", StringComparison.OrdinalIgnoreCase))
					{
						throw JsonSerializationException.Create(reader, "Unexpected property '{0}' found when reading union.".FormatWith(CultureInfo.InvariantCulture, text2));
					}
					ReadAndAssert(reader);
					if (reader.TokenType != JsonToken.StartArray)
					{
						throw JsonSerializationException.Create(reader, "Union fields must been an array.");
					}
					jArray = (JArray)JToken.ReadFrom(reader);
				}
				ReadAndAssert(reader);
			}
			if (obj == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union name found.".FormatWith(CultureInfo.InvariantCulture, "Case"));
			}
			PropertyInfo[] array = (PropertyInfo[])FSharpUtils.GetUnionCaseInfoFields(obj);
			object[] array2 = new object[array.Length];
			if (array.Length > 0 && jArray == null)
			{
				throw JsonSerializationException.Create(reader, "No '{0}' property with union fields found.".FormatWith(CultureInfo.InvariantCulture, "Fields"));
			}
			if (jArray != null)
			{
				if (array.Length != jArray.Count)
				{
					throw JsonSerializationException.Create(reader, "The number of field values does not match the number of properties definied by union '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				for (int i = 0; i < jArray.Count; i++)
				{
					JToken jToken = jArray[i];
					PropertyInfo propertyInfo = array[i];
					array2[i] = jToken.ToObject(propertyInfo.PropertyType, serializer);
				}
			}
			return FSharpUtils.MakeUnion(null, obj, array2, null);
		}

		public override bool CanConvert(Type objectType)
		{
			if (typeof(IEnumerable).IsAssignableFrom(objectType))
			{
				return false;
			}
			object[] customAttributes = objectType.GetCustomAttributes(inherit: true);
			bool flag = false;
			object[] array = customAttributes;
			foreach (object obj in array)
			{
				Type type = obj.GetType();
				if (type.FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute")
				{
					FSharpUtils.EnsureInitialized(type.Assembly());
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			return (bool)FSharpUtils.IsUnion(null, objectType, null);
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading union.");
			}
		}
	}
	public class EntityKeyMemberConverter : JsonConverter
	{
		private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

		private const string KeyPropertyName = "Key";

		private const string TypePropertyName = "Type";

		private const string ValuePropertyName = "Value";

		private static ReflectionObject _reflectionObject;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			EnsureReflectionObject(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			string value2 = (string)_reflectionObject.GetValue(value, "Key");
			object value3 = _reflectionObject.GetValue(value, "Value");
			Type type = value3?.GetType();
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			writer.WriteValue(value2);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Type") : "Type");
			writer.WriteValue((type != null) ? type.FullName : null);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			if (type != null)
			{
				if (JsonSerializerInternalWriter.TryConvertToString(value3, type, out var s))
				{
					writer.WriteValue(s);
				}
				else
				{
					writer.WriteValue(value3);
				}
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteEndObject();
		}

		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			ReadAndAssert(reader);
			if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
			{
				throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
			}
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw new JsonSerializationException("Unexpected end.");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			EnsureReflectionObject(objectType);
			object obj = _reflectionObject.Creator();
			ReadAndAssertProperty(reader, "Key");
			ReadAndAssert(reader);
			_reflectionObject.SetValue(obj, "Key", reader.Value.ToString());
			ReadAndAssertProperty(reader, "Type");
			ReadAndAssert(reader);
			string typeName = reader.Value.ToString();
			Type type = Type.GetType(typeName);
			ReadAndAssertProperty(reader, "Value");
			ReadAndAssert(reader);
			_reflectionObject.SetValue(obj, "Value", serializer.Deserialize(reader, type));
			ReadAndAssert(reader);
			return obj;
		}

		private static void EnsureReflectionObject(Type objectType)
		{
			if (_reflectionObject == null)
			{
				_reflectionObject = ReflectionObject.Create(objectType, "Key", "Value");
			}
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember");
		}
	}
	public class ExpandoObjectConverter : JsonConverter
	{
		public override bool CanWrite => false;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return ReadValue(reader);
		}

		private object ReadValue(JsonReader reader)
		{
			while (reader.TokenType == JsonToken.Comment)
			{
				if (!reader.Read())
				{
					throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
				}
			}
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return ReadObject(reader);
			case JsonToken.StartArray:
				return ReadList(reader);
			default:
				if (JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
				{
					return reader.Value;
				}
				throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
		}

		private object ReadList(JsonReader reader)
		{
			IList<object> list = new List<object>();
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.EndArray:
					return list;
				case JsonToken.Comment:
					continue;
				}
				object item = ReadValue(reader);
				list.Add(item);
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string key = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					object value = ReadValue(reader);
					dictionary[key] = value;
					break;
				}
				case JsonToken.EndObject:
					return dictionary;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}
	}
	public class KeyValuePairConverter : JsonConverter
	{
		private const string KeyName = "Key";

		private const string ValueName = "Value";

		private static readonly ThreadSafeStore<Type, ReflectionObject> ReflectionObjectPerType = new ThreadSafeStore<Type, ReflectionObject>(InitializeReflectionObject);

		private static ReflectionObject InitializeReflectionObject(Type t)
		{
			IList<Type> genericArguments = t.GetGenericArguments();
			Type type = genericArguments[0];
			Type type2 = genericArguments[1];
			return ReflectionObject.Create(t, t.GetConstructor(new Type[2] { type, type2 }), "Key", "Value");
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			ReflectionObject reflectionObject = ReflectionObjectPerType.Get(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Key"), reflectionObject.GetType("Key"));
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			serializer.Serialize(writer, reflectionObject.GetValue(value, "Value"), reflectionObject.GetType("Value"));
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type key = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
			ReflectionObject reflectionObject = ReflectionObjectPerType.Get(key);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to KeyValuePair.");
				}
				return null;
			}
			object obj = null;
			object obj2 = null;
			ReadAndAssert(reader);
			while (reader.TokenType == JsonToken.PropertyName)
			{
				string a = reader.Value.ToString();
				if (string.Equals(a, "Key", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					obj = serializer.Deserialize(reader, reflectionObject.GetType("Key"));
				}
				else if (string.Equals(a, "Value", StringComparison.OrdinalIgnoreCase))
				{
					ReadAndAssert(reader);
					obj2 = serializer.Deserialize(reader, reflectionObject.GetType("Value"));
				}
				else
				{
					reader.Skip();
				}
				ReadAndAssert(reader);
			}
			return reflectionObject.Creator(obj, obj2);
		}

		public override bool CanConvert(Type objectType)
		{
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (type.IsValueType() && type.IsGenericType())
			{
				return type.GetGenericTypeDefinition() == typeof(KeyValuePair<, >);
			}
			return false;
		}

		private static void ReadAndAssert(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading KeyValuePair.");
			}
		}
	}
	public class BsonObjectIdConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			BsonObjectId bsonObjectId = (BsonObjectId)value;
			if (writer is BsonWriter bsonWriter)
			{
				bsonWriter.WriteObjectId(bsonObjectId.Value);
			}
			else
			{
				writer.WriteValue(bsonObjectId.Value);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.Bytes)
			{
				throw new JsonSerializationException("Expected Bytes but got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			byte[] value = (byte[])reader.Value;
			return new BsonObjectId(value);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(BsonObjectId);
		}
	}
	public class RegexConverter : JsonConverter
	{
		private const string PatternName = "Pattern";

		private const string OptionsName = "Options";

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Regex regex = (Regex)value;
			if (writer is BsonWriter writer2)
			{
				WriteBson(writer2, regex);
			}
			else
			{
				WriteJson(writer, regex, serializer);
			}
		}

		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string text = null;
			if (HasFlag(regex.Options, RegexOptions.IgnoreCase))
			{
				text += "i";
			}
			if (HasFlag(regex.Options, RegexOptions.Multiline))
			{
				text += "m";
			}
			if (HasFlag(regex.Options, RegexOptions.Singleline))
			{
				text += "s";
			}
			text += "u";
			if (HasFlag(regex.Options, RegexOptions.ExplicitCapture))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Options") : "Options");
			serializer.Serialize(writer, regex.Options);
			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject)
			{
				return ReadRegexObject(reader, serializer);
			}
			if (reader.TokenType == JsonToken.String)
			{
				return ReadRegexString(reader);
			}
			throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
		}

		private object ReadRegexString(JsonReader reader)
		{
			string text = (string)reader.Value;
			int num = text.LastIndexOf('/');
			string pattern = text.Substring(1, num - 1);
			string text2 = text.Substring(num + 1);
			RegexOptions regexOptions = RegexOptions.None;
			string text3 = text2;
			for (int i = 0; i < text3.Length; i++)
			{
				switch (text3[i])
				{
				case 'i':
					regexOptions |= RegexOptions.IgnoreCase;
					break;
				case 'm':
					regexOptions |= RegexOptions.Multiline;
					break;
				case 's':
					regexOptions |= RegexOptions.Singleline;
					break;
				case 'x':
					regexOptions |= RegexOptions.ExplicitCapture;
					break;
				}
			}
			return new Regex(pattern, regexOptions);
		}

		private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
		{
			string text = null;
			RegexOptions? regexOptions = null;
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string a = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
					}
					if (string.Equals(a, "Pattern", StringComparison.OrdinalIgnoreCase))
					{
						text = (string)reader.Value;
					}
					else if (string.Equals(a, "Options", StringComparison.OrdinalIgnoreCase))
					{
						regexOptions = serializer.Deserialize<RegexOptions>(reader);
					}
					else
					{
						reader.Skip();
					}
					break;
				}
				case JsonToken.EndObject:
					if (text == null)
					{
						throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
					}
					return new Regex(text, regexOptions ?? RegexOptions.None);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Regex);
		}
	}
	public class StringEnumConverter : JsonConverter
	{
		public bool CamelCaseText { get; set; }

		public bool AllowIntegerValues { get; set; }

		public StringEnumConverter()
		{
			AllowIntegerValues = true;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Enum @enum = (Enum)value;
			string text = @enum.ToString("G");
			if (char.IsNumber(text[0]) || text[0] == '-')
			{
				writer.WriteValue(value);
				return;
			}
			Type type = @enum.GetType();
			string value2 = EnumUtils.ToEnumName(type, text, CamelCaseText);
			writer.WriteValue(value2);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type type = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			try
			{
				if (reader.TokenType == JsonToken.String)
				{
					string enumText = reader.Value.ToString();
					return EnumUtils.ParseEnumName(enumText, flag, type);
				}
				if (reader.TokenType == JsonToken.Integer)
				{
					if (!AllowIntegerValues)
					{
						throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith(CultureInfo.InvariantCulture, reader.Value));
					}
					return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
				}
			}
			catch (Exception ex)
			{
				throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(reader.Value), objectType), ex);
			}
			throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		public override bool CanConvert(Type objectType)
		{
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			return type.IsEnum();
		}
	}
}
namespace Newtonsoft.Json
{
	public enum ConstructorHandling
	{
		Default,
		AllowNonPublicDefaultConstructor
	}
}
namespace Newtonsoft.Json.Converters
{
	public class VersionConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			if (value is Version)
			{
				writer.WriteValue(value.ToString());
				return;
			}
			throw new JsonSerializationException("Expected Version object value");
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			if (reader.TokenType == JsonToken.String)
			{
				try
				{
					return new Version((string)reader.Value);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error parsing version string: {0}".FormatWith(CultureInfo.InvariantCulture, reader.Value), ex);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing version. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Version);
		}
	}
}
namespace Newtonsoft.Json
{
	public enum FloatFormatHandling
	{
		String,
		Symbol,
		DefaultValue
	}
	public enum FloatParseHandling
	{
		Double,
		Decimal
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public abstract class JsonContainerAttribute : Attribute
	{
		internal bool? _isReference;

		internal bool? _itemIsReference;

		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		internal TypeNameHandling? _itemTypeNameHandling;

		public string Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public Type ItemConverterType { get; set; }

		public object[] ItemConverterParameters { get; set; }

		public bool IsReference
		{
			get
			{
				return _isReference ?? false;
			}
			set
			{
				_isReference = value;
			}
		}

		public bool ItemIsReference
		{
			get
			{
				return _itemIsReference ?? false;
			}
			set
			{
				_itemIsReference = value;
			}
		}

		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return _itemReferenceLoopHandling ?? ReferenceLoopHandling.Error;
			}
			set
			{
				_itemReferenceLoopHandling = value;
			}
		}

		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return _itemTypeNameHandling ?? TypeNameHandling.None;
			}
			set
			{
				_itemTypeNameHandling = value;
			}
		}

		protected JsonContainerAttribute()
		{
		}

		protected JsonContainerAttribute(string id)
		{
			Id = id;
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonDictionaryAttribute : JsonContainerAttribute
	{
		public JsonDictionaryAttribute()
		{
		}

		public JsonDictionaryAttribute(string id)
			: base(id)
		{
		}
	}
	[Serializable]
	public class JsonException : Exception
	{
		public JsonException()
		{
		}

		public JsonException(string message)
			: base(message)
		{
		}

		public JsonException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public JsonException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal static JsonException Create(IJsonLineInfo lineInfo, string path, string message)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonException(message);
		}
	}
	public enum DateFormatHandling
	{
		IsoDateFormat,
		MicrosoftDateFormat
	}
	public enum DateParseHandling
	{
		None,
		DateTime,
		DateTimeOffset
	}
	public enum DateTimeZoneHandling
	{
		Local,
		Utc,
		Unspecified,
		RoundtripKind
	}
	public enum Formatting
	{
		None,
		Indented
	}
	[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
	public sealed class JsonConstructorAttribute : Attribute
	{
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class JsonExtensionDataAttribute : Attribute
	{
		public bool WriteData { get; set; }

		public bool ReadData { get; set; }

		public JsonExtensionDataAttribute()
		{
			WriteData = true;
			ReadData = true;
		}
	}
	internal enum JsonContainerType
	{
		None,
		Object,
		Array,
		Constructor
	}
	internal struct JsonPosition
	{
		private static readonly char[] SpecialCharacters = new char[6] { '.', ' ', '[', ']', '(', ')' };

		internal JsonContainerType Type;

		internal int Position;

		internal string PropertyName;

		internal bool HasIndex;

		public JsonPosition(JsonContainerType type)
		{
			Type = type;
			HasIndex = TypeHasIndex(type);
			Position = -1;
			PropertyName = null;
		}

		internal void WriteTo(StringBuilder sb)
		{
			switch (Type)
			{
			case JsonContainerType.Object:
			{
				if (sb.Length > 0)
				{
					sb.Append('.');
				}
				string propertyName = PropertyName;
				if (propertyName.IndexOfAny(SpecialCharacters) != -1)
				{
					sb.Append("['");
					sb.Append(propertyName);
					sb.Append("']");
				}
				else
				{
					sb.Append(propertyName);
				}
				break;
			}
			case JsonContainerType.Array:
			case JsonContainerType.Constructor:
				sb.Append('[');
				sb.Append(Position);
				sb.Append(']');
				break;
			}
		}

		internal static bool TypeHasIndex(JsonContainerType type)
		{
			if (type != JsonContainerType.Array)
			{
				return type == JsonContainerType.Constructor;
			}
			return true;
		}

		internal static string BuildPath(IEnumerable<JsonPosition> positions)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (JsonPosition position in positions)
			{
				position.WriteTo(stringBuilder);
			}
			return stringBuilder.ToString();
		}

		internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
		{
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal))
			{
				message = message.Trim();
				if (!StringUtils.EndsWith(message, '.'))
				{
					message += ".";
				}
				message += " ";
			}
			message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
			}
			message += ".";
			return message;
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	internal class JPropertyKeyedCollection : Collection<JToken>
	{
		private static readonly IEqualityComparer<string> Comparer = StringComparer.Ordinal;

		private Dictionary<string, JToken> _dictionary;

		public JToken this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				if (_dictionary != null)
				{
					return _dictionary[key];
				}
				throw new KeyNotFoundException();
			}
		}

		public ICollection<string> Keys
		{
			get
			{
				EnsureDictionary();
				return _dictionary.Keys;
			}
		}

		public ICollection<JToken> Values
		{
			get
			{
				EnsureDictionary();
				return _dictionary.Values;
			}
		}

		private void AddKey(string key, JToken item)
		{
			EnsureDictionary();
			_dictionary[key] = item;
		}

		protected void ChangeItemKey(JToken item, string newKey)
		{
			if (!ContainsItem(item))
			{
				throw new ArgumentException("The specified item does not exist in this KeyedCollection.");
			}
			string keyForItem = GetKeyForItem(item);
			if (!Comparer.Equals(keyForItem, newKey))
			{
				if (newKey != null)
				{
					AddKey(newKey, item);
				}
				if (keyForItem != null)
				{
					RemoveKey(keyForItem);
				}
			}
		}

		protected override void ClearItems()
		{
			base.ClearItems();
			if (_dictionary != null)
			{
				_dictionary.Clear();
			}
		}

		public bool Contains(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (_dictionary != null)
			{
				return _dictionary.ContainsKey(key);
			}
			return false;
		}

		private bool ContainsItem(JToken item)
		{
			if (_dictionary == null)
			{
				return false;
			}
			string keyForItem = GetKeyForItem(item);
			JToken value;
			return _dictionary.TryGetValue(keyForItem, out value);
		}

		private void EnsureDictionary()
		{
			if (_dictionary == null)
			{
				_dictionary = new Dictionary<string, JToken>(Comparer);
			}
		}

		private string GetKeyForItem(JToken item)
		{
			return ((JProperty)item).Name;
		}

		protected override void InsertItem(int index, JToken item)
		{
			AddKey(GetKeyForItem(item), item);
			base.InsertItem(index, item);
		}

		public bool Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (_dictionary != null)
			{
				if (_dictionary.ContainsKey(key))
				{
					return Remove(_dictionary[key]);
				}
				return false;
			}
			return false;
		}

		protected override void RemoveItem(int index)
		{
			string keyForItem = GetKeyForItem(base.Items[index]);
			RemoveKey(keyForItem);
			base.RemoveItem(index);
		}

		private void RemoveKey(string key)
		{
			if (_dictionary != null)
			{
				_dictionary.Remove(key);
			}
		}

		protected override void SetItem(int index, JToken item)
		{
			string keyForItem = GetKeyForItem(item);
			string keyForItem2 = GetKeyForItem(base.Items[index]);
			if (Comparer.Equals(keyForItem2, keyForItem))
			{
				if (_dictionary != null)
				{
					_dictionary[keyForItem] = item;
				}
			}
			else
			{
				AddKey(keyForItem, item);
				if (keyForItem2 != null)
				{
					RemoveKey(keyForItem2);
				}
			}
			base.SetItem(index, item);
		}

		public bool TryGetValue(string key, out JToken value)
		{
			if (_dictionary == null)
			{
				value = null;
				return false;
			}
			return _dictionary.TryGetValue(key, out value);
		}

		public bool Compare(JPropertyKeyedCollection other)
		{
			if (this == other)
			{
				return true;
			}
			Dictionary<string, JToken> dictionary = _dictionary;
			Dictionary<string, JToken> dictionary2 = other._dictionary;
			if (dictionary == null && dictionary2 == null)
			{
				return true;
			}
			if (dictionary == null)
			{
				return dictionary2.Count == 0;
			}
			if (dictionary2 == null)
			{
				return dictionary.Count == 0;
			}
			if (dictionary.Count != dictionary2.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, JToken> item in dictionary)
			{
				if (!dictionary2.TryGetValue(item.Key, out var value))
				{
					return false;
				}
				JProperty jProperty = (JProperty)item.Value;
				JProperty jProperty2 = (JProperty)value;
				if (jProperty.Value == null)
				{
					return jProperty2.Value == null;
				}
				if (!jProperty.Value.DeepEquals(jProperty2.Value))
				{
					return false;
				}
			}
			return true;
		}
	}
	public class JsonMergeSettings
	{
		private MergeArrayHandling _mergeArrayHandling;

		public MergeArrayHandling MergeArrayHandling
		{
			get
			{
				return _mergeArrayHandling;
			}
			set
			{
				if (value < MergeArrayHandling.Concat || value > MergeArrayHandling.Merge)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_mergeArrayHandling = value;
			}
		}
	}
}
namespace Newtonsoft.Json.Linq.JsonPath
{
	internal abstract class PathFilter
	{
		public abstract IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch);

		protected static JToken GetTokenIndex(JToken t, bool errorWhenNoMatch, int index)
		{
			JArray jArray = t as JArray;
			JConstructor jConstructor = t as JConstructor;
			if (jArray != null)
			{
				if (jArray.Count <= index)
				{
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, index));
					}
					return null;
				}
				return jArray[index];
			}
			if (jConstructor != null)
			{
				if (jConstructor.Count <= index)
				{
					if (errorWhenNoMatch)
					{
						throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith(CultureInfo.InvariantCulture, index));
					}
					return null;
				}
				return jConstructor[index];
			}
			if (errorWhenNoMatch)
			{
				throw new JsonException("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, index, t.GetType().Name));
			}
			return null;
		}
	}
	internal class ArrayIndexFilter : PathFilter
	{
		public int? Index { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				if (Index.HasValue)
				{
					JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, Index.Value);
					if (v != null)
					{
						yield return v;
					}
				}
				else if (t is JArray || t is JConstructor)
				{
					foreach (JToken item in (IEnumerable<JToken>)t)
					{
						yield return item;
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Index * not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
		}
	}
	internal class ArrayMultipleIndexFilter : PathFilter
	{
		public List<int> Indexes { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				foreach (int i in Indexes)
				{
					JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, i);
					if (v != null)
					{
						yield return v;
					}
				}
			}
		}
	}
	internal class ArraySliceFilter : PathFilter
	{
		public int? Start { get; set; }

		public int? End { get; set; }

		public int? Step { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			int? step = Step;
			if (step.GetValueOrDefault() == 0 && step.HasValue)
			{
				throw new JsonException("Step cannot be zero.");
			}
			foreach (JToken t in current)
			{
				if (t is JArray a)
				{
					int stepCount = Step ?? 1;
					int startIndex = Start ?? ((stepCount <= 0) ? (a.Count - 1) : 0);
					int stopIndex = End ?? ((stepCount > 0) ? a.Count : (-1));
					if (Start < 0)
					{
						startIndex = a.Count + startIndex;
					}
					if (End < 0)
					{
						stopIndex = a.Count + stopIndex;
					}
					startIndex = Math.Max(startIndex, (stepCount <= 0) ? int.MinValue : 0);
					startIndex = Math.Min(startIndex, (stepCount > 0) ? a.Count : (a.Count - 1));
					stopIndex = Math.Max(stopIndex, -1);
					stopIndex = Math.Min(stopIndex, a.Count);
					bool positiveStep = stepCount > 0;
					if (IsValid(startIndex, stopIndex, positiveStep))
					{
						for (int i = startIndex; IsValid(i, stopIndex, positiveStep); i += stepCount)
						{
							yield return a[i];
						}
					}
					else if (errorWhenNoMatch)
					{
						throw new JsonException("Array slice of {0} to {1} returned no results.".FormatWith(CultureInfo.InvariantCulture, Start.HasValue ? Start.Value.ToString(CultureInfo.InvariantCulture) : "*", End.HasValue ? End.Value.ToString(CultureInfo.InvariantCulture) : "*"));
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Array slice is not valid on {0}.".FormatWith(CultureInfo.InvariantCulture, t.GetType().Name));
				}
			}
		}

		private bool IsValid(int index, int stopIndex, bool positiveStep)
		{
			if (positiveStep)
			{
				return index < stopIndex;
			}
			return index > stopIndex;
		}
	}
	internal class FieldFilter : PathFilter
	{
		public string Name { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				if (t is JObject o)
				{
					if (Name != null)
					{
						JToken v = o[Name];
						if (v != null)
						{
							yield return v;
						}
						else if (errorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, Name));
						}
						continue;
					}
					foreach (KeyValuePair<string, JToken> p in o)
					{
						KeyValuePair<string, JToken> keyValuePair = p;
						yield return keyValuePair.Value;
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, Name ?? "*", t.GetType().Name));
				}
			}
		}
	}
	internal class FieldMultipleFilter : PathFilter
	{
		public List<string> Names { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				if (t is JObject o)
				{
					foreach (string name in Names)
					{
						JToken v = o[name];
						if (v != null)
						{
							yield return v;
						}
						if (errorWhenNoMatch)
						{
							throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, name));
						}
					}
				}
				else if (errorWhenNoMatch)
				{
					throw new JsonException("Properties {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", Names.Select((string n) => "'" + n + "'").ToArray()), t.GetType().Name));
				}
			}
		}
	}
	internal class JPath
	{
		private readonly string _expression;

		private int _currentIndex;

		public List<PathFilter> Filters { get; private set; }

		public JPath(string expression)
		{
			ValidationUtils.ArgumentNotNull(expression, "expression");
			_expression = expression;
			Filters = new List<PathFilter>();
			ParseMain();
		}

		private void ParseMain()
		{
			int currentIndex = _currentIndex;
			EatWhitespace();
			if (_expression.Length == _currentIndex)
			{
				return;
			}
			if (_expression[_currentIndex] == '$')
			{
				if (_expression.Length == 1)
				{
					return;
				}
				char c = _expression[_currentIndex + 1];
				if (c == '.' || c == '[')
				{
					_currentIndex++;
					currentIndex = _currentIndex;
				}
			}
			if (!ParsePath(Filters, currentIndex, query: false))
			{
				int currentIndex2 = _currentIndex;
				EatWhitespace();
				if (_currentIndex < _expression.Length)
				{
					throw new JsonException("Unexpected character while parsing path: " + _expression[currentIndex2]);
				}
			}
		}

		private bool ParsePath(List<PathFilter> filters, int currentPartStartIndex, bool query)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			while (_currentIndex < _expression.Length && !flag4)
			{
				char c = _expression[_currentIndex];
				switch (c)
				{
				case '(':
				case '[':
					if (_currentIndex > currentPartStartIndex)
					{
						string name = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex);
						object obj2;
						if (!flag)
						{
							FieldFilter fieldFilter2 = new FieldFilter();
							fieldFilter2.Name = name;
							obj2 = fieldFilter2;
						}
						else
						{
							ScanFilter scanFilter2 = new ScanFilter();
							scanFilter2.Name = name;
							obj2 = scanFilter2;
						}
						PathFilter item2 = (PathFilter)obj2;
						filters.Add(item2);
						flag = false;
					}
					filters.Add(ParseIndexer(c));
					_currentIndex++;
					currentPartStartIndex = _currentIndex;
					flag2 = true;
					flag3 = false;
					break;
				case ')':
				case ']':
					flag4 = true;
					break;
				case ' ':
					if (_currentIndex < _expression.Length)
					{
						flag4 = true;
					}
					break;
				case '.':
					if (_currentIndex > currentPartStartIndex)
					{
						string text = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex);
						if (text == "*")
						{
							text = null;
						}
						object obj;
						if (!flag)
						{
							FieldFilter fieldFilter = new FieldFilter();
							fieldFilter.Name = text;
							obj = fieldFilter;
						}
						else
						{
							ScanFilter scanFilter = new ScanFilter();
							scanFilter.Name = text;
							obj = scanFilter;
						}
						PathFilter item = (PathFilter)obj;
						filters.Add(item);
						flag = false;
					}
					if (_currentIndex + 1 < _expression.Length && _expression[_currentIndex + 1] == '.')
					{
						flag = true;
						_currentIndex++;
					}
					_currentIndex++;
					currentPartStartIndex = _currentIndex;
					flag2 = false;
					flag3 = true;
					break;
				default:
					if (query && (c == '=' || c == '<' || c == '!' || c == '>' || c == '|' || c == '&'))
					{
						flag4 = true;
						break;
					}
					if (flag2)
					{
						throw new JsonException("Unexpected character following indexer: " + c);
					}
					_currentIndex++;
					break;
				}
			}
			bool flag5 = _currentIndex == _expression.Length;
			if (_currentIndex > currentPartStartIndex)
			{
				string text2 = _expression.Substring(currentPartStartIndex, _currentIndex - currentPartStartIndex).TrimEnd(new char[0]);
				if (text2 == "*")
				{
					text2 = null;
				}
				object obj3;
				if (!flag)
				{
					FieldFilter fieldFilter3 = new FieldFilter();
					fieldFilter3.Name = text2;
					obj3 = fieldFilter3;
				}
				else
				{
					ScanFilter scanFilter3 = new ScanFilter();
					scanFilter3.Name = text2;
					obj3 = scanFilter3;
				}
				PathFilter item3 = (PathFilter)obj3;
				filters.Add(item3);
			}
			else if (flag3 && (flag5 || query))
			{
				throw new JsonException("Unexpected end while parsing path.");
			}
			return flag5;
		}

		private PathFilter ParseIndexer(char indexerOpenChar)
		{
			_currentIndex++;
			char indexerCloseChar = ((indexerOpenChar == '[') ? ']' : ')');
			EnsureLength("Path ended with open indexer.");
			EatWhitespace();
			if (_expression[_currentIndex] == '\'')
			{
				return ParseQuotedField(indexerCloseChar);
			}
			if (_expression[_currentIndex] == '?')
			{
				return ParseQuery(indexerCloseChar);
			}
			return ParseArrayIndexer(indexerCloseChar);
		}

		private PathFilter ParseArrayIndexer(char indexerCloseChar)
		{
			int currentIndex = _currentIndex;
			int? num = null;
			List<int> list = null;
			int num2 = 0;
			int? start = null;
			int? end = null;
			int? step = null;
			while (_currentIndex < _expression.Length)
			{
				char c = _expression[_currentIndex];
				if (c == ' ')
				{
					num = _currentIndex;
					EatWhitespace();
					continue;
				}
				if (c == indexerCloseChar)
				{
					int num3 = (num ?? _currentIndex) - currentIndex;
					if (list != null)
					{
						if (num3 == 0)
						{
							throw new JsonException("Array index expected.");
						}
						string value = _expression.Substring(currentIndex, num3);
						int item = Convert.ToInt32(value, CultureInfo.InvariantCulture);
						list.Add(item);
						ArrayMultipleIndexFilter arrayMultipleIndexFilter = new ArrayMultipleIndexFilter();
						arrayMultipleIndexFilter.Indexes = list;
						return arrayMultipleIndexFilter;
					}
					if (num2 > 0)
					{
						if (num3 > 0)
						{
							string value2 = _expression.Substring(currentIndex, num3);
							int value3 = Convert.ToInt32(value2, CultureInfo.InvariantCulture);
							if (num2 == 1)
							{
								end = value3;
							}
							else
							{
								step = value3;
							}
						}
						ArraySliceFilter arraySliceFilter = new ArraySliceFilter();
						arraySliceFilter.Start = start;
						arraySliceFilter.End = end;
						arraySliceFilter.Step = step;
						return arraySliceFilter;
					}
					if (num3 == 0)
					{
						throw new JsonException("Array index expected.");
					}
					string value4 = _expression.Substring(currentIndex, num3);
					int value5 = Convert.ToInt32(value4, CultureInfo.InvariantCulture);
					ArrayIndexFilter arrayIndexFilter = new ArrayIndexFilter();
					arrayIndexFilter.Index = value5;
					return arrayIndexFilter;
				}
				switch (c)
				{
				case ',':
				{
					int num5 = (num ?? _currentIndex) - currentIndex;
					if (num5 == 0)
					{
						throw new JsonException("Array index expected.");
					}
					if (list == null)
					{
						list = new List<int>();
					}
					string value8 = _expression.Substring(currentIndex, num5);
					list.Add(Convert.ToInt32(value8, CultureInfo.InvariantCulture));
					_currentIndex++;
					EatWhitespace();
					currentIndex = _currentIndex;
					num = null;
					break;
				}
				case '*':
					_currentIndex++;
					EnsureLength("Path ended with open indexer.");
					EatWhitespace();
					if (_expression[_currentIndex] != indexerCloseChar)
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					return new ArrayIndexFilter();
				case ':':
				{
					int num4 = (num ?? _currentIndex) - currentIndex;
					if (num4 > 0)
					{
						string value6 = _expression.Substring(currentIndex, num4);
						int value7 = Convert.ToInt32(value6, CultureInfo.InvariantCulture);
						switch (num2)
						{
						case 0:
							start = value7;
							break;
						case 1:
							end = value7;
							break;
						default:
							step = value7;
							break;
						}
					}
					num2++;
					_currentIndex++;
					EatWhitespace();
					currentIndex = _currentIndex;
					num = null;
					break;
				}
				default:
					if (!char.IsDigit(c) && c != '-')
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					if (num.HasValue)
					{
						throw new JsonException("Unexpected character while parsing path indexer: " + c);
					}
					_currentIndex++;
					break;
				}
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private void EatWhitespace()
		{
			while (_currentIndex < _expression.Length && _expression[_currentIndex] == ' ')
			{
				_currentIndex++;
			}
		}

		private PathFilter ParseQuery(char indexerCloseChar)
		{
			_currentIndex++;
			EnsureLength("Path ended with open indexer.");
			if (_expression[_currentIndex] != '(')
			{
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			_currentIndex++;
			QueryExpression expression = ParseExpression();
			_currentIndex++;
			EnsureLength("Path ended with open indexer.");
			EatWhitespace();
			if (_expression[_currentIndex] != indexerCloseChar)
			{
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			QueryFilter queryFilter = new QueryFilter();
			queryFilter.Expression = expression;
			return queryFilter;
		}

		private QueryExpression ParseExpression()
		{
			QueryExpression queryExpression = null;
			CompositeExpression compositeExpression = null;
			while (_currentIndex < _expression.Length)
			{
				EatWhitespace();
				if (_expression[_currentIndex] != '@')
				{
					throw new JsonException("Unexpected character while parsing path query: " + _expression[_currentIndex]);
				}
				_currentIndex++;
				List<PathFilter> list = new List<PathFilter>();
				if (ParsePath(list, _currentIndex, query: true))
				{
					throw new JsonException("Path ended with open query.");
				}
				EatWhitespace();
				EnsureLength("Path ended with open query.");
				object value = null;
				QueryOperator queryOperator;
				if (_expression[_currentIndex] == ')' || _expression[_currentIndex] == '|' || _expression[_currentIndex] == '&')
				{
					queryOperator = QueryOperator.Exists;
				}
				else
				{
					queryOperator = ParseOperator();
					EatWhitespace();
					EnsureLength("Path ended with open query.");
					value = ParseValue();
					EatWhitespace();
					EnsureLength("Path ended with open query.");
				}
				BooleanQueryExpression booleanQueryExpression = new BooleanQueryExpression();
				booleanQueryExpression.Path = list;
				booleanQueryExpression.Operator = queryOperator;
				booleanQueryExpression.Value = ((queryOperator != QueryOperator.Exists) ? new JValue(value) : null);
				BooleanQueryExpression booleanQueryExpression2 = booleanQueryExpression;
				if (_expression[_currentIndex] == ')')
				{
					if (compositeExpression != null)
					{
						compositeExpression.Expressions.Add(booleanQueryExpression2);
						return queryExpression;
					}
					return booleanQueryExpression2;
				}
				if (_expression[_currentIndex] == '&' && Match("&&"))
				{
					if (compositeExpression == null || compositeExpression.Operator != QueryOperator.And)
					{
						CompositeExpression compositeExpression2 = new CompositeExpression();
						compositeExpression2.Operator = QueryOperator.And;
						CompositeExpression compositeExpression3 = compositeExpression2;
						compositeExpression?.Expressions.Add(compositeExpression3);
						compositeExpression = compositeExpression3;
						if (queryExpression == null)
						{
							queryExpression = compositeExpression;
						}
					}
					compositeExpression.Expressions.Add(booleanQueryExpression2);
				}
				if (_expression[_currentIndex] != '|' || !Match("||"))
				{
					continue;
				}
				if (compositeExpression == null || compositeExpression.Operator != QueryOperator.Or)
				{
					CompositeExpression compositeExpression4 = new CompositeExpression();
					compositeExpression4.Operator = QueryOperator.Or;
					CompositeExpression compositeExpression5 = compositeExpression4;
					compositeExpression?.Expressions.Add(compositeExpression5);
					compositeExpression = compositeExpression5;
					if (queryExpression == null)
					{
						queryExpression = compositeExpression;
					}
				}
				compositeExpression.Expressions.Add(booleanQueryExpression2);
			}
			throw new JsonException("Path ended with open query.");
		}

		private object ParseValue()
		{
			char c = _expression[_currentIndex];
			if (c == '\'')
			{
				return ReadQuotedString();
			}
			if (char.IsDigit(c) || c == '-')
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(c);
				_currentIndex++;
				while (_currentIndex < _expression.Length)
				{
					c = _expression[_currentIndex];
					if (c == ' ' || c == ')')
					{
						string text = stringBuilder.ToString();
						if (text.IndexOfAny(new char[3] { '.', 'E', 'e' }) != -1)
						{
							if (double.TryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result))
							{
								return result;
							}
							throw new JsonException("Could not read query value.");
						}
						if (long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result2))
						{
							return result2;
						}
						throw new JsonException("Could not read query value.");
					}
					stringBuilder.Append(c);
					_currentIndex++;
				}
			}
			else
			{
				switch (c)
				{
				case 't':
					if (Match("true"))
					{
						return true;
					}
					break;
				case 'f':
					if (Match("false"))
					{
						return false;
					}
					break;
				case 'n':
					if (Match("null"))
					{
						return null;
					}
					break;
				}
			}
			throw new JsonException("Could not read query value.");
		}

		private string ReadQuotedString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			_currentIndex++;
			while (_currentIndex < _expression.Length)
			{
				char c = _expression[_currentIndex];
				if (c == '\\' && _currentIndex + 1 < _expression.Length)
				{
					_currentIndex++;
					if (_expression[_currentIndex] == '\'')
					{
						stringBuilder.Append('\'');
					}
					else
					{
						if (_expression[_currentIndex] != '\\')
						{
							throw new JsonException("Unknown escape chracter: \\" + _expression[_currentIndex]);
						}
						stringBuilder.Append('\\');
					}
					_currentIndex++;
				}
				else
				{
					if (c == '\'')
					{
						_currentIndex++;
						return stringBuilder.ToString();
					}
					_currentIndex++;
					stringBuilder.Append(c);
				}
			}
			throw new JsonException("Path ended with an open string.");
		}

		private bool Match(string s)
		{
			int num = _currentIndex;
			foreach (char c in s)
			{
				if (num < _expression.Length && _expression[num] == c)
				{
					num++;
					continue;
				}
				return false;
			}
			_currentIndex = num;
			return true;
		}

		private QueryOperator ParseOperator()
		{
			if (_currentIndex + 1 >= _expression.Length)
			{
				throw new JsonException("Path ended with open query.");
			}
			if (Match("=="))
			{
				return QueryOperator.Equals;
			}
			if (Match("!=") || Match("<>"))
			{
				return QueryOperator.NotEquals;
			}
			if (Match("<="))
			{
				return QueryOperator.LessThanOrEquals;
			}
			if (Match("<"))
			{
				return QueryOperator.LessThan;
			}
			if (Match(">="))
			{
				return QueryOperator.GreaterThanOrEquals;
			}
			if (Match(">"))
			{
				return QueryOperator.GreaterThan;
			}
			throw new JsonException("Could not read query operator.");
		}

		private PathFilter ParseQuotedField(char indexerCloseChar)
		{
			List<string> list = null;
			while (_currentIndex < _expression.Length)
			{
				string text = ReadQuotedString();
				EatWhitespace();
				EnsureLength("Path ended with open indexer.");
				if (_expression[_currentIndex] == indexerCloseChar)
				{
					if (list != null)
					{
						list.Add(text);
						FieldMultipleFilter fieldMultipleFilter = new FieldMultipleFilter();
						fieldMultipleFilter.Names = list;
						return fieldMultipleFilter;
					}
					FieldFilter fieldFilter = new FieldFilter();
					fieldFilter.Name = text;
					return fieldFilter;
				}
				if (_expression[_currentIndex] == ',')
				{
					_currentIndex++;
					EatWhitespace();
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add(text);
					continue;
				}
				throw new JsonException("Unexpected character while parsing path indexer: " + _expression[_currentIndex]);
			}
			throw new JsonException("Path ended with open indexer.");
		}

		private void EnsureLength(string message)
		{
			if (_currentIndex >= _expression.Length)
			{
				throw new JsonException(message);
			}
		}

		internal IEnumerable<JToken> Evaluate(JToken t, bool errorWhenNoMatch)
		{
			return Evaluate(Filters, t, errorWhenNoMatch);
		}

		internal static IEnumerable<JToken> Evaluate(List<PathFilter> filters, JToken t, bool errorWhenNoMatch)
		{
			IEnumerable<JToken> enumerable = new JToken[1] { t };
			foreach (PathFilter filter in filters)
			{
				enumerable = filter.ExecuteFilter(enumerable, errorWhenNoMatch);
			}
			return enumerable;
		}
	}
	internal enum QueryOperator
	{
		None,
		Equals,
		NotEquals,
		Exists,
		LessThan,
		LessThanOrEquals,
		GreaterThan,
		GreaterThanOrEquals,
		And,
		Or
	}
	internal abstract class QueryExpression
	{
		public QueryOperator Operator { get; set; }

		public abstract bool IsMatch(JToken t);
	}
	internal class CompositeExpression : QueryExpression
	{
		public List<QueryExpression> Expressions { get; set; }

		public CompositeExpression()
		{
			Expressions = new List<QueryExpression>();
		}

		public override bool IsMatch(JToken t)
		{
			switch (base.Operator)
			{
			case QueryOperator.And:
				foreach (QueryExpression expression in Expressions)
				{
					if (!expression.IsMatch(t))
					{
						return false;
					}
				}
				return true;
			case QueryOperator.Or:
				foreach (QueryExpression expression2 in Expressions)
				{
					if (expression2.IsMatch(t))
					{
						return true;
					}
				}
				return false;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
	internal class BooleanQueryExpression : QueryExpression
	{
		public List<PathFilter> Path { get; set; }

		public JValue Value { get; set; }

		public override bool IsMatch(JToken t)
		{
			IEnumerable<JToken> enumerable = JPath.Evaluate(Path, t, errorWhenNoMatch: false);
			foreach (JToken item in enumerable)
			{
				JValue jValue = item as JValue;
				switch (base.Operator)
				{
				case QueryOperator.Equals:
					if (jValue != null && jValue.Equals(Value))
					{
						return true;
					}
					break;
				case QueryOperator.NotEquals:
					if (jValue != null && !jValue.Equals(Value))
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThan:
					if (jValue != null && jValue.CompareTo(Value) > 0)
					{
						return true;
					}
					break;
				case QueryOperator.GreaterThanOrEquals:
					if (jValue != null && jValue.CompareTo(Value) >= 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThan:
					if (jValue != null && jValue.CompareTo(Value) < 0)
					{
						return true;
					}
					break;
				case QueryOperator.LessThanOrEquals:
					if (jValue != null && jValue.CompareTo(Value) <= 0)
					{
						return true;
					}
					break;
				case QueryOperator.Exists:
					return true;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			return false;
		}
	}
	internal class QueryFilter : PathFilter
	{
		public QueryExpression Expression { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken t in current)
			{
				foreach (JToken v in (IEnumerable<JToken>)t)
				{
					if (Expression.IsMatch(v))
					{
						yield return v;
					}
				}
			}
		}
	}
	internal class ScanFilter : PathFilter
	{
		public string Name { get; set; }

		public override IEnumerable<JToken> ExecuteFilter(IEnumerable<JToken> current, bool errorWhenNoMatch)
		{
			foreach (JToken root in current)
			{
				if (Name == null)
				{
					yield return root;
				}
				JToken value = root;
				JToken container = root;
				while (true)
				{
					if (container != null && container.HasValues)
					{
						value = container.First;
					}
					else
					{
						while (value != null && value != root && value == value.Parent.Last)
						{
							value = value.Parent;
						}
						if (value == null || value == root)
						{
							break;
						}
						value = value.Next;
					}
					if (value is JProperty e)
					{
						if (e.Name == Name)
						{
							yield return e.Value;
						}
					}
					else if (Name == null)
					{
						yield return value;
					}
					container = value as JContainer;
				}
			}
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	public enum MergeArrayHandling
	{
		Concat,
		Union,
		Replace,
		Merge
	}
}
namespace Newtonsoft.Json
{
	public enum MetadataPropertyHandling
	{
		Default,
		ReadAhead,
		Ignore
	}
}
namespace Newtonsoft.Json.Serialization
{
	public interface ITraceWriter
	{
		TraceLevel LevelFilter { get; }

		void Trace(TraceLevel level, string message, Exception ex);
	}
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		public TraceLevel LevelFilter { get; set; }

		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			return level switch
			{
				TraceLevel.Error => TraceEventType.Error, 
				TraceLevel.Warning => TraceEventType.Warning, 
				TraceLevel.Info => TraceEventType.Information, 
				TraceLevel.Verbose => TraceEventType.Verbose, 
				_ => throw new ArgumentOutOfRangeException("level"), 
			};
		}

		public void Trace(TraceLevel level, string message, Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache eventCache = new TraceEventCache();
			TraceEventType traceEventType = GetTraceEventType(level);
			foreach (TraceListener listener in System.Diagnostics.Trace.Listeners)
			{
				if (!listener.IsThreadSafe)
				{
					lock (listener)
					{
						listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
					}
				}
				else
				{
					listener.TraceEvent(eventCache, "Newtonsoft.Json", traceEventType, 0, message);
				}
				if (System.Diagnostics.Trace.AutoFlush)
				{
					listener.Flush();
				}
			}
		}
	}
	public interface IValueProvider
	{
		void SetValue(object target, object value);

		object GetValue(object target);
	}
	public class ExpressionValueProvider : IValueProvider
	{
		private readonly MemberInfo _memberInfo;

		private Func<object, object> _getter;

		private Action<object, object> _setter;

		public ExpressionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			_memberInfo = memberInfo;
		}

		public void SetValue(object target, object value)
		{
			try
			{
				if (_setter == null)
				{
					_setter = ExpressionReflectionDelegateFactory.Instance.CreateSet<object>(_memberInfo);
				}
				_setter(target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}

		public object GetValue(object target)
		{
			try
			{
				if (_getter == null)
				{
					_getter = ExpressionReflectionDelegateFactory.Instance.CreateGet<object>(_memberInfo);
				}
				return _getter(target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}
	}
	public interface IAttributeProvider
	{
		IList<Attribute> GetAttributes(bool inherit);

		IList<Attribute> GetAttributes(Type attributeType, bool inherit);
	}
	public abstract class JsonContract
	{
		internal bool IsNullable;

		internal bool IsConvertable;

		internal bool IsEnum;

		internal Type NonNullableUnderlyingType;

		internal ReadType InternalReadType;

		internal JsonContractType ContractType;

		internal bool IsReadOnlyOrFixedSize;

		internal bool IsSealed;

		internal bool IsInstantiable;

		private List<SerializationCallback> _onDeserializedCallbacks;

		private IList<SerializationCallback> _onDeserializingCallbacks;

		private IList<SerializationCallback> _onSerializedCallbacks;

		private IList<SerializationCallback> _onSerializingCallbacks;

		private IList<SerializationErrorCallback> _onErrorCallbacks;

		private Type _createdType;

		public Type UnderlyingType { get; private set; }

		public Type CreatedType
		{
			get
			{
				return _createdType;
			}
			set
			{
				_createdType = value;
				IsSealed = _createdType.IsSealed();
				IsInstantiable = !_createdType.IsInterface() && !_createdType.IsAbstract();
			}
		}

		public bool? IsReference { get; set; }

		public JsonConverter Converter { get; set; }

		internal JsonConverter InternalConverter { get; set; }

		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (_onDeserializedCallbacks == null)
				{
					_onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return _onDeserializedCallbacks;
			}
		}

		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (_onDeserializingCallbacks == null)
				{
					_onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return _onDeserializingCallbacks;
			}
		}

		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (_onSerializedCallbacks == null)
				{
					_onSerializedCallbacks = new List<SerializationCallback>();
				}
				return _onSerializedCallbacks;
			}
		}

		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (_onSerializingCallbacks == null)
				{
					_onSerializingCallbacks = new List<SerializationCallback>();
				}
				return _onSerializingCallbacks;
			}
		}

		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (_onErrorCallbacks == null)
				{
					_onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return _onErrorCallbacks;
			}
		}

		[Obsolete("This property is obsolete and has been replaced by the OnDeserializedCallbacks collection.")]
		public MethodInfo OnDeserialized
		{
			get
			{
				if (OnDeserializedCallbacks.Count <= 0)
				{
					return null;
				}
				return OnDeserializedCallbacks[0].Method();
			}
			set
			{
				OnDeserializedCallbacks.Clear();
				OnDeserializedCallbacks.Add(CreateSerializationCallback(value));
			}
		}

		[Obsolete("This property is obsolete and has been replaced by the OnDeserializingCallbacks collection.")]
		public MethodInfo OnDeserializing
		{
			get
			{
				if (OnDeserializingCallbacks.Count <= 0)
				{
					return null;
				}
				return OnDeserializingCallbacks[0].Method();
			}
			set
			{
				OnDeserializingCallbacks.Clear();
				OnDeserializingCallbacks.Add(CreateSerializationCallback(value));
			}
		}

		[Obsolete("This property is obsolete and has been replaced by the OnSerializedCallbacks collection.")]
		public MethodInfo OnSerialized
		{
			get
			{
				if (OnSerializedCallbacks.Count <= 0)
				{
					return null;
				}
				return OnSerializedCallbacks[0].Method();
			}
			set
			{
				OnSerializedCallbacks.Clear();
				OnSerializedCallbacks.Add(CreateSerializationCallback(value));
			}
		}

		[Obsolete("This property is obsolete and has been replaced by the OnSerializingCallbacks collection.")]
		public MethodInfo OnSerializing
		{
			get
			{
				if (OnSerializingCallbacks.Count <= 0)
				{
					return null;
				}
				return OnSerializingCallbacks[0].Method();
			}
			set
			{
				OnSerializingCallbacks.Clear();
				OnSerializingCallbacks.Add(CreateSerializationCallback(value));
			}
		}

		[Obsolete("This property is obsolete and has been replaced by the OnErrorCallbacks collection.")]
		public MethodInfo OnError
		{
			get
			{
				if (OnErrorCallbacks.Count <= 0)
				{
					return null;
				}
				return OnErrorCallbacks[0].Method();
			}
			set
			{
				OnErrorCallbacks.Clear();
				OnErrorCallbacks.Add(CreateSerializationErrorCallback(value));
			}
		}

		public Func<object> DefaultCreator { get; set; }

		public bool DefaultCreatorNonPublic { get; set; }

		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			UnderlyingType = underlyingType;
			IsNullable = ReflectionUtils.IsNullable(underlyingType);
			NonNullableUnderlyingType = ((IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			CreatedType = NonNullableUnderlyingType;
			IsConvertable = ConvertUtils.IsConvertible(NonNullableUnderlyingType);
			IsEnum = NonNullableUnderlyingType.IsEnum();
			if (NonNullableUnderlyingType == typeof(byte[]))
			{
				InternalReadType = ReadType.ReadAsBytes;
			}
			else if (NonNullableUnderlyingType == typeof(int))
			{
				InternalReadType = ReadType.ReadAsInt32;
			}
			else if (NonNullableUnderlyingType == typeof(decimal))
			{
				InternalReadType = ReadType.ReadAsDecimal;
			}
			else if (NonNullableUnderlyingType == typeof(string))
			{
				InternalReadType = ReadType.ReadAsString;
			}
			else if (NonNullableUnderlyingType == typeof(DateTime))
			{
				InternalReadType = ReadType.ReadAsDateTime;
			}
			else if (NonNullableUnderlyingType == typeof(DateTimeOffset))
			{
				InternalReadType = ReadType.ReadAsDateTimeOffset;
			}
			else
			{
				InternalReadType = ReadType.Read;
			}
		}

		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (_onSerializingCallbacks == null)
			{
				return;
			}
			foreach (SerializationCallback onSerializingCallback in _onSerializingCallbacks)
			{
				onSerializingCallback(o, context);
			}
		}

		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (_onSerializedCallbacks == null)
			{
				return;
			}
			foreach (SerializationCallback onSerializedCallback in _onSerializedCallbacks)
			{
				onSerializedCallback(o, context);
			}
		}

		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (_onDeserializingCallbacks == null)
			{
				return;
			}
			foreach (SerializationCallback onDeserializingCallback in _onDeserializingCallbacks)
			{
				onDeserializingCallback(o, context);
			}
		}

		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (_onDeserializedCallbacks == null)
			{
				return;
			}
			foreach (SerializationCallback onDeserializedCallback in _onDeserializedCallbacks)
			{
				onDeserializedCallback(o, context);
			}
		}

		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (_onErrorCallbacks == null)
			{
				return;
			}
			foreach (SerializationErrorCallback onErrorCallback in _onErrorCallbacks)
			{
				onErrorCallback(o, context, errorContext);
			}
		}

		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[1] { context });
			};
		}

		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[2] { context, econtext });
			};
		}
	}
	public class JsonContainerContract : JsonContract
	{
		private JsonContract _itemContract;

		private JsonContract _finalItemContract;

		internal JsonContract ItemContract
		{
			get
			{
				return _itemContract;
			}
			set
			{
				_itemContract = value;
				if (_itemContract != null)
				{
					_finalItemContract = (_itemContract.UnderlyingType.IsSealed() ? _itemContract : null);
				}
				else
				{
					_finalItemContract = null;
				}
			}
		}

		internal JsonContract FinalItemContract => _finalItemContract;

		public JsonConverter ItemConverter { get; set; }

		public bool? ItemIsReference { get; set; }

		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		internal JsonContainerContract(Type underlyingType)
			: base(underlyingType)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(underlyingType);
			if (cachedAttribute != null)
			{
				if (cachedAttribute.ItemConverterType != null)
				{
					ItemConverter = JsonTypeReflector.CreateJsonConverterInstance(cachedAttribute.ItemConverterType, cachedAttribute.ItemConverterParameters);
				}
				ItemIsReference = cachedAttribute._itemIsReference;
				ItemReferenceLoopHandling = cachedAttribute._itemReferenceLoopHandling;
				ItemTypeNameHandling = cachedAttribute._itemTypeNameHandling;
			}
		}
	}
	public class MemoryTraceWriter : ITraceWriter
	{
		private readonly Queue<string> _traceMessages;

		public TraceLevel LevelFilter { get; set; }

		public MemoryTraceWriter()
		{
			LevelFilter = TraceLevel.Verbose;
			_traceMessages = new Queue<string>();
		}

		public void Trace(TraceLevel level, string message, Exception ex)
		{
			string item = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture) + " " + level.ToString("g") + " " + message;
			if (_traceMessages.Count >= 1000)
			{
				_traceMessages.Dequeue();
			}
			_traceMessages.Enqueue(item);
		}

		public IEnumerable<string> GetTraceMessages()
		{
			return _traceMessages;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string traceMessage in _traceMessages)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(traceMessage);
			}
			return stringBuilder.ToString();
		}
	}
	public class ReflectionAttributeProvider : IAttributeProvider
	{
		private readonly object _attributeProvider;

		public ReflectionAttributeProvider(object attributeProvider)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			_attributeProvider = attributeProvider;
		}

		public IList<Attribute> GetAttributes(bool inherit)
		{
			return ReflectionUtils.GetAttributes(_attributeProvider, null, inherit);
		}

		public IList<Attribute> GetAttributes(Type attributeType, bool inherit)
		{
			return ReflectionUtils.GetAttributes(_attributeProvider, attributeType, inherit);
		}
	}
}
namespace Newtonsoft.Json
{
	public interface IJsonLineInfo
	{
		int LineNumber { get; }

		int LinePosition { get; }

		bool HasLineInfo();
	}
}
namespace Newtonsoft.Json.Serialization
{
	internal class TraceJsonReader : JsonReader, IJsonLineInfo
	{
		private readonly JsonReader _innerReader;

		private readonly JsonTextWriter _textWriter;

		private readonly StringWriter _sw;

		public override int Depth => _innerReader.Depth;

		public override string Path => _innerReader.Path;

		public override char QuoteChar
		{
			get
			{
				return _innerReader.QuoteChar;
			}
			protected internal set
			{
				_innerReader.QuoteChar = value;
			}
		}

		public override JsonToken TokenType => _innerReader.TokenType;

		public override object Value => _innerReader.Value;

		public override Type ValueType => _innerReader.ValueType;

		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (!(_innerReader is IJsonLineInfo jsonLineInfo))
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (!(_innerReader is IJsonLineInfo jsonLineInfo))
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		public TraceJsonReader(JsonReader innerReader)
		{
			_innerReader = innerReader;
			_sw = new StringWriter(CultureInfo.InvariantCulture);
			_textWriter = new JsonTextWriter(_sw);
			_textWriter.Formatting = Formatting.Indented;
		}

		public string GetJson()
		{
			return _sw.ToString();
		}

		public override bool Read()
		{
			bool result = _innerReader.Read();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override int? ReadAsInt32()
		{
			int? result = _innerReader.ReadAsInt32();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override string ReadAsString()
		{
			string result = _innerReader.ReadAsString();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override byte[] ReadAsBytes()
		{
			byte[] result = _innerReader.ReadAsBytes();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? result = _innerReader.ReadAsDecimal();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = _innerReader.ReadAsDateTime();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = _innerReader.ReadAsDateTimeOffset();
			_textWriter.WriteToken(_innerReader, writeChildren: false, writeDateConstructorAsDate: false);
			return result;
		}

		public override void Close()
		{
			_innerReader.Close();
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			if (_innerReader is IJsonLineInfo jsonLineInfo)
			{
				return jsonLineInfo.HasLineInfo();
			}
			return false;
		}
	}
	internal class TraceJsonWriter : JsonWriter
	{
		private readonly JsonWriter _innerWriter;

		private readonly JsonTextWriter _textWriter;

		private readonly StringWriter _sw;

		public TraceJsonWriter(JsonWriter innerWriter)
		{
			_innerWriter = innerWriter;
			_sw = new StringWriter(CultureInfo.InvariantCulture);
			_textWriter = new JsonTextWriter(_sw);
			_textWriter.Formatting = Formatting.Indented;
			_textWriter.Culture = innerWriter.Culture;
			_textWriter.DateFormatHandling = innerWriter.DateFormatHandling;
			_textWriter.DateFormatString = innerWriter.DateFormatString;
			_textWriter.DateTimeZoneHandling = innerWriter.DateTimeZoneHandling;
			_textWriter.FloatFormatHandling = innerWriter.FloatFormatHandling;
		}

		public string GetJson()
		{
			return _sw.ToString();
		}

		public override void WriteValue(decimal value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(bool value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(byte value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(byte? value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(char value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(byte[] value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(DateTime value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(double value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteUndefined()
		{
			_textWriter.WriteUndefined();
			_innerWriter.WriteUndefined();
			base.WriteUndefined();
		}

		public override void WriteNull()
		{
			_textWriter.WriteNull();
			_innerWriter.WriteNull();
			base.WriteUndefined();
		}

		public override void WriteValue(float value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(Guid value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(int value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(long value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				_textWriter.WriteValue(value);
				_innerWriter.WriteValue(value);
				InternalWriteValue(JsonToken.Integer);
			}
			else
			{
				_textWriter.WriteValue(value);
				_innerWriter.WriteValue(value);
				base.WriteValue(value);
			}
		}

		public override void WriteValue(sbyte value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(short value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(string value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(TimeSpan value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(uint value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(ulong value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(Uri value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteValue(ushort value)
		{
			_textWriter.WriteValue(value);
			_innerWriter.WriteValue(value);
			base.WriteValue(value);
		}

		public override void WriteWhitespace(string ws)
		{
			_textWriter.WriteWhitespace(ws);
			_innerWriter.WriteWhitespace(ws);
			base.WriteWhitespace(ws);
		}

		public override void WriteComment(string text)
		{
			_textWriter.WriteComment(text);
			_innerWriter.WriteComment(text);
			base.WriteComment(text);
		}

		public override void WriteStartArray()
		{
			_textWriter.WriteStartArray();
			_innerWriter.WriteStartArray();
			base.WriteStartArray();
		}

		public override void WriteEndArray()
		{
			_textWriter.WriteEndArray();
			_innerWriter.WriteEndArray();
			base.WriteEndArray();
		}

		public override void WriteStartConstructor(string name)
		{
			_textWriter.WriteStartConstructor(name);
			_innerWriter.WriteStartConstructor(name);
			base.WriteStartConstructor(name);
		}

		public override void WriteEndConstructor()
		{
			_textWriter.WriteEndConstructor();
			_innerWriter.WriteEndConstructor();
			base.WriteEndConstructor();
		}

		public override void WritePropertyName(string name)
		{
			_textWriter.WritePropertyName(name);
			_innerWriter.WritePropertyName(name);
			base.WritePropertyName(name);
		}

		public override void WritePropertyName(string name, bool escape)
		{
			_textWriter.WritePropertyName(name, escape);
			_innerWriter.WritePropertyName(name, escape);
			base.WritePropertyName(name);
		}

		public override void WriteStartObject()
		{
			_textWriter.WriteStartObject();
			_innerWriter.WriteStartObject();
			base.WriteStartObject();
		}

		public override void WriteEndObject()
		{
			_textWriter.WriteEndObject();
			_innerWriter.WriteEndObject();
			base.WriteEndObject();
		}

		public override void WriteRaw(string json)
		{
			_textWriter.WriteRaw(json);
			_innerWriter.WriteRaw(json);
			base.WriteRaw(json);
		}

		public override void WriteRawValue(string json)
		{
			_textWriter.WriteRawValue(json);
			_innerWriter.WriteRawValue(json);
			base.WriteRawValue(json);
		}

		public override void Close()
		{
			_textWriter.Close();
			_innerWriter.Close();
			base.Close();
		}

		public override void Flush()
		{
			_textWriter.Flush();
			_innerWriter.Flush();
		}
	}
}
namespace Newtonsoft.Json
{
	public enum StringEscapeHandling
	{
		Default,
		EscapeNonAscii,
		EscapeHtml
	}
}
namespace Newtonsoft.Json.Utilities
{
	internal enum ParserTimeZone
	{
		Unspecified,
		Utc,
		LocalWestOfUtc,
		LocalEastOfUtc
	}
	internal struct DateTimeParser
	{
		private const short MaxFractionDigits = 7;

		public int Year;

		public int Month;

		public int Day;

		public int Hour;

		public int Minute;

		public int Second;

		public int Fraction;

		public int ZoneHour;

		public int ZoneMinute;

		public ParserTimeZone Zone;

		private string _text;

		private int _length;

		private static readonly int[] Power10;

		private static readonly int Lzyyyy;

		private static readonly int Lzyyyy_;

		private static readonly int Lzyyyy_MM;

		private static readonly int Lzyyyy_MM_;

		private static readonly int Lzyyyy_MM_dd;

		private static readonly int Lzyyyy_MM_ddT;

		private static readonly int LzHH;

		private static readonly int LzHH_;

		private static readonly int LzHH_mm;

		private static readonly int LzHH_mm_;

		private static readonly int LzHH_mm_ss;

		private static readonly int Lz_;

		private static readonly int Lz_zz;

		static DateTimeParser()
		{
			Power10 = new int[7] { -1, 10, 100, 1000, 10000, 100000, 1000000 };
			Lzyyyy = "yyyy".Length;
			Lzyyyy_ = "yyyy-".Length;
			Lzyyyy_MM = "yyyy-MM".Length;
			Lzyyyy_MM_ = "yyyy-MM-".Length;
			Lzyyyy_MM_dd = "yyyy-MM-dd".Length;
			Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;
			LzHH = "HH".Length;
			LzHH_ = "HH:".Length;
			LzHH_mm = "HH:mm".Length;
			LzHH_mm_ = "HH:mm:".Length;
			LzHH_mm_ss = "HH:mm:ss".Length;
			Lz_ = "-".Length;
			Lz_zz = "-zz".Length;
		}

		public bool Parse(string text)
		{
			_text = text;
			_length = text.Length;
			if (ParseDate(0) && ParseChar(Lzyyyy_MM_dd, 'T') && ParseTimeAndZoneAndWhitespace(Lzyyyy_MM_ddT))
			{
				return true;
			}
			return false;
		}

		private bool ParseDate(int start)
		{
			if (Parse4Digit(start, out Year) && 1 <= Year && ParseChar(start + Lzyyyy, '-') && Parse2Digit(start + Lzyyyy_, out Month) && 1 <= Month && Month <= 12 && ParseChar(start + Lzyyyy_MM, '-') && Parse2Digit(start + Lzyyyy_MM_, out Day) && 1 <= Day)
			{
				return Day <= DateTime.DaysInMonth(Year, Month);
			}
			return false;
		}

		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			if (ParseTime(ref start))
			{
				return ParseZone(start);
			}
			return false;
		}

		private bool ParseTime(ref int start)
		{
			if (!Parse2Digit(start, out Hour) || Hour >= 24 || !ParseChar(start + LzHH, ':') || !Parse2Digit(start + LzHH_, out Minute) || Minute >= 60 || !ParseChar(start + LzHH_mm, ':') || !Parse2Digit(start + LzHH_mm_, out Second) || Second >= 60)
			{
				return false;
			}
			start += LzHH_mm_ss;
			if (ParseChar(start, '.'))
			{
				Fraction = 0;
				int num = 0;
				while (++start < _length && num < 7)
				{
					int num2 = _text[start] - 48;
					if (num2 < 0 || num2 > 9)
					{
						break;
					}
					Fraction = Fraction * 10 + num2;
					num++;
				}
				if (num < 7)
				{
					if (num == 0)
					{
						return false;
					}
					Fraction *= Power10[7 - num];
				}
			}
			return true;
		}

		private bool ParseZone(int start)
		{
			if (start < _length)
			{
				char c = _text[start];
				if (c == 'Z' || c == 'z')
				{
					Zone = ParserTimeZone.Utc;
					start++;
				}
				else
				{
					if (start + 2 < _length && Parse2Digit(start + Lz_, out ZoneHour) && ZoneHour <= 99)
					{
						switch (c)
						{
						case '-':
							Zone = ParserTimeZone.LocalWestOfUtc;
							start += Lz_zz;
							break;
						case '+':
							Zone = ParserTimeZone.LocalEastOfUtc;
							start += Lz_zz;
							break;
						}
					}
					if (start < _length)
					{
						if (ParseChar(start, ':'))
						{
							start++;
							if (start + 1 < _length && Parse2Digit(start, out ZoneMinute) && ZoneMinute <= 99)
							{
								start += 2;
							}
						}
						else if (start + 1 < _length && Parse2Digit(start, out ZoneMinute) && ZoneMinute <= 99)
						{
							start += 2;
						}
					}
				}
			}
			return start == _length;
		}

		private bool Parse4Digit(int start, out int num)
		{
			if (start + 3 < _length)
			{
				int num2 = _text[start] - 48;
				int num3 = _text[start + 1] - 48;
				int num4 = _text[start + 2] - 48;
				int num5 = _text[start + 3] - 48;
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			num = 0;
			return false;
		}

		private bool Parse2Digit(int start, out int num)
		{
			if (start + 1 < _length)
			{
				int num2 = _text[start] - 48;
				int num3 = _text[start + 1] - 48;
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			num = 0;
			return false;
		}

		private bool ParseChar(int start, char ch)
		{
			if (start < _length)
			{
				return _text[start] == ch;
			}
			return false;
		}
	}
	internal class DynamicProxy<T>
	{
		public virtual IEnumerable<string> GetDynamicMemberNames(T instance)
		{
			return new string[0];
		}

		public virtual bool TryBinaryOperation(T instance, BinaryOperationBinder binder, object arg, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryConvert(T instance, ConvertBinder binder, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryCreateInstance(T instance, CreateInstanceBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryDeleteIndex(T instance, DeleteIndexBinder binder, object[] indexes)
		{
			return false;
		}

		public virtual bool TryDeleteMember(T instance, DeleteMemberBinder binder)
		{
			return false;
		}

		public virtual bool TryGetIndex(T instance, GetIndexBinder binder, object[] indexes, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryGetMember(T instance, GetMemberBinder binder, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryInvoke(T instance, InvokeBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TryInvokeMember(T instance, InvokeMemberBinder binder, object[] args, out object result)
		{
			result = null;
			return false;
		}

		public virtual bool TrySetIndex(T instance, SetIndexBinder binder, object[] indexes, object value)
		{
			return false;
		}

		public virtual bool TrySetMember(T instance, SetMemberBinder binder, object value)
		{
			return false;
		}

		public virtual bool TryUnaryOperation(T instance, UnaryOperationBinder binder, out object result)
		{
			result = null;
			return false;
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	public interface IJEnumerable<out T> : IEnumerable<T>, IEnumerable where T : JToken
	{
		IJEnumerable<JToken> this[object key] { get; }
	}
	public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable, IDynamicMetaObjectProvider
	{
		private class LineInfoAnnotation
		{
			internal readonly int LineNumber;

			internal readonly int LinePosition;

			public LineInfoAnnotation(int lineNumber, int linePosition)
			{
				LineNumber = lineNumber;
				LinePosition = linePosition;
			}
		}

		private static JTokenEqualityComparer _equalityComparer;

		private JContainer _parent;

		private JToken _previous;

		private JToken _next;

		private object _annotations;

		private static readonly JTokenType[] BooleanTypes = new JTokenType[6]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		private static readonly JTokenType[] NumberTypes = new JTokenType[6]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean
		};

		private static readonly JTokenType[] BigIntegerTypes = new JTokenType[7]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes
		};

		private static readonly JTokenType[] StringTypes = new JTokenType[11]
		{
			JTokenType.Date,
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Boolean,
			JTokenType.Bytes,
			JTokenType.Guid,
			JTokenType.TimeSpan,
			JTokenType.Uri
		};

		private static readonly JTokenType[] GuidTypes = new JTokenType[5]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Guid,
			JTokenType.Bytes
		};

		private static readonly JTokenType[] TimeSpanTypes = new JTokenType[4]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.TimeSpan
		};

		private static readonly JTokenType[] UriTypes = new JTokenType[4]
		{
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Uri
		};

		private static readonly JTokenType[] CharTypes = new JTokenType[5]
		{
			JTokenType.Integer,
			JTokenType.Float,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		private static readonly JTokenType[] DateTimeTypes = new JTokenType[4]
		{
			JTokenType.Date,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw
		};

		private static readonly JTokenType[] BytesTypes = new JTokenType[5]
		{
			JTokenType.Bytes,
			JTokenType.String,
			JTokenType.Comment,
			JTokenType.Raw,
			JTokenType.Integer
		};

		public static JTokenEqualityComparer EqualityComparer
		{
			get
			{
				if (_equalityComparer == null)
				{
					_equalityComparer = new JTokenEqualityComparer();
				}
				return _equalityComparer;
			}
		}

		public JContainer Parent
		{
			[DebuggerStepThrough]
			get
			{
				return _parent;
			}
			internal set
			{
				_parent = value;
			}
		}

		public JToken Root
		{
			get
			{
				JContainer parent = Parent;
				if (parent == null)
				{
					return this;
				}
				while (parent.Parent != null)
				{
					parent = parent.Parent;
				}
				return parent;
			}
		}

		public abstract JTokenType Type { get; }

		public abstract bool HasValues { get; }

		public JToken Next
		{
			get
			{
				return _next;
			}
			internal set
			{
				_next = value;
			}
		}

		public JToken Previous
		{
			get
			{
				return _previous;
			}
			internal set
			{
				_previous = value;
			}
		}

		public string Path
		{
			get
			{
				if (Parent == null)
				{
					return string.Empty;
				}
				IList<JToken> list = AncestorsAndSelf().Reverse().ToList();
				IList<JsonPosition> list2 = new List<JsonPosition>();
				for (int i = 0; i < list.Count; i++)
				{
					JToken jToken = list[i];
					JToken jToken2 = null;
					if (i + 1 < list.Count)
					{
						jToken2 = list[i + 1];
					}
					else if (list[i].Type == JTokenType.Property)
					{
						jToken2 = list[i];
					}
					if (jToken2 != null)
					{
						switch (jToken.Type)
						{
						case JTokenType.Property:
						{
							JProperty jProperty = (JProperty)jToken;
							list2.Add(new JsonPosition(JsonContainerType.Object)
							{
								PropertyName = jProperty.Name
							});
							break;
						}
						case JTokenType.Array:
						case JTokenType.Constructor:
						{
							int position = ((IList<JToken>)jToken).IndexOf(jToken2);
							list2.Add(new JsonPosition(JsonContainerType.Array)
							{
								Position = position
							});
							break;
						}
						}
					}
				}
				return JsonPosition.BuildPath(list2);
			}
		}

		public virtual JToken this[object key]
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
			set
			{
				throw new InvalidOperationException("Cannot set child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
		}

		public virtual JToken First
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
		}

		public virtual JToken Last
		{
			get
			{
				throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
		}

		IJEnumerable<JToken> IJEnumerable<JToken>.this[object key] => this[key];

		int IJsonLineInfo.LineNumber => Annotation<LineInfoAnnotation>()?.LineNumber ?? 0;

		int IJsonLineInfo.LinePosition => Annotation<LineInfoAnnotation>()?.LinePosition ?? 0;

		internal abstract JToken CloneToken();

		internal abstract bool DeepEquals(JToken node);

		public static bool DeepEquals(JToken t1, JToken t2)
		{
			if (t1 != t2)
			{
				if (t1 != null && t2 != null)
				{
					return t1.DeepEquals(t2);
				}
				return false;
			}
			return true;
		}

		internal JToken()
		{
		}

		public void AddAfterSelf(object content)
		{
			if (_parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int num = _parent.IndexOfItem(this);
			_parent.AddInternal(num + 1, content, skipParentCheck: false);
		}

		public void AddBeforeSelf(object content)
		{
			if (_parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			int index = _parent.IndexOfItem(this);
			_parent.AddInternal(index, content, skipParentCheck: false);
		}

		public IEnumerable<JToken> Ancestors()
		{
			return GetAncestors(self: false);
		}

		public IEnumerable<JToken> AncestorsAndSelf()
		{
			return GetAncestors(self: true);
		}

		internal IEnumerable<JToken> GetAncestors(bool self)
		{
			for (JToken current = (self ? this : Parent); current != null; current = current.Parent)
			{
				yield return current;
			}
		}

		public IEnumerable<JToken> AfterSelf()
		{
			if (Parent != null)
			{
				for (JToken o = Next; o != null; o = o.Next)
				{
					yield return o;
				}
			}
		}

		public IEnumerable<JToken> BeforeSelf()
		{
			for (JToken o = Parent.First; o != this; o = o.Next)
			{
				yield return o;
			}
		}

		public virtual T Value<T>(object key)
		{
			JToken jToken = this[key];
			if (jToken != null)
			{
				return jToken.Convert<JToken, T>();
			}
			return default(T);
		}

		public virtual JEnumerable<JToken> Children()
		{
			return JEnumerable<JToken>.Empty;
		}

		public JEnumerable<T> Children<T>() where T : JToken
		{
			return new JEnumerable<T>(Children().OfType<T>());
		}

		public virtual IEnumerable<T> Values<T>()
		{
			throw new InvalidOperationException("Cannot access child value on {0}.".FormatWith(CultureInfo.InvariantCulture, GetType()));
		}

		public void Remove()
		{
			if (_parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			_parent.RemoveItem(this);
		}

		public void Replace(JToken value)
		{
			if (_parent == null)
			{
				throw new InvalidOperationException("The parent is missing.");
			}
			_parent.ReplaceItem(this, value);
		}

		public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

		public override string ToString()
		{
			return ToString(Formatting.Indented);
		}

		public string ToString(Formatting formatting, params JsonConverter[] converters)
		{
			using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.Formatting = formatting;
			WriteTo(jsonTextWriter, converters);
			return stringWriter.ToString();
		}

		private static JValue EnsureValue(JToken value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value is JProperty)
			{
				value = ((JProperty)value).Value;
			}
			return value as JValue;
		}

		private static string GetType(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			if (token is JProperty)
			{
				token = ((JProperty)token).Value;
			}
			return token.Type.ToString();
		}

		private static bool ValidateToken(JToken o, JTokenType[] validTypes, bool nullable)
		{
			if (Array.IndexOf(validTypes, o.Type) == -1)
			{
				if (nullable)
				{
					if (o.Type != JTokenType.Null)
					{
						return o.Type == JTokenType.Undefined;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		public static explicit operator bool(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, BooleanTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return Convert.ToBoolean((int)(BigInteger)jValue.Value);
			}
			return Convert.ToBoolean(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator DateTimeOffset(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, DateTimeTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is DateTimeOffset)
			{
				return (DateTimeOffset)jValue.Value;
			}
			if (jValue.Value is string)
			{
				return DateTimeOffset.Parse((string)jValue.Value, CultureInfo.InvariantCulture);
			}
			return new DateTimeOffset(Convert.ToDateTime(jValue.Value, CultureInfo.InvariantCulture));
		}

		public static explicit operator bool?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, BooleanTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Boolean.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return Convert.ToBoolean((int)(BigInteger)jValue.Value);
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToBoolean(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator long(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (long)(BigInteger)jValue.Value;
			}
			return Convert.ToInt64(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator DateTime?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, DateTimeTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is DateTimeOffset)
			{
				return ((DateTimeOffset)jValue.Value).DateTime;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToDateTime(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator DateTimeOffset?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, DateTimeTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to DateTimeOffset.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			if (jValue.Value is DateTimeOffset)
			{
				return (DateTimeOffset?)jValue.Value;
			}
			if (jValue.Value is string)
			{
				return DateTimeOffset.Parse((string)jValue.Value, CultureInfo.InvariantCulture);
			}
			return new DateTimeOffset(Convert.ToDateTime(jValue.Value, CultureInfo.InvariantCulture));
		}

		public static explicit operator decimal?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (decimal)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToDecimal(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator double?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (double)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToDouble(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator char?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, CharTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (char)(ushort)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToChar(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator int(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (int)(BigInteger)jValue.Value;
			}
			return Convert.ToInt32(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator short(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (short)(BigInteger)jValue.Value;
			}
			return Convert.ToInt16(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator ushort(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (ushort)(BigInteger)jValue.Value;
			}
			return Convert.ToUInt16(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator char(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, CharTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Char.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (char)(ushort)(BigInteger)jValue.Value;
			}
			return Convert.ToChar(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator byte(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (byte)(BigInteger)jValue.Value;
			}
			return Convert.ToByte(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator sbyte(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (sbyte)(BigInteger)jValue.Value;
			}
			return Convert.ToSByte(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator int?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Int32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (int)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToInt32(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator short?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Int16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (short)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToInt16(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator ushort?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to UInt16.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (ushort)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToUInt16(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator byte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Byte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (byte)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToByte(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator sbyte?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to SByte.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (sbyte)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToSByte(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator DateTime(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, DateTimeTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to DateTime.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is DateTimeOffset)
			{
				return ((DateTimeOffset)jValue.Value).DateTime;
			}
			return Convert.ToDateTime(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator long?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Int64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (long)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToInt64(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator float?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (float)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToSingle(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator decimal(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Decimal.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (decimal)(BigInteger)jValue.Value;
			}
			return Convert.ToDecimal(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator uint?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (uint)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToUInt32(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator ulong?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (ulong)(BigInteger)jValue.Value;
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return Convert.ToUInt64(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator double(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Double.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (double)(BigInteger)jValue.Value;
			}
			return Convert.ToDouble(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator float(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Single.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (float)(BigInteger)jValue.Value;
			}
			return Convert.ToSingle(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator string(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, StringTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to String.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			if (jValue.Value is byte[])
			{
				return Convert.ToBase64String((byte[])jValue.Value);
			}
			if (jValue.Value is BigInteger)
			{
				return ((BigInteger)jValue.Value).ToString(CultureInfo.InvariantCulture);
			}
			return Convert.ToString(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator uint(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to UInt32.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (uint)(BigInteger)jValue.Value;
			}
			return Convert.ToUInt32(jValue.Value, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static explicit operator ulong(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, NumberTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to UInt64.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is BigInteger)
			{
				return (ulong)(BigInteger)jValue.Value;
			}
			return Convert.ToUInt64(jValue.Value, CultureInfo.InvariantCulture);
		}

		public static explicit operator byte[](JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, BytesTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is string)
			{
				return Convert.FromBase64String(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
			}
			if (jValue.Value is BigInteger)
			{
				return ((BigInteger)jValue.Value).ToByteArray();
			}
			if (jValue.Value is byte[])
			{
				return (byte[])jValue.Value;
			}
			throw new ArgumentException("Can not convert {0} to byte array.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
		}

		public static explicit operator Guid(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, GuidTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value is byte[])
			{
				return new Guid((byte[])jValue.Value);
			}
			if (!(jValue.Value is Guid))
			{
				return new Guid(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
			}
			return (Guid)jValue.Value;
		}

		public static explicit operator Guid?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, GuidTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Guid.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			if (jValue.Value is byte[])
			{
				return new Guid((byte[])jValue.Value);
			}
			return (jValue.Value is Guid) ? ((Guid)jValue.Value) : new Guid(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
		}

		public static explicit operator TimeSpan(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, TimeSpanTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (!(jValue.Value is TimeSpan))
			{
				return ConvertUtils.ParseTimeSpan(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
			}
			return (TimeSpan)jValue.Value;
		}

		public static explicit operator TimeSpan?(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, TimeSpanTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to TimeSpan.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return (jValue.Value is TimeSpan) ? ((TimeSpan)jValue.Value) : ConvertUtils.ParseTimeSpan(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
		}

		public static explicit operator Uri(JToken value)
		{
			if (value == null)
			{
				return null;
			}
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, UriTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to Uri.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			if (!(jValue.Value is Uri))
			{
				return new Uri(Convert.ToString(jValue.Value, CultureInfo.InvariantCulture));
			}
			return (Uri)jValue.Value;
		}

		private static BigInteger ToBigInteger(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, BigIntegerTypes, nullable: false))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			return ConvertUtils.ToBigInteger(jValue.Value);
		}

		private static BigInteger? ToBigIntegerNullable(JToken value)
		{
			JValue jValue = EnsureValue(value);
			if (jValue == null || !ValidateToken(jValue, BigIntegerTypes, nullable: true))
			{
				throw new ArgumentException("Can not convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, GetType(value)));
			}
			if (jValue.Value == null)
			{
				return null;
			}
			return ConvertUtils.ToBigInteger(jValue.Value);
		}

		public static implicit operator JToken(bool value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(DateTimeOffset value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(byte value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(byte? value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(sbyte? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(bool? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(long value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(DateTime? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(DateTimeOffset? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(decimal? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(double? value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(short value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(ushort value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(int value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(int? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(DateTime value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(long? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(float? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(decimal value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(short? value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(ushort? value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(uint? value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(ulong? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(double value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(float value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(string value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(uint value)
		{
			return new JValue(value);
		}

		[CLSCompliant(false)]
		public static implicit operator JToken(ulong value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(byte[] value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(Uri value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(TimeSpan value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(TimeSpan? value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(Guid value)
		{
			return new JValue(value);
		}

		public static implicit operator JToken(Guid? value)
		{
			return new JValue(value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<JToken>)this).GetEnumerator();
		}

		IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
		{
			return Children().GetEnumerator();
		}

		internal abstract int GetDeepHashCode();

		public JsonReader CreateReader()
		{
			return new JTokenReader(this, Path);
		}

		internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			using JTokenWriter jTokenWriter = new JTokenWriter();
			jsonSerializer.Serialize(jTokenWriter, o);
			return jTokenWriter.Token;
		}

		public static JToken FromObject(object o)
		{
			return FromObjectInternal(o, JsonSerializer.CreateDefault());
		}

		public static JToken FromObject(object o, JsonSerializer jsonSerializer)
		{
			return FromObjectInternal(o, jsonSerializer);
		}

		public T ToObject<T>()
		{
			return (T)ToObject(typeof(T));
		}

		public object ToObject(Type objectType)
		{
			if (JsonConvert.DefaultSettings == null)
			{
				bool isEnum;
				PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(objectType, out isEnum);
				if (isEnum && Type == JTokenType.String)
				{
					Type type = (objectType.IsEnum() ? objectType : Nullable.GetUnderlyingType(objectType));
					try
					{
						return Enum.Parse(type, (string)this, ignoreCase: true);
					}
					catch (Exception innerException)
					{
						throw new ArgumentException("Could not convert '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, (string)this, type.Name), innerException);
					}
				}
				switch (typeCode)
				{
				case PrimitiveTypeCode.BooleanNullable:
					return (bool?)this;
				case PrimitiveTypeCode.Boolean:
					return (bool)this;
				case PrimitiveTypeCode.CharNullable:
					return (char?)this;
				case PrimitiveTypeCode.Char:
					return (char)this;
				case PrimitiveTypeCode.SByte:
					return (sbyte?)this;
				case PrimitiveTypeCode.SByteNullable:
					return (sbyte)this;
				case PrimitiveTypeCode.ByteNullable:
					return (byte?)this;
				case PrimitiveTypeCode.Byte:
					return (byte)this;
				case PrimitiveTypeCode.Int16Nullable:
					return (short?)this;
				case PrimitiveTypeCode.Int16:
					return (short)this;
				case PrimitiveTypeCode.UInt16Nullable:
					return (ushort?)this;
				case PrimitiveTypeCode.UInt16:
					return (ushort)this;
				case PrimitiveTypeCode.Int32Nullable:
					return (int?)this;
				case PrimitiveTypeCode.Int32:
					return (int)this;
				case PrimitiveTypeCode.UInt32Nullable:
					return (uint?)this;
				case PrimitiveTypeCode.UInt32:
					return (uint)this;
				case PrimitiveTypeCode.Int64Nullable:
					return (long?)this;
				case PrimitiveTypeCode.Int64:
					return (long)this;
				case PrimitiveTypeCode.UInt64Nullable:
					return (ulong?)this;
				case PrimitiveTypeCode.UInt64:
					return (ulong)this;
				case PrimitiveTypeCode.SingleNullable:
					return (float?)this;
				case PrimitiveTypeCode.Single:
					return (float)this;
				case PrimitiveTypeCode.DoubleNullable:
					return (double?)this;
				case PrimitiveTypeCode.Double:
					return (double)this;
				case PrimitiveTypeCode.DecimalNullable:
					return (decimal?)this;
				case PrimitiveTypeCode.Decimal:
					return (decimal)this;
				case PrimitiveTypeCode.DateTimeNullable:
					return (DateTime?)this;
				case PrimitiveTypeCode.DateTime:
					return (DateTime)this;
				case PrimitiveTypeCode.DateTimeOffsetNullable:
					return (DateTimeOffset?)this;
				case PrimitiveTypeCode.DateTimeOffset:
					return (DateTimeOffset)this;
				case PrimitiveTypeCode.String:
					return (string)this;
				case PrimitiveTypeCode.GuidNullable:
					return (Guid?)this;
				case PrimitiveTypeCode.Guid:
					return (Guid)this;
				case PrimitiveTypeCode.Uri:
					return (Uri)this;
				case PrimitiveTypeCode.TimeSpanNullable:
					return (TimeSpan?)this;
				case PrimitiveTypeCode.TimeSpan:
					return (TimeSpan)this;
				case PrimitiveTypeCode.BigIntegerNullable:
					return ToBigIntegerNullable(this);
				case PrimitiveTypeCode.BigInteger:
					return ToBigInteger(this);
				}
			}
			return ToObject(objectType, JsonSerializer.CreateDefault());
		}

		public T ToObject<T>(JsonSerializer jsonSerializer)
		{
			return (T)ToObject(typeof(T), jsonSerializer);
		}

		public object ToObject(Type objectType, JsonSerializer jsonSerializer)
		{
			ValidationUtils.ArgumentNotNull(jsonSerializer, "jsonSerializer");
			using JTokenReader reader = new JTokenReader(this);
			return jsonSerializer.Deserialize(reader, objectType);
		}

		public static JToken ReadFrom(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
			}
			IJsonLineInfo lineInfo = reader as IJsonLineInfo;
			switch (reader.TokenType)
			{
			case JsonToken.StartObject:
				return JObject.Load(reader);
			case JsonToken.StartArray:
				return JArray.Load(reader);
			case JsonToken.StartConstructor:
				return JConstructor.Load(reader);
			case JsonToken.PropertyName:
				return JProperty.Load(reader);
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Date:
			case JsonToken.Bytes:
			{
				JValue jValue = new JValue(reader.Value);
				jValue.SetLineInfo(lineInfo);
				return jValue;
			}
			case JsonToken.Comment:
			{
				JValue jValue = JValue.CreateComment(reader.Value.ToString());
				jValue.SetLineInfo(lineInfo);
				return jValue;
			}
			case JsonToken.Null:
			{
				JValue jValue = JValue.CreateNull();
				jValue.SetLineInfo(lineInfo);
				return jValue;
			}
			case JsonToken.Undefined:
			{
				JValue jValue = JValue.CreateUndefined();
				jValue.SetLineInfo(lineInfo);
				return jValue;
			}
			default:
				throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader. Unexpected token: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
		}

		public static JToken Parse(string json)
		{
			using JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JToken result = Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		public static JToken Load(JsonReader reader)
		{
			return ReadFrom(reader);
		}

		internal void SetLineInfo(IJsonLineInfo lineInfo)
		{
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
			}
		}

		internal void SetLineInfo(int lineNumber, int linePosition)
		{
			AddAnnotation(new LineInfoAnnotation(lineNumber, linePosition));
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			return Annotation<LineInfoAnnotation>() != null;
		}

		public JToken SelectToken(string path)
		{
			return SelectToken(path, errorWhenNoMatch: false);
		}

		public JToken SelectToken(string path, bool errorWhenNoMatch)
		{
			JPath jPath = new JPath(path);
			JToken jToken = null;
			foreach (JToken item in jPath.Evaluate(this, errorWhenNoMatch))
			{
				if (jToken != null)
				{
					throw new JsonException("Path returned multiple tokens.");
				}
				jToken = item;
			}
			return jToken;
		}

		public IEnumerable<JToken> SelectTokens(string path)
		{
			return SelectTokens(path, errorWhenNoMatch: false);
		}

		public IEnumerable<JToken> SelectTokens(string path, bool errorWhenNoMatch)
		{
			JPath jPath = new JPath(path);
			return jPath.Evaluate(this, errorWhenNoMatch);
		}

		protected virtual DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JToken>(parameter, this, new DynamicProxy<JToken>(), dontFallbackFirst: true);
		}

		DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(Expression parameter)
		{
			return GetMetaObject(parameter);
		}

		object ICloneable.Clone()
		{
			return DeepClone();
		}

		public JToken DeepClone()
		{
			return CloneToken();
		}

		public void AddAnnotation(object annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			if (_annotations == null)
			{
				_annotations = ((annotation is object[]) ? new object[1] { annotation } : annotation);
				return;
			}
			object[] array = _annotations as object[];
			if (array == null)
			{
				_annotations = new object[2] { _annotations, annotation };
				return;
			}
			int i;
			for (i = 0; i < array.Length && array[i] != null; i++)
			{
			}
			if (i == array.Length)
			{
				Array.Resize(ref array, i * 2);
				_annotations = array;
			}
			array[i] = annotation;
		}

		public T Annotation<T>() where T : class
		{
			if (_annotations != null)
			{
				if (!(_annotations is object[] array))
				{
					return _annotations as T;
				}
				foreach (object obj in array)
				{
					if (obj == null)
					{
						break;
					}
					if (obj is T result)
					{
						return result;
					}
				}
			}
			return null;
		}

		public object Annotation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (_annotations != null)
			{
				if (!(_annotations is object[] array))
				{
					if (type.IsInstanceOfType(_annotations))
					{
						return _annotations;
					}
				}
				else
				{
					foreach (object obj in array)
					{
						if (obj == null)
						{
							break;
						}
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
			}
			return null;
		}

		public IEnumerable<T> Annotations<T>() where T : class
		{
			if (_annotations == null)
			{
				yield break;
			}
			if (_annotations is object[] annotations)
			{
				foreach (object o in annotations)
				{
					if (o != null)
					{
						if (o is T casted)
						{
							yield return casted;
						}
						continue;
					}
					break;
				}
			}
			else if (_annotations is T annotation)
			{
				yield return annotation;
			}
		}

		public IEnumerable<object> Annotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (_annotations == null)
			{
				yield break;
			}
			if (_annotations is object[] annotations)
			{
				foreach (object o in annotations)
				{
					if (o != null)
					{
						if (type.IsInstanceOfType(o))
						{
							yield return o;
						}
						continue;
					}
					break;
				}
			}
			else if (type.IsInstanceOfType(_annotations))
			{
				yield return _annotations;
			}
		}

		public void RemoveAnnotations<T>() where T : class
		{
			if (_annotations == null)
			{
				return;
			}
			if (!(_annotations is object[] array))
			{
				if (_annotations is T)
				{
					_annotations = null;
				}
				return;
			}
			int i = 0;
			int num = 0;
			for (; i < array.Length; i++)
			{
				object obj = array[i];
				if (obj == null)
				{
					break;
				}
				if (!(obj is T))
				{
					array[num++] = obj;
				}
			}
			if (num != 0)
			{
				while (num < i)
				{
					array[num++] = null;
				}
			}
			else
			{
				_annotations = null;
			}
		}

		public void RemoveAnnotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (_annotations == null)
			{
				return;
			}
			if (!(_annotations is object[] array))
			{
				if (type.IsInstanceOfType(_annotations))
				{
					_annotations = null;
				}
				return;
			}
			int i = 0;
			int num = 0;
			for (; i < array.Length; i++)
			{
				object obj = array[i];
				if (obj == null)
				{
					break;
				}
				if (!type.IsInstanceOfType(obj))
				{
					array[num++] = obj;
				}
			}
			if (num != 0)
			{
				while (num < i)
				{
					array[num++] = null;
				}
			}
			else
			{
				_annotations = null;
			}
		}
	}
	public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>, IConvertible
	{
		private class JValueDynamicProxy : DynamicProxy<JValue>
		{
			public override bool TryConvert(JValue instance, ConvertBinder binder, out object result)
			{
				if (binder.Type == typeof(JValue))
				{
					result = instance;
					return true;
				}
				object value = instance.Value;
				if (value == null)
				{
					result = null;
					return ReflectionUtils.IsNullable(binder.Type);
				}
				result = ConvertUtils.Convert(value, CultureInfo.InvariantCulture, binder.Type);
				return true;
			}

			public override bool TryBinaryOperation(JValue instance, BinaryOperationBinder binder, object arg, out object result)
			{
				object objB = ((arg is JValue) ? ((JValue)arg).Value : arg);
				switch (binder.Operation)
				{
				case ExpressionType.Equal:
					result = Compare(instance.Type, instance.Value, objB) == 0;
					return true;
				case ExpressionType.NotEqual:
					result = Compare(instance.Type, instance.Value, objB) != 0;
					return true;
				case ExpressionType.GreaterThan:
					result = Compare(instance.Type, instance.Value, objB) > 0;
					return true;
				case ExpressionType.GreaterThanOrEqual:
					result = Compare(instance.Type, instance.Value, objB) >= 0;
					return true;
				case ExpressionType.LessThan:
					result = Compare(instance.Type, instance.Value, objB) < 0;
					return true;
				case ExpressionType.LessThanOrEqual:
					result = Compare(instance.Type, instance.Value, objB) <= 0;
					return true;
				case ExpressionType.Add:
				case ExpressionType.Divide:
				case ExpressionType.Multiply:
				case ExpressionType.Subtract:
				case ExpressionType.AddAssign:
				case ExpressionType.DivideAssign:
				case ExpressionType.MultiplyAssign:
				case ExpressionType.SubtractAssign:
					if (Operation(binder.Operation, instance.Value, objB, out result))
					{
						result = new JValue(result);
						return true;
					}
					break;
				}
				result = null;
				return false;
			}
		}

		private JTokenType _valueType;

		private object _value;

		public override bool HasValues => false;

		public override JTokenType Type => _valueType;

		public new object Value
		{
			get
			{
				return _value;
			}
			set
			{
				Type type = ((_value != null) ? _value.GetType() : null);
				Type type2 = value?.GetType();
				if (type != type2)
				{
					_valueType = GetValueType(_valueType, value);
				}
				_value = value;
			}
		}

		internal JValue(object value, JTokenType type)
		{
			_value = value;
			_valueType = type;
		}

		public JValue(JValue other)
			: this(other.Value, other.Type)
		{
		}

		public JValue(long value)
			: this(value, JTokenType.Integer)
		{
		}

		public JValue(decimal value)
			: this(value, JTokenType.Float)
		{
		}

		public JValue(char value)
			: this(value, JTokenType.String)
		{
		}

		[CLSCompliant(false)]
		public JValue(ulong value)
			: this(value, JTokenType.Integer)
		{
		}

		public JValue(double value)
			: this(value, JTokenType.Float)
		{
		}

		public JValue(float value)
			: this(value, JTokenType.Float)
		{
		}

		public JValue(DateTime value)
			: this(value, JTokenType.Date)
		{
		}

		public JValue(DateTimeOffset value)
			: this(value, JTokenType.Date)
		{
		}

		public JValue(bool value)
			: this(value, JTokenType.Boolean)
		{
		}

		public JValue(string value)
			: this(value, JTokenType.String)
		{
		}

		public JValue(Guid value)
			: this(value, JTokenType.Guid)
		{
		}

		public JValue(Uri value)
			: this(value, (value != null) ? JTokenType.Uri : JTokenType.Null)
		{
		}

		public JValue(TimeSpan value)
			: this(value, JTokenType.TimeSpan)
		{
		}

		public JValue(object value)
			: this(value, GetValueType(null, value))
		{
		}

		internal override bool DeepEquals(JToken node)
		{
			if (!(node is JValue jValue))
			{
				return false;
			}
			if (jValue == this)
			{
				return true;
			}
			return ValuesEquals(this, jValue);
		}

		private static int CompareBigInteger(BigInteger i1, object i2)
		{
			int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
			if (num != 0)
			{
				return num;
			}
			if (i2 is decimal num2)
			{
				return 0m.CompareTo(Math.Abs(num2 - Math.Truncate(num2)));
			}
			if (i2 is double || i2 is float)
			{
				double num3 = Convert.ToDouble(i2, CultureInfo.InvariantCulture);
				return 0.0.CompareTo(Math.Abs(num3 - Math.Truncate(num3)));
			}
			return num;
		}

		internal static int Compare(JTokenType valueType, object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return 0;
			}
			if (objA != null && objB == null)
			{
				return 1;
			}
			if (objA == null && objB != null)
			{
				return -1;
			}
			switch (valueType)
			{
			case JTokenType.Integer:
				if (objA is BigInteger)
				{
					return CompareBigInteger((BigInteger)objA, objB);
				}
				if (objB is BigInteger)
				{
					return -CompareBigInteger((BigInteger)objB, objA);
				}
				if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
				{
					return Convert.ToDecimal(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, CultureInfo.InvariantCulture));
				}
				if (objA is float || objB is float || objA is double || objB is double)
				{
					return CompareFloat(objA, objB);
				}
				return Convert.ToInt64(objA, CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, CultureInfo.InvariantCulture));
			case JTokenType.Float:
				if (objA is BigInteger)
				{
					return CompareBigInteger((BigInteger)objA, objB);
				}
				if (objB is BigInteger)
				{
					return -CompareBigInteger((BigInteger)objB, objA);
				}
				return CompareFloat(objA, objB);
			case JTokenType.Comment:
			case JTokenType.String:
			case JTokenType.Raw:
			{
				string strA = Convert.ToString(objA, CultureInfo.InvariantCulture);
				string strB = Convert.ToString(objB, CultureInfo.InvariantCulture);
				return string.CompareOrdinal(strA, strB);
			}
			case JTokenType.Boolean:
			{
				bool flag = Convert.ToBoolean(objA, CultureInfo.InvariantCulture);
				bool value3 = Convert.ToBoolean(objB, CultureInfo.InvariantCulture);
				return flag.CompareTo(value3);
			}
			case JTokenType.Date:
			{
				if (objA is DateTime dateTime)
				{
					DateTime value2 = ((!(objB is DateTimeOffset dateTimeOffset)) ? Convert.ToDateTime(objB, CultureInfo.InvariantCulture) : dateTimeOffset.DateTime);
					return dateTime.CompareTo(value2);
				}
				DateTimeOffset dateTimeOffset2 = (DateTimeOffset)objA;
				DateTimeOffset other = ((!(objB is DateTimeOffset)) ? new DateTimeOffset(Convert.ToDateTime(objB, CultureInfo.InvariantCulture)) : ((DateTimeOffset)objB));
				return dateTimeOffset2.CompareTo(other);
			}
			case JTokenType.Bytes:
			{
				if (!(objB is byte[]))
				{
					throw new ArgumentException("Object must be of type byte[].");
				}
				byte[] array = objA as byte[];
				byte[] array2 = objB as byte[];
				if (array == null)
				{
					return -1;
				}
				if (array2 == null)
				{
					return 1;
				}
				return MiscellaneousUtils.ByteArrayCompare(array, array2);
			}
			case JTokenType.Guid:
			{
				if (!(objB is Guid))
				{
					throw new ArgumentException("Object must be of type Guid.");
				}
				Guid guid = (Guid)objA;
				Guid value4 = (Guid)objB;
				return guid.CompareTo(value4);
			}
			case JTokenType.Uri:
			{
				if (!(objB is Uri))
				{
					throw new ArgumentException("Object must be of type Uri.");
				}
				Uri uri = (Uri)objA;
				Uri uri2 = (Uri)objB;
				return Comparer<string>.Default.Compare(uri.ToString(), uri2.ToString());
			}
			case JTokenType.TimeSpan:
			{
				if (!(objB is TimeSpan))
				{
					throw new ArgumentException("Object must be of type TimeSpan.");
				}
				TimeSpan timeSpan = (TimeSpan)objA;
				TimeSpan value = (TimeSpan)objB;
				return timeSpan.CompareTo(value);
			}
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", valueType, "Unexpected value type: {0}".FormatWith(CultureInfo.InvariantCulture, valueType));
			}
		}

		private static int CompareFloat(object objA, object objB)
		{
			double d = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
			double num = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
			if (MathUtils.ApproxEquals(d, num))
			{
				return 0;
			}
			return d.CompareTo(num);
		}

		private static bool Operation(ExpressionType operation, object objA, object objB, out object result)
		{
			if ((objA is string || objB is string) && (operation == ExpressionType.Add || operation == ExpressionType.AddAssign))
			{
				result = objA?.ToString() + objB;
				return true;
			}
			if (objA is BigInteger || objB is BigInteger)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				BigInteger bigInteger = ConvertUtils.ToBigInteger(objA);
				BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
				switch (operation)
				{
				case ExpressionType.Add:
				case ExpressionType.AddAssign:
					result = bigInteger + bigInteger2;
					return true;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractAssign:
					result = bigInteger - bigInteger2;
					return true;
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyAssign:
					result = bigInteger * bigInteger2;
					return true;
				case ExpressionType.Divide:
				case ExpressionType.DivideAssign:
					result = bigInteger / bigInteger2;
					return true;
				}
			}
			else if (objA is ulong || objB is ulong || objA is decimal || objB is decimal)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				decimal num = Convert.ToDecimal(objA, CultureInfo.InvariantCulture);
				decimal num2 = Convert.ToDecimal(objB, CultureInfo.InvariantCulture);
				switch (operation)
				{
				case ExpressionType.Add:
				case ExpressionType.AddAssign:
					result = num + num2;
					return true;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractAssign:
					result = num - num2;
					return true;
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyAssign:
					result = num * num2;
					return true;
				case ExpressionType.Divide:
				case ExpressionType.DivideAssign:
					result = num / num2;
					return true;
				}
			}
			else if (objA is float || objB is float || objA is double || objB is double)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				double num3 = Convert.ToDouble(objA, CultureInfo.InvariantCulture);
				double num4 = Convert.ToDouble(objB, CultureInfo.InvariantCulture);
				switch (operation)
				{
				case ExpressionType.Add:
				case ExpressionType.AddAssign:
					result = num3 + num4;
					return true;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractAssign:
					result = num3 - num4;
					return true;
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyAssign:
					result = num3 * num4;
					return true;
				case ExpressionType.Divide:
				case ExpressionType.DivideAssign:
					result = num3 / num4;
					return true;
				}
			}
			else if (objA is int || objA is uint || objA is long || objA is short || objA is ushort || objA is sbyte || objA is byte || objB is int || objB is uint || objB is long || objB is short || objB is ushort || objB is sbyte || objB is byte)
			{
				if (objA == null || objB == null)
				{
					result = null;
					return true;
				}
				long num5 = Convert.ToInt64(objA, CultureInfo.InvariantCulture);
				long num6 = Convert.ToInt64(objB, CultureInfo.InvariantCulture);
				switch (operation)
				{
				case ExpressionType.Add:
				case ExpressionType.AddAssign:
					result = num5 + num6;
					return true;
				case ExpressionType.Subtract:
				case ExpressionType.SubtractAssign:
					result = num5 - num6;
					return true;
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyAssign:
					result = num5 * num6;
					return true;
				case ExpressionType.Divide:
				case ExpressionType.DivideAssign:
					result = num5 / num6;
					return true;
				}
			}
			result = null;
			return false;
		}

		internal override JToken CloneToken()
		{
			return new JValue(this);
		}

		public static JValue CreateComment(string value)
		{
			return new JValue(value, JTokenType.Comment);
		}

		public static JValue CreateString(string value)
		{
			return new JValue(value, JTokenType.String);
		}

		public static JValue CreateNull()
		{
			return new JValue(null, JTokenType.Null);
		}

		public static JValue CreateUndefined()
		{
			return new JValue(null, JTokenType.Undefined);
		}

		private static JTokenType GetValueType(JTokenType? current, object value)
		{
			if (value == null)
			{
				return JTokenType.Null;
			}
			if (value == DBNull.Value)
			{
				return JTokenType.Null;
			}
			if (value is string)
			{
				return GetStringValueType(current);
			}
			if (value is long || value is int || value is short || value is sbyte || value is ulong || value is uint || value is ushort || value is byte)
			{
				return JTokenType.Integer;
			}
			if (value is Enum)
			{
				return JTokenType.Integer;
			}
			if (value is BigInteger)
			{
				return JTokenType.Integer;
			}
			if (value is double || value is float || value is decimal)
			{
				return JTokenType.Float;
			}
			if (value is DateTime)
			{
				return JTokenType.Date;
			}
			if (value is DateTimeOffset)
			{
				return JTokenType.Date;
			}
			if (value is byte[])
			{
				return JTokenType.Bytes;
			}
			if (value is bool)
			{
				return JTokenType.Boolean;
			}
			if (value is Guid)
			{
				return JTokenType.Guid;
			}
			if (value is Uri)
			{
				return JTokenType.Uri;
			}
			if (value is TimeSpan)
			{
				return JTokenType.TimeSpan;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		private static JTokenType GetStringValueType(JTokenType? current)
		{
			if (!current.HasValue)
			{
				return JTokenType.String;
			}
			JTokenType value = current.Value;
			if (value == JTokenType.Comment || value == JTokenType.String || value == JTokenType.Raw)
			{
				return current.Value;
			}
			return JTokenType.String;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			if (converters != null && converters.Length > 0 && _value != null)
			{
				JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter(converters, _value.GetType());
				if (matchingConverter != null && matchingConverter.CanWrite)
				{
					matchingConverter.WriteJson(writer, _value, JsonSerializer.CreateDefault());
					return;
				}
			}
			switch (_valueType)
			{
			case JTokenType.Comment:
				writer.WriteComment((_value != null) ? _value.ToString() : null);
				break;
			case JTokenType.Raw:
				writer.WriteRawValue((_value != null) ? _value.ToString() : null);
				break;
			case JTokenType.Null:
				writer.WriteNull();
				break;
			case JTokenType.Undefined:
				writer.WriteUndefined();
				break;
			case JTokenType.Integer:
				if (_value is BigInteger)
				{
					writer.WriteValue((BigInteger)_value);
				}
				else
				{
					writer.WriteValue(Convert.ToInt64(_value, CultureInfo.InvariantCulture));
				}
				break;
			case JTokenType.Float:
				if (_value is decimal)
				{
					writer.WriteValue((decimal)_value);
				}
				else if (_value is double)
				{
					writer.WriteValue((double)_value);
				}
				else if (_value is float)
				{
					writer.WriteValue((float)_value);
				}
				else
				{
					writer.WriteValue(Convert.ToDouble(_value, CultureInfo.InvariantCulture));
				}
				break;
			case JTokenType.String:
				writer.WriteValue((_value != null) ? _value.ToString() : null);
				break;
			case JTokenType.Boolean:
				writer.WriteValue(Convert.ToBoolean(_value, CultureInfo.InvariantCulture));
				break;
			case JTokenType.Date:
				if (_value is DateTimeOffset)
				{
					writer.WriteValue((DateTimeOffset)_value);
				}
				else
				{
					writer.WriteValue(Convert.ToDateTime(_value, CultureInfo.InvariantCulture));
				}
				break;
			case JTokenType.Bytes:
				writer.WriteValue((byte[])_value);
				break;
			case JTokenType.Guid:
			case JTokenType.Uri:
			case JTokenType.TimeSpan:
				writer.WriteValue((_value != null) ? _value.ToString() : null);
				break;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", _valueType, "Unexpected token type.");
			}
		}

		internal override int GetDeepHashCode()
		{
			int num = ((_value != null) ? _value.GetHashCode() : 0);
			int valueType = (int)_valueType;
			return valueType.GetHashCode() ^ num;
		}

		private static bool ValuesEquals(JValue v1, JValue v2)
		{
			if (v1 != v2)
			{
				if (v1._valueType == v2._valueType)
				{
					return Compare(v1._valueType, v1._value, v2._value) == 0;
				}
				return false;
			}
			return true;
		}

		public bool Equals(JValue other)
		{
			if (other == null)
			{
				return false;
			}
			return ValuesEquals(this, other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is JValue other)
			{
				return Equals(other);
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			if (_value == null)
			{
				return 0;
			}
			return _value.GetHashCode();
		}

		public override string ToString()
		{
			if (_value == null)
			{
				return string.Empty;
			}
			return _value.ToString();
		}

		public string ToString(string format)
		{
			return ToString(format, CultureInfo.CurrentCulture);
		}

		public string ToString(IFormatProvider formatProvider)
		{
			return ToString(null, formatProvider);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			if (_value == null)
			{
				return string.Empty;
			}
			if (_value is IFormattable formattable)
			{
				return formattable.ToString(format, formatProvider);
			}
			return _value.ToString();
		}

		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JValue>(parameter, this, new JValueDynamicProxy(), dontFallbackFirst: true);
		}

		int IComparable.CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			object objB = ((obj is JValue) ? ((JValue)obj).Value : obj);
			return Compare(_valueType, _value, objB);
		}

		public int CompareTo(JValue obj)
		{
			if (obj == null)
			{
				return 1;
			}
			return Compare(_valueType, _value, obj._value);
		}

		TypeCode IConvertible.GetTypeCode()
		{
			if (_value == null)
			{
				return TypeCode.Empty;
			}
			if (_value is DateTimeOffset)
			{
				return TypeCode.DateTime;
			}
			if (_value is BigInteger)
			{
				return TypeCode.Object;
			}
			return System.Type.GetTypeCode(_value.GetType());
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return (bool)(JToken)this;
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return (char)(JToken)this;
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return (sbyte)(JToken)this;
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return (byte)(JToken)this;
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return (short)(JToken)this;
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return (ushort)(JToken)this;
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)(JToken)this;
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return (uint)(JToken)this;
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return (long)(JToken)this;
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return (ulong)(JToken)this;
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return (float)(JToken)this;
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return (double)(JToken)this;
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return (decimal)(JToken)this;
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return (DateTime)(JToken)this;
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ToObject(conversionType);
		}
	}
	public class JRaw : JValue
	{
		public JRaw(JRaw other)
			: base(other)
		{
		}

		public JRaw(object rawJson)
			: base(rawJson, JTokenType.Raw)
		{
		}

		public static JRaw Create(JsonReader reader)
		{
			using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			using JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.WriteToken(reader);
			return new JRaw(stringWriter.ToString());
		}

		internal override JToken CloneToken()
		{
			return new JRaw(this);
		}
	}
}
namespace Newtonsoft.Json
{
	public enum Required
	{
		Default,
		AllowNull,
		Always
	}
}
namespace Newtonsoft.Json.Serialization
{
	public class JsonDynamicContract : JsonContainerContract
	{
		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>> _callSiteGetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object>>>(CreateCallSiteGetter);

		private readonly ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>> _callSiteSetters = new ThreadSafeStore<string, CallSite<Func<CallSite, object, object, object>>>(CreateCallSiteSetter);

		public JsonPropertyCollection Properties { get; private set; }

		public Func<string, string> PropertyNameResolver { get; set; }

		private static CallSite<Func<CallSite, object, object>> CreateCallSiteGetter(string name)
		{
			GetMemberBinder innerBinder = (GetMemberBinder)DynamicUtils.BinderWrapper.GetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object>>.Create(new NoThrowGetBinderMember(innerBinder));
		}

		private static CallSite<Func<CallSite, object, object, object>> CreateCallSiteSetter(string name)
		{
			SetMemberBinder innerBinder = (SetMemberBinder)DynamicUtils.BinderWrapper.SetMember(name, typeof(DynamicUtils));
			return CallSite<Func<CallSite, object, object, object>>.Create(new NoThrowSetBinderMember(innerBinder));
		}

		public JsonDynamicContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Dynamic;
			Properties = new JsonPropertyCollection(base.UnderlyingType);
		}

		internal bool TryGetMember(IDynamicMetaObjectProvider dynamicProvider, string name, out object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object>> callSite = _callSiteGetters.Get(name);
			object obj = callSite.Target(callSite, dynamicProvider);
			if (!object.ReferenceEquals(obj, NoThrowExpressionVisitor.ErrorResult))
			{
				value = obj;
				return true;
			}
			value = null;
			return false;
		}

		internal bool TrySetMember(IDynamicMetaObjectProvider dynamicProvider, string name, object value)
		{
			ValidationUtils.ArgumentNotNull(dynamicProvider, "dynamicProvider");
			CallSite<Func<CallSite, object, object, object>> callSite = _callSiteSetters.Get(name);
			object objA = callSite.Target(callSite, dynamicProvider, value);
			return !object.ReferenceEquals(objA, NoThrowExpressionVisitor.ErrorResult);
		}
	}
	internal class JsonFormatterConverter : IFormatterConverter
	{
		private readonly JsonSerializerInternalReader _reader;

		private readonly JsonISerializableContract _contract;

		private readonly JsonProperty _member;

		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "serializer");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			_reader = reader;
			_contract = contract;
			_member = member;
		}

		private T GetTokenValue<T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jValue = (JValue)value;
			return (T)System.Convert.ChangeType(jValue.Value, typeof(T), CultureInfo.InvariantCulture);
		}

		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (!(value is JToken token))
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return _reader.CreateISerializableItem(token, type, _contract, _member);
		}

		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			if (value is JValue)
			{
				value = ((JValue)value).Value;
			}
			return System.Convert.ChangeType(value, typeCode, CultureInfo.InvariantCulture);
		}

		public bool ToBoolean(object value)
		{
			return GetTokenValue<bool>(value);
		}

		public byte ToByte(object value)
		{
			return GetTokenValue<byte>(value);
		}

		public char ToChar(object value)
		{
			return GetTokenValue<char>(value);
		}

		public DateTime ToDateTime(object value)
		{
			return GetTokenValue<DateTime>(value);
		}

		public decimal ToDecimal(object value)
		{
			return GetTokenValue<decimal>(value);
		}

		public double ToDouble(object value)
		{
			return GetTokenValue<double>(value);
		}

		public short ToInt16(object value)
		{
			return GetTokenValue<short>(value);
		}

		public int ToInt32(object value)
		{
			return GetTokenValue<int>(value);
		}

		public long ToInt64(object value)
		{
			return GetTokenValue<long>(value);
		}

		public sbyte ToSByte(object value)
		{
			return GetTokenValue<sbyte>(value);
		}

		public float ToSingle(object value)
		{
			return GetTokenValue<float>(value);
		}

		public string ToString(object value)
		{
			return GetTokenValue<string>(value);
		}

		public ushort ToUInt16(object value)
		{
			return GetTokenValue<ushort>(value);
		}

		public uint ToUInt32(object value)
		{
			return GetTokenValue<uint>(value);
		}

		public ulong ToUInt64(object value)
		{
			return GetTokenValue<ulong>(value);
		}
	}
	public class JsonISerializableContract : JsonContainerContract
	{
		public ObjectConstructor<object> ISerializableCreator { get; set; }

		public JsonISerializableContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Serializable;
		}
	}
	public class JsonLinqContract : JsonContract
	{
		public JsonLinqContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Linq;
		}
	}
	public class JsonPrimitiveContract : JsonContract
	{
		internal PrimitiveTypeCode TypeCode { get; set; }

		public JsonPrimitiveContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Primitive;
			TypeCode = ConvertUtils.GetTypeCode(underlyingType);
			IsReadOnlyOrFixedSize = true;
		}
	}
	public class DynamicValueProvider : IValueProvider
	{
		private readonly MemberInfo _memberInfo;

		private Func<object, object> _getter;

		private Action<object, object> _setter;

		public DynamicValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			_memberInfo = memberInfo;
		}

		public void SetValue(object target, object value)
		{
			try
			{
				if (_setter == null)
				{
					_setter = DynamicReflectionDelegateFactory.Instance.CreateSet<object>(_memberInfo);
				}
				_setter(target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}

		public object GetValue(object target)
		{
			try
			{
				if (_getter == null)
				{
					_getter = DynamicReflectionDelegateFactory.Instance.CreateGet<object>(_memberInfo);
				}
				return _getter(target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}
	}
	public class ErrorEventArgs : EventArgs
	{
		public object CurrentObject { get; private set; }

		public ErrorContext ErrorContext { get; private set; }

		public ErrorEventArgs(object currentObject, ErrorContext errorContext)
		{
			CurrentObject = currentObject;
			ErrorContext = errorContext;
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	public class JPropertyDescriptor : PropertyDescriptor
	{
		public override Type ComponentType => typeof(JObject);

		public override bool IsReadOnly => false;

		public override Type PropertyType => typeof(object);

		protected override int NameHashCode => base.NameHashCode;

		public JPropertyDescriptor(string name)
			: base(name, null)
		{
		}

		private static JObject CastInstance(object instance)
		{
			return (JObject)instance;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}

		public override object GetValue(object component)
		{
			return CastInstance(component)[Name];
		}

		public override void ResetValue(object component)
		{
		}

		public override void SetValue(object component, object value)
		{
			JToken value2 = ((value is JToken) ? ((JToken)value) : new JValue(value));
			CastInstance(component)[Name] = value2;
		}

		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
	}
}
namespace Newtonsoft.Json.Serialization
{
	public interface IReferenceResolver
	{
		object ResolveReference(object context, string reference);

		string GetReference(object context, object value);

		bool IsReferenced(object context, object value);

		void AddReference(object context, string reference, object value);
	}
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		private int _referenceCount;

		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase;
			if (context is JsonSerializerInternalBase)
			{
				jsonSerializerInternalBase = (JsonSerializerInternalBase)context;
			}
			else
			{
				if (!(context is JsonSerializerProxy))
				{
					throw new JsonException("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = ((JsonSerializerProxy)context).GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		public object ResolveReference(object context, string reference)
		{
			GetMappings(context).TryGetByFirst(reference, out var second);
			return second;
		}

		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = GetMappings(context);
			if (!mappings.TryGetBySecond(value, out var first))
			{
				_referenceCount++;
				first = _referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Set(first, value);
			}
			return first;
		}

		public void AddReference(object context, string reference, object value)
		{
			GetMappings(context).Set(reference, value);
		}

		public bool IsReferenced(object context, object value)
		{
			string first;
			return GetMappings(context).TryGetBySecond(value, out first);
		}
	}
}
namespace Newtonsoft.Json
{
	[Flags]
	public enum PreserveReferencesHandling
	{
		None = 0,
		Objects = 1,
		Arrays = 2,
		All = 3
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonArrayAttribute : JsonContainerAttribute
	{
		private bool _allowNullItems;

		public bool AllowNullItems
		{
			get
			{
				return _allowNullItems;
			}
			set
			{
				_allowNullItems = value;
			}
		}

		public JsonArrayAttribute()
		{
		}

		public JsonArrayAttribute(bool allowNullItems)
		{
			_allowNullItems = allowNullItems;
		}

		public JsonArrayAttribute(string id)
			: base(id)
		{
		}
	}
	[Flags]
	public enum DefaultValueHandling
	{
		Include = 0,
		Ignore = 1,
		Populate = 2,
		IgnoreAndPopulate = 3
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		private readonly Type _converterType;

		public Type ConverterType => _converterType;

		public object[] ConverterParameters { get; private set; }

		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			_converterType = converterType;
		}

		public JsonConverterAttribute(Type converterType, params object[] converterParameters)
			: this(converterType)
		{
			ConverterParameters = converterParameters;
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : JsonContainerAttribute
	{
		private MemberSerialization _memberSerialization;

		internal Required? _itemRequired;

		public MemberSerialization MemberSerialization
		{
			get
			{
				return _memberSerialization;
			}
			set
			{
				_memberSerialization = value;
			}
		}

		public Required ItemRequired
		{
			get
			{
				return _itemRequired ?? Required.Default;
			}
			set
			{
				_itemRequired = value;
			}
		}

		public JsonObjectAttribute()
		{
		}

		public JsonObjectAttribute(MemberSerialization memberSerialization)
		{
			MemberSerialization = memberSerialization;
		}

		public JsonObjectAttribute(string id)
			: base(id)
		{
		}
	}
	public class JsonSerializerSettings
	{
		internal const ReferenceLoopHandling DefaultReferenceLoopHandling = ReferenceLoopHandling.Error;

		internal const MissingMemberHandling DefaultMissingMemberHandling = MissingMemberHandling.Ignore;

		internal const NullValueHandling DefaultNullValueHandling = NullValueHandling.Include;

		internal const DefaultValueHandling DefaultDefaultValueHandling = DefaultValueHandling.Include;

		internal const ObjectCreationHandling DefaultObjectCreationHandling = ObjectCreationHandling.Auto;

		internal const PreserveReferencesHandling DefaultPreserveReferencesHandling = PreserveReferencesHandling.None;

		internal const ConstructorHandling DefaultConstructorHandling = ConstructorHandling.Default;

		internal const TypeNameHandling DefaultTypeNameHandling = TypeNameHandling.None;

		internal const MetadataPropertyHandling DefaultMetadataPropertyHandling = MetadataPropertyHandling.Default;

		internal const FormatterAssemblyStyle DefaultTypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;

		internal const Formatting DefaultFormatting = Formatting.None;

		internal const DateFormatHandling DefaultDateFormatHandling = DateFormatHandling.IsoDateFormat;

		internal const DateTimeZoneHandling DefaultDateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;

		internal const DateParseHandling DefaultDateParseHandling = DateParseHandling.DateTime;

		internal const FloatParseHandling DefaultFloatParseHandling = FloatParseHandling.Double;

		internal const FloatFormatHandling DefaultFloatFormatHandling = FloatFormatHandling.String;

		internal const StringEscapeHandling DefaultStringEscapeHandling = StringEscapeHandling.Default;

		internal const FormatterAssemblyStyle DefaultFormatterAssemblyStyle = FormatterAssemblyStyle.Simple;

		internal const bool DefaultCheckAdditionalContent = false;

		internal const string DefaultDateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		internal static readonly StreamingContext DefaultContext;

		internal static readonly CultureInfo DefaultCulture;

		internal Formatting? _formatting;

		internal DateFormatHandling? _dateFormatHandling;

		internal DateTimeZoneHandling? _dateTimeZoneHandling;

		internal DateParseHandling? _dateParseHandling;

		internal FloatFormatHandling? _floatFormatHandling;

		internal FloatParseHandling? _floatParseHandling;

		internal StringEscapeHandling? _stringEscapeHandling;

		internal CultureInfo _culture;

		internal bool? _checkAdditionalContent;

		internal int? _maxDepth;

		internal bool _maxDepthSet;

		internal string _dateFormatString;

		internal bool _dateFormatStringSet;

		internal FormatterAssemblyStyle? _typeNameAssemblyFormat;

		internal DefaultValueHandling? _defaultValueHandling;

		internal PreserveReferencesHandling? _preserveReferencesHandling;

		internal NullValueHandling? _nullValueHandling;

		internal ObjectCreationHandling? _objectCreationHandling;

		internal MissingMemberHandling? _missingMemberHandling;

		internal ReferenceLoopHandling? _referenceLoopHandling;

		internal StreamingContext? _context;

		internal ConstructorHandling? _constructorHandling;

		internal TypeNameHandling? _typeNameHandling;

		internal MetadataPropertyHandling? _metadataPropertyHandling;

		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling ?? ReferenceLoopHandling.Error;
			}
			set
			{
				_referenceLoopHandling = value;
			}
		}

		public MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return _missingMemberHandling ?? MissingMemberHandling.Ignore;
			}
			set
			{
				_missingMemberHandling = value;
			}
		}

		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling ?? ObjectCreationHandling.Auto;
			}
			set
			{
				_objectCreationHandling = value;
			}
		}

		public NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling ?? NullValueHandling.Include;
			}
			set
			{
				_nullValueHandling = value;
			}
		}

		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling ?? DefaultValueHandling.Include;
			}
			set
			{
				_defaultValueHandling = value;
			}
		}

		public IList<JsonConverter> Converters { get; set; }

		public PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return _preserveReferencesHandling ?? PreserveReferencesHandling.None;
			}
			set
			{
				_preserveReferencesHandling = value;
			}
		}

		public TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling ?? TypeNameHandling.None;
			}
			set
			{
				_typeNameHandling = value;
			}
		}

		public MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return _metadataPropertyHandling ?? MetadataPropertyHandling.Default;
			}
			set
			{
				_metadataPropertyHandling = value;
			}
		}

		public FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return _typeNameAssemblyFormat ?? FormatterAssemblyStyle.Simple;
			}
			set
			{
				_typeNameAssemblyFormat = value;
			}
		}

		public ConstructorHandling ConstructorHandling
		{
			get
			{
				return _constructorHandling ?? ConstructorHandling.Default;
			}
			set
			{
				_constructorHandling = value;
			}
		}

		public IContractResolver ContractResolver { get; set; }

		public IReferenceResolver ReferenceResolver { get; set; }

		public ITraceWriter TraceWriter { get; set; }

		public SerializationBinder Binder { get; set; }

		public EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error { get; set; }

		public StreamingContext Context
		{
			get
			{
				return _context ?? DefaultContext;
			}
			set
			{
				_context = value;
			}
		}

		public string DateFormatString
		{
			get
			{
				return _dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				_dateFormatString = value;
				_dateFormatStringSet = true;
			}
		}

		public int? MaxDepth
		{
			get
			{
				return _maxDepth;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				_maxDepth = value;
				_maxDepthSet = true;
			}
		}

		public Formatting Formatting
		{
			get
			{
				return _formatting ?? Formatting.None;
			}
			set
			{
				_formatting = value;
			}
		}

		public DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
			}
			set
			{
				_dateFormatHandling = value;
			}
		}

		public DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
			}
			set
			{
				_dateTimeZoneHandling = value;
			}
		}

		public DateParseHandling DateParseHandling
		{
			get
			{
				return _dateParseHandling ?? DateParseHandling.DateTime;
			}
			set
			{
				_dateParseHandling = value;
			}
		}

		public FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _floatFormatHandling ?? FloatFormatHandling.String;
			}
			set
			{
				_floatFormatHandling = value;
			}
		}

		public FloatParseHandling FloatParseHandling
		{
			get
			{
				return _floatParseHandling ?? FloatParseHandling.Double;
			}
			set
			{
				_floatParseHandling = value;
			}
		}

		public StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _stringEscapeHandling ?? StringEscapeHandling.Default;
			}
			set
			{
				_stringEscapeHandling = value;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? DefaultCulture;
			}
			set
			{
				_culture = value;
			}
		}

		public bool CheckAdditionalContent
		{
			get
			{
				return _checkAdditionalContent ?? false;
			}
			set
			{
				_checkAdditionalContent = value;
			}
		}

		static JsonSerializerSettings()
		{
			DefaultContext = default(StreamingContext);
			DefaultCulture = CultureInfo.InvariantCulture;
		}

		public JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>();
		}
	}
	public class JsonValidatingReader : JsonReader, IJsonLineInfo
	{
		private class SchemaScope
		{
			private readonly JTokenType _tokenType;

			private readonly IList<JsonSchemaModel> _schemas;

			private readonly Dictionary<string, bool> _requiredProperties;

			public string CurrentPropertyName { get; set; }

			public int ArrayItemCount { get; set; }

			public bool IsUniqueArray { get; set; }

			public IList<JToken> UniqueArrayItems { get; set; }

			public JTokenWriter CurrentItemWriter { get; set; }

			public IList<JsonSchemaModel> Schemas => _schemas;

			public Dictionary<string, bool> RequiredProperties => _requiredProperties;

			public JTokenType TokenType => _tokenType;

			public SchemaScope(JTokenType tokenType, IList<JsonSchemaModel> schemas)
			{
				_tokenType = tokenType;
				_schemas = schemas;
				_requiredProperties = schemas.SelectMany(GetRequiredProperties).Distinct().ToDictionary((string p) => p, (string p) => false);
				if (tokenType == JTokenType.Array && schemas.Any((JsonSchemaModel s) => s.UniqueItems))
				{
					IsUniqueArray = true;
					UniqueArrayItems = new List<JToken>();
				}
			}

			private IEnumerable<string> GetRequiredProperties(JsonSchemaModel schema)
			{
				if (schema == null || schema.Properties == null)
				{
					return Enumerable.Empty<string>();
				}
				return from p in schema.Properties
					where p.Value.Required
					select p.Key;
			}
		}

		private readonly JsonReader _reader;

		private readonly Stack<SchemaScope> _stack;

		private JsonSchema _schema;

		private JsonSchemaModel _model;

		private SchemaScope _currentScope;

		private static readonly IList<JsonSchemaModel> EmptySchemaList = new List<JsonSchemaModel>();

		public override object Value => _reader.Value;

		public override int Depth => _reader.Depth;

		public override string Path => _reader.Path;

		public override char QuoteChar
		{
			get
			{
				return _reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		public override JsonToken TokenType => _reader.TokenType;

		public override Type ValueType => _reader.ValueType;

		private IList<JsonSchemaModel> CurrentSchemas => _currentScope.Schemas;

		private IList<JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (_currentScope == null)
				{
					return new List<JsonSchemaModel>(new JsonSchemaModel[1] { _model });
				}
				if (_currentScope.Schemas == null || _currentScope.Schemas.Count == 0)
				{
					return EmptySchemaList;
				}
				switch (_currentScope.TokenType)
				{
				case JTokenType.None:
					return _currentScope.Schemas;
				case JTokenType.Object:
				{
					if (_currentScope.CurrentPropertyName == null)
					{
						throw new JsonReaderException("CurrentPropertyName has not been set on scope.");
					}
					IList<JsonSchemaModel> list2 = new List<JsonSchemaModel>();
					{
						foreach (JsonSchemaModel currentSchema in CurrentSchemas)
						{
							if (currentSchema.Properties != null && currentSchema.Properties.TryGetValue(_currentScope.CurrentPropertyName, out var value))
							{
								list2.Add(value);
							}
							if (currentSchema.PatternProperties != null)
							{
								foreach (KeyValuePair<string, JsonSchemaModel> patternProperty in currentSchema.PatternProperties)
								{
									if (Regex.IsMatch(_currentScope.CurrentPropertyName, patternProperty.Key))
									{
										list2.Add(patternProperty.Value);
									}
								}
							}
							if (list2.Count == 0 && currentSchema.AllowAdditionalProperties && currentSchema.AdditionalProperties != null)
							{
								list2.Add(currentSchema.AdditionalProperties);
							}
						}
						return list2;
					}
				}
				case JTokenType.Array:
				{
					IList<JsonSchemaModel> list = new List<JsonSchemaModel>();
					{
						foreach (JsonSchemaModel currentSchema2 in CurrentSchemas)
						{
							if (!currentSchema2.PositionalItemsValidation)
							{
								if (currentSchema2.Items != null && currentSchema2.Items.Count > 0)
								{
									list.Add(currentSchema2.Items[0]);
								}
								continue;
							}
							if (currentSchema2.Items != null && currentSchema2.Items.Count > 0 && currentSchema2.Items.Count > _currentScope.ArrayItemCount - 1)
							{
								list.Add(currentSchema2.Items[_currentScope.ArrayItemCount - 1]);
							}
							if (currentSchema2.AllowAdditionalItems && currentSchema2.AdditionalItems != null)
							{
								list.Add(currentSchema2.AdditionalItems);
							}
						}
						return list;
					}
				}
				case JTokenType.Constructor:
					return EmptySchemaList;
				default:
					throw new ArgumentOutOfRangeException("TokenType", "Unexpected token type: {0}".FormatWith(CultureInfo.InvariantCulture, _currentScope.TokenType));
				}
			}
		}

		public JsonSchema Schema
		{
			get
			{
				return _schema;
			}
			set
			{
				if (TokenType != 0)
				{
					throw new InvalidOperationException("Cannot change schema while validating JSON.");
				}
				_schema = value;
				_model = null;
			}
		}

		public JsonReader Reader => _reader;

		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (!(_reader is IJsonLineInfo jsonLineInfo))
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (!(_reader is IJsonLineInfo jsonLineInfo))
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		public event ValidationEventHandler ValidationEventHandler;

		private void Push(SchemaScope scope)
		{
			_stack.Push(scope);
			_currentScope = scope;
		}

		private SchemaScope Pop()
		{
			SchemaScope result = _stack.Pop();
			_currentScope = ((_stack.Count != 0) ? _stack.Peek() : null);
			return result;
		}

		private void RaiseError(string message, JsonSchemaModel schema)
		{
			string message2 = (((IJsonLineInfo)this).HasLineInfo() ? (message + " Line {0}, position {1}.".FormatWith(CultureInfo.InvariantCulture, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition)) : message);
			OnValidationEvent(new JsonSchemaException(message2, null, Path, ((IJsonLineInfo)this).LineNumber, ((IJsonLineInfo)this).LinePosition));
		}

		private void OnValidationEvent(JsonSchemaException exception)
		{
			ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		public JsonValidatingReader(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			_reader = reader;
			_stack = new Stack<SchemaScope>();
		}

		private void ValidateNotDisallowed(JsonSchemaModel schema)
		{
			if (schema != null)
			{
				JsonSchemaType? currentNodeSchemaType = GetCurrentNodeSchemaType();
				if (currentNodeSchemaType.HasValue && JsonSchemaGenerator.HasFlag(schema.Disallow, currentNodeSchemaType.Value))
				{
					RaiseError("Type {0} is disallowed.".FormatWith(CultureInfo.InvariantCulture, currentNodeSchemaType), schema);
				}
			}
		}

		private JsonSchemaType? GetCurrentNodeSchemaType()
		{
			return _reader.TokenType switch
			{
				JsonToken.StartObject => JsonSchemaType.Object, 
				JsonToken.StartArray => JsonSchemaType.Array, 
				JsonToken.Integer => JsonSchemaType.Integer, 
				JsonToken.Float => JsonSchemaType.Float, 
				JsonToken.String => JsonSchemaType.String, 
				JsonToken.Boolean => JsonSchemaType.Boolean, 
				JsonToken.Null => JsonSchemaType.Null, 
				_ => null, 
			};
		}

		public override int? ReadAsInt32()
		{
			int? result = _reader.ReadAsInt32();
			ValidateCurrentToken();
			return result;
		}

		public override byte[] ReadAsBytes()
		{
			byte[] result = _reader.ReadAsBytes();
			ValidateCurrentToken();
			return result;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? result = _reader.ReadAsDecimal();
			ValidateCurrentToken();
			return result;
		}

		public override string ReadAsString()
		{
			string result = _reader.ReadAsString();
			ValidateCurrentToken();
			return result;
		}

		public override DateTime? ReadAsDateTime()
		{
			DateTime? result = _reader.ReadAsDateTime();
			ValidateCurrentToken();
			return result;
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			DateTimeOffset? result = _reader.ReadAsDateTimeOffset();
			ValidateCurrentToken();
			return result;
		}

		public override bool Read()
		{
			if (!_reader.Read())
			{
				return false;
			}
			if (_reader.TokenType == JsonToken.Comment)
			{
				return true;
			}
			ValidateCurrentToken();
			return true;
		}

		private void ValidateCurrentToken()
		{
			if (_model == null)
			{
				JsonSchemaModelBuilder jsonSchemaModelBuilder = new JsonSchemaModelBuilder();
				_model = jsonSchemaModelBuilder.Build(_schema);
				if (!JsonTokenUtils.IsStartToken(_reader.TokenType))
				{
					Push(new SchemaScope(JTokenType.None, CurrentMemberSchemas));
				}
			}
			switch (_reader.TokenType)
			{
			case JsonToken.StartObject:
			{
				ProcessValue();
				IList<JsonSchemaModel> schemas2 = CurrentMemberSchemas.Where(ValidateObject).ToList();
				Push(new SchemaScope(JTokenType.Object, schemas2));
				WriteToken(CurrentSchemas);
				break;
			}
			case JsonToken.StartArray:
			{
				ProcessValue();
				IList<JsonSchemaModel> schemas = CurrentMemberSchemas.Where(ValidateArray).ToList();
				Push(new SchemaScope(JTokenType.Array, schemas));
				WriteToken(CurrentSchemas);
				break;
			}
			case JsonToken.StartConstructor:
				ProcessValue();
				Push(new SchemaScope(JTokenType.Constructor, null));
				WriteToken(CurrentSchemas);
				break;
			case JsonToken.PropertyName:
				WriteToken(CurrentSchemas);
				{
					foreach (JsonSchemaModel currentSchema in CurrentSchemas)
					{
						ValidatePropertyName(currentSchema);
					}
					break;
				}
			case JsonToken.Raw:
				ProcessValue();
				break;
			case JsonToken.Integer:
				ProcessValue();
				WriteToken(CurrentMemberSchemas);
				{
					foreach (JsonSchemaModel currentMemberSchema in CurrentMemberSchemas)
					{
						ValidateInteger(currentMemberSchema);
					}
					break;
				}
			case JsonToken.Float:
				ProcessValue();
				WriteToken(CurrentMemberSchemas);
				{
					foreach (JsonSchemaModel currentMemberSchema2 in CurrentMemberSchemas)
					{
						ValidateFloat(currentMemberSchema2);
					}
					break;
				}
			case JsonToken.String:
				ProcessValue();
				WriteToken(CurrentMemberSchemas);
				{
					foreach (JsonSchemaModel currentMemberSchema3 in CurrentMemberSchemas)
					{
						ValidateString(currentMemberSchema3);
					}
					break;
				}
			case JsonToken.Boolean:
				ProcessValue();
				WriteToken(CurrentMemberSchemas);
				{
					foreach (JsonSchemaModel currentMemberSchema4 in CurrentMemberSchemas)
					{
						ValidateBoolean(currentMemberSchema4);
					}
					break;
				}
			case JsonToken.Null:
				ProcessValue();
				WriteToken(CurrentMemberSchemas);
				{
					foreach (JsonSchemaModel currentMemberSchema5 in CurrentMemberSchemas)
					{
						ValidateNull(currentMemberSchema5);
					}
					break;
				}
			case JsonToken.EndObject:
				WriteToken(CurrentSchemas);
				foreach (JsonSchemaModel currentSchema2 in CurrentSchemas)
				{
					ValidateEndObject(currentSchema2);
				}
				Pop();
				break;
			case JsonToken.EndArray:
				WriteToken(CurrentSchemas);
				foreach (JsonSchemaModel currentSchema3 in CurrentSchemas)
				{
					ValidateEndArray(currentSchema3);
				}
				Pop();
				break;
			case JsonToken.EndConstructor:
				WriteToken(CurrentSchemas);
				Pop();
				break;
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				WriteToken(CurrentMemberSchemas);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			case JsonToken.None:
				break;
			}
		}

		private void WriteToken(IList<JsonSchemaModel> schemas)
		{
			foreach (SchemaScope item in _stack)
			{
				bool flag = item.TokenType == JTokenType.Array && item.IsUniqueArray && item.ArrayItemCount > 0;
				if (!flag && !schemas.Any((JsonSchemaModel s) => s.Enum != null))
				{
					continue;
				}
				if (item.CurrentItemWriter == null)
				{
					if (JsonTokenUtils.IsEndToken(_reader.TokenType))
					{
						continue;
					}
					item.CurrentItemWriter = new JTokenWriter();
				}
				item.CurrentItemWriter.WriteToken(_reader, writeChildren: false);
				if (item.CurrentItemWriter.Top != 0 || _reader.TokenType == JsonToken.PropertyName)
				{
					continue;
				}
				JToken token = item.CurrentItemWriter.Token;
				item.CurrentItemWriter = null;
				if (flag)
				{
					if (item.UniqueArrayItems.Contains(token, JToken.EqualityComparer))
					{
						RaiseError("Non-unique array item at index {0}.".FormatWith(CultureInfo.InvariantCulture, item.ArrayItemCount - 1), item.Schemas.First((JsonSchemaModel s) => s.UniqueItems));
					}
					item.UniqueArrayItems.Add(token);
				}
				else
				{
					if (!schemas.Any((JsonSchemaModel s) => s.Enum != null))
					{
						continue;
					}
					foreach (JsonSchemaModel schema in schemas)
					{
						if (schema.Enum != null && !schema.Enum.ContainsValue(token, JToken.EqualityComparer))
						{
							StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
							token.WriteTo(new JsonTextWriter(stringWriter));
							RaiseError("Value {0} is not defined in enum.".FormatWith(CultureInfo.InvariantCulture, stringWriter.ToString()), schema);
						}
					}
				}
			}
		}

		private void ValidateEndObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			Dictionary<string, bool> requiredProperties = _currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				List<string> list = (from kv in requiredProperties
					where !kv.Value
					select kv.Key).ToList();
				if (list.Count > 0)
				{
					RaiseError("Required properties are missing from object: {0}.".FormatWith(CultureInfo.InvariantCulture, string.Join(", ", list.ToArray())), schema);
				}
			}
		}

		private void ValidateEndArray(JsonSchemaModel schema)
		{
			if (schema != null)
			{
				int arrayItemCount = _currentScope.ArrayItemCount;
				if (schema.MaximumItems.HasValue && arrayItemCount > schema.MaximumItems)
				{
					RaiseError("Array item count {0} exceeds maximum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
				}
				if (schema.MinimumItems.HasValue && arrayItemCount < schema.MinimumItems)
				{
					RaiseError("Array item count {0} is less than minimum count of {1}.".FormatWith(CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
				}
			}
		}

		private void ValidateNull(JsonSchemaModel schema)
		{
			if (schema != null && TestType(schema, JsonSchemaType.Null))
			{
				ValidateNotDisallowed(schema);
			}
		}

		private void ValidateBoolean(JsonSchemaModel schema)
		{
			if (schema != null && TestType(schema, JsonSchemaType.Boolean))
			{
				ValidateNotDisallowed(schema);
			}
		}

		private void ValidateString(JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, JsonSchemaType.String))
			{
				return;
			}
			ValidateNotDisallowed(schema);
			string text = _reader.Value.ToString();
			if (schema.MaximumLength.HasValue && text.Length > schema.MaximumLength)
			{
				RaiseError("String '{0}' exceeds maximum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MaximumLength), schema);
			}
			if (schema.MinimumLength.HasValue && text.Length < schema.MinimumLength)
			{
				RaiseError("String '{0}' is less than minimum length of {1}.".FormatWith(CultureInfo.InvariantCulture, text, schema.MinimumLength), schema);
			}
			if (schema.Patterns == null)
			{
				return;
			}
			foreach (string pattern in schema.Patterns)
			{
				if (!Regex.IsMatch(text, pattern))
				{
					RaiseError("String '{0}' does not match regex pattern '{1}'.".FormatWith(CultureInfo.InvariantCulture, text, pattern), schema);
				}
			}
		}

		private void ValidateInteger(JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, JsonSchemaType.Integer))
			{
				return;
			}
			ValidateNotDisallowed(schema);
			object value = _reader.Value;
			if (schema.Maximum.HasValue)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Maximum) > 0)
				{
					RaiseError("Integer {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && JValue.Compare(JTokenType.Integer, value, schema.Maximum) == 0)
				{
					RaiseError("Integer {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Maximum), schema);
				}
			}
			if (schema.Minimum.HasValue)
			{
				if (JValue.Compare(JTokenType.Integer, value, schema.Minimum) < 0)
				{
					RaiseError("Integer {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && JValue.Compare(JTokenType.Integer, value, schema.Minimum) == 0)
				{
					RaiseError("Integer {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, value, schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy.HasValue && ((!(value is BigInteger bigInteger)) ? (!IsZero((double)Convert.ToInt64(value, CultureInfo.InvariantCulture) % schema.DivisibleBy.Value)) : (Math.Abs(schema.DivisibleBy.Value - Math.Truncate(schema.DivisibleBy.Value)).Equals(0.0) ? (bigInteger % new BigInteger(schema.DivisibleBy.Value) != 0L) : (bigInteger != 0L))))
			{
				RaiseError("Integer {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(value), schema.DivisibleBy), schema);
			}
		}

		private void ProcessValue()
		{
			if (_currentScope == null || _currentScope.TokenType != JTokenType.Array)
			{
				return;
			}
			_currentScope.ArrayItemCount++;
			foreach (JsonSchemaModel currentSchema in CurrentSchemas)
			{
				if (currentSchema != null && currentSchema.PositionalItemsValidation && !currentSchema.AllowAdditionalItems && (currentSchema.Items == null || _currentScope.ArrayItemCount - 1 >= currentSchema.Items.Count))
				{
					RaiseError("Index {0} has not been defined and the schema does not allow additional items.".FormatWith(CultureInfo.InvariantCulture, _currentScope.ArrayItemCount), currentSchema);
				}
			}
		}

		private void ValidateFloat(JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, JsonSchemaType.Float))
			{
				return;
			}
			ValidateNotDisallowed(schema);
			double num = Convert.ToDouble(_reader.Value, CultureInfo.InvariantCulture);
			if (schema.Maximum.HasValue)
			{
				if (num > schema.Maximum)
				{
					RaiseError("Float {0} exceeds maximum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && num == schema.Maximum)
				{
					RaiseError("Float {0} equals maximum value of {1} and exclusive maximum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Maximum), schema);
				}
			}
			if (schema.Minimum.HasValue)
			{
				if (num < schema.Minimum)
				{
					RaiseError("Float {0} is less than minimum value of {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && num == schema.Minimum)
				{
					RaiseError("Float {0} equals minimum value of {1} and exclusive minimum is true.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy.HasValue)
			{
				double value = FloatingPointRemainder(num, schema.DivisibleBy.Value);
				if (!IsZero(value))
				{
					RaiseError("Float {0} is not evenly divisible by {1}.".FormatWith(CultureInfo.InvariantCulture, JsonConvert.ToString(num), schema.DivisibleBy), schema);
				}
			}
		}

		private static double FloatingPointRemainder(double dividend, double divisor)
		{
			return dividend - Math.Floor(dividend / divisor) * divisor;
		}

		private static bool IsZero(double value)
		{
			return Math.Abs(value) < 4.440892098500626E-15;
		}

		private void ValidatePropertyName(JsonSchemaModel schema)
		{
			if (schema != null)
			{
				string text = Convert.ToString(_reader.Value, CultureInfo.InvariantCulture);
				if (_currentScope.RequiredProperties.ContainsKey(text))
				{
					_currentScope.RequiredProperties[text] = true;
				}
				if (!schema.AllowAdditionalProperties && !IsPropertyDefinied(schema, text))
				{
					RaiseError("Property '{0}' has not been defined and the schema does not allow additional properties.".FormatWith(CultureInfo.InvariantCulture, text), schema);
				}
				_currentScope.CurrentPropertyName = text;
			}
		}

		private bool IsPropertyDefinied(JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				foreach (string key in schema.PatternProperties.Keys)
				{
					if (Regex.IsMatch(propertyName, key))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool ValidateArray(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return TestType(schema, JsonSchemaType.Array);
		}

		private bool ValidateObject(JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return TestType(schema, JsonSchemaType.Object);
		}

		private bool TestType(JsonSchemaModel currentSchema, JsonSchemaType currentType)
		{
			if (!JsonSchemaGenerator.HasFlag(currentSchema.Type, currentType))
			{
				RaiseError("Invalid type. Expected {0} but got {1}.".FormatWith(CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
				return false;
			}
			return true;
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			if (_reader is IJsonLineInfo jsonLineInfo)
			{
				return jsonLineInfo.HasLineInfo();
			}
			return false;
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		public int GetHashCode(JToken obj)
		{
			return obj?.GetDeepHashCode() ?? 0;
		}
	}
}
namespace Newtonsoft.Json
{
	public enum MemberSerialization
	{
		OptOut,
		OptIn,
		Fields
	}
	public enum ObjectCreationHandling
	{
		Auto,
		Reuse,
		Replace
	}
}
namespace Newtonsoft.Json.Converters
{
	public class IsoDateTimeConverter : DateTimeConverterBase
	{
		private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

		private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;

		private string _dateTimeFormat;

		private CultureInfo _culture;

		public DateTimeStyles DateTimeStyles
		{
			get
			{
				return _dateTimeStyles;
			}
			set
			{
				_dateTimeStyles = value;
			}
		}

		public string DateTimeFormat
		{
			get
			{
				return _dateTimeFormat ?? string.Empty;
			}
			set
			{
				_dateTimeFormat = StringUtils.NullEmptyString(value);
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return _culture ?? CultureInfo.CurrentCulture;
			}
			set
			{
				_culture = value;
			}
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			string value2;
			if (value is DateTime dateTime)
			{
				if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
				{
					dateTime = dateTime.ToUniversalTime();
				}
				value2 = dateTime.ToString(_dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", Culture);
			}
			else
			{
				if (!(value is DateTimeOffset dateTimeOffset))
				{
					throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)));
				}
				if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
				{
					dateTimeOffset = dateTimeOffset.ToUniversalTime();
				}
				value2 = dateTimeOffset.ToString(_dateTimeFormat ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK", Culture);
			}
			writer.WriteValue(value2);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullableType(objectType);
			Type type = (flag ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullableType(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType == JsonToken.Date)
			{
				if (type == typeof(DateTimeOffset))
				{
					if (!(reader.Value is DateTimeOffset))
					{
						return new DateTimeOffset((DateTime)reader.Value);
					}
					return reader.Value;
				}
				return reader.Value;
			}
			if (reader.TokenType != JsonToken.String)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			string text = reader.Value.ToString();
			if (string.IsNullOrEmpty(text) && flag)
			{
				return null;
			}
			if (type == typeof(DateTimeOffset))
			{
				if (!string.IsNullOrEmpty(_dateTimeFormat))
				{
					return DateTimeOffset.ParseExact(text, _dateTimeFormat, Culture, _dateTimeStyles);
				}
				return DateTimeOffset.Parse(text, Culture, _dateTimeStyles);
			}
			if (!string.IsNullOrEmpty(_dateTimeFormat))
			{
				return DateTime.ParseExact(text, _dateTimeFormat, Culture, _dateTimeStyles);
			}
			return DateTime.Parse(text, Culture, _dateTimeStyles);
		}
	}
	public class JavaScriptDateTimeConverter : DateTimeConverterBase
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			long value2;
			if (value is DateTime dateTime)
			{
				DateTime dateTime2 = dateTime.ToUniversalTime();
				value2 = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime2);
			}
			else
			{
				if (!(value is DateTimeOffset dateTimeOffset))
				{
					throw new JsonSerializationException("Expected date object value.");
				}
				value2 = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.ToUniversalTime().UtcDateTime);
			}
			writer.WriteStartConstructor("Date");
			writer.WriteValue(value2);
			writer.WriteEndConstructor();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Type type = (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!ReflectionUtils.IsNullable(objectType))
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
			{
				throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType, reader.Value));
			}
			reader.Read();
			if (reader.TokenType != JsonToken.Integer)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			long javaScriptTicks = (long)reader.Value;
			DateTime dateTime = DateTimeUtils.ConvertJavaScriptTicksToDateTime(javaScriptTicks);
			reader.Read();
			if (reader.TokenType != JsonToken.EndConstructor)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected EndConstructor, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			if (type == typeof(DateTimeOffset))
			{
				return new DateTimeOffset(dateTime);
			}
			return dateTime;
		}
	}
	internal interface IXmlNode
	{
		XmlNodeType NodeType { get; }

		string LocalName { get; }

		IList<IXmlNode> ChildNodes { get; }

		IList<IXmlNode> Attributes { get; }

		IXmlNode ParentNode { get; }

		string Value { get; set; }

		string NamespaceUri { get; }

		object WrappedNode { get; }

		IXmlNode AppendChild(IXmlNode newChild);
	}
	internal class XmlNodeWrapper : IXmlNode
	{
		private readonly XmlNode _node;

		private IList<IXmlNode> _childNodes;

		public object WrappedNode => _node;

		public XmlNodeType NodeType => _node.NodeType;

		public virtual string LocalName => _node.LocalName;

		public IList<IXmlNode> ChildNodes
		{
			get
			{
				if (_childNodes == null)
				{
					_childNodes = _node.ChildNodes.Cast<XmlNode>().Select(WrapNode).ToList();
				}
				return _childNodes;
			}
		}

		public IList<IXmlNode> Attributes
		{
			get
			{
				if (_node.Attributes == null)
				{
					return null;
				}
				return _node.Attributes.Cast<XmlAttribute>().Select(WrapNode).ToList();
			}
		}

		public IXmlNode ParentNode
		{
			get
			{
				XmlNode xmlNode = ((_node is XmlAttribute) ? ((XmlAttribute)_node).OwnerElement : _node.ParentNode);
				if (xmlNode == null)
				{
					return null;
				}
				return WrapNode(xmlNode);
			}
		}

		public string Value
		{
			get
			{
				return _node.Value;
			}
			set
			{
				_node.Value = value;
			}
		}

		public string NamespaceUri => _node.NamespaceURI;

		public XmlNodeWrapper(XmlNode node)
		{
			_node = node;
		}

		internal static IXmlNode WrapNode(XmlNode node)
		{
			return node.NodeType switch
			{
				XmlNodeType.Element => new XmlElementWrapper((XmlElement)node), 
				XmlNodeType.XmlDeclaration => new XmlDeclarationWrapper((XmlDeclaration)node), 
				XmlNodeType.DocumentType => new XmlDocumentTypeWrapper((XmlDocumentType)node), 
				_ => new XmlNodeWrapper(node), 
			};
		}

		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			_node.AppendChild(xmlNodeWrapper._node);
			_childNodes = null;
			return newChild;
		}
	}
	internal interface IXmlDocument : IXmlNode
	{
		IXmlElement DocumentElement { get; }

		IXmlNode CreateComment(string text);

		IXmlNode CreateTextNode(string text);

		IXmlNode CreateCDataSection(string data);

		IXmlNode CreateWhitespace(string text);

		IXmlNode CreateSignificantWhitespace(string text);

		IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

		IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset);

		IXmlNode CreateProcessingInstruction(string target, string data);

		IXmlElement CreateElement(string elementName);

		IXmlElement CreateElement(string qualifiedName, string namespaceUri);

		IXmlNode CreateAttribute(string name, string value);

		IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);
	}
	internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
	{
		private readonly XmlDocument _document;

		public IXmlElement DocumentElement
		{
			get
			{
				if (_document.DocumentElement == null)
				{
					return null;
				}
				return new XmlElementWrapper(_document.DocumentElement);
			}
		}

		public XmlDocumentWrapper(XmlDocument document)
			: base(document)
		{
			_document = document;
		}

		public IXmlNode CreateComment(string data)
		{
			return new XmlNodeWrapper(_document.CreateComment(data));
		}

		public IXmlNode CreateTextNode(string text)
		{
			return new XmlNodeWrapper(_document.CreateTextNode(text));
		}

		public IXmlNode CreateCDataSection(string data)
		{
			return new XmlNodeWrapper(_document.CreateCDataSection(data));
		}

		public IXmlNode CreateWhitespace(string text)
		{
			return new XmlNodeWrapper(_document.CreateWhitespace(text));
		}

		public IXmlNode CreateSignificantWhitespace(string text)
		{
			return new XmlNodeWrapper(_document.CreateSignificantWhitespace(text));
		}

		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			return new XmlDeclarationWrapper(_document.CreateXmlDeclaration(version, encoding, standalone));
		}

		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			return new XmlDocumentTypeWrapper(_document.CreateDocumentType(name, publicId, systemId, null));
		}

		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			return new XmlNodeWrapper(_document.CreateProcessingInstruction(target, data));
		}

		public IXmlElement CreateElement(string elementName)
		{
			return new XmlElementWrapper(_document.CreateElement(elementName));
		}

		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			return new XmlElementWrapper(_document.CreateElement(qualifiedName, namespaceUri));
		}

		public IXmlNode CreateAttribute(string name, string value)
		{
			XmlNodeWrapper xmlNodeWrapper = new XmlNodeWrapper(_document.CreateAttribute(name));
			xmlNodeWrapper.Value = value;
			return xmlNodeWrapper;
		}

		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			XmlNodeWrapper xmlNodeWrapper = new XmlNodeWrapper(_document.CreateAttribute(qualifiedName, namespaceUri));
			xmlNodeWrapper.Value = value;
			return xmlNodeWrapper;
		}
	}
	internal interface IXmlElement : IXmlNode
	{
		bool IsEmpty { get; }

		void SetAttributeNode(IXmlNode attribute);

		string GetPrefixOfNamespace(string namespaceUri);
	}
	internal class XmlElementWrapper : XmlNodeWrapper, IXmlElement, IXmlNode
	{
		private readonly XmlElement _element;

		public bool IsEmpty => _element.IsEmpty;

		public XmlElementWrapper(XmlElement element)
			: base(element)
		{
			_element = element;
		}

		public void SetAttributeNode(IXmlNode attribute)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)attribute;
			_element.SetAttributeNode((XmlAttribute)xmlNodeWrapper.WrappedNode);
		}

		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return _element.GetPrefixOfNamespace(namespaceUri);
		}
	}
	internal interface IXmlDeclaration : IXmlNode
	{
		string Version { get; }

		string Encoding { get; set; }

		string Standalone { get; set; }
	}
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		private readonly XmlDeclaration _declaration;

		public string Version => _declaration.Version;

		public string Encoding
		{
			get
			{
				return _declaration.Encoding;
			}
			set
			{
				_declaration.Encoding = value;
			}
		}

		public string Standalone
		{
			get
			{
				return _declaration.Standalone;
			}
			set
			{
				_declaration.Standalone = value;
			}
		}

		public XmlDeclarationWrapper(XmlDeclaration declaration)
			: base(declaration)
		{
			_declaration = declaration;
		}
	}
	internal interface IXmlDocumentType : IXmlNode
	{
		string Name { get; }

		string System { get; }

		string Public { get; }

		string InternalSubset { get; }
	}
	internal class XmlDocumentTypeWrapper : XmlNodeWrapper, IXmlDocumentType, IXmlNode
	{
		private readonly XmlDocumentType _documentType;

		public string Name => _documentType.Name;

		public string System => _documentType.SystemId;

		public string Public => _documentType.PublicId;

		public string InternalSubset => _documentType.InternalSubset;

		public override string LocalName => "DOCTYPE";

		public XmlDocumentTypeWrapper(XmlDocumentType documentType)
			: base(documentType)
		{
			_documentType = documentType;
		}
	}
	internal class XObjectWrapper : IXmlNode
	{
		private readonly XObject _xmlObject;

		public object WrappedNode => _xmlObject;

		public virtual XmlNodeType NodeType => _xmlObject.NodeType;

		public virtual string LocalName => null;

		public virtual IList<IXmlNode> ChildNodes => new List<IXmlNode>();

		public virtual IList<IXmlNode> Attributes => null;

		public virtual IXmlNode ParentNode => null;

		public virtual string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public virtual string NamespaceUri => null;

		public XObjectWrapper(XObject xmlObject)
		{
			_xmlObject = xmlObject;
		}

		public virtual IXmlNode AppendChild(IXmlNode newChild)
		{
			throw new InvalidOperationException();
		}
	}
	internal class XDeclarationWrapper : XObjectWrapper, IXmlDeclaration, IXmlNode
	{
		internal XDeclaration Declaration { get; private set; }

		public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

		public string Version => Declaration.Version;

		public string Encoding
		{
			get
			{
				return Declaration.Encoding;
			}
			set
			{
				Declaration.Encoding = value;
			}
		}

		public string Standalone
		{
			get
			{
				return Declaration.Standalone;
			}
			set
			{
				Declaration.Standalone = value;
			}
		}

		public XDeclarationWrapper(XDeclaration declaration)
			: base(null)
		{
			Declaration = declaration;
		}
	}
	internal class XDocumentTypeWrapper : XObjectWrapper, IXmlDocumentType, IXmlNode
	{
		private readonly XDocumentType _documentType;

		public string Name => _documentType.Name;

		public string System => _documentType.SystemId;

		public string Public => _documentType.PublicId;

		public string InternalSubset => _documentType.InternalSubset;

		public override string LocalName => "DOCTYPE";

		public XDocumentTypeWrapper(XDocumentType documentType)
			: base((XObject)(object)documentType)
		{
			_documentType = documentType;
		}
	}
	internal class XContainerWrapper : XObjectWrapper
	{
		private IList<IXmlNode> _childNodes;

		private XContainer Container => (XContainer)base.WrappedNode;

		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				if (_childNodes == null)
				{
					_childNodes = Container.Nodes().Select(WrapNode).ToList();
				}
				return _childNodes;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (((XObject)Container).Parent == null)
				{
					return null;
				}
				return WrapNode((XObject)(object)((XObject)Container).Parent);
			}
		}

		public XContainerWrapper(XContainer container)
			: base((XObject)(object)container)
		{
		}

		internal static IXmlNode WrapNode(XObject node)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			if (node is XDocument)
			{
				return new XDocumentWrapper((XDocument)node);
			}
			if (node is XElement)
			{
				return new XElementWrapper((XElement)node);
			}
			if (node is XContainer)
			{
				return new XContainerWrapper((XContainer)node);
			}
			if (node is XProcessingInstruction)
			{
				return new XProcessingInstructionWrapper((XProcessingInstruction)node);
			}
			if (node is XText)
			{
				return new XTextWrapper((XText)node);
			}
			if (node is XComment)
			{
				return new XCommentWrapper((XComment)node);
			}
			if (node is XAttribute)
			{
				return new XAttributeWrapper((XAttribute)node);
			}
			if (node is XDocumentType)
			{
				return new XDocumentTypeWrapper((XDocumentType)node);
			}
			return new XObjectWrapper(node);
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			Container.Add(newChild.WrappedNode);
			_childNodes = null;
			return newChild;
		}
	}
	internal class XDocumentWrapper : XContainerWrapper, IXmlDocument, IXmlNode
	{
		private XDocument Document => (XDocument)base.WrappedNode;

		public override IList<IXmlNode> ChildNodes
		{
			get
			{
				IList<IXmlNode> childNodes = base.ChildNodes;
				if (Document.Declaration != null && childNodes[0].NodeType != XmlNodeType.XmlDeclaration)
				{
					childNodes.Insert(0, new XDeclarationWrapper(Document.Declaration));
				}
				return childNodes;
			}
		}

		public IXmlElement DocumentElement
		{
			get
			{
				if (Document.Root == null)
				{
					return null;
				}
				return new XElementWrapper(Document.Root);
			}
		}

		public XDocumentWrapper(XDocument document)
			: base((XContainer)(object)document)
		{
		}

		public IXmlNode CreateComment(string text)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new XObjectWrapper((XObject)new XComment(text));
		}

		public IXmlNode CreateTextNode(string text)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new XObjectWrapper((XObject)new XText(text));
		}

		public IXmlNode CreateCDataSection(string data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new XObjectWrapper((XObject)new XCData(data));
		}

		public IXmlNode CreateWhitespace(string text)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new XObjectWrapper((XObject)new XText(text));
		}

		public IXmlNode CreateSignificantWhitespace(string text)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new XObjectWrapper((XObject)new XText(text));
		}

		public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
		}

		public IXmlNode CreateXmlDocumentType(string name, string publicId, string systemId, string internalSubset)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			return new XDocumentTypeWrapper(new XDocumentType(name, publicId, systemId, internalSubset));
		}

		public IXmlNode CreateProcessingInstruction(string target, string data)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
		}

		public IXmlElement CreateElement(string elementName)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			return new XElementWrapper(new XElement(XName.op_Implicit(elementName)));
		}

		public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XElementWrapper(new XElement(XName.Get(localName, namespaceUri)));
		}

		public IXmlNode CreateAttribute(string name, string value)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return new XAttributeWrapper(new XAttribute(XName.op_Implicit(name), (object)value));
		}

		public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
			return new XAttributeWrapper(new XAttribute(XName.Get(localName, namespaceUri), (object)value));
		}

		public override IXmlNode AppendChild(IXmlNode newChild)
		{
			if (newChild is XDeclarationWrapper xDeclarationWrapper)
			{
				Document.Declaration = xDeclarationWrapper.Declaration;
				return xDeclarationWrapper;
			}
			return base.AppendChild(newChild);
		}
	}
	internal class XTextWrapper : XObjectWrapper
	{
		private XText Text => (XText)base.WrappedNode;

		public override string Value
		{
			get
			{
				return Text.Value;
			}
			set
			{
				Text.Value = value;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (((XObject)Text).Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode((XObject)(object)((XObject)Text).Parent);
			}
		}

		public XTextWrapper(XText text)
			: base((XObject)(object)text)
		{
		}
	}
	internal class XCommentWrapper : XObjectWrapper
	{
		private XComment Text => (XComment)base.WrappedNode;

		public override string Value
		{
			get
			{
				return Text.Value;
			}
			set
			{
				Text.Value = value;
			}
		}

		public override IXmlNode ParentNode
		{
			get
			{
				if (((XObject)Text).Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode((XObject)(object)((XObject)Text).Parent);
			}
		}

		public XCommentWrapper(XComment text)
			: base((XObject)(object)text)
		{
		}
	}
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		private XProcessingInstruction ProcessingInstruction => (XProcessingInstruction)base.WrappedNode;

		public override string LocalName => ProcessingInstruction.Target;

		public override string Value
		{
			get
			{
				return ProcessingInstruction.Data;
			}
			set
			{
				ProcessingInstruction.Data = value;
			}
		}

		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction)
			: base((XObject)(object)processingInstruction)
		{
		}
	}
	internal class XAttributeWrapper : XObjectWrapper
	{
		private XAttribute Attribute => (XAttribute)base.WrappedNode;

		public override string Value
		{
			get
			{
				return Attribute.Value;
			}
			set
			{
				Attribute.Value = value;
			}
		}

		public override string LocalName => Attribute.Name.LocalName;

		public override string NamespaceUri => Attribute.Name.NamespaceName;

		public override IXmlNode ParentNode
		{
			get
			{
				if (((XObject)Attribute).Parent == null)
				{
					return null;
				}
				return XContainerWrapper.WrapNode((XObject)(object)((XObject)Attribute).Parent);
			}
		}

		public XAttributeWrapper(XAttribute attribute)
			: base((XObject)(object)attribute)
		{
		}
	}
	internal class XElementWrapper : XContainerWrapper, IXmlElement, IXmlNode
	{
		private XElement Element => (XElement)base.WrappedNode;

		public override IList<IXmlNode> Attributes => (from a in Element.Attributes()
			select new XAttributeWrapper(a)).Cast<IXmlNode>().ToList();

		public override string Value
		{
			get
			{
				return Element.Value;
			}
			set
			{
				Element.Value = value;
			}
		}

		public override string LocalName => Element.Name.LocalName;

		public override string NamespaceUri => Element.Name.NamespaceName;

		public bool IsEmpty => Element.IsEmpty;

		public XElementWrapper(XElement element)
			: base((XContainer)(object)element)
		{
		}

		public void SetAttributeNode(IXmlNode attribute)
		{
			XObjectWrapper xObjectWrapper = (XObjectWrapper)attribute;
			((XContainer)Element).Add(xObjectWrapper.WrappedNode);
		}

		public string GetPrefixOfNamespace(string namespaceUri)
		{
			return Element.GetPrefixOfNamespace(XNamespace.op_Implicit(namespaceUri));
		}
	}
	public class XmlNodeConverter : JsonConverter
	{
		private const string TextName = "#text";

		private const string CommentName = "#comment";

		private const string CDataName = "#cdata-section";

		private const string WhitespaceName = "#whitespace";

		private const string SignificantWhitespaceName = "#significant-whitespace";

		private const string DeclarationName = "?xml";

		private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

		public string DeserializeRootElementName { get; set; }

		public bool WriteArrayAttribute { get; set; }

		public bool OmitRootObject { get; set; }

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			IXmlNode node = WrapXml(value);
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			PushParentNamespaces(node, manager);
			if (!OmitRootObject)
			{
				writer.WriteStartObject();
			}
			SerializeNode(writer, node, manager, !OmitRootObject);
			if (!OmitRootObject)
			{
				writer.WriteEndObject();
			}
		}

		private IXmlNode WrapXml(object value)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			if (value is XObject)
			{
				return XContainerWrapper.WrapNode((XObject)value);
			}
			if (value is XmlNode)
			{
				return XmlNodeWrapper.WrapNode((XmlNode)value);
			}
			throw new ArgumentException("Value must be an XML object.", "value");
		}

		private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
		{
			List<IXmlNode> list = null;
			IXmlNode xmlNode = node;
			while ((xmlNode = xmlNode.ParentNode) != null)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (list == null)
					{
						list = new List<IXmlNode>();
					}
					list.Add(xmlNode);
				}
			}
			if (list == null)
			{
				return;
			}
			list.Reverse();
			foreach (IXmlNode item in list)
			{
				manager.PushScope();
				foreach (IXmlNode attribute in item.Attributes)
				{
					if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/" && attribute.LocalName != "xmlns")
					{
						manager.AddNamespace(attribute.LocalName, attribute.Value);
					}
				}
			}
		}

		private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
		{
			string text = ((node.NamespaceUri == null || (node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/")) ? null : manager.LookupPrefix(node.NamespaceUri));
			if (!string.IsNullOrEmpty(text))
			{
				return text + ":" + node.LocalName;
			}
			return node.LocalName;
		}

		private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Attribute:
				if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
				{
					return "$" + node.LocalName;
				}
				return "@" + ResolveFullName(node, manager);
			case XmlNodeType.CDATA:
				return "#cdata-section";
			case XmlNodeType.Comment:
				return "#comment";
			case XmlNodeType.Element:
				return ResolveFullName(node, manager);
			case XmlNodeType.ProcessingInstruction:
				return "?" + ResolveFullName(node, manager);
			case XmlNodeType.DocumentType:
				return "!" + ResolveFullName(node, manager);
			case XmlNodeType.XmlDeclaration:
				return "?xml";
			case XmlNodeType.SignificantWhitespace:
				return "#significant-whitespace";
			case XmlNodeType.Text:
				return "#text";
			case XmlNodeType.Whitespace:
				return "#whitespace";
			default:
				throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
			}
		}

		private bool IsArray(IXmlNode node)
		{
			IXmlNode xmlNode = ((node.Attributes != null) ? node.Attributes.SingleOrDefault((IXmlNode a) => a.LocalName == "Array" && a.NamespaceUri == "http://james.newtonking.com/projects/json") : null);
			if (xmlNode != null)
			{
				return XmlConvert.ToBoolean(xmlNode.Value);
			}
			return false;
		}

		private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			Dictionary<string, List<IXmlNode>> dictionary = new Dictionary<string, List<IXmlNode>>();
			for (int i = 0; i < node.ChildNodes.Count; i++)
			{
				IXmlNode xmlNode = node.ChildNodes[i];
				string propertyName = GetPropertyName(xmlNode, manager);
				if (!dictionary.TryGetValue(propertyName, out var value))
				{
					value = new List<IXmlNode>();
					dictionary.Add(propertyName, value);
				}
				value.Add(xmlNode);
			}
			foreach (KeyValuePair<string, List<IXmlNode>> item in dictionary)
			{
				List<IXmlNode> value2 = item.Value;
				if (value2.Count == 1 && !IsArray(value2[0]))
				{
					SerializeNode(writer, value2[0], manager, writePropertyName);
					continue;
				}
				string key = item.Key;
				if (writePropertyName)
				{
					writer.WritePropertyName(key);
				}
				writer.WriteStartArray();
				for (int j = 0; j < value2.Count; j++)
				{
					SerializeNode(writer, value2[j], manager, writePropertyName: false);
				}
				writer.WriteEndArray();
			}
		}

		private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.NodeType)
			{
			case XmlNodeType.Document:
			case XmlNodeType.DocumentFragment:
				SerializeGroupedNodes(writer, node, manager, writePropertyName);
				break;
			case XmlNodeType.Element:
				if (IsArray(node) && node.ChildNodes.All((IXmlNode n) => n.LocalName == node.LocalName) && node.ChildNodes.Count > 0)
				{
					SerializeGroupedNodes(writer, node, manager, writePropertyName: false);
					break;
				}
				manager.PushScope();
				foreach (IXmlNode attribute in node.Attributes)
				{
					if (attribute.NamespaceUri == "http://www.w3.org/2000/xmlns/")
					{
						string prefix = ((attribute.LocalName != "xmlns") ? attribute.LocalName : string.Empty);
						string value = attribute.Value;
						manager.AddNamespace(prefix, value);
					}
				}
				if (writePropertyName)
				{
					writer.WritePropertyName(GetPropertyName(node, manager));
				}
				if (!ValueAttributes(node.Attributes).Any() && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
				{
					writer.WriteValue(node.ChildNodes[0].Value);
				}
				else if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty(node.Attributes))
				{
					IXmlElement xmlElement = (IXmlElement)node;
					if (xmlElement.IsEmpty)
					{
						writer.WriteNull();
					}
					else
					{
						writer.WriteValue(string.Empty);
					}
				}
				else
				{
					writer.WriteStartObject();
					for (int i = 0; i < node.Attributes.Count; i++)
					{
						SerializeNode(writer, node.Attributes[i], manager, writePropertyName: true);
					}
					SerializeGroupedNodes(writer, node, manager, writePropertyName: true);
					writer.WriteEndObject();
				}
				manager.PopScope();
				break;
			case XmlNodeType.Comment:
				if (writePropertyName)
				{
					writer.WriteComment(node.Value);
				}
				break;
			case XmlNodeType.Attribute:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				if ((!(node.NamespaceUri == "http://www.w3.org/2000/xmlns/") || !(node.Value == "http://james.newtonking.com/projects/json")) && (!(node.NamespaceUri == "http://james.newtonking.com/projects/json") || !(node.LocalName == "Array")))
				{
					if (writePropertyName)
					{
						writer.WritePropertyName(GetPropertyName(node, manager));
					}
					writer.WriteValue(node.Value);
				}
				break;
			case XmlNodeType.XmlDeclaration:
			{
				IXmlDeclaration xmlDeclaration = (IXmlDeclaration)node;
				writer.WritePropertyName(GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDeclaration.Version))
				{
					writer.WritePropertyName("@version");
					writer.WriteValue(xmlDeclaration.Version);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
				{
					writer.WritePropertyName("@encoding");
					writer.WriteValue(xmlDeclaration.Encoding);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
				{
					writer.WritePropertyName("@standalone");
					writer.WriteValue(xmlDeclaration.Standalone);
				}
				writer.WriteEndObject();
				break;
			}
			case XmlNodeType.DocumentType:
			{
				IXmlDocumentType xmlDocumentType = (IXmlDocumentType)node;
				writer.WritePropertyName(GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDocumentType.Name))
				{
					writer.WritePropertyName("@name");
					writer.WriteValue(xmlDocumentType.Name);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.Public))
				{
					writer.WritePropertyName("@public");
					writer.WriteValue(xmlDocumentType.Public);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.System))
				{
					writer.WritePropertyName("@system");
					writer.WriteValue(xmlDocumentType.System);
				}
				if (!string.IsNullOrEmpty(xmlDocumentType.InternalSubset))
				{
					writer.WritePropertyName("@internalSubset");
					writer.WriteValue(xmlDocumentType.InternalSubset);
				}
				writer.WriteEndObject();
				break;
			}
			default:
				throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			IXmlDocument xmlDocument = null;
			IXmlNode xmlNode = null;
			if (typeof(XObject).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XDocument) && objectType != typeof(XElement))
				{
					throw new JsonSerializationException("XmlNodeConverter only supports deserializing XDocument or XElement.");
				}
				XDocument document = new XDocument();
				xmlDocument = new XDocumentWrapper(document);
				xmlNode = xmlDocument;
			}
			if (typeof(XmlNode).IsAssignableFrom(objectType))
			{
				if (objectType != typeof(XmlDocument))
				{
					throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");
				}
				XmlDocument xmlDocument2 = new XmlDocument();
				xmlDocument2.XmlResolver = null;
				xmlDocument = new XmlDocumentWrapper(xmlDocument2);
				xmlNode = xmlDocument;
			}
			if (xmlDocument == null || xmlNode == null)
			{
				throw new JsonSerializationException("Unexpected type when converting XML: " + objectType);
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
			}
			if (!string.IsNullOrEmpty(DeserializeRootElementName))
			{
				ReadElement(reader, xmlDocument, xmlNode, DeserializeRootElementName, manager);
			}
			else
			{
				reader.Read();
				DeserializeNode(reader, xmlDocument, manager, xmlNode);
			}
			if (objectType == typeof(XElement))
			{
				XElement val = (XElement)xmlDocument.DocumentElement.WrappedNode;
				((XNode)val).Remove();
				return val;
			}
			return xmlDocument.WrappedNode;
		}

		private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
		{
			switch (propertyName)
			{
			case "#text":
				currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
				return;
			case "#cdata-section":
				currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
				return;
			case "#whitespace":
				currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
				return;
			case "#significant-whitespace":
				currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
				return;
			}
			if (!string.IsNullOrEmpty(propertyName) && propertyName[0] == '?')
			{
				CreateInstruction(reader, document, currentNode, propertyName);
			}
			else if (string.Equals(propertyName, "!DOCTYPE", StringComparison.OrdinalIgnoreCase))
			{
				CreateDocumentType(reader, document, currentNode);
			}
			else if (reader.TokenType == JsonToken.StartArray)
			{
				ReadArrayElements(reader, document, propertyName, currentNode, manager);
			}
			else
			{
				ReadElement(reader, document, currentNode, propertyName, manager);
			}
		}

		private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new JsonSerializationException("XmlNodeConverter cannot convert JSON with an empty property name to XML.");
			}
			Dictionary<string, string> dictionary = ReadAttributeElements(reader, manager);
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			if (StringUtils.StartsWith(propertyName, '@'))
			{
				string text = propertyName.Substring(1);
				string value = reader.Value.ToString();
				string prefix2 = MiscellaneousUtils.GetPrefix(text);
				IXmlNode attributeNode = ((!string.IsNullOrEmpty(prefix2)) ? document.CreateAttribute(text, manager.LookupNamespace(prefix2), value) : document.CreateAttribute(text, value));
				((IXmlElement)currentNode).SetAttributeNode(attributeNode);
				return;
			}
			IXmlElement xmlElement = CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				string prefix3 = MiscellaneousUtils.GetPrefix(item.Key);
				IXmlNode attributeNode2 = ((!string.IsNullOrEmpty(prefix3)) ? document.CreateAttribute(item.Key, manager.LookupNamespace(prefix3), item.Value) : document.CreateAttribute(item.Key, item.Value));
				xmlElement.SetAttributeNode(attributeNode2);
			}
			if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer || reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Boolean || reader.TokenType == JsonToken.Date)
			{
				xmlElement.AppendChild(document.CreateTextNode(ConvertTokenToXmlValue(reader)));
			}
			else if (reader.TokenType != JsonToken.Null)
			{
				if (reader.TokenType != JsonToken.EndObject)
				{
					manager.PushScope();
					DeserializeNode(reader, document, manager, xmlElement);
					manager.PopScope();
				}
				manager.RemoveNamespace(string.Empty, manager.DefaultNamespace);
			}
		}

		private string ConvertTokenToXmlValue(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.String)
			{
				return reader.Value.ToString();
			}
			if (reader.TokenType == JsonToken.Integer)
			{
				return XmlConvert.ToString(Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture));
			}
			if (reader.TokenType == JsonToken.Float)
			{
				if (reader.Value is decimal)
				{
					return XmlConvert.ToString((decimal)reader.Value);
				}
				if (reader.Value is float)
				{
					return XmlConvert.ToString((float)reader.Value);
				}
				return XmlConvert.ToString(Convert.ToDouble(reader.Value, CultureInfo.InvariantCulture));
			}
			if (reader.TokenType == JsonToken.Boolean)
			{
				return XmlConvert.ToString(Convert.ToBoolean(reader.Value, CultureInfo.InvariantCulture));
			}
			if (reader.TokenType == JsonToken.Date)
			{
				if (reader.Value is DateTimeOffset)
				{
					return XmlConvert.ToString((DateTimeOffset)reader.Value);
				}
				DateTime dateTime = Convert.ToDateTime(reader.Value, CultureInfo.InvariantCulture);
				return XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.Kind));
			}
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			throw JsonSerializationException.Create(reader, "Cannot get an XML string value from token type '{0}'.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
		{
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			int num = 0;
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				DeserializeValue(reader, document, manager, propertyName, xmlElement);
				num++;
			}
			if (WriteArrayAttribute)
			{
				AddJsonArrayAttribute(xmlElement, document);
			}
			if (num == 1 && WriteArrayAttribute)
			{
				IXmlElement element = xmlElement.ChildNodes.OfType<IXmlElement>().Single((IXmlElement n) => n.LocalName == propertyName);
				AddJsonArrayAttribute(element, document);
			}
		}

		private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
		{
			element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
			if (element is XElementWrapper && element.GetPrefixOfNamespace("http://james.newtonking.com/projects/json") == null)
			{
				element.SetAttributeNode(document.CreateAttribute("xmlns:json", "http://www.w3.org/2000/xmlns/", "http://james.newtonking.com/projects/json"));
			}
		}

		private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = false;
			bool flag2 = false;
			if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor)
			{
				while (!flag && !flag2 && reader.Read())
				{
					switch (reader.TokenType)
					{
					case JsonToken.PropertyName:
					{
						string text = reader.Value.ToString();
						if (!string.IsNullOrEmpty(text))
						{
							switch (text[0])
							{
							case '@':
							{
								text = text.Substring(1);
								reader.Read();
								string value = ConvertTokenToXmlValue(reader);
								dictionary.Add(text, value);
								if (IsNamespaceAttribute(text, out var prefix))
								{
									manager.AddNamespace(prefix, value);
								}
								break;
							}
							case '$':
							{
								text = text.Substring(1);
								reader.Read();
								string value = reader.Value.ToString();
								string text2 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
								if (text2 == null)
								{
									int? num = null;
									while (manager.LookupNamespace("json" + num) != null)
									{
										num = num.GetValueOrDefault() + 1;
									}
									text2 = "json" + num;
									dictionary.Add("xmlns:" + text2, "http://james.newtonking.com/projects/json");
									manager.AddNamespace(text2, "http://james.newtonking.com/projects/json");
								}
								dictionary.Add(text2 + ":" + text, value);
								break;
							}
							default:
								flag = true;
								break;
							}
						}
						else
						{
							flag = true;
						}
						break;
					}
					case JsonToken.EndObject:
						flag2 = true;
						break;
					case JsonToken.Comment:
						flag2 = true;
						break;
					default:
						throw new JsonSerializationException("Unexpected JsonToken: " + reader.TokenType);
					}
				}
			}
			return dictionary;
		}

		private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
		{
			if (propertyName == "?xml")
			{
				string version = null;
				string encoding = null;
				string standalone = null;
				while (reader.Read() && reader.TokenType != JsonToken.EndObject)
				{
					switch (reader.Value.ToString())
					{
					case "@version":
						reader.Read();
						version = reader.Value.ToString();
						break;
					case "@encoding":
						reader.Read();
						encoding = reader.Value.ToString();
						break;
					case "@standalone":
						reader.Read();
						standalone = reader.Value.ToString();
						break;
					default:
						throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
					}
				}
				IXmlNode newChild = document.CreateXmlDeclaration(version, encoding, standalone);
				currentNode.AppendChild(newChild);
			}
			else
			{
				IXmlNode newChild2 = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
				currentNode.AppendChild(newChild2);
			}
		}

		private void CreateDocumentType(JsonReader reader, IXmlDocument document, IXmlNode currentNode)
		{
			string name = null;
			string publicId = null;
			string systemId = null;
			string internalSubset = null;
			while (reader.Read() && reader.TokenType != JsonToken.EndObject)
			{
				switch (reader.Value.ToString())
				{
				case "@name":
					reader.Read();
					name = reader.Value.ToString();
					break;
				case "@public":
					reader.Read();
					publicId = reader.Value.ToString();
					break;
				case "@system":
					reader.Read();
					systemId = reader.Value.ToString();
					break;
				case "@internalSubset":
					reader.Read();
					internalSubset = reader.Value.ToString();
					break;
				default:
					throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
				}
			}
			IXmlNode newChild = document.CreateXmlDocumentType(name, publicId, systemId, internalSubset);
			currentNode.AppendChild(newChild);
		}

		private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			string text = (string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix));
			return (!string.IsNullOrEmpty(text)) ? document.CreateElement(elementName, text) : document.CreateElement(elementName);
		}

		private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
		{
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
					{
						throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
					}
					string propertyName = reader.Value.ToString();
					reader.Read();
					if (reader.TokenType == JsonToken.StartArray)
					{
						int num = 0;
						while (reader.Read() && reader.TokenType != JsonToken.EndArray)
						{
							DeserializeValue(reader, document, manager, propertyName, currentNode);
							num++;
						}
						if (num == 1 && WriteArrayAttribute)
						{
							IXmlElement element = currentNode.ChildNodes.OfType<IXmlElement>().Single((IXmlElement n) => n.LocalName == propertyName);
							AddJsonArrayAttribute(element, document);
						}
					}
					else
					{
						DeserializeValue(reader, document, manager, propertyName, currentNode);
					}
					break;
				}
				case JsonToken.StartConstructor:
				{
					string propertyName2 = reader.Value.ToString();
					while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
					{
						DeserializeValue(reader, document, manager, propertyName2, currentNode);
					}
					break;
				}
				case JsonToken.Comment:
					currentNode.AppendChild(document.CreateComment((string)reader.Value));
					break;
				case JsonToken.EndObject:
				case JsonToken.EndArray:
					return;
				default:
					throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + reader.TokenType);
				}
			}
			while (reader.TokenType == JsonToken.PropertyName || reader.Read());
		}

		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
			{
				if (attributeName.Length == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName[5] == ':')
				{
					prefix = attributeName.Substring(6, attributeName.Length - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		private IEnumerable<IXmlNode> ValueAttributes(IEnumerable<IXmlNode> c)
		{
			return c.Where((IXmlNode a) => a.NamespaceUri != "http://james.newtonking.com/projects/json");
		}

		public override bool CanConvert(Type valueType)
		{
			if (typeof(XObject).IsAssignableFrom(valueType))
			{
				return true;
			}
			if (typeof(XmlNode).IsAssignableFrom(valueType))
			{
				return true;
			}
			return false;
		}
	}
}
namespace Newtonsoft.Json
{
	internal enum ReadType
	{
		Read,
		ReadAsInt32,
		ReadAsBytes,
		ReadAsString,
		ReadAsDecimal,
		ReadAsDateTime,
		ReadAsDateTimeOffset
	}
	public class JsonTextReader : JsonReader, IJsonLineInfo
	{
		private const char UnicodeReplacementChar = '\ufffd';

		private const int MaximumJavascriptIntegerCharacterLength = 380;

		private readonly TextReader _reader;

		private char[] _chars;

		private int _charsUsed;

		private int _charPos;

		private int _lineStartPos;

		private int _lineNumber;

		private bool _isEndOfFile;

		private StringBuffer _buffer;

		private StringReference _stringReference;

		internal PropertyNameTable NameTable;

		public int LineNumber
		{
			get
			{
				if (base.CurrentState == State.Start && LinePosition == 0)
				{
					return 0;
				}
				return _lineNumber;
			}
		}

		public int LinePosition => _charPos - _lineStartPos;

		public JsonTextReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			_reader = reader;
			_lineNumber = 1;
			_chars = new char[1025];
		}

		private StringBuffer GetBuffer()
		{
			if (_buffer == null)
			{
				_buffer = new StringBuffer(1025);
			}
			else
			{
				_buffer.Position = 0;
			}
			return _buffer;
		}

		private void OnNewLine(int pos)
		{
			_lineNumber++;
			_lineStartPos = pos - 1;
		}

		private void ParseString(char quote)
		{
			_charPos++;
			ShiftBufferIfNeeded();
			ReadStringIntoBuffer(quote);
			SetPostValueState(updateIndex: true);
			if (_readType == ReadType.ReadAsBytes)
			{
				byte[] value = ((_stringReference.Length != 0) ? Convert.FromBase64CharArray(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length) : new byte[0]);
				SetToken(JsonToken.Bytes, value, updateIndex: false);
				return;
			}
			if (_readType == ReadType.ReadAsString)
			{
				string value2 = _stringReference.ToString();
				SetToken(JsonToken.String, value2, updateIndex: false);
				_quoteChar = quote;
				return;
			}
			string text = _stringReference.ToString();
			if (_dateParseHandling != 0)
			{
				DateParseHandling dateParseHandling = ((_readType == ReadType.ReadAsDateTime) ? DateParseHandling.DateTime : ((_readType != ReadType.ReadAsDateTimeOffset) ? _dateParseHandling : DateParseHandling.DateTimeOffset));
				if (DateTimeUtils.TryParseDateTime(text, dateParseHandling, base.DateTimeZoneHandling, base.DateFormatString, base.Culture, out var dt))
				{
					SetToken(JsonToken.Date, dt, updateIndex: false);
					return;
				}
			}
			SetToken(JsonToken.String, text, updateIndex: false);
			_quoteChar = quote;
		}

		private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
		{
			Buffer.BlockCopy(src, srcOffset * 2, dst, dstOffset * 2, count * 2);
		}

		private void ShiftBufferIfNeeded()
		{
			int num = _chars.Length;
			if ((double)(num - _charPos) <= (double)num * 0.1)
			{
				int num2 = _charsUsed - _charPos;
				if (num2 > 0)
				{
					BlockCopyChars(_chars, _charPos, _chars, 0, num2);
				}
				_lineStartPos -= _charPos;
				_charPos = 0;
				_charsUsed = num2;
				_chars[_charsUsed] = '\0';
			}
		}

		private int ReadData(bool append)
		{
			return ReadData(append, 0);
		}

		private int ReadData(bool append, int charsRequired)
		{
			if (_isEndOfFile)
			{
				return 0;
			}
			if (_charsUsed + charsRequired >= _chars.Length - 1)
			{
				if (append)
				{
					int num = Math.Max(_chars.Length * 2, _charsUsed + charsRequired + 1);
					char[] array = new char[num];
					BlockCopyChars(_chars, 0, array, 0, _chars.Length);
					_chars = array;
				}
				else
				{
					int num2 = _charsUsed - _charPos;
					if (num2 + charsRequired + 1 >= _chars.Length)
					{
						char[] array2 = new char[num2 + charsRequired + 1];
						if (num2 > 0)
						{
							BlockCopyChars(_chars, _charPos, array2, 0, num2);
						}
						_chars = array2;
					}
					else if (num2 > 0)
					{
						BlockCopyChars(_chars, _charPos, _chars, 0, num2);
					}
					_lineStartPos -= _charPos;
					_charPos = 0;
					_charsUsed = num2;
				}
			}
			int count = _chars.Length - _charsUsed - 1;
			int num3 = _reader.Read(_chars, _charsUsed, count);
			_charsUsed += num3;
			if (num3 == 0)
			{
				_isEndOfFile = true;
			}
			_chars[_charsUsed] = '\0';
			return num3;
		}

		private bool EnsureChars(int relativePosition, bool append)
		{
			if (_charPos + relativePosition >= _charsUsed)
			{
				return ReadChars(relativePosition, append);
			}
			return true;
		}

		private bool ReadChars(int relativePosition, bool append)
		{
			if (_isEndOfFile)
			{
				return false;
			}
			int num = _charPos + relativePosition - _charsUsed + 1;
			int num2 = 0;
			do
			{
				int num3 = ReadData(append, num - num2);
				if (num3 == 0)
				{
					break;
				}
				num2 += num3;
			}
			while (num2 < num);
			if (num2 < num)
			{
				return false;
			}
			return true;
		}

		[DebuggerStepThrough]
		public override bool Read()
		{
			_readType = ReadType.Read;
			if (!ReadInternal())
			{
				SetToken(JsonToken.None);
				return false;
			}
			return true;
		}

		public override byte[] ReadAsBytes()
		{
			return ReadAsBytesInternal();
		}

		public override decimal? ReadAsDecimal()
		{
			return ReadAsDecimalInternal();
		}

		public override int? ReadAsInt32()
		{
			return ReadAsInt32Internal();
		}

		public override string ReadAsString()
		{
			return ReadAsStringInternal();
		}

		public override DateTime? ReadAsDateTime()
		{
			return ReadAsDateTimeInternal();
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return ReadAsDateTimeOffsetInternal();
		}

		internal override bool ReadInternal()
		{
			do
			{
				switch (_currentState)
				{
				case State.Start:
				case State.Property:
				case State.ArrayStart:
				case State.Array:
				case State.ConstructorStart:
				case State.Constructor:
					return ParseValue();
				case State.ObjectStart:
				case State.Object:
					return ParseObject();
				case State.PostValue:
					break;
				case State.Finished:
					if (EnsureChars(0, append: false))
					{
						EatWhitespace(oneOrMore: false);
						if (_isEndOfFile)
						{
							return false;
						}
						if (_chars[_charPos] == '/')
						{
							ParseComment();
							return true;
						}
						throw JsonReaderException.Create(this, "Additional text encountered after finished reading JSON content: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
					}
					return false;
				default:
					throw JsonReaderException.Create(this, "Unexpected state: {0}.".FormatWith(CultureInfo.InvariantCulture, base.CurrentState));
				}
			}
			while (!ParsePostValue());
			return true;
		}

		private void ReadStringIntoBuffer(char quote)
		{
			int num = _charPos;
			int charPos = _charPos;
			int num2 = _charPos;
			StringBuffer stringBuffer = null;
			while (true)
			{
				switch (_chars[num++])
				{
				case '\0':
					if (_charsUsed == num - 1)
					{
						num--;
						if (ReadData(append: true) == 0)
						{
							_charPos = num;
							throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
						}
					}
					break;
				case '\\':
				{
					_charPos = num;
					if (!EnsureChars(0, append: true))
					{
						_charPos = num;
						throw JsonReaderException.Create(this, "Unterminated string. Expected delimiter: {0}.".FormatWith(CultureInfo.InvariantCulture, quote));
					}
					int writeToPosition = num - 1;
					char c = _chars[num];
					char c2;
					switch (c)
					{
					case 'b':
						num++;
						c2 = '\b';
						break;
					case 't':
						num++;
						c2 = '\t';
						break;
					case 'n':
						num++;
						c2 = '\n';
						break;
					case 'f':
						num++;
						c2 = '\f';
						break;
					case 'r':
						num++;
						c2 = '\r';
						break;
					case '\\':
						num++;
						c2 = '\\';
						break;
					case '"':
					case '\'':
					case '/':
						c2 = c;
						num++;
						break;
					case 'u':
						num++;
						_charPos = num;
						c2 = ParseUnicode();
						if (StringUtils.IsLowSurrogate(c2))
						{
							c2 = '\ufffd';
						}
						else if (StringUtils.IsHighSurrogate(c2))
						{
							bool flag;
							do
							{
								flag = false;
								if (EnsureChars(2, append: true) && _chars[_charPos] == '\\' && _chars[_charPos + 1] == 'u')
								{
									char writeChar = c2;
									_charPos += 2;
									c2 = ParseUnicode();
									if (!StringUtils.IsLowSurrogate(c2))
									{
										if (StringUtils.IsHighSurrogate(c2))
										{
											writeChar = '\ufffd';
											flag = true;
										}
										else
										{
											writeChar = '\ufffd';
										}
									}
									if (stringBuffer == null)
									{
										stringBuffer = GetBuffer();
									}
									WriteCharToBuffer(stringBuffer, writeChar, num2, writeToPosition);
									num2 = _charPos;
								}
								else
								{
									c2 = '\ufffd';
								}
							}
							while (flag);
						}
						num = _charPos;
						break;
					default:
						num++;
						_charPos = num;
						throw JsonReaderException.Create(this, "Bad JSON escape sequence: {0}.".FormatWith(CultureInfo.InvariantCulture, "\\" + c));
					}
					if (stringBuffer == null)
					{
						stringBuffer = GetBuffer();
					}
					WriteCharToBuffer(stringBuffer, c2, num2, writeToPosition);
					num2 = num;
					break;
				}
				case '\r':
					_charPos = num - 1;
					ProcessCarriageReturn(append: true);
					num = _charPos;
					break;
				case '\n':
					_charPos = num - 1;
					ProcessLineFeed();
					num = _charPos;
					break;
				case '"':
				case '\'':
					if (_chars[num - 1] != quote)
					{
						break;
					}
					num--;
					if (charPos == num2)
					{
						_stringReference = new StringReference(_chars, charPos, num - charPos);
					}
					else
					{
						if (stringBuffer == null)
						{
							stringBuffer = GetBuffer();
						}
						if (num > num2)
						{
							stringBuffer.Append(_chars, num2, num - num2);
						}
						_stringReference = new StringReference(stringBuffer.GetInternalBuffer(), 0, stringBuffer.Position);
					}
					num++;
					_charPos = num;
					return;
				}
			}
		}

		private void WriteCharToBuffer(StringBuffer buffer, char writeChar, int lastWritePosition, int writeToPosition)
		{
			if (writeToPosition > lastWritePosition)
			{
				buffer.Append(_chars, lastWritePosition, writeToPosition - lastWritePosition);
			}
			buffer.Append(writeChar);
		}

		private char ParseUnicode()
		{
			if (EnsureChars(4, append: true))
			{
				string s = new string(_chars, _charPos, 4);
				char c = Convert.ToChar(int.Parse(s, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo));
				char result = c;
				_charPos += 4;
				return result;
			}
			throw JsonReaderException.Create(this, "Unexpected end while parsing unicode character.");
		}

		private void ReadNumberIntoBuffer()
		{
			int num = _charPos;
			while (true)
			{
				switch (_chars[num])
				{
				case '\0':
					_charPos = num;
					if (_charsUsed != num || ReadData(append: true) == 0)
					{
						return;
					}
					continue;
				case '+':
				case '-':
				case '.':
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
				case 'A':
				case 'B':
				case 'C':
				case 'D':
				case 'E':
				case 'F':
				case 'X':
				case 'a':
				case 'b':
				case 'c':
				case 'd':
				case 'e':
				case 'f':
				case 'x':
					num++;
					continue;
				}
				_charPos = num;
				char c = _chars[_charPos];
				if (char.IsWhiteSpace(c) || c == ',' || c == '}' || c == ']' || c == ')' || c == '/')
				{
					return;
				}
				throw JsonReaderException.Create(this, "Unexpected character encountered while parsing number: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
			}
		}

		private void ClearRecentString()
		{
			if (_buffer != null)
			{
				_buffer.Position = 0;
			}
			_stringReference = default(StringReference);
		}

		private bool ParsePostValue()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							_currentState = State.Finished;
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '}':
					_charPos++;
					SetToken(JsonToken.EndObject);
					return true;
				case ']':
					_charPos++;
					SetToken(JsonToken.EndArray);
					return true;
				case ')':
					_charPos++;
					SetToken(JsonToken.EndConstructor);
					return true;
				case '/':
					ParseComment();
					return true;
				case ',':
					_charPos++;
					SetStateBasedOnCurrent();
					return false;
				case '\t':
				case ' ':
					_charPos++;
					break;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					throw JsonReaderException.Create(this, "After parsing a value an unexpected character was encountered: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
			}
		}

		private bool ParseObject()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '}':
					SetToken(JsonToken.EndObject);
					_charPos++;
					return true;
				case '/':
					ParseComment();
					return true;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				case '\t':
				case ' ':
					_charPos++;
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					return ParseProperty();
				}
			}
		}

		private bool ParseProperty()
		{
			char c = _chars[_charPos];
			char c2;
			if (c == '"' || c == '\'')
			{
				_charPos++;
				c2 = c;
				ShiftBufferIfNeeded();
				ReadStringIntoBuffer(c2);
			}
			else
			{
				if (!ValidIdentifierChar(c))
				{
					throw JsonReaderException.Create(this, "Invalid property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				c2 = '\0';
				ShiftBufferIfNeeded();
				ParseUnquotedProperty();
			}
			string text;
			if (NameTable != null)
			{
				text = NameTable.Get(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length);
				if (text == null)
				{
					text = _stringReference.ToString();
				}
			}
			else
			{
				text = _stringReference.ToString();
			}
			EatWhitespace(oneOrMore: false);
			if (_chars[_charPos] != ':')
			{
				throw JsonReaderException.Create(this, "Invalid character after parsing property name. Expected ':' but got: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
			}
			_charPos++;
			SetToken(JsonToken.PropertyName, text);
			_quoteChar = c2;
			ClearRecentString();
			return true;
		}

		private bool ValidIdentifierChar(char value)
		{
			if (!char.IsLetterOrDigit(value) && value != '_')
			{
				return value == '$';
			}
			return true;
		}

		private void ParseUnquotedProperty()
		{
			int charPos = _charPos;
			char c;
			while (true)
			{
				if (_chars[_charPos] == '\0')
				{
					if (_charsUsed != _charPos)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						return;
					}
					if (ReadData(append: true) == 0)
					{
						throw JsonReaderException.Create(this, "Unexpected end while parsing unquoted property name.");
					}
				}
				else
				{
					c = _chars[_charPos];
					if (!ValidIdentifierChar(c))
					{
						break;
					}
					_charPos++;
				}
			}
			if (char.IsWhiteSpace(c) || c == ':')
			{
				_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
				return;
			}
			throw JsonReaderException.Create(this, "Invalid JavaScript property identifier character: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
		}

		private bool ParseValue()
		{
			while (true)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							return false;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '"':
				case '\'':
					ParseString(c);
					return true;
				case 't':
					ParseTrue();
					return true;
				case 'f':
					ParseFalse();
					return true;
				case 'n':
					if (EnsureChars(1, append: true))
					{
						switch (_chars[_charPos + 1])
						{
						case 'u':
							ParseNull();
							break;
						case 'e':
							ParseConstructor();
							break;
						default:
							throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
						}
						return true;
					}
					throw JsonReaderException.Create(this, "Unexpected end.");
				case 'N':
					ParseNumberNaN();
					return true;
				case 'I':
					ParseNumberPositiveInfinity();
					return true;
				case '-':
					if (EnsureChars(1, append: true) && _chars[_charPos + 1] == 'I')
					{
						ParseNumberNegativeInfinity();
					}
					else
					{
						ParseNumber();
					}
					return true;
				case '/':
					ParseComment();
					return true;
				case 'u':
					ParseUndefined();
					return true;
				case '{':
					_charPos++;
					SetToken(JsonToken.StartObject);
					return true;
				case '[':
					_charPos++;
					SetToken(JsonToken.StartArray);
					return true;
				case ']':
					_charPos++;
					SetToken(JsonToken.EndArray);
					return true;
				case ',':
					SetToken(JsonToken.Undefined);
					return true;
				case ')':
					_charPos++;
					SetToken(JsonToken.EndConstructor);
					return true;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				case '\t':
				case ' ':
					_charPos++;
					break;
				default:
					if (char.IsWhiteSpace(c))
					{
						_charPos++;
						break;
					}
					if (char.IsNumber(c) || c == '-' || c == '.')
					{
						ParseNumber();
						return true;
					}
					throw JsonReaderException.Create(this, "Unexpected character encountered while parsing value: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
				}
			}
		}

		private void ProcessLineFeed()
		{
			_charPos++;
			OnNewLine(_charPos);
		}

		private void ProcessCarriageReturn(bool append)
		{
			_charPos++;
			if (EnsureChars(1, append) && _chars[_charPos] == '\n')
			{
				_charPos++;
			}
			OnNewLine(_charPos);
		}

		private bool EatWhitespace(bool oneOrMore)
		{
			bool flag = false;
			bool result = false;
			while (!flag)
			{
				char c = _chars[_charPos];
				switch (c)
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: false) == 0)
						{
							flag = true;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '\r':
					ProcessCarriageReturn(append: false);
					break;
				case '\n':
					ProcessLineFeed();
					break;
				default:
					if (c == ' ' || char.IsWhiteSpace(c))
					{
						result = true;
						_charPos++;
					}
					else
					{
						flag = true;
					}
					break;
				}
			}
			if (oneOrMore)
			{
				return result;
			}
			return true;
		}

		private void ParseConstructor()
		{
			if (MatchValueWithTrailingSeparator("new"))
			{
				EatWhitespace(oneOrMore: false);
				int charPos = _charPos;
				int charPos2;
				while (true)
				{
					char c = _chars[_charPos];
					if (c == '\0')
					{
						if (_charsUsed == _charPos)
						{
							if (ReadData(append: true) == 0)
							{
								throw JsonReaderException.Create(this, "Unexpected end while parsing constructor.");
							}
							continue;
						}
						charPos2 = _charPos;
						_charPos++;
						break;
					}
					if (char.IsLetterOrDigit(c))
					{
						_charPos++;
						continue;
					}
					switch (c)
					{
					case '\r':
						charPos2 = _charPos;
						ProcessCarriageReturn(append: true);
						break;
					case '\n':
						charPos2 = _charPos;
						ProcessLineFeed();
						break;
					default:
						if (char.IsWhiteSpace(c))
						{
							charPos2 = _charPos;
							_charPos++;
							break;
						}
						if (c == '(')
						{
							charPos2 = _charPos;
							break;
						}
						throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, c));
					}
					break;
				}
				_stringReference = new StringReference(_chars, charPos, charPos2 - charPos);
				string value = _stringReference.ToString();
				EatWhitespace(oneOrMore: false);
				if (_chars[_charPos] != '(')
				{
					throw JsonReaderException.Create(this, "Unexpected character while parsing constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				_charPos++;
				ClearRecentString();
				SetToken(JsonToken.StartConstructor, value);
				return;
			}
			throw JsonReaderException.Create(this, "Unexpected content while parsing JSON.");
		}

		private void ParseNumber()
		{
			ShiftBufferIfNeeded();
			char c = _chars[_charPos];
			int charPos = _charPos;
			ReadNumberIntoBuffer();
			SetPostValueState(updateIndex: true);
			_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
			bool flag = char.IsDigit(c) && _stringReference.Length == 1;
			bool flag2 = c == '0' && _stringReference.Length > 1 && _stringReference.Chars[_stringReference.StartIndex + 1] != '.' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'e' && _stringReference.Chars[_stringReference.StartIndex + 1] != 'E';
			object value;
			JsonToken newToken;
			if (_readType == ReadType.ReadAsInt32)
			{
				if (flag)
				{
					value = c - 48;
				}
				else if (flag2)
				{
					string text = _stringReference.ToString();
					try
					{
						int num = (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(text, 16) : Convert.ToInt32(text, 8));
						value = num;
					}
					catch (Exception ex)
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, text), ex);
					}
				}
				else
				{
					int value2;
					switch (ConvertUtils.Int32TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out value2))
					{
					case ParseResult.Success:
						break;
					case ParseResult.Overflow:
						throw JsonReaderException.Create(this, "JSON integer {0} is too large or small for an Int32.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					default:
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid integer.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = value2;
				}
				newToken = JsonToken.Integer;
			}
			else if (_readType == ReadType.ReadAsDecimal)
			{
				if (flag)
				{
					value = (decimal)c - 48m;
				}
				else if (flag2)
				{
					string text2 = _stringReference.ToString();
					try
					{
						long value3 = (text2.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text2, 16) : Convert.ToInt64(text2, 8));
						value = Convert.ToDecimal(value3);
					}
					catch (Exception ex2)
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text2), ex2);
					}
				}
				else
				{
					string s = _stringReference.ToString();
					if (!decimal.TryParse(s, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var result))
					{
						throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = result;
				}
				newToken = JsonToken.Float;
			}
			else if (flag)
			{
				value = (long)c - 48L;
				newToken = JsonToken.Integer;
			}
			else if (flag2)
			{
				string text3 = _stringReference.ToString();
				try
				{
					value = (text3.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(text3, 16) : Convert.ToInt64(text3, 8));
				}
				catch (Exception ex3)
				{
					throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text3), ex3);
				}
				newToken = JsonToken.Integer;
			}
			else
			{
				long value4;
				switch (ConvertUtils.Int64TryParse(_stringReference.Chars, _stringReference.StartIndex, _stringReference.Length, out value4))
				{
				case ParseResult.Success:
					value = value4;
					newToken = JsonToken.Integer;
					break;
				case ParseResult.Overflow:
				{
					string text5 = _stringReference.ToString();
					if (text5.Length > 380)
					{
						throw JsonReaderException.Create(this, "JSON integer {0} is too large to parse.".FormatWith(CultureInfo.InvariantCulture, _stringReference.ToString()));
					}
					value = BigIntegerParse(text5, CultureInfo.InvariantCulture);
					newToken = JsonToken.Integer;
					break;
				}
				default:
				{
					string text4 = _stringReference.ToString();
					if (_floatParseHandling == FloatParseHandling.Decimal)
					{
						if (!decimal.TryParse(text4, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var result2))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid decimal.".FormatWith(CultureInfo.InvariantCulture, text4));
						}
						value = result2;
					}
					else
					{
						if (!double.TryParse(text4, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var result3))
						{
							throw JsonReaderException.Create(this, "Input string '{0}' is not a valid number.".FormatWith(CultureInfo.InvariantCulture, text4));
						}
						value = result3;
					}
					newToken = JsonToken.Float;
					break;
				}
				}
			}
			ClearRecentString();
			SetToken(newToken, value, updateIndex: false);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static object BigIntegerParse(string number, CultureInfo culture)
		{
			return BigInteger.Parse(number, culture);
		}

		private void ParseComment()
		{
			_charPos++;
			if (!EnsureChars(1, append: false))
			{
				throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
			}
			bool flag;
			if (_chars[_charPos] == '*')
			{
				flag = false;
			}
			else
			{
				if (_chars[_charPos] != '/')
				{
					throw JsonReaderException.Create(this, "Error parsing comment. Expected: *, got {0}.".FormatWith(CultureInfo.InvariantCulture, _chars[_charPos]));
				}
				flag = true;
			}
			_charPos++;
			int charPos = _charPos;
			bool flag2 = false;
			while (!flag2)
			{
				switch (_chars[_charPos])
				{
				case '\0':
					if (_charsUsed == _charPos)
					{
						if (ReadData(append: true) == 0)
						{
							if (!flag)
							{
								throw JsonReaderException.Create(this, "Unexpected end while parsing comment.");
							}
							_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
							flag2 = true;
						}
					}
					else
					{
						_charPos++;
					}
					break;
				case '*':
					_charPos++;
					if (!flag && EnsureChars(0, append: true) && _chars[_charPos] == '/')
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos - 1);
						_charPos++;
						flag2 = true;
					}
					break;
				case '\r':
					if (flag)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						flag2 = true;
					}
					ProcessCarriageReturn(append: true);
					break;
				case '\n':
					if (flag)
					{
						_stringReference = new StringReference(_chars, charPos, _charPos - charPos);
						flag2 = true;
					}
					ProcessLineFeed();
					break;
				default:
					_charPos++;
					break;
				}
			}
			SetToken(JsonToken.Comment, _stringReference.ToString());
			ClearRecentString();
		}

		private bool MatchValue(string value)
		{
			if (!EnsureChars(value.Length - 1, append: true))
			{
				return false;
			}
			for (int i = 0; i < value.Length; i++)
			{
				if (_chars[_charPos + i] != value[i])
				{
					return false;
				}
			}
			_charPos += value.Length;
			return true;
		}

		private bool MatchValueWithTrailingSeparator(string value)
		{
			if (!MatchValue(value))
			{
				return false;
			}
			if (!EnsureChars(0, append: false))
			{
				return true;
			}
			if (!IsSeparator(_chars[_charPos]))
			{
				return _chars[_charPos] == '\0';
			}
			return true;
		}

		private bool IsSeparator(char c)
		{
			switch (c)
			{
			case ',':
			case ']':
			case '}':
				return true;
			case '/':
			{
				if (!EnsureChars(1, append: false))
				{
					return false;
				}
				char c2 = _chars[_charPos + 1];
				if (c2 != '*')
				{
					return c2 == '/';
				}
				return true;
			}
			case ')':
				if (base.CurrentState == State.Constructor || base.CurrentState == State.ConstructorStart)
				{
					return true;
				}
				break;
			case '\t':
			case '\n':
			case '\r':
			case ' ':
				return true;
			default:
				if (char.IsWhiteSpace(c))
				{
					return true;
				}
				break;
			}
			return false;
		}

		private void ParseTrue()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.True))
			{
				SetToken(JsonToken.Boolean, true);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		private void ParseNull()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.Null))
			{
				SetToken(JsonToken.Null);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing null value.");
		}

		private void ParseUndefined()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.Undefined))
			{
				SetToken(JsonToken.Undefined);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing undefined value.");
		}

		private void ParseFalse()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.False))
			{
				SetToken(JsonToken.Boolean, false);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing boolean value.");
		}

		private void ParseNumberNegativeInfinity()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.NegativeInfinity))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read -Infinity as a decimal.");
				}
				SetToken(JsonToken.Float, double.NegativeInfinity);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing negative infinity value.");
		}

		private void ParseNumberPositiveInfinity()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.PositiveInfinity))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read Infinity as a decimal.");
				}
				SetToken(JsonToken.Float, double.PositiveInfinity);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing positive infinity value.");
		}

		private void ParseNumberNaN()
		{
			if (MatchValueWithTrailingSeparator(JsonConvert.NaN))
			{
				if (_floatParseHandling == FloatParseHandling.Decimal)
				{
					throw new JsonReaderException("Cannot read NaN as a decimal.");
				}
				SetToken(JsonToken.Float, double.NaN);
				return;
			}
			throw JsonReaderException.Create(this, "Error parsing NaN value.");
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseInput && _reader != null)
			{
				_reader.Close();
			}
			if (_buffer != null)
			{
				_buffer.Clear();
			}
		}

		public bool HasLineInfo()
		{
			return true;
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : Attribute
	{
		internal NullValueHandling? _nullValueHandling;

		internal DefaultValueHandling? _defaultValueHandling;

		internal ReferenceLoopHandling? _referenceLoopHandling;

		internal ObjectCreationHandling? _objectCreationHandling;

		internal TypeNameHandling? _typeNameHandling;

		internal bool? _isReference;

		internal int? _order;

		internal Required? _required;

		internal bool? _itemIsReference;

		internal ReferenceLoopHandling? _itemReferenceLoopHandling;

		internal TypeNameHandling? _itemTypeNameHandling;

		public Type ItemConverterType { get; set; }

		public object[] ItemConverterParameters { get; set; }

		public NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling ?? NullValueHandling.Include;
			}
			set
			{
				_nullValueHandling = value;
			}
		}

		public DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling ?? DefaultValueHandling.Include;
			}
			set
			{
				_defaultValueHandling = value;
			}
		}

		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling ?? ReferenceLoopHandling.Error;
			}
			set
			{
				_referenceLoopHandling = value;
			}
		}

		public ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling ?? ObjectCreationHandling.Auto;
			}
			set
			{
				_objectCreationHandling = value;
			}
		}

		public TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling ?? TypeNameHandling.None;
			}
			set
			{
				_typeNameHandling = value;
			}
		}

		public bool IsReference
		{
			get
			{
				return _isReference ?? false;
			}
			set
			{
				_isReference = value;
			}
		}

		public int Order
		{
			get
			{
				return _order ?? 0;
			}
			set
			{
				_order = value;
			}
		}

		public Required Required
		{
			get
			{
				return _required ?? Required.Default;
			}
			set
			{
				_required = value;
			}
		}

		public string PropertyName { get; set; }

		public ReferenceLoopHandling ItemReferenceLoopHandling
		{
			get
			{
				return _itemReferenceLoopHandling ?? ReferenceLoopHandling.Error;
			}
			set
			{
				_itemReferenceLoopHandling = value;
			}
		}

		public TypeNameHandling ItemTypeNameHandling
		{
			get
			{
				return _itemTypeNameHandling ?? TypeNameHandling.None;
			}
			set
			{
				_itemTypeNameHandling = value;
			}
		}

		public bool ItemIsReference
		{
			get
			{
				return _itemIsReference ?? false;
			}
			set
			{
				_itemIsReference = value;
			}
		}

		public JsonPropertyAttribute()
		{
		}

		public JsonPropertyAttribute(string propertyName)
		{
			PropertyName = propertyName;
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class JsonIgnoreAttribute : Attribute
	{
	}
	public class JsonTextWriter : JsonWriter
	{
		private readonly TextWriter _writer;

		private Base64Encoder _base64Encoder;

		private char _indentChar;

		private int _indentation;

		private char _quoteChar;

		private bool _quoteName;

		private bool[] _charEscapeFlags;

		private char[] _writeBuffer;

		private char[] _indentChars;

		private Base64Encoder Base64Encoder
		{
			get
			{
				if (_base64Encoder == null)
				{
					_base64Encoder = new Base64Encoder(_writer);
				}
				return _base64Encoder;
			}
		}

		public int Indentation
		{
			get
			{
				return _indentation;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException("Indentation value must be greater than 0.");
				}
				_indentation = value;
			}
		}

		public char QuoteChar
		{
			get
			{
				return _quoteChar;
			}
			set
			{
				if (value != '"' && value != '\'')
				{
					throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
				}
				_quoteChar = value;
				UpdateCharEscapeFlags();
			}
		}

		public char IndentChar
		{
			get
			{
				return _indentChar;
			}
			set
			{
				if (value != _indentChar)
				{
					_indentChar = value;
					_indentChars = null;
				}
			}
		}

		public bool QuoteName
		{
			get
			{
				return _quoteName;
			}
			set
			{
				_quoteName = value;
			}
		}

		public JsonTextWriter(TextWriter textWriter)
		{
			if (textWriter == null)
			{
				throw new ArgumentNullException("textWriter");
			}
			_writer = textWriter;
			_quoteChar = '"';
			_quoteName = true;
			_indentChar = ' ';
			_indentation = 2;
			UpdateCharEscapeFlags();
		}

		public override void Flush()
		{
			_writer.Flush();
		}

		public override void Close()
		{
			base.Close();
			if (base.CloseOutput && _writer != null)
			{
				_writer.Close();
			}
		}

		public override void WriteStartObject()
		{
			InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);
			_writer.Write('{');
		}

		public override void WriteStartArray()
		{
			InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);
			_writer.Write('[');
		}

		public override void WriteStartConstructor(string name)
		{
			InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);
			_writer.Write("new ");
			_writer.Write(name);
			_writer.Write('(');
		}

		protected override void WriteEnd(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
				_writer.Write('}');
				break;
			case JsonToken.EndArray:
				_writer.Write(']');
				break;
			case JsonToken.EndConstructor:
				_writer.Write(')');
				break;
			default:
				throw JsonWriterException.Create(this, "Invalid JsonToken: " + token, null);
			}
		}

		public override void WritePropertyName(string name)
		{
			InternalWritePropertyName(name);
			WriteEscapedString(name, _quoteName);
			_writer.Write(':');
		}

		public override void WritePropertyName(string name, bool escape)
		{
			InternalWritePropertyName(name);
			if (escape)
			{
				WriteEscapedString(name, _quoteName);
			}
			else
			{
				if (_quoteName)
				{
					_writer.Write(_quoteChar);
				}
				_writer.Write(name);
				if (_quoteName)
				{
					_writer.Write(_quoteChar);
				}
			}
			_writer.Write(':');
		}

		internal override void OnStringEscapeHandlingChanged()
		{
			UpdateCharEscapeFlags();
		}

		private void UpdateCharEscapeFlags()
		{
			_charEscapeFlags = JavaScriptUtils.GetCharEscapeFlags(base.StringEscapeHandling, _quoteChar);
		}

		protected override void WriteIndent()
		{
			_writer.WriteLine();
			int num = base.Top * _indentation;
			if (num > 0)
			{
				if (_indentChars == null)
				{
					_indentChars = new string(_indentChar, 10).ToCharArray();
				}
				while (num > 0)
				{
					int num2 = Math.Min(num, 10);
					_writer.Write(_indentChars, 0, num2);
					num -= num2;
				}
			}
		}

		protected override void WriteValueDelimiter()
		{
			_writer.Write(',');
		}

		protected override void WriteIndentSpace()
		{
			_writer.Write(' ');
		}

		private void WriteValueInternal(string value, JsonToken token)
		{
			_writer.Write(value);
		}

		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				InternalWriteValue(JsonToken.Integer);
				WriteValueInternal(((BigInteger)value).ToString(CultureInfo.InvariantCulture), JsonToken.String);
			}
			else
			{
				base.WriteValue(value);
			}
		}

		public override void WriteNull()
		{
			InternalWriteValue(JsonToken.Null);
			WriteValueInternal(JsonConvert.Null, JsonToken.Null);
		}

		public override void WriteUndefined()
		{
			InternalWriteValue(JsonToken.Undefined);
			WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
		}

		public override void WriteRaw(string json)
		{
			InternalWriteRaw();
			_writer.Write(json);
		}

		public override void WriteValue(string value)
		{
			InternalWriteValue(JsonToken.String);
			if (value == null)
			{
				WriteValueInternal(JsonConvert.Null, JsonToken.Null);
			}
			else
			{
				WriteEscapedString(value, quote: true);
			}
		}

		private void WriteEscapedString(string value, bool quote)
		{
			EnsureWriteBuffer();
			JavaScriptUtils.WriteEscapedJavaScriptString(_writer, value, _quoteChar, quote, _charEscapeFlags, base.StringEscapeHandling, ref _writeBuffer);
		}

		public override void WriteValue(int value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(long value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(float value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, QuoteChar, nullable: false), JsonToken.Float);
		}

		public override void WriteValue(float? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value.Value, base.FloatFormatHandling, QuoteChar, nullable: true), JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value, base.FloatFormatHandling, QuoteChar, nullable: false), JsonToken.Float);
		}

		public override void WriteValue(double? value)
		{
			if (!value.HasValue)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value.Value, base.FloatFormatHandling, QuoteChar, nullable: true), JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			InternalWriteValue(JsonToken.Boolean);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(char value)
		{
			InternalWriteValue(JsonToken.String);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
		}

		public override void WriteValue(byte value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			InternalWriteValue(JsonToken.Integer);
			WriteIntegerValue(value);
		}

		public override void WriteValue(decimal value)
		{
			InternalWriteValue(JsonToken.Float);
			WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
		}

		public override void WriteValue(DateTime value)
		{
			InternalWriteValue(JsonToken.Date);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				EnsureWriteBuffer();
				int start = 0;
				_writeBuffer[start++] = _quoteChar;
				start = DateTimeUtils.WriteDateTimeString(_writeBuffer, start, value, null, value.Kind, base.DateFormatHandling);
				_writeBuffer[start++] = _quoteChar;
				_writer.Write(_writeBuffer, 0, start);
			}
			else
			{
				_writer.Write(_quoteChar);
				_writer.Write(value.ToString(base.DateFormatString, base.Culture));
				_writer.Write(_quoteChar);
			}
		}

		public override void WriteValue(byte[] value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.Bytes);
			_writer.Write(_quoteChar);
			Base64Encoder.Encode(value, 0, value.Length);
			Base64Encoder.Flush();
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			InternalWriteValue(JsonToken.Date);
			if (string.IsNullOrEmpty(base.DateFormatString))
			{
				EnsureWriteBuffer();
				int start = 0;
				_writeBuffer[start++] = _quoteChar;
				start = DateTimeUtils.WriteDateTimeString(_writeBuffer, start, (base.DateFormatHandling == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, value.Offset, DateTimeKind.Local, base.DateFormatHandling);
				_writeBuffer[start++] = _quoteChar;
				_writer.Write(_writeBuffer, 0, start);
			}
			else
			{
				_writer.Write(_quoteChar);
				_writer.Write(value.ToString(base.DateFormatString, base.Culture));
				_writer.Write(_quoteChar);
			}
		}

		public override void WriteValue(Guid value)
		{
			InternalWriteValue(JsonToken.String);
			string text = null;
			text = value.ToString("D", CultureInfo.InvariantCulture);
			_writer.Write(_quoteChar);
			_writer.Write(text);
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(TimeSpan value)
		{
			InternalWriteValue(JsonToken.String);
			string value2 = value.ToString(null, CultureInfo.InvariantCulture);
			_writer.Write(_quoteChar);
			_writer.Write(value2);
			_writer.Write(_quoteChar);
		}

		public override void WriteValue(Uri value)
		{
			if (value == null)
			{
				WriteNull();
				return;
			}
			InternalWriteValue(JsonToken.String);
			WriteEscapedString(value.OriginalString, quote: true);
		}

		public override void WriteComment(string text)
		{
			InternalWriteComment();
			_writer.Write("/*");
			_writer.Write(text);
			_writer.Write("*/");
		}

		public override void WriteWhitespace(string ws)
		{
			InternalWriteWhitespace(ws);
			_writer.Write(ws);
		}

		private void EnsureWriteBuffer()
		{
			if (_writeBuffer == null)
			{
				_writeBuffer = new char[35];
			}
		}

		private void WriteIntegerValue(long value)
		{
			if (value >= 0 && value <= 9)
			{
				_writer.Write((char)(48 + value));
				return;
			}
			ulong uvalue = (ulong)((value < 0) ? (-value) : value);
			if (value < 0)
			{
				_writer.Write('-');
			}
			WriteIntegerValue(uvalue);
		}

		private void WriteIntegerValue(ulong uvalue)
		{
			if (uvalue <= 9)
			{
				_writer.Write((char)(48 + uvalue));
				return;
			}
			EnsureWriteBuffer();
			int num = MathUtils.IntLength(uvalue);
			int num2 = 0;
			do
			{
				_writeBuffer[num - ++num2] = (char)(48 + uvalue % 10);
				uvalue /= 10;
			}
			while (uvalue != 0);
			_writer.Write(_writeBuffer, 0, num2);
		}
	}
	[Serializable]
	public class JsonWriterException : JsonException
	{
		public string Path { get; private set; }

		public JsonWriterException()
		{
		}

		public JsonWriterException(string message)
			: base(message)
		{
		}

		public JsonWriterException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public JsonWriterException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal JsonWriterException(string message, Exception innerException, string path)
			: base(message, innerException)
		{
			Path = path;
		}

		internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
		{
			return Create(writer.ContainerPath, message, ex);
		}

		internal static JsonWriterException Create(string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);
			return new JsonWriterException(message, ex, path);
		}
	}
	[Serializable]
	public class JsonReaderException : JsonException
	{
		public int LineNumber { get; private set; }

		public int LinePosition { get; private set; }

		public string Path { get; private set; }

		public JsonReaderException()
		{
		}

		public JsonReaderException(string message)
			: base(message)
		{
		}

		public JsonReaderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public JsonReaderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal JsonReaderException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			Path = path;
			LineNumber = lineNumber;
			LinePosition = linePosition;
		}

		internal static JsonReaderException Create(JsonReader reader, string message)
		{
			return Create(reader, message, null);
		}

		internal static JsonReaderException Create(JsonReader reader, string message, Exception ex)
		{
			return Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		internal static JsonReaderException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			int lineNumber;
			int linePosition;
			if (lineInfo != null && lineInfo.HasLineInfo())
			{
				lineNumber = lineInfo.LineNumber;
				linePosition = lineInfo.LinePosition;
			}
			else
			{
				lineNumber = 0;
				linePosition = 0;
			}
			return new JsonReaderException(message, ex, path, lineNumber, linePosition);
		}
	}
	public class JsonConverterCollection : Collection<JsonConverter>
	{
	}
	public static class JsonConvert
	{
		public static readonly string True = "true";

		public static readonly string False = "false";

		public static readonly string Null = "null";

		public static readonly string Undefined = "undefined";

		public static readonly string PositiveInfinity = "Infinity";

		public static readonly string NegativeInfinity = "-Infinity";

		public static readonly string NaN = "NaN";

		public static Func<JsonSerializerSettings> DefaultSettings { get; set; }

		public static string ToString(DateTime value)
		{
			return ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
		}

		public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
		{
			DateTime value2 = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);
			using StringWriter stringWriter = StringUtils.CreateStringWriter(64);
			stringWriter.Write('"');
			DateTimeUtils.WriteDateTimeString(stringWriter, value2, format, null, CultureInfo.InvariantCulture);
			stringWriter.Write('"');
			return stringWriter.ToString();
		}

		public static string ToString(DateTimeOffset value)
		{
			return ToString(value, DateFormatHandling.IsoDateFormat);
		}

		public static string ToString(DateTimeOffset value, DateFormatHandling format)
		{
			using StringWriter stringWriter = StringUtils.CreateStringWriter(64);
			stringWriter.Write('"');
			DateTimeUtils.WriteDateTimeOffsetString(stringWriter, value, format, null, CultureInfo.InvariantCulture);
			stringWriter.Write('"');
			return stringWriter.ToString();
		}

		public static string ToString(bool value)
		{
			if (!value)
			{
				return False;
			}
			return True;
		}

		public static string ToString(char value)
		{
			return ToString(char.ToString(value));
		}

		public static string ToString(Enum value)
		{
			return value.ToString("D");
		}

		public static string ToString(int value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(short value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(ushort value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(uint value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(long value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		private static string ToStringInternal(BigInteger value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(ulong value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(float value)
		{
			return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || (!double.IsInfinity(value) && !double.IsNaN(value)))
			{
				return text;
			}
			if (floatFormatHandling == FloatFormatHandling.DefaultValue)
			{
				if (nullable)
				{
					return Null;
				}
				return "0.0";
			}
			return quoteChar + text + quoteChar;
		}

		public static string ToString(double value)
		{
			return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
		}

		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || text.IndexOf('.') != -1 || text.IndexOf('E') != -1 || text.IndexOf('e') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		private static string EnsureDecimalPlace(string text)
		{
			if (text.IndexOf('.') != -1)
			{
				return text;
			}
			return text + ".0";
		}

		public static string ToString(byte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		[CLSCompliant(false)]
		public static string ToString(sbyte value)
		{
			return value.ToString(null, CultureInfo.InvariantCulture);
		}

		public static string ToString(decimal value)
		{
			return EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		public static string ToString(Guid value)
		{
			return ToString(value, '"');
		}

		internal static string ToString(Guid value, char quoteChar)
		{
			string text = value.ToString("D", CultureInfo.InvariantCulture);
			string text2 = quoteChar.ToString(CultureInfo.InvariantCulture);
			return text2 + text + text2;
		}

		public static string ToString(TimeSpan value)
		{
			return ToString(value, '"');
		}

		internal static string ToString(TimeSpan value, char quoteChar)
		{
			return ToString(value.ToString(), quoteChar);
		}

		public static string ToString(Uri value)
		{
			if (value == null)
			{
				return Null;
			}
			return ToString(value, '"');
		}

		internal static string ToString(Uri value, char quoteChar)
		{
			return ToString(value.OriginalString, quoteChar);
		}

		public static string ToString(string value)
		{
			return ToString(value, '"');
		}

		public static string ToString(string value, char delimiter)
		{
			return ToString(value, delimiter, StringEscapeHandling.Default);
		}

		public static string ToString(string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'')
			{
				throw new ArgumentException("Delimiter must be a single or double quote.", "delimiter");
			}
			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, appendDelimiters: true, stringEscapeHandling);
		}

		public static string ToString(object value)
		{
			if (value == null)
			{
				return Null;
			}
			return ConvertUtils.GetTypeCode(value.GetType()) switch
			{
				PrimitiveTypeCode.String => ToString((string)value), 
				PrimitiveTypeCode.Char => ToString((char)value), 
				PrimitiveTypeCode.Boolean => ToString((bool)value), 
				PrimitiveTypeCode.SByte => ToString((sbyte)value), 
				PrimitiveTypeCode.Int16 => ToString((short)value), 
				PrimitiveTypeCode.UInt16 => ToString((ushort)value), 
				PrimitiveTypeCode.Int32 => ToString((int)value), 
				PrimitiveTypeCode.Byte => ToString((byte)value), 
				PrimitiveTypeCode.UInt32 => ToString((uint)value), 
				PrimitiveTypeCode.Int64 => ToString((long)value), 
				PrimitiveTypeCode.UInt64 => ToString((ulong)value), 
				PrimitiveTypeCode.Single => ToString((float)value), 
				PrimitiveTypeCode.Double => ToString((double)value), 
				PrimitiveTypeCode.DateTime => ToString((DateTime)value), 
				PrimitiveTypeCode.Decimal => ToString((decimal)value), 
				PrimitiveTypeCode.DBNull => Null, 
				PrimitiveTypeCode.DateTimeOffset => ToString((DateTimeOffset)value), 
				PrimitiveTypeCode.Guid => ToString((Guid)value), 
				PrimitiveTypeCode.Uri => ToString((Uri)value), 
				PrimitiveTypeCode.TimeSpan => ToString((TimeSpan)value), 
				PrimitiveTypeCode.BigInteger => ToStringInternal((BigInteger)value), 
				_ => throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType())), 
			};
		}

		public static string SerializeObject(object value)
		{
			return SerializeObject(value, (Type)null, (JsonSerializerSettings)null);
		}

		public static string SerializeObject(object value, Formatting formatting)
		{
			return SerializeObject(value, formatting, (JsonSerializerSettings)null);
		}

		public static string SerializeObject(object value, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return SerializeObject(value, null, settings);
		}

		public static string SerializeObject(object value, Formatting formatting, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return SerializeObject(value, null, formatting, settings);
		}

		public static string SerializeObject(object value, JsonSerializerSettings settings)
		{
			return SerializeObject(value, null, settings);
		}

		public static string SerializeObject(object value, Type type, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			return SerializeObjectInternal(value, type, jsonSerializer);
		}

		public static string SerializeObject(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return SerializeObject(value, null, formatting, settings);
		}

		public static string SerializeObject(object value, Type type, Formatting formatting, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			jsonSerializer.Formatting = formatting;
			return SerializeObjectInternal(value, type, jsonSerializer);
		}

		private static string SerializeObjectInternal(object value, Type type, JsonSerializer jsonSerializer)
		{
			StringBuilder sb = new StringBuilder(256);
			StringWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture);
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				jsonTextWriter.Formatting = jsonSerializer.Formatting;
				jsonSerializer.Serialize(jsonTextWriter, value, type);
			}
			return stringWriter.ToString();
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value))")]
		public static Task<string> SerializeObjectAsync(object value)
		{
			return SerializeObjectAsync(value, Formatting.None, null);
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting)
		{
			return SerializeObjectAsync(value, formatting, null);
		}

		[Obsolete("SerializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to serialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.SerializeObject(value, formatting, settings))")]
		public static Task<string> SerializeObjectAsync(object value, Formatting formatting, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => SerializeObject(value, formatting, settings));
		}

		public static object DeserializeObject(string value)
		{
			return DeserializeObject(value, (Type)null, (JsonSerializerSettings)null);
		}

		public static object DeserializeObject(string value, JsonSerializerSettings settings)
		{
			return DeserializeObject(value, null, settings);
		}

		public static object DeserializeObject(string value, Type type)
		{
			return DeserializeObject(value, type, (JsonSerializerSettings)null);
		}

		public static T DeserializeObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value, (JsonSerializerSettings)null);
		}

		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
		{
			return DeserializeObject<T>(value);
		}

		public static T DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
		{
			return DeserializeObject<T>(value, settings);
		}

		public static T DeserializeObject<T>(string value, params JsonConverter[] converters)
		{
			return (T)DeserializeObject(value, typeof(T), converters);
		}

		public static T DeserializeObject<T>(string value, JsonSerializerSettings settings)
		{
			return (T)DeserializeObject(value, typeof(T), settings);
		}

		public static object DeserializeObject(string value, Type type, params JsonConverter[] converters)
		{
			object obj;
			if (converters == null || converters.Length <= 0)
			{
				obj = null;
			}
			else
			{
				JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
				jsonSerializerSettings.Converters = converters;
				obj = jsonSerializerSettings;
			}
			JsonSerializerSettings settings = (JsonSerializerSettings)obj;
			return DeserializeObject(value, type, settings);
		}

		public static object DeserializeObject(string value, Type type, JsonSerializerSettings settings)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			if (!jsonSerializer.IsCheckAdditionalContentSet())
			{
				jsonSerializer.CheckAdditionalContent = true;
			}
			using JsonTextReader reader = new JsonTextReader(new StringReader(value));
			return jsonSerializer.Deserialize(reader, type);
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value))")]
		public static Task<T> DeserializeObjectAsync<T>(string value)
		{
			return DeserializeObjectAsync<T>(value, null);
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value, settings))")]
		public static Task<T> DeserializeObjectAsync<T>(string value, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => DeserializeObject<T>(value, settings));
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value))")]
		public static Task<object> DeserializeObjectAsync(string value)
		{
			return DeserializeObjectAsync(value, null, null);
		}

		[Obsolete("DeserializeObjectAsync is obsolete. Use the Task.Factory.StartNew method to deserialize JSON asynchronously: Task.Factory.StartNew(() => JsonConvert.DeserializeObject(value, type, settings))")]
		public static Task<object> DeserializeObjectAsync(string value, Type type, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(() => DeserializeObject(value, type, settings));
		}

		public static void PopulateObject(string value, object target)
		{
			PopulateObject(value, target, null);
		}

		public static void PopulateObject(string value, object target, JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
			using JsonReader jsonReader = new JsonTextReader(new StringReader(value));
			jsonSerializer.Populate(jsonReader, target);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
			}
		}

		[Obsolete("PopulateObjectAsync is obsolete. Use the Task.Factory.StartNew method to populate an object with JSON values asynchronously: Task.Factory.StartNew(() => JsonConvert.PopulateObject(value, target, settings))")]
		public static Task PopulateObjectAsync(string value, object target, JsonSerializerSettings settings)
		{
			return Task.Factory.StartNew(delegate
			{
				PopulateObject(value, target, settings);
			});
		}

		public static string SerializeXmlNode(XmlNode node)
		{
			return SerializeXmlNode(node, Formatting.None);
		}

		public static string SerializeXmlNode(XmlNode node, Formatting formatting)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			return SerializeObject(node, formatting, xmlNodeConverter);
		}

		public static string SerializeXmlNode(XmlNode node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.OmitRootObject = omitRootObject;
			XmlNodeConverter xmlNodeConverter2 = xmlNodeConverter;
			return SerializeObject(node, formatting, xmlNodeConverter2);
		}

		public static XmlDocument DeserializeXmlNode(string value)
		{
			return DeserializeXmlNode(value, null);
		}

		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName)
		{
			return DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute: false);
		}

		public static XmlDocument DeserializeXmlNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XmlDocument)DeserializeObject(value, typeof(XmlDocument), xmlNodeConverter);
		}

		public static string SerializeXNode(XObject node)
		{
			return SerializeXNode(node, Formatting.None);
		}

		public static string SerializeXNode(XObject node, Formatting formatting)
		{
			return SerializeXNode(node, formatting, omitRootObject: false);
		}

		public static string SerializeXNode(XObject node, Formatting formatting, bool omitRootObject)
		{
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.OmitRootObject = omitRootObject;
			XmlNodeConverter xmlNodeConverter2 = xmlNodeConverter;
			return SerializeObject(node, formatting, xmlNodeConverter2);
		}

		public static XDocument DeserializeXNode(string value)
		{
			return DeserializeXNode(value, null);
		}

		public static XDocument DeserializeXNode(string value, string deserializeRootElementName)
		{
			return DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute: false);
		}

		public static XDocument DeserializeXNode(string value, string deserializeRootElementName, bool writeArrayAttribute)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			XmlNodeConverter xmlNodeConverter = new XmlNodeConverter();
			xmlNodeConverter.DeserializeRootElementName = deserializeRootElementName;
			xmlNodeConverter.WriteArrayAttribute = writeArrayAttribute;
			return (XDocument)DeserializeObject(value, typeof(XDocument), xmlNodeConverter);
		}
	}
	[Serializable]
	public class JsonSerializationException : JsonException
	{
		public JsonSerializationException()
		{
		}

		public JsonSerializationException(string message)
			: base(message)
		{
		}

		public JsonSerializationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public JsonSerializationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal static JsonSerializationException Create(JsonReader reader, string message)
		{
			return Create(reader, message, null);
		}

		internal static JsonSerializationException Create(JsonReader reader, string message, Exception ex)
		{
			return Create(reader as IJsonLineInfo, reader.Path, message, ex);
		}

		internal static JsonSerializationException Create(IJsonLineInfo lineInfo, string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(lineInfo, path, message);
			return new JsonSerializationException(message, ex);
		}
	}
	public class JsonSerializer
	{
		internal TypeNameHandling _typeNameHandling;

		internal FormatterAssemblyStyle _typeNameAssemblyFormat;

		internal PreserveReferencesHandling _preserveReferencesHandling;

		internal ReferenceLoopHandling _referenceLoopHandling;

		internal MissingMemberHandling _missingMemberHandling;

		internal ObjectCreationHandling _objectCreationHandling;

		internal NullValueHandling _nullValueHandling;

		internal DefaultValueHandling _defaultValueHandling;

		internal ConstructorHandling _constructorHandling;

		internal MetadataPropertyHandling _metadataPropertyHandling;

		internal JsonConverterCollection _converters;

		internal IContractResolver _contractResolver;

		internal ITraceWriter _traceWriter;

		internal SerializationBinder _binder;

		internal StreamingContext _context;

		private IReferenceResolver _referenceResolver;

		private Formatting? _formatting;

		private DateFormatHandling? _dateFormatHandling;

		private DateTimeZoneHandling? _dateTimeZoneHandling;

		private DateParseHandling? _dateParseHandling;

		private FloatFormatHandling? _floatFormatHandling;

		private FloatParseHandling? _floatParseHandling;

		private StringEscapeHandling? _stringEscapeHandling;

		private CultureInfo _culture;

		private int? _maxDepth;

		private bool _maxDepthSet;

		private bool? _checkAdditionalContent;

		private string _dateFormatString;

		private bool _dateFormatStringSet;

		public virtual IReferenceResolver ReferenceResolver
		{
			get
			{
				return GetReferenceResolver();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				_referenceResolver = value;
			}
		}

		public virtual SerializationBinder Binder
		{
			get
			{
				return _binder;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				_binder = value;
			}
		}

		public virtual ITraceWriter TraceWriter
		{
			get
			{
				return _traceWriter;
			}
			set
			{
				_traceWriter = value;
			}
		}

		public virtual TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling;
			}
			set
			{
				if (value < TypeNameHandling.None || value > TypeNameHandling.Auto)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_typeNameHandling = value;
			}
		}

		public virtual FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return _typeNameAssemblyFormat;
			}
			set
			{
				if (value < FormatterAssemblyStyle.Simple || value > FormatterAssemblyStyle.Full)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_typeNameAssemblyFormat = value;
			}
		}

		public virtual PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return _preserveReferencesHandling;
			}
			set
			{
				if (value < PreserveReferencesHandling.None || value > PreserveReferencesHandling.All)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_preserveReferencesHandling = value;
			}
		}

		public virtual ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling;
			}
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_referenceLoopHandling = value;
			}
		}

		public virtual MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return _missingMemberHandling;
			}
			set
			{
				if (value < MissingMemberHandling.Ignore || value > MissingMemberHandling.Error)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_missingMemberHandling = value;
			}
		}

		public virtual NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling;
			}
			set
			{
				if (value < NullValueHandling.Include || value > NullValueHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_nullValueHandling = value;
			}
		}

		public virtual DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling;
			}
			set
			{
				if (value < DefaultValueHandling.Include || value > DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_defaultValueHandling = value;
			}
		}

		public virtual ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling;
			}
			set
			{
				if (value < ObjectCreationHandling.Auto || value > ObjectCreationHandling.Replace)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_objectCreationHandling = value;
			}
		}

		public virtual ConstructorHandling ConstructorHandling
		{
			get
			{
				return _constructorHandling;
			}
			set
			{
				if (value < ConstructorHandling.Default || value > ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_constructorHandling = value;
			}
		}

		public virtual MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return _metadataPropertyHandling;
			}
			set
			{
				if (value < MetadataPropertyHandling.Default || value > MetadataPropertyHandling.Ignore)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_metadataPropertyHandling = value;
			}
		}

		public virtual JsonConverterCollection Converters
		{
			get
			{
				if (_converters == null)
				{
					_converters = new JsonConverterCollection();
				}
				return _converters;
			}
		}

		public virtual IContractResolver ContractResolver
		{
			get
			{
				return _contractResolver;
			}
			set
			{
				_contractResolver = value ?? DefaultContractResolver.Instance;
			}
		}

		public virtual StreamingContext Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		public virtual Formatting Formatting
		{
			get
			{
				return _formatting ?? Formatting.None;
			}
			set
			{
				_formatting = value;
			}
		}

		public virtual DateFormatHandling DateFormatHandling
		{
			get
			{
				return _dateFormatHandling ?? DateFormatHandling.IsoDateFormat;
			}
			set
			{
				_dateFormatHandling = value;
			}
		}

		public virtual DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _dateTimeZoneHandling ?? DateTimeZoneHandling.RoundtripKind;
			}
			set
			{
				_dateTimeZoneHandling = value;
			}
		}

		public virtual DateParseHandling DateParseHandling
		{
			get
			{
				return _dateParseHandling ?? DateParseHandling.DateTime;
			}
			set
			{
				_dateParseHandling = value;
			}
		}

		public virtual FloatParseHandling FloatParseHandling
		{
			get
			{
				return _floatParseHandling ?? FloatParseHandling.Double;
			}
			set
			{
				_floatParseHandling = value;
			}
		}

		public virtual FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _floatFormatHandling ?? FloatFormatHandling.String;
			}
			set
			{
				_floatFormatHandling = value;
			}
		}

		public virtual StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _stringEscapeHandling ?? StringEscapeHandling.Default;
			}
			set
			{
				_stringEscapeHandling = value;
			}
		}

		public virtual string DateFormatString
		{
			get
			{
				return _dateFormatString ?? "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
			}
			set
			{
				_dateFormatString = value;
				_dateFormatStringSet = true;
			}
		}

		public virtual CultureInfo Culture
		{
			get
			{
				return _culture ?? JsonSerializerSettings.DefaultCulture;
			}
			set
			{
				_culture = value;
			}
		}

		public virtual int? MaxDepth
		{
			get
			{
				return _maxDepth;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Value must be positive.", "value");
				}
				_maxDepth = value;
				_maxDepthSet = true;
			}
		}

		public virtual bool CheckAdditionalContent
		{
			get
			{
				return _checkAdditionalContent ?? false;
			}
			set
			{
				_checkAdditionalContent = value;
			}
		}

		public virtual event EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

		internal bool IsCheckAdditionalContentSet()
		{
			return _checkAdditionalContent.HasValue;
		}

		public JsonSerializer()
		{
			_referenceLoopHandling = ReferenceLoopHandling.Error;
			_missingMemberHandling = MissingMemberHandling.Ignore;
			_nullValueHandling = NullValueHandling.Include;
			_defaultValueHandling = DefaultValueHandling.Include;
			_objectCreationHandling = ObjectCreationHandling.Auto;
			_preserveReferencesHandling = PreserveReferencesHandling.None;
			_constructorHandling = ConstructorHandling.Default;
			_typeNameHandling = TypeNameHandling.None;
			_metadataPropertyHandling = MetadataPropertyHandling.Default;
			_context = JsonSerializerSettings.DefaultContext;
			_binder = DefaultSerializationBinder.Instance;
			_culture = JsonSerializerSettings.DefaultCulture;
			_contractResolver = DefaultContractResolver.Instance;
		}

		public static JsonSerializer Create()
		{
			return new JsonSerializer();
		}

		public static JsonSerializer Create(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = Create();
			if (settings != null)
			{
				ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		public static JsonSerializer CreateDefault()
		{
			JsonSerializerSettings settings = JsonConvert.DefaultSettings?.Invoke();
			return Create(settings);
		}

		public static JsonSerializer CreateDefault(JsonSerializerSettings settings)
		{
			JsonSerializer jsonSerializer = CreateDefault();
			if (settings != null)
			{
				ApplySerializerSettings(jsonSerializer, settings);
			}
			return jsonSerializer;
		}

		private static void ApplySerializerSettings(JsonSerializer serializer, JsonSerializerSettings settings)
		{
			if (!CollectionUtils.IsNullOrEmpty(settings.Converters))
			{
				for (int i = 0; i < settings.Converters.Count; i++)
				{
					serializer.Converters.Insert(i, settings.Converters[i]);
				}
			}
			if (settings._typeNameHandling.HasValue)
			{
				serializer.TypeNameHandling = settings.TypeNameHandling;
			}
			if (settings._metadataPropertyHandling.HasValue)
			{
				serializer.MetadataPropertyHandling = settings.MetadataPropertyHandling;
			}
			if (settings._typeNameAssemblyFormat.HasValue)
			{
				serializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
			}
			if (settings._preserveReferencesHandling.HasValue)
			{
				serializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
			}
			if (settings._referenceLoopHandling.HasValue)
			{
				serializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
			}
			if (settings._missingMemberHandling.HasValue)
			{
				serializer.MissingMemberHandling = settings.MissingMemberHandling;
			}
			if (settings._objectCreationHandling.HasValue)
			{
				serializer.ObjectCreationHandling = settings.ObjectCreationHandling;
			}
			if (settings._nullValueHandling.HasValue)
			{
				serializer.NullValueHandling = settings.NullValueHandling;
			}
			if (settings._defaultValueHandling.HasValue)
			{
				serializer.DefaultValueHandling = settings.DefaultValueHandling;
			}
			if (settings._constructorHandling.HasValue)
			{
				serializer.ConstructorHandling = settings.ConstructorHandling;
			}
			if (settings._context.HasValue)
			{
				serializer.Context = settings.Context;
			}
			if (settings._checkAdditionalContent.HasValue)
			{
				serializer._checkAdditionalContent = settings._checkAdditionalContent;
			}
			if (settings.Error != null)
			{
				serializer.Error += settings.Error;
			}
			if (settings.ContractResolver != null)
			{
				serializer.ContractResolver = settings.ContractResolver;
			}
			if (settings.ReferenceResolver != null)
			{
				serializer.ReferenceResolver = settings.ReferenceResolver;
			}
			if (settings.TraceWriter != null)
			{
				serializer.TraceWriter = settings.TraceWriter;
			}
			if (settings.Binder != null)
			{
				serializer.Binder = settings.Binder;
			}
			if (settings._formatting.HasValue)
			{
				serializer._formatting = settings._formatting;
			}
			if (settings._dateFormatHandling.HasValue)
			{
				serializer._dateFormatHandling = settings._dateFormatHandling;
			}
			if (settings._dateTimeZoneHandling.HasValue)
			{
				serializer._dateTimeZoneHandling = settings._dateTimeZoneHandling;
			}
			if (settings._dateParseHandling.HasValue)
			{
				serializer._dateParseHandling = settings._dateParseHandling;
			}
			if (settings._dateFormatStringSet)
			{
				serializer._dateFormatString = settings._dateFormatString;
				serializer._dateFormatStringSet = settings._dateFormatStringSet;
			}
			if (settings._floatFormatHandling.HasValue)
			{
				serializer._floatFormatHandling = settings._floatFormatHandling;
			}
			if (settings._floatParseHandling.HasValue)
			{
				serializer._floatParseHandling = settings._floatParseHandling;
			}
			if (settings._stringEscapeHandling.HasValue)
			{
				serializer._stringEscapeHandling = settings._stringEscapeHandling;
			}
			if (settings._culture != null)
			{
				serializer._culture = settings._culture;
			}
			if (settings._maxDepthSet)
			{
				serializer._maxDepth = settings._maxDepth;
				serializer._maxDepthSet = settings._maxDepthSet;
			}
		}

		public void Populate(TextReader reader, object target)
		{
			Populate(new JsonTextReader(reader), target);
		}

		public void Populate(JsonReader reader, object target)
		{
			PopulateInternal(reader, target);
		}

		internal virtual void PopulateInternal(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(target, "target");
			SetupReader(reader, out var previousCulture, out var previousDateTimeZoneHandling, out var previousDateParseHandling, out var previousFloatParseHandling, out var previousMaxDepth, out var previousDateFormatString);
			TraceJsonReader traceJsonReader = ((TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonReader(reader) : null);
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			jsonSerializerInternalReader.Populate(traceJsonReader ?? reader, target);
			if (traceJsonReader != null)
			{
				TraceWriter.Trace(TraceLevel.Verbose, "Deserialized JSON: " + Environment.NewLine + traceJsonReader.GetJson(), null);
			}
			ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
		}

		public object Deserialize(JsonReader reader)
		{
			return Deserialize(reader, null);
		}

		public object Deserialize(TextReader reader, Type objectType)
		{
			return Deserialize(new JsonTextReader(reader), objectType);
		}

		public T Deserialize<T>(JsonReader reader)
		{
			return (T)Deserialize(reader, typeof(T));
		}

		public object Deserialize(JsonReader reader, Type objectType)
		{
			return DeserializeInternal(reader, objectType);
		}

		internal virtual object DeserializeInternal(JsonReader reader, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			SetupReader(reader, out var previousCulture, out var previousDateTimeZoneHandling, out var previousDateParseHandling, out var previousFloatParseHandling, out var previousMaxDepth, out var previousDateFormatString);
			TraceJsonReader traceJsonReader = ((TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonReader(reader) : null);
			JsonSerializerInternalReader jsonSerializerInternalReader = new JsonSerializerInternalReader(this);
			object result = jsonSerializerInternalReader.Deserialize(traceJsonReader ?? reader, objectType, CheckAdditionalContent);
			if (traceJsonReader != null)
			{
				TraceWriter.Trace(TraceLevel.Verbose, "Deserialized JSON: " + Environment.NewLine + traceJsonReader.GetJson(), null);
			}
			ResetReader(reader, previousCulture, previousDateTimeZoneHandling, previousDateParseHandling, previousFloatParseHandling, previousMaxDepth, previousDateFormatString);
			return result;
		}

		private void SetupReader(JsonReader reader, out CultureInfo previousCulture, out DateTimeZoneHandling? previousDateTimeZoneHandling, out DateParseHandling? previousDateParseHandling, out FloatParseHandling? previousFloatParseHandling, out int? previousMaxDepth, out string previousDateFormatString)
		{
			if (_culture != null && !_culture.Equals(reader.Culture))
			{
				previousCulture = reader.Culture;
				reader.Culture = _culture;
			}
			else
			{
				previousCulture = null;
			}
			if (_dateTimeZoneHandling.HasValue && reader.DateTimeZoneHandling != _dateTimeZoneHandling)
			{
				previousDateTimeZoneHandling = reader.DateTimeZoneHandling;
				reader.DateTimeZoneHandling = _dateTimeZoneHandling.Value;
			}
			else
			{
				previousDateTimeZoneHandling = null;
			}
			if (_dateParseHandling.HasValue && reader.DateParseHandling != _dateParseHandling)
			{
				previousDateParseHandling = reader.DateParseHandling;
				reader.DateParseHandling = _dateParseHandling.Value;
			}
			else
			{
				previousDateParseHandling = null;
			}
			if (_floatParseHandling.HasValue && reader.FloatParseHandling != _floatParseHandling)
			{
				previousFloatParseHandling = reader.FloatParseHandling;
				reader.FloatParseHandling = _floatParseHandling.Value;
			}
			else
			{
				previousFloatParseHandling = null;
			}
			if (_maxDepthSet && reader.MaxDepth != _maxDepth)
			{
				previousMaxDepth = reader.MaxDepth;
				reader.MaxDepth = _maxDepth;
			}
			else
			{
				previousMaxDepth = null;
			}
			if (_dateFormatStringSet && reader.DateFormatString != _dateFormatString)
			{
				previousDateFormatString = reader.DateFormatString;
				reader.DateFormatString = _dateFormatString;
			}
			else
			{
				previousDateFormatString = null;
			}
			if (reader is JsonTextReader jsonTextReader && _contractResolver is DefaultContractResolver defaultContractResolver)
			{
				jsonTextReader.NameTable = defaultContractResolver.GetState().NameTable;
			}
		}

		private void ResetReader(JsonReader reader, CultureInfo previousCulture, DateTimeZoneHandling? previousDateTimeZoneHandling, DateParseHandling? previousDateParseHandling, FloatParseHandling? previousFloatParseHandling, int? previousMaxDepth, string previousDateFormatString)
		{
			if (previousCulture != null)
			{
				reader.Culture = previousCulture;
			}
			if (previousDateTimeZoneHandling.HasValue)
			{
				reader.DateTimeZoneHandling = previousDateTimeZoneHandling.Value;
			}
			if (previousDateParseHandling.HasValue)
			{
				reader.DateParseHandling = previousDateParseHandling.Value;
			}
			if (previousFloatParseHandling.HasValue)
			{
				reader.FloatParseHandling = previousFloatParseHandling.Value;
			}
			if (_maxDepthSet)
			{
				reader.MaxDepth = previousMaxDepth;
			}
			if (_dateFormatStringSet)
			{
				reader.DateFormatString = previousDateFormatString;
			}
			if (reader is JsonTextReader jsonTextReader)
			{
				jsonTextReader.NameTable = null;
			}
		}

		public void Serialize(TextWriter textWriter, object value)
		{
			Serialize(new JsonTextWriter(textWriter), value);
		}

		public void Serialize(JsonWriter jsonWriter, object value, Type objectType)
		{
			SerializeInternal(jsonWriter, value, objectType);
		}

		public void Serialize(TextWriter textWriter, object value, Type objectType)
		{
			Serialize(new JsonTextWriter(textWriter), value, objectType);
		}

		public void Serialize(JsonWriter jsonWriter, object value)
		{
			SerializeInternal(jsonWriter, value, null);
		}

		internal virtual void SerializeInternal(JsonWriter jsonWriter, object value, Type objectType)
		{
			ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			Formatting? formatting = null;
			if (_formatting.HasValue && jsonWriter.Formatting != _formatting)
			{
				formatting = jsonWriter.Formatting;
				jsonWriter.Formatting = _formatting.Value;
			}
			DateFormatHandling? dateFormatHandling = null;
			if (_dateFormatHandling.HasValue && jsonWriter.DateFormatHandling != _dateFormatHandling)
			{
				dateFormatHandling = jsonWriter.DateFormatHandling;
				jsonWriter.DateFormatHandling = _dateFormatHandling.Value;
			}
			DateTimeZoneHandling? dateTimeZoneHandling = null;
			if (_dateTimeZoneHandling.HasValue && jsonWriter.DateTimeZoneHandling != _dateTimeZoneHandling)
			{
				dateTimeZoneHandling = jsonWriter.DateTimeZoneHandling;
				jsonWriter.DateTimeZoneHandling = _dateTimeZoneHandling.Value;
			}
			FloatFormatHandling? floatFormatHandling = null;
			if (_floatFormatHandling.HasValue && jsonWriter.FloatFormatHandling != _floatFormatHandling)
			{
				floatFormatHandling = jsonWriter.FloatFormatHandling;
				jsonWriter.FloatFormatHandling = _floatFormatHandling.Value;
			}
			StringEscapeHandling? stringEscapeHandling = null;
			if (_stringEscapeHandling.HasValue && jsonWriter.StringEscapeHandling != _stringEscapeHandling)
			{
				stringEscapeHandling = jsonWriter.StringEscapeHandling;
				jsonWriter.StringEscapeHandling = _stringEscapeHandling.Value;
			}
			CultureInfo cultureInfo = null;
			if (_culture != null && !_culture.Equals(jsonWriter.Culture))
			{
				cultureInfo = jsonWriter.Culture;
				jsonWriter.Culture = _culture;
			}
			string dateFormatString = null;
			if (_dateFormatStringSet && jsonWriter.DateFormatString != _dateFormatString)
			{
				dateFormatString = jsonWriter.DateFormatString;
				jsonWriter.DateFormatString = _dateFormatString;
			}
			TraceJsonWriter traceJsonWriter = ((TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose) ? new TraceJsonWriter(jsonWriter) : null);
			JsonSerializerInternalWriter jsonSerializerInternalWriter = new JsonSerializerInternalWriter(this);
			jsonSerializerInternalWriter.Serialize(traceJsonWriter ?? jsonWriter, value, objectType);
			if (traceJsonWriter != null)
			{
				TraceWriter.Trace(TraceLevel.Verbose, "Serialized JSON: " + Environment.NewLine + traceJsonWriter.GetJson(), null);
			}
			if (formatting.HasValue)
			{
				jsonWriter.Formatting = formatting.Value;
			}
			if (dateFormatHandling.HasValue)
			{
				jsonWriter.DateFormatHandling = dateFormatHandling.Value;
			}
			if (dateTimeZoneHandling.HasValue)
			{
				jsonWriter.DateTimeZoneHandling = dateTimeZoneHandling.Value;
			}
			if (floatFormatHandling.HasValue)
			{
				jsonWriter.FloatFormatHandling = floatFormatHandling.Value;
			}
			if (stringEscapeHandling.HasValue)
			{
				jsonWriter.StringEscapeHandling = stringEscapeHandling.Value;
			}
			if (_dateFormatStringSet)
			{
				jsonWriter.DateFormatString = dateFormatString;
			}
			if (cultureInfo != null)
			{
				jsonWriter.Culture = cultureInfo;
			}
		}

		internal IReferenceResolver GetReferenceResolver()
		{
			if (_referenceResolver == null)
			{
				_referenceResolver = new DefaultReferenceResolver();
			}
			return _referenceResolver;
		}

		internal JsonConverter GetMatchingConverter(Type type)
		{
			return GetMatchingConverter(_converters, type);
		}

		internal static JsonConverter GetMatchingConverter(IList<JsonConverter> converters, Type objectType)
		{
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		internal void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			this.Error?.Invoke(this, e);
		}
	}
}
namespace Newtonsoft.Json.Linq
{
	public static class Extensions
	{
		public static IJEnumerable<JToken> Ancestors<T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.Ancestors()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> AncestorsAndSelf<T>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.AncestorsAndSelf()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Descendants<T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.Descendants()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> DescendantsAndSelf<T>(this IEnumerable<T> source) where T : JContainer
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T j) => j.DescendantsAndSelf()).AsJEnumerable();
		}

		public static IJEnumerable<JProperty> Properties(this IEnumerable<JObject> source)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((JObject d) => d.Properties()).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source, object key)
		{
			return source.Values<JToken, JToken>(key).AsJEnumerable();
		}

		public static IJEnumerable<JToken> Values(this IEnumerable<JToken> source)
		{
			return source.Values(null);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source, object key)
		{
			return source.Values<JToken, U>(key);
		}

		public static IEnumerable<U> Values<U>(this IEnumerable<JToken> source)
		{
			return source.Values<JToken, U>(null);
		}

		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, "source");
			if (!(value is JToken token))
			{
				throw new ArgumentException("Source value must be a JToken.");
			}
			return token.Convert<JToken, U>();
		}

		internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T token in source)
			{
				if (key == null)
				{
					if (token is JValue)
					{
						yield return ((JValue)(object)token).Convert<JValue, U>();
						continue;
					}
					foreach (JToken t in token.Children())
					{
						yield return t.Convert<JToken, U>();
					}
				}
				else
				{
					JToken value = token[key];
					if (value != null)
					{
						yield return value.Convert<JToken, U>();
					}
				}
			}
		}

		public static IJEnumerable<JToken> Children<T>(this IEnumerable<T> source) where T : JToken
		{
			return source.Children<T, JToken>().AsJEnumerable();
		}

		public static IEnumerable<U> Children<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			return source.SelectMany((T c) => c.Children()).Convert<JToken, U>();
		}

		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			foreach (T token in source)
			{
				yield return token.Convert<JToken, U>();
			}
		}

		internal static U Convert<T, U>(this T token) where T : JToken
		{
			if (token == null)
			{
				return default(U);
			}
			if (token is U && typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable))
			{
				return (U)(object)token;
			}
			if (!(token is JValue jValue))
			{
				throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
			}
			if (jValue.Value is U)
			{
				return (U)jValue.Value;
			}
			Type type = typeof(U);
			if (ReflectionUtils.IsNullableType(type))
			{
				if (jValue.Value == null)
				{
					return default(U);
				}
				type = Nullable.GetUnderlyingType(type);
			}
			return (U)System.Convert.ChangeType(jValue.Value, type, CultureInfo.InvariantCulture);
		}

		public static IJEnumerable<JToken> AsJEnumerable(this IEnumerable<JToken> source)
		{
			return source.AsJEnumerable<JToken>();
		}

		public static IJEnumerable<T> AsJEnumerable<T>(this IEnumerable<T> source) where T : JToken
		{
			if (source == null)
			{
				return null;
			}
			if (source is IJEnumerable<T>)
			{
				return (IJEnumerable<T>)source;
			}
			return new JEnumerable<T>(source);
		}
	}
	public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, ITypedList, IBindingList, IList, ICollection, IEnumerable, INotifyCollectionChanged
	{
		private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
		{
			public static readonly JTokenReferenceEqualityComparer Instance = new JTokenReferenceEqualityComparer();

			public bool Equals(JToken x, JToken y)
			{
				return object.ReferenceEquals(x, y);
			}

			public int GetHashCode(JToken obj)
			{
				return obj?.GetHashCode() ?? 0;
			}
		}

		internal ListChangedEventHandler _listChanged;

		internal AddingNewEventHandler _addingNew;

		internal NotifyCollectionChangedEventHandler _collectionChanged;

		private object _syncRoot;

		private bool _busy;

		protected abstract IList<JToken> ChildrenTokens { get; }

		public override bool HasValues => ChildrenTokens.Count > 0;

		public override JToken First => ChildrenTokens.FirstOrDefault();

		public override JToken Last => ChildrenTokens.LastOrDefault();

		JToken IList<JToken>.this[int index]
		{
			get
			{
				return GetItem(index);
			}
			set
			{
				SetItem(index, value);
			}
		}

		bool ICollection<JToken>.IsReadOnly => false;

		bool IList.IsFixedSize => false;

		bool IList.IsReadOnly => false;

		object IList.this[int index]
		{
			get
			{
				return GetItem(index);
			}
			set
			{
				SetItem(index, EnsureValue(value));
			}
		}

		public int Count => ChildrenTokens.Count;

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		bool IBindingList.AllowEdit => true;

		bool IBindingList.AllowNew => true;

		bool IBindingList.AllowRemove => true;

		bool IBindingList.IsSorted => false;

		ListSortDirection IBindingList.SortDirection => ListSortDirection.Ascending;

		PropertyDescriptor IBindingList.SortProperty => null;

		bool IBindingList.SupportsChangeNotification => true;

		bool IBindingList.SupportsSearching => false;

		bool IBindingList.SupportsSorting => false;

		public event ListChangedEventHandler ListChanged
		{
			add
			{
				_listChanged = (ListChangedEventHandler)Delegate.Combine(_listChanged, value);
			}
			remove
			{
				_listChanged = (ListChangedEventHandler)Delegate.Remove(_listChanged, value);
			}
		}

		public event AddingNewEventHandler AddingNew
		{
			add
			{
				_addingNew = (AddingNewEventHandler)Delegate.Combine(_addingNew, value);
			}
			remove
			{
				_addingNew = (AddingNewEventHandler)Delegate.Remove(_addingNew, value);
			}
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add
			{
				_collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(_collectionChanged, value);
			}
			remove
			{
				_collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(_collectionChanged, value);
			}
		}

		internal JContainer()
		{
		}

		internal JContainer(JContainer other)
			: this()
		{
			ValidationUtils.ArgumentNotNull(other, "c");
			int num = 0;
			foreach (JToken item in (IEnumerable<JToken>)other)
			{
				AddInternal(num, item, skipParentCheck: false);
				num++;
			}
		}

		internal void CheckReentrancy()
		{
			if (_busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
		}

		internal virtual IList<JToken> CreateChildrenCollection()
		{
			return new List<JToken>();
		}

		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			_addingNew?.Invoke(this, e);
		}

		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			ListChangedEventHandler listChanged = _listChanged;
			if (listChanged != null)
			{
				_busy = true;
				try
				{
					listChanged(this, e);
				}
				finally
				{
					_busy = false;
				}
			}
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler collectionChanged = _collectionChanged;
			if (collectionChanged != null)
			{
				_busy = true;
				try
				{
					collectionChanged(this, e);
				}
				finally
				{
					_busy = false;
				}
			}
		}

		internal bool ContentsEqual(JContainer container)
		{
			if (container == this)
			{
				return true;
			}
			IList<JToken> childrenTokens = ChildrenTokens;
			IList<JToken> childrenTokens2 = container.ChildrenTokens;
			if (childrenTokens.Count != childrenTokens2.Count)
			{
				return false;
			}
			for (int i = 0; i < childrenTokens.Count; i++)
			{
				if (!childrenTokens[i].DeepEquals(childrenTokens2[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(ChildrenTokens);
		}

		public override IEnumerable<T> Values<T>()
		{
			return ChildrenTokens.Convert<JToken, T>();
		}

		public IEnumerable<JToken> Descendants()
		{
			return GetDescendants(self: false);
		}

		public IEnumerable<JToken> DescendantsAndSelf()
		{
			return GetDescendants(self: true);
		}

		internal IEnumerable<JToken> GetDescendants(bool self)
		{
			if (self)
			{
				yield return this;
			}
			foreach (JToken o in ChildrenTokens)
			{
				yield return o;
				if (!(o is JContainer c))
				{
					continue;
				}
				foreach (JToken item in c.Descendants())
				{
					yield return item;
				}
			}
		}

		internal bool IsMultiContent(object content)
		{
			if (content is IEnumerable && !(content is string) && !(content is JToken))
			{
				return !(content is byte[]);
			}
			return false;
		}

		internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
		{
			if (item == null)
			{
				return JValue.CreateNull();
			}
			if (skipParentCheck)
			{
				return item;
			}
			if (item.Parent != null || item == this || (item.HasValues && base.Root == item))
			{
				item = item.CloneToken();
			}
			return item;
		}

		internal int IndexOfItem(JToken item)
		{
			return ChildrenTokens.IndexOf(item, JTokenReferenceEqualityComparer.Instance);
		}

		internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (index > ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			CheckReentrancy();
			item = EnsureParentToken(item, skipParentCheck);
			JToken jToken = ((index == 0) ? null : ChildrenTokens[index - 1]);
			JToken jToken2 = ((index == ChildrenTokens.Count) ? null : ChildrenTokens[index]);
			ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jToken;
			if (jToken != null)
			{
				jToken.Next = item;
			}
			item.Next = jToken2;
			if (jToken2 != null)
			{
				jToken2.Previous = item;
			}
			ChildrenTokens.Insert(index, item);
			if (_listChanged != null)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
			if (_collectionChanged != null)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
		}

		internal virtual void RemoveItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			CheckReentrancy();
			JToken jToken = ChildrenTokens[index];
			JToken jToken2 = ((index == 0) ? null : ChildrenTokens[index - 1]);
			JToken jToken3 = ((index == ChildrenTokens.Count - 1) ? null : ChildrenTokens[index + 1]);
			if (jToken2 != null)
			{
				jToken2.Next = jToken3;
			}
			if (jToken3 != null)
			{
				jToken3.Previous = jToken2;
			}
			jToken.Parent = null;
			jToken.Previous = null;
			jToken.Next = null;
			ChildrenTokens.RemoveAt(index);
			if (_listChanged != null)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			}
			if (_collectionChanged != null)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, jToken, index));
			}
		}

		internal virtual bool RemoveItem(JToken item)
		{
			int num = IndexOfItem(item);
			if (num >= 0)
			{
				RemoveItemAt(num);
				return true;
			}
			return false;
		}

		internal virtual JToken GetItem(int index)
		{
			return ChildrenTokens[index];
		}

		internal virtual void SetItem(int index, JToken item)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= ChildrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jToken = ChildrenTokens[index];
			if (!IsTokenUnchanged(jToken, item))
			{
				CheckReentrancy();
				item = EnsureParentToken(item, skipParentCheck: false);
				ValidateToken(item, jToken);
				JToken jToken2 = ((index == 0) ? null : ChildrenTokens[index - 1]);
				JToken jToken3 = ((index == ChildrenTokens.Count - 1) ? null : ChildrenTokens[index + 1]);
				item.Parent = this;
				item.Previous = jToken2;
				if (jToken2 != null)
				{
					jToken2.Next = item;
				}
				item.Next = jToken3;
				if (jToken3 != null)
				{
					jToken3.Previous = item;
				}
				ChildrenTokens[index] = item;
				jToken.Parent = null;
				jToken.Previous = null;
				jToken.Next = null;
				if (_listChanged != null)
				{
					OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
				}
				if (_collectionChanged != null)
				{
					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, jToken, index));
				}
			}
		}

		internal virtual void ClearItems()
		{
			CheckReentrancy();
			foreach (JToken childrenToken in ChildrenTokens)
			{
				childrenToken.Parent = null;
				childrenToken.Previous = null;
				childrenToken.Next = null;
			}
			ChildrenTokens.Clear();
			if (_listChanged != null)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
			if (_collectionChanged != null)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing != null && existing.Parent == this)
			{
				int index = IndexOfItem(existing);
				SetItem(index, replacement);
			}
		}

		internal virtual bool ContainsItem(JToken item)
		{
			return IndexOfItem(item) != -1;
		}

		internal virtual void CopyItemsTo(Array array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken childrenToken in ChildrenTokens)
			{
				array.SetValue(childrenToken, arrayIndex + num);
				num++;
			}
		}

		internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
		{
			if (currentValue is JValue jValue)
			{
				if (jValue.Type == JTokenType.Null && newValue == null)
				{
					return true;
				}
				return jValue.Equals(newValue);
			}
			return false;
		}

		internal virtual void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), GetType()));
			}
		}

		public virtual void Add(object content)
		{
			AddInternal(ChildrenTokens.Count, content, skipParentCheck: false);
		}

		internal void AddAndSkipParentCheck(JToken token)
		{
			AddInternal(ChildrenTokens.Count, token, skipParentCheck: true);
		}

		public void AddFirst(object content)
		{
			AddInternal(0, content, skipParentCheck: false);
		}

		internal void AddInternal(int index, object content, bool skipParentCheck)
		{
			if (IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				{
					foreach (object item2 in enumerable)
					{
						AddInternal(num, item2, skipParentCheck);
						num++;
					}
					return;
				}
			}
			JToken item = CreateFromContent(content);
			InsertItem(index, item, skipParentCheck);
		}

		internal static JToken CreateFromContent(object content)
		{
			if (content is JToken)
			{
				return (JToken)content;
			}
			return new JValue(content);
		}

		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		public void ReplaceAll(object content)
		{
			ClearItems();
			Add(content);
		}

		public void RemoveAll()
		{
			ClearItems();
		}

		internal abstract void MergeItem(object content, JsonMergeSettings settings);

		public void Merge(object content)
		{
			MergeItem(content, new JsonMergeSettings());
		}

		public void Merge(object content, JsonMergeSettings settings)
		{
			MergeItem(content, settings);
		}

		internal void ReadTokenFrom(JsonReader reader)
		{
			int depth = reader.Depth;
			if (!reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, GetType().Name));
			}
			ReadContentFrom(reader);
			int depth2 = reader.Depth;
			if (depth2 > depth)
			{
				throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, GetType().Name));
			}
		}

		internal void ReadContentFrom(JsonReader r)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo lineInfo = r as IJsonLineInfo;
			JContainer jContainer = this;
			do
			{
				if (jContainer is JProperty && ((JProperty)jContainer).Value != null)
				{
					if (jContainer == this)
					{
						break;
					}
					jContainer = jContainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.StartArray:
				{
					JArray jArray = new JArray();
					jArray.SetLineInfo(lineInfo);
					jContainer.Add(jArray);
					jContainer = jArray;
					break;
				}
				case JsonToken.EndArray:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case JsonToken.StartObject:
				{
					JObject jObject2 = new JObject();
					jObject2.SetLineInfo(lineInfo);
					jContainer.Add(jObject2);
					jContainer = jObject2;
					break;
				}
				case JsonToken.EndObject:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case JsonToken.StartConstructor:
				{
					JConstructor jConstructor = new JConstructor(r.Value.ToString());
					jConstructor.SetLineInfo(lineInfo);
					jContainer.Add(jConstructor);
					jContainer = jConstructor;
					break;
				}
				case JsonToken.EndConstructor:
					if (jContainer == this)
					{
						return;
					}
					jContainer = jContainer.Parent;
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jValue = new JValue(r.Value);
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case JsonToken.Comment:
				{
					JValue jValue = JValue.CreateComment(r.Value.ToString());
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case JsonToken.Null:
				{
					JValue jValue = JValue.CreateNull();
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case JsonToken.Undefined:
				{
					JValue jValue = JValue.CreateUndefined();
					jValue.SetLineInfo(lineInfo);
					jContainer.Add(jValue);
					break;
				}
				case JsonToken.PropertyName:
				{
					string name = r.Value.ToString();
					JProperty jProperty = new JProperty(name);
					jProperty.SetLineInfo(lineInfo);
					JObject jObject = (JObject)jContainer;
					JProperty jProperty2 = jObject.Property(name);
					if (jProperty2 == null)
					{
						jContainer.Add(jProperty);
					}
					else
					{
						jProperty2.Replace(jProperty);
					}
					jContainer = jProperty;
					break;
				}
				default:
					throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, r.TokenType));
				case JsonToken.None:
					break;
				}
			}
			while (r.Read());
		}

		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken childrenToken in ChildrenTokens)
			{
				num ^= childrenToken.GetDeepHashCode();
			}
			return num;
		}

		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			if (First is ICustomTypeDescriptor customTypeDescriptor)
			{
				return customTypeDescriptor.GetProperties();
			}
			return null;
		}

		int IList<JToken>.IndexOf(JToken item)
		{
			return IndexOfItem(item);
		}

		void IList<JToken>.Insert(int index, JToken item)
		{
			InsertItem(index, item, skipParentCheck: false);
		}

		void IList<JToken>.RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		void ICollection<JToken>.Add(JToken item)
		{
			Add(item);
		}

		void ICollection<JToken>.Clear()
		{
			ClearItems();
		}

		bool ICollection<JToken>.Contains(JToken item)
		{
			return ContainsItem(item);
		}

		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			CopyItemsTo(array, arrayIndex);
		}

		bool ICollection<JToken>.Remove(JToken item)
		{
			return RemoveItem(item);
		}

		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			if (value is JToken)
			{
				return (JToken)value;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}

		int IList.Add(object value)
		{
			Add(EnsureValue(value));
			return Count - 1;
		}

		void IList.Clear()
		{
			ClearItems();
		}

		bool IList.Contains(object value)
		{
			return ContainsItem(EnsureValue(value));
		}

		int IList.IndexOf(object value)
		{
			return IndexOfItem(EnsureValue(value));
		}

		void IList.Insert(int index, object value)
		{
			InsertItem(index, EnsureValue(value), skipParentCheck: false);
		}

		void IList.Remove(object value)
		{
			RemoveItem(EnsureValue(value));
		}

		void IList.RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			CopyItemsTo(array, index);
		}

		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		object IBindingList.AddNew()
		{
			AddingNewEventArgs addingNewEventArgs = new AddingNewEventArgs();
			OnAddingNew(addingNewEventArgs);
			if (addingNewEventArgs.NewObject == null)
			{
				throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, GetType()));
			}
			if (!(addingNewEventArgs.NewObject is JToken))
			{
				throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JToken)));
			}
			JToken jToken = (JToken)addingNewEventArgs.NewObject;
			Add(jToken);
			return jToken;
		}

		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		internal static void MergeEnumerableContent(JContainer target, IEnumerable content, JsonMergeSettings settings)
		{
			switch (settings.MergeArrayHandling)
			{
			case MergeArrayHandling.Concat:
			{
				foreach (JToken item in content)
				{
					target.Add(item);
				}
				break;
			}
			case MergeArrayHandling.Union:
			{
				HashSet<JToken> hashSet = new HashSet<JToken>(target, JToken.EqualityComparer);
				{
					foreach (JToken item2 in content)
					{
						if (hashSet.Add(item2))
						{
							target.Add(item2);
						}
					}
					break;
				}
			}
			case MergeArrayHandling.Replace:
				target.ClearItems();
				{
					foreach (JToken item3 in content)
					{
						target.Add(item3);
					}
					break;
				}
			case MergeArrayHandling.Merge:
			{
				int num = 0;
				{
					foreach (object item4 in content)
					{
						if (num < target.Count)
						{
							JToken jToken = target[num];
							if (jToken is JContainer jContainer)
							{
								jContainer.Merge(item4, settings);
							}
							else if (item4 != null)
							{
								JToken jToken2 = CreateFromContent(item4);
								if (jToken2.Type != JTokenType.Null)
								{
									target[num] = jToken2;
								}
							}
						}
						else
						{
							target.Add(item4);
						}
						num++;
					}
					break;
				}
			}
			default:
				throw new ArgumentOutOfRangeException("settings", "Unexpected merge array handling when merging JSON.");
			}
		}
	}
	public class JConstructor : JContainer
	{
		private string _name;

		private readonly List<JToken> _values = new List<JToken>();

		protected override IList<JToken> ChildrenTokens => _values;

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public override JTokenType Type => JTokenType.Constructor;

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				SetItem((int)key, value);
			}
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			if (content is JConstructor jConstructor)
			{
				if (jConstructor.Name != null)
				{
					Name = jConstructor.Name;
				}
				JContainer.MergeEnumerableContent(this, jConstructor, settings);
			}
		}

		public JConstructor()
		{
		}

		public JConstructor(JConstructor other)
			: base(other)
		{
			_name = other.Name;
		}

		public JConstructor(string name, params object[] content)
			: this(name, (object)content)
		{
		}

		public JConstructor(string name, object content)
			: this(name)
		{
			Add(content);
		}

		public JConstructor(string name)
		{
			ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
			_name = name;
		}

		internal override bool DeepEquals(JToken node)
		{
			if (node is JConstructor jConstructor && _name == jConstructor.Name)
			{
				return ContentsEqual(jConstructor);
			}
			return false;
		}

		internal override JToken CloneToken()
		{
			return new JConstructor(this);
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(_name);
			foreach (JToken item in Children())
			{
				item.WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		internal override int GetDeepHashCode()
		{
			return _name.GetHashCode() ^ ContentsHashCode();
		}

		public new static JConstructor Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jConstructor = new JConstructor((string)reader.Value);
			jConstructor.SetLineInfo(reader as IJsonLineInfo);
			jConstructor.ReadTokenFrom(reader);
			return jConstructor;
		}
	}
	public struct JEnumerable<T> : IJEnumerable<T>, IEnumerable<T>, IEnumerable, IEquatable<JEnumerable<T>> where T : JToken
	{
		public static readonly JEnumerable<T> Empty = new JEnumerable<T>(Enumerable.Empty<T>());

		private readonly IEnumerable<T> _enumerable;

		public IJEnumerable<JToken> this[object key]
		{
			get
			{
				if (_enumerable == null)
				{
					return JEnumerable<JToken>.Empty;
				}
				return new JEnumerable<JToken>(_enumerable.Values<T, JToken>(key));
			}
		}

		public JEnumerable(IEnumerable<T> enumerable)
		{
			ValidationUtils.ArgumentNotNull(enumerable, "enumerable");
			_enumerable = enumerable;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (_enumerable == null)
			{
				return Empty.GetEnumerator();
			}
			return _enumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool Equals(JEnumerable<T> other)
		{
			return object.Equals(_enumerable, other._enumerable);
		}

		public override bool Equals(object obj)
		{
			if (obj is JEnumerable<T>)
			{
				return Equals((JEnumerable<T>)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			if (_enumerable == null)
			{
				return 0;
			}
			return _enumerable.GetHashCode();
		}
	}
	public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
	{
		private class JObjectDynamicProxy : DynamicProxy<JObject>
		{
			public override bool TryGetMember(JObject instance, GetMemberBinder binder, out object result)
			{
				result = instance[binder.Name];
				return true;
			}

			public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
			{
				JToken jToken = value as JToken;
				if (jToken == null)
				{
					jToken = new JValue(value);
				}
				instance[binder.Name] = jToken;
				return true;
			}

			public override IEnumerable<string> GetDynamicMemberNames(JObject instance)
			{
				return from p in instance.Properties()
					select p.Name;
			}
		}

		private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

		protected override IList<JToken> ChildrenTokens => _properties;

		public override JTokenType Type => JTokenType.Object;

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is string propertyName))
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this[propertyName];
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is string propertyName))
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this[propertyName] = value;
			}
		}

		public JToken this[string propertyName]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				return Property(propertyName)?.Value;
			}
			set
			{
				JProperty jProperty = Property(propertyName);
				if (jProperty != null)
				{
					jProperty.Value = value;
					return;
				}
				OnPropertyChanging(propertyName);
				Add(new JProperty(propertyName, value));
				OnPropertyChanged(propertyName);
			}
		}

		ICollection<string> IDictionary<string, JToken>.Keys => _properties.Keys;

		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly => false;

		public event PropertyChangedEventHandler PropertyChanged;

		public event PropertyChangingEventHandler PropertyChanging;

		public JObject()
		{
		}

		public JObject(JObject other)
			: base(other)
		{
		}

		public JObject(params object[] content)
			: this((object)content)
		{
		}

		public JObject(object content)
		{
			Add(content);
		}

		internal override bool DeepEquals(JToken node)
		{
			if (!(node is JObject jObject))
			{
				return false;
			}
			return _properties.Compare(jObject._properties);
		}

		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item == null || item.Type != JTokenType.Comment)
			{
				base.InsertItem(index, item, skipParentCheck);
			}
		}

		internal override void ValidateToken(JToken o, JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), GetType()));
			}
			JProperty jProperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jProperty2 = (JProperty)existing;
				if (jProperty.Name == jProperty2.Name)
				{
					return;
				}
			}
			if (_properties.TryGetValue(jProperty.Name, out existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, jProperty.Name, GetType()));
			}
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			if (!(content is JObject jObject))
			{
				return;
			}
			foreach (KeyValuePair<string, JToken> item in jObject)
			{
				JProperty jProperty = Property(item.Key);
				if (jProperty == null)
				{
					Add(item.Key, item.Value);
				}
				else
				{
					if (item.Value == null)
					{
						continue;
					}
					if (!(jProperty.Value is JContainer jContainer))
					{
						if (item.Value.Type != JTokenType.Null)
						{
							jProperty.Value = item.Value;
						}
					}
					else if (jContainer.Type != item.Value.Type)
					{
						jProperty.Value = item.Value;
					}
					else
					{
						jContainer.Merge(item.Value, settings);
					}
				}
			}
		}

		internal void InternalPropertyChanged(JProperty childProperty)
		{
			OnPropertyChanged(childProperty.Name);
			if (_listChanged != null)
			{
				OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, IndexOfItem(childProperty)));
			}
			if (_collectionChanged != null)
			{
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, childProperty, childProperty, IndexOfItem(childProperty)));
			}
		}

		internal void InternalPropertyChanging(JProperty childProperty)
		{
			OnPropertyChanging(childProperty.Name);
		}

		internal override JToken CloneToken()
		{
			return new JObject(this);
		}

		public IEnumerable<JProperty> Properties()
		{
			return _properties.Cast<JProperty>();
		}

		public JProperty Property(string name)
		{
			if (name == null)
			{
				return null;
			}
			_properties.TryGetValue(name, out var value);
			return (JProperty)value;
		}

		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(from p in Properties()
				select p.Value);
		}

		public new static JObject Load(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JObject jObject = new JObject();
			jObject.SetLineInfo(reader as IJsonLineInfo);
			jObject.ReadTokenFrom(reader);
			return jObject;
		}

		public new static JObject Parse(string json)
		{
			using JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JObject result = Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		public new static JObject FromObject(object o)
		{
			return FromObject(o, JsonSerializer.CreateDefault());
		}

		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jToken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jToken != null && jToken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, jToken.Type));
			}
			return (JObject)jToken;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			for (int i = 0; i < _properties.Count; i++)
			{
				_properties[i].WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		public JToken GetValue(string propertyName)
		{
			return GetValue(propertyName, StringComparison.Ordinal);
		}

		public JToken GetValue(string propertyName, StringComparison comparison)
		{
			if (propertyName == null)
			{
				return null;
			}
			JProperty jProperty = Property(propertyName);
			if (jProperty != null)
			{
				return jProperty.Value;
			}
			if (comparison != StringComparison.Ordinal)
			{
				foreach (JProperty property in _properties)
				{
					if (string.Equals(property.Name, propertyName, comparison))
					{
						return property.Value;
					}
				}
			}
			return null;
		}

		public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
		{
			value = GetValue(propertyName, comparison);
			return value != null;
		}

		public void Add(string propertyName, JToken value)
		{
			Add(new JProperty(propertyName, value));
		}

		bool IDictionary<string, JToken>.ContainsKey(string key)
		{
			return _properties.Contains(key);
		}

		public bool Remove(string propertyName)
		{
			JProperty jProperty = Property(propertyName);
			if (jProperty == null)
			{
				return false;
			}
			jProperty.Remove();
			return true;
		}

		public bool TryGetValue(string propertyName, out JToken value)
		{
			JProperty jProperty = Property(propertyName);
			if (jProperty == null)
			{
				value = null;
				return false;
			}
			value = jProperty.Value;
			return true;
		}

		void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
		{
			Add(new JProperty(item.Key, item.Value));
		}

		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			RemoveAll();
		}

		bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
		{
			JProperty jProperty = Property(item.Key);
			if (jProperty == null)
			{
				return false;
			}
			return jProperty.Value == item.Value;
		}

		void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (base.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JProperty property in _properties)
			{
				ref KeyValuePair<string, JToken> reference = ref array[arrayIndex + num];
				reference = new KeyValuePair<string, JToken>(property.Name, property.Value);
				num++;
			}
		}

		bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
			{
				return false;
			}
			((IDictionary<string, JToken>)this).Remove(item.Key);
			return true;
		}

		internal override int GetDeepHashCode()
		{
			return ContentsHashCode();
		}

		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JProperty property in _properties)
			{
				yield return new KeyValuePair<string, JToken>(property.Name, property.Value);
			}
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual void OnPropertyChanging(string propertyName)
		{
			if (this.PropertyChanging != null)
			{
				this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
			}
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties((Attribute[]?)null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = new PropertyDescriptorCollection(null);
			using IEnumerator<KeyValuePair<string, JToken>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				propertyDescriptorCollection.Add(new JPropertyDescriptor(enumerator.Current.Key));
			}
			return propertyDescriptorCollection;
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return new TypeConverter();
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return null;
		}

		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JObject>(parameter, this, new JObjectDynamicProxy(), dontFallbackFirst: true);
		}
	}
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		private readonly List<JToken> _values = new List<JToken>();

		protected override IList<JToken> ChildrenTokens => _values;

		public override JTokenType Type => JTokenType.Array;

		public override JToken this[object key]
		{
			get
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return GetItem((int)key);
			}
			set
			{
				ValidationUtils.ArgumentNotNull(key, "o");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Array position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				SetItem((int)key, value);
			}
		}

		public JToken this[int index]
		{
			get
			{
				return GetItem(index);
			}
			set
			{
				SetItem(index, value);
			}
		}

		public bool IsReadOnly => false;

		public JArray()
		{
		}

		public JArray(JArray other)
			: base(other)
		{
		}

		public JArray(params object[] content)
			: this((object)content)
		{
		}

		public JArray(object content)
		{
			Add(content);
		}

		internal override bool DeepEquals(JToken node)
		{
			if (node is JArray container)
			{
				return ContentsEqual(container);
			}
			return false;
		}

		internal override JToken CloneToken()
		{
			return new JArray(this);
		}

		public new static JArray Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jArray = new JArray();
			jArray.SetLineInfo(reader as IJsonLineInfo);
			jArray.ReadTokenFrom(reader);
			return jArray;
		}

		public new static JArray Parse(string json)
		{
			using JsonReader jsonReader = new JsonTextReader(new StringReader(json));
			JArray result = Load(jsonReader);
			if (jsonReader.Read() && jsonReader.TokenType != JsonToken.Comment)
			{
				throw JsonReaderException.Create(jsonReader, "Additional text found in JSON string after parsing content.");
			}
			return result;
		}

		public new static JArray FromObject(object o)
		{
			return FromObject(o, JsonSerializer.CreateDefault());
		}

		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jToken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jToken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jToken.Type));
			}
			return (JArray)jToken;
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < _values.Count; i++)
			{
				_values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			IEnumerable enumerable = ((IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null);
			if (enumerable != null)
			{
				JContainer.MergeEnumerableContent(this, enumerable, settings);
			}
		}

		public int IndexOf(JToken item)
		{
			return IndexOfItem(item);
		}

		public void Insert(int index, JToken item)
		{
			InsertItem(index, item, skipParentCheck: false);
		}

		public void RemoveAt(int index)
		{
			RemoveItemAt(index);
		}

		public IEnumerator<JToken> GetEnumerator()
		{
			return Children().GetEnumerator();
		}

		public void Add(JToken item)
		{
			Add((object)item);
		}

		public void Clear()
		{
			ClearItems();
		}

		public bool Contains(JToken item)
		{
			return ContainsItem(item);
		}

		public void CopyTo(JToken[] array, int arrayIndex)
		{
			CopyItemsTo(array, arrayIndex);
		}

		public bool Remove(JToken item)
		{
			return RemoveItem(item);
		}

		internal override int GetDeepHashCode()
		{
			return ContentsHashCode();
		}
	}
	public class JTokenReader : JsonReader, IJsonLineInfo
	{
		private readonly string _initialPath;

		private readonly JToken _root;

		private JToken _parent;

		private JToken _current;

		public JToken CurrentToken => _current;

		int IJsonLineInfo.LineNumber
		{
			get
			{
				if (base.CurrentState == State.Start)
				{
					return 0;
				}
				return ((IJsonLineInfo)_current)?.LineNumber ?? 0;
			}
		}

		int IJsonLineInfo.LinePosition
		{
			get
			{
				if (base.CurrentState == State.Start)
				{
					return 0;
				}
				return ((IJsonLineInfo)_current)?.LinePosition ?? 0;
			}
		}

		public override string Path
		{
			get
			{
				string text = base.Path;
				if (!string.IsNullOrEmpty(_initialPath))
				{
					if (string.IsNullOrEmpty(text))
					{
						return _initialPath;
					}
					text = ((!StringUtils.EndsWith(_initialPath, ']') && !StringUtils.StartsWith(text, '[')) ? (_initialPath + "." + text) : (_initialPath + text));
				}
				return text;
			}
		}

		public JTokenReader(JToken token)
		{
			ValidationUtils.ArgumentNotNull(token, "token");
			_root = token;
		}

		internal JTokenReader(JToken token, string initialPath)
			: this(token)
		{
			_initialPath = initialPath;
		}

		public override byte[] ReadAsBytes()
		{
			return ReadAsBytesInternal();
		}

		public override decimal? ReadAsDecimal()
		{
			return ReadAsDecimalInternal();
		}

		public override int? ReadAsInt32()
		{
			return ReadAsInt32Internal();
		}

		public override string ReadAsString()
		{
			return ReadAsStringInternal();
		}

		public override DateTime? ReadAsDateTime()
		{
			return ReadAsDateTimeInternal();
		}

		public override DateTimeOffset? ReadAsDateTimeOffset()
		{
			return ReadAsDateTimeOffsetInternal();
		}

		internal override bool ReadInternal()
		{
			if (base.CurrentState != 0)
			{
				if (_current == null)
				{
					return false;
				}
				if (_current is JContainer jContainer && _parent != jContainer)
				{
					return ReadInto(jContainer);
				}
				return ReadOver(_current);
			}
			_current = _root;
			SetToken(_current);
			return true;
		}

		public override bool Read()
		{
			_readType = ReadType.Read;
			return ReadInternal();
		}

		private bool ReadOver(JToken t)
		{
			if (t == _root)
			{
				return ReadToEnd();
			}
			JToken next = t.Next;
			if (next == null || next == t || t == t.Parent.Last)
			{
				if (t.Parent == null)
				{
					return ReadToEnd();
				}
				return SetEnd(t.Parent);
			}
			_current = next;
			SetToken(_current);
			return true;
		}

		private bool ReadToEnd()
		{
			_current = null;
			SetToken(JsonToken.None);
			return false;
		}

		private JsonToken? GetEndToken(JContainer c)
		{
			return c.Type switch
			{
				JTokenType.Object => JsonToken.EndObject, 
				JTokenType.Array => JsonToken.EndArray, 
				JTokenType.Constructor => JsonToken.EndConstructor, 
				JTokenType.Property => null, 
				_ => throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", c.Type, "Unexpected JContainer type."), 
			};
		}

		private bool ReadInto(JContainer c)
		{
			JToken first = c.First;
			if (first == null)
			{
				return SetEnd(c);
			}
			SetToken(first);
			_current = first;
			_parent = c;
			return true;
		}

		private bool SetEnd(JContainer c)
		{
			JsonToken? endToken = GetEndToken(c);
			if (endToken.HasValue)
			{
				SetToken(endToken.Value);
				_current = c;
				_parent = c;
				return true;
			}
			return ReadOver(c);
		}

		private void SetToken(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Object:
				SetToken(JsonToken.StartObject);
				break;
			case JTokenType.Array:
				SetToken(JsonToken.StartArray);
				break;
			case JTokenType.Constructor:
				SetToken(JsonToken.StartConstructor, ((JConstructor)token).Name);
				break;
			case JTokenType.Property:
				SetToken(JsonToken.PropertyName, ((JProperty)token).Name);
				break;
			case JTokenType.Comment:
				SetToken(JsonToken.Comment, ((JValue)token).Value);
				break;
			case JTokenType.Integer:
				SetToken(JsonToken.Integer, ((JValue)token).Value);
				break;
			case JTokenType.Float:
				SetToken(JsonToken.Float, ((JValue)token).Value);
				break;
			case JTokenType.String:
				SetToken(JsonToken.String, ((JValue)token).Value);
				break;
			case JTokenType.Boolean:
				SetToken(JsonToken.Boolean, ((JValue)token).Value);
				break;
			case JTokenType.Null:
				SetToken(JsonToken.Null, ((JValue)token).Value);
				break;
			case JTokenType.Undefined:
				SetToken(JsonToken.Undefined, ((JValue)token).Value);
				break;
			case JTokenType.Date:
				SetToken(JsonToken.Date, ((JValue)token).Value);
				break;
			case JTokenType.Raw:
				SetToken(JsonToken.Raw, ((JValue)token).Value);
				break;
			case JTokenType.Bytes:
				SetToken(JsonToken.Bytes, ((JValue)token).Value);
				break;
			case JTokenType.Guid:
				SetToken(JsonToken.String, SafeToString(((JValue)token).Value));
				break;
			case JTokenType.Uri:
				SetToken(JsonToken.String, SafeToString(((JValue)token).Value));
				break;
			case JTokenType.TimeSpan:
				SetToken(JsonToken.String, SafeToString(((JValue)token).Value));
				break;
			default:
				throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", token.Type, "Unexpected JTokenType.");
			}
		}

		private string SafeToString(object value)
		{
			return value?.ToString();
		}

		bool IJsonLineInfo.HasLineInfo()
		{
			if (base.CurrentState == State.Start)
			{
				return false;
			}
			return ((IJsonLineInfo)_current)?.HasLineInfo() ?? false;
		}
	}
	public class JTokenWriter : JsonWriter
	{
		private JContainer _token;

		private JContainer _parent;

		private JValue _value;

		private JToken _current;

		public JToken CurrentToken => _current;

		public JToken Token
		{
			get
			{
				if (_token != null)
				{
					return _token;
				}
				return _value;
			}
		}

		public JTokenWriter(JContainer container)
		{
			ValidationUtils.ArgumentNotNull(container, "container");
			_token = container;
			_parent = container;
		}

		public JTokenWriter()
		{
		}

		public override void Flush()
		{
		}

		public override void Close()
		{
			base.Close();
		}

		public override void WriteStartObject()
		{
			base.WriteStartObject();
			AddParent(new JObject());
		}

		private void AddParent(JContainer container)
		{
			if (_parent == null)
			{
				_token = container;
			}
			else
			{
				_parent.AddAndSkipParentCheck(container);
			}
			_parent = container;
			_current = container;
		}

		private void RemoveParent()
		{
			_current = _parent;
			_parent = _parent.Parent;
			if (_parent != null && _parent.Type == JTokenType.Property)
			{
				_parent = _parent.Parent;
			}
		}

		public override void WriteStartArray()
		{
			base.WriteStartArray();
			AddParent(new JArray());
		}

		public override void WriteStartConstructor(string name)
		{
			base.WriteStartConstructor(name);
			AddParent(new JConstructor(name));
		}

		protected override void WriteEnd(JsonToken token)
		{
			RemoveParent();
		}

		public override void WritePropertyName(string name)
		{
			AddParent(new JProperty(name));
			base.WritePropertyName(name);
		}

		private void AddValue(object value, JsonToken token)
		{
			AddValue(new JValue(value), token);
		}

		internal void AddValue(JValue value, JsonToken token)
		{
			if (_parent != null)
			{
				_parent.Add(value);
				_current = _parent.Last;
				if (_parent.Type == JTokenType.Property)
				{
					_parent = _parent.Parent;
				}
			}
			else
			{
				_value = value ?? JValue.CreateNull();
				_current = _value;
			}
		}

		public override void WriteValue(object value)
		{
			if (value is BigInteger)
			{
				InternalWriteValue(JsonToken.Integer);
				AddValue(value, JsonToken.Integer);
			}
			else
			{
				base.WriteValue(value);
			}
		}

		public override void WriteNull()
		{
			base.WriteNull();
			AddValue(null, JsonToken.Null);
		}

		public override void WriteUndefined()
		{
			base.WriteUndefined();
			AddValue(null, JsonToken.Undefined);
		}

		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			AddValue(new JRaw(json), JsonToken.Raw);
		}

		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			AddValue(JValue.CreateComment(text), JsonToken.Comment);
		}

		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.String);
		}

		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Float);
		}

		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Float);
		}

		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Boolean);
		}

		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			string text = null;
			text = value.ToString(CultureInfo.InvariantCulture);
			AddValue(text, JsonToken.String);
		}

		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Integer);
		}

		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Float);
		}

		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			value = DateTimeUtils.EnsureDateTime(value, base.DateTimeZoneHandling);
			AddValue(value, JsonToken.Date);
		}

		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Date);
		}

		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.Bytes);
		}

		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.String);
		}

		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.String);
		}

		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			AddValue(value, JsonToken.String);
		}
	}
	public class JProperty : JContainer
	{
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			internal JToken _token;

			public int Count
			{
				get
				{
					if (_token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			public bool IsReadOnly => false;

			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						return null;
					}
					return _token;
				}
				set
				{
					if (index == 0)
					{
						_token = value;
					}
				}
			}

			public IEnumerator<JToken> GetEnumerator()
			{
				if (_token != null)
				{
					yield return _token;
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void Add(JToken item)
			{
				_token = item;
			}

			public void Clear()
			{
				_token = null;
			}

			public bool Contains(JToken item)
			{
				return _token == item;
			}

			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (_token != null)
				{
					array[arrayIndex] = _token;
				}
			}

			public bool Remove(JToken item)
			{
				if (_token == item)
				{
					_token = null;
					return true;
				}
				return false;
			}

			public int IndexOf(JToken item)
			{
				if (_token != item)
				{
					return -1;
				}
				return 0;
			}

			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					_token = item;
				}
			}

			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					_token = null;
				}
			}
		}

		private readonly JPropertyList _content = new JPropertyList();

		private readonly string _name;

		protected override IList<JToken> ChildrenTokens => _content;

		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return _name;
			}
		}

		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return _content._token;
			}
			set
			{
				CheckReentrancy();
				JToken item = value ?? JValue.CreateNull();
				if (_content._token == null)
				{
					InsertItem(0, item, skipParentCheck: false);
				}
				else
				{
					SetItem(0, item);
				}
			}
		}

		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		public JProperty(JProperty other)
			: base(other)
		{
			_name = other.Name;
		}

		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return Value;
		}

		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (!JContainer.IsTokenUnchanged(Value, item))
			{
				if (base.Parent != null)
				{
					((JObject)base.Parent).InternalPropertyChanging(this);
				}
				base.SetItem(0, item);
				if (base.Parent != null)
				{
					((JObject)base.Parent).InternalPropertyChanged(this);
				}
			}
		}

		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override void InsertItem(int index, JToken item, bool skipParentCheck)
		{
			if (item == null || item.Type != JTokenType.Comment)
			{
				if (Value != null)
				{
					throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
				}
				base.InsertItem(0, item, skipParentCheck: false);
			}
		}

		internal override bool ContainsItem(JToken item)
		{
			return Value == item;
		}

		internal override void MergeItem(object content, JsonMergeSettings settings)
		{
			if (content is JProperty { Value: not null } jProperty && jProperty.Value.Type != JTokenType.Null)
			{
				Value = jProperty.Value;
			}
		}

		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		internal override bool DeepEquals(JToken node)
		{
			if (node is JProperty jProperty && _name == jProperty.Name)
			{
				return ContentsEqual(jProperty);
			}
			return false;
		}

		internal override JToken CloneToken()
		{
			return new JProperty(this);
		}

		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			_name = name;
		}

		public JProperty(string name, params object[] content)
			: this(name, (object)content)
		{
		}

		public JProperty(string name, object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			_name = name;
			Value = (IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(_name);
			JToken value = Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
			}
			else
			{
				writer.WriteNull();
			}
		}

		internal override int GetDeepHashCode()
		{
			return _name.GetHashCode() ^ ((Value != null) ? Value.GetDeepHashCode() : 0);
		}

		public new static JProperty Load(JsonReader reader)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			while (reader.TokenType == JsonToken.Comment)
			{
				reader.Read();
			}
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jProperty = new JProperty((string)reader.Value);
			jProperty.SetLineInfo(reader as IJsonLineInfo);
			jProperty.ReadTokenFrom(reader);
			return jProperty;
		}
	}
	public enum JTokenType
	{
		None,
		Object,
		Array,
		Constructor,
		Property,
		Comment,
		Integer,
		Float,
		String,
		Boolean,
		Null,
		Undefined,
		Date,
		Raw,
		Bytes,
		Guid,
		Uri,
		TimeSpan
	}
}
namespace Newtonsoft.Json.Schema
{
	public static class Extensions
	{
		public static bool IsValid(this JToken source, JsonSchema schema)
		{
			bool valid = true;
			source.Validate(schema, delegate
			{
				valid = false;
			});
			return valid;
		}

		public static bool IsValid(this JToken source, JsonSchema schema, out IList<string> errorMessages)
		{
			IList<string> errors = new List<string>();
			source.Validate(schema, delegate(object sender, ValidationEventArgs args)
			{
				errors.Add(args.Message);
			});
			errorMessages = errors;
			return errorMessages.Count == 0;
		}

		public static void Validate(this JToken source, JsonSchema schema)
		{
			source.Validate(schema, null);
		}

		public static void Validate(this JToken source, JsonSchema schema, ValidationEventHandler validationEventHandler)
		{
			ValidationUtils.ArgumentNotNull(source, "source");
			ValidationUtils.ArgumentNotNull(schema, "schema");
			using JsonValidatingReader jsonValidatingReader = new JsonValidatingReader(source.CreateReader());
			jsonValidatingReader.Schema = schema;
			if (validationEventHandler != null)
			{
				jsonValidatingReader.ValidationEventHandler += validationEventHandler;
			}
			while (jsonValidatingReader.Read())
			{
			}
		}
	}
	[Serializable]
	public class JsonSchemaException : JsonException
	{
		public int LineNumber { get; private set; }

		public int LinePosition { get; private set; }

		public string Path { get; private set; }

		public JsonSchemaException()
		{
		}

		public JsonSchemaException(string message)
			: base(message)
		{
		}

		public JsonSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public JsonSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal JsonSchemaException(string message, Exception innerException, string path, int lineNumber, int linePosition)
			: base(message, innerException)
		{
			Path = path;
			LineNumber = lineNumber;
			LinePosition = linePosition;
		}
	}
	internal class JsonSchemaModel
	{
		public bool Required { get; set; }

		public JsonSchemaType Type { get; set; }

		public int? MinimumLength { get; set; }

		public int? MaximumLength { get; set; }

		public double? DivisibleBy { get; set; }

		public double? Minimum { get; set; }

		public double? Maximum { get; set; }

		public bool ExclusiveMinimum { get; set; }

		public bool ExclusiveMaximum { get; set; }

		public int? MinimumItems { get; set; }

		public int? MaximumItems { get; set; }

		public IList<string> Patterns { get; set; }

		public IList<JsonSchemaModel> Items { get; set; }

		public IDictionary<string, JsonSchemaModel> Properties { get; set; }

		public IDictionary<string, JsonSchemaModel> PatternProperties { get; set; }

		public JsonSchemaModel AdditionalProperties { get; set; }

		public JsonSchemaModel AdditionalItems { get; set; }

		public bool PositionalItemsValidation { get; set; }

		public bool AllowAdditionalProperties { get; set; }

		public bool AllowAdditionalItems { get; set; }

		public bool UniqueItems { get; set; }

		public IList<JToken> Enum { get; set; }

		public JsonSchemaType Disallow { get; set; }

		public JsonSchemaModel()
		{
			Type = JsonSchemaType.Any;
			AllowAdditionalProperties = true;
			AllowAdditionalItems = true;
			Required = false;
		}

		public static JsonSchemaModel Create(IList<JsonSchema> schemata)
		{
			JsonSchemaModel jsonSchemaModel = new JsonSchemaModel();
			foreach (JsonSchema schematum in schemata)
			{
				Combine(jsonSchemaModel, schematum);
			}
			return jsonSchemaModel;
		}

		private static void Combine(JsonSchemaModel model, JsonSchema schema)
		{
			model.Required = model.Required || (schema.Required ?? false);
			model.Type &= schema.Type ?? JsonSchemaType.Any;
			model.MinimumLength = MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || (schema.ExclusiveMinimum ?? false);
			model.ExclusiveMaximum = model.ExclusiveMaximum || (schema.ExclusiveMaximum ?? false);
			model.MinimumItems = MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.PositionalItemsValidation = model.PositionalItemsValidation || schema.PositionalItemsValidation;
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			model.AllowAdditionalItems = model.AllowAdditionalItems && schema.AllowAdditionalItems;
			model.UniqueItems = model.UniqueItems || schema.UniqueItems;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new List<JToken>();
				}
				model.Enum.AddRangeDistinct(schema.Enum, JToken.EqualityComparer);
			}
			model.Disallow |= schema.Disallow ?? JsonSchemaType.None;
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new List<string>();
				}
				model.Patterns.AddDistinct(schema.Pattern);
			}
		}
	}
	internal class JsonSchemaModelBuilder
	{
		private JsonSchemaNodeCollection _nodes = new JsonSchemaNodeCollection();

		private Dictionary<JsonSchemaNode, JsonSchemaModel> _nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();

		private JsonSchemaNode _node;

		public JsonSchemaModel Build(JsonSchema schema)
		{
			_nodes = new JsonSchemaNodeCollection();
			_node = AddSchema(null, schema);
			_nodeModels = new Dictionary<JsonSchemaNode, JsonSchemaModel>();
			return BuildNodeModel(_node);
		}

		public JsonSchemaNode AddSchema(JsonSchemaNode existingNode, JsonSchema schema)
		{
			string id;
			if (existingNode != null)
			{
				if (existingNode.Schemas.Contains(schema))
				{
					return existingNode;
				}
				id = JsonSchemaNode.GetId(existingNode.Schemas.Union(new JsonSchema[1] { schema }));
			}
			else
			{
				id = JsonSchemaNode.GetId(new JsonSchema[1] { schema });
			}
			if (_nodes.Contains(id))
			{
				return _nodes[id];
			}
			JsonSchemaNode jsonSchemaNode = ((existingNode != null) ? existingNode.Combine(schema) : new JsonSchemaNode(schema));
			_nodes.Add(jsonSchemaNode);
			AddProperties(schema.Properties, jsonSchemaNode.Properties);
			AddProperties(schema.PatternProperties, jsonSchemaNode.PatternProperties);
			if (schema.Items != null)
			{
				for (int i = 0; i < schema.Items.Count; i++)
				{
					AddItem(jsonSchemaNode, i, schema.Items[i]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				AddAdditionalItems(jsonSchemaNode, schema.AdditionalItems);
			}
			if (schema.AdditionalProperties != null)
			{
				AddAdditionalProperties(jsonSchemaNode, schema.AdditionalProperties);
			}
			if (schema.Extends != null)
			{
				foreach (JsonSchema extend in schema.Extends)
				{
					jsonSchemaNode = AddSchema(jsonSchemaNode, extend);
				}
			}
			return jsonSchemaNode;
		}

		public void AddProperties(IDictionary<string, JsonSchema> source, IDictionary<string, JsonSchemaNode> target)
		{
			if (source == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JsonSchema> item in source)
			{
				AddProperty(target, item.Key, item.Value);
			}
		}

		public void AddProperty(IDictionary<string, JsonSchemaNode> target, string propertyName, JsonSchema schema)
		{
			target.TryGetValue(propertyName, out var value);
			target[propertyName] = AddSchema(value, schema);
		}

		public void AddItem(JsonSchemaNode parentNode, int index, JsonSchema schema)
		{
			JsonSchemaNode existingNode = ((parentNode.Items.Count > index) ? parentNode.Items[index] : null);
			JsonSchemaNode jsonSchemaNode = AddSchema(existingNode, schema);
			if (parentNode.Items.Count <= index)
			{
				parentNode.Items.Add(jsonSchemaNode);
			}
			else
			{
				parentNode.Items[index] = jsonSchemaNode;
			}
		}

		public void AddAdditionalProperties(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalProperties = AddSchema(parentNode.AdditionalProperties, schema);
		}

		public void AddAdditionalItems(JsonSchemaNode parentNode, JsonSchema schema)
		{
			parentNode.AdditionalItems = AddSchema(parentNode.AdditionalItems, schema);
		}

		private JsonSchemaModel BuildNodeModel(JsonSchemaNode node)
		{
			if (_nodeModels.TryGetValue(node, out var value))
			{
				return value;
			}
			value = JsonSchemaModel.Create(node.Schemas);
			_nodeModels[node] = value;
			foreach (KeyValuePair<string, JsonSchemaNode> property in node.Properties)
			{
				if (value.Properties == null)
				{
					value.Properties = new Dictionary<string, JsonSchemaModel>();
				}
				value.Properties[property.Key] = BuildNodeModel(property.Value);
			}
			foreach (KeyValuePair<string, JsonSchemaNode> patternProperty in node.PatternProperties)
			{
				if (value.PatternProperties == null)
				{
					value.PatternProperties = new Dictionary<string, JsonSchemaModel>();
				}
				value.PatternProperties[patternProperty.Key] = BuildNodeModel(patternProperty.Value);
			}
			foreach (JsonSchemaNode item in node.Items)
			{
				if (value.Items == null)
				{
					value.Items = new List<JsonSchemaModel>();
				}
				value.Items.Add(BuildNodeModel(item));
			}
			if (node.AdditionalProperties != null)
			{
				value.AdditionalProperties = BuildNodeModel(node.AdditionalProperties);
			}
			if (node.AdditionalItems != null)
			{
				value.AdditionalItems = BuildNodeModel(node.AdditionalItems);
			}
			return value;
		}
	}
	internal class JsonSchemaNodeCollection : KeyedCollection<string, JsonSchemaNode>
	{
		protected override string GetKeyForItem(JsonSchemaNode item)
		{
			return item.Id;
		}
	}
	internal class JsonSchemaNode
	{
		public string Id { get; private set; }

		public ReadOnlyCollection<JsonSchema> Schemas { get; private set; }

		public Dictionary<string, JsonSchemaNode> Properties { get; private set; }

		public Dictionary<string, JsonSchemaNode> PatternProperties { get; private set; }

		public List<JsonSchemaNode> Items { get; private set; }

		public JsonSchemaNode AdditionalProperties { get; set; }

		public JsonSchemaNode AdditionalItems { get; set; }

		public JsonSchemaNode(JsonSchema schema)
		{
			Schemas = new ReadOnlyCollection<JsonSchema>(new JsonSchema[1] { schema });
			Properties = new Dictionary<string, JsonSchemaNode>();
			PatternProperties = new Dictionary<string, JsonSchemaNode>();
			Items = new List<JsonSchemaNode>();
			Id = GetId(Schemas);
		}

		private JsonSchemaNode(JsonSchemaNode source, JsonSchema schema)
		{
			Schemas = new ReadOnlyCollection<JsonSchema>(source.Schemas.Union(new JsonSchema[1] { schema }).ToList());
			Properties = new Dictionary<string, JsonSchemaNode>(source.Properties);
			PatternProperties = new Dictionary<string, JsonSchemaNode>(source.PatternProperties);
			Items = new List<JsonSchemaNode>(source.Items);
			AdditionalProperties = source.AdditionalProperties;
			AdditionalItems = source.AdditionalItems;
			Id = GetId(Schemas);
		}

		public JsonSchemaNode Combine(JsonSchema schema)
		{
			return new JsonSchemaNode(this, schema);
		}

		public static string GetId(IEnumerable<JsonSchema> schemata)
		{
			return string.Join("-", schemata.Select((JsonSchema s) => s.InternalId).OrderBy<string, string>((string id) => id, StringComparer.Ordinal).ToArray());
		}
	}
	public class JsonSchemaResolver
	{
		public IList<JsonSchema> LoadedSchemas { get; protected set; }

		public JsonSchemaResolver()
		{
			LoadedSchemas = new List<JsonSchema>();
		}

		public virtual JsonSchema GetSchema(string reference)
		{
			JsonSchema jsonSchema = LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Id, reference, StringComparison.Ordinal));
			if (jsonSchema == null)
			{
				jsonSchema = LoadedSchemas.SingleOrDefault((JsonSchema s) => string.Equals(s.Location, reference, StringComparison.Ordinal));
			}
			return jsonSchema;
		}
	}
	internal class JsonSchemaWriter
	{
		private readonly JsonWriter _writer;

		private readonly JsonSchemaResolver _resolver;

		public JsonSchemaWriter(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = writer;
			_resolver = resolver;
		}

		private void ReferenceOrWriteSchema(JsonSchema schema)
		{
			if (schema.Id != null && _resolver.GetSchema(schema.Id) != null)
			{
				_writer.WriteStartObject();
				_writer.WritePropertyName("$ref");
				_writer.WriteValue(schema.Id);
				_writer.WriteEndObject();
			}
			else
			{
				WriteSchema(schema);
			}
		}

		public void WriteSchema(JsonSchema schema)
		{
			ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!_resolver.LoadedSchemas.Contains(schema))
			{
				_resolver.LoadedSchemas.Add(schema);
			}
			_writer.WriteStartObject();
			WritePropertyIfNotNull(_writer, "id", schema.Id);
			WritePropertyIfNotNull(_writer, "title", schema.Title);
			WritePropertyIfNotNull(_writer, "description", schema.Description);
			WritePropertyIfNotNull(_writer, "required", schema.Required);
			WritePropertyIfNotNull(_writer, "readonly", schema.ReadOnly);
			WritePropertyIfNotNull(_writer, "hidden", schema.Hidden);
			WritePropertyIfNotNull(_writer, "transient", schema.Transient);
			if (schema.Type.HasValue)
			{
				WriteType("type", _writer, schema.Type.Value);
			}
			if (!schema.AllowAdditionalProperties)
			{
				_writer.WritePropertyName("additionalProperties");
				_writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				_writer.WritePropertyName("additionalProperties");
				ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			if (!schema.AllowAdditionalItems)
			{
				_writer.WritePropertyName("additionalItems");
				_writer.WriteValue(schema.AllowAdditionalItems);
			}
			else if (schema.AdditionalItems != null)
			{
				_writer.WritePropertyName("additionalItems");
				ReferenceOrWriteSchema(schema.AdditionalItems);
			}
			WriteSchemaDictionaryIfNotNull(_writer, "properties", schema.Properties);
			WriteSchemaDictionaryIfNotNull(_writer, "patternProperties", schema.PatternProperties);
			WriteItems(schema);
			WritePropertyIfNotNull(_writer, "minimum", schema.Minimum);
			WritePropertyIfNotNull(_writer, "maximum", schema.Maximum);
			WritePropertyIfNotNull(_writer, "exclusiveMinimum", schema.ExclusiveMinimum);
			WritePropertyIfNotNull(_writer, "exclusiveMaximum", schema.ExclusiveMaximum);
			WritePropertyIfNotNull(_writer, "minLength", schema.MinimumLength);
			WritePropertyIfNotNull(_writer, "maxLength", schema.MaximumLength);
			WritePropertyIfNotNull(_writer, "minItems", schema.MinimumItems);
			WritePropertyIfNotNull(_writer, "maxItems", schema.MaximumItems);
			WritePropertyIfNotNull(_writer, "divisibleBy", schema.DivisibleBy);
			WritePropertyIfNotNull(_writer, "format", schema.Format);
			WritePropertyIfNotNull(_writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				_writer.WritePropertyName("enum");
				_writer.WriteStartArray();
				foreach (JToken item in schema.Enum)
				{
					item.WriteTo(_writer);
				}
				_writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				_writer.WritePropertyName("default");
				schema.Default.WriteTo(_writer);
			}
			if (schema.Disallow.HasValue)
			{
				WriteType("disallow", _writer, schema.Disallow.Value);
			}
			if (schema.Extends != null && schema.Extends.Count > 0)
			{
				_writer.WritePropertyName("extends");
				if (schema.Extends.Count == 1)
				{
					ReferenceOrWriteSchema(schema.Extends[0]);
				}
				else
				{
					_writer.WriteStartArray();
					foreach (JsonSchema extend in schema.Extends)
					{
						ReferenceOrWriteSchema(extend);
					}
					_writer.WriteEndArray();
				}
			}
			_writer.WriteEndObject();
		}

		private void WriteSchemaDictionaryIfNotNull(JsonWriter writer, string propertyName, IDictionary<string, JsonSchema> properties)
		{
			if (properties == null)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			writer.WriteStartObject();
			foreach (KeyValuePair<string, JsonSchema> property in properties)
			{
				writer.WritePropertyName(property.Key);
				ReferenceOrWriteSchema(property.Value);
			}
			writer.WriteEndObject();
		}

		private void WriteItems(JsonSchema schema)
		{
			if (schema.Items == null && !schema.PositionalItemsValidation)
			{
				return;
			}
			_writer.WritePropertyName("items");
			if (!schema.PositionalItemsValidation)
			{
				if (schema.Items != null && schema.Items.Count > 0)
				{
					ReferenceOrWriteSchema(schema.Items[0]);
					return;
				}
				_writer.WriteStartObject();
				_writer.WriteEndObject();
				return;
			}
			_writer.WriteStartArray();
			if (schema.Items != null)
			{
				foreach (JsonSchema item in schema.Items)
				{
					ReferenceOrWriteSchema(item);
				}
			}
			_writer.WriteEndArray();
		}

		private void WriteType(string propertyName, JsonWriter writer, JsonSchemaType type)
		{
			IList<JsonSchemaType> list2;
			if (Enum.IsDefined(typeof(JsonSchemaType), type))
			{
				List<JsonSchemaType> list = new List<JsonSchemaType>();
				list.Add(type);
				list2 = list;
			}
			else
			{
				list2 = (from v in EnumUtils.GetFlagsValues(type)
					where v != JsonSchemaType.None
					select v).ToList();
			}
			if (list2.Count == 0)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			if (list2.Count == 1)
			{
				writer.WriteValue(JsonSchemaBuilder.MapType(list2[0]));
				return;
			}
			writer.WriteStartArray();
			foreach (JsonSchemaType item in list2)
			{
				writer.WriteValue(JsonSchemaBuilder.MapType(item));
			}
			writer.WriteEndArray();
		}

		private void WritePropertyIfNotNull(JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}
	}
	public enum UndefinedSchemaIdHandling
	{
		None,
		UseTypeName,
		UseAssemblyQualifiedName
	}
	public class ValidationEventArgs : EventArgs
	{
		private readonly JsonSchemaException _ex;

		public JsonSchemaException Exception => _ex;

		public string Path => _ex.Path;

		public string Message => _ex.Message;

		internal ValidationEventArgs(JsonSchemaException ex)
		{
			ValidationUtils.ArgumentNotNull(ex, "ex");
			_ex = ex;
		}
	}
	public delegate void ValidationEventHandler(object sender, ValidationEventArgs e);
}
namespace Newtonsoft.Json.Serialization
{
	public interface IContractResolver
	{
		JsonContract ResolveContract(Type type);
	}
	public class DefaultContractResolver : IContractResolver
	{
		internal struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IEnumerable<KeyValuePair<object, object>>, IEnumerable, IEnumerator<KeyValuePair<object, object>>, IDisposable, IEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public KeyValuePair<object, object> Current => new KeyValuePair<object, object>(_e.Current.Key, _e.Current.Value);

			object IEnumerator.Current => Current;

			public DictionaryEnumerator(IEnumerable<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				_e = e.GetEnumerator();
			}

			public bool MoveNext()
			{
				return _e.MoveNext();
			}

			public void Reset()
			{
				_e.Reset();
			}

			public void Dispose()
			{
				_e.Dispose();
			}

			public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
			{
				return this;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}
		}

		private static readonly IContractResolver _instance = new DefaultContractResolver(shareCache: true);

		private static readonly JsonConverter[] BuiltInConverters = new JsonConverter[10]
		{
			new EntityKeyMemberConverter(),
			new ExpandoObjectConverter(),
			new XmlNodeConverter(),
			new BinaryConverter(),
			new DataSetConverter(),
			new DataTableConverter(),
			new DiscriminatedUnionConverter(),
			new KeyValuePairConverter(),
			new BsonObjectIdConverter(),
			new RegexConverter()
		};

		private static readonly object TypeContractCacheLock = new object();

		private static readonly DefaultContractResolverState _sharedState = new DefaultContractResolverState();

		private readonly DefaultContractResolverState _instanceState = new DefaultContractResolverState();

		private readonly bool _sharedCache;

		internal static IContractResolver Instance => _instance;

		public bool DynamicCodeGeneration => JsonTypeReflector.DynamicCodeGeneration;

		[Obsolete("DefaultMembersSearchFlags is obsolete. To modify the members serialized inherit from DefaultContractResolver and override the GetSerializableMembers method instead.")]
		public BindingFlags DefaultMembersSearchFlags { get; set; }

		public bool SerializeCompilerGeneratedMembers { get; set; }

		public bool IgnoreSerializableInterface { get; set; }

		public bool IgnoreSerializableAttribute { get; set; }

		public DefaultContractResolver()
			: this(shareCache: false)
		{
		}

		public DefaultContractResolver(bool shareCache)
		{
			DefaultMembersSearchFlags = BindingFlags.Instance | BindingFlags.Public;
			IgnoreSerializableAttribute = true;
			_sharedCache = shareCache;
		}

		internal DefaultContractResolverState GetState()
		{
			if (_sharedCache)
			{
				return _sharedState;
			}
			return _instanceState;
		}

		public virtual JsonContract ResolveContract(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			DefaultContractResolverState state = GetState();
			ResolverContractKey key = new ResolverContractKey(GetType(), type);
			Dictionary<ResolverContractKey, JsonContract> contractCache = state.ContractCache;
			if (contractCache == null || !contractCache.TryGetValue(key, out var value))
			{
				value = CreateContract(type);
				lock (TypeContractCacheLock)
				{
					contractCache = state.ContractCache;
					Dictionary<ResolverContractKey, JsonContract> dictionary = ((contractCache != null) ? new Dictionary<ResolverContractKey, JsonContract>(contractCache) : new Dictionary<ResolverContractKey, JsonContract>());
					dictionary[key] = value;
					state.ContractCache = dictionary;
				}
			}
			return value;
		}

		protected virtual List<MemberInfo> GetSerializableMembers(Type objectType)
		{
			bool ignoreSerializableAttribute = IgnoreSerializableAttribute;
			MemberSerialization objectMemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(objectType, ignoreSerializableAttribute);
			List<MemberInfo> list = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				where !ReflectionUtils.IsIndexedProperty(m)
				select m).ToList();
			List<MemberInfo> list2 = new List<MemberInfo>();
			if (objectMemberSerialization != MemberSerialization.Fields)
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(objectType);
				List<MemberInfo> list3 = (from m in ReflectionUtils.GetFieldsAndProperties(objectType, DefaultMembersSearchFlags)
					where !ReflectionUtils.IsIndexedProperty(m)
					select m).ToList();
				foreach (MemberInfo item in list)
				{
					if (SerializeCompilerGeneratedMembers || !item.IsDefined(typeof(CompilerGeneratedAttribute), inherit: true))
					{
						if (list3.Contains(item))
						{
							list2.Add(item);
						}
						else if (JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(item) != null)
						{
							list2.Add(item);
						}
						else if (dataContractAttribute != null && JsonTypeReflector.GetAttribute<DataMemberAttribute>(item) != null)
						{
							list2.Add(item);
						}
						else if (objectMemberSerialization == MemberSerialization.Fields && item.MemberType() == MemberTypes.Field)
						{
							list2.Add(item);
						}
					}
				}
				if (objectType.AssignableToTypeName("System.Data.Objects.DataClasses.EntityObject", out var _))
				{
					list2 = list2.Where(ShouldSerializeEntityMember).ToList();
				}
			}
			else
			{
				foreach (MemberInfo item2 in list)
				{
					FieldInfo fieldInfo = item2 as FieldInfo;
					if (fieldInfo != null && !fieldInfo.IsStatic)
					{
						list2.Add(item2);
					}
				}
			}
			return list2;
		}

		private bool ShouldSerializeEntityMember(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null && propertyInfo.PropertyType.IsGenericType() && propertyInfo.PropertyType.GetGenericTypeDefinition().FullName == "System.Data.Objects.DataClasses.EntityReference`1")
			{
				return false;
			}
			return true;
		}

		protected virtual JsonObjectContract CreateObjectContract(Type objectType)
		{
			JsonObjectContract jsonObjectContract = new JsonObjectContract(objectType);
			InitializeContract(jsonObjectContract);
			bool ignoreSerializableAttribute = IgnoreSerializableAttribute;
			jsonObjectContract.MemberSerialization = JsonTypeReflector.GetObjectMemberSerialization(jsonObjectContract.NonNullableUnderlyingType, ignoreSerializableAttribute);
			jsonObjectContract.Properties.AddRange(CreateProperties(jsonObjectContract.NonNullableUnderlyingType, jsonObjectContract.MemberSerialization));
			JsonObjectAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonObjectAttribute>(jsonObjectContract.NonNullableUnderlyingType);
			if (cachedAttribute != null)
			{
				jsonObjectContract.ItemRequired = cachedAttribute._itemRequired;
			}
			if (jsonObjectContract.IsInstantiable)
			{
				ConstructorInfo attributeConstructor = GetAttributeConstructor(jsonObjectContract.NonNullableUnderlyingType);
				if (attributeConstructor != null)
				{
					jsonObjectContract.OverrideConstructor = attributeConstructor;
					jsonObjectContract.CreatorParameters.AddRange(CreateConstructorParameters(attributeConstructor, jsonObjectContract.Properties));
				}
				else if (jsonObjectContract.MemberSerialization == MemberSerialization.Fields)
				{
					if (JsonTypeReflector.FullyTrusted)
					{
						jsonObjectContract.DefaultCreator = jsonObjectContract.GetUninitializedObject;
					}
				}
				else if (jsonObjectContract.DefaultCreator == null || jsonObjectContract.DefaultCreatorNonPublic)
				{
					ConstructorInfo parametrizedConstructor = GetParametrizedConstructor(jsonObjectContract.NonNullableUnderlyingType);
					if (parametrizedConstructor != null)
					{
						jsonObjectContract.ParametrizedConstructor = parametrizedConstructor;
						jsonObjectContract.CreatorParameters.AddRange(CreateConstructorParameters(parametrizedConstructor, jsonObjectContract.Properties));
					}
				}
			}
			MemberInfo extensionDataMemberForType = GetExtensionDataMemberForType(jsonObjectContract.NonNullableUnderlyingType);
			if (extensionDataMemberForType != null)
			{
				SetExtensionDataDelegates(jsonObjectContract, extensionDataMemberForType);
			}
			return jsonObjectContract;
		}

		private MemberInfo GetExtensionDataMemberForType(Type type)
		{
			IEnumerable<MemberInfo> source = GetClassHierarchyForType(type).SelectMany(delegate(Type baseType)
			{
				IList<MemberInfo> list = new List<MemberInfo>();
				list.AddRange(baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				list.AddRange(baseType.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
				return list;
			});
			return source.LastOrDefault(delegate(MemberInfo m)
			{
				MemberTypes memberTypes = m.MemberType();
				if (memberTypes != MemberTypes.Property && memberTypes != MemberTypes.Field)
				{
					return false;
				}
				if (!m.IsDefined(typeof(JsonExtensionDataAttribute), inherit: false))
				{
					return false;
				}
				Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(m);
				if (ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >), out var implementingType))
				{
					Type type2 = implementingType.GetGenericArguments()[0];
					Type type3 = implementingType.GetGenericArguments()[1];
					if (type2.IsAssignableFrom(typeof(string)) && type3.IsAssignableFrom(typeof(JToken)))
					{
						return true;
					}
				}
				throw new JsonException("Invalid extension data attribute on '{0}'. Member '{1}' type must implement IDictionary<string, JToken>.".FormatWith(CultureInfo.InvariantCulture, GetClrTypeFullName(m.DeclaringType), m.Name));
			});
		}

		private static void SetExtensionDataDelegates(JsonObjectContract contract, MemberInfo member)
		{
			JsonExtensionDataAttribute attribute = ReflectionUtils.GetAttribute<JsonExtensionDataAttribute>(member);
			if (attribute == null)
			{
				return;
			}
			Type memberUnderlyingType = ReflectionUtils.GetMemberUnderlyingType(member);
			ReflectionUtils.ImplementsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >), out var implementingType);
			Type type = implementingType.GetGenericArguments()[0];
			Type type2 = implementingType.GetGenericArguments()[1];
			bool isJTokenValueType = typeof(JToken).IsAssignableFrom(type2);
			Type type3 = ((!ReflectionUtils.IsGenericDefinition(memberUnderlyingType, typeof(IDictionary<, >))) ? memberUnderlyingType : typeof(Dictionary<, >).MakeGenericType(type, type2));
			MethodInfo method = memberUnderlyingType.GetMethod("Add", new Type[2] { type, type2 });
			Func<object, object> getExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(member);
			Action<object, object> setExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(member);
			Func<object> createExtensionDataDictionary = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type3);
			MethodCall<object, object> setExtensionDataDictionaryValue = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			ExtensionDataSetter extensionDataSetter = delegate(object o, string key, object value)
			{
				object obj2 = getExtensionDataDictionary(o);
				if (obj2 == null)
				{
					obj2 = createExtensionDataDictionary();
					setExtensionDataDictionary(o, obj2);
				}
				if (isJTokenValueType && !(value is JToken))
				{
					value = ((value != null) ? JToken.FromObject(value) : JValue.CreateNull());
				}
				setExtensionDataDictionaryValue(obj2, key, value);
			};
			Type type4 = typeof(DictionaryEnumerator<, >).MakeGenericType(type, type2);
			ConstructorInfo method2 = type4.GetConstructors().First();
			ObjectConstructor<object> createEnumerableWrapper = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(method2);
			ExtensionDataGetter extensionDataGetter = delegate(object o)
			{
				object obj = getExtensionDataDictionary(o);
				return (obj == null) ? null : ((IEnumerable<KeyValuePair<object, object>>)createEnumerableWrapper(obj));
			};
			if (attribute.ReadData)
			{
				contract.ExtensionDataSetter = extensionDataSetter;
			}
			if (attribute.WriteData)
			{
				contract.ExtensionDataGetter = extensionDataGetter;
			}
		}

		private ConstructorInfo GetAttributeConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = (from c in objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where c.IsDefined(typeof(JsonConstructorAttribute), inherit: true)
				select c).ToList();
			if (list.Count > 1)
			{
				throw new JsonException("Multiple constructors with the JsonConstructorAttribute.");
			}
			if (list.Count == 1)
			{
				return list[0];
			}
			if (objectType == typeof(Version))
			{
				return objectType.GetConstructor(new Type[4]
				{
					typeof(int),
					typeof(int),
					typeof(int),
					typeof(int)
				});
			}
			return null;
		}

		private ConstructorInfo GetParametrizedConstructor(Type objectType)
		{
			IList<ConstructorInfo> list = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).ToList();
			if (list.Count == 1)
			{
				return list[0];
			}
			return null;
		}

		protected virtual IList<JsonProperty> CreateConstructorParameters(ConstructorInfo constructor, JsonPropertyCollection memberProperties)
		{
			ParameterInfo[] parameters = constructor.GetParameters();
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(constructor.DeclaringType);
			ParameterInfo[] array = parameters;
			foreach (ParameterInfo parameterInfo in array)
			{
				JsonProperty jsonProperty = ((parameterInfo.Name != null) ? memberProperties.GetClosestMatchProperty(parameterInfo.Name) : null);
				if (jsonProperty != null && jsonProperty.PropertyType != parameterInfo.ParameterType)
				{
					jsonProperty = null;
				}
				if (jsonProperty != null || parameterInfo.Name != null)
				{
					JsonProperty jsonProperty2 = CreatePropertyFromConstructorParameter(jsonProperty, parameterInfo);
					if (jsonProperty2 != null)
					{
						jsonPropertyCollection.AddProperty(jsonProperty2);
					}
				}
			}
			return jsonPropertyCollection;
		}

		protected virtual JsonProperty CreatePropertyFromConstructorParameter(JsonProperty matchingMemberProperty, ParameterInfo parameterInfo)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = parameterInfo.ParameterType;
			jsonProperty.AttributeProvider = new ReflectionAttributeProvider(parameterInfo);
			SetPropertySettingsFromAttributes(jsonProperty, parameterInfo, parameterInfo.Name, parameterInfo.Member.DeclaringType, MemberSerialization.OptOut, out var _);
			jsonProperty.Readable = false;
			jsonProperty.Writable = true;
			if (matchingMemberProperty != null)
			{
				jsonProperty.PropertyName = ((jsonProperty.PropertyName != parameterInfo.Name) ? jsonProperty.PropertyName : matchingMemberProperty.PropertyName);
				jsonProperty.Converter = jsonProperty.Converter ?? matchingMemberProperty.Converter;
				jsonProperty.MemberConverter = jsonProperty.MemberConverter ?? matchingMemberProperty.MemberConverter;
				if (!jsonProperty._hasExplicitDefaultValue && matchingMemberProperty._hasExplicitDefaultValue)
				{
					jsonProperty.DefaultValue = matchingMemberProperty.DefaultValue;
				}
				jsonProperty._required = jsonProperty._required ?? matchingMemberProperty._required;
				jsonProperty.IsReference = jsonProperty.IsReference ?? matchingMemberProperty.IsReference;
				jsonProperty.NullValueHandling = jsonProperty.NullValueHandling ?? matchingMemberProperty.NullValueHandling;
				jsonProperty.DefaultValueHandling = jsonProperty.DefaultValueHandling ?? matchingMemberProperty.DefaultValueHandling;
				jsonProperty.ReferenceLoopHandling = jsonProperty.ReferenceLoopHandling ?? matchingMemberProperty.ReferenceLoopHandling;
				jsonProperty.ObjectCreationHandling = jsonProperty.ObjectCreationHandling ?? matchingMemberProperty.ObjectCreationHandling;
				jsonProperty.TypeNameHandling = jsonProperty.TypeNameHandling ?? matchingMemberProperty.TypeNameHandling;
			}
			return jsonProperty;
		}

		protected virtual JsonConverter ResolveContractConverter(Type objectType)
		{
			return JsonTypeReflector.GetJsonConverter(objectType);
		}

		private Func<object> GetDefaultCreator(Type createdType)
		{
			return JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(createdType);
		}

		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework", MessageId = "System.Runtime.Serialization.DataContractAttribute.#get_IsReference()")]
		private void InitializeContract(JsonContract contract)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(contract.NonNullableUnderlyingType);
			if (cachedAttribute != null)
			{
				contract.IsReference = cachedAttribute._isReference;
			}
			else
			{
				DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(contract.NonNullableUnderlyingType);
				if (dataContractAttribute != null && dataContractAttribute.IsReference)
				{
					contract.IsReference = true;
				}
			}
			contract.Converter = ResolveContractConverter(contract.NonNullableUnderlyingType);
			contract.InternalConverter = JsonSerializer.GetMatchingConverter(BuiltInConverters, contract.NonNullableUnderlyingType);
			if (contract.IsInstantiable && (ReflectionUtils.HasDefaultConstructor(contract.CreatedType, nonPublic: true) || contract.CreatedType.IsValueType()))
			{
				contract.DefaultCreator = GetDefaultCreator(contract.CreatedType);
				contract.DefaultCreatorNonPublic = !contract.CreatedType.IsValueType() && ReflectionUtils.GetDefaultConstructor(contract.CreatedType) == null;
			}
			ResolveCallbackMethods(contract, contract.NonNullableUnderlyingType);
		}

		private void ResolveCallbackMethods(JsonContract contract, Type t)
		{
			GetCallbackMethodsForType(t, out var onSerializing, out var onSerialized, out var onDeserializing, out var onDeserialized, out var onError);
			if (onSerializing != null && t.Name != "FSharpSet`1" && t.Name != "FSharpMap`2")
			{
				contract.OnSerializingCallbacks.AddRange(onSerializing);
			}
			if (onSerialized != null)
			{
				contract.OnSerializedCallbacks.AddRange(onSerialized);
			}
			if (onDeserializing != null)
			{
				contract.OnDeserializingCallbacks.AddRange(onDeserializing);
			}
			if (onDeserialized != null && t.Name != "FSharpSet`1" && t.Name != "FSharpMap`2" && (!t.IsGenericType() || t.GetGenericTypeDefinition() != typeof(ConcurrentDictionary<, >)))
			{
				contract.OnDeserializedCallbacks.AddRange(onDeserialized);
			}
			if (onError != null)
			{
				contract.OnErrorCallbacks.AddRange(onError);
			}
		}

		private void GetCallbackMethodsForType(Type type, out List<SerializationCallback> onSerializing, out List<SerializationCallback> onSerialized, out List<SerializationCallback> onDeserializing, out List<SerializationCallback> onDeserialized, out List<SerializationErrorCallback> onError)
		{
			onSerializing = null;
			onSerialized = null;
			onDeserializing = null;
			onDeserialized = null;
			onError = null;
			foreach (Type item in GetClassHierarchyForType(type))
			{
				MethodInfo currentCallback = null;
				MethodInfo currentCallback2 = null;
				MethodInfo currentCallback3 = null;
				MethodInfo currentCallback4 = null;
				MethodInfo currentCallback5 = null;
				MethodInfo[] methods = item.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				foreach (MethodInfo methodInfo in methods)
				{
					if (!methodInfo.ContainsGenericParameters)
					{
						Type prevAttributeType = null;
						ParameterInfo[] parameters = methodInfo.GetParameters();
						if (IsValidCallback(methodInfo, parameters, typeof(OnSerializingAttribute), currentCallback, ref prevAttributeType))
						{
							onSerializing = onSerializing ?? new List<SerializationCallback>();
							onSerializing.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback = methodInfo;
						}
						if (IsValidCallback(methodInfo, parameters, typeof(OnSerializedAttribute), currentCallback2, ref prevAttributeType))
						{
							onSerialized = onSerialized ?? new List<SerializationCallback>();
							onSerialized.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback2 = methodInfo;
						}
						if (IsValidCallback(methodInfo, parameters, typeof(OnDeserializingAttribute), currentCallback3, ref prevAttributeType))
						{
							onDeserializing = onDeserializing ?? new List<SerializationCallback>();
							onDeserializing.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback3 = methodInfo;
						}
						if (IsValidCallback(methodInfo, parameters, typeof(OnDeserializedAttribute), currentCallback4, ref prevAttributeType))
						{
							onDeserialized = onDeserialized ?? new List<SerializationCallback>();
							onDeserialized.Add(JsonContract.CreateSerializationCallback(methodInfo));
							currentCallback4 = methodInfo;
						}
						if (IsValidCallback(methodInfo, parameters, typeof(OnErrorAttribute), currentCallback5, ref prevAttributeType))
						{
							onError = onError ?? new List<SerializationErrorCallback>();
							onError.Add(JsonContract.CreateSerializationErrorCallback(methodInfo));
							currentCallback5 = methodInfo;
						}
					}
				}
			}
		}

		private List<Type> GetClassHierarchyForType(Type type)
		{
			List<Type> list = new List<Type>();
			Type type2 = type;
			while (type2 != null && type2 != typeof(object))
			{
				list.Add(type2);
				type2 = type2.BaseType();
			}
			list.Reverse();
			return list;
		}

		protected virtual JsonDictionaryContract CreateDictionaryContract(Type objectType)
		{
			JsonDictionaryContract jsonDictionaryContract = new JsonDictionaryContract(objectType);
			InitializeContract(jsonDictionaryContract);
			jsonDictionaryContract.PropertyNameResolver = ResolvePropertyName;
			return jsonDictionaryContract;
		}

		protected virtual JsonArrayContract CreateArrayContract(Type objectType)
		{
			JsonArrayContract jsonArrayContract = new JsonArrayContract(objectType);
			InitializeContract(jsonArrayContract);
			return jsonArrayContract;
		}

		protected virtual JsonPrimitiveContract CreatePrimitiveContract(Type objectType)
		{
			JsonPrimitiveContract jsonPrimitiveContract = new JsonPrimitiveContract(objectType);
			InitializeContract(jsonPrimitiveContract);
			return jsonPrimitiveContract;
		}

		protected virtual JsonLinqContract CreateLinqContract(Type objectType)
		{
			JsonLinqContract jsonLinqContract = new JsonLinqContract(objectType);
			InitializeContract(jsonLinqContract);
			return jsonLinqContract;
		}

		protected virtual JsonISerializableContract CreateISerializableContract(Type objectType)
		{
			JsonISerializableContract jsonISerializableContract = new JsonISerializableContract(objectType);
			InitializeContract(jsonISerializableContract);
			ConstructorInfo constructor = jsonISerializableContract.NonNullableUnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
			{
				typeof(SerializationInfo),
				typeof(StreamingContext)
			}, null);
			if (constructor != null)
			{
				ObjectConstructor<object> iSerializableCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
				jsonISerializableContract.ISerializableCreator = iSerializableCreator;
			}
			return jsonISerializableContract;
		}

		protected virtual JsonDynamicContract CreateDynamicContract(Type objectType)
		{
			JsonDynamicContract jsonDynamicContract = new JsonDynamicContract(objectType);
			InitializeContract(jsonDynamicContract);
			jsonDynamicContract.PropertyNameResolver = ResolvePropertyName;
			jsonDynamicContract.Properties.AddRange(CreateProperties(objectType, MemberSerialization.OptOut));
			return jsonDynamicContract;
		}

		protected virtual JsonStringContract CreateStringContract(Type objectType)
		{
			JsonStringContract jsonStringContract = new JsonStringContract(objectType);
			InitializeContract(jsonStringContract);
			return jsonStringContract;
		}

		protected virtual JsonContract CreateContract(Type objectType)
		{
			if (IsJsonPrimitiveType(objectType))
			{
				return CreatePrimitiveContract(objectType);
			}
			Type type = ReflectionUtils.EnsureNotNullableType(objectType);
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute is JsonObjectAttribute)
			{
				return CreateObjectContract(objectType);
			}
			if (cachedAttribute is JsonArrayAttribute)
			{
				return CreateArrayContract(objectType);
			}
			if (cachedAttribute is JsonDictionaryAttribute)
			{
				return CreateDictionaryContract(objectType);
			}
			if (type == typeof(JToken) || type.IsSubclassOf(typeof(JToken)))
			{
				return CreateLinqContract(objectType);
			}
			if (CollectionUtils.IsDictionaryType(type))
			{
				return CreateDictionaryContract(objectType);
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return CreateArrayContract(objectType);
			}
			if (CanConvertToString(type))
			{
				return CreateStringContract(objectType);
			}
			if (!IgnoreSerializableInterface && typeof(ISerializable).IsAssignableFrom(type))
			{
				return CreateISerializableContract(objectType);
			}
			if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(type))
			{
				return CreateDynamicContract(objectType);
			}
			if (IsIConvertible(type))
			{
				return CreatePrimitiveContract(type);
			}
			return CreateObjectContract(objectType);
		}

		internal static bool IsJsonPrimitiveType(Type t)
		{
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(t);
			if (typeCode != 0)
			{
				return typeCode != PrimitiveTypeCode.Object;
			}
			return false;
		}

		internal static bool IsIConvertible(Type t)
		{
			if (typeof(IConvertible).IsAssignableFrom(t) || (ReflectionUtils.IsNullableType(t) && typeof(IConvertible).IsAssignableFrom(Nullable.GetUnderlyingType(t))))
			{
				return !typeof(JToken).IsAssignableFrom(t);
			}
			return false;
		}

		internal static bool CanConvertToString(Type type)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && !(converter is ComponentConverter) && !(converter is ReferenceConverter) && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				return true;
			}
			if (type == typeof(Type) || type.IsSubclassOf(typeof(Type)))
			{
				return true;
			}
			return false;
		}

		private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
		{
			if (!method.IsDefined(attributeType, inherit: false))
			{
				return false;
			}
			if (currentCallback != null)
			{
				throw new JsonException("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.".FormatWith(CultureInfo.InvariantCulture, method, currentCallback, GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (prevAttributeType != null)
			{
				throw new JsonException("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.".FormatWith(CultureInfo.InvariantCulture, prevAttributeType, attributeType, GetClrTypeFullName(method.DeclaringType), method));
			}
			if (method.IsVirtual)
			{
				throw new JsonException("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.".FormatWith(CultureInfo.InvariantCulture, method, GetClrTypeFullName(method.DeclaringType), attributeType));
			}
			if (method.ReturnType != typeof(void))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must return void.".FormatWith(CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method));
			}
			if (attributeType == typeof(OnErrorAttribute))
			{
				if (parameters == null || parameters.Length != 2 || parameters[0].ParameterType != typeof(StreamingContext) || parameters[1].ParameterType != typeof(ErrorContext))
				{
					throw new JsonException("Serialization Error Callback '{1}' in type '{0}' must have two parameters of type '{2}' and '{3}'.".FormatWith(CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext), typeof(ErrorContext)));
				}
			}
			else if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(StreamingContext))
			{
				throw new JsonException("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.".FormatWith(CultureInfo.InvariantCulture, GetClrTypeFullName(method.DeclaringType), method, typeof(StreamingContext)));
			}
			prevAttributeType = attributeType;
			return true;
		}

		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition() || !type.ContainsGenericParameters())
			{
				return type.FullName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[2] { type.Namespace, type.Name });
		}

		protected virtual IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			List<MemberInfo> serializableMembers = GetSerializableMembers(type);
			if (serializableMembers == null)
			{
				throw new JsonSerializationException("Null collection of seralizable members returned.");
			}
			JsonPropertyCollection jsonPropertyCollection = new JsonPropertyCollection(type);
			foreach (MemberInfo item in serializableMembers)
			{
				JsonProperty jsonProperty = CreateProperty(item, memberSerialization);
				if (jsonProperty != null)
				{
					DefaultContractResolverState state = GetState();
					lock (state.NameTable)
					{
						jsonProperty.PropertyName = state.NameTable.Add(jsonProperty.PropertyName);
					}
					jsonPropertyCollection.AddProperty(jsonProperty);
				}
			}
			return jsonPropertyCollection.OrderBy((JsonProperty p) => p.Order ?? (-1)).ToList();
		}

		protected virtual IValueProvider CreateMemberValueProvider(MemberInfo member)
		{
			if (DynamicCodeGeneration)
			{
				return new DynamicValueProvider(member);
			}
			return new ReflectionValueProvider(member);
		}

		protected virtual JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = new JsonProperty();
			jsonProperty.PropertyType = ReflectionUtils.GetMemberUnderlyingType(member);
			jsonProperty.DeclaringType = member.DeclaringType;
			jsonProperty.ValueProvider = CreateMemberValueProvider(member);
			jsonProperty.AttributeProvider = new ReflectionAttributeProvider(member);
			SetPropertySettingsFromAttributes(jsonProperty, member, member.Name, member.DeclaringType, memberSerialization, out var allowNonPublicAccess);
			if (memberSerialization != MemberSerialization.Fields)
			{
				jsonProperty.Readable = ReflectionUtils.CanReadMemberValue(member, allowNonPublicAccess);
				jsonProperty.Writable = ReflectionUtils.CanSetMemberValue(member, allowNonPublicAccess, jsonProperty.HasMemberAttribute);
			}
			else
			{
				jsonProperty.Readable = true;
				jsonProperty.Writable = true;
			}
			jsonProperty.ShouldSerialize = CreateShouldSerializeTest(member);
			SetIsSpecifiedActions(jsonProperty, member, allowNonPublicAccess);
			return jsonProperty;
		}

		private void SetPropertySettingsFromAttributes(JsonProperty property, object attributeProvider, string name, Type declaringType, MemberSerialization memberSerialization, out bool allowNonPublicAccess)
		{
			DataContractAttribute dataContractAttribute = JsonTypeReflector.GetDataContractAttribute(declaringType);
			MemberInfo memberInfo = attributeProvider as MemberInfo;
			DataMemberAttribute dataMemberAttribute = ((dataContractAttribute == null || !(memberInfo != null)) ? null : JsonTypeReflector.GetDataMemberAttribute(memberInfo));
			JsonPropertyAttribute attribute = JsonTypeReflector.GetAttribute<JsonPropertyAttribute>(attributeProvider);
			if (attribute != null)
			{
				property.HasMemberAttribute = true;
			}
			string propertyName = ((attribute != null && attribute.PropertyName != null) ? attribute.PropertyName : ((dataMemberAttribute == null || dataMemberAttribute.Name == null) ? name : dataMemberAttribute.Name));
			property.PropertyName = ResolvePropertyName(propertyName);
			property.UnderlyingName = name;
			bool flag = false;
			if (attribute != null)
			{
				property._required = attribute._required;
				property.Order = attribute._order;
				property.DefaultValueHandling = attribute._defaultValueHandling;
				flag = true;
			}
			else if (dataMemberAttribute != null)
			{
				property._required = (dataMemberAttribute.IsRequired ? Required.AllowNull : Required.Default);
				property.Order = ((dataMemberAttribute.Order != -1) ? new int?(dataMemberAttribute.Order) : null);
				property.DefaultValueHandling = ((!dataMemberAttribute.EmitDefaultValue) ? new DefaultValueHandling?(DefaultValueHandling.Ignore) : null);
				flag = true;
			}
			bool flag2 = JsonTypeReflector.GetAttribute<JsonIgnoreAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<JsonExtensionDataAttribute>(attributeProvider) != null || JsonTypeReflector.GetAttribute<NonSerializedAttribute>(attributeProvider) != null;
			if (memberSerialization != MemberSerialization.OptIn)
			{
				bool flag3 = false;
				flag3 = JsonTypeReflector.GetAttribute<IgnoreDataMemberAttribute>(attributeProvider) != null;
				property.Ignored = flag2 || flag3;
			}
			else
			{
				property.Ignored = flag2 || !flag;
			}
			property.Converter = JsonTypeReflector.GetJsonConverter(attributeProvider);
			property.MemberConverter = JsonTypeReflector.GetJsonConverter(attributeProvider);
			DefaultValueAttribute attribute2 = JsonTypeReflector.GetAttribute<DefaultValueAttribute>(attributeProvider);
			if (attribute2 != null)
			{
				property.DefaultValue = attribute2.Value;
			}
			property.NullValueHandling = attribute?._nullValueHandling;
			property.ReferenceLoopHandling = attribute?._referenceLoopHandling;
			property.ObjectCreationHandling = attribute?._objectCreationHandling;
			property.TypeNameHandling = attribute?._typeNameHandling;
			property.IsReference = attribute?._isReference;
			property.ItemIsReference = attribute?._itemIsReference;
			property.ItemConverter = ((attribute != null && attribute.ItemConverterType != null) ? JsonTypeReflector.CreateJsonConverterInstance(attribute.ItemConverterType, attribute.ItemConverterParameters) : null);
			property.ItemReferenceLoopHandling = attribute?._itemReferenceLoopHandling;
			property.ItemTypeNameHandling = attribute?._itemTypeNameHandling;
			allowNonPublicAccess = false;
			if ((DefaultMembersSearchFlags & BindingFlags.NonPublic) == BindingFlags.NonPublic)
			{
				allowNonPublicAccess = true;
			}
			if (attribute != null)
			{
				allowNonPublicAccess = true;
			}
			if (memberSerialization == MemberSerialization.Fields)
			{
				allowNonPublicAccess = true;
			}
			if (dataMemberAttribute != null)
			{
				allowNonPublicAccess = true;
				property.HasMemberAttribute = true;
			}
		}

		private Predicate<object> CreateShouldSerializeTest(MemberInfo member)
		{
			MethodInfo method = member.DeclaringType.GetMethod("ShouldSerialize" + member.Name, ReflectionUtils.EmptyTypes);
			if (method == null || method.ReturnType != typeof(bool))
			{
				return null;
			}
			MethodCall<object, object> shouldSerializeCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => (bool)shouldSerializeCall(o);
		}

		private void SetIsSpecifiedActions(JsonProperty property, MemberInfo member, bool allowNonPublicAccess)
		{
			MemberInfo memberInfo = member.DeclaringType.GetProperty(member.Name + "Specified");
			if (memberInfo == null)
			{
				memberInfo = member.DeclaringType.GetField(member.Name + "Specified");
			}
			if (!(memberInfo == null) && !(ReflectionUtils.GetMemberUnderlyingType(memberInfo) != typeof(bool)))
			{
				Func<object, object> specifiedPropertyGet = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(memberInfo);
				property.GetIsSpecified = (object o) => (bool)specifiedPropertyGet(o);
				if (ReflectionUtils.CanSetMemberValue(memberInfo, allowNonPublicAccess, canSetReadOnly: false))
				{
					property.SetIsSpecified = JsonTypeReflector.ReflectionDelegateFactory.CreateSet<object>(memberInfo);
				}
			}
		}

		protected internal virtual string ResolvePropertyName(string propertyName)
		{
			return propertyName;
		}

		public string GetResolvedPropertyName(string propertyName)
		{
			return ResolvePropertyName(propertyName);
		}
	}
	public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
	{
		public CamelCasePropertyNamesContractResolver()
			: base(shareCache: true)
		{
		}

		protected internal override string ResolvePropertyName(string propertyName)
		{
			return StringUtils.ToCamelCase(propertyName);
		}
	}
	internal struct ResolverContractKey : IEquatable<ResolverContractKey>
	{
		private readonly Type _resolverType;

		private readonly Type _contractType;

		public ResolverContractKey(Type resolverType, Type contractType)
		{
			_resolverType = resolverType;
			_contractType = contractType;
		}

		public override int GetHashCode()
		{
			return _resolverType.GetHashCode() ^ _contractType.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ResolverContractKey))
			{
				return false;
			}
			return Equals((ResolverContractKey)obj);
		}

		public bool Equals(ResolverContractKey other)
		{
			if (_resolverType == other._resolverType)
			{
				return _contractType == other._contractType;
			}
			return false;
		}
	}
	internal class DefaultContractResolverState
	{
		public Dictionary<ResolverContractKey, JsonContract> ContractCache;

		public PropertyNameTable NameTable = new PropertyNameTable();
	}
	public class DefaultSerializationBinder : SerializationBinder
	{
		internal struct TypeNameKey : IEquatable<TypeNameKey>
		{
			internal readonly string AssemblyName;

			internal readonly string TypeName;

			public TypeNameKey(string assemblyName, string typeName)
			{
				AssemblyName = assemblyName;
				TypeName = typeName;
			}

			public override int GetHashCode()
			{
				return ((AssemblyName != null) ? AssemblyName.GetHashCode() : 0) ^ ((TypeName != null) ? TypeName.GetHashCode() : 0);
			}

			public override bool Equals(object obj)
			{
				if (!(obj is TypeNameKey))
				{
					return false;
				}
				return Equals((TypeNameKey)obj);
			}

			public bool Equals(TypeNameKey other)
			{
				if (AssemblyName == other.AssemblyName)
				{
					return TypeName == other.TypeName;
				}
				return false;
			}
		}

		internal static readonly DefaultSerializationBinder Instance = new DefaultSerializationBinder();

		private readonly ThreadSafeStore<TypeNameKey, Type> _typeCache = new ThreadSafeStore<TypeNameKey, Type>(GetTypeFromTypeNameKey);

		private static Type GetTypeFromTypeNameKey(TypeNameKey typeNameKey)
		{
			string assemblyName = typeNameKey.AssemblyName;
			string typeName = typeNameKey.TypeName;
			if (assemblyName != null)
			{
				Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
				if (assembly == null)
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					Assembly[] array = assemblies;
					foreach (Assembly assembly2 in array)
					{
						if (assembly2.FullName == assemblyName)
						{
							assembly = assembly2;
							break;
						}
					}
				}
				if (assembly == null)
				{
					throw new JsonSerializationException("Could not load assembly '{0}'.".FormatWith(CultureInfo.InvariantCulture, assemblyName));
				}
				Type type = assembly.GetType(typeName);
				if (type == null)
				{
					throw new JsonSerializationException("Could not find type '{0}' in assembly '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeName, assembly.FullName));
				}
				return type;
			}
			return Type.GetType(typeName);
		}

		public override Type BindToType(string assemblyName, string typeName)
		{
			return _typeCache.Get(new TypeNameKey(assemblyName, typeName));
		}

		public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
		{
			assemblyName = serializedType.Assembly.FullName;
			typeName = serializedType.FullName;
		}
	}
	public class ErrorContext
	{
		internal bool Traced { get; set; }

		public Exception Error { get; private set; }

		public object OriginalObject { get; private set; }

		public object Member { get; private set; }

		public string Path { get; private set; }

		public bool Handled { get; set; }

		internal ErrorContext(object originalObject, object member, string path, Exception error)
		{
			OriginalObject = originalObject;
			Member = member;
			Error = error;
			Path = path;
		}
	}
	public class JsonArrayContract : JsonContainerContract
	{
		private readonly Type _genericCollectionDefinitionType;

		private Type _genericWrapperType;

		private ObjectConstructor<object> _genericWrapperCreator;

		private Func<object> _genericTemporaryCollectionCreator;

		private readonly ConstructorInfo _parametrizedConstructor;

		private ObjectConstructor<object> _parametrizedCreator;

		public Type CollectionItemType { get; private set; }

		public bool IsMultidimensionalArray { get; private set; }

		internal bool IsArray { get; private set; }

		internal bool ShouldCreateWrapper { get; private set; }

		internal bool CanDeserialize { get; private set; }

		internal ObjectConstructor<object> ParametrizedCreator
		{
			get
			{
				if (_parametrizedCreator == null)
				{
					_parametrizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(_parametrizedConstructor);
				}
				return _parametrizedCreator;
			}
		}

		internal bool HasParametrizedCreator
		{
			get
			{
				if (_parametrizedCreator == null)
				{
					return _parametrizedConstructor != null;
				}
				return true;
			}
		}

		public JsonArrayContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Array;
			IsArray = base.CreatedType.IsArray;
			bool canDeserialize;
			Type implementingType;
			if (IsArray)
			{
				CollectionItemType = ReflectionUtils.GetCollectionItemType(base.UnderlyingType);
				IsReadOnlyOrFixedSize = true;
				_genericCollectionDefinitionType = typeof(List<>).MakeGenericType(CollectionItemType);
				canDeserialize = true;
				IsMultidimensionalArray = IsArray && base.UnderlyingType.GetArrayRank() > 1;
			}
			else if (typeof(IList).IsAssignableFrom(underlyingType))
			{
				if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out _genericCollectionDefinitionType))
				{
					CollectionItemType = _genericCollectionDefinitionType.GetGenericArguments()[0];
				}
				else
				{
					CollectionItemType = ReflectionUtils.GetCollectionItemType(underlyingType);
				}
				if (underlyingType == typeof(IList))
				{
					base.CreatedType = typeof(List<object>);
				}
				if (CollectionItemType != null)
				{
					_parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, CollectionItemType);
				}
				IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof(ReadOnlyCollection<>));
				canDeserialize = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out _genericCollectionDefinitionType))
			{
				CollectionItemType = _genericCollectionDefinitionType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ICollection<>)) || ReflectionUtils.IsGenericDefinition(underlyingType, typeof(IList<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(CollectionItemType);
				}
				if (ReflectionUtils.IsGenericDefinition(underlyingType, typeof(ISet<>)))
				{
					base.CreatedType = typeof(HashSet<>).MakeGenericType(CollectionItemType);
				}
				_parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, CollectionItemType);
				canDeserialize = true;
				ShouldCreateWrapper = true;
			}
			else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IEnumerable<>), out implementingType))
			{
				CollectionItemType = implementingType.GetGenericArguments()[0];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IEnumerable<>)))
				{
					base.CreatedType = typeof(List<>).MakeGenericType(CollectionItemType);
				}
				_parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(underlyingType, CollectionItemType);
				if (!HasParametrizedCreator && underlyingType.Name == "FSharpList`1")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					_parametrizedCreator = FSharpUtils.CreateSeq(CollectionItemType);
				}
				if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
				{
					_genericCollectionDefinitionType = implementingType;
					IsReadOnlyOrFixedSize = false;
					ShouldCreateWrapper = false;
					canDeserialize = true;
				}
				else
				{
					_genericCollectionDefinitionType = typeof(List<>).MakeGenericType(CollectionItemType);
					IsReadOnlyOrFixedSize = true;
					ShouldCreateWrapper = true;
					canDeserialize = HasParametrizedCreator;
				}
			}
			else
			{
				canDeserialize = false;
				ShouldCreateWrapper = true;
			}
			CanDeserialize = canDeserialize;
		}

		internal IWrappedCollection CreateWrapper(object list)
		{
			if (_genericWrapperCreator == null)
			{
				_genericWrapperType = typeof(CollectionWrapper<>).MakeGenericType(CollectionItemType);
				Type type = ((!ReflectionUtils.InheritsGenericDefinition(_genericCollectionDefinitionType, typeof(List<>)) && !(_genericCollectionDefinitionType.GetGenericTypeDefinition() == typeof(IEnumerable<>))) ? _genericCollectionDefinitionType : typeof(ICollection<>).MakeGenericType(CollectionItemType));
				ConstructorInfo constructor = _genericWrapperType.GetConstructor(new Type[1] { type });
				_genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			}
			return (IWrappedCollection)_genericWrapperCreator(list);
		}

		internal IList CreateTemporaryCollection()
		{
			if (_genericTemporaryCollectionCreator == null)
			{
				Type type = (IsMultidimensionalArray ? typeof(object) : CollectionItemType);
				Type type2 = typeof(List<>).MakeGenericType(type);
				_genericTemporaryCollectionCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type2);
			}
			return (IList)_genericTemporaryCollectionCreator();
		}
	}
	internal enum JsonContractType
	{
		None,
		Object,
		Array,
		Primitive,
		String,
		Dictionary,
		Dynamic,
		Serializable,
		Linq
	}
	public delegate void SerializationCallback(object o, StreamingContext context);
	public delegate void SerializationErrorCallback(object o, StreamingContext context, ErrorContext errorContext);
	public delegate void ExtensionDataSetter(object o, string key, object value);
	public delegate IEnumerable<KeyValuePair<object, object>> ExtensionDataGetter(object o);
	public class JsonDictionaryContract : JsonContainerContract
	{
		private readonly Type _genericCollectionDefinitionType;

		private Type _genericWrapperType;

		private ObjectConstructor<object> _genericWrapperCreator;

		private Func<object> _genericTemporaryDictionaryCreator;

		private readonly ConstructorInfo _parametrizedConstructor;

		private ObjectConstructor<object> _parametrizedCreator;

		public Func<string, string> PropertyNameResolver { get; set; }

		public Type DictionaryKeyType { get; private set; }

		public Type DictionaryValueType { get; private set; }

		internal JsonContract KeyContract { get; set; }

		internal bool ShouldCreateWrapper { get; private set; }

		internal ObjectConstructor<object> ParametrizedCreator
		{
			get
			{
				if (_parametrizedCreator == null)
				{
					_parametrizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(_parametrizedConstructor);
				}
				return _parametrizedCreator;
			}
		}

		internal bool HasParametrizedCreator
		{
			get
			{
				if (_parametrizedCreator == null)
				{
					return _parametrizedConstructor != null;
				}
				return true;
			}
		}

		public JsonDictionaryContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Dictionary;
			Type keyType;
			Type valueType;
			if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(IDictionary<, >), out _genericCollectionDefinitionType))
			{
				keyType = _genericCollectionDefinitionType.GetGenericArguments()[0];
				valueType = _genericCollectionDefinitionType.GetGenericArguments()[1];
				if (ReflectionUtils.IsGenericDefinition(base.UnderlyingType, typeof(IDictionary<, >)))
				{
					base.CreatedType = typeof(Dictionary<, >).MakeGenericType(keyType, valueType);
				}
			}
			else
			{
				ReflectionUtils.GetDictionaryKeyValueTypes(base.UnderlyingType, out keyType, out valueType);
				if (base.UnderlyingType == typeof(IDictionary))
				{
					base.CreatedType = typeof(Dictionary<object, object>);
				}
			}
			if (keyType != null && valueType != null)
			{
				_parametrizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(base.CreatedType, typeof(KeyValuePair<, >).MakeGenericType(keyType, valueType));
				if (!HasParametrizedCreator && underlyingType.Name == "FSharpMap`2")
				{
					FSharpUtils.EnsureInitialized(underlyingType.Assembly());
					_parametrizedCreator = FSharpUtils.CreateMap(keyType, valueType);
				}
			}
			ShouldCreateWrapper = !typeof(IDictionary).IsAssignableFrom(base.CreatedType);
			DictionaryKeyType = keyType;
			DictionaryValueType = valueType;
		}

		internal IWrappedDictionary CreateWrapper(object dictionary)
		{
			if (_genericWrapperCreator == null)
			{
				_genericWrapperType = typeof(DictionaryWrapper<, >).MakeGenericType(DictionaryKeyType, DictionaryValueType);
				ConstructorInfo constructor = _genericWrapperType.GetConstructor(new Type[1] { _genericCollectionDefinitionType });
				_genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			}
			return (IWrappedDictionary)_genericWrapperCreator(dictionary);
		}

		internal IDictionary CreateTemporaryDictionary()
		{
			if (_genericTemporaryDictionaryCreator == null)
			{
				Type type = typeof(Dictionary<, >).MakeGenericType(DictionaryKeyType, DictionaryValueType);
				_genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type);
			}
			return (IDictionary)_genericTemporaryDictionaryCreator();
		}
	}
	public class JsonProperty
	{
		internal Required? _required;

		internal bool _hasExplicitDefaultValue;

		private object _defaultValue;

		private bool _hasGeneratedDefaultValue;

		private string _propertyName;

		internal bool _skipPropertyNameEscape;

		private Type _propertyType;

		internal JsonContract PropertyContract { get; set; }

		public string PropertyName
		{
			get
			{
				return _propertyName;
			}
			set
			{
				_propertyName = value;
				_skipPropertyNameEscape = !JavaScriptUtils.ShouldEscapeJavaScriptString(_propertyName, JavaScriptUtils.HtmlCharEscapeFlags);
			}
		}

		public Type DeclaringType { get; set; }

		public int? Order { get; set; }

		public string UnderlyingName { get; set; }

		public IValueProvider ValueProvider { get; set; }

		public IAttributeProvider AttributeProvider { get; set; }

		public Type PropertyType
		{
			get
			{
				return _propertyType;
			}
			set
			{
				if (_propertyType != value)
				{
					_propertyType = value;
					_hasGeneratedDefaultValue = false;
				}
			}
		}

		public JsonConverter Converter { get; set; }

		public JsonConverter MemberConverter { get; set; }

		public bool Ignored { get; set; }

		public bool Readable { get; set; }

		public bool Writable { get; set; }

		public bool HasMemberAttribute { get; set; }

		public object DefaultValue
		{
			get
			{
				if (!_hasExplicitDefaultValue)
				{
					return null;
				}
				return _defaultValue;
			}
			set
			{
				_hasExplicitDefaultValue = true;
				_defaultValue = value;
			}
		}

		public Required Required
		{
			get
			{
				return _required ?? Required.Default;
			}
			set
			{
				_required = value;
			}
		}

		public bool? IsReference { get; set; }

		public NullValueHandling? NullValueHandling { get; set; }

		public DefaultValueHandling? DefaultValueHandling { get; set; }

		public ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		public ObjectCreationHandling? ObjectCreationHandling { get; set; }

		public TypeNameHandling? TypeNameHandling { get; set; }

		public Predicate<object> ShouldSerialize { get; set; }

		public Predicate<object> GetIsSpecified { get; set; }

		public Action<object, object> SetIsSpecified { get; set; }

		public JsonConverter ItemConverter { get; set; }

		public bool? ItemIsReference { get; set; }

		public TypeNameHandling? ItemTypeNameHandling { get; set; }

		public ReferenceLoopHandling? ItemReferenceLoopHandling { get; set; }

		internal object GetResolvedDefaultValue()
		{
			if (_propertyType == null)
			{
				return null;
			}
			if (!_hasExplicitDefaultValue && !_hasGeneratedDefaultValue)
			{
				_defaultValue = ReflectionUtils.GetDefaultValue(PropertyType);
				_hasGeneratedDefaultValue = true;
			}
			return _defaultValue;
		}

		public override string ToString()
		{
			return PropertyName;
		}

		internal void WritePropertyName(JsonWriter writer)
		{
			if (_skipPropertyNameEscape)
			{
				writer.WritePropertyName(PropertyName, escape: false);
			}
			else
			{
				writer.WritePropertyName(PropertyName);
			}
		}
	}
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		private readonly Type _type;

		public JsonPropertyCollection(Type type)
			: base((IEqualityComparer<string>?)StringComparer.Ordinal)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			_type = type;
		}

		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		public void AddProperty(JsonProperty property)
		{
			if (Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base[property.PropertyName];
				bool flag = true;
				if (jsonProperty.Ignored)
				{
					Remove(jsonProperty);
					flag = false;
				}
				else if (property.DeclaringType != null && jsonProperty.DeclaringType != null)
				{
					if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType))
					{
						Remove(jsonProperty);
						flag = false;
					}
					if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType))
					{
						return;
					}
				}
				if (flag)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, _type));
				}
			}
			Add(property);
		}

		public JsonProperty GetClosestMatchProperty(string propertyName)
		{
			JsonProperty property = GetProperty(propertyName, StringComparison.Ordinal);
			if (property == null)
			{
				property = GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
			}
			return property;
		}

		private new bool TryGetValue(string key, out JsonProperty item)
		{
			if (base.Dictionary == null)
			{
				item = null;
				return false;
			}
			return base.Dictionary.TryGetValue(key, out item);
		}

		public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
		{
			if (comparisonType == StringComparison.Ordinal)
			{
				if (TryGetValue(propertyName, out var item))
				{
					return item;
				}
				return null;
			}
			using (IEnumerator<JsonProperty> enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JsonProperty current = enumerator.Current;
					if (string.Equals(propertyName, current.PropertyName, comparisonType))
					{
						return current;
					}
				}
			}
			return null;
		}
	}
}
namespace Newtonsoft.Json
{
	public enum MissingMemberHandling
	{
		Ignore,
		Error
	}
	public enum NullValueHandling
	{
		Include,
		Ignore
	}
	public enum ReferenceLoopHandling
	{
		Error,
		Ignore,
		Serialize
	}
}
namespace Newtonsoft.Json.Schema
{
	public class JsonSchema
	{
		private readonly string _internalId = Guid.NewGuid().ToString("N");

		public string Id { get; set; }

		public string Title { get; set; }

		public bool? Required { get; set; }

		public bool? ReadOnly { get; set; }

		public bool? Hidden { get; set; }

		public bool? Transient { get; set; }

		public string Description { get; set; }

		public JsonSchemaType? Type { get; set; }

		public string Pattern { get; set; }

		public int? MinimumLength { get; set; }

		public int? MaximumLength { get; set; }

		public double? DivisibleBy { get; set; }

		public double? Minimum { get; set; }

		public double? Maximum { get; set; }

		public bool? ExclusiveMinimum { get; set; }

		public bool? ExclusiveMaximum { get; set; }

		public int? MinimumItems { get; set; }

		public int? MaximumItems { get; set; }

		public IList<JsonSchema> Items { get; set; }

		public bool PositionalItemsValidation { get; set; }

		public JsonSchema AdditionalItems { get; set; }

		public bool AllowAdditionalItems { get; set; }

		public bool UniqueItems { get; set; }

		public IDictionary<string, JsonSchema> Properties { get; set; }

		public JsonSchema AdditionalProperties { get; set; }

		public IDictionary<string, JsonSchema> PatternProperties { get; set; }

		public bool AllowAdditionalProperties { get; set; }

		public string Requires { get; set; }

		public IList<JToken> Enum { get; set; }

		public JsonSchemaType? Disallow { get; set; }

		public JToken Default { get; set; }

		public IList<JsonSchema> Extends { get; set; }

		public string Format { get; set; }

		internal string Location { get; set; }

		internal string InternalId => _internalId;

		internal string DeferredReference { get; set; }

		internal bool ReferencesResolved { get; set; }

		public JsonSchema()
		{
			AllowAdditionalProperties = true;
			AllowAdditionalItems = true;
		}

		public static JsonSchema Read(JsonReader reader)
		{
			return Read(reader, new JsonSchemaResolver());
		}

		public static JsonSchema Read(JsonReader reader, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaBuilder jsonSchemaBuilder = new JsonSchemaBuilder(resolver);
			return jsonSchemaBuilder.Read(reader);
		}

		public static JsonSchema Parse(string json)
		{
			return Parse(json, new JsonSchemaResolver());
		}

		public static JsonSchema Parse(string json, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(json, "json");
			using JsonReader reader = new JsonTextReader(new StringReader(json));
			return Read(reader, resolver);
		}

		public void WriteTo(JsonWriter writer)
		{
			WriteTo(writer, new JsonSchemaResolver());
		}

		public void WriteTo(JsonWriter writer, JsonSchemaResolver resolver)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			JsonSchemaWriter jsonSchemaWriter = new JsonSchemaWriter(writer, resolver);
			jsonSchemaWriter.WriteSchema(this);
		}

		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter);
			jsonTextWriter.Formatting = Formatting.Indented;
			WriteTo(jsonTextWriter);
			return stringWriter.ToString();
		}
	}
	internal class JsonSchemaBuilder
	{
		private readonly IList<JsonSchema> _stack;

		private readonly JsonSchemaResolver _resolver;

		private readonly IDictionary<string, JsonSchema> _documentSchemas;

		private JsonSchema _currentSchema;

		private JObject _rootSchema;

		private JsonSchema CurrentSchema => _currentSchema;

		public JsonSchemaBuilder(JsonSchemaResolver resolver)
		{
			_stack = new List<JsonSchema>();
			_documentSchemas = new Dictionary<string, JsonSchema>();
			_resolver = resolver;
		}

		private void Push(JsonSchema value)
		{
			_currentSchema = value;
			_stack.Add(value);
			_resolver.LoadedSchemas.Add(value);
			_documentSchemas.Add(value.Location, value);
		}

		private JsonSchema Pop()
		{
			JsonSchema currentSchema = _currentSchema;
			_stack.RemoveAt(_stack.Count - 1);
			_currentSchema = _stack.LastOrDefault();
			return currentSchema;
		}

		internal JsonSchema Read(JsonReader reader)
		{
			JToken jToken = JToken.ReadFrom(reader);
			_rootSchema = jToken as JObject;
			JsonSchema jsonSchema = BuildSchema(jToken);
			ResolveReferences(jsonSchema);
			return jsonSchema;
		}

		private string UnescapeReference(string reference)
		{
			return Uri.UnescapeDataString(reference).Replace("~1", "/").Replace("~0", "~");
		}

		private JsonSchema ResolveReferences(JsonSchema schema)
		{
			if (schema.DeferredReference != null)
			{
				string text = schema.DeferredReference;
				bool flag = text.StartsWith("#", StringComparison.Ordinal);
				if (flag)
				{
					text = UnescapeReference(text);
				}
				JsonSchema jsonSchema = _resolver.GetSchema(text);
				if (jsonSchema == null)
				{
					if (flag)
					{
						string[] array = schema.DeferredReference.TrimStart(new char[1] { '#' }).Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
						JToken jToken = _rootSchema;
						string[] array2 = array;
						foreach (string reference in array2)
						{
							string text2 = UnescapeReference(reference);
							if (jToken.Type == JTokenType.Object)
							{
								jToken = jToken[text2];
							}
							else if (jToken.Type == JTokenType.Array || jToken.Type == JTokenType.Constructor)
							{
								jToken = ((!int.TryParse(text2, out var result) || result < 0 || result >= jToken.Count()) ? null : jToken[result]);
							}
							if (jToken == null)
							{
								break;
							}
						}
						if (jToken != null)
						{
							jsonSchema = BuildSchema(jToken);
						}
					}
					if (jsonSchema == null)
					{
						throw new JsonException("Could not resolve schema reference '{0}'.".FormatWith(CultureInfo.InvariantCulture, schema.DeferredReference));
					}
				}
				schema = jsonSchema;
			}
			if (schema.ReferencesResolved)
			{
				return schema;
			}
			schema.ReferencesResolved = true;
			if (schema.Extends != null)
			{
				for (int j = 0; j < schema.Extends.Count; j++)
				{
					schema.Extends[j] = ResolveReferences(schema.Extends[j]);
				}
			}
			if (schema.Items != null)
			{
				for (int k = 0; k < schema.Items.Count; k++)
				{
					schema.Items[k] = ResolveReferences(schema.Items[k]);
				}
			}
			if (schema.AdditionalItems != null)
			{
				schema.AdditionalItems = ResolveReferences(schema.AdditionalItems);
			}
			if (schema.PatternProperties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> item in schema.PatternProperties.ToList())
				{
					schema.PatternProperties[item.Key] = ResolveReferences(item.Value);
				}
			}
			if (schema.Properties != null)
			{
				foreach (KeyValuePair<string, JsonSchema> item2 in schema.Properties.ToList())
				{
					schema.Properties[item2.Key] = ResolveReferences(item2.Value);
				}
			}
			if (schema.AdditionalProperties != null)
			{
				schema.AdditionalProperties = ResolveReferences(schema.AdditionalProperties);
			}
			return schema;
		}

		private JsonSchema BuildSchema(JToken token)
		{
			if (!(token is JObject jObject))
			{
				throw JsonException.Create(token, token.Path, "Expected object while parsing schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			if (jObject.TryGetValue("$ref", out var value))
			{
				JsonSchema jsonSchema = new JsonSchema();
				jsonSchema.DeferredReference = (string)value;
				return jsonSchema;
			}
			string text = token.Path.Replace(".", "/").Replace("[", "/").Replace("]", string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				text = "/" + text;
			}
			text = "#" + text;
			if (_documentSchemas.TryGetValue(text, out var value2))
			{
				return value2;
			}
			Push(new JsonSchema
			{
				Location = text
			});
			ProcessSchemaProperties(jObject);
			return Pop();
		}

		private void ProcessSchemaProperties(JObject schemaObject)
		{
			foreach (KeyValuePair<string, JToken> item in schemaObject)
			{
				switch (item.Key)
				{
				case "type":
					CurrentSchema.Type = ProcessType(item.Value);
					break;
				case "id":
					CurrentSchema.Id = (string)item.Value;
					break;
				case "title":
					CurrentSchema.Title = (string)item.Value;
					break;
				case "description":
					CurrentSchema.Description = (string)item.Value;
					break;
				case "properties":
					CurrentSchema.Properties = ProcessProperties(item.Value);
					break;
				case "items":
					ProcessItems(item.Value);
					break;
				case "additionalProperties":
					ProcessAdditionalProperties(item.Value);
					break;
				case "additionalItems":
					ProcessAdditionalItems(item.Value);
					break;
				case "patternProperties":
					CurrentSchema.PatternProperties = ProcessProperties(item.Value);
					break;
				case "required":
					CurrentSchema.Required = (bool)item.Value;
					break;
				case "requires":
					CurrentSchema.Requires = (string)item.Value;
					break;
				case "minimum":
					CurrentSchema.Minimum = (double)item.Value;
					break;
				case "maximum":
					CurrentSchema.Maximum = (double)item.Value;
					break;
				case "exclusiveMinimum":
					CurrentSchema.ExclusiveMinimum = (bool)item.Value;
					break;
				case "exclusiveMaximum":
					CurrentSchema.ExclusiveMaximum = (bool)item.Value;
					break;
				case "maxLength":
					CurrentSchema.MaximumLength = (int)item.Value;
					break;
				case "minLength":
					CurrentSchema.MinimumLength = (int)item.Value;
					break;
				case "maxItems":
					CurrentSchema.MaximumItems = (int)item.Value;
					break;
				case "minItems":
					CurrentSchema.MinimumItems = (int)item.Value;
					break;
				case "divisibleBy":
					CurrentSchema.DivisibleBy = (double)item.Value;
					break;
				case "disallow":
					CurrentSchema.Disallow = ProcessType(item.Value);
					break;
				case "default":
					CurrentSchema.Default = item.Value.DeepClone();
					break;
				case "hidden":
					CurrentSchema.Hidden = (bool)item.Value;
					break;
				case "readonly":
					CurrentSchema.ReadOnly = (bool)item.Value;
					break;
				case "format":
					CurrentSchema.Format = (string)item.Value;
					break;
				case "pattern":
					CurrentSchema.Pattern = (string)item.Value;
					break;
				case "enum":
					ProcessEnum(item.Value);
					break;
				case "extends":
					ProcessExtends(item.Value);
					break;
				case "uniqueItems":
					CurrentSchema.UniqueItems = (bool)item.Value;
					break;
				}
			}
		}

		private void ProcessExtends(JToken token)
		{
			IList<JsonSchema> list = new List<JsonSchema>();
			if (token.Type == JTokenType.Array)
			{
				foreach (JToken item in (IEnumerable<JToken>)token)
				{
					list.Add(BuildSchema(item));
				}
			}
			else
			{
				JsonSchema jsonSchema = BuildSchema(token);
				if (jsonSchema != null)
				{
					list.Add(jsonSchema);
				}
			}
			if (list.Count > 0)
			{
				CurrentSchema.Extends = list;
			}
		}

		private void ProcessEnum(JToken token)
		{
			if (token.Type != JTokenType.Array)
			{
				throw JsonException.Create(token, token.Path, "Expected Array token while parsing enum values, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			CurrentSchema.Enum = new List<JToken>();
			foreach (JToken item in (IEnumerable<JToken>)token)
			{
				CurrentSchema.Enum.Add(item.DeepClone());
			}
		}

		private void ProcessAdditionalProperties(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				CurrentSchema.AllowAdditionalProperties = (bool)token;
			}
			else
			{
				CurrentSchema.AdditionalProperties = BuildSchema(token);
			}
		}

		private void ProcessAdditionalItems(JToken token)
		{
			if (token.Type == JTokenType.Boolean)
			{
				CurrentSchema.AllowAdditionalItems = (bool)token;
			}
			else
			{
				CurrentSchema.AdditionalItems = BuildSchema(token);
			}
		}

		private IDictionary<string, JsonSchema> ProcessProperties(JToken token)
		{
			IDictionary<string, JsonSchema> dictionary = new Dictionary<string, JsonSchema>();
			if (token.Type != JTokenType.Object)
			{
				throw JsonException.Create(token, token.Path, "Expected Object token while parsing schema properties, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
			foreach (JProperty item in (IEnumerable<JToken>)token)
			{
				if (dictionary.ContainsKey(item.Name))
				{
					throw new JsonException("Property {0} has already been defined in schema.".FormatWith(CultureInfo.InvariantCulture, item.Name));
				}
				dictionary.Add(item.Name, BuildSchema(item.Value));
			}
			return dictionary;
		}

		private void ProcessItems(JToken token)
		{
			CurrentSchema.Items = new List<JsonSchema>();
			switch (token.Type)
			{
			case JTokenType.Object:
				CurrentSchema.Items.Add(BuildSchema(token));
				CurrentSchema.PositionalItemsValidation = false;
				break;
			case JTokenType.Array:
				CurrentSchema.PositionalItemsValidation = true;
				{
					foreach (JToken item in (IEnumerable<JToken>)token)
					{
						CurrentSchema.Items.Add(BuildSchema(item));
					}
					break;
				}
			default:
				throw JsonException.Create(token, token.Path, "Expected array or JSON schema object, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
		}

		private JsonSchemaType? ProcessType(JToken token)
		{
			switch (token.Type)
			{
			case JTokenType.Array:
			{
				JsonSchemaType? jsonSchemaType = JsonSchemaType.None;
				{
					foreach (JToken item in (IEnumerable<JToken>)token)
					{
						if (item.Type != JTokenType.String)
						{
							throw JsonException.Create(item, item.Path, "Exception JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
						}
						jsonSchemaType |= MapType((string)item);
					}
					return jsonSchemaType;
				}
			}
			case JTokenType.String:
				return MapType((string)token);
			default:
				throw JsonException.Create(token, token.Path, "Expected array or JSON schema type string token, got {0}.".FormatWith(CultureInfo.InvariantCulture, token.Type));
			}
		}

		internal static JsonSchemaType MapType(string type)
		{
			if (!JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out var value))
			{
				throw new JsonException("Invalid JSON schema type: {0}".FormatWith(CultureInfo.InvariantCulture, type));
			}
			return value;
		}

		internal static string MapType(JsonSchemaType type)
		{
			return JsonSchemaConstants.JsonSchemaTypeMapping.Single((KeyValuePair<string, JsonSchemaType> kv) => kv.Value == type).Key;
		}
	}
	internal static class JsonSchemaConstants
	{
		public const string TypePropertyName = "type";

		public const string PropertiesPropertyName = "properties";

		public const string ItemsPropertyName = "items";

		public const string AdditionalItemsPropertyName = "additionalItems";

		public const string RequiredPropertyName = "required";

		public const string PatternPropertiesPropertyName = "patternProperties";

		public const string AdditionalPropertiesPropertyName = "additionalProperties";

		public const string RequiresPropertyName = "requires";

		public const string MinimumPropertyName = "minimum";

		public const string MaximumPropertyName = "maximum";

		public const string ExclusiveMinimumPropertyName = "exclusiveMinimum";

		public const string ExclusiveMaximumPropertyName = "exclusiveMaximum";

		public const string MinimumItemsPropertyName = "minItems";

		public const string MaximumItemsPropertyName = "maxItems";

		public const string PatternPropertyName = "pattern";

		public const string MaximumLengthPropertyName = "maxLength";

		public const string MinimumLengthPropertyName = "minLength";

		public const string EnumPropertyName = "enum";

		public const string ReadOnlyPropertyName = "readonly";

		public const string TitlePropertyName = "title";

		public const string DescriptionPropertyName = "description";

		public const string FormatPropertyName = "format";

		public const string DefaultPropertyName = "default";

		public const string TransientPropertyName = "transient";

		public const string DivisibleByPropertyName = "divisibleBy";

		public const string HiddenPropertyName = "hidden";

		public const string DisallowPropertyName = "disallow";

		public const string ExtendsPropertyName = "extends";

		public const string IdPropertyName = "id";

		public const string UniqueItemsPropertyName = "uniqueItems";

		public const string OptionValuePropertyName = "value";

		public const string OptionLabelPropertyName = "label";

		public static readonly IDictionary<string, JsonSchemaType> JsonSchemaTypeMapping = new Dictionary<string, JsonSchemaType>
		{
			{
				"string",
				JsonSchemaType.String
			},
			{
				"object",
				JsonSchemaType.Object
			},
			{
				"integer",
				JsonSchemaType.Integer
			},
			{
				"number",
				JsonSchemaType.Float
			},
			{
				"null",
				JsonSchemaType.Null
			},
			{
				"boolean",
				JsonSchemaType.Boolean
			},
			{
				"array",
				JsonSchemaType.Array
			},
			{
				"any",
				JsonSchemaType.Any
			}
		};
	}
	public class JsonSchemaGenerator
	{
		private class TypeSchema
		{
			public Type Type { get; private set; }

			public JsonSchema Schema { get; private set; }

			public TypeSchema(Type type, JsonSchema schema)
			{
				ValidationUtils.ArgumentNotNull(type, "type");
				ValidationUtils.ArgumentNotNull(schema, "schema");
				Type = type;
				Schema = schema;
			}
		}

		private IContractResolver _contractResolver;

		private JsonSchemaResolver _resolver;

		private readonly IList<TypeSchema> _stack = new List<TypeSchema>();

		private JsonSchema _currentSchema;

		public UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

		public IContractResolver ContractResolver
		{
			get
			{
				if (_contractResolver == null)
				{
					return DefaultContractResolver.Instance;
				}
				return _contractResolver;
			}
			set
			{
				_contractResolver = value;
			}
		}

		private JsonSchema CurrentSchema => _currentSchema;

		private void Push(TypeSchema typeSchema)
		{
			_currentSchema = typeSchema.Schema;
			_stack.Add(typeSchema);
			_resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		private TypeSchema Pop()
		{
			TypeSchema result = _stack[_stack.Count - 1];
			_stack.RemoveAt(_stack.Count - 1);
			TypeSchema typeSchema = _stack.LastOrDefault();
			if (typeSchema != null)
			{
				_currentSchema = typeSchema.Schema;
			}
			else
			{
				_currentSchema = null;
			}
			return result;
		}

		public JsonSchema Generate(Type type)
		{
			return Generate(type, new JsonSchemaResolver(), rootSchemaNullable: false);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver)
		{
			return Generate(type, resolver, rootSchemaNullable: false);
		}

		public JsonSchema Generate(Type type, bool rootSchemaNullable)
		{
			return Generate(type, new JsonSchemaResolver(), rootSchemaNullable);
		}

		public JsonSchema Generate(Type type, JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(resolver, "resolver");
			_resolver = resolver;
			return GenerateInternal(type, (!rootSchemaNullable) ? Required.Always : Required.Default, required: false);
		}

		private string GetTitle(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Title))
			{
				return cachedAttribute.Title;
			}
			return null;
		}

		private string GetDescription(Type type)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Description))
			{
				return cachedAttribute.Description;
			}
			return ReflectionUtils.GetAttribute<DescriptionAttribute>(type)?.Description;
		}

		private string GetTypeId(Type type, bool explicitOnly)
		{
			JsonContainerAttribute cachedAttribute = JsonTypeReflector.GetCachedAttribute<JsonContainerAttribute>(type);
			if (cachedAttribute != null && !string.IsNullOrEmpty(cachedAttribute.Id))
			{
				return cachedAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			return UndefinedSchemaIdHandling switch
			{
				UndefinedSchemaIdHandling.UseTypeName => type.FullName, 
				UndefinedSchemaIdHandling.UseAssemblyQualifiedName => type.AssemblyQualifiedName, 
				_ => null, 
			};
		}

		private JsonSchema GenerateInternal(Type type, Required valueRequired, bool required)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = GetTypeId(type, explicitOnly: false);
			string typeId2 = GetTypeId(type, explicitOnly: true);
			if (!string.IsNullOrEmpty(typeId))
			{
				JsonSchema schema = _resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != Required.Always && !HasFlag(schema.Type, JsonSchemaType.Null))
					{
						schema.Type |= JsonSchemaType.Null;
					}
					if (required && schema.Required != true)
					{
						schema.Required = true;
					}
					return schema;
				}
			}
			if (_stack.Any((TypeSchema tc) => tc.Type == type))
			{
				throw new JsonException("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			JsonContract jsonContract = ContractResolver.ResolveContract(type);
			JsonConverter jsonConverter;
			if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
			{
				JsonSchema schema2 = jsonConverter.GetSchema();
				if (schema2 != null)
				{
					return schema2;
				}
			}
			Push(new TypeSchema(type, new JsonSchema()));
			if (typeId2 != null)
			{
				CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				CurrentSchema.Required = true;
			}
			CurrentSchema.Title = GetTitle(type);
			CurrentSchema.Description = GetDescription(type);
			if (jsonConverter != null)
			{
				CurrentSchema.Type = JsonSchemaType.Any;
			}
			else
			{
				switch (jsonContract.ContractType)
				{
				case JsonContractType.Object:
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					GenerateObjectSchema(type, (JsonObjectContract)jsonContract);
					break;
				case JsonContractType.Array:
				{
					CurrentSchema.Type = AddNullType(JsonSchemaType.Array, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					bool flag = JsonTypeReflector.GetCachedAttribute<JsonArrayAttribute>(type)?.AllowNullItems ?? true;
					Type collectionItemType = ReflectionUtils.GetCollectionItemType(type);
					if (collectionItemType != null)
					{
						CurrentSchema.Items = new List<JsonSchema>();
						CurrentSchema.Items.Add(GenerateInternal(collectionItemType, (!flag) ? Required.Always : Required.Default, required: false));
					}
					break;
				}
				case JsonContractType.Primitive:
				{
					CurrentSchema.Type = GetJsonSchemaType(type, valueRequired);
					JsonSchemaType? type2 = CurrentSchema.Type;
					if (type2.GetValueOrDefault() != JsonSchemaType.Integer || !type2.HasValue || !type.IsEnum() || type.IsDefined(typeof(FlagsAttribute), inherit: true))
					{
						break;
					}
					CurrentSchema.Enum = new List<JToken>();
					IList<EnumValue<long>> namesAndValues = EnumUtils.GetNamesAndValues<long>(type);
					foreach (EnumValue<long> item2 in namesAndValues)
					{
						JToken item = JToken.FromObject(item2.Value);
						CurrentSchema.Enum.Add(item);
					}
					break;
				}
				case JsonContractType.String:
				{
					JsonSchemaType value = ((!ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? JsonSchemaType.String : AddNullType(JsonSchemaType.String, valueRequired));
					CurrentSchema.Type = value;
					break;
				}
				case JsonContractType.Dictionary:
				{
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					ReflectionUtils.GetDictionaryKeyValueTypes(type, out var keyType, out var valueType);
					if (keyType != null)
					{
						JsonContract jsonContract2 = ContractResolver.ResolveContract(keyType);
						if (jsonContract2.ContractType == JsonContractType.Primitive)
						{
							CurrentSchema.AdditionalProperties = GenerateInternal(valueType, Required.Default, required: false);
						}
					}
					break;
				}
				case JsonContractType.Serializable:
					CurrentSchema.Type = AddNullType(JsonSchemaType.Object, valueRequired);
					CurrentSchema.Id = GetTypeId(type, explicitOnly: false);
					GenerateISerializableContract(type, (JsonISerializableContract)jsonContract);
					break;
				case JsonContractType.Dynamic:
				case JsonContractType.Linq:
					CurrentSchema.Type = JsonSchemaType.Any;
					break;
				default:
					throw new JsonException("Unexpected contract type: {0}".FormatWith(CultureInfo.InvariantCulture, jsonContract));
				}
			}
			return Pop().Schema;
		}

		private JsonSchemaType AddNullType(JsonSchemaType type, Required valueRequired)
		{
			if (valueRequired != Required.Always)
			{
				return type | JsonSchemaType.Null;
			}
			return type;
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private void GenerateObjectSchema(Type type, JsonObjectContract contract)
		{
			CurrentSchema.Properties = new Dictionary<string, JsonSchema>();
			foreach (JsonProperty property in contract.Properties)
			{
				if (!property.Ignored)
				{
					bool flag = property.NullValueHandling == NullValueHandling.Ignore || HasFlag(property.DefaultValueHandling.GetValueOrDefault(), DefaultValueHandling.Ignore) || property.ShouldSerialize != null || property.GetIsSpecified != null;
					JsonSchema jsonSchema = GenerateInternal(property.PropertyType, property.Required, !flag);
					if (property.DefaultValue != null)
					{
						jsonSchema.Default = JToken.FromObject(property.DefaultValue);
					}
					CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
				}
			}
			if (type.IsSealed())
			{
				CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		private void GenerateISerializableContract(Type type, JsonISerializableContract contract)
		{
			CurrentSchema.AllowAdditionalProperties = true;
		}

		internal static bool HasFlag(JsonSchemaType? value, JsonSchemaType flag)
		{
			if (!value.HasValue)
			{
				return true;
			}
			if ((value & flag) == flag)
			{
				return true;
			}
			if (flag == JsonSchemaType.Integer && ((uint?)value & 2u) == 2)
			{
				return true;
			}
			return false;
		}

		private JsonSchemaType GetJsonSchemaType(Type type, Required valueRequired)
		{
			JsonSchemaType jsonSchemaType = JsonSchemaType.None;
			if (valueRequired != Required.Always && ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = JsonSchemaType.Null;
				if (ReflectionUtils.IsNullableType(type))
				{
					type = Nullable.GetUnderlyingType(type);
				}
			}
			PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(type);
			switch (typeCode)
			{
			case PrimitiveTypeCode.Empty:
			case PrimitiveTypeCode.Object:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.DBNull:
				return jsonSchemaType | JsonSchemaType.Null;
			case PrimitiveTypeCode.Boolean:
				return jsonSchemaType | JsonSchemaType.Boolean;
			case PrimitiveTypeCode.Char:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
			case PrimitiveTypeCode.BigInteger:
				return jsonSchemaType | JsonSchemaType.Integer;
			case PrimitiveTypeCode.Single:
			case PrimitiveTypeCode.Double:
			case PrimitiveTypeCode.Decimal:
				return jsonSchemaType | JsonSchemaType.Float;
			case PrimitiveTypeCode.DateTime:
			case PrimitiveTypeCode.DateTimeOffset:
				return jsonSchemaType | JsonSchemaType.String;
			case PrimitiveTypeCode.Guid:
			case PrimitiveTypeCode.TimeSpan:
			case PrimitiveTypeCode.Uri:
			case PrimitiveTypeCode.String:
			case PrimitiveTypeCode.Bytes:
				return jsonSchemaType | JsonSchemaType.String;
			default:
				throw new JsonException("Unexpected type code '{0}' for type '{1}'.".FormatWith(CultureInfo.InvariantCulture, typeCode, type));
			}
		}
	}
	[Flags]
	public enum JsonSchemaType
	{
		None = 0,
		String = 1,
		Float = 2,
		Integer = 4,
		Boolean = 8,
		Object = 0x10,
		Array = 0x20,
		Null = 0x40,
		Any = 0x7F
	}
}
namespace Newtonsoft.Json.Serialization
{
	public class JsonObjectContract : JsonContainerContract
	{
		private bool? _hasRequiredOrDefaultValueProperties;

		private ConstructorInfo _parametrizedConstructor;

		private ConstructorInfo _overrideConstructor;

		private ObjectConstructor<object> _overrideCreator;

		private ObjectConstructor<object> _parametrizedCreator;

		public MemberSerialization MemberSerialization { get; set; }

		public Required? ItemRequired { get; set; }

		public JsonPropertyCollection Properties { get; private set; }

		[Obsolete("ConstructorParameters is obsolete. Use CreatorParameters instead.")]
		public JsonPropertyCollection ConstructorParameters => CreatorParameters;

		public JsonPropertyCollection CreatorParameters { get; private set; }

		[Obsolete("OverrideConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo OverrideConstructor
		{
			get
			{
				return _overrideConstructor;
			}
			set
			{
				_overrideConstructor = value;
				_overrideCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		[Obsolete("ParametrizedConstructor is obsolete. Use OverrideCreator instead.")]
		public ConstructorInfo ParametrizedConstructor
		{
			get
			{
				return _parametrizedConstructor;
			}
			set
			{
				_parametrizedConstructor = value;
				_parametrizedCreator = ((value != null) ? JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(value) : null);
			}
		}

		public ObjectConstructor<object> OverrideCreator
		{
			get
			{
				return _overrideCreator;
			}
			set
			{
				_overrideCreator = value;
				_overrideConstructor = null;
			}
		}

		internal ObjectConstructor<object> ParametrizedCreator => _parametrizedCreator;

		public ExtensionDataSetter ExtensionDataSetter { get; set; }

		public ExtensionDataGetter ExtensionDataGetter { get; set; }

		internal bool HasRequiredOrDefaultValueProperties
		{
			get
			{
				if (!_hasRequiredOrDefaultValueProperties.HasValue)
				{
					_hasRequiredOrDefaultValueProperties = false;
					if (ItemRequired.GetValueOrDefault(Required.Default) != 0)
					{
						_hasRequiredOrDefaultValueProperties = true;
					}
					else
					{
						foreach (JsonProperty property in Properties)
						{
							if (property.Required == Required.Default)
							{
								DefaultValueHandling? defaultValueHandling = property.DefaultValueHandling & DefaultValueHandling.Populate;
								if (defaultValueHandling.GetValueOrDefault() != DefaultValueHandling.Populate || !defaultValueHandling.HasValue || !property.Writable)
								{
									continue;
								}
							}
							_hasRequiredOrDefaultValueProperties = true;
							break;
						}
					}
				}
				return _hasRequiredOrDefaultValueProperties.Value;
			}
		}

		public JsonObjectContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.Object;
			Properties = new JsonPropertyCollection(base.UnderlyingType);
			CreatorParameters = new JsonPropertyCollection(base.UnderlyingType);
		}

		[SecuritySafeCritical]
		internal object GetUninitializedObject()
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith(CultureInfo.InvariantCulture, NonNullableUnderlyingType));
			}
			return FormatterServices.GetUninitializedObject(NonNullableUnderlyingType);
		}
	}
	internal abstract class JsonSerializerInternalBase
	{
		private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
		{
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return object.ReferenceEquals(x, y);
			}

			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}

		private ErrorContext _currentErrorContext;

		private BidirectionalDictionary<string, object> _mappings;

		private bool _serializing;

		internal readonly JsonSerializer Serializer;

		internal readonly ITraceWriter TraceWriter;

		internal BidirectionalDictionary<string, object> DefaultReferenceMappings
		{
			get
			{
				if (_mappings == null)
				{
					_mappings = new BidirectionalDictionary<string, object>(EqualityComparer<string>.Default, new ReferenceEqualsEqualityComparer(), "A different value already has the Id '{0}'.", "A different Id has already been assigned for value '{0}'.");
				}
				return _mappings;
			}
		}

		protected JsonSerializerInternalBase(JsonSerializer serializer)
		{
			ValidationUtils.ArgumentNotNull(serializer, "serializer");
			Serializer = serializer;
			TraceWriter = serializer.TraceWriter;
			_serializing = GetType() == typeof(JsonSerializerInternalWriter);
		}

		private ErrorContext GetErrorContext(object currentObject, object member, string path, Exception error)
		{
			if (_currentErrorContext == null)
			{
				_currentErrorContext = new ErrorContext(currentObject, member, path, error);
			}
			if (_currentErrorContext.Error != error)
			{
				throw new InvalidOperationException("Current error context error is different to requested error.");
			}
			return _currentErrorContext;
		}

		protected void ClearErrorContext()
		{
			if (_currentErrorContext == null)
			{
				throw new InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			_currentErrorContext = null;
		}

		protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, IJsonLineInfo lineInfo, string path, Exception ex)
		{
			ErrorContext errorContext = GetErrorContext(currentObject, keyValue, path, ex);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Error && !errorContext.Traced)
			{
				errorContext.Traced = true;
				string text = (_serializing ? "Error serializing" : "Error deserializing");
				if (contract != null)
				{
					text = text + " " + contract.UnderlyingType;
				}
				text = text + ". " + ex.Message;
				if (!(ex is JsonException))
				{
					text = JsonPosition.FormatMessage(lineInfo, path, text);
				}
				TraceWriter.Trace(TraceLevel.Error, text, ex);
			}
			if (contract != null && currentObject != null)
			{
				contract.InvokeOnError(currentObject, Serializer.Context, errorContext);
			}
			if (!errorContext.Handled)
			{
				Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}
	}
	internal class JsonSerializerInternalReader : JsonSerializerInternalBase
	{
		internal enum PropertyPresence
		{
			None,
			Null,
			Value
		}

		private JsonSerializerProxy _internalSerializer;

		public JsonSerializerInternalReader(JsonSerializer serializer)
			: base(serializer)
		{
		}

		public void Populate(JsonReader reader, object target)
		{
			ValidationUtils.ArgumentNotNull(target, "target");
			Type type = target.GetType();
			JsonContract jsonContract = Serializer._contractResolver.ResolveContract(type);
			if (reader.TokenType == JsonToken.None)
			{
				reader.Read();
			}
			if (reader.TokenType == JsonToken.StartArray)
			{
				if (jsonContract.ContractType == JsonContractType.Array)
				{
					JsonArrayContract jsonArrayContract = (JsonArrayContract)jsonContract;
					PopulateList(jsonArrayContract.ShouldCreateWrapper ? jsonArrayContract.CreateWrapper(target) : ((IList)target), reader, jsonArrayContract, null, null);
					return;
				}
				throw JsonSerializationException.Create(reader, "Cannot populate JSON array onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			if (reader.TokenType == JsonToken.StartObject)
			{
				CheckedRead(reader);
				string id = null;
				if (Serializer.MetadataPropertyHandling != MetadataPropertyHandling.Ignore && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$id", StringComparison.Ordinal))
				{
					CheckedRead(reader);
					id = ((reader.Value != null) ? reader.Value.ToString() : null);
					CheckedRead(reader);
				}
				if (jsonContract.ContractType == JsonContractType.Dictionary)
				{
					JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)jsonContract;
					PopulateDictionary(jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(target) : ((IDictionary)target), reader, jsonDictionaryContract, null, id);
					return;
				}
				if (jsonContract.ContractType == JsonContractType.Object)
				{
					PopulateObject(target, reader, (JsonObjectContract)jsonContract, null, id);
					return;
				}
				throw JsonSerializationException.Create(reader, "Cannot populate JSON object onto type '{0}'.".FormatWith(CultureInfo.InvariantCulture, type));
			}
			throw JsonSerializationException.Create(reader, "Unexpected initial token '{0}' when populating object. Expected JSON object or array.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		private JsonContract GetContractSafe(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return Serializer._contractResolver.ResolveContract(type);
		}

		public object Deserialize(JsonReader reader, Type objectType, bool checkAdditionalContent)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			JsonContract contractSafe = GetContractSafe(objectType);
			try
			{
				JsonConverter converter = GetConverter(contractSafe, null, null, null);
				if (reader.TokenType == JsonToken.None && !ReadForType(reader, contractSafe, converter != null))
				{
					if (contractSafe != null && !contractSafe.IsNullable)
					{
						throw JsonSerializationException.Create(reader, "No JSON content found and type '{0}' is not nullable.".FormatWith(CultureInfo.InvariantCulture, contractSafe.UnderlyingType));
					}
					return null;
				}
				object result = ((converter == null || !converter.CanRead) ? CreateValueInternal(reader, objectType, contractSafe, null, null, null, null) : DeserializeConvertable(converter, reader, objectType, null));
				if (checkAdditionalContent && reader.Read() && reader.TokenType != JsonToken.Comment)
				{
					throw new JsonSerializationException("Additional text found in JSON string after finishing deserializing object.");
				}
				return result;
			}
			catch (Exception ex)
			{
				if (IsErrorHandled(null, contractSafe, null, reader as IJsonLineInfo, reader.Path, ex))
				{
					HandleError(reader, readPastError: false, 0);
					return null;
				}
				ClearErrorContext();
				throw;
			}
		}

		private JsonSerializerProxy GetInternalSerializer()
		{
			if (_internalSerializer == null)
			{
				_internalSerializer = new JsonSerializerProxy(this);
			}
			return _internalSerializer;
		}

		private JToken CreateJToken(JsonReader reader, JsonContract contract)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (contract != null)
			{
				if (contract.UnderlyingType == typeof(JRaw))
				{
					return JRaw.Create(reader);
				}
				if (reader.TokenType == JsonToken.Null && !(contract.UnderlyingType == typeof(JValue)) && !(contract.UnderlyingType == typeof(JToken)))
				{
					return null;
				}
			}
			using JTokenWriter jTokenWriter = new JTokenWriter();
			jTokenWriter.WriteToken(reader);
			return jTokenWriter.Token;
		}

		private JToken CreateJObject(JsonReader reader)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			using JTokenWriter jTokenWriter = new JTokenWriter();
			jTokenWriter.WriteStartObject();
			do
			{
				if (reader.TokenType == JsonToken.PropertyName)
				{
					string text = (string)reader.Value;
					while (reader.Read() && reader.TokenType == JsonToken.Comment)
					{
					}
					if (!CheckPropertyName(reader, text))
					{
						jTokenWriter.WritePropertyName(text);
						jTokenWriter.WriteToken(reader, writeChildren: true, writeDateConstructorAsDate: true);
					}
				}
				else if (reader.TokenType != JsonToken.Comment)
				{
					jTokenWriter.WriteEndObject();
					return jTokenWriter.Token;
				}
			}
			while (reader.Read());
			throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
		}

		private object CreateValueInternal(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
		{
			if (contract != null && contract.ContractType == JsonContractType.Linq)
			{
				return CreateJToken(reader, contract);
			}
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.StartObject:
					return CreateObject(reader, objectType, contract, member, containerContract, containerMember, existingValue);
				case JsonToken.StartArray:
					return CreateList(reader, objectType, contract, member, existingValue, null);
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
					return EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
				case JsonToken.String:
				{
					string text = (string)reader.Value;
					if (string.IsNullOrEmpty(text) && objectType != typeof(string) && objectType != typeof(object) && contract != null && contract.IsNullable)
					{
						return null;
					}
					if (objectType == typeof(byte[]))
					{
						return Convert.FromBase64String(text);
					}
					return EnsureType(reader, text, CultureInfo.InvariantCulture, contract, objectType);
				}
				case JsonToken.StartConstructor:
				{
					string value = reader.Value.ToString();
					return EnsureType(reader, value, CultureInfo.InvariantCulture, contract, objectType);
				}
				case JsonToken.Null:
				case JsonToken.Undefined:
					if (objectType == typeof(DBNull))
					{
						return DBNull.Value;
					}
					return EnsureType(reader, reader.Value, CultureInfo.InvariantCulture, contract, objectType);
				case JsonToken.Raw:
					return new JRaw((string)reader.Value);
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token while deserializing object: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (reader.Read());
			throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
		}

		internal string GetExpectedDescription(JsonContract contract)
		{
			switch (contract.ContractType)
			{
			case JsonContractType.Object:
			case JsonContractType.Dictionary:
			case JsonContractType.Dynamic:
			case JsonContractType.Serializable:
				return "JSON object (e.g. {\"name\":\"value\"})";
			case JsonContractType.Array:
				return "JSON array (e.g. [1,2,3])";
			case JsonContractType.Primitive:
				return "JSON primitive value (e.g. string, number, boolean, null)";
			case JsonContractType.String:
				return "JSON string value";
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private JsonConverter GetConverter(JsonContract contract, JsonConverter memberConverter, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			JsonConverter result = null;
			if (memberConverter != null)
			{
				result = memberConverter;
			}
			else if (containerProperty != null && containerProperty.ItemConverter != null)
			{
				result = containerProperty.ItemConverter;
			}
			else if (containerContract != null && containerContract.ItemConverter != null)
			{
				result = containerContract.ItemConverter;
			}
			else if (contract != null)
			{
				JsonConverter matchingConverter;
				if (contract.Converter != null)
				{
					result = contract.Converter;
				}
				else if ((matchingConverter = Serializer.GetMatchingConverter(contract.UnderlyingType)) != null)
				{
					result = matchingConverter;
				}
				else if (contract.InternalConverter != null)
				{
					result = contract.InternalConverter;
				}
			}
			return result;
		}

		private object CreateObject(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue)
		{
			Type objectType2 = objectType;
			string id;
			if (Serializer.MetadataPropertyHandling == MetadataPropertyHandling.Ignore)
			{
				CheckedRead(reader);
				id = null;
			}
			else if (Serializer.MetadataPropertyHandling == MetadataPropertyHandling.ReadAhead)
			{
				JTokenReader jTokenReader = reader as JTokenReader;
				if (jTokenReader == null)
				{
					JToken jToken = JToken.ReadFrom(reader);
					jTokenReader = (JTokenReader)jToken.CreateReader();
					jTokenReader.Culture = reader.Culture;
					jTokenReader.DateFormatString = reader.DateFormatString;
					jTokenReader.DateParseHandling = reader.DateParseHandling;
					jTokenReader.DateTimeZoneHandling = reader.DateTimeZoneHandling;
					jTokenReader.FloatParseHandling = reader.FloatParseHandling;
					jTokenReader.SupportMultipleContent = reader.SupportMultipleContent;
					CheckedRead(jTokenReader);
					reader = jTokenReader;
				}
				if (ReadMetadataPropertiesToken(jTokenReader, ref objectType2, ref contract, member, containerContract, containerMember, existingValue, out var newValue, out id))
				{
					return newValue;
				}
			}
			else
			{
				CheckedRead(reader);
				if (ReadMetadataProperties(reader, ref objectType2, ref contract, member, containerContract, containerMember, existingValue, out var newValue2, out id))
				{
					return newValue2;
				}
			}
			if (HasNoDefinedType(contract))
			{
				return CreateJObject(reader);
			}
			switch (contract.ContractType)
			{
			case JsonContractType.Object:
			{
				bool createdFromNonDefaultCreator2 = false;
				JsonObjectContract jsonObjectContract = (JsonObjectContract)contract;
				object obj = ((existingValue == null || (!(objectType2 == objectType) && !objectType2.IsAssignableFrom(existingValue.GetType()))) ? CreateNewObject(reader, jsonObjectContract, member, containerMember, id, out createdFromNonDefaultCreator2) : existingValue);
				if (createdFromNonDefaultCreator2)
				{
					return obj;
				}
				return PopulateObject(obj, reader, jsonObjectContract, member, id);
			}
			case JsonContractType.Primitive:
			{
				JsonPrimitiveContract contract4 = (JsonPrimitiveContract)contract;
				if (Serializer.MetadataPropertyHandling != MetadataPropertyHandling.Ignore && reader.TokenType == JsonToken.PropertyName && string.Equals(reader.Value.ToString(), "$value", StringComparison.Ordinal))
				{
					CheckedRead(reader);
					if (reader.TokenType == JsonToken.StartObject)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token when deserializing primitive value: " + reader.TokenType);
					}
					object result = CreateValueInternal(reader, objectType2, contract4, member, null, null, existingValue);
					CheckedRead(reader);
					return result;
				}
				break;
			}
			case JsonContractType.Dictionary:
			{
				JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)contract;
				if (existingValue == null)
				{
					bool createdFromNonDefaultCreator;
					IDictionary dictionary = CreateNewDictionary(reader, jsonDictionaryContract, out createdFromNonDefaultCreator);
					if (createdFromNonDefaultCreator)
					{
						if (id != null)
						{
							throw JsonSerializationException.Create(reader, "Cannot preserve reference to readonly dictionary, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (contract.OnSerializingCallbacks.Count > 0)
						{
							throw JsonSerializationException.Create(reader, "Cannot call OnSerializing on readonly dictionary, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (contract.OnErrorCallbacks.Count > 0)
						{
							throw JsonSerializationException.Create(reader, "Cannot call OnError on readonly list, or dictionary created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
						if (!jsonDictionaryContract.HasParametrizedCreator)
						{
							throw JsonSerializationException.Create(reader, "Cannot deserialize readonly or fixed size dictionary: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
						}
					}
					PopulateDictionary(dictionary, reader, jsonDictionaryContract, member, id);
					if (createdFromNonDefaultCreator)
					{
						return jsonDictionaryContract.ParametrizedCreator(dictionary);
					}
					if (dictionary is IWrappedDictionary)
					{
						return ((IWrappedDictionary)dictionary).UnderlyingDictionary;
					}
					return dictionary;
				}
				return PopulateDictionary(jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(existingValue) : ((IDictionary)existingValue), reader, jsonDictionaryContract, member, id);
			}
			case JsonContractType.Dynamic:
			{
				JsonDynamicContract contract3 = (JsonDynamicContract)contract;
				return CreateDynamic(reader, contract3, member, id);
			}
			case JsonContractType.Serializable:
			{
				JsonISerializableContract contract2 = (JsonISerializableContract)contract;
				return CreateISerializable(reader, contract2, member, id);
			}
			}
			string format = "Cannot deserialize the current JSON object (e.g. {{\"name\":\"value\"}}) into type '{0}' because the type requires a {1} to deserialize correctly." + Environment.NewLine + "To fix this error either change the JSON to a {1} or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object." + Environment.NewLine;
			format = format.FormatWith(CultureInfo.InvariantCulture, objectType2, GetExpectedDescription(contract));
			throw JsonSerializationException.Create(reader, format);
		}

		private bool ReadMetadataPropertiesToken(JTokenReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue, out object newValue, out string id)
		{
			id = null;
			newValue = null;
			if (reader.TokenType == JsonToken.StartObject)
			{
				JObject jObject = (JObject)reader.CurrentToken;
				JToken jToken = jObject["$ref"];
				if (jToken != null)
				{
					if (jToken.Type != JTokenType.String && jToken.Type != JTokenType.Null)
					{
						throw JsonSerializationException.Create(jToken, jToken.Path, "JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.InvariantCulture, "$ref"), null);
					}
					JToken parent = jToken.Parent;
					JToken jToken2 = null;
					if (parent.Next != null)
					{
						jToken2 = parent.Next;
					}
					else if (parent.Previous != null)
					{
						jToken2 = parent.Previous;
					}
					string text = (string)jToken;
					if (text != null)
					{
						if (jToken2 != null)
						{
							throw JsonSerializationException.Create(jToken2, jToken2.Path, "Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, "$ref"), null);
						}
						newValue = Serializer.GetReferenceResolver().ResolveReference(this, text);
						if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
						{
							TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader, reader.Path, "Resolved object reference '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, text, newValue.GetType())), null);
						}
						reader.Skip();
						return true;
					}
				}
				JToken jToken3 = jObject["$type"];
				if (jToken3 != null)
				{
					string qualifiedTypeName = (string)jToken3;
					JsonReader reader2 = jToken3.CreateReader();
					CheckedRead(reader2);
					ResolveTypeName(reader2, ref objectType, ref contract, member, containerContract, containerMember, qualifiedTypeName);
					JToken jToken4 = jObject["$value"];
					if (jToken4 != null)
					{
						while (true)
						{
							CheckedRead(reader);
							if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "$value")
							{
								break;
							}
							CheckedRead(reader);
							reader.Skip();
						}
						return false;
					}
				}
				JToken jToken5 = jObject["$id"];
				if (jToken5 != null)
				{
					id = (string)jToken5;
				}
				JToken jToken6 = jObject["$values"];
				if (jToken6 != null)
				{
					JsonReader reader3 = jToken6.CreateReader();
					CheckedRead(reader3);
					newValue = CreateList(reader3, objectType, contract, member, existingValue, id);
					reader.Skip();
					return true;
				}
			}
			CheckedRead(reader);
			return false;
		}

		private bool ReadMetadataProperties(JsonReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, object existingValue, out object newValue, out string id)
		{
			id = null;
			newValue = null;
			if (reader.TokenType == JsonToken.PropertyName)
			{
				string text = reader.Value.ToString();
				if (text.Length > 0 && text[0] == '$')
				{
					bool flag;
					do
					{
						text = reader.Value.ToString();
						if (string.Equals(text, "$ref", StringComparison.Ordinal))
						{
							CheckedRead(reader);
							if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null)
							{
								throw JsonSerializationException.Create(reader, "JSON reference {0} property must have a string or null value.".FormatWith(CultureInfo.InvariantCulture, "$ref"));
							}
							string text2 = ((reader.Value != null) ? reader.Value.ToString() : null);
							CheckedRead(reader);
							if (text2 != null)
							{
								if (reader.TokenType == JsonToken.PropertyName)
								{
									throw JsonSerializationException.Create(reader, "Additional content found in JSON reference object. A JSON reference object should only have a {0} property.".FormatWith(CultureInfo.InvariantCulture, "$ref"));
								}
								newValue = Serializer.GetReferenceResolver().ResolveReference(this, text2);
								if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
								{
									TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Resolved object reference '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, text2, newValue.GetType())), null);
								}
								return true;
							}
							flag = true;
						}
						else if (string.Equals(text, "$type", StringComparison.Ordinal))
						{
							CheckedRead(reader);
							string qualifiedTypeName = reader.Value.ToString();
							ResolveTypeName(reader, ref objectType, ref contract, member, containerContract, containerMember, qualifiedTypeName);
							CheckedRead(reader);
							flag = true;
						}
						else if (string.Equals(text, "$id", StringComparison.Ordinal))
						{
							CheckedRead(reader);
							id = ((reader.Value != null) ? reader.Value.ToString() : null);
							CheckedRead(reader);
							flag = true;
						}
						else
						{
							if (string.Equals(text, "$values", StringComparison.Ordinal))
							{
								CheckedRead(reader);
								object obj = CreateList(reader, objectType, contract, member, existingValue, id);
								CheckedRead(reader);
								newValue = obj;
								return true;
							}
							flag = false;
						}
					}
					while (flag && reader.TokenType == JsonToken.PropertyName);
				}
			}
			return false;
		}

		private void ResolveTypeName(JsonReader reader, ref Type objectType, ref JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerMember, string qualifiedTypeName)
		{
			if ((member?.TypeNameHandling ?? containerContract?.ItemTypeNameHandling ?? containerMember?.ItemTypeNameHandling ?? Serializer._typeNameHandling) != 0)
			{
				ReflectionUtils.SplitFullyQualifiedTypeName(qualifiedTypeName, out var typeName, out var assemblyName);
				Type type;
				try
				{
					type = Serializer._binder.BindToType(assemblyName, typeName);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error resolving type specified in JSON '{0}'.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName), ex);
				}
				if (type == null)
				{
					throw JsonSerializationException.Create(reader, "Type specified in JSON '{0}' was not resolved.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName));
				}
				if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
				{
					TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Resolved type '{0}' to {1}.".FormatWith(CultureInfo.InvariantCulture, qualifiedTypeName, type)), null);
				}
				if (objectType != null && objectType != typeof(IDynamicMetaObjectProvider) && !objectType.IsAssignableFrom(type))
				{
					throw JsonSerializationException.Create(reader, "Type specified in JSON '{0}' is not compatible with '{1}'.".FormatWith(CultureInfo.InvariantCulture, type.AssemblyQualifiedName, objectType.AssemblyQualifiedName));
				}
				objectType = type;
				contract = GetContractSafe(type);
			}
		}

		private JsonArrayContract EnsureArrayContract(JsonReader reader, Type objectType, JsonContract contract)
		{
			if (contract == null)
			{
				throw JsonSerializationException.Create(reader, "Could not resolve type '{0}' to a JsonContract.".FormatWith(CultureInfo.InvariantCulture, objectType));
			}
			if (!(contract is JsonArrayContract result))
			{
				string format = "Cannot deserialize the current JSON array (e.g. [1,2,3]) into type '{0}' because the type requires a {1} to deserialize correctly." + Environment.NewLine + "To fix this error either change the JSON to a {1} or change the deserialized type to an array or a type that implements a collection interface (e.g. ICollection, IList) like List<T> that can be deserialized from a JSON array. JsonArrayAttribute can also be added to the type to force it to deserialize from a JSON array." + Environment.NewLine;
				format = format.FormatWith(CultureInfo.InvariantCulture, objectType, GetExpectedDescription(contract));
				throw JsonSerializationException.Create(reader, format);
			}
			return result;
		}

		private void CheckedRead(JsonReader reader)
		{
			if (!reader.Read())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
			}
		}

		private object CreateList(JsonReader reader, Type objectType, JsonContract contract, JsonProperty member, object existingValue, string id)
		{
			if (HasNoDefinedType(contract))
			{
				return CreateJToken(reader, contract);
			}
			JsonArrayContract jsonArrayContract = EnsureArrayContract(reader, objectType, contract);
			if (existingValue == null)
			{
				bool createdFromNonDefaultCreator;
				IList list = CreateNewList(reader, jsonArrayContract, out createdFromNonDefaultCreator);
				if (createdFromNonDefaultCreator)
				{
					if (id != null)
					{
						throw JsonSerializationException.Create(reader, "Cannot preserve reference to array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (contract.OnSerializingCallbacks.Count > 0)
					{
						throw JsonSerializationException.Create(reader, "Cannot call OnSerializing on an array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (contract.OnErrorCallbacks.Count > 0)
					{
						throw JsonSerializationException.Create(reader, "Cannot call OnError on an array or readonly list, or list created from a non-default constructor: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
					if (!jsonArrayContract.HasParametrizedCreator && !jsonArrayContract.IsArray)
					{
						throw JsonSerializationException.Create(reader, "Cannot deserialize readonly or fixed size list: {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
					}
				}
				if (!jsonArrayContract.IsMultidimensionalArray)
				{
					PopulateList(list, reader, jsonArrayContract, member, id);
				}
				else
				{
					PopulateMultidimensionalArray(list, reader, jsonArrayContract, member, id);
				}
				if (createdFromNonDefaultCreator)
				{
					if (jsonArrayContract.IsMultidimensionalArray)
					{
						list = CollectionUtils.ToMultidimensionalArray(list, jsonArrayContract.CollectionItemType, contract.CreatedType.GetArrayRank());
					}
					else
					{
						if (!jsonArrayContract.IsArray)
						{
							return jsonArrayContract.ParametrizedCreator(list);
						}
						Array array = Array.CreateInstance(jsonArrayContract.CollectionItemType, list.Count);
						list.CopyTo(array, 0);
						list = array;
					}
				}
				else if (list is IWrappedCollection)
				{
					return ((IWrappedCollection)list).UnderlyingCollection;
				}
				return list;
			}
			if (!jsonArrayContract.CanDeserialize)
			{
				throw JsonSerializationException.Create(reader, "Cannot populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.CreatedType));
			}
			return PopulateList(jsonArrayContract.ShouldCreateWrapper ? jsonArrayContract.CreateWrapper(existingValue) : ((IList)existingValue), reader, jsonArrayContract, member, id);
		}

		private bool HasNoDefinedType(JsonContract contract)
		{
			if (contract != null && !(contract.UnderlyingType == typeof(object)) && contract.ContractType != JsonContractType.Linq)
			{
				return contract.UnderlyingType == typeof(IDynamicMetaObjectProvider);
			}
			return true;
		}

		private object EnsureType(JsonReader reader, object value, CultureInfo culture, JsonContract contract, Type targetType)
		{
			if (targetType == null)
			{
				return value;
			}
			Type objectType = ReflectionUtils.GetObjectType(value);
			if (objectType != targetType)
			{
				if (value == null && contract.IsNullable)
				{
					return null;
				}
				try
				{
					if (contract.IsConvertable)
					{
						JsonPrimitiveContract jsonPrimitiveContract = (JsonPrimitiveContract)contract;
						if (contract.IsEnum)
						{
							if (value is string)
							{
								return Enum.Parse(contract.NonNullableUnderlyingType, value.ToString(), ignoreCase: true);
							}
							if (ConvertUtils.IsInteger(jsonPrimitiveContract.TypeCode))
							{
								return Enum.ToObject(contract.NonNullableUnderlyingType, value);
							}
						}
						if (value is BigInteger)
						{
							return ConvertUtils.FromBigInteger((BigInteger)value, targetType);
						}
						return Convert.ChangeType(value, contract.NonNullableUnderlyingType, culture);
					}
					return ConvertUtils.ConvertOrCast(value, culture, contract.NonNullableUnderlyingType);
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.FormatValueForPrint(value), targetType), ex);
				}
			}
			return value;
		}

		private bool SetPropertyValue(JsonProperty property, JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target)
		{
			if (CalculatePropertyDetails(property, ref propertyConverter, containerContract, containerProperty, reader, target, out var useExistingValue, out var currentValue, out var propertyContract, out var gottenCurrentValue))
			{
				return false;
			}
			object obj;
			if (propertyConverter != null && propertyConverter.CanRead)
			{
				if (!gottenCurrentValue && target != null && property.Readable)
				{
					currentValue = property.ValueProvider.GetValue(target);
				}
				obj = DeserializeConvertable(propertyConverter, reader, property.PropertyType, currentValue);
			}
			else
			{
				obj = CreateValueInternal(reader, property.PropertyType, propertyContract, property, containerContract, containerProperty, useExistingValue ? currentValue : null);
			}
			if ((!useExistingValue || obj != currentValue) && ShouldSetPropertyValue(property, obj))
			{
				property.ValueProvider.SetValue(target, obj);
				if (property.SetIsSpecified != null)
				{
					if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "IsSpecified for property '{0}' on {1} set to true.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType)), null);
					}
					property.SetIsSpecified(target, true);
				}
				return true;
			}
			return useExistingValue;
		}

		private bool CalculatePropertyDetails(JsonProperty property, ref JsonConverter propertyConverter, JsonContainerContract containerContract, JsonProperty containerProperty, JsonReader reader, object target, out bool useExistingValue, out object currentValue, out JsonContract propertyContract, out bool gottenCurrentValue)
		{
			currentValue = null;
			useExistingValue = false;
			propertyContract = null;
			gottenCurrentValue = false;
			if (property.Ignored)
			{
				return true;
			}
			JsonToken tokenType = reader.TokenType;
			if (property.PropertyContract == null)
			{
				property.PropertyContract = GetContractSafe(property.PropertyType);
			}
			ObjectCreationHandling valueOrDefault = property.ObjectCreationHandling.GetValueOrDefault(Serializer._objectCreationHandling);
			if (valueOrDefault != ObjectCreationHandling.Replace && (tokenType == JsonToken.StartArray || tokenType == JsonToken.StartObject) && property.Readable)
			{
				currentValue = property.ValueProvider.GetValue(target);
				gottenCurrentValue = true;
				if (currentValue != null)
				{
					propertyContract = GetContractSafe(currentValue.GetType());
					useExistingValue = !propertyContract.IsReadOnlyOrFixedSize && !propertyContract.UnderlyingType.IsValueType();
				}
			}
			if (!property.Writable && !useExistingValue)
			{
				return true;
			}
			if (property.NullValueHandling.GetValueOrDefault(Serializer._nullValueHandling) == NullValueHandling.Ignore && tokenType == JsonToken.Null)
			{
				return true;
			}
			if (HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Ignore) && !HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Populate) && JsonTokenUtils.IsPrimitiveToken(tokenType) && MiscellaneousUtils.ValueEquals(reader.Value, property.GetResolvedDefaultValue()))
			{
				return true;
			}
			if (currentValue == null)
			{
				propertyContract = property.PropertyContract;
			}
			else
			{
				propertyContract = GetContractSafe(currentValue.GetType());
				if (propertyContract != property.PropertyContract)
				{
					propertyConverter = GetConverter(propertyContract, property.MemberConverter, containerContract, containerProperty);
				}
			}
			return false;
		}

		private void AddReference(JsonReader reader, string id, object value)
		{
			try
			{
				if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
				{
					TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Read object reference Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, id, value.GetType())), null);
				}
				Serializer.GetReferenceResolver().AddReference(this, id, value);
			}
			catch (Exception ex)
			{
				throw JsonSerializationException.Create(reader, "Error reading object reference '{0}'.".FormatWith(CultureInfo.InvariantCulture, id), ex);
			}
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool ShouldSetPropertyValue(JsonProperty property, object value)
		{
			if (property.NullValueHandling.GetValueOrDefault(Serializer._nullValueHandling) == NullValueHandling.Ignore && value == null)
			{
				return false;
			}
			if (HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Ignore) && !HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Populate) && MiscellaneousUtils.ValueEquals(value, property.GetResolvedDefaultValue()))
			{
				return false;
			}
			if (!property.Writable)
			{
				return false;
			}
			return true;
		}

		private IList CreateNewList(JsonReader reader, JsonArrayContract contract, out bool createdFromNonDefaultCreator)
		{
			if (!contract.CanDeserialize)
			{
				throw JsonSerializationException.Create(reader, "Cannot create and populate list type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.CreatedType));
			}
			if (contract.IsReadOnlyOrFixedSize)
			{
				createdFromNonDefaultCreator = true;
				IList list = contract.CreateTemporaryCollection();
				if (contract.ShouldCreateWrapper)
				{
					list = contract.CreateWrapper(list);
				}
				return list;
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object obj = contract.DefaultCreator();
				if (contract.ShouldCreateWrapper)
				{
					obj = contract.CreateWrapper(obj);
				}
				createdFromNonDefaultCreator = false;
				return (IList)obj;
			}
			if (contract.HasParametrizedCreator)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryCollection();
			}
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			throw JsonSerializationException.Create(reader, "Unable to find a constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		private IDictionary CreateNewDictionary(JsonReader reader, JsonDictionaryContract contract, out bool createdFromNonDefaultCreator)
		{
			if (contract.IsReadOnlyOrFixedSize)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryDictionary();
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				object obj = contract.DefaultCreator();
				if (contract.ShouldCreateWrapper)
				{
					obj = contract.CreateWrapper(obj);
				}
				createdFromNonDefaultCreator = false;
				return (IDictionary)obj;
			}
			if (contract.HasParametrizedCreator)
			{
				createdFromNonDefaultCreator = true;
				return contract.CreateTemporaryDictionary();
			}
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			throw JsonSerializationException.Create(reader, "Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		private void OnDeserializing(JsonReader reader, JsonContract contract, object value)
		{
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Started deserializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnDeserializing(value, Serializer._context);
		}

		private void OnDeserialized(JsonReader reader, JsonContract contract, object value)
		{
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Finished deserializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnDeserialized(value, Serializer._context);
		}

		private object PopulateDictionary(IDictionary dictionary, JsonReader reader, JsonDictionaryContract contract, JsonProperty containerProperty, string id)
		{
			object obj = ((dictionary is IWrappedDictionary wrappedDictionary) ? wrappedDictionary.UnderlyingDictionary : dictionary);
			if (id != null)
			{
				AddReference(reader, id, obj);
			}
			OnDeserializing(reader, contract, obj);
			int depth = reader.Depth;
			if (contract.KeyContract == null)
			{
				contract.KeyContract = GetContractSafe(contract.DictionaryKeyType);
			}
			if (contract.ItemContract == null)
			{
				contract.ItemContract = GetContractSafe(contract.DictionaryValueType);
			}
			JsonConverter jsonConverter = contract.ItemConverter ?? GetConverter(contract.ItemContract, null, contract, containerProperty);
			PrimitiveTypeCode primitiveTypeCode = ((contract.KeyContract is JsonPrimitiveContract) ? ((JsonPrimitiveContract)contract.KeyContract).TypeCode : PrimitiveTypeCode.Empty);
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					object obj2 = reader.Value;
					if (CheckPropertyName(reader, obj2.ToString()))
					{
						break;
					}
					try
					{
						try
						{
							DateParseHandling dateParseHandling;
							switch (primitiveTypeCode)
							{
							case PrimitiveTypeCode.DateTime:
							case PrimitiveTypeCode.DateTimeNullable:
								dateParseHandling = DateParseHandling.DateTime;
								break;
							case PrimitiveTypeCode.DateTimeOffset:
							case PrimitiveTypeCode.DateTimeOffsetNullable:
								dateParseHandling = DateParseHandling.DateTimeOffset;
								break;
							default:
								dateParseHandling = DateParseHandling.None;
								break;
							}
							obj2 = ((dateParseHandling == DateParseHandling.None || !DateTimeUtils.TryParseDateTime(obj2.ToString(), dateParseHandling, reader.DateTimeZoneHandling, reader.DateFormatString, reader.Culture, out var dt)) ? EnsureType(reader, obj2, CultureInfo.InvariantCulture, contract.KeyContract, contract.DictionaryKeyType) : dt);
						}
						catch (Exception ex)
						{
							throw JsonSerializationException.Create(reader, "Could not convert string '{0}' to dictionary key type '{1}'. Create a TypeConverter to convert from the string to the key type object.".FormatWith(CultureInfo.InvariantCulture, reader.Value, contract.DictionaryKeyType), ex);
						}
						if (!ReadForType(reader, contract.ItemContract, jsonConverter != null))
						{
							throw JsonSerializationException.Create(reader, "Unexpected end when deserializing object.");
						}
						object value = ((jsonConverter == null || !jsonConverter.CanRead) ? CreateValueInternal(reader, contract.DictionaryValueType, contract.ItemContract, null, contract, containerProperty, null) : DeserializeConvertable(jsonConverter, reader, contract.DictionaryValueType, null));
						dictionary[obj2] = value;
					}
					catch (Exception ex2)
					{
						if (IsErrorHandled(obj, contract, obj2, reader as IJsonLineInfo, reader.Path, ex2))
						{
							HandleError(reader, readPastError: true, depth);
							break;
						}
						throw;
					}
					break;
				}
				case JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			if (!flag)
			{
				ThrowUnexpectedEndException(reader, contract, obj, "Unexpected end when deserializing object.");
			}
			OnDeserialized(reader, contract, obj);
			return obj;
		}

		private object PopulateMultidimensionalArray(IList list, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
		{
			int arrayRank = contract.UnderlyingType.GetArrayRank();
			if (id != null)
			{
				AddReference(reader, id, list);
			}
			OnDeserializing(reader, contract, list);
			JsonContract contractSafe = GetContractSafe(contract.CollectionItemType);
			JsonConverter converter = GetConverter(contractSafe, null, contract, containerProperty);
			int? num = null;
			Stack<IList> stack = new Stack<IList>();
			stack.Push(list);
			IList list2 = list;
			bool flag = false;
			do
			{
				int depth = reader.Depth;
				if (stack.Count == arrayRank)
				{
					try
					{
						if (ReadForType(reader, contractSafe, converter != null))
						{
							switch (reader.TokenType)
							{
							case JsonToken.EndArray:
								stack.Pop();
								list2 = stack.Peek();
								num = null;
								break;
							default:
							{
								object value = ((converter == null || !converter.CanRead) ? CreateValueInternal(reader, contract.CollectionItemType, contractSafe, null, contract, containerProperty, null) : DeserializeConvertable(converter, reader, contract.CollectionItemType, null));
								list2.Add(value);
								break;
							}
							case JsonToken.Comment:
								break;
							}
							continue;
						}
					}
					catch (Exception ex)
					{
						JsonPosition position = reader.GetPosition(depth);
						if (IsErrorHandled(list, contract, position.Position, reader as IJsonLineInfo, reader.Path, ex))
						{
							HandleError(reader, readPastError: true, depth);
							if (num.HasValue && num == position.Position)
							{
								throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
							}
							num = position.Position;
							continue;
						}
						throw;
					}
					break;
				}
				if (!reader.Read())
				{
					break;
				}
				switch (reader.TokenType)
				{
				case JsonToken.StartArray:
				{
					IList list3 = new List<object>();
					list2.Add(list3);
					stack.Push(list3);
					list2 = list3;
					break;
				}
				case JsonToken.EndArray:
					stack.Pop();
					if (stack.Count > 0)
					{
						list2 = stack.Peek();
					}
					else
					{
						flag = true;
					}
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when deserializing multidimensional array: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (!flag);
			if (!flag)
			{
				ThrowUnexpectedEndException(reader, contract, list, "Unexpected end when deserializing array.");
			}
			OnDeserialized(reader, contract, list);
			return list;
		}

		private void ThrowUnexpectedEndException(JsonReader reader, JsonContract contract, object currentObject, string message)
		{
			try
			{
				throw JsonSerializationException.Create(reader, message);
			}
			catch (Exception ex)
			{
				if (IsErrorHandled(currentObject, contract, null, reader as IJsonLineInfo, reader.Path, ex))
				{
					HandleError(reader, readPastError: false, 0);
					return;
				}
				throw;
			}
		}

		private object PopulateList(IList list, JsonReader reader, JsonArrayContract contract, JsonProperty containerProperty, string id)
		{
			object obj = ((list is IWrappedCollection wrappedCollection) ? wrappedCollection.UnderlyingCollection : list);
			if (id != null)
			{
				AddReference(reader, id, obj);
			}
			if (list.IsFixedSize)
			{
				reader.Skip();
				return obj;
			}
			OnDeserializing(reader, contract, obj);
			int depth = reader.Depth;
			if (contract.ItemContract == null)
			{
				contract.ItemContract = GetContractSafe(contract.CollectionItemType);
			}
			JsonConverter converter = GetConverter(contract.ItemContract, null, contract, containerProperty);
			int? num = null;
			bool flag = false;
			do
			{
				try
				{
					if (ReadForType(reader, contract.ItemContract, converter != null))
					{
						switch (reader.TokenType)
						{
						case JsonToken.EndArray:
							flag = true;
							break;
						default:
						{
							object value = ((converter == null || !converter.CanRead) ? CreateValueInternal(reader, contract.CollectionItemType, contract.ItemContract, null, contract, containerProperty, null) : DeserializeConvertable(converter, reader, contract.CollectionItemType, null));
							list.Add(value);
							break;
						}
						case JsonToken.Comment:
							break;
						}
						continue;
					}
				}
				catch (Exception ex)
				{
					JsonPosition position = reader.GetPosition(depth);
					if (IsErrorHandled(obj, contract, position.Position, reader as IJsonLineInfo, reader.Path, ex))
					{
						HandleError(reader, readPastError: true, depth);
						if (num.HasValue && num == position.Position)
						{
							throw JsonSerializationException.Create(reader, "Infinite loop detected from error handling.", ex);
						}
						num = position.Position;
						continue;
					}
					throw;
				}
				break;
			}
			while (!flag);
			if (!flag)
			{
				ThrowUnexpectedEndException(reader, contract, obj, "Unexpected end when deserializing array.");
			}
			OnDeserialized(reader, contract, obj);
			return obj;
		}

		private object CreateISerializable(JsonReader reader, JsonISerializableContract contract, JsonProperty member, string id)
		{
			Type underlyingType = contract.UnderlyingType;
			if (!JsonTypeReflector.FullyTrusted)
			{
				string format = "Type '{0}' implements ISerializable but cannot be deserialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine + "To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
				format = format.FormatWith(CultureInfo.InvariantCulture, underlyingType);
				throw JsonSerializationException.Create(reader, format);
			}
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Deserializing {0} using ISerializable constructor.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, new JsonFormatterConverter(this, contract, member));
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
					}
					serializationInfo.AddValue(text, JToken.ReadFrom(reader));
					break;
				}
				case JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			if (!flag)
			{
				ThrowUnexpectedEndException(reader, contract, serializationInfo, "Unexpected end when deserializing object.");
			}
			if (contract.ISerializableCreator == null)
			{
				throw JsonSerializationException.Create(reader, "ISerializable type '{0}' does not have a valid constructor. To correctly implement ISerializable a constructor that takes SerializationInfo and StreamingContext parameters should be present.".FormatWith(CultureInfo.InvariantCulture, underlyingType));
			}
			object obj = contract.ISerializableCreator(serializationInfo, Serializer._context);
			if (id != null)
			{
				AddReference(reader, id, obj);
			}
			OnDeserializing(reader, contract, obj);
			OnDeserialized(reader, contract, obj);
			return obj;
		}

		internal object CreateISerializableItem(JToken token, Type type, JsonISerializableContract contract, JsonProperty member)
		{
			JsonContract contractSafe = GetContractSafe(type);
			JsonConverter converter = GetConverter(contractSafe, null, contract, member);
			JsonReader reader = token.CreateReader();
			CheckedRead(reader);
			if (converter != null && converter.CanRead)
			{
				return DeserializeConvertable(converter, reader, type, null);
			}
			return CreateValueInternal(reader, type, contractSafe, null, contract, member, null);
		}

		private object CreateDynamic(JsonReader reader, JsonDynamicContract contract, JsonProperty member, string id)
		{
			if (!contract.IsInstantiable)
			{
				throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
			}
			if (contract.DefaultCreator != null && (!contract.DefaultCreatorNonPublic || Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor))
			{
				IDynamicMetaObjectProvider dynamicMetaObjectProvider = (IDynamicMetaObjectProvider)contract.DefaultCreator();
				if (id != null)
				{
					AddReference(reader, id, dynamicMetaObjectProvider);
				}
				OnDeserializing(reader, contract, dynamicMetaObjectProvider);
				int depth = reader.Depth;
				bool flag = false;
				do
				{
					switch (reader.TokenType)
					{
					case JsonToken.PropertyName:
					{
						string text = reader.Value.ToString();
						try
						{
							if (!reader.Read())
							{
								throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
							}
							JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
							if (closestMatchProperty != null && closestMatchProperty.Writable && !closestMatchProperty.Ignored)
							{
								if (closestMatchProperty.PropertyContract == null)
								{
									closestMatchProperty.PropertyContract = GetContractSafe(closestMatchProperty.PropertyType);
								}
								JsonConverter converter = GetConverter(closestMatchProperty.PropertyContract, closestMatchProperty.MemberConverter, null, null);
								if (!SetPropertyValue(closestMatchProperty, converter, null, member, reader, dynamicMetaObjectProvider))
								{
									reader.Skip();
								}
							}
							else
							{
								Type type = (JsonTokenUtils.IsPrimitiveToken(reader.TokenType) ? reader.ValueType : typeof(IDynamicMetaObjectProvider));
								JsonContract contractSafe = GetContractSafe(type);
								JsonConverter converter2 = GetConverter(contractSafe, null, null, member);
								object value = ((converter2 == null || !converter2.CanRead) ? CreateValueInternal(reader, type, contractSafe, null, null, member, null) : DeserializeConvertable(converter2, reader, type, null));
								contract.TrySetMember(dynamicMetaObjectProvider, text, value);
							}
						}
						catch (Exception ex)
						{
							if (IsErrorHandled(dynamicMetaObjectProvider, contract, text, reader as IJsonLineInfo, reader.Path, ex))
							{
								HandleError(reader, readPastError: true, depth);
								break;
							}
							throw;
						}
						break;
					}
					case JsonToken.EndObject:
						flag = true;
						break;
					default:
						throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
					}
				}
				while (!flag && reader.Read());
				if (!flag)
				{
					ThrowUnexpectedEndException(reader, contract, dynamicMetaObjectProvider, "Unexpected end when deserializing object.");
				}
				OnDeserialized(reader, contract, dynamicMetaObjectProvider);
				return dynamicMetaObjectProvider;
			}
			throw JsonSerializationException.Create(reader, "Unable to find a default constructor to use for type {0}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType));
		}

		private object CreateObjectUsingCreatorWithParameters(JsonReader reader, JsonObjectContract contract, JsonProperty containerProperty, ObjectConstructor<object> creator, string id)
		{
			ValidationUtils.ArgumentNotNull(creator, "creator");
			Dictionary<JsonProperty, PropertyPresence> dictionary = ((contract.HasRequiredOrDefaultValueProperties || HasFlag(Serializer._defaultValueHandling, DefaultValueHandling.Populate)) ? contract.Properties.ToDictionary((JsonProperty m) => m, (JsonProperty m) => PropertyPresence.None) : null);
			Type underlyingType = contract.UnderlyingType;
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				string arg = string.Join(", ", contract.CreatorParameters.Select((JsonProperty p) => p.PropertyName).ToArray());
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Deserializing {0} using creator with parameters: {1}.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType, arg)), null);
			}
			IDictionary<string, object> extensionData;
			IDictionary<JsonProperty, object> dictionary2 = ResolvePropertyAndCreatorValues(contract, containerProperty, reader, underlyingType, out extensionData);
			object[] array = new object[contract.CreatorParameters.Count];
			IDictionary<JsonProperty, object> dictionary3 = new Dictionary<JsonProperty, object>();
			foreach (KeyValuePair<JsonProperty, object> item in dictionary2)
			{
				JsonProperty property = item.Key;
				JsonProperty jsonProperty = ((!contract.CreatorParameters.Contains(property)) ? contract.CreatorParameters.ForgivingCaseSensitiveFind((JsonProperty p) => p.PropertyName, property.UnderlyingName) : property);
				if (jsonProperty != null)
				{
					int num = contract.CreatorParameters.IndexOf(jsonProperty);
					array[num] = item.Value;
				}
				else
				{
					dictionary3.Add(item);
				}
				if (dictionary != null)
				{
					JsonProperty jsonProperty2 = dictionary.Keys.FirstOrDefault((JsonProperty p) => p.PropertyName == property.PropertyName);
					if (jsonProperty2 != null)
					{
						dictionary[jsonProperty2] = ((item.Value == null) ? PropertyPresence.Null : PropertyPresence.Value);
					}
				}
			}
			object obj = creator(array);
			if (id != null)
			{
				AddReference(reader, id, obj);
			}
			OnDeserializing(reader, contract, obj);
			foreach (KeyValuePair<JsonProperty, object> item2 in dictionary3)
			{
				JsonProperty key = item2.Key;
				object value = item2.Value;
				if (ShouldSetPropertyValue(key, value))
				{
					key.ValueProvider.SetValue(obj, value);
				}
				else
				{
					if (key.Writable || value == null)
					{
						continue;
					}
					JsonContract jsonContract = Serializer._contractResolver.ResolveContract(key.PropertyType);
					if (jsonContract.ContractType == JsonContractType.Array)
					{
						JsonArrayContract jsonArrayContract = (JsonArrayContract)jsonContract;
						object value2 = key.ValueProvider.GetValue(obj);
						if (value2 == null)
						{
							continue;
						}
						IWrappedCollection wrappedCollection = jsonArrayContract.CreateWrapper(value2);
						IWrappedCollection wrappedCollection2 = jsonArrayContract.CreateWrapper(value);
						foreach (object item3 in wrappedCollection2)
						{
							wrappedCollection.Add(item3);
						}
					}
					else
					{
						if (jsonContract.ContractType != JsonContractType.Dictionary)
						{
							continue;
						}
						JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)jsonContract;
						object value3 = key.ValueProvider.GetValue(obj);
						if (value3 == null)
						{
							continue;
						}
						IDictionary dictionary4 = (jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(value3) : ((IDictionary)value3));
						IDictionary dictionary5 = (jsonDictionaryContract.ShouldCreateWrapper ? jsonDictionaryContract.CreateWrapper(value) : ((IDictionary)value));
						foreach (DictionaryEntry item4 in dictionary5)
						{
							dictionary4.Add(item4.Key, item4.Value);
						}
					}
				}
			}
			if (extensionData != null)
			{
				foreach (KeyValuePair<string, object> item5 in extensionData)
				{
					contract.ExtensionDataSetter(obj, item5.Key, item5.Value);
				}
			}
			EndObject(obj, reader, contract, reader.Depth, dictionary);
			OnDeserialized(reader, contract, obj);
			return obj;
		}

		private object DeserializeConvertable(JsonConverter converter, JsonReader reader, Type objectType, object existingValue)
		{
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Started deserializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, objectType, converter.GetType())), null);
			}
			object result = converter.ReadJson(reader, objectType, existingValue, GetInternalSerializer());
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Finished deserializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, objectType, converter.GetType())), null);
			}
			return result;
		}

		private IDictionary<JsonProperty, object> ResolvePropertyAndCreatorValues(JsonObjectContract contract, JsonProperty containerProperty, JsonReader reader, Type objectType, out IDictionary<string, object> extensionData)
		{
			extensionData = ((contract.ExtensionDataSetter != null) ? new Dictionary<string, object>() : null);
			IDictionary<JsonProperty, object> dictionary = new Dictionary<JsonProperty, object>();
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					JsonProperty jsonProperty = contract.CreatorParameters.GetClosestMatchProperty(text) ?? contract.Properties.GetClosestMatchProperty(text);
					if (jsonProperty != null)
					{
						if (jsonProperty.PropertyContract == null)
						{
							jsonProperty.PropertyContract = GetContractSafe(jsonProperty.PropertyType);
						}
						JsonConverter converter = GetConverter(jsonProperty.PropertyContract, jsonProperty.MemberConverter, contract, containerProperty);
						if (!ReadForType(reader, jsonProperty.PropertyContract, converter != null))
						{
							throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
						}
						if (!jsonProperty.Ignored)
						{
							if (jsonProperty.PropertyContract == null)
							{
								jsonProperty.PropertyContract = GetContractSafe(jsonProperty.PropertyType);
							}
							object value = ((converter == null || !converter.CanRead) ? CreateValueInternal(reader, jsonProperty.PropertyType, jsonProperty.PropertyContract, jsonProperty, contract, containerProperty, null) : DeserializeConvertable(converter, reader, jsonProperty.PropertyType, null));
							dictionary[jsonProperty] = value;
							break;
						}
					}
					else
					{
						if (!reader.Read())
						{
							throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
						}
						if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
						{
							TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Could not find member '{0}' on {1}.".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType)), null);
						}
						if (Serializer._missingMemberHandling == MissingMemberHandling.Error)
						{
							throw JsonSerializationException.Create(reader, "Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, text, objectType.Name));
						}
					}
					if (extensionData != null)
					{
						object value2 = CreateValueInternal(reader, null, null, null, contract, containerProperty, null);
						extensionData[text] = value2;
					}
					else
					{
						reader.Skip();
					}
					break;
				}
				case JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			return dictionary;
		}

		private bool ReadForType(JsonReader reader, JsonContract contract, bool hasConverter)
		{
			if (hasConverter)
			{
				return reader.Read();
			}
			switch (contract?.InternalReadType ?? ReadType.Read)
			{
			case ReadType.Read:
				do
				{
					if (!reader.Read())
					{
						return false;
					}
				}
				while (reader.TokenType == JsonToken.Comment);
				return true;
			case ReadType.ReadAsInt32:
				reader.ReadAsInt32();
				break;
			case ReadType.ReadAsDecimal:
				reader.ReadAsDecimal();
				break;
			case ReadType.ReadAsBytes:
				reader.ReadAsBytes();
				break;
			case ReadType.ReadAsString:
				reader.ReadAsString();
				break;
			case ReadType.ReadAsDateTime:
				reader.ReadAsDateTime();
				break;
			case ReadType.ReadAsDateTimeOffset:
				reader.ReadAsDateTimeOffset();
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return reader.TokenType != JsonToken.None;
		}

		public object CreateNewObject(JsonReader reader, JsonObjectContract objectContract, JsonProperty containerMember, JsonProperty containerProperty, string id, out bool createdFromNonDefaultCreator)
		{
			object obj = null;
			if (objectContract.OverrideCreator != null)
			{
				if (objectContract.CreatorParameters.Count > 0)
				{
					createdFromNonDefaultCreator = true;
					return CreateObjectUsingCreatorWithParameters(reader, objectContract, containerMember, objectContract.OverrideCreator, id);
				}
				obj = objectContract.OverrideCreator();
			}
			else if (objectContract.DefaultCreator != null && (!objectContract.DefaultCreatorNonPublic || Serializer._constructorHandling == ConstructorHandling.AllowNonPublicDefaultConstructor || objectContract.ParametrizedCreator == null))
			{
				obj = objectContract.DefaultCreator();
			}
			else if (objectContract.ParametrizedCreator != null)
			{
				createdFromNonDefaultCreator = true;
				return CreateObjectUsingCreatorWithParameters(reader, objectContract, containerMember, objectContract.ParametrizedCreator, id);
			}
			if (obj == null)
			{
				if (!objectContract.IsInstantiable)
				{
					throw JsonSerializationException.Create(reader, "Could not create an instance of type {0}. Type is an interface or abstract class and cannot be instantiated.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
				}
				throw JsonSerializationException.Create(reader, "Unable to find a constructor to use for type {0}. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute.".FormatWith(CultureInfo.InvariantCulture, objectContract.UnderlyingType));
			}
			createdFromNonDefaultCreator = false;
			return obj;
		}

		private object PopulateObject(object newObject, JsonReader reader, JsonObjectContract contract, JsonProperty member, string id)
		{
			OnDeserializing(reader, contract, newObject);
			Dictionary<JsonProperty, PropertyPresence> dictionary = ((contract.HasRequiredOrDefaultValueProperties || HasFlag(Serializer._defaultValueHandling, DefaultValueHandling.Populate)) ? contract.Properties.ToDictionary((JsonProperty m) => m, (JsonProperty m) => PropertyPresence.None) : null);
			if (id != null)
			{
				AddReference(reader, id, newObject);
			}
			int depth = reader.Depth;
			bool flag = false;
			do
			{
				switch (reader.TokenType)
				{
				case JsonToken.PropertyName:
				{
					string text = reader.Value.ToString();
					if (CheckPropertyName(reader, text))
					{
						break;
					}
					try
					{
						JsonProperty closestMatchProperty = contract.Properties.GetClosestMatchProperty(text);
						if (closestMatchProperty == null)
						{
							if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
							{
								TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(reader as IJsonLineInfo, reader.Path, "Could not find member '{0}' on {1}".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType)), null);
							}
							if (Serializer._missingMemberHandling == MissingMemberHandling.Error)
							{
								throw JsonSerializationException.Create(reader, "Could not find member '{0}' on object of type '{1}'".FormatWith(CultureInfo.InvariantCulture, text, contract.UnderlyingType.Name));
							}
							if (reader.Read())
							{
								SetExtensionData(contract, member, reader, text, newObject);
							}
							break;
						}
						if (closestMatchProperty.PropertyContract == null)
						{
							closestMatchProperty.PropertyContract = GetContractSafe(closestMatchProperty.PropertyType);
						}
						JsonConverter converter = GetConverter(closestMatchProperty.PropertyContract, closestMatchProperty.MemberConverter, contract, member);
						if (!ReadForType(reader, closestMatchProperty.PropertyContract, converter != null))
						{
							throw JsonSerializationException.Create(reader, "Unexpected end when setting {0}'s value.".FormatWith(CultureInfo.InvariantCulture, text));
						}
						SetPropertyPresence(reader, closestMatchProperty, dictionary);
						if (!SetPropertyValue(closestMatchProperty, converter, contract, member, reader, newObject))
						{
							SetExtensionData(contract, member, reader, text, newObject);
						}
					}
					catch (Exception ex)
					{
						if (IsErrorHandled(newObject, contract, text, reader as IJsonLineInfo, reader.Path, ex))
						{
							HandleError(reader, readPastError: true, depth);
							break;
						}
						throw;
					}
					break;
				}
				case JsonToken.EndObject:
					flag = true;
					break;
				default:
					throw JsonSerializationException.Create(reader, "Unexpected token when deserializing object: " + reader.TokenType);
				case JsonToken.Comment:
					break;
				}
			}
			while (!flag && reader.Read());
			if (!flag)
			{
				ThrowUnexpectedEndException(reader, contract, newObject, "Unexpected end when deserializing object.");
			}
			EndObject(newObject, reader, contract, depth, dictionary);
			OnDeserialized(reader, contract, newObject);
			return newObject;
		}

		private bool CheckPropertyName(JsonReader reader, string memberName)
		{
			if (Serializer.MetadataPropertyHandling == MetadataPropertyHandling.ReadAhead)
			{
				switch (memberName)
				{
				case "$id":
				case "$ref":
				case "$type":
				case "$values":
					reader.Skip();
					return true;
				}
			}
			return false;
		}

		private void SetExtensionData(JsonObjectContract contract, JsonProperty member, JsonReader reader, string memberName, object o)
		{
			if (contract.ExtensionDataSetter != null)
			{
				try
				{
					object value = CreateValueInternal(reader, null, null, null, contract, member, null);
					contract.ExtensionDataSetter(o, memberName, value);
					return;
				}
				catch (Exception ex)
				{
					throw JsonSerializationException.Create(reader, "Error setting value in extension data for type '{0}'.".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType), ex);
				}
			}
			reader.Skip();
		}

		private void EndObject(object newObject, JsonReader reader, JsonObjectContract contract, int initialDepth, Dictionary<JsonProperty, PropertyPresence> propertiesPresence)
		{
			if (propertiesPresence == null)
			{
				return;
			}
			foreach (KeyValuePair<JsonProperty, PropertyPresence> item in propertiesPresence)
			{
				JsonProperty key = item.Key;
				PropertyPresence value = item.Value;
				if (value != 0 && value != PropertyPresence.Null)
				{
					continue;
				}
				try
				{
					Required required = key._required ?? contract.ItemRequired ?? Required.Default;
					switch (value)
					{
					case PropertyPresence.None:
						if (required == Required.AllowNull || required == Required.Always)
						{
							throw JsonSerializationException.Create(reader, "Required property '{0}' not found in JSON.".FormatWith(CultureInfo.InvariantCulture, key.PropertyName));
						}
						if (key.PropertyContract == null)
						{
							key.PropertyContract = GetContractSafe(key.PropertyType);
						}
						if (HasFlag(key.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Populate) && key.Writable && !key.Ignored)
						{
							key.ValueProvider.SetValue(newObject, EnsureType(reader, key.GetResolvedDefaultValue(), CultureInfo.InvariantCulture, key.PropertyContract, key.PropertyType));
						}
						break;
					case PropertyPresence.Null:
						if (required == Required.Always)
						{
							throw JsonSerializationException.Create(reader, "Required property '{0}' expects a value but got null.".FormatWith(CultureInfo.InvariantCulture, key.PropertyName));
						}
						break;
					}
				}
				catch (Exception ex)
				{
					if (IsErrorHandled(newObject, contract, key.PropertyName, reader as IJsonLineInfo, reader.Path, ex))
					{
						HandleError(reader, readPastError: true, initialDepth);
						continue;
					}
					throw;
				}
			}
		}

		private void SetPropertyPresence(JsonReader reader, JsonProperty property, Dictionary<JsonProperty, PropertyPresence> requiredProperties)
		{
			if (property != null && requiredProperties != null)
			{
				requiredProperties[property] = ((reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.Undefined) ? PropertyPresence.Null : PropertyPresence.Value);
			}
		}

		private void HandleError(JsonReader reader, bool readPastError, int initialDepth)
		{
			ClearErrorContext();
			if (readPastError)
			{
				reader.Skip();
				while (reader.Depth > initialDepth + 1 && reader.Read())
				{
				}
			}
		}
	}
	internal class JsonSerializerInternalWriter : JsonSerializerInternalBase
	{
		private JsonContract _rootContract;

		private int _rootLevel;

		private readonly List<object> _serializeStack = new List<object>();

		private JsonSerializerProxy _internalSerializer;

		public JsonSerializerInternalWriter(JsonSerializer serializer)
			: base(serializer)
		{
		}

		public void Serialize(JsonWriter jsonWriter, object value, Type objectType)
		{
			if (jsonWriter == null)
			{
				throw new ArgumentNullException("jsonWriter");
			}
			_rootContract = ((objectType != null) ? Serializer._contractResolver.ResolveContract(objectType) : null);
			_rootLevel = _serializeStack.Count + 1;
			JsonContract contractSafe = GetContractSafe(value);
			try
			{
				SerializeValue(jsonWriter, value, contractSafe, null, null, null);
			}
			catch (Exception ex)
			{
				if (IsErrorHandled(null, contractSafe, null, null, jsonWriter.Path, ex))
				{
					HandleError(jsonWriter, 0);
					return;
				}
				ClearErrorContext();
				throw;
			}
			finally
			{
				_rootContract = null;
			}
		}

		private JsonSerializerProxy GetInternalSerializer()
		{
			if (_internalSerializer == null)
			{
				_internalSerializer = new JsonSerializerProxy(this);
			}
			return _internalSerializer;
		}

		private JsonContract GetContractSafe(object value)
		{
			if (value == null)
			{
				return null;
			}
			return Serializer._contractResolver.ResolveContract(value.GetType());
		}

		private void SerializePrimitive(JsonWriter writer, object value, JsonPrimitiveContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (contract.TypeCode == PrimitiveTypeCode.Bytes && ShouldWriteType(TypeNameHandling.Objects, contract, member, containerContract, containerProperty))
			{
				writer.WriteStartObject();
				WriteTypeProperty(writer, contract.CreatedType);
				writer.WritePropertyName("$value", escape: false);
				JsonWriter.WriteValue(writer, contract.TypeCode, value);
				writer.WriteEndObject();
			}
			else
			{
				JsonWriter.WriteValue(writer, contract.TypeCode, value);
			}
		}

		private void SerializeValue(JsonWriter writer, object value, JsonContract valueContract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			JsonConverter jsonConverter = member?.Converter ?? containerProperty?.ItemConverter ?? containerContract?.ItemConverter ?? valueContract.Converter ?? Serializer.GetMatchingConverter(valueContract.UnderlyingType) ?? valueContract.InternalConverter;
			if (jsonConverter != null && jsonConverter.CanWrite)
			{
				SerializeConvertable(writer, jsonConverter, value, valueContract, containerContract, containerProperty);
				return;
			}
			switch (valueContract.ContractType)
			{
			case JsonContractType.Object:
				SerializeObject(writer, value, (JsonObjectContract)valueContract, member, containerContract, containerProperty);
				break;
			case JsonContractType.Array:
			{
				JsonArrayContract jsonArrayContract = (JsonArrayContract)valueContract;
				if (!jsonArrayContract.IsMultidimensionalArray)
				{
					SerializeList(writer, (IEnumerable)value, jsonArrayContract, member, containerContract, containerProperty);
				}
				else
				{
					SerializeMultidimensionalArray(writer, (Array)value, jsonArrayContract, member, containerContract, containerProperty);
				}
				break;
			}
			case JsonContractType.Primitive:
				SerializePrimitive(writer, value, (JsonPrimitiveContract)valueContract, member, containerContract, containerProperty);
				break;
			case JsonContractType.String:
				SerializeString(writer, value, (JsonStringContract)valueContract);
				break;
			case JsonContractType.Dictionary:
			{
				JsonDictionaryContract jsonDictionaryContract = (JsonDictionaryContract)valueContract;
				SerializeDictionary(writer, (value is IDictionary) ? ((IDictionary)value) : jsonDictionaryContract.CreateWrapper(value), jsonDictionaryContract, member, containerContract, containerProperty);
				break;
			}
			case JsonContractType.Dynamic:
				SerializeDynamic(writer, (IDynamicMetaObjectProvider)value, (JsonDynamicContract)valueContract, member, containerContract, containerProperty);
				break;
			case JsonContractType.Serializable:
				SerializeISerializable(writer, (ISerializable)value, (JsonISerializableContract)valueContract, member, containerContract, containerProperty);
				break;
			case JsonContractType.Linq:
				((JToken)value).WriteTo(writer, Serializer.Converters.ToArray());
				break;
			}
		}

		private bool? ResolveIsReference(JsonContract contract, JsonProperty property, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			bool? result = null;
			if (property != null)
			{
				result = property.IsReference;
			}
			if (!result.HasValue && containerProperty != null)
			{
				result = containerProperty.ItemIsReference;
			}
			if (!result.HasValue && collectionContract != null)
			{
				result = collectionContract.ItemIsReference;
			}
			if (!result.HasValue)
			{
				result = contract.IsReference;
			}
			return result;
		}

		private bool ShouldWriteReference(object value, JsonProperty property, JsonContract valueContract, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (value == null)
			{
				return false;
			}
			if (valueContract.ContractType == JsonContractType.Primitive || valueContract.ContractType == JsonContractType.String)
			{
				return false;
			}
			bool? flag = ResolveIsReference(valueContract, property, collectionContract, containerProperty);
			if (!flag.HasValue)
			{
				flag = ((valueContract.ContractType != JsonContractType.Array) ? new bool?(HasFlag(Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects)) : new bool?(HasFlag(Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays)));
			}
			if (!flag.Value)
			{
				return false;
			}
			return Serializer.GetReferenceResolver().IsReferenced(this, value);
		}

		private bool ShouldWriteProperty(object memberValue, JsonProperty property)
		{
			if (property.NullValueHandling.GetValueOrDefault(Serializer._nullValueHandling) == NullValueHandling.Ignore && memberValue == null)
			{
				return false;
			}
			if (HasFlag(property.DefaultValueHandling.GetValueOrDefault(Serializer._defaultValueHandling), DefaultValueHandling.Ignore) && MiscellaneousUtils.ValueEquals(memberValue, property.GetResolvedDefaultValue()))
			{
				return false;
			}
			return true;
		}

		private bool CheckForCircularReference(JsonWriter writer, object value, JsonProperty property, JsonContract contract, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			if (value == null || contract.ContractType == JsonContractType.Primitive || contract.ContractType == JsonContractType.String)
			{
				return true;
			}
			ReferenceLoopHandling? referenceLoopHandling = null;
			if (property != null)
			{
				referenceLoopHandling = property.ReferenceLoopHandling;
			}
			if (!referenceLoopHandling.HasValue && containerProperty != null)
			{
				referenceLoopHandling = containerProperty.ItemReferenceLoopHandling;
			}
			if (!referenceLoopHandling.HasValue && containerContract != null)
			{
				referenceLoopHandling = containerContract.ItemReferenceLoopHandling;
			}
			if (_serializeStack.IndexOf(value) != -1)
			{
				string text = "Self referencing loop detected";
				if (property != null)
				{
					text += " for property '{0}'".FormatWith(CultureInfo.InvariantCulture, property.PropertyName);
				}
				text += " with type '{0}'.".FormatWith(CultureInfo.InvariantCulture, value.GetType());
				switch (referenceLoopHandling.GetValueOrDefault(Serializer._referenceLoopHandling))
				{
				case ReferenceLoopHandling.Error:
					throw JsonSerializationException.Create(null, writer.ContainerPath, text, null);
				case ReferenceLoopHandling.Ignore:
					if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, text + ". Skipping serializing self referenced value."), null);
					}
					return false;
				case ReferenceLoopHandling.Serialize:
					if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
					{
						TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, text + ". Serializing self referenced value."), null);
					}
					return true;
				}
			}
			return true;
		}

		private void WriteReference(JsonWriter writer, object value)
		{
			string reference = GetReference(writer, value);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Writing object reference to Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, reference, value.GetType())), null);
			}
			writer.WriteStartObject();
			writer.WritePropertyName("$ref", escape: false);
			writer.WriteValue(reference);
			writer.WriteEndObject();
		}

		private string GetReference(JsonWriter writer, object value)
		{
			try
			{
				return Serializer.GetReferenceResolver().GetReference(this, value);
			}
			catch (Exception ex)
			{
				throw JsonSerializationException.Create(null, writer.ContainerPath, "Error writing object reference for '{0}'.".FormatWith(CultureInfo.InvariantCulture, value.GetType()), ex);
			}
		}

		internal static bool TryConvertToString(object value, Type type, out string s)
		{
			TypeConverter converter = ConvertUtils.GetConverter(type);
			if (converter != null && !(converter is ComponentConverter) && converter.GetType() != typeof(TypeConverter) && converter.CanConvertTo(typeof(string)))
			{
				s = converter.ConvertToInvariantString(value);
				return true;
			}
			if (value is Type)
			{
				s = ((Type)value).AssemblyQualifiedName;
				return true;
			}
			s = null;
			return false;
		}

		private void SerializeString(JsonWriter writer, object value, JsonStringContract contract)
		{
			OnSerializing(writer, contract, value);
			TryConvertToString(value, contract.UnderlyingType, out var s);
			writer.WriteValue(s);
			OnSerialized(writer, contract, value);
		}

		private void OnSerializing(JsonWriter writer, JsonContract contract, object value)
		{
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Started serializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnSerializing(value, Serializer._context);
		}

		private void OnSerialized(JsonWriter writer, JsonContract contract, object value)
		{
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
			{
				TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Finished serializing {0}".FormatWith(CultureInfo.InvariantCulture, contract.UnderlyingType)), null);
			}
			contract.InvokeOnSerialized(value, Serializer._context);
		}

		private void SerializeObject(JsonWriter writer, object value, JsonObjectContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			OnSerializing(writer, contract, value);
			_serializeStack.Add(value);
			WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			int top = writer.Top;
			for (int i = 0; i < contract.Properties.Count; i++)
			{
				JsonProperty jsonProperty = contract.Properties[i];
				try
				{
					if (CalculatePropertyValues(writer, value, contract, member, jsonProperty, out var memberContract, out var memberValue))
					{
						jsonProperty.WritePropertyName(writer);
						SerializeValue(writer, memberValue, memberContract, jsonProperty, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (IsErrorHandled(value, contract, jsonProperty.PropertyName, null, writer.ContainerPath, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			if (contract.ExtensionDataGetter != null)
			{
				IEnumerable<KeyValuePair<object, object>> enumerable = contract.ExtensionDataGetter(value);
				if (enumerable != null)
				{
					foreach (KeyValuePair<object, object> item in enumerable)
					{
						JsonContract contractSafe = GetContractSafe(item.Key);
						JsonContract contractSafe2 = GetContractSafe(item.Value);
						bool escape;
						string propertyName = GetPropertyName(writer, item.Key, contractSafe, out escape);
						if (ShouldWriteReference(item.Value, null, contractSafe2, contract, member))
						{
							writer.WritePropertyName(propertyName);
							WriteReference(writer, item.Value);
						}
						else if (CheckForCircularReference(writer, item.Value, null, contractSafe2, contract, member))
						{
							writer.WritePropertyName(propertyName);
							SerializeValue(writer, item.Value, contractSafe2, null, contract, member);
						}
					}
				}
			}
			writer.WriteEndObject();
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, value);
		}

		private bool CalculatePropertyValues(JsonWriter writer, object value, JsonContainerContract contract, JsonProperty member, JsonProperty property, out JsonContract memberContract, out object memberValue)
		{
			if (!property.Ignored && property.Readable && ShouldSerialize(writer, property, value) && IsSpecified(writer, property, value))
			{
				if (property.PropertyContract == null)
				{
					property.PropertyContract = Serializer._contractResolver.ResolveContract(property.PropertyType);
				}
				memberValue = property.ValueProvider.GetValue(value);
				memberContract = (property.PropertyContract.IsSealed ? property.PropertyContract : GetContractSafe(memberValue));
				if (ShouldWriteProperty(memberValue, property))
				{
					if (ShouldWriteReference(memberValue, property, memberContract, contract, member))
					{
						property.WritePropertyName(writer);
						WriteReference(writer, memberValue);
						return false;
					}
					if (!CheckForCircularReference(writer, memberValue, property, memberContract, contract, member))
					{
						return false;
					}
					if (memberValue == null)
					{
						JsonObjectContract jsonObjectContract = contract as JsonObjectContract;
						Required required = property._required ?? jsonObjectContract?.ItemRequired ?? Required.Default;
						if (required == Required.Always)
						{
							throw JsonSerializationException.Create(null, writer.ContainerPath, "Cannot write a null value for property '{0}'. Property requires a value.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName), null);
						}
					}
					return true;
				}
			}
			memberContract = null;
			memberValue = null;
			return false;
		}

		private void WriteObjectStart(JsonWriter writer, object value, JsonContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			writer.WriteStartObject();
			if (ResolveIsReference(contract, member, collectionContract, containerProperty) ?? HasFlag(Serializer._preserveReferencesHandling, PreserveReferencesHandling.Objects))
			{
				WriteReferenceIdProperty(writer, contract.UnderlyingType, value);
			}
			if (ShouldWriteType(TypeNameHandling.Objects, contract, member, collectionContract, containerProperty))
			{
				WriteTypeProperty(writer, contract.UnderlyingType);
			}
		}

		private void WriteReferenceIdProperty(JsonWriter writer, Type type, object value)
		{
			string reference = GetReference(writer, value);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "Writing object reference Id '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, reference, type)), null);
			}
			writer.WritePropertyName("$id", escape: false);
			writer.WriteValue(reference);
		}

		private void WriteTypeProperty(JsonWriter writer, Type type)
		{
			string typeName = ReflectionUtils.GetTypeName(type, Serializer._typeNameAssemblyFormat, Serializer._binder);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "Writing type name '{0}' for {1}.".FormatWith(CultureInfo.InvariantCulture, typeName, type)), null);
			}
			writer.WritePropertyName("$type", escape: false);
			writer.WriteValue(typeName);
		}

		private bool HasFlag(DefaultValueHandling value, DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool HasFlag(PreserveReferencesHandling value, PreserveReferencesHandling flag)
		{
			return (value & flag) == flag;
		}

		private bool HasFlag(TypeNameHandling value, TypeNameHandling flag)
		{
			return (value & flag) == flag;
		}

		private void SerializeConvertable(JsonWriter writer, JsonConverter converter, object value, JsonContract contract, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (ShouldWriteReference(value, null, contract, collectionContract, containerProperty))
			{
				WriteReference(writer, value);
			}
			else if (CheckForCircularReference(writer, value, null, contract, collectionContract, containerProperty))
			{
				_serializeStack.Add(value);
				if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
				{
					TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Started serializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, value.GetType(), converter.GetType())), null);
				}
				converter.WriteJson(writer, value, GetInternalSerializer());
				if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Info)
				{
					TraceWriter.Trace(TraceLevel.Info, JsonPosition.FormatMessage(null, writer.Path, "Finished serializing {0} with converter {1}.".FormatWith(CultureInfo.InvariantCulture, value.GetType(), converter.GetType())), null);
				}
				_serializeStack.RemoveAt(_serializeStack.Count - 1);
			}
		}

		private void SerializeList(JsonWriter writer, IEnumerable values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			object obj = ((values is IWrappedCollection wrappedCollection) ? wrappedCollection.UnderlyingCollection : values);
			OnSerializing(writer, contract, obj);
			_serializeStack.Add(obj);
			bool flag = WriteStartArray(writer, obj, contract, member, collectionContract, containerProperty);
			writer.WriteStartArray();
			int top = writer.Top;
			int num = 0;
			foreach (object value in values)
			{
				try
				{
					JsonContract jsonContract = contract.FinalItemContract ?? GetContractSafe(value);
					if (ShouldWriteReference(value, null, jsonContract, contract, member))
					{
						WriteReference(writer, value);
					}
					else if (CheckForCircularReference(writer, value, null, jsonContract, contract, member))
					{
						SerializeValue(writer, value, jsonContract, null, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (IsErrorHandled(obj, contract, num, null, writer.ContainerPath, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
				finally
				{
					num++;
				}
			}
			writer.WriteEndArray();
			if (flag)
			{
				writer.WriteEndObject();
			}
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, obj);
		}

		private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			OnSerializing(writer, contract, values);
			_serializeStack.Add(values);
			bool flag = WriteStartArray(writer, values, contract, member, collectionContract, containerProperty);
			SerializeMultidimensionalArray(writer, values, contract, member, writer.Top, new int[0]);
			if (flag)
			{
				writer.WriteEndObject();
			}
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, values);
		}

		private void SerializeMultidimensionalArray(JsonWriter writer, Array values, JsonArrayContract contract, JsonProperty member, int initialDepth, int[] indices)
		{
			int num = indices.Length;
			int[] array = new int[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = indices[i];
			}
			writer.WriteStartArray();
			for (int j = 0; j < values.GetLength(num); j++)
			{
				array[num] = j;
				if (array.Length == values.Rank)
				{
					object value = values.GetValue(array);
					try
					{
						JsonContract jsonContract = contract.FinalItemContract ?? GetContractSafe(value);
						if (ShouldWriteReference(value, null, jsonContract, contract, member))
						{
							WriteReference(writer, value);
						}
						else if (CheckForCircularReference(writer, value, null, jsonContract, contract, member))
						{
							SerializeValue(writer, value, jsonContract, null, contract, member);
						}
					}
					catch (Exception ex)
					{
						if (IsErrorHandled(values, contract, j, null, writer.ContainerPath, ex))
						{
							HandleError(writer, initialDepth + 1);
							continue;
						}
						throw;
					}
				}
				else
				{
					SerializeMultidimensionalArray(writer, values, contract, member, initialDepth + 1, array);
				}
			}
			writer.WriteEndArray();
		}

		private bool WriteStartArray(JsonWriter writer, object values, JsonArrayContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			bool flag = ResolveIsReference(contract, member, containerContract, containerProperty) ?? HasFlag(Serializer._preserveReferencesHandling, PreserveReferencesHandling.Arrays);
			bool flag2 = ShouldWriteType(TypeNameHandling.Arrays, contract, member, containerContract, containerProperty);
			bool flag3 = flag || flag2;
			if (flag3)
			{
				writer.WriteStartObject();
				if (flag)
				{
					WriteReferenceIdProperty(writer, contract.UnderlyingType, values);
				}
				if (flag2)
				{
					WriteTypeProperty(writer, values.GetType());
				}
				writer.WritePropertyName("$values", escape: false);
			}
			if (contract.ItemContract == null)
			{
				contract.ItemContract = Serializer._contractResolver.ResolveContract(contract.CollectionItemType ?? typeof(object));
			}
			return flag3;
		}

		[SecuritySafeCritical]
		private void SerializeISerializable(JsonWriter writer, ISerializable value, JsonISerializableContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			if (!JsonTypeReflector.FullyTrusted)
			{
				string format = "Type '{0}' implements ISerializable but cannot be serialized using the ISerializable interface because the current application is not fully trusted and ISerializable can expose secure data." + Environment.NewLine + "To fix this error either change the environment to be fully trusted, change the application to not deserialize the type, add JsonObjectAttribute to the type or change the JsonSerializer setting ContractResolver to use a new DefaultContractResolver with IgnoreSerializableInterface set to true." + Environment.NewLine;
				format = format.FormatWith(CultureInfo.InvariantCulture, value.GetType());
				throw JsonSerializationException.Create(null, writer.ContainerPath, format, null);
			}
			OnSerializing(writer, contract, value);
			_serializeStack.Add(value);
			WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			SerializationInfo serializationInfo = new SerializationInfo(contract.UnderlyingType, new FormatterConverter());
			value.GetObjectData(serializationInfo, Serializer._context);
			SerializationInfoEnumerator enumerator = serializationInfo.GetEnumerator();
			while (enumerator.MoveNext())
			{
				SerializationEntry current = enumerator.Current;
				JsonContract contractSafe = GetContractSafe(current.Value);
				if (ShouldWriteReference(current.Value, null, contractSafe, contract, member))
				{
					writer.WritePropertyName(current.Name);
					WriteReference(writer, current.Value);
				}
				else if (CheckForCircularReference(writer, current.Value, null, contractSafe, contract, member))
				{
					writer.WritePropertyName(current.Name);
					SerializeValue(writer, current.Value, contractSafe, null, contract, member);
				}
			}
			writer.WriteEndObject();
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, value);
		}

		private void SerializeDynamic(JsonWriter writer, IDynamicMetaObjectProvider value, JsonDynamicContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			OnSerializing(writer, contract, value);
			_serializeStack.Add(value);
			WriteObjectStart(writer, value, contract, member, collectionContract, containerProperty);
			int top = writer.Top;
			for (int i = 0; i < contract.Properties.Count; i++)
			{
				JsonProperty jsonProperty = contract.Properties[i];
				if (!jsonProperty.HasMemberAttribute)
				{
					continue;
				}
				try
				{
					if (CalculatePropertyValues(writer, value, contract, member, jsonProperty, out var memberContract, out var memberValue))
					{
						jsonProperty.WritePropertyName(writer);
						SerializeValue(writer, memberValue, memberContract, jsonProperty, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (IsErrorHandled(value, contract, jsonProperty.PropertyName, null, writer.ContainerPath, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			foreach (string dynamicMemberName in value.GetDynamicMemberNames())
			{
				if (!contract.TryGetMember(value, dynamicMemberName, out var value2))
				{
					continue;
				}
				try
				{
					JsonContract contractSafe = GetContractSafe(value2);
					if (ShouldWriteDynamicProperty(value2) && CheckForCircularReference(writer, value2, null, contractSafe, contract, member))
					{
						string name = ((contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(dynamicMemberName) : dynamicMemberName);
						writer.WritePropertyName(name);
						SerializeValue(writer, value2, contractSafe, null, contract, member);
					}
				}
				catch (Exception ex2)
				{
					if (IsErrorHandled(value, contract, dynamicMemberName, null, writer.ContainerPath, ex2))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			writer.WriteEndObject();
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, value);
		}

		private bool ShouldWriteDynamicProperty(object memberValue)
		{
			if (Serializer._nullValueHandling == NullValueHandling.Ignore && memberValue == null)
			{
				return false;
			}
			if (HasFlag(Serializer._defaultValueHandling, DefaultValueHandling.Ignore) && (memberValue == null || MiscellaneousUtils.ValueEquals(memberValue, ReflectionUtils.GetDefaultValue(memberValue.GetType()))))
			{
				return false;
			}
			return true;
		}

		private bool ShouldWriteType(TypeNameHandling typeNameHandlingFlag, JsonContract contract, JsonProperty member, JsonContainerContract containerContract, JsonProperty containerProperty)
		{
			TypeNameHandling value = member?.TypeNameHandling ?? containerProperty?.ItemTypeNameHandling ?? containerContract?.ItemTypeNameHandling ?? Serializer._typeNameHandling;
			if (HasFlag(value, typeNameHandlingFlag))
			{
				return true;
			}
			if (HasFlag(value, TypeNameHandling.Auto))
			{
				if (member != null)
				{
					if (contract.UnderlyingType != member.PropertyContract.CreatedType)
					{
						return true;
					}
				}
				else if (containerContract != null)
				{
					if (containerContract.ItemContract == null || contract.UnderlyingType != containerContract.ItemContract.CreatedType)
					{
						return true;
					}
				}
				else if (_rootContract != null && _serializeStack.Count == _rootLevel && contract.UnderlyingType != _rootContract.CreatedType)
				{
					return true;
				}
			}
			return false;
		}

		private void SerializeDictionary(JsonWriter writer, IDictionary values, JsonDictionaryContract contract, JsonProperty member, JsonContainerContract collectionContract, JsonProperty containerProperty)
		{
			object obj = ((values is IWrappedDictionary wrappedDictionary) ? wrappedDictionary.UnderlyingDictionary : values);
			OnSerializing(writer, contract, obj);
			_serializeStack.Add(obj);
			WriteObjectStart(writer, obj, contract, member, collectionContract, containerProperty);
			if (contract.ItemContract == null)
			{
				contract.ItemContract = Serializer._contractResolver.ResolveContract(contract.DictionaryValueType ?? typeof(object));
			}
			if (contract.KeyContract == null)
			{
				contract.KeyContract = Serializer._contractResolver.ResolveContract(contract.DictionaryKeyType ?? typeof(object));
			}
			int top = writer.Top;
			foreach (DictionaryEntry value2 in values)
			{
				string propertyName = GetPropertyName(writer, value2.Key, contract.KeyContract, out var escape);
				propertyName = ((contract.PropertyNameResolver != null) ? contract.PropertyNameResolver(propertyName) : propertyName);
				try
				{
					object value = value2.Value;
					JsonContract jsonContract = contract.FinalItemContract ?? GetContractSafe(value);
					if (ShouldWriteReference(value, null, jsonContract, contract, member))
					{
						writer.WritePropertyName(propertyName, escape);
						WriteReference(writer, value);
					}
					else if (CheckForCircularReference(writer, value, null, jsonContract, contract, member))
					{
						writer.WritePropertyName(propertyName, escape);
						SerializeValue(writer, value, jsonContract, null, contract, member);
					}
				}
				catch (Exception ex)
				{
					if (IsErrorHandled(obj, contract, propertyName, null, writer.ContainerPath, ex))
					{
						HandleError(writer, top);
						continue;
					}
					throw;
				}
			}
			writer.WriteEndObject();
			_serializeStack.RemoveAt(_serializeStack.Count - 1);
			OnSerialized(writer, contract, obj);
		}

		private string GetPropertyName(JsonWriter writer, object name, JsonContract contract, out bool escape)
		{
			if (contract.ContractType == JsonContractType.Primitive)
			{
				JsonPrimitiveContract jsonPrimitiveContract = (JsonPrimitiveContract)contract;
				if (jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTime || jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeNullable)
				{
					escape = false;
					StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
					DateTimeUtils.WriteDateTimeString(stringWriter, (DateTime)name, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
					return stringWriter.ToString();
				}
				if (jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeOffset || jsonPrimitiveContract.TypeCode == PrimitiveTypeCode.DateTimeOffsetNullable)
				{
					escape = false;
					StringWriter stringWriter2 = new StringWriter(CultureInfo.InvariantCulture);
					DateTimeUtils.WriteDateTimeOffsetString(stringWriter2, (DateTimeOffset)name, writer.DateFormatHandling, writer.DateFormatString, writer.Culture);
					return stringWriter2.ToString();
				}
				escape = true;
				return Convert.ToString(name, CultureInfo.InvariantCulture);
			}
			if (TryConvertToString(name, name.GetType(), out var s))
			{
				escape = true;
				return s;
			}
			escape = true;
			return name.ToString();
		}

		private void HandleError(JsonWriter writer, int initialDepth)
		{
			ClearErrorContext();
			if (writer.WriteState == WriteState.Property)
			{
				writer.WriteNull();
			}
			while (writer.Top > initialDepth)
			{
				writer.WriteEnd();
			}
		}

		private bool ShouldSerialize(JsonWriter writer, JsonProperty property, object target)
		{
			if (property.ShouldSerialize == null)
			{
				return true;
			}
			bool flag = property.ShouldSerialize(target);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "ShouldSerialize result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, flag)), null);
			}
			return flag;
		}

		private bool IsSpecified(JsonWriter writer, JsonProperty property, object target)
		{
			if (property.GetIsSpecified == null)
			{
				return true;
			}
			bool flag = property.GetIsSpecified(target);
			if (TraceWriter != null && TraceWriter.LevelFilter >= TraceLevel.Verbose)
			{
				TraceWriter.Trace(TraceLevel.Verbose, JsonPosition.FormatMessage(null, writer.Path, "IsSpecified result for property '{0}' on {1}: {2}".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, property.DeclaringType, flag)), null);
			}
			return flag;
		}
	}
	internal class JsonSerializerProxy : JsonSerializer
	{
		private readonly JsonSerializerInternalReader _serializerReader;

		private readonly JsonSerializerInternalWriter _serializerWriter;

		private readonly JsonSerializer _serializer;

		public override IReferenceResolver ReferenceResolver
		{
			get
			{
				return _serializer.ReferenceResolver;
			}
			set
			{
				_serializer.ReferenceResolver = value;
			}
		}

		public override ITraceWriter TraceWriter
		{
			get
			{
				return _serializer.TraceWriter;
			}
			set
			{
				_serializer.TraceWriter = value;
			}
		}

		public override JsonConverterCollection Converters => _serializer.Converters;

		public override DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _serializer.DefaultValueHandling;
			}
			set
			{
				_serializer.DefaultValueHandling = value;
			}
		}

		public override IContractResolver ContractResolver
		{
			get
			{
				return _serializer.ContractResolver;
			}
			set
			{
				_serializer.ContractResolver = value;
			}
		}

		public override MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return _serializer.MissingMemberHandling;
			}
			set
			{
				_serializer.MissingMemberHandling = value;
			}
		}

		public override NullValueHandling NullValueHandling
		{
			get
			{
				return _serializer.NullValueHandling;
			}
			set
			{
				_serializer.NullValueHandling = value;
			}
		}

		public override ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _serializer.ObjectCreationHandling;
			}
			set
			{
				_serializer.ObjectCreationHandling = value;
			}
		}

		public override ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _serializer.ReferenceLoopHandling;
			}
			set
			{
				_serializer.ReferenceLoopHandling = value;
			}
		}

		public override PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return _serializer.PreserveReferencesHandling;
			}
			set
			{
				_serializer.PreserveReferencesHandling = value;
			}
		}

		public override TypeNameHandling TypeNameHandling
		{
			get
			{
				return _serializer.TypeNameHandling;
			}
			set
			{
				_serializer.TypeNameHandling = value;
			}
		}

		public override MetadataPropertyHandling MetadataPropertyHandling
		{
			get
			{
				return _serializer.MetadataPropertyHandling;
			}
			set
			{
				_serializer.MetadataPropertyHandling = value;
			}
		}

		public override FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return _serializer.TypeNameAssemblyFormat;
			}
			set
			{
				_serializer.TypeNameAssemblyFormat = value;
			}
		}

		public override ConstructorHandling ConstructorHandling
		{
			get
			{
				return _serializer.ConstructorHandling;
			}
			set
			{
				_serializer.ConstructorHandling = value;
			}
		}

		public override SerializationBinder Binder
		{
			get
			{
				return _serializer.Binder;
			}
			set
			{
				_serializer.Binder = value;
			}
		}

		public override StreamingContext Context
		{
			get
			{
				return _serializer.Context;
			}
			set
			{
				_serializer.Context = value;
			}
		}

		public override Formatting Formatting
		{
			get
			{
				return _serializer.Formatting;
			}
			set
			{
				_serializer.Formatting = value;
			}
		}

		public override DateFormatHandling DateFormatHandling
		{
			get
			{
				return _serializer.DateFormatHandling;
			}
			set
			{
				_serializer.DateFormatHandling = value;
			}
		}

		public override DateTimeZoneHandling DateTimeZoneHandling
		{
			get
			{
				return _serializer.DateTimeZoneHandling;
			}
			set
			{
				_serializer.DateTimeZoneHandling = value;
			}
		}

		public override DateParseHandling DateParseHandling
		{
			get
			{
				return _serializer.DateParseHandling;
			}
			set
			{
				_serializer.DateParseHandling = value;
			}
		}

		public override FloatFormatHandling FloatFormatHandling
		{
			get
			{
				return _serializer.FloatFormatHandling;
			}
			set
			{
				_serializer.FloatFormatHandling = value;
			}
		}

		public override FloatParseHandling FloatParseHandling
		{
			get
			{
				return _serializer.FloatParseHandling;
			}
			set
			{
				_serializer.FloatParseHandling = value;
			}
		}

		public override StringEscapeHandling StringEscapeHandling
		{
			get
			{
				return _serializer.StringEscapeHandling;
			}
			set
			{
				_serializer.StringEscapeHandling = value;
			}
		}

		public override string DateFormatString
		{
			get
			{
				return _serializer.DateFormatString;
			}
			set
			{
				_serializer.DateFormatString = value;
			}
		}

		public override CultureInfo Culture
		{
			get
			{
				return _serializer.Culture;
			}
			set
			{
				_serializer.Culture = value;
			}
		}

		public override int? MaxDepth
		{
			get
			{
				return _serializer.MaxDepth;
			}
			set
			{
				_serializer.MaxDepth = value;
			}
		}

		public override bool CheckAdditionalContent
		{
			get
			{
				return _serializer.CheckAdditionalContent;
			}
			set
			{
				_serializer.CheckAdditionalContent = value;
			}
		}

		public override event EventHandler<ErrorEventArgs> Error
		{
			add
			{
				_serializer.Error += value;
			}
			remove
			{
				_serializer.Error -= value;
			}
		}

		internal JsonSerializerInternalBase GetInternalSerializer()
		{
			if (_serializerReader != null)
			{
				return _serializerReader;
			}
			return _serializerWriter;
		}

		public JsonSerializerProxy(JsonSerializerInternalReader serializerReader)
		{
			ValidationUtils.ArgumentNotNull(serializerReader, "serializerReader");
			_serializerReader = serializerReader;
			_serializer = serializerReader.Serializer;
		}

		public JsonSerializerProxy(JsonSerializerInternalWriter serializerWriter)
		{
			ValidationUtils.ArgumentNotNull(serializerWriter, "serializerWriter");
			_serializerWriter = serializerWriter;
			_serializer = serializerWriter.Serializer;
		}

		internal override object DeserializeInternal(JsonReader reader, Type objectType)
		{
			if (_serializerReader != null)
			{
				return _serializerReader.Deserialize(reader, objectType, checkAdditionalContent: false);
			}
			return _serializer.Deserialize(reader, objectType);
		}

		internal override void PopulateInternal(JsonReader reader, object target)
		{
			if (_serializerReader != null)
			{
				_serializerReader.Populate(reader, target);
			}
			else
			{
				_serializer.Populate(reader, target);
			}
		}

		internal override void SerializeInternal(JsonWriter jsonWriter, object value, Type rootType)
		{
			if (_serializerWriter != null)
			{
				_serializerWriter.Serialize(jsonWriter, value, rootType);
			}
			else
			{
				_serializer.Serialize(jsonWriter, value);
			}
		}
	}
	public class JsonStringContract : JsonPrimitiveContract
	{
		public JsonStringContract(Type underlyingType)
			: base(underlyingType)
		{
			ContractType = JsonContractType.String;
		}
	}
	internal static class JsonTypeReflector
	{
		public const string IdPropertyName = "$id";

		public const string RefPropertyName = "$ref";

		public const string TypePropertyName = "$type";

		public const string ValuePropertyName = "$value";

		public const string ArrayValuesPropertyName = "$values";

		public const string ShouldSerializePrefix = "ShouldSerialize";

		public const string SpecifiedPostfix = "Specified";

		private static bool? _dynamicCodeGeneration;

		private static bool? _fullyTrusted;

		private static readonly ThreadSafeStore<Type, Func<object[], JsonConverter>> JsonConverterCreatorCache = new ThreadSafeStore<Type, Func<object[], JsonConverter>>(GetJsonConverterCreator);

		private static readonly ThreadSafeStore<Type, Type> AssociatedMetadataTypesCache = new ThreadSafeStore<Type, Type>(GetAssociateMetadataTypeFromAttribute);

		private static ReflectionObject _metadataTypeAttributeReflectionObject;

		public static bool DynamicCodeGeneration
		{
			[SecuritySafeCritical]
			get
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				if (!_dynamicCodeGeneration.HasValue)
				{
					try
					{
						((CodeAccessPermission)new ReflectionPermission((ReflectionPermissionFlag)2)).Demand();
						((CodeAccessPermission)new ReflectionPermission((ReflectionPermissionFlag)8)).Demand();
						((CodeAccessPermission)new SecurityPermission(SecurityPermissionFlag.SkipVerification)).Demand();
						((CodeAccessPermission)new SecurityPermission(SecurityPermissionFlag.UnmanagedCode)).Demand();
						((CodeAccessPermission)new SecurityPermission(PermissionState.Unrestricted)).Demand();
						_dynamicCodeGeneration = true;
					}
					catch (Exception)
					{
						_dynamicCodeGeneration = false;
					}
				}
				return _dynamicCodeGeneration.Value;
			}
		}

		public static bool FullyTrusted
		{
			get
			{
				if (!_fullyTrusted.HasValue)
				{
					AppDomain currentDomain = AppDomain.CurrentDomain;
					_fullyTrusted = currentDomain.IsHomogenous && currentDomain.IsFullyTrusted;
				}
				return _fullyTrusted.Value;
			}
		}

		public static ReflectionDelegateFactory ReflectionDelegateFactory
		{
			get
			{
				if (DynamicCodeGeneration)
				{
					return DynamicReflectionDelegateFactory.Instance;
				}
				return LateBoundReflectionDelegateFactory.Instance;
			}
		}

		public static T GetCachedAttribute<T>(object attributeProvider) where T : Attribute
		{
			return CachedAttributeGetter<T>.GetAttribute(attributeProvider);
		}

		public static DataContractAttribute GetDataContractAttribute(Type type)
		{
			Type type2 = type;
			while (type2 != null)
			{
				DataContractAttribute attribute = CachedAttributeGetter<DataContractAttribute>.GetAttribute(type2);
				if (attribute != null)
				{
					return attribute;
				}
				type2 = type2.BaseType();
			}
			return null;
		}

		public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
		{
			if (memberInfo.MemberType() == MemberTypes.Field)
			{
				return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			DataMemberAttribute attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
			if (attribute == null && propertyInfo.IsVirtual())
			{
				Type type = propertyInfo.DeclaringType;
				while (attribute == null && type != null)
				{
					PropertyInfo propertyInfo2 = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(type, propertyInfo);
					if (propertyInfo2 != null && propertyInfo2.IsVirtual())
					{
						attribute = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo2);
					}
					type = type.BaseType();
				}
			}
			return attribute;
		}

		public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
		{
			JsonObjectAttribute cachedAttribute = GetCachedAttribute<JsonObjectAttribute>(objectType);
			if (cachedAttribute != null)
			{
				return cachedAttribute.MemberSerialization;
			}
			DataContractAttribute dataContractAttribute = GetDataContractAttribute(objectType);
			if (dataContractAttribute != null)
			{
				return MemberSerialization.OptIn;
			}
			if (!ignoreSerializableAttribute)
			{
				SerializableAttribute cachedAttribute2 = GetCachedAttribute<SerializableAttribute>(objectType);
				if (cachedAttribute2 != null)
				{
					return MemberSerialization.Fields;
				}
			}
			return MemberSerialization.OptOut;
		}

		public static JsonConverter GetJsonConverter(object attributeProvider)
		{
			JsonConverterAttribute cachedAttribute = GetCachedAttribute<JsonConverterAttribute>(attributeProvider);
			if (cachedAttribute != null)
			{
				Func<object[], JsonConverter> func = JsonConverterCreatorCache.Get(cachedAttribute.ConverterType);
				if (func != null)
				{
					return func(cachedAttribute.ConverterParameters);
				}
			}
			return null;
		}

		public static JsonConverter CreateJsonConverterInstance(Type converterType, object[] converterArgs)
		{
			Func<object[], JsonConverter> func = JsonConverterCreatorCache.Get(converterType);
			return func(converterArgs);
		}

		private static Func<object[], JsonConverter> GetJsonConverterCreator(Type converterType)
		{
			Func<object> defaultConstructor = (ReflectionUtils.HasDefaultConstructor(converterType, nonPublic: false) ? ReflectionDelegateFactory.CreateDefaultConstructor<object>(converterType) : null);
			return delegate(object[] parameters)
			{
				try
				{
					if (parameters != null)
					{
						ObjectConstructor<object> objectConstructor = null;
						Type[] types = parameters.Select((object param) => param.GetType()).ToArray();
						ConstructorInfo constructor = converterType.GetConstructor(types);
						if (!(null != constructor))
						{
							throw new JsonException("No matching parameterized constructor found for '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType));
						}
						objectConstructor = ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
						return (JsonConverter)objectConstructor(parameters);
					}
					if (defaultConstructor == null)
					{
						throw new JsonException("No parameterless constructor defined for '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType));
					}
					return (JsonConverter)defaultConstructor();
				}
				catch (Exception innerException)
				{
					throw new JsonException("Error creating '{0}'.".FormatWith(CultureInfo.InvariantCulture, converterType), innerException);
				}
			};
		}

		public static TypeConverter GetTypeConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type);
		}

		private static Type GetAssociatedMetadataType(Type type)
		{
			return AssociatedMetadataTypesCache.Get(type);
		}

		private static Type GetAssociateMetadataTypeFromAttribute(Type type)
		{
			Attribute[] attributes = ReflectionUtils.GetAttributes(type, null, inherit: true);
			Attribute[] array = attributes;
			foreach (Attribute attribute in array)
			{
				Type type2 = attribute.GetType();
				if (string.Equals(type2.FullName, "System.ComponentModel.DataAnnotations.MetadataTypeAttribute", StringComparison.Ordinal))
				{
					if (_metadataTypeAttributeReflectionObject == null)
					{
						_metadataTypeAttributeReflectionObject = ReflectionObject.Create(type2, "MetadataClassType");
					}
					return (Type)_metadataTypeAttributeReflectionObject.GetValue(attribute, "MetadataClassType");
				}
			}
			return null;
		}

		private static T GetAttribute<T>(Type type) where T : Attribute
		{
			Type associatedMetadataType = GetAssociatedMetadataType(type);
			T attribute;
			if (associatedMetadataType != null)
			{
				attribute = ReflectionUtils.GetAttribute<T>(associatedMetadataType, inherit: true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(type, inherit: true);
			if (attribute != null)
			{
				return attribute;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type attributeProvider in interfaces)
			{
				attribute = ReflectionUtils.GetAttribute<T>(attributeProvider, inherit: true);
				if (attribute != null)
				{
					return attribute;
				}
			}
			return null;
		}

		private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
		{
			Type associatedMetadataType = GetAssociatedMetadataType(memberInfo.DeclaringType);
			T attribute;
			if (associatedMetadataType != null)
			{
				MemberInfo memberInfoFromType = ReflectionUtils.GetMemberInfoFromType(associatedMetadataType, memberInfo);
				if (memberInfoFromType != null)
				{
					attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType, inherit: true);
					if (attribute != null)
					{
						return attribute;
					}
				}
			}
			attribute = ReflectionUtils.GetAttribute<T>(memberInfo, inherit: true);
			if (attribute != null)
			{
				return attribute;
			}
			if (memberInfo.DeclaringType != null)
			{
				Type[] interfaces = memberInfo.DeclaringType.GetInterfaces();
				foreach (Type targetType in interfaces)
				{
					MemberInfo memberInfoFromType2 = ReflectionUtils.GetMemberInfoFromType(targetType, memberInfo);
					if (memberInfoFromType2 != null)
					{
						attribute = ReflectionUtils.GetAttribute<T>(memberInfoFromType2, inherit: true);
						if (attribute != null)
						{
							return attribute;
						}
					}
				}
			}
			return null;
		}

		public static T GetAttribute<T>(object provider) where T : Attribute
		{
			Type type = provider as Type;
			if (type != null)
			{
				return GetAttribute<T>(type);
			}
			MemberInfo memberInfo = provider as MemberInfo;
			if (memberInfo != null)
			{
				return GetAttribute<T>(memberInfo);
			}
			return ReflectionUtils.GetAttribute<T>(provider, inherit: true);
		}
	}
	internal static class CachedAttributeGetter<T> where T : Attribute
	{
		private static readonly ThreadSafeStore<object, T> TypeAttributeCache = new ThreadSafeStore<object, T>(JsonTypeReflector.GetAttribute<T>);

		public static T GetAttribute(object type)
		{
			return TypeAttributeCache.Get(type);
		}
	}
	public class ReflectionValueProvider : IValueProvider
	{
		private readonly MemberInfo _memberInfo;

		public ReflectionValueProvider(MemberInfo memberInfo)
		{
			ValidationUtils.ArgumentNotNull(memberInfo, "memberInfo");
			_memberInfo = memberInfo;
		}

		public void SetValue(object target, object value)
		{
			try
			{
				ReflectionUtils.SetMemberValue(_memberInfo, target, value);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}

		public object GetValue(object target)
		{
			try
			{
				return ReflectionUtils.GetMemberValue(_memberInfo, target);
			}
			catch (Exception innerException)
			{
				throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith(CultureInfo.InvariantCulture, _memberInfo.Name, target.GetType()), innerException);
			}
		}
	}
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class OnErrorAttribute : Attribute
	{
	}
}
namespace Newtonsoft.Json.Utilities
{
	internal class Base64Encoder
	{
		private const int Base64LineSize = 76;

		private const int LineSizeInBytes = 57;

		private readonly char[] _charsLine = new char[76];

		private readonly TextWriter _writer;

		private byte[] _leftOverBytes;

		private int _leftOverBytesCount;

		public Base64Encoder(TextWriter writer)
		{
			ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = writer;
		}

		public void Encode(byte[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (count > buffer.Length - index)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (_leftOverBytesCount > 0)
			{
				int leftOverBytesCount = _leftOverBytesCount;
				while (leftOverBytesCount < 3 && count > 0)
				{
					_leftOverBytes[leftOverBytesCount++] = buffer[index++];
					count--;
				}
				if (count == 0 && leftOverBytesCount < 3)
				{
					_leftOverBytesCount = leftOverBytesCount;
					return;
				}
				int count2 = Convert.ToBase64CharArray(_leftOverBytes, 0, 3, _charsLine, 0);
				WriteChars(_charsLine, 0, count2);
			}
			_leftOverBytesCount = count % 3;
			if (_leftOverBytesCount > 0)
			{
				count -= _leftOverBytesCount;
				if (_leftOverBytes == null)
				{
					_leftOverBytes = new byte[3];
				}
				for (int i = 0; i < _leftOverBytesCount; i++)
				{
					_leftOverBytes[i] = buffer[index + count + i];
				}
			}
			int num = index + count;
			int num2 = 57;
			while (index < num)
			{
				if (index + num2 > num)
				{
					num2 = num - index;
				}
				int count3 = Convert.ToBase64CharArray(buffer, index, num2, _charsLine, 0);
				WriteChars(_charsLine, 0, count3);
				index += num2;
			}
		}

		public void Flush()
		{
			if (_leftOverBytesCount > 0)
			{
				int count = Convert.ToBase64CharArray(_leftOverBytes, 0, _leftOverBytesCount, _charsLine, 0);
				WriteChars(_charsLine, 0, count);
				_leftOverBytesCount = 0;
			}
		}

		private void WriteChars(char[] chars, int index, int count)
		{
			_writer.Write(chars, index, count);
		}
	}
	internal sealed class DynamicProxyMetaObject<T> : DynamicMetaObject
	{
		private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

		private sealed class GetBinderAdapter : GetMemberBinder
		{
			internal GetBinderAdapter(InvokeMemberBinder binder)
				: base(binder.Name, binder.IgnoreCase)
			{
			}

			public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
			{
				throw new NotSupportedException();
			}
		}

		private readonly DynamicProxy<T> _proxy;

		private readonly bool _dontFallbackFirst;

		private static readonly Expression[] NoArgs = new Expression[0];

		private new T Value => (T)base.Value;

		internal DynamicProxyMetaObject(Expression expression, T value, DynamicProxy<T> proxy, bool dontFallbackFirst)
			: base(expression, BindingRestrictions.Empty, value)
		{
			_proxy = proxy;
			_dontFallbackFirst = dontFallbackFirst;
		}

		private bool IsOverridden(string method)
		{
			return ReflectionUtils.IsMethodOverridden(_proxy.GetType(), typeof(DynamicProxy<T>), method);
		}

		public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
		{
			if (!IsOverridden("TryGetMember"))
			{
				return base.BindGetMember(binder);
			}
			return CallMethodWithResult("TryGetMember", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackGetMember(this, e));
		}

		public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
		{
			if (!IsOverridden("TrySetMember"))
			{
				return base.BindSetMember(binder, value);
			}
			return CallMethodReturnLast("TrySetMember", binder, GetArgs(value), (DynamicMetaObject e) => binder.FallbackSetMember(this, value, e));
		}

		public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
		{
			if (!IsOverridden("TryDeleteMember"))
			{
				return base.BindDeleteMember(binder);
			}
			return CallMethodNoResult("TryDeleteMember", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackDeleteMember(this, e));
		}

		public override DynamicMetaObject BindConvert(ConvertBinder binder)
		{
			if (!IsOverridden("TryConvert"))
			{
				return base.BindConvert(binder);
			}
			return CallMethodWithResult("TryConvert", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackConvert(this, e));
		}

		public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryInvokeMember"))
			{
				return base.BindInvokeMember(binder, args);
			}
			Fallback fallback = (DynamicMetaObject e) => binder.FallbackInvokeMember(this, args, e);
			DynamicMetaObject dynamicMetaObject = BuildCallMethodWithResult("TryInvokeMember", binder, GetArgArray(args), BuildCallMethodWithResult("TryGetMember", new GetBinderAdapter(binder), NoArgs, fallback(null), (DynamicMetaObject e) => binder.FallbackInvoke(e, args, null)), null);
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryCreateInstance"))
			{
				return base.BindCreateInstance(binder, args);
			}
			return CallMethodWithResult("TryCreateInstance", binder, GetArgArray(args), (DynamicMetaObject e) => binder.FallbackCreateInstance(this, args, e));
		}

		public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
		{
			if (!IsOverridden("TryInvoke"))
			{
				return base.BindInvoke(binder, args);
			}
			return CallMethodWithResult("TryInvoke", binder, GetArgArray(args), (DynamicMetaObject e) => binder.FallbackInvoke(this, args, e));
		}

		public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
		{
			if (!IsOverridden("TryBinaryOperation"))
			{
				return base.BindBinaryOperation(binder, arg);
			}
			return CallMethodWithResult("TryBinaryOperation", binder, GetArgs(arg), (DynamicMetaObject e) => binder.FallbackBinaryOperation(this, arg, e));
		}

		public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
		{
			if (!IsOverridden("TryUnaryOperation"))
			{
				return base.BindUnaryOperation(binder);
			}
			return CallMethodWithResult("TryUnaryOperation", binder, NoArgs, (DynamicMetaObject e) => binder.FallbackUnaryOperation(this, e));
		}

		public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!IsOverridden("TryGetIndex"))
			{
				return base.BindGetIndex(binder, indexes);
			}
			return CallMethodWithResult("TryGetIndex", binder, GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackGetIndex(this, indexes, e));
		}

		public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
		{
			if (!IsOverridden("TrySetIndex"))
			{
				return base.BindSetIndex(binder, indexes, value);
			}
			return CallMethodReturnLast("TrySetIndex", binder, GetArgArray(indexes, value), (DynamicMetaObject e) => binder.FallbackSetIndex(this, indexes, value, e));
		}

		public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
		{
			if (!IsOverridden("TryDeleteIndex"))
			{
				return base.BindDeleteIndex(binder, indexes);
			}
			return CallMethodNoResult("TryDeleteIndex", binder, GetArgArray(indexes), (DynamicMetaObject e) => binder.FallbackDeleteIndex(this, indexes, e));
		}

		private static Expression[] GetArgs(params DynamicMetaObject[] args)
		{
			return args.Select((DynamicMetaObject arg) => System.Linq.Expressions.Expression.Convert(arg.Expression, typeof(object))).ToArray();
		}

		private static Expression[] GetArgArray(DynamicMetaObject[] args)
		{
			return new NewArrayExpression[1] { System.Linq.Expressions.Expression.NewArrayInit(typeof(object), GetArgs(args)) };
		}

		private static Expression[] GetArgArray(DynamicMetaObject[] args, DynamicMetaObject value)
		{
			return new Expression[2]
			{
				System.Linq.Expressions.Expression.NewArrayInit(typeof(object), GetArgs(args)),
				System.Linq.Expressions.Expression.Convert(value.Expression, typeof(object))
			};
		}

		private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
		{
			Type type = binder.GetType();
			while (!type.IsVisible())
			{
				type = type.BaseType();
			}
			return System.Linq.Expressions.Expression.Constant(binder, type);
		}

		private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback, Fallback fallbackInvoke = null)
		{
			DynamicMetaObject fallbackResult = fallback(null);
			DynamicMetaObject dynamicMetaObject = BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject);
			}
			return dynamicMetaObject;
		}

		private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicMetaObject fallbackResult, Fallback fallbackInvoke)
		{
			ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			list.Add(parameterExpression);
			DynamicMetaObject dynamicMetaObject = new DynamicMetaObject(parameterExpression, BindingRestrictions.Empty);
			if (binder.ReturnType != typeof(object))
			{
				UnaryExpression expression = System.Linq.Expressions.Expression.Convert(dynamicMetaObject.Expression, binder.ReturnType);
				dynamicMetaObject = new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
			}
			if (fallbackInvoke != null)
			{
				dynamicMetaObject = fallbackInvoke(dynamicMetaObject);
			}
			return new DynamicMetaObject(System.Linq.Expressions.Expression.Block(new ParameterExpression[1] { parameterExpression }, System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), dynamicMetaObject.Expression, fallbackResult.Expression, binder.ReturnType)), GetRestrictions().Merge(dynamicMetaObject.Restrictions).Merge(fallbackResult.Restrictions));
		}

		private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			ParameterExpression parameterExpression = System.Linq.Expressions.Expression.Parameter(typeof(object), null);
			IList<Expression> list = new List<Expression>();
			list.Add(System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			list[args.Length + 1] = System.Linq.Expressions.Expression.Assign(parameterExpression, list[args.Length + 1]);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(System.Linq.Expressions.Expression.Block(new ParameterExpression[1] { parameterExpression }, System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), parameterExpression, dynamicMetaObject.Expression, typeof(object))), GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
		{
			DynamicMetaObject dynamicMetaObject = fallback(null);
			IList<Expression> list = new List<Expression>();
			list.Add(System.Linq.Expressions.Expression.Convert(base.Expression, typeof(T)));
			list.Add(Constant(binder));
			list.AddRange(args);
			DynamicMetaObject dynamicMetaObject2 = new DynamicMetaObject(System.Linq.Expressions.Expression.Condition(System.Linq.Expressions.Expression.Call(System.Linq.Expressions.Expression.Constant(_proxy), typeof(DynamicProxy<T>).GetMethod(methodName), list), System.Linq.Expressions.Expression.Empty(), dynamicMetaObject.Expression, typeof(void)), GetRestrictions().Merge(dynamicMetaObject.Restrictions));
			if (!_dontFallbackFirst)
			{
				return fallback(dynamicMetaObject2);
			}
			return dynamicMetaObject2;
		}

		private BindingRestrictions GetRestrictions()
		{
			if (Value != null || !base.HasValue)
			{
				return BindingRestrictions.GetTypeRestriction(base.Expression, base.LimitType);
			}
			return BindingRestrictions.GetInstanceRestriction(base.Expression, null);
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return _proxy.GetDynamicMemberNames(Value);
		}
	}
	internal static class DynamicUtils
	{
		internal static class BinderWrapper
		{
			public const string CSharpAssemblyName = "Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private const string BinderTypeName = "Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private const string CSharpArgumentInfoTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private const string CSharpArgumentInfoFlagsTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private const string CSharpBinderFlagsTypeName = "Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";

			private static object _getCSharpArgumentInfoArray;

			private static object _setCSharpArgumentInfoArray;

			private static MethodCall<object, object> _getMemberCall;

			private static MethodCall<object, object> _setMemberCall;

			private static bool _init;

			private static void Init()
			{
				if (!_init)
				{
					Type type = Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", throwOnError: false);
					if (type == null)
					{
						throw new InvalidOperationException("Could not resolve type '{0}'. You may need to add a reference to Microsoft.CSharp.dll to work with dynamic types.".FormatWith(CultureInfo.InvariantCulture, "Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"));
					}
					int[] values = new int[1];
					_getCSharpArgumentInfoArray = CreateSharpArgumentInfoArray(values);
					_setCSharpArgumentInfoArray = CreateSharpArgumentInfoArray(0, 3);
					CreateMemberCalls();
					_init = true;
				}
			}

			private static object CreateSharpArgumentInfoArray(params int[] values)
			{
				Type type = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				Type type2 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfoFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
				Array array = Array.CreateInstance(type, values.Length);
				for (int i = 0; i < values.Length; i++)
				{
					MethodInfo method = type.GetMethod("Create", new Type[2]
					{
						type2,
						typeof(string)
					});
					object value = method.Invoke(null, new object[2] { 0, null });
					array.SetValue(value, i);
				}
				return array;
			}

			private static void CreateMemberCalls()
			{
				Type type = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", throwOnError: true);
				Type type2 = Type.GetType("Microsoft.CSharp.RuntimeBinder.CSharpBinderFlags, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", throwOnError: true);
				Type type3 = Type.GetType("Microsoft.CSharp.RuntimeBinder.Binder, Microsoft.CSharp, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", throwOnError: true);
				Type type4 = typeof(IEnumerable<>).MakeGenericType(type);
				MethodInfo method = type3.GetMethod("GetMember", new Type[4]
				{
					type2,
					typeof(string),
					typeof(Type),
					type4
				});
				_getMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
				MethodInfo method2 = type3.GetMethod("SetMember", new Type[4]
				{
					type2,
					typeof(string),
					typeof(Type),
					type4
				});
				_setMemberCall = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method2);
			}

			public static CallSiteBinder GetMember(string name, Type context)
			{
				Init();
				return (CallSiteBinder)_getMemberCall(null, 0, name, context, _getCSharpArgumentInfoArray);
			}

			public static CallSiteBinder SetMember(string name, Type context)
			{
				Init();
				return (CallSiteBinder)_setMemberCall(null, 0, name, context, _setCSharpArgumentInfoArray);
			}
		}

		public static IEnumerable<string> GetDynamicMemberNames(this IDynamicMetaObjectProvider dynamicProvider)
		{
			DynamicMetaObject metaObject = dynamicProvider.GetMetaObject(Expression.Constant(dynamicProvider));
			return metaObject.GetDynamicMemberNames();
		}
	}
	internal class NoThrowGetBinderMember : GetMemberBinder
	{
		private readonly GetMemberBinder _innerBinder;

		public NoThrowGetBinderMember(GetMemberBinder innerBinder)
			: base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			_innerBinder = innerBinder;
		}

		public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = _innerBinder.Bind(target, new DynamicMetaObject[0]);
			NoThrowExpressionVisitor noThrowExpressionVisitor = new NoThrowExpressionVisitor();
			Expression expression = noThrowExpressionVisitor.Visit(dynamicMetaObject.Expression);
			return new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
		}
	}
	internal class NoThrowSetBinderMember : SetMemberBinder
	{
		private readonly SetMemberBinder _innerBinder;

		public NoThrowSetBinderMember(SetMemberBinder innerBinder)
			: base(innerBinder.Name, innerBinder.IgnoreCase)
		{
			_innerBinder = innerBinder;
		}

		public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
		{
			DynamicMetaObject dynamicMetaObject = _innerBinder.Bind(target, new DynamicMetaObject[1] { value });
			NoThrowExpressionVisitor noThrowExpressionVisitor = new NoThrowExpressionVisitor();
			Expression expression = noThrowExpressionVisitor.Visit(dynamicMetaObject.Expression);
			return new DynamicMetaObject(expression, dynamicMetaObject.Restrictions);
		}
	}
	internal class NoThrowExpressionVisitor : ExpressionVisitor
	{
		internal static readonly object ErrorResult = new object();

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			if (node.IfFalse.NodeType == ExpressionType.Throw)
			{
				return Expression.Condition(node.Test, node.IfTrue, Expression.Constant(ErrorResult));
			}
			return base.VisitConditional(node);
		}
	}
	internal abstract class ReflectionDelegateFactory
	{
		public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return CreateGet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return CreateGet<T>(fieldInfo);
			}
			throw new Exception("Could not create getter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
		{
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return CreateSet<T>(propertyInfo);
			}
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return CreateSet<T>(fieldInfo);
			}
			throw new Exception("Could not create setter for {0}.".FormatWith(CultureInfo.InvariantCulture, memberInfo));
		}

		public abstract MethodCall<T, object> CreateMethodCall<T>(MethodBase method);

		public abstract ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method);

		public abstract Func<T> CreateDefaultConstructor<T>(Type type);

		public abstract Func<T, object> CreateGet<T>(PropertyInfo propertyInfo);

		public abstract Func<T, object> CreateGet<T>(FieldInfo fieldInfo);

		public abstract Action<T, object> CreateSet<T>(FieldInfo fieldInfo);

		public abstract Action<T, object> CreateSet<T>(PropertyInfo propertyInfo);
	}
	internal class DynamicReflectionDelegateFactory : ReflectionDelegateFactory
	{
		public static DynamicReflectionDelegateFactory Instance = new DynamicReflectionDelegateFactory();

		private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
		{
			return (!owner.IsInterface()) ? new DynamicMethod(name, returnType, parameterTypes, owner, skipVisibility: true) : new DynamicMethod(name, returnType, parameterTypes, owner.Module, skipVisibility: true);
		}

		public override ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod(method.ToString(), typeof(object), new Type[1] { typeof(object[]) }, method.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateMethodCallIL(method, iLGenerator, 0);
			return (ObjectConstructor<object>)dynamicMethod.CreateDelegate(typeof(ObjectConstructor<object>));
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod(method.ToString(), typeof(object), new Type[2]
			{
				typeof(object),
				typeof(object[])
			}, method.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateMethodCallIL(method, iLGenerator, 1);
			return (MethodCall<T, object>)dynamicMethod.CreateDelegate(typeof(MethodCall<T, object>));
		}

		private void GenerateCreateMethodCallIL(MethodBase method, ILGenerator generator, int argsIndex)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Label label = generator.DefineLabel();
			generator.Emit(OpCodes.Ldarg, argsIndex);
			generator.Emit(OpCodes.Ldlen);
			generator.Emit(OpCodes.Ldc_I4, parameters.Length);
			generator.Emit(OpCodes.Beq, label);
			generator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(ReflectionUtils.EmptyTypes));
			generator.Emit(OpCodes.Throw);
			generator.MarkLabel(label);
			if (!method.IsConstructor && !method.IsStatic)
			{
				generator.PushInstance(method.DeclaringType);
			}
			int num = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type parameterType = parameterInfo.ParameterType;
				if (parameterType.IsByRef)
				{
					parameterType = parameterType.GetElementType();
					LocalBuilder local = generator.DeclareLocal(parameterType);
					if (!parameterInfo.IsOut)
					{
						generator.PushArrayInstance(argsIndex, i);
						if (parameterType.IsValueType())
						{
							Label label2 = generator.DefineLabel();
							Label label3 = generator.DefineLabel();
							generator.Emit(OpCodes.Brtrue_S, label2);
							generator.Emit(OpCodes.Ldloca_S, local);
							generator.Emit(OpCodes.Initobj, parameterType);
							generator.Emit(OpCodes.Br_S, label3);
							generator.MarkLabel(label2);
							generator.PushArrayInstance(argsIndex, i);
							generator.UnboxIfNeeded(parameterType);
							generator.Emit(OpCodes.Stloc, num);
							generator.MarkLabel(label3);
						}
						else
						{
							generator.UnboxIfNeeded(parameterType);
							generator.Emit(OpCodes.Stloc, num);
						}
					}
					generator.Emit(OpCodes.Ldloca_S, local);
					num++;
				}
				else if (parameterType.IsValueType())
				{
					generator.PushArrayInstance(argsIndex, i);
					Label label4 = generator.DefineLabel();
					Label label5 = generator.DefineLabel();
					generator.Emit(OpCodes.Brtrue_S, label4);
					LocalBuilder local2 = generator.DeclareLocal(parameterType);
					generator.Emit(OpCodes.Ldloca_S, local2);
					generator.Emit(OpCodes.Initobj, parameterType);
					generator.Emit(OpCodes.Ldloc, num);
					generator.Emit(OpCodes.Br_S, label5);
					generator.MarkLabel(label4);
					generator.PushArrayInstance(argsIndex, i);
					generator.UnboxIfNeeded(parameterType);
					generator.MarkLabel(label5);
					num++;
				}
				else
				{
					generator.PushArrayInstance(argsIndex, i);
					generator.UnboxIfNeeded(parameterType);
				}
			}
			if (method.IsConstructor)
			{
				generator.Emit(OpCodes.Newobj, (ConstructorInfo)method);
			}
			else
			{
				generator.CallMethod((MethodInfo)method);
			}
			Type type = (method.IsConstructor ? method.DeclaringType : ((MethodInfo)method).ReturnType);
			if (type != typeof(void))
			{
				generator.BoxIfNeeded(type);
			}
			else
			{
				generator.Emit(OpCodes.Ldnull);
			}
			generator.Return();
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod("Create" + type.FullName, typeof(T), ReflectionUtils.EmptyTypes, type);
			dynamicMethod.InitLocals = true;
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateDefaultConstructorIL(type, iLGenerator);
			return (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
		}

		private void GenerateCreateDefaultConstructorIL(Type type, ILGenerator generator)
		{
			if (type.IsValueType())
			{
				generator.DeclareLocal(type);
				generator.Emit(OpCodes.Ldloc_0);
				generator.Emit(OpCodes.Box, type);
			}
			else
			{
				ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ReflectionUtils.EmptyTypes, null);
				if (constructor == null)
				{
					throw new ArgumentException("Could not get constructor for {0}.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				generator.Emit(OpCodes.Newobj, constructor);
			}
			generator.Return();
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod("Get" + propertyInfo.Name, typeof(T), new Type[1] { typeof(object) }, propertyInfo.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateGetPropertyIL(propertyInfo, iLGenerator);
			return (Func<T, object>)dynamicMethod.CreateDelegate(typeof(Func<T, object>));
		}

		private void GenerateCreateGetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
		{
			MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property '{0}' does not have a getter.".FormatWith(CultureInfo.InvariantCulture, propertyInfo.Name));
			}
			if (!getMethod.IsStatic)
			{
				generator.PushInstance(propertyInfo.DeclaringType);
			}
			generator.CallMethod(getMethod);
			generator.BoxIfNeeded(propertyInfo.PropertyType);
			generator.Return();
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsLiteral)
			{
				object constantValue = fieldInfo.GetValue(null);
				return (T o) => constantValue;
			}
			DynamicMethod dynamicMethod = CreateDynamicMethod("Get" + fieldInfo.Name, typeof(T), new Type[1] { typeof(object) }, fieldInfo.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateGetFieldIL(fieldInfo, iLGenerator);
			return (Func<T, object>)dynamicMethod.CreateDelegate(typeof(Func<T, object>));
		}

		private void GenerateCreateGetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
		{
			if (!fieldInfo.IsStatic)
			{
				generator.PushInstance(fieldInfo.DeclaringType);
				generator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			else
			{
				generator.Emit(OpCodes.Ldsfld, fieldInfo);
			}
			generator.BoxIfNeeded(fieldInfo.FieldType);
			generator.Return();
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod("Set" + fieldInfo.Name, null, new Type[2]
			{
				typeof(T),
				typeof(object)
			}, fieldInfo.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateSetFieldIL(fieldInfo, iLGenerator);
			return (Action<T, object>)dynamicMethod.CreateDelegate(typeof(Action<T, object>));
		}

		internal static void GenerateCreateSetFieldIL(FieldInfo fieldInfo, ILGenerator generator)
		{
			if (!fieldInfo.IsStatic)
			{
				generator.PushInstance(fieldInfo.DeclaringType);
			}
			generator.Emit(OpCodes.Ldarg_1);
			generator.UnboxIfNeeded(fieldInfo.FieldType);
			if (!fieldInfo.IsStatic)
			{
				generator.Emit(OpCodes.Stfld, fieldInfo);
			}
			else
			{
				generator.Emit(OpCodes.Stsfld, fieldInfo);
			}
			generator.Return();
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			DynamicMethod dynamicMethod = CreateDynamicMethod("Set" + propertyInfo.Name, null, new Type[2]
			{
				typeof(T),
				typeof(object)
			}, propertyInfo.DeclaringType);
			ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
			GenerateCreateSetPropertyIL(propertyInfo, iLGenerator);
			return (Action<T, object>)dynamicMethod.CreateDelegate(typeof(Action<T, object>));
		}

		internal static void GenerateCreateSetPropertyIL(PropertyInfo propertyInfo, ILGenerator generator)
		{
			MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
			if (!setMethod.IsStatic)
			{
				generator.PushInstance(propertyInfo.DeclaringType);
			}
			generator.Emit(OpCodes.Ldarg_1);
			generator.UnboxIfNeeded(propertyInfo.PropertyType);
			generator.CallMethod(setMethod);
			generator.Return();
		}
	}
}
namespace Newtonsoft.Json.Serialization
{
	public delegate object ObjectConstructor<T>(params object[] args);
}
namespace Newtonsoft.Json.Utilities
{
	internal class ExpressionReflectionDelegateFactory : ReflectionDelegateFactory
	{
		private class ByRefParameter
		{
			public Expression Value;

			public ParameterExpression Variable;

			public bool IsOut;
		}

		private static readonly ExpressionReflectionDelegateFactory _instance = new ExpressionReflectionDelegateFactory();

		internal static ReflectionDelegateFactory Instance => _instance;

		public override ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeof(object[]), "args");
			Expression body = BuildMethodCall(method, typeFromHandle, null, parameterExpression);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(ObjectConstructor<object>), body, parameterExpression);
			return (ObjectConstructor<object>)lambdaExpression.Compile();
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			Type typeFromHandle = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "target");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object[]), "args");
			Expression body = BuildMethodCall(method, typeFromHandle, parameterExpression, parameterExpression2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(MethodCall<T, object>), body, parameterExpression, parameterExpression2);
			return (MethodCall<T, object>)lambdaExpression.Compile();
		}

		private Expression BuildMethodCall(MethodBase method, Type type, ParameterExpression targetParameterExpression, ParameterExpression argsParameterExpression)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Expression[] array = new Expression[parameters.Length];
			IList<ByRefParameter> list = new List<ByRefParameter>();
			for (int i = 0; i < parameters.Length; i++)
			{
				ParameterInfo parameterInfo = parameters[i];
				Type type2 = parameterInfo.ParameterType;
				bool flag = false;
				if (type2.IsByRef)
				{
					type2 = type2.GetElementType();
					flag = true;
				}
				Expression index = Expression.Constant(i);
				Expression expression = Expression.ArrayIndex(argsParameterExpression, index);
				Expression expression3;
				if (type2.IsValueType())
				{
					BinaryExpression expression2 = Expression.Coalesce(expression, Expression.New(type2));
					expression3 = EnsureCastExpression(expression2, type2);
				}
				else
				{
					expression3 = EnsureCastExpression(expression, type2);
				}
				if (flag)
				{
					ParameterExpression parameterExpression = Expression.Variable(type2);
					list.Add(new ByRefParameter
					{
						Value = expression3,
						Variable = parameterExpression,
						IsOut = parameterInfo.IsOut
					});
					expression3 = parameterExpression;
				}
				array[i] = expression3;
			}
			Expression expression4;
			if (method.IsConstructor)
			{
				expression4 = Expression.New((ConstructorInfo)method, array);
			}
			else if (method.IsStatic)
			{
				expression4 = Expression.Call((MethodInfo)method, array);
			}
			else
			{
				Expression instance = EnsureCastExpression(targetParameterExpression, method.DeclaringType);
				expression4 = Expression.Call(instance, (MethodInfo)method, array);
			}
			if (method is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)method;
				expression4 = ((!(methodInfo.ReturnType != typeof(void))) ? Expression.Block(expression4, Expression.Constant(null)) : EnsureCastExpression(expression4, type));
			}
			else
			{
				expression4 = EnsureCastExpression(expression4, type);
			}
			if (list.Count > 0)
			{
				IList<ParameterExpression> list2 = new List<ParameterExpression>();
				IList<Expression> list3 = new List<Expression>();
				foreach (ByRefParameter item in list)
				{
					if (!item.IsOut)
					{
						list3.Add(Expression.Assign(item.Variable, item.Value));
					}
					list2.Add(item.Variable);
				}
				list3.Add(expression4);
				expression4 = Expression.Block(list2, list3);
			}
			return expression4;
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsAbstract())
			{
				return () => (T)Activator.CreateInstance(type);
			}
			try
			{
				Type typeFromHandle = typeof(T);
				Expression expression = Expression.New(type);
				expression = EnsureCastExpression(expression, typeFromHandle);
				LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T>), expression);
				return (Func<T>)lambdaExpression.Compile();
			}
			catch
			{
				return () => (T)Activator.CreateInstance(type);
			}
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			MethodInfo getMethod = propertyInfo.GetGetMethod(nonPublic: true);
			Expression expression;
			if (getMethod.IsStatic)
			{
				expression = Expression.MakeMemberAccess(null, propertyInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				expression = Expression.MakeMemberAccess(expression2, propertyInfo);
			}
			expression = EnsureCastExpression(expression, typeFromHandle2);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Func<T, object>), expression, parameterExpression);
			return (Func<T, object>)lambdaExpression.Compile();
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			expression = EnsureCastExpression(expression, typeof(object));
			return Expression.Lambda<Func<T, object>>(expression, new ParameterExpression[1] { parameterExpression }).Compile();
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			if (fieldInfo.DeclaringType.IsValueType() || fieldInfo.IsInitOnly)
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(fieldInfo);
			}
			ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "source");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "value");
			Expression expression;
			if (fieldInfo.IsStatic)
			{
				expression = Expression.Field(null, fieldInfo);
			}
			else
			{
				Expression expression2 = EnsureCastExpression(parameterExpression, fieldInfo.DeclaringType);
				expression = Expression.Field(expression2, fieldInfo);
			}
			Expression right = EnsureCastExpression(parameterExpression2, expression.Type);
			BinaryExpression body = Expression.Assign(expression, right);
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, parameterExpression, parameterExpression2);
			return (Action<T, object>)lambdaExpression.Compile();
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			if (propertyInfo.DeclaringType.IsValueType())
			{
				return LateBoundReflectionDelegateFactory.Instance.CreateSet<T>(propertyInfo);
			}
			Type typeFromHandle = typeof(T);
			Type typeFromHandle2 = typeof(object);
			ParameterExpression parameterExpression = Expression.Parameter(typeFromHandle, "instance");
			ParameterExpression parameterExpression2 = Expression.Parameter(typeFromHandle2, "value");
			Expression expression = EnsureCastExpression(parameterExpression2, propertyInfo.PropertyType);
			MethodInfo setMethod = propertyInfo.GetSetMethod(nonPublic: true);
			Expression body;
			if (setMethod.IsStatic)
			{
				body = Expression.Call(setMethod, expression);
			}
			else
			{
				Expression instance = EnsureCastExpression(parameterExpression, propertyInfo.DeclaringType);
				body = Expression.Call(instance, setMethod, expression);
			}
			LambdaExpression lambdaExpression = Expression.Lambda(typeof(Action<T, object>), body, parameterExpression, parameterExpression2);
			return (Action<T, object>)lambdaExpression.Compile();
		}

		private Expression EnsureCastExpression(Expression expression, Type targetType)
		{
			Type type = expression.Type;
			if (type == targetType || (!type.IsValueType() && targetType.IsAssignableFrom(type)))
			{
				return expression;
			}
			return Expression.Convert(expression, targetType);
		}
	}
	internal static class FSharpUtils
	{
		public const string FSharpSetTypeName = "FSharpSet`1";

		public const string FSharpListTypeName = "FSharpList`1";

		public const string FSharpMapTypeName = "FSharpMap`2";

		private static readonly object Lock = new object();

		private static bool _initialized;

		private static MethodInfo _ofSeq;

		private static Type _mapType;

		public static Assembly FSharpCoreAssembly { get; private set; }

		public static MethodCall<object, object> IsUnion { get; private set; }

		public static MethodCall<object, object> GetUnionFields { get; private set; }

		public static MethodCall<object, object> GetUnionCases { get; private set; }

		public static MethodCall<object, object> MakeUnion { get; private set; }

		public static Func<object, object> GetUnionCaseInfoName { get; private set; }

		public static Func<object, object> GetUnionCaseInfo { get; private set; }

		public static Func<object, object> GetUnionCaseFields { get; private set; }

		public static MethodCall<object, object> GetUnionCaseInfoFields { get; private set; }

		public static void EnsureInitialized(Assembly fsharpCoreAssembly)
		{
			if (_initialized)
			{
				return;
			}
			lock (Lock)
			{
				if (!_initialized)
				{
					FSharpCoreAssembly = fsharpCoreAssembly;
					Type type = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpType");
					MethodInfo method = type.GetMethod("IsUnion", BindingFlags.Static | BindingFlags.Public);
					IsUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
					MethodInfo method2 = type.GetMethod("GetUnionCases", BindingFlags.Static | BindingFlags.Public);
					GetUnionCases = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method2);
					Type type2 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.FSharpValue");
					MethodInfo method3 = type2.GetMethod("GetUnionFields", BindingFlags.Static | BindingFlags.Public);
					GetUnionFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method3);
					GetUnionCaseInfo = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item1"));
					GetUnionCaseFields = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(method3.ReturnType.GetProperty("Item2"));
					MethodInfo method4 = type2.GetMethod("MakeUnion", BindingFlags.Static | BindingFlags.Public);
					MakeUnion = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method4);
					Type type3 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Reflection.UnionCaseInfo");
					GetUnionCaseInfoName = JsonTypeReflector.ReflectionDelegateFactory.CreateGet<object>(type3.GetProperty("Name"));
					GetUnionCaseInfoFields = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(type3.GetMethod("GetFields"));
					Type type4 = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.ListModule");
					_ofSeq = type4.GetMethod("OfSeq");
					_mapType = fsharpCoreAssembly.GetType("Microsoft.FSharp.Collections.FSharpMap`2");
					Thread.MemoryBarrier();
					_initialized = true;
				}
			}
		}

		public static ObjectConstructor<object> CreateSeq(Type t)
		{
			MethodInfo method = _ofSeq.MakeGenericMethod(t);
			return JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(method);
		}

		public static ObjectConstructor<object> CreateMap(Type keyType, Type valueType)
		{
			MethodInfo method = typeof(FSharpUtils).GetMethod("BuildMapCreator");
			MethodInfo methodInfo = method.MakeGenericMethod(keyType, valueType);
			return (ObjectConstructor<object>)methodInfo.Invoke(null, null);
		}

		public static ObjectConstructor<object> BuildMapCreator<TKey, TValue>()
		{
			Type type = _mapType.MakeGenericType(typeof(TKey), typeof(TValue));
			ConstructorInfo constructor = type.GetConstructor(new Type[1] { typeof(IEnumerable<Tuple<TKey, TValue>>) });
			ObjectConstructor<object> ctorDelegate = JsonTypeReflector.ReflectionDelegateFactory.CreateParametrizedConstructor(constructor);
			return delegate(object[] args)
			{
				IEnumerable<KeyValuePair<TKey, TValue>> source = (IEnumerable<KeyValuePair<TKey, TValue>>)args[0];
				IEnumerable<Tuple<TKey, TValue>> enumerable = source.Select((KeyValuePair<TKey, TValue> kv) => new Tuple<TKey, TValue>(kv.Key, kv.Value));
				return ctorDelegate(enumerable);
			};
		}
	}
	internal static class ILGeneratorExtensions
	{
		public static void PushInstance(this ILGenerator generator, Type type)
		{
			generator.Emit(OpCodes.Ldarg_0);
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Unbox, type);
			}
			else
			{
				generator.Emit(OpCodes.Castclass, type);
			}
		}

		public static void PushArrayInstance(this ILGenerator generator, int argsIndex, int arrayIndex)
		{
			generator.Emit(OpCodes.Ldarg, argsIndex);
			generator.Emit(OpCodes.Ldc_I4, arrayIndex);
			generator.Emit(OpCodes.Ldelem_Ref);
		}

		public static void BoxIfNeeded(this ILGenerator generator, Type type)
		{
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Box, type);
			}
			else
			{
				generator.Emit(OpCodes.Castclass, type);
			}
		}

		public static void UnboxIfNeeded(this ILGenerator generator, Type type)
		{
			if (type.IsValueType())
			{
				generator.Emit(OpCodes.Unbox_Any, type);
			}
			else
			{
				generator.Emit(OpCodes.Castclass, type);
			}
		}

		public static void CallMethod(this ILGenerator generator, MethodInfo methodInfo)
		{
			if (methodInfo.IsFinal || !methodInfo.IsVirtual)
			{
				generator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				generator.Emit(OpCodes.Callvirt, methodInfo);
			}
		}

		public static void Return(this ILGenerator generator)
		{
			generator.Emit(OpCodes.Ret);
		}
	}
	internal static class JsonTokenUtils
	{
		internal static bool IsEndToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.EndObject:
			case JsonToken.EndArray:
			case JsonToken.EndConstructor:
				return true;
			default:
				return false;
			}
		}

		internal static bool IsStartToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.StartObject:
			case JsonToken.StartArray:
			case JsonToken.StartConstructor:
				return true;
			default:
				return false;
			}
		}

		internal static bool IsPrimitiveToken(JsonToken token)
		{
			switch (token)
			{
			case JsonToken.Integer:
			case JsonToken.Float:
			case JsonToken.String:
			case JsonToken.Boolean:
			case JsonToken.Null:
			case JsonToken.Undefined:
			case JsonToken.Date:
			case JsonToken.Bytes:
				return true;
			default:
				return false;
			}
		}
	}
	internal class PropertyNameTable
	{
		private class Entry
		{
			internal readonly string Value;

			internal readonly int HashCode;

			internal Entry Next;

			internal Entry(string value, int hashCode, Entry next)
			{
				Value = value;
				HashCode = hashCode;
				Next = next;
			}
		}

		private static readonly int HashCodeRandomizer;

		private int _count;

		private Entry[] _entries;

		private int _mask = 31;

		static PropertyNameTable()
		{
			HashCodeRandomizer = Environment.TickCount;
		}

		public PropertyNameTable()
		{
			_entries = new Entry[_mask + 1];
		}

		public string Get(char[] key, int start, int length)
		{
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + HashCodeRandomizer;
			num += (num << 7) ^ key[start];
			int num2 = start + length;
			for (int i = start + 1; i < num2; i++)
			{
				num += (num << 7) ^ key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (Entry entry = _entries[num & _mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && TextEquals(entry.Value, key, start, length))
				{
					return entry.Value;
				}
			}
			return null;
		}

		public string Add(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			int length = key.Length;
			if (length == 0)
			{
				return string.Empty;
			}
			int num = length + HashCodeRandomizer;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7) ^ key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			num -= num >> 5;
			for (Entry entry = _entries[num & _mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Value.Equals(key))
				{
					return entry.Value;
				}
			}
			return AddEntry(key, num);
		}

		private string AddEntry(string str, int hashCode)
		{
			int num = hashCode & _mask;
			Entry entry = new Entry(str, hashCode, _entries[num]);
			_entries[num] = entry;
			if (_count++ == _mask)
			{
				Grow();
			}
			return entry.Value;
		}

		private void Grow()
		{
			Entry[] entries = _entries;
			int num = _mask * 2 + 1;
			Entry[] array = new Entry[num + 1];
			for (int i = 0; i < entries.Length; i++)
			{
				Entry entry = entries[i];
				while (entry != null)
				{
					int num2 = entry.HashCode & num;
					Entry next = entry.Next;
					entry.Next = array[num2];
					array[num2] = entry;
					entry = next;
				}
			}
			_entries = array;
			_mask = num;
		}

		private static bool TextEquals(string str1, char[] str2, int str2Start, int str2Length)
		{
			if (str1.Length != str2Length)
			{
				return false;
			}
			for (int i = 0; i < str1.Length; i++)
			{
				if (str1[i] != str2[str2Start + i])
				{
					return false;
				}
			}
			return true;
		}
	}
	internal class LateBoundReflectionDelegateFactory : ReflectionDelegateFactory
	{
		private static readonly LateBoundReflectionDelegateFactory _instance = new LateBoundReflectionDelegateFactory();

		internal static ReflectionDelegateFactory Instance => _instance;

		public override ObjectConstructor<object> CreateParametrizedConstructor(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo constructorInfo = method as ConstructorInfo;
			if (constructorInfo != null)
			{
				return constructorInfo.Invoke;
			}
			return (object[] a) => method.Invoke(null, a);
		}

		public override MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
		{
			ValidationUtils.ArgumentNotNull(method, "method");
			ConstructorInfo c = method as ConstructorInfo;
			if (c != null)
			{
				return (T o, object[] a) => c.Invoke(a);
			}
			return (T o, object[] a) => method.Invoke(o, a);
		}

		public override Func<T> CreateDefaultConstructor<T>(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsValueType())
			{
				return () => (T)Activator.CreateInstance(type);
			}
			ConstructorInfo constructorInfo = ReflectionUtils.GetDefaultConstructor(type, nonPublic: true);
			return () => (T)constructorInfo.Invoke(null);
		}

		public override Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return (T o) => propertyInfo.GetValue(o, null);
		}

		public override Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return (T o) => fieldInfo.GetValue(o);
		}

		public override Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
		{
			ValidationUtils.ArgumentNotNull(fieldInfo, "fieldInfo");
			return delegate(T o, object v)
			{
				fieldInfo.SetValue(o, v);
			};
		}

		public override Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			return delegate(T o, object v)
			{
				propertyInfo.SetValue(o, v, null);
			};
		}
	}
	internal delegate TResult MethodCall<T, TResult>(T target, params object[] args);
	internal class ReflectionMember
	{
		public Type MemberType { get; set; }

		public Func<object, object> Getter { get; set; }

		public Action<object, object> Setter { get; set; }
	}
	internal class ReflectionObject
	{
		public ObjectConstructor<object> Creator { get; private set; }

		public IDictionary<string, ReflectionMember> Members { get; private set; }

		public ReflectionObject()
		{
			Members = new Dictionary<string, ReflectionMember>();
		}

		public object GetValue(object target, string member)
		{
			Func<object, object> getter = Members[member].Getter;
			return getter(target);
		}

		public void SetValue(object target, string member, object value)
		{
			Action<object, object> setter = Members[member].Setter;
			setter(target, value);
		}

		public Type GetType(string member)
		{
			return Members[member].MemberType;
		}

		public static ReflectionObject Create(Type t, params string[] memberNames)
		{
			return Create(t, null, memberNames);
		}

		public static ReflectionObject Create(Type t, MethodBase creator, params string[] memberNames)
		{
			ReflectionObject reflectionObject = new ReflectionObject();
			ReflectionDelegateFactory reflectionDelegateFactory = JsonTypeReflector.ReflectionDelegateFactory;
			if (creator != null)
			{
				reflectionObject.Creator = reflectionDelegateFactory.CreateParametrizedConstructor(creator);
			}
			else if (ReflectionUtils.HasDefaultConstructor(t, nonPublic: false))
			{
				Func<object> ctor = reflectionDelegateFactory.CreateDefaultConstructor<object>(t);
				reflectionObject.Creator = (object[] args) => ctor();
			}
			foreach (string text in memberNames)
			{
				MemberInfo[] member = t.GetMember(text, BindingFlags.Instance | BindingFlags.Public);
				if (member.Length != 1)
				{
					throw new ArgumentException("Expected a single member with the name '{0}'.".FormatWith(CultureInfo.InvariantCulture, text));
				}
				MemberInfo memberInfo = member.Single();
				ReflectionMember reflectionMember = new ReflectionMember();
				switch (memberInfo.MemberType())
				{
				case MemberTypes.Field:
				case MemberTypes.Property:
					if (ReflectionUtils.CanReadMemberValue(memberInfo, nonPublic: false))
					{
						reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
					}
					if (ReflectionUtils.CanSetMemberValue(memberInfo, nonPublic: false, canSetReadOnly: false))
					{
						reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
					}
					break;
				case MemberTypes.Method:
				{
					MethodInfo methodInfo = (MethodInfo)memberInfo;
					if (!methodInfo.IsPublic)
					{
						break;
					}
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 0 && methodInfo.ReturnType != typeof(void))
					{
						MethodCall<object, object> call2 = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
						reflectionMember.Getter = (object target) => call2(target);
					}
					else if (parameters.Length == 1 && methodInfo.ReturnType == typeof(void))
					{
						MethodCall<object, object> call = reflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
						reflectionMember.Setter = delegate(object target, object arg)
						{
							call(target, arg);
						};
					}
					break;
				}
				default:
					throw new ArgumentException("Unexpected member type '{0}' for member '{1}'.".FormatWith(CultureInfo.InvariantCulture, memberInfo.MemberType(), memberInfo.Name));
				}
				if (ReflectionUtils.CanReadMemberValue(memberInfo, nonPublic: false))
				{
					reflectionMember.Getter = reflectionDelegateFactory.CreateGet<object>(memberInfo);
				}
				if (ReflectionUtils.CanSetMemberValue(memberInfo, nonPublic: false, canSetReadOnly: false))
				{
					reflectionMember.Setter = reflectionDelegateFactory.CreateSet<object>(memberInfo);
				}
				reflectionMember.MemberType = ReflectionUtils.GetMemberUnderlyingType(memberInfo);
				reflectionObject.Members[text] = reflectionMember;
			}
			return reflectionObject;
		}
	}
	internal struct StringReference
	{
		private readonly char[] _chars;

		private readonly int _startIndex;

		private readonly int _length;

		public char[] Chars => _chars;

		public int StartIndex => _startIndex;

		public int Length => _length;

		public StringReference(char[] chars, int startIndex, int length)
		{
			_chars = chars;
			_startIndex = startIndex;
			_length = length;
		}

		public override string ToString()
		{
			return new string(_chars, _startIndex, _length);
		}
	}
	internal class ThreadSafeStore<TKey, TValue>
	{
		private readonly object _lock = new object();

		private Dictionary<TKey, TValue> _store;

		private readonly Func<TKey, TValue> _creator;

		public ThreadSafeStore(Func<TKey, TValue> creator)
		{
			if (creator == null)
			{
				throw new ArgumentNullException("creator");
			}
			_creator = creator;
			_store = new Dictionary<TKey, TValue>();
		}

		public TValue Get(TKey key)
		{
			if (!_store.TryGetValue(key, out var value))
			{
				return AddValue(key);
			}
			return value;
		}

		private TValue AddValue(TKey key)
		{
			TValue val = _creator(key);
			lock (_lock)
			{
				if (_store == null)
				{
					_store = new Dictionary<TKey, TValue>();
					_store[key] = val;
				}
				else
				{
					if (_store.TryGetValue(key, out var value))
					{
						return value;
					}
					Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(_store);
					dictionary[key] = val;
					Thread.MemoryBarrier();
					_store = dictionary;
				}
				return val;
			}
		}
	}
}
namespace Newtonsoft.Json
{
	[Flags]
	public enum TypeNameHandling
	{
		None = 0,
		Objects = 1,
		Arrays = 2,
		All = 3,
		Auto = 4
	}
}
namespace Newtonsoft.Json.Utilities
{
	internal class BidirectionalDictionary<TFirst, TSecond>
	{
		private readonly IDictionary<TFirst, TSecond> _firstToSecond;

		private readonly IDictionary<TSecond, TFirst> _secondToFirst;

		private readonly string _duplicateFirstErrorMessage;

		private readonly string _duplicateSecondErrorMessage;

		public BidirectionalDictionary()
			: this((IEqualityComparer<TFirst>)EqualityComparer<TFirst>.Default, (IEqualityComparer<TSecond>)EqualityComparer<TSecond>.Default)
		{
		}

		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer)
			: this(firstEqualityComparer, secondEqualityComparer, "Duplicate item already exists for '{0}'.", "Duplicate item already exists for '{0}'.")
		{
		}

		public BidirectionalDictionary(IEqualityComparer<TFirst> firstEqualityComparer, IEqualityComparer<TSecond> secondEqualityComparer, string duplicateFirstErrorMessage, string duplicateSecondErrorMessage)
		{
			_firstToSecond = new Dictionary<TFirst, TSecond>(firstEqualityComparer);
			_secondToFirst = new Dictionary<TSecond, TFirst>(secondEqualityComparer);
			_duplicateFirstErrorMessage = duplicateFirstErrorMessage;
			_duplicateSecondErrorMessage = duplicateSecondErrorMessage;
		}

		public void Set(TFirst first, TSecond second)
		{
			if (_firstToSecond.TryGetValue(first, out var value) && !value.Equals(second))
			{
				throw new ArgumentException(_duplicateFirstErrorMessage.FormatWith(CultureInfo.InvariantCulture, first));
			}
			if (_secondToFirst.TryGetValue(second, out var value2) && !value2.Equals(first))
			{
				throw new ArgumentException(_duplicateSecondErrorMessage.FormatWith(CultureInfo.InvariantCulture, second));
			}
			_firstToSecond.Add(first, second);
			_secondToFirst.Add(second, first);
		}

		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return _firstToSecond.TryGetValue(first, out second);
		}

		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return _secondToFirst.TryGetValue(second, out first);
		}
	}
	internal enum PrimitiveTypeCode
	{
		Empty,
		Object,
		Char,
		CharNullable,
		Boolean,
		BooleanNullable,
		SByte,
		SByteNullable,
		Int16,
		Int16Nullable,
		UInt16,
		UInt16Nullable,
		Int32,
		Int32Nullable,
		Byte,
		ByteNullable,
		UInt32,
		UInt32Nullable,
		Int64,
		Int64Nullable,
		UInt64,
		UInt64Nullable,
		Single,
		SingleNullable,
		Double,
		DoubleNullable,
		DateTime,
		DateTimeNullable,
		DateTimeOffset,
		DateTimeOffsetNullable,
		Decimal,
		DecimalNullable,
		Guid,
		GuidNullable,
		TimeSpan,
		TimeSpanNullable,
		BigInteger,
		BigIntegerNullable,
		Uri,
		String,
		Bytes,
		DBNull
	}
	internal class TypeInformation
	{
		public Type Type { get; set; }

		public PrimitiveTypeCode TypeCode { get; set; }
	}
	internal enum ParseResult
	{
		None,
		Success,
		Overflow,
		Invalid
	}
	internal static class ConvertUtils
	{
		internal struct TypeConvertKey : IEquatable<TypeConvertKey>
		{
			private readonly Type _initialType;

			private readonly Type _targetType;

			public Type InitialType => _initialType;

			public Type TargetType => _targetType;

			public TypeConvertKey(Type initialType, Type targetType)
			{
				_initialType = initialType;
				_targetType = targetType;
			}

			public override int GetHashCode()
			{
				return _initialType.GetHashCode() ^ _targetType.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (!(obj is TypeConvertKey))
				{
					return false;
				}
				return Equals((TypeConvertKey)obj);
			}

			public bool Equals(TypeConvertKey other)
			{
				if (_initialType == other._initialType)
				{
					return _targetType == other._targetType;
				}
				return false;
			}
		}

		internal enum ConvertResult
		{
			Success,
			CannotConvertNull,
			NotInstantiableType,
			NoValidConversion
		}

		private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap = new Dictionary<Type, PrimitiveTypeCode>
		{
			{
				typeof(char),
				PrimitiveTypeCode.Char
			},
			{
				typeof(char?),
				PrimitiveTypeCode.CharNullable
			},
			{
				typeof(bool),
				PrimitiveTypeCode.Boolean
			},
			{
				typeof(bool?),
				PrimitiveTypeCode.BooleanNullable
			},
			{
				typeof(sbyte),
				PrimitiveTypeCode.SByte
			},
			{
				typeof(sbyte?),
				PrimitiveTypeCode.SByteNullable
			},
			{
				typeof(short),
				PrimitiveTypeCode.Int16
			},
			{
				typeof(short?),
				PrimitiveTypeCode.Int16Nullable
			},
			{
				typeof(ushort),
				PrimitiveTypeCode.UInt16
			},
			{
				typeof(ushort?),
				PrimitiveTypeCode.UInt16Nullable
			},
			{
				typeof(int),
				PrimitiveTypeCode.Int32
			},
			{
				typeof(int?),
				PrimitiveTypeCode.Int32Nullable
			},
			{
				typeof(byte),
				PrimitiveTypeCode.Byte
			},
			{
				typeof(byte?),
				PrimitiveTypeCode.ByteNullable
			},
			{
				typeof(uint),
				PrimitiveTypeCode.UInt32
			},
			{
				typeof(uint?),
				PrimitiveTypeCode.UInt32Nullable
			},
			{
				typeof(long),
				PrimitiveTypeCode.Int64
			},
			{
				typeof(long?),
				PrimitiveTypeCode.Int64Nullable
			},
			{
				typeof(ulong),
				PrimitiveTypeCode.UInt64
			},
			{
				typeof(ulong?),
				PrimitiveTypeCode.UInt64Nullable
			},
			{
				typeof(float),
				PrimitiveTypeCode.Single
			},
			{
				typeof(float?),
				PrimitiveTypeCode.SingleNullable
			},
			{
				typeof(double),
				PrimitiveTypeCode.Double
			},
			{
				typeof(double?),
				PrimitiveTypeCode.DoubleNullable
			},
			{
				typeof(DateTime),
				PrimitiveTypeCode.DateTime
			},
			{
				typeof(DateTime?),
				PrimitiveTypeCode.DateTimeNullable
			},
			{
				typeof(DateTimeOffset),
				PrimitiveTypeCode.DateTimeOffset
			},
			{
				typeof(DateTimeOffset?),
				PrimitiveTypeCode.DateTimeOffsetNullable
			},
			{
				typeof(decimal),
				PrimitiveTypeCode.Decimal
			},
			{
				typeof(decimal?),
				PrimitiveTypeCode.DecimalNullable
			},
			{
				typeof(Guid),
				PrimitiveTypeCode.Guid
			},
			{
				typeof(Guid?),
				PrimitiveTypeCode.GuidNullable
			},
			{
				typeof(TimeSpan),
				PrimitiveTypeCode.TimeSpan
			},
			{
				typeof(TimeSpan?),
				PrimitiveTypeCode.TimeSpanNullable
			},
			{
				typeof(BigInteger),
				PrimitiveTypeCode.BigInteger
			},
			{
				typeof(BigInteger?),
				PrimitiveTypeCode.BigIntegerNullable
			},
			{
				typeof(Uri),
				PrimitiveTypeCode.Uri
			},
			{
				typeof(string),
				PrimitiveTypeCode.String
			},
			{
				typeof(byte[]),
				PrimitiveTypeCode.Bytes
			},
			{
				typeof(DBNull),
				PrimitiveTypeCode.DBNull
			}
		};

		private static readonly TypeInformation[] PrimitiveTypeCodes = new TypeInformation[19]
		{
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Object
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.DBNull
			},
			new TypeInformation
			{
				Type = typeof(bool),
				TypeCode = PrimitiveTypeCode.Boolean
			},
			new TypeInformation
			{
				Type = typeof(char),
				TypeCode = PrimitiveTypeCode.Char
			},
			new TypeInformation
			{
				Type = typeof(sbyte),
				TypeCode = PrimitiveTypeCode.SByte
			},
			new TypeInformation
			{
				Type = typeof(byte),
				TypeCode = PrimitiveTypeCode.Byte
			},
			new TypeInformation
			{
				Type = typeof(short),
				TypeCode = PrimitiveTypeCode.Int16
			},
			new TypeInformation
			{
				Type = typeof(ushort),
				TypeCode = PrimitiveTypeCode.UInt16
			},
			new TypeInformation
			{
				Type = typeof(int),
				TypeCode = PrimitiveTypeCode.Int32
			},
			new TypeInformation
			{
				Type = typeof(uint),
				TypeCode = PrimitiveTypeCode.UInt32
			},
			new TypeInformation
			{
				Type = typeof(long),
				TypeCode = PrimitiveTypeCode.Int64
			},
			new TypeInformation
			{
				Type = typeof(ulong),
				TypeCode = PrimitiveTypeCode.UInt64
			},
			new TypeInformation
			{
				Type = typeof(float),
				TypeCode = PrimitiveTypeCode.Single
			},
			new TypeInformation
			{
				Type = typeof(double),
				TypeCode = PrimitiveTypeCode.Double
			},
			new TypeInformation
			{
				Type = typeof(decimal),
				TypeCode = PrimitiveTypeCode.Decimal
			},
			new TypeInformation
			{
				Type = typeof(DateTime),
				TypeCode = PrimitiveTypeCode.DateTime
			},
			new TypeInformation
			{
				Type = typeof(object),
				TypeCode = PrimitiveTypeCode.Empty
			},
			new TypeInformation
			{
				Type = typeof(string),
				TypeCode = PrimitiveTypeCode.String
			}
		};

		private static readonly ThreadSafeStore<TypeConvertKey, Func<object, object>> CastConverters = new ThreadSafeStore<TypeConvertKey, Func<object, object>>(CreateCastConverter);

		public static PrimitiveTypeCode GetTypeCode(Type t)
		{
			bool isEnum;
			return GetTypeCode(t, out isEnum);
		}

		public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
		{
			if (TypeCodeMap.TryGetValue(t, out var value))
			{
				isEnum = false;
				return value;
			}
			if (t.IsEnum())
			{
				isEnum = true;
				return GetTypeCode(Enum.GetUnderlyingType(t));
			}
			if (ReflectionUtils.IsNullableType(t))
			{
				Type underlyingType = Nullable.GetUnderlyingType(t);
				if (underlyingType.IsEnum())
				{
					Type t2 = typeof(Nullable<>).MakeGenericType(Enum.GetUnderlyingType(underlyingType));
					isEnum = true;
					return GetTypeCode(t2);
				}
			}
			isEnum = false;
			return PrimitiveTypeCode.Object;
		}

		public static TypeInformation GetTypeInformation(IConvertible convertable)
		{
			return PrimitiveTypeCodes[(int)convertable.GetTypeCode()];
		}

		public static bool IsConvertible(Type t)
		{
			return typeof(IConvertible).IsAssignableFrom(t);
		}

		public static TimeSpan ParseTimeSpan(string input)
		{
			return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
		}

		private static Func<object, object> CreateCastConverter(TypeConvertKey t)
		{
			MethodInfo method = t.TargetType.GetMethod("op_Implicit", new Type[1] { t.InitialType });
			if (method == null)
			{
				method = t.TargetType.GetMethod("op_Explicit", new Type[1] { t.InitialType });
			}
			if (method == null)
			{
				return null;
			}
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(method);
			return (object o) => call(null, o);
		}

		internal static BigInteger ToBigInteger(object value)
		{
			if (value is BigInteger)
			{
				return (BigInteger)value;
			}
			if (value is string)
			{
				return BigInteger.Parse((string)value, CultureInfo.InvariantCulture);
			}
			if (value is float)
			{
				return new BigInteger((float)value);
			}
			if (value is double)
			{
				return new BigInteger((double)value);
			}
			if (value is decimal)
			{
				return new BigInteger((decimal)value);
			}
			if (value is int)
			{
				return new BigInteger((int)value);
			}
			if (value is long)
			{
				return new BigInteger((long)value);
			}
			if (value is uint)
			{
				return new BigInteger((uint)value);
			}
			if (value is ulong)
			{
				return new BigInteger((ulong)value);
			}
			if (value is byte[])
			{
				return new BigInteger((byte[])value);
			}
			throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		public static object FromBigInteger(BigInteger i, Type targetType)
		{
			if (targetType == typeof(decimal))
			{
				return (decimal)i;
			}
			if (targetType == typeof(double))
			{
				return (double)i;
			}
			if (targetType == typeof(float))
			{
				return (float)i;
			}
			if (targetType == typeof(ulong))
			{
				return (ulong)i;
			}
			try
			{
				return System.Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), innerException);
			}
		}

		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			object value;
			return TryConvertInternal(initialValue, culture, targetType, out value) switch
			{
				ConvertResult.Success => value, 
				ConvertResult.CannotConvertNull => throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType)), 
				ConvertResult.NotInstantiableType => throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, targetType), "targetType"), 
				ConvertResult.NoValidConversion => throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType)), 
				_ => throw new InvalidOperationException("Unexpected conversion result."), 
			};
		}

		private static bool TryConvert(object initialValue, CultureInfo culture, Type targetType, out object value)
		{
			try
			{
				if (TryConvertInternal(initialValue, culture, targetType, out value) == ConvertResult.Success)
				{
					return true;
				}
				value = null;
				return false;
			}
			catch
			{
				value = null;
				return false;
			}
		}

		private static ConvertResult TryConvertInternal(object initialValue, CultureInfo culture, Type targetType, out object value)
		{
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				value = initialValue;
				return ConvertResult.Success;
			}
			if (IsConvertible(initialValue.GetType()) && IsConvertible(targetType))
			{
				if (targetType.IsEnum())
				{
					if (initialValue is string)
					{
						value = Enum.Parse(targetType, initialValue.ToString(), ignoreCase: true);
						return ConvertResult.Success;
					}
					if (IsInteger(initialValue))
					{
						value = Enum.ToObject(targetType, initialValue);
						return ConvertResult.Success;
					}
				}
				value = System.Convert.ChangeType(initialValue, targetType, culture);
				return ConvertResult.Success;
			}
			if (initialValue is DateTime && targetType == typeof(DateTimeOffset))
			{
				value = new DateTimeOffset((DateTime)initialValue);
				return ConvertResult.Success;
			}
			if (initialValue is byte[] && targetType == typeof(Guid))
			{
				value = new Guid((byte[])initialValue);
				return ConvertResult.Success;
			}
			if (initialValue is Guid && targetType == typeof(byte[]))
			{
				value = ((Guid)initialValue).ToByteArray();
				return ConvertResult.Success;
			}
			if (initialValue is string)
			{
				if (targetType == typeof(Guid))
				{
					value = new Guid((string)initialValue);
					return ConvertResult.Success;
				}
				if (targetType == typeof(Uri))
				{
					value = new Uri((string)initialValue, UriKind.RelativeOrAbsolute);
					return ConvertResult.Success;
				}
				if (targetType == typeof(TimeSpan))
				{
					value = ParseTimeSpan((string)initialValue);
					return ConvertResult.Success;
				}
				if (targetType == typeof(byte[]))
				{
					value = System.Convert.FromBase64String((string)initialValue);
					return ConvertResult.Success;
				}
				if (typeof(Type).IsAssignableFrom(targetType))
				{
					value = Type.GetType((string)initialValue, throwOnError: true);
					return ConvertResult.Success;
				}
			}
			if (targetType == typeof(BigInteger))
			{
				value = ToBigInteger(initialValue);
				return ConvertResult.Success;
			}
			if (initialValue is BigInteger)
			{
				value = FromBigInteger((BigInteger)initialValue, targetType);
				return ConvertResult.Success;
			}
			TypeConverter converter = GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				value = converter.ConvertTo(null, culture, initialValue, targetType);
				return ConvertResult.Success;
			}
			TypeConverter converter2 = GetConverter(targetType);
			if (converter2 != null && converter2.CanConvertFrom(type))
			{
				value = converter2.ConvertFrom(null, culture, initialValue);
				return ConvertResult.Success;
			}
			if (initialValue == DBNull.Value)
			{
				if (ReflectionUtils.IsNullable(targetType))
				{
					value = EnsureTypeAssignable(null, type, targetType);
					return ConvertResult.Success;
				}
				value = null;
				return ConvertResult.CannotConvertNull;
			}
			if (initialValue is INullable)
			{
				value = EnsureTypeAssignable(ToValue((INullable)initialValue), type, targetType);
				return ConvertResult.Success;
			}
			if (targetType.IsInterface() || targetType.IsGenericTypeDefinition() || targetType.IsAbstract())
			{
				value = null;
				return ConvertResult.NotInstantiableType;
			}
			value = null;
			return ConvertResult.NoValidConversion;
		}

		public static object ConvertOrCast(object initialValue, CultureInfo culture, Type targetType)
		{
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			if (TryConvert(initialValue, culture, targetType, out var value))
			{
				return value;
			}
			return EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		private static object EnsureTypeAssignable(object value, Type initialType, Type targetType)
		{
			Type type = value?.GetType();
			if (value != null)
			{
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				Func<object, object> func = CastConverters.Get(new TypeConvertKey(type, targetType));
				if (func != null)
				{
					return func(value);
				}
			}
			else if (ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, (initialType != null) ? initialType.ToString() : "{null}", targetType));
		}

		public static object ToValue(INullable nullableValue)
		{
			if (nullableValue == null)
			{
				return null;
			}
			if (nullableValue is SqlInt32)
			{
				return ToValue((SqlInt32)(object)nullableValue);
			}
			if (nullableValue is SqlInt64)
			{
				return ToValue((SqlInt64)(object)nullableValue);
			}
			if (nullableValue is SqlBoolean)
			{
				return ToValue((SqlBoolean)(object)nullableValue);
			}
			if (nullableValue is SqlString)
			{
				return ToValue((SqlString)(object)nullableValue);
			}
			if (nullableValue is SqlDateTime)
			{
				return ToValue((SqlDateTime)(object)nullableValue);
			}
			throw new ArgumentException("Unsupported INullable type: {0}".FormatWith(CultureInfo.InvariantCulture, nullableValue.GetType()));
		}

		internal static TypeConverter GetConverter(Type t)
		{
			return JsonTypeReflector.GetTypeConverter(t);
		}

		public static bool IsInteger(object value)
		{
			switch (GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
				return true;
			default:
				return false;
			}
		}

		public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
		{
			value = 0;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			for (int i = start; i < num; i++)
			{
				int num2 = chars[i] - 48;
				if (num2 < 0 || num2 > 9)
				{
					return ParseResult.Invalid;
				}
				int num3 = 10 * value - num2;
				if (num3 > value)
				{
					for (i++; i < num; i++)
					{
						num2 = chars[i] - 48;
						if (num2 < 0 || num2 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num3;
			}
			if (!flag)
			{
				if (value == int.MinValue)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
		{
			value = 0L;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			for (int i = start; i < num; i++)
			{
				int num2 = chars[i] - 48;
				if (num2 < 0 || num2 > 9)
				{
					return ParseResult.Invalid;
				}
				long num3 = 10 * value - num2;
				if (num3 > value)
				{
					for (i++; i < num; i++)
					{
						num2 = chars[i] - 48;
						if (num2 < 0 || num2 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num3;
			}
			if (!flag)
			{
				if (value == long.MinValue)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		public static bool TryConvertGuid(string s, out Guid g)
		{
			return Guid.TryParse(s, out g);
		}
	}
	internal interface IWrappedCollection : IList, ICollection, IEnumerable
	{
		object UnderlyingCollection { get; }
	}
	internal class CollectionWrapper<T> : ICollection<T>, IEnumerable<T>, IWrappedCollection, IList, ICollection, IEnumerable
	{
		private readonly IList _list;

		private readonly ICollection<T> _genericCollection;

		private object _syncRoot;

		public virtual int Count
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.Count;
				}
				return _list.Count;
			}
		}

		public virtual bool IsReadOnly
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.IsReadOnly;
				}
				return _list.IsReadOnly;
			}
		}

		bool IList.IsFixedSize
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection.IsReadOnly;
				}
				return _list.IsFixedSize;
			}
		}

		object IList.this[int index]
		{
			get
			{
				if (_genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				return _list[index];
			}
			set
			{
				if (_genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				VerifyValueType(value);
				_list[index] = (T)value;
			}
		}

		bool ICollection.IsSynchronized => false;

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		public object UnderlyingCollection
		{
			get
			{
				if (_genericCollection != null)
				{
					return _genericCollection;
				}
				return _list;
			}
		}

		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			if (list is ICollection<T>)
			{
				_genericCollection = (ICollection<T>)list;
			}
			else
			{
				_list = list;
			}
		}

		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			_genericCollection = list;
		}

		public virtual void Add(T item)
		{
			if (_genericCollection != null)
			{
				_genericCollection.Add(item);
			}
			else
			{
				_list.Add(item);
			}
		}

		public virtual void Clear()
		{
			if (_genericCollection != null)
			{
				_genericCollection.Clear();
			}
			else
			{
				_list.Clear();
			}
		}

		public virtual bool Contains(T item)
		{
			if (_genericCollection != null)
			{
				return _genericCollection.Contains(item);
			}
			return _list.Contains(item);
		}

		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (_genericCollection != null)
			{
				_genericCollection.CopyTo(array, arrayIndex);
			}
			else
			{
				_list.CopyTo(array, arrayIndex);
			}
		}

		public virtual bool Remove(T item)
		{
			if (_genericCollection != null)
			{
				return _genericCollection.Remove(item);
			}
			bool flag = _list.Contains(item);
			if (flag)
			{
				_list.Remove(item);
			}
			return flag;
		}

		public virtual IEnumerator<T> GetEnumerator()
		{
			if (_genericCollection != null)
			{
				return _genericCollection.GetEnumerator();
			}
			return _list.Cast<T>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			if (_genericCollection != null)
			{
				return _genericCollection.GetEnumerator();
			}
			return _list.GetEnumerator();
		}

		int IList.Add(object value)
		{
			VerifyValueType(value);
			Add((T)value);
			return Count - 1;
		}

		bool IList.Contains(object value)
		{
			if (IsCompatibleObject(value))
			{
				return Contains((T)value);
			}
			return false;
		}

		int IList.IndexOf(object value)
		{
			if (_genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (IsCompatibleObject(value))
			{
				return _list.IndexOf((T)value);
			}
			return -1;
		}

		void IList.RemoveAt(int index)
		{
			if (_genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
			}
			_list.RemoveAt(index);
		}

		void IList.Insert(int index, object value)
		{
			if (_genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
			}
			VerifyValueType(value);
			_list.Insert(index, (T)value);
		}

		void IList.Remove(object value)
		{
			if (IsCompatibleObject(value))
			{
				Remove((T)value);
			}
		}

		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			CopyTo((T[])array, arrayIndex);
		}

		private static void VerifyValueType(object value)
		{
			if (!IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		private static bool IsCompatibleObject(object value)
		{
			if (!(value is T) && (value != null || (typeof(T).IsValueType() && !ReflectionUtils.IsNullableType(typeof(T)))))
			{
				return false;
			}
			return true;
		}
	}
	internal static class DateTimeUtils
	{
		private const int DaysPer100Years = 36524;

		private const int DaysPer400Years = 146097;

		private const int DaysPer4Years = 1461;

		private const int DaysPerYear = 365;

		private const long TicksPerDay = 864000000000L;

		internal static readonly long InitialJavaScriptDateTicks;

		private static readonly int[] DaysToMonth365;

		private static readonly int[] DaysToMonth366;

		static DateTimeUtils()
		{
			InitialJavaScriptDateTicks = 621355968000000000L;
			DaysToMonth365 = new int[13]
			{
				0, 31, 59, 90, 120, 151, 181, 212, 243, 273,
				304, 334, 365
			};
			DaysToMonth366 = new int[13]
			{
				0, 31, 60, 91, 121, 152, 182, 213, 244, 274,
				305, 335, 366
			};
		}

		public static TimeSpan GetUtcOffset(this DateTime d)
		{
			return TimeZoneInfo.Local.GetUtcOffset(d);
		}

		public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
		{
			return kind switch
			{
				DateTimeKind.Local => XmlDateTimeSerializationMode.Local, 
				DateTimeKind.Unspecified => XmlDateTimeSerializationMode.Unspecified, 
				DateTimeKind.Utc => XmlDateTimeSerializationMode.Utc, 
				_ => throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value."), 
			};
		}

		internal static DateTime EnsureDateTime(DateTime value, DateTimeZoneHandling timeZone)
		{
			switch (timeZone)
			{
			case DateTimeZoneHandling.Local:
				value = SwitchToLocalTime(value);
				break;
			case DateTimeZoneHandling.Utc:
				value = SwitchToUtcTime(value);
				break;
			case DateTimeZoneHandling.Unspecified:
				value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
				break;
			default:
				throw new ArgumentException("Invalid date time handling value.");
			case DateTimeZoneHandling.RoundtripKind:
				break;
			}
			return value;
		}

		private static DateTime SwitchToLocalTime(DateTime value)
		{
			return value.Kind switch
			{
				DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Local), 
				DateTimeKind.Utc => value.ToLocalTime(), 
				DateTimeKind.Local => value, 
				_ => value, 
			};
		}

		private static DateTime SwitchToUtcTime(DateTime value)
		{
			return value.Kind switch
			{
				DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Utc), 
				DateTimeKind.Utc => value, 
				DateTimeKind.Local => value.ToUniversalTime(), 
				_ => value, 
			};
		}

		private static long ToUniversalTicks(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime.Ticks;
			}
			return ToUniversalTicks(dateTime, dateTime.GetUtcOffset());
		}

		private static long ToUniversalTicks(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Utc || dateTime == DateTime.MaxValue || dateTime == DateTime.MinValue)
			{
				return dateTime.Ticks;
			}
			long num = dateTime.Ticks - offset.Ticks;
			if (num > 3155378975999999999L)
			{
				return 3155378975999999999L;
			}
			if (num < 0)
			{
				return 0L;
			}
			return num;
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, TimeSpan offset)
		{
			long universialTicks = ToUniversalTicks(dateTime, offset);
			return UniversialTicksToJavaScriptTicks(universialTicks);
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime)
		{
			return ConvertDateTimeToJavaScriptTicks(dateTime, convertToUtc: true);
		}

		internal static long ConvertDateTimeToJavaScriptTicks(DateTime dateTime, bool convertToUtc)
		{
			long universialTicks = (convertToUtc ? ToUniversalTicks(dateTime) : dateTime.Ticks);
			return UniversialTicksToJavaScriptTicks(universialTicks);
		}

		private static long UniversialTicksToJavaScriptTicks(long universialTicks)
		{
			return (universialTicks - InitialJavaScriptDateTicks) / 10000;
		}

		internal static DateTime ConvertJavaScriptTicksToDateTime(long javaScriptTicks)
		{
			return new DateTime(javaScriptTicks * 10000 + InitialJavaScriptDateTicks, DateTimeKind.Utc);
		}

		internal static bool TryParseDateIso(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			DateTimeParser dateTimeParser = default(DateTimeParser);
			if (!dateTimeParser.Parse(text))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = new DateTime(dateTimeParser.Year, dateTimeParser.Month, dateTimeParser.Day, dateTimeParser.Hour, dateTimeParser.Minute, dateTimeParser.Second).AddTicks(dateTimeParser.Fraction);
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				TimeSpan offset = dateTimeParser.Zone switch
				{
					ParserTimeZone.Utc => new TimeSpan(0L), 
					ParserTimeZone.LocalWestOfUtc => new TimeSpan(-dateTimeParser.ZoneHour, -dateTimeParser.ZoneMinute, 0), 
					ParserTimeZone.LocalEastOfUtc => new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0), 
					_ => TimeZoneInfo.Local.GetUtcOffset(dateTime), 
				};
				long num = dateTime.Ticks - offset.Ticks;
				if (num < 0 || num > 3155378975999999999L)
				{
					dt = null;
					return false;
				}
				dt = new DateTimeOffset(dateTime, offset);
				return true;
			}
			switch (dateTimeParser.Zone)
			{
			case ParserTimeZone.Utc:
				dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
				break;
			case ParserTimeZone.LocalWestOfUtc:
			{
				TimeSpan timeSpan2 = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num2 = dateTime.Ticks + timeSpan2.Ticks;
				long num5 = num2;
				DateTime maxValue = DateTime.MaxValue;
				if (num5 <= maxValue.Ticks)
				{
					dateTime = new DateTime(num2, DateTimeKind.Utc).ToLocalTime();
					break;
				}
				num2 += dateTime.GetUtcOffset().Ticks;
				long num6 = num2;
				DateTime maxValue2 = DateTime.MaxValue;
				if (num6 > maxValue2.Ticks)
				{
					DateTime maxValue3 = DateTime.MaxValue;
					num2 = maxValue3.Ticks;
				}
				dateTime = new DateTime(num2, DateTimeKind.Local);
				break;
			}
			case ParserTimeZone.LocalEastOfUtc:
			{
				TimeSpan timeSpan = new TimeSpan(dateTimeParser.ZoneHour, dateTimeParser.ZoneMinute, 0);
				long num2 = dateTime.Ticks - timeSpan.Ticks;
				long num3 = num2;
				DateTime minValue = DateTime.MinValue;
				if (num3 >= minValue.Ticks)
				{
					dateTime = new DateTime(num2, DateTimeKind.Utc).ToLocalTime();
					break;
				}
				num2 += dateTime.GetUtcOffset().Ticks;
				long num4 = num2;
				DateTime minValue2 = DateTime.MinValue;
				if (num4 < minValue2.Ticks)
				{
					DateTime minValue3 = DateTime.MinValue;
					num2 = minValue3.Ticks;
				}
				dateTime = new DateTime(num2, DateTimeKind.Local);
				break;
			}
			}
			dt = EnsureDateTime(dateTime, dateTimeZoneHandling);
			return true;
		}

		internal static bool TryParseDateTime(string s, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			if (s.Length > 0)
			{
				if (s[0] == '/')
				{
					if (s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal) && TryParseDateMicrosoft(s, dateParseHandling, dateTimeZoneHandling, out dt))
					{
						return true;
					}
				}
				else if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T' && TryParseDateIso(s, dateParseHandling, dateTimeZoneHandling, out dt))
				{
					return true;
				}
				if (!string.IsNullOrEmpty(dateFormatString) && TryParseDateExact(s, dateParseHandling, dateTimeZoneHandling, dateFormatString, culture, out dt))
				{
					return true;
				}
			}
			dt = null;
			return false;
		}

		private static bool TryParseDateMicrosoft(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, out object dt)
		{
			string text2 = text.Substring(6, text.Length - 8);
			DateTimeKind dateTimeKind = DateTimeKind.Utc;
			int num = text2.IndexOf('+', 1);
			if (num == -1)
			{
				num = text2.IndexOf('-', 1);
			}
			TimeSpan timeSpan = TimeSpan.Zero;
			if (num != -1)
			{
				dateTimeKind = DateTimeKind.Local;
				timeSpan = ReadOffset(text2.Substring(num));
				text2 = text2.Substring(0, num);
			}
			if (!long.TryParse(text2, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
			{
				dt = null;
				return false;
			}
			DateTime dateTime = ConvertJavaScriptTicksToDateTime(result);
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				dt = new DateTimeOffset(dateTime.Add(timeSpan).Ticks, timeSpan);
				return true;
			}
			dt = EnsureDateTime(dateTimeKind switch
			{
				DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified), 
				DateTimeKind.Local => dateTime.ToLocalTime(), 
				_ => dateTime, 
			}, dateTimeZoneHandling);
			return true;
		}

		private static bool TryParseDateExact(string text, DateParseHandling dateParseHandling, DateTimeZoneHandling dateTimeZoneHandling, string dateFormatString, CultureInfo culture, out object dt)
		{
			DateTime result2;
			if (dateParseHandling == DateParseHandling.DateTimeOffset)
			{
				if (DateTimeOffset.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out var result))
				{
					dt = result;
					return true;
				}
			}
			else if (DateTime.TryParseExact(text, dateFormatString, culture, DateTimeStyles.RoundtripKind, out result2))
			{
				result2 = EnsureDateTime(result2, dateTimeZoneHandling);
				dt = result2;
				return true;
			}
			dt = null;
			return false;
		}

		private static TimeSpan ReadOffset(string offsetText)
		{
			bool flag = offsetText[0] == '-';
			int num = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			int num2 = 0;
			if (offsetText.Length >= 5)
			{
				num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			TimeSpan result = TimeSpan.FromHours(num) + TimeSpan.FromMinutes(num2);
			if (flag)
			{
				result = result.Negate();
			}
			return result;
		}

		internal static void WriteDateTimeString(TextWriter writer, DateTime value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = WriteDateTimeString(array, 0, value, null, value.Kind, format);
				writer.Write(array, 0, count);
			}
			else
			{
				writer.Write(value.ToString(formatString, culture));
			}
		}

		internal static int WriteDateTimeString(char[] chars, int start, DateTime value, TimeSpan? offset, DateTimeKind kind, DateFormatHandling format)
		{
			int num = start;
			if (format == DateFormatHandling.MicrosoftDateFormat)
			{
				TimeSpan offset2 = offset ?? value.GetUtcOffset();
				long num2 = ConvertDateTimeToJavaScriptTicks(value, offset2);
				"\\/Date(".CopyTo(0, chars, num, 7);
				num += 7;
				string text = num2.ToString(CultureInfo.InvariantCulture);
				text.CopyTo(0, chars, num, text.Length);
				num += text.Length;
				switch (kind)
				{
				case DateTimeKind.Unspecified:
					if (value != DateTime.MaxValue && value != DateTime.MinValue)
					{
						num = WriteDateTimeOffset(chars, num, offset2, format);
					}
					break;
				case DateTimeKind.Local:
					num = WriteDateTimeOffset(chars, num, offset2, format);
					break;
				}
				")\\/".CopyTo(0, chars, num, 3);
				num += 3;
			}
			else
			{
				num = WriteDefaultIsoDate(chars, num, value);
				switch (kind)
				{
				case DateTimeKind.Local:
					num = WriteDateTimeOffset(chars, num, offset ?? value.GetUtcOffset(), format);
					break;
				case DateTimeKind.Utc:
					chars[num++] = 'Z';
					break;
				}
			}
			return num;
		}

		internal static int WriteDefaultIsoDate(char[] chars, int start, DateTime dt)
		{
			int num = 19;
			GetDateValues(dt, out var year, out var month, out var day);
			CopyIntToCharArray(chars, start, year, 4);
			chars[start + 4] = '-';
			CopyIntToCharArray(chars, start + 5, month, 2);
			chars[start + 7] = '-';
			CopyIntToCharArray(chars, start + 8, day, 2);
			chars[start + 10] = 'T';
			CopyIntToCharArray(chars, start + 11, dt.Hour, 2);
			chars[start + 13] = ':';
			CopyIntToCharArray(chars, start + 14, dt.Minute, 2);
			chars[start + 16] = ':';
			CopyIntToCharArray(chars, start + 17, dt.Second, 2);
			int num2 = (int)(dt.Ticks % 10000000);
			if (num2 != 0)
			{
				int num3 = 7;
				while (num2 % 10 == 0)
				{
					num3--;
					num2 /= 10;
				}
				chars[start + 19] = '.';
				CopyIntToCharArray(chars, start + 20, num2, num3);
				num += num3 + 1;
			}
			return start + num;
		}

		private static void CopyIntToCharArray(char[] chars, int start, int value, int digits)
		{
			while (digits-- != 0)
			{
				chars[start + digits] = (char)(value % 10 + 48);
				value /= 10;
			}
		}

		internal static int WriteDateTimeOffset(char[] chars, int start, TimeSpan offset, DateFormatHandling format)
		{
			chars[start++] = ((offset.Ticks >= 0) ? '+' : '-');
			int value = Math.Abs(offset.Hours);
			CopyIntToCharArray(chars, start, value, 2);
			start += 2;
			if (format == DateFormatHandling.IsoDateFormat)
			{
				chars[start++] = ':';
			}
			int value2 = Math.Abs(offset.Minutes);
			CopyIntToCharArray(chars, start, value2, 2);
			start += 2;
			return start;
		}

		internal static void WriteDateTimeOffsetString(TextWriter writer, DateTimeOffset value, DateFormatHandling format, string formatString, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(formatString))
			{
				char[] array = new char[64];
				int count = WriteDateTimeString(array, 0, (format == DateFormatHandling.IsoDateFormat) ? value.DateTime : value.UtcDateTime, value.Offset, DateTimeKind.Local, format);
				writer.Write(array, 0, count);
			}
			else
			{
				writer.Write(value.ToString(formatString, culture));
			}
		}

		private static void GetDateValues(DateTime td, out int year, out int month, out int day)
		{
			long ticks = td.Ticks;
			int num = (int)(ticks / 864000000000L);
			int num2 = num / 146097;
			num -= num2 * 146097;
			int num3 = num / 36524;
			if (num3 == 4)
			{
				num3 = 3;
			}
			num -= num3 * 36524;
			int num4 = num / 1461;
			num -= num4 * 1461;
			int num5 = num / 365;
			if (num5 == 4)
			{
				num5 = 3;
			}
			year = num2 * 400 + num3 * 100 + num4 * 4 + num5 + 1;
			num -= num5 * 365;
			int[] array = ((num5 == 3 && (num4 != 24 || num3 == 3)) ? DaysToMonth366 : DaysToMonth365);
			int i;
			for (i = num >> 6; num >= array[i]; i++)
			{
			}
			month = i;
			day = num - array[i - 1] + 1;
		}
	}
	internal interface IWrappedDictionary : IDictionary, ICollection, IEnumerable
	{
		object UnderlyingDictionary { get; }
	}
	internal class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IWrappedDictionary, IDictionary, ICollection, IEnumerable
	{
		private struct DictionaryEnumerator<TEnumeratorKey, TEnumeratorValue> : IDictionaryEnumerator, IEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> _e;

			public DictionaryEntry Entry => (DictionaryEntry)Current;

			public object Key => Entry.Key;

			public object Value => Entry.Value;

			public object Current => new DictionaryEntry(_e.Current.Key, _e.Current.Value);

			public DictionaryEnumerator(IEnumerator<KeyValuePair<TEnumeratorKey, TEnumeratorValue>> e)
			{
				ValidationUtils.ArgumentNotNull(e, "e");
				_e = e;
			}

			public bool MoveNext()
			{
				return _e.MoveNext();
			}

			public void Reset()
			{
				_e.Reset();
			}
		}

		private readonly IDictionary _dictionary;

		private readonly IDictionary<TKey, TValue> _genericDictionary;

		private object _syncRoot;

		public ICollection<TKey> Keys
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Keys.Cast<TKey>().ToList();
				}
				return _genericDictionary.Keys;
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Values.Cast<TValue>().ToList();
				}
				return _genericDictionary.Values;
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				if (_dictionary != null)
				{
					return (TValue)_dictionary[key];
				}
				return _genericDictionary[key];
			}
			set
			{
				if (_dictionary != null)
				{
					_dictionary[key] = value;
				}
				else
				{
					_genericDictionary[key] = value;
				}
			}
		}

		public int Count
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.Count;
				}
				return _genericDictionary.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.IsReadOnly;
				}
				return _genericDictionary.IsReadOnly;
			}
		}

		object IDictionary.this[object key]
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary[key];
				}
				return _genericDictionary[(TKey)key];
			}
			set
			{
				if (_dictionary != null)
				{
					_dictionary[key] = value;
				}
				else
				{
					_genericDictionary[(TKey)key] = (TValue)value;
				}
			}
		}

		bool IDictionary.IsFixedSize
		{
			get
			{
				if (_genericDictionary != null)
				{
					return false;
				}
				return _dictionary.IsFixedSize;
			}
		}

		ICollection IDictionary.Keys
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Keys.ToList();
				}
				return _dictionary.Keys;
			}
		}

		ICollection IDictionary.Values
		{
			get
			{
				if (_genericDictionary != null)
				{
					return _genericDictionary.Values.ToList();
				}
				return _dictionary.Values;
			}
		}

		bool ICollection.IsSynchronized
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary.IsSynchronized;
				}
				return false;
			}
		}

		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		public object UnderlyingDictionary
		{
			get
			{
				if (_dictionary != null)
				{
					return _dictionary;
				}
				return _genericDictionary;
			}
		}

		public DictionaryWrapper(IDictionary dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_dictionary = dictionary;
		}

		public DictionaryWrapper(IDictionary<TKey, TValue> dictionary)
		{
			ValidationUtils.ArgumentNotNull(dictionary, "dictionary");
			_genericDictionary = dictionary;
		}

		public void Add(TKey key, TValue value)
		{
			if (_dictionary != null)
			{
				_dictionary.Add(key, value);
				return;
			}
			if (_genericDictionary != null)
			{
				_genericDictionary.Add(key, value);
				return;
			}
			throw new NotSupportedException();
		}

		public bool ContainsKey(TKey key)
		{
			if (_dictionary != null)
			{
				return _dictionary.Contains(key);
			}
			return _genericDictionary.ContainsKey(key);
		}

		public bool Remove(TKey key)
		{
			if (_dictionary != null)
			{
				if (_dictionary.Contains(key))
				{
					_dictionary.Remove(key);
					return true;
				}
				return false;
			}
			return _genericDictionary.Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			if (_dictionary != null)
			{
				if (!_dictionary.Contains(key))
				{
					value = default(TValue);
					return false;
				}
				value = (TValue)_dictionary[key];
				return true;
			}
			return _genericDictionary.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				((IList)_dictionary).Add(item);
			}
			else if (_genericDictionary != null)
			{
				_genericDictionary.Add(item);
			}
		}

		public void Clear()
		{
			if (_dictionary != null)
			{
				_dictionary.Clear();
			}
			else
			{
				_genericDictionary.Clear();
			}
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				return ((IList)_dictionary).Contains(item);
			}
			return _genericDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (_dictionary != null)
			{
				foreach (DictionaryEntry item in _dictionary)
				{
					ref KeyValuePair<TKey, TValue> reference = ref array[arrayIndex++];
					reference = new KeyValuePair<TKey, TValue>((TKey)item.Key, (TValue)item.Value);
				}
				return;
			}
			_genericDictionary.CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (_dictionary != null)
			{
				if (_dictionary.Contains(item.Key))
				{
					object objA = _dictionary[item.Key];
					if (object.Equals(objA, item.Value))
					{
						_dictionary.Remove(item.Key);
						return true;
					}
					return false;
				}
				return true;
			}
			return _genericDictionary.Remove(item);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			if (_dictionary != null)
			{
				return (from DictionaryEntry de in _dictionary
					select new KeyValuePair<TKey, TValue>((TKey)de.Key, (TValue)de.Value)).GetEnumerator();
			}
			return _genericDictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		void IDictionary.Add(object key, object value)
		{
			if (_dictionary != null)
			{
				_dictionary.Add(key, value);
			}
			else
			{
				_genericDictionary.Add((TKey)key, (TValue)value);
			}
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			if (_dictionary != null)
			{
				return _dictionary.GetEnumerator();
			}
			return new DictionaryEnumerator<TKey, TValue>(_genericDictionary.GetEnumerator());
		}

		bool IDictionary.Contains(object key)
		{
			if (_genericDictionary != null)
			{
				return _genericDictionary.ContainsKey((TKey)key);
			}
			return _dictionary.Contains(key);
		}

		public void Remove(object key)
		{
			if (_dictionary != null)
			{
				_dictionary.Remove(key);
			}
			else
			{
				_genericDictionary.Remove((TKey)key);
			}
		}

		void ICollection.CopyTo(Array array, int index)
		{
			if (_dictionary != null)
			{
				_dictionary.CopyTo(array, index);
			}
			else
			{
				_genericDictionary.CopyTo((KeyValuePair<TKey, TValue>[])array, index);
			}
		}
	}
	internal static class EnumUtils
	{
		private static readonly ThreadSafeStore<Type, BidirectionalDictionary<string, string>> EnumMemberNamesPerType = new ThreadSafeStore<Type, BidirectionalDictionary<string, string>>(InitializeEnumType);

		private static BidirectionalDictionary<string, string> InitializeEnumType(Type type)
		{
			BidirectionalDictionary<string, string> bidirectionalDictionary = new BidirectionalDictionary<string, string>(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				string name = fieldInfo.Name;
				string text = (from EnumMemberAttribute a in fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), inherit: true)
					select a.Value).SingleOrDefault() ?? fieldInfo.Name;
				if (bidirectionalDictionary.TryGetBySecond(text, out var _))
				{
					throw new InvalidOperationException("Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.InvariantCulture, text, type.Name));
				}
				bidirectionalDictionary.Set(name, text);
			}
			return bidirectionalDictionary;
		}

		public static IList<T> GetFlagsValues<T>(T value) where T : struct
		{
			Type typeFromHandle = typeof(T);
			if (!typeFromHandle.IsDefined(typeof(FlagsAttribute), inherit: false))
			{
				throw new ArgumentException("Enum type {0} is not a set of flags.".FormatWith(CultureInfo.InvariantCulture, typeFromHandle));
			}
			Type underlyingType = Enum.GetUnderlyingType(value.GetType());
			ulong num = Convert.ToUInt64(value, CultureInfo.InvariantCulture);
			IList<EnumValue<ulong>> namesAndValues = GetNamesAndValues<T>();
			IList<T> list = new List<T>();
			foreach (EnumValue<ulong> item in namesAndValues)
			{
				if ((num & item.Value) == item.Value && item.Value != 0)
				{
					list.Add((T)Convert.ChangeType(item.Value, underlyingType, CultureInfo.CurrentCulture));
				}
			}
			if (list.Count == 0 && namesAndValues.SingleOrDefault((EnumValue<ulong> v) => v.Value == 0) != null)
			{
				list.Add(default(T));
			}
			return list;
		}

		public static IList<EnumValue<ulong>> GetNamesAndValues<T>() where T : struct
		{
			return GetNamesAndValues<ulong>(typeof(T));
		}

		public static IList<EnumValue<TUnderlyingType>> GetNamesAndValues<TUnderlyingType>(Type enumType) where TUnderlyingType : struct
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			ValidationUtils.ArgumentTypeIsEnum(enumType, "enumType");
			IList<object> values = GetValues(enumType);
			IList<string> names = GetNames(enumType);
			IList<EnumValue<TUnderlyingType>> list = new List<EnumValue<TUnderlyingType>>();
			for (int i = 0; i < values.Count; i++)
			{
				try
				{
					list.Add(new EnumValue<TUnderlyingType>(names[i], (TUnderlyingType)Convert.ChangeType(values[i], typeof(TUnderlyingType), CultureInfo.CurrentCulture)));
				}
				catch (OverflowException innerException)
				{
					throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Value from enum with the underlying type of {0} cannot be added to dictionary with a value type of {1}. Value was too large: {2}", new object[3]
					{
						Enum.GetUnderlyingType(enumType),
						typeof(TUnderlyingType),
						Convert.ToUInt64(values[i], CultureInfo.InvariantCulture)
					}), innerException);
				}
			}
			return list;
		}

		public static IList<object> GetValues(Type enumType)
		{
			if (!enumType.IsEnum())
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			List<object> list = new List<object>();
			IEnumerable<FieldInfo> enumerable = from f in enumType.GetFields()
				where f.IsLiteral
				select f;
			foreach (FieldInfo item in enumerable)
			{
				object value = item.GetValue(enumType);
				list.Add(value);
			}
			return list;
		}

		public static IList<string> GetNames(Type enumType)
		{
			if (!enumType.IsEnum())
			{
				throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");
			}
			List<string> list = new List<string>();
			IEnumerable<FieldInfo> enumerable = from f in enumType.GetFields()
				where f.IsLiteral
				select f;
			foreach (FieldInfo item in enumerable)
			{
				list.Add(item.Name);
			}
			return list;
		}

		public static object ParseEnumName(string enumText, bool isNullable, Type t)
		{
			if (enumText == string.Empty && isNullable)
			{
				return null;
			}
			BidirectionalDictionary<string, string> map = EnumMemberNamesPerType.Get(t);
			string value;
			if (enumText.IndexOf(',') != -1)
			{
				string[] array = enumText.Split(new char[1] { ',' });
				for (int i = 0; i < array.Length; i++)
				{
					string enumText2 = array[i].Trim();
					array[i] = ResolvedEnumName(map, enumText2);
				}
				value = string.Join(", ", array);
			}
			else
			{
				value = ResolvedEnumName(map, enumText);
			}
			return Enum.Parse(t, value, ignoreCase: true);
		}

		public static string ToEnumName(Type enumType, string enumText, bool camelCaseText)
		{
			BidirectionalDictionary<string, string> bidirectionalDictionary = EnumMemberNamesPerType.Get(enumType);
			string[] array = enumText.Split(new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				bidirectionalDictionary.TryGetByFirst(text, out var second);
				second = second ?? text;
				if (camelCaseText)
				{
					second = StringUtils.ToCamelCase(second);
				}
				array[i] = second;
			}
			return string.Join(", ", array);
		}

		private static string ResolvedEnumName(BidirectionalDictionary<string, string> map, string enumText)
		{
			map.TryGetBySecond(enumText, out var first);
			return first ?? enumText;
		}
	}
	internal class EnumValue<T> where T : struct
	{
		private readonly string _name;

		private readonly T _value;

		public string Name => _name;

		public T Value => _value;

		public EnumValue(string name, T value)
		{
			_name = name;
			_value = value;
		}
	}
	internal static class JavaScriptUtils
	{
		private const string EscapedUnicodeText = "!";

		internal static readonly bool[] SingleQuoteCharEscapeFlags;

		internal static readonly bool[] DoubleQuoteCharEscapeFlags;

		internal static readonly bool[] HtmlCharEscapeFlags;

		static JavaScriptUtils()
		{
			SingleQuoteCharEscapeFlags = new bool[128];
			DoubleQuoteCharEscapeFlags = new bool[128];
			HtmlCharEscapeFlags = new bool[128];
			IList<char> list = new List<char> { '\n', '\r', '\t', '\\', '\f', '\b' };
			for (int i = 0; i < 32; i++)
			{
				list.Add((char)i);
			}
			foreach (char item in list.Union(new char[1] { '\'' }))
			{
				SingleQuoteCharEscapeFlags[(uint)item] = true;
			}
			foreach (char item2 in list.Union(new char[1] { '"' }))
			{
				DoubleQuoteCharEscapeFlags[(uint)item2] = true;
			}
			foreach (char item3 in list.Union(new char[5] { '"', '\'', '<', '>', '&' }))
			{
				HtmlCharEscapeFlags[(uint)item3] = true;
			}
		}

		public static bool[] GetCharEscapeFlags(StringEscapeHandling stringEscapeHandling, char quoteChar)
		{
			if (stringEscapeHandling == StringEscapeHandling.EscapeHtml)
			{
				return HtmlCharEscapeFlags;
			}
			if (quoteChar == '"')
			{
				return DoubleQuoteCharEscapeFlags;
			}
			return SingleQuoteCharEscapeFlags;
		}

		public static bool ShouldEscapeJavaScriptString(string s, bool[] charEscapeFlags)
		{
			if (s == null)
			{
				return false;
			}
			foreach (char c in s)
			{
				if (c >= charEscapeFlags.Length || charEscapeFlags[(uint)c])
				{
					return true;
				}
			}
			return false;
		}

		public static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter, bool appendDelimiters, bool[] charEscapeFlags, StringEscapeHandling stringEscapeHandling, ref char[] writeBuffer)
		{
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
			if (s != null)
			{
				int num = 0;
				for (int i = 0; i < s.Length; i++)
				{
					char c = s[i];
					if (c < charEscapeFlags.Length && !charEscapeFlags[(uint)c])
					{
						continue;
					}
					string text;
					switch (c)
					{
					case '\t':
						text = "\\t";
						break;
					case '\n':
						text = "\\n";
						break;
					case '\r':
						text = "\\r";
						break;
					case '\f':
						text = "\\f";
						break;
					case '\b':
						text = "\\b";
						break;
					case '\\':
						text = "\\\\";
						break;
					case '\u0085':
						text = "\\u0085";
						break;
					case '\u2028':
						text = "\\u2028";
						break;
					case '\u2029':
						text = "\\u2029";
						break;
					default:
						if (c < charEscapeFlags.Length || stringEscapeHandling == StringEscapeHandling.EscapeNonAscii)
						{
							if (c == '\'' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
							{
								text = "\\'";
								break;
							}
							if (c == '"' && stringEscapeHandling != StringEscapeHandling.EscapeHtml)
							{
								text = "\\\"";
								break;
							}
							if (writeBuffer == null)
							{
								writeBuffer = new char[6];
							}
							StringUtils.ToCharAsUnicode(c, writeBuffer);
							text = "!";
						}
						else
						{
							text = null;
						}
						break;
					}
					if (text == null)
					{
						continue;
					}
					bool flag = string.Equals(text, "!");
					if (i > num)
					{
						int num2 = i - num + (flag ? 6 : 0);
						int num3 = (flag ? 6 : 0);
						if (writeBuffer == null || writeBuffer.Length < num2)
						{
							char[] array = new char[num2];
							if (flag)
							{
								Array.Copy(writeBuffer, array, 6);
							}
							writeBuffer = array;
						}
						s.CopyTo(num, writeBuffer, num3, num2 - num3);
						writer.Write(writeBuffer, num3, num2 - num3);
					}
					num = i + 1;
					if (!flag)
					{
						writer.Write(text);
					}
					else
					{
						writer.Write(writeBuffer, 0, 6);
					}
				}
				if (num == 0)
				{
					writer.Write(s);
				}
				else
				{
					int num4 = s.Length - num;
					if (writeBuffer == null || writeBuffer.Length < num4)
					{
						writeBuffer = new char[num4];
					}
					s.CopyTo(num, writeBuffer, 0, num4);
					writer.Write(writeBuffer, 0, num4);
				}
			}
			if (appendDelimiters)
			{
				writer.Write(delimiter);
			}
		}

		public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
		{
			return ToEscapedJavaScriptString(value, delimiter, appendDelimiters, StringEscapeHandling.Default);
		}

		public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters, StringEscapeHandling stringEscapeHandling)
		{
			bool[] charEscapeFlags = GetCharEscapeFlags(stringEscapeHandling, delimiter);
			using StringWriter stringWriter = StringUtils.CreateStringWriter(StringUtils.GetLength(value) ?? 16);
			char[] writeBuffer = null;
			WriteEscapedJavaScriptString(stringWriter, value, delimiter, appendDelimiters, charEscapeFlags, stringEscapeHandling, ref writeBuffer);
			return stringWriter.ToString();
		}
	}
}
namespace Newtonsoft.Json
{
	public enum JsonToken
	{
		None,
		StartObject,
		StartArray,
		StartConstructor,
		PropertyName,
		Comment,
		Raw,
		Integer,
		Float,
		String,
		Boolean,
		Null,
		Undefined,
		EndObject,
		EndArray,
		EndConstructor,
		Date,
		Bytes
	}
}
namespace Newtonsoft.Json.Utilities
{
	internal class StringBuffer
	{
		private char[] _buffer;

		private int _position;

		private static readonly char[] EmptyBuffer = new char[0];

		public int Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public StringBuffer()
		{
			_buffer = EmptyBuffer;
		}

		public StringBuffer(int initalSize)
		{
			_buffer = new char[initalSize];
		}

		public void Append(char value)
		{
			if (_position == _buffer.Length)
			{
				EnsureSize(1);
			}
			_buffer[_position++] = value;
		}

		public void Append(char[] buffer, int startIndex, int count)
		{
			if (_position + count >= _buffer.Length)
			{
				EnsureSize(count);
			}
			Array.Copy(buffer, startIndex, _buffer, _position, count);
			_position += count;
		}

		public void Clear()
		{
			_buffer = EmptyBuffer;
			_position = 0;
		}

		private void EnsureSize(int appendLength)
		{
			char[] array = new char[(_position + appendLength) * 2];
			Array.Copy(_buffer, array, _position);
			_buffer = array;
		}

		public override string ToString()
		{
			return ToString(0, _position);
		}

		public string ToString(int start, int length)
		{
			return new string(_buffer, start, length);
		}

		public char[] GetInternalBuffer()
		{
			return _buffer;
		}
	}
	internal static class CollectionUtils
	{
		public static bool IsNullOrEmpty<T>(ICollection<T> collection)
		{
			if (collection != null)
			{
				return collection.Count == 0;
			}
			return true;
		}

		public static void AddRange<T>(this IList<T> initial, IEnumerable<T> collection)
		{
			if (initial == null)
			{
				throw new ArgumentNullException("initial");
			}
			if (collection == null)
			{
				return;
			}
			foreach (T item in collection)
			{
				initial.Add(item);
			}
		}

		public static bool IsDictionaryType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (ReflectionUtils.ImplementsGenericDefinition(type, typeof(IDictionary<, >)))
			{
				return true;
			}
			return false;
		}

		public static ConstructorInfo ResolveEnumerableCollectionConstructor(Type collectionType, Type collectionItemType)
		{
			Type type = typeof(IEnumerable<>).MakeGenericType(collectionItemType);
			ConstructorInfo constructorInfo = null;
			ConstructorInfo[] constructors = collectionType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			foreach (ConstructorInfo constructorInfo2 in constructors)
			{
				IList<ParameterInfo> parameters = constructorInfo2.GetParameters();
				if (parameters.Count == 1)
				{
					if (type == parameters[0].ParameterType)
					{
						constructorInfo = constructorInfo2;
						break;
					}
					if (constructorInfo == null && type.IsAssignableFrom(parameters[0].ParameterType))
					{
						constructorInfo = constructorInfo2;
					}
				}
			}
			return constructorInfo;
		}

		public static bool AddDistinct<T>(this IList<T> list, T value)
		{
			return list.AddDistinct(value, EqualityComparer<T>.Default);
		}

		public static bool AddDistinct<T>(this IList<T> list, T value, IEqualityComparer<T> comparer)
		{
			if (list.ContainsValue(value, comparer))
			{
				return false;
			}
			list.Add(value);
			return true;
		}

		public static bool ContainsValue<TSource>(this IEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer)
		{
			if (comparer == null)
			{
				comparer = EqualityComparer<TSource>.Default;
			}
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			foreach (TSource item in source)
			{
				if (comparer.Equals(item, value))
				{
					return true;
				}
			}
			return false;
		}

		public static bool AddRangeDistinct<T>(this IList<T> list, IEnumerable<T> values, IEqualityComparer<T> comparer)
		{
			bool result = true;
			foreach (T value in values)
			{
				if (!list.AddDistinct(value, comparer))
				{
					result = false;
				}
			}
			return result;
		}

		public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
		{
			int num = 0;
			foreach (T item in collection)
			{
				if (predicate(item))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static int IndexOf<TSource>(this IEnumerable<TSource> list, TSource value, IEqualityComparer<TSource> comparer)
		{
			int num = 0;
			foreach (TSource item in list)
			{
				if (comparer.Equals(item, value))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		private static IList<int> GetDimensions(IList values, int dimensionsCount)
		{
			IList<int> list = new List<int>();
			IList list2 = values;
			while (true)
			{
				list.Add(list2.Count);
				if (list.Count == dimensionsCount || list2.Count == 0)
				{
					break;
				}
				object obj = list2[0];
				if (!(obj is IList))
				{
					break;
				}
				list2 = (IList)obj;
			}
			return list;
		}

		private static void CopyFromJaggedToMultidimensionalArray(IList values, Array multidimensionalArray, int[] indices)
		{
			int num = indices.Length;
			if (num == multidimensionalArray.Rank)
			{
				multidimensionalArray.SetValue(JaggedArrayGetValue(values, indices), indices);
				return;
			}
			int length = multidimensionalArray.GetLength(num);
			IList list = (IList)JaggedArrayGetValue(values, indices);
			int count = list.Count;
			if (count != length)
			{
				throw new Exception("Cannot deserialize non-cubical array as multidimensional array.");
			}
			int[] array = new int[num + 1];
			for (int i = 0; i < num; i++)
			{
				array[i] = indices[i];
			}
			for (int j = 0; j < multidimensionalArray.GetLength(num); j++)
			{
				array[num] = j;
				CopyFromJaggedToMultidimensionalArray(values, multidimensionalArray, array);
			}
		}

		private static object JaggedArrayGetValue(IList values, int[] indices)
		{
			IList list = values;
			for (int i = 0; i < indices.Length; i++)
			{
				int index = indices[i];
				if (i == indices.Length - 1)
				{
					return list[index];
				}
				list = (IList)list[index];
			}
			return list;
		}

		public static Array ToMultidimensionalArray(IList values, Type type, int rank)
		{
			IList<int> dimensions = GetDimensions(values, rank);
			while (dimensions.Count < rank)
			{
				dimensions.Add(0);
			}
			Array array = Array.CreateInstance(type, dimensions.ToArray());
			CopyFromJaggedToMultidimensionalArray(values, array, new int[0]);
			return array;
		}
	}
	internal static class MathUtils
	{
		public static int IntLength(ulong i)
		{
			if (i < 10000000000L)
			{
				if (i < 10)
				{
					return 1;
				}
				if (i < 100)
				{
					return 2;
				}
				if (i < 1000)
				{
					return 3;
				}
				if (i < 10000)
				{
					return 4;
				}
				if (i < 100000)
				{
					return 5;
				}
				if (i < 1000000)
				{
					return 6;
				}
				if (i < 10000000)
				{
					return 7;
				}
				if (i < 100000000)
				{
					return 8;
				}
				if (i < 1000000000)
				{
					return 9;
				}
				return 10;
			}
			if (i < 100000000000L)
			{
				return 11;
			}
			if (i < 1000000000000L)
			{
				return 12;
			}
			if (i < 10000000000000L)
			{
				return 13;
			}
			if (i < 100000000000000L)
			{
				return 14;
			}
			if (i < 1000000000000000L)
			{
				return 15;
			}
			if (i < 10000000000000000L)
			{
				return 16;
			}
			if (i < 100000000000000000L)
			{
				return 17;
			}
			if (i < 1000000000000000000L)
			{
				return 18;
			}
			if (i < 10000000000000000000uL)
			{
				return 19;
			}
			return 20;
		}

		public static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		public static int? Min(int? val1, int? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Min(val1.Value, val2.Value);
		}

		public static int? Max(int? val1, int? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Max(val1.Value, val2.Value);
		}

		public static double? Max(double? val1, double? val2)
		{
			if (!val1.HasValue)
			{
				return val2;
			}
			if (!val2.HasValue)
			{
				return val1;
			}
			return Math.Max(val1.Value, val2.Value);
		}

		public static bool ApproxEquals(double d1, double d2)
		{
			if (d1 == d2)
			{
				return true;
			}
			double num = (Math.Abs(d1) + Math.Abs(d2) + 10.0) * 2.220446049250313E-16;
			double num2 = d1 - d2;
			if (0.0 - num < num2)
			{
				return num > num2;
			}
			return false;
		}
	}
	internal delegate T Creator<T>();
	internal static class MiscellaneousUtils
	{
		public static bool ValueEquals(object objA, object objB)
		{
			if (objA == null && objB == null)
			{
				return true;
			}
			if (objA != null && objB == null)
			{
				return false;
			}
			if (objA == null && objB != null)
			{
				return false;
			}
			if (objA.GetType() != objB.GetType())
			{
				if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
				{
					return Convert.ToDecimal(objA, CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, CultureInfo.CurrentCulture));
				}
				if ((objA is double || objA is float || objA is decimal) && (objB is double || objB is float || objB is decimal))
				{
					return MathUtils.ApproxEquals(Convert.ToDouble(objA, CultureInfo.CurrentCulture), Convert.ToDouble(objB, CultureInfo.CurrentCulture));
				}
				return false;
			}
			return objA.Equals(objB);
		}

		public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
		{
			string message2 = message + Environment.NewLine + "Actual value was {0}.".FormatWith(CultureInfo.InvariantCulture, actualValue);
			return new ArgumentOutOfRangeException(paramName, message2);
		}

		public static string ToString(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (!(value is string))
			{
				return value.ToString();
			}
			return "\"" + value.ToString() + "\"";
		}

		public static int ByteArrayCompare(byte[] a1, byte[] a2)
		{
			int num = a1.Length.CompareTo(a2.Length);
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < a1.Length; i++)
			{
				int num2 = a1[i].CompareTo(a2[i]);
				if (num2 != 0)
				{
					return num2;
				}
			}
			return 0;
		}

		public static string GetPrefix(string qualifiedName)
		{
			GetQualifiedNameParts(qualifiedName, out var prefix, out var _);
			return prefix;
		}

		public static string GetLocalName(string qualifiedName)
		{
			GetQualifiedNameParts(qualifiedName, out var _, out var localName);
			return localName;
		}

		public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
		{
			int num = qualifiedName.IndexOf(':');
			if (num == -1 || num == 0 || qualifiedName.Length - 1 == num)
			{
				prefix = null;
				localName = qualifiedName;
			}
			else
			{
				prefix = qualifiedName.Substring(0, num);
				localName = qualifiedName.Substring(num + 1);
			}
		}

		internal static string FormatValueForPrint(object value)
		{
			if (value == null)
			{
				return "{null}";
			}
			if (value is string)
			{
				return string.Concat("\"", value, "\"");
			}
			return value.ToString();
		}
	}
	internal static class ReflectionUtils
	{
		public static readonly Type[] EmptyTypes;

		static ReflectionUtils()
		{
			EmptyTypes = Type.EmptyTypes;
		}

		public static bool IsVirtual(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			getMethod = propertyInfo.GetSetMethod();
			if (getMethod != null && getMethod.IsVirtual)
			{
				return true;
			}
			return false;
		}

		public static MethodInfo GetBaseDefinition(this PropertyInfo propertyInfo)
		{
			ValidationUtils.ArgumentNotNull(propertyInfo, "propertyInfo");
			MethodInfo getMethod = propertyInfo.GetGetMethod();
			if (getMethod != null)
			{
				return getMethod.GetBaseDefinition();
			}
			getMethod = propertyInfo.GetSetMethod();
			if (getMethod != null)
			{
				return getMethod.GetBaseDefinition();
			}
			return null;
		}

		public static bool IsPublic(PropertyInfo property)
		{
			if (property.GetGetMethod() != null && property.GetGetMethod().IsPublic)
			{
				return true;
			}
			if (property.GetSetMethod() != null && property.GetSetMethod().IsPublic)
			{
				return true;
			}
			return false;
		}

		public static Type GetObjectType(object v)
		{
			return v?.GetType();
		}

		public static string GetTypeName(Type t, FormatterAssemblyStyle assemblyFormat, SerializationBinder binder)
		{
			string text;
			if (binder != null)
			{
				binder.BindToName(t, out string assemblyName, out string typeName);
				text = typeName + ((assemblyName == null) ? "" : (", " + assemblyName));
			}
			else
			{
				text = t.AssemblyQualifiedName;
			}
			return assemblyFormat switch
			{
				FormatterAssemblyStyle.Simple => RemoveAssemblyDetails(text), 
				FormatterAssemblyStyle.Full => text, 
				_ => throw new ArgumentOutOfRangeException(), 
			};
		}

		private static string RemoveAssemblyDetails(string fullyQualifiedTypeName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			foreach (char c in fullyQualifiedTypeName)
			{
				switch (c)
				{
				case '[':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					break;
				case ']':
					flag = false;
					flag2 = false;
					stringBuilder.Append(c);
					break;
				case ',':
					if (!flag)
					{
						flag = true;
						stringBuilder.Append(c);
					}
					else
					{
						flag2 = true;
					}
					break;
				default:
					if (!flag2)
					{
						stringBuilder.Append(c);
					}
					break;
				}
			}
			return stringBuilder.ToString();
		}

		public static bool HasDefaultConstructor(Type t, bool nonPublic)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsValueType())
			{
				return true;
			}
			return GetDefaultConstructor(t, nonPublic) != null;
		}

		public static ConstructorInfo GetDefaultConstructor(Type t)
		{
			return GetDefaultConstructor(t, nonPublic: false);
		}

		public static ConstructorInfo GetDefaultConstructor(Type t, bool nonPublic)
		{
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
			if (nonPublic)
			{
				bindingFlags |= BindingFlags.NonPublic;
			}
			return t.GetConstructors(bindingFlags).SingleOrDefault((ConstructorInfo c) => !c.GetParameters().Any());
		}

		public static bool IsNullable(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsValueType())
			{
				return IsNullableType(t);
			}
			return true;
		}

		public static bool IsNullableType(Type t)
		{
			ValidationUtils.ArgumentNotNull(t, "t");
			if (t.IsGenericType())
			{
				return t.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			return false;
		}

		public static Type EnsureNotNullableType(Type t)
		{
			if (!IsNullableType(t))
			{
				return t;
			}
			return Nullable.GetUnderlyingType(t);
		}

		public static bool IsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			if (!type.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == genericInterfaceDefinition;
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
		{
			Type implementingType;
			return ImplementsGenericDefinition(type, genericInterfaceDefinition, out implementingType);
		}

		public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericInterfaceDefinition, "genericInterfaceDefinition");
			if (!genericInterfaceDefinition.IsInterface() || !genericInterfaceDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic interface definition.".FormatWith(CultureInfo.InvariantCulture, genericInterfaceDefinition));
			}
			if (type.IsInterface() && type.IsGenericType())
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericInterfaceDefinition == genericTypeDefinition)
				{
					implementingType = type;
					return true;
				}
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				if (type2.IsGenericType())
				{
					Type genericTypeDefinition2 = type2.GetGenericTypeDefinition();
					if (genericInterfaceDefinition == genericTypeDefinition2)
					{
						implementingType = type2;
						return true;
					}
				}
			}
			implementingType = null;
			return false;
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
		{
			Type implementingType;
			return InheritsGenericDefinition(type, genericClassDefinition, out implementingType);
		}

		public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			ValidationUtils.ArgumentNotNull(genericClassDefinition, "genericClassDefinition");
			if (!genericClassDefinition.IsClass() || !genericClassDefinition.IsGenericTypeDefinition())
			{
				throw new ArgumentNullException("'{0}' is not a generic class definition.".FormatWith(CultureInfo.InvariantCulture, genericClassDefinition));
			}
			return InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
		}

		private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
		{
			if (currentType.IsGenericType())
			{
				Type genericTypeDefinition = currentType.GetGenericTypeDefinition();
				if (genericClassDefinition == genericTypeDefinition)
				{
					implementingType = currentType;
					return true;
				}
			}
			if (currentType.BaseType() == null)
			{
				implementingType = null;
				return false;
			}
			return InheritsGenericDefinitionInternal(currentType.BaseType(), genericClassDefinition, out implementingType);
		}

		public static Type GetCollectionItemType(Type type)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			if (type.IsArray)
			{
				return type.GetElementType();
			}
			if (ImplementsGenericDefinition(type, typeof(IEnumerable<>), out var implementingType))
			{
				if (implementingType.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
				}
				return implementingType.GetGenericArguments()[0];
			}
			if (typeof(IEnumerable).IsAssignableFrom(type))
			{
				return null;
			}
			throw new Exception("Type {0} is not a collection.".FormatWith(CultureInfo.InvariantCulture, type));
		}

		public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
		{
			ValidationUtils.ArgumentNotNull(dictionaryType, "type");
			if (ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<, >), out var implementingType))
			{
				if (implementingType.IsGenericTypeDefinition())
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				Type[] genericArguments = implementingType.GetGenericArguments();
				keyType = genericArguments[0];
				valueType = genericArguments[1];
			}
			else
			{
				if (!typeof(IDictionary).IsAssignableFrom(dictionaryType))
				{
					throw new Exception("Type {0} is not a dictionary.".FormatWith(CultureInfo.InvariantCulture, dictionaryType));
				}
				keyType = null;
				valueType = null;
			}
		}

		public static Type GetMemberUnderlyingType(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			return member.MemberType() switch
			{
				MemberTypes.Field => ((FieldInfo)member).FieldType, 
				MemberTypes.Property => ((PropertyInfo)member).PropertyType, 
				MemberTypes.Event => ((EventInfo)member).EventHandlerType, 
				MemberTypes.Method => ((MethodInfo)member).ReturnType, 
				_ => throw new ArgumentException("MemberInfo must be of type FieldInfo, PropertyInfo, EventInfo or MethodInfo", "member"), 
			};
		}

		public static bool IsIndexedProperty(MemberInfo member)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return IsIndexedProperty(propertyInfo);
			}
			return false;
		}

		public static bool IsIndexedProperty(PropertyInfo property)
		{
			ValidationUtils.ArgumentNotNull(property, "property");
			return property.GetIndexParameters().Length > 0;
		}

		public static object GetMemberValue(MemberInfo member, object target)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			switch (member.MemberType())
			{
			case MemberTypes.Field:
				return ((FieldInfo)member).GetValue(target);
			case MemberTypes.Property:
				try
				{
					return ((PropertyInfo)member).GetValue(target, null);
				}
				catch (TargetParameterCountException innerException)
				{
					throw new ArgumentException("MemberInfo '{0}' has index parameters".FormatWith(CultureInfo.InvariantCulture, member.Name), innerException);
				}
			default:
				throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture, member.Name), "member");
			}
		}

		public static void SetMemberValue(MemberInfo member, object target, object value)
		{
			ValidationUtils.ArgumentNotNull(member, "member");
			ValidationUtils.ArgumentNotNull(target, "target");
			switch (member.MemberType())
			{
			case MemberTypes.Field:
				((FieldInfo)member).SetValue(target, value);
				break;
			case MemberTypes.Property:
				((PropertyInfo)member).SetValue(target, value, null);
				break;
			default:
				throw new ArgumentException("MemberInfo '{0}' must be of type FieldInfo or PropertyInfo".FormatWith(CultureInfo.InvariantCulture, member.Name), "member");
			}
		}

		public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
		{
			switch (member.MemberType())
			{
			case MemberTypes.Field:
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (nonPublic)
				{
					return true;
				}
				if (fieldInfo.IsPublic)
				{
					return true;
				}
				return false;
			}
			case MemberTypes.Property:
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				if (!propertyInfo.CanRead)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				return propertyInfo.GetGetMethod(nonPublic) != null;
			}
			default:
				return false;
			}
		}

		public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
		{
			switch (member.MemberType())
			{
			case MemberTypes.Field:
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (fieldInfo.IsLiteral)
				{
					return false;
				}
				if (fieldInfo.IsInitOnly && !canSetReadOnly)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				if (fieldInfo.IsPublic)
				{
					return true;
				}
				return false;
			}
			case MemberTypes.Property:
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				if (!propertyInfo.CanWrite)
				{
					return false;
				}
				if (nonPublic)
				{
					return true;
				}
				return propertyInfo.GetSetMethod(nonPublic) != null;
			}
			default:
				return false;
			}
		}

		public static List<MemberInfo> GetFieldsAndProperties(Type type, BindingFlags bindingAttr)
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.AddRange(GetFields(type, bindingAttr));
			list.AddRange(GetProperties(type, bindingAttr));
			List<MemberInfo> list2 = new List<MemberInfo>(list.Count);
			foreach (IGrouping<string, MemberInfo> item in from m in list
				group m by m.Name)
			{
				int num = item.Count();
				IList<MemberInfo> list3 = item.ToList();
				if (num == 1)
				{
					list2.Add(list3.First());
					continue;
				}
				IList<MemberInfo> list4 = new List<MemberInfo>();
				foreach (MemberInfo item2 in list3)
				{
					if (list4.Count == 0)
					{
						list4.Add(item2);
					}
					else if (!IsOverridenGenericMember(item2, bindingAttr) || item2.Name == "Item")
					{
						list4.Add(item2);
					}
				}
				list2.AddRange(list4);
			}
			return list2;
		}

		private static bool IsOverridenGenericMember(MemberInfo memberInfo, BindingFlags bindingAttr)
		{
			if (memberInfo.MemberType() != MemberTypes.Property)
			{
				return false;
			}
			PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
			if (!propertyInfo.IsVirtual())
			{
				return false;
			}
			Type declaringType = propertyInfo.DeclaringType;
			if (!declaringType.IsGenericType())
			{
				return false;
			}
			Type genericTypeDefinition = declaringType.GetGenericTypeDefinition();
			if (genericTypeDefinition == null)
			{
				return false;
			}
			MemberInfo[] member = genericTypeDefinition.GetMember(propertyInfo.Name, bindingAttr);
			if (member.Length == 0)
			{
				return false;
			}
			Type memberUnderlyingType = GetMemberUnderlyingType(member[0]);
			if (!memberUnderlyingType.IsGenericParameter)
			{
				return false;
			}
			return true;
		}

		public static T GetAttribute<T>(object attributeProvider) where T : Attribute
		{
			return GetAttribute<T>(attributeProvider, inherit: true);
		}

		public static T GetAttribute<T>(object attributeProvider, bool inherit) where T : Attribute
		{
			T[] attributes = GetAttributes<T>(attributeProvider, inherit);
			if (attributes == null)
			{
				return null;
			}
			return attributes.FirstOrDefault();
		}

		public static T[] GetAttributes<T>(object attributeProvider, bool inherit) where T : Attribute
		{
			Attribute[] attributes = GetAttributes(attributeProvider, typeof(T), inherit);
			if (attributes is T[] result)
			{
				return result;
			}
			return attributes.Cast<T>().ToArray();
		}

		public static Attribute[] GetAttributes(object attributeProvider, Type attributeType, bool inherit)
		{
			ValidationUtils.ArgumentNotNull(attributeProvider, "attributeProvider");
			if (attributeProvider is Type)
			{
				Type type = (Type)attributeProvider;
				object[] source = ((attributeType != null) ? type.GetCustomAttributes(attributeType, inherit) : type.GetCustomAttributes(inherit));
				return source.Cast<Attribute>().ToArray();
			}
			if (attributeProvider is Assembly)
			{
				Assembly element = (Assembly)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element);
				}
				return Attribute.GetCustomAttributes(element, attributeType);
			}
			if (attributeProvider is MemberInfo)
			{
				MemberInfo element2 = (MemberInfo)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element2, inherit);
				}
				return Attribute.GetCustomAttributes(element2, attributeType, inherit);
			}
			if (attributeProvider is Module)
			{
				Module element3 = (Module)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element3, inherit);
				}
				return Attribute.GetCustomAttributes(element3, attributeType, inherit);
			}
			if (attributeProvider is ParameterInfo)
			{
				ParameterInfo element4 = (ParameterInfo)attributeProvider;
				if (!(attributeType != null))
				{
					return Attribute.GetCustomAttributes(element4, inherit);
				}
				return Attribute.GetCustomAttributes(element4, attributeType, inherit);
			}
			ICustomAttributeProvider customAttributeProvider = (ICustomAttributeProvider)attributeProvider;
			object[] array = ((attributeType != null) ? customAttributeProvider.GetCustomAttributes(attributeType, inherit) : customAttributeProvider.GetCustomAttributes(inherit));
			return (Attribute[])array;
		}

		public static void SplitFullyQualifiedTypeName(string fullyQualifiedTypeName, out string typeName, out string assemblyName)
		{
			int? assemblyDelimiterIndex = GetAssemblyDelimiterIndex(fullyQualifiedTypeName);
			if (assemblyDelimiterIndex.HasValue)
			{
				typeName = fullyQualifiedTypeName.Substring(0, assemblyDelimiterIndex.Value).Trim();
				assemblyName = fullyQualifiedTypeName.Substring(assemblyDelimiterIndex.Value + 1, fullyQualifiedTypeName.Length - assemblyDelimiterIndex.Value - 1).Trim();
			}
			else
			{
				typeName = fullyQualifiedTypeName;
				assemblyName = null;
			}
		}

		private static int? GetAssemblyDelimiterIndex(string fullyQualifiedTypeName)
		{
			int num = 0;
			for (int i = 0; i < fullyQualifiedTypeName.Length; i++)
			{
				switch (fullyQualifiedTypeName[i])
				{
				case '[':
					num++;
					break;
				case ']':
					num--;
					break;
				case ',':
					if (num == 0)
					{
						return i;
					}
					break;
				}
			}
			return null;
		}

		public static MemberInfo GetMemberInfoFromType(Type targetType, MemberInfo memberInfo)
		{
			MemberTypes memberTypes = memberInfo.MemberType();
			if (memberTypes == MemberTypes.Property)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				Type[] types = (from p in propertyInfo.GetIndexParameters()
					select p.ParameterType).ToArray();
				return targetType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, propertyInfo.PropertyType, types, null);
			}
			return targetType.GetMember(memberInfo.Name, memberInfo.MemberType(), BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).SingleOrDefault();
		}

		public static IEnumerable<FieldInfo> GetFields(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<MemberInfo> list = new List<MemberInfo>(targetType.GetFields(bindingAttr));
			GetChildPrivateFields(list, targetType, bindingAttr);
			return list.Cast<FieldInfo>();
		}

		private static void GetChildPrivateFields(IList<MemberInfo> initialFields, Type targetType, BindingFlags bindingAttr)
		{
			if ((bindingAttr & BindingFlags.NonPublic) == 0)
			{
				return;
			}
			BindingFlags bindingAttr2 = bindingAttr.RemoveFlag(BindingFlags.Public);
			while ((targetType = targetType.BaseType()) != null)
			{
				IEnumerable<MemberInfo> collection = (from f in targetType.GetFields(bindingAttr2)
					where f.IsPrivate
					select f).Cast<MemberInfo>();
				initialFields.AddRange(collection);
			}
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type targetType, BindingFlags bindingAttr)
		{
			ValidationUtils.ArgumentNotNull(targetType, "targetType");
			List<PropertyInfo> list = new List<PropertyInfo>(targetType.GetProperties(bindingAttr));
			GetChildPrivateProperties(list, targetType, bindingAttr);
			for (int i = 0; i < list.Count; i++)
			{
				PropertyInfo propertyInfo = list[i];
				if (propertyInfo.DeclaringType != targetType)
				{
					PropertyInfo value = (PropertyInfo)GetMemberInfoFromType(propertyInfo.DeclaringType, propertyInfo);
					list[i] = value;
				}
			}
			return list;
		}

		public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
		{
			if ((bindingAttr & flag) != flag)
			{
				return bindingAttr;
			}
			return bindingAttr ^ flag;
		}

		private static void GetChildPrivateProperties(IList<PropertyInfo> initialProperties, Type targetType, BindingFlags bindingAttr)
		{
			while ((targetType = targetType.BaseType()) != null)
			{
				PropertyInfo[] properties = targetType.GetProperties(bindingAttr);
				foreach (PropertyInfo propertyInfo in properties)
				{
					PropertyInfo subTypeProperty = propertyInfo;
					if (!IsPublic(subTypeProperty))
					{
						int num = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name);
						if (num == -1)
						{
							initialProperties.Add(subTypeProperty);
							continue;
						}
						PropertyInfo property = initialProperties[num];
						if (!IsPublic(property))
						{
							initialProperties[num] = subTypeProperty;
						}
					}
					else if (!subTypeProperty.IsVirtual())
					{
						int num2 = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name && p.DeclaringType == subTypeProperty.DeclaringType);
						if (num2 == -1)
						{
							initialProperties.Add(subTypeProperty);
						}
					}
					else
					{
						int num3 = initialProperties.IndexOf((PropertyInfo p) => p.Name == subTypeProperty.Name && p.IsVirtual() && p.GetBaseDefinition() != null && p.GetBaseDefinition().DeclaringType.IsAssignableFrom(subTypeProperty.DeclaringType));
						if (num3 == -1)
						{
							initialProperties.Add(subTypeProperty);
						}
					}
				}
			}
		}

		public static bool IsMethodOverridden(Type currentType, Type methodDeclaringType, string method)
		{
			return currentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any((MethodInfo info) => info.Name == method && info.DeclaringType != methodDeclaringType && info.GetBaseDefinition().DeclaringType == methodDeclaringType);
		}

		public static object GetDefaultValue(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			switch (ConvertUtils.GetTypeCode(type))
			{
			case PrimitiveTypeCode.Boolean:
				return false;
			case PrimitiveTypeCode.Char:
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
				return 0;
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
				return 0L;
			case PrimitiveTypeCode.Single:
				return 0f;
			case PrimitiveTypeCode.Double:
				return 0.0;
			case PrimitiveTypeCode.Decimal:
				return 0m;
			case PrimitiveTypeCode.DateTime:
				return default(DateTime);
			case PrimitiveTypeCode.BigInteger:
				return default(BigInteger);
			case PrimitiveTypeCode.Guid:
				return default(Guid);
			case PrimitiveTypeCode.DateTimeOffset:
				return default(DateTimeOffset);
			default:
				if (IsNullable(type))
				{
					return null;
				}
				return Activator.CreateInstance(type);
			}
		}
	}
	internal static class StringUtils
	{
		public const string CarriageReturnLineFeed = "\r\n";

		public const string Empty = "";

		public const char CarriageReturn = '\r';

		public const char LineFeed = '\n';

		public const char Tab = '\t';

		public static string FormatWith(this string format, IFormatProvider provider, object arg0)
		{
			return format.FormatWith(provider, new object[1] { arg0 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
		{
			return format.FormatWith(provider, new object[2] { arg0, arg1 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2)
		{
			return format.FormatWith(provider, new object[3] { arg0, arg1, arg2 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1, object arg2, object arg3)
		{
			return format.FormatWith(provider, new object[4] { arg0, arg1, arg2, arg3 });
		}

		private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static string NullEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder sb = new StringBuilder(capacity);
			return new StringWriter(sb, CultureInfo.InvariantCulture);
		}

		public static int? GetLength(string value)
		{
			return value?.Length;
		}

		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex(((int)c >> 12) & 0xF);
			buffer[3] = MathUtils.IntToHex(((int)c >> 8) & 0xF);
			buffer[4] = MathUtils.IntToHex(((int)c >> 4) & 0xF);
			buffer[5] = MathUtils.IntToHex(c & 0xF);
		}

		public static TSource ForgivingCaseSensitiveFind<TSource>(this IEnumerable<TSource> source, Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new ArgumentNullException("valueSelector");
			}
			IEnumerable<TSource> source2 = source.Where((TSource s) => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase));
			if (source2.Count() <= 1)
			{
				return source2.SingleOrDefault();
			}
			IEnumerable<TSource> source3 = source.Where((TSource s) => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal));
			return source3.SingleOrDefault();
		}

		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (!char.IsUpper(s[0]))
			{
				return s;
			}
			char[] array = s.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = i + 1 < array.Length;
				if (i > 0 && flag && !char.IsUpper(array[i + 1]))
				{
					break;
				}
				array[i] = char.ToLower(array[i], CultureInfo.InvariantCulture);
			}
			return new string(array);
		}

		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		public static bool StartsWith(this string source, char value)
		{
			if (source.Length > 0)
			{
				return source[0] == value;
			}
			return false;
		}

		public static bool EndsWith(this string source, char value)
		{
			if (source.Length > 0)
			{
				return source[source.Length - 1] == value;
			}
			return false;
		}
	}
	internal static class TypeExtensions
	{
		public static MethodInfo Method(this Delegate d)
		{
			return d.Method;
		}

		public static MemberTypes MemberType(this MemberInfo memberInfo)
		{
			return memberInfo.MemberType;
		}

		public static bool ContainsGenericParameters(this Type type)
		{
			return type.ContainsGenericParameters;
		}

		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		public static Assembly Assembly(this Type type)
		{
			return type.Assembly;
		}

		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		public static bool IsClass(this Type type)
		{
			return type.IsClass;
		}

		public static bool IsSealed(this Type type)
		{
			return type.IsSealed;
		}

		public static bool IsAbstract(this Type type)
		{
			return type.IsAbstract;
		}

		public static bool IsVisible(this Type type)
		{
			return type.IsVisible;
		}

		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		public static bool AssignableToTypeName(this Type type, string fullTypeName, out Type match)
		{
			Type type2 = type;
			while (type2 != null)
			{
				if (string.Equals(type2.FullName, fullTypeName, StringComparison.Ordinal))
				{
					match = type2;
					return true;
				}
				type2 = type2.BaseType();
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type3 in interfaces)
			{
				if (string.Equals(type3.Name, fullTypeName, StringComparison.Ordinal))
				{
					match = type;
					return true;
				}
			}
			match = null;
			return false;
		}

		public static bool AssignableToTypeName(this Type type, string fullTypeName)
		{
			Type match;
			return type.AssignableToTypeName(fullTypeName, out match);
		}
	}
	internal static class ValidationUtils
	{
		public static void ArgumentNotNullOrEmpty(string value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (value.Length == 0)
			{
				throw new ArgumentException("'{0}' cannot be empty.".FormatWith(CultureInfo.InvariantCulture, parameterName), parameterName);
			}
		}

		public static void ArgumentTypeIsEnum(Type enumType, string parameterName)
		{
			ArgumentNotNull(enumType, "enumType");
			if (!enumType.IsEnum())
			{
				throw new ArgumentException("Type {0} is not an Enum.".FormatWith(CultureInfo.InvariantCulture, enumType), parameterName);
			}
		}

		public static void ArgumentNotNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}
namespace Newtonsoft.Json
{
	public enum WriteState
	{
		Error,
		Closed,
		Object,
		Array,
		Constructor,
		Property,
		Start
	}
}

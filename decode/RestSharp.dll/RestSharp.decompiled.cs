using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using RestSharp.Authenticators.OAuth;
using RestSharp.Authenticators.OAuth.Extensions;
using RestSharp.Contrib;
using RestSharp.Deserializers;
using RestSharp.Extensions;
using RestSharp.Reflection;
using RestSharp.Serializers;

[assembly: CLSCompliant(true)]
[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyFileVersion("104.0.0.0")]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: Guid("97044cbf-6c9d-4b08-87e3-bf30fbde1933")]
[assembly: TargetFramework(".NETFramework,Version=v4.0", FrameworkDisplayName = ".NET Framework 4")]
[assembly: AssemblyTitle("RestSharp")]
[assembly: ComVisible(false)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCopyright("Copyright © RestSharp Project 2009-2012")]
[assembly: AssemblyDescription("Simple REST and HTTP API Client")]
[assembly: InternalsVisibleTo("RestSharp.IntegrationTests")]
[assembly: AssemblyCompany("restsharp.org")]
[assembly: AssemblyProduct("RestSharp")]
[assembly: AssemblyVersion("104.0.0.0")]
namespace RestSharp
{
	public interface IAuthenticator
	{
		void Authenticate(IRestClient client, IRestRequest request);
	}
	public class HttpBasicAuthenticator : IAuthenticator
	{
		private readonly string _username;

		private readonly string _password;

		public HttpBasicAuthenticator(string username, string password)
		{
			_password = password;
			_username = username;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			if (!request.Parameters.Any((Parameter p) => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
			{
				string arg = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
				string value = $"Basic {arg}";
				request.AddParameter("Authorization", value, ParameterType.HttpHeader);
			}
		}
	}
	public class NtlmAuthenticator : IAuthenticator
	{
		private readonly ICredentials credentials;

		public NtlmAuthenticator()
			: this(CredentialCache.DefaultCredentials)
		{
		}

		public NtlmAuthenticator(string username, string password)
			: this(new NetworkCredential(username, password))
		{
		}

		public NtlmAuthenticator(ICredentials credentials)
		{
			if (credentials == null)
			{
				throw new ArgumentNullException("credentials");
			}
			this.credentials = credentials;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.Credentials = credentials;
		}
	}
}
namespace RestSharp.Authenticators
{
	public class OAuth1Authenticator : IAuthenticator
	{
		public virtual string Realm { get; set; }

		public virtual OAuthParameterHandling ParameterHandling { get; set; }

		public virtual OAuthSignatureMethod SignatureMethod { get; set; }

		public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

		internal virtual OAuthType Type { get; set; }

		internal virtual string ConsumerKey { get; set; }

		internal virtual string ConsumerSecret { get; set; }

		internal virtual string Token { get; set; }

		internal virtual string TokenSecret { get; set; }

		internal virtual string Verifier { get; set; }

		internal virtual string Version { get; set; }

		internal virtual string CallbackUrl { get; set; }

		internal virtual string SessionHandle { get; set; }

		internal virtual string ClientUsername { get; set; }

		internal virtual string ClientPassword { get; set; }

		public static OAuth1Authenticator ForRequestToken(string consumerKey, string consumerSecret)
		{
			OAuth1Authenticator oAuth1Authenticator = new OAuth1Authenticator();
			oAuth1Authenticator.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
			oAuth1Authenticator.SignatureMethod = OAuthSignatureMethod.HmacSha1;
			oAuth1Authenticator.SignatureTreatment = OAuthSignatureTreatment.Escaped;
			oAuth1Authenticator.ConsumerKey = consumerKey;
			oAuth1Authenticator.ConsumerSecret = consumerSecret;
			oAuth1Authenticator.Type = OAuthType.RequestToken;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForRequestToken(string consumerKey, string consumerSecret, string callbackUrl)
		{
			OAuth1Authenticator oAuth1Authenticator = ForRequestToken(consumerKey, consumerSecret);
			oAuth1Authenticator.CallbackUrl = callbackUrl;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForAccessToken(string consumerKey, string consumerSecret, string token, string tokenSecret)
		{
			OAuth1Authenticator oAuth1Authenticator = new OAuth1Authenticator();
			oAuth1Authenticator.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
			oAuth1Authenticator.SignatureMethod = OAuthSignatureMethod.HmacSha1;
			oAuth1Authenticator.SignatureTreatment = OAuthSignatureTreatment.Escaped;
			oAuth1Authenticator.ConsumerKey = consumerKey;
			oAuth1Authenticator.ConsumerSecret = consumerSecret;
			oAuth1Authenticator.Token = token;
			oAuth1Authenticator.TokenSecret = tokenSecret;
			oAuth1Authenticator.Type = OAuthType.AccessToken;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForAccessToken(string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier)
		{
			OAuth1Authenticator oAuth1Authenticator = ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
			oAuth1Authenticator.Verifier = verifier;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForAccessTokenRefresh(string consumerKey, string consumerSecret, string token, string tokenSecret, string sessionHandle)
		{
			OAuth1Authenticator oAuth1Authenticator = ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
			oAuth1Authenticator.SessionHandle = sessionHandle;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForAccessTokenRefresh(string consumerKey, string consumerSecret, string token, string tokenSecret, string verifier, string sessionHandle)
		{
			OAuth1Authenticator oAuth1Authenticator = ForAccessToken(consumerKey, consumerSecret, token, tokenSecret);
			oAuth1Authenticator.SessionHandle = sessionHandle;
			oAuth1Authenticator.Verifier = verifier;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForClientAuthentication(string consumerKey, string consumerSecret, string username, string password)
		{
			OAuth1Authenticator oAuth1Authenticator = new OAuth1Authenticator();
			oAuth1Authenticator.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
			oAuth1Authenticator.SignatureMethod = OAuthSignatureMethod.HmacSha1;
			oAuth1Authenticator.SignatureTreatment = OAuthSignatureTreatment.Escaped;
			oAuth1Authenticator.ConsumerKey = consumerKey;
			oAuth1Authenticator.ConsumerSecret = consumerSecret;
			oAuth1Authenticator.ClientUsername = username;
			oAuth1Authenticator.ClientPassword = password;
			oAuth1Authenticator.Type = OAuthType.ClientAuthentication;
			return oAuth1Authenticator;
		}

		public static OAuth1Authenticator ForProtectedResource(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
		{
			OAuth1Authenticator oAuth1Authenticator = new OAuth1Authenticator();
			oAuth1Authenticator.Type = OAuthType.ProtectedResource;
			oAuth1Authenticator.ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader;
			oAuth1Authenticator.SignatureMethod = OAuthSignatureMethod.HmacSha1;
			oAuth1Authenticator.SignatureTreatment = OAuthSignatureTreatment.Escaped;
			oAuth1Authenticator.ConsumerKey = consumerKey;
			oAuth1Authenticator.ConsumerSecret = consumerSecret;
			oAuth1Authenticator.Token = accessToken;
			oAuth1Authenticator.TokenSecret = accessTokenSecret;
			return oAuth1Authenticator;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			OAuthWorkflow oAuthWorkflow = new OAuthWorkflow();
			oAuthWorkflow.ConsumerKey = ConsumerKey;
			oAuthWorkflow.ConsumerSecret = ConsumerSecret;
			oAuthWorkflow.ParameterHandling = ParameterHandling;
			oAuthWorkflow.SignatureMethod = SignatureMethod;
			oAuthWorkflow.SignatureTreatment = SignatureTreatment;
			oAuthWorkflow.Verifier = Verifier;
			oAuthWorkflow.Version = Version;
			oAuthWorkflow.CallbackUrl = CallbackUrl;
			oAuthWorkflow.SessionHandle = SessionHandle;
			oAuthWorkflow.Token = Token;
			oAuthWorkflow.TokenSecret = TokenSecret;
			oAuthWorkflow.ClientUsername = ClientUsername;
			oAuthWorkflow.ClientPassword = ClientPassword;
			OAuthWorkflow workflow = oAuthWorkflow;
			AddOAuthData(client, request, workflow);
		}

		private void AddOAuthData(IRestClient client, IRestRequest request, OAuthWorkflow workflow)
		{
			string text = client.BuildUri(request).ToString();
			int num = text.IndexOf('?');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			string method = request.Method.ToString().ToUpperInvariant();
			WebParameterCollection webParameterCollection = new WebParameterCollection();
			foreach (Parameter item in client.DefaultParameters.Where((Parameter p) => p.Type == ParameterType.GetOrPost))
			{
				webParameterCollection.Add(new WebPair(item.Name, item.Value.ToString()));
			}
			foreach (Parameter item2 in request.Parameters.Where((Parameter p) => p.Type == ParameterType.GetOrPost))
			{
				webParameterCollection.Add(new WebPair(item2.Name, item2.Value.ToString()));
			}
			OAuthWebQueryInfo oAuthWebQueryInfo;
			switch (Type)
			{
			case OAuthType.RequestToken:
				workflow.RequestTokenUrl = text;
				oAuthWebQueryInfo = workflow.BuildRequestTokenInfo(method, webParameterCollection);
				break;
			case OAuthType.AccessToken:
				workflow.AccessTokenUrl = text;
				oAuthWebQueryInfo = workflow.BuildAccessTokenInfo(method, webParameterCollection);
				break;
			case OAuthType.ClientAuthentication:
				workflow.AccessTokenUrl = text;
				oAuthWebQueryInfo = workflow.BuildClientAuthAccessTokenInfo(method, webParameterCollection);
				break;
			case OAuthType.ProtectedResource:
				oAuthWebQueryInfo = workflow.BuildProtectedResourceInfo(method, webParameterCollection, text);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			switch (ParameterHandling)
			{
			case OAuthParameterHandling.HttpAuthorizationHeader:
				webParameterCollection.Add("oauth_signature", oAuthWebQueryInfo.Signature);
				request.AddHeader("Authorization", GetAuthorizationHeader(webParameterCollection));
				break;
			case OAuthParameterHandling.UrlOrPostParameters:
				webParameterCollection.Add("oauth_signature", oAuthWebQueryInfo.Signature);
				{
					foreach (WebPair item3 in webParameterCollection.Where((WebPair parameter) => !parameter.Name.IsNullOrBlank() && parameter.Name.StartsWith("oauth_")))
					{
						request.AddParameter(item3.Name, HttpUtility.UrlDecode(item3.Value));
					}
					break;
				}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private string GetAuthorizationHeader(WebPairCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder("OAuth ");
			if (!Realm.IsNullOrBlank())
			{
				stringBuilder.Append("realm=\"{0}\",".FormatWith(OAuthTools.UrlEncodeRelaxed(Realm)));
			}
			parameters.Sort((WebPair l, WebPair r) => l.Name.CompareTo(r.Name));
			int num = 0;
			List<WebPair> list = parameters.Where((WebPair parameter) => !parameter.Name.IsNullOrBlank() && !parameter.Value.IsNullOrBlank() && (parameter.Name.StartsWith("oauth_") || parameter.Name.StartsWith("x_auth_"))).ToList();
			foreach (WebPair item in list)
			{
				num++;
				string format = ((num < list.Count) ? "{0}=\"{1}\"," : "{0}=\"{1}\"");
				stringBuilder.Append(format.FormatWith(item.Name, item.Value));
			}
			return stringBuilder.ToString();
		}
	}
}
namespace RestSharp
{
	public abstract class OAuth2Authenticator : IAuthenticator
	{
		private readonly string _accessToken;

		public string AccessToken => _accessToken;

		public OAuth2Authenticator(string accessToken)
		{
			_accessToken = accessToken;
		}

		public abstract void Authenticate(IRestClient client, IRestRequest request);
	}
	public class OAuth2UriQueryParameterAuthenticator : OAuth2Authenticator
	{
		public OAuth2UriQueryParameterAuthenticator(string accessToken)
			: base(accessToken)
		{
		}

		public override void Authenticate(IRestClient client, IRestRequest request)
		{
			request.AddParameter("oauth_token", base.AccessToken, ParameterType.GetOrPost);
		}
	}
	public class OAuth2AuthorizationRequestHeaderAuthenticator : OAuth2Authenticator
	{
		private readonly string _authorizationValue;

		public OAuth2AuthorizationRequestHeaderAuthenticator(string accessToken)
			: base(accessToken)
		{
			_authorizationValue = "OAuth " + accessToken;
		}

		public override void Authenticate(IRestClient client, IRestRequest request)
		{
			if (!request.Parameters.Any((Parameter p) => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase)))
			{
				request.AddParameter("Authorization", _authorizationValue, ParameterType.HttpHeader);
			}
		}
	}
}
namespace RestSharp.Authenticators.OAuth.Extensions
{
	internal static class CollectionExtensions
	{
		public static IEnumerable<T> AsEnumerable<T>(this T item)
		{
			return new T[1] { item };
		}

		public static IEnumerable<T> And<T>(this T item, T other)
		{
			return new T[2] { item, other };
		}

		public static IEnumerable<T> And<T>(this IEnumerable<T> items, T item)
		{
			foreach (T item2 in items)
			{
				yield return item2;
			}
			yield return item;
		}

		public static K TryWithKey<T, K>(this IDictionary<T, K> dictionary, T key)
		{
			if (!dictionary.ContainsKey(key))
			{
				return default(K);
			}
			return dictionary[key];
		}

		public static IEnumerable<T> ToEnumerable<T>(this object[] items) where T : class
		{
			try
			{
				foreach (object item in items)
				{
					yield return item as T;
				}
			}
			finally
			{
			}
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items)
			{
				action(item);
			}
		}

		public static void AddRange(this IDictionary<string, string> collection, NameValueCollection range)
		{
			string[] allKeys = range.AllKeys;
			foreach (string text in allKeys)
			{
				collection.Add(text, range[text]);
			}
		}

		public static string ToQueryString(this NameValueCollection collection)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (collection.Count > 0)
			{
				stringBuilder.Append("?");
			}
			int num = 0;
			string[] allKeys = collection.AllKeys;
			foreach (string text in allKeys)
			{
				stringBuilder.AppendFormat("{0}={1}", text, collection[text].UrlEncode());
				num++;
				if (num < collection.Count)
				{
					stringBuilder.Append("&");
				}
			}
			return stringBuilder.ToString();
		}

		public static string Concatenate(this WebParameterCollection collection, string separator, string spacer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = collection.Count;
			int num = 0;
			foreach (WebPair item in collection)
			{
				stringBuilder.Append(item.Name);
				stringBuilder.Append(separator);
				stringBuilder.Append(item.Value);
				num++;
				if (num < count)
				{
					stringBuilder.Append(spacer);
				}
			}
			return stringBuilder.ToString();
		}
	}
	internal static class OAuthExtensions
	{
		public static string ToRequestValue(this OAuthSignatureMethod signatureMethod)
		{
			string text = signatureMethod.ToString().ToUpper();
			int num = text.IndexOf("SHA1");
			if (num <= -1)
			{
				return text;
			}
			return text.Insert(num, "-");
		}

		public static OAuthSignatureMethod FromRequestValue(this string signatureMethod)
		{
			return signatureMethod switch
			{
				"HMAC-SHA1" => OAuthSignatureMethod.HmacSha1, 
				"RSA-SHA1" => OAuthSignatureMethod.RsaSha1, 
				_ => OAuthSignatureMethod.PlainText, 
			};
		}

		public static string HashWith(this string input, HashAlgorithm algorithm)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			byte[] inArray = algorithm.ComputeHash(bytes);
			return Convert.ToBase64String(inArray);
		}
	}
	internal static class StringExtensions
	{
		private const RegexOptions Options = RegexOptions.IgnoreCase | RegexOptions.Compiled;

		public static bool IsNullOrBlank(this string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(value))
				{
					return value.Trim() == string.Empty;
				}
				return false;
			}
			return true;
		}

		public static bool EqualsIgnoreCase(this string left, string right)
		{
			return string.Compare(left, right, StringComparison.OrdinalIgnoreCase) == 0;
		}

		public static bool EqualsAny(this string input, params string[] args)
		{
			return args.Aggregate(seed: false, (bool current, string arg) => current | input.Equals(arg));
		}

		public static string FormatWith(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

		public static string FormatWithInvariantCulture(this string format, params object[] args)
		{
			return string.Format(CultureInfo.InvariantCulture, format, args);
		}

		public static string Then(this string input, string value)
		{
			return input + value;
		}

		public static string UrlEncode(this string value)
		{
			return Uri.EscapeDataString(value);
		}

		public static string UrlDecode(this string value)
		{
			return Uri.UnescapeDataString(value);
		}

		public static Uri AsUri(this string value)
		{
			return new Uri(value);
		}

		public static string ToBase64String(this byte[] input)
		{
			return Convert.ToBase64String(input);
		}

		public static byte[] GetBytes(this string input)
		{
			return Encoding.UTF8.GetBytes(input);
		}

		public static string PercentEncode(this string s)
		{
			byte[] bytes = s.GetBytes();
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = bytes;
			foreach (byte b in array)
			{
				if ((b > 7 && b < 11) || b == 13)
				{
					stringBuilder.Append($"%0{b:X}");
				}
				else
				{
					stringBuilder.Append($"%{b:X}");
				}
			}
			return stringBuilder.ToString();
		}

		public static IDictionary<string, string> ParseQueryString(this string query)
		{
			if (query.StartsWith("?"))
			{
				query = query.Substring(1);
			}
			if (query.Equals(string.Empty))
			{
				return new Dictionary<string, string>();
			}
			string[] source = query.Split(new char[1] { '&' });
			return source.Select((string part) => part.Split(new char[1] { '=' })).ToDictionary((string[] pair) => pair[0], (string[] pair) => pair[1]);
		}
	}
	internal static class TimeExtensions
	{
		public static DateTime FromNow(this TimeSpan value)
		{
			return new DateTime((DateTime.Now + value).Ticks);
		}

		public static DateTime FromUnixTime(this long seconds)
		{
			return new DateTime(1970, 1, 1).AddSeconds(seconds).ToLocalTime();
		}

		public static long ToUnixTime(this DateTime dateTime)
		{
			return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
		}
	}
}
namespace RestSharp.Authenticators.OAuth
{
	internal class WebPair
	{
		public string Value { get; set; }

		public string Name { get; set; }

		public WebPair(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
	[Serializable]
	[DebuggerDisplay("{Name}:{Value}")]
	internal class WebParameter : WebPair
	{
		public WebParameter(string name, string value)
			: base(name, value)
		{
		}
	}
	internal class HttpPostParameter : WebParameter
	{
		public virtual HttpPostParameterType Type { get; private set; }

		public virtual string FileName { get; private set; }

		public virtual string FilePath { get; private set; }

		public virtual Stream FileStream { get; set; }

		public virtual string ContentType { get; private set; }

		public HttpPostParameter(string name, string value)
			: base(name, value)
		{
		}

		public static HttpPostParameter CreateFile(string name, string fileName, string filePath, string contentType)
		{
			HttpPostParameter httpPostParameter = new HttpPostParameter(name, string.Empty);
			httpPostParameter.Type = HttpPostParameterType.File;
			httpPostParameter.FileName = fileName;
			httpPostParameter.FilePath = filePath;
			httpPostParameter.ContentType = contentType;
			return httpPostParameter;
		}

		public static HttpPostParameter CreateFile(string name, string fileName, Stream fileStream, string contentType)
		{
			HttpPostParameter httpPostParameter = new HttpPostParameter(name, string.Empty);
			httpPostParameter.Type = HttpPostParameterType.File;
			httpPostParameter.FileName = fileName;
			httpPostParameter.FileStream = fileStream;
			httpPostParameter.ContentType = contentType;
			return httpPostParameter;
		}
	}
	[Serializable]
	internal enum HttpPostParameterType
	{
		Field,
		File
	}
	[Serializable]
	public enum OAuthParameterHandling
	{
		HttpAuthorizationHeader,
		UrlOrPostParameters
	}
	[Serializable]
	public enum OAuthSignatureMethod
	{
		HmacSha1,
		PlainText,
		RsaSha1
	}
	[Serializable]
	public enum OAuthSignatureTreatment
	{
		Escaped,
		Unescaped
	}
	[Serializable]
	internal static class OAuthTools
	{
		private const string AlphaNumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

		private const string Digit = "1234567890";

		private const string Lower = "abcdefghijklmnopqrstuvwxyz";

		private const string Unreserved = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~";

		private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static readonly Random _random;

		private static readonly object _randomLock;

		private static readonly RandomNumberGenerator _rng;

		private static readonly Encoding _encoding;

		private static readonly string[] UriRfc3986CharsToEscape;

		private static readonly string[] UriRfc3968EscapedHex;

		static OAuthTools()
		{
			_randomLock = new object();
			_rng = RandomNumberGenerator.Create();
			_encoding = Encoding.UTF8;
			UriRfc3986CharsToEscape = new string[5] { "!", "*", "'", "(", ")" };
			UriRfc3968EscapedHex = new string[5] { "%21", "%2A", "%27", "%28", "%29" };
			byte[] array = new byte[4];
			_rng.GetNonZeroBytes(array);
			_random = new Random(BitConverter.ToInt32(array, 0));
		}

		public static string GetNonce()
		{
			char[] array = new char[16];
			lock (_randomLock)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = "abcdefghijklmnopqrstuvwxyz1234567890"[_random.Next(0, "abcdefghijklmnopqrstuvwxyz1234567890".Length)];
				}
			}
			return new string(array);
		}

		public static string GetTimestamp()
		{
			return GetTimestamp(DateTime.UtcNow);
		}

		public static string GetTimestamp(DateTime dateTime)
		{
			return dateTime.ToUnixTime().ToString();
		}

		public static string UrlEncodeRelaxed(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(Uri.EscapeDataString(value));
			for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
			{
				string oldValue = UriRfc3986CharsToEscape[i];
				stringBuilder.Replace(oldValue, UriRfc3968EscapedHex[i]);
			}
			return stringBuilder.ToString();
		}

		public static string UrlEncodeStrict(string value)
		{
			string text = value.Where((char c) => !Enumerable.Contains("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-._~", c) && c != '%').Aggregate(value, (string current, char c) => current.Replace(c.ToString(), c.ToString().PercentEncode()));
			return text.Replace("%%", "%25%");
		}

		public static string NormalizeRequestParameters(WebParameterCollection parameters)
		{
			WebParameterCollection collection = SortParametersExcludingSignature(parameters);
			return collection.Concatenate("=", "&");
		}

		public static WebParameterCollection SortParametersExcludingSignature(WebParameterCollection parameters)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection(parameters);
			IEnumerable<WebPair> parameters2 = webParameterCollection.Where((WebPair n) => n.Name.EqualsIgnoreCase("oauth_signature"));
			webParameterCollection.RemoveAll(parameters2);
			webParameterCollection.ForEach(delegate(WebPair p)
			{
				p.Name = UrlEncodeStrict(p.Name);
				p.Value = UrlEncodeStrict(p.Value);
			});
			webParameterCollection.Sort((WebPair x, WebPair y) => (string.CompareOrdinal(x.Name, y.Name) == 0) ? string.CompareOrdinal(x.Value, y.Value) : string.CompareOrdinal(x.Name, y.Name));
			return webParameterCollection;
		}

		public static string ConstructRequestUrl(Uri url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			StringBuilder stringBuilder = new StringBuilder();
			string value = "{0}://{1}".FormatWith(url.Scheme, url.Host);
			string text = ":{0}".FormatWith(url.Port);
			bool flag = url.Scheme == "http" && url.Port == 80;
			bool flag2 = url.Scheme == "https" && url.Port == 443;
			stringBuilder.Append(value);
			stringBuilder.Append((!flag && !flag2) ? text : "");
			stringBuilder.Append(url.AbsolutePath);
			return stringBuilder.ToString();
		}

		public static string ConcatenateRequestElements(string method, string url, WebParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = method.ToUpper().Then("&");
			string value2 = UrlEncodeRelaxed(ConstructRequestUrl(url.AsUri())).Then("&");
			string value3 = UrlEncodeRelaxed(NormalizeRequestParameters(parameters));
			stringBuilder.Append(value);
			stringBuilder.Append(value2);
			stringBuilder.Append(value3);
			return stringBuilder.ToString();
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, string signatureBase, string consumerSecret)
		{
			return GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, signatureBase, consumerSecret, null);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, OAuthSignatureTreatment signatureTreatment, string signatureBase, string consumerSecret)
		{
			return GetSignature(signatureMethod, signatureTreatment, signatureBase, consumerSecret, null);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, string signatureBase, string consumerSecret, string tokenSecret)
		{
			return GetSignature(signatureMethod, OAuthSignatureTreatment.Escaped, consumerSecret, tokenSecret);
		}

		public static string GetSignature(OAuthSignatureMethod signatureMethod, OAuthSignatureTreatment signatureTreatment, string signatureBase, string consumerSecret, string tokenSecret)
		{
			if (tokenSecret.IsNullOrBlank())
			{
				tokenSecret = string.Empty;
			}
			consumerSecret = UrlEncodeRelaxed(consumerSecret);
			tokenSecret = UrlEncodeRelaxed(tokenSecret);
			string text;
			switch (signatureMethod)
			{
			case OAuthSignatureMethod.HmacSha1:
			{
				HMACSHA1 hMACSHA = new HMACSHA1();
				string s = "{0}&{1}".FormatWith(consumerSecret, tokenSecret);
				hMACSHA.Key = _encoding.GetBytes(s);
				text = signatureBase.HashWith(hMACSHA);
				break;
			}
			case OAuthSignatureMethod.PlainText:
				text = "{0}&{1}".FormatWith(consumerSecret, tokenSecret);
				break;
			default:
				throw new NotImplementedException("Only HMAC-SHA1 is currently supported.");
			}
			return (signatureTreatment == OAuthSignatureTreatment.Escaped) ? UrlEncodeRelaxed(text) : text;
		}
	}
	[Serializable]
	public enum OAuthType
	{
		RequestToken,
		AccessToken,
		ProtectedResource,
		ClientAuthentication
	}
	[Serializable]
	public class OAuthWebQueryInfo
	{
		public virtual string ConsumerKey { get; set; }

		public virtual string Token { get; set; }

		public virtual string Nonce { get; set; }

		public virtual string Timestamp { get; set; }

		public virtual string SignatureMethod { get; set; }

		public virtual string Signature { get; set; }

		public virtual string Version { get; set; }

		public virtual string Callback { get; set; }

		public virtual string Verifier { get; set; }

		public virtual string ClientMode { get; set; }

		public virtual string ClientUsername { get; set; }

		public virtual string ClientPassword { get; set; }

		public virtual string UserAgent { get; set; }

		public virtual string WebMethod { get; set; }

		public virtual OAuthParameterHandling ParameterHandling { get; set; }

		public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

		internal virtual string ConsumerSecret { get; set; }

		internal virtual string TokenSecret { get; set; }
	}
	internal class OAuthWorkflow
	{
		public virtual string Version { get; set; }

		public virtual string ConsumerKey { get; set; }

		public virtual string ConsumerSecret { get; set; }

		public virtual string Token { get; set; }

		public virtual string TokenSecret { get; set; }

		public virtual string CallbackUrl { get; set; }

		public virtual string Verifier { get; set; }

		public virtual string SessionHandle { get; set; }

		public virtual OAuthSignatureMethod SignatureMethod { get; set; }

		public virtual OAuthSignatureTreatment SignatureTreatment { get; set; }

		public virtual OAuthParameterHandling ParameterHandling { get; set; }

		public virtual string ClientUsername { get; set; }

		public virtual string ClientPassword { get; set; }

		public virtual string RequestTokenUrl { get; set; }

		public virtual string AccessTokenUrl { get; set; }

		public virtual string AuthorizationUrl { get; set; }

		public OAuthWebQueryInfo BuildRequestTokenInfo(string method)
		{
			return BuildRequestTokenInfo(method, null);
		}

		public virtual OAuthWebQueryInfo BuildRequestTokenInfo(string method, WebParameterCollection parameters)
		{
			ValidateTokenRequestState();
			if (parameters == null)
			{
				parameters = new WebParameterCollection();
			}
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			AddAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(method, RequestTokenUrl, parameters);
			string signature = OAuthTools.GetSignature(SignatureMethod, SignatureTreatment, signatureBase, ConsumerSecret);
			OAuthWebQueryInfo oAuthWebQueryInfo = new OAuthWebQueryInfo();
			oAuthWebQueryInfo.WebMethod = method;
			oAuthWebQueryInfo.ParameterHandling = ParameterHandling;
			oAuthWebQueryInfo.ConsumerKey = ConsumerKey;
			oAuthWebQueryInfo.SignatureMethod = SignatureMethod.ToRequestValue();
			oAuthWebQueryInfo.SignatureTreatment = SignatureTreatment;
			oAuthWebQueryInfo.Signature = signature;
			oAuthWebQueryInfo.Timestamp = timestamp;
			oAuthWebQueryInfo.Nonce = nonce;
			oAuthWebQueryInfo.Version = Version ?? "1.0";
			oAuthWebQueryInfo.Callback = OAuthTools.UrlEncodeRelaxed(CallbackUrl ?? "");
			oAuthWebQueryInfo.TokenSecret = TokenSecret;
			oAuthWebQueryInfo.ConsumerSecret = ConsumerSecret;
			return oAuthWebQueryInfo;
		}

		public virtual OAuthWebQueryInfo BuildAccessTokenInfo(string method)
		{
			return BuildAccessTokenInfo(method, null);
		}

		public virtual OAuthWebQueryInfo BuildAccessTokenInfo(string method, WebParameterCollection parameters)
		{
			ValidateAccessRequestState();
			if (parameters == null)
			{
				parameters = new WebParameterCollection();
			}
			Uri uri = new Uri(AccessTokenUrl);
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			AddAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(method, uri.ToString(), parameters);
			string signature = OAuthTools.GetSignature(SignatureMethod, SignatureTreatment, signatureBase, ConsumerSecret, TokenSecret);
			OAuthWebQueryInfo oAuthWebQueryInfo = new OAuthWebQueryInfo();
			oAuthWebQueryInfo.WebMethod = method;
			oAuthWebQueryInfo.ParameterHandling = ParameterHandling;
			oAuthWebQueryInfo.ConsumerKey = ConsumerKey;
			oAuthWebQueryInfo.Token = Token;
			oAuthWebQueryInfo.SignatureMethod = SignatureMethod.ToRequestValue();
			oAuthWebQueryInfo.SignatureTreatment = SignatureTreatment;
			oAuthWebQueryInfo.Signature = signature;
			oAuthWebQueryInfo.Timestamp = timestamp;
			oAuthWebQueryInfo.Nonce = nonce;
			oAuthWebQueryInfo.Version = Version ?? "1.0";
			oAuthWebQueryInfo.Verifier = Verifier;
			oAuthWebQueryInfo.Callback = CallbackUrl;
			oAuthWebQueryInfo.TokenSecret = TokenSecret;
			oAuthWebQueryInfo.ConsumerSecret = ConsumerSecret;
			return oAuthWebQueryInfo;
		}

		public virtual OAuthWebQueryInfo BuildClientAuthAccessTokenInfo(string method, WebParameterCollection parameters)
		{
			ValidateClientAuthAccessRequestState();
			if (parameters == null)
			{
				parameters = new WebParameterCollection();
			}
			Uri uri = new Uri(AccessTokenUrl);
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			AddXAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(method, uri.ToString(), parameters);
			string signature = OAuthTools.GetSignature(SignatureMethod, SignatureTreatment, signatureBase, ConsumerSecret);
			OAuthWebQueryInfo oAuthWebQueryInfo = new OAuthWebQueryInfo();
			oAuthWebQueryInfo.WebMethod = method;
			oAuthWebQueryInfo.ParameterHandling = ParameterHandling;
			oAuthWebQueryInfo.ClientMode = "client_auth";
			oAuthWebQueryInfo.ClientUsername = ClientUsername;
			oAuthWebQueryInfo.ClientPassword = ClientPassword;
			oAuthWebQueryInfo.ConsumerKey = ConsumerKey;
			oAuthWebQueryInfo.SignatureMethod = SignatureMethod.ToRequestValue();
			oAuthWebQueryInfo.SignatureTreatment = SignatureTreatment;
			oAuthWebQueryInfo.Signature = signature;
			oAuthWebQueryInfo.Timestamp = timestamp;
			oAuthWebQueryInfo.Nonce = nonce;
			oAuthWebQueryInfo.Version = Version ?? "1.0";
			oAuthWebQueryInfo.TokenSecret = TokenSecret;
			oAuthWebQueryInfo.ConsumerSecret = ConsumerSecret;
			return oAuthWebQueryInfo;
		}

		public virtual OAuthWebQueryInfo BuildProtectedResourceInfo(string method, WebParameterCollection parameters, string url)
		{
			ValidateProtectedResourceState();
			if (parameters == null)
			{
				parameters = new WebParameterCollection();
			}
			Uri uri = new Uri(url);
			NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(uri.Query);
			string[] allKeys = nameValueCollection.AllKeys;
			foreach (string name in allKeys)
			{
				string text;
				if ((text = method.ToUpperInvariant()) != null && text == "POST")
				{
					parameters.Add(new HttpPostParameter(name, nameValueCollection[name]));
				}
				else
				{
					parameters.Add(name, nameValueCollection[name]);
				}
			}
			string timestamp = OAuthTools.GetTimestamp();
			string nonce = OAuthTools.GetNonce();
			AddAuthParameters(parameters, timestamp, nonce);
			string signatureBase = OAuthTools.ConcatenateRequestElements(method, url, parameters);
			string signature = OAuthTools.GetSignature(SignatureMethod, SignatureTreatment, signatureBase, ConsumerSecret, TokenSecret);
			OAuthWebQueryInfo oAuthWebQueryInfo = new OAuthWebQueryInfo();
			oAuthWebQueryInfo.WebMethod = method;
			oAuthWebQueryInfo.ParameterHandling = ParameterHandling;
			oAuthWebQueryInfo.ConsumerKey = ConsumerKey;
			oAuthWebQueryInfo.Token = Token;
			oAuthWebQueryInfo.SignatureMethod = SignatureMethod.ToRequestValue();
			oAuthWebQueryInfo.SignatureTreatment = SignatureTreatment;
			oAuthWebQueryInfo.Signature = signature;
			oAuthWebQueryInfo.Timestamp = timestamp;
			oAuthWebQueryInfo.Nonce = nonce;
			oAuthWebQueryInfo.Version = Version ?? "1.0";
			oAuthWebQueryInfo.Callback = CallbackUrl;
			oAuthWebQueryInfo.ConsumerSecret = ConsumerSecret;
			oAuthWebQueryInfo.TokenSecret = TokenSecret;
			return oAuthWebQueryInfo;
		}

		private void ValidateTokenRequestState()
		{
			if (RequestTokenUrl.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a request token URL");
			}
			if (ConsumerKey.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (ConsumerSecret.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
		}

		private void ValidateAccessRequestState()
		{
			if (AccessTokenUrl.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify an access token URL");
			}
			if (ConsumerKey.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (ConsumerSecret.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
			if (Token.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a token");
			}
		}

		private void ValidateClientAuthAccessRequestState()
		{
			if (AccessTokenUrl.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify an access token URL");
			}
			if (ConsumerKey.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (ConsumerSecret.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
			if (ClientUsername.IsNullOrBlank() || ClientPassword.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify user credentials");
			}
		}

		private void ValidateProtectedResourceState()
		{
			if (ConsumerKey.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer key");
			}
			if (ConsumerSecret.IsNullOrBlank())
			{
				throw new ArgumentException("You must specify a consumer secret");
			}
		}

		private void AddAuthParameters(ICollection<WebPair> parameters, string timestamp, string nonce)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection();
			webParameterCollection.Add(new WebPair("oauth_consumer_key", ConsumerKey));
			webParameterCollection.Add(new WebPair("oauth_nonce", nonce));
			webParameterCollection.Add(new WebPair("oauth_signature_method", SignatureMethod.ToRequestValue()));
			webParameterCollection.Add(new WebPair("oauth_timestamp", timestamp));
			webParameterCollection.Add(new WebPair("oauth_version", Version ?? "1.0"));
			WebParameterCollection webParameterCollection2 = webParameterCollection;
			if (!Token.IsNullOrBlank())
			{
				webParameterCollection2.Add(new WebPair("oauth_token", Token));
			}
			if (!CallbackUrl.IsNullOrBlank())
			{
				webParameterCollection2.Add(new WebPair("oauth_callback", CallbackUrl));
			}
			if (!Verifier.IsNullOrBlank())
			{
				webParameterCollection2.Add(new WebPair("oauth_verifier", Verifier));
			}
			if (!SessionHandle.IsNullOrBlank())
			{
				webParameterCollection2.Add(new WebPair("oauth_session_handle", SessionHandle));
			}
			foreach (WebPair item in webParameterCollection2)
			{
				parameters.Add(item);
			}
		}

		private void AddXAuthParameters(ICollection<WebPair> parameters, string timestamp, string nonce)
		{
			WebParameterCollection webParameterCollection = new WebParameterCollection();
			webParameterCollection.Add(new WebPair("x_auth_username", ClientUsername));
			webParameterCollection.Add(new WebPair("x_auth_password", ClientPassword));
			webParameterCollection.Add(new WebPair("x_auth_mode", "client_auth"));
			webParameterCollection.Add(new WebPair("oauth_consumer_key", ConsumerKey));
			webParameterCollection.Add(new WebPair("oauth_signature_method", SignatureMethod.ToRequestValue()));
			webParameterCollection.Add(new WebPair("oauth_timestamp", timestamp));
			webParameterCollection.Add(new WebPair("oauth_nonce", nonce));
			webParameterCollection.Add(new WebPair("oauth_version", Version ?? "1.0"));
			WebParameterCollection webParameterCollection2 = webParameterCollection;
			foreach (WebPair item in webParameterCollection2)
			{
				parameters.Add(item);
			}
		}
	}
	internal class WebPairCollection : IList<WebPair>, ICollection<WebPair>, IEnumerable<WebPair>, IEnumerable
	{
		private IList<WebPair> _parameters;

		public virtual WebPair this[string name] => this.SingleOrDefault((WebPair p) => p.Name.Equals(name));

		public virtual IEnumerable<string> Names => _parameters.Select((WebPair p) => p.Name);

		public virtual IEnumerable<string> Values => _parameters.Select((WebPair p) => p.Value);

		public virtual int Count => _parameters.Count;

		public virtual bool IsReadOnly => _parameters.IsReadOnly;

		public virtual WebPair this[int index]
		{
			get
			{
				return _parameters[index];
			}
			set
			{
				_parameters[index] = value;
			}
		}

		public WebPairCollection(IEnumerable<WebPair> parameters)
		{
			_parameters = new List<WebPair>(parameters);
		}

		public WebPairCollection(NameValueCollection collection)
			: this()
		{
			AddCollection(collection);
		}

		public virtual void AddRange(NameValueCollection collection)
		{
			AddCollection(collection);
		}

		private void AddCollection(NameValueCollection collection)
		{
			IEnumerable<WebPair> enumerable = collection.AllKeys.Select((string key) => new WebPair(key, collection[key]));
			foreach (WebPair item in enumerable)
			{
				_parameters.Add(item);
			}
		}

		public WebPairCollection(IDictionary<string, string> collection)
			: this()
		{
			AddCollection(collection);
		}

		public void AddCollection(IDictionary<string, string> collection)
		{
			foreach (string key in collection.Keys)
			{
				WebPair item = new WebPair(key, collection[key]);
				_parameters.Add(item);
			}
		}

		public WebPairCollection()
		{
			_parameters = new List<WebPair>(0);
		}

		public WebPairCollection(int capacity)
		{
			_parameters = new List<WebPair>(capacity);
		}

		private void AddCollection(IEnumerable<WebPair> collection)
		{
			foreach (WebPair item2 in collection)
			{
				WebPair item = new WebPair(item2.Name, item2.Value);
				_parameters.Add(item);
			}
		}

		public virtual void AddRange(WebPairCollection collection)
		{
			AddCollection(collection);
		}

		public virtual void AddRange(IEnumerable<WebPair> collection)
		{
			AddCollection(collection);
		}

		public virtual void Sort(Comparison<WebPair> comparison)
		{
			List<WebPair> list = new List<WebPair>(_parameters);
			list.Sort(comparison);
			_parameters = list;
		}

		public virtual bool RemoveAll(IEnumerable<WebPair> parameters)
		{
			bool flag = true;
			WebPair[] array = parameters.ToArray();
			foreach (WebPair item in array)
			{
				flag &= _parameters.Remove(item);
			}
			if (flag)
			{
				return array.Length > 0;
			}
			return false;
		}

		public virtual void Add(string name, string value)
		{
			WebPair item = new WebPair(name, value);
			_parameters.Add(item);
		}

		public virtual IEnumerator<WebPair> GetEnumerator()
		{
			return _parameters.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public virtual void Add(WebPair parameter)
		{
			_parameters.Add(parameter);
		}

		public virtual void Clear()
		{
			_parameters.Clear();
		}

		public virtual bool Contains(WebPair parameter)
		{
			return _parameters.Contains(parameter);
		}

		public virtual void CopyTo(WebPair[] parameters, int arrayIndex)
		{
			_parameters.CopyTo(parameters, arrayIndex);
		}

		public virtual bool Remove(WebPair parameter)
		{
			return _parameters.Remove(parameter);
		}

		public virtual int IndexOf(WebPair parameter)
		{
			return _parameters.IndexOf(parameter);
		}

		public virtual void Insert(int index, WebPair parameter)
		{
			_parameters.Insert(index, parameter);
		}

		public virtual void RemoveAt(int index)
		{
			_parameters.RemoveAt(index);
		}
	}
	internal class WebParameterCollection : WebPairCollection
	{
		public WebParameterCollection(IEnumerable<WebPair> parameters)
			: base(parameters)
		{
		}

		public WebParameterCollection(NameValueCollection collection)
			: base(collection)
		{
		}

		public WebParameterCollection()
		{
		}

		public WebParameterCollection(int capacity)
			: base(capacity)
		{
		}

		public WebParameterCollection(IDictionary<string, string> collection)
			: base(collection)
		{
		}

		public override void Add(string name, string value)
		{
			WebParameter parameter = new WebParameter(name, value);
			base.Add(parameter);
		}
	}
}
namespace RestSharp
{
	public class SimpleAuthenticator : IAuthenticator
	{
		private readonly string _usernameKey;

		private readonly string _username;

		private readonly string _passwordKey;

		private readonly string _password;

		public SimpleAuthenticator(string usernameKey, string username, string passwordKey, string password)
		{
			_usernameKey = usernameKey;
			_username = username;
			_passwordKey = passwordKey;
			_password = password;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
		{
			request.AddParameter(_usernameKey, _username);
			request.AddParameter(_passwordKey, _password);
		}
	}
}
namespace RestSharp.Deserializers
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class DeserializeAsAttribute : Attribute
	{
		public string Name { get; set; }

		public bool Attribute { get; set; }
	}
	public interface IDeserializer
	{
		string RootElement { get; set; }

		string Namespace { get; set; }

		string DateFormat { get; set; }

		T Deserialize<T>(IRestResponse response);
	}
	public class DotNetXmlDeserializer : IDeserializer
	{
		public string DateFormat { get; set; }

		public string Namespace { get; set; }

		public string RootElement { get; set; }

		public T Deserialize<T>(IRestResponse response)
		{
			if (string.IsNullOrEmpty(response.Content))
			{
				return default(T);
			}
			using MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(response.Content));
			System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
			return (T)xmlSerializer.Deserialize(stream);
		}
	}
	public class JsonDeserializer : IDeserializer
	{
		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string DateFormat { get; set; }

		public CultureInfo Culture { get; set; }

		public JsonDeserializer()
		{
			Culture = CultureInfo.InvariantCulture;
		}

		public T Deserialize<T>(IRestResponse response)
		{
			T val = Activator.CreateInstance<T>();
			if (val is IList)
			{
				Type type = val.GetType();
				if (RootElement.HasValue())
				{
					object parent = FindRoot(response.Content);
					val = (T)BuildList(type, parent);
				}
				else
				{
					object parent2 = SimpleJson.DeserializeObject(response.Content);
					val = (T)BuildList(type, parent2);
				}
			}
			else if (val is IDictionary)
			{
				object parent3 = FindRoot(response.Content);
				val = (T)BuildDictionary(val.GetType(), parent3);
			}
			else
			{
				object obj = FindRoot(response.Content);
				Map(val, (IDictionary<string, object>)obj);
			}
			return val;
		}

		private object FindRoot(string content)
		{
			IDictionary<string, object> dictionary = (IDictionary<string, object>)SimpleJson.DeserializeObject(content);
			if (RootElement.HasValue() && dictionary.ContainsKey(RootElement))
			{
				return dictionary[RootElement];
			}
			return dictionary;
		}

		private void Map(object target, IDictionary<string, object> data)
		{
			Type type = target.GetType();
			List<PropertyInfo> list = (from p in type.GetProperties()
				where p.CanWrite
				select p).ToList();
			foreach (PropertyInfo item in list)
			{
				Type type2 = item.PropertyType;
				string name = item.Name;
				string text = name.GetNameVariants(Culture).FirstOrDefault((string n) => data.ContainsKey(n));
				object obj = ((text != null) ? data[text] : null);
				if (obj != null)
				{
					if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						type2 = type2.GetGenericArguments()[0];
					}
					item.SetValue(target, ConvertValue(type2, obj), null);
				}
			}
		}

		private IDictionary BuildDictionary(Type type, object parent)
		{
			IDictionary dictionary = (IDictionary)Activator.CreateInstance(type);
			Type type2 = type.GetGenericArguments()[1];
			foreach (KeyValuePair<string, object> item in (IDictionary<string, object>)parent)
			{
				string key = item.Key;
				object value = ConvertValue(type2, item.Value);
				dictionary.Add(key, value);
			}
			return dictionary;
		}

		private IList BuildList(Type type, object parent)
		{
			IList list = (IList)Activator.CreateInstance(type);
			Type type2 = type.GetInterfaces().First((Type x) => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
			Type type3 = type2.GetGenericArguments()[0];
			if (parent is IList)
			{
				foreach (object item in (IList)parent)
				{
					if (type3.IsPrimitive)
					{
						string source = item.ToString();
						list.Add(source.ChangeType(type3, Culture));
					}
					else if (type3 == typeof(string))
					{
						if (item == null)
						{
							list.Add(null);
						}
						else
						{
							list.Add(item.ToString());
						}
					}
					else if (item == null)
					{
						list.Add(null);
					}
					else
					{
						object value = ConvertValue(type3, item);
						list.Add(value);
					}
				}
			}
			else
			{
				list.Add(ConvertValue(type3, parent));
			}
			return list;
		}

		private object ConvertValue(Type type, object value)
		{
			string text = Convert.ToString(value, Culture);
			if (type.IsPrimitive)
			{
				string source = text.Replace("\"", string.Empty);
				return source.ChangeType(type, Culture);
			}
			if (type.IsEnum)
			{
				return type.FindEnumValue(text, Culture);
			}
			if (type == typeof(Uri))
			{
				return new Uri(text, UriKind.RelativeOrAbsolute);
			}
			if (type == typeof(string))
			{
				return text;
			}
			if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
			{
				DateTime dateTime = ((!DateFormat.HasValue()) ? text.ParseJsonDate(Culture) : DateTime.ParseExact(text, DateFormat, Culture));
				if (type == typeof(DateTime))
				{
					return dateTime;
				}
				if (type == typeof(DateTimeOffset))
				{
					return (DateTimeOffset)dateTime;
				}
			}
			else
			{
				if (type == typeof(decimal))
				{
					return decimal.Parse(text, Culture);
				}
				if (type == typeof(Guid))
				{
					return string.IsNullOrEmpty(text) ? Guid.Empty : new Guid(text);
				}
				if (type == typeof(TimeSpan))
				{
					return TimeSpan.Parse(text);
				}
				if (!type.IsGenericType)
				{
					return CreateAndMap(type, value);
				}
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(List<>))
				{
					return BuildList(type, value);
				}
				if (!(genericTypeDefinition == typeof(Dictionary<, >)))
				{
					return CreateAndMap(type, value);
				}
				Type type2 = type.GetGenericArguments()[0];
				if (type2 == typeof(string))
				{
					return BuildDictionary(type, value);
				}
			}
			return null;
		}

		private object CreateAndMap(Type type, object element)
		{
			object obj = Activator.CreateInstance(type);
			Map(obj, (IDictionary<string, object>)element);
			return obj;
		}
	}
	public class XmlAttributeDeserializer : IDeserializer
	{
		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string DateFormat { get; set; }

		public CultureInfo Culture { get; set; }

		public XmlAttributeDeserializer()
		{
			Culture = CultureInfo.InvariantCulture;
		}

		public T Deserialize<T>(IRestResponse response)
		{
			if (response.Content == null)
			{
				return default(T);
			}
			XDocument val = XDocument.Parse(response.Content);
			XElement root = val.Root;
			if (RootElement.HasValue() && val.Root != null)
			{
				root = ((XContainer)val.Root).Element(RootElement.AsNamespaced(Namespace));
			}
			if (!Namespace.HasValue())
			{
				RemoveNamespace(val);
			}
			T val2 = Activator.CreateInstance<T>();
			Type type = val2.GetType();
			if (type.IsSubclassOfRawGeneric(typeof(List<>)))
			{
				val2 = (T)HandleListDerivative(val2, root, type.Name, type);
			}
			else
			{
				Map(val2, root);
			}
			return val2;
		}

		private void RemoveNamespace(XDocument xdoc)
		{
			foreach (XElement item in xdoc.Root.DescendantsAndSelf())
			{
				if (item.Name.Namespace != XNamespace.None)
				{
					item.Name = XNamespace.None.GetName(item.Name.LocalName);
				}
				if (item.Attributes().Any((XAttribute a) => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None))
				{
					item.ReplaceAttributes((object)(from a in item.Attributes()
						select (XAttribute)((!a.IsNamespaceDeclaration) ? ((a.Name.Namespace != XNamespace.None) ? ((object)new XAttribute(XNamespace.None.GetName(a.Name.LocalName), (object)a.Value)) : ((object)a)) : null)));
				}
			}
		}

		private void Map(object x, XElement root)
		{
			Type type = x.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type2 = propertyInfo.PropertyType;
				if (!type2.IsPublic || !propertyInfo.CanWrite)
				{
					continue;
				}
				XName val = propertyInfo.Name.AsNamespaced(Namespace);
				bool attribute = false;
				DeserializeAsAttribute attribute2 = propertyInfo.GetAttribute<DeserializeAsAttribute>();
				if (attribute2 != null)
				{
					string name = attribute2.Name;
					val = ((name != null) ? XName.op_Implicit(name) : val);
					attribute = attribute2.Attribute;
				}
				object valueFromXml = GetValueFromXml(root, val, attribute);
				if (valueFromXml == null)
				{
					if (type2.IsGenericType)
					{
						Type type3 = type2.GetGenericArguments()[0];
						XElement elementByName = GetElementByName(root, XName.op_Implicit(type3.Name));
						if (elementByName != null)
						{
							IEnumerable<XElement> elements = ((XContainer)root).Elements(elementByName.Name);
							IList list = (IList)Activator.CreateInstance(type2);
							PopulateListFromElements(type3, elements, list);
							propertyInfo.SetValue(x, list, null);
						}
					}
					continue;
				}
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					type2 = type2.GetGenericArguments()[0];
					if (string.IsNullOrEmpty(valueFromXml.ToString()))
					{
						continue;
					}
				}
				if (type2 == typeof(bool))
				{
					string s = valueFromXml.ToString().ToLower();
					propertyInfo.SetValue(x, XmlConvert.ToBoolean(s), null);
				}
				else if (type2.IsPrimitive)
				{
					propertyInfo.SetValue(x, valueFromXml.ChangeType(type2, Culture), null);
				}
				else if (type2.IsEnum)
				{
					object value = type2.FindEnumValue(valueFromXml.ToString(), Culture);
					propertyInfo.SetValue(x, value, null);
				}
				else if (type2 == typeof(Uri))
				{
					Uri value2 = new Uri(valueFromXml.ToString(), UriKind.RelativeOrAbsolute);
					propertyInfo.SetValue(x, value2, null);
				}
				else if (type2 == typeof(string))
				{
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(DateTime))
				{
					valueFromXml = ((!DateFormat.HasValue()) ? ((object)DateTime.Parse(valueFromXml.ToString(), Culture)) : ((object)DateTime.ParseExact(valueFromXml.ToString(), DateFormat, Culture)));
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(decimal))
				{
					valueFromXml = decimal.Parse(valueFromXml.ToString(), Culture);
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(Guid))
				{
					valueFromXml = new Guid(valueFromXml.ToString());
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2.IsGenericType)
				{
					Type t = type2.GetGenericArguments()[0];
					IList list2 = (IList)Activator.CreateInstance(type2);
					XElement elementByName2 = GetElementByName(root, val);
					XElement first = ((XContainer)elementByName2).Elements().FirstOrDefault();
					IEnumerable<XElement> elements2 = from d in ((XContainer)elementByName2).Elements()
						where d.Name == first.Name
						select d;
					PopulateListFromElements(t, elements2, list2);
					propertyInfo.SetValue(x, list2, null);
				}
				else if (type2.IsSubclassOfRawGeneric(typeof(List<>)))
				{
					object value3 = HandleListDerivative(x, root, ((object)val).ToString(), type2);
					propertyInfo.SetValue(x, value3, null);
				}
				else if (root != null)
				{
					XElement elementByName3 = GetElementByName(root, val);
					if (elementByName3 != null)
					{
						object value4 = CreateAndMap(type2, elementByName3);
						propertyInfo.SetValue(x, value4, null);
					}
				}
			}
		}

		private void PopulateListFromElements(Type t, IEnumerable<XElement> elements, IList list)
		{
			foreach (XElement element in elements)
			{
				object value = CreateAndMap(t, element);
				list.Add(value);
			}
		}

		private object HandleListDerivative(object x, XElement root, string propName, Type type)
		{
			Type type2 = type.BaseType.GetGenericArguments()[0];
			string text = type2.Name;
			DeserializeAsAttribute attribute = type2.GetAttribute<DeserializeAsAttribute>();
			if (attribute != null)
			{
				text = attribute.Name ?? text;
			}
			string text2 = text.ToLower();
			string text3 = text.ToCamelCase(Culture);
			IList list = (IList)Activator.CreateInstance(type);
			IEnumerable<XElement> elements = null;
			if (((XContainer)root).Descendants(text.AsNamespaced(Namespace)).Count() != 0)
			{
				elements = ((XContainer)root).Descendants(type2.Name.AsNamespaced(Namespace));
			}
			if (((XContainer)root).Descendants(XName.op_Implicit(text2)).Count() != 0)
			{
				elements = ((XContainer)root).Descendants(XName.op_Implicit(text2));
			}
			if (((XContainer)root).Descendants(XName.op_Implicit(text3)).Count() != 0)
			{
				elements = ((XContainer)root).Descendants(XName.op_Implicit(text3));
			}
			PopulateListFromElements(type2, elements, list);
			Map(list, ((XContainer)root).Element(propName.AsNamespaced(Namespace)));
			return list;
		}

		private object CreateAndMap(Type t, XElement element)
		{
			object obj = Activator.CreateInstance(t);
			Map(obj, element);
			return obj;
		}

		private object GetValueFromXml(XElement root, XName name, bool attribute)
		{
			object result = null;
			if (root == null)
			{
				return null;
			}
			if (attribute)
			{
				XAttribute attributeByName = GetAttributeByName(root, name);
				if (attributeByName != null)
				{
					result = attributeByName.Value;
				}
			}
			else
			{
				XElement elementByName = GetElementByName(root, name);
				if (elementByName == null)
				{
					XAttribute attributeByName2 = GetAttributeByName(root, name);
					if (attributeByName2 != null)
					{
						result = attributeByName2.Value;
					}
				}
				else if (!elementByName.IsEmpty || elementByName.HasElements || elementByName.HasAttributes)
				{
					result = elementByName.Value;
				}
			}
			return result;
		}

		private XElement GetElementByName(XElement root, XName name)
		{
			XName val = XName.Get(name.LocalName.ToLower(), name.NamespaceName);
			XName val2 = XName.Get(name.LocalName.ToCamelCase(Culture), name.NamespaceName);
			if (((XContainer)root).Element(name) != null)
			{
				return ((XContainer)root).Element(name);
			}
			if (((XContainer)root).Element(val) != null)
			{
				return ((XContainer)root).Element(val);
			}
			if (((XContainer)root).Element(val2) != null)
			{
				return ((XContainer)root).Element(val2);
			}
			if (name == XName.op_Implicit("Value") && root.Value != null)
			{
				return root;
			}
			XElement val3 = ((IEnumerable<XElement>)(from d in ((XContainer)root).Descendants()
				orderby ((XNode)d).Ancestors().Count()
				select d)).FirstOrDefault((Func<XElement, bool>)((XElement d) => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName));
			if (val3 != null)
			{
				return val3;
			}
			return null;
		}

		private XAttribute GetAttributeByName(XElement root, XName name)
		{
			XName val = XName.Get(name.LocalName.ToLower(), name.NamespaceName);
			XName val2 = XName.Get(name.LocalName.ToCamelCase(Culture), name.NamespaceName);
			if (root.Attribute(name) != null)
			{
				return root.Attribute(name);
			}
			if (root.Attribute(val) != null)
			{
				return root.Attribute(val);
			}
			if (root.Attribute(val2) != null)
			{
				return root.Attribute(val2);
			}
			XAttribute val3 = root.Attributes().FirstOrDefault((Func<XAttribute, bool>)((XAttribute d) => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName));
			if (val3 != null)
			{
				return val3;
			}
			return null;
		}
	}
	public class XmlDeserializer : IDeserializer
	{
		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string DateFormat { get; set; }

		public CultureInfo Culture { get; set; }

		public XmlDeserializer()
		{
			Culture = CultureInfo.InvariantCulture;
		}

		public T Deserialize<T>(IRestResponse response)
		{
			if (string.IsNullOrEmpty(response.Content))
			{
				return default(T);
			}
			XDocument val = XDocument.Parse(response.Content);
			XElement root = val.Root;
			if (RootElement.HasValue() && val.Root != null)
			{
				root = ((XContainer)val.Root).Element(RootElement.AsNamespaced(Namespace));
			}
			if (!Namespace.HasValue())
			{
				RemoveNamespace(val);
			}
			T val2 = Activator.CreateInstance<T>();
			Type type = val2.GetType();
			if (type.IsSubclassOfRawGeneric(typeof(List<>)))
			{
				val2 = (T)HandleListDerivative(val2, root, type.Name, type);
			}
			else
			{
				Map(val2, root);
			}
			return val2;
		}

		private void RemoveNamespace(XDocument xdoc)
		{
			foreach (XElement item in xdoc.Root.DescendantsAndSelf())
			{
				if (item.Name.Namespace != XNamespace.None)
				{
					item.Name = XNamespace.None.GetName(item.Name.LocalName);
				}
				if (item.Attributes().Any((XAttribute a) => a.IsNamespaceDeclaration || a.Name.Namespace != XNamespace.None))
				{
					item.ReplaceAttributes((object)(from a in item.Attributes()
						select (XAttribute)((!a.IsNamespaceDeclaration) ? ((a.Name.Namespace != XNamespace.None) ? ((object)new XAttribute(XNamespace.None.GetName(a.Name.LocalName), (object)a.Value)) : ((object)a)) : null)));
				}
			}
		}

		private void Map(object x, XElement root)
		{
			Type type = x.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				Type type2 = propertyInfo.PropertyType;
				if (!type2.IsPublic || !propertyInfo.CanWrite)
				{
					continue;
				}
				XName name = propertyInfo.Name.AsNamespaced(Namespace);
				object valueFromXml = GetValueFromXml(root, name);
				if (valueFromXml == null)
				{
					if (type2.IsGenericType)
					{
						Type type3 = type2.GetGenericArguments()[0];
						XElement elementByName = GetElementByName(root, XName.op_Implicit(type3.Name));
						IList list = (IList)Activator.CreateInstance(type2);
						if (elementByName != null)
						{
							IEnumerable<XElement> elements = ((XContainer)root).Elements(elementByName.Name);
							PopulateListFromElements(type3, elements, list);
						}
						propertyInfo.SetValue(x, list, null);
					}
					continue;
				}
				if (type2.IsGenericType && type2.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					if (valueFromXml == null || string.IsNullOrEmpty(valueFromXml.ToString()))
					{
						propertyInfo.SetValue(x, null, null);
						continue;
					}
					type2 = type2.GetGenericArguments()[0];
				}
				if (type2 == typeof(bool))
				{
					string s = valueFromXml.ToString().ToLower();
					propertyInfo.SetValue(x, XmlConvert.ToBoolean(s), null);
				}
				else if (type2.IsPrimitive)
				{
					propertyInfo.SetValue(x, valueFromXml.ChangeType(type2, Culture), null);
				}
				else if (type2.IsEnum)
				{
					object value = type2.FindEnumValue(valueFromXml.ToString(), Culture);
					propertyInfo.SetValue(x, value, null);
				}
				else if (type2 == typeof(Uri))
				{
					Uri value2 = new Uri(valueFromXml.ToString(), UriKind.RelativeOrAbsolute);
					propertyInfo.SetValue(x, value2, null);
				}
				else if (type2 == typeof(string))
				{
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(DateTime))
				{
					valueFromXml = ((!DateFormat.HasValue()) ? ((object)DateTime.Parse(valueFromXml.ToString(), Culture)) : ((object)DateTime.ParseExact(valueFromXml.ToString(), DateFormat, Culture)));
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(decimal))
				{
					valueFromXml = decimal.Parse(valueFromXml.ToString(), Culture);
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(Guid))
				{
					string value3 = valueFromXml.ToString();
					valueFromXml = (string.IsNullOrEmpty(value3) ? Guid.Empty : new Guid(valueFromXml.ToString()));
					propertyInfo.SetValue(x, valueFromXml, null);
				}
				else if (type2 == typeof(TimeSpan))
				{
					TimeSpan timeSpan = XmlConvert.ToTimeSpan(valueFromXml.ToString());
					propertyInfo.SetValue(x, timeSpan, null);
				}
				else if (type2.IsGenericType)
				{
					Type t = type2.GetGenericArguments()[0];
					IList list2 = (IList)Activator.CreateInstance(type2);
					XElement elementByName2 = GetElementByName(root, propertyInfo.Name.AsNamespaced(Namespace));
					if (elementByName2.HasElements)
					{
						XElement val = ((XContainer)elementByName2).Elements().FirstOrDefault();
						IEnumerable<XElement> elements2 = ((XContainer)elementByName2).Elements(val.Name);
						PopulateListFromElements(t, elements2, list2);
					}
					propertyInfo.SetValue(x, list2, null);
				}
				else if (type2.IsSubclassOfRawGeneric(typeof(List<>)))
				{
					object value4 = HandleListDerivative(x, root, propertyInfo.Name, type2);
					propertyInfo.SetValue(x, value4, null);
				}
				else if (root != null)
				{
					XElement elementByName3 = GetElementByName(root, name);
					if (elementByName3 != null)
					{
						object value5 = CreateAndMap(type2, elementByName3);
						propertyInfo.SetValue(x, value5, null);
					}
				}
			}
		}

		private void PopulateListFromElements(Type t, IEnumerable<XElement> elements, IList list)
		{
			foreach (XElement element in elements)
			{
				object value = CreateAndMap(t, element);
				list.Add(value);
			}
		}

		private object HandleListDerivative(object x, XElement root, string propName, Type type)
		{
			Type type2 = ((!type.IsGenericType) ? type.BaseType.GetGenericArguments()[0] : type.GetGenericArguments()[0]);
			IList list = (IList)Activator.CreateInstance(type);
			IEnumerable<XElement> enumerable = ((XContainer)root).Descendants(type2.Name.AsNamespaced(Namespace));
			string name = type2.Name;
			if (!enumerable.Any())
			{
				XName val = name.ToLower().AsNamespaced(Namespace);
				enumerable = ((XContainer)root).Descendants(val);
			}
			if (!enumerable.Any())
			{
				XName val2 = name.ToCamelCase(Culture).AsNamespaced(Namespace);
				enumerable = ((XContainer)root).Descendants(val2);
			}
			if (!enumerable.Any())
			{
				enumerable = from e in ((XContainer)root).Descendants()
					where e.Name.LocalName.RemoveUnderscoresAndDashes() == name
					select e;
			}
			if (!enumerable.Any())
			{
				XName lowerName = name.ToLower().AsNamespaced(Namespace);
				enumerable = from e in ((XContainer)root).Descendants()
					where XName.op_Implicit(e.Name.LocalName.RemoveUnderscoresAndDashes()) == lowerName
					select e;
			}
			PopulateListFromElements(type2, enumerable, list);
			if (!type.IsGenericType)
			{
				Map(list, ((XContainer)root).Element(propName.AsNamespaced(Namespace)) ?? root);
			}
			return list;
		}

		private object CreateAndMap(Type t, XElement element)
		{
			object obj;
			if (t == typeof(string))
			{
				obj = element.Value;
			}
			else if (t.IsPrimitive)
			{
				obj = element.Value.ChangeType(t, Culture);
			}
			else
			{
				obj = Activator.CreateInstance(t);
				Map(obj, element);
			}
			return obj;
		}

		private object GetValueFromXml(XElement root, XName name)
		{
			object result = null;
			if (root != null)
			{
				XElement elementByName = GetElementByName(root, name);
				if (elementByName == null)
				{
					XAttribute attributeByName = GetAttributeByName(root, name);
					if (attributeByName != null)
					{
						result = attributeByName.Value;
					}
				}
				else if (!elementByName.IsEmpty || elementByName.HasElements || elementByName.HasAttributes)
				{
					result = elementByName.Value;
				}
			}
			return result;
		}

		private XElement GetElementByName(XElement root, XName name)
		{
			XName val = name.LocalName.ToLower().AsNamespaced(name.NamespaceName);
			XName val2 = name.LocalName.ToCamelCase(Culture).AsNamespaced(name.NamespaceName);
			if (((XContainer)root).Element(name) != null)
			{
				return ((XContainer)root).Element(name);
			}
			if (((XContainer)root).Element(val) != null)
			{
				return ((XContainer)root).Element(val);
			}
			if (((XContainer)root).Element(val2) != null)
			{
				return ((XContainer)root).Element(val2);
			}
			if (name == "Value".AsNamespaced(name.NamespaceName))
			{
				return root;
			}
			XElement val3 = ((IEnumerable<XElement>)(from d in ((XContainer)root).Descendants()
				orderby ((XNode)d).Ancestors().Count()
				select d)).FirstOrDefault((Func<XElement, bool>)((XElement d) => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName)) ?? ((IEnumerable<XElement>)(from d in ((XContainer)root).Descendants()
				orderby ((XNode)d).Ancestors().Count()
				select d)).FirstOrDefault((Func<XElement, bool>)((XElement d) => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName.ToLower()));
			if (val3 != null)
			{
				return val3;
			}
			return null;
		}

		private XAttribute GetAttributeByName(XElement root, XName name)
		{
			XName val = name.LocalName.ToLower().AsNamespaced(name.NamespaceName);
			XName val2 = name.LocalName.ToCamelCase(Culture).AsNamespaced(name.NamespaceName);
			if (root.Attribute(name) != null)
			{
				return root.Attribute(name);
			}
			if (root.Attribute(val) != null)
			{
				return root.Attribute(val);
			}
			if (root.Attribute(val2) != null)
			{
				return root.Attribute(val2);
			}
			XAttribute val3 = root.Attributes().FirstOrDefault((Func<XAttribute, bool>)((XAttribute d) => d.Name.LocalName.RemoveUnderscoresAndDashes() == name.LocalName));
			if (val3 != null)
			{
				return val3;
			}
			return null;
		}
	}
}
namespace RestSharp
{
	public enum ParameterType
	{
		Cookie,
		GetOrPost,
		UrlSegment,
		HttpHeader,
		RequestBody
	}
	public enum DataFormat
	{
		Json,
		Xml
	}
	public enum Method
	{
		GET,
		POST,
		PUT,
		DELETE,
		HEAD,
		OPTIONS,
		PATCH
	}
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct DateFormat
	{
		public const string Iso8601 = "s";

		public const string RoundTrip = "u";
	}
	public enum ResponseStatus
	{
		None,
		Completed,
		Error,
		TimedOut,
		Aborted
	}
}
namespace RestSharp.Extensions
{
	public static class MiscExtensions
	{
		public static void SaveAs(this byte[] input, string path)
		{
			File.WriteAllBytes(path, input);
		}

		public static byte[] ReadAsBytes(this Stream input)
		{
			byte[] array = new byte[16384];
			using MemoryStream memoryStream = new MemoryStream();
			int count;
			while ((count = input.Read(array, 0, array.Length)) > 0)
			{
				memoryStream.Write(array, 0, count);
			}
			return memoryStream.ToArray();
		}

		public static void CopyTo(this Stream input, Stream output)
		{
			byte[] array = new byte[32768];
			while (true)
			{
				int num = input.Read(array, 0, array.Length);
				if (num <= 0)
				{
					break;
				}
				output.Write(array, 0, num);
			}
		}

		public static string AsString(this byte[] buffer)
		{
			if (buffer == null)
			{
				return "";
			}
			Encoding uTF = Encoding.UTF8;
			return uTF.GetString(buffer);
		}
	}
}
namespace RestSharp.Contrib
{
	internal class Helpers
	{
		public static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;
	}
	internal class HttpEncoder
	{
		private static char[] hexChars;

		private static object entitiesLock;

		private static SortedDictionary<string, char> entities;

		private static HttpEncoder defaultEncoder;

		private static HttpEncoder currentEncoder;

		private static IDictionary<string, char> Entities
		{
			get
			{
				lock (entitiesLock)
				{
					if (entities == null)
					{
						InitEntities();
					}
					return entities;
				}
			}
		}

		public static HttpEncoder Current => currentEncoder;

		public static HttpEncoder Default => defaultEncoder;

		static HttpEncoder()
		{
			hexChars = "0123456789abcdef".ToCharArray();
			entitiesLock = new object();
			defaultEncoder = new HttpEncoder();
			currentEncoder = defaultEncoder;
		}

		internal static void HeaderNameValueEncode(string headerName, string headerValue, out string encodedHeaderName, out string encodedHeaderValue)
		{
			if (string.IsNullOrEmpty(headerName))
			{
				encodedHeaderName = headerName;
			}
			else
			{
				encodedHeaderName = EncodeHeaderString(headerName);
			}
			if (string.IsNullOrEmpty(headerValue))
			{
				encodedHeaderValue = headerValue;
			}
			else
			{
				encodedHeaderValue = EncodeHeaderString(headerValue);
			}
		}

		private static void StringBuilderAppend(string s, ref StringBuilder sb)
		{
			if (sb == null)
			{
				sb = new StringBuilder(s);
			}
			else
			{
				sb.Append(s);
			}
		}

		private static string EncodeHeaderString(string input)
		{
			StringBuilder sb = null;
			foreach (char c in input)
			{
				if ((c < ' ' && c != '\t') || c == '\u007f')
				{
					StringBuilderAppend($"%{(int)c:x2}", ref sb);
				}
			}
			if (sb != null)
			{
				return sb.ToString();
			}
			return input;
		}

		internal static string UrlPathEncode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			MemoryStream memoryStream = new MemoryStream();
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				UrlPathEncodeChar(value[i], memoryStream);
			}
			return Encoding.ASCII.GetString(memoryStream.ToArray());
		}

		internal static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int num = bytes.Length;
			if (num == 0)
			{
				return new byte[0];
			}
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > num - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			MemoryStream memoryStream = new MemoryStream(count);
			int num2 = offset + count;
			for (int i = offset; i < num2; i++)
			{
				UrlEncodeChar((char)bytes[i], memoryStream, isUnicode: false);
			}
			return memoryStream.ToArray();
		}

		internal static string HtmlEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return string.Empty;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c == '&' || c == '"' || c == '<' || c == '>' || c > '\u009f')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = s.Length;
			for (int j = 0; j < length; j++)
			{
				switch (s[j])
				{
				case '&':
					stringBuilder.Append("&amp;");
					continue;
				case '>':
					stringBuilder.Append("&gt;");
					continue;
				case '<':
					stringBuilder.Append("&lt;");
					continue;
				case '"':
					stringBuilder.Append("&quot;");
					continue;
				case '＜':
					stringBuilder.Append("&#65308;");
					continue;
				case '＞':
					stringBuilder.Append("&#65310;");
					continue;
				}
				char c2 = s[j];
				if (c2 > '\u009f' && c2 < 'Ā')
				{
					stringBuilder.Append("&#");
					int num = c2;
					stringBuilder.Append(num.ToString(Helpers.InvariantCulture));
					stringBuilder.Append(";");
				}
				else
				{
					stringBuilder.Append(c2);
				}
			}
			return stringBuilder.ToString();
		}

		internal static string HtmlAttributeEncode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return string.Empty;
			}
			bool flag = false;
			foreach (char c in s)
			{
				if (c == '&' || c == '"' || c == '<')
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int length = s.Length;
			for (int j = 0; j < length; j++)
			{
				switch (s[j])
				{
				case '&':
					stringBuilder.Append("&amp;");
					break;
				case '"':
					stringBuilder.Append("&quot;");
					break;
				case '<':
					stringBuilder.Append("&lt;");
					break;
				default:
					stringBuilder.Append(s[j]);
					break;
				}
			}
			return stringBuilder.ToString();
		}

		internal static string HtmlDecode(string s)
		{
			if (s == null)
			{
				return null;
			}
			if (s.Length == 0)
			{
				return string.Empty;
			}
			if (s.IndexOf('&') == -1)
			{
				return s;
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			int length = s.Length;
			int num = 0;
			int num2 = 0;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if (num == 0)
				{
					if (c == '&')
					{
						stringBuilder.Append(c);
						num = 1;
					}
					else
					{
						stringBuilder2.Append(c);
					}
					continue;
				}
				if (c == '&')
				{
					num = 1;
					if (flag2)
					{
						stringBuilder.Append(num2.ToString(Helpers.InvariantCulture));
						flag2 = false;
					}
					stringBuilder2.Append(stringBuilder.ToString());
					stringBuilder.Length = 0;
					stringBuilder.Append('&');
					continue;
				}
				switch (num)
				{
				case 1:
					if (c == ';')
					{
						num = 0;
						stringBuilder2.Append(stringBuilder.ToString());
						stringBuilder2.Append(c);
						stringBuilder.Length = 0;
					}
					else
					{
						num2 = 0;
						flag = false;
						num = ((c == '#') ? 3 : 2);
						stringBuilder.Append(c);
					}
					break;
				case 2:
					stringBuilder.Append(c);
					if (c == ';')
					{
						string text = stringBuilder.ToString();
						if (text.Length > 1 && Entities.ContainsKey(text.Substring(1, text.Length - 2)))
						{
							text = Entities[text.Substring(1, text.Length - 2)].ToString();
						}
						stringBuilder2.Append(text);
						num = 0;
						stringBuilder.Length = 0;
					}
					break;
				case 3:
					if (c == ';')
					{
						if (num2 > 65535)
						{
							stringBuilder2.Append("&#");
							stringBuilder2.Append(num2.ToString(Helpers.InvariantCulture));
							stringBuilder2.Append(";");
						}
						else
						{
							stringBuilder2.Append((char)num2);
						}
						num = 0;
						stringBuilder.Length = 0;
						flag2 = false;
					}
					else if (flag && Uri.IsHexDigit(c))
					{
						num2 = num2 * 16 + Uri.FromHex(c);
						flag2 = true;
					}
					else if (char.IsDigit(c))
					{
						num2 = num2 * 10 + (c - 48);
						flag2 = true;
					}
					else if (num2 == 0 && (c == 'x' || c == 'X'))
					{
						flag = true;
					}
					else
					{
						num = 2;
						if (flag2)
						{
							stringBuilder.Append(num2.ToString(Helpers.InvariantCulture));
							flag2 = false;
						}
						stringBuilder.Append(c);
					}
					break;
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder2.Append(stringBuilder.ToString());
			}
			else if (flag2)
			{
				stringBuilder2.Append(num2.ToString(Helpers.InvariantCulture));
			}
			return stringBuilder2.ToString();
		}

		internal static bool NotEncoded(char c)
		{
			if (c != '!' && c != '(' && c != ')' && c != '*' && c != '-' && c != '.' && c != '_')
			{
				return c == '\'';
			}
			return true;
		}

		internal static void UrlEncodeChar(char c, Stream result, bool isUnicode)
		{
			if (c > 'ÿ')
			{
				result.WriteByte(37);
				result.WriteByte(117);
				int num = (int)c >> 12;
				result.WriteByte((byte)hexChars[num]);
				num = ((int)c >> 8) & 0xF;
				result.WriteByte((byte)hexChars[num]);
				num = ((int)c >> 4) & 0xF;
				result.WriteByte((byte)hexChars[num]);
				num = c & 0xF;
				result.WriteByte((byte)hexChars[num]);
			}
			else if (c > ' ' && NotEncoded(c))
			{
				result.WriteByte((byte)c);
			}
			else if (c == ' ')
			{
				result.WriteByte(43);
			}
			else if (c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z')
			{
				if (isUnicode && c > '\u007f')
				{
					result.WriteByte(37);
					result.WriteByte(117);
					result.WriteByte(48);
					result.WriteByte(48);
				}
				else
				{
					result.WriteByte(37);
				}
				int num2 = (int)c >> 4;
				result.WriteByte((byte)hexChars[num2]);
				num2 = c & 0xF;
				result.WriteByte((byte)hexChars[num2]);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		internal static void UrlPathEncodeChar(char c, Stream result)
		{
			if (c < '!' || c > '~')
			{
				byte[] bytes = Encoding.UTF8.GetBytes(c.ToString());
				for (int i = 0; i < bytes.Length; i++)
				{
					result.WriteByte(37);
					int num = bytes[i] >> 4;
					result.WriteByte((byte)hexChars[num]);
					num = bytes[i] & 0xF;
					result.WriteByte((byte)hexChars[num]);
				}
			}
			else if (c == ' ')
			{
				result.WriteByte(37);
				result.WriteByte(50);
				result.WriteByte(48);
			}
			else
			{
				result.WriteByte((byte)c);
			}
		}

		private static void InitEntities()
		{
			entities = new SortedDictionary<string, char>(StringComparer.Ordinal);
			entities.Add("nbsp", '\u00a0');
			entities.Add("iexcl", '¡');
			entities.Add("cent", '¢');
			entities.Add("pound", '£');
			entities.Add("curren", '¤');
			entities.Add("yen", '¥');
			entities.Add("brvbar", '¦');
			entities.Add("sect", '§');
			entities.Add("uml", '\u00a8');
			entities.Add("copy", '©');
			entities.Add("ordf", 'ª');
			entities.Add("laquo", '«');
			entities.Add("not", '¬');
			entities.Add("shy", '\u00ad');
			entities.Add("reg", '®');
			entities.Add("macr", '\u00af');
			entities.Add("deg", '°');
			entities.Add("plusmn", '±');
			entities.Add("sup2", '²');
			entities.Add("sup3", '³');
			entities.Add("acute", '\u00b4');
			entities.Add("micro", 'µ');
			entities.Add("para", '¶');
			entities.Add("middot", '·');
			entities.Add("cedil", '\u00b8');
			entities.Add("sup1", '¹');
			entities.Add("ordm", 'º');
			entities.Add("raquo", '»');
			entities.Add("frac14", '¼');
			entities.Add("frac12", '½');
			entities.Add("frac34", '¾');
			entities.Add("iquest", '¿');
			entities.Add("Agrave", 'À');
			entities.Add("Aacute", 'Á');
			entities.Add("Acirc", 'Â');
			entities.Add("Atilde", 'Ã');
			entities.Add("Auml", 'Ä');
			entities.Add("Aring", 'Å');
			entities.Add("AElig", 'Æ');
			entities.Add("Ccedil", 'Ç');
			entities.Add("Egrave", 'È');
			entities.Add("Eacute", 'É');
			entities.Add("Ecirc", 'Ê');
			entities.Add("Euml", 'Ë');
			entities.Add("Igrave", 'Ì');
			entities.Add("Iacute", 'Í');
			entities.Add("Icirc", 'Î');
			entities.Add("Iuml", 'Ï');
			entities.Add("ETH", 'Ð');
			entities.Add("Ntilde", 'Ñ');
			entities.Add("Ograve", 'Ò');
			entities.Add("Oacute", 'Ó');
			entities.Add("Ocirc", 'Ô');
			entities.Add("Otilde", 'Õ');
			entities.Add("Ouml", 'Ö');
			entities.Add("times", '×');
			entities.Add("Oslash", 'Ø');
			entities.Add("Ugrave", 'Ù');
			entities.Add("Uacute", 'Ú');
			entities.Add("Ucirc", 'Û');
			entities.Add("Uuml", 'Ü');
			entities.Add("Yacute", 'Ý');
			entities.Add("THORN", 'Þ');
			entities.Add("szlig", 'ß');
			entities.Add("agrave", 'à');
			entities.Add("aacute", 'á');
			entities.Add("acirc", 'â');
			entities.Add("atilde", 'ã');
			entities.Add("auml", 'ä');
			entities.Add("aring", 'å');
			entities.Add("aelig", 'æ');
			entities.Add("ccedil", 'ç');
			entities.Add("egrave", 'è');
			entities.Add("eacute", 'é');
			entities.Add("ecirc", 'ê');
			entities.Add("euml", 'ë');
			entities.Add("igrave", 'ì');
			entities.Add("iacute", 'í');
			entities.Add("icirc", 'î');
			entities.Add("iuml", 'ï');
			entities.Add("eth", 'ð');
			entities.Add("ntilde", 'ñ');
			entities.Add("ograve", 'ò');
			entities.Add("oacute", 'ó');
			entities.Add("ocirc", 'ô');
			entities.Add("otilde", 'õ');
			entities.Add("ouml", 'ö');
			entities.Add("divide", '÷');
			entities.Add("oslash", 'ø');
			entities.Add("ugrave", 'ù');
			entities.Add("uacute", 'ú');
			entities.Add("ucirc", 'û');
			entities.Add("uuml", 'ü');
			entities.Add("yacute", 'ý');
			entities.Add("thorn", 'þ');
			entities.Add("yuml", 'ÿ');
			entities.Add("fnof", 'ƒ');
			entities.Add("Alpha", 'Α');
			entities.Add("Beta", 'Β');
			entities.Add("Gamma", 'Γ');
			entities.Add("Delta", 'Δ');
			entities.Add("Epsilon", 'Ε');
			entities.Add("Zeta", 'Ζ');
			entities.Add("Eta", 'Η');
			entities.Add("Theta", 'Θ');
			entities.Add("Iota", 'Ι');
			entities.Add("Kappa", 'Κ');
			entities.Add("Lambda", 'Λ');
			entities.Add("Mu", 'Μ');
			entities.Add("Nu", 'Ν');
			entities.Add("Xi", 'Ξ');
			entities.Add("Omicron", 'Ο');
			entities.Add("Pi", 'Π');
			entities.Add("Rho", 'Ρ');
			entities.Add("Sigma", 'Σ');
			entities.Add("Tau", 'Τ');
			entities.Add("Upsilon", 'Υ');
			entities.Add("Phi", 'Φ');
			entities.Add("Chi", 'Χ');
			entities.Add("Psi", 'Ψ');
			entities.Add("Omega", 'Ω');
			entities.Add("alpha", 'α');
			entities.Add("beta", 'β');
			entities.Add("gamma", 'γ');
			entities.Add("delta", 'δ');
			entities.Add("epsilon", 'ε');
			entities.Add("zeta", 'ζ');
			entities.Add("eta", 'η');
			entities.Add("theta", 'θ');
			entities.Add("iota", 'ι');
			entities.Add("kappa", 'κ');
			entities.Add("lambda", 'λ');
			entities.Add("mu", 'μ');
			entities.Add("nu", 'ν');
			entities.Add("xi", 'ξ');
			entities.Add("omicron", 'ο');
			entities.Add("pi", 'π');
			entities.Add("rho", 'ρ');
			entities.Add("sigmaf", 'ς');
			entities.Add("sigma", 'σ');
			entities.Add("tau", 'τ');
			entities.Add("upsilon", 'υ');
			entities.Add("phi", 'φ');
			entities.Add("chi", 'χ');
			entities.Add("psi", 'ψ');
			entities.Add("omega", 'ω');
			entities.Add("thetasym", 'ϑ');
			entities.Add("upsih", 'ϒ');
			entities.Add("piv", 'ϖ');
			entities.Add("bull", '•');
			entities.Add("hellip", '…');
			entities.Add("prime", '′');
			entities.Add("Prime", '″');
			entities.Add("oline", '‾');
			entities.Add("frasl", '⁄');
			entities.Add("weierp", '℘');
			entities.Add("image", 'ℑ');
			entities.Add("real", 'ℜ');
			entities.Add("trade", '™');
			entities.Add("alefsym", 'ℵ');
			entities.Add("larr", '←');
			entities.Add("uarr", '↑');
			entities.Add("rarr", '→');
			entities.Add("darr", '↓');
			entities.Add("harr", '↔');
			entities.Add("crarr", '↵');
			entities.Add("lArr", '⇐');
			entities.Add("uArr", '⇑');
			entities.Add("rArr", '⇒');
			entities.Add("dArr", '⇓');
			entities.Add("hArr", '⇔');
			entities.Add("forall", '∀');
			entities.Add("part", '∂');
			entities.Add("exist", '∃');
			entities.Add("empty", '∅');
			entities.Add("nabla", '∇');
			entities.Add("isin", '∈');
			entities.Add("notin", '∉');
			entities.Add("ni", '∋');
			entities.Add("prod", '∏');
			entities.Add("sum", '∑');
			entities.Add("minus", '−');
			entities.Add("lowast", '∗');
			entities.Add("radic", '√');
			entities.Add("prop", '∝');
			entities.Add("infin", '∞');
			entities.Add("ang", '∠');
			entities.Add("and", '∧');
			entities.Add("or", '∨');
			entities.Add("cap", '∩');
			entities.Add("cup", '∪');
			entities.Add("int", '∫');
			entities.Add("there4", '∴');
			entities.Add("sim", '∼');
			entities.Add("cong", '≅');
			entities.Add("asymp", '≈');
			entities.Add("ne", '≠');
			entities.Add("equiv", '≡');
			entities.Add("le", '≤');
			entities.Add("ge", '≥');
			entities.Add("sub", '⊂');
			entities.Add("sup", '⊃');
			entities.Add("nsub", '⊄');
			entities.Add("sube", '⊆');
			entities.Add("supe", '⊇');
			entities.Add("oplus", '⊕');
			entities.Add("otimes", '⊗');
			entities.Add("perp", '⊥');
			entities.Add("sdot", '⋅');
			entities.Add("lceil", '⌈');
			entities.Add("rceil", '⌉');
			entities.Add("lfloor", '⌊');
			entities.Add("rfloor", '⌋');
			entities.Add("lang", '〈');
			entities.Add("rang", '〉');
			entities.Add("loz", '◊');
			entities.Add("spades", '♠');
			entities.Add("clubs", '♣');
			entities.Add("hearts", '♥');
			entities.Add("diams", '♦');
			entities.Add("quot", '"');
			entities.Add("amp", '&');
			entities.Add("lt", '<');
			entities.Add("gt", '>');
			entities.Add("OElig", 'Œ');
			entities.Add("oelig", 'œ');
			entities.Add("Scaron", 'Š');
			entities.Add("scaron", 'š');
			entities.Add("Yuml", 'Ÿ');
			entities.Add("circ", 'ˆ');
			entities.Add("tilde", '\u02dc');
			entities.Add("ensp", '\u2002');
			entities.Add("emsp", '\u2003');
			entities.Add("thinsp", '\u2009');
			entities.Add("zwnj", '\u200c');
			entities.Add("zwj", '\u200d');
			entities.Add("lrm", '\u200e');
			entities.Add("rlm", '\u200f');
			entities.Add("ndash", '–');
			entities.Add("mdash", '—');
			entities.Add("lsquo", '‘');
			entities.Add("rsquo", '’');
			entities.Add("sbquo", '‚');
			entities.Add("ldquo", '“');
			entities.Add("rdquo", '”');
			entities.Add("bdquo", '„');
			entities.Add("dagger", '†');
			entities.Add("Dagger", '‡');
			entities.Add("permil", '‰');
			entities.Add("lsaquo", '‹');
			entities.Add("rsaquo", '›');
			entities.Add("euro", '€');
		}
	}
	public sealed class HttpUtility
	{
		private sealed class HttpQSCollection : NameValueCollection
		{
			public override string ToString()
			{
				int count = Count;
				if (count == 0)
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
				string[] allKeys = AllKeys;
				for (int i = 0; i < count; i++)
				{
					stringBuilder.AppendFormat("{0}={1}&", allKeys[i], base[allKeys[i]]);
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Length--;
				}
				return stringBuilder.ToString();
			}
		}

		public static void HtmlAttributeEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new NullReferenceException(".NET emulation");
			}
			output.Write(HttpEncoder.HtmlAttributeEncode(s));
		}

		public static string HtmlAttributeEncode(string s)
		{
			return HttpEncoder.HtmlAttributeEncode(s);
		}

		public static string UrlDecode(string str)
		{
			return UrlDecode(str, Encoding.UTF8);
		}

		private static char[] GetChars(MemoryStream b, Encoding e)
		{
			return e.GetChars(b.GetBuffer(), 0, (int)b.Length);
		}

		private static void WriteCharBytes(IList buf, char ch, Encoding e)
		{
			if (ch > 'ÿ')
			{
				byte[] bytes = e.GetBytes(new char[1] { ch });
				foreach (byte b in bytes)
				{
					buf.Add(b);
				}
			}
			else
			{
				buf.Add((byte)ch);
			}
		}

		public static string UrlDecode(string s, Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
			{
				return s;
			}
			if (e == null)
			{
				e = Encoding.UTF8;
			}
			long num = s.Length;
			List<byte> list = new List<byte>();
			for (int i = 0; i < num; i++)
			{
				char c = s[i];
				if (c == '%' && i + 2 < num && s[i + 1] != '%')
				{
					int @char;
					if (s[i + 1] == 'u' && i + 5 < num)
					{
						@char = GetChar(s, i + 2, 4);
						if (@char != -1)
						{
							WriteCharBytes(list, (char)@char, e);
							i += 5;
						}
						else
						{
							WriteCharBytes(list, '%', e);
						}
					}
					else if ((@char = GetChar(s, i + 1, 2)) != -1)
					{
						WriteCharBytes(list, (char)@char, e);
						i += 2;
					}
					else
					{
						WriteCharBytes(list, '%', e);
					}
				}
				else if (c == '+')
				{
					WriteCharBytes(list, ' ', e);
				}
				else
				{
					WriteCharBytes(list, c, e);
				}
			}
			byte[] bytes = list.ToArray();
			list = null;
			return e.GetString(bytes);
		}

		public static string UrlDecode(byte[] bytes, Encoding e)
		{
			if (bytes == null)
			{
				return null;
			}
			return UrlDecode(bytes, 0, bytes.Length, e);
		}

		private static int GetInt(byte b)
		{
			char c = (char)b;
			if (c >= '0' && c <= '9')
			{
				return c - 48;
			}
			if (c >= 'a' && c <= 'f')
			{
				return c - 97 + 10;
			}
			if (c >= 'A' && c <= 'F')
			{
				return c - 65 + 10;
			}
			return -1;
		}

		private static int GetChar(byte[] bytes, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				int @int = GetInt(bytes[i]);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		private static int GetChar(string str, int offset, int length)
		{
			int num = 0;
			int num2 = length + offset;
			for (int i = offset; i < num2; i++)
			{
				char c = str[i];
				if (c > '\u007f')
				{
					return -1;
				}
				int @int = GetInt((byte)c);
				if (@int == -1)
				{
					return -1;
				}
				num = (num << 4) + @int;
			}
			return num;
		}

		public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
		{
			if (bytes == null)
			{
				return null;
			}
			if (count == 0)
			{
				return string.Empty;
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (offset < 0 || offset > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			StringBuilder stringBuilder = new StringBuilder();
			MemoryStream memoryStream = new MemoryStream();
			int num = count + offset;
			for (int i = offset; i < num; i++)
			{
				if (bytes[i] == 37 && i + 2 < count && bytes[i + 1] != 37)
				{
					int @char;
					if (bytes[i + 1] == 117 && i + 5 < num)
					{
						if (memoryStream.Length > 0)
						{
							stringBuilder.Append(GetChars(memoryStream, e));
							memoryStream.SetLength(0L);
						}
						@char = GetChar(bytes, i + 2, 4);
						if (@char != -1)
						{
							stringBuilder.Append((char)@char);
							i += 5;
							continue;
						}
					}
					else if ((@char = GetChar(bytes, i + 1, 2)) != -1)
					{
						memoryStream.WriteByte((byte)@char);
						i += 2;
						continue;
					}
				}
				if (memoryStream.Length > 0)
				{
					stringBuilder.Append(GetChars(memoryStream, e));
					memoryStream.SetLength(0L);
				}
				if (bytes[i] == 43)
				{
					stringBuilder.Append(' ');
				}
				else
				{
					stringBuilder.Append((char)bytes[i]);
				}
			}
			if (memoryStream.Length > 0)
			{
				stringBuilder.Append(GetChars(memoryStream, e));
			}
			memoryStream = null;
			return stringBuilder.ToString();
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return UrlDecodeToBytes(bytes, 0, bytes.Length);
		}

		public static byte[] UrlDecodeToBytes(string str)
		{
			return UrlDecodeToBytes(str, Encoding.UTF8);
		}

		public static byte[] UrlDecodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			return UrlDecodeToBytes(e.GetBytes(str));
		}

		public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			if (count == 0)
			{
				return new byte[0];
			}
			int num = bytes.Length;
			if (offset < 0 || offset >= num)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || offset > num - count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			MemoryStream memoryStream = new MemoryStream();
			int num2 = offset + count;
			for (int i = offset; i < num2; i++)
			{
				char c = (char)bytes[i];
				switch (c)
				{
				case '+':
					c = ' ';
					break;
				case '%':
					if (i < num2 - 2)
					{
						int @char = GetChar(bytes, i + 1, 2);
						if (@char != -1)
						{
							c = (char)@char;
							i += 2;
						}
					}
					break;
				}
				memoryStream.WriteByte((byte)c);
			}
			return memoryStream.ToArray();
		}

		public static string UrlEncode(string str)
		{
			return UrlEncode(str, Encoding.UTF8);
		}

		public static string UrlEncode(string s, Encoding Enc)
		{
			if (s == null)
			{
				return null;
			}
			if (s == string.Empty)
			{
				return string.Empty;
			}
			bool flag = false;
			int length = s.Length;
			for (int i = 0; i < length; i++)
			{
				char c = s[i];
				if ((c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z') && !HttpEncoder.NotEncoded(c))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return s;
			}
			byte[] bytes = new byte[Enc.GetMaxByteCount(s.Length)];
			int bytes2 = Enc.GetBytes(s, 0, s.Length, bytes, 0);
			return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes2));
		}

		public static string UrlEncode(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes.Length));
		}

		public static string UrlEncode(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return string.Empty;
			}
			return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, offset, count));
		}

		public static byte[] UrlEncodeToBytes(string str)
		{
			return UrlEncodeToBytes(str, Encoding.UTF8);
		}

		public static byte[] UrlEncodeToBytes(string str, Encoding e)
		{
			if (str == null)
			{
				return null;
			}
			if (str.Length == 0)
			{
				return new byte[0];
			}
			byte[] bytes = e.GetBytes(str);
			return UrlEncodeToBytes(bytes, 0, bytes.Length);
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			if (bytes.Length == 0)
			{
				return new byte[0];
			}
			return UrlEncodeToBytes(bytes, 0, bytes.Length);
		}

		public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
		{
			if (bytes == null)
			{
				return null;
			}
			return HttpEncoder.UrlEncodeToBytes(bytes, offset, count);
		}

		public static string UrlEncodeUnicode(string str)
		{
			if (str == null)
			{
				return null;
			}
			return Encoding.ASCII.GetString(UrlEncodeUnicodeToBytes(str));
		}

		public static byte[] UrlEncodeUnicodeToBytes(string str)
		{
			if (str == null)
			{
				return null;
			}
			if (str.Length == 0)
			{
				return new byte[0];
			}
			MemoryStream memoryStream = new MemoryStream(str.Length);
			foreach (char c in str)
			{
				HttpEncoder.UrlEncodeChar(c, memoryStream, isUnicode: true);
			}
			return memoryStream.ToArray();
		}

		public static string HtmlDecode(string s)
		{
			return HttpEncoder.HtmlDecode(s);
		}

		public static void HtmlDecode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new NullReferenceException(".NET emulation");
			}
			if (!string.IsNullOrEmpty(s))
			{
				output.Write(HttpEncoder.HtmlDecode(s));
			}
		}

		public static string HtmlEncode(string s)
		{
			return HttpEncoder.HtmlEncode(s);
		}

		public static void HtmlEncode(string s, TextWriter output)
		{
			if (output == null)
			{
				throw new NullReferenceException(".NET emulation");
			}
			if (!string.IsNullOrEmpty(s))
			{
				output.Write(HttpEncoder.HtmlEncode(s));
			}
		}

		public static string UrlPathEncode(string s)
		{
			return HttpEncoder.UrlPathEncode(s);
		}

		public static NameValueCollection ParseQueryString(string query)
		{
			return ParseQueryString(query, Encoding.UTF8);
		}

		public static NameValueCollection ParseQueryString(string query, Encoding encoding)
		{
			if (query == null)
			{
				throw new ArgumentNullException("query");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
			{
				return new NameValueCollection();
			}
			if (query[0] == '?')
			{
				query = query.Substring(1);
			}
			NameValueCollection result = new HttpQSCollection();
			ParseQueryString(query, encoding, result);
			return result;
		}

		internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
		{
			if (query.Length == 0)
			{
				return;
			}
			string text = HtmlDecode(query);
			int length = text.Length;
			int num = 0;
			bool flag = true;
			while (num <= length)
			{
				int num2 = -1;
				int num3 = -1;
				for (int i = num; i < length; i++)
				{
					if (num2 == -1 && text[i] == '=')
					{
						num2 = i + 1;
					}
					else if (text[i] == '&')
					{
						num3 = i;
						break;
					}
				}
				if (flag)
				{
					flag = false;
					if (text[num] == '?')
					{
						num++;
					}
				}
				string name;
				if (num2 == -1)
				{
					name = null;
					num2 = num;
				}
				else
				{
					name = UrlDecode(text.Substring(num, num2 - num - 1), encoding);
				}
				if (num3 < 0)
				{
					num = -1;
					num3 = text.Length;
				}
				else
				{
					num = num3 + 1;
				}
				string value = UrlDecode(text.Substring(num2, num3 - num2), encoding);
				result.Add(name, value);
				if (num == -1)
				{
					break;
				}
			}
		}
	}
}
namespace RestSharp.Extensions
{
	public static class ReflectionExtensions
	{
		public static T GetAttribute<T>(this MemberInfo prop) where T : Attribute
		{
			return Attribute.GetCustomAttribute(prop, typeof(T)) as T;
		}

		public static T GetAttribute<T>(this Type type) where T : Attribute
		{
			return Attribute.GetCustomAttribute(type, typeof(T)) as T;
		}

		public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
		{
			while (toCheck != typeof(object))
			{
				Type type = (toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck);
				if (generic == type)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}

		public static object ChangeType(this object source, Type newType)
		{
			return Convert.ChangeType(source, newType);
		}

		public static object ChangeType(this object source, Type newType, CultureInfo culture)
		{
			return Convert.ChangeType(source, newType, culture);
		}

		public static object FindEnumValue(this Type type, string value, CultureInfo culture)
		{
			Enum @enum = Enum.GetValues(type).Cast<Enum>().FirstOrDefault((Enum v) => v.ToString().GetNameVariants(culture).Contains<string>(value, StringComparer.Create(culture, ignoreCase: true)));
			if (@enum == null && int.TryParse(value, out var result) && Enum.IsDefined(type, result))
			{
				@enum = (Enum)Enum.ToObject(type, result);
			}
			return @enum;
		}
	}
	public static class ResponseExtensions
	{
		public static IRestResponse<T> toAsyncResponse<T>(this IRestResponse response)
		{
			RestResponse<T> restResponse = new RestResponse<T>();
			restResponse.ContentEncoding = response.ContentEncoding;
			restResponse.ContentLength = response.ContentLength;
			restResponse.ContentType = response.ContentType;
			restResponse.Cookies = response.Cookies;
			restResponse.ErrorMessage = response.ErrorMessage;
			restResponse.Headers = response.Headers;
			restResponse.RawBytes = response.RawBytes;
			restResponse.ResponseStatus = response.ResponseStatus;
			restResponse.ResponseUri = response.ResponseUri;
			restResponse.Server = response.Server;
			restResponse.StatusCode = response.StatusCode;
			restResponse.StatusDescription = response.StatusDescription;
			return restResponse;
		}
	}
	public static class StringExtensions
	{
		public static string UrlDecode(this string input)
		{
			return HttpUtility.UrlDecode(input);
		}

		public static string UrlEncode(this string input)
		{
			return Uri.EscapeDataString(input);
		}

		public static string HtmlDecode(this string input)
		{
			return HttpUtility.HtmlDecode(input);
		}

		public static string HtmlEncode(this string input)
		{
			return HttpUtility.HtmlEncode(input);
		}

		public static string HtmlAttributeEncode(this string input)
		{
			return HttpUtility.HtmlAttributeEncode(input);
		}

		public static bool HasValue(this string input)
		{
			return !string.IsNullOrEmpty(input);
		}

		public static string RemoveUnderscoresAndDashes(this string input)
		{
			return input.Replace("_", "").Replace("-", "");
		}

		public static DateTime ParseJsonDate(this string input, CultureInfo culture)
		{
			input = input.Replace("\n", "");
			input = input.Replace("\r", "");
			input = input.RemoveSurroundingQuotes();
			if (long.TryParse(input, out var result))
			{
				return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(result);
			}
			if (input.Contains("/Date("))
			{
				return ExtractDate(input, "\\\\?/Date\\((-?\\d+)(-|\\+)?([0-9]{4})?\\)\\\\?/", culture);
			}
			if (input.Contains("new Date("))
			{
				input = input.Replace(" ", "");
				return ExtractDate(input, "newDate\\((-?\\d+)*\\)", culture);
			}
			return ParseFormattedDate(input, culture);
		}

		public static string RemoveSurroundingQuotes(this string input)
		{
			if (input.StartsWith("\"") && input.EndsWith("\""))
			{
				input = input.Substring(1, input.Length - 2);
			}
			return input;
		}

		private static DateTime ParseFormattedDate(string input, CultureInfo culture)
		{
			string[] formats = new string[8] { "u", "s", "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", "yyyy-MM-ddTHH:mm:ssZ", "yyyy-MM-dd HH:mm:ssZ", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:sszzzzzz", "M/d/yyyy h:mm:ss tt" };
			if (DateTime.TryParseExact(input, formats, culture, DateTimeStyles.None, out var result))
			{
				return result;
			}
			if (DateTime.TryParse(input, culture, DateTimeStyles.None, out result))
			{
				return result;
			}
			return default(DateTime);
		}

		private static DateTime ExtractDate(string input, string pattern, CultureInfo culture)
		{
			DateTime result = DateTime.MinValue;
			Regex regex = new Regex(pattern);
			if (regex.IsMatch(input))
			{
				MatchCollection matchCollection = regex.Matches(input);
				Match match = matchCollection[0];
				long num = Convert.ToInt64(match.Groups[1].Value);
				result = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(num);
				if (match.Groups.Count > 2 && !string.IsNullOrEmpty(match.Groups[3].Value))
				{
					DateTime dateTime = DateTime.ParseExact(match.Groups[3].Value, "HHmm", culture);
					result = ((!(match.Groups[2].Value == "+")) ? result.Subtract(dateTime.TimeOfDay) : result.Add(dateTime.TimeOfDay));
				}
			}
			return result;
		}

		public static bool Matches(this string input, string pattern)
		{
			return Regex.IsMatch(input, pattern);
		}

		public static string ToPascalCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
		{
			return lowercaseAndUnderscoredWord.ToPascalCase(removeUnderscores: true, culture);
		}

		public static string ToPascalCase(this string text, bool removeUnderscores, CultureInfo culture)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			text = text.Replace("_", " ");
			string separator = (removeUnderscores ? string.Empty : "_");
			string[] array = text.Split(new char[1] { ' ' });
			if (array.Length > 1 || array[0].IsUpperCase())
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Length > 0)
					{
						string text2 = array[i];
						string text3 = text2.Substring(1);
						if (text3.IsUpperCase())
						{
							text3 = text3.ToLower(culture);
						}
						char c = char.ToUpper(text2[0], culture);
						array[i] = c + text3;
					}
				}
				return string.Join(separator, array);
			}
			return array[0].Substring(0, 1).ToUpper(culture) + array[0].Substring(1);
		}

		public static string ToCamelCase(this string lowercaseAndUnderscoredWord, CultureInfo culture)
		{
			return lowercaseAndUnderscoredWord.ToPascalCase(culture).MakeInitialLowerCase();
		}

		public static string MakeInitialLowerCase(this string word)
		{
			return word.Substring(0, 1).ToLower() + word.Substring(1);
		}

		public static bool IsUpperCase(this string inputString)
		{
			return Regex.IsMatch(inputString, "^[A-Z]+$");
		}

		public static string AddUnderscores(this string pascalCasedWord)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, "([A-Z]+)([A-Z][a-z])", "$1_$2"), "([a-z\\d])([A-Z])", "$1_$2"), "[-\\s]", "_");
		}

		public static string AddDashes(this string pascalCasedWord)
		{
			return Regex.Replace(Regex.Replace(Regex.Replace(pascalCasedWord, "([A-Z]+)([A-Z][a-z])", "$1-$2"), "([a-z\\d])([A-Z])", "$1-$2"), "[\\s]", "-");
		}

		public static string AddUnderscorePrefix(this string pascalCasedWord)
		{
			return $"_{pascalCasedWord}";
		}

		public static IEnumerable<string> GetNameVariants(this string name, CultureInfo culture)
		{
			if (!string.IsNullOrEmpty(name))
			{
				yield return name;
				yield return name.ToCamelCase(culture);
				yield return name.ToLower(culture);
				yield return name.AddUnderscores();
				yield return name.AddUnderscores().ToLower(culture);
				yield return name.AddDashes();
				yield return name.AddDashes().ToLower(culture);
				yield return name.AddUnderscorePrefix();
				yield return name.ToCamelCase(culture).AddUnderscorePrefix();
			}
		}
	}
	public static class XmlExtensions
	{
		public static XName AsNamespaced(this string name, string @namespace)
		{
			XName result = XName.op_Implicit(name);
			if (@namespace.HasValue())
			{
				result = XName.Get(name, @namespace);
			}
			return result;
		}
	}
}
namespace RestSharp
{
	public class FileParameter
	{
		public long ContentLength { get; set; }

		public Action<Stream> Writer { get; set; }

		public string FileName { get; set; }

		public string ContentType { get; set; }

		public string Name { get; set; }

		public static FileParameter Create(string name, byte[] data, string filename, string contentType)
		{
			long longLength = data.LongLength;
			FileParameter fileParameter = new FileParameter();
			fileParameter.Writer = delegate(Stream s)
			{
				s.Write(data, 0, data.Length);
			};
			fileParameter.FileName = filename;
			fileParameter.ContentType = contentType;
			fileParameter.ContentLength = longLength;
			fileParameter.Name = name;
			return fileParameter;
		}

		public static FileParameter Create(string name, byte[] data, string filename)
		{
			return Create(name, data, filename, null);
		}
	}
	public interface IHttp
	{
		CookieContainer CookieContainer { get; set; }

		ICredentials Credentials { get; set; }

		string UserAgent { get; set; }

		int Timeout { get; set; }

		bool FollowRedirects { get; set; }

		X509CertificateCollection ClientCertificates { get; set; }

		int? MaxRedirects { get; set; }

		IList<HttpHeader> Headers { get; }

		IList<HttpParameter> Parameters { get; }

		IList<HttpFile> Files { get; }

		IList<HttpCookie> Cookies { get; }

		string RequestBody { get; set; }

		string RequestContentType { get; set; }

		Uri Url { get; set; }

		IWebProxy Proxy { get; set; }

		HttpWebRequest DeleteAsync(Action<HttpResponse> action);

		HttpWebRequest GetAsync(Action<HttpResponse> action);

		HttpWebRequest HeadAsync(Action<HttpResponse> action);

		HttpWebRequest OptionsAsync(Action<HttpResponse> action);

		HttpWebRequest PostAsync(Action<HttpResponse> action);

		HttpWebRequest PutAsync(Action<HttpResponse> action);

		HttpWebRequest PatchAsync(Action<HttpResponse> action);

		HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod);

		HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod);

		HttpResponse Delete();

		HttpResponse Get();

		HttpResponse Head();

		HttpResponse Options();

		HttpResponse Post();

		HttpResponse Put();

		HttpResponse Patch();

		HttpResponse AsPost(string httpMethod);

		HttpResponse AsGet(string httpMethod);
	}
	public interface IHttpFactory
	{
		IHttp Create();
	}
	public class Http : IHttp, IHttpFactory
	{
		private class TimeOutState
		{
			public bool TimedOut { get; set; }

			public HttpWebRequest Request { get; set; }
		}

		private const string _lineBreak = "\r\n";

		private const string FormBoundary = "-----------------------------28947758029299";

		private TimeOutState _timeoutState;

		private static readonly Encoding _defaultEncoding = Encoding.UTF8;

		private readonly IDictionary<string, Action<HttpWebRequest, string>> _restrictedHeaderActions;

		protected bool HasParameters => Parameters.Any();

		protected bool HasCookies => Cookies.Any();

		protected bool HasBody => !string.IsNullOrEmpty(RequestBody);

		protected bool HasFiles => Files.Any();

		public string UserAgent { get; set; }

		public int Timeout { get; set; }

		public ICredentials Credentials { get; set; }

		public CookieContainer CookieContainer { get; set; }

		public IList<HttpFile> Files { get; private set; }

		public bool FollowRedirects { get; set; }

		public X509CertificateCollection ClientCertificates { get; set; }

		public int? MaxRedirects { get; set; }

		public IList<HttpHeader> Headers { get; private set; }

		public IList<HttpParameter> Parameters { get; private set; }

		public IList<HttpCookie> Cookies { get; private set; }

		public string RequestBody { get; set; }

		public string RequestContentType { get; set; }

		public Uri Url { get; set; }

		public IWebProxy Proxy { get; set; }

		public HttpWebRequest DeleteAsync(Action<HttpResponse> action)
		{
			return GetStyleMethodInternalAsync("DELETE", action);
		}

		public HttpWebRequest GetAsync(Action<HttpResponse> action)
		{
			return GetStyleMethodInternalAsync("GET", action);
		}

		public HttpWebRequest HeadAsync(Action<HttpResponse> action)
		{
			return GetStyleMethodInternalAsync("HEAD", action);
		}

		public HttpWebRequest OptionsAsync(Action<HttpResponse> action)
		{
			return GetStyleMethodInternalAsync("OPTIONS", action);
		}

		public HttpWebRequest PostAsync(Action<HttpResponse> action)
		{
			return PutPostInternalAsync("POST", action);
		}

		public HttpWebRequest PutAsync(Action<HttpResponse> action)
		{
			return PutPostInternalAsync("PUT", action);
		}

		public HttpWebRequest PatchAsync(Action<HttpResponse> action)
		{
			return PutPostInternalAsync("PATCH", action);
		}

		public HttpWebRequest AsPostAsync(Action<HttpResponse> action, string httpMethod)
		{
			return PutPostInternalAsync(httpMethod.ToUpperInvariant(), action);
		}

		public HttpWebRequest AsGetAsync(Action<HttpResponse> action, string httpMethod)
		{
			return GetStyleMethodInternalAsync(httpMethod.ToUpperInvariant(), action);
		}

		private HttpWebRequest GetStyleMethodInternalAsync(string method, Action<HttpResponse> callback)
		{
			HttpWebRequest httpWebRequest = null;
			try
			{
				Uri url = Url;
				httpWebRequest = ConfigureAsyncWebRequest(method, url);
				_timeoutState = new TimeOutState
				{
					Request = httpWebRequest
				};
				IAsyncResult asyncResult = httpWebRequest.BeginGetResponse(delegate(IAsyncResult result)
				{
					ResponseCallback(result, callback);
				}, httpWebRequest);
				SetTimeout(asyncResult, _timeoutState);
			}
			catch (Exception ex)
			{
				HttpResponse httpResponse = new HttpResponse();
				httpResponse.ErrorMessage = ex.Message;
				httpResponse.ErrorException = ex;
				httpResponse.ResponseStatus = ResponseStatus.Error;
				ExecuteCallback(httpResponse, callback);
			}
			return httpWebRequest;
		}

		private HttpWebRequest PutPostInternalAsync(string method, Action<HttpResponse> callback)
		{
			HttpWebRequest httpWebRequest = null;
			try
			{
				httpWebRequest = ConfigureAsyncWebRequest(method, Url);
				PreparePostBody(httpWebRequest);
				WriteRequestBodyAsync(httpWebRequest, callback);
			}
			catch (Exception ex)
			{
				HttpResponse httpResponse = new HttpResponse();
				httpResponse.ErrorMessage = ex.Message;
				httpResponse.ErrorException = ex;
				httpResponse.ResponseStatus = ResponseStatus.Error;
				ExecuteCallback(httpResponse, callback);
			}
			return httpWebRequest;
		}

		private void WriteRequestBodyAsync(HttpWebRequest webRequest, Action<HttpResponse> callback)
		{
			_timeoutState = new TimeOutState
			{
				Request = webRequest
			};
			IAsyncResult asyncResult;
			if (HasBody || HasFiles)
			{
				webRequest.ContentLength = CalculateContentLength();
				asyncResult = webRequest.BeginGetRequestStream(delegate(IAsyncResult result)
				{
					RequestStreamCallback(result, callback);
				}, webRequest);
			}
			else
			{
				asyncResult = webRequest.BeginGetResponse(delegate(IAsyncResult r)
				{
					ResponseCallback(r, callback);
				}, webRequest);
			}
			SetTimeout(asyncResult, _timeoutState);
		}

		private long CalculateContentLength()
		{
			if (!HasFiles)
			{
				return _defaultEncoding.GetByteCount(RequestBody);
			}
			long num = 0L;
			foreach (HttpFile file in Files)
			{
				num += _defaultEncoding.GetByteCount(GetMultipartFileHeader(file));
				num += file.ContentLength;
				num += _defaultEncoding.GetByteCount("\r\n");
			}
			foreach (HttpParameter parameter in Parameters)
			{
				num += _defaultEncoding.GetByteCount(GetMultipartFormData(parameter));
			}
			return num + _defaultEncoding.GetByteCount(GetMultipartFooter());
		}

		private void RequestStreamCallback(IAsyncResult result, Action<HttpResponse> callback)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)result.AsyncState;
			if (_timeoutState.TimedOut)
			{
				HttpResponse httpResponse = new HttpResponse();
				httpResponse.ResponseStatus = ResponseStatus.TimedOut;
				HttpResponse response = httpResponse;
				ExecuteCallback(response, callback);
				return;
			}
			try
			{
				using Stream stream = httpWebRequest.EndGetRequestStream(result);
				if (HasFiles)
				{
					WriteMultipartFormData(stream);
				}
				else
				{
					WriteStringTo(stream, RequestBody);
				}
			}
			catch (Exception ex)
			{
				if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.RequestCanceled)
				{
					HttpResponse httpResponse2 = new HttpResponse();
					httpResponse2.ResponseStatus = ResponseStatus.TimedOut;
					HttpResponse response2 = httpResponse2;
					ExecuteCallback(response2, callback);
				}
				else
				{
					HttpResponse httpResponse3 = new HttpResponse();
					httpResponse3.ErrorMessage = ex.Message;
					httpResponse3.ErrorException = ex;
					httpResponse3.ResponseStatus = ResponseStatus.Error;
					HttpResponse response2 = httpResponse3;
					ExecuteCallback(response2, callback);
				}
				return;
			}
			httpWebRequest.BeginGetResponse(delegate(IAsyncResult r)
			{
				ResponseCallback(r, callback);
			}, httpWebRequest);
		}

		private void SetTimeout(IAsyncResult asyncResult, TimeOutState timeOutState)
		{
			if (Timeout != 0)
			{
				ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, TimeoutCallback, timeOutState, Timeout, executeOnlyOnce: true);
			}
		}

		private static void TimeoutCallback(object state, bool timedOut)
		{
			if (timedOut && state is TimeOutState timeOutState)
			{
				lock (timeOutState)
				{
					timeOutState.TimedOut = true;
				}
				if (timeOutState.Request != null)
				{
					timeOutState.Request.Abort();
				}
			}
		}

		private static void GetRawResponseAsync(IAsyncResult result, Action<HttpWebResponse> callback)
		{
			HttpResponse httpResponse = new HttpResponse();
			httpResponse.ResponseStatus = ResponseStatus.None;
			HttpWebResponse httpWebResponse = null;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)result.AsyncState;
				httpWebResponse = httpWebRequest.EndGetResponse(result) as HttpWebResponse;
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.RequestCanceled)
				{
					throw ex;
				}
				if (!(ex.Response is HttpWebResponse))
				{
					throw ex;
				}
				httpWebResponse = ex.Response as HttpWebResponse;
			}
			callback(httpWebResponse);
			httpWebResponse.Close();
		}

		private void ResponseCallback(IAsyncResult result, Action<HttpResponse> callback)
		{
			HttpResponse response = new HttpResponse
			{
				ResponseStatus = ResponseStatus.None
			};
			try
			{
				if (_timeoutState.TimedOut)
				{
					response.ResponseStatus = ResponseStatus.TimedOut;
					ExecuteCallback(response, callback);
					return;
				}
				GetRawResponseAsync(result, delegate(HttpWebResponse webResponse)
				{
					ExtractResponseData(response, webResponse);
					ExecuteCallback(response, callback);
				});
			}
			catch (Exception ex)
			{
				if (ex is WebException && ((WebException)ex).Status == WebExceptionStatus.RequestCanceled)
				{
					response.ResponseStatus = ResponseStatus.Aborted;
					ExecuteCallback(response, callback);
					return;
				}
				response.ErrorMessage = ex.Message;
				response.ErrorException = ex;
				response.ResponseStatus = ResponseStatus.Error;
				ExecuteCallback(response, callback);
			}
		}

		private static void ExecuteCallback(HttpResponse response, Action<HttpResponse> callback)
		{
			callback(response);
		}

		private void AddAsyncHeaderActions()
		{
		}

		private HttpWebRequest ConfigureAsyncWebRequest(string method, Uri url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.UseDefaultCredentials = false;
			AppendHeaders(httpWebRequest);
			AppendCookies(httpWebRequest);
			httpWebRequest.Method = method;
			if (!HasFiles)
			{
				httpWebRequest.ContentLength = 0L;
			}
			if (Credentials != null)
			{
				httpWebRequest.Credentials = Credentials;
			}
			if (UserAgent.HasValue())
			{
				httpWebRequest.UserAgent = UserAgent;
			}
			if (ClientCertificates != null)
			{
				httpWebRequest.ClientCertificates = ClientCertificates;
			}
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			ServicePointManager.Expect100Continue = false;
			if (Timeout != 0)
			{
				httpWebRequest.Timeout = Timeout;
			}
			if (Proxy != null)
			{
				httpWebRequest.Proxy = Proxy;
			}
			if (FollowRedirects && MaxRedirects.HasValue)
			{
				httpWebRequest.MaximumAutomaticRedirections = MaxRedirects.Value;
			}
			httpWebRequest.AllowAutoRedirect = FollowRedirects;
			return httpWebRequest;
		}

		public IHttp Create()
		{
			return new Http();
		}

		public Http()
		{
			Headers = new List<HttpHeader>();
			Files = new List<HttpFile>();
			Parameters = new List<HttpParameter>();
			Cookies = new List<HttpCookie>();
			_restrictedHeaderActions = new Dictionary<string, Action<HttpWebRequest, string>>(StringComparer.OrdinalIgnoreCase);
			AddSharedHeaderActions();
			AddSyncHeaderActions();
		}

		private void AddSharedHeaderActions()
		{
			_restrictedHeaderActions.Add("Accept", delegate(HttpWebRequest r, string v)
			{
				r.Accept = v;
			});
			_restrictedHeaderActions.Add("Content-Type", delegate(HttpWebRequest r, string v)
			{
				r.ContentType = v;
			});
			_restrictedHeaderActions.Add("Date", delegate(HttpWebRequest r, string v)
			{
				if (DateTime.TryParse(v, out var result))
				{
					r.Date = result;
				}
			});
			_restrictedHeaderActions.Add("Host", delegate(HttpWebRequest r, string v)
			{
				r.Host = v;
			});
			_restrictedHeaderActions.Add("Range", delegate(HttpWebRequest r, string v)
			{
				AddRange(r, v);
			});
		}

		private static string GetMultipartFormContentType()
		{
			return string.Format("multipart/form-data; boundary={0}", "-----------------------------28947758029299");
		}

		private static string GetMultipartFileHeader(HttpFile file)
		{
			return string.Format("--{0}{4}Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"{4}Content-Type: {3}{4}{4}", "-----------------------------28947758029299", file.Name, file.FileName, file.ContentType ?? "application/octet-stream", "\r\n");
		}

		private static string GetMultipartFormData(HttpParameter param)
		{
			return string.Format("--{0}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}", "-----------------------------28947758029299", param.Name, param.Value, "\r\n");
		}

		private static string GetMultipartFooter()
		{
			return string.Format("--{0}--{1}", "-----------------------------28947758029299", "\r\n");
		}

		private void AppendHeaders(HttpWebRequest webRequest)
		{
			foreach (HttpHeader header in Headers)
			{
				if (_restrictedHeaderActions.ContainsKey(header.Name))
				{
					_restrictedHeaderActions[header.Name](webRequest, header.Value);
				}
				else
				{
					webRequest.Headers.Add(header.Name, header.Value);
				}
			}
		}

		private void AppendCookies(HttpWebRequest webRequest)
		{
			webRequest.CookieContainer = CookieContainer ?? new CookieContainer();
			foreach (HttpCookie cookie3 in Cookies)
			{
				Cookie cookie = new Cookie();
				cookie.Name = cookie3.Name;
				cookie.Value = cookie3.Value;
				cookie.Domain = webRequest.RequestUri.Host;
				Cookie cookie2 = cookie;
				webRequest.CookieContainer.Add(cookie2);
			}
		}

		private string EncodeParameters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (HttpParameter parameter in Parameters)
			{
				if (stringBuilder.Length > 1)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.AppendFormat("{0}={1}", RestSharp.Extensions.StringExtensions.UrlEncode(parameter.Name), RestSharp.Extensions.StringExtensions.UrlEncode(parameter.Value));
			}
			return stringBuilder.ToString();
		}

		private void PreparePostBody(HttpWebRequest webRequest)
		{
			if (HasFiles)
			{
				webRequest.ContentType = GetMultipartFormContentType();
			}
			else if (HasParameters)
			{
				webRequest.ContentType = "application/x-www-form-urlencoded";
				RequestBody = EncodeParameters();
			}
			else if (HasBody)
			{
				webRequest.ContentType = RequestContentType;
			}
		}

		private static void WriteStringTo(Stream stream, string toWrite)
		{
			byte[] bytes = _defaultEncoding.GetBytes(toWrite);
			stream.Write(bytes, 0, bytes.Length);
		}

		private void WriteMultipartFormData(Stream requestStream)
		{
			foreach (HttpParameter parameter in Parameters)
			{
				WriteStringTo(requestStream, GetMultipartFormData(parameter));
			}
			foreach (HttpFile file in Files)
			{
				WriteStringTo(requestStream, GetMultipartFileHeader(file));
				file.Writer(requestStream);
				WriteStringTo(requestStream, "\r\n");
			}
			WriteStringTo(requestStream, GetMultipartFooter());
		}

		private static void ExtractResponseData(HttpResponse response, HttpWebResponse webResponse)
		{
			using (webResponse)
			{
				response.ContentEncoding = webResponse.ContentEncoding;
				response.Server = webResponse.Server;
				response.ContentType = webResponse.ContentType;
				response.ContentLength = webResponse.ContentLength;
				response.RawBytes = webResponse.GetResponseStream().ReadAsBytes();
				response.StatusCode = webResponse.StatusCode;
				response.StatusDescription = webResponse.StatusDescription;
				response.ResponseUri = webResponse.ResponseUri;
				response.ResponseStatus = ResponseStatus.Completed;
				if (webResponse.Cookies != null)
				{
					foreach (Cookie cookie in webResponse.Cookies)
					{
						response.Cookies.Add(new HttpCookie
						{
							Comment = cookie.Comment,
							CommentUri = cookie.CommentUri,
							Discard = cookie.Discard,
							Domain = cookie.Domain,
							Expired = cookie.Expired,
							Expires = cookie.Expires,
							HttpOnly = cookie.HttpOnly,
							Name = cookie.Name,
							Path = cookie.Path,
							Port = cookie.Port,
							Secure = cookie.Secure,
							TimeStamp = cookie.TimeStamp,
							Value = cookie.Value,
							Version = cookie.Version
						});
					}
				}
				string[] allKeys = webResponse.Headers.AllKeys;
				foreach (string name in allKeys)
				{
					string value = webResponse.Headers[name];
					response.Headers.Add(new HttpHeader
					{
						Name = name,
						Value = value
					});
				}
				webResponse.Close();
			}
		}

		private void AddRange(HttpWebRequest r, string range)
		{
			Match match = Regex.Match(range, "=(\\d+)-(\\d+)$");
			if (match.Success)
			{
				int from = Convert.ToInt32(match.Groups[1].Value);
				int to = Convert.ToInt32(match.Groups[2].Value);
				r.AddRange(from, to);
			}
		}

		public HttpResponse Post()
		{
			return PostPutInternal("POST");
		}

		public HttpResponse Put()
		{
			return PostPutInternal("PUT");
		}

		public HttpResponse Get()
		{
			return GetStyleMethodInternal("GET");
		}

		public HttpResponse Head()
		{
			return GetStyleMethodInternal("HEAD");
		}

		public HttpResponse Options()
		{
			return GetStyleMethodInternal("OPTIONS");
		}

		public HttpResponse Delete()
		{
			return GetStyleMethodInternal("DELETE");
		}

		public HttpResponse Patch()
		{
			return PostPutInternal("PATCH");
		}

		public HttpResponse AsGet(string httpMethod)
		{
			return GetStyleMethodInternal(httpMethod.ToUpperInvariant());
		}

		public HttpResponse AsPost(string httpMethod)
		{
			return PostPutInternal(httpMethod.ToUpperInvariant());
		}

		private HttpResponse GetStyleMethodInternal(string method)
		{
			HttpWebRequest request = ConfigureWebRequest(method, Url);
			return GetResponse(request);
		}

		private HttpResponse PostPutInternal(string method)
		{
			HttpWebRequest httpWebRequest = ConfigureWebRequest(method, Url);
			PreparePostData(httpWebRequest);
			WriteRequestBody(httpWebRequest);
			return GetResponse(httpWebRequest);
		}

		private void AddSyncHeaderActions()
		{
			_restrictedHeaderActions.Add("Connection", delegate(HttpWebRequest r, string v)
			{
				r.Connection = v;
			});
			_restrictedHeaderActions.Add("Content-Length", delegate(HttpWebRequest r, string v)
			{
				r.ContentLength = Convert.ToInt64(v);
			});
			_restrictedHeaderActions.Add("Expect", delegate(HttpWebRequest r, string v)
			{
				r.Expect = v;
			});
			_restrictedHeaderActions.Add("If-Modified-Since", delegate(HttpWebRequest r, string v)
			{
				r.IfModifiedSince = Convert.ToDateTime(v);
			});
			_restrictedHeaderActions.Add("Referer", delegate(HttpWebRequest r, string v)
			{
				r.Referer = v;
			});
			_restrictedHeaderActions.Add("Transfer-Encoding", delegate(HttpWebRequest r, string v)
			{
				r.TransferEncoding = v;
				r.SendChunked = true;
			});
			_restrictedHeaderActions.Add("User-Agent", delegate(HttpWebRequest r, string v)
			{
				r.UserAgent = v;
			});
		}

		private HttpResponse GetResponse(HttpWebRequest request)
		{
			HttpResponse httpResponse = new HttpResponse();
			httpResponse.ResponseStatus = ResponseStatus.None;
			try
			{
				HttpWebResponse rawResponse = GetRawResponse(request);
				ExtractResponseData(httpResponse, rawResponse);
			}
			catch (Exception ex)
			{
				httpResponse.ErrorMessage = ex.Message;
				httpResponse.ErrorException = ex;
				httpResponse.ResponseStatus = ResponseStatus.Error;
			}
			return httpResponse;
		}

		private static HttpWebResponse GetRawResponse(HttpWebRequest request)
		{
			try
			{
				return (HttpWebResponse)request.GetResponse();
			}
			catch (WebException ex)
			{
				if (ex.Response is HttpWebResponse)
				{
					return ex.Response as HttpWebResponse;
				}
				throw;
			}
		}

		private void PreparePostData(HttpWebRequest webRequest)
		{
			if (HasFiles)
			{
				webRequest.ContentType = GetMultipartFormContentType();
				using Stream requestStream = webRequest.GetRequestStream();
				WriteMultipartFormData(requestStream);
			}
			PreparePostBody(webRequest);
		}

		private void WriteRequestBody(HttpWebRequest webRequest)
		{
			if (!HasBody)
			{
				return;
			}
			byte[] bytes = _defaultEncoding.GetBytes(RequestBody);
			webRequest.ContentLength = bytes.Length;
			using Stream stream = webRequest.GetRequestStream();
			stream.Write(bytes, 0, bytes.Length);
		}

		private HttpWebRequest ConfigureWebRequest(string method, Uri url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.UseDefaultCredentials = false;
			ServicePointManager.Expect100Continue = false;
			AppendHeaders(httpWebRequest);
			AppendCookies(httpWebRequest);
			httpWebRequest.Method = method;
			if (!HasFiles)
			{
				httpWebRequest.ContentLength = 0L;
			}
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			if (ClientCertificates != null)
			{
				httpWebRequest.ClientCertificates = ClientCertificates;
			}
			if (UserAgent.HasValue())
			{
				httpWebRequest.UserAgent = UserAgent;
			}
			if (Timeout != 0)
			{
				httpWebRequest.Timeout = Timeout;
			}
			if (Credentials != null)
			{
				httpWebRequest.Credentials = Credentials;
			}
			if (Proxy != null)
			{
				httpWebRequest.Proxy = Proxy;
			}
			httpWebRequest.AllowAutoRedirect = FollowRedirects;
			if (FollowRedirects && MaxRedirects.HasValue)
			{
				httpWebRequest.MaximumAutomaticRedirections = MaxRedirects.Value;
			}
			return httpWebRequest;
		}
	}
	public class HttpCookie
	{
		public string Comment { get; set; }

		public Uri CommentUri { get; set; }

		public bool Discard { get; set; }

		public string Domain { get; set; }

		public bool Expired { get; set; }

		public DateTime Expires { get; set; }

		public bool HttpOnly { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }

		public string Port { get; set; }

		public bool Secure { get; set; }

		public DateTime TimeStamp { get; set; }

		public string Value { get; set; }

		public int Version { get; set; }
	}
	public class HttpFile
	{
		public long ContentLength { get; set; }

		public Action<Stream> Writer { get; set; }

		public string FileName { get; set; }

		public string ContentType { get; set; }

		public string Name { get; set; }
	}
	public class HttpHeader
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
	public class HttpParameter
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
	public interface IHttpResponse
	{
		string ContentType { get; set; }

		long ContentLength { get; set; }

		string ContentEncoding { get; set; }

		string Content { get; }

		HttpStatusCode StatusCode { get; set; }

		string StatusDescription { get; set; }

		byte[] RawBytes { get; set; }

		Uri ResponseUri { get; set; }

		string Server { get; set; }

		IList<HttpHeader> Headers { get; }

		IList<HttpCookie> Cookies { get; }

		ResponseStatus ResponseStatus { get; set; }

		string ErrorMessage { get; set; }

		Exception ErrorException { get; set; }
	}
	public class HttpResponse : IHttpResponse
	{
		private string _content;

		private ResponseStatus _responseStatus;

		public string ContentType { get; set; }

		public long ContentLength { get; set; }

		public string ContentEncoding { get; set; }

		public string Content
		{
			get
			{
				if (_content == null)
				{
					_content = RawBytes.AsString();
				}
				return _content;
			}
		}

		public HttpStatusCode StatusCode { get; set; }

		public string StatusDescription { get; set; }

		public byte[] RawBytes { get; set; }

		public Uri ResponseUri { get; set; }

		public string Server { get; set; }

		public IList<HttpHeader> Headers { get; private set; }

		public IList<HttpCookie> Cookies { get; private set; }

		public ResponseStatus ResponseStatus
		{
			get
			{
				return _responseStatus;
			}
			set
			{
				_responseStatus = value;
			}
		}

		public string ErrorMessage { get; set; }

		public Exception ErrorException { get; set; }

		public HttpResponse()
		{
			Headers = new List<HttpHeader>();
			Cookies = new List<HttpCookie>();
		}
	}
	public class SimpleFactory<T> : IHttpFactory where T : IHttp, new()
	{
		public IHttp Create()
		{
			return new T();
		}
	}
	public interface IRestClient
	{
		CookieContainer CookieContainer { get; set; }

		string UserAgent { get; set; }

		int Timeout { get; set; }

		bool UseSynchronizationContext { get; set; }

		IAuthenticator Authenticator { get; set; }

		string BaseUrl { get; set; }

		IList<Parameter> DefaultParameters { get; }

		X509CertificateCollection ClientCertificates { get; set; }

		IWebProxy Proxy { get; set; }

		RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback);

		RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback);

		IRestResponse Execute(IRestRequest request);

		IRestResponse<T> Execute<T>(IRestRequest request) where T : new();

		Uri BuildUri(IRestRequest request);

		RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod);

		RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod);

		RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod);

		RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod);

		IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod);

		IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod);

		IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new();

		IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new();
	}
	public interface IRestRequest
	{
		ISerializer JsonSerializer { get; set; }

		ISerializer XmlSerializer { get; set; }

		List<Parameter> Parameters { get; }

		List<FileParameter> Files { get; }

		Method Method { get; set; }

		string Resource { get; set; }

		DataFormat RequestFormat { get; set; }

		string RootElement { get; set; }

		string DateFormat { get; set; }

		string XmlNamespace { get; set; }

		ICredentials Credentials { get; set; }

		int Timeout { get; set; }

		int Attempts { get; }

		Action<IRestResponse> OnBeforeDeserialization { get; set; }

		IRestRequest AddFile(string name, string path);

		IRestRequest AddFile(string name, byte[] bytes, string fileName);

		IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType);

		IRestRequest AddBody(object obj, string xmlNamespace);

		IRestRequest AddBody(object obj);

		IRestRequest AddObject(object obj, params string[] whitelist);

		IRestRequest AddObject(object obj);

		IRestRequest AddParameter(Parameter p);

		IRestRequest AddParameter(string name, object value);

		IRestRequest AddParameter(string name, object value, ParameterType type);

		IRestRequest AddHeader(string name, string value);

		IRestRequest AddCookie(string name, string value);

		IRestRequest AddUrlSegment(string name, string value);

		void IncreaseNumAttempts();
	}
	public interface IRestResponse
	{
		IRestRequest Request { get; set; }

		string ContentType { get; set; }

		long ContentLength { get; set; }

		string ContentEncoding { get; set; }

		string Content { get; set; }

		HttpStatusCode StatusCode { get; set; }

		string StatusDescription { get; set; }

		byte[] RawBytes { get; set; }

		Uri ResponseUri { get; set; }

		string Server { get; set; }

		IList<RestResponseCookie> Cookies { get; }

		IList<Parameter> Headers { get; }

		ResponseStatus ResponseStatus { get; set; }

		string ErrorMessage { get; set; }

		Exception ErrorException { get; set; }
	}
	public interface IRestResponse<T> : IRestResponse
	{
		T Data { get; set; }
	}
	public class Parameter
	{
		public string Name { get; set; }

		public object Value { get; set; }

		public ParameterType Type { get; set; }

		public override string ToString()
		{
			return $"{Name}={Value}";
		}
	}
	public class RestClient : IRestClient
	{
		private static readonly Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;

		public IHttpFactory HttpFactory = new SimpleFactory<Http>();

		private string _baseUrl;

		private IDictionary<string, IDeserializer> ContentHandlers { get; set; }

		private IList<string> AcceptTypes { get; set; }

		public IList<Parameter> DefaultParameters { get; private set; }

		public int? MaxRedirects { get; set; }

		public X509CertificateCollection ClientCertificates { get; set; }

		public bool FollowRedirects { get; set; }

		public CookieContainer CookieContainer { get; set; }

		public string UserAgent { get; set; }

		public int Timeout { get; set; }

		public bool UseSynchronizationContext { get; set; }

		public IAuthenticator Authenticator { get; set; }

		public virtual string BaseUrl
		{
			get
			{
				return _baseUrl;
			}
			set
			{
				_baseUrl = value;
				if (_baseUrl != null && _baseUrl.EndsWith("/"))
				{
					_baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);
				}
			}
		}

		public IWebProxy Proxy { get; set; }

		public virtual RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			string name = Enum.GetName(typeof(Method), request.Method);
			switch (request.Method)
			{
			case Method.POST:
			case Method.PUT:
			case Method.PATCH:
				return ExecuteAsync(request, callback, name, DoAsGetAsync);
			default:
				return ExecuteAsync(request, callback, name, DoAsPostAsync);
			}
		}

		public virtual RestRequestAsyncHandle ExecuteAsyncGet(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
		{
			return ExecuteAsync(request, callback, httpMethod, DoAsPostAsync);
		}

		public virtual RestRequestAsyncHandle ExecuteAsyncPost(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod)
		{
			request.Method = Method.POST;
			return ExecuteAsync(request, callback, httpMethod, DoAsGetAsync);
		}

		private RestRequestAsyncHandle ExecuteAsync(IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback, string httpMethod, Func<IHttp, Action<HttpResponse>, string, HttpWebRequest> getWebRequest)
		{
			IHttp http = HttpFactory.Create();
			AuthenticateIfNeeded(this, request);
			string value = string.Join(", ", AcceptTypes.ToArray());
			this.AddDefaultParameter("Accept", value, ParameterType.HttpHeader);
			ConfigureHttp(request, http);
			RestRequestAsyncHandle asyncHandle = new RestRequestAsyncHandle();
			Action<HttpResponse> action = delegate(HttpResponse r)
			{
				ProcessResponse(request, r, asyncHandle, callback);
			};
			if (UseSynchronizationContext && SynchronizationContext.Current != null)
			{
				SynchronizationContext ctx = SynchronizationContext.Current;
				Action<HttpResponse> cb = action;
				action = delegate(HttpResponse resp)
				{
					ctx.Post(delegate
					{
						cb(resp);
					}, null);
				};
			}
			asyncHandle.WebRequest = getWebRequest(http, action, httpMethod);
			return asyncHandle;
		}

		private static HttpWebRequest DoAsGetAsync(IHttp http, Action<HttpResponse> response_cb, string method)
		{
			return http.AsGetAsync(response_cb, method);
		}

		private static HttpWebRequest DoAsPostAsync(IHttp http, Action<HttpResponse> response_cb, string method)
		{
			return http.AsPostAsync(response_cb, method);
		}

		private void ProcessResponse(IRestRequest request, HttpResponse httpResponse, RestRequestAsyncHandle asyncHandle, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			RestResponse arg = ConvertToRestResponse(request, httpResponse);
			callback(arg, asyncHandle);
		}

		public virtual RestRequestAsyncHandle ExecuteAsync<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback)
		{
			return ExecuteAsync(request, delegate(IRestResponse response, RestRequestAsyncHandle asyncHandle)
			{
				DeserializeResponse(request, callback, response, asyncHandle);
			});
		}

		public virtual RestRequestAsyncHandle ExecuteAsyncGet<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
		{
			return ExecuteAsyncGet(request, delegate(IRestResponse response, RestRequestAsyncHandle asyncHandle)
			{
				DeserializeResponse(request, callback, response, asyncHandle);
			}, httpMethod);
		}

		public virtual RestRequestAsyncHandle ExecuteAsyncPost<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, string httpMethod)
		{
			return ExecuteAsyncPost(request, delegate(IRestResponse response, RestRequestAsyncHandle asyncHandle)
			{
				DeserializeResponse(request, callback, response, asyncHandle);
			}, httpMethod);
		}

		private void DeserializeResponse<T>(IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback, IRestResponse response, RestRequestAsyncHandle asyncHandle)
		{
			IRestResponse<T> arg = response as RestResponse<T>;
			if (response.ResponseStatus != ResponseStatus.Aborted)
			{
				arg = Deserialize<T>(request, response);
			}
			callback(arg, asyncHandle);
		}

		public RestClient()
		{
			ContentHandlers = new Dictionary<string, IDeserializer>();
			AcceptTypes = new List<string>();
			DefaultParameters = new List<Parameter>();
			AddHandler("application/json", new JsonDeserializer());
			AddHandler("application/xml", new XmlDeserializer());
			AddHandler("text/json", new JsonDeserializer());
			AddHandler("text/x-json", new JsonDeserializer());
			AddHandler("text/javascript", new JsonDeserializer());
			AddHandler("text/xml", new XmlDeserializer());
			AddHandler("*", new XmlDeserializer());
			FollowRedirects = true;
		}

		public RestClient(string baseUrl)
			: this()
		{
			BaseUrl = baseUrl;
		}

		public void AddHandler(string contentType, IDeserializer deserializer)
		{
			ContentHandlers[contentType] = deserializer;
			if (contentType != "*")
			{
				AcceptTypes.Add(contentType);
			}
		}

		public void RemoveHandler(string contentType)
		{
			ContentHandlers.Remove(contentType);
			AcceptTypes.Remove(contentType);
		}

		public void ClearHandlers()
		{
			ContentHandlers.Clear();
			AcceptTypes.Clear();
		}

		private IDeserializer GetHandler(string contentType)
		{
			if (string.IsNullOrEmpty(contentType) && ContentHandlers.ContainsKey("*"))
			{
				return ContentHandlers["*"];
			}
			int num = contentType.IndexOf(';');
			if (num > -1)
			{
				contentType = contentType.Substring(0, num);
			}
			IDeserializer result = null;
			if (ContentHandlers.ContainsKey(contentType))
			{
				result = ContentHandlers[contentType];
			}
			else if (ContentHandlers.ContainsKey("*"))
			{
				result = ContentHandlers["*"];
			}
			return result;
		}

		private void AuthenticateIfNeeded(RestClient client, IRestRequest request)
		{
			if (Authenticator != null)
			{
				Authenticator.Authenticate(client, request);
			}
		}

		public Uri BuildUri(IRestRequest request)
		{
			string text = request.Resource;
			IEnumerable<Parameter> enumerable = request.Parameters.Where((Parameter p) => p.Type == ParameterType.UrlSegment);
			foreach (Parameter item in enumerable)
			{
				text = text.Replace("{" + item.Name + "}", RestSharp.Extensions.StringExtensions.UrlEncode(item.Value.ToString()));
			}
			if (!string.IsNullOrEmpty(text) && text.StartsWith("/"))
			{
				text = text.Substring(1);
			}
			if (!string.IsNullOrEmpty(BaseUrl))
			{
				text = ((!string.IsNullOrEmpty(text)) ? $"{BaseUrl}/{text}" : BaseUrl);
			}
			if (request.Method != Method.POST && request.Method != Method.PUT && request.Method != Method.PATCH && request.Parameters.Any((Parameter p) => p.Type == ParameterType.GetOrPost))
			{
				if (text.EndsWith("/"))
				{
					text = text.Substring(0, text.Length - 1);
				}
				string arg = EncodeParameters(request);
				text = $"{text}?{arg}";
			}
			return new Uri(text);
		}

		private string EncodeParameters(IRestRequest request)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Parameter item in request.Parameters.Where((Parameter p) => p.Type == ParameterType.GetOrPost))
			{
				if (stringBuilder.Length > 1)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.AppendFormat("{0}={1}", RestSharp.Extensions.StringExtensions.UrlEncode(item.Name), RestSharp.Extensions.StringExtensions.UrlEncode(item.Value.ToString()));
			}
			return stringBuilder.ToString();
		}

		private void ConfigureHttp(IRestRequest request, IHttp http)
		{
			http.CookieContainer = CookieContainer;
			Parameter p3;
			foreach (Parameter defaultParameter in DefaultParameters)
			{
				p3 = defaultParameter;
				if (!request.Parameters.Any((Parameter p2) => p2.Name == p3.Name && p2.Type == p3.Type))
				{
					request.AddParameter(p3);
				}
			}
			http.Url = BuildUri(request);
			string text = UserAgent ?? http.UserAgent;
			http.UserAgent = (text.HasValue() ? text : ("RestSharp " + version.ToString()));
			int num = ((request.Timeout > 0) ? request.Timeout : Timeout);
			if (num > 0)
			{
				http.Timeout = num;
			}
			http.FollowRedirects = FollowRedirects;
			if (ClientCertificates != null)
			{
				http.ClientCertificates = ClientCertificates;
			}
			http.MaxRedirects = MaxRedirects;
			if (request.Credentials != null)
			{
				http.Credentials = request.Credentials;
			}
			IEnumerable<HttpHeader> enumerable = from p in request.Parameters
				where p.Type == ParameterType.HttpHeader
				select new HttpHeader
				{
					Name = p.Name,
					Value = p.Value.ToString()
				};
			foreach (HttpHeader item in enumerable)
			{
				http.Headers.Add(item);
			}
			IEnumerable<HttpCookie> enumerable2 = from p in request.Parameters
				where p.Type == ParameterType.Cookie
				select new HttpCookie
				{
					Name = p.Name,
					Value = p.Value.ToString()
				};
			foreach (HttpCookie item2 in enumerable2)
			{
				http.Cookies.Add(item2);
			}
			IEnumerable<HttpParameter> enumerable3 = from p in request.Parameters
				where p.Type == ParameterType.GetOrPost && p.Value != null
				select new HttpParameter
				{
					Name = p.Name,
					Value = p.Value.ToString()
				};
			foreach (HttpParameter item3 in enumerable3)
			{
				http.Parameters.Add(item3);
			}
			foreach (FileParameter file in request.Files)
			{
				http.Files.Add(new HttpFile
				{
					Name = file.Name,
					ContentType = file.ContentType,
					Writer = file.Writer,
					FileName = file.FileName,
					ContentLength = file.ContentLength
				});
			}
			Parameter parameter = request.Parameters.Where((Parameter p) => p.Type == ParameterType.RequestBody).FirstOrDefault();
			if (parameter != null)
			{
				http.RequestBody = parameter.Value.ToString();
				http.RequestContentType = parameter.Name;
			}
		}

		private RestResponse ConvertToRestResponse(IRestRequest request, HttpResponse httpResponse)
		{
			RestResponse restResponse = new RestResponse();
			restResponse.Content = httpResponse.Content;
			restResponse.ContentEncoding = httpResponse.ContentEncoding;
			restResponse.ContentLength = httpResponse.ContentLength;
			restResponse.ContentType = httpResponse.ContentType;
			restResponse.ErrorException = httpResponse.ErrorException;
			restResponse.ErrorMessage = httpResponse.ErrorMessage;
			restResponse.RawBytes = httpResponse.RawBytes;
			restResponse.ResponseStatus = httpResponse.ResponseStatus;
			restResponse.ResponseUri = httpResponse.ResponseUri;
			restResponse.Server = httpResponse.Server;
			restResponse.StatusCode = httpResponse.StatusCode;
			restResponse.StatusDescription = httpResponse.StatusDescription;
			restResponse.Request = request;
			foreach (HttpHeader header in httpResponse.Headers)
			{
				restResponse.Headers.Add(new Parameter
				{
					Name = header.Name,
					Value = header.Value,
					Type = ParameterType.HttpHeader
				});
			}
			foreach (HttpCookie cookie in httpResponse.Cookies)
			{
				restResponse.Cookies.Add(new RestResponseCookie
				{
					Comment = cookie.Comment,
					CommentUri = cookie.CommentUri,
					Discard = cookie.Discard,
					Domain = cookie.Domain,
					Expired = cookie.Expired,
					Expires = cookie.Expires,
					HttpOnly = cookie.HttpOnly,
					Name = cookie.Name,
					Path = cookie.Path,
					Port = cookie.Port,
					Secure = cookie.Secure,
					TimeStamp = cookie.TimeStamp,
					Value = cookie.Value,
					Version = cookie.Version
				});
			}
			return restResponse;
		}

		private IRestResponse<T> Deserialize<T>(IRestRequest request, IRestResponse raw)
		{
			request.OnBeforeDeserialization(raw);
			IDeserializer handler = GetHandler(raw.ContentType);
			handler.RootElement = request.RootElement;
			handler.DateFormat = request.DateFormat;
			handler.Namespace = request.XmlNamespace;
			IRestResponse<T> restResponse = new RestResponse<T>();
			try
			{
				restResponse = raw.toAsyncResponse<T>();
				restResponse.Data = handler.Deserialize<T>(raw);
				restResponse.Request = request;
			}
			catch (Exception ex)
			{
				restResponse.ResponseStatus = ResponseStatus.Error;
				restResponse.ErrorMessage = ex.Message;
				restResponse.ErrorException = ex;
			}
			return restResponse;
		}

		public byte[] DownloadData(IRestRequest request)
		{
			IRestResponse restResponse = Execute(request);
			return restResponse.RawBytes;
		}

		public virtual IRestResponse Execute(IRestRequest request)
		{
			string name = Enum.GetName(typeof(Method), request.Method);
			switch (request.Method)
			{
			case Method.POST:
			case Method.PUT:
			case Method.PATCH:
				return Execute(request, name, DoExecuteAsPost);
			default:
				return Execute(request, name, DoExecuteAsGet);
			}
		}

		public IRestResponse ExecuteAsGet(IRestRequest request, string httpMethod)
		{
			return Execute(request, httpMethod, DoExecuteAsGet);
		}

		public IRestResponse ExecuteAsPost(IRestRequest request, string httpMethod)
		{
			request.Method = Method.POST;
			return Execute(request, httpMethod, DoExecuteAsPost);
		}

		private IRestResponse Execute(IRestRequest request, string httpMethod, Func<IHttp, string, HttpResponse> getResponse)
		{
			AuthenticateIfNeeded(this, request);
			string value = string.Join(", ", AcceptTypes.ToArray());
			this.AddDefaultParameter("Accept", value, ParameterType.HttpHeader);
			IRestResponse restResponse = new RestResponse();
			try
			{
				IHttp http = HttpFactory.Create();
				ConfigureHttp(request, http);
				ConfigureProxy(http);
				restResponse = ConvertToRestResponse(request, getResponse(http, httpMethod));
				restResponse.Request = request;
				restResponse.Request.IncreaseNumAttempts();
			}
			catch (Exception ex)
			{
				restResponse.ResponseStatus = ResponseStatus.Error;
				restResponse.ErrorMessage = ex.Message;
				restResponse.ErrorException = ex;
			}
			return restResponse;
		}

		private static HttpResponse DoExecuteAsGet(IHttp http, string method)
		{
			return http.AsGet(method);
		}

		private static HttpResponse DoExecuteAsPost(IHttp http, string method)
		{
			return http.AsPost(method);
		}

		public virtual IRestResponse<T> Execute<T>(IRestRequest request) where T : new()
		{
			return Deserialize<T>(request, Execute(request));
		}

		public IRestResponse<T> ExecuteAsGet<T>(IRestRequest request, string httpMethod) where T : new()
		{
			return Deserialize<T>(request, ExecuteAsGet(request, httpMethod));
		}

		public IRestResponse<T> ExecuteAsPost<T>(IRestRequest request, string httpMethod) where T : new()
		{
			return Deserialize<T>(request, ExecuteAsPost(request, httpMethod));
		}

		private void ConfigureProxy(IHttp http)
		{
			if (Proxy != null)
			{
				http.Proxy = Proxy;
			}
		}
	}
	public static class RestClientExtensions
	{
		public static RestRequestAsyncHandle ExecuteAsync(this IRestClient client, IRestRequest request, Action<IRestResponse> callback)
		{
			return client.ExecuteAsync(request, delegate(IRestResponse response, RestRequestAsyncHandle handle)
			{
				callback(response);
			});
		}

		public static RestRequestAsyncHandle ExecuteAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>> callback) where T : new()
		{
			return client.ExecuteAsync(request, delegate(IRestResponse<T> response, RestRequestAsyncHandle asyncHandle)
			{
				callback(response);
			});
		}

		public static RestRequestAsyncHandle GetAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.GET;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PostAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.POST;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PutAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.PUT;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle HeadAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.HEAD;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle OptionsAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.OPTIONS;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PatchAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.PATCH;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle DeleteAsync<T>(this IRestClient client, IRestRequest request, Action<IRestResponse<T>, RestRequestAsyncHandle> callback) where T : new()
		{
			request.Method = Method.DELETE;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle GetAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.GET;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PostAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.POST;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PutAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.PUT;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle HeadAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.HEAD;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle OptionsAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.OPTIONS;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle PatchAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.PATCH;
			return client.ExecuteAsync(request, callback);
		}

		public static RestRequestAsyncHandle DeleteAsync(this IRestClient client, IRestRequest request, Action<IRestResponse, RestRequestAsyncHandle> callback)
		{
			request.Method = Method.DELETE;
			return client.ExecuteAsync(request, callback);
		}

		public static IRestResponse<T> Get<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.GET;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Post<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.POST;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Put<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.PUT;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Head<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.HEAD;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Options<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.OPTIONS;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Patch<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.PATCH;
			return client.Execute<T>(request);
		}

		public static IRestResponse<T> Delete<T>(this IRestClient client, IRestRequest request) where T : new()
		{
			request.Method = Method.DELETE;
			return client.Execute<T>(request);
		}

		public static IRestResponse Get(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.GET;
			return client.Execute(request);
		}

		public static IRestResponse Post(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.POST;
			return client.Execute(request);
		}

		public static IRestResponse Put(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.PUT;
			return client.Execute(request);
		}

		public static IRestResponse Head(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.HEAD;
			return client.Execute(request);
		}

		public static IRestResponse Options(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.OPTIONS;
			return client.Execute(request);
		}

		public static IRestResponse Patch(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.PATCH;
			return client.Execute(request);
		}

		public static IRestResponse Delete(this IRestClient client, IRestRequest request)
		{
			request.Method = Method.DELETE;
			return client.Execute(request);
		}

		public static void AddDefaultParameter(this IRestClient restClient, Parameter p)
		{
			if (p.Type == ParameterType.RequestBody)
			{
				throw new NotSupportedException("Cannot set request body from default headers. Use Request.AddBody() instead.");
			}
			restClient.DefaultParameters.Add(p);
		}

		public static void AddDefaultParameter(this IRestClient restClient, string name, object value)
		{
			restClient.AddDefaultParameter(new Parameter
			{
				Name = name,
				Value = value,
				Type = ParameterType.GetOrPost
			});
		}

		public static void AddDefaultParameter(this IRestClient restClient, string name, object value, ParameterType type)
		{
			restClient.AddDefaultParameter(new Parameter
			{
				Name = name,
				Value = value,
				Type = type
			});
		}

		public static void AddDefaultHeader(this IRestClient restClient, string name, string value)
		{
			restClient.AddDefaultParameter(name, value, ParameterType.HttpHeader);
		}

		public static void AddDefaultUrlSegment(this IRestClient restClient, string name, string value)
		{
			restClient.AddDefaultParameter(name, value, ParameterType.UrlSegment);
		}

		public static RestResponse<dynamic> ExecuteDynamic(this IRestClient client, IRestRequest request)
		{
			IRestResponse restResponse = client.Execute(request);
			RestResponse<object> restResponse2 = (RestResponse<object>)restResponse;
			object data = SimpleJson.DeserializeObject(restResponse.Content);
			restResponse2.Data = data;
			return restResponse2;
		}
	}
	public class RestRequest : IRestRequest
	{
		private Method _method;

		private DataFormat _requestFormat = DataFormat.Xml;

		private int _attempts;

		public ISerializer JsonSerializer { get; set; }

		public ISerializer XmlSerializer { get; set; }

		public List<Parameter> Parameters { get; private set; }

		public List<FileParameter> Files { get; private set; }

		public Method Method
		{
			get
			{
				return _method;
			}
			set
			{
				_method = value;
			}
		}

		public string Resource { get; set; }

		public DataFormat RequestFormat
		{
			get
			{
				return _requestFormat;
			}
			set
			{
				_requestFormat = value;
			}
		}

		public string RootElement { get; set; }

		public Action<IRestResponse> OnBeforeDeserialization { get; set; }

		public string DateFormat { get; set; }

		public string XmlNamespace { get; set; }

		public ICredentials Credentials { get; set; }

		public object UserState { get; set; }

		public int Timeout { get; set; }

		public int Attempts => _attempts;

		public RestRequest()
		{
			Parameters = new List<Parameter>();
			Files = new List<FileParameter>();
			XmlSerializer = new RestSharp.Serializers.XmlSerializer();
			JsonSerializer = new JsonSerializer();
			OnBeforeDeserialization = delegate
			{
			};
		}

		public RestRequest(Method method)
			: this()
		{
			Method = method;
		}

		public RestRequest(string resource)
			: this(resource, Method.GET)
		{
		}

		public RestRequest(string resource, Method method)
			: this()
		{
			Resource = resource;
			Method = method;
		}

		public RestRequest(Uri resource)
			: this(resource, Method.GET)
		{
		}

		public RestRequest(Uri resource, Method method)
			: this(resource.IsAbsoluteUri ? (resource.AbsolutePath + resource.Query) : resource.OriginalString, method)
		{
		}

		public IRestRequest AddFile(string name, string path)
		{
			return AddFile(new FileParameter
			{
				Name = name,
				FileName = Path.GetFileName(path),
				Writer = delegate(Stream s)
				{
					using StreamReader streamReader = new StreamReader(path);
					streamReader.BaseStream.CopyTo(s);
				}
			});
		}

		public IRestRequest AddFile(string name, byte[] bytes, string fileName)
		{
			return AddFile(FileParameter.Create(name, bytes, fileName));
		}

		public IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType)
		{
			return AddFile(FileParameter.Create(name, bytes, fileName, contentType));
		}

		public IRestRequest AddFile(string name, Action<Stream> writer, string fileName)
		{
			return AddFile(name, writer, fileName, null);
		}

		public IRestRequest AddFile(string name, Action<Stream> writer, string fileName, string contentType)
		{
			return AddFile(new FileParameter
			{
				Name = name,
				Writer = writer,
				FileName = fileName,
				ContentType = contentType
			});
		}

		private IRestRequest AddFile(FileParameter file)
		{
			Files.Add(file);
			return this;
		}

		public IRestRequest AddBody(object obj, string xmlNamespace)
		{
			string value;
			string name;
			switch (RequestFormat)
			{
			case DataFormat.Json:
				value = JsonSerializer.Serialize(obj);
				name = JsonSerializer.ContentType;
				break;
			case DataFormat.Xml:
				XmlSerializer.Namespace = xmlNamespace;
				value = XmlSerializer.Serialize(obj);
				name = XmlSerializer.ContentType;
				break;
			default:
				value = "";
				name = "";
				break;
			}
			return AddParameter(name, value, ParameterType.RequestBody);
		}

		public IRestRequest AddBody(object obj)
		{
			return AddBody(obj, "");
		}

		public IRestRequest AddObject(object obj, params string[] whitelist)
		{
			Type type = obj.GetType();
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			foreach (PropertyInfo propertyInfo in array)
			{
				if (whitelist.Length != 0 && (whitelist.Length <= 0 || !whitelist.Contains(propertyInfo.Name)))
				{
					continue;
				}
				Type propertyType = propertyInfo.PropertyType;
				object obj2 = propertyInfo.GetValue(obj, null);
				if (obj2 != null)
				{
					if (propertyType.IsArray)
					{
						obj2 = string.Join(",", (string[])obj2);
					}
					AddParameter(propertyInfo.Name, obj2);
				}
			}
			return this;
		}

		public IRestRequest AddObject(object obj)
		{
			AddObject(obj, new string[0]);
			return this;
		}

		public IRestRequest AddParameter(Parameter p)
		{
			Parameters.Add(p);
			return this;
		}

		public IRestRequest AddParameter(string name, object value)
		{
			return AddParameter(new Parameter
			{
				Name = name,
				Value = value,
				Type = ParameterType.GetOrPost
			});
		}

		public IRestRequest AddParameter(string name, object value, ParameterType type)
		{
			return AddParameter(new Parameter
			{
				Name = name,
				Value = value,
				Type = type
			});
		}

		public IRestRequest AddHeader(string name, string value)
		{
			return AddParameter(name, value, ParameterType.HttpHeader);
		}

		public IRestRequest AddCookie(string name, string value)
		{
			return AddParameter(name, value, ParameterType.Cookie);
		}

		public IRestRequest AddUrlSegment(string name, string value)
		{
			return AddParameter(name, value, ParameterType.UrlSegment);
		}

		public void IncreaseNumAttempts()
		{
			_attempts++;
		}
	}
	public class RestRequestAsyncHandle
	{
		public HttpWebRequest WebRequest;

		public RestRequestAsyncHandle()
		{
		}

		public RestRequestAsyncHandle(HttpWebRequest webRequest)
		{
			WebRequest = webRequest;
		}

		public void Abort()
		{
			if (WebRequest != null)
			{
				WebRequest.Abort();
			}
		}
	}
	public abstract class RestResponseBase
	{
		private string _content;

		private ResponseStatus _responseStatus;

		public IRestRequest Request { get; set; }

		public string ContentType { get; set; }

		public long ContentLength { get; set; }

		public string ContentEncoding { get; set; }

		public string Content
		{
			get
			{
				if (_content == null)
				{
					_content = RawBytes.AsString();
				}
				return _content;
			}
			set
			{
				_content = value;
			}
		}

		public HttpStatusCode StatusCode { get; set; }

		public string StatusDescription { get; set; }

		public byte[] RawBytes { get; set; }

		public Uri ResponseUri { get; set; }

		public string Server { get; set; }

		public IList<RestResponseCookie> Cookies { get; protected internal set; }

		public IList<Parameter> Headers { get; protected internal set; }

		public ResponseStatus ResponseStatus
		{
			get
			{
				return _responseStatus;
			}
			set
			{
				_responseStatus = value;
			}
		}

		public string ErrorMessage { get; set; }

		public Exception ErrorException { get; set; }

		public RestResponseBase()
		{
			Headers = new List<Parameter>();
			Cookies = new List<RestResponseCookie>();
		}
	}
	public class RestResponse<T> : RestResponseBase, IRestResponse<T>, IRestResponse
	{
		public T Data { get; set; }

		public static explicit operator RestResponse<T>(RestResponse response)
		{
			RestResponse<T> restResponse = new RestResponse<T>();
			restResponse.ContentEncoding = response.ContentEncoding;
			restResponse.ContentLength = response.ContentLength;
			restResponse.ContentType = response.ContentType;
			restResponse.Cookies = response.Cookies;
			restResponse.ErrorMessage = response.ErrorMessage;
			restResponse.Headers = response.Headers;
			restResponse.RawBytes = response.RawBytes;
			restResponse.ResponseStatus = response.ResponseStatus;
			restResponse.ResponseUri = response.ResponseUri;
			restResponse.Server = response.Server;
			restResponse.StatusCode = response.StatusCode;
			restResponse.StatusDescription = response.StatusDescription;
			restResponse.Request = response.Request;
			return restResponse;
		}
	}
	public class RestResponse : RestResponseBase, IRestResponse
	{
	}
	public class RestResponseCookie
	{
		public string Comment { get; set; }

		public Uri CommentUri { get; set; }

		public bool Discard { get; set; }

		public string Domain { get; set; }

		public bool Expired { get; set; }

		public DateTime Expires { get; set; }

		public bool HttpOnly { get; set; }

		public string Name { get; set; }

		public string Path { get; set; }

		public string Port { get; set; }

		public bool Secure { get; set; }

		public DateTime TimeStamp { get; set; }

		public string Value { get; set; }

		public int Version { get; set; }
	}
}
namespace RestSharp.Serializers
{
	public interface ISerializer
	{
		string RootElement { get; set; }

		string Namespace { get; set; }

		string DateFormat { get; set; }

		string ContentType { get; set; }

		string Serialize(object obj);
	}
	public class DotNetXmlSerializer : ISerializer
	{
		private class EncodingStringWriter : StringWriter
		{
			private readonly Encoding encoding;

			public override Encoding Encoding => encoding;

			public EncodingStringWriter(Encoding encoding)
			{
				this.encoding = encoding;
			}
		}

		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string DateFormat { get; set; }

		public string ContentType { get; set; }

		public Encoding Encoding { get; set; }

		public DotNetXmlSerializer()
		{
			ContentType = "application/xml";
			Encoding = Encoding.UTF8;
		}

		public DotNetXmlSerializer(string @namespace)
			: this()
		{
			Namespace = @namespace;
		}

		public string Serialize(object obj)
		{
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add(string.Empty, Namespace);
			System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
			EncodingStringWriter encodingStringWriter = new EncodingStringWriter(Encoding);
			xmlSerializer.Serialize(encodingStringWriter, obj, xmlSerializerNamespaces);
			return encodingStringWriter.ToString();
		}
	}
	public class JsonSerializer : ISerializer
	{
		public string DateFormat { get; set; }

		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string ContentType { get; set; }

		public JsonSerializer()
		{
			ContentType = "application/json";
		}

		public string Serialize(object obj)
		{
			return SimpleJson.SerializeObject(obj);
		}
	}
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class SerializeAsAttribute : Attribute
	{
		public string Name { get; set; }

		public bool Attribute { get; set; }

		public CultureInfo Culture { get; set; }

		public NameStyle NameStyle { get; set; }

		public int Index { get; set; }

		public SerializeAsAttribute()
		{
			NameStyle = NameStyle.AsIs;
			Index = int.MaxValue;
			Culture = CultureInfo.InvariantCulture;
		}

		public string TransformName(string input)
		{
			string text = Name ?? input;
			return NameStyle switch
			{
				NameStyle.CamelCase => text.ToCamelCase(Culture), 
				NameStyle.PascalCase => text.ToPascalCase(Culture), 
				NameStyle.LowerCase => text.ToLower(), 
				_ => input, 
			};
		}
	}
	public enum NameStyle
	{
		AsIs,
		CamelCase,
		LowerCase,
		PascalCase
	}
	public class XmlSerializer : ISerializer
	{
		public string RootElement { get; set; }

		public string Namespace { get; set; }

		public string DateFormat { get; set; }

		public string ContentType { get; set; }

		public XmlSerializer()
		{
			ContentType = "text/xml";
		}

		public XmlSerializer(string @namespace)
		{
			Namespace = @namespace;
			ContentType = "text/xml";
		}

		public string Serialize(object obj)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			XDocument val = new XDocument();
			Type type = obj.GetType();
			string text = type.Name;
			SerializeAsAttribute attribute = type.GetAttribute<SerializeAsAttribute>();
			if (attribute != null)
			{
				text = attribute.TransformName(attribute.Name ?? text);
			}
			XElement val2 = new XElement(text.AsNamespaced(Namespace));
			if (obj is IList)
			{
				string text2 = "";
				foreach (object item in (IList)obj)
				{
					Type type2 = item.GetType();
					SerializeAsAttribute attribute2 = type2.GetAttribute<SerializeAsAttribute>();
					if (attribute2 != null)
					{
						text2 = attribute2.TransformName(attribute2.Name ?? text);
					}
					if (text2 == "")
					{
						text2 = type2.Name;
					}
					XElement val3 = new XElement(XName.op_Implicit(text2));
					Map(val3, item);
					((XContainer)val2).Add((object)val3);
				}
			}
			else
			{
				Map(val2, obj);
			}
			if (RootElement.HasValue())
			{
				XElement val4 = new XElement(RootElement.AsNamespaced(Namespace), (object)val2);
				((XContainer)val).Add((object)val4);
			}
			else
			{
				((XContainer)val).Add((object)val2);
			}
			return ((object)val).ToString();
		}

		private void Map(XElement root, object obj)
		{
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Expected O, but got Unknown
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Expected O, but got Unknown
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Expected O, but got Unknown
			Type type = obj.GetType();
			IEnumerable<PropertyInfo> enumerable = from p in type.GetProperties()
				let indexAttribute = p.GetAttribute<SerializeAsAttribute>()
				where p.CanRead && p.CanWrite
				orderby (indexAttribute != null) ? indexAttribute.Index : int.MaxValue
				select p;
			SerializeAsAttribute attribute = type.GetAttribute<SerializeAsAttribute>();
			foreach (PropertyInfo item in enumerable)
			{
				string text = item.Name;
				object value = item.GetValue(obj, null);
				if (value == null)
				{
					continue;
				}
				string serializedValue = GetSerializedValue(value);
				Type propertyType = item.PropertyType;
				bool flag = false;
				SerializeAsAttribute attribute2 = item.GetAttribute<SerializeAsAttribute>();
				if (attribute2 != null)
				{
					text = (attribute2.Name.HasValue() ? attribute2.Name : text);
					flag = attribute2.Attribute;
				}
				SerializeAsAttribute attribute3 = item.GetAttribute<SerializeAsAttribute>();
				if (attribute3 != null)
				{
					text = attribute3.TransformName(text);
				}
				else if (attribute != null)
				{
					text = attribute.TransformName(text);
				}
				XName val = text.AsNamespaced(Namespace);
				XElement val2 = new XElement(val);
				if (propertyType.IsPrimitive || propertyType.IsValueType || propertyType == typeof(string))
				{
					if (flag)
					{
						((XContainer)root).Add((object)new XAttribute(XName.op_Implicit(text), (object)serializedValue));
						continue;
					}
					val2.Value = serializedValue;
				}
				else if (value is IList)
				{
					string text2 = "";
					foreach (object item2 in (IList)value)
					{
						if (text2 == "")
						{
							text2 = item2.GetType().Name;
						}
						XElement val3 = new XElement(XName.op_Implicit(text2));
						Map(val3, item2);
						((XContainer)val2).Add((object)val3);
					}
				}
				else
				{
					Map(val2, value);
				}
				((XContainer)root).Add((object)val2);
			}
		}

		private string GetSerializedValue(object obj)
		{
			object obj2 = obj;
			if (obj is DateTime && DateFormat.HasValue())
			{
				obj2 = ((DateTime)obj).ToString(DateFormat);
			}
			if (obj is bool)
			{
				obj2 = obj.ToString().ToLower();
			}
			return obj2.ToString();
		}
	}
}
namespace RestSharp
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonArray : List<object>
	{
		public JsonArray()
		{
		}

		public JsonArray(int capacity)
			: base(capacity)
		{
		}

		public override string ToString()
		{
			return SimpleJson.SerializeObject(this) ?? string.Empty;
		}
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class JsonObject : DynamicObject, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		private readonly Dictionary<string, object> _members = new Dictionary<string, object>();

		public object this[int index] => GetAtIndex(_members, index);

		public ICollection<string> Keys => _members.Keys;

		public ICollection<object> Values => _members.Values;

		public object this[string key]
		{
			get
			{
				return _members[key];
			}
			set
			{
				_members[key] = value;
			}
		}

		public int Count => _members.Count;

		public bool IsReadOnly => false;

		internal static object GetAtIndex(IDictionary<string, object> obj, int index)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (index >= obj.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int num = 0;
			foreach (KeyValuePair<string, object> item in obj)
			{
				if (num++ == index)
				{
					return item.Value;
				}
			}
			return null;
		}

		public void Add(string key, object value)
		{
			_members.Add(key, value);
		}

		public bool ContainsKey(string key)
		{
			return _members.ContainsKey(key);
		}

		public bool Remove(string key)
		{
			return _members.Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return _members.TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			_members.Add(item.Key, item.Value);
		}

		public void Clear()
		{
			_members.Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			if (_members.ContainsKey(item.Key))
			{
				return _members[item.Key] == item.Value;
			}
			return false;
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			int num = Count;
			using IEnumerator<KeyValuePair<string, object>> enumerator = GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, object> current = enumerator.Current;
				array[arrayIndex++] = current;
				if (--num <= 0)
				{
					break;
				}
			}
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return _members.Remove(item.Key);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return _members.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _members.GetEnumerator();
		}

		public override string ToString()
		{
			return SimpleJson.SerializeObject(this);
		}

		public override bool TryConvert(ConvertBinder binder, out object result)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			Type type = binder.Type;
			if (type == typeof(IEnumerable) || type == typeof(IEnumerable<KeyValuePair<string, object>>) || type == typeof(IDictionary<string, object>) || type == typeof(IDictionary))
			{
				result = this;
				return true;
			}
			return base.TryConvert(binder, out result);
		}

		public override bool TryDeleteMember(DeleteMemberBinder binder)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			return _members.Remove(binder.Name);
		}

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			if (indexes.Length == 1)
			{
				result = ((IDictionary<string, object>)this)[(string)indexes[0]];
				return true;
			}
			result = null;
			return true;
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (_members.TryGetValue(binder.Name, out var value))
			{
				result = value;
				return true;
			}
			result = null;
			return true;
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			if (indexes.Length == 1)
			{
				((IDictionary<string, object>)this)[(string)indexes[0]] = value;
				return true;
			}
			return base.TrySetIndex(binder, indexes, value);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (binder == null)
			{
				throw new ArgumentNullException("binder");
			}
			_members[binder.Name] = value;
			return true;
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			foreach (string key in Keys)
			{
				yield return key;
			}
		}
	}
	internal class SimpleJson
	{
		private const int TOKEN_NONE = 0;

		private const int TOKEN_CURLY_OPEN = 1;

		private const int TOKEN_CURLY_CLOSE = 2;

		private const int TOKEN_SQUARED_OPEN = 3;

		private const int TOKEN_SQUARED_CLOSE = 4;

		private const int TOKEN_COLON = 5;

		private const int TOKEN_COMMA = 6;

		private const int TOKEN_STRING = 7;

		private const int TOKEN_NUMBER = 8;

		private const int TOKEN_TRUE = 9;

		private const int TOKEN_FALSE = 10;

		private const int TOKEN_NULL = 11;

		private const int BUILDER_CAPACITY = 2000;

		private static IJsonSerializerStrategy currentJsonSerializerStrategy;

		private static PocoJsonSerializerStrategy pocoJsonSerializerStrategy;

		public static IJsonSerializerStrategy CurrentJsonSerializerStrategy
		{
			get
			{
				return currentJsonSerializerStrategy ?? (currentJsonSerializerStrategy = PocoJsonSerializerStrategy);
			}
			set
			{
				currentJsonSerializerStrategy = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static PocoJsonSerializerStrategy PocoJsonSerializerStrategy => pocoJsonSerializerStrategy ?? (pocoJsonSerializerStrategy = new PocoJsonSerializerStrategy());

		public static object DeserializeObject(string json)
		{
			if (TryDeserializeObject(json, out var @object))
			{
				return @object;
			}
			throw new SerializationException("Invalid JSON string");
		}

		public static bool TryDeserializeObject(string json, out object @object)
		{
			bool success = true;
			if (json != null)
			{
				char[] json2 = json.ToCharArray();
				int index = 0;
				@object = ParseValue(json2, ref index, ref success);
			}
			else
			{
				@object = null;
			}
			return success;
		}

		public static object DeserializeObject(string json, Type type, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			object obj = DeserializeObject(json);
			if (!(type == null) && (obj == null || !obj.GetType().IsAssignableFrom(type)))
			{
				return (jsonSerializerStrategy ?? CurrentJsonSerializerStrategy).DeserializeObject(obj, type);
			}
			return obj;
		}

		public static object DeserializeObject(string json, Type type)
		{
			return DeserializeObject(json, type, null);
		}

		public static T DeserializeObject<T>(string json, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			return (T)DeserializeObject(json, typeof(T), jsonSerializerStrategy);
		}

		public static T DeserializeObject<T>(string json)
		{
			return (T)DeserializeObject(json, typeof(T), null);
		}

		public static string SerializeObject(object json, IJsonSerializerStrategy jsonSerializerStrategy)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			if (!SerializeValue(jsonSerializerStrategy, json, stringBuilder))
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		public static string SerializeObject(object json)
		{
			return SerializeObject(json, CurrentJsonSerializerStrategy);
		}

		public static string EscapeToJavascriptString(string jsonString)
		{
			if (string.IsNullOrEmpty(jsonString))
			{
				return jsonString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (num < jsonString.Length)
			{
				char c = jsonString[num++];
				if (c == '\\')
				{
					int num2 = jsonString.Length - num;
					if (num2 >= 2)
					{
						switch (jsonString[num])
						{
						case '\\':
							stringBuilder.Append('\\');
							num++;
							break;
						case '"':
							stringBuilder.Append("\"");
							num++;
							break;
						case 't':
							stringBuilder.Append('\t');
							num++;
							break;
						case 'b':
							stringBuilder.Append('\b');
							num++;
							break;
						case 'n':
							stringBuilder.Append('\n');
							num++;
							break;
						case 'r':
							stringBuilder.Append('\r');
							num++;
							break;
						}
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		protected static IDictionary<string, object> ParseObject(char[] json, ref int index, ref bool success)
		{
			IDictionary<string, object> dictionary = new JsonObject();
			NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (LookAhead(json, index))
				{
				case 0:
					success = false;
					return null;
				case 6:
					NextToken(json, ref index);
					continue;
				case 2:
					NextToken(json, ref index);
					return dictionary;
				}
				string key = ParseString(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				int num = NextToken(json, ref index);
				if (num != 5)
				{
					success = false;
					return null;
				}
				object value = ParseValue(json, ref index, ref success);
				if (!success)
				{
					success = false;
					return null;
				}
				dictionary[key] = value;
			}
			return dictionary;
		}

		protected static JsonArray ParseArray(char[] json, ref int index, ref bool success)
		{
			JsonArray jsonArray = new JsonArray();
			NextToken(json, ref index);
			bool flag = false;
			while (!flag)
			{
				switch (LookAhead(json, index))
				{
				case 0:
					success = false;
					return null;
				case 6:
					NextToken(json, ref index);
					continue;
				case 4:
					break;
				default:
				{
					object item = ParseValue(json, ref index, ref success);
					if (!success)
					{
						return null;
					}
					jsonArray.Add(item);
					continue;
				}
				}
				NextToken(json, ref index);
				break;
			}
			return jsonArray;
		}

		protected static object ParseValue(char[] json, ref int index, ref bool success)
		{
			switch (LookAhead(json, index))
			{
			case 7:
				return ParseString(json, ref index, ref success);
			case 8:
				return ParseNumber(json, ref index, ref success);
			case 1:
				return ParseObject(json, ref index, ref success);
			case 3:
				return ParseArray(json, ref index, ref success);
			case 9:
				NextToken(json, ref index);
				return true;
			case 10:
				NextToken(json, ref index);
				return false;
			case 11:
				NextToken(json, ref index);
				return null;
			default:
				success = false;
				return null;
			}
		}

		protected static string ParseString(char[] json, ref int index, ref bool success)
		{
			StringBuilder stringBuilder = new StringBuilder(2000);
			EatWhitespace(json, ref index);
			char c = json[index++];
			bool flag = false;
			while (!flag && index != json.Length)
			{
				c = json[index++];
				switch (c)
				{
				case '"':
					flag = true;
					break;
				case '\\':
				{
					if (index == json.Length)
					{
						break;
					}
					switch (json[index++])
					{
					case '"':
						stringBuilder.Append('"');
						continue;
					case '\\':
						stringBuilder.Append('\\');
						continue;
					case '/':
						stringBuilder.Append('/');
						continue;
					case 'b':
						stringBuilder.Append('\b');
						continue;
					case 'f':
						stringBuilder.Append('\f');
						continue;
					case 'n':
						stringBuilder.Append('\n');
						continue;
					case 'r':
						stringBuilder.Append('\r');
						continue;
					case 't':
						stringBuilder.Append('\t');
						continue;
					case 'u':
						break;
					default:
						continue;
					}
					int num = json.Length - index;
					if (num < 4)
					{
						break;
					}
					if (!(success = uint.TryParse(new string(json, index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result)))
					{
						return "";
					}
					if (55296 <= result && result <= 56319)
					{
						index += 4;
						num = json.Length - index;
						if (num < 6 || !(new string(json, index, 2) == "\\u") || !uint.TryParse(new string(json, index + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result2) || 56320 > result2 || result2 > 57343)
						{
							success = false;
							return "";
						}
						stringBuilder.Append((char)result);
						stringBuilder.Append((char)result2);
						index += 6;
					}
					else
					{
						stringBuilder.Append(char.ConvertFromUtf32((int)result));
						index += 4;
					}
					continue;
				}
				default:
					stringBuilder.Append(c);
					continue;
				}
				break;
			}
			if (!flag)
			{
				success = false;
				return null;
			}
			return stringBuilder.ToString();
		}

		protected static object ParseNumber(char[] json, ref int index, ref bool success)
		{
			EatWhitespace(json, ref index);
			int lastIndexOfNumber = GetLastIndexOfNumber(json, index);
			int length = lastIndexOfNumber - index + 1;
			string text = new string(json, index, length);
			object result2;
			if (text.IndexOf(".", StringComparison.OrdinalIgnoreCase) != -1 || text.IndexOf("e", StringComparison.OrdinalIgnoreCase) != -1)
			{
				success = double.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out var result);
				result2 = result;
			}
			else
			{
				success = long.TryParse(new string(json, index, length), NumberStyles.Any, CultureInfo.InvariantCulture, out var result3);
				result2 = result3;
			}
			index = lastIndexOfNumber + 1;
			return result2;
		}

		protected static int GetLastIndexOfNumber(char[] json, int index)
		{
			int i;
			for (i = index; i < json.Length && "0123456789+-.eE".IndexOf(json[i]) != -1; i++)
			{
			}
			return i - 1;
		}

		protected static void EatWhitespace(char[] json, ref int index)
		{
			while (index < json.Length && " \t\n\r\b\f".IndexOf(json[index]) != -1)
			{
				index++;
			}
		}

		protected static int LookAhead(char[] json, int index)
		{
			int index2 = index;
			return NextToken(json, ref index2);
		}

		protected static int NextToken(char[] json, ref int index)
		{
			EatWhitespace(json, ref index);
			if (index == json.Length)
			{
				return 0;
			}
			char c = json[index];
			index++;
			switch (c)
			{
			case '{':
				return 1;
			case '}':
				return 2;
			case '[':
				return 3;
			case ']':
				return 4;
			case ',':
				return 6;
			case '"':
				return 7;
			case '-':
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
				return 8;
			case ':':
				return 5;
			default:
			{
				index--;
				int num = json.Length - index;
				if (num >= 5 && json[index] == 'f' && json[index + 1] == 'a' && json[index + 2] == 'l' && json[index + 3] == 's' && json[index + 4] == 'e')
				{
					index += 5;
					return 10;
				}
				if (num >= 4 && json[index] == 't' && json[index + 1] == 'r' && json[index + 2] == 'u' && json[index + 3] == 'e')
				{
					index += 4;
					return 9;
				}
				if (num >= 4 && json[index] == 'n' && json[index + 1] == 'u' && json[index + 2] == 'l' && json[index + 3] == 'l')
				{
					index += 4;
					return 11;
				}
				return 0;
			}
			}
		}

		protected static bool SerializeValue(IJsonSerializerStrategy jsonSerializerStrategy, object value, StringBuilder builder)
		{
			bool flag = true;
			if (value is string)
			{
				flag = SerializeString((string)value, builder);
			}
			else if (value is IDictionary<string, object>)
			{
				IDictionary<string, object> dictionary = (IDictionary<string, object>)value;
				flag = SerializeObject(jsonSerializerStrategy, dictionary.Keys, dictionary.Values, builder);
			}
			else if (value is IDictionary<string, string>)
			{
				IDictionary<string, string> dictionary2 = (IDictionary<string, string>)value;
				flag = SerializeObject(jsonSerializerStrategy, dictionary2.Keys, dictionary2.Values, builder);
			}
			else if (value is IEnumerable)
			{
				flag = SerializeArray(jsonSerializerStrategy, (IEnumerable)value, builder);
			}
			else if (IsNumeric(value))
			{
				flag = SerializeNumber(value, builder);
			}
			else if (value is bool)
			{
				builder.Append(((bool)value) ? "true" : "false");
			}
			else if (value == null)
			{
				builder.Append("null");
			}
			else
			{
				flag = jsonSerializerStrategy.SerializeNonPrimitiveObject(value, out var output);
				if (flag)
				{
					SerializeValue(jsonSerializerStrategy, output, builder);
				}
			}
			return flag;
		}

		protected static bool SerializeObject(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable keys, IEnumerable values, StringBuilder builder)
		{
			builder.Append("{");
			IEnumerator enumerator = keys.GetEnumerator();
			IEnumerator enumerator2 = values.GetEnumerator();
			bool flag = true;
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				object current = enumerator.Current;
				object current2 = enumerator2.Current;
				if (!flag)
				{
					builder.Append(",");
				}
				if (current is string)
				{
					SerializeString((string)current, builder);
				}
				else if (!SerializeValue(jsonSerializerStrategy, current2, builder))
				{
					return false;
				}
				builder.Append(":");
				if (!SerializeValue(jsonSerializerStrategy, current2, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("}");
			return true;
		}

		protected static bool SerializeArray(IJsonSerializerStrategy jsonSerializerStrategy, IEnumerable anArray, StringBuilder builder)
		{
			builder.Append("[");
			bool flag = true;
			foreach (object item in anArray)
			{
				if (!flag)
				{
					builder.Append(",");
				}
				if (!SerializeValue(jsonSerializerStrategy, item, builder))
				{
					return false;
				}
				flag = false;
			}
			builder.Append("]");
			return true;
		}

		protected static bool SerializeString(string aString, StringBuilder builder)
		{
			builder.Append("\"");
			char[] array = aString.ToCharArray();
			foreach (char c in array)
			{
				switch (c)
				{
				case '"':
					builder.Append("\\\"");
					break;
				case '\\':
					builder.Append("\\\\");
					break;
				case '\b':
					builder.Append("\\b");
					break;
				case '\f':
					builder.Append("\\f");
					break;
				case '\n':
					builder.Append("\\n");
					break;
				case '\r':
					builder.Append("\\r");
					break;
				case '\t':
					builder.Append("\\t");
					break;
				default:
					builder.Append(c);
					break;
				}
			}
			builder.Append("\"");
			return true;
		}

		protected static bool SerializeNumber(object number, StringBuilder builder)
		{
			if (number is long)
			{
				builder.Append(((long)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is ulong)
			{
				builder.Append(((ulong)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is int)
			{
				builder.Append(((int)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is uint)
			{
				builder.Append(((uint)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is decimal)
			{
				builder.Append(((decimal)number).ToString(CultureInfo.InvariantCulture));
			}
			else if (number is float)
			{
				builder.Append(((float)number).ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				builder.Append(Convert.ToDouble(number, CultureInfo.InvariantCulture).ToString("r", CultureInfo.InvariantCulture));
			}
			return true;
		}

		protected static bool IsNumeric(object value)
		{
			if (value is sbyte)
			{
				return true;
			}
			if (value is byte)
			{
				return true;
			}
			if (value is short)
			{
				return true;
			}
			if (value is ushort)
			{
				return true;
			}
			if (value is int)
			{
				return true;
			}
			if (value is uint)
			{
				return true;
			}
			if (value is long)
			{
				return true;
			}
			if (value is ulong)
			{
				return true;
			}
			if (value is float)
			{
				return true;
			}
			if (value is double)
			{
				return true;
			}
			if (value is decimal)
			{
				return true;
			}
			return false;
		}
	}
	internal interface IJsonSerializerStrategy
	{
		bool SerializeNonPrimitiveObject(object input, out object output);

		object DeserializeObject(object value, Type type);
	}
	internal class PocoJsonSerializerStrategy : IJsonSerializerStrategy
	{
		internal CacheResolver CacheResolver;

		private static readonly string[] Iso8601Format = new string[3] { "yyyy-MM-dd\\THH:mm:ss.FFFFFFF\\Z", "yyyy-MM-dd\\THH:mm:ss\\Z", "yyyy-MM-dd\\THH:mm:ssK" };

		public PocoJsonSerializerStrategy()
		{
			CacheResolver = new CacheResolver(BuildMap);
		}

		protected virtual void BuildMap(Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps)
		{
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				memberMaps.Add(propertyInfo.Name, new CacheResolver.MemberMap(propertyInfo));
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			foreach (FieldInfo fieldInfo in fields)
			{
				memberMaps.Add(fieldInfo.Name, new CacheResolver.MemberMap(fieldInfo));
			}
		}

		public virtual bool SerializeNonPrimitiveObject(object input, out object output)
		{
			if (!TrySerializeKnownTypes(input, out output))
			{
				return TrySerializeUnknownTypes(input, out output);
			}
			return true;
		}

		public virtual object DeserializeObject(object value, Type type)
		{
			object obj = null;
			if (value is string)
			{
				string text = value as string;
				obj = ((string.IsNullOrEmpty(text) || (!(type == typeof(DateTime)) && (!ReflectionUtils.IsNullableType(type) || !(Nullable.GetUnderlyingType(type) == typeof(DateTime))))) ? text : ((object)DateTime.ParseExact(text, Iso8601Format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal)));
			}
			else if (value is bool)
			{
				obj = value;
			}
			else if (value == null)
			{
				obj = null;
			}
			else if ((value is long && type == typeof(long)) || (value is double && type == typeof(double)))
			{
				obj = value;
			}
			else
			{
				if ((!(value is double) || !(type != typeof(double))) && (!(value is long) || !(type != typeof(long))))
				{
					if (value is IDictionary<string, object>)
					{
						IDictionary<string, object> dictionary = (IDictionary<string, object>)value;
						if (ReflectionUtils.IsTypeDictionary(type))
						{
							Type type2 = type.GetGenericArguments()[0];
							Type type3 = type.GetGenericArguments()[1];
							Type type4 = typeof(Dictionary<, >).MakeGenericType(type2, type3);
							IDictionary dictionary2 = (IDictionary)CacheResolver.GetNewInstance(type4);
							foreach (KeyValuePair<string, object> item in dictionary)
							{
								dictionary2.Add(item.Key, DeserializeObject(item.Value, type3));
							}
							obj = dictionary2;
						}
						else
						{
							obj = CacheResolver.GetNewInstance(type);
							SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = CacheResolver.LoadMaps(type);
							if (safeDictionary == null)
							{
								obj = value;
							}
							else
							{
								foreach (KeyValuePair<string, CacheResolver.MemberMap> item2 in safeDictionary)
								{
									CacheResolver.MemberMap value2 = item2.Value;
									if (value2.Setter != null)
									{
										string key = item2.Key;
										if (dictionary.ContainsKey(key))
										{
											object value3 = DeserializeObject(dictionary[key], value2.Type);
											value2.Setter(obj, value3);
										}
									}
								}
							}
						}
					}
					else if (value is IList<object>)
					{
						IList<object> list = (IList<object>)value;
						IList list2 = null;
						if (type.IsArray)
						{
							list2 = (IList)Activator.CreateInstance(type, list.Count);
							int num = 0;
							foreach (object item3 in list)
							{
								list2[num++] = DeserializeObject(item3, type.GetElementType());
							}
						}
						else if (ReflectionUtils.IsTypeGenericeCollectionInterface(type) || typeof(IList).IsAssignableFrom(type))
						{
							Type type5 = type.GetGenericArguments()[0];
							Type type6 = typeof(List<>).MakeGenericType(type5);
							list2 = (IList)CacheResolver.GetNewInstance(type6);
							foreach (object item4 in list)
							{
								list2.Add(DeserializeObject(item4, type5));
							}
						}
						obj = list2;
					}
					return obj;
				}
				obj = (typeof(IConvertible).IsAssignableFrom(type) ? Convert.ChangeType(value, type, CultureInfo.InvariantCulture) : value);
			}
			if (ReflectionUtils.IsNullableType(type))
			{
				return ReflectionUtils.ToNullableType(obj, type);
			}
			return obj;
		}

		protected virtual object SerializeEnum(Enum p)
		{
			return Convert.ToDouble(p, CultureInfo.InvariantCulture);
		}

		protected virtual bool TrySerializeKnownTypes(object input, out object output)
		{
			bool result = true;
			if (input is DateTime)
			{
				output = ((DateTime)input).ToUniversalTime().ToString(Iso8601Format[0], CultureInfo.InvariantCulture);
			}
			else if (input is Guid)
			{
				output = ((Guid)input).ToString("D");
			}
			else if (input is Uri)
			{
				output = input.ToString();
			}
			else if (input is Enum)
			{
				output = SerializeEnum((Enum)input);
			}
			else
			{
				result = false;
				output = null;
			}
			return result;
		}

		protected virtual bool TrySerializeUnknownTypes(object input, out object output)
		{
			output = null;
			Type type = input.GetType();
			if (type.FullName == null)
			{
				return false;
			}
			IDictionary<string, object> dictionary = new JsonObject();
			SafeDictionary<string, CacheResolver.MemberMap> safeDictionary = CacheResolver.LoadMaps(type);
			foreach (KeyValuePair<string, CacheResolver.MemberMap> item in safeDictionary)
			{
				if (item.Value.Getter != null)
				{
					dictionary.Add(item.Key, item.Value.Getter(input));
				}
			}
			output = dictionary;
			return true;
		}
	}
}
namespace RestSharp.Reflection
{
	internal class ReflectionUtils
	{
		public static Attribute GetAttribute(MemberInfo info, Type type)
		{
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(info, type);
		}

		public static Attribute GetAttribute(Type objectType, Type attributeType)
		{
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				return null;
			}
			return Attribute.GetCustomAttribute(objectType, attributeType);
		}

		public static bool IsTypeGenericeCollectionInterface(Type type)
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!(genericTypeDefinition == typeof(IList<>)) && !(genericTypeDefinition == typeof(ICollection<>)))
			{
				return genericTypeDefinition == typeof(IEnumerable<>);
			}
			return true;
		}

		public static bool IsTypeDictionary(Type type)
		{
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				return true;
			}
			if (!type.IsGenericType)
			{
				return false;
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			return genericTypeDefinition == typeof(IDictionary<, >);
		}

		public static bool IsNullableType(Type type)
		{
			if (type.IsGenericType)
			{
				return type.GetGenericTypeDefinition() == typeof(Nullable<>);
			}
			return false;
		}

		public static object ToNullableType(object obj, Type nullableType)
		{
			if (obj != null)
			{
				return Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture);
			}
			return null;
		}
	}
	internal delegate object GetHandler(object source);
	internal delegate void SetHandler(object source, object value);
	internal delegate void MemberMapLoader(Type type, SafeDictionary<string, CacheResolver.MemberMap> memberMaps);
	internal class CacheResolver
	{
		private delegate object CtorDelegate();

		internal sealed class MemberMap
		{
			public readonly MemberInfo MemberInfo;

			public readonly Type Type;

			public readonly GetHandler Getter;

			public readonly SetHandler Setter;

			public MemberMap(PropertyInfo propertyInfo)
			{
				MemberInfo = propertyInfo;
				Type = propertyInfo.PropertyType;
				Getter = CreateGetHandler(propertyInfo);
				Setter = CreateSetHandler(propertyInfo);
			}

			public MemberMap(FieldInfo fieldInfo)
			{
				MemberInfo = fieldInfo;
				Type = fieldInfo.FieldType;
				Getter = CreateGetHandler(fieldInfo);
				Setter = CreateSetHandler(fieldInfo);
			}
		}

		private readonly MemberMapLoader _memberMapLoader;

		private readonly SafeDictionary<Type, SafeDictionary<string, MemberMap>> _memberMapsCache = new SafeDictionary<Type, SafeDictionary<string, MemberMap>>();

		private static readonly SafeDictionary<Type, CtorDelegate> ConstructorCache = new SafeDictionary<Type, CtorDelegate>();

		public CacheResolver(MemberMapLoader memberMapLoader)
		{
			_memberMapLoader = memberMapLoader;
		}

		public static object GetNewInstance(Type type)
		{
			if (ConstructorCache.TryGetValue(type, out var value))
			{
				return value();
			}
			ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			value = () => constructorInfo.Invoke(null);
			ConstructorCache.Add(type, value);
			return value();
		}

		public SafeDictionary<string, MemberMap> LoadMaps(Type type)
		{
			if (type == null || type == typeof(object))
			{
				return null;
			}
			if (_memberMapsCache.TryGetValue(type, out var value))
			{
				return value;
			}
			value = new SafeDictionary<string, MemberMap>();
			_memberMapLoader(type, value);
			_memberMapsCache.Add(type, value);
			return value;
		}

		private static GetHandler CreateGetHandler(FieldInfo fieldInfo)
		{
			return (object instance) => fieldInfo.GetValue(instance);
		}

		private static SetHandler CreateSetHandler(FieldInfo fieldInfo)
		{
			if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				fieldInfo.SetValue(instance, value);
			};
		}

		private static GetHandler CreateGetHandler(PropertyInfo propertyInfo)
		{
			MethodInfo getMethodInfo = propertyInfo.GetGetMethod(nonPublic: true);
			if (getMethodInfo == null)
			{
				return null;
			}
			return (object instance) => getMethodInfo.Invoke(instance, Type.EmptyTypes);
		}

		private static SetHandler CreateSetHandler(PropertyInfo propertyInfo)
		{
			MethodInfo setMethodInfo = propertyInfo.GetSetMethod(nonPublic: true);
			if (setMethodInfo == null)
			{
				return null;
			}
			return delegate(object instance, object value)
			{
				setMethodInfo.Invoke(instance, new object[1] { value });
			};
		}
	}
	internal class SafeDictionary<TKey, TValue>
	{
		private readonly object _padlock = new object();

		private readonly Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();

		public TValue this[TKey key] => _dictionary[key];

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();
		}

		public void Add(TKey key, TValue value)
		{
			lock (_padlock)
			{
				if (!_dictionary.ContainsKey(key))
				{
					_dictionary.Add(key, value);
				}
			}
		}
	}
}
namespace RestSharp.Validation
{
	public class Require
	{
		public static void Argument(string argumentName, object argumentValue)
		{
			if (argumentValue == null)
			{
				throw new ArgumentException("Argument cannot be null.", argumentName);
			}
		}
	}
	public class Validate
	{
		public static void IsBetween(int value, int min, int max)
		{
			if (value < min || value > max)
			{
				throw new ArgumentException($"Value ({value}) is not between {min} and {max}.");
			}
		}

		public static void IsValidLength(string value, int maxSize)
		{
			if (value == null || value.Length <= maxSize)
			{
				return;
			}
			throw new ArgumentException($"String is longer than max allowed size ({maxSize}).");
		}
	}
}

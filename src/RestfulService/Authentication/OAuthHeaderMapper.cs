using System;
using System.Collections.Specialized;

namespace RestfulService.Authentication
{
	public class OAuthHeaderMapper : IHeaderMapper<OAuthRequestHeader>
	{
		public OAuthRequestHeader Map(string headerValue) {
			string[] pairs = headerValue.Split(',');
			if(pairs.Length < 1)
				throw new ArgumentException("OAuth header does not seem to be valid");

			var values = new NameValueCollection();
			foreach (var pair in pairs) {
				string[] keyValue = pair.Trim().Split('=');
				string key = keyValue[0];
				string value = keyValue[1] ?? string.Empty;
				values.Add(key, value);
			}

			return new OAuthRequestHeader
			{
				ConsumerKey = values["oauth_consumer_key"],
				Nonce = values["oauth_nonce"],
				Realm = values["OAuth realm"],
				Signature = values["oauth_signature"],
				SignatureMethod = values["oauth_signature_method"],
				Timestamp = values["oauth_timestamp"],
				Token = values["oauth_token"],
				Version = values["oauth_version"],
				Verifier = values["oauth_verifier"]
			};
		}
	}
}
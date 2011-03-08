using System;

namespace RestfulService.Authentication
{
	public class OAuthRequestHeader
	{
		public string Realm { get; set; }

		public string ConsumerKey { get; set; }

		public string Token { get; set; }

		public string Nonce { get; set; }

		public string Timestamp { get; set; }

		public string SignatureMethod { get; set; }

		public string Version { get; set; }

		public string Signature { get; set; }

		public string Verifier { get; set; }
	}
}
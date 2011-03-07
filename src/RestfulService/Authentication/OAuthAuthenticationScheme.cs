using System;
using OpenRasta;
using OpenRasta.Authentication;
using OpenRasta.Authentication.Basic;
using OpenRasta.Security;
using OpenRasta.Web;

namespace RestfulService.Authentication
{
	public class OAuthAuthenticationScheme : IAuthenticationScheme
	{
		const string SCHEME = "OAuth";

		public AuthenticationResult Authenticate(IRequest request)
		{
			//TODO - find out whats in the IRequest, here is where you authenticate

			// http://hueniverse.com/2008/10/beginners-guide-to-oauth-part-iv-signing-requests/
			
			// Needs a fake user to map to something (config file?)

			// Needs a fake consumer key to map to something (config file?)

			// Needs to be able to return a request token (generate and save to filesystem)

			// and read it back for an access token (generate and save to filesystem with a timeout value)

			// Write AAT to sign and test

			return new AuthenticationResult.Success("greg", "Administrator");
		}

		public void Challenge(IResponse response)
		{
			response.Headers["WWW-Authenticate"] = string.Format("{0} realm=\"{1}\"", SCHEME, "www.whereimcallingfrom.com");
		}

		public string Name
		{
			get { return SCHEME; }
		}

		internal static OAuthRequestHeader ExtractOAuthHeader(string headerValue)
		{
			try
			{
				var basicBase64Credentials = headerValue.Split(' ')[1];

				var basicCredentials = basicBase64Credentials.FromBase64String().Split(':');

				if (basicCredentials.Length != 2)
					return null;

				return new OAuthRequestHeader();
			}
			catch
			{
				return null;
			}
		} 
	}


	/// <summary>
	/// https://github.com/openrasta/openrasta-stable/pull/2
	/// </summary>
	public class OAuthRequestHeader
	{
		public string Realm { get; set; }

		public string ConsumerKey { get; set; }

		public string Token { get; set; }

		public string Nonce { get; set; }

		public DateTime Timestamp { get; set; }

		public string SignatureMethod { get; set; }

		public string Version { get; set; }

		public string Signature { get; set; }
	}

	public class OAuthAuthenticator : IBasicAuthenticator
	{
		public AuthenticationResult Authenticate(BasicAuthRequestHeader header) {
			throw new NotImplementedException();
		}

		public string Realm {
			get {
				return "OAuth";
			}
		}
	}
}
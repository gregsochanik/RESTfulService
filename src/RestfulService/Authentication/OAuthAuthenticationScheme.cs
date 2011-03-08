using System;
using OpenRasta;
using OpenRasta.Authentication;
using OpenRasta.Authentication.Basic;
using OpenRasta.Security;
using OpenRasta.Web;

namespace RestfulService.Authentication
{
	/// <summary>
	/// https://github.com/openrasta/openrasta-stable/pull/2
	/// </summary>
	public class OAuthAuthenticationScheme : IAuthenticationScheme
	{
		private readonly IHeaderMapper<OAuthRequestHeader> _headerMapper;
		private readonly ICommunicationContext _context;
		const string SCHEME = "OAuth";
		private const string EXPECTED_USER_ID = "12345";
		private const string EXPECTED_CONSUMER_KEY = "YOUR_KEY_HERE";
		private const string EXPECTED_CONSUMER_SECRET = "12345678";

		public OAuthAuthenticationScheme(IHeaderMapper<OAuthRequestHeader> headerMapper, ICommunicationContext context) {
			_headerMapper = headerMapper;
			_context = context;
		}

		public AuthenticationResult Authenticate(IRequest request)
		{
			//TODO - find out whats in the IRequest, here is where you authenticate
			string headerValue = request.Headers["Authorization"];
			if (string.IsNullOrEmpty(headerValue))
				return new AuthenticationResult.Failed();

			OAuthRequestHeader oAuthRequestHeader = _headerMapper.Map(headerValue);
			// http://hueniverse.com/2008/10/beginners-guide-to-oauth-part-iv-signing-requests/
			
			// Needs to be able to return a request token (generate and save to filesystem) - Should this be an endpoint, or happen here?

			// and read it back for an access token (generate and save to filesystem with a timeout value) - do this first with fake token

			// Write AAT to sign and test - i.e. get request token first and then access token)

			return new AuthenticationResult.Failed();
		}

		public void Challenge(IResponse response)
		{
			response.Headers["WWW-Authenticate"] = string.Format("{0} realm=\"{1}\"", SCHEME, "http://localhost/restful_service/voucher");
		}

		public string Name
		{
			get { return SCHEME; }
		}
	}
}
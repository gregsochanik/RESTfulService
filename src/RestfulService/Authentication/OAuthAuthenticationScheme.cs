using System;
using OpenRasta;
using OpenRasta.Authentication;
using OpenRasta.Authentication.Basic;
using OpenRasta.Security;
using OpenRasta.Web;
using RestfulService.Handlers;

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
		private const string EXPECTED_CONSUMER_KEY = "light";
		private const string EXPECTED_REQUEST_TOKEN = "b0c2ec2c";
		private const string EXPECTED_VERIFIER = "c87677a4";
		private const string EXPECTED_ACCESS_TOKEN = "99fe97e1";
		private const string EXPECTED_ACCESS_TOKEN_SECRET = "255ae587";

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

			if(oAuthRequestHeader.ConsumerKey != EXPECTED_CONSUMER_KEY)
				return new AuthenticationResult.MalformedCredentials();

			bool isAccessTokenRequest = !string.IsNullOrEmpty(oAuthRequestHeader.Verifier);

			if (isAccessTokenRequest)
			{
				// Check credentials
				if (oAuthRequestHeader.Token == EXPECTED_REQUEST_TOKEN && oAuthRequestHeader.Verifier == EXPECTED_VERIFIER)
					return new OAuthSuccess(new OAuthCredentials(EXPECTED_ACCESS_TOKEN, EXPECTED_ACCESS_TOKEN_SECRET, true));
			}
			else
			{
				// and read it back for an access token (generate and save to filesystem with a timeout value) - do this first with fake token
				if (oAuthRequestHeader.Token == EXPECTED_ACCESS_TOKEN && oAuthRequestHeader.TokenSecret == EXPECTED_ACCESS_TOKEN_SECRET)
					return new OAuthSuccess();
			}

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

	public class OAuthSuccess : AuthenticationResult
	{
		public OAuthSuccess()
		{}

		public OAuthSuccess(OAuthCredentials credentials)
		{
			Credentials = credentials;
		}

		public OAuthCredentials Credentials { get; set; }
	}
}
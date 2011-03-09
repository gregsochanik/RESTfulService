using NUnit.Framework;

namespace RestfulService.Acceptance.Tests
{
	[TestFixture]
	public class AccessTokenEndpointTests
	{
		private const string OAUTH_REQUEST_HEADER = @"OAuth oauth_consumer_key='YOUR_KEY_HERE', 
													oauth_callback=http://localhost/voucher,  
													oauth_signature=GHU4a%2Fv9JnvZFTXnRiVf3HqDGfk%3D,
													oauth_version=1.0,
													oauth_nonce=05565e78,
													oauth_signature_method=HMAC-SHA1,
													oauth_consumer_key=light
													oauth_timestamp=1270254467";

		[Test]
		public void Should_get_unauthorized_if_incorrect_request_token_creds_provided()
		{

		}
	}
}
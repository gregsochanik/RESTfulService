using System;
using System.Configuration;
using System.Net;
using EndpointTesting;
using NUnit.Framework;

namespace RestfulService.Acceptance.Tests
{
	[TestFixture]
	public class VoucherEndpointTests
	{
		private const string CORRECT_OAUTH_REQUEST_HEADER = @"OAuth oauth_consumer_key=light, 
													oauth_callback=http://localhost/voucher,  
													oauth_signature=GHU4a%2Fv9JnvZFTXnRiVf3HqDGfk%3D,
													oauth_version=1.0,
													oauth_nonce=05565e78,
													oauth_signature_method=HMAC-SHA1,
													oauth_timestamp=1270254467";

		private const string INCORRECT_OAUTH_CONSUMER_KEY = @"OAuth oauth_consumer_key=YOUR_KEY_HERE";

		[Test]
		public void Should_get_unauthorized_with_correct_challenge_header_if_no_creds_provided_on_delete() {
			string url = ConfigurationManager.AppSettings["Application.BaseUrl"];
			var httpGetResolver = new HttpGetResolver();
			var webHeaderCollection = new WebHeaderCollection();

			HttpWebResponse response =  httpGetResolver.ResolveAsResponse(new Uri(url + "/voucher/1"), "DELETE", webHeaderCollection);

			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
			Assert.That(response.GetResponseHeader("WWW-Authenticate"), Is.StringStarting("OAuth realm=\""));

		}

		[Test]
		public void Should_get_unauthorised_if_incorrect_consumer_key_provided()
		{
			
		}

		[Test]
		public void Should_get_unauthorized_if_incorrect_access_token_creds_provided() {
			
		}

		[Test]
		public void Should_get_200_if_correct_credentials_provided_along_with_correct_creds() {
			
		}
	}
}
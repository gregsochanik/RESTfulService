using System;
using System.Configuration;
using System.Net;
using EndpointTesting;
using NUnit.Framework;

namespace RestfulService.Acceptance.Tests {

	[TestFixture]
	public class SetupTests {

		[Test]
		public void Should_be_able_to_hit_endpoint() {
			string url = ConfigurationManager.AppSettings["Application.BaseUrl"];
			string output = new HttpGetResolver().Resolve(new Uri(url+"/home"), "GET", new WebHeaderCollection());

			Console.WriteLine(output);
			Assert.That(output, Is.Not.Null);
		}
	}

	[TestFixture]
	public class VoucherEndpointTests
	{
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
		public void Should_get_unauthorized_if_incorrect_access_token_creds_provided() {
			
		}

		[Test]
		public void Should_get_200_if_correct_credentials_provided_along_with_correct_creds() {
			
		}
	}

	[TestFixture]
	public class RequestTokenEndpointTests
	{
		[Test]
		public void Should_get_unauthorized_if_incorrect_access_token_creds_provided()
		{

		}
	}


	[TestFixture]
	public class ArtistEndpointTests
	{
		[Test]
		public void Should_be_able_to_add_update_and_delete_artist() {
			string url = ConfigurationManager.AppSettings["Application.BaseUrl"];
			var httpPostResolver = new HttpPostResolver(new WebClientFactory(), "Id=100001&Name=Test&Genre=Rock");
			var webHeaderCollection = new WebHeaderCollection
			{
				{"Accept", "*/*"},
				{"Content-Type", "application/x-www-form-urlencoded"}
			};

			string output = httpPostResolver.Resolve(new Uri(url + "/artist"), "POST", webHeaderCollection);

			Console.WriteLine(output);
			Assert.That(output, Is.Not.Null);

			httpPostResolver = new HttpPostResolver(new WebClientFactory(), "Name=TEST2&Genre=Rock");

			output = httpPostResolver.Resolve(new Uri(url + "/artist/100001"), "PUT", webHeaderCollection);

			Console.WriteLine(output);
			Assert.That(output, Is.Not.Null);
		}

		[Test]
		public void Should_be_able_to_get_404_if_artist_iunknown() {
			string url = ConfigurationManager.AppSettings["Application.BaseUrl"];

			Assert.Throws<WebException>(
				() =>
				new HttpGetResolver().Resolve(new Uri(url + "/artist/1"), "GET",
											  new WebHeaderCollection()), "The remote server returned an error: (404) Not Found.");
		}
	}
}

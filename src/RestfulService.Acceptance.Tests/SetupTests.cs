﻿using System;
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

			httpPostResolver = new HttpPostResolver(new WebClientFactory(), "Id=100001&Name=TEST2&Genre=Rock");
			webHeaderCollection = new WebHeaderCollection
			{
				{"Accept", "*/*"},
				{"Content-Type", "application/x-www-form-urlencoded"}
			};

			output = httpPostResolver.Resolve(new Uri(url + "/artist"), "PUT", webHeaderCollection);

			Console.WriteLine(output);
			Assert.That(output, Is.Not.Null);

			var httpGetResolver = new HttpGetResolver();

			output = httpGetResolver.Resolve(new Uri(url + "/artist/100001"), "DELETE", new WebHeaderCollection());

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

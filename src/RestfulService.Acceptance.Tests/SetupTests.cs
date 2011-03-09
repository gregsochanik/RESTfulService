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
}

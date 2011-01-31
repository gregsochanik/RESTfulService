using System;
using System.Configuration;
using EndpointTesting;
using NUnit.Framework;

namespace RestfulService.Acceptance.Tests {

	[TestFixture]
	public class SetupTests {

		[Test]
		public void Should_be_able_to_hit_endpoint() {
			string url = ConfigurationManager.AppSettings["Application.BaseUrl"];
			string output = new HttpUrlResolver().Resolve(new Uri(url+"/home"));

			Console.WriteLine(output);
			Assert.That(output, Is.Not.Null);
		}
	}
}

using System;
using System.IO;
using System.Net;

namespace EndpointTesting {
	public class HttpUrlResolver : IUrlResolver  {
		// TODO: should be its own library/sln
		// TODO: Unit Test drive this
		// TODO: need to use abstractions that - already used on github
		public string Resolve(Uri endpoint) {
			var webRequest = WebRequest.Create(endpoint.ToString());
			var webResponse = webRequest.GetResponse();
			string output;
			using (var sr = new StreamReader(webResponse.GetResponseStream())) {
				output = sr.ReadToEnd();
			}
			return output;
		}
	}
}

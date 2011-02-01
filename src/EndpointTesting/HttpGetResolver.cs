﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace EndpointTesting {
	public class HttpGetResolver : IUrlResolver  {
		// TODO: should be its own library/sln
		// TODO: Unit Test drive this
		// TODO: need to use abstractions that - already used on github
		public string Resolve(Uri endpoint, string method, WebHeaderCollection headers) {
			var webRequest = (HttpWebRequest)WebRequest.Create(endpoint.ToString());
			webRequest.Headers.Add(headers);
			var webResponse = webRequest.GetResponse();
			string output;
			using (var sr = new StreamReader(webResponse.GetResponseStream())) {
				output = sr.ReadToEnd();
			}
			return output;
		}
	}

	public class HttpPostResolver : IUrlResolver
	{
		private readonly IWebClientFactory _webClientFactory;
		public string ParametersAsString { get; set; }

		public HttpPostResolver(IWebClientFactory webClientFactory) {
			_webClientFactory = webClientFactory;
		}
		public HttpPostResolver(IWebClientFactory webClientFactory, string parametersAsString) {
			_webClientFactory = webClientFactory;
			ParametersAsString = parametersAsString;
		}

		public string Resolve(Uri endpoint, string method, WebHeaderCollection headers) {
			using (var webClientWrapper = _webClientFactory.GetWebClient()) {

				webClientWrapper.Encoding = Encoding.UTF8;
				webClientWrapper.Headers.Add(headers);

				return webClientWrapper.UploadString(endpoint.AbsoluteUri, method, ParametersAsString);
			}
		}
	}

	public class WebClientWrapper : IWebClientWrapper {
		private readonly WebClient _client;

		public WebClientWrapper(WebClient client) {
			_client = client;
		}

		public string UploadString(string address, string method, string data) {
			return _client.UploadString(address, method, data);
		}

		public Encoding Encoding {
			get { return _client.Encoding; }
			set { _client.Encoding = value; }
		}

		public WebHeaderCollection Headers {
			get { return _client.Headers ?? new WebHeaderCollection(); }
			set { _client.Headers = value; }
		}

		public void Dispose() {
			_client.Dispose();
		}
	}

	public class WebClientFactory : IWebClientFactory {
		public IWebClientWrapper GetWebClient() {
			return new WebClientWrapper(new WebClient());
		}
	}

	public interface IWebClientWrapper : IDisposable {
		string UploadString(string address, string method, string data);
		Encoding Encoding { get; set; }
		WebHeaderCollection Headers { get; set; }
	}

	public interface IWebClientFactory {
		IWebClientWrapper GetWebClient();
	}
}

using System;
using System.Net;

namespace EndpointTesting
{
	public interface IUrlResolver{
		string Resolve(Uri endpoint, string method, WebHeaderCollection headers);
	}
}
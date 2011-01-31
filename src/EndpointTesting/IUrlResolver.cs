using System;

namespace EndpointTesting
{
	public interface IUrlResolver{
		string Resolve(Uri endpoint);
	}
}
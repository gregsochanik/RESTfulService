﻿using System;
using OpenRasta;
using OpenRasta.Authentication;
using OpenRasta.Web;

namespace RestfulService.Authentication
{
	public class OAuthAuthenticationScheme : IAuthenticationScheme
	{
		const string SCHEME = "OAuth";

		public AuthenticationResult Authenticate(IRequest request)
		{
			//TODO - find out whats in the IRequest, here is where you authenticate
			throw new NotImplementedException();
		}

		public void Challenge(IResponse response)
		{
			response.Headers["WWW-Authenticate"] = string.Format("{0} realm=\"{1}\"", SCHEME, "www.whereimcallingfrom.com");
		}

		public string Name
		{
			get { return SCHEME; }
		}

		internal static OAuthRequestHeader ExtractOAuthHeader(string headerValue)
		{
			try
			{
				var basicBase64Credentials = headerValue.Split(' ')[1];

				var basicCredentials = basicBase64Credentials.FromBase64String().Split(':');

				if (basicCredentials.Length != 2)
					return null;

				return new OAuthRequestHeader();
			}
			catch
			{
				return null;
			}
		} 
	}

	public class OAuthRequestHeader
	{
		// TODO - this is where you store the credentials?
	}
}
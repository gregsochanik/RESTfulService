using System;
using System.IO;
using OpenRasta.Binding;
using OpenRasta.IO;
using OpenRasta.Security;
using OpenRasta.Web;
using RestfulService.Authentication;

namespace RestfulService.Handlers {
	public class VoucherHandler
	{
		public OperationResult Get(VoucherResource voucher)
		{
			return new OperationResult.OK {ResponseResource = new VoucherResource {DiscountValue = 10, Id=voucher.Id, Redeemed = false}};
		}
		
		[RequiresOAuth]
		public OperationResult Delete(VoucherResource voucher) {

		    return new OperationResult.OK();
		}
	}

	public class AccessTokenHandler
	{
		[RequiresOAuth]
		public OperationResult Post(int voucherId)
		{
			// find a way of getting back the AuthorisationResult
			return new OperationResult.OK { ResponseResource = new OAuthCredentials("99fe97e1", "255ae587", true) };
		}
	}

	public class OAuthCredentials
	{
		public string Token { get; set; }
		public string TokenSecret { get; set; }
		public bool CallbackConfirmed { get; set; }

		public OAuthCredentials(string token, string tokensecret, bool callbackConfirmed)
		{
			Token = token;
			TokenSecret = tokensecret;
			CallbackConfirmed = callbackConfirmed;
		}
	}

	[Serializable]
	[KeyedValuesBinder]
	public class VoucherResource
	{
		public int Id { get; set; }
		public int DiscountValue { get; set; }
		public bool Redeemed { get; set; }
	}
}
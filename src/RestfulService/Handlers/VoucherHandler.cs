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

	public class PaymentHandler
	{
		public OperationResult Put(VoucherResource voucher) {

			return new OperationResult.OK { ResponseResource = new VoucherResource { DiscountValue = 10, Id = voucher.Id, Redeemed = false } };
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
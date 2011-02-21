using OpenRasta.Web;

namespace RestfulService.OperationResults
{
	public class ServiceUnavailable : OperationResult
	{
		public override int StatusCode {
			get {
				return 503;
			}
			set {
				base.StatusCode = value;
			}
		}

		public override string Title {
			get {
				return "Service Unavailable";
			}
			set {
				base.Title = value;
			}
		}
	}
}
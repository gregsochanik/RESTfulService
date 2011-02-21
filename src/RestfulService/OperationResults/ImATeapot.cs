using OpenRasta.Web;

namespace RestfulService.OperationResults
{
	public class ImATeapot : OperationResult
	{
		public override int StatusCode {
			get {
				return 418;
			}
			set {
				base.StatusCode = value;
			}
		}

		public override string Title {
			get {
				return "I'm a teapot";
			}
			set {
				base.Title = value;
			}
		}
	}
}
using OpenRasta.Web;
using RestfulService.Resources;

namespace RestfulService.Handlers
{
	public class HomeHandler {
		public OperationResult Get() {
			return new OperationResult.OK(new HomeResponse { Title = "Welcome home." });
		}
	}
}
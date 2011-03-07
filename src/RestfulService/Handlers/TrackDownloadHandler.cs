using System.IO;
using OpenRasta.IO;
using OpenRasta.Security;
using OpenRasta.Web;

namespace RestfulService.Handlers {
	public class TrackDownloadHandler
	{
		[RequiresAuthentication]
		public OperationResult Get(int trackId = 1) {
			return new OperationResult.OK() {ResponseResource = "Hello world"};
		}
	}
}
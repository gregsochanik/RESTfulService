using System.IO;
using OpenRasta.IO;
using OpenRasta.Security;
using OpenRasta.Web;
using RestfulService.Authentication;

namespace RestfulService.Handlers {
	public class TrackDownloadHandler
	{
		//[RequiresOAuth]
		//public IFile Get(int trackId = 1) {

		//    var fs = File.OpenRead(string.Format("C:/artist/{0}.mp3", trackId));
		//    return new InMemoryFile(fs) {
		//        ContentType = new MediaType("application/mpeg"),
		//        FileName = "MyFile.mp3",
		//        Length = fs.Length
		//    };
		//}

		[RequiresOAuth]
		public OperationResult Get(int trackId=1)
		{
			return new OperationResult.OK {ResponseResource = "You have been allowed in"};
		}
	}
}
using System.IO;
using OpenRasta.IO;
using OpenRasta.Security;
using OpenRasta.Web;

namespace RestfulService.Handlers {
	[RequiresAuthentication]
	//TODO - need to figure out how to bootstrap OAuthAuthentication as the default auth model
	public class TrackDownloadHandler {
		public IFile Get(int trackId = 1) {
			var fs = File.OpenRead(string.Format("C:/artist/{0}.mp3", trackId));
			return new InMemoryFile(fs)
			{
				ContentType = new MediaType("application/mpeg"), 
				FileName="MyFile.mp3", 
				Length = fs.Length
			};
		}
	}
}
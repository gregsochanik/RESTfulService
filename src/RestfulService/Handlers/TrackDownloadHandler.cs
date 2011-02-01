using System.IO;
using OpenRasta.IO;
using OpenRasta.Web;

namespace RestfulService.Handlers {
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
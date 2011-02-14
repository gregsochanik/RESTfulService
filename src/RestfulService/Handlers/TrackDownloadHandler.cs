using System.IO;
using OpenRasta.IO;
using OpenRasta.Security;
using OpenRasta.Web;

namespace RestfulService.Handlers {
	[RequiresAuthentication]
	public class TrackDownloadHandler
	{
		private ICommunicationContext _context;
		public TrackDownloadHandler(ICommunicationContext context) {
			_context = context;
		}

		public IFile Get(int trackId = 1) {
			string name = _context.User.Identity.Name;

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
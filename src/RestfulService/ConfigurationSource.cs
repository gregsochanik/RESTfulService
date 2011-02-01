using OpenRasta.Configuration;
using OpenRasta.IO;
using OpenRasta.Web;
using RestfulService.Handlers;
using RestfulService.Resources;

namespace RestfulService
{
	public class ConfigurationSource : IConfigurationSource{
		public void Configure(){
			using (OpenRastaConfiguration.Manual) {
				ResourceSpace.Has
					.ResourcesOfType<HomeResponse>()
					.AtUri("/home")
					.HandledBy<HomeHandler>().AsXmlSerializer().ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Has
					.ResourcesOfType<ArtistResponse>()
					.AtUri("/artist").And
					.AtUri("/artist/{artistId}")
					.HandledBy<ArtistHandler>()
					.AsXmlSerializer().ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Has
					.ResourcesOfType<IFile>()
					.AtUri("/download").And
					.AtUri("/download/{trackId}")
					.HandledBy<TrackDownloadHandler>();

			}
		}
	}
}
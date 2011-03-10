using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
using RestfulService.Codecs;
using RestfulService.Handlers;
using RestfulService.OperationInterceptors;
using RestfulService.Resources;

namespace RestfulService
{
	public class ConfigurationSource : IConfigurationSource{
		public void Configure(){
			using (OpenRastaConfiguration.Manual) {
				ResourceSpace.Has
					.ResourcesOfType<HomeResource>()
					.AtUri("/home")
					.HandledBy<HomeHandler>().AsXmlSerializer()
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Has
					.ResourcesOfType<Artist>()
					.AtUri("/artist/").And
					.AtUri("/artist/{Id}")
					.HandledBy<ArtistHandler>()
					.TranscodedBy(typeof(SevenDigitalDataContractCodec))
					.ForMediaType("application/vnd.7digital+xml")
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);
				
				ResourceSpace.Has
					.ResourcesOfType<SearchResource>()
					.AtUri("/search/{SearchTerm}/{Page}/{PageSize}")
					.And 
					.AtUri("/search?q={SearchTerm}&page={Page}&pagesize={PageSize}")
					.HandledBy<SearchHandler>()
					.TranscodedBy(typeof(SevenDigitalDataContractCodec))
					.ForMediaType("application/vnd.7digital+xml")
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Uses.CustomDependency<IOperationInterceptor, ValidationOperationInterceptor>(DependencyLifetime.Singleton);
				
			}
		}

		
	}
}
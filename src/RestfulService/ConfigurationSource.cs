using OpenRasta.Authentication;
using OpenRasta.Authentication.Basic;
using OpenRasta.Configuration;
using OpenRasta.Configuration.Fluent;
using OpenRasta.DI;
using OpenRasta.IO;
using OpenRasta.Web;
using RestfulService.Authentication;
using RestfulService.Codecs;
using RestfulService.Handlers;
using RestfulService.Resources;

namespace RestfulService
{
	public class ConfigurationSource : IConfigurationSource{
		public void Configure(){
			using (OpenRastaConfiguration.Manual) {

				ResourceSpace.Uses.OAuthAuthentication<OAuthAuthenticator>();

				ResourceSpace.Has
					.ResourcesOfType<HomeResource>()
					.AtUri("/home")
					.HandledBy<HomeHandler>().AsXmlSerializer().ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Has
					.ResourcesOfType<Artist>()
					.AtUri("/artist").And
					.AtUri("/artist/{Id}")
					.HandledBy<ArtistHandler>()
					.TranscodedBy(typeof(SevenDigitalDataContractCodec))
					.ForMediaType("application/vnd.7digital+xml")
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);
				
				ResourceSpace.Has
					.ResourcesOfType<IFile>()
					.AtUri("/download").And
					.AtUri("/download/{trackId}")
					.HandledBy<TrackDownloadHandler>();

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
			}
		}
	}

	public static class ExtensionsToIUses {
		public static void OAuthAuthentication<TBasicAuthenticator>(this IUses uses) where TBasicAuthenticator : class, IBasicAuthenticator {
			uses.CustomDependency<IAuthenticationScheme, OAuthAuthenticationScheme>(DependencyLifetime.Transient);

			uses.CustomDependency<IBasicAuthenticator, TBasicAuthenticator>(DependencyLifetime.Transient);
		}
	}
}
using OpenRasta.Authentication;
using OpenRasta.Authentication.Basic;
using OpenRasta.Configuration;
using OpenRasta.Configuration.Fluent;
using OpenRasta.DI;
using OpenRasta.IO;
using OpenRasta.Web;
using RestfulService.Authentication;
using RestfulService.Handlers;
using RestfulService.Resources;

namespace RestfulService
{
	public class ConfigurationSource : IConfigurationSource{
		public void Configure(){
			using (OpenRastaConfiguration.Manual) {

				ResourceSpace.Uses.OAuthAuthentication<OAuthAuthenticator>();

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

	public static class ExtensionsToIUses {
		public static void OAuthAuthentication<TBasicAuthenticator>(this IUses uses) where TBasicAuthenticator : class, IBasicAuthenticator {
			uses.CustomDependency<IAuthenticationScheme, OAuthAuthenticationScheme>(DependencyLifetime.Transient);

			uses.CustomDependency<IBasicAuthenticator, TBasicAuthenticator>(DependencyLifetime.Transient);
		}
	}
}
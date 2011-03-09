using System.Linq;
using Castle.Windsor;
using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.IO;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Pipeline;
using OpenRasta.Pipeline.Contributors;
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
					.AtUri("/artist").And
					.AtUri("/artist/{Id}")
					.HandledBy<ArtistHandler>()
					.TranscodedBy(typeof(SevenDigitalDataContractCodec))
					.ForMediaType("application/vnd.7digital+xml")
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml)
					.And
					.AsJsonDataContract().ForMediaType(MediaType.Json);

				ResourceSpace.Has
					.ResourcesOfType<VoucherResource>()
					.AtUri("/voucher/{Id}")
					.HandledBy<VoucherHandler>()
					.AsXmlSerializer()
					.ForMediaType(new MediaType("text/xml")).ForMediaType(MediaType.Xml);

				ResourceSpace.Has
					.ResourcesOfType<VoucherResource>()
					.AtUri("/accessToken/{voucherId}")
					.HandledBy<AccessTokenHandler>()
					.AsXmlSerializer()
					.ForMediaType(MediaType.MultipartFormData).ForMediaType(MediaType.Xml);
				
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

				RemoveDigestAuthorisationContributor(DependencyResolver.Container);
			}
		}

		/// <summary>
		/// http://trac.caffeine-it.com/openrasta/ticket/118#comment:1
		/// See OpenRasta.DigestAuthorizerContributor.WriteCredentialRequest
		/// TODO: Need to implement 
		/// </summary>
		public static void RemoveDigestAuthorisationContributor(IWindsorContainer container) {
			var contributors = container.Kernel.GetHandlers(typeof(IPipelineContributor));
			var digestContributor = contributors.SingleOrDefault(i => i.ComponentModel.Implementation == typeof(DigestAuthorizerContributor));

			container.Kernel.RemoveComponent(digestContributor.ComponentModel.Name);
		}
	}
}
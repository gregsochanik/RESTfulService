using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace RestfulService
{
	public class LocalInstaller : IWindsorInstaller {
		public void Install(IWindsorContainer container, IConfigurationStore store) {
			var descriptor = AllTypes
				.FromAssembly(typeof(DependencyResolver).Assembly)
				.Pick()
				.WithService.FirstInterface()
				.Configure(c => c.LifeStyle.Transient);

			container.Register(descriptor);
		}
	}
}
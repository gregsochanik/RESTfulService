using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using OpenRasta.DI;
using OpenRasta.DI.Windsor;

namespace RestfulService
{
	public class DependencyResolver : IDependencyResolverAccessor {

		private static IWindsorContainer _container;

		public static IWindsorContainer Container {
			get { return _container ?? (_container = ConfigureContainer()); }
		}

		public IDependencyResolver Resolver {
			get { return new WindsorDependencyResolver(Container); }
		}

		static IWindsorContainer ConfigureContainer(){
			_container = new WindsorContainer(new XmlInterpreter());
			_container.Install(new LocalInstaller());
			return _container;
		}
	}
}
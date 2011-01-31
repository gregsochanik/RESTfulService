using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using OpenRasta.DI;
using OpenRasta.DI.Windsor;

namespace RestfulService
{
	public class DependancyResolver {

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

				var registrations = AllTypes
					.FromAssembly(typeof(DependencyResolver).Assembly)
					.Pick()
					.WithService.FirstInterface()
					.Configure(c => c.LifeStyle.Transient);

				_container.Register(registrations);

				return _container;
			}
		}
	}
}
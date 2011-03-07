using System;
using System.Collections.Generic;
using OpenRasta.Authentication;
using OpenRasta.DI;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
using RestfulService.OperationInterceptors;

namespace RestfulService.Authentication
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class RequiresOAuthAttribute : InterceptorProviderAttribute {
		
		public override IEnumerable<IOperationInterceptor> GetInterceptors(IOperation operation) {
			return new[]
			{
				new RequiresOAuthInterceptor(DependencyManager.GetService<ICommunicationContext>(), DependencyManager.GetService<IAuthenticationScheme>())
			};
		}
	}
}
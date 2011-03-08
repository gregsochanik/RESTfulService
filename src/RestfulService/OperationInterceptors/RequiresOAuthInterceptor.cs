using OpenRasta.Authentication;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;

namespace RestfulService.OperationInterceptors
{
	public class RequiresOAuthInterceptor : OperationInterceptor
	{
		private readonly ICommunicationContext _context;
		private readonly IAuthenticationScheme _scheme;

		public RequiresOAuthInterceptor(ICommunicationContext context, IAuthenticationScheme scheme) {
			_context = context;
			_scheme = scheme;
		}

		public override bool BeforeExecute(IOperation operation) {
			AuthenticationResult authenticationResult = _scheme.Authenticate(_context.Request);
			if(authenticationResult is AuthenticationResult.Failed) {
				_scheme.Challenge(_context.Response);
				_context.OperationResult = new OperationResult.Unauthorized { ResponseResource = "You aint got no authoritah!!" };
				return false;
			}
			return true;
		}
	}
}
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenRasta.Authentication;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
using RestfulService.Authentication;
using RestfulService.Handlers;
using RestfulService.OperationInterceptors;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests.Authentication {
	
	[TestFixture, Ignore("not implemented")]
	public class RequiresOAuthAttributeTests
	{
		[Test]
		public void Should_provide_correct_interceptor() {
			var requiresOAuthAttribute = new RequiresOAuthAttribute();
			IEnumerable<IOperationInterceptor> operationInterceptors = requiresOAuthAttribute.GetInterceptors(MockRepository.GenerateStub<IOperation>());
			Assert.That(operationInterceptors.Count(), Is.EqualTo(1));
			Assert.That(operationInterceptors.FirstOrDefault().GetType(), Is.TypeOf<RequiresOAuthInterceptor>());
		}
	}

	[TestFixture]
	public class RequiresOAuthInterceptortests {
		private ICommunicationContext _communicationContext;
		private IAuthenticationScheme _authenticationScheme;
		private IOperation _operation;

		[SetUp]
		public void SetUp() {
			_communicationContext = MockRepository.GenerateStub<ICommunicationContext>();
			_communicationContext.Stub(x=>x.Request).Return(MockRepository.GenerateStub<IRequest>());

			_authenticationScheme = MockRepository.GenerateStub<IAuthenticationScheme>();
			_operation = MockRepository.GenerateStub<IOperation>();
		}

		[Test]
		public void Should_fire_Authenticate_on_beginexecute() {
			var requiresOAuthInterceptor = new RequiresOAuthInterceptor(_communicationContext, _authenticationScheme);
			requiresOAuthInterceptor.BeforeExecute(_operation);
			_authenticationScheme.AssertWasCalled(x=>x.Authenticate(_communicationContext.Request));
		}

		[Test]
		public void Should_return_false_on_failed_and_set_context_to_notAuthorized() {
			var requiresOAuthInterceptor = new RequiresOAuthInterceptor(_communicationContext, _authenticationScheme);
			_authenticationScheme.Stub(x => x.Authenticate(_communicationContext.Request)).
				Return(new AuthenticationResult.Failed());

			bool result = requiresOAuthInterceptor.BeforeExecute(_operation);

			Assert.That(result, Is.False);

			Assert.That(_communicationContext.OperationResult.StatusCode, Is.EqualTo(401));
		}

		[Test]
		public void Should_return_true_on_success() {
			var requiresOAuthInterceptor = new RequiresOAuthInterceptor(_communicationContext, _authenticationScheme);
			_authenticationScheme.Stub(x => x.Authenticate(_communicationContext.Request)).
				Return(new AuthenticationResult.Success("test"));

			bool result = requiresOAuthInterceptor.BeforeExecute(_operation);

			Assert.That(result);
		}
	}
}

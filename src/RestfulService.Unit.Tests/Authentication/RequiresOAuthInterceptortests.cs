using NUnit.Framework;
using OpenRasta.Authentication;
using OpenRasta.OperationModel;
using OpenRasta.Web;
using RestfulService.Authentication;
using RestfulService.OperationInterceptors;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests.Authentication {
	
	[TestFixture]
	public class OAuthAuthenticationTests
	{
		private const string OAUTH_HEADER = @"OAuth realm='http://localhost/restful_service/download', 
												oauth_consumer_key='YOUR_KEY_HERE', 
												oauth_token='TOKEN', 
												oauth_nonce='nonce', 
												oauth_timestamp='TIMESTAMP', 
												oauth_signature_method='HMAC-SHA1', 
												oauth_version='1.0', 
												oauth_signature='SIGNATURE'";

		[Test]
		public void Should_fire_header_mapper_with_correct_value() {
			var headerMapper = MockRepository.GenerateStub<IHeaderMapper<OAuthRequestHeader>>();
			var request = MockRepository.GenerateStub<IRequest>();
			var context = MockRepository.GenerateStub<ICommunicationContext>();
			request.Stub(x => x.Headers).Return(new HttpHeaderDictionary() { { "Authorization", OAUTH_HEADER } });
			var oAuthAuthenticationScheme = new OAuthAuthenticationScheme(headerMapper, context);
			oAuthAuthenticationScheme.Authenticate(request);
			headerMapper.AssertWasCalled(x=>x.Map(OAUTH_HEADER));
		}
	}

	[TestFixture]
	public class OAuthHeaderMapperTests
	{
		private const string OAUTH_HEADER = @"OAuth realm='http://localhost/restful_service/download', 
												oauth_consumer_key=YOUR_KEY_HERE, 
												oauth_token=TOKEN, 
												oauth_nonce=nonce, 
												oauth_timestamp=TIMESTAMP, 
												oauth_signature_method=HMAC-SHA1, 
												oauth_version=1.0, 
												oauth_signature=SIGNATURE";

		[Test]
		public void Should_map_fields_correctly() {
			OAuthRequestHeader oAuthRequestHeader = new OAuthHeaderMapper().Map(OAUTH_HEADER);
			Assert.That(oAuthRequestHeader.ConsumerKey, Is.EqualTo("YOUR_KEY_HERE"));
			Assert.That(oAuthRequestHeader.Token, Is.EqualTo("TOKEN"));
			Assert.That(oAuthRequestHeader.Nonce, Is.EqualTo("nonce"));
			Assert.That(oAuthRequestHeader.Timestamp, Is.EqualTo("TIMESTAMP"));
			Assert.That(oAuthRequestHeader.SignatureMethod, Is.EqualTo("HMAC-SHA1"));
			Assert.That(oAuthRequestHeader.Version, Is.EqualTo("1.0"));
			Assert.That(oAuthRequestHeader.Signature, Is.EqualTo("SIGNATURE"));
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

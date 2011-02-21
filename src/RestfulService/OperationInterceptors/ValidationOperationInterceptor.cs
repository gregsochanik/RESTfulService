using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenRasta.Binding;
using OpenRasta.DI;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
using RestfulService.Resources;
using RestfulService.Validation;

namespace RestfulService.OperationInterceptors
{
	public class ValidationOperationInterceptor : IOperationInterceptor {

		private readonly IDependencyResolver _resolver;
		private readonly ICommunicationContext _context;

		public ValidationOperationInterceptor(IDependencyResolver resolver, ICommunicationContext context) {
			_resolver = resolver;
			_context = context;
		}

		public bool BeforeExecute(IOperation operation) {

			foreach (var input in operation.Inputs) {
				if (input == null)
					continue;

				var parameter = input.Binder.BuildObject();

				try {
					ISelfValidator validator = ResolveValidator(parameter);

					var errors = validator.Validate(parameter.Instance).ToList();

					if (errors.Count > 0) {
						_context.OperationResult = new OperationResult.BadRequest {
							ResponseResource = parameter.Instance,
							Errors = errors
						};
						return false;
					}
				} catch (Exception ex) {}
			}
			return true;
		}

		private ISelfValidator ResolveValidator(BindingResult parameter) {
			var validationFactory =
				typeof (IValidationFactory<>).MakeGenericType(parameter.Instance.GetType());

			var resolve = _resolver.Resolve(validationFactory) as IValidationFactory<Artist>;
			return resolve.GetValidator(_context.Request.HttpMethod);
		}

		public bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers) {
			return true;
		}

		public Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder) {
			return null;
		}
	}
}
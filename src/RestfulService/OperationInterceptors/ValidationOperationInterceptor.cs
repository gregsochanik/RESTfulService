using System;
using System.Collections.Generic;
using System.Linq;
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
					IValidator validator = ResolveValidator(parameter);

					var errors = validator.Validate(parameter.Instance, _context.Request.HttpMethod).ToList();

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

		private IValidator ResolveValidator(BindingResult parameter) {
			Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.Instance.GetType());

			return _resolver.Resolve(validatorType) as IValidator;
		}

		public bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers) {
			return true;
		}

		public Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder) {
			return null;
		}
	}
}
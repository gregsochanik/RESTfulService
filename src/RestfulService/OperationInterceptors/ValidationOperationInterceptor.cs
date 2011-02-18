using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.Binding;
using OpenRasta.DI;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
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
			var input = operation.Inputs.FirstOrDefault();

			if (input == null)
				return true;

			var parameter = input.Binder.BuildObject();

			try {

				ISelfValidator validator = ResolveValidator(parameter);

				var errors = validator.Validate(parameter.Instance);

				if (errors.Count() < 1)
					return true;

				_context.OperationResult = new OperationResult.BadRequest
				{ResponseResource = parameter.Instance, Errors = errors.ToList()};

				return false;
			} catch(Exception) {
				return true;
			}
		}

		private ISelfValidator ResolveValidator(BindingResult parameter) {
			var validatorType = typeof (ISelfValidator<>).MakeGenericType(parameter.Instance.GetType());
			return _resolver.Resolve(validatorType) as ISelfValidator;
		}

		public bool AfterExecute(IOperation operation, IEnumerable<OutputMember> outputMembers) {
			return true;
		}

		public Func<IEnumerable<OutputMember>> RewriteOperation(Func<IEnumerable<OutputMember>> operationBuilder) {
			return null;
		}
	}
}
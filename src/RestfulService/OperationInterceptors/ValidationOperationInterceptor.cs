using System;
using System.Linq;
using OpenRasta.Binding;
using OpenRasta.DI;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;
using RestfulService.Validation;

namespace RestfulService.OperationInterceptors
{
	public class ValidationOperationInterceptor : OperationInterceptor {

		private readonly IDependencyResolver _resolver;
		private readonly ICommunicationContext _context;

		public ValidationOperationInterceptor(IDependencyResolver resolver, ICommunicationContext context) {
			_resolver = resolver;
			_context = context;
		}

		public override bool BeforeExecute(IOperation operation) {
			
			foreach (InputMember input in operation.Inputs) {
				if (input == null)
					continue;

				var parameter = input.Binder.BuildObject();

				try {
					IValidator validator = ResolveValidator(parameter);
					if (validator == null)
						continue;

					var errors = validator.Validate(parameter.Instance, operation.Name.ToUpper()).ToList();

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

			if (_resolver.HasDependency(validatorType))
				return _resolver.Resolve(validatorType) as IValidator;

			return null;
		}
	}
}
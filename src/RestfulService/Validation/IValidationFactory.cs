using RestfulService.Resources;

namespace RestfulService.Validation
{
	public interface IValidationFactory<T>
	{
		IValidator<T> GetValidator(string httpMethod);
	}
}
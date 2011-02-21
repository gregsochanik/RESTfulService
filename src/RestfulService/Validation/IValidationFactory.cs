namespace RestfulService.Validation
{
	public interface IValidationFactory<T>
	{
		ISelfValidator<T> GetValidator(string httpMethod);
	}
}
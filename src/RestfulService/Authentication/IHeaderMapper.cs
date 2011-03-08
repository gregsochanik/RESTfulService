namespace RestfulService.Authentication
{
	public interface IHeaderMapper<T>
	{
		T Map(string headerValue);
	}
}
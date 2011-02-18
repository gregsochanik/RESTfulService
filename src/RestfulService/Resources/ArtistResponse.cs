namespace RestfulService.Resources
{
	public interface IApiResponse<T> {
		T Response { get; set; }
	}
}
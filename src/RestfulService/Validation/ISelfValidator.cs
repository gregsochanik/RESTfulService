using System.Collections.Generic;

namespace RestfulService.Validation
{
	public interface ISelfValidator<T>
	{
		IEnumerable<ValidationResponse> Validate(T item);
	}
}
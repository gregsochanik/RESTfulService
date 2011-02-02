using System.Collections.Generic;
using OpenRasta;

namespace RestfulService.Validation
{
	public interface ISelfValidator<T>
	{
		IEnumerable<Error> Validate(T item);
	}
}
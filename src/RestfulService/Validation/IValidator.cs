using System.Collections.Generic;
using OpenRasta;

namespace RestfulService.Validation
{
	public interface IValidator<T> :IValidator
	{}

	public interface IValidator
	{
		IEnumerable<Error> Validate(object item, string httpMethod);
	}
}
using System.Collections.Generic;
using OpenRasta;

namespace RestfulService.Validation
{
	public interface ISelfValidator<T> :ISelfValidator
	{}

	public interface ISelfValidator
	{
		IEnumerable<Error> Validate(object item, string httpMethod);
	}
}
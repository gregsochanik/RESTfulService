using System;
using OpenRasta.Web;

namespace RestfulService.Handlers.ExceptionOutput
{
	public interface IOperationOutput
	{
		OperationResult HandleOutput(Exception ex, object parameter);
	}
}
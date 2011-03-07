using System;
using OpenRasta.Binding;
using OpenRasta.Web;

namespace RestfulService.OperationInterceptors
{
	public interface IOutputHandler
	{
		OperationResult HandleOutput(Exception ex, object parameter);
	}
}
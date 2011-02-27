using System;
using OpenRasta.Binding;

namespace RestfulService.OperationInterceptors
{
	public interface IOutputHandler
	{
		void HandleOutput(Exception ex, BindingResult parameter);
	}
}
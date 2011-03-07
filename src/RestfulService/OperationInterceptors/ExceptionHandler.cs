using System;
using System.Collections.Generic;
using System.Linq;
using OpenRasta.OperationModel;
using OpenRasta.OperationModel.Interceptors;
using OpenRasta.Web;

namespace RestfulService.OperationInterceptors
{
	public class ExceptionHandler : OperationInterceptor
	{
		private readonly ICommunicationContext _context;
		private readonly IOutputHandler _outputHandler;


		public ExceptionHandler(ICommunicationContext context, IOutputHandler outputHandler)
		{
			_context = context;
			_outputHandler = outputHandler;
		}

		public override bool BeforeExecute(IOperation operation)
		{
			InputMember inputMember = operation.Inputs.FirstOrDefault();

			if (inputMember == null)
				return true;

			var parameter = inputMember.Binder.BuildObject();

			try
			{
				IEnumerable<OutputMember> outputMembers = operation.Invoke();
				_context.OperationResult  = (OperationResult)outputMembers.FirstOrDefault().Value;
			}
			catch (Exception ex)
			{
				_outputHandler.HandleOutput(ex, parameter);
			}
			return false;
		}
	}
}
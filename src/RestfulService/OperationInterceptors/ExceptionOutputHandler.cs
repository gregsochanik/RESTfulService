using System;
using System.IO;
using System.Reflection;
using log4net;
using OpenRasta.Binding;
using OpenRasta.Web;
using RestfulService.Exceptions;
using RestfulService.Handlers;
using RestfulService.Resources;

namespace RestfulService.OperationInterceptors
{
	public class ExceptionOutputHandler : IOutputHandler
	{
		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly ICommunicationContext _context;

		public ExceptionOutputHandler(ICommunicationContext context)
		{
			_context = context;
		}

		public void HandleOutput(Exception ex, BindingResult parameter)
		{
			if (ex.InnerException is FileNotFoundException)
			{
				_context.OperationResult = new OperationResult.NotFound
				                           	{
				                           		ResponseResource = parameter.Instance,
				                           		Description = String.Format("Artist {0} not found", 1)
				                           	};
			}
			else if (ex.InnerException is ResourceExistsException)
			{
				_context.OperationResult = new OperationResult.Found { ResponseResource = parameter.Instance };
			}
			else if (ex.InnerException is IOException)
			{
				int resourceId = ((IHasId)parameter.Instance).Id;
				var uriString = ServiceEnvironment.CreateArtistUriString(resourceId);
				_context.OperationResult = new OperationResult.MethodNotAllowed(new Uri(uriString), HttpVerb.DELETE.ToString(), resourceId);
			}
			else
			{
				_log.Error(ex.InnerException);
				_context.OperationResult = new OperationResult.InternalServerError { ResponseResource = parameter.Instance };
			}
		}
	}
}
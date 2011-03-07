﻿using System;
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
		
		public OperationResult HandleOutput(Exception ex, object parameter) {
			if (ex is FileNotFoundException) {
				return new OperationResult.NotFound {
					ResponseResource = parameter,
					Description = String.Format("Artist {0} not found", 1)
				};
			}

			if (ex is ResourceExistsException) {
				return new OperationResult.Found { ResponseResource = parameter };
			}

			if (ex is IOException) {
				int resourceId = ((IHasId)parameter).Id;
				var uriString = ServiceEnvironment.CreateArtistUriString(resourceId);
				return new OperationResult.MethodNotAllowed(new Uri(uriString), HttpVerb.DELETE.ToString(), resourceId);
			}

			_log.Error(ex);
			return new OperationResult.InternalServerError { ResponseResource = parameter };
		}

		public void HandleOutput(Exception ex, BindingResult parameter) {
			HandleOutput(ex, parameter.Instance);
		}
	}
}
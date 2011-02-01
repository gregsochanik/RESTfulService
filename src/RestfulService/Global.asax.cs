using System;
using System.Reflection;
using log4net;
using log4net.Config;

namespace RestfulService
{
	public class Global : System.Web.HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e) {
			try {
				XmlConfigurator.Configure();
			} catch (Exception ex) {
				Console.WriteLine("Cannot create log4Net logger. Ex.Message: {0}. StackTrace: {1}", ex.Message, ex.StackTrace);
			}
		}

		void Application_End(object sender, EventArgs e)
		{
			//  Code that runs on application shutdown

		}

		void Application_Error(object sender, EventArgs e)
		{
			// Code that runs when an unhandled error occurs

		}

		void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started

		}

		void Session_End(object sender, EventArgs e)
		{
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.

		}

	}
}

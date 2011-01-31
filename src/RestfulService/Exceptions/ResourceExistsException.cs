using System.IO;

namespace RestfulService.Exceptions
{
	public class ResourceExistsException : IOException
	{
		public ResourceExistsException (string message) : base(message) {}
	}
}
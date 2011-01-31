using System;
using System.Xml.XPath;

namespace RestfulService.Utility.IO
{
	public interface IFileWrapper{
		void CreateDirectory(string path);
		bool FileExists(string filePath);
		string FileAsString(string filePath);
		IXPathNavigable FileAsXml(string filePath);
	}

	public class FileWrapper : IFileWrapper
	{
		public void CreateDirectory(string path) {
			throw new NotImplementedException();
		}

		public bool FileExists(string filePath) {
			throw new NotImplementedException();
		}

		public string FileAsString(string filePath) {
			throw new NotImplementedException();
		}

		public IXPathNavigable FileAsXml(string filePath) {
			throw new NotImplementedException();
		}
	}
}
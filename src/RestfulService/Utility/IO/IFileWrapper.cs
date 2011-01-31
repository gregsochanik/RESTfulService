using System.Xml.XPath;

namespace RestfulService.Utility.IO
{
	public interface IFileWrapper{
		void CreateDirectory(string path);
		bool FileExists(string filePath);
		string FileAsString(string filePath);
		IXPathNavigable FileAsXml(string filePath);
	}
}
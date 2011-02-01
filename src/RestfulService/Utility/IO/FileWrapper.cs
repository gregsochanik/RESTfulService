using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace RestfulService.Utility.IO
{
	public class FileWrapper : IFileWrapper
	{
		public void CreateDirectory(string path) {
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}

		public bool FileExists(string filePath) {
			return File.Exists(filePath);
		}

		public string FileAsString(string filePath) {
			using (var fileStream = File.OpenRead(filePath)) {
				var sr = new StreamReader(fileStream);
				return sr.ReadToEnd().Trim();
			}
		}

		public IXPathNavigable FileAsXml(string filePath) {
			string raw = FileAsString(filePath);
			var xml = new XmlDocument();
			xml.LoadXml(raw);
			return xml;
		}

		public void WriteFile(string data, string filepath) {
			using (var fs = File.OpenWrite(filepath)) {
				var sw = new StreamWriter(fs);
				sw.Write(data);
				sw.Flush();
			}
		}

		public void DeleteFile(string filepath) {
			File.Delete(filepath);
		}
	}
}
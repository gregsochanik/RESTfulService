using System.IO;
using System.Xml;
using NUnit.Framework;
using RestfulService.Utility.IO;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	[Category("Integration")]
	public class FileWrapperTests
	{
		[Test]
		public void Can_create_directory() {
			
			string path = Path.GetTempPath() + "/testdirectory123";
			var fileWrapper = new FileWrapper();
			fileWrapper.CreateDirectory(path);

			Assert.That(Directory.Exists(path));
			Directory.Delete(path);
			Assert.That(Directory.Exists(path), Is.False);
		}

		[Test]
		public void Can_check_if_file_exists() {
			var tempFileName = Path.GetTempFileName();
			var fileWrapper = new FileWrapper();

			Assert.That(fileWrapper.FileExists(tempFileName));
		}

		[Test]
		public void Can_return_file_as_string() {
			var tempFileName = Path.GetTempFileName();

			const string expected = "Hello world";
			using(var fileStream = File.OpenWrite(tempFileName)) {
				var sw = new StreamWriter(fileStream);
				sw.WriteLine(expected);
				sw.Flush();
			}

			var fileWrapper = new FileWrapper();

			Assert.That(fileWrapper.FileAsString(tempFileName), Is.EqualTo(expected));
		}

		[Test]
		public void Can_return_file_as_xml() {
			var tempFileName = Path.GetTempFileName();
			var fileWrapper = new FileWrapper();

			const string expected = "<xml>Hello world</xml>";
			var expectedXml = new XmlDocument();
			expectedXml.LoadXml(expected);
			using (var fileStream = File.OpenWrite(tempFileName)) {
				var sw = new StreamWriter(fileStream);
				sw.WriteLine(expected);
				sw.Flush();
			}

			Assert.That(fileWrapper.FileAsXml(tempFileName) as XmlDocument, Is.EqualTo(expectedXml));
		}

		[Test]
		public void Can_file_as_xml_if_invalid_xml_throws_error() {
			var tempFileName = Path.GetTempFileName();
			var fileWrapper = new FileWrapper();

			const string expected = "<>Hello world</xml>";
			using (var fileStream = File.OpenWrite(tempFileName)) {
				var sw = new StreamWriter(fileStream);
				sw.WriteLine(expected);
				sw.Flush();
			}

			Assert.Throws<XmlException>(() => fileWrapper.FileAsXml(tempFileName));
		}

		[Test]
		public void Can_write_data() {
			var tempFileName = Path.GetTempFileName();
			var fileWrapper = new FileWrapper();
			const string expected = "Foo man chu";
			fileWrapper.WriteFile(expected, tempFileName);

			var fileAsString = fileWrapper.FileAsString(tempFileName);

			Assert.That(fileAsString, Is.EqualTo(expected));
		}

		[Test]
		public void Can_delete_file() {
			var tempFileName = Path.GetTempFileName();
			var fileWrapper = new FileWrapper();
			fileWrapper.DeleteFile(tempFileName);

			Assert.That(fileWrapper.FileExists(tempFileName), Is.False);
		}
	}
}
using System.IO;
using System.Xml;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Writers
{
	public class Writer<T> : IWriter<T> where T : IHasId
	{
		protected string MainDirectory = "";
		private const string FILE_PATH_FORMAT = "{0}/{1}.xml";

		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<T> _serializer;

		public Writer(IFileWrapper fileWrapper, ISerializer<T> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public void CreateFile(T artist) {
			_fileWrapper.CreateDirectory(MainDirectory);
			string filePath = GetFilePath(artist.Id);
			if (_fileWrapper.FileExists(filePath))
				throw new ResourceExistsException(string.Format("{0} {1} already exists", typeof(T), artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;

			if (xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void UpdateFile(T artist) {
			string filePath = GetFilePath(artist.Id);
			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Artist {0} not found", artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;
			if (xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void DeleteFile(int id) {
			string filePath = GetFilePath(id);
			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Artist {0} not found", id));
			_fileWrapper.DeleteFile(filePath);
		}

		private string GetFilePath(int id) {
			return string.Format(FILE_PATH_FORMAT, MainDirectory, id);
		}
	} 

	public class ArtistWriter : Writer<Artist>
	{
		public ArtistWriter(IFileWrapper fileWrapper, ISerializer<Artist> serializer) 
			: base(fileWrapper, serializer) {
			MainDirectory = "C:/artist";
		}
	}
}
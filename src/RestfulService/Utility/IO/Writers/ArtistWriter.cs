using System.IO;
using System.Xml;
using RestfulService.Exceptions;
using RestfulService.Handlers;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Writers
{
	public class Writer<T> : IWriter<T> where T : IHasId
	{
		protected string MainDirectory = "";

		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<T> _serializer;

		public Writer(IFileWrapper fileWrapper, ISerializer<T> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public void CreateFile(T artist) {
			_fileWrapper.CreateDirectory(MainDirectory);
			string filePath = ServiceEnvironment.GetFilePath(artist.Id, MainDirectory);
			if (_fileWrapper.FileExists(filePath))
				throw new ResourceExistsException(string.Format("{0} {1} already exists", typeof(T), artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;

			if (xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void UpdateFile(T artist) {
			string filePath = ServiceEnvironment.GetFilePath(artist.Id, MainDirectory);
			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Artist {0} not found", artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;
			if (xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void DeleteFile(int id) {
			string filePath = ServiceEnvironment.GetFilePath(id, MainDirectory);
			_fileWrapper.DeleteFile(filePath);
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
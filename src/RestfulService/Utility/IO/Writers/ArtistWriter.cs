using System;
using System.IO;
using System.Xml;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Writers
{
	public class ArtistWriter : IWriter<Artist>
	{
		private const string ARTIST_DIRECTORY = "C:/artist";
		private const string FILE_PATH_FORMAT = "{0}/{1}.xml";

		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<Artist> _serializer;

		public ArtistWriter(IFileWrapper fileWrapper, ISerializer<Artist> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public void CreateFile(Artist artist) {
			_fileWrapper.CreateDirectory(ARTIST_DIRECTORY);
			string filePath = GetFilePath(artist.Id);
			if(_fileWrapper.FileExists(filePath))
				throw new ResourceExistsException(string.Format("Artist {0} already exists", artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;

			if (xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void UpdateFile(Artist artist) {
			string filePath = GetFilePath(artist.Id);
			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Artist {0} not found", artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;
			if(xPathNavigable != null)
				_fileWrapper.WriteFile(xPathNavigable.InnerXml, filePath);
		}

		public void DeleteFile(int id) {
			string filePath = GetFilePath(id);
			if (_fileWrapper.FileExists(filePath))
				_fileWrapper.DeleteFile(filePath);
		}

		private static string GetFilePath(int id) {
			return string.Format(FILE_PATH_FORMAT, ARTIST_DIRECTORY, id);
		}
	}
}
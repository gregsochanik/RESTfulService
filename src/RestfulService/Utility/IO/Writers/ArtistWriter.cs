using System;
using System.Xml;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Writers
{
	public class ArtistWriter : IWriter<Artist>
	{
		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<Artist> _serializer;

		public ArtistWriter(IFileWrapper fileWrapper, ISerializer<Artist> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public void CreateFile(Artist artist) {
			const string directory = "~/artist";
			_fileWrapper.CreateDirectory(directory);
			string filePath = string.Format("{0}/{1}.xml", directory, artist.Id);
			if(_fileWrapper.FileExists(filePath))
				throw new ResourceExistsException(string.Format("Artist {0} already exists", artist.Id));

			var xPathNavigable = _serializer.Serialize(artist) as XmlDocument;

			if (xPathNavigable != null)
				xPathNavigable.Save(filePath);
		}

		public void UpdateFile(Artist item) {
			throw new NotImplementedException();
		}

		public void DeleteFile(int id) {
			throw new NotImplementedException();
		}
	}
}
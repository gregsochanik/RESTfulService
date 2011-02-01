using System;
using System.IO;
using RestfulService.Handlers;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Readers
{
	public class ArtistReader : IReader<Artist>
	{
		private const string ARTIST_DIRECTORY = "C:/artist";
		private const string FILE_PATH_FORMAT = "{0}/{1}.xml";


		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<Artist> _serializer;

		public ArtistReader(IFileWrapper fileWrapper, ISerializer<Artist> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public Artist ReadFromFile(int id) {
			_fileWrapper.CreateDirectory(ARTIST_DIRECTORY);

			string filePath = String.Format(FILE_PATH_FORMAT, ARTIST_DIRECTORY, id);

			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Could not find this artist {0}", id));

			var fileAsXml = _fileWrapper.FileAsXml(filePath);

			return _serializer.DeSerialize(fileAsXml);
		}
	}
}
using System;
using System.IO;
using RestfulService.Handlers;
using RestfulService.Resources;
using RestfulService.Utility.Serialization;

namespace RestfulService.Utility.IO.Readers
{
	public class ArtistReader : IReader<Artist>
	{
		private readonly IFileWrapper _fileWrapper;
		private readonly ISerializer<Artist> _serializer;

		public ArtistReader(IFileWrapper fileWrapper, ISerializer<Artist> serializer) {
			_fileWrapper = fileWrapper;
			_serializer = serializer;
		}

		public Artist ReadFromFile(int id) {
			const string folderpath = "~/artist";
			_fileWrapper.CreateDirectory(folderpath);

			string filePath = String.Format("{0}/{1}.xml", folderpath, id);

			if (!_fileWrapper.FileExists(filePath))
				throw new FileNotFoundException(string.Format("Could not find this artist {0}", id));

			var fileAsXml = _fileWrapper.FileAsXml(filePath);

			return _serializer.DeSerialize(fileAsXml);
		}
	}
}
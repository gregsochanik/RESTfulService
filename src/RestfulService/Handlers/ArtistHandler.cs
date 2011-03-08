using System;
using System.IO;
using OpenRasta.Web;
using RestfulService.Handlers.ExceptionOutput;
using RestfulService.OperationInterceptors;
using RestfulService.Resources;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;

namespace RestfulService.Handlers
{
	public class ArtistHandler {
		private readonly IWriter<Artist> _writer;
		private readonly IReader<Artist> _reader;
		private readonly IOperationOutput _operationOutput;

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader, IOperationOutput operationOutput) {
			_writer = writer;
			_reader = reader;
			_operationOutput = operationOutput;
		}

		[HttpOperation("GET")]
		public OperationResult Get(Artist artist) {
			try {
				Artist fromFile = _reader.ReadFromFile(artist.Id);
				return new OperationResult.OK(fromFile);
			} catch(Exception ex) {
				return _operationOutput.HandleOutput(ex, artist);
			}
		}

		[HttpOperation("POST")]
		public OperationResult Post(Artist artist) {
			try {
				var uriString = ServiceEnvironment.CreateArtistUriString(artist.Id);
				_writer.CreateFile(artist);
				return new OperationResult.Created
				{RedirectLocation = new Uri(uriString), ResponseResource = artist};
			}catch(Exception ex) {
				return _operationOutput.HandleOutput(ex, artist);
			}
		}

		[HttpOperation("PUT")]
		public OperationResult Put(Artist artist) {
			try {
				var artistToUpdate = _reader.ReadFromFile(artist.Id);
				ReMapArtist(artist, artistToUpdate);

				_writer.UpdateFile(artistToUpdate);
				var uriString = ServiceEnvironment.CreateArtistUriString(artistToUpdate.Id);

				return new OperationResult.NoContent { ResponseResource = artistToUpdate, RedirectLocation = new Uri(uriString) };
			}catch(Exception ex) {
				return _operationOutput.HandleOutput(ex, artist);
			}
		}

		[HttpOperation("DELETE")]
		public OperationResult Delete(Artist artist) {
			try {
				string filePath = ServiceEnvironment.GetFilePath(artist.Id, "C:/artist");

				if (!_reader.Exists(filePath))
					throw new FileNotFoundException(string.Format("Could not find {0}", filePath));

				_writer.DeleteFile(artist.Id);
				return new OperationResult.NoContent();
			}
			catch(Exception ex) {
				return _operationOutput.HandleOutput(ex, artist);
			}
		}

		private static void ReMapArtist(Artist fromArtist, Artist toArtist) {
			if(!string.IsNullOrEmpty(fromArtist.Name))
				toArtist.Name = fromArtist.Name;
			if (!string.IsNullOrEmpty(fromArtist.Genre))
				toArtist.Genre = fromArtist.Genre;
		}
	}
}
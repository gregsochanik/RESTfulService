using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using log4net;
using OpenRasta.Web;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;

namespace RestfulService.Handlers
{
	public class ArtistHandler {
		private readonly IWriter<Artist> _writer;
		private readonly IReader<Artist> _reader;
		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly string _baseUrl = ConfigurationManager.AppSettings["Application.BaseUrl"];
		private const string URI_STRING_FORMAT = "{0}artist/{1}";

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader) {
			_writer = writer;
			_reader = reader;
		}

		[HttpOperation("GET")]
		public OperationResult Get(int artistId = 0) {

			try {
				var artist = _reader.ReadFromFile(artistId);
				return new OperationResult.OK(artist);

			} catch (FileNotFoundException) {
				return new OperationResult.NotFound { Description = String.Format("Artist {0} not found", artistId) };
			}
			catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("POST")]
		public OperationResult Post(Artist artist) {

			var uriString = CreateUriString(artist.Id);

			try {
				_writer.CreateFile(artist);
				return new OperationResult.Created { RedirectLocation = new Uri(uriString), ResponseResource = artist };
			} catch (ResourceExistsException) {
				return new OperationResult.Found { RedirectLocation = new Uri(uriString) };
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("PUT")]
		public OperationResult Put(int artistId, Artist artist) {

			try {
				var artistToUpdate = _reader.ReadFromFile(artistId);
				ReMapArtist(artist, artistToUpdate);

				_writer.UpdateFile(artistToUpdate);
				var uriString = CreateUriString(artistToUpdate.Id);
				
				return new OperationResult.NoContent { RedirectLocation = new Uri(uriString) };

			} catch (FileNotFoundException) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("DELETE")]
		public OperationResult Delete(int artistId = 0) {
			
			try {
				_writer.DeleteFile(artistId);
				return new OperationResult.NoContent();
			} catch (FileNotFoundException) {
				return new OperationResult.NotFound();
			} catch (IOException) {
				var uriString = CreateUriString(artistId);
				return new OperationResult.MethodNotAllowed(new Uri(uriString), HttpVerb.DELETE.ToString(), artistId);
			} catch (Exception ex) {
				_log.Error(ex);
				return new ServiceUnavailable();
			}
		}

		private static void ReMapArtist(Artist fromArtist, Artist toArtist)
		{
			if(!string.IsNullOrEmpty(fromArtist.Name))
				toArtist.Name = fromArtist.Name;
			if (!string.IsNullOrEmpty(fromArtist.Genre))
				toArtist.Genre = fromArtist.Genre;
		}

		private string CreateUriString(int artistId) {
			return string.Format(URI_STRING_FORMAT, _baseUrl, artistId);
		}
	}
}
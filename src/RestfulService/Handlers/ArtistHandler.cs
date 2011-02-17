using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using OpenRasta;
using OpenRasta.Web;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Validation;

namespace RestfulService.Handlers
{
	public class ArtistHandler {
		private readonly IWriter<Artist> _writer;
		private readonly IReader<Artist> _reader;
		private readonly ISelfValidator<Artist> _artistValidator;
		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private readonly string _baseUrl = ConfigurationManager.AppSettings["Application.BaseUrl"];
		private const string URI_STRING_FORMAT = "{0}artist/{1}";

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader, ISelfValidator<Artist> artistValidator) {
			_writer = writer;
			_reader = reader;
			_artistValidator = artistValidator;
		}

		[HttpOperation("GET")]
		public OperationResult Get(int artistId = 0) {
			if (artistId <= 0)
				return CreateBadRequestResponse("ArtistId parameter should be supplied");

			var uriString = string.Format(URI_STRING_FORMAT, _baseUrl, artistId);

			try {
				var artist = _reader.ReadFromFile(artistId);
				return new OperationResult.OK(new ArtistResponse { Response = artist, Link = new Link("artist", uriString, HttpVerb.DELETE) });

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
			var errors = _artistValidator.Validate(artist);
			if (errors.Count() > 0)
				return CreateBadRequestResponse("ArtistId parameter should be supplied", errors.ToList());

			var uriString = CreateUriString(artist.Id);

			try {
				_writer.CreateFile(artist);
				return new OperationResult.Created { RedirectLocation = new Uri(uriString), ResponseResource = new ArtistResponse { Response = artist, Link = new Link("artist", uriString, HttpVerb.DELETE) } };
			} catch (ResourceExistsException) {
				return new OperationResult.Found { RedirectLocation = new Uri(uriString) };
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("PUT")]
		public OperationResult Put(Artist artist, int artistId = -1) {
			if (artistId <= 0)
				return CreateBadRequestResponse("ArtistId parameter should be supplied");

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
			if (artistId <= 0)
				return CreateBadRequestResponse("ArtistId parameter should be supplied");
			
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

		private static OperationResult.BadRequest CreateBadRequestResponse(string message)
		{
			return new OperationResult.BadRequest
				{
					Title = message,
					Errors = new List<Error> { new Error { Title = message, Exception = new ArgumentException(), Message = message } }
				};
		}

		private static OperationResult.BadRequest CreateBadRequestResponse(string title, IList<Error> errors)
		{
			return new OperationResult.BadRequest
			{
				Title = title,
				Errors = errors 
			};
		}

		private string CreateUriString(int artistId) {
			return string.Format(URI_STRING_FORMAT, _baseUrl, artistId);
		}
	}
}
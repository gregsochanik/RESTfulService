using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
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

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader, ISelfValidator<Artist> artistValidator) {
			_writer = writer;
			_reader = reader;
			_artistValidator = artistValidator;
		}

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader, ISelfValidator<Artist> artistValidator, ILog log) {
			_writer = writer;
			_reader = reader;
			_artistValidator = artistValidator;
			_log = log;
		}

		[HttpOperation("GET")]
		public OperationResult Get(int artistId = 0) {
			if (artistId <= 0)
				return new OperationResult.BadRequest { Title="ArtistId parameter should be given" };

			var uriString = string.Format("{0}artist/{1}", _baseUrl, artistId);

			try {
				var artist = _reader.ReadFromFile(artistId);
				return new OperationResult.OK(new ArtistResponse { Response = artist, Link = new Link("artist", uriString, HttpVerb.DELETE) });

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound { Description = String.Format("Artist {0} not found", artistId) };
			}
			catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("POST")]
		public OperationResult Post(Artist artist) {
			var errors = artist.GetErrors(_artistValidator);
			if (errors.Count() > 0)
				return new OperationResult.BadRequest { Title = errors.FirstOrDefault().ErrorMessage, Description = errors.FirstOrDefault().ErrorMessage, };

			var uriString = string.Format("{0}artist/{1}",_baseUrl, artist.Id);

			try {
				_writer.CreateFile(artist);
				return new OperationResult.Created { RedirectLocation = new Uri(uriString) };

			} catch (ResourceExistsException rex) {
				return new OperationResult.Found { RedirectLocation = new Uri(uriString) };
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("PUT")]
		public OperationResult Put(int artistId, Artist artist) {
			if (artistId <= 0)
				return new OperationResult.BadRequest { Title = "ArtistId parameter should be given" };

			try {
				var artistToUpdate = _reader.ReadFromFile(artistId);

				if(!string.IsNullOrEmpty(artist.Name))
					artistToUpdate.Name = artist.Name;
				if (!string.IsNullOrEmpty(artist.Genre))
					artistToUpdate.Genre = artist.Genre;
				
				var uriString = string.Format("{0}artist/{1}", _baseUrl, artistToUpdate.Id);

				_writer.UpdateFile(artistToUpdate);
				return new OperationResult.NoContent { RedirectLocation = new Uri(uriString), ResponseResource = new ArtistResponse{Response= artistToUpdate}};

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}

		[HttpOperation("DELETE")]
		public OperationResult Delete(int artistId = 0) {
			if (artistId <= 0)
				return new OperationResult.BadRequest { Title = "ArtistId parameter should be given" };
			try {
				_writer.DeleteFile(artistId);
				return new OperationResult.NoContent();

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				_log.Error(ex);
				return new OperationResult.InternalServerError();
			}
		}
	}
}
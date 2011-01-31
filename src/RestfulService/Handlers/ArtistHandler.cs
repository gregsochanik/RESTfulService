using System;
using System.Configuration;
using System.IO;
using System.Linq;
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

		private readonly string _baseUrl = ConfigurationManager.AppSettings["Application.BaseUrl"];

		public ArtistHandler(IWriter<Artist> writer, IReader<Artist> reader, ISelfValidator<Artist> artistValidator) {
			_writer = writer;
			_reader = reader;
			_artistValidator = artistValidator;
		}

		public OperationResult Get(int artistId) {
			if (artistId <= 0)
				return new OperationResult.BadRequest { Title="ArtistId parameter should be given" };

			try {
				var artist = _reader.ReadFromFile(artistId);
				return new OperationResult.OK(new ArtistResponse {Output = artist});

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				return new OperationResult.InternalServerError();
			}
		}

		public OperationResult Post(Artist artist) {
			var errors = artist.GetErrors(_artistValidator);
			if (errors.Count() > 0)
				return new OperationResult.BadRequest { Title = errors.FirstOrDefault().ErrorMessage };

			var uriString = string.Format("{0}/artist/{1}",_baseUrl, artist.Id);

			try {
				_writer.CreateFile(artist);
				return new OperationResult.Created { CreatedResourceUrl = new Uri(uriString) };

			} catch (ResourceExistsException rex) {
				return new OperationResult.Found { RedirectLocation = new Uri(uriString) };
			} catch (Exception ex) {
				return new OperationResult.InternalServerError();
			}
		}

		public OperationResult Put(Artist artist) {
			var errors = artist.GetErrors(_artistValidator);
			if (errors.Count() > 0)
				return new OperationResult.BadRequest { Title = errors.FirstOrDefault().ErrorMessage };

			try {
				_writer.UpdateFile(artist);
				return new OperationResult.NoContent();

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				return new OperationResult.InternalServerError();
			}
		}

		public OperationResult Delete(int artistId) {
			if (artistId <= 0)
				return new OperationResult.BadRequest { Title = "ArtistId parameter should be given" };

			// delete an existing artist
			try {
				_writer.DeleteFile(artistId);
				return new OperationResult.NoContent();

			} catch (FileNotFoundException fex) {
				return new OperationResult.NotFound();
			} catch (Exception ex) {
				return new OperationResult.InternalServerError();
			}
		}
	}
}
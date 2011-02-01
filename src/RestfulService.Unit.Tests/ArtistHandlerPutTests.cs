using System;
using System.IO;
using NUnit.Framework;
using RestfulService.Handlers;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using RestfulService.Validation;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	public class ArtistHandlerPutTests {
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();

			_reader = MockRepository.GenerateStub<IReader<Artist>>();
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();
		}

		[Test]
		public void Should_return_BadRequest_if_parameters_are_missing() {
			var artistHandler = new ArtistHandler(_writer, _reader, new ArtistValidator());
			var operationResult = artistHandler.Put(0, new Artist { Id = 0, Genre = "", Name = "" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(400));
			Assert.That(operationResult.Title, Is.EqualTo("ArtistId parameter should be given"));
		}

		[Test]
		public void Should_return_InternalServerError_on_exception() {
			_reader.Stub(x => x.ReadFromFile(0)).IgnoreArguments().Throw(new Exception());
			var artistHandler = new ArtistHandler(_writer, _reader, new ArtistValidator());
			var operationResult = artistHandler.Put(1, new Artist { Id = 1, Genre = "r", Name = "r" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(500));
		}

		[Test]
		public void Should_return_NotFound_with_incorrect_artist() {
			_reader.Stub(x => x.ReadFromFile(0)).IgnoreArguments().Throw(new FileNotFoundException());
			var artistHandler = new ArtistHandler(_writer, _reader, new ArtistValidator());
			var operationResult = artistHandler.Put(1, new Artist { Id = 1, Genre = "r", Name = "r" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(404));
		}

		[Test]
		public void Should_return_NoContent_on_successful_update() {
			var artistHandler = new ArtistHandler(_writer, _reader, new ArtistValidator());
			var artist = new Artist { Id = 1, Genre = "r", Name = "r" };
			_reader.Stub(x => x.ReadFromFile(0)).IgnoreArguments().Return(artist);
			var operationResult = artistHandler.Put(1, artist);
			Assert.That(operationResult.StatusCode, Is.EqualTo(204));
		}
	}
}
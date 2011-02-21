using System;
using System.IO;
using NUnit.Framework;
using RestfulService.Handlers;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	public class ArtistHandlerDeleteTests {
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();

			_reader = new ArtistReader(_fileWrapper, _serializer);
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();
		}

		[Test]
		public void Should_return_NotAvailable_on_exception() {
			_writer.Stub(x => x.DeleteFile(1)).IgnoreArguments().Throw(new Exception());
			var artistHandler = new ArtistHandler(_writer, _reader);
			var operationResult = artistHandler.Delete(new Artist{Id = 1});
			Assert.That(operationResult.StatusCode, Is.EqualTo(503));
		}

		[Test]
		public void Should_return_NotFound_with_incorrect_artist() {
			_writer.Stub(x => x.DeleteFile(0)).IgnoreArguments().Throw(new FileNotFoundException());
			var artistHandler = new ArtistHandler(_writer, _reader);
			var operationResult = artistHandler.Delete(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(404));
		}

		[Test]
		public void Should_return_NoContent_on_successful_delete() {
			var artistHandler = new ArtistHandler(_writer, _reader);
			var operationResult = artistHandler.Delete(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(204));
		}

	}
}
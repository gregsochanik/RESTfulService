using System;
using System.IO;
using NUnit.Framework;
using RestfulService.Handlers;
using RestfulService.OperationInterceptors;
using RestfulService.Resources;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	public class ArtistHandlerDeleteTests {
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IOutputHandler _outputHandler;

		[SetUp]
		public void SetUp() {

			_reader = MockRepository.GenerateStub<IReader<Artist>>();
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();
			_outputHandler = new ExceptionOutputHandler();
		}

		[Test]
		public void Should_return_NoContent_on_successful_delete() {
			_reader.Stub(x => x.Exists("")).IgnoreArguments().Return(true);
			var artistHandler = new ArtistHandler(_writer, _reader, _outputHandler);
			var operationResult = artistHandler.Delete(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(204));
		}

		[Test]
		public void Should_return_NotAvailable_on_exception() {
			_reader.Stub(x => x.Exists("")).IgnoreArguments().Throw(new IOException());
			var artistHandler = new ArtistHandler(_writer, _reader, _outputHandler);
			var operationResult = artistHandler.Delete(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(405));
		}

		[Test]
		public void Should_return_NotFound_with_incorrect_artist() {
			_writer.Stub(x => x.DeleteFile(0)).IgnoreArguments().Throw(new FileNotFoundException());
			var artistHandler = new ArtistHandler(_writer, _reader, _outputHandler);
			var operationResult = artistHandler.Delete(new Artist{Id=1});
			Assert.That(operationResult.StatusCode, Is.EqualTo(404));
		}
	}
}
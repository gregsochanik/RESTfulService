using System;
using System.IO;
using NUnit.Framework;
using RestfulService.Handlers;
using RestfulService.OperationInterceptors;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	public class ArtistHandlerPutTests {
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;
		private IOutputHandler _outputHandler;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();

			_reader = MockRepository.GenerateStub<IReader<Artist>>();
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();

			_outputHandler = MockRepository.GenerateStub<IOutputHandler>();
		}

		[Test]
		public void Should_return_NoContent_on_successful_update() {
			var artistHandler = new ArtistHandler(_writer, _reader, _outputHandler);
			var artist = new Artist { Id = 1, Genre = "r", Name = "r" };
			_reader.Stub(x => x.ReadFromFile(0)).IgnoreArguments().Return(artist);
			var operationResult = artistHandler.Put(artist);
			Assert.That(operationResult.StatusCode, Is.EqualTo(204));
		}
	}
}
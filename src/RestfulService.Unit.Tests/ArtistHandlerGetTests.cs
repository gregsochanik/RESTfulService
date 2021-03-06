﻿using System;
using NUnit.Framework;
using RestfulService.Handlers;
using RestfulService.Handlers.ExceptionOutput;
using RestfulService.OperationInterceptors;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests {
	[TestFixture]
	public class ArtistHandlerGetTests
	{
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;
		private IOperationOutput _operationOutput;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();

			_reader = new ArtistReader(_fileWrapper, _serializer);
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();

			_operationOutput = new ExceptionOperationOutput();;
		}

		[Test]
		public void Get_should_return_NotFound_with_incorrect_artist() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(false);
			var artistHandler = new ArtistHandler(_writer, _reader, _operationOutput);
			var operationResult = artistHandler.Get(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(404));
		}

		[Test]
		public void Get_should_return_InternalServerError_on_exception() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Throw(new Exception());
			var artistHandler = new ArtistHandler(_writer, _reader, _operationOutput);
			var operationResult = artistHandler.Get(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(500));
		}

		[Test]
		public void Get_should_return_OK_if_found() {
			var reader = MockRepository.GenerateStub<IReader<Artist>>();
			reader.Stub(x => x.ReadFromFile(0)).IgnoreArguments().Return(new Artist());
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(true);

			var artistHandler = new ArtistHandler(_writer, reader, _operationOutput);
			var operationResult = artistHandler.Get(new Artist{Id=1});
			Assert.That(operationResult.StatusCode, Is.EqualTo(200));
		}

		[Test]
		public void Get_should_return_correct_artist_if_found() {
			var reader = MockRepository.GenerateStub<IReader<Artist>>();
			const string expected = "Meat Loaf";
			const string expectedGenre = "Rock";
			const int artistId = 1;
			reader.Stub(x => x.ReadFromFile(artistId)).IgnoreArguments().Return(new Artist { Id = artistId, Genre = expectedGenre, Name = expected });
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(true);

			var artistHandler = new ArtistHandler(_writer, reader, _operationOutput);
			var operationResult = artistHandler.Get(new Artist { Id = 1 });
			Assert.That(operationResult.StatusCode, Is.EqualTo(200));
			Assert.That(operationResult.ResponseResource, Is.Not.Null);
			Assert.That(((Artist)operationResult.ResponseResource).Name, Is.EqualTo(expected));
			Assert.That(((Artist)operationResult.ResponseResource).Id, Is.EqualTo(artistId));
			Assert.That(((Artist)operationResult.ResponseResource).Genre, Is.EqualTo(expectedGenre));
		}
	}
}

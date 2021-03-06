﻿using System;
using System.Configuration;
using NUnit.Framework;
using OpenRasta.Web;
using RestfulService.Exceptions;
using RestfulService.Handlers;
using RestfulService.Handlers.ExceptionOutput;
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
	public class ArtistHandlerPostTests
	{
		private IReader<Artist> _reader;
		private IWriter<Artist> _writer;
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;
		private readonly string _baseUrl = ConfigurationManager.AppSettings["Application.BaseUrl"];
		private IOperationOutput _operationOutput;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();

			_reader = new ArtistReader(_fileWrapper, _serializer);
			_writer = MockRepository.GenerateStub<IWriter<Artist>>();

			_operationOutput = new ExceptionOperationOutput();
		}
		
		[Test]
		public void Should_return_Created_on_successful_creation() {
			var artistHandler = new ArtistHandler(_writer, _reader, _operationOutput);
			var operationResult = artistHandler.Post(new Artist { Id = 1, Genre = "r", Name = "r" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(201));
			Assert.That(operationResult.RedirectLocation, Is.EqualTo(new Uri(_baseUrl + "artist/1")));
		}
		[Test]
		public void Should_return_InternalServerError_on_exception() {
			_writer.Stub(x => x.CreateFile(null)).IgnoreArguments().Throw(new Exception());
			var artistHandler = new ArtistHandler(_writer, _reader, _operationOutput);
			var operationResult = artistHandler.Post(new Artist { Id = 1, Genre = "r", Name = "r" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(500));
		}

		[Test]
		public void Should_return_Found_if_resource_exists() {
			_writer.Stub(x => x.CreateFile(null)).IgnoreArguments().Throw(new ResourceExistsException(""));
			var artistHandler = new ArtistHandler(_writer, _reader, _operationOutput);
			var operationResult = artistHandler.Post(new Artist { Id = 1, Genre = "r", Name = "r" });
			Assert.That(operationResult.StatusCode, Is.EqualTo(302));
			Assert.That(operationResult.RedirectLocation, Is.EqualTo(new Uri(_baseUrl + "artist/1")));
		}
	}
}
using System.Xml;
using NUnit.Framework;
using RestfulService.Exceptions;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests {
	[TestFixture]
	[Category("Integration")]
	public class ArtistWriterTests {

		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();
		}

		[Test]
		public void Should_create_folder_if_does_not_exist() {

			var artistReader = new ArtistWriter(_fileWrapper, _serializer);
			artistReader.CreateFile(new Artist());
			_fileWrapper.AssertWasCalled(x => x.CreateDirectory("C:/artist"));
		}

		[Test]
		public void Should_throw_exception_if_file_already_exists() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(true);
			var artistWriter = new ArtistWriter(_fileWrapper, _serializer);
			Assert.Throws<ResourceExistsException>(() => artistWriter.CreateFile(new Artist()));
		}

		[Test]
		public void Should_fire_serializer_with_correct_artist_if_file_does_not_exist() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(false);
			var artistWriter = new ArtistWriter(_fileWrapper, _serializer);
			var artist = new Artist(){Id=1, Genre="Rock",Name="Test"};
			artistWriter.CreateFile(artist);
			_serializer.AssertWasCalled(x => x.Serialize(artist));
		}

		[Test]
		public void Should_fire_save_with_correct_xml_if_file_does_not_exist() {
			var xPathNavigable = MockRepository.GenerateStub<XmlDocument>();
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(false);
			_serializer.Stub(x => x.Serialize(null)).IgnoreArguments().Return(xPathNavigable);

			var artistWriter = new ArtistWriter(_fileWrapper, _serializer);
			var artist = new Artist() { Id = 1, Genre = "Rock", Name = "Test" };
			artistWriter.CreateFile(artist);
			_fileWrapper.AssertWasCalled(x => x.WriteFile(Arg<string>.Is.Anything, Arg<string>.Is.Anything));
		}
	}
}

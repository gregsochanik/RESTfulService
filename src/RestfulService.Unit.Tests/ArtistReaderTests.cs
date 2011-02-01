using System.IO;
using NUnit.Framework;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
using RestfulService.Utility.IO.Writers;
using RestfulService.Utility.Serialization;
using Rhino.Mocks;

namespace RestfulService.Unit.Tests
{
	[TestFixture]
	public class ArtistReaderTests
	{
		private IFileWrapper _fileWrapper;
		private ISerializer<Artist> _serializer;

		[SetUp]
		public void SetUp() {
			_fileWrapper = MockRepository.GenerateStub<IFileWrapper>();
			_serializer = MockRepository.GenerateStub<ISerializer<Artist>>();
		}

		[Test]
		public void Should_create_folder_if_does_not_exist() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(true);

			var artistReader = new ArtistReader(_fileWrapper, _serializer);
			artistReader.ReadFromFile(1);
			_fileWrapper.AssertWasCalled(x=>x.CreateDirectory("C:/artist"));
		}

		[Test]
		public void Should_throw_exception_if_file_not_found() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(false);
			var artistReader = new ArtistReader(_fileWrapper, _serializer);
			Assert.Throws<FileNotFoundException>(() => artistReader.ReadFromFile(1));
		}

		[Test]
		[Category("Integration")]
		public void Can_read_from_output_folder() {
			var fileWrapper = new FileWrapper();
			var artistWriter = new ArtistWriter(fileWrapper, new XmlSerializer<Artist>());
			const int artistId = 1000001;

			artistWriter.DeleteFile(artistId);

			var testArtist = new Artist{Id=artistId, Name="Test Artist", Genre="Rock"};
			artistWriter.CreateFile(testArtist);
			var artistReader = new ArtistReader(fileWrapper, new XmlSerializer<Artist>());
			Artist artist = artistReader.ReadFromFile(artistId);
			Assert.That(artist.Id, Is.EqualTo(testArtist.Id));
			Assert.That(artist.Name, Is.EqualTo(testArtist.Name));
			Assert.That(artist.Genre, Is.EqualTo(testArtist.Genre));

			testArtist.Name = "Updated Artist";
			artistWriter.UpdateFile(testArtist);

			artist = artistReader.ReadFromFile(artistId);
			Assert.That(artist.Id, Is.EqualTo(testArtist.Id));
			Assert.That(artist.Name, Is.EqualTo(testArtist.Name));
			Assert.That(artist.Genre, Is.EqualTo(testArtist.Genre));

			artistWriter.DeleteFile(artistId);
		}
	}
}
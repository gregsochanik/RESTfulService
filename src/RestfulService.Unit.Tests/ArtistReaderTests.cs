using System.IO;
using NUnit.Framework;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
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
			_fileWrapper.AssertWasCalled(x=>x.CreateDirectory("~/artist"));
		}

		[Test]
		public void Should_throw_exception_if_file_not_found() {
			_fileWrapper.Stub(x => x.FileExists("")).IgnoreArguments().Return(false);
			var artistReader = new ArtistReader(_fileWrapper, _serializer);
			Assert.Throws<FileNotFoundException>(() => artistReader.ReadFromFile(1));
		}

		[Test]
		[Category("Integration")]
		[Ignore("Not implemented yet")]
		public void Can_read_from_output_folder() {
			// TODO: create new artist file
			var artistReader = new ArtistReader(new FileWrapper(), new XmlSerializer<Artist>());
			artistReader.ReadFromFile(1);
			// TODO: tear down artist file
		}
	}
}
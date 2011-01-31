using NUnit.Framework;
using RestfulService.Resources;
using RestfulService.Utility.IO;
using RestfulService.Utility.IO.Readers;
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
			_fileWrapper.AssertWasCalled(x => x.CreateDirectory("~/artist"));
		}

		[Test]
		[Ignore("Not implemented yet")]
		public void Can_write_to_output_folder() {
			var artist = new Artist{Id=1, Genre="rock", Name="Meat Loaf"};

			var writer = new ArtistWriter(_fileWrapper, _serializer);
			Assert.DoesNotThrow(() => writer.CreateFile(artist));
		}
	}
}

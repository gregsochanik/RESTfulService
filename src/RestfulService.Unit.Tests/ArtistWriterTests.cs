using NUnit.Framework;
using RestfulService.Resources;
using RestfulService.Utility.IO.Writers;

namespace RestfulService.Unit.Tests {
	[TestFixture]
	[Category("Integration")]
	public class ArtistWriterTests {
		[Test]
		public void Can_read_from_output_folder() {
			var artist = new Artist{Id=1, Genre="rock", Name="Meat Loaf"};

			var writer = new ArtistWriter();
			Assert.DoesNotThrow(() => writer.CreateFile(artist));
		}
	}
}

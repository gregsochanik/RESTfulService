using System;
using RestfulService.Handlers;
using RestfulService.Resources;

namespace RestfulService.Utility.IO.Writers
{
	public class ArtistWriter : IWriter<Artist>
	{
		public void CreateFile(Artist artist) {
			throw new NotImplementedException();
		}

		public void UpdateFile(Artist item) {
			throw new NotImplementedException();
		}

		public void DeleteFile(int id) {
			throw new NotImplementedException();
		}
	}
}
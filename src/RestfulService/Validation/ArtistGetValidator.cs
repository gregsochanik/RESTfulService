using System;
using System.Collections.Generic;
using OpenRasta;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class ArtistGetValidator : ISelfValidator<Artist> {
		public IEnumerable<Error> Validate(object item) {
			var artist = (Artist)item;

			if (artist.Id <= 0)
				yield return new Error {
				                       	Title = "ArtistId parameter missing",
				                       	Message = "ArtistId parameter missing",
				                       	Exception = new ArgumentException()
				                       };
		}
	}
}
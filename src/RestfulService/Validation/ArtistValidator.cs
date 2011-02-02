using System;
using System.Collections.Generic;
using OpenRasta;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class ArtistValidator : ISelfValidator<Artist>{
		public IEnumerable<Error> Validate(Artist item) {
			if(item.Id <= 0)
				yield return new Error{Title="ArtistId parameter missing", Message="ArtistId parameter missing",Exception=new ArgumentException()};
			if (string.IsNullOrEmpty(item.Name))
				yield return new Error { Title = "Name parameter missing", Message = "Name parameter missing", Exception = new ArgumentException() };
			if (string.IsNullOrEmpty(item.Genre))
				yield return new Error { Title = "Genre parameter missing", Message = "Genre parameter missing", Exception = new ArgumentException() };
			yield break;
		}
	}
}
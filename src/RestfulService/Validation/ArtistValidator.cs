using System.Collections.Generic;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class ArtistValidator : ISelfValidator<Artist>{
		public IEnumerable<ValidationResponse> Validate(Artist item) {
			if(item.Id <= 0)
				yield return new ValidationResponse(1001, "ArtistId parameter missing");
			if (string.IsNullOrEmpty(item.Name))
				yield return new ValidationResponse(1001, "Name parameter missing");
			if (string.IsNullOrEmpty(item.Genre))
				yield return new ValidationResponse(1001, "Genre parameter missing");
			yield break;
		}
	}
}
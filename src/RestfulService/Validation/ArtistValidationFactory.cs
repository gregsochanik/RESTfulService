using System;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class ArtistValidationFactory : IValidationFactory<Artist>
	{
		public ISelfValidator<Artist> GetValidator(string httpMethod) {
			switch(httpMethod) {
				case "GET":
					return new ArtistGetValidator();
				case "POST":
					return new ArtistPostValidator();
				case "PUT":
					return new ArtistGetValidator();
				case "DELETE":
					return new ArtistGetValidator();
				default:
					throw new ArgumentOutOfRangeException("httpMethod");
			}
		}
	}
}
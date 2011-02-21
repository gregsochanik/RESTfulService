using System;
using System.Collections.Generic;
using OpenRasta;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class ArtistValidationFactory : IValidationFactory<Artist> {
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

	public class SearchValidationFactory : IValidationFactory<SearchResource> {
		public ISelfValidator<SearchResource> GetValidator(string httpMethod) {
			switch (httpMethod) {
				case "GET":
					return new SearchResourceValidator();
				default:
					throw new ArgumentOutOfRangeException("httpMethod");
			}
		}
	}

	public class SearchResourceValidator : ISelfValidator<SearchResource> {
		public IEnumerable<Error> Validate(object item) {
			var searchResource = (SearchResource) item;

			if(string.IsNullOrEmpty(searchResource.Searchterm))
				yield return new Error { Title = "You need a search term", Message = "You need a search term", Exception = new ArgumentException() };
				yield break;
		}
	}
}
using System;
using System.Collections.Generic;
using OpenRasta;
using RestfulService.Resources;

namespace RestfulService.Validation
{
	public class SearchResourceValidator : ISelfValidator<SearchResource> {
		public IEnumerable<Error> Validate(object item, string httpMethod) {
			var searchResource = (SearchResource) item;

			if(string.IsNullOrEmpty(searchResource.Searchterm))
				yield return new Error { Title = "You need a search term", Message = "You need a search term", Exception = new ArgumentException() };
				yield break;
		}
	}
}
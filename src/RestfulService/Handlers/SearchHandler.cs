using System.Collections.Generic;
using OpenRasta.Web;
using RestfulService.Resources;

namespace RestfulService.Handlers
{
	public class SearchHandler
	{
		public OperationResult Get(SearchResource resource) {
			GetSearch(resource);
			return new OperationResult.OK(resource);
		}


		private static void GetSearch(SearchResource resource) {
			var results = new List<Result>
			{
				new Result
				{
					Type = "artist",
					Score = 1.234,
					Artist = new Artist {Genre = "Test", Name = "Greg", Id = 123}
				},
				new Result
				{
					Type = "artist",
					Score = 2.234,
					Artist = new Artist {Genre = "est", Name = "George", Id = 999}
				}
			};
			resource.TotalItems = 2;
			resource.SearchResults = results;
		}
	}
}
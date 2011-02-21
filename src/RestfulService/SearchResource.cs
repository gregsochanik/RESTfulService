using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using OpenRasta.Binding;

namespace RestfulService
{
	[Serializable]
	[KeyedValuesBinder]
	[XmlRoot("searchResults", Namespace = "")]
	public class SearchResource
	{
		[XmlElement("page")]
		public int Page { get; set; }
		[XmlElement("pageSize")]
		public int PageSize { get; set; }
		[XmlElement("totalItems")]
		public int TotalItems { get; set; }
		[XmlElement("searchTerm")]
		public string Searchterm { get; set; }
		[XmlElement("searchResult")]
		public List<Result> SearchResults { get; set; }
	}
}
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using OpenRasta.Binding;

namespace RestfulService.Resources
{
	[Serializable]
	[KeyedValuesBinder]
	[XmlRoot("searchResults", Namespace = "")]
	[DataContract(Name="searchResults")]
	public class SearchResource
	{
		[XmlElement("page")]
		[DataMember(Name = "page")]
		public int Page { get; set; }

		[XmlElement("pageSize")]
		[DataMember(Name = "pageSize")]
		public int PageSize { get; set; }

		[XmlElement("totalItems")]
		[DataMember(Name = "totalItems")]
		public int TotalItems { get; set; }

		[XmlElement("searchTerm")]
		[DataMember(Name = "searchTerm")]
		public string Searchterm { get; set; }

		[XmlElement("searchResult")]
		[DataMember(Name = "searchResult")]
		public List<Result> SearchResults { get; set; }
	}
}
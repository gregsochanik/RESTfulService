using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace RestfulService.Resources
{
	[XmlRoot("searchResult")]
	[DataContract(Name="searchResult")]
	public class Result
	{
		[XmlElement("type")]
		[DataMember(Name = "type")]
		public string Type { get; set; }

		[XmlElement("score")]
		[DataMember(Name = "score")]
		public double Score { get; set; }

		[XmlElement("artist")]
		[DataMember(Name = "artist")]
		public Artist Artist { get; set; }
	}
}
using System.Xml.Serialization;
using RestfulService.Resources;

namespace RestfulService
{
	[XmlRoot("searchResult")]
	public class Result
	{
		[XmlElement("type")]
		public string Type { get; set; }
		[XmlElement("score")]
		public double Score { get; set; }
		[XmlElement("artist")]
		public Artist Artist { get; set; }
	}
}
using System.Xml.Serialization;
using RestfulService.Handlers;

namespace RestfulService.Resources
{
	public class Link {
		[XmlAttribute("rel")]
		public string Rel { get; set; }
		[XmlAttribute("href")]
		public string Href { get; set; }
		[XmlAttribute("method")]
		public HttpVerb Method { get; set; }

		public Link() {}

		public Link(string rel, string href, HttpVerb method) {
			Rel = rel;
			Href = href;
			Method = method;
		}
	}
}
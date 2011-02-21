using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using OpenRasta;
using OpenRasta.Binding;

namespace RestfulService.Resources
{
	[Serializable]
	[KeyedValuesBinder]
	[XmlRoot("artist", Namespace = "")]
	public class Artist : IHasId
	{
		[XmlElement("id")]
		public int Id { get; set; }
		[XmlElement("name")]
		public string Name { get; set; }
		[XmlElement("genre")]
		public string Genre { get; set; }

	}

	public interface IHasId
	{
		int Id { get; set; }
	}
}
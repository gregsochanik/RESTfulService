using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using OpenRasta.Binding;

namespace RestfulService.Resources
{
	[Serializable]
	[KeyedValuesBinder]
	[XmlRoot("artist", Namespace = "")]
	[DataContract(Name = "artist")]
	public class Artist : IHasId
	{
		[XmlElement("id")]
		[DataMember(Name = "id")]
		public int Id { get; set; }
		[XmlElement("name")]
		[DataMember(Name = "name")]
		public string Name { get; set; }
		[XmlElement("genre")]
		[DataMember(Name = "genre")]
		public string Genre { get; set; }

		public Artist()
		{
			Id = 0;
			Name = string.Empty;
			Genre = string.Empty;
		}
	}

	public interface IHasId
	{
		int Id { get; set; }
	}
}
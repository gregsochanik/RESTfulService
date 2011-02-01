using System;
using System.Xml.Serialization;

namespace RestfulService.Resources
{
	[Serializable]
	public class ArtistResponse : Response, IApiResponse<Artist>
	{
		[XmlElement("artist", Order=1)]
		public Artist Response { get; set; }
	}

	public interface IApiResponse<T> {
		T Response { get; set; }
	}
}
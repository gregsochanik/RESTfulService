using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using RestfulService.Validation;

namespace RestfulService.Resources
{
	[Serializable]
	public class Artist
	{
		[XmlAttribute("id")]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Genre { get; set; }

		public IEnumerable<ValidationResponse> GetErrors(ISelfValidator<Artist> validator) {
			return validator.Validate(this);
		}
	}
}
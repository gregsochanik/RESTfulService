using System;
using System.Collections.Generic;
using OpenRasta;
using OpenRasta.Binding;
using RestfulService.Validation;

namespace RestfulService.Resources
{
	[Serializable]
	[KeyedValuesBinder]
	public class Artist : IHasId
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Genre { get; set; }

	}

	public interface IHasId
	{
		int Id { get; set; }
	}
}
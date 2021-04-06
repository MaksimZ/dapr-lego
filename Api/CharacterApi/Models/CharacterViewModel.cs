using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CharacterApi.Models
{
	public class CharacterViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Bio { get; set; }
		public string ArchiType { get; set; }
		public string LocationId { get; set; }
		public IEnumerable<string> KnownLocations { get; set; }
		public Uri Picture { get; set; }
	}
}
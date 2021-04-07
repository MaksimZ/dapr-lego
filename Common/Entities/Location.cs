using System.Collections.Generic;

namespace Common.Entities
{
	public class Location
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public IEnumerable<string> ConnectedLocations { get; set; }
	}
}
using System.Collections.Generic;

namespace Api.LocationApi.Models
{
	public class LocationCreateRequestModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<string> ConnectedLocations { get; set; }
		public IEnumerable<string> QuestsInLocation { get; set; }
	}
}
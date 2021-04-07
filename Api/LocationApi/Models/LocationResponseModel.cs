using System.Collections.Generic;
namespace Api.LocationApi.Models
{
    public class LocationResponseModel
    {
        public string Id {get;set;}
        public string Description {get;set;}
        public IEnumerable<string> ConnectedLocations {get;set;}

    }
}
using AirRouteFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AirRouteFinder.Controllers
{
    public class RouteController : ApiController
    {
        // GET/origin/destination
        [Route("GET/{origin}/{destination}")]
        public string Get(string origin, string destination)
        {
            var originRow = ApplicationDb.RunQueryAndReturnFirstRow($"Select * from airports.csv where [IATA 3] = \"{origin}\"");
            var destinationRow = ApplicationDb.RunQueryAndReturnFirstRow($"Select * from airports.csv where [IATA 3] = \"{destination}\"");

            if (originRow == null)
                return "Invalid Origin";

            if (destinationRow == null)
                return "Invalid Destination";

            string path = Logic.GetShortestPath(origin, destination);

            if (string.IsNullOrEmpty(path))
                return "No Route";
            else
                return path;
        }
    }
}

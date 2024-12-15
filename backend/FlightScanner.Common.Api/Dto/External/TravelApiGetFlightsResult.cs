using amadeus.resources;

namespace FlightScanner.Common.Api.Dto.External
{
    public class TravelApiGetFlightsResult
	{
        public class Links
        {
            public string self { get; set; }
        }

        public class MetaModel
        {
            public int count { get; set; }
            public Links links { get; set; }
        }
        public MetaModel meta { get; set; }
        public FlightOffer[] data { get; set; }
        public Dictionaries dictionaries { get; set; }
    }

}

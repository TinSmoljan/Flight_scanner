using System.Net;

namespace FlightScanner.Common.Api.Exceptions
{
	public class TravelApiException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public TravelApiException(string message, HttpStatusCode status)
			: base(message)
		{
			StatusCode = status;
		}
	}
}

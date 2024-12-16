using FlightScanner.Common.Api.Dto;

namespace FlightScanner.Services.Api.Services
{
	public interface IFlightHttpService
	{
		Task<IEnumerable<TravelApiDto>> GetFlightOffers(string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int adults, int children, int infants, string currency);
	}
}

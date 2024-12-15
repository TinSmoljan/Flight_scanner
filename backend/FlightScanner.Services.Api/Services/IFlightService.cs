using FlightScanner.Common.Api.Dto;
using FlightScanner.Common.Api.Models;

namespace FlightScanner.Services.Api.Services
{
	public interface IFlightService
	{
		Task<PagingResult<TravelApiDto>> GetFlightOffers(
			string originLocationCode,
			string destinationLocationCode,
			DateTime departureDate,
			DateTime? returnDate,
			int adults,
			int children,
			int infants,
			string currency,
			int page = 1,
			int perPage = 20,
			string sort = "price",
			string sortDirection = "asc");
	}
}

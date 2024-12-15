using FlightScanner.Common.Api.Dto;
using FlightScanner.Common.Api.Models;
using FlightScanner.Services.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightScanner.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FlightController : ControllerBase
	{
		private readonly IFlightService _flightService;

		public FlightController(IFlightService flightService)
		{
			_flightService = flightService;
		}

		[HttpGet]
		public async Task<PagingResult<TravelApiDto>> GetFlights(
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
			string sortDirection = "asc")
		
		{
			return await _flightService.GetFlightOffers(
				originLocationCode,
				destinationLocationCode,
				departureDate,
				returnDate,
				adults,
				children,
				infants,
				currency,
				page,
				perPage,
				sort,
				sortDirection);
		}
	}
}

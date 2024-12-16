using FlightScanner.Common.Api.Dto.External;
using FlightScanner.Services.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightScanner.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AirportController : ControllerBase
	{
		private readonly IAirportService _airportService;

		public AirportController(IAirportService airportService)
		{
			_airportService = airportService;
		}

		[HttpGet]
		public async Task<IEnumerable<AirportResult>> GetAirports(string contains)
		{
			return await _airportService.GetAirports(contains);
		}
	}
}

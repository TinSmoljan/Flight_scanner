using FlightScanner.Common.Api.Dto.External;
using FlightScanner.Services.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightScanner.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AirportController : ControllerBase
	{
		private readonly IAirportService _airportService;

		public AirportController(IAirportService airportService)
		{
			_airportService = airportService;
		}

		[HttpGet]
		public async Task<IEnumerable<AirportResult>> GetAirports()
		{
			return await _airportService.GetAirports();
		}
	}
}

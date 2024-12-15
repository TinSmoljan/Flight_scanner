using FlightScanner.Common.Api.Dto.External;

namespace FlightScanner.Services.Api.Services
{
	public interface IAirportHttpService
	{
		Task<IEnumerable<AirportResult>> GetAirports();
	}
}

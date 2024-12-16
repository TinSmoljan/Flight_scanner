using FlightScanner.Services.Api.Services;

namespace FlightScanner.Api.Initilazers
{
	public class AirportCacheInitializer : IHostedService
	{
		private readonly IAirportService _airportService;

		public AirportCacheInitializer(IAirportService airportService)
		{
			_airportService = airportService;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			await _airportService.GetAirports();
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}

using FlightScanner.Common.Api.Dto.External;

namespace FlightScanner.Services.Api.Services
{
	public class AirportService : IAirportService
	{
		private readonly IAirportHttpService _airportHttpService;
		private readonly ICacheService _cacheService;

		public AirportService(ICacheService cacheService, IAirportHttpService airportHttpService)
		{
			_cacheService = cacheService;
			_airportHttpService = airportHttpService;
		}

		public async Task<IEnumerable<AirportResult>> GetAirports()
		{
			var cacheKey = $"airports";
			var cachedOffers = await _cacheService.GetItem<IEnumerable<AirportResult>>(cacheKey);

			if (cachedOffers != null)
			{
				return cachedOffers;
			}

			var offers = await _airportHttpService.GetAirports();
			await _cacheService.SetItem(cacheKey, offers);

			return await _airportHttpService.GetAirports();
		}
	}
}

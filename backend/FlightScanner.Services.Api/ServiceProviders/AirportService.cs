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
			var sortedOffers = offers.OrderBy(a => a.Name);
			await _cacheService.SetItem(cacheKey, sortedOffers, slidingExpirtion: new TimeSpan(1, 0, 0, 0));

			return sortedOffers;
		}

		public async Task<IEnumerable<AirportResult>> GetAirports(string contains)
		{
			var cacheKey = $"airports";
			var cachedOffers = await _cacheService.GetItem<IEnumerable<AirportResult>>(cacheKey);

			if (cachedOffers == null)
			{
				await Task.Delay(1000);
				return await GetAirports(contains);
			}

			var offers = cachedOffers.Where(
				a => a.Name.StartsWith(contains, StringComparison.OrdinalIgnoreCase)
				|| a.Code.StartsWith(contains, StringComparison.OrdinalIgnoreCase)).ToList();
			return offers;
		}
	}
}

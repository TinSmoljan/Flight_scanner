using FlightScanner.Common.Api.Dto;
using FlightScanner.Common.Api.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightScanner.Services.Api.Services
{
	public class FlightService : IFlightService
	{
		private readonly IFlightHttpService _flightHttpService;
		private readonly ICacheService _cacheService;

		public FlightService(ICacheService cacheService, IFlightHttpService flightHttpService)
		{
			_flightHttpService = flightHttpService;
			_cacheService = cacheService;
		}

		public async Task<PagingResult<TravelApiDto>> GetFlightOffers(
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
			var cacheKey = $"{originLocationCode}-{destinationLocationCode}-{departureDate:yyyy-MM-dd}-{returnDate:yyyy-MM-dd}-{adults}-{children}-{infants}-{currency}";
			var cachedOffers = await _cacheService.GetItem<IEnumerable<TravelApiDto>>(cacheKey);

			if (cachedOffers != null)
			{
				return new PagingResult<TravelApiDto>(cachedOffers.Count(), page, perPage, sort, sortDirection, cachedOffers);
			}

			var offers = await _flightHttpService.GetFlightOffers(originLocationCode, destinationLocationCode, departureDate, returnDate, adults, children, infants, currency);
			await _cacheService.SetItem(cacheKey, offers);

			return new PagingResult<TravelApiDto>(offers.Count(), page, perPage, sort, sortDirection, offers);
		}
	}
}

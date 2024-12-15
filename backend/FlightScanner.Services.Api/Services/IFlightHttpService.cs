using FlightScanner.Common.Api.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlightScanner.Services.Api.Services.FlightHttpService;

namespace FlightScanner.Services.Api.Services
{
	public interface IFlightHttpService
	{
		Task<IEnumerable<TravelApiDto>> GetFlightOffers(string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int adults, int children, int infants, string currency);
	}
}

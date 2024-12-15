﻿using FlightScanner.Common.Api.Dto.External;

namespace FlightScanner.Services.Api.Services
{
	public interface IAirportService
	{
		Task<IEnumerable<AirportResult>> GetAirports();
	}
}
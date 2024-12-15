using AngleSharp;
using FlightScanner.Common.Api.Dto.External;

namespace FlightScanner.Services.Api.Services
{
	public class AirportHttpService : IAirportHttpService
	{
		private readonly string _listOfAirportsUrl = "List_of_airports_by_IATA_airport_code:_A";

		private readonly HttpClient _http;
		public AirportHttpService(IHttpClientFactory httpFactory)
		{
			_http = httpFactory.CreateClient("Wikipedia");
		}

		public async Task<IEnumerable<AirportResult>> GetAirports()
		{
			string html = await _http.GetStringAsync(_listOfAirportsUrl);
			List<AirportResult> airportList = await AirportHtmlParse(html);

			return airportList;
		}

		private async Task<List<AirportResult>> AirportHtmlParse(string html)
		{
			var context = BrowsingContext.New();
			var document = await context.OpenAsync(req => req.Content(html));

			var airportList = new List<AirportResult>();

			var tableRows = document.QuerySelectorAll("table.wikitable tbody tr");

			foreach (var row in tableRows)
			{
				var cells = row.QuerySelectorAll("td");
				if (cells.Length >= 2)
				{
					string code = cells[0].TextContent.Trim();
					string name = cells[2].TextContent.Trim();

					if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
					{
						airportList.Add(new AirportResult { Code = code, Name = name });
					}
				}
			}

			return airportList;
		}
	}
}

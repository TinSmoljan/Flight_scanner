using AngleSharp;
using FlightScanner.Common.Api.Dto.External;

namespace FlightScanner.Services.Api.Services
{
	public class AirportHttpService : IAirportHttpService
	{
		private readonly HttpClient _http;
		private readonly string _listOfAirportsUrl = "List_of_airports_by_IATA_airport_code:_";

		public AirportHttpService(IHttpClientFactory httpFactory)
		{
			_http = httpFactory.CreateClient("Wikipedia");
		}

		public async Task<List<AirportResult>> GetAirports()
		{
			var airportList = new List<AirportResult>();

			foreach (char letter in Enumerable.Range('A', 26).Select(i => (char)i))
			{
				string url = _listOfAirportsUrl + letter;
				string html = await _http.GetStringAsync(url);

				var parsedAirports = await ParseAirportsFromHtml(html);
				airportList.AddRange(parsedAirports);
			}

			return airportList;
		}

		private async Task<List<AirportResult>> ParseAirportsFromHtml(string html)
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

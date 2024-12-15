using FlightScanner.Common.Api.Dto;
using FlightScanner.Common.Api.Dto.External;
using FlightScanner.Common.Api.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace FlightScanner.Services.Api.Services
{
	public class FlightHttpService : IFlightHttpService
	{
		private readonly string _apiKey;
		private readonly string _apiSecret;
		private string _bearerToken;
		private readonly HttpClient _http;

		public FlightHttpService(IHttpClientFactory httpFactory, IConfiguration configuration)
		{
			_apiKey = configuration["AmadeusAPI:APIKey"] ?? string.Empty;
			_apiSecret = configuration["AmadeusAPI:ApiSecret"] ?? string.Empty;
			_http = httpFactory.CreateClient("TravelAPI");


			ConnectOAuth().GetAwaiter().GetResult();
		}

		public async Task<IEnumerable<TravelApiDto>> GetFlightOffers(string originLocationCode, string destinationLocationCode, DateTime departureDate, DateTime? returnDate, int adults, int children, int infants, string currency)
		{
			List<TravelApiDto> flightOffers = new List<TravelApiDto>();

			var returnDateStr = returnDate.HasValue ? returnDate.Value.ToString("yyyy-MM-dd") : string.Empty;

			var requestUrl = $"/v2/shopping/flight-offers" +
				$"?originLocationCode={originLocationCode}" +
				$"&destinationLocationCode={destinationLocationCode}" +
				$"&departureDate={departureDate.ToString("yyyy-MM-dd")}" +
				$"{(string.IsNullOrEmpty(returnDateStr) ? "" : $"&returnDate={returnDateStr}")}" +
				$"&adults={adults}" +
				$"&children={children}" +
				$"&infants={infants}" +
				$"&nonStop=false" +
				$"&currencyCode={currency}" +
				$"&max=250";

			var apiResult = await SendGetRequest<TravelApiGetFlightsResult>(requestUrl);

			if (apiResult.meta.count == 0)
			{
				return flightOffers;
			}
			foreach (var offer in apiResult.data)
			{
				flightOffers.Add(new TravelApiDto
				{
					DepartedAirport = originLocationCode,
					ArrivedAirport = destinationLocationCode,
					DepartureDate = departureDate,
					ReturnDate = returnDate,
					Adults = adults,
					Currency = currency,
					Price = double.Parse(offer.price.grandTotal),
					DepartedLayovers = offer.itineraries[0].segments.Count - 1,
					ReturnLayovers = returnDate.HasValue ? offer.itineraries[1].segments.Count - 1 : 0
				});
			}

			return flightOffers;
		}

		private async Task<T> SendGetRequest<T>(string requestUrl)
		{
			ConfigBearerTokenHeader();

			var response = await _http.GetAsync(requestUrl);
			if (!response.IsSuccessStatusCode)
			{
				throw new TravelApiException("Failed to get flight offers from Amadeus API", response.StatusCode);
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			var apiResult = JsonConvert.DeserializeObject<T>(responseContent);

			return apiResult;
		}

		private async Task ConnectOAuth()
		{
			var message = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token");
			message.Content = new StringContent(
				$"grant_type=client_credentials&client_id={_apiKey}&client_secret={_apiSecret}",
				Encoding.UTF8, "application/x-www-form-urlencoded"
			);

			var result = await _http.SendAsync(message);
		    var content = await result.Content.ReadAsStringAsync();
			var oauthResults = JsonConvert.DeserializeObject<TravelApiOauthResult>(content);

			_bearerToken = oauthResults.access_token;
		}

		private void ConfigBearerTokenHeader()
		{
			_http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_bearerToken}");
		}
	}
}

namespace FlightScanner.Common.Api.Dto
{
	public class TravelApiDto
	{
		public required string DepartedAirport { get; set; }
		public required string ArrivedAirport { get; set; }
		public DateTime DepartureDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public int DepartedLayovers { get; set; }
		public int? ReturnLayovers { get; set; }
		public int Adults { get; set; }
		public int Children { get; set; }
		public int Infants { get; set; }
		public required string Currency { get; set; }
		public double Price { get; set; }
	}
}

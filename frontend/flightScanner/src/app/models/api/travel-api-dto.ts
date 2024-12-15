export interface TravelApiDto {
  departedAirport: string;
  arrivedAirport: string;
  departureDate: string;
  returnDate?: string;
  departedLayovers: number;
  returnLayovers?: number;
  adults: number;
  children: number;
  infants: number;
  currency: string;
  price: number;
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagingResult } from '../../models/paging-result';
import { TravelApiDto } from '../../models/api/travel-api-dto';
import { Airport } from '../../models/airport';

@Injectable({
  providedIn: 'root',
})
export class FlightService {
  private readonly apiUrl = 'https://localhost:7130/Flight';
  private readonly airportsDataUrl = '/assets/airports.json';

  constructor(private readonly http: HttpClient) {}

  getAirports(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.airportsDataUrl);
  }

  getFlights(
    origin: string,
    destination: string,
    departureDate: string,
    returnDate: string | null,
    adults: number,
    children: number,
    infants: number,
    currency: string,
    page: number,
    perPage: number,
    sort: string,
    sortDirection: string
  ): Observable<PagingResult<TravelApiDto>> {
    console.log('sort', sort);
    let params = new HttpParams()
      .set('originLocationCode', origin)
      .set('destinationLocationCode', destination)
      .set('departureDate', departureDate)
      .set('adults', adults)
      .set('children', children)
      .set('infants', infants)
      .set('currency', currency)
      .set('page', page)
      .set('perPage', perPage)
      .set('sort', sort)
      .set('sortDirection', sortDirection);

    if (returnDate) {
      params = params.set('returnDate', returnDate);
    }

    return this.http.get<PagingResult<TravelApiDto>>(this.apiUrl, { params });
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Airport } from '../../models/airport';

@Injectable({
  providedIn: 'root',
})
export class AirportService {
  private readonly apiUrl = 'https://localhost:7130/api/Airport';

  constructor(private readonly http: HttpClient) {}

  getAirports(contains: string): Observable<Airport[]> {
    let params = new HttpParams().set('contains', contains);
    return this.http.get<Airport[]>(this.apiUrl, { params });
  }
}

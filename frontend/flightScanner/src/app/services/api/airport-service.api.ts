import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Airport } from '../../models/airport';

@Injectable({
  providedIn: 'root',
})
export class AirportService {
  private readonly apiUrl = 'https://localhost:7130/Airport';

  constructor(private readonly http: HttpClient) {}

  getAirports(): Observable<Airport[]> {
    return this.http.get<Airport[]>(this.apiUrl);
  }
}

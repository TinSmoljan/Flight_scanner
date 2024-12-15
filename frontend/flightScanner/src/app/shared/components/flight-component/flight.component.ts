import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FlightService } from '../../../services/api/flight-service.api';
import { Airport } from '../../../models/airport';
import { TravelApiDto } from '../../../models/api/travel-api-dto';
import { PagingResult } from '../../../models/paging-result';
import { AirportService } from '../../../services/api/airport-service.api';

@Component({
  selector: 'app-flight',
  templateUrl: './flight.component.html',
  styleUrls: ['./flight.component.scss'],
})
export class FlightComponent implements OnInit {
  airports: Airport[] = [];
  flights?: PagingResult<TravelApiDto> = undefined;
  loading = false;
  initialLoad = true;

  origin = '';
  destination = '';
  departureDate: string = '';
  returnDate: string = '';
  adults = 1;
  children = 0;
  infants = 0;
  currency = 'USD';
  page = 1;
  perPage = 10;
  sort = 'Price';
  sortDirection = 'asc';

  today: string = new Date().toISOString().split('T')[0];

  constructor(
    private flightService: FlightService,
    private airportService: AirportService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadAirports();
    this.route.queryParams.subscribe((params) => {
      this.origin = params['originLocationCode'] || this.origin;
      this.destination = params['destinationLocationCode'] || this.destination;
      this.departureDate = params['departureDate'] || this.departureDate;
      this.returnDate = params['returnDate'] || this.returnDate;
      this.adults = params['adults'] || this.adults;
      this.children = params['children'] || this.children;
      this.infants = params['infants'] || this.infants;
      this.currency = params['currency'] || this.currency;
      this.page = parseInt(params['page'], 10) || this.page;
      this.perPage = parseInt(params['perPage'], 10) || this.perPage;
      this.sort = params['sort'] || this.sort;
      this.sortDirection = params['sortDirection'] || this.sortDirection;

      if (this.isFormValid() && this.initialLoad) {
        this.searchFlights(false);
        this.initialLoad = false;
      }
    });
  }

  loadAirports(): void {
    this.airportService.getAirports().subscribe((data: Airport[]) => {
      this.airports = data;
    });
  }

  toggleSort(column: string): void {
    if (this.sort === column) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sort = column;
      this.sortDirection = 'asc';
    }
    this.searchFlights();
  }

  searchFlights(updateUrl: boolean = true): void {
    if (!this.isFormValid()) {
      return;
    }

    this.loading = true;

    const params = {
      originLocationCode: this.origin,
      destinationLocationCode: this.destination,
      departureDate: this.departureDate,
      returnDate: this.returnDate,
      adults: this.adults,
      children: this.children,
      infants: this.infants,
      currency: this.currency,
      page: this.page,
      perPage: this.perPage,
      sort: this.sort,
      sortDirection: this.sortDirection,
    };

    if (updateUrl) {
      this.router.navigate([], {
        relativeTo: this.route,
        queryParams: params,
        queryParamsHandling: 'merge',
      });
    }

    this.flightService
      .getFlights(
        this.origin,
        this.destination,
        this.departureDate,
        this.returnDate,
        this.adults,
        this.children,
        this.infants,
        this.currency,
        this.page,
        this.perPage,
        this.sort,
        this.sortDirection
      )
      .subscribe(
        (response: PagingResult<TravelApiDto>) => {
          this.flights = response;
          this.loading = false;
        },
        () => {
          this.loading = false;
        }
      );
  }

  isFormValid(): boolean {
    return (
      !!this.origin &&
      !!this.destination &&
      !!this.departureDate &&
      this.departureDate >= this.today &&
      (!this.returnDate || this.returnDate >= this.departureDate)
    );
  }

  getTotalPages(): number {
    return Math.ceil((this.flights?.count ?? 0) / this.perPage);
  }

  changePage(direction: number): void {
    this.page += direction;
    this.searchFlights();
  }

  getAirportName(code: string): string {
    const airport = this.airports.find(a => a.code === code);
    return airport ? airport.name : code;
  }
}

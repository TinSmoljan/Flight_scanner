import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { FlightService } from './services/api/flight-service.api';
import { FlightComponent } from './shared/components/flight-component/flight.component';
import { FormsModule } from '@angular/forms';
import { AirportService } from './services/api/airport-service.api';

@NgModule({
  declarations: [
    AppComponent,
    FlightComponent,
  ],
  imports: [
    FormsModule,
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
  ],
  providers: [FlightService, AirportService],
  bootstrap: [AppComponent]
})
export class AppModule { }

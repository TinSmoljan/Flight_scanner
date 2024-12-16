import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { FlightService } from './services/api/flight-service.api';
import { FlightComponent } from './shared/components/flight-component/flight.component';
import { FormsModule } from '@angular/forms';
import { AirportService } from './services/api/airport-service.api';
import { ErrorInterceptor } from './interceptors/error-interceptor';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [AppComponent, FlightComponent],
  imports: [
    FormsModule,
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    MatSnackBarModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatAutocompleteModule,
    MatFormFieldModule,
    ReactiveFormsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    FlightService,
    AirportService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

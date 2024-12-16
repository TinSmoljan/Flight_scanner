import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpErrorResponse,
} from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private snackBar: MatSnackBar) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = 'An unknown error occurred!';
        if (error.error instanceof ErrorEvent) {
          errorMsg = `Error: ${error.error.message}`;
        } else {
          if (error.error?.detail) {
            errorMsg = error.error.detail;
          } else {
            errorMsg = `Error Code: ${error.status}\nMessage: ${error.message}`;
          }
        }

        setTimeout(() => {
          this.snackBar.open(errorMsg, 'Close', {
            duration: 15000,
            verticalPosition: 'top',
            horizontalPosition: 'center',
          });
        });
        return throwError(errorMsg);
      })
    );
  }
}

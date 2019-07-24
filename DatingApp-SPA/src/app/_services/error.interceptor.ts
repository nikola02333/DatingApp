import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any> , next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
         catchError(error => {
                if (error instanceof HttpErrorResponse) {
                 const aplicationError = error.headers.get('Aplication-Error');
                 console.log(aplicationError);
                 return throwError(aplicationError);
                }
         })
        );
    }
    }

export const ErrorIntercepotorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
};

import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { Navigation, NavigationExtras, Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService); // Assuming you have a ToastService for notifications
  const router = inject(Router); // Assuming you have a Router for navigation
  return next(req).pipe(
    catchError((error) => {
      switch (error.status) {
        case 400:
          // Handle 400 Bad Request
          if (error.error.errors) {
            const modalStateErrors = [];
            let errors = error.error.errors;
            for (const key in errors) {
              if (errors[key]) {
                modalStateErrors.push(errors[key]);
              }
            }
            throw modalStateErrors.flat();
          } else {
            toast.error(error.error, error.status);
          }
          break;
        case 401:
          // Handle 401 Unauthorized
          toast.error('Unauthorized');
          break;
        case 404:
          router.navigateByUrl('/not-found'); // Navigate to Not Found page
          break;
        case 500:
          const navigationExtras: NavigationExtras = {state: {error: error.error}};
          router.navigateByUrl('/server-error', navigationExtras); // Navigate to Server Error page
          break;
        default:
          toast.error('Something went wrong.');
          break;
      }
      return throwError(() => error);
    })
  );
};

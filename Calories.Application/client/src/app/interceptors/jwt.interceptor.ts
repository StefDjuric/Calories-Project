import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../services/account.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);

  if (accountService.accessToken() || localStorage.getItem('accessToken')) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.accessToken()}`,
      },
    });
  }
  return next(req);
};

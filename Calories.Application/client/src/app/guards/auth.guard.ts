import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const toastr = inject(ToastrService);
  const router = inject(Router);

  if (localStorage.getItem('accessToken')) return true;
  else {
    toastr.error('You must login before visiting this route.');
    router.navigateByUrl('/login');
    return false;
  }
};

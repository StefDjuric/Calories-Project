import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const guestGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  if (localStorage.getItem('accessToken')) {
    const role = accountService.getRoleFromToken();
    if (role === 'User Manager') {
      toastr.error('Can not go to guest routes while logged in');
      router.navigateByUrl('manager-dashboard');
      return false;
    } else {
      toastr.error('Can not go to guest routes while logged in');
      router.navigateByUrl(`${role?.toLowerCase()}-dashboard`);
    }
  }
  return true;
};

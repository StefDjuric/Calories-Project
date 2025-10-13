import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { ToastrService } from 'ngx-toastr';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  const allowedRoles = route.data['roles'] as string[];
  const userRole = accountService.getRoleFromToken();

  console.log('User role from token:', userRole);
  console.log('Allowed roles:', allowedRoles);

  if (userRole && allowedRoles.includes(userRole)) {
    return true;
  }

  toastr.error('Access denied');
  return false;
};

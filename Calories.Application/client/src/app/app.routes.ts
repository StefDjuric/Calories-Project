import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { UserDashboardComponent } from './components/user-dashboard/user-dashboard.component';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';
import { ManagerDashboardComponent } from './components/manager-dashboard/manager-dashboard.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  {
    path: 'user-dashboard',
    component: UserDashboardComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['User'] },
  },
  {
    path: 'manager-dashboard',
    component: ManagerDashboardComponent,
    canActivate: [authGuard, roleGuard],
    data: { roles: ['User Manager'] },
  },

  { path: '**', redirectTo: '' },
];

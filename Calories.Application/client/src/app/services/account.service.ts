import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { LoginModel } from '../models/LoginModel';
import { map } from 'rxjs';
import { RegisterModel } from '../models/RegisterModel';
import { User } from '../models/User';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;
  private toastr = inject(ToastrService);
  private router = inject(Router);
  accessToken = signal<string | null>(null);

  login(model: LoginModel) {
    return this.http
      .post(`${this.baseUrl}/account/login`, model, { responseType: 'text' })
      .pipe(
        map((token) => {
          if (token) {
            localStorage.setItem('accessToken', token);
            this.accessToken.set(token);
          }
          return token;
        })
      );
  }

  register(model: RegisterModel) {
    return this.http.post<User>(`${this.baseUrl}/account/register`, model, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  logout() {
    localStorage.removeItem('accessToken');
    this.accessToken.set(null);
    this.toastr.success('Successfully logged out.');
    this.router.navigateByUrl('/');
  }

  private decodeJwtToken(token: string) {
    try {
      const base64Ul = token.split('.')[1];
      const base64 = base64Ul.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((char) => '%' + char.charCodeAt(0).toString(16).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Error decoding JWT token: ', error);
      return null;
    }
  }

  getUserIdFromToken(): string | null {
    const token = this.accessToken();
    if (token) {
      const decodedToken = this.decodeJwtToken(token);
      return decodedToken?.nameid || null;
    }
    return null;
  }

  getUsernameFromToken(): string | null {
    const token = this.accessToken();
    if (token) {
      const decodedToken = this.decodeJwtToken(token);
      return decodedToken?.unique_name || null;
    }
    return null;
  }

  getRoleFromToken(): string | null {
    const token = this.accessToken();
    if (token) {
      const decodedToken = this.decodeJwtToken(token);
      return decodedToken?.role || null;
    }
    return null;
  }
}

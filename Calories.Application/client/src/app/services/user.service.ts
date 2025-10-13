import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  getUserById(userId: string) {
    return this.http.get<User>(`${this.baseUrl}/users/${userId}`);
  }

  getAllUsers() {
    return this.http.get<User[]>(`${this.baseUrl}/users/admin`);
  }

  adminCreateUser(model: User) {
    return this.http.post<User>(`${this.baseUrl}/users/admin`, model);
  }

  adminUpdateUser(model: User, userId: string) {
    return this.http.put<void>(`${this.baseUrl}/users/admin/${userId}`, model);
  }

  editUserData(model: User) {
    return this.http.put<void>(`${this.baseUrl}/users`, model);
  }

  adminDeleteUser(userId: string) {
    return this.http.delete<void>(`${this.baseUrl}/users/admin/${userId}`);
  }
}

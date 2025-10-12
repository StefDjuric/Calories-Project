import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Meal } from '../models/MealModel';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class MealsService {
  private http = inject(HttpClient);
  private baseUrl = environment.baseUrl;

  adminGetMealsForUser(userId: string) {
    return this.http.get<Meal[]>(
      `${this.baseUrl}/meals/admin/for-user/${userId}`
    );
  }

  getMealsForUser(
    fromDate?: string,
    toDate?: string,
    fromTime?: string,
    toTime?: string
  ) {
    let params = new HttpParams();
    if (fromDate) params = params.set('fromDate', fromDate);
    if (toDate) params = params.set('toDate', toDate);
    if (fromTime) params = params.set('fromTime', fromTime);
    if (toTime) params = params.set('toTime', toTime);

    return this.http.get<Meal[]>(`${this.baseUrl}/meals/for-user`, { params });
  }

  adminGetAllMeals() {
    return this.http.get<Meal[]>(`${this.baseUrl}/meals/admin`);
  }

  getMealById(id: number) {
    return this.http.get<Meal>(`${this.baseUrl}/meals/${id}`);
  }

  userCreateMeal(model: Meal) {
    return this.http.post<Meal>(`${this.baseUrl}/meals`, model);
  }

  adminCreateMeal(model: Meal, userId: string) {
    return this.http.post<Meal>(`${this.baseUrl}/meals/admin/${userId}`, model);
  }

  editMeal(mealId: number, model: Meal) {
    return this.http.put<void>(`${this.baseUrl}/meals/${mealId}`, model);
  }

  deleteMeal(mealId: number) {
    return this.http.delete<void>(`${this.baseUrl}/meals/${mealId}`);
  }
}

import { Component, inject, OnInit, signal } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { MealsService } from '../../services/meals.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../models/User';
import { Meal } from '../../models/MealModel';
import { UserService } from '../../services/user.service';
import { MealsListComponent } from '../shared/meals-list/meals-list.component';
import { FormsModule } from '@angular/forms';
import { NgClass } from '@angular/common';
import { CreateMealModalComponent } from '../shared/create-meal-modal/create-meal-modal.component';

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [MealsListComponent, FormsModule, NgClass, CreateMealModalComponent],
  templateUrl: './user-dashboard.component.html',
  styleUrl: './user-dashboard.component.css',
})
export class UserDashboardComponent implements OnInit {
  private accountService = inject(AccountService);
  private mealService = inject(MealsService);
  private userService = inject(UserService);
  private toastr = inject(ToastrService);

  user: User | null = null;
  meals = signal<Meal[]>([]);
  totalCalories = signal<number>(0);
  isModalOpen: boolean = false;
  mealModel: Meal = {
    id: 0,
    mealCalories: 0,
    mealDate: '',
    mealTime: '',
    mealDescription: '',
  };
  filter = {
    fromDate: '',
    toDate: '',
    fromTime: '',
    toTime: '',
  };
  filteredCalories = signal<number>(0);
  isFiltered = signal<boolean>(false);

  ngOnInit(): void {
    this.loadUser();
    this.loadMeals();
  }

  loadUser() {
    const userId = this.accountService.getUserIdFromToken();
    if (userId) {
      this.userService.getUserById(userId).subscribe({
        next: (data) => {
          this.user = data;
        },
      });
    }
  }

  loadMeals() {
    const fromDate = this.filter.fromDate
      ? this.formatDate(this.filter.fromDate)
      : undefined;
    const toDate = this.filter.toDate
      ? this.formatDate(this.filter.toDate)
      : undefined;
    const fromTime = this.filter.fromTime
      ? this.formatTime(this.filter.fromTime)
      : undefined;
    const toTime = this.filter.toTime
      ? this.formatTime(this.filter.toTime)
      : undefined;

    this.isFiltered.set(!!(fromDate || toDate || fromTime || toTime));

    this.mealService
      .getMealsForUser(fromDate, toDate, fromTime, toTime)
      .subscribe({
        next: (data) => {
          this.meals.set(data);
          this.calculateTotalCalories();
          this.calculateTodayCalories();
        },
        error: (err) => {
          this.toastr.error(err.error);
        },
      });
  }

  calculateTodayCalories() {
    const today = new Date().toISOString().split('T')[0];

    const todaysMeals = this.meals().filter((meal) => {
      const mealDate = new Date(meal.mealDate).toISOString().split('T')[0];
      return mealDate === today;
    });

    const total = todaysMeals.reduce((sum, meal) => sum + meal.mealCalories, 0);
    this.totalCalories.set(total);
  }

  calculateTotalCalories() {
    const total = this.meals().reduce(
      (sum, meal) => sum + meal.mealCalories,
      0
    );
    this.filteredCalories.set(total);
  }

  clearFilters() {
    this.filter = {
      fromDate: '',
      toDate: '',
      fromTime: '',
      toTime: '',
    };
    this.isFiltered.set(false);
    this.filteredCalories.set(0);
    this.loadMeals();
  }

  hasActiveFilters(): boolean {
    return !!(
      this.filter.fromDate ||
      this.filter.toDate ||
      this.filter.fromTime ||
      this.filter.toTime
    );
  }

  getCaloriesStatus(): 'under' | 'over' | 'neutral' {
    if (this.isFiltered()) return 'neutral';

    const expectedCalories = this.user?.expectedCaloriesPerDay || 0;
    const calories = this.totalCalories();

    if (expectedCalories === 0) return 'neutral';
    return calories <= expectedCalories ? 'under' : 'over';
  }

  deleteMeal(mealId: number) {
    if (!confirm('Are you sure you want to delete this meal?')) return;

    this.mealService.deleteMeal(mealId).subscribe({
      next: (_) => {
        this.toastr.success('Meal deleted successfully.');
        this.meals.set(this.meals().filter((m) => m.id !== mealId));
        this.calculateTotalCalories();
        this.calculateTodayCalories();
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  openCreateMealModal() {
    this.isModalOpen = true;
  }

  closeCreateMealModal() {
    this.isModalOpen = false;
  }

  saveCreateMeal(mealData: any) {
    this.mealService.userCreateMeal(mealData).subscribe({
      next: (_) => {
        this.toastr.success('Successfully created meal.');
        this.loadMeals();
        this.closeCreateMealModal();
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  formatDate(date: string | Date) {
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }

  formatTime(time: string | Date) {
    if (typeof time === 'string') {
      return time;
    }
    const d = new Date(time);
    return d.toTimeString().slice(0, 5);
  }
}

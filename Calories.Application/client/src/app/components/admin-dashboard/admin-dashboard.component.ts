import { Component, inject, OnInit, signal } from '@angular/core';
import { UserListComponent } from '../shared/user-list/user-list.component';
import { CreateEditUserComponent } from '../shared/create-edit-user/create-edit-user.component';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../models/User';
import { MealsListComponent } from '../shared/meals-list/meals-list.component';
import { CreateMealModalComponent } from '../shared/create-meal-modal/create-meal-modal.component';
import { MealsService } from '../../services/meals.service';
import { Meal } from '../../models/MealModel';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    UserListComponent,
    CreateEditUserComponent,
    MealsListComponent,
    CreateMealModalComponent,
  ],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent implements OnInit {
  private userService = inject(UserService);
  private mealService = inject(MealsService);
  private toastr = inject(ToastrService);

  users = signal<User[]>([]);
  usersInUserRole = signal<User[]>([]);
  meals = signal<Meal[]>([]);
  isModalOpen: boolean = false;
  isCreateMealModelOpen: boolean = false;
  isEditMode: boolean = false;
  selectedUser: User | null = null;
  newUserModel: any = {
    userName: '',
    email: '',
    password: '',
    role: 'User',
  };
  mealModel: any = {
    id: 0,
    userId: '',
    mealCalories: 0,
    mealDate: '',
    mealTime: '',
    mealDescription: '',
  };
  editUserModel: any = {
    userName: '',
    email: '',
    expectedCaloriesPerDay: 0,
    updatedPassword: '',
  };

  ngOnInit(): void {
    this.loadUsers();
    this.loadUsersInUserRole();
    this.loadMeals();
  }

  loadUsersInUserRole() {
    return this.userService.getAllUsersInUserRole().subscribe({
      next: (data) => {
        this.usersInUserRole.set(data);
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  loadUsers() {
    return this.userService.getAllUsers().subscribe({
      next: (data) => {
        this.users.set(data);
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  loadMeals() {
    this.mealService.adminGetAllMeals().subscribe({
      next: (data) => {
        this.meals.set(data);
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  openCreateModal() {
    this.isModalOpen = true;
    this.isEditMode = false;
    this.newUserModel = {
      userName: '',
      email: '',
      password: '',
      role: 'User',
    };
  }

  openEditModal(user: User) {
    this.isModalOpen = true;
    this.isEditMode = true;
    this.editUserModel = { ...user };
  }

  closeModal() {
    this.isModalOpen = false;
  }

  openCreateMealModal() {
    this.isCreateMealModelOpen = true;
  }

  closeCreateMealModal() {
    this.isCreateMealModelOpen = false;
  }

  saveUser(userData: any) {
    if (this.isEditMode) {
      this.updateUser(userData);
    } else {
      this.createUser(userData);
    }
  }

  saveCreateMeal(mealData: any) {
    this.mealService.adminCreateMeal(mealData, mealData.userId).subscribe({
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

  deleteMeal(mealId: number) {
    if (!confirm('Are you sure you want to delete this meal?')) return;

    this.mealService.deleteMeal(mealId).subscribe({
      next: (_) => {
        this.toastr.success('Meal deleted successfully.');
        this.meals.set(this.meals().filter((m) => m.id !== mealId));
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }

  deleteUser(userId: string) {
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.userService.adminDeleteUser(userId).subscribe({
      next: (_) => {
        this.toastr.success('Successfully deleted user.');
        this.loadUsers();
        this.loadMeals();
      },
      error: (err) => this.toastr.error(err.error),
    });
  }

  createUser(userData: any) {
    this.userService.adminCreateUser(userData).subscribe({
      next: (_) => {
        this.toastr.success('Successfully created new user.');
        this.loadUsers();
        this.loadUsersInUserRole();
        this.closeModal();
      },
      error: (err) => this.toastr.error(err.error),
    });
  }

  updateUser(userData: any) {
    this.userService.adminUpdateUser(userData, userData.id).subscribe({
      next: (_) => {
        this.toastr.success('Successfully edited user.');
        this.loadUsers();
        this.loadMeals();
        this.closeModal();
      },
      error: (err) => this.toastr.error(err.error),
    });
  }
}

import { Component, inject, OnInit, signal } from '@angular/core';
import { UserListComponent } from '../shared/user-list/user-list.component';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../models/User';
import { AccountService } from '../../services/account.service';

@Component({
  selector: 'app-manager-dashboard',
  standalone: true,
  imports: [UserListComponent, FormsModule],
  templateUrl: './manager-dashboard.component.html',
  styleUrl: './manager-dashboard.component.css',
})
export class ManagerDashboardComponent implements OnInit {
  private userService = inject(UserService);
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);

  users = signal<User[]>([]);
  isModalOpen: boolean = false;
  isEditMode: boolean = false;
  selectedUser: User | null = null;
  newUserModel: any = {
    userName: '',
    email: '',
    password: '',
    role: 'User',
  };

  editUserModel: any = {
    userName: '',
    email: '',
    expectedCaloriesPerDay: 0,
    updatedPassword: '',
  };

  ngOnInit(): void {
    this.loadUsers();
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
    this.selectedUser = user;
    this.editUserModel = { ...user };
  }

  closeModal() {
    this.isModalOpen = false;
    this.selectedUser = null;
  }

  saveUser() {
    if (this.isEditMode && this.selectedUser) {
      if (!!this.selectedUser.id)
        this.userService
          .adminUpdateUser(this.editUserModel, this.selectedUser.id)
          .subscribe({
            next: (_) => {
              this.toastr.success('Successfully edited user.');
              this.loadUsers();
              this.closeModal();
            },
            error: (err) => this.toastr.error(err.error),
          });
    } else {
      this.userService.adminCreateUser(this.newUserModel).subscribe({
        next: (_) => {
          this.toastr.success('Successfully created new user.');
          this.loadUsers();
          this.closeModal();
        },
        error: (err) => this.toastr.error(err.error),
      });
    }
  }

  deleteUser(userId: string) {
    if (!confirm('Are you sure you want to delete this user?')) return;

    this.userService.adminDeleteUser(userId).subscribe({
      next: (_) => {
        this.toastr.success('Successfully deleted user.');
        this.loadUsers();
      },
      error: (err) => this.toastr.error(err.error),
    });
  }
}

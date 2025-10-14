import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../../models/User';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
})
export class UserListComponent {
  @Input() users: User[] = [];
  @Output() editUser = new EventEmitter<User>();
  @Output() deleteUser = new EventEmitter<string>();

  onEdit(user: User) {
    return this.editUser.emit(user);
  }

  onDelete(userId: string) {
    return this.deleteUser.emit(userId);
  }
}

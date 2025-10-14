import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../../../models/User';

@Component({
  selector: 'app-edit-user-by-user-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-user-by-user-modal.component.html',
  styleUrl: './edit-user-by-user-modal.component.css',
})
export class EditUserByUserModalComponent {
  @Input() editUserModel: User = {};
  @Input() isOpen: boolean = false;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  saveUser() {
    this.save.emit(this.editUserModel);
  }

  closeModal() {
    this.close.emit();
  }
}

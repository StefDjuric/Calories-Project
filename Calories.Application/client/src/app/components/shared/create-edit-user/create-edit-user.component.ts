import { Component, EventEmitter, input, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-edit-user',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-edit-user.component.html',
  styleUrl: './create-edit-user.component.css',
})
export class CreateEditUserComponent {
  @Input() isEditMode: boolean = false;
  @Input() userModel: any = {};
  @Input() isOpen: boolean = false;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();

  saveUser() {
    this.save.emit(this.userModel);
  }

  closeModal() {
    this.close.emit();
  }
}

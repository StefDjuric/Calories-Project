import {
  Component,
  EventEmitter,
  inject,
  input,
  Input,
  Output,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Meal } from '../../../models/MealModel';
import { UserService } from '../../../services/user.service';
import { User } from '../../../models/User';

@Component({
  selector: 'app-create-meal-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-meal-modal.component.html',
  styleUrl: './create-meal-modal.component.css',
})
export class CreateMealModalComponent {
  @Input() isOpen: boolean = false;
  @Input() mealModel: any = {
    id: 0,
    userId: '',
    mealCalories: 0,
    mealDate: '',
    mealDescription: '',
    mealTime: '',
  };
  @Input() isAdminMealCreate: boolean = false;
  @Input() users: User[] = [];
  @Output() save = new EventEmitter<Meal>();
  @Output() close = new EventEmitter<void>();

  saveMeal() {
    this.save.emit(this.mealModel);
  }

  closeModal() {
    this.close.emit();
  }
}

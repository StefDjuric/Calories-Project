import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Meal } from '../../../models/MealModel';

@Component({
  selector: 'app-create-meal-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-meal-modal.component.html',
  styleUrl: './create-meal-modal.component.css',
})
export class CreateMealModalComponent {
  @Input() isOpen: boolean = false;
  @Input() mealModel: Meal = {
    id: 0,
    mealCalories: 0,
    mealDate: '',
    mealDescription: '',
    mealTime: '',
  };

  @Output() save = new EventEmitter<Meal>();
  @Output() close = new EventEmitter<void>();

  saveMeal() {
    this.save.emit(this.mealModel);
  }

  closeModal() {
    this.close.emit();
  }
}

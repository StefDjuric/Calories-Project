import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { Meal } from '../../../models/MealModel';
import { DatePipe } from '@angular/common';
import { MealsService } from '../../../services/meals.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-meals-list',
  standalone: true,
  imports: [DatePipe, FormsModule],
  templateUrl: './meals-list.component.html',
  styleUrl: './meals-list.component.css',
})
export class MealsListComponent {
  private mealService = inject(MealsService);
  private toastr = inject(ToastrService);
  @Input() meals: Meal[] = [];
  @Output() deleteMeal = new EventEmitter<number>();
  @Output() mealEdited = new EventEmitter<void>();
  selectedMeal: Meal | null = null;

  onDelete(id: number) {
    this.deleteMeal.emit(id);
  }

  loadMeals() {
    this.mealService.getMealsForUser().subscribe({
      next: (meals) => {
        this.meals = meals;
      },
      error: (err) => this.toastr.error(err.error),
    });
  }

  openEditModal(meal: Meal) {
    this.selectedMeal = { ...meal };
  }

  closeEditModal() {
    this.selectedMeal = null;
  }

  saveEdit() {
    if (!this.selectedMeal) return;

    const payload = {
      mealDescription: this.selectedMeal.mealDescription,
      mealCalories: this.selectedMeal.mealCalories,
      mealDate: this.selectedMeal.mealDate,
      mealTime: this.selectedMeal.mealTime,
    };

    this.mealService.editMeal(this.selectedMeal.id, payload).subscribe({
      next: (_) => {
        this.toastr.success('Successfully updated meal');
        this.mealEdited.emit();
        this.closeEditModal();
      },
      error: (err) => this.toastr.error(err.error),
    });
  }
}

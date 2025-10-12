import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Meal } from '../../../models/MealModel';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-meals-list',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './meals-list.component.html',
  styleUrl: './meals-list.component.css',
})
export class MealsListComponent {
  @Input() meals: Meal[] = [];
  @Output() editMeal = new EventEmitter<number>();
  @Output() deleteMeal = new EventEmitter<number>();

  onEdit(id: number) {
    this.editMeal.emit(id);
  }

  onDelete(id: number) {
    this.deleteMeal.emit(id);
  }
}

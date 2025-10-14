import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMealModalComponent } from './create-meal-modal.component';

describe('CreateMealModalComponent', () => {
  let component: CreateMealModalComponent;
  let fixture: ComponentFixture<CreateMealModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateMealModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateMealModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

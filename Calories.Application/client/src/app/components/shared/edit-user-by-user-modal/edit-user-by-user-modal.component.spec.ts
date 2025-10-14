import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditUserByUserModalComponent } from './edit-user-by-user-modal.component';

describe('EditUserByUserModalComponent', () => {
  let component: EditUserByUserModalComponent;
  let fixture: ComponentFixture<EditUserByUserModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditUserByUserModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditUserByUserModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

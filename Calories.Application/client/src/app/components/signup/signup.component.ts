import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { RegisterModel } from '../../models/RegisterModel';
import { FormsModule, NgForm } from '@angular/forms';
import { AccountService } from '../../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css',
})
export class SignupComponent {
  private accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  model: RegisterModel = { email: '', password: '', userName: '' };

  onSubmit(signupForm: NgForm) {
    if (signupForm.invalid) {
      return;
    }

    this.register();
  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: (_) => {
        this.toastr.success('Successfully registered user.');
        this.router.navigateByUrl('login');
      },
      error: (err) => {
        this.toastr.error(err.error);
      },
    });
  }
}

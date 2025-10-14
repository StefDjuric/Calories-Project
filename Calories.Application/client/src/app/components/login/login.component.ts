import { Component, inject } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginModel } from '../../models/LoginModel';
import { AccountService } from '../../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);
  private router = inject(Router);
  model: LoginModel = { emailOrUsername: '', password: '' };

  onSubmit(loginForm: NgForm) {
    if (loginForm.invalid) {
      return;
    }

    this.login();
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: (_) => {
        if (this.accountService.getRoleFromToken() === 'User')
          this.router.navigateByUrl('/user-dashboard');
        else if (this.accountService.getRoleFromToken() === 'User Manager')
          this.router.navigateByUrl('/manager-dashboard');

        this.toastr.success('Successfully logged in');
      },
      error: (err) => {
        this.toastr.error('Could not log in. ', err.error);
      },
    });
  }
}

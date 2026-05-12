import { Component } from '@angular/core';
import {FormBuilder,ReactiveFormsModule,Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({selector: 'app-login',standalone: true,imports: [CommonModule,ReactiveFormsModule,MatIconModule,MatProgressSpinnerModule],templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']})
export class LoginComponent {
  isLoading = false;
  errorMessage = '';
  loginForm;
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  )
  {
    this.loginForm = this.fb.group({
      email: ['', [
        Validators.required,
        Validators.email
      ]],

      password: ['', [
        Validators.required
      ]]
    });
  }

  onSubmit(): void
  {
    if (this.loginForm.invalid)
    {
      return;
    }
    this.isLoading = true;
    this.errorMessage = '';
    this.authService.login(
      this.loginForm.value
    )
    .subscribe({
      next: (response) =>
      {
        this.authService.saveToken(
          response.token
        );

        this.router.navigate([
          '/dashboard'
        ]);
      },
      error: () =>
      {
        this.errorMessage =
          'Invalid email or password';

        this.isLoading = false;
      }
    });
  }
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormGroup,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthFormComponent } from '../components/auth-form/auth-form.component';
import { IconModule } from '@coreui/icons-angular';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../auth.service';
import { LoginDto } from '../models/auth.dto';
import { AuthFormService } from '../form/auth-form.service';

@Component({
  selector: 'app-login',
  imports: [AuthFormComponent, 
    ReactiveFormsModule, RouterModule, CommonModule,
    IconModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  // src/app/features/auth/components/login-form/login-form.component.ts

  authForm!: FormGroup;
  showPassword = false;
  loading = false;
  error: string | null = null;

  constructor(
    private authFormService: AuthFormService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authForm =this.authFormService.createLoginForm();
  }

  get f() {
    return this.authForm.controls;
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.authForm.invalid) {
      this.authForm.markAllAsTouched();
      this.error = 'Merci de remplir tous les champs correctement.';
      return;
    }

    this.loading = true;
    this.error = null;

    const payload: LoginDto = {
      ...this.authForm.value,
      ipAddress: '127.0.0.1',
    };

    this.authService.login(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/Home/dashboard']);
      },
      error: () => {
        this.error = 'Échec de la connexion. Vérifiez vos identifiants.';
        this.loading = false;
      },
    });
  }
}

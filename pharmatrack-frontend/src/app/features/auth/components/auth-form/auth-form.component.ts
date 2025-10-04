import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../auth.service';
import { AuthFormService } from '../../form/auth-form.service';
import { LoginDto, RegisterDto } from '../../models/auth.dto';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-auth-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule,],
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss'],
})
export class AuthFormComponent implements OnInit {
  @Input() mode: 'login' | 'register' = 'login';

  authForm!: FormGroup;
  showPassword = false;
  loading = false;
  error: string | null = null;

  constructor(
    private authService: AuthService,
    private authFormService: AuthFormService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authForm =
      this.mode === 'login'
        ? this.authFormService.createLoginForm()
        : this.authFormService.createRegisterForm();
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.authForm.invalid) return;
    this.loading = true;
    this.error = null;

    this.mode === 'login' ? this.handleLogin() : this.handleRegister();
  }

  private handleLogin(): void {
    const payload: LoginDto = {
      ...this.authForm.value,
      ipAddress: '127.0.0.1',
    };

    this.authService.login(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        this.error = 'Échec de la connexion. Vérifiez vos identifiants.';
        this.loading = false;
      },
    });
  }

  private handleRegister(): void {
    const { password, confirmPassword, ...rest } = this.authForm.value;

    if (password !== confirmPassword) {
      this.error = 'Les mots de passe ne correspondent pas.';
      this.loading = false;
      return;
    }

    const payload: RegisterDto = { ...rest, password, ipAddress: '127.0.0.1' };

    this.authService.register(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/auth/login']);
      },
      error: () => {
        this.error = 'Échec de l’inscription. Vérifiez vos informations.';
        this.loading = false;
      },
    });
  }
}

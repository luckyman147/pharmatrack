import { Component, OnInit } from '@angular/core';
import { AuthFormComponent } from '../components/auth-form/auth-form.component';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../auth.service';
import { AuthFormService } from '../form/auth-form.service';
import { RegisterDto } from '../models/auth.dto';
import { IconModule } from '@coreui/icons-angular';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, AuthFormComponent,IconModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  authForm!: FormGroup;
  showPassword = false;
  showComfiPassword = false;
  loading = false;
  error: string | null = null;

  constructor(
     private authFormService: AuthFormService,
 
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authForm = this.authFormService.createRegisterForm();
  }

  get f() { return this.authForm.controls; }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }
  
  toggleConfirmPassword(): void {
    this.showComfiPassword = !this.showComfiPassword;
  }


  onSubmit(): void {
    if (this.authForm.invalid || this.f['password'].value !== this.f['confirmPassword'].value) {
      this.authForm.markAllAsTouched();
      this.error = this.f['password'].value !== this.f['confirmPassword'].value
        ? 'Les mots de passe ne correspondent pas.'
        : 'Merci de remplir tous les champs correctement.';
      return;
    }

    this.loading = true;
    this.error = null;

    const { confirmPassword, ...rest } = this.authForm.value;
    const payload: RegisterDto = { ...rest};

    this.authService.register(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/auth/login']);
      },
      error: () => {
        this.error = 'Échec de l’inscription. Vérifiez vos informations.';
        this.loading = false;
      }
    });
  }}
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AuthFormService } from '../form/auth-form.service';
import { FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';
import { RegisterDto } from '../models/auth.dto';


@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
})
export class SignUpComponent implements OnInit {
  authForm!: FormGroup;
  showPassword = false;
  showComfiPassword = false;
  loading = false;
  error: string | null = null;

  constructor(
   
private router: Router, private route: ActivatedRoute,
private authFormService:AuthFormService, private authService:AuthService


  ) {}

  ngOnInit(): void {
    this.authForm = this.authFormService.createRegisterForm();
  }

  get f() {
    return this.authForm.controls;
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword(): void {
    this.showComfiPassword = !this.showComfiPassword;
  }

  onSubmit(): void {
    if (
      this.authForm.invalid ||
      this.f['password'].value !== this.f['confirmPassword'].value
    ) {
      this.authForm.markAllAsTouched();
      this.error =
        this.f['password'].value !== this.f['confirmPassword'].value
          ? 'Les mots de passe ne correspondent pas.'
          : 'Merci de remplir tous les champs correctement.';
      return;
    }

    this.loading = true;
    this.error = null;

    const { confirmPassword, ...rest } = this.authForm.value;
    const payload: RegisterDto = { ...rest };

    this.authService.register(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/auth/sign-in']);
      },
      error: () => {
        this.error = 'Échec de l’inscription. Vérifiez vos informations.';
        this.loading = false;
      },
    });
  }



  // On Signup link click
  onSignIn() {
    this.router.navigate(['sign-in'], { relativeTo: this.route.parent });
  }


}

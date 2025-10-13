import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from "@angular/router";
import { AuthLoginResponse, LoginDto } from '../models/auth.dto';
import { AuthFormService } from '../form/auth-form.service';
import { AuthService } from '../auth.service';
import { UserProfileService } from 'src/app/user-profile/user-profile.service';
import { switchMap } from 'rxjs/operators';


@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',

  styleUrls: ['./sign-in.component.scss'],
})
export class SignInComponent implements OnInit {
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authFormService: AuthFormService,
    private authService: AuthService,
    private userProfileService: UserProfileService
  ) {}
  authForm!: FormGroup;
  showPassword = false;
  loading = false;
  error: string | null = null;
  // On Forgotpassword link click
  onForgotpassword() {
    this.router.navigate(['forgot-password'], {
      relativeTo: this.route.parent,
    });
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

    this.authService.login(payload)
      .pipe(
    switchMap((loginresponse:AuthLoginResponse) => this.userProfileService.getUserProfile(loginresponse.id))
  ).
    subscribe({
      next: (user) => {
         console.log('User fetched:', user);
      localStorage.setItem('user', JSON.stringify(user));
        this.loading = false;
        if (user.role === 'Admin') {
          this.router.navigate(['dashboard']);
        }
        this.router.navigate(['Transactions']);
      },
      error: () => {
        this.error = 'Échec de la connexion. Vérifiez vos identifiants.';
        this.loading = false;
      },
    });
  }
  // On Signup link click
  onSignup() {
    this.router.navigate(['sign-up'], { relativeTo: this.route.parent });
  }
  get f() {
    return this.authForm.controls;
  }

  ngOnInit(): void {
    this.authForm = this.authFormService.createLoginForm();
  }
}

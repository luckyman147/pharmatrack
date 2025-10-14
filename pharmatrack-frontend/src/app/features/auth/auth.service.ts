import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import { AppConfigService } from '../../core/config/app-config.service';
import { AuthLoginResponse, AuthRegisterResponse, AuthStatus, LoginDto, RegisterDto } from './models/auth.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {
    this.authUrl = `${this.appConfigService.apiUrl}/Authentication`;
  }
  private authUrl!: string;

  // ✅ Login
  login(credentials: LoginDto): Observable<AuthLoginResponse> {
    const body = { login: credentials };
    console.log(body);
    return this.http.post<AuthLoginResponse>(`${this.authUrl}/login`, body, {
      withCredentials: true,
    });
  }

  // ✅ Register
  register(user: RegisterDto): Observable<AuthRegisterResponse> {
    const body = { user: user };
    return this.http.post<AuthRegisterResponse>(
      `${this.authUrl}/register`,
      body,
      {
        withCredentials: true,
      }
    );
  }

  // ✅ Logout
  logout(): Observable<void> {
    return this.http.post<void>(
      `${this.authUrl}/logout`,
      {},
      { withCredentials: true }
    );
  }

  // ✅ Check Auth Status
  isAuthenticated(): Observable<boolean> {
    console.log("AuthService#isAuthenticated called");
    // Backend checks accessToken cookie
    return this.http
      .post<{ isAuthenticated: boolean }>(`${this.authUrl}/me`, {}  , { withCredentials: true },)
        // <-- important
      .pipe(
        map((res) =>{
          console.log("AuthService#isAuthenticated response:", res);
          return res.isAuthenticated;}),
        catchError((error) => {
          console.error('AuthService#isAuthenticated error:', error);

          // 🔔 Optional: Show notification or error popup here
          // this.notificationService.error("Authentication check failed");

          return of(false); // fallback to false
        })
      );
  }
}

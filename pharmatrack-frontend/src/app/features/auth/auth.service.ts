import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigService } from '../../core/config/app-config.service';
import { AuthLoginResponse, AuthRegisterResponse, AuthStatus, LoginDto, RegisterDto } from './models/auth.dto';

@Injectable({ providedIn: 'root' })
export class AuthService {
  
  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) { }
  private authUrl = '';

  ngOnInit(): void {
    this.authUrl = `${this.appConfigService.apiUrl}/Authentication`;
  }

  // ✅ Login
  login(credentials: LoginDto): Observable<AuthLoginResponse> {
    const body = { login: credentials };
    return this.http.post<AuthLoginResponse>(`${this.authUrl}/login`, body, {
      withCredentials: true,
    });
  }

  // ✅ Register
  register(user: RegisterDto): Observable<AuthRegisterResponse> {
    const body = { user: user };
    return this.http.post<AuthRegisterResponse>(`${this.authUrl}/register`, body, {
      withCredentials: true,
    });
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
  isAuthenticated(): Observable<AuthStatus> {
    return this.http.post<AuthStatus>(
      `${this.authUrl}/me`,
      {},
      { withCredentials: true }
    );
  }
}

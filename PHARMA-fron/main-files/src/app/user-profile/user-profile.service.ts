import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppConfigService } from '../shared/config/app-config.service';
import { User } from '../shared/Models/User';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {
    this.authUrl = `${this.appConfigService.apiUrl}`;
  }
  private authUrl!: string;
  getUserProfile(id: string): Observable<User> {
    return this.http.get<User>(`${this.authUrl}/me/${id}`, {
      withCredentials: true,
    });
  }
}

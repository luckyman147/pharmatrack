import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CloudinaryService {
  constructor(private http: HttpClient) {}

  uploadFile(file: File): Observable<string> {
    const url = `https://api.cloudinary.com/v1_1/${environment.cloudinary.cloudName}/upload`;
    const formData = new FormData();
    formData.append('file', file);
    formData.append('upload_preset', environment.cloudinary.uploadPreset);

    return this.http.post<any>(url, formData).pipe(
      map(response => response.secure_url)
    );
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigService } from 'src/app/shared/config/app-config.service';
import { PharmacieFilterRequest } from './dto/PharmacieFilterRequest';
import { CreatePharmacieDto } from './dto/createPharmacieDto';

@Injectable({
  providedIn: 'root',
})
export class PharmacieService {
  private PharmacieeUrl: string;

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {
    this.PharmacieeUrl = `${this.appConfigService.apiUrl}/Pharmacies`;
  }

  getPharmacies(filter: PharmacieFilterRequest): Observable<any> {
    return this.http.post<any>(`${this.PharmacieeUrl}`, filter);
  }
  deletePharmacie(id: string): Observable<void> {
    console.log('Deleting pharmacie with id:', id);
    return this.http.delete<void>(`${this.PharmacieeUrl}/${id}`);
  }
  createPharmacie(dto: CreatePharmacieDto): Observable<void> {
    return this.http.post<void>(
      `${this.appConfigService.apiUrl}/Create-Pharmacie`,
      dto
    );
  }
}

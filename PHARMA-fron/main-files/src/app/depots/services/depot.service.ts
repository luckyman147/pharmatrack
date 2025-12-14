import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CreateDepotDto } from '../dto/create-depot.dto';
import { Depot } from '../models/depot';

@Injectable({
  providedIn: 'root'
})
export class DepotService {
  private apiUrl = `${environment.apiUrl}/Depots`;

  constructor(private http: HttpClient) { }

  createDepot(depot: CreateDepotDto): Observable<Depot> {
    return this.http.post<Depot>(this.apiUrl, depot);
  }

  updateDepot(id: string, depot: CreateDepotDto): Observable<Depot> {
    return this.http.put<Depot>(`${this.apiUrl}/${id}`, depot);
  }

  getDepot(id: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}

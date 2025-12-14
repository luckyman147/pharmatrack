import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AppConfigService } from '../shared/config/app-config.service';
import {
  Commande,
  CommandeFilter,
  CommandeSummaryInfo,
  CreateCommandeDto,
  CreateCommandeResponse,
  GetAllCommandeResponse,
  GetCommandeByIdResponse,
  PaginationRequest,
  PaginationResult,
  UpdateCommandeDto,
  UpdateCommandeResponse,
  StatusCommande
} from './models/commande.models';

@Injectable({ providedIn: 'root' })
export class CommandeService {
  private commandeUrl!: string;

  constructor(
    private http: HttpClient,
    private appConfigService: AppConfigService
  ) {
    this.commandeUrl = `${this.appConfigService.apiUrl}`;
  }

  /**
   * Get all commandes with pagination and optional filtering
   */
  getAllCommandes(
    paginationRequest: PaginationRequest,
    filter?: CommandeFilter
  ): Observable<PaginationResult<CommandeSummaryInfo>> {
    // Build query params for pagination
    let params = new HttpParams()
      .set('pageIndex', paginationRequest.pageIndex.toString())
      .set('pageSize', paginationRequest.pageSize.toString());

    // Send filter in the body, pagination in query params
    return this.http
      .post<GetAllCommandeResponse>(`${this.commandeUrl}/commandes`, filter || {}, {
        params,
      })
      .pipe(map((response) => response.commandes));
  }

  /**
   * Get a commande by ID
   */
  getCommandeById(id: string): Observable<Commande | null> {
    return this.http
      .get<GetCommandeByIdResponse>(`${this.commandeUrl}/Commande/${id}`)
      .pipe(map((response) => response.commande));
  }

  /**
   * Create a new commande
   */
  createCommande(dto: CreateCommandeDto): Observable<string> {
    return this.http
      .post<CreateCommandeResponse>(`${this.commandeUrl}/Commande/Create`, dto)
      .pipe(map((response) => response.id));
  }

  /**
   * Update an existing commande
   */
  updateCommande(id: string, dto: UpdateCommandeDto): Observable<boolean> {
    return this.http
      .put<UpdateCommandeResponse>(`${this.commandeUrl}/Commande/${id}`, dto)
      .pipe(map((response) => response.success));
  }

  /**
   * Delete a commande
   */
  deleteCommande(id: string): Observable<void> {
    return this.http.delete<void>(`${this.commandeUrl}/Commande/${id}`);
  }

  /**
   * Update commande status
   */
  updateStatus(id: string, status: StatusCommande): Observable<boolean> {
    return this.http
      .patch<{ success: boolean }>(`${this.commandeUrl}/Commande/${id}/status`, { newStatus: status })
      .pipe(map((response) => response.success));
  }
}

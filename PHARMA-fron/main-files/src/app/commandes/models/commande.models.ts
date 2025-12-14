// Commande (Order) models and DTOs

import { Adress } from "src/app/transaction/pharmacies/models/Pharmacie";

// ============= Enums =============

export enum   StatusCommande {
  EnAttente = 'EnAttente',     // Pending
  Valide = 'Valide',           // Validated
  EnRetour = 'EnRetour',       // Returned
  Annulee = 'Annulee'          // Cancelled
}

// ============= Nested Basic Info =============




export interface PharamcieBasicInfo {
  id: string;
  name: string;
  address?: string;
}

export interface DepotBasicInfo {
  id: string;
  name: string;
  address?: Adress;
}

export interface ProfileBasicInfo {
  id: string;
  fullName: string;
  avatarUrl?: string;
}

// ============= Allocation & Line Items =============

export interface AllocationDepotDto {
  depotId: string;
  depotName: string;
  depotAddress?: Adress;
  quantityAllocated: number;
}

export interface AllocationDepot {
  depotInfo: DepotBasicInfo;
  quantityAllocated: number;
}

export interface LigneCommandeDto {
  produitId: string;
  nomProuit: string;  // Note: keeping backend typo for consistency
  uniteProduitPrix: number;
  allocationsDepots: AllocationDepotDto[];
}

export interface LigneCommande {
  produitId: string;
  nomProuit: string;
  totalQuantite: number;
  uniteProduitPrix: number;
  totalLineAmount: number;
  allocationsDepots: AllocationDepot[];
}

// ============= Commande Summary (for list view) =============

export interface CommandeSummaryInfo {
  id: string;
  pharamcieInfo: PharamcieBasicInfo;  // Note: keeping backend typo
  userInfo: ProfileBasicInfo;
  orderDate: string;  // ISO date string
  status: StatusCommande;
  totalAmount: number;
  codeComm: number;
  totalLines: number;
  originalDocumentPath?: string;
  exportedDocumentPath?: string;
}

// ============= Full Commande Entity =============

export interface Commande {
  id: string;
  createdAt?: string;
  createdBy?: string;
  lastModified?: string;
  lastModifiedBy?: string;
  pharamcieInfo: PharamcieBasicInfo;
  userInfo: ProfileBasicInfo;
  orderDate: string;  // ISO date string
  status: StatusCommande;
  totalAmount: number;
  codeComm: number;
  originalDocumentPath: string;
  exportedDocumentPath: string;
  lines: LigneCommande[];
}

// ============= DTOs for Create/Update =============

export interface CreateCommandeDto {
  pharamcieInfo: PharamcieBasicInfo;
  userInfo: ProfileBasicInfo;
  originalDocumentPath?: string;
  exportedDocumentPath?: string;
  codeComm: number;
  lines: LigneCommandeDto[];
}

export interface UpdateCommandeDto {
  status: StatusCommande;
  originalDocumentPath?: string;
  exportedDocumentPath?: string;
  lines: LigneCommandeDto[];
}

// ============= Filter for List Query =============

export interface CommandeFilter {
  pharmacieId?: string;
  status?: StatusCommande;
  startDate?: string;  // ISO date string
  endDate?: string;    // ISO date string
  minAmount?: number;
  maxAmount?: number;
  codeComm?: number;
}

// ============= Pagination Types =============

export interface PaginationRequest {
  pageIndex: number;
  pageSize: number;
}

export interface PaginationResult<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  pagesReste?: number;
  data: T[];
}

// ============= API Response Wrappers =============

export interface GetAllCommandeResponse {
  commandes: PaginationResult<CommandeSummaryInfo>;
}

export interface GetCommandeByIdResponse {
  commande: Commande | null;
}

export interface CreateCommandeResponse {
  id: string;
}

export interface UpdateCommandeResponse {
  success: boolean;
}

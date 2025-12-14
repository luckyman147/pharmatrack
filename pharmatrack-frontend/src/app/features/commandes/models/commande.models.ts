// Commande (Order) models and DTOs

// ============= Enums =============

export enum StatusCommande {
  EnAttente = 'EnAttente',     // Pending
  Valide = 'Valide',           // Validated
  EnRetour = 'EnRetour',       // Returned
  Annulée = 'Annulée'          // Cancelled
}

// ============= Nested Basic Info =============

export interface PharamcieBasicInfo {
  id: string;
  nom: string;
  adresse?: string;
  telephone?: string;
}

export interface ProfileBasicInfo {
  id: string;
  fullName: string;
  email?: string;
}

// ============= Allocation & Line Items =============

export interface AllocationDepotDto {
  depotId: string;
  depotName: string;
  quantityAllocated: number;
}

export interface LigneCommandeDto {
  produitId: string;
  nomProuit: string;  // Note: keeping backend typo for consistency
  uniteProduitPrix: number;
  allocationsDepots: AllocationDepotDto[];
}

export interface LigneCommande extends LigneCommandeDto {
  totalQuantite?: number;
  totalLineAmount?: number;
}

// ============= Commande Summary (for list view) =============

export interface CommandeSummaryInfo {
  id: string;
  pharmacieId: string;
  pharamcieInfo: PharamcieBasicInfo;  // Note: keeping backend typo
  userInfo: ProfileBasicInfo;
  orderDate: string;  // ISO date string
  status: StatusCommande;
  totalAmount: number;
  codeComm: number;
  totalLines: number;
}

// ============= Full Commande Entity =============

export interface Commande {
  id: string;
  pharmacieId: string;
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
  pharmacieId: string;
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

// ============= API Response Wrappers =============

export interface GetAllCommandeResponse {
  commandes: {
    pageIndex: number;
    pageSize: number;
    totalCount: number;
    items: CommandeSummaryInfo[];
  };
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

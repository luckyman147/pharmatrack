export interface StockTransactionHistory {
    id: string;
    depotId: string;
    produitId: string;
    commandeId?: string;
    oldQuantity: number;
    newQuantity: number;
    reason: number; // Enum value
    transactionDate: Date;
    // Helper properties populated if possible, or we just show IDs for now
}

export interface StockHistoryFilter {
    depotId?: string;
    produitId?: string;
    commandeId?: string;
    status?: number;
    startDate?: string;
    endDate?: string;
}

export interface StockHistoryResponse {
    result: {
        pageIndex: number;
        pageSize: number;
        count: number;
        data: StockTransactionHistory[];
    }
}

export enum TransactionReason {
    InitialStock = 0,
    ManualAdjustment = 1,
    CommandValidated = 2,
    CommandCancelled = 3,
    StockReturn = 4,
    Restocking = 5,
    Damage = 6,
    Expired = 7,
    TransferOut = 8,
    TransferIn = 9
}

export const TransactionReasonLabel: {[key: number]: string} = {
    0: 'Stock Initial',
    1: 'Ajustement Manuel',
    2: 'Commande Validée',
    3: 'Commande Annulée',
    4: 'Retour Stock',
    5: 'Réapprovisionnement',
    6: 'Dommage',
    7: 'Expiré',
    8: 'Transfert Sortant',
    9: 'Transfert Entrant'
};

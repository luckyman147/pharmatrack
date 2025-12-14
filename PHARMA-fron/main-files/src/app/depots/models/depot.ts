import { Adress } from "src/app/transaction/pharmacies/models/Pharmacie";

export interface Depot {
    id: string;
    name:  string;
    telephone: string;
    location:Adress;
    rapportDocumentUrl?: string;
    produitStock?: any[]; // Or specific type if known for read model
    numberOfProducts: number;
    numberOfStockTotal: number;
    produitsEnRupture: number;
    lastModified?: Date;
}



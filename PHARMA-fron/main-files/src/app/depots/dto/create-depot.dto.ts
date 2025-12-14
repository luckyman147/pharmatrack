import { Adress } from "src/app/transaction/pharmacies/models/Pharmacie";
import { LigneDepotProduit } from "../models/ligne-depot-produit";

export interface CreateDepotDto {
    name: string;
    telephone: string;
    rapportDocumentUrl: string;
    location: Adress;
    produitStock: LigneDepotProduit[];
}

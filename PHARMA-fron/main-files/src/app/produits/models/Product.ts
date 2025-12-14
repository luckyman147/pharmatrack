import { Category } from "../categorie/models/Categorie";

export interface Product {
  id: string;
  name: string;
  description: string;
  prixVente: number;
  numberDepots: number;
  categorie: Category;
  // Omitted other fields like createdAt, createdBy as per requirement
}

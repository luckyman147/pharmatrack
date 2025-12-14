import { Adress, Horaire, PharmacieType } from "../models/Pharmacie";

export interface CreatePharmacieDto {
      name: string;
      ownerName: string;
      telephone: string;
      horaire: Horaire;
      pharmacieType: PharmacieType;
      location: Adress;
}
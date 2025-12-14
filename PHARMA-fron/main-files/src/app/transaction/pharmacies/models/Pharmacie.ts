export interface Pharmacie {
  id?: string; // optional if not always present
  name: string;
  ownerName: string;
  telephone: string;
  horaire: Horaire;
  pharmacieType: PharmacieType;
  location: Adress;
}

// ✅ Enums matching your backend
export enum Horaire {
  JOUR = 'Jour',
  NUIT = 'Nuit',
}

export enum PharmacieType {
  PARAPHARMACIE = 'Parapharmacie',
  PHARMACIE = 'Pharmacie',
}

// ✅ Embedded Address model
export interface Adress {
    rue?: string;
  gouvernerat?: string;
  delegation?: string;
  zone?: string;
}

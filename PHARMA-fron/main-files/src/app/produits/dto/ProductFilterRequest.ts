export interface ProductFilterRequest {
  filter: {
    categorieId: string | null;
    name: string | null;
    categorieName: string | null;
    minPrixVente: number | null;
    maxPrixVente: number | null;
  };
  pageNumber: number;
  pageSize: number;
}

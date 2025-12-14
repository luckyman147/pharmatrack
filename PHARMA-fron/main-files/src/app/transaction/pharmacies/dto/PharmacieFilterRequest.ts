export interface PharmacieFilterRequest {
  type?: string | null;
  horaire?: string | null;
  zone?: string | null;
  gouvernerat?: string | null;
  delegation?: string | null;
  pageIndex: number;
  ByName?: string | null;
  pageSize: number;
}

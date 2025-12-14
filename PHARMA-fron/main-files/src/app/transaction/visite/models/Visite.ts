import { DelegueInfo } from "./DelegueInfo";
import { PharmacieBasicInfo } from "./PharmacieBasicInfo";

export interface Visite {
  id: string | null;
  visitDate: string;
  description: string;
  reportUrl: string | null;
  pharamcieBasicInfo: PharmacieBasicInfo;
  delegueteInfo: DelegueInfo;
}

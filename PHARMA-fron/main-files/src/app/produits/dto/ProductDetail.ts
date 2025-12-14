import { DepotStock } from "../models/depotStock";
import { Product } from "../models/Product";

export interface ProductDetail  extends Product {
    depotStocks: DepotStock[];
}

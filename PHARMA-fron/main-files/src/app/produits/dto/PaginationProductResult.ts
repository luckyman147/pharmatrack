import { PaginatedResult } from "src/app/transaction/visite/models/PaginatedResult";
import { Product } from "../models/Product";

export interface PaginatedProductResult extends Partial<PaginatedResult<Product>> {}
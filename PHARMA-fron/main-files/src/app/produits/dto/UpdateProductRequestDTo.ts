export interface UpdateProductRequestDto {

        name: string;
        description: string;
        prixVente: number;
        categorieId: string;
        imageUrl?: string;
    
}
export interface CreateProductRequestDto {
    dto: {
        name: string;
        description: string;
        prixVente: number;
        categorieId: string;
        imageUrl?: string;
    };
}
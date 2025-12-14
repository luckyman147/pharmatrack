using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Commands.categorie.DeleteCAtegorie;

public class DeleteCategoryCommandHandler(
    ICategorieRepository categorieRepository,
    IProduitRepository produitRepository) : ICommandHandler<DeleteCategoryCommand, DeleteCategoryResult>
{
    private readonly ICategorieRepository _cate = categorieRepository;
    private readonly IProduitRepository _produitRepository = produitRepository;

    public async Task<DeleteCategoryResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        // Check if any products are using this category
        var productsInCategory = await _produitRepository.GetProduitsByCategorieAsync(
            request.id, 
            new PaginationRequest { PageIndex = 0, PageSize = 1 });

        if (productsInCategory.Count > 0)
        {
            throw new InvalidOperationException(
                $"Cannot delete category with id {request.id} because it has {productsInCategory.Count} product(s) associated with it. " +
                "Please reassign or delete the products first.");
        }

        await _cate.DeleteCategorieAsync(request.id);
        return new DeleteCategoryResult();
    }
}


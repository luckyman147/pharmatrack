using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Queries.CategorieQuery.GetAllGategoryQuery;

public class GetAllCategoriesQueryHandler(ICategorieRepository categorieRepository) : IQueryHandler<GetAllCategoryQuery, GetAllCategoryQueryResult>
{
    private readonly ICategorieRepository _cat = categorieRepository;
    public async Task<GetAllCategoryQueryResult> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _cat.GetCategoriesByNameAsync(request.name);
        if (categories == null || !categories.Any())
            throw new NotFoundException("emptuy");
        return new GetAllCategoryQueryResult(categories);
    }
}
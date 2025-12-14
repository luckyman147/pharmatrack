using PharmaTrack.Domain.Models.Produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.CategorieQuery.GetAllGategoryQuery;

public record  GetAllCategoryQuery(string? name):IQuery<GetAllCategoryQueryResult>;

public record GetAllCategoryQueryResult(IEnumerable<Categorie> Categories);
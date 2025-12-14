using System;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.Domain.Converters;

public static  class CategoryConverter
{
    public static CategoryInfo ToCategoryInfo(this Categorie? categorie)
    {
        if (categorie == null)
            throw new ArgumentNullException(nameof(categorie));

        return new CategoryInfo
        {
            id = categorie.id,
            name = categorie.name
        };
    }

    // Convert a list of Categorie to a list of CategoryInfo
    public static IEnumerable<CategoryInfo> ToCategoryInfoList( this IEnumerable<Categorie> categories)
    {
        if (categories == null)
            throw new ArgumentNullException(nameof(categories));

        return categories.Select(ToCategoryInfo).ToList();
    }
}

using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Commands.categorie.UpdateCategorie;

public class UpdateCatgoryCommandHandler(ICategorieRepository categorieRepository) : ICommandHandler<UpdateCategoryCommand, UpdateCategoryResult>
{
    private readonly ICategorieRepository _cat = categorieRepository;
    public async Task<UpdateCategoryResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _cat.GetCategorieByIdAsync(request.UpdateCategoryDto.Id);
        if (existingCategory == null)
        {
            throw new NotFoundException("not found");
        }
        existingCategory.name = request.UpdateCategoryDto.name;
        await _cat.UpdateCategorieAsync(existingCategory);
        return new UpdateCategoryResult(existingCategory);
    }
}

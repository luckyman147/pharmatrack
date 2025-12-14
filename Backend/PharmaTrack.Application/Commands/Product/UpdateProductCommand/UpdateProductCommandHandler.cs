using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Converters;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Commands.Product.UpdateProductCommand;

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProduitRepository _produitRepository;

    public UpdateProductCommandHandler(IProduitRepository produitRepository)
    {
        _produitRepository = produitRepository;
    }

    public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var produit = await _produitRepository.GetProduitByIdAsync(request.Id);
        if (produit == null)
        {
            throw new NotFoundException("Product not found");
        }
 

        produit.Name = request.Dto.name;
        produit.Description = request.Dto.description;
        produit.PrixVente = request.Dto.prixVente;
        produit.ImageUrl = request.Dto.imageUrl;

        Console.WriteLine($"Product with ID {produit.Id} updated successfully.");
        var produitUpdated = produit.ToProduit();
        Console.WriteLine($"Updated Product: {produitUpdated.id}");
        await _produitRepository.UpdateProduitAsync(produit.ToProduit(), request.Dto.categorieId);


        return new UpdateProductResult(produit.Id);
    }
}

using FluentValidation;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.UpdateProduitStock;

public record UpdateDepotProduitStockCommand(Guid depotId, LigneDepotProduit LigneDepotProduit) : ICommand<UpdateDepotProduitStockResult>;
public record UpdateDepotProduitStockResult(LigneDepotProduit LigneDepotProduit);

public class UpdateDepotProduitStockCommandValidator : AbstractValidator<UpdateDepotProduitStockCommand>
{
    public UpdateDepotProduitStockCommandValidator()
    {
        RuleFor(x => x.depotId).NotEqual(Guid.Empty).WithMessage("DepotId is required.");
        RuleFor(x => x.LigneDepotProduit.ProduitId).NotNull().WithMessage("LigneDepotProduit.ProduitId is required.").Must(x => x != Guid.Empty);
        RuleFor(x => x.LigneDepotProduit.Quantity).GreaterThan(0).WithMessage("LigneDepotProduit.Quantity must be greater than zero.");
        RuleFor(x => x.LigneDepotProduit.ProduitNom).NotEmpty().WithMessage("LigneDepotProduit.ProduitNom is required.");
    }
}


using PharmaTrack.Application.DTOs.produit;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Product.GetProductById;

public record  GetProductByIdCommand(Guid Id) : ICommand<GetProductByIdResult>;
public record GetProductByIdResult(ProduitDetailsDto? Produit);
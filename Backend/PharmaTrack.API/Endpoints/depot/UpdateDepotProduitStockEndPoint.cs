using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.depot.UpdateProduitStock;
using PharmaTrack.Domain.Models.depot;

namespace PharmaTrack.API.Endpoints.depot;
public record UpdateDepotProduitStockRequest(Guid depotId, LigneDepotProduit LigneDepotProduit);
public record UpdateDepotProduitStockResponse(LigneDepotProduit LigneDepotProduit);
public class UpdateDepotProduitStockEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/Depots/{depotId}/produit-stock", async (Guid depotId, UpdateDepotProduitStockRequest request, ISender mediator) =>
        {
            var command = new UpdateDepotProduitStockCommand(depotId, request.LigneDepotProduit);
            var result = await mediator.Send(command);
            return new UpdateDepotProduitStockResponse(result.LigneDepotProduit);
        }).WithName("UpdateDepotProduitStock").WithTags("Depots")
        .Produces<UpdateDepotProduitStockResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        ;
       
    }
}

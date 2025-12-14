using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.Product.GetProductById;
using PharmaTrack.Application.DTOs.produit;

namespace PharmaTrack.API.Endpoints.Produits;
public record GetProductByIdRequest(Guid Id);
public record GetProductByIdResponse(ProduitDetailsDto? Produit);
public class GetProductByIdEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/produits/{id}", async (Guid id, IMediator mediator) =>
        {
            var request = new GetProductByIdRequest(id);
            var command = new GetProductByIdCommand(request.Id);
            var result =  await mediator.Send(command);
            return Results.Ok(new GetProductByIdResponse(result.Produit));
        }).WithName("GetProductById").WithTags("Products");
    }
}

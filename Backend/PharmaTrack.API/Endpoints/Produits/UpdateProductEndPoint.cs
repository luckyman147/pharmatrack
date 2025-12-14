using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.Product.UpdateProductCommand;
using PharmaTrack.Application.DTOs.produit;

namespace PharmaTrack.API.Endpoints.Produits;
    public record UpdateProductRequest(Guid Id, UpdateProductDto Dto);
    public record UpdateProductResponse(Guid Id);
public class UpdateProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("products/{id}", async (Guid id, UpdateProductDto dto, ISender mediator) =>
        {
            var command = new UpdateProductCommand(id, dto);
            var result = await mediator.Send(command);
            return new UpdateProductResponse(result.Id);
        }).WithName("UpdateProduct").WithTags("Products")
        .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        ;
    }
}

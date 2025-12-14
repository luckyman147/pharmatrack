using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.Product.DeleteProductCommand;

namespace PharmaTrack.API.Endpoints.Produits;
public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(Guid Id);
public class DeleteProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{id}", async (Guid id, ISender mediator) =>
        {
            var command = new DeleteProductCommand(id);
            var result = await mediator.Send(command);
            return new DeleteProductResponse(result.Id);
        }).WithName("DeleteProduct").WithTags("Products")
        .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        ;
    }
}




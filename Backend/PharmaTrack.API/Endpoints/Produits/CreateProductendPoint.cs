using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.Product.CreateProduitCommand;
using PharmaTrack.Application.DTOs.produit;

namespace PharmaTrack.API.Endpoints.Produits;

public record CreateProductRequest(CreateProductDto Dto);
public record CreateProductResponse(Guid Id);
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender mediator) =>
        {
            var command = new CreateProductCommand(request.Dto);
            var result = await mediator.Send(command);
            return new CreateProductResponse(result.Id);
        }).WithName("CreateProduct").WithTags("Products")
        .Produces<CreateProductResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        ;
    }
}

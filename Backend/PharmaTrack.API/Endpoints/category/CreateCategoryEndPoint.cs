using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.categorie;
using PharmaTrack.Application.DTOs.Categorie;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.API.Endpoints.category;
public record CreateCategoryRequest(CreateCategorieDto CreateCategorieDto);
public record CreateCategoryResponse( Categorie Categorie);

public class CreateCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost("/Category", async ([FromBody ]CreateCategorieDto createCategoryRequest, ISender mediator) =>
        {
            var request = new CreateCategoryRequest(createCategoryRequest);
            var result = await mediator.Send(new CreateCategoryCommand(request.CreateCategorieDto));
            var reponse = new CreateCategoryResponse(result.categorie);
            return Results.Ok(reponse.Categorie);
        }).WithName("CreateCategory").WithTags("Category")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
        ;

    }
}

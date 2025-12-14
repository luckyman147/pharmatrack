using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.categorie.UpdateCategorie;
using PharmaTrack.Application.DTOs.Categorie;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.API.Endpoints.category;
public record UpdateCategoryRequest( UpdateCategoryDto dto);
public record UpdateCategoryResponse(Categorie Category);
public class UpdateCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
 app.MapPut("/Category", async ([FromBody] UpdateCategoryDto dto, IMediator mediator) =>
        {
            var request = new UpdateCategoryRequest( dto);
            var result = await mediator.Send(new UpdateCategoryCommand( request.dto));
            var response = new UpdateCategoryResponse(result.Categorie);
            return Results.Ok(response);
        })
         .WithName("UpdateCategory")
         .Produces<UpdateCategoryResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithTags("Category")
         .WithSummary("Update an existing Category by ID");    }
}

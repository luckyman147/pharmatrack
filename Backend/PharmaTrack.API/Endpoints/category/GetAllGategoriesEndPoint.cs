using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Queries.CategorieQuery.GetAllGategoryQuery;
using PharmaTrack.Domain.Models.Produit;

namespace PharmaTrack.API.Endpoints.category;
public record GetAllCategoryRequest(string? name);
public record GetAllCategoryResponse(IEnumerable <Categorie> categories);
public class GetAllGategoriesEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Category", async ([FromQuery] string?name, IMediator mediator) =>
        {
            var request = new GetAllCategoryRequest(name);
            var result = await mediator.Send(new GetAllCategoryQuery(request.name));
            var response = new GetAllCategoryResponse(result.Categories);
            return Results.Ok(response.categories);



        }).WithName("Get Category by pagination and filters").Produces<GetAllCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound).WithTags("Category");
        ;
    }
}

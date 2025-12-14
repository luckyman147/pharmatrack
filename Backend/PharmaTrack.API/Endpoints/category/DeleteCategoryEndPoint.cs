using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.categorie.DeleteCAtegorie;

namespace PharmaTrack.API.Endpoints.category;
public record DeleteCategoryRequest(Guid Id);
public record DeleteCategoryResponse();
public class DeleteCategoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete("/Category/{id}", async (Guid id, ISender mediator) =>
        {
     var request = new DeleteCategoryRequest(id);
            await mediator.Send(new DeleteCategoryCommand(request.Id));
            var response = new DeleteCategoryResponse();
            return Results.Ok(response);
        }).WithName("Delete Category").
        Produces<DeleteCategoryResponse>(StatusCodes.Status204NoContent).WithTags("Category")
        .ProducesProblem(StatusCodes.Status404NotFound).WithSummary("Delete a Category by ID");
    }
}

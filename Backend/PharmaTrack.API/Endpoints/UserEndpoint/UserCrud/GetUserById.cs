using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.API.Endpoints.UserEndpoint;

public record GetUserByIdRequest(Guid id);
public record GetUserByIdResponse(User user);
public class GetUserById : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/me/{id}", async (Guid id, ISender mediator, HttpContext httpContext) =>
        {
            var request = new GetUserByIdRequest(id);

            // Send the request via the mediator
            var result = await mediator.Send(new GetUserByIdQuery(request.id));
            var response = new GetUserByIdResponse(result.user);
               if (response.user == null)
            {
                return Results.NotFound($"User with ID {id} not found.");
            }

            return Results.Ok(response.user);

        })     .WithName("GetUserById")
        .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
     
    }
}

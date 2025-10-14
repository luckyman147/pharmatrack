using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Queries.User.GetDeleguets;
using PharmaTrack.Domain.Models;

namespace PharmaTrack.API.Endpoints.UserEndpoint;
public  record GetDeleguetsRequest(bool? isActive);
public  record GetDeleguetsResponse(IEnumerable<Delegue> Delegues);
public class GetDelegues : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapGet("/delegues", async([FromQuery] bool ? isActive, ISender mediator, HttpContext httpContext) =>
        {
            var request = new GetDeleguetsRequest(isActive);

            // Send the request via the mediator
            var result = await mediator.Send(new GetDelegateQuery(request.isActive));
            var response = new GetDeleguetsResponse(result.Delegates);
            return Results.Ok(response.Delegues);
            


        })   .WithName("Get Deleguets")
        .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);
     ;

    }
}

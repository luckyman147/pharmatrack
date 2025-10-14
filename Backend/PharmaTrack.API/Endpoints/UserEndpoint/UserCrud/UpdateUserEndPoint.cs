using System;
using System.ComponentModel;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.user.CurrentUser.UpdateUser;
using PharmaTrack.Application.DTOs.user;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;

namespace PharmaTrack.API.Endpoints.UserEndpoint.UserCrud;


public record UpdateUserRequest(UpdateUserDto UpdateUserDto);
public record UpdateUserResponse(UserProfile Profile);
public class UpdateUserEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPut("/User/{id}", async (Guid id, [FromBody] UpdateUserRequest request, ISender mediator) =>
        {   request.UpdateUserDto.Id = id;
            var result = await mediator.Send(new UpdateUserCommand( request.UpdateUserDto));
            var response = new UpdateUserResponse(result.Profile);
            return Results.Ok(response);
        }).WithName("Update user ").
        Produces<UpdateUserResponse>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status404NotFound);
        ;

    }
}

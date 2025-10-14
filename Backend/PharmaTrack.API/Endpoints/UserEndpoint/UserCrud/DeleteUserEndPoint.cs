using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Commands.user.CurrentUser.DeleteUser;

namespace PharmaTrack.API.Endpoints.UserEndpoint.UserCrud;

public record DeleteUserRequest(Guid id);
public record DeleteUserResponse(String message);

public class DeleteUserEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapDelete("/user/{id}", async (Guid id, ISender mediator, HttpContext httpContext) =>
        {

            var result = await mediator.Send(new DeleteUserCommand(id));
            var response = new DeleteUserResponse(result.message);
            return Results.NoContent();

        })
.WithName("Delete User").Produces(StatusCodes.Status204NoContent).ProducesProblem
(StatusCodes.Status404NotFound);

                ;

    }
}

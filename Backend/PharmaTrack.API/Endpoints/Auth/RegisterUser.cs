using System;
using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.user.RegisterUser;
using PharmaTrack.Application.DTOs;

namespace PharmaTrack.API.Endpoints.Auth;

public record RegisterUserRequest(RegisterUserDto user);
public record RegisterUserResponse(String Code);

public class RegisterUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Authentication/Register", async ([FromBody] RegisterUserRequest request, ISender mediator) =>
        {

          Console.WriteLine("RegisterUser endpoint called");    
          Console.WriteLine(request.user.Email);    
            var result = await mediator.Send(new RegisterUserCommand(request.user));
            var response = result.Adapt<RegisterUserResponse>();
            return Results.Ok(response);
        }).WithName("RegisterUser")
        .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Register a new user");
        
    
    }
}

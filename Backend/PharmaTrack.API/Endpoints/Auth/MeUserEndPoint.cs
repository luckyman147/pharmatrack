using System;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace PharmaTrack.API.Endpoints.Auth;
public record MeUserRequest();
public record MeUserResponse(bool isAuthenticated);
public class MeUserEndPoint : ICarterModule
{
    private readonly ILogger<MeUserEndPoint> logger;
    public MeUserEndPoint(ILogger<MeUserEndPoint> logger)
    {
        this.logger = logger;
    }
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Authentication/me", (HttpContext httpContextAccessor) =>
          {
              //logging 
              logger.LogInformation("MeUser endpoint called");

              var user = httpContextAccessor.User;
              var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
              Console.WriteLine($"IsAuthenticated: {isAuthenticated}");
              var response = new MeUserResponse(isAuthenticated);
              return Results.Ok(response);
          }).WithName("MeUser")
          .Produces<MeUserResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Get the authentication status of the current user");
    }
}

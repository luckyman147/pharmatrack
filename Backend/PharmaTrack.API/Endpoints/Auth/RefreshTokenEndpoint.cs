using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.user.RefreshToken;

namespace PharmaTrack.API.Endpoints.Auth;

public record RefreshTokenRequest(string? AccessToken, string? RefreshToken);

public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/Authentication/refresh", async ([FromBody] RefreshTokenRequest? request, ISender sender, HttpContext context) =>
        {
            // Try to get tokens from Body or Cookie
            var accessToken = request?.AccessToken ?? context.Request.Cookies["accessToken"];
            var refreshToken = request?.RefreshToken ?? context.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return Results.BadRequest("Tokens are missing");

            try 
            {
                var result = await sender.Send(new RefreshTokenCommand(accessToken, refreshToken));

                // Set Cookies
                context.Response.Cookies.Append("accessToken", result.AccessToken, 
                    new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });

                context.Response.Cookies.Append("refreshToken", result.RefreshToken, 
                    new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict, Expires = DateTime.UtcNow.AddDays(7) });

                return Results.Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithTags("Authentication")
        .WithName("RefreshToken")
        .Produces<RefreshTokenResult>(200)
        .Produces(401);
    }
}

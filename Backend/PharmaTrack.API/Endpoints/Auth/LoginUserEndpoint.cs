using System;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.Commands.user.Login;
using PharmaTrack.Application.DTOs;

namespace PharmaTrack.API.Endpoints.Auth;
public record LoginUserRequest(LoginDto login);
public record LoginUserResponse(string AccessToken, string accessTokenExpires, string RefreshToken,Guid Id);
public class LoginUserEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        _ = app.MapPost("/Authentication/login", static async ([FromBody] LoginUserRequest request, ISender mediator, HttpContext httpContext) =>
        {
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var result = await mediator.Send(new LoginUserCommand(new LoginDto(request.login.Code, request.login.Password , ipAddress) ));
            var loginResponse = new LoginUserResponse(result.AccessToken, result.AccessTokenExpiresAt.ToString(), result.RefreshToken, result.Id);
               httpContext. Response.Cookies.Append("accessToken",result. AccessToken, new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.Strict });
            httpContext.Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });
            return Results.Ok(loginResponse);
        }).WithName("LoginUser")
        .Produces<LoginUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Login a user and return JWT tokens");

    }
}
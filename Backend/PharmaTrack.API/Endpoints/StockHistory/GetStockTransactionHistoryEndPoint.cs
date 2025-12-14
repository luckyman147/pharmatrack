using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaTrack.Application.DTOs.StockHistory;
using PharmaTrack.Application.Queries.StockHistoryQuery.GetStockTransactionHistory;
using PharmaTrack.Shared.Pagination;
using PharmaTrack.Domain.Models.Shared;

namespace PharmaTrack.API.Endpoints.StockHistory;

public record GetStockHistoryResponse(PaginationResult<StockTransactionHistory> Result);

public class GetStockTransactionHistoryEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/StockHistory", async ([FromQuery] int pageIndex, [FromQuery] int pageSize, [FromBody] StockTransactionHistoryFilter? filter, ISender mediator) =>
        {
            var pagination = new PaginationRequest(pageIndex, pageSize);
            var query = new GetStockTransactionHistoryQuery(filter ?? new StockTransactionHistoryFilter(), pagination);
            var result = await mediator.Send(query);
            return Results.Ok(new GetStockHistoryResponse(result));
        })
        .WithName("GetStockHistory")
        .WithTags("StockHistory")
        .Produces<GetStockHistoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithDescription("Retrieve stock transaction history with optional filters (Date Range, Depot, Product, Command, Status).");
    }
}

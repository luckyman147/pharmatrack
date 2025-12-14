using System;
using Carter;
using MediatR;
using PharmaTrack.Application.Queries.DepotQuery.GetDepotById;
using PharmaTrack.Domain.Models.depot;

namespace PharmaTrack.API.Endpoints.depot;
public  record GetDepotByIdRequest(Guid id);
public record GetDepotByIdResponse(Depot depot);


public class GetDepotByIdEndPoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Depots/{id}", async (Guid id, ISender queryProcessor) =>
        {
            var request = new GetDepotByIdRequest(id);
            var result = await queryProcessor.Send(new GetDepotByIdQuery(request.id));
            return new GetDepotByIdResponse(result.Depot);
        }).WithName("GetDepotById").WithTags("Depots")
        .Produces<GetDepotByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }

}

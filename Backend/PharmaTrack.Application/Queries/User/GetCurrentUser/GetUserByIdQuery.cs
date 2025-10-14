using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Shared.CQRS;


public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdResult>;
public record GetUserByIdResult(User user);
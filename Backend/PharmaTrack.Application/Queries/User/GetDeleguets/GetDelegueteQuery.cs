using System;
using PharmaTrack.Domain.Models;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.User.GetDeleguets;

public record GetDelegateQuery(bool? isActive):IQuery<GetDelegateResult>;
public record GetDelegateResult(IEnumerable<Delegue> Delegates);



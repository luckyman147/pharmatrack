using System;
using MediatR;

namespace PharmaTrack.Shared.CQRS;
    public interface IQueryHandler<in TQuery, TResponse> :IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
using System;
using MediatR;

namespace PharmaTrack.Shared.CQRS;
  public interface IQuery<out TResponse>:IRequest<TResponse> where TResponse : notnull
  {
  }
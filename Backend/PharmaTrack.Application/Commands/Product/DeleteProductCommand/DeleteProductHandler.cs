using System;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Product.DeleteProductCommand;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(Guid Id);

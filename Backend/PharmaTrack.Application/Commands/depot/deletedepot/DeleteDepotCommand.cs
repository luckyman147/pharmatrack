using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.depot.deletedepot;

public record  DeleteDepotCommand(Guid Id) : ICommand<DeleteDepotResult>;
public record DeleteDepotResult(Guid Id);


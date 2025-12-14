using PharmaTrack.Domain.enums;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.UpdateCommandeStatus;

public record UpdateCommandeStatusCommand(Guid Id, StatusCommande NewStatus) : ICommand<UpdateCommandeStatusResult>;

public record UpdateCommandeStatusResult(bool IsSuccess);

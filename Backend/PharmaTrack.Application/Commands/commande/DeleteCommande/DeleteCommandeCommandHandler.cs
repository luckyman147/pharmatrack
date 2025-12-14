using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.enums;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.DeleteCommande;

public class DeleteCommandeCommandHandler(ICommandeRepository commandeRepository) 
    : ICommandHandler<DeleteCommandeCommand, DeleteCommandeResult>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;

    public async Task<DeleteCommandeResult> Handle(DeleteCommandeCommand request, CancellationToken cancellationToken)
    {
        var commande = await _commandeRepository.GetByIdAsync(request.Id);
        
        if (commande == null)
        {
            throw new Exception($"Commande with id {request.Id} not found");
        }

        // Prevent deletion if the commande status is Valide
        if (commande.Status == StatusCommande.Valide)
        {
            throw new InvalidOperationException($"Cannot delete commande with id {request.Id} because its status is 'Valide'. Only commandes with status 'EnAttente', 'EnRetour', or 'Annulee' can be deleted.");
        }

        await _commandeRepository.DeleteAsync(request.Id);

        return new DeleteCommandeResult(true);
    }
}

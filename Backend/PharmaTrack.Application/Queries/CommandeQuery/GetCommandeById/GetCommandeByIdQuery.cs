using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.CommandeQuery.GetCommandeById;

public record GetCommandeByIdQuery(Guid Id) : IQuery<GetCommandeByIdResult>;

public record GetCommandeByIdResult(Commande? Commande);

public class GetCommandeByIdQueryHandler(ICommandeRepository commandeRepository) 
    : IQueryHandler<GetCommandeByIdQuery, GetCommandeByIdResult>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;

    public async Task<GetCommandeByIdResult> Handle(GetCommandeByIdQuery request, CancellationToken cancellationToken)
    {
        var commande = await _commandeRepository.GetByIdAsync(request.Id);
        
        return new GetCommandeByIdResult(commande);
    }
}

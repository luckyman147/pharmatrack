using System.ComponentModel;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Domain.Models.Commande;
using PharmaTrack.Domain.Models.depot;
using PharmaTrack.Domain.Models.Shared;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.commande.AddCommande;

public class AddCommandeCommandHandler(
    ICommandeRepository commandeRepository,
    IActivityLogRepository activityLogRepository) 
    : ICommandHandler<AddCommandeCommand, AddCommandeResult>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;
    private readonly IActivityLogRepository _activityLogRepository = activityLogRepository;

    public async Task<AddCommandeResult> Handle(AddCommandeCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        
        var commande = new Commande
        {
            id = id,
            PharamcieInfo = request.CommandeDto.PharamcieInfo,
            UserInfo = request.CommandeDto.UserInfo,
            OrderDate = DateTime.Now,
            CodeComm = new Random().Next(100000, 999999),
            OriginalDocumentPath = request.CommandeDto.OriginalDocumentPath,
            Lines = request.CommandeDto.Lines.Select(static line => new LigneCommande
            {
                ProduitId = line.ProduitId,
                NomProuit = line.NomProuit,
                UniteProduitPrix = line.UniteProduitPrix,
                allocationsDepots = line.AllocationsDepots.Select(alloc => new AllocationDepot
                {
                    DepotInfo = new DepotBasicInfo
                    {
                        Name = alloc.DepotName,
                        address = alloc.DepotAddress,
                        Id = alloc.DepotId
                    },
                    QuantityAllocated = alloc.QuantityAllocated
                }).ToList()
            }).ToList()
        };

        await _commandeRepository.AddAsync(commande);
        
        if (commande == null)
        {
            throw new Exception("Commande not added");
        }

        // Add activity history log
        var activityLog = new ActivityLog
        {
            id = Guid.NewGuid(),
            UserId = request.CommandeDto.UserInfo.Id,
            UserName = request.CommandeDto.UserInfo.fullName,
            UserRole = "User",
            Action = "Create",
            EntityType = "Commande",
            EntityId = id,
            Description = $"Created new commande (Code: {commande.CodeComm}) for pharmacy '{commande.PharamcieInfo.name}' with {commande.Lines.Count} line(s)",
            Timestamp = DateTime.UtcNow
        };

        await _activityLogRepository.AddAsync(activityLog);

        return new AddCommandeResult(id);
    }
}

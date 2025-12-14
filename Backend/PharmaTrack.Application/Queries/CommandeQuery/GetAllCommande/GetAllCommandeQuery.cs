using PharmaTrack.Application.DTOs.commande;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Queries.CommandeQuery.GetAllCommande;

public record GetAllCommandeQuery(PaginationRequest PaginationRequest, CommandeFilter? Filter) 
    : IQuery<GetAllCommandeResult>;

public record GetAllCommandeResult(PaginationResult<CommandeSummaryInfo> Commandes);

public class GetAllCommandeQueryHandler(ICommandeRepository commandeRepository) 
    : IQueryHandler<GetAllCommandeQuery, GetAllCommandeResult>
{
    private readonly ICommandeRepository _commandeRepository = commandeRepository;

    public async Task<GetAllCommandeResult> Handle(GetAllCommandeQuery request, CancellationToken cancellationToken)
    {
        var allCommandes = await _commandeRepository.GetAllAsync();
        
        // Apply filters
        var filtered = allCommandes.AsEnumerable();
        
        if (request.Filter != null)
        {
            if (request.Filter.PharmacieId.HasValue)
            {
                filtered = filtered.Where(c => c.PharamcieInfo.Id == request.Filter.PharmacieId.Value);
            }
            
            if (request.Filter.Status.HasValue)
            {
                filtered = filtered.Where(c => c.Status == request.Filter.Status.Value);
            }
            
            if (request.Filter.StartDate.HasValue)
            {
                filtered = filtered.Where(c => c.OrderDate >= request.Filter.StartDate.Value);
            }
            
            if (request.Filter.EndDate.HasValue)
            {
                filtered = filtered.Where(c => c.OrderDate <= request.Filter.EndDate.Value);
            }
            
            if (request.Filter.MinAmount.HasValue)
            {
                filtered = filtered.Where(c => c.TotalAmount >= request.Filter.MinAmount.Value);
            }
            
            if (request.Filter.MaxAmount.HasValue)
            {
                filtered = filtered.Where(c => c.TotalAmount <= request.Filter.MaxAmount.Value);
            }
            
            if (request.Filter.CodeComm.HasValue)
            {
                filtered = filtered.Where(c => c.CodeComm == request.Filter.CodeComm.Value);
            }
        }
        
        var totalCount = filtered.Count();
        
        // Apply pagination
        var paginatedCommandes = filtered
            .Skip(request.PaginationRequest.PageIndex * request.PaginationRequest.PageSize)
            .Take(request.PaginationRequest.PageSize)
            .Select(c => new CommandeSummaryInfo
            {
                Id = c.id,
              
                PharamcieInfo = c.PharamcieInfo,
                UserInfo = c.UserInfo,
                OrderDate = c.OrderDate,
                Status = c.Status,
                TotalAmount = c.TotalAmount,
                CodeComm = c.CodeComm,
                TotalLines = c.Lines.Count
            })
            .ToList();
        
        var result = new PaginationResult<CommandeSummaryInfo>(
            request.PaginationRequest.PageIndex,
            request.PaginationRequest.PageSize,
            totalCount,
            paginatedCommandes
        );
        
        return new GetAllCommandeResult(result);
    }
}

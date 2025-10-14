using System;
using PharmaTrack.Application.DTOs.Visite;
using PharmaTrack.Domain.Models.Visite;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface IVisiteRepository
{
    Task CreateVisite(Visite visite);
    Task UpdateVisite(Visite visite);

    Task DeleteVisite(Guid id);
    
    Task<Visite> Get(Guid id);
    Task<IEnumerable<PaginationResult<Object>>> GetFiltredVisites();
    Task<PaginationResult<Visite>> GetVisitesByPharmacieId(Guid pharmacieId,PaginationRequest request);
    Task<PaginationResult<Object>> GetVisitesByDeleguesId(GetVisiteByDelegueDto getVisiteByDelegueDto);

}

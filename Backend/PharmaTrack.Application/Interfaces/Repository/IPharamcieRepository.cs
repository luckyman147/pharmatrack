using System;
using PharmaTrack.Application.DTOs.Pharamcie;
using PharmaTrack.Domain.Models.Pharamcie;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Application.Interfaces.Repository;

public interface IPharamcieRepository
{
    Task CreatePharmacie(Pharmacie Pharmacie);
    Task Updateharmacie(Pharmacie Pharmacie);
        Task<Pharmacie?> GetPharmacieById(Guid id);
    Task DeletePharmacie(Guid id);
    Task<PaginationResult<Pharmacie>> GetFilteredPharmaciesAsync(PharmacieFilter filter); 
   Task<IEnumerable<Pharmacie>> GetPharmacieByName(String name);
  public  Task<PaginationResult<object>> GetPharmaciesAsync(PharmacieFilter filter, string? groupBy = null);
    Task<IEnumerable<PharmacieGroupResult>> GroupPharmacieByAsync(string fieldName);

}

using System;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Application.DTOs.StockHistory;

public class StockTransactionHistoryFilter
{
    public Guid? DepotId { get; set; }
    public Guid? ProduitId { get; set; }
    public Guid? CommandeId { get; set; }
    public TransactionReason? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

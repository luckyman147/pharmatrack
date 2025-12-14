using System;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Domain.Models.Shared;

public class StockTransactionHistory : Entity<Guid>
{
    public Guid DepotId { get; set; }
    public string DepotName { get; set; }= string.Empty;
    public Guid ProduitId { get; set; }
    public string ProduitName { get; set; }= string.Empty;
    public Guid? CommandeId { get; set; }
    public int OldQuantity { get; set; }
    public int NewQuantity { get; set; }
    public TransactionReason Reason { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}

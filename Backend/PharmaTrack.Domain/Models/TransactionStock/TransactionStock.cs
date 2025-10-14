using System;
using MongoDB.Bson.Serialization.Attributes;
using PharmaTrack.Domain.Abstractions;
using PharmaTrack.Domain.enums;

namespace PharmaTrack.Domain.Models.TransactionStock;

public class TransactionStock : Entity<Guid>
{
  public Guid DepotId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? commandeId { get; set; }
        public Guid? AdminId { get; set; }
public TypeTransactionSTock TransactionType { get; set; }
        public int PreviousQuantity { get; set; }
        public int NewQuantity { get; set; }
}

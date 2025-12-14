using System;
using PharmaTrack.Domain.Abstractions;

namespace PharmaTrack.Domain.Models.Shared;

public class ActivityLog : Entity<Guid>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty; // Store name snapshot
    public string UserRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty; // e.g., "Create", "Delete", "Visit"
    public string EntityType { get; set; } = string.Empty; // e.g., "Product", "Depot", "Pharmacy"
    public Guid? EntityId { get; set; }
    public string Description { get; set; } = string.Empty; // e.g., "Added new product X"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

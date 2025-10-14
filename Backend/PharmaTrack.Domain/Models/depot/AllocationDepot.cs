using System;

namespace PharmaTrack.Domain.Models.depot;

public class AllocationDepot
{
        public Guid DepotId { get; set; }    // Reference to the Depot document
       
        // UML attribute
        public int QuantityAllocated { get; set; } 
}

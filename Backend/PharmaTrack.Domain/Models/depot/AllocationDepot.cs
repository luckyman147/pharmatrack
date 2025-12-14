using System;

namespace PharmaTrack.Domain.Models.depot;

public class AllocationDepot
{
        public required DepotBasicInfo DepotInfo { get; set; }
        
        
           // Reference to the Depot document
       
        // UML attribute
        public int QuantityAllocated { get; set; } 
}

using System;

namespace PharmaTrack.Application.DTOs.depot;

  public class DepotFilter
    {
        public string? Name { get; set; }
        public string? Telephone { get; set; }

        // Address-based filters
        public string? Zone { get; set; }
        public string? Gouvernorat { get; set; }
        public string? Delegation { get; set; }
    }
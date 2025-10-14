using System;
using PharmaTrack.Domain.Models.Visite;



// 1. DTO for a Grouped Result (used for both Date and Pharmacie grouping)
public class GroupedResult
{
    public string Title { get; set; } // The name of the group (Date string or Pharmacie name)
    public int Count { get; set; }    // Number of visits in the group
    public IEnumerable<Visite> Items { get; set; } // The actual visits in this group

    // Used internally for sorting groups (only available if grouped by Pharmacie)

}


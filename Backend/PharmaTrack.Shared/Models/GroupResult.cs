using System;

namespace PharmaTrack.Shared.Models;

public class GroupResult<T>
{
      public string Key { get; set; } = string.Empty;          // e.g. "Garde", "Nord-Est", etc.
    public int Count { get; set; }                           // number of pharmacies in this group
    public IEnumerable<T> data { get; set; } = new List<T>();

}

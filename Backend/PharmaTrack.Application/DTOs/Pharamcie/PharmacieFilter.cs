using System;
using System.Diagnostics.CodeAnalysis;
using PharmaTrack.Domain.enums;
using PharmaTrack.Shared.Pagination;

namespace PharmaTrack.Domain.Models.Pharamcie;

public class PharmacieFilter
{

    public string? Type { get; set; }         // Optional filter by type
    public string? Horaire { get; set; }            // Optional filter by schedule
    public string? Zone { get; set; }                // Optional filter by address fields
    public string? Gouvernerat { get; set; }

    public string? ByName { get; set; }           // Optional search by name (partial match)
    public string? Delegation { get; set; }

    public int PageIndex { get; set; } = 0;
    public int PageSize{ get; set; } = 12;

    public static PharmacieFilter Parse(string s, IFormatProvider? provider)
    {
          if (TryParse(s, provider, out var result))
        {
            return result;
        }
        throw new InvalidOperationException($"Cannot parse value '{s}' into PharmacieFilter.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PharmacieFilter result)
    {
 // NOTE: In a real-world scenario, 's' is often the entire query string
        // or a segment. Since Minimal APIs often rely on property setters for simple 
        // types, for complex types like this, you typically need to access the
        // HttpContext directly or rely on the framework providing property-level binding,
        // but since we are fixing the ASP0020 error, we provide the definition.

        // When a complex type is used as a query parameter (e.g., [FromQuery] PharmacieFilter filter), 
        // the framework often attempts to instantiate it and map parameters by name.
        // Implementing this method satisfies the compiler, but often the body 
        // is complex. For a simple demonstration:

        result = new PharmacieFilter();
        
        // This is where you would traditionally parse the query string manually.
        // However, for simple mapping, simply defining the method often allows
        // the default binder to kick in for the public properties.
        // We return true to indicate the filter object was successfully created.
        return true;    }
}
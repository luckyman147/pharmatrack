using MongoDB.Driver;

namespace PharmaTrack.Infrastructure.Helper;

public class FilterHelper 
{
    public static FilterDefinition<T> BuildFilter<T>(Action<FilterDefinitionBuilder<T>, List<FilterDefinition<T>>>? applyFilters = null)
    {
        var builder = Builders<T>.Filter;
        var filters = new List<FilterDefinition<T>>();

        // Let the caller populate the filters list
        applyFilters?.Invoke(builder, filters);

        // Combine all filters into one
        return filters.Count > 0 ? builder.And(filters) : builder.Empty;
    }
}
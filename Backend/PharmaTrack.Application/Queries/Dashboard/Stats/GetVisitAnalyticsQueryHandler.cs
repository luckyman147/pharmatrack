using PharmaTrack.Application.DTOs.Dashboard;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.Dashboard.Stats;

public class GetVisitAnalyticsQueryHandler : IQueryHandler<GetVisitAnalyticsQuery, VisitAnalyticsDto>
{
    public Task<VisitAnalyticsDto> Handle(GetVisitAnalyticsQuery request, CancellationToken cancellationToken)
    {
        // Placeholder as Visits logic is mocked
        return Task.FromResult(new VisitAnalyticsDto
        {
            Stats = new StatCardDto { Value = 0, GrowthPercentage = 0, IsPositive = true },
            DelegateVisits = [],
            TopVisitedPharmacies = [],
            VisitsTrend = []
        });
    }
}

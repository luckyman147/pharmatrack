using System;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Domain.Models;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.User.GetDeleguets;

public class GetDelegateQueryHandler(IUserRepository userRepository) : IQueryHandler<GetDelegateQuery, GetDelegateResult>
{
        private readonly IUserRepository _user = userRepository;

    public async Task<GetDelegateResult> Handle(GetDelegateQuery request, CancellationToken cancellationToken)
    {
        var deleguets = await _user.GetDeleguesAsync(request.isActive);
    // ✅ 1. If list is empty → throw exception
            if (deleguets == null || !deleguets.Any())
                throw new InvalidOperationException("Aucun délégué trouvé.");

            // ✅ 2. Convert to IEnumerable (if it's a List, it's already fine)
            IEnumerable<Delegue> deleguesEnumerable = deleguets.AsEnumerable();

            // ✅ 3. Return as result
            return new GetDelegateResult(deleguesEnumerable);
    }
}

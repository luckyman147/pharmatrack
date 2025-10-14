using System;
using System.ComponentModel.DataAnnotations;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Application.Queries.User.GetCurrentUser;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Queries.User.GetCurrentUser;

public class GetUserByIdHandler(IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
{
    private readonly IUserRepository _userRepo=userRepository;



    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(request.Id);

        if (user == null)
        {
            throw  new  ValidationException("Wrong Id");
        }

        var result = new GetUserByIdResult(user);

        return result;
    }
}

using System;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Domain.Models;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Commands.user.CurrentUser.DeleteUser;

public class DeleteUserCommandHandler(IUserRepository userRepository) : ICommandHandler<DeleteUserCommand, DeleteUserResult>
{
    private readonly IUserRepository _user = userRepository;
    public async Task<DeleteUserResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _user.GetByIdAsync(request.id);

        if (user is null)
        {
            // If the user doesn't exist, return a NotFound failure result
            throw new NotFoundException("user Not Found");
        }
        await _user.DeleteAsync(request.id);
        return new DeleteUserResult("Deleted with success");


    }
}

using System;
using PharmaTrack.Application.DTOs.user;
using PharmaTrack.Application.Interfaces;
using PharmaTrack.Domain.Models;
using PharmaTrack.Domain.Models.shared;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Shared.CQRS;
using PharmaTrack.Shared.Exceptions;

namespace PharmaTrack.Application.Commands.user.CurrentUser.UpdateUser;

public class UpdateUserCommandHandler(IUserRepository userRepository) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _user = userRepository;
    public async Task<UpdateUserResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
           
              User? user = await _user.GetByIdAsync(request.updateUserdto.Id);

            if (user is null)
            {
                // If the user doesn't exist, return a NotFound failure result
                throw new NotFoundException("user Not Found");
            }

            // Destructure the DTO for clarity
            UpdateUserDto dto = request.updateUserdto;
        // var adress = new  Adress( dto.Zone, dto.Gouvernerat,dto.delegation, dto.Rue);
        // var profile = new UserProfile(dto.PhoneNumber,adress,dto.Speciality,dto.AvatarUrl);
        user.SetProfile(dto.userProfile);
        user.LastModified = DateTime.Now;
            // 3. Persist the changes
        await _user.UpdateAsync(user);
            
            // 4. Return success with the newly updated profile object
            // The profile is now retrieved directly from the updated user object.
            return new  UpdateUserResult (  user.Profile);
    }
}

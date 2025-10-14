using FluentValidation;
using PharmaTrack.Application.DTOs.user;
using PharmaTrack.Application.validators;
using PharmaTrack.Domain.Models.user;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.CurrentUser.UpdateUser;

public record  UpdateUserCommand(UpdateUserDto updateUserdto):ICommand<UpdateUserResult>;
public record UpdateUserResult(UserProfile Profile);
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.updateUserdto.userProfile).SetValidator(new ProfileValidator());
        
    }
}

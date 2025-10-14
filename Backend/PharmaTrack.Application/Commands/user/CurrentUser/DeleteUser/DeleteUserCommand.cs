using System;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.user.CurrentUser.DeleteUser;

public record DeleteUserResult(string message );

public record DeleteUserCommand(Guid id ):ICommand<DeleteUserResult>;
using FluentValidation;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.VisiteCommand.DeleteVisite;

            public record  DeleteVisiteCommand(Guid Id) : ICommand<DeleteVisiteResult>;
            public record DeleteVisiteResult(bool IsDeleted);

            public class DeleteVisiteCommandValidator : AbstractValidator<DeleteVisiteCommand>
            {
                public DeleteVisiteCommandValidator()
                {
                    RuleFor(x => x.Id).NotEmpty().Must(id => id != Guid.Empty);
                }
            }

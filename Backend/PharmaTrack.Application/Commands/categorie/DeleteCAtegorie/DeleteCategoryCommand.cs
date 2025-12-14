using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.categorie.DeleteCAtegorie;

public record  DeleteCategoryCommand (Guid id):ICommand<DeleteCategoryResult>;

public record DeleteCategoryResult (); 

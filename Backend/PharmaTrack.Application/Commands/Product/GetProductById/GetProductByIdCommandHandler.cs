using System;
using PharmaTrack.Application.Interfaces.Repository;
using PharmaTrack.Shared.CQRS;

namespace PharmaTrack.Application.Commands.Product.GetProductById;

public class GetProductByIdCommandHandler : ICommandHandler<GetProductByIdCommand, GetProductByIdResult>
{
    private readonly IProduitRepository _productRepository;

    public GetProductByIdCommandHandler(IProduitRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductByIdResult> Handle(GetProductByIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProduitByIdAsync(request.Id);
        return new GetProductByIdResult(product);
    }
}

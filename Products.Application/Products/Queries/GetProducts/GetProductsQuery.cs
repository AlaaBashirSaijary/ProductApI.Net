using MediatR;
using Products.Application.Abstractions;

namespace Products.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<ProductDto>>;

public class GetProductsQueryHandler(IProductRepository repository)
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken ct)
        => (await repository.GetAllAsync(ct)).Select(ProductDto.From).ToList();
}

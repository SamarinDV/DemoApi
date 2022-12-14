using Microsoft.EntityFrameworkCore;
using ShoppingCart.AppServices.Product.Repositories;
using ShoppingCart.Contracts;
using ShoppingCart.Contracts.Product;
using ShoppingCart.Infrastructure.Repository;

namespace ShoppingCart.DataAccess.EntityConfigurations.Product;

/// <inheritdoc />
public class ProductRepository : IProductRepository
{
    private readonly IRepository<Domain.Product> _repository;

    /// <summary>
    /// Инициализирует экземпляр <see cref="ProductRepository"/>.
    /// </summary>
    /// <param name="repository">Базовый репозиторий.</param>
    public ProductRepository(IRepository<Domain.Product> repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ProductDto>> GetAll(int take, int skip, CancellationToken cancellation)
    {
        return await _repository.GetAll()
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            })
            .Take(take).Skip(skip).ToListAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ProductDto>> GetAllFiltered(ProductFilterRequest request,
        CancellationToken cancellation)
    {
        var query = _repository.GetAll();

        if (request.Id.HasValue)
        {
            query = query.Where(p => p.Id == request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(request.Name.ToLower()));
        }
            
        return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            }).ToListAsync(cancellation);
    }
}
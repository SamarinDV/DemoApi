using Microsoft.EntityFrameworkCore;
using ShoppingCart.AppServices.ShoppingCart.Repositories;
using ShoppingCart.Contracts;
using ShoppingCart.Infrastructure.Repository;

namespace ShoppingCart.DataAccess.EntityConfigurations.ShoppingCart;

/// <inheritdoc />
public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly IRepository<Domain.ShoppingCart> _repository;

    /// <summary>
    /// Инициализирует экземпляр <see cref="ShoppingCartRepository"/>.
    /// </summary>
    /// <param name="repository">Базовый репозиторий.</param>
    public ShoppingCartRepository(IRepository<Domain.ShoppingCart> repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<ShoppingCartDto>> GetAllAsync(CancellationToken cancellation)
    {
        return await _repository.GetAll()
            .Include(s => s.Product)
            .Select(s => new ShoppingCartDto
            {
                Id = s.Id,
                ProductName = s.Product.Name,
                Quantity = s.Quantity,
                Price = s.Price,
                Amount = s.Amount
            }).ToListAsync(cancellation);
    }

    /// <inheritdoc />
    public async Task UpdateQuantityAsync(Guid id, int quantity, CancellationToken cancellation)
    {
        var existingCart = await _repository.GetByIdAsync(id);

        if (existingCart == null)
        {
            throw new InvalidOperationException($"Корзины с идентификатором {id} не найдено!");
        }
        
        existingCart.Quantity = quantity;
        await _repository.UpdateAsync(existingCart);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellation)
    {
        var existingCart = await _repository.GetByIdAsync(id);

        if (existingCart == null)
        {
            throw new InvalidOperationException($"Корзины с идентификатором {id} не найдено!");
        }
        
        await _repository.DeleteAsync(existingCart);
    }
}
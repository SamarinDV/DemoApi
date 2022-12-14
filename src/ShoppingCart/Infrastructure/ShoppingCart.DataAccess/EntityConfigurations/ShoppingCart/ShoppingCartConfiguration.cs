using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoppingCart.DataAccess.EntityConfigurations.ShoppingCart;

/// <summary>
/// Конфигурация таблицы ShoppingCarts.
/// </summary>
public class ShoppingCartConfiguration : IEntityTypeConfiguration<Domain.ShoppingCart>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Domain.ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd();
        
    }
}
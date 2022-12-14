using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoppingCart.DataAccess.Interfaces;

namespace ShoppingCart.DataAccess;

/// <summary>
/// Конфигурация контекста БД.
/// </summary>
public class ShoppingCartContextConfiguration : IDbContextOptionsConfigurator<ShoppingCartContext>
{
    private const string PostgesConnectionStringName = "PostgresShoppingCartDb";
    private const string MsSqlShoppingCartDb = "MsSqlShoppingCartDb";
    private readonly IConfiguration _configuration;
    private readonly ILoggerFactory _loggerFactory;

    /// <summary>
    /// Инициализирует экземпляр <see cref="ShoppingCartContextConfiguration"/>.
    /// </summary>
    /// <param name="configuration">Конфигурация.</param>
    /// <param name="loggerFactory">Фабрика средства логирования.</param>
    public ShoppingCartContextConfiguration(ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _loggerFactory = loggerFactory;
        _configuration = configuration;
    }

    /// <inheritdoc />
    public void Configure(DbContextOptionsBuilder<ShoppingCartContext> options)
    {
        string connectionString;

         var useMsSql = _configuration.GetSection("DataBaseOptions:UseMsSql").Get<bool>();

        if (!useMsSql)
        {
            connectionString = _configuration.GetConnectionString(PostgesConnectionStringName);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    $"Не найдена строка подключения с именем '{PostgesConnectionStringName}'");
            }
            options.UseNpgsql(connectionString);
        }
        else
        {
            connectionString = _configuration.GetConnectionString(MsSqlShoppingCartDb);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    $"Не найдена строка подключения с именем '{MsSqlShoppingCartDb}'");
            }
            options.UseSqlServer(connectionString);
        }
        
        options.UseLoggerFactory(_loggerFactory);
    }
}
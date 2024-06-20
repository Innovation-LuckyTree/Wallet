using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WalletService.Application.Interfaces;

namespace WalletService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<WalletDbContext>(opts => opts.UseSqlServer(connectionString));

        services.AddScoped<IWalletDbContext>(provider => provider.GetService<WalletDbContext>());

        return services;
    }
}


using Microsoft.EntityFrameworkCore;
using Wallet.Config;
using Wallet.Data;
using Wallet.Services.Interface;
using Wallet.Services;

namespace Wallet;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configuration, services, logging, etc. can be set up here
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Database configurations
        builder.Services.AddDbContext<PaymentTransactionDbContext>(options =>
          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddDbContext<LedgerWalletDbContext>(options =>
          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Service registrations
        builder.Services.AddSingleton<ILedgerService, LedgerService>();
        builder.Services.AddSingleton<ITransactionService, TransactionService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseRouting();

        app.MapControllers();

        app.Run();
    }
}
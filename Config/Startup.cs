using Wallet.Services.Interface;
using Wallet.Services;

namespace Wallet.Config
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register your services here
            services.AddSingleton<ILedgerService, LedgerService>();
            services.AddSingleton<ITransactionService, TransactionService>(); // Replace with your actual implementation
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

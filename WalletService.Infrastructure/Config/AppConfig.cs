using WalletService.Infrastructure.Interfaces;

namespace WalletService.Infrastructure.Config;

public class AppConfig : IAppConfig
{
    public string AppId { get; set; }
    public string MobileAppId { get; set; }
    public JwtConfig JwtConfig { get; set; }
}

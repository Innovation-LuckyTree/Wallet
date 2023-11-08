using WalletService.Infrastructure.Config;

namespace WalletService.Infrastructure.Interfaces;

public interface IAppConfig
{
    string AppId { get; set; }
    string MobileAppId { get; set; }
    JwtConfig JwtConfig { get; set; }
}
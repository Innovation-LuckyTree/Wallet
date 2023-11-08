namespace WalletService.Application.Interfaces;
public interface ICurrentUserService
{
    string UserId { get; }
    string AuthenticationBearer { get; }
}
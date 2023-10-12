using Wallet.Services.Interface;

namespace Wallet.Services.Factory;

public record WalletEventResult : IWalletEventResult
{
   
    public IWalletEventResult.Status isSuccess { get; init; } = IWalletEventResult.Status.Pending;
    public string? Message { get; init; }
    public object? Data { get; init; }
}
public class WalletEventResultFactory : IWalletEventResultFactory
{
    public IWalletEventResult CreateSuccessResult(string message, object data)
    {
        return new WalletEventResult
        {
            isSuccess = IWalletEventResult.Status.Success,
            Message = message,
            Data = data
        };
    }

    public IWalletEventResult CreateFailureResult(string message)
    {
        return new WalletEventResult
        {
            isSuccess = IWalletEventResult.Status.Failed,
            Message = message,
        };
    }
}

namespace Wallet.Services.Factory
{
    public record TransactionEventResult : ITransactionEventResult
    {
        public enum Status { Success, Failed, Pending};
        public Status Success { get; set; } = Status.Pending;
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class TransactionEventResultFactory : ITransactionEventResultFactory
    {
        public ITransactionEventResult CreateSuccessResult(string message, object data)
        {
            return new TransactionEventResult
            {
                Success = TransactionEventResult.Status.Success,
                Message = message,
                Data = data
            };
        }

        public ITransactionEventResult CreateFailureResult(string message)
        {
            return new TransactionEventResult
            {
                Success = TransactionEventResult.Status.Failed,
                Message = message,
            };
        }
    }
}

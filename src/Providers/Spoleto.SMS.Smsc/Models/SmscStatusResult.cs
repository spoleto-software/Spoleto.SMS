namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Delivery status. Check <see cref="IsSuccess"/> or <see cref="IsBatchSuccess"/> before accessing <see cref="Success"/> or <see cref="Error"/>.
    /// </summary>
    public record SmscStatusResult
    {
        public bool IsSuccess => Success is not null;

        public bool IsBatchSuccess => BatchSuccess is not null;

        public SmscStatusSuccessResult? Success { get; set; }

        public SmscBatchStatusSuccessResult? BatchSuccess { get; set; }

        public SmscErrorResult? Error { get; set; }

        public static SmscStatusResult Ok(SmscStatusSuccessResult success) => new() { Success = success };

        public static SmscStatusResult OkBatch(SmscBatchStatusSuccessResult batchSuccess) => new() { BatchSuccess = batchSuccess };

        public static SmscStatusResult Fail(SmscErrorResult error) => new() { Error = error };
    }
}

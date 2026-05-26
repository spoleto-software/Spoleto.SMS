namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Result of a smsc query. Check <see cref="IsSuccess"/> before accessing <see cref="Success"/> or <see cref="Error"/>.
    /// </summary>
    public record SmscResult<TSuccess> : IResult<TSuccess>
    {
        public bool IsSuccess => Success is not null;

        public TSuccess? Success { get; set; }

        public SmscErrorResult? Error { get; set; }

        public static SmscResult<TSuccess> Ok(TSuccess success) => new() { Success = success };

        public static SmscResult<TSuccess> Fail(SmscErrorResult error) => new() { Error = error };
    }
}

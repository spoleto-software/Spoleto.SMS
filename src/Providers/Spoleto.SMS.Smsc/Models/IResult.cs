namespace Spoleto.SMS.Providers.Smsc
{
    public interface IResult
    {
        bool IsSuccess { get; }

        public SmscErrorResult? Error { get; set; }
    }

    public interface IResult<TSuccess>
    {
        TSuccess? Success { get; set; }
    }
}

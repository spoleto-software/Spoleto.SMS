namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Successful result of a send operation (<c>cost=3, fmt=1</c>):
    /// <c>&lt;id&gt;,&lt;cnt&gt;,&lt;cost&gt;,&lt;balance&gt;</c>
    /// </summary>
    public record SmscSendSuccessResult
    {
        public string Id { get; set; }

        public int SmsCount { get; set; }

        public decimal Cost { get; set; }

        public decimal Balance { get; set; }
    }
}

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Cost-estimation result (<c>cost=1, fmt=1</c>): <c>&lt;cost&gt;,&lt;cnt&gt;</c>
    /// </summary>
    public record SmscCostSuccessResult
    {
        public decimal Cost { get; set; }

        public int SmsCount { get; set; }
    }
}

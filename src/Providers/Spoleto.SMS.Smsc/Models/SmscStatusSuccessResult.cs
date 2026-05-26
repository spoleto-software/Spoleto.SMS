namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Delivery status for a single SMS message (<c>all=0, fmt=1</c>):
    /// <c>&lt;status&gt;,&lt;last_timestamp&gt;,&lt;err&gt;</c>
    /// </summary>
    public record SmscStatusSuccessResult
    {
        /// <summary>
        /// Delivery status code. See statuses reference page.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// UTC time of the last status change, or null if not yet changed.
        /// </summary>
        public DateTimeOffset? LastChanged { get; set; }

        /// <summary>
        /// Delivery error code (0 = no error).
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Additional fields returned when <c>all=1</c> or <c>all=2</c> is requested.
        /// For SMS <c>all=1</c>: send_timestamp, phone, cost, sender, status_name, message, comment, type.
        /// For SMS <c>all=2</c>: same + country, operator, region inserted after phone.
        /// For HLR <c>all=0</c>: imsi, msc, mcc, mnc, cn, net, rcn, rnet.
        /// </summary>
        public IReadOnlyList<string> ExtraInfo { get; set; }
    }
}

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// API error response.
    /// For error codes 1,2,4,5,9 the server returns <c>0,-N</c>.
    /// For error codes 3,6,7,8 (where a message ID was assigned) the server returns <c>&lt;id&gt;,-N</c>.
    /// </summary>
    /// <param name="MessageId">Assigned message ID, or "0" when no ID was assigned.</param>
    /// <param name="ErrorCode">Positive numeric error code.</param>
    public record SmscErrorResult
    {
        public string MessageId { get; set; }

        public int ErrorCode { get; set; }
    }
}

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Batch status result — one row per requested message ID.
    /// Each row is the raw comma-separated response split into fields.
    /// </summary>
    public record SmscBatchStatusSuccessResult
    {
        public IReadOnlyList<IReadOnlyList<string>> Rows { get; set; }
    }
}

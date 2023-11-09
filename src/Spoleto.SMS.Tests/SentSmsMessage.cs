namespace Spoleto.SMS.Tests
{
    internal record SentSmsMessage
    {
        public string Id { get; init; }

        public string To { get; init; }
    }
}

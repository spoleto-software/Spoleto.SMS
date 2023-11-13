namespace Spoleto.SMS
{
    public record GetSmsOptions
    {
        public string Login { get; init; }

        public string Password { get; init; }

        public string ServiceUrl { get; init; }

        public string? Nickname { get; init; }

    }
}

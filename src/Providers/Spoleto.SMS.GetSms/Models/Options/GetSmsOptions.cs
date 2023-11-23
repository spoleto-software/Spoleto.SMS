namespace Spoleto.SMS.Providers.GetSms
{
    public record GetSmsOptions
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string ServiceUrl { get; set; } = "http://185.8.212.184/smsgateway/";

        /// <summary>
        /// Checks that all the settings within the options are configured properly.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Login"/> or <see cref="Password"/> or <see cref="ServiceUrl"/> are null.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Login))
                throw new ArgumentNullException($"{nameof(Login)}");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException($"{nameof(Password)}");

            if (string.IsNullOrWhiteSpace(ServiceUrl))
                throw new ArgumentNullException($"{nameof(ServiceUrl)}");
        }
    }
}

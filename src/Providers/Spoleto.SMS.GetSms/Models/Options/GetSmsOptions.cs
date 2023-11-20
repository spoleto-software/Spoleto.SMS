namespace Spoleto.SMS.Providers.GetSms
{
    public record GetSmsOptions
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string ServiceUrl { get; set; }

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
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

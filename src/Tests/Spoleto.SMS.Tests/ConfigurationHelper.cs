using Microsoft.Extensions.Configuration;

namespace Spoleto.SMS.Tests
{
    internal static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _config;

        static ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true)
               .AddUserSecrets("dca10f99-9e5e-4110-b1e1-41945474b031")
               .Build();
        }

        public static IConfigurationRoot Configuration => _config;

        public static SmsMessage GetSmsMessageSmsc()
        {
            var sms = _config.GetSection("SmsMessageSmsc").Get<SmsMessage>()!;

            return sms;
        }

        public static SmsMessage GetSmsMessageGetSms()
        {
            var sms = _config.GetSection("SmsMessageGetSms").Get<SmsMessage>()!;

            return sms;
        }

        public static SentSmsMessage GetSentSmsMessageSmsc()
        {
            var sms = _config.GetSection("SentSmsMessageSmsc").Get<SentSmsMessage>()!;

            return sms;
        }

        public static SentSmsMessage GetSentSmsMessageGetSms()
        {
            var sms = _config.GetSection("SentSmsMessageGetSms").Get<SentSmsMessage>()!;

            return sms;
        }
    }
}

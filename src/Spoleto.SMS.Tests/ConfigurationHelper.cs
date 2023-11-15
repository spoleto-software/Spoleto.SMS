using Microsoft.Extensions.Configuration;
using Spoleto.SMS;
using Spoleto.SMS.Tests;

namespace CIS.Service.Client.Tests
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

        public static SmsMessage GetSmsMessage()
        {
            var sms = _config.GetSection(nameof(SmsMessage)).Get<SmsMessage>()!;

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

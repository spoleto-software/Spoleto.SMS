using Microsoft.Extensions.Configuration;

namespace Spoleto.SMS.Tests
{
    internal class SmsMessageFactory
    {
        public SmsMessage Create(IConfiguration config)
        {
            // Determine which constructor parameters are available in the config
            // and create an instance of SmsMessage with the appropriate constructor.
            // This is a sample and must be implemented according to the logic you need.
            var body = config["Body"];
            var from = config["From"];
            var to = config["To"];

            // If 'listOfTo' is provided in the configuration, use the constructor which accepts a list
            var listOfToConfig = config.GetSection("ListOfTo");
            if (listOfToConfig.Exists())
            {
                var listOfTo = listOfToConfig.Get<List<string>>();
                return new SmsMessage(body, from, listOfTo);
            }

            // Otherwise, use the constructor that operates on a single 'To'
            return new SmsMessage(body, from, to);
        }
    }
}

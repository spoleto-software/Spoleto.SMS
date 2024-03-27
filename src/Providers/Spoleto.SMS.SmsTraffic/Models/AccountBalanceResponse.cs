using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    [XmlRoot("reply")]
    public class AccountBalanceResponse
    {
        [XmlElement("account")]
        public int Account { get; set; }
    }
}

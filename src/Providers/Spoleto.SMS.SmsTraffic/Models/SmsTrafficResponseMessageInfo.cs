using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    public class SmsTrafficResponseMessageInfo
    {
        [XmlElement(ElementName = "phone")]
        public long Phone { get; set; }

        [XmlElement(ElementName = "sms_id")]
        public long SmsId { get; set; }

        [XmlElement(ElementName = "push_id")]
        public string PushId { get; set; }
    }
}
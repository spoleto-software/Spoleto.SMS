using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    public class SmsTrafficResponseMessageInfos
    {
        [XmlElement(ElementName = "message_info")]
        public List<SmsTrafficResponseMessageInfo> MessageInfo { get; set; }
    }
}
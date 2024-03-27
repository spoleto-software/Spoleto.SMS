using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    [XmlRoot(ElementName = "reply")]
    public class SmsTrafficResponse
    {
        public const string SuccessfulCode = "OK";

        [XmlElement(ElementName = "result")]
        public string Result { get; set; }

        [XmlElement(ElementName = "code")]
        public int Code { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "message_infos")]
        public SmsTrafficResponseMessageInfos MessageInfos { get; set; }
    }
}

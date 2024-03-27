using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    [XmlRoot("reply")]
    public class GroupListInformation
    {
        [XmlElement("result")]
        public string Result { get; set; }

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("groups")]
        public Groups Groups { get; set; }
    }
}

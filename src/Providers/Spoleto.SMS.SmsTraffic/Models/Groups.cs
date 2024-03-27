using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    public class Groups
    {
        [XmlElement("group")]
        public List<GroupInformation> GroupList { get; set; }
    }
}
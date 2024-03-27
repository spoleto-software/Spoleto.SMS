using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    /// <summary>
    /// The information of adding or removing members from the SMS group.
    /// </summary>
    [XmlRoot("reply")]
    public class GroupOperation
    {
        [XmlElement("result")]
        public string Result { get; set; }

        [XmlElement("code")]
        public int Code { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        public override string ToString() => Description;
    }
}
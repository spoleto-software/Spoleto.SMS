using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    [XmlRoot(ElementName = "reply")]
    public class SmsTrafficStatusResponse
    {
        [XmlElement("error")]
        public string Error { get; set; }

        [XmlElement("submition_date")]
        public string SubmissionDate { get; set; }

        [XmlElement("send_date")]
        public string SendDate { get; set; }

        [XmlElement("last_status_change_date")]
        public string LastStatusChangeDate { get; set; }

        [XmlElement("sms_id")]
        public long SmsId { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }
    }
}

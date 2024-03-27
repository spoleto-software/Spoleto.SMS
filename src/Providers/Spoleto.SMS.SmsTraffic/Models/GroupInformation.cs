using System.Xml.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    public class GroupInformation
    {
        /// <summary>
        /// уникальный идентификатор списка для рассылки.
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// Текстовое имя списка для рассылки. Отображается в личном кабинете.
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }

        /// <summary>
        /// Дата и время создания списка. Указывается в часовом поясе Москвы.
        /// </summary>
        [XmlElement("created")]
        public string Created { get; set; }

        /// <summary>
        /// Cтатус поздравления c днём рождения.
        /// </summary>
        [XmlElement("congratulate")]
        public string Congratulate { get; set; }

        public override string ToString() => Name;
    }
}
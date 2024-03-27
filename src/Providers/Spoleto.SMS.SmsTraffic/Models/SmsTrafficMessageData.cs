namespace Spoleto.SMS.Providers.SmsTraffic
{
    public record SmsTrafficMessageData
    {
        /// <summary>
        /// Кодировка сообщения.
        /// </summary>
        public int Rus { get; set; } = 5;

        /// <summary>
        /// Флаг flash SMS.
        /// </summary>
        public bool? Flash { get; set; }

        /// <summary>
        /// Дата и время отправки СМС.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Максимальное количество частей сообщения.
        /// </summary>
        public int? MaxParts { get; set; }

        /// <summary>
        /// Интервал между сообщениями.
        /// </summary>
        public decimal? Gap { get; set; }

        /// <summary>
        /// Имя группы для рассылки.
        /// </summary>
        public string? Group { get; set; }

        /// <summary>
        /// Время жизни СМС.
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Индивидуальные сообщения.
        /// </summary>
        public bool? IndividualMessages { get; set; }

        /// <summary>
        /// Разделитель для индивидуальных сообщений.
        /// </summary>
        public string? Delimiter { get; set; }

        /// <summary>
        /// Параметр для получения идентификаторов SMS.
        /// </summary>
        public bool? WantSmsIds { get; set; }

        /// <summary>
        /// Использование push ID.
        /// </summary>
        public bool? WithPushId { get; set; }

        /// <summary>
        /// Игнорирование формата номера телефона.
        /// </summary>
        public bool? IgnorePhoneFormat { get; set; }

        /// <summary>
        /// Способ UDH-склейки.
        /// </summary>
        public bool? TwoByteConcat { get; set; }
    }
}

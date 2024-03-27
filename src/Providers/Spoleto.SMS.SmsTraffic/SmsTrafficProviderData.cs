namespace Spoleto.SMS.Providers.SmsTraffic
{
    /// <summary>
    /// Константы для установки дополнительных опций СМС сообщения в <see cref="SmsMessage.ProviderData"/>.
    /// </summary>
    public static class SmsTrafficProviderData
    {
        /// <summary>
        /// Кодировка сообщения.
        /// </summary>
        public const string Rus = nameof(SmsTrafficRequest.Rus);

        /// <summary>
        /// Флаг flash SMS.
        /// </summary>
        public const string Flash = nameof(SmsTrafficRequest.Flash);

        /// <summary>
        /// Дата и время отправки СМС.
        /// </summary>
        public const string StartDate = nameof(SmsTrafficRequest.StartDate);

        /// <summary>
        /// Максимальное количество частей сообщения.
        /// </summary>
        public const string MaxParts = nameof(SmsTrafficRequest.MaxParts);

        /// <summary>
        /// Интервал между сообщениями.
        /// </summary>
        public const string Gap = nameof(SmsTrafficRequest.Gap);

        /// <summary>
        /// Имя группы для рассылки.
        /// </summary>
        /// <remarks>
        /// Передать здесь либо в <see cref="SmsMessage.To"/>.
        /// </remarks>
        public const string Group = nameof(SmsTrafficRequest.Group);

        /// <summary>
        /// Время жизни СМС.
        /// </summary>
        public const string Timeout = nameof(SmsTrafficRequest.Timeout);

        /// <summary>
        /// Индивидуальные сообщения.
        /// </summary>
        public const string IndividualMessages = nameof(SmsTrafficRequest.IndividualMessages);

        /// <summary>
        /// Разделитель для индивидуальных сообщений.
        /// </summary>
        public const string Delimiter = nameof(SmsTrafficRequest.Delimiter);

        /// <summary>
        /// Параметр для получения идентификаторов SMS.
        /// </summary>
        public const string WantSmsIds = nameof(SmsTrafficRequest.WantSmsIds);

        /// <summary>
        /// Использование push ID.
        /// </summary>
        public const string WithPushId = nameof(SmsTrafficRequest.WithPushId);

        /// <summary>
        /// Игнорирование формата номера телефона.
        /// </summary>
        public const string IgnorePhoneFormat = nameof(SmsTrafficRequest.IgnorePhoneFormat);

        /// <summary>
        /// Способ UDH-склейки.
        /// </summary>
        public const string TwoByteConcat = nameof(SmsTrafficRequest.TwoByteConcat);
    }
}

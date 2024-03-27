using System.Text.Json.Serialization;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    /// <summary>
    /// The string representation of the properties of the original <see cref="SmsTrafficMessage"/>.
    /// </summary>
    internal class SmsTrafficRequest
    {
        /// <summary>
        /// Ваш логин в системе SMS Traffic. Параметр обязателен.
        /// </summary>
        [JsonPropertyName("login")]
        public string Login { get; set; }

        /// <summary>
        /// Пароль от вашего аккаунта. Параметр обязателен.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Список номеров через запятую.
        /// </summary>
        [JsonPropertyName("phones")]
        public string? Phones { get; set; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Кодировка сообщения.
        /// </summary>
        [JsonPropertyName("rus")]
        public string Rus { get; set; }

        /// <summary>
        /// Отправитель сообщения.
        /// </summary>
        [JsonPropertyName("originator")]
        public string Originator { get; set; }

        /// <summary>
        /// Флаг flash SMS.
        /// </summary>
        [JsonPropertyName("flash")]
        public string? Flash { get; set; }

        /// <summary>
        /// Дата и время отправки СМС.
        /// </summary>
        [JsonPropertyName("start_date")]
        public string? StartDate { get; set; }

        /// <summary>
        /// Максимальное количество частей сообщения.
        /// </summary>
        [JsonPropertyName("max_parts")]
        public string? MaxParts { get; set; }

        /// <summary>
        /// Интервал между сообщениями.
        /// </summary>
        [JsonPropertyName("gap")]
        public string? Gap { get; set; }

        /// <summary>
        /// Имя группы для рассылки.
        /// </summary>
        [JsonPropertyName("group")]
        public string? Group { get; set; }

        /// <summary>
        /// Время жизни СМС.
        /// </summary>
        [JsonPropertyName("timeout")]
        public string? Timeout { get; set; }

        /// <summary>
        /// Индивидуальные сообщения.
        /// </summary>
        [JsonPropertyName("individual_messages")]
        public string? IndividualMessages { get; set; }

        /// <summary>
        /// Разделитель для индивидуальных сообщений.
        /// </summary>
        [JsonPropertyName("delimiter")]
        public string? Delimiter { get; set; }

        /// <summary>
        /// Параметр для получения идентификаторов SMS.
        /// </summary>
        [JsonPropertyName("want_sms_ids")]
        public string? WantSmsIds { get; set; }

        /// <summary>
        /// Использование push ID.
        /// </summary>
        [JsonPropertyName("with_push_id")]
        public string? WithPushId { get; set; }

        /// <summary>
        /// Игнорирование формата номера телефона.
        /// </summary>
        [JsonPropertyName("ignore_phone_format")]
        public string? IgnorePhoneFormat { get; set; }

        /// <summary>
        /// Способ UDH-склейки.
        /// </summary>
        [JsonPropertyName("two_byte_concat")]
        public string? TwoByteConcat { get; set; }
    }
}

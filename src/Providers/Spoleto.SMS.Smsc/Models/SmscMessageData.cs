using System.Text.Json.Serialization;

namespace Spoleto.SMS.Providers.Smsc
{
    public record SmscMessageData
    {
        /// <summary>
        /// Идентификатор сообщения. Назначается Клиентом. Служит для дальнейшей идентификации сообщения. Если не указывать, то будет назначен автоматически. Не обязательно уникален. Идентификатор представляет собой 32-битное число в диапазоне от 1 до 2147483647, либо строку длиной до 40 символов, состоящую из латинских букв, цифр и символов ".-_".
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Признак того, что сообщение необходимо перевести в транслит.
        /// 0 - нет, 1 - translit, 2 - mpaHc/Ium.
        /// </summary>
        [JsonPropertyName("translit")]
        public int? Translit { get; set; }

        /// <summary>
        /// Автоматически сокращать ссылки в сообщениях.
        /// </summary>
        [JsonPropertyName("tinyurl")]
        public bool? TinyUrl { get; set; }

        /// <summary>
        /// Время отправки SMS-сообщения абоненту.
        /// </summary>
        [JsonPropertyName("time")]
        public string? Time { get; set; }

        /// <summary>
        /// Часовой пояс, в котором задается параметр time. Указывается относительно московского времени. Параметр tz может быть как положительным, так и отрицательным. Если tz равен 0, то будет использован московский часовой пояс, если же параметр tz не задан, то часовой пояс будет взят из настроек Клиента.
        /// </summary>
        [JsonPropertyName("tz")]
        public int? TimeZone { get; set; }

        /// <summary>
        /// Промежуток времени, в течение которого необходимо отправить рассылку. Представляет собой число в диапазоне от 0.1 до 720 часов. Применяется совместно с параметром freq. Данный параметр позволяет растянуть рассылку во времени для постепенного получения SMS-сообщений абонентами.
        /// </summary>
        [JsonPropertyName("period")]
        public decimal? Period { get; set; }

        /// <summary>
        /// Интервал или частота, с которой нужно отправлять SMS-рассылку на очередную группу номеров. Количество номеров в группе рассчитывается автоматически на основе параметров period и freq. Задается в промежутке от 1 до 1440 минут. Без параметра period параметр freq игнорируется.
        /// </summary>
        [JsonPropertyName("freq")]
        public int? Frequency { get; set; }

        /// <summary>
        /// Признак Flash сообщения, отображаемого сразу на экране телефона.
        /// </summary>
        [JsonPropertyName("flash")]
        public bool? Flash { get; set; }

        /// <summary>
        /// Признак бинарного сообщения.
        /// </summary>
        [JsonPropertyName("bin")]
        public int? Binary { get; set; }

        /// <summary>
        /// Признак wap-push сообщения, с помощью которого можно отправить интернет-ссылку на телефон.
        /// </summary>
        [JsonPropertyName("push")]
        public bool? Push { get; set; }

        /// <summary>
        /// Признак HLR-запроса для получения информации о номере из базы оператора без отправки реального SMS.
        /// </summary>
        [JsonPropertyName("hlr")]
        public bool? Hlr { get; set; }

        /// <summary>
        /// Признак специального SMS, не отображаемого в телефоне, для проверки номеров на доступность в реальном времени по статусу доставки.
        /// </summary>
        [JsonPropertyName("ping")]
        public bool? Ping { get; set; }

        /// <summary>
        /// Признак MMS-сообщения, с помощью которого можно передавать текст (txt), изображения различных форматов (jpg, gif, png), музыку (wav, amr, mp3, mid) и видео (mp4, 3gp). Файлы передаются в теле http-запроса.
        /// </summary>
        [JsonPropertyName("mms")]
        public bool? Mms { get; set; }

        /// <summary>
        /// Признак e-mail сообщения. Файлы, прикрепляемые к сообщению, передаются методом POST в теле http-запроса.
        /// </summary>
        [JsonPropertyName("mail")]
        public bool? Mail { get; set; }

        /// <summary>
        /// Признак soc-сообщения, отправляемого пользователям социальных сетей "Одноклассники", "ВКонтакте" или пользователям "Mail.Ru Агент".
        /// </summary>
        [JsonPropertyName("soc")]
        public bool? Social { get; set; }

        /// <summary>
        /// Признак viber-сообщения, отправляемого пользователям мессенджера Viber.
        /// </summary>
        [JsonPropertyName("viber")]
        public bool? Viber { get; set; }

        /// <summary>
        /// При указании значения данного параметра равным 1 будет отправлено telegram-сообщение с кодом подтверждения, переданным в параметре mes.
        /// </summary>
        [JsonPropertyName("tg")]
        public bool? Telegram { get; set; }

        /// <summary>
        /// Имя бота, в который необходимо отправить сообщение. Для telegram имеет вид - "@botname_bot", для whatsapp - "wa:botnumber".
        /// </summary>
        [JsonPropertyName("bot")]
        public string? Bot { get; set; }

        /// <summary>
        /// Используется совместно с параметром bot для telegram. При указании данного параметра, система не будет отображать текст сообщения, отправленного пользователю и выводить предупреждение о необходимости подтверждения номера телефона, если с момента последнего подтверждения прошло больше smsreq дней. Диапазон значений от 10 до 999.
        /// </summary>
        [JsonPropertyName("smsreq")]
        public int? SmsRequestDays { get; set; }

        /// <summary>
        /// Полный http-адрес файла для загрузки и передачи в сообщении. Минимальный размер файла составляет 101 байт.
        /// </summary>
        [JsonPropertyName("fileurl")]
        public string? FileUrl { get; set; }

        /// <summary>
        /// Признак голосового сообщения. При формировании голосового сообщения можно передавать как текст, так и прикреплять файлы. Файлы, добавляемые к сообщению, должны передаваться методом POST в теле http-запроса.
        /// </summary>
        [JsonPropertyName("call")]
        public bool? Call { get; set; }

        /// <summary>
        /// Голос, используемый для озвучивания текста (только для голосовых сообщений).
        /// </summary>
        [JsonPropertyName("voice")]
        public string? Voice { get; set; }

        /// <summary>
        /// Тема MMS или e-mail сообщения. При отправке e-mail указание темы, текста и адреса отправителя обязательно. Для MMS обязательным является указание темы или текста. Если не указать тему MMS, то в ее качестве будет использовано имя отправителя, переданное в запросе или используемое по умолчанию.
        /// </summary>
        [JsonPropertyName("subj")]
        public string? Subject { get; set; }

        /// <summary>
        /// Кодировка переданного сообщения, если используется отличная от кодировки по умолчанию windows-1251. Варианты: utf-8 и koi8-r.
        /// </summary>
        [JsonPropertyName("charset")]
        public string? Charset { get; set; }

        ///// <summary>
        ///// Признак необходимости получения стоимости рассылки.
        ///// </summary>
        //[JsonPropertyName("cost")]
        //public int? Cost { get; set; }

        ///// <summary>
        ///// Формат ответа сервера об успешной отправке.
        ///// </summary>
        //[JsonPropertyName("fmt")]
        //public int? Format { get; set; }

        /// <summary>
        /// Список номеров телефонов и соответствующих им сообщений, разделенных двоеточием или точкой с запятой и представленный в виде:<br/>
        /// phones1:mes1<br/>
        /// phones2:mes2<br/>
        /// Строки разделяются через символ новой строки \n.
        /// </summary>
        [JsonPropertyName("list")]
        public string? List { get; set; }

        /// <summary>
        /// Срок "жизни" SMS-сообщения. Определяет время, в течение которого оператор будет пытаться доставить сообщение абоненту. Диапазон от 1 до 24 часов.
        /// </summary>
        [JsonPropertyName("valid")]
        public string? Validity { get; set; }

        /// <summary>
        /// Максимальное количество SMS, на которые может разбиться длинное сообщение. Слишком длинные сообщения будут обрезаться так, чтобы не переполнить количество SMS, требуемых для их передачи. Этим параметром вы можете ограничить максимальную стоимость сообщений, так как за каждое SMS снимается отдельная плата.
        /// </summary>
        [JsonPropertyName("maxsms")]
        public int? MaxSms { get; set; }

        /// <summary>
        /// Значение буквенно-цифрового кода, введенного с "captcha" при использовании антиспам проверки. Данный параметр должен использоваться совместно с параметром userip.
        /// </summary>
        [JsonPropertyName("imgcode")]
        public string? ImageCode { get; set; }

        /// <summary>
        /// Значение IP-адреса, для которого будет действовать лимит на максимальное количество сообщений с одного IP-адреса в сутки, установленный в настройках личного кабинета в пункте "Лимиты и ограничения".
        /// </summary>
        [JsonPropertyName("userip")]
        public string? UserIp { get; set; }

        /// <summary>
        /// Признак необходимости добавления в ответ сервера списка ошибочных номеров.
        /// </summary>
        [JsonPropertyName("err")]
        public bool? ErrorList { get; set; }

        /// <summary>
        /// Признак необходимости добавления в ответ сервера информации по каждому номеру.
        /// </summary>
        [JsonPropertyName("op")]
        public bool? OperatorInfo { get; set; }

        /// <summary>
        /// Осуществляет привязку Клиента в качестве реферала к определенному ID партнера для текущего запроса. При передаче данного параметра в виде "pp=ID партнера" Клиент с логином login временно становится рефералом партнера с ID партнера.
        /// </summary>
        [JsonPropertyName("pp")]
        public int? PartnerId { get; set; }
    }
}

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// Константы для установки дополнительных опций СМС сообщения в <see cref="SmsMessage.ProviderData"/>.
    /// </summary>
    public static class SmscProviderData
    {
        /// <summary>
        /// Идентификатор сообщения. Назначается Клиентом. Служит для дальнейшей идентификации сообщения. Если не указывать, то будет назначен автоматически. Не обязательно уникален. Идентификатор представляет собой 32-битное число в диапазоне от 1 до 2147483647, либо строку длиной до 40 символов, состоящую из латинских букв, цифр и символов ".-_".
        /// </summary>
        public const string Id = nameof(SmscMessageData.Id);

        /// <summary>
        /// Признак того, что сообщение необходимо перевести в транслит.
        /// 0 - нет, 1 - translit, 2 - mpaHc/Ium.
        /// </summary>
        public const string Translit = nameof(SmscMessageData.Translit);

        /// <summary>
        /// Автоматически сокращать ссылки в сообщениях.
        /// </summary>
        public const string TinyUrl = nameof(SmscMessageData.TinyUrl);

        /// <summary>
        /// Время отправки SMS-сообщения абоненту.
        /// </summary>
        public const string Time = nameof(SmscMessageData.Time);

        /// <summary>
        /// Часовой пояс, в котором задается параметр time. Указывается относительно московского времени. Параметр tz может быть как положительным, так и отрицательным. Если tz равен 0, то будет использован московский часовой пояс, если же параметр tz не задан, то часовой пояс будет взят из настроек Клиента.
        /// </summary>
        public const string TimeZone = nameof(SmscMessageData.TimeZone);

        /// <summary>
        /// Промежуток времени, в течение которого необходимо отправить рассылку. Представляет собой число в диапазоне от 0.1 до 720 часов. Применяется совместно с параметром freq. Данный параметр позволяет растянуть рассылку во времени для постепенного получения SMS-сообщений абонентами.
        /// </summary>
        public const string Period = nameof(SmscMessageData.Period);

        /// <summary>
        /// Интервал или частота, с которой нужно отправлять SMS-рассылку на очередную группу номеров. Количество номеров в группе рассчитывается автоматически на основе параметров period и freq. Задается в промежутке от 1 до 1440 минут. Без параметра period параметр freq игнорируется.
        /// </summary>
        public const string Frequency = nameof(SmscMessageData.Frequency);

        /// <summary>
        /// Признак Flash сообщения, отображаемого сразу на экране телефона.
        /// </summary>
        public const string Flash = nameof(SmscMessageData.Flash);

        /// <summary>
        /// Признак бинарного сообщения.
        /// </summary>
        public const string Binary = nameof(SmscMessageData.Binary);

        /// <summary>
        /// Признак wap-push сообщения, с помощью которого можно отправить интернет-ссылку на телефон.
        /// </summary>
        public const string Push = nameof(SmscMessageData.Push);

        /// <summary>
        /// Признак HLR-запроса для получения информации о номере из базы оператора без отправки реального SMS.
        /// </summary>
        public const string Hlr = nameof(SmscMessageData.Hlr);

        /// <summary>
        /// Признак специального SMS, не отображаемого в телефоне, для проверки номеров на доступность в реальном времени по статусу доставки.
        /// </summary>
        public const string Ping = nameof(SmscMessageData.Ping);

        /// <summary>
        /// Признак MMS-сообщения, с помощью которого можно передавать текст (txt), изображения различных форматов (jpg, gif, png), музыку (wav, amr, mp3, mid) и видео (mp4, 3gp). Файлы передаются в теле http-запроса.
        /// </summary>
        public const string Mms = nameof(SmscMessageData.Mms);

        /// <summary>
        /// Признак e-mail сообщения. Файлы, прикрепляемые к сообщению, передаются методом POST в теле http-запроса.
        /// </summary>
        public const string Mail = nameof(SmscMessageData.Mail);

        /// <summary>
        /// Признак soc-сообщения, отправляемого пользователям социальных сетей "Одноклассники", "ВКонтакте" или пользователям "Mail.Ru Агент".
        /// </summary>
        public const string Social = nameof(SmscMessageData.Social);

        /// <summary>
        /// Признак viber-сообщения, отправляемого пользователям мессенджера Viber.
        /// </summary>
        public const string Viber = nameof(SmscMessageData.Viber);

        /// <summary>
        /// При указании значения данного параметра равным 1 будет отправлено telegram-сообщение с кодом подтверждения, переданным в параметре mes.
        /// </summary>
        public const string Telegram = nameof(SmscMessageData.Telegram);

        /// <summary>
        /// Имя бота, в который необходимо отправить сообщение. Для telegram имеет вид - "@botname_bot", для whatsapp - "wa:botnumber".
        /// </summary>
        public const string Bot = nameof(SmscMessageData.Bot);

        /// <summary>
        /// Используется совместно с параметром bot для telegram. При указании данного параметра, система не будет отображать текст сообщения, отправленного пользователю и выводить предупреждение о необходимости подтверждения номера телефона, если с момента последнего подтверждения прошло больше smsreq дней. Диапазон значений от 10 до 999.
        /// </summary>
        public const string SmsRequestDays = nameof(SmscMessageData.SmsRequestDays);

        /// <summary>
        /// Полный http-адрес файла для загрузки и передачи в сообщении. Минимальный размер файла составляет 101 байт.
        /// </summary>
        public const string FileUrl = nameof(SmscMessageData.FileUrl);

        /// <summary>
        /// Признак голосового сообщения. При формировании голосового сообщения можно передавать как текст, так и прикреплять файлы. Файлы, добавляемые к сообщению, должны передаваться методом POST в теле http-запроса.
        /// </summary>
        public const string Call = nameof(SmscMessageData.Call);

        /// <summary>
        /// Голос, используемый для озвучивания текста (только для голосовых сообщений).
        /// </summary>
        public const string Voice = nameof(SmscMessageData.Voice);

        /// <summary>
        /// Тема MMS или e-mail сообщения. При отправке e-mail указание темы, текста и адреса отправителя обязательно. Для MMS обязательным является указание темы или текста. Если не указать тему MMS, то в ее качестве будет использовано имя отправителя, переданное в запросе или используемое по умолчанию.
        /// </summary>
        public const string Subject = nameof(SmscMessageData.Subject);

        /// <summary>
        /// Кодировка переданного сообщения, если используется отличная от кодировки по умолчанию windows-1251. Варианты: utf-8 и koi8-r.
        /// </summary>
        public const string Charset = nameof(SmscMessageData.Charset);

        /// <summary>
        /// Список номеров телефонов и соответствующих им сообщений, разделенных двоеточием или точкой с запятой и представленный в виде:
        /// phones1:mes1
        /// phones2:mes2
        /// Строки разделяются через символ новой строки \n.
        /// </summary>
        public const string List = nameof(SmscMessageData.List);

        /// <summary>
        /// Срок "жизни" SMS-сообщения. Определяет время, в течение которого оператор будет пытаться доставить сообщение абоненту. Диапазон от 1 до 24 часов.
        /// </summary>
        public const string Validity = nameof(SmscMessageData.Validity);

        /// <summary>
        /// Максимальное количество SMS, на которые может разбиться длинное сообщение. Слишком длинные сообщения будут обрезаться так, чтобы не переполнить количество SMS, требуемых для их передачи. Этим параметром вы можете ограничить максимальную стоимость сообщений, так как за каждое SMS снимается отдельная плата.
        /// </summary>
        public const string MaxSms = nameof(SmscMessageData.MaxSms);

        /// <summary>
        /// Значение буквенно-цифрового кода, введенного с "captcha" при использовании антиспам проверки. Данный параметр должен использоваться совместно с параметром userip.
        /// </summary>
        public const string ImageCode = nameof(SmscMessageData.ImageCode);

        /// <summary>
        /// Значение IP-адреса, для которого будет действовать лимит на максимальное количество сообщений с одного IP-адреса в сутки, установленный в настройках личного кабинета в пункте "Лимиты и ограничения".
        /// </summary>
        public const string UserIp = nameof(SmscMessageData.UserIp);

        /// <summary>
        /// Признак необходимости добавления в ответ сервера списка ошибочных номеров.
        /// </summary>
        public const string ErrorList = nameof(SmscMessageData.ErrorList);

        /// <summary>
        /// Признак необходимости добавления в ответ сервера информации по каждому номеру.
        /// </summary>
        public const string OperatorInfo = nameof(SmscMessageData.OperatorInfo);

        /// <summary>
        /// Осуществляет привязку Клиента в качестве реферала к определенному ID партнера для текущего запроса. При передаче данного параметра в виде "pp=ID партнера" Клиент с логином login временно становится рефералом партнера с ID партнера.
        /// </summary>
        public const string PartnerId = nameof(SmscMessageData.PartnerId);
    }
}

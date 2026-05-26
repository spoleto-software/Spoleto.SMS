namespace Spoleto.SMS.Providers.Smsc
{
    public record SmscOptions
    {
        public const string DefaultCharset = "utf-8";
        public const int DefaultRetryCount = 5;

        #region Константы с параметрами отправки

        /// <summary>
        /// логин клиента
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// пароль или MD5-хеш пароля в нижнем регистре
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Специальный API-ключ, используемый для упрощенной авторизации вместо пары "логин+пароль".
        /// </summary>
        public string? ApiKey { get; set; }

        /// <summary>
        /// Использовать метод POST
        /// </summary>
        public bool UsePost { get; set; } = true;

        /// <summary>
        /// Использовать HTTPS протокол
        /// </summary>
        public bool UseHttps { get; set; }

        /// <summary>
        /// Кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8
        /// </summary>
        public string Charset { get; set; } = DefaultCharset;

        /// <summary>
        /// Флаг отладки
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        /// Number of retry attempts on failure.
        /// </summary>
        public int RetryCount { get; set; } = DefaultRetryCount;

        #endregion

        #region Константы для отправки SMS по SMTP

        /// <summary>
        /// e-mail адрес отправителя
        /// </summary>
        public string SmtpFrom { get; set; } = "api@smsc.ru";

        /// <summary>
        /// Адрес smtp сервера
        /// </summary>
        public string SmtpServer { get; set; } = "send.smsc.ru";

        /// <summary>
        /// Порт smtp сервера
        /// </summary>
        public int SmtpPort { get; set; } = 25;

        /// <summary>
        /// Логин для smtp сервера
        /// </summary>
        public string SmtpLogin { get; set; }

        /// <summary>
        /// Пароль для smtp сервера
        /// </summary>
        public string SmtpPassword { get; set; }
        #endregion

        /// <summary>
        /// Checks that all the settings within the options are configured properly.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <see cref="Login"/> or <see cref="Password"/> are null.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Login))
                throw new ArgumentNullException($"{nameof(Login)}");

            if (string.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException($"{nameof(Password)}");
        }
    }
}

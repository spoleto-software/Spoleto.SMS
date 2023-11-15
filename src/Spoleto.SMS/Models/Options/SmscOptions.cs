namespace Spoleto.SMS
{
    public record SmscOptions
    {
        #region Константы с параметрами отправки

        /// <summary>
        /// логин клиента
        /// </summary>
        public string SMSC_LOGIN { get; init; }

        /// <summary>
        /// пароль или MD5-хеш пароля в нижнем регистре
        /// </summary>
        public string SMSC_PASSWORD { get; init; }

        /// <summary>
        /// использовать метод POST
        /// </summary>
        public bool SMSC_POST { get;  init; }

        /// <summary>
        /// использовать HTTPS протокол
        /// </summary>
        public bool SMSC_HTTPS { get; init; }

        /// <summary>
        /// кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8
        /// </summary>
        public string SMSC_CHARSET = "utf-8";

        /// <summary>
        /// флаг отладки
        /// </summary>
        public bool SMSC_DEBUG { get; init; }

        #endregion

        #region Константы для отправки SMS по SMTP

        /// <summary>
        /// e-mail адрес отправителя
        /// </summary>
        public string SMTP_FROM { get; init; } = "api@smsc.ru";

        /// <summary>
        /// адрес smtp сервера
        /// </summary>
        public string SMTP_SERVER { get; init; } = "send.smsc.ru";

        /// <summary>
        /// логин для smtp сервера
        /// </summary>
        public string SMTP_LOGIN { get; init; }

        /// <summary>
        /// пароль для smtp сервера
        /// </summary>
        public string SMTP_PASSWORD { get; init; }

        #endregion
    }
}

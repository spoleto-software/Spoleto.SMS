namespace Spoleto.SMS.Providers.Smsc
{
    public record SmscOptions
    {
        #region Константы с параметрами отправки

        /// <summary>
        /// логин клиента
        /// </summary>
        public string SMSC_LOGIN { get; set; }

        /// <summary>
        /// пароль или MD5-хеш пароля в нижнем регистре
        /// </summary>
        public string SMSC_PASSWORD { get; set; }

        /// <summary>
        /// использовать метод POST
        /// </summary>
        public bool SMSC_POST { get; set; } = true;

        /// <summary>
        /// использовать HTTPS протокол
        /// </summary>
        public bool SMSC_HTTPS { get; set; }

        /// <summary>
        /// кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8
        /// </summary>
        public string SMSC_CHARSET = "utf-8";

        /// <summary>
        /// флаг отладки
        /// </summary>
        public bool SMSC_DEBUG { get; set; }

        #endregion

        #region Константы для отправки SMS по SMTP

        /// <summary>
        /// e-mail адрес отправителя
        /// </summary>
        public string SMTP_FROM { get; set; } = "api@smsc.ru";

        /// <summary>
        /// адрес smtp сервера
        /// </summary>
        public string SMTP_SERVER { get; set; } = "send.smsc.ru";

        /// <summary>
        /// логин для smtp сервера
        /// </summary>
        public string SMTP_LOGIN { get; set; }

        /// <summary>
        /// пароль для smtp сервера
        /// </summary>
        public string SMTP_PASSWORD { get; set; }

        #endregion

        /// <summary>
        /// validate if the options are all set correctly
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SMSC_LOGIN))
                throw new ArgumentNullException($"{nameof(SMSC_LOGIN)}");

            if (string.IsNullOrWhiteSpace(SMSC_PASSWORD))
                throw new ArgumentNullException($"{nameof(SMSC_PASSWORD)}");
        }
    }
}

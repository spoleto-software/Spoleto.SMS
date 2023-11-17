using Spoleto.SMS.Exceptions;
using Spoleto.SMS.Extensions;

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// The SMSC provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>.
    /// </remarks>
    public partial class SmscProvider : SmsProviderBase, ISmscProvider
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.SMSC);

        private readonly SmscOptions _options;

        public SmscProvider(SmscOptions options)
        {
            // Validates if the options are valid
            options.Validate();
            _options = options;

            SMSC_LOGIN = _options.SMSC_LOGIN;
            SMSC_PASSWORD = _options.SMSC_PASSWORD;
            SMSC_POST = _options.SMSC_POST;
            SMSC_HTTPS = _options.SMSC_HTTPS;
            SMSC_CHARSET = _options.SMSC_CHARSET;
            SMSC_DEBUG = _options.SMSC_DEBUG;
            SMTP_FROM = _options.SMTP_FROM;
            SMTP_SERVER = _options.SMTP_SERVER;
            SMTP_LOGIN = _options.SMTP_LOGIN;
            SMTP_PASSWORD = _options.SMTP_PASSWORD;
        }

        //public SmscProvider(IOptions<SmscOptions> options)
        //{
        //    _options = options?.Value ?? throw new ArgumentNullException(nameof(SmscOptions));

        //    SMSC_LOGIN = _options.SMSC_LOGIN;
        //    SMSC_PASSWORD = _options.SMSC_PASSWORD;
        //    SMSC_POST = _options.SMSC_POST;
        //    SMSC_HTTPS = _options.SMSC_HTTPS;
        //    SMSC_CHARSET = _options.SMSC_CHARSET;
        //    SMSC_DEBUG = _options.SMSC_DEBUG;
        //    SMTP_FROM = _options.SMTP_FROM;
        //    SMTP_SERVER = _options.SMTP_SERVER;
        //    SMTP_LOGIN = _options.SMTP_LOGIN;
        //    SMTP_PASSWORD = _options.SMTP_PASSWORD;
        //}

        /// <inheritdoc/>
        public override string Name => ProviderName;

        /// <inheritdoc/>
        public override bool IsAllowNullFrom => false;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = new List<string> { "7", "8" };

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
        {
            // Validate:
            message.To.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ForEach(number => ValidateDataForSMS(number, message.Body, message.IsAllowSendToForeignNumbers));

            var result = send_sms(message.To, message.Body, sender: message.From);

            return GetSmsSendingResult(result);
        }

        /// <inheritdoc/>
        public override async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            // Validate:
            message.To.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ForEach(number => ValidateDataForSMS(number, message.Body, message.IsAllowSendToForeignNumbers));

            var result = await send_smsAsync(message.To, message.Body, sender: message.From).ConfigureAwait(false);

            return GetSmsSendingResult(result);
        }

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var result = get_status(id, phoneNumber);

            return GetSmsStatusResult(result);
        }

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var result = await get_statusAsync(id, phoneNumber).ConfigureAwait(false);

            return GetSmsStatusResult(result);
        }

        /// <inheritdoc/>
        public void CheckPhoneNumber(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(sender))
                throw new ArgumentNullException(nameof(sender));

            phoneNumber.Split(Separator).ForEach(number => ValidatePhoneNumber(number, isAllowSendToForeignNumbers));

            var result = send_sms(phoneNumber, string.Empty, sender: sender, query: "hlr=1");

            var smsResult = GetSmsSendingResult(result);
            if (!smsResult.Success)
            {
                throw new SmsSendingException(smsResult.Errors.First().Message);
            }
        }

        /// <inheritdoc/>
        public string GetBalance()
        {
            return get_balance();
        }

        protected override void ValidatePhoneNumber(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            phoneNumber = phoneNumber.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Replace("+", "");
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 11 || phoneNumber.Length > 15)
            {
                throw new ArgumentException($"The phone number {phoneNumber} is not in the correct format.");
            }

            base.ValidatePhoneNumber(phoneNumber, isAllowSendToForeignNumbers);
        }

        private SmsSendingResult GetSmsSendingResult(string[] result)
        {
            if (Convert.ToInt32(result[1]) > 0)
            {
                return new SmsSendingResult
                {
                    ProviderName = Name,
                    Success = true
                };
            }

            return new SmsSendingResult
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError>
                {
                    new SmsSendingError
                    {
                        Code = result[1],
                        Message= GetErrorMessage(result[1])
                    }
                }
            };
        }

        private SmsStatusResult GetSmsStatusResult(string[] result)
        {
            if (Convert.ToInt32(result[0]) > 0)
            {
                return new SmsStatusResult
                {
                    ProviderName = Name,
                    Success = true
                };
            }

            return new SmsStatusResult
            {
                ProviderName = Name,
                Success = false
            };
        }

        private static string GetErrorMessage(string code)
            => code switch
            {
                "-1" => "Ошибка в параметрах.",
                "-2" => "Неверный логин или пароль.",
                "-3" => "Недостаточно средств на счете Клиента.",
                "-4" => "IP-адрес временно заблокирован из-за частых ошибок в запросах.",
                "-5" => "Неверный формат даты.",
                "-6" => "Сообщение запрещено (по тексту или по имени отправителя).",
                "-7" => "Неверный формат номера телефона.",
                "-8" => "Сообщение на указанный номер не может быть доставлено.",
                "-9" => "Отправка более одного одинакового запроса на передачу SMS-сообщения либо более пяти одинаковых запросов на получение стоимости сообщения в течение минуты.",
                _ => $"Неизвестная ошибка. Свяжитесь с ИТ отделом. Код ошибки : {code}.",
            };
    }
}

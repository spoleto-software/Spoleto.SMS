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
    public partial class SmscProvider : SmsProviderBase<SmsMessage>, ISmscProvider
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.SMSC);

        private readonly SmscOptions _options;
        private static readonly char _phoneNumberSeparator = new SmsMessage("body", "from", "to").PhoneNumberSeparator;

        /// <summary>
        /// Creates an instanse of <see cref="SmscProvider"/>.
        /// </summary>
        /// <param name="options">The provider options.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        public SmscProvider(SmscOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

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

        /// <inheritdoc/>
        public override string Name => ProviderName;

        /// <inheritdoc/>
        public override bool IsAllowNullFrom => false;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = ["7", "8"];

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var result = get_status(id, phoneNumber);

            return GetSmsStatusResult(result);
        }

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var result = await get_statusAsync(id, phoneNumber).ConfigureAwait(false);

            return GetSmsStatusResult(result);
        }

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var smscMessage = CreateMessage(message);

            
#if NET5_0_OR_GREATER
            var phoneNumbers = smscMessage.To.Split(smscMessage.PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

#else
            var phoneNumbers = smscMessage.To.Split(smscMessage.PhoneNumberSeparator);
#endif
            // Validate:
            ValidateDataForSMS(phoneNumbers, smscMessage);

            var result = send_sms(smscMessage.To, smscMessage.Body, sender: smscMessage.From);

            return GetSmsSendingResult(result);
        }

        /// <inheritdoc/>
        public override async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var smscMessage = CreateMessage(message);

#if NET5_0_OR_GREATER
            var phoneNumbers = smscMessage.To.Split(smscMessage.PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
            var phoneNumbers= smscMessage.To.Split(smscMessage.PhoneNumberSeparator);
#endif
            // Validate:
            ValidateDataForSMS(phoneNumbers, smscMessage);

            var result = await send_smsAsync(smscMessage.To, smscMessage.Body, sender: smscMessage.From).ConfigureAwait(false);

            return GetSmsSendingResult(result);
        }

        /// <inheritdoc/>
        public void CheckPhoneNumber(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(sender))
                throw new ArgumentNullException(nameof(sender));

            phoneNumber.Split(_phoneNumberSeparator).ForEach(number => ValidatePhoneNumber(number, isAllowSendToForeignNumbers));

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

        protected override sealed void ValidatePhoneNumber(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            phoneNumber = CleanPhoneNumber(phoneNumber);
            if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length < 11 || phoneNumber.Length > 15)
            {
                throw new ArgumentException($"The phone number {phoneNumber} is not in the correct format.");
            }

            base.ValidatePhoneNumber(phoneNumber, isAllowSendToForeignNumbers);
        }

        private SmsSendingResult GetSmsSendingResult(string[] result)
        {
            if (result.Length >= 2
                && Int32.TryParse(result[1], out var res)
                && res > 0)
            {
                return new()
                {
                    ProviderName = Name,
                    Success = true
                };
            }

            var errorCode = result.Length >= 2 ? result[1] : string.Join(",", result);

            return new()
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError>
                {
                    new ()
                    {
                        Code = errorCode,
                        Message= GetSendErrorMessage(errorCode)
                    }
                }
            };
        }

        private SmsStatusResult GetSmsStatusResult(string[] result)
        {
            if (result.Length >= 2
                && Int32.TryParse(result[1], out var res)
                && res < 0)
            {
                return new()
                {
                    ProviderName = Name,
                    Success = false,
                    Errors = new List<SmsSendingError>
                    {
                        new()
                        {
                            Code = result[1],
                            Message= GetStatusErrorMessage(result[1])
                        }
                    }
                };
            }

            if (result.Length >= 1
                && Int32.TryParse(result[0], out var _))
            {
                return new()
                {
                    ProviderName = Name,
                    Success = true,
                    SmsStatusData = new List<SmsStatusData>
                    {
                        GetStatusData(result[0])
                    }
                };
            }

            return new()
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError>
                {
                    new ()
                    {
                        Message=  string.Join(",", result)
                    }
                }
            };
        }

        private static string GetSendErrorMessage(string code)
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

        private static string GetStatusErrorMessage(string code)
            => code switch
            {
                "-1" => "Ошибка в параметрах.",
                "-2" => "Неверный логин или пароль. Также возникает при попытке отправки сообщения с IP - адреса, не входящего в список разрешенных Клиентом (если такой список был настроен Клиентом ранее).",
                "-4" => "IP - адрес временно заблокирован.",
                "-5" => "Ошибка удаления сообщения.",
                "-9" => "Попытка отправки более пяти запросов на получение статуса одного и того же сообщения или более одного массового запроса в течение минуты. Данная ошибка возникает также при попытке отправки пяти и более запросов одновременно с разных подключений под одним логином(too many concurrent requests).",
                _ => $"Неизвестная ошибка. Свяжитесь с ИТ отделом. Код ошибки : {code}.",
            };

        private static SmsStatusData GetStatusData(string code)
            => code switch
            {
                "-3" => new()
                {
                    Status = "-3",
                    Text = "Сообщение не найдено.",
                    Description = "Возникает, если для указанного номера телефона и ID сообщение не найдено."
                },
                "-2" => new()
                {
                    Status = "-2",
                    Text = "Остановлено",
                    Description = "Возникает у сообщений из рассылки, которые не успели уйти оператору до момента временной остановки данной рассылки на странице Рассылки и задания."
                },
                "-1" => new()
                {
                    Status = "-1",
                    Text = "Ожидает отправки",
                    Description = "Если при отправке сообщения было задано время получения абонентом, то до этого времени сообщение будет находиться в данном статусе, в других случаях сообщение в этом статусе находится непродолжительное время перед отправкой на SMS-центр."
                },
                "0" => new()
                {
                    Status = "0",
                    Text = "Передано оператору",
                    Description = "Сообщение было передано на SMS - центр оператора для доставки."
                },
                "1" => new()
                {
                    Status = "1",
                    Text = "Доставлено",
                    Description = "Сообщение было успешно доставлено абоненту."
                },
                "2" => new()
                {
                    Status = "2",
                    Text = "Прочитано",
                    Description = "Сообщение было прочитано (открыто) абонентом. Данный статус возможен для e-mail - сообщений, имеющих формат html - документа."
                },
                "3" => new()
                {
                    Status = "3",
                    Text = "Просрочено",
                    Description = "Возникает, если время \"жизни\" сообщения истекло, а оно так и не было доставлено получателю, например, если абонент не был доступен в течение определенного времени или в его телефоне был переполнен буфер сообщений."
                },
                "4" => new()
                {
                    Status = "4",
                    Text = "Нажата ссылка",
                    Description = "Сообщение было доставлено, и абонентом была нажата короткая ссылка, переданная в сообщении. Данный статус возможен при включенных в настройках опциях \"Автоматически сокращать ссылки в сообщениях\" и \"отслеживать номера абонентов\"."
                },
                "20" => new()
                {
                    Status = "20",
                    Text = "Невозможно доставить",
                    Description = "Попытка доставить сообщение закончилась неудачно, это может быть вызвано разными причинами, например, абонент заблокирован, не существует, находится в роуминге без поддержки обмена SMS, или на его телефоне не поддерживается прием SMS-сообщений."
                },
                "22" => new()
                {
                    Status = "22",
                    Text = "Неверный номер",
                    Description = "Неправильный формат номера телефона."
                },
                "23" => new()
                {
                    Status = "23",
                    Text = "Запрещено",
                    Description = "Возникает при срабатывании ограничений на отправку дублей, на частые сообщения на один номер (флуд), на номера из черного списка, на запрещенные спам фильтром тексты или имена отправителей (Sender ID)."
                },
                "24" => new()
                {
                    Status = "24",
                    Text = "Недостаточно средств",
                    Description = "На счете Клиента недостаточная сумма для отправки сообщения."
                },
                "25" => new()
                {
                    Status = "25",
                    Text = "Недоступный номер.",
                    Description = "Телефонный номер не принимает SMS-сообщения, или на этого оператора нет рабочего маршрута."
                },
                _ => new()
                {
                    Status = code,
                    Text = $"Неизвестный статус. Свяжитесь с ИТ отделом. Код ошибки : {code}."
                },
            };
    }
}

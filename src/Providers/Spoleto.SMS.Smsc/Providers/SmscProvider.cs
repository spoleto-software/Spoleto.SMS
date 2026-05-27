using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
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
    public partial class SmscProvider : SmsProviderBase<SmscMessage>, IDisposable
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.SMSC);

        private readonly SmscOptions _options;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SmscProvider> _logger;
        private readonly bool _ownsHttpClient;

        private static readonly char _phoneNumberSeparator = new SmsMessage("body", "from", "to").PhoneNumberSeparator;

        /// <summary>
        /// Initializes a new instance using a shared <see cref="HttpClient"/> (recommended for DI scenarios).
        /// </summary>
        public SmscProvider(SmscOptions options, HttpClient httpClient, ILogger<SmscProvider>? logger = null)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));


            // Validates if the options are valid
            options.Validate();
            _options = options;

            _httpClient = httpClient;
            _logger = logger ?? NullLogger<SmscProvider>.Instance;
            _ownsHttpClient = false;
        }

        /// <summary>
        /// Initializes a new instance that owns its own <see cref="HttpClient"/>.
        /// </summary>
        public SmscProvider(SmscOptions options, ILogger<SmscProvider>? logger = null)
            : this(options, new HttpClient(), logger)
        {
            _ownsHttpClient = true;
        }

        /// <inheritdoc/>
        public override string Name => ProviderName;

        /// <inheritdoc/>
        public override bool IsAllowNullFrom => false;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = ["7", "8"];

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
            => GetStatusAsync(id, phoneNumber, CancellationToken.None).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (phoneNumber == null)
                throw new ArgumentNullException(nameof(phoneNumber));

            var result = await GetStatusAsync(id, phoneNumber, 0, false, cancellationToken).ConfigureAwait(false);

            return GetSmsStatusResult(result);
        }

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var smscMessage = CreateMessage(message);

            var result = await SendAsync(smscMessage, cancellationToken: cancellationToken).ConfigureAwait(false);

            return GetSmsSendingResult(result, smscMessage.GetRecipients());
        }

        protected override SmscMessage CreateMessage(SmsMessage originalMessage)
        {
            if (originalMessage is SmscMessage smscMessage)
            {
                return smscMessage;
            }

            SmscMessageData? messageData = default;
            var providerData = originalMessage.ProviderData;
            if (providerData != null
                && providerData.Count > 0)
            {
                messageData = new SmscMessageData();
                foreach (var data in providerData)
                {
                    var property = GetSmscDataProperty(data.Name);
                    if (property == null)
                        continue;

                    property.SetValue(messageData, data.Value);
                }
            }

            var message = new SmscMessage(originalMessage.Body, originalMessage.From, originalMessage.To, originalMessage.IsAllowSendToForeignNumbers, messageData);

            return message;
        }


        private static readonly ConcurrentDictionary<string, PropertyInfo?> _propertyCache = [];

        private static PropertyInfo? GetSmscDataProperty(string name)
        {
            if (!_propertyCache.TryGetValue(name, out var propertyInfo))
            {
                propertyInfo = _propertyCache[name] = typeof(SmscMessageData).GetProperty(name);
            }

            return propertyInfo;
        }

        protected override void ValidateSmsMessage(SmsMessage smsMessage)
        {
            if (smsMessage is SmscMessage message)
            {

                if (message.SmscProviderData?.List != null
                    && (message.Body != null || message.To != null))
                {
                    throw new Exception($"The SMS body or To is not empty but <{nameof(SmscMessageData.List)}> is set.");
                }
                else if (message.Body == null
                    && message.To == null
                    && message.SmscProviderData?.List == null)
                {
                    throw new ArgumentNullException(nameof(message.Body));
                }
            }
            else
            {
                base.ValidateSmsMessage(smsMessage);
            }
        }

        /// <inheritdoc/>
        public void CheckPhoneNumber(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false)
            => CheckPhoneNumberAsync(phoneNumber, sender, isAllowSendToForeignNumbers).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task CheckPhoneNumberAsync(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (string.IsNullOrWhiteSpace(sender))
                throw new ArgumentNullException(nameof(sender));

            phoneNumber.Split(_phoneNumberSeparator).ForEach(number => ValidatePhoneNumber(number, isAllowSendToForeignNumbers));

            var data = new SmscMessageData { Hlr = true };
            var smscMessage = new SmscMessage(string.Empty, sender, phoneNumber, isAllowSendToForeignNumbers, data);

            var result = await SendAsync(smscMessage).ConfigureAwait(false);

            var smsResult = GetSmsSendingResult(result, phoneNumber);
            if (!smsResult.Success)
            {
                throw new SmsSendingException(smsResult.Errors.First().Message);
            }
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

        private SmsSendingResult GetSmsSendingResult(SmscResult<SmscSendSuccessResult> result, params IEnumerable<string> recipients)
        {
            if (result.IsSuccess)
            {
                return new()
                {
                    ProviderName = Name,
                    Success = true,
                    SmsSendingData = recipients.Select(x =>
                        new SmdSendingData()
                        {
                            MessageId = $"{x}:{result.Success!.Id}", // composite key (to make it unique)
                            Recipient = x
                        })
                };
            }

            return new()
            {
                ProviderName = Name,
                Success = false,
                Errors =
                [
                    new ()
                    {
                        Code = result.Error!.ErrorCode.ToString(),
                        Message= GetSendErrorMessage(result.Error!.ErrorCode)
                    }
                ]
            };
        }

        private SmsStatusResult GetSmsStatusResult(SmscStatusResult result)
        {
            if (result.IsSuccess)
            {
                var date = result.Success!.LastChanged?.UtcDateTime;
                var data = GetStatusData(result.Success!.Status, date, null);
                var successful = GetStatusSuccessfulFlag(result.Success!.Status);

                if (!successful)
                {
                    return new()
                    {
                        ProviderName = Name,
                        Success = false,
                        Errors =
                        [
                            new()
                            {
                                Error = result.Success!.ErrorCode,
                                Code = result.Success!.Status.ToString(),
                                Message =$"{data.Text} ({data.Description})"
                            }
                        ]
                    };
                }
                
                return new()
                {
                    ProviderName = Name,
                    Success = true,
                    SmsStatusData =
                    [
                        data
                    ]
                };
            }

            if (result.IsBatchSuccess)
            {
                static (SmscMessageStatus Status, bool Successful, DateTime? StatusDate, string? PhoneNumber) GetStatusInfo(IReadOnlyList<string> raw)
                {
                    var statusCode = (SmscMessageStatus)(int.TryParse(raw[0], out int s) ? s : 0);
                    var timestamp = raw.Count > 1 && int.TryParse(raw[1], out int t) ? t : 0;
                    var successful = GetStatusSuccessfulFlag(statusCode);
                    var phoneNumber = raw.Count > 4 ? raw[4] : null;

                    DateTimeOffset? dt = timestamp > 0
                        ? DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime()
                        : null;

                    return (statusCode, successful, dt?.UtcDateTime, phoneNumber);
                }

                var statusList = result.BatchSuccess!.Rows.Select(GetStatusInfo).ToList();

                return new()
                {
                    ProviderName = Name,
                    Success = statusList.Any(x => x.Successful),
                    SmsStatusData = statusList.Select(x => GetStatusData(x.Status, x.StatusDate, x.PhoneNumber)).ToList()
                };
            }


            return new()
            {
                ProviderName = Name,
                Success = false,
                Errors =
                    [
                        new()
                        {
                            Code = result.Error!.ErrorCode.ToString(),
                            Message = GetStatusErrorMessage(result.Error!.ErrorCode)
                        }
                    ]
            };
        }

        private static string GetSendErrorMessage(int code)
            => code switch
            {
                1 => "Ошибка в параметрах.",
                2 => "Неверный логин или пароль.",
                3 => "Недостаточно средств на счете Клиента.",
                4 => "IP-адрес временно заблокирован из-за частых ошибок в запросах.",
                5 => "Неверный формат даты.",
                6 => "Сообщение запрещено (по тексту или по имени отправителя).",
                7 => "Неверный формат номера телефона.",
                8 => "Сообщение на указанный номер не может быть доставлено.",
                9 => "Отправка более одного одинакового запроса на передачу SMS-сообщения либо более пяти одинаковых запросов на получение стоимости сообщения в течение минуты.",
                _ => $"Неизвестная ошибка. Свяжитесь с ИТ отделом. Код ошибки : {code}.",
            };

        private static string GetStatusErrorMessage(int code)
            => code switch
            {
                1 => "Ошибка в параметрах.",
                2 => "Неверный логин или пароль. Также возникает при попытке отправки сообщения с IP - адреса, не входящего в список разрешенных Клиентом (если такой список был настроен Клиентом ранее).",
                4 => "IP - адрес временно заблокирован.",
                5 => "Ошибка удаления сообщения.",
                9 => "Попытка отправки более пяти запросов на получение статуса одного и того же сообщения или более одного массового запроса в течение минуты. Данная ошибка возникает также при попытке отправки пяти и более запросов одновременно с разных подключений под одним логином(too many concurrent requests).",
                _ => $"Неизвестная ошибка. Свяжитесь с ИТ отделом. Код ошибки : {code}.",
            };

        private static SmsStatusData GetStatusData(SmscMessageStatus status, DateTime? date, string phoneNumber)
            => status switch
            {
                SmscMessageStatus.MessageNotFound => new()
                {
                    Status = "-3",
                    Text = "Сообщение не найдено.",
                    Description = "Возникает, если для указанного номера телефона и ID сообщение не найдено.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.Stopped => new()
                {
                    Status = "-2",
                    Text = "Остановлено",
                    Description = "Возникает у сообщений из рассылки, которые не успели уйти оператору до момента временной остановки данной рассылки на странице Рассылки и задания.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.WaitingForSending => new()
                {
                    Status = "-1",
                    Text = "Ожидает отправки",
                    Description = "Если при отправке сообщения было задано время получения абонентом, то до этого времени сообщение будет находиться в данном статусе, в других случаях сообщение в этом статусе находится непродолжительное время перед отправкой на SMS-центр.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.SentToOperator => new()
                {
                    Status = "0",
                    Text = "Передано оператору",
                    Description = "Сообщение было передано на SMS - центр оператора для доставки.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.Delivered => new()
                {
                    Status = "1",
                    Text = "Доставлено",
                    Description = "Сообщение было успешно доставлено абоненту.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.Read => new()
                {
                    Status = "2",
                    Text = "Прочитано",
                    Description = "Сообщение было прочитано (открыто) абонентом. Данный статус возможен для e-mail - сообщений, имеющих формат html - документа.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.Expired => new()
                {
                    Status = "3",
                    Text = "Просрочено",
                    Description = "Возникает, если время \"жизни\" сообщения истекло, а оно так и не было доставлено получателю, например, если абонент не был доступен в течение определенного времени или в его телефоне был переполнен буфер сообщений.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.LinkClicked => new()
                {
                    Status = "4",
                    Text = "Нажата ссылка",
                    Description = "Сообщение было доставлено, и абонентом была нажата короткая ссылка, переданная в сообщении. Данный статус возможен при включенных в настройках опциях \"Автоматически сокращать ссылки в сообщениях\" и \"отслеживать номера абонентов\".",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.CannotBeDelivered => new()
                {
                    Status = "20",
                    Text = "Невозможно доставить",
                    Description = "Попытка доставить сообщение закончилась неудачно, это может быть вызвано разными причинами, например, абонент заблокирован, не существует, находится в роуминге без поддержки обмена SMS, или на его телефоне не поддерживается прием SMS-сообщений.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.InvalidPhoneNumber => new()
                {
                    Status = "22",
                    Text = "Неверный номер",
                    Description = "Неправильный формат номера телефона.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.Forbidden => new()
                {
                    Status = "23",
                    Text = "Запрещено",
                    Description = "Возникает при срабатывании ограничений на отправку дублей, на частые сообщения на один номер (флуд), на номера из черного списка, на запрещенные спам фильтром тексты или имена отправителей (Sender ID).",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.InsufficientFunds => new()
                {
                    Status = "24",
                    Text = "Недостаточно средств",
                    Description = "На счете Клиента недостаточная сумма для отправки сообщения.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                SmscMessageStatus.UnavailableNumber => new()
                {
                    Status = "25",
                    Text = "Недоступный номер.",
                    Description = "Телефонный номер не принимает SMS-сообщения, или на этого оператора нет рабочего маршрута.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                },
                _ => new()
                {
                    Status = status.ToString(),
                    Text = $"Неизвестный статус. Свяжитесь с ИТ отделом. Код ошибки : {(int)status}.",
                    DateSent = date ?? DateTime.MinValue,
                    DateReceived = date ?? DateTime.MinValue,
                    DateDelivered = date ?? DateTime.MinValue,
                    Recipient = phoneNumber
                }
            };

        private static bool GetStatusSuccessfulFlag(SmscMessageStatus status)
            => status switch
            {
                SmscMessageStatus.Delivered => true, // Доставлено
                SmscMessageStatus.Read => true, // Прочитано
                SmscMessageStatus.LinkClicked => true, // Нажата ссылка
                _ => false
            };

        private static string EscapeMessageBody(string body)
        {
            if (body.Contains('+'))
            {
                return body.Replace("+", "%2B");
            }

            return body;
        }
    }
}

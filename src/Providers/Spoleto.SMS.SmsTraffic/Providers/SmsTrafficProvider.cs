using System.Collections.Concurrent;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Spoleto.Common.Helpers;
using Spoleto.SMS.Extensions;

namespace Spoleto.SMS.Providers.SmsTraffic
{
    /// <summary>
    /// The SmsTraffic provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://www.smstraffic.ru/api"/>.
    /// </remarks>
    public class SmsTrafficProvider : SmsProviderBase<SmsTrafficMessage>, ISmsTrafficProvider
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.SmsTraffic);

        private readonly SmsTrafficOptions _options;
        private readonly HttpClient _httpClient;

        private const string Windows1251FileEncoding = "windows-1251";
        private readonly static Encoding _windows1251Encoding;

        private const string Utf8Code = "5";
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        private static readonly char _phoneNumberSeparator = new SmsTrafficMessage("body", "from", "to").PhoneNumberSeparator;

        static SmsTrafficProvider()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//support for "windows-1251"
            _windows1251Encoding = Encoding.GetEncoding(Windows1251FileEncoding);
        }

        public SmsTrafficProvider(SmsTrafficOptions options)
            : this(new HttpClient(), options)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="SmsTrafficProvider"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance.</param>
        /// <param name="options">The options instance.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="httpClient"/> or <paramref name="options"/> are null.</exception>
        public SmsTrafficProvider(HttpClient httpClient, SmsTrafficOptions options)
        {
            if (httpClient is null)
                throw new ArgumentNullException(nameof(httpClient));

            if (options is null)
                throw new ArgumentNullException(nameof(options));

            // Validates if the options are valid
            options.Validate();
            _options = options;

            // if the HTTP client is null
            _httpClient = ConfigureHttpClient(httpClient);
        }

        /// <inheritdoc/>
        public override string Name => ProviderName;

        /// <inheritdoc/>
        public override bool IsAllowNullFrom => false;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = ["7", "8"];

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
            => GetStatusAsync(id, phoneNumber).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "status" },
                { "sms_id", id }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "multi.php", cancellationToken).ConfigureAwait(false);

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            if (response.IsSuccessStatusCode)
            {
                var smsResponse = DeserializeResponse<SmsTrafficStatusResponse>(responseString);

                if (String.IsNullOrEmpty(smsResponse.Error))
                {
                    var dateSend = TryDateTimeParse(smsResponse.SendDate, out var dt1) ? dt1 : default;
                    var dateReceived = TryDateTimeParse(smsResponse.SubmissionDate, out var dt2) ? dt2 : default;
                    var dateDelivered = TryDateTimeParse(smsResponse.LastStatusChangeDate, out var dt3) ? dt3 : default;

                    return new SmsStatusResult
                    {
                        ProviderName = Name,
                        Success = true,
                        SmsStatusData = new List<SmsStatusData>
                        {
                            new()
                            {
                                Status = smsResponse.Status,
                                DateSent = dateSend,
                                DateReceived = dateReceived,
                                DateDelivered = dateDelivered,
                                MessageId = smsResponse.SmsId.ToString()
                            }
                        }
                    };
                }

                return new SmsStatusResult
                {
                    ProviderName = Name,
                    Success = false,
                    Errors = new List<SmsSendingError> { new() { Message = smsResponse.Error } }
                };
            }

            return new SmsStatusResult
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError> { new() { Message = responseString } }
            };
        }

        private async Task<HttpResponseMessage> SendRequest(HttpContent content, string relativeUri, CancellationToken cancellationToken)
        {
            try
            {
                var mainUri = new Uri(new Uri(_options.ServiceUrl), relativeUri);

                return await _httpClient.PostAsync(mainUri, content, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException) // handle exceptions if the main server is not responding
            {
                var duplicateUri = new Uri(new Uri(_options.DuplicateServiceUrl), relativeUri);

                return await _httpClient.PostAsync(duplicateUri, content, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException) // handle exceptions if the main server is not responding
            {
                var duplicateUri = new Uri(new Uri(_options.DuplicateServiceUrl), relativeUri);

                return await _httpClient.PostAsync(duplicateUri, content, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var smsTrafficMessage = CreateMessage(message);

            return SendAsync(smsTrafficMessage, cancellationToken);
        }

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsTrafficMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<SmsSendingResult> SendAsync(SmsTrafficMessage message, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

#if NET5_0_OR_GREATER
            var phoneNumbers = message.To.Split(message.PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
            var phoneNumbers = message.To.Split(message.PhoneNumberSeparator);
#endif

            // Validate:
            phoneNumbers.ForEach(number => ValidateDataForSMS(number, message.Body, message.IsAllowSendToForeignNumbers));

            var requestObject = ConvertToRequestObject(message);
            var json = JsonHelper.ToJson(requestObject);

            var requestData = JsonHelper.FromJson<Dictionary<string, string>>(json);

            HttpContent content;
            if (requestObject.Rus == Utf8Code)
            {
                content = new FormUrlEncodedContent(requestData);
            }
            else
            {
                var encodedItems = requestData.Select(x => $"{x.Key}={(x.Key == nameof(requestObject.Message) ? WebUtility.UrlEncode(x.Value) : x.Value)}");

                string encodedContent = string.Join("&", encodedItems);

                content = new StringContent(encodedContent, _windows1251Encoding, "application/x-www-form-urlencoded");
            }

            var response = await SendRequest(content, "multi.php", cancellationToken).ConfigureAwait(false);

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            if (response.IsSuccessStatusCode)
            {
                var smsResponse = DeserializeResponse<SmsTrafficResponse>(responseString);

                if (smsResponse.Result == SmsTrafficResponse.SuccessfulCode)
                {
                    return new SmsSendingResult
                    {
                        ProviderName = Name,
                        Success = true,
                        SmsSendingData = smsResponse.MessageInfos?.MessageInfo?.Count > 0
                        ? new List<SmdSendingData>
                        {
                            new()
                            {
                                MessageId = smsResponse.MessageInfos.MessageInfo[0].SmsId
                            }
                        }
                        : null
                    };
                }

                return new SmsSendingResult
                {
                    ProviderName = Name,
                    Success = false,
                    Errors = new List<SmsSendingError>
                    {
                        new()
                        {
                            Code = smsResponse.Code.ToString(),
                            Message = smsResponse.Description
                        }
                    }
                };

            }

            return new SmsSendingResult
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError> { new() { Message = responseString } }
            };
        }

        /// <inheritdoc/>
        public int GetBalance()
            => GetBalanceAsync().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<int> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "account" }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "multi.php", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            var smsResponse = DeserializeResponse<AccountBalanceResponse>(responseString);

            return smsResponse.Account;
        }

        /// <inheritdoc/>
        public GroupListInformation GetGroupListInformation()
            => GetGroupListInformationAsync().GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<GroupListInformation> GetGroupListInformationAsync(CancellationToken cancellationToken = default)
        {
            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "status_all" }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "list.php", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            var smsResponse = DeserializeResponse<GroupListInformation>(responseString);

            return smsResponse;
        }

        /// <inheritdoc/>
        public GroupInformation? GetGroupInformation(string groupId)
            => GetGroupInformationAsync(groupId).GetAwaiter().GetResult();

        public async Task<GroupInformation?> GetGroupInformationAsync(string groupId, CancellationToken cancellationToken = default)
        {
            if (groupId == null)
                throw new ArgumentNullException(nameof(groupId));

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "status" },
                { "group_id", groupId }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "list.php", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            var smsResponse = DeserializeResponse<GroupListInformation>(responseString);

            var result = smsResponse?.Groups?.GroupList?.Count > 0 ? smsResponse?.Groups?.GroupList[0] : null;

            return result;
        }

        /// <inheritdoc/>
        public GroupOperation AddGroupMember(string groupId, string memberNumber)
            => AddGroupMemberAsync(groupId, memberNumber).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public Task<GroupOperation> AddGroupMemberAsync(string groupId, string memberNumber, CancellationToken cancellationToken = default)
            => AddGroupMembersAsync(groupId, new string[1] { memberNumber }, cancellationToken);

        /// <inheritdoc/>
        public GroupOperation AddGroupMembers(string groupId, IEnumerable<string> memberNumbers)
        => AddGroupMembersAsync(groupId, memberNumbers).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<GroupOperation> AddGroupMembersAsync(string groupId, IEnumerable<string> memberNumbers, CancellationToken cancellationToken = default)
        {
            if (groupId == null)
                throw new ArgumentNullException(nameof(groupId));

            if (memberNumbers == null)
                throw new ArgumentNullException(nameof(memberNumbers));

            var count = memberNumbers.Count();
            if (count > 5000)
                throw new ArgumentException($"The count of {nameof(memberNumbers)} cannot exceed 5000. The current count of members: {count}.");

#if NET5_0_OR_GREATER
            var numbers = String.Join(_phoneNumberSeparator, memberNumbers);
#else
            var numbers = String.Join(_phoneNumberSeparator.ToString(), memberNumbers);
#endif

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "add_member" },
                { "group_id", groupId },
                { "member",  numbers }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "list.php", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            var smsResponse = DeserializeResponse<GroupOperation>(responseString);

            return smsResponse;
        }

        /// <inheritdoc/>
        public GroupOperation RemoveGroupMember(string groupId, string memberNumber)
            => RemoveGroupMemberAsync(groupId, memberNumber).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public Task<GroupOperation> RemoveGroupMemberAsync(string groupId, string memberNumber, CancellationToken cancellationToken = default)
            => RemoveGroupMembersAsync(groupId, new string[1] { memberNumber }, cancellationToken);

        /// <inheritdoc/>
        public GroupOperation RemoveGroupMembers(string groupId, IEnumerable<string> memberNumbers)
            => RemoveGroupMembersAsync(groupId, memberNumbers).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public async Task<GroupOperation> RemoveGroupMembersAsync(string groupId, IEnumerable<string> memberNumbers, CancellationToken cancellationToken = default)
        {
            if (groupId == null)
                throw new ArgumentNullException(nameof(groupId));

            if (memberNumbers == null)
                throw new ArgumentNullException(nameof(memberNumbers));

            var count = memberNumbers.Count();
            if (count > 5000)
                throw new ArgumentException($"The count of {nameof(memberNumbers)} cannot exceed 5000. The current count of members: {count}.");

#if NET5_0_OR_GREATER
            var numbers = String.Join(_phoneNumberSeparator, memberNumbers);
#else
            var numbers = String.Join(_phoneNumberSeparator.ToString(), memberNumbers);
#endif

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "operation", "remove_member" },
                { "group_id", groupId },
                { "member",  numbers }
            };

            var content = new FormUrlEncodedContent(requestData);

            var response = await SendRequest(content, "list.php", cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            var smsResponse = DeserializeResponse<GroupOperation>(responseString);

            return smsResponse;
        }

        protected override SmsTrafficMessage CreateMessage(SmsMessage originalMessage)
        {
            if (originalMessage is SmsTrafficMessage smsTrafficMessage)
            {
                return smsTrafficMessage;
            }


            SmsTrafficMessageData? messageData = default;
            var providerData = originalMessage.ProviderData;
            if (providerData != null
                && providerData.Count > 0)
            {
                messageData = new SmsTrafficMessageData();
                foreach (var data in providerData)
                {
                    var property = GetSmsTrafficDataProperty(data.Name);
                    if (property == null)
                        continue;

                    property.SetValue(messageData, data.Value);
                }
            }

            var message = new SmsTrafficMessage(originalMessage.Body, originalMessage.From, originalMessage.To, originalMessage.IsAllowSendToForeignNumbers, messageData);

            return message;
        }

        private SmsTrafficRequest ConvertToRequestObject(SmsTrafficMessage smsTrafficMessage)
        {
            var additionalData = smsTrafficMessage.SmsTrafficProviderData;
            var rus = additionalData?.Rus.ToString() ?? Utf8Code;
            var message = rus == Utf8Code ? smsTrafficMessage.Body : System.Web.HttpUtility.UrlEncode(smsTrafficMessage.Body, _windows1251Encoding);
            var isToGroup = smsTrafficMessage.To.Any(char.IsLetter);

            return new SmsTrafficRequest
            {
                Login = _options.Login,
                Password = _options.Password,
                Phones = isToGroup ? null : smsTrafficMessage.To,
                Message = message,
                Originator = smsTrafficMessage.From,
                Rus = rus,
                Flash = BoolToString(additionalData?.Flash),
                StartDate = additionalData?.StartDate?.ToString(DateTimeFormat),
                MaxParts = additionalData?.MaxParts?.ToString(),
                Gap = additionalData?.Gap?.ToString(),
                Group = additionalData?.Group ?? (isToGroup ? smsTrafficMessage.To : null),
                Timeout = additionalData?.Timeout?.ToString(),
                IndividualMessages = BoolToString(additionalData?.IndividualMessages),
                Delimiter = additionalData?.Delimiter?.ToString(),
                WantSmsIds = BoolToString(additionalData?.WantSmsIds),
                WithPushId = BoolToString(additionalData?.WithPushId),
                IgnorePhoneFormat = BoolToString(additionalData?.IgnorePhoneFormat),
                TwoByteConcat = BoolToString(additionalData?.TwoByteConcat)
            };
        }

        private static string? BoolToString(bool? boolValue) => boolValue == null ? null : boolValue.Value ? "1" : "0";

        private static bool TryDateTimeParse(string value, out DateTime dt) => DateTime.TryParseExact(value, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt);

        private static HttpClient ConfigureHttpClient(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(60);

            return client;
        }

        private static readonly ConcurrentDictionary<string, PropertyInfo?> _propertyCache = [];

        private static PropertyInfo? GetSmsTrafficDataProperty(string name)
        {
            if (!_propertyCache.TryGetValue(name, out var propertyInfo))
            {
                propertyInfo = _propertyCache[name] = typeof(SmsTrafficMessageData).GetProperty(name);
            }

            return propertyInfo;
        }

        private static T DeserializeResponse<T>(string responseString)
        {
            var serializer = new XmlSerializer(typeof(T));
            T smsResponse;

            using (var reader = new StringReader(responseString))
            {
                smsResponse = (T)serializer.Deserialize(reader);
            }

            return smsResponse;
        }
    }
}

﻿using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spoleto.Common.Helpers;
using Spoleto.SMS.Extensions;

namespace Spoleto.SMS.Providers.GetSms
{
    /// <summary>
    /// The GetSMS provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://getsms.uz/page/index/16"/>.
    /// </remarks>
    public class GetSmsProvider : SmsProviderBase<SmsMessage>, IGetSmsProvider
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.GetSMS);

        private readonly GetSmsOptions _options;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Creates an instance of <see cref="GetSmsProvider"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance.</param>
        /// <param name="options">The options instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        public GetSmsProvider(GetSmsOptions options)
            : this(new HttpClient(), options)
        {
        }

        /// <summary>
        /// Creates an instance of <see cref="GetSmsProvider"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> instance.</param>
        /// <param name="options">The options instance.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="httpClient"/> or <paramref name="options"/> are null.</exception>
        public GetSmsProvider(HttpClient httpClient, GetSmsOptions options)
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
        public override bool IsAllowNullFrom => true;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = ["998"];

        /// <inheritdoc/>
        public override sealed bool CanSend(string phoneNumber, bool isAllowSendToForeignNumbers = false)
        {
            phoneNumber = CleanPhoneNumber(phoneNumber);
            if (!LocalPrefixPhoneNumbers.Any(phoneNumber.StartsWith))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
            => GetStatusAsync(id, phoneNumber).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var requestIdData = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "request_id", id }
                }
            };

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password },
                { "data", JsonHelper.ToJson(requestIdData) }
            };

            var content = new FormUrlEncodedContent(requestData);

            var url = new Uri(new Uri(_options.ServiceUrl), "status/");
            var response = await _httpClient.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            if (response.IsSuccessStatusCode)
            {
                using var jsonDocument = JsonDocument.Parse(responseString);
                var rootElement = jsonDocument.RootElement;

                // Check if the returned JSON is an object
                if (rootElement.ValueKind == JsonValueKind.Object)
                {
                    // Check for a distinct property that's only in one of the JSON types
                    if (rootElement.TryGetProperty(GetJsonPropertyName<SmsSendingError>(nameof(SmsSendingError.Error)), out var _))
                    {
                        // Handle the error case when JSON is an object
                        var error = JsonHelper.FromJson<SmsSendingError>(responseString);
                        return new SmsStatusResult
                        {
                            ProviderName = Name,
                            Success = false,
                            Errors = new List<SmsSendingError> { error }
                        };
                    }

                    // Handle the success case when JSON is an object
                    var statusData = JsonHelper.FromJson<SmsStatusData>(responseString);
                    return new SmsStatusResult
                    {
                        ProviderName = Name,
                        Success = true,
                        SmsStatusData = new List<SmsStatusData> { statusData }
                    };
                }
                // Check if the returned JSON is an array
                else if (rootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in rootElement.EnumerateArray())
                    {
                        // Assuming the array items are objects and have at least one property
                        if (item.ValueKind == JsonValueKind.Object)
                        {
                            // Check for a distinct property that's only in one of the JSON types
                            if (item.TryGetProperty(GetJsonPropertyName<SmsSendingError>(nameof(SmsSendingError.Error)), out var _))
                            {
                                // Handle the error case when JSON is an object
                                var errors = JsonHelper.FromJson<List<SmsSendingError>>(responseString);
                                return new SmsStatusResult
                                {
                                    ProviderName = Name,
                                    Success = false,
                                    Errors = errors
                                };
                            }

                            // Handle the success case when JSON is an object
                            var statusData = JsonHelper.FromJson<List<SmsStatusData>>(responseString);
                            return new SmsStatusResult
                            {
                                ProviderName = Name,
                                Success = true,
                                SmsStatusData = statusData
                            };
                        }
                    }
                }
            }

            return new SmsStatusResult
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError> { new() { Message = responseString } }
            };
        }

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            var getSmsMessage = CreateMessage(message);

#if NET5_0_OR_GREATER
            var phoneNumbers = getSmsMessage.To.Split(getSmsMessage.PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
            var phoneNumbers = getSmsMessage.To.Split(getSmsMessage.PhoneNumberSeparator);
#endif

            // Validate:
            ValidateDataForSMS(phoneNumbers, getSmsMessage);

            var smsList = phoneNumbers
                .Select(x => new Dictionary<string, string>()
                {
                    {"phone", x},
                    {"text", getSmsMessage.Body}
                }).ToList();

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password }
            };

            if (!string.IsNullOrEmpty(getSmsMessage.From))
            {
                requestData.Add("nickname", getSmsMessage.From);
            }

            requestData.Add("data", JsonHelper.ToJson(smsList));

            var content = new FormUrlEncodedContent(requestData);

            var response = await _httpClient.PostAsync(_options.ServiceUrl, content, cancellationToken).ConfigureAwait(false);
#if NET5_0_OR_GREATER
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

            if (response.IsSuccessStatusCode)
            {
                using var jsonDocument = JsonDocument.Parse(responseString);
                var rootElement = jsonDocument.RootElement;

                // Check if the returned JSON is an array
                if (rootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in rootElement.EnumerateArray())
                    {
                        // Assuming the array items are objects and have at least one property
                        if (item.ValueKind == JsonValueKind.Object)
                        {
                            // Check for a distinct property that's only in one of the JSON types
                            if (item.TryGetProperty(GetJsonPropertyName<SmsSendingError>(nameof(SmsSendingError.Error)), out var _))
                            {
                                // Handle the error case when JSON is an object
                                var errors = JsonHelper.FromJson<List<SmsSendingError>>(responseString);
                                return new SmsSendingResult
                                {
                                    Success = false,
                                    ProviderName = Name,
                                    Errors = errors
                                };
                            }

                            // Handle the success case when JSON is an object
                            var statusData = JsonHelper.FromJson<List<SmdSendingData>>(responseString);
                            return new SmsSendingResult
                            {
                                ProviderName = Name,
                                Success = true,
                                SmsSendingData = statusData
                            };
                        }
                    }
                }
                // Check if the returned JSON is an object
                else if (rootElement.ValueKind == JsonValueKind.Object)
                {
                    // Check for a distinct property that's only in one of the JSON types
                    if (rootElement.TryGetProperty(GetJsonPropertyName<SmsSendingError>(nameof(SmsSendingError.Error)), out var _))
                    {
                        // Handle the error case when JSON is an object
                        var error = JsonHelper.FromJson<SmsSendingError>(responseString);
                        return new SmsSendingResult
                        {
                            ProviderName = Name,
                            Success = false,
                            Errors = new List<SmsSendingError> { error }
                        };
                    }

                    // Handle the success case when JSON is an object
                    var statusData = JsonHelper.FromJson<SmdSendingData>(responseString);
                    return new SmsSendingResult
                    {
                        ProviderName = Name,
                        Success = true,
                        SmsSendingData = new List<SmdSendingData> { statusData }
                    };
                }


            }

            return new SmsSendingResult
            {
                ProviderName = Name,
                Success = false,
                Errors = new List<SmsSendingError> { new() { Message = responseString } }
            };
        }

        private static readonly Dictionary<string, string> _reflectionCache = new();

        private static string GetJsonPropertyName<T>(string propertyName)
        {
            lock (((ICollection)_reflectionCache).SyncRoot)
            {
                var key = typeof(T).FullName + "." + propertyName;
                if (!_reflectionCache.TryGetValue(key, out var jsonPropertyName))
                {
                    var propertyInfo = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException("Property not found.", nameof(propertyName));

                    var jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();

                    _reflectionCache[key] = jsonPropertyName = jsonPropertyNameAttribute?.Name ?? propertyName; // Json attribute name or the actual property name if not defined.
                }

                return jsonPropertyName;
            }
        }

        private static HttpClient ConfigureHttpClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Opera 10.00");
            client.Timeout = TimeSpan.FromSeconds(60);

            return client;
        }
    }
}

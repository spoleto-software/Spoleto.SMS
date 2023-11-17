using System.Collections;
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
    public class GetSmsProvider : SmsProviderBase, IGetSmsProvider
    {
        /// <summary>
        /// The name of the SMS provider.
        /// </summary>
        public const string ProviderName = nameof(SmsProviderName.GetSMS);

        private readonly GetSmsOptions _options;

        public GetSmsProvider(GetSmsOptions options)
        {
            // Validates if the options are valid
            options.Validate();
            _options = options;
        }

        /// <inheritdoc/>
        public override string Name => ProviderName;

        /// <inheritdoc/>
        public override bool IsAllowNullFrom => true;

        protected override List<string> LocalPrefixPhoneNumbers { get; } = new List<string> { "998" };

        /// <inheritdoc/>
        public override SmsSendingResult Send(SmsMessage message)
            => SendAsync(message).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            var httpClient = new HttpClient(); //todo:

            var phoneNumbers = message.To.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // Validate:
            phoneNumbers.ForEach(number => ValidateDataForSMS(number, message.Body, message.IsAllowSendToForeignNumbers));

            var smsList = phoneNumbers
                .Select(x => new Dictionary<string, string>()
                {
                    {"phone", x},
                    {"text", message.Body}
                }).ToList();

            var requestData = new Dictionary<string, string>
            {
                { "login", _options.Login },
                { "password", _options.Password }
            };

            if (!string.IsNullOrEmpty(message.From))
            {
                requestData.Add("nickname", message.From);
            }

            requestData.Add("data", JsonHelper.ToJson(smsList));

            var content = new FormUrlEncodedContent(requestData);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Opera 10.00");
            httpClient.Timeout = TimeSpan.FromSeconds(60);

            var response = await httpClient.PostAsync(_options.ServiceUrl, content, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

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

        /// <inheritdoc/>
        public override SmsStatusResult GetStatus(string id, string? phoneNumber)
            => GetStatusAsync(id, phoneNumber).GetAwaiter().GetResult();

        /// <inheritdoc/>
        public override async Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            var httpClient = new HttpClient(); //todo:

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

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Opera 10.00");
            httpClient.Timeout = TimeSpan.FromSeconds(60);

            var url = new Uri(new Uri(_options.ServiceUrl), "status/");
            var response = await httpClient.PostAsync(url, content, cancellationToken).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

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

        private static readonly Dictionary<string, string> _reflectionCache = new();

        public static string GetJsonPropertyName<T>(string propertyName)
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
    }
}

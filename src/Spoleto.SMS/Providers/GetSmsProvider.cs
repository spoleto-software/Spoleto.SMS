using Spoleto.Common.Helpers;

namespace Spoleto.SMS.Providers
{
    /// <summary>
    /// The GetSMS provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://getsms.uz/page/index/16"/>.
    /// </remarks>
    public class GetSmsProvider : ISmsProvider
    {
        private const string ProviderName = nameof(SmsProviderName.GetSMS);
        private readonly GetSmsOptions _options;

        public GetSmsProvider(GetSmsOptions options)
        {
            _options = options;
        }

        /// <inheritdoc/>
        public string Name => ProviderName;

        /// <inheritdoc/>
        public SmsSendingResult Send(SmsMessage message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<SmsSendingResult> SendAsync(SmsMessage message, CancellationToken cancellationToken = default)
        {
            //todo: process if message.To is multiple numbers

            var httpClient = new HttpClient();

            var smsList = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
            {
                { "phone", "998909711322" },
                { "text", "Ваш текст СМС" }
            },
            new Dictionary<string, string>
            {
                { "phone", "998909711322" },
                { "text", "Ваш текст СМС 2" }
            }
        };

            var requestData = new Dictionary<string, string>
            {
                { "login", Uri.EscapeDataString(_options.Login) },
                { "password", Uri.EscapeDataString(_options.Password) }
            };

            if (!string.IsNullOrEmpty(_options.Nickname))
            {
                requestData.Add("nickname", Uri.EscapeDataString(_options.Nickname));
            }

            requestData.Add("data", Uri.EscapeDataString(JsonHelper.ToJson(smsList)));

            var content = new FormUrlEncodedContent(requestData);

            httpClient.DefaultRequestHeaders.Add("User-Agent", "Opera 10.00");


            var response = await httpClient.PostAsync(_options.ServiceUrl, content, cancellationToken).ConfigureAwait(false);

            var result = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
                return new SmsSendingResult
                {
                    Success = true,
                };

            return new SmsSendingResult();

            //    var smsList = new List<Dictionary<string, string>>()
            //{
            //    new Dictionary<string, string>(){
            //        {"phone", message.To},
            //        {"text", message.Body}
            //    }
            //};

            //var dataDict = new Dictionary<string, string>()
            //{
            //    { "login", _options.Login },
            //    { "password", _options.Password },
            //    { "data", JsonHelper.ToJson(smsList)}
            //};

            ////Extend this conditional block if you need to add 'nickname' in your request data
            //string nickname = null;
            //if (!string.IsNullOrEmpty(nickname))
            //{
            //    dataDict.Add("nickname", nickname);
            //}

            //var dataContent = new FormUrlEncodedContent(dataDict);



        }

        /// <inheritdoc/>
        public SmsStatusResult GetStatus(string id, string? phoneNumber)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<SmsStatusResult> GetStatusAsync(string id, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

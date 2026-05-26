using System.Text.RegularExpressions;

namespace Spoleto.SMS.Providers.Smsc
{
    public record SmscMessage : SmsMessage
    {
        private bool _skipValidation = true;

        // Ищет от 7 до 15 цифр в начале строки или после переноса строки, перед двоеточием
        private static readonly Regex _phoneRegex = new Regex(@"(?m)^\d{7,15}(?=:)", RegexOptions.Compiled);

        public SmscMessage(string? body, string? from, string to, bool isAllowSendToForeignNumbers = false, SmscMessageData? providerData = null)
            : base(body, from, to, isAllowSendToForeignNumbers)
        {
            SmscProviderData = providerData;

            _skipValidation = false;
            Validate();
        }

        public SmscMessage(string? body, string? from, List<string> listOfTo, bool isAllowSendToForeignNumbers = false, SmscMessageData? providerData = null)
            : base(body, from, listOfTo, isAllowSendToForeignNumbers)
        {
            SmscProviderData = providerData;

            _skipValidation = false;
            Validate();
        }

        /// <summary>
        /// Additional data for the message.
        /// </summary>
        public SmscMessageData? SmscProviderData { get; }

        protected override void Validate()
        {
            if (_skipValidation)
            {
                return;
            }

            if (SmscProviderData?.List != null
                && (!string.IsNullOrEmpty(Body) || To != null))
            {
                throw new Exception($"The SMS body or To is not empty but <{nameof(SmscProviderData.List)}> is set.");
            }
            else if (Body == null
                && To == null
                && SmscProviderData?.List == null)
            {
                throw new ArgumentNullException(nameof(Body));
            }
        }

        public IEnumerable<string> GetRecipients()
        {
            if (SmscProviderData?.List != null)
            {
                var matches = _phoneRegex.Matches(SmscProviderData.List);
                var multiPhones = new List<string>(matches.Count);

                foreach (Match match in matches)
                {
                    multiPhones.Add(match.Value);
                }

                return multiPhones;
            }

            var phones = To.Split([PhoneNumberSeparator.ToString()], StringSplitOptions.RemoveEmptyEntries);

            return phones;
        }
    }
}




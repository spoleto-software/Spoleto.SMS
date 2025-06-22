namespace Spoleto.SMS.Providers.SmsTraffic
{
    public record SmsTrafficMessage : SmsMessage
    {
        private bool _skipValidation = true;

        public SmsTrafficMessage(string? body, string? from, string to, bool isAllowSendToForeignNumbers = false, SmsTrafficMessageData? providerData = null)
            : base(body, from, to, isAllowSendToForeignNumbers)
        {
            SmsTrafficProviderData = providerData;
            
            _skipValidation = false;
            Validate();
        }

        public SmsTrafficMessage(string? body, string? from, List<string> listOfTo, bool isAllowSendToForeignNumbers = false, SmsTrafficMessageData? providerData = null)
            : base(body, from, listOfTo, isAllowSendToForeignNumbers)
        {
            SmsTrafficProviderData = providerData;

            _skipValidation = false;
            Validate();
        }

        /// <summary>
        /// The phone number separator.
        /// </summary>
        public override char PhoneNumberSeparator => ',';

        /// <summary>
        /// Additional data for the message.
        /// </summary>
        public SmsTrafficMessageData? SmsTrafficProviderData { get; }

        protected override void Validate()
        {
            if (_skipValidation)
            {
                return;
            }

            if (SmsTrafficProviderData?.IndividualMessages == true
                && Body != null)
            {
                throw new Exception($"The SMS body is not empty but <{nameof(SmsTrafficProviderData.IndividualMessages)}> flag is set.");
            }
            else if (Body == null 
                && (SmsTrafficProviderData?.IndividualMessages ?? false) == false)
            {
                throw new ArgumentNullException(nameof(Body));
            }
        }
    }
}




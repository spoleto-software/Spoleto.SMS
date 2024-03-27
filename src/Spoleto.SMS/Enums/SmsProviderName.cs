namespace Spoleto.SMS
{
    /// <summary>
    /// SMS provider names.
    /// </summary>
    public enum SmsProviderName
    {
        /// <summary>
        /// The Russian service: <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>.
        /// </summary>
        SMSC,

        /// <summary>
        /// The Russian service: <see href="https://www.smstraffic.ru/api"/>. 
        /// </summary>
        SmsTraffic,

        /// <summary>
        /// The Uzbek service: <see href="https://getsms.uz/page/index/16"/>.
        /// </summary>
        GetSMS
    }
}

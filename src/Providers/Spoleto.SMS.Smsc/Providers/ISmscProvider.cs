﻿namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// The SMSC provider for sending SMS messages.
    /// </summary>
    /// <remarks>
    /// <see href="https://smsc.ru/api/code/libraries/http_smtp/cs/#menu"/>.
    /// </remarks>
    public interface ISmscProvider : ISmsProvider
    {
        string GetBalance();

        void CheckPhoneNumber(string phoneNumber, string sender, bool isAllowSendToForeignNumbers = false);
    }
}

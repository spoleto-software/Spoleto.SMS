﻿using Spoleto.SMS.Providers;

namespace Spoleto.SMS
{
    /// <summary>
    /// Extension methods for the <see cref="ISmsService"/> type.
    /// </summary>
    public static class SmsServiceExtensions
    {
        /// <summary>
        /// Gets a suitable SMS provider for the specified phone number.
        /// </summary>
        /// <param name="smsService">The <see cref="ISmsService"/> instance.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="returnDefaultIfNotFound">The flag indicating whether the default SMS provider will be return, if a suitable SMS provider cannot be found.</param>
        /// <param name="isAllowSendToForeignNumbers">The flag indicating whether the message can be sent to international numbers.</param>
        /// <returns>A suitable provider if found, null otherwise.</returns>
        public static ISmsProvider? GetProviderForPhoneNumber(this ISmsService smsService, string phoneNumber, bool returnDefaultIfNotFound = true, bool isAllowSendToForeignNumbers = false)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            foreach (var provider in smsService.Providers)
            {
                if (provider.CanSend(phoneNumber, isAllowSendToForeignNumbers))
                    return provider;
            }

            if (returnDefaultIfNotFound)
                return smsService.DefaultProvider;

            return default;
        }

        /// <summary>
        /// Finds a suitable SMS provider for the specified phone number and sends the message using this provider.
        /// </summary>
        /// <param name="smsService">The <see cref="ISmsService"/> instance.</param>
        /// <param name="message">The SMS message.</param>
        /// <param name="sendUsingDefaultIfNotFound">The flag indicating whether the default SMS provider will be used, if a suitable SMS provider cannot be found.</param>
        /// <exception cref="ArgumentException">Thrown when a suitable SMS provider is not found.</exception>
        public static SmsSendingResult SendUsingSuitableProvider(this ISmsService smsService, SmsMessage message, bool sendUsingDefaultIfNotFound = true)
        {
            var provider = smsService.GetProviderForPhoneNumber(message.To, sendUsingDefaultIfNotFound, message.IsAllowSendToForeignNumbers);
            if (provider == null)
            {
                throw new ArgumentException($"Couldn't find a suitable SMS provider for the phone number {message.To}.");
            }

            return provider.Send(message);
        }

        /// <summary>
        /// Finds a suitable SMS provider for the specified phone number and asynchronously sends the message using this provider.
        /// </summary>
        /// <param name="smsService">The <see cref="ISmsService"/> instance.</param>
        /// <param name="message">The SMS message.</param>
        /// <param name="sendUsingDefaultIfNotFound"></param>
        /// <exception cref="ArgumentException">Thrown when a suitable SMS provider is not found.</exception>
        public static Task<SmsSendingResult> SendUsingSuitableProviderAsync(this ISmsService smsService, SmsMessage message, bool sendUsingDefaultIfNotFound)
        {
            var provider = smsService.GetProviderForPhoneNumber(message.To, sendUsingDefaultIfNotFound);
            if (provider == null)
            {
                throw new ArgumentException($"Couldn't find a suitable SMS provider for the phone number {message.To}.");
            }

            return provider.SendAsync(message);
        }
    }
}

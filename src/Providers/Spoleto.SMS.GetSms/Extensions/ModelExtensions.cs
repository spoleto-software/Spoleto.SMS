namespace Spoleto.SMS.Providers.GetSms
{
    internal static class ModelExtensions
    {
        public static Spoleto.SMS.SmdSendingData ToSmsSendingResult(this Spoleto.SMS.Providers.GetSms.SmdSendingData source)
            => new()
            {
                ClientIp = source.ClientIp,
                DateReceived = source.DateReceived,
                MessageId = source.MessageId.ToString(),
                Recipient = source.Recipient.ToString(),
                RequestId = source.RequestId,
                Text = source.Text,
                UserId = source.UserId
            };

        public static Spoleto.SMS.SmsSendingError ToSmsSendingError(this Spoleto.SMS.Providers.GetSms.SmsSendingError source)
            => new()
            {
                ClientIp = source.ClientIp,
                DateReceived = source.DateReceived,
                Recipient = source.Recipient,
                RequestId = source.RequestId,
                Text = source.Text,
                UserId = source.UserId,
                MessageId = source.MessageId.ToString(),
                Error = source.Error,
                Code = source.Code,
                Message = source.Message,
                NumCode = source.NumCode
            };

        public static Spoleto.SMS.SmsStatusData ToSmsStatusData(this Spoleto.SMS.Providers.GetSms.SmsStatusData source)
            => new()
            {
                ClientIp = source.ClientIp,
                DateReceived = source.DateReceived,
                Recipient = source.Recipient.ToString(),
                RequestId = source.RequestId,
                Text = source.Text,
                UserId = source.UserId,
                MessageId = source.MessageId,
                CountMessages = source.CountMessages,
                DateDelivered = source.DateDelivered,
                DateSent = source.DateSent,
                Description = source.Description,
                Status = source.Status
            };
    }
}

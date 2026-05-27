using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Microsoft.Extensions.Logging;

namespace Spoleto.SMS.Providers.Smsc
{
    /// <summary>
    /// The SMSC API.
    /// </summary>
    public partial class SmscProvider : ISmscProvider
    {
        // The client always requests comma-delimited plain-text responses (fmt=1).
        private const string FmtCsv = "1";
        private const string BaseHost = "smsc.ru";

        // Endpoint file names:
        private const string SendEndpoint = "send.php";
        private const string StatusEndpoint = "status.php";
        private const string BalanceEndpoint = "balance.php";

        /// <inheritdoc/>
        public async Task<SmscResult<SmscSendSuccessResult>> SendAsync(
            SmscMessage message,
            IEnumerable<string>? files = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(message.SmscProviderData?.List))
            {
                if (string.IsNullOrWhiteSpace(message.To))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(message.To));
                }

                if (message.Body == null)
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(message.Body));
                }

#if NET5_0_OR_GREATER
                var phoneNumbers = message.To.Split(message.PhoneNumberSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
#else
                var phoneNumbers = message.To.Split(message.PhoneNumberSeparator);
#endif
                // Validate:
                ValidateDataForSMS(phoneNumbers, message);
            }

            var fileList = files?.ToArray();
            var usePost = _options.UsePost || fileList?.Length > 0;

            var parameters = BuildAuthParameters();
            if (!string.IsNullOrWhiteSpace(message.To))
            {
                parameters["phones"] = message.To;
            }
            if (!string.IsNullOrEmpty(message.Body))
            {
                parameters["mes"] = message.Body;
            }

            if (!string.IsNullOrEmpty(message.SmscProviderData?.List))
            {
                parameters["nl"] = "1"; // process the symbol '/n' in messages
            }

            // cost=3 → response: <id>,<cnt>,<cost>,<balance>
            parameters["cost"] = "3";

            if (!string.IsNullOrWhiteSpace(message.From))
            {
                parameters["sender"] = message.From!;
            }

            ApplyMessageData(parameters, message.SmscProviderData);

            var raw = await ExecuteAsync(SendEndpoint, parameters, usePost, fileList, cancellationToken).ConfigureAwait(false);

            // fmt=1 success (cost=3): <id>,<cnt>,<cost>,<balance>
            // fmt=1 error 1,2,4,5,9:  0,-N
            // fmt=1 error 3,6,7,8:   <id>,-N
            if (TryParseError(raw, out var error))
            {
                LogDebug("Send failed. MessageId: {Id}, Error: {Code}.", error!.MessageId, error.ErrorCode);

                return SmscResult<SmscSendSuccessResult>.Fail(error);
            }

            var success = new SmscSendSuccessResult
            {
                Id = raw[0],
                SmsCount = raw.Length > 1 && int.TryParse(raw[1], out int cnt) ? cnt : 0,
                Cost = raw.Length > 2 && TryParseDecimal(raw[2], out decimal c) ? c : 0m,
                Balance = raw.Length > 3 && TryParseDecimal(raw[3], out decimal b) ? b : 0m
            };

            LogDebug("SMS sent. ID: {Id}, Count: {Count}, Cost: {Cost}, Balance: {Balance}",
                success.Id, success.SmsCount, success.Cost, success.Balance);

            return SmscResult<SmscSendSuccessResult>.Ok(success);
        }

        /// <inheritdoc/>
        public async Task<SmscResult<SmscCostSuccessResult>> GetSmsCostAsync(
            string phones,
            string message,
            string? sender = null,
            SmscMessageData? data = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(data?.List))
            {
                if (string.IsNullOrWhiteSpace(phones))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(phones));
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
                }
            }

            var parameters = BuildAuthParameters();
            if (!string.IsNullOrWhiteSpace(phones))
            {
                parameters["phones"] = phones;
            }
            if (!string.IsNullOrEmpty(message))
            {
                parameters["mes"] = message;
            }
            // cost=1 → estimate only, no message is sent; response: <cost>,<cnt>
            parameters["cost"] = "1";

            if (!string.IsNullOrWhiteSpace(sender))
            {
                parameters["sender"] = sender!;
            }

            ApplyMessageData(parameters, data);

            var raw = await ExecuteAsync(SendEndpoint, parameters, _options.UsePost, null, cancellationToken).ConfigureAwait(false);

            if (TryParseError(raw, out var error))
            {
                LogDebug("Cost check failed. Error: {Code}.", error!.ErrorCode);

                return SmscResult<SmscCostSuccessResult>.Fail(error);
            }

            var success = new SmscCostSuccessResult
            {
                Cost = TryParseDecimal(raw[0], out decimal cost) ? cost : 0m,
                SmsCount = raw.Length > 1 && int.TryParse(raw[1], out int cnt) ? cnt : 0
            };

            LogDebug("Cost: {Cost}, SMS count: {Count}", success.Cost, success.SmsCount);

            return SmscResult<SmscCostSuccessResult>.Ok(success);
        }

        /// <inheritdoc/>
        public async Task<SmscStatusResult> GetStatusAsync(
            string id,
            string phone,
            int all = 0,
            bool forTelegramBot = false,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(phone));
            }

            var parameters = BuildAuthParameters();
            parameters["phone"] = phone;
            parameters["id"] = id;
            parameters["all"] = all.ToString();

            // bot=1: query messages delivered via a Telegram bot (documented in status.php params)
            if (forTelegramBot)
            {
                parameters["bot"] = "1";
            }

            var isBatch = id.Contains(',');

            // Batch responses use newline as the row separator; each row is still comma-delimited.
            var raw = await ExecuteAsync(StatusEndpoint, parameters, _options.UsePost, null, cancellationToken, delimiter: isBatch ? '\n' : ',').ConfigureAwait(false);

            return isBatch ? ParseBatchStatus(raw) : ParseSingleStatus(raw);
        }

        /// <inheritdoc/>
        public decimal? GetBalance(CancellationToken cancellationToken = default)
        {
            return GetBalanceAsync(cancellationToken).GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public async Task<decimal?> GetBalanceAsync(CancellationToken cancellationToken = default)
        {
            var parameters = BuildAuthParameters();

            var raw = await ExecuteAsync(BalanceEndpoint, parameters, _options.UsePost, null, cancellationToken).ConfigureAwait(false);

            // fmt=1 success: <balance>
            // fmt=1 error:   0,-N
            if (raw.Length == 1 && TryParseDecimal(raw[0], out decimal balance))
            {
                LogDebug("Balance: {Balance}", balance);
                return balance;
            }

            if (raw.Length >= 2)
            {
                LogDebug("Balance check failed. Error: {Code}", raw[1].TrimStart('-'));
            }

            return null;
        }

        /// <summary>
        /// Sends an SMS via SMTP (alternative delivery channel documented at smsc.ru/api/smtp).
        /// </summary>
        /// <remarks>
        /// SMTP body format: <c>login:password:id:time:translit,format,sender:phones:message</c>
        /// </remarks>
        public async Task SendSmsViaSmtpAsync(
            string phones,
            string message,
            string? sender = null,
            SmscMessageData? data = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(phones))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(phones));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(message));
            }

            var id = data?.Id ?? "0";
            var time = data?.Time ?? string.Empty;
            var translit = data?.Translit ?? 0;
            var format = ResolveFormatEmailFormat(data);

            using var mail = new MailMessage();
            mail.To.Add("send@send.smsc.ru");
            mail.From = new MailAddress(_options.SmtpFrom);
            mail.Body = string.Join(":",
                _options.Login ?? string.Empty,
                _options.Password ?? string.Empty,
                id, time,
                $"{translit},{format},{sender ?? string.Empty}",
                phones, message);

            mail.BodyEncoding = Encoding.GetEncoding(_options.Charset);
            mail.IsBodyHtml = false;

            using var smtpClient = new SmtpClient(_options.SmtpServer, _options.SmtpPort)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false,
                UseDefaultCredentials = false
            };

            if (!string.IsNullOrEmpty(_options.SmtpLogin))
            {
                smtpClient.Credentials = new NetworkCredential(_options.SmtpLogin, _options.SmtpPassword);
            }

#if NET7_0_OR_GREATER
            await smtpClient.SendMailAsync(mail, cancellationToken).ConfigureAwait(false);
#else
            await smtpClient.SendMailAsync(mail).ConfigureAwait(false);
#endif
        }

        /// <summary>
        /// Builds the mandatory parameters that every request must carry.
        /// Includes authentication (apikey OR login+psw), fixed response format
        /// (<c>fmt=1</c>), and the default charset from options.
        /// </summary>
        private Dictionary<string, string> BuildAuthParameters()
        {
            var p = new Dictionary<string, string>(StringComparer.Ordinal)
            {
                // fmt=1 → comma-separated plain text (most compact)
                ["fmt"] = FmtCsv,
                ["charset"] = _options.Charset
            };

            // API accepts either apikey alone, or login + psw together.
            if (!string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                p["apikey"] = _options.ApiKey!;
            }
            else
            {
                p["login"] = _options.Login!;
                p["psw"] = _options.Password!;
            }

            return p;
        }

        private static void ApplyMessageData(Dictionary<string, string> p, SmscMessageData? data)
        {
            if (data is null) return;

            using var doc = JsonSerializer.SerializeToDocument(data, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });

            foreach (var prop in doc.RootElement.EnumerateObject())
            {
                string apiValue = prop.Value.ValueKind switch
                {
                    JsonValueKind.True => "1",
                    JsonValueKind.False => "0",
                    JsonValueKind.Number => prop.Value.GetRawText(),
                    JsonValueKind.String => prop.Value.GetString()!,
                    _ => prop.Value.GetRawText()
                };

                p[prop.Name] = apiValue;
            }
        }

        private static int ResolveFormatEmailFormat(SmscMessageData? data)
        {
            if (data is null) return 8; // default for email

            if (data.Flash == true) return 1;
            if (data.Push == true) return 2;
            if (data.Hlr == true) return 3;
            if (data.Binary == 1) return 4;
            if (data.Binary == 2) return 5;
            if (data.Ping == true) return 6;
            if (data.Mms == true) return 7;
            if (data.Mail == true) return 8;
            if (data.Call == true) return 9;
            if (data.Viber == true) return 10;
            if (data.Social == true) return 11;

            return 0;
        }

        /// <summary>
        /// Sends the API request, retrying against numbered mirror hosts on transient failures.
        /// Primary host: smsc.ru → mirrors: www2.smsc.ru, www3.smsc.ru, …
        /// </summary>
        private async Task<string[]> ExecuteAsync(
            string endpoint,
            Dictionary<string, string> parameters,
            bool usePost,
            string[]? files,
            CancellationToken cancellationToken,
            char delimiter = ',')
        {
            var scheme = _options.UseHttps ? "https" : "http";
            string? lastError = null;

            for (var attempt = 0; attempt < _options.RetryCount; attempt++)
            {
                var host = attempt == 0 ? BaseHost : $"www{attempt + 1}.{BaseHost}";
                var url = $"{scheme}://{host}/sys/{endpoint}";

                try
                {
                    using var request = BuildRequest(url, parameters, files, usePost);
                    using var response = await _httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

#if NET7_0_OR_GREATER
                    var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
                    var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif

                    if (!string.IsNullOrWhiteSpace(body))
                    {
                        return body.Trim().Split(delimiter);
                    }
                }
                catch (Exception ex) when (ex is HttpRequestException ||
                                            ex is TaskCanceledException { CancellationToken.IsCancellationRequested: false })
                {
                    lastError = ex.Message;

                    LogDebug("Attempt {Attempt}/{Total} failed for {Url}: {Error}", attempt + 1, _options.RetryCount, url, lastError);
                }
            }

            LogDebug("All {Total} attempts exhausted. Last error: {Error}", _options.RetryCount, lastError);

            // Minimal dummy response that callers treat as an unrecognised error.
            return ["0", "-0"];
        }

        /// <summary>
        /// Builds the <see cref="HttpRequestMessage"/>: multipart POST with files,
        /// form-urlencoded POST without files, or GET with a query string.
        /// </summary>
        private static HttpRequestMessage BuildRequest(
            string url,
            Dictionary<string, string> parameters,
            string[]? files,
            bool usePost)
        {
            bool hasFiles = files is { Length: > 0 };

            if (hasFiles)
            {
                // Multipart POST: file parts first ("File1", "File2", …), then text params.
                var multipart = new MultipartFormDataContent();

                for (int i = 0; i < files!.Length; i++)
                {
                    var fileContent = new StreamContent(File.OpenRead(files[i]));
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    multipart.Add(fileContent, $"File{i + 1}", Path.GetFileName(files[i]));
                }

                foreach (var keyValue in parameters)
                    multipart.Add(new StringContent(keyValue.Value, Encoding.UTF8), keyValue.Key);

                return new HttpRequestMessage(HttpMethod.Post, url) { Content = multipart };
            }

            if (usePost)
            {
                return new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new FormUrlEncodedContent(parameters)
                };
            }

            // GET — build query string with proper percent-encoding.
            var qs = string.Join("&", parameters.Select(kv => $"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}"));

            return new HttpRequestMessage(HttpMethod.Get, $"{url}?{qs}");
        }

        /// <summary>
        /// Detects the fmt=1 error pattern in a raw response array.
        /// <para>
        /// Errors 1,2,4,5,9 → <c>["0", "-N"]</c><br/>
        /// Errors 3,6,7,8   → <c>["&lt;id&gt;", "-N"]</c>
        /// </para>
        /// </summary>
        private static bool TryParseError(string[] raw, out SmscErrorResult? result)
        {
            if (raw.Length >= 2
                && raw[1].StartsWith("-")
                && int.TryParse(raw[1].Substring(1), out var code))
            {
                result = new SmscErrorResult
                {
                    MessageId = raw[0],
                    ErrorCode = code
                };

                return true;
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Parses the fmt=1 status response for a single message ID.
        /// </summary>
        /// <remarks>
        /// Field layout (fmt=1, all=0, SMS/HLR):
        ///   <c>status, last_timestamp, err [, imsi, msc, mcc, mnc, cn, net, rcn, rnet]</c>
        /// Field layout (fmt=1, all=1, SMS):
        ///   <c>status, last_timestamp, err, send_timestamp, phone, cost, sender,
        ///      status_name, message, comment, type</c>
        /// Field layout (fmt=1, all=2, SMS):
        ///   same as all=1 but country, operator, region inserted after phone.
        /// </remarks>
        private SmscStatusResult ParseSingleStatus(string[] raw)
        {
            if (TryParseError(raw, out var error))
            {
                LogDebug("Status check failed. Error: {Code}.", error!.ErrorCode);

                return SmscStatusResult.Fail(error);
            }

            var statusCode = (SmscMessageStatus)(int.TryParse(raw[0], out int s) ? s : 0);
            var timestamp = raw.Length > 1 && int.TryParse(raw[1], out int t) ? t : 0;
            var errCode = raw.Length > 2 && int.TryParse(raw[2], out int e) ? e : 0;

            DateTimeOffset? lastChanged = timestamp > 0
                ? DateTimeOffset.FromUnixTimeSeconds(timestamp)
                : null;

            LogDebug("Status: {Status}{At}", statusCode,
                lastChanged.HasValue ? $", changed at {lastChanged}" : string.Empty);

            // Indices 0–2 are always status/timestamp/err; everything after is "extra".
            IReadOnlyList<string> extra = raw.Length > 3
                ? raw.Skip(3).ToList()
                : Array.Empty<string>();

            var success = new SmscStatusSuccessResult
            {
                Status = statusCode,
                LastChanged = lastChanged,
                ErrorCode = errCode,
                ExtraInfo = extra
            };

            return SmscStatusResult.Ok(success);
        }

        /// <summary>
        /// Parses the newline-delimited batch status response.
        /// Each row is a comma-separated string that is split independently.
        /// </summary>
        private static SmscStatusResult ParseBatchStatus(string[] rows)
        {
            // A single-row response may still be an error ("0,-2")
            if (rows.Length == 1)
            {
                var parts = rows[0].Split(',');
                if (parts.Length >= 2
                    && parts[1].StartsWith("-")
                    && int.TryParse(parts[1].Substring(1), out int code))
                {
                    var error = new SmscErrorResult
                    {
                        MessageId = parts[0],
                        ErrorCode = code
                    };

                    return SmscStatusResult.Fail(error);
                }
            }

            var parsed = rows
                .Select(row => (IReadOnlyList<string>)row.Split(','))
                .ToArray();

            var success = new SmscBatchStatusSuccessResult
            {
                Rows = parsed
            };

            return SmscStatusResult.OkBatch(success);
        }

        private static bool TryParseDecimal(string s, out decimal value) => decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out value);

        private void LogDebug(string message, params object?[] args)
        {
            if (_options.IsDebug)
            {
                _logger.LogDebug(message, args);
            }
        }

        public void Dispose()
        {
            if (_ownsHttpClient)
            {
                _httpClient.Dispose();
            }
        }
    }
}

# Spoleto.SMS

[![](https://img.shields.io/github/license/spoleto-software/Spoleto.SMS)](https://github.com/spoleto-software/Spoleto.SMS/blob/main/LICENSE)
[![](https://img.shields.io/nuget/v/Spoleto.SMS)](https://www.nuget.org/packages/Spoleto.SMS/)
![Build](https://github.com/spoleto-software/Spoleto.SMS/actions/workflows/ci.yml/badge.svg)

Incorporate SMS functionality into your .NET application using a versatile solution that ensures maintainable architecture and provides access to various delivery providers (e.g., SMSC, GetSms).

https://github.com/spoleto-software/Spoleto.SMS

## Quick setup

Begin by installing the package through the [NuGet](https://www.nuget.org/packages/Spoleto.SMS/) package manager with the command:  
``Install-Package Spoleto.SMS``.

## Getting started

To send an SMS message, you will engage with three key elements:

- **SmsMessage**: This represents the actual content, the recipients and the sender of the SMS that you wish to send;
- **SmsService**: This is the mechanism through which the SMS is dispatched;
- **SmsProvider**: This refers to the SMS delivery provider.

Initially, you create the content of the ``SmsMessage``, set the recipients and the sender.  
Afterward, this message is handed over to the ``SmsService``.  
Finally, the ``SmsService`` dispatches the message through the pre-configured ``SmsProvider``.

### SmsMessage

SmsMessage contains your message details:

- **From:** - the phone number or another ID that will appear as the sender;
- **To:** - the phone numbers of the intended recipients (separated by semicolons ; if several);
- **Body:** - the content of the SMS message;
- **IsAllowSendToForeignNumbers** - a flag indicating whether the message can be sent to international numbers.

Example of SmsMessage:

```csharp
var smsMessage = new SmsMessage("SMS content", "Sender number/ID", "Recipients numbers");
```

### SmsProvider

SmsProvider is the underlying mechanisms that enable the actual transmission of SMS messages. When you incorporate Spoleto.SMS into your application, it's mandatory to install at least one of the available providers for message dispatch capabilities.

The providers come as pre-configured NuGet packages:

- **[Spoleto.SMS.GetSms](https://www.nuget.org/packages/Spoleto.SMS.GetSms/)**: Send SMS messages through GetSms https://getsms.uz/; 
- **[Spoleto.SMS.Smsc](https://www.nuget.org/packages/Spoleto.SMS.Smsc/)**: Send SMS messages through SMSC https://smsc.ru/.


If you wish to add a custom provider, you can do it by implementing the interface ``Spoleto.SMS.Providers.ISmsProvider`` or the abstract class ``Spoleto.SMS.Providers.SmsProviderBase``.

### SmsService

The SmsService acts as the service with which you communicate to dispatch your messages. To instantiate the service, you can use the ``Spoleto.SMS.SmsServiceFactory``, which serves as a factory for creating service instances.

Example of creating ``SmsService`` using ``SmsServiceFactory``:

```csharp
var smsService = new SmsServiceFactory()
    .WithOptions(options =>
    {
        options.DefaultFrom = "Default Sender ID";
        options.DefaultProvider = SmscProvider.ProviderName;
    })
    .AddSmsc("SMSC_LOGIN", "SMSC_PASSWORD")
    .AddGetSms("GetSmsLogin", "GetSmsPassword", "GetSmsServiceUrl")
    .Create();
```

#### SmsService Overview

The factory provides you with three essential methods:

- **WithOptions()**: This method allows you to specify the SMS service settings:
  - **DefaultFrom**: This setting allows you to assign a default sender phone number or another ID, so you do not need to to specify it for each individual message. Note that this default is overridden if a sender phone number is explicitly provided in the SmsMessage.
  - **DefaultProvider**: This is where you specify the default SMS provider for sending SMS messages. Since it's possible to set up multiple SMS providers, it's important to point out which one you prefer for default use.
- **AddProvider()**: With this method, you can add the SMS provider that will be utilized for SMS message transmission.
- **Create()**: This method is used to generate a new instance of the ``SmsService``.


For ``AddProvider()``, the method expects a SMS provider instance, e.g.: ``AddProvider(new SmscProvider(options))``. 
However, direct usage of this method is generally unnecessary since the SMS providers include extension methods for registration. 
For instance, the SMSC provider provides the ``AddSmsc()`` extension method that simplifies its registration.

The ``Create()`` function straightforwardly creates a new ``SmsService`` instance.

It is only necessary to create the SMS service once and thereafter it can be reused throughout your application.

Now you have an instance of the ``SmsService``.  
So, you're ready to begin sending SMS messages:

```csharp
var smsService = new SmsServiceFactory()
    .WithOptions(options =>
    {
        options.DefaultFrom = "Default Sender ID";
        options.DefaultProvider = SmscProvider.ProviderName;
    })
    .AddSmsc("SMSC_LOGIN", "SMSC_PASSWORD")
    .AddGetSms("GetSmsLogin", "GetSmsPassword", "GetSmsServiceUrl")
    .Create();

var smsMessage = new SmsMessage("SMS content", "Sender number/ID", "Recipients numbers");

var result = smsService.Send(smsMessage);

// or async:
var result = await smsService.SendAsync(smsMessage);

// and then you can additionally check a status of SMS delivery:
var status = smsService.GetStatus("id", "phoneNumber");

// or async:
var status = await smsService.GetStatusAsync("id", "phoneNumber");
```

If you want to send message through the particular SMS provider, you need to specify it as the first argument:

```csharp
var result = smsService.Send(SmsProviderName.GetSMS, smsMessage);

// or async:
var result = await smsService.SendAsync(SmsProviderName.GetSMS, smsMessage);
```

## Dependency Injection

To integrate Spoleto.SMS into Microsoft Dependency injection framework, you should utilize the [**Spoleto.SMS.Extensions.Messaging**](https://www.nuget.org/packages/Spoleto.SMS.Extensions.Messaging/) NuGet package. This package provides an extension method for the ``IServiceCollection`` interface, which register the SmsService as a scoped service.

The extentions for SMS providers come as pre-configured NuGet packages:

- **[Spoleto.SMS.Extensions.GetSms](https://www.nuget.org/packages/Spoleto.SMS.Extensions.GetSms/)**: GetSms registration; 
- **[Spoleto.SMS.Extensions.Smsc](https://www.nuget.org/packages/Spoleto.SMS.Extensions.Smsc/)**: SMSC registration.

After ensuring that the ``Spoleto.SMS.Extensions.Messaging`` package with at least one SMS provider package are installed from NuGet, you can proceed with the registration of Spoleto.SMS within the ``Startup.cs`` or your DI configuration file in the following manner:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Other DI registrations...

    // Register Spoleto.SMS as a scoped service:
    services.AddSMS(SmscProvider.ProviderName)
        .AddGetSms(getSmsOptions.Login, getSmsOptions.Password, getSmsOptions.ServiceUrl)
        .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD);
    
    // or:
    services.AddSMS(options =>
    {
        options.DefaultFrom = "Default Sender ID";
        options.DefaultProvider = SmscProvider.ProviderName;
    })
    .AddGetSms(getSmsOptions.Login, getSmsOptions.Password, getSmsOptions.ServiceUrl)
    .AddSmsc(smscOptions.SMSC_LOGIN, smscOptions.SMSC_PASSWORD);

    // Continue with the rest of your service configuration...
}

```

#### Injecting the SMS Service into Your Classes
Once Spoleto.SMS has been registered with your Dependency Injection framework, you can facilitate the injection of the SMS service into any class within your application.

Inject the ``ISmsService`` interface into the constructors of the classes where you want to use SMS functionality:
```csharp
public class YourSmsSender
{
    private readonly ILogger<YourSmsSender> _logger;
    private readonly ISmsService _smsService;

    public YourSmsSender(ILogger<YourSmsSender> logger, ISmsService smsService)
    {
        _logger = logger;
        _smsService = smsService;
    }

    public void Send(string from, string to, string content)
    {
        // create a SmsMessage
        var smsMessage = new SmsMessage("SMS content", "Sender number/ID", "Recipients numbers");

        // send the SmsMessage using the default SMS provider:
        var result = await _smsService.SendAsync(message);

        // or send the SmsMessage using the specified SMS provider:
        var result = await _smsService.SendAsync(SmsProviderName.GetSMS, message);

        // log the result:
        _logger.LogInformation("Sent to {to} with result: {result}", message.To, result.IsSuccess);
    }
}
```
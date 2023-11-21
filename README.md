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

- **From:** The phone number or another ID that will appear as the sender;
- **To:** The phone numbers of the intended recipients (separated by semicolons ; if several);
- **Body:** The content of the SMS message;
- **IsAllowSendToForeignNumbers:** A flag indicating whether the message can be sent to international numbers.

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
    .AddGetSms("GetSmsLogin", "GetSmsPassword")
    .Build();
```

#### SmsService Overview

The factory provides you with three essential methods:

- **WithOptions()**: This method allows you to specify the SMS service settings:
  - **DefaultFrom**: This setting allows you to assign a default sender phone number or another ID, so you do not need to to specify it for each individual message. Note that this default is overridden if a sender phone number is explicitly provided in the SmsMessage.
  - **DefaultProvider**: This is where you specify the default SMS provider for sending SMS messages. Since it's possible to set up multiple SMS providers, it's important to point out which one you prefer for default use.
- **AddProvider()**: With this method, you can add the SMS provider that will be utilized for SMS message transmission.
- **Build()**: This method is used to generate a new instance of the ``SmsService``.


For ``AddProvider()``, the method expects a SMS provider instance, e.g.: ``AddProvider(new SmscProvider(options))``. 
However, direct usage of this method is generally unnecessary since the SMS providers include extension methods for registration. 
For instance, the SMSC provider provides the ``AddSmsc()`` extension method that simplifies its registration.

The ``Build()`` function straightforwardly creates a new ``SmsService`` instance.

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
    .AddGetSms("GetSmsLogin", "GetSmsPassword")
    .Build();

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
        .AddSmsc("SMSC_LOGIN", "SMSC_PASSWORD")
        .AddGetSms("GetSmsLogin", "GetSmsPassword");
    
    // or:
    services.AddSMS(options =>
    {
        options.DefaultFrom = "Default Sender ID";
        options.DefaultProvider = SmscProvider.ProviderName;
    })
    .AddSmsc("SMSC_LOGIN", "SMSC_PASSWORD")
    .AddGetSms("GetSmsLogin", "GetSmsPassword");

    // Continue with the rest of your service configuration...
}

```

### Injecting the SMS Service into Your Classes
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
        var smsMessage = new SmsMessage(content, from, to);

        // send the SmsMessage using the default SMS provider:
        var result = await _smsService.SendAsync(message);

        // or send the SmsMessage using the specified SMS provider:
        var result = await _smsService.SendAsync(SmsProviderName.GetSMS, message);

        // log the result:
        _logger.LogInformation("Sent to {to} with result: {result}", message.To, result.IsSuccess);
    }
}
```

## SmsService extensions

There are several extensions for ``ISmsService`` that can help you to send messages.

### GetProviderForPhoneNumber Method

```csharp
ISmsProvider? GetProviderForPhoneNumber(this ISmsService smsService, string phoneNumber, bool returnDefaultIfNotFound = true, bool isAllowSendToForeignNumbers = false);
```

**Description:**  
This extension method is designed for selecting a suitable SMS provider based on the provided phone number.

**Parameters:**  
- ``smsService``: The instance of the ``ISmsService`` which this method extends.  
- ``phoneNumber``: The target phone number for which an SMS provider needs to be picked. Must be provided as a non-null and non-empty string.

- ``returnDefaultIfNotFound`` (optional): A boolean flag indicating whether to return the default provider in case none is found specifically for the given phone number. Defaults to true.

- ``isAllowSendToForeignNumbers`` (optional): A boolean flag indicating whether the message can be sent to international numbers. Defaults to false.

**Returns:**  
An instance of ``ISmsProvider`` that is suitable for the provided phone number. If no suitable provider is found and ``returnDefaultIfNotFound`` is set to true, the default provider will be returned. If set to false, the method returns null.

**Exceptions:**  
- ``ArgumentNullException``: If phoneNumber is null or an empty string.

**Usage Example:**

```csharp
// Assume smsService is an instance of ISmsService:
var uzbekSmsProvider = smsService.GetProviderForPhoneNumber("+998111111111");
```

This code returns a suitable SMS provider or the default provider, if no suitable provider is found for the provided phone number.

### SendUsingSuitableProvider Method

```csharp
void SendUsingSuitableProvider(this ISmsService smsService, SmsMessage message, bool sendUsingDefaultIfNotFound = true);
```

**Description:**  
This extension method makes easier the sending of an SMS message using a suitable provider that is selected based on the phone number.

**Parameters:**  
- ``smsService``: The instance of the ``ISmsService`` which this method extends.

- ``message``: An instance of SmsMessage that holds all necessary data for the SMS to be sent, such as the text content, the recipients and the sender.

- ``sendUsingDefaultIfNotFound`` (optional): Specifies whether to send the message using the default provider if no suitable provider is found for the provided phone number. Defaults to true.

**Returns:**  
This method does not return a value, indicating a void return type.

**Exceptions:**  
- ``ArgumentException``: If no suitable SMS provider is found for the provided phone number and ``sendUsingDefaultIfNotFound`` is set to false, an ``ArgumentException`` is thrown with a message indicating the inability to find a suitable provider.

**Usage Example:**

```csharp
// Assume smsService is an instance of ISmsService and message is an instance of SmsMessage:
smsService.SendUsingSuitableProvider("+71111111111", message);
```

This code sends the SMS using a suitable provider that is selected based on the phone number with the flag ``sendUsingDefaultIfNotFound`` is set to its default value, true, which could be omitted in the method call.

### SendUsingSuitableProviderAsync Method

**Description:**  
Asynchronous version for the method [SendUsingSuitableProvider](#sendusingsuitableprovider-method).  
This method sends messages asynchronously.
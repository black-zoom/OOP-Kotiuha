using System;
using System.Collections.Generic;

abstract class NotificationService
{
    public string Recipient { get; set; }
    public string Message { get; set; }
    public bool IsSent { get; private set; }

    protected NotificationService(string recipient, string message)
    {
        Recipient = recipient;
        Message = message;
    }

    public void Notify()
    {
        ValidateRecipient();
        FormatMessage();
        Send();
        LogDelivery();
    }

    protected abstract void ValidateRecipient();

    protected virtual void FormatMessage()
    {
        Message = Message.Trim();
    }

    protected abstract void Send();

    protected virtual void LogDelivery()
    {
        IsSent = true;
        Console.WriteLine("Delivery logged");
    }
}

class EmailNotification : NotificationService
{
    public string Subject { get; set; }

    public EmailNotification(string recipient, string message, string subject)
        : base(recipient, message)
    {
        Subject = subject;
    }

    protected override void ValidateRecipient()
    {
        if (!Recipient.Contains("@"))
            throw new ArgumentException("Invalid email address");

        Console.WriteLine("Email recipient validated");
    }

    protected override void FormatMessage()
    {
        base.FormatMessage();
        Message = $"Subject: {Subject}\n{Message}";
        Console.WriteLine("Email message formatted");
    }

    protected override void Send()
    {
        Console.WriteLine($"Sending email to {Recipient}");
    }
}

class SMSNotification : NotificationService
{
    public SMSNotification(string recipient, string message)
        : base(recipient, message) { }

    protected override void ValidateRecipient()
    {
        if (string.IsNullOrWhiteSpace(Recipient))
            throw new ArgumentException("Invalid phone number");

        Console.WriteLine("Phone number validated");
    }

    protected override void Send()
    {
        Console.WriteLine($"Sending SMS to {Recipient}");
    }
}

class PushNotification : NotificationService
{
    public string DeviceToken { get; set; }

    public PushNotification(string recipient, string message, string deviceToken)
        : base(recipient, message)
    {
        DeviceToken = deviceToken;
    }

    protected override void ValidateRecipient()
    {
        if (string.IsNullOrWhiteSpace(DeviceToken))
            throw new ArgumentException("Invalid device token");

        Console.WriteLine("Device token validated");
    }

    protected override void Send()
    {
        Console.WriteLine($"Sending push notification to {Recipient}");
    }

    protected override void LogDelivery()
    {
        base.LogDelivery();
        Console.WriteLine($"Push notification sent: {IsSent}");
    }
}

class Program
{
    static void Main()
    {
        List<NotificationService> notifications = new()
        {
            new EmailNotification("user@example.com", "Hello!", "Greeting"),
            new SMSNotification("+380991234567", "Your code is 1234"),
            new PushNotification("User123", "New message", "device-token-123")
        };

        foreach (NotificationService notification in notifications)
        {
            notification.Notify();
            Console.WriteLine();
        }
    }
}

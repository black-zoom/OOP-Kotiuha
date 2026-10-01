using System;
using System.Collections.Generic;
using System.Linq;

class Payment
{
    public decimal Amount { get; }

    public Payment(decimal amount)
    {
        Amount = amount;
    }

    public virtual void Process()
    {
        Console.WriteLine($"Обробка платежу: {Amount:F2} грн");
    }
}

class CreditCardPayment : Payment
{
    public string CardNumber { get; }

    public CreditCardPayment(decimal amount, string cardNumber)
        : base(amount)
    {
        CardNumber = cardNumber;
    }

    public override void Process()
    {
        Console.WriteLine($"Оплата карткою: {Amount:F2} грн");
    }
}

class CashPayment : Payment
{
    public string Currency { get; }

    public CashPayment(decimal amount, string currency)
        : base(amount)
    {
        Currency = currency;
    }

    public override void Process()
    {
        Console.WriteLine($"Готівковий платіж: {Amount:F2} {Currency}");
    }
}

class OnlinePayment : Payment
{
    public string Gateway { get; }

    public OnlinePayment(decimal amount, string gateway)
        : base(amount)
    {
        Gateway = gateway;
    }

    public override void Process()
    {
        Console.WriteLine($"Онлайн-платіж через {Gateway}: {Amount:F2} грн");
    }
}

class Program
{
    static void Main()
    {
        List<Payment> payments = new List<Payment>
        {
            new CreditCardPayment(1250.50m, "5168742398761234"),
            new CashPayment(800.00m, "UAH"),
            new OnlinePayment(2300.75m, "WayForPay"),
            new CreditCardPayment(450.25m, "4321567890123456"),
            new OnlinePayment(999.99m, "Portmone")
        };

        foreach (Payment payment in payments)
        {
            payment.Process();
        }

        decimal totalAmount = payments.Sum(payment => payment.Amount);

        Console.WriteLine($"Загальна сума платежів: {totalAmount:F2} грн");
    }
}


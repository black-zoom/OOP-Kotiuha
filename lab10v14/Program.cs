using System;
using System.Collections.Generic;
using System.IO;

interface IObserver<T>
{
    void Update(T value);
}

class ConsoleObserver<T> : IObserver<T>
{
    public void Update(T value) => Console.WriteLine(value);
}

class FileObserver<T> : IObserver<T>
{
    public void Update(T value) =>
        File.AppendAllText("events.txt", $"{value}{Environment.NewLine}");
}

abstract class EventPublisher
{
    protected List<IObserver<string>> observers = new();

    public abstract void Subscribe(IObserver<string> observer);
    public abstract void Unsubscribe(IObserver<string> observer);

    public void NotifySubscribers(string message)
    {
        foreach (var observer in observers)
            observer.Update(message);
    }
}

class StockPricePublisher : EventPublisher
{
    public override void Subscribe(IObserver<string> observer) {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public override void Unsubscribe(IObserver<string> observer) =>
        observers.Remove(observer);
}

class WeatherUpdatePublisher : EventPublisher
{
    public override void Subscribe(IObserver<string> observer) =>
        observers.Add(observer);

    public override void Unsubscribe(IObserver<string> observer) =>
        observers.Remove(observer);
}

class Program
{
    static void Main()
    {
        List<IObserver<int>> numberObservers =
        [
            new ConsoleObserver<int>(),
            new FileObserver<int>()
        ];

        foreach (var observer in numberObservers)
            observer.Update(42);

        IObserver<string> console = new ConsoleObserver<string>();
        IObserver<string> file = new FileObserver<string>();

        List<EventPublisher> publishers =
        [
            new StockPricePublisher(),
            new WeatherUpdatePublisher()
        ];

        string[] messages =
        [
            "Змінилася ціна акції",
            "Оновлено прогноз погоди"
        ];

        for (int i = 0; i < publishers.Count; i++)
        {
            publishers[i].Subscribe(console);
            publishers[i].Subscribe(console);
            publishers[i].Subscribe(file);
            publishers[i].NotifySubscribers(messages[i]);
        }
    }
}


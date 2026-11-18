using System;
using System.Threading;

public class RetryPolicy
{
    private int _maxRetries;
    private TimeSpan _initialDelay;
    private double _jitterFactor;
    private Random _random;

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
        _random = new Random();
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        int retryAttempt = 0;
        
        while (true)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    throw;
                }

                retryAttempt++;
                
                if (retryAttempt > _maxRetries)
                {
                    throw;
                }

                double factor = Math.Pow(2, retryAttempt) * (1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor);
                TimeSpan delay = TimeSpan.FromMilliseconds(_initialDelay.TotalMilliseconds * factor);

                Console.WriteLine($"Спроба {retryAttempt}/{_maxRetries}. Затримка: {delay.TotalMilliseconds:F0} мс");
                Thread.Sleep(delay);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var retryPolicy = new RetryPolicy(4, TimeSpan.FromMilliseconds(100), 0.2);

        Console.WriteLine("Сценарій 1:");
        int attempt1 = 0;
        try
        {
            string result1 = retryPolicy.Execute(() =>
            {
                attempt1++;
                if (attempt1 < 3) throw new InvalidOperationException("Connection lost");
                return "Success Data";
            });
            Console.WriteLine(result1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("\nСценарій 2:");
        try
        {
            retryPolicy.Execute<int>(() =>
            {
                throw new TimeoutException("Database timeout");
            }, e => e is TimeoutException);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }
}

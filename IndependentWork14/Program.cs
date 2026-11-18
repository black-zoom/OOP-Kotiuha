using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

class Program
{
    private static int _dbAttempts = 0;
    private static int _paymentAttempts = 0;

    static async Task Main()
    {
        RunDatabaseScenario();
        Console.WriteLine(new string('-', 40));
        
        RunPaymentScenario();
        Console.WriteLine(new string('-', 40));
        
        await RunReportScenarioAsync();
    }

    static void RunDatabaseScenario()
    {
        Console.WriteLine("--- Scenario 1: Database Connection (Retry) ---");

        var retryPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<InvalidOperationException>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Constant,
                OnRetry = args =>
                {
                    Console.WriteLine($"[Polly DB] Retry #{args.AttemptNumber + 1} after {args.RetryDelay.TotalSeconds}s due to: {args.Outcome.Exception?.Message}");
                    return default;
                }
            })
            .Build();

        try
        {
            retryPipeline.Execute(() => ConnectToDatabase());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[App DB] Operation failed permanently: {ex.Message}");
        }
    }

    static void ConnectToDatabase()
    {
        _dbAttempts++;
        Console.WriteLine($"[DB] Attempt {_dbAttempts}: Connecting to database...");
        if (_dbAttempts <= 2)
        {
            throw new InvalidOperationException("Connection pool is full.");
        }
        Console.WriteLine("[DB] Successfully connected to the database!");
    }

    static void RunPaymentScenario()
    {
        Console.WriteLine("--- Scenario 2: Payment Gateway (Circuit Breaker) ---");

        var cbPipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                FailureRatio = 0.5,
                SamplingDuration = TimeSpan.FromSeconds(10),
                MinimumThroughput = 2,
                BreakDuration = TimeSpan.FromSeconds(3),
                OnOpened = args =>
                {
                    Console.WriteLine("[Polly Payment] Circuit Opened! Halting requests.");
                    return default;
                },
                OnClosed = args =>
                {
                    Console.WriteLine("[Polly Payment] Circuit Closed! Resuming operation.");
                    return default;
                },
                OnHalfOpened = args =>
                {
                    Console.WriteLine("[Polly Payment] Circuit Half-Opened! Testing connection.");
                    return default;
                }
            })
            .Build();

        for (int i = 1; i <= 5; i++)
        {
            try
            {
                cbPipeline.Execute(() => ProcessPayment());
            }
            catch (BrokenCircuitException)
            {
                Console.WriteLine($"[App Payment] Call {i} rejected immediately by Circuit Breaker.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[App Payment] Call {i} failed: {ex.Message}");
            }
            Thread.Sleep(1000);
        }
    }

    static void ProcessPayment()
    {
        _paymentAttempts++;
        Console.WriteLine($"[Payment] Processing attempt {_paymentAttempts}...");
        throw new Exception("Gateway timeout (504).");
    }

    static async Task RunReportScenarioAsync()
    {
        Console.WriteLine("--- Scenario 3: Long-running Report (Timeout) ---");

        var timeoutPipeline = new ResiliencePipelineBuilder()
            .AddTimeout(TimeSpan.FromSeconds(2))
            .Build();

        try
        {
            await timeoutPipeline.ExecuteAsync(async ct =>
            {
                Console.WriteLine("[Report] Starting complex data aggregation (needs 5s)...");
                await Task.Delay(5000, ct); 
                Console.WriteLine("[Report] Aggregation completed!");
            });
        }
        catch (TimeoutRejectedException)
        {
            Console.WriteLine("[App Report] Operation timed out and was cancelled to free thread resources.");
        }
    }
}

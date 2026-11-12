using System;
using System.IO;
using System.Net.Http;
using System.Threading;

class FileProcessor
{
    private int attempts = 0;

    public void UpdateUserProfile(string path, string profileJson)
    {
        attempts++;

        if (attempts <= 3)
            throw new FileNotFoundException("Файл не знайдено.");

        File.WriteAllText(path, profileJson);
    }
}

class NetworkClient
{
    private int attempts = 0;

    public bool PostUserProfile(string url, string profileJson)
    {
        attempts++;

        if (attempts <= 2)
            throw new HttpRequestException("Помилка мережевого запиту.");

        return true;
    }
}

static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        if (retryCount < 0)
            throw new ArgumentOutOfRangeException(nameof(retryCount));

        if (initialDelay == default)
            initialDelay = TimeSpan.FromMilliseconds(500);

        int attempt = 0;

        while (true)
        {
            try
            {
                Console.WriteLine($"Спроба {attempt + 1}");
                return operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");

                if (attempt >= retryCount ||
                    (shouldRetry != null && !shouldRetry(ex)))
                    throw;

                Thread.Sleep(initialDelay * (int)Math.Pow(2, attempt));
                attempt++;
            }
        }
    }
}

class Program
{
    static void Main()
    {
        FileProcessor fileProcessor = new FileProcessor();
        NetworkClient networkClient = new NetworkClient();

        string path = "userProfile.json";
        string profileJson = "{\"name\":\"Student\",\"age\":20}";
        string url = "https://example.com/api/profile";

        Func<Exception, bool> shouldRetry = ex =>
            ex is FileNotFoundException ||
            ex is HttpRequestException;

        try
        {
            RetryHelper.ExecuteWithRetry(
                () =>
                {
                    fileProcessor.UpdateUserProfile(path, profileJson);
                    return true;
                },
                3,
                TimeSpan.FromMilliseconds(500),
                shouldRetry
            );

            Console.WriteLine("Профіль успішно оновлено.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            bool result = RetryHelper.ExecuteWithRetry(
                () => networkClient.PostUserProfile(url, profileJson),
                3,
                TimeSpan.FromMilliseconds(500),
                shouldRetry
            );

            Console.WriteLine(result
                ? "Профіль успішно відправлено."
                : "Помилка відправлення.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

using System;

class HttpClient : IDisposable
{
    private string _baseUrl;
    private bool _isConnected;
    private bool _disposed;

    public string BaseUrl
    {
        get { return _baseUrl; }
        set { _baseUrl = value; }
    }

    public bool IsConnected
    {
        get { return _isConnected; }
    }

    public HttpClient(string baseUrl)
    {
        BaseUrl = baseUrl;
        _isConnected = true;
        _disposed = false;

        Console.WriteLine($"Підключено до: {_baseUrl}");
    }

    public void Get(string endpoint)
    {
        if (_disposed)
        {
            Console.WriteLine("Помилка: HttpClient вже звільнено.");
            return;
        }

        if (_isConnected)
        {
            Console.WriteLine($"GET-запит: {_baseUrl}{endpoint}");
        }
        else
        {
            Console.WriteLine("З'єднання закрито.");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Звільнення керованих ресурсів.");
            }

            if (_isConnected)
            {
                Console.WriteLine($"Закриття з'єднання з: {_baseUrl}");
                _isConnected = false;
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~HttpClient()
    {
        Dispose(false);
    }
}

class Program
{
    static void Main()
    {
        using (HttpClient client = new HttpClient("https://example.com"))
        {
            client.Get("/users");
        }

        HttpClient client2 = new HttpClient("https://google.com");
        client2.Get("/search");
        client2.Dispose();

        CreateClient();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Збирач сміття завершив роботу.");
    }

    static void CreateClient()
    {
        HttpClient client3 = new HttpClient("https://github.com");
        client3.Get("/repositories");
    }
}


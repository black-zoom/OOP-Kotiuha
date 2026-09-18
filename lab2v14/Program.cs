using System;

class Restaurant
{
    private string _name;
    private string _cuisine;
    private double _rating;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Cuisine
    {
        get { return _cuisine; }
        set { _cuisine = value; }
    }

    public double Rating
    {
        get { return _rating; }
        set
        {
            if (value < 1 || value > 5)
            {
                throw new ArgumentException("Рейтинг повинен бути від 1 до 5.");
            }

            _rating = value;
        }
    }

    public Restaurant()
        : this("Unnamed", "Mixed", 3.0)
    {
        Console.WriteLine("Викликано конструктор за замовчуванням.");
    }

    public Restaurant(string name, string cuisine, double rating)
    {
        Name = name;
        Cuisine = cuisine;
        Rating = rating;

        Console.WriteLine($"Створено ресторан: {Name}");
    }

    public void ServeDish(string dishName)
    {
        Console.WriteLine($"Ресторан \"{Name}\" подає страву: {dishName}.");
    }

    ~Restaurant()
    {
        Console.WriteLine($"Фіналізатор: об'єкт ресторану \"{Name}\" знищується.");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Restaurant restaurant1 = new Restaurant();
        Restaurant restaurant2 = new Restaurant("Bella Italia", "Italian", 4.8);
        Restaurant restaurant3 = new Restaurant("Sakura", "Japanese", 4.5);

        restaurant1.ServeDish("Паста");
        restaurant2.ServeDish("Піца Маргарита");
        restaurant3.ServeDish("Суші");

        Console.WriteLine($"Ресторан 1: {restaurant1.Name}, кухня: {restaurant1.Cuisine}, рейтинг: {restaurant1.Rating}");
        Console.WriteLine($"Ресторан 2: {restaurant2.Name}, кухня: {restaurant2.Cuisine}, рейтинг: {restaurant2.Rating}");
        Console.WriteLine($"Ресторан 3: {restaurant3.Name}, кухня: {restaurant3.Cuisine}, рейтинг: {restaurant3.Rating}");

        try
        {
            restaurant1.Rating = 6;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }

        restaurant1 = null;
        restaurant2 = null;
        restaurant3 = null;

        Console.WriteLine("Запускаємо збирач сміття...");

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Збирач сміття завершив роботу.");
    }
}


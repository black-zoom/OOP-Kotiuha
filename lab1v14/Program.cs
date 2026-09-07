using System;

class Restaurant
{
    private string name;
    private string cuisine;
    private double rating;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Cuisine
    {
        get { return cuisine; }
        set { cuisine = value; }
    }

    public double Rating
    {
        get { return rating; }
        set { rating = value; }
    }

    public Restaurant(string name, string cuisine, double rating)
    {
        this.name = name;
        this.cuisine = cuisine;
        this.rating = rating;
    }

    public void ServeDish()
    {
        Console.WriteLine($"Ресторан \"{Name}\" подає страви {Cuisine} кухні. Рейтинг: {Rating}/5");
    }
}

class Program
{
    static void Main()
    {
        Restaurant restaurant1 = new Restaurant("Артем", "української", 1);
        Restaurant restaurant2 = new Restaurant("Петро", "японської", 2);
        Restaurant restaurant3 = new Restaurant("Вадим", "італійської", 3);

        restaurant1.ServeDish();
        restaurant2.ServeDish();
        restaurant3.ServeDish();
    }
}

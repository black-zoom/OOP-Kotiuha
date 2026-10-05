public class Car
{
    private string _brand;
    private int _year;
    private double _speed;

    public string Brand
    {
        get { return _brand; }
        set { _brand = value; }
    }

    public int Year
    {
        get { return _year; }
    }

    public Car(string brand, int year, double speed)
    {
        _brand = brand;
        _year = year;
        _speed = speed;
    }

    public bool IsSpeedLimitExceeded(double speedLimit)
    {
        return _speed > speedLimit;
    }
}


public class BankAccount
{
    private string _owner;
    private double _balance;
    private string _accountNumber;

    public string Owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public double Balance
    {
        get { return _balance; }
    }

    public BankAccount(string owner, double balance, string accountNumber)
    {
        _owner = owner;
        _balance = balance;
        _accountNumber = accountNumber;
    }

    public void Deposit(double amount)
    {
        if (amount > 0)
        {
            _balance += amount;
        }
    }
}


public class Book
{
    private string _title;
    private string _author;
    private int _pages;

    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    public string Author
    {
        get { return _author; }
        set { _author = value; }
    }

    public Book(string title, string author, int pages)
    {
        _title = title;
        _author = author;
        _pages = pages;
    }

    public bool IsLongBook()
    {
        return _pages >= 300;
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        Car car = new Car("Toyota", 2020, 110);

        Console.WriteLine($"Марка автомобіля: {car.Brand}");
        Console.WriteLine($"Рік випуску: {car.Year}");
        Console.WriteLine($"Перевищення швидкості: {car.IsSpeedLimitExceeded(90)}");

        BankAccount account = new BankAccount("Іван Петренко", 5000, "UA123456789");

        Console.WriteLine($"Власник рахунку: {account.Owner}");
        Console.WriteLine($"Баланс: {account.Balance} грн");

        account.Deposit(1500);

        Console.WriteLine($"Баланс після поповнення: {account.Balance} грн");

        Book book = new Book("Кобзар", "Тарас Шевченко", 352);

        Console.WriteLine($"Назва книги: {book.Title}");
        Console.WriteLine($"Автор: {book.Author}");
        Console.WriteLine($"Книга має 300 або більше сторінок: {book.IsLongBook()}");
    }
}


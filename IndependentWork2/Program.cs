public class Product
{
    private int _id;
    private string _name;
    private decimal _price;
    private string _category;
    private int _stockCount;

    public int Id
    {
        get { return _id; }
    }

    public string Name
    {
        get { return _name; }
    }

    public decimal Price
    {
        get { return _price; }
    }

    public string Category
    {
        get { return _category; }
    }

    public int StockCount
    {
        get { return _stockCount; }
    }

    public Product(int id, string name, decimal price, string category, int stockCount)
    {
        _id = id;
        _name = name;
        _price = price;
        _category = category;
        _stockCount = stockCount;
    }

    public Product(int id, string name, decimal price)
        : this(id, name, price, "Uncategorized", 0)
    {
    }

    public Product(Product product)
        : this(
            product.Id,
            product.Name,
            product.Price,
            product.Category,
            product.StockCount)
    {
    }

    public override string ToString()
    {
        return $"ID: {Id}, Name: {Name}, Price: {Price:C}, Category: {Category}, Stock: {StockCount}";
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Product product1 = new Product(101, "Laptop", 35000m, "Electronics", 15);
        Product product2 = new Product(102, "Mouse", 800m);
        Product product3 = new Product(product1);

        Console.WriteLine($"Товар 1 (основний конструктор): {product1}");
        Console.WriteLine($"Товар 2 (скорочений конструктор): {product2}");
        Console.WriteLine($"Товар 3 (конструктор копіювання): {product3}");
    }
}


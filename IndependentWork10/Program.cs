using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

        var squares = CollectionUtils.Map(numbers, x => x * x);
        Console.WriteLine("Squares: " + string.Join(", ", squares));

        var maxNumber = CollectionUtils.MaxBy(numbers, x => x);
        Console.WriteLine("Maximum: " + maxNumber);

        var evenNumbers = CollectionUtils.Filter(numbers, x => x % 2 == 0);
        Console.WriteLine("Even numbers: " + string.Join(", ", evenNumbers));

        CollectionUtils.ForEach(numbers, x => Console.Write(x + " "));
        Console.WriteLine();

        List<string> words = new List<string>
        {
            "Apple", "Pear", "Banana"
        };

        var lengths = CollectionUtils.Map(words, x => x.Length);
        Console.WriteLine("Lengths: " + string.Join(", ", lengths));

        var longestWord = CollectionUtils.MaxBy(words, x => x.Length);
        Console.WriteLine("Longest word: " + longestWord);

        var longWords = CollectionUtils.Filter(words, x => x.Length > 4);
        Console.WriteLine("Long words: " + string.Join(", ", longWords));

        CollectionUtils.ForEach(words, x => Console.WriteLine(x));

        List<Product> products = new List<Product>
        {
            new Product { Name = "Laptop", Price = 25000 },
            new Product { Name = "Mouse", Price = 500 },
            new Product { Name = "Keyboard", Price = 1500 }
        };

        var names = CollectionUtils.Map(products, p => p.Name);
        Console.WriteLine("Names: " + string.Join(", ", names));

        var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Most expensive: {mostExpensive.Name} - {mostExpensive.Price}");

        var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
        CollectionUtils.ForEach(expensive, p => Console.WriteLine(p.Name));

        CollectionUtils.ForEach(products, p =>
            Console.WriteLine($"{p.Name}: {p.Price} грн"));
    }
}

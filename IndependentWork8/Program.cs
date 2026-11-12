using System;

class Program
{
    static void Main(string[] args)
    {
        Cache<string, string> cache = new Cache<string, string>(3);

        cache.Add("1", "Apple");
        cache.Add("2", "Banana");
        cache.Add("3", "Orange");

        Console.WriteLine(cache.Get("1"));

        cache.Add("4", "Grape");

        Console.WriteLine(cache.ContainsKey("1"));
        Console.WriteLine(cache.Get("4"));
        Console.WriteLine(cache.Count);
    }
}

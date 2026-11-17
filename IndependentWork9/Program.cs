using System;

class Program
{
    static void Main(string[] args)
    {
        var cache = new FileMetadataCache();

        cache.Add(new FileMetadata
        {
            FileName = "document.pdf",
            Size = 2048,
            LastModified = DateTime.Now,
            Hash = "A1B2C3"
        });

        Console.WriteLine(cache.Contains("document.pdf"));
        Console.WriteLine(cache.Get("document.pdf")?.Size);

        var tags = new TagManager();

        tags.AddTag("1", "C#");
        tags.AddTag("1", ".NET");
        tags.AddTag("1", "Programming");

        tags.AddTag("2", "C#");
        tags.AddTag("2", "Java");
        tags.AddTag("2", "Programming");

        Console.WriteLine(string.Join(", ", tags.GetAllUniqueTags()));
        Console.WriteLine(string.Join(", ", tags.FindCommonTags("1", "2")));

        var difference = tags.GetAllUniqueTags();
        difference.ExceptWith(tags.FindCommonTags("1", "2"));

        Console.WriteLine(string.Join(", ", difference));
        
        var index = new SearchIndex();

        index.AddDocument(1, "C# is a programming language");
        index.AddDocument(2, "Java is a programming language");
        index.AddDocument(3, "C# and .NET programming");

        var result1 = index.Search("C#");
        var result2 = index.SearchMultiple("C#", "programming");

        Console.WriteLine("SearchIndex:");
        Console.WriteLine(string.Join(", ", result1));
        Console.WriteLine(string.Join(", ", result2));
    }
}

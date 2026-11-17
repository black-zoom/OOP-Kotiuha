using System;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var documents = new List<Document>
        {
            new Document
            {
                Id = 1,
                Title = "Introduction to C#",
                Content = "C# is a modern object-oriented programming language developed by Microsoft."
            },
            new Document
            {
                Id = 2,
                Title = "Java Basics",
                Content = "Java is a popular programming language known for its portability and performance."
            },
            new Document
            {
                Id = 3,
                Title = "Python Overview",
                Content = "Python is a high-level programming language with simple syntax."
            },
            new Document
            {
                Id = 4,
                Title = "C# Advanced Features",
                Content = "C# supports LINQ, async/await, and many modern programming paradigms."
            },
            new Document
            {
                Id = 5,
                Title = "Comparing Languages",
                Content = "C#, Java, and Python are among the most popular programming languages today."
            }
        };

        var index = new InvertedIndex();

        foreach (var doc in documents)
            index.AddDocument(doc);

        Console.WriteLine($"Loaded {index.DocumentCount} documents");
        Console.WriteLine($"Index contains {index.UniqueWords} unique words");

        Console.WriteLine("\n| Performance Tests |");

        int[] sizes = { 10, 100, 1000, 10000 };

        foreach (int size in sizes)
        {
            var testDocuments = PerformanceTest.GenerateDocuments(size);
            PerformanceTest.CompareSearchMethods(testDocuments, "programming");
        }

        Console.WriteLine("| Interactive Search |");

        while (true)
        {
            Console.Write("\nEnter search query (or 'exit'): ");

            string query = Console.ReadLine() ?? "";

            if (query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
                break;

            string[] words = query.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

            List<Document> results;

            if (words.Length > 0 &&
                words[0].Equals("OR", StringComparison.OrdinalIgnoreCase))
            {
                results = index.SearchAnyWord(words.Skip(1).ToArray());
            }
            else
            {
                if (words.Length > 0 &&
                    words[0].Equals("AND", StringComparison.OrdinalIgnoreCase))
                {
                    words = words.Skip(1).ToArray();
                }

                results = index.SearchMultipleWords(words);
            }

            Console.WriteLine("\n| Search Results |");

            foreach (var doc in results)
            {
                Console.WriteLine(doc);

                string content = doc.Content.Substring(
                    0,
                    Math.Min(100, doc.Content.Length));

                Console.WriteLine($" {content}...");
            }

            Console.WriteLine($"\nFound {results.Count} documents");
        }
    }
}

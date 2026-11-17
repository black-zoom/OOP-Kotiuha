using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PerformanceTest
{
    public static void CompareSearchMethods(
        List<Document> documents,
        string searchWord)
    {
        var index = new InvertedIndex();

        foreach (var doc in documents)
            index.AddDocument(doc);

        var normalizedWord = TextProcessor.RemoveStopWords(
            TextProcessor.Tokenize(searchWord)).FirstOrDefault();

        if (normalizedWord == null)
            return;

        const int iterations = 100;

        List<Document> linearResults = new List<Document>();
        List<Document> indexResults = new List<Document>();

        var sw = new Stopwatch();

        sw.Start();

        for (int i = 0; i < iterations; i++)
        {
            linearResults = documents.Where(d =>
                TextProcessor.RemoveStopWords(
                    TextProcessor.Tokenize(d.Title + " " + d.Content))
                    .Contains(normalizedWord)).ToList();
        }

        sw.Stop();
        long linearTicks = sw.ElapsedTicks / iterations;

        sw.Restart();

        for (int i = 0; i < iterations; i++)
        {
            indexResults = index.SearchSingleWord(normalizedWord);
        }

        sw.Stop();
        long indexTicks = sw.ElapsedTicks / iterations;

        Console.WriteLine($"Documents: {documents.Count}");
        Console.WriteLine($"Linear search: {linearTicks} ticks");
        Console.WriteLine($"Inverted Index: {indexTicks} ticks");
        Console.WriteLine($"Linear found: {linearResults.Count}");
        Console.WriteLine($"Index found: {indexResults.Count}");
        Console.WriteLine(
            $"Results match: {linearResults.Count == indexResults.Count}");
        Console.WriteLine();
    }

    public static List<Document> GenerateDocuments(int count)
    {
        string[] titles =
        {
            "Introduction to C#",
            "Java Basics",
            "Python Overview",
            "C# Advanced Features",
            "Comparing Languages"
        };

        string[] contents =
        {
            "C# is a modern object-oriented programming language developed by Microsoft.",
            "Java is a popular programming language known for its portability and performance.",
            "Python is a high-level programming language with simple syntax.",
            "C# supports LINQ, async/await, and many modern programming paradigms.",
            "C#, Java, and Python are among the most popular programming languages today."
        };

        var documents = new List<Document>();

        for (int i = 0; i < count; i++)
        {
            int position = i % titles.Length;

            documents.Add(new Document
            {
                Id = i + 1,
                Title = titles[position],
                Content = contents[position]
            });
        }

        return documents;
    }
}

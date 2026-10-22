using System;
using System.Collections.Generic;

public static class FunctionalUtils
{
    public static List<TResult> Map<T, TResult>(
        List<T> list, Func<T, TResult> mapper)
    {
        var result = new List<TResult>();

        foreach (var item in list)
            result.Add(mapper(item));

        return result;
    }

    public static List<T> Filter<T>(
        List<T> list, Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (var item in list)
            if (predicate(item))
                result.Add(item);

        return result;
    }

    public static List<T> Take<T>(List<T> list, int count)
    {
        var result = new List<T>();
        int limit = Math.Max(0, Math.Min(count, list.Count));

        for (int i = 0; i < limit; i++)
            result.Add(list[i]);

        return result;
    }

    public static List<T> Skip<T>(List<T> list, int count)
    {
        var result = new List<T>();
        int start = Math.Max(0, Math.Min(count, list.Count));

        for (int i = start; i < list.Count; i++)
            result.Add(list[i]);

        return result;
    }

    public static int Count<T>(
        List<T> list, Predicate<T> predicate)
    {
        int count = 0;

        foreach (var item in list)
            if (predicate(item))
                count++;

        return count;
    }
}

public class Student
{
    public string Name { get; set; }
    public int Grade { get; set; }

    public Student(string name, int grade)
    {
        Name = name;
        Grade = grade;
    }
}

public class Program
{
    public static void Main()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6 };
        var words = new List<string> { "apple", "banana", "kiwi", "orange" };
        var students = new List<Student>
        {
            new Student("Anna", 90),
            new Student("Ivan", 75),
            new Student("Olena", 95),
            new Student("Petro", 60)
        };

        var doubled = FunctionalUtils.Map(numbers, x => x * 2);
        Console.WriteLine(string.Join(", ", doubled));

        var evenNumbers = FunctionalUtils.Filter(numbers, x => x % 2 == 0);
        Console.WriteLine(string.Join(", ", evenNumbers));

        var firstThree = FunctionalUtils.Take(numbers, 3);
        Console.WriteLine(string.Join(", ", firstThree));

        var skipped = FunctionalUtils.Skip(words, 2);
        Console.WriteLine(string.Join(", ", skipped));

        Console.WriteLine(FunctionalUtils.Count(numbers, x => x > 3));

        var wordLengths = FunctionalUtils.Map(words, word => word.Length);
        Console.WriteLine(string.Join(", ", wordLengths));

        var longWords = FunctionalUtils.Filter(words, word => word.Length > 5);
        Console.WriteLine(string.Join(", ", longWords));

        var topStudents = FunctionalUtils.Take(
            FunctionalUtils.Filter(students, student => student.Grade >= 80), 2);
        Console.WriteLine(string.Join(", ",
            FunctionalUtils.Map(topStudents, student => $"{student.Name}: {student.Grade}")));

        var skippedStudents = FunctionalUtils.Skip(students, 1);
        Console.WriteLine(string.Join(", ",
            FunctionalUtils.Map(skippedStudents, student => student.Name)));

        Console.WriteLine(FunctionalUtils.Count(students, student => student.Grade >= 80));
    }
}

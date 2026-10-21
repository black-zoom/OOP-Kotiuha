using System;
using System.Collections.Generic;
using System.Linq;

public interface IEntity
{
    int Id { get; set; }
}

public interface IRepository<T> where T : IEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
}

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = new();
    
    public T GetById(int id)
    {
        return _items.FirstOrDefault(item => item.Id == id)
            ?? throw new KeyNotFoundException(
                $"Елемент з ID {id} не знайдено.");
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Add(T item)
    {
        if (_items.Any(existing => existing.Id == item.Id))
            throw new InvalidOperationException();

        _items.Add(item);
    }

    public void Update(T item)
    {
        int index = _items.FindIndex(existing => existing.Id == item.Id);

        if (index == -1)
            throw new KeyNotFoundException();

        _items[index] = item;
    }

    public void Delete(int id)
    {
        int index = _items.FindIndex(item => item.Id == id);

        if (index == -1)
            throw new KeyNotFoundException();

        _items.RemoveAt(index);
    }
}

public class Course : IEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int TeacherId { get; set; }
}

public class Program
{
    public static void Main()
    {
        IRepository<Course> repository = new InMemoryRepository<Course>();

        repository.Add(new Course { Id = 1, Title = "C#", Credits = 5, TeacherId = 101 });
        repository.Add(new Course { Id = 2, Title = "OOP", Credits = 4, TeacherId = 102 });

        Console.WriteLine("Усі курси:");
        foreach (Course course in repository.GetAll())
            Console.WriteLine($"{course.Id}: {course.Title}, Credits: {course.Credits}, TeacherId: {course.TeacherId}");

        Course updatedCourse = new Course
        {
            Id = 1,
            Title = "Advanced C#",
            Credits = 6,
            TeacherId = 101
        };

        repository.Update(updatedCourse);

        Console.WriteLine("\nПісля оновлення:");
        Course courseById = repository.GetById(1);
        Console.WriteLine($"{courseById.Id}: {courseById.Title}, Credits: {courseById.Credits}, TeacherId: {courseById.TeacherId}");

        repository.Delete(2);

        Console.WriteLine("\nКількість курсів після видалення: " + repository.GetAll().Count());
    }
}

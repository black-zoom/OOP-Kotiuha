using System;
using System.Linq;
using Xunit;

public class RepositoryTests
{
    private readonly InMemoryRepository<Course> _repository = new();

    [Fact]
    public void Add_ShouldAddCourse()
    {
        _repository.Add(new Course
        {
            Id = 1,
            Title = "C#",
            Credits = 5,
            TeacherId = 101
        });

        Assert.Single(_repository.GetAll());
    }

    [Fact]
    public void Add_ShouldThrowForDuplicateId()
    {
        _repository.Add(new Course { Id = 1, Title = "C#" });

        Assert.Throws<InvalidOperationException>(() =>
            _repository.Add(new Course { Id = 1, Title = "OOP" }));
    }

    [Fact]
    public void GetById_ShouldReturnCourse()
    {
        var course = new Course { Id = 1, Title = "C#" };
        _repository.Add(course);

        Assert.Equal(course, _repository.GetById(1));
    }

    [Fact]
    public void GetById_ShouldThrowWhenCourseDoesNotExist()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _repository.GetById(999));
    }

    [Fact]
    public void GetAll_ShouldReturnAllCourses()
    {
        _repository.Add(new Course { Id = 1, Title = "C#" });
        _repository.Add(new Course { Id = 2, Title = "OOP" });

        Assert.Equal(2, _repository.GetAll().Count());
    }

    [Fact]
    public void Update_ShouldUpdateCourse()
    {
        _repository.Add(new Course { Id = 1, Title = "C#" });

        _repository.Update(new Course { Id = 1, Title = "Advanced C#" });

        Assert.Equal("Advanced C#", _repository.GetById(1).Title);
    }

    [Fact]
    public void Update_ShouldThrowWhenCourseDoesNotExist()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _repository.Update(new Course { Id = 999, Title = "C#" }));
    }

    [Fact]
    public void Delete_ShouldRemoveCourse()
    {
        _repository.Add(new Course { Id = 1, Title = "C#" });

        _repository.Delete(1);

        Assert.Empty(_repository.GetAll());
    }

    [Fact]
    public void Delete_ShouldThrowWhenCourseDoesNotExist()
    {
        Assert.Throws<KeyNotFoundException>(() =>
            _repository.Delete(999));
    }
}

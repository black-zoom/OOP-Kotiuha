using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
}

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    public decimal Salary { get; set; }
}

class Program
{
    static void Measure<T>(
        string title,
        Func<T> query,
        Action<T> display)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        T result = query();

        stopwatch.Stop();

        display(result);

        Console.WriteLine($"Час виконання: {stopwatch.Elapsed.TotalMilliseconds:F4} мс");
        Console.WriteLine();
    }

    static void Main()
    {
        List<Department> departments = new()
        {
            new() { Id = 1, Name = "Sales", Budget = 150000 },
            new() { Id = 2, Name = "IT", Budget = 250000 },
            new() { Id = 3, Name = "HR", Budget = 100000 },
            new() { Id = 4, Name = "Marketing", Budget = 180000 },
            new() { Id = 5, Name = "Finance", Budget = 200000 }
        };

        List<Employee> employees = new()
        {
            new() { Id = 1, Name = "John Smith", DepartmentId = 1, Salary = 60000 },
            new() { Id = 2, Name = "Anna Brown", DepartmentId = 2, Salary = 75000 },
            new() { Id = 3, Name = "Peter Wilson", DepartmentId = 1, Salary = 45000 },
            new() { Id = 4, Name = "Kate Johnson", DepartmentId = 3, Salary = 52000 },
            new() { Id = 5, Name = "Michael Davis", DepartmentId = 2, Salary = 90000 },
            new() { Id = 6, Name = "Emma Taylor", DepartmentId = 4, Salary = 48000 },
            new() { Id = 7, Name = "David Miller", DepartmentId = 5, Salary = 85000 },
            new() { Id = 8, Name = "Olivia Anderson", DepartmentId = 1, Salary = 72000 },
            new() { Id = 9, Name = "James Thomas", DepartmentId = 2, Salary = 55000 },
            new() { Id = 10, Name = "Sophia Moore", DepartmentId = 3, Salary = 62000 },
            new() { Id = 11, Name = "Robert Martin", DepartmentId = 4, Salary = 58000 },
            new() { Id = 12, Name = "Emily Jackson", DepartmentId = 5, Salary = 95000 },
            new() { Id = 13, Name = "Daniel White", DepartmentId = 1, Salary = 51000 },
            new() { Id = 14, Name = "Grace Harris", DepartmentId = 2, Salary = 68000 },
            new() { Id = 15, Name = "William Clark", DepartmentId = 5, Salary = 42000 }
        };

        Console.WriteLine("1. Співробітники із зарплатою > 50000 у відділі Sales");

        Measure(
            "Query Syntax:",
            () =>
                (from e in employees
                 join d in departments on e.DepartmentId equals d.Id
                 where e.Salary > 50000 && d.Name == "Sales"
                 select e).ToList(),
            result =>
                result.ForEach(e =>
                    Console.WriteLine($"{e.Name} - {e.Salary}"))
        );

        Measure(
            "Method Syntax:",
            () => employees
                .Join(
                    departments,
                    e => e.DepartmentId,
                    d => d.Id,
                    (e, d) => new { Employee = e, Department = d }
                )
                .Where(x => x.Employee.Salary > 50000 &&
                            x.Department.Name == "Sales")
                .Select(x => x.Employee)
                .ToList(),
            result =>
                result.ForEach(e =>
                    Console.WriteLine($"{e.Name} - {e.Salary}"))
        );

        Console.WriteLine("2. Об'єднання співробітників з відділами");

        Measure(
            "Query Syntax:",
            () =>
                (from e in employees
                 join d in departments on e.DepartmentId equals d.Id
                 select new
                 {
                     EmployeeName = e.Name,
                     DepartmentName = d.Name
                 }).ToList(),
            result =>
                result.ForEach(x =>
                    Console.WriteLine($"{x.EmployeeName} - {x.DepartmentName}"))
        );

        Measure(
            "Method Syntax:",
            () => employees
                .Join(
                    departments,
                    e => e.DepartmentId,
                    d => d.Id,
                    (e, d) => new
                    {
                        EmployeeName = e.Name,
                        DepartmentName = d.Name
                    }
                ).ToList(),
            result =>
                result.ForEach(x =>
                    Console.WriteLine($"{x.EmployeeName} - {x.DepartmentName}"))
        );

        Console.WriteLine("3. Загальна зарплата для кожного відділу");

        Measure(
            "Query Syntax:",
            () =>
                (from d in departments
                 join e in employees on d.Id equals e.DepartmentId into employeeGroup
                 select new
                 {
                     DepartmentName = d.Name,
                     TotalSalary = employeeGroup.Sum(e => e.Salary)
                 }).ToList(),
            result =>
                result.ForEach(x =>
                    Console.WriteLine($"{x.DepartmentName} - {x.TotalSalary}"))
        );

        Measure(
            "Method Syntax:",
            () => departments
                .GroupJoin(
                    employees,
                    d => d.Id,
                    e => e.DepartmentId,
                    (d, group) => new
                    {
                        DepartmentName = d.Name,
                        TotalSalary = group.Sum(e => e.Salary)
                    }
                ).ToList(),
            result =>
                result.ForEach(x =>
                    Console.WriteLine($"{x.DepartmentName} - {x.TotalSalary}"))
        );

        Console.WriteLine("4. Співробітник з найвищою зарплатою");

        Measure(
            "Query Syntax:",
            () =>
                (from e in employees
                 orderby e.Salary descending
                 select e).First(),
            e =>
                Console.WriteLine($"{e.Name} - {e.Salary}")
        );

        Measure(
            "Method Syntax:",
            () => employees.OrderByDescending(e => e.Salary).First(),
            e =>
                Console.WriteLine($"{e.Name} - {e.Salary}")
        );
    }
}

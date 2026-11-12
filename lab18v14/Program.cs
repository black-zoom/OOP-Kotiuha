using System;

class TaskAlreadyCompletedException : Exception
{
    public int TaskId { get; }

    public TaskAlreadyCompletedException(int taskId)
        : base($"Завдання {taskId} вже виконано.")
    {
        TaskId = taskId;
    }
}

class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(T value, bool isSuccess, string error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }

    public static Result<T> Failure(string error)
    {
        return new Result<T>(default!, false, error);
    }

    public static Result<T> Failure(Exception ex)
    {
        return Failure(ex.Message);
    }
}

class TaskService
{
    private readonly Dictionary<int, bool> tasks = new()
    {
        { 1, false },
        { 2, true },
        { 3, false }
    };

    public void CompleteTask(int taskId)
    {
        if (!tasks.ContainsKey(taskId))
            throw new ArgumentException("Завдання не знайдено.");

        if (tasks[taskId])
            throw new TaskAlreadyCompletedException(taskId);

        tasks[taskId] = true;
    }

    public Result<bool> AssignTask(int taskId, string assignee)
    {
        if (!tasks.ContainsKey(taskId))
            return Result<bool>.Failure("Завдання не знайдено.");

        if (string.IsNullOrWhiteSpace(assignee))
            return Result<bool>.Failure("Не вказано виконавця.");

        return Result<bool>.Success(true);
    }
}

class Program
{
    static void Main()
    {
        TaskService service = new TaskService();

        try
        {
            service.CompleteTask(2);
            Console.WriteLine("Завдання виконано.");
        }
        catch (TaskAlreadyCompletedException ex)
        {
            Console.WriteLine(ex.Message);
        }

        try
        {
            service.CompleteTask(1);
            Console.WriteLine("Завдання виконано.");
        }
        catch (TaskAlreadyCompletedException ex)
        {
            Console.WriteLine(ex.Message);
        }

        Result<bool> result1 = service.AssignTask(1, "Student");

        if (result1.IsSuccess)
            Console.WriteLine("Завдання успішно призначено.");
        else
            Console.WriteLine(result1.Error);

        Result<bool> result2 = service.AssignTask(10, "Student");

        if (result2.IsSuccess)
            Console.WriteLine("Завдання успішно призначено.");
        else
            Console.WriteLine(result2.Error);
    }
}

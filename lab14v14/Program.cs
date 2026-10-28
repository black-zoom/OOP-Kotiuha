using System;
using System.Collections.Generic;

class BackupSystem
{
    private readonly HashSet<string> _uniqueFiles = new(StringComparer.OrdinalIgnoreCase);
    private readonly Queue<string> _filesToCopy = new();
    private readonly Stack<string> _backupHistory = new();

    public bool AddFile(string fileName)
    {
        if (!_uniqueFiles.Add(fileName))
        {
            return false;
        }

        _filesToCopy.Enqueue(fileName);
        return true;
    }

    public bool ContainsFile(string fileName)
    {
        return _uniqueFiles.Contains(fileName);
    }

    public string? PeekNext()
    {
        return _filesToCopy.Count > 0 ? _filesToCopy.Peek() : null;
    }

    public string? ProcessNext()
    {
        if (_filesToCopy.Count == 0)
        {
            return null;
        }

        string fileName = _filesToCopy.Dequeue();
        _backupHistory.Push(fileName);
        return fileName;
    }

    public string? PeekLastBackup()
    {
        return _backupHistory.Count > 0 ? _backupHistory.Peek() : null;
    }

    public string? UndoLastBackup()
    {
        if (_backupHistory.Count == 0)
        {
            return null;
        }

        string fileName = _backupHistory.Pop();
        _uniqueFiles.Remove(fileName);
        return fileName;
    }
}

class Program
{
    static void Main()
    {
        var backupSystem = new BackupSystem();

        backupSystem.AddFile("report.pdf");
        backupSystem.AddFile("photo.jpg");
        Console.WriteLine($"Дубль додано: {backupSystem.AddFile("report.pdf")}");
        Console.WriteLine($"Наступний файл: {backupSystem.PeekNext()}");

        string? file;
        while ((file = backupSystem.ProcessNext()) is not null)
        {
            Console.WriteLine($"Скопійовано: {file}");
        }

        Console.WriteLine($"Останній бекап: {backupSystem.PeekLastBackup()}");
        Console.WriteLine($"Скасовано: {backupSystem.UndoLastBackup()}");
        Console.WriteLine($"Файл є серед унікальних: {backupSystem.ContainsFile("photo.jpg")}");
    }
}

using System;
using System.Collections.Generic;
using System.IO;

abstract class ImageProcessor
{
    public string ImagePath { get; set; }

    protected ImageProcessor(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new ArgumentException("Шлях до зображення не може бути порожнім.");

        ImagePath = imagePath;
    }

    public abstract void Process();

    protected void ValidateFile()
    {
        if (!File.Exists(ImagePath))
            throw new FileNotFoundException(
                $"Файл зображення не знайдено: {ImagePath}");
    }
}

class ResizeProcessor : ImageProcessor
{
    public int Width { get; set; }
    public int Height { get; set; }

    public ResizeProcessor(string imagePath, int width, int height)
        : base(imagePath)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException(
                "Ширина та висота повинні бути більшими за 0.");

        Width = width;
        Height = height;
    }

    public override void Process()
    {
        ValidateFile();
        Console.WriteLine(
            $"[Resize] Зображення {ImagePath} змінено до {Width}x{Height}.");
    }
}

class FilterProcessor : ImageProcessor
{
    public string FilterName { get; set; }

    public FilterProcessor(string imagePath, string filterName)
        : base(imagePath)
    {
        if (string.IsNullOrWhiteSpace(filterName))
            throw new ArgumentException(
                "Назва фільтра не може бути порожньою.");

        FilterName = filterName;
    }

    public override void Process()
    {
        ValidateFile();
        Console.WriteLine(
            $"[Filter] До зображення {ImagePath} застосовано фільтр \"{FilterName}\".");
    }
}

class CompressProcessor : ImageProcessor
{
    public int Quality { get; set; }

    public CompressProcessor(string imagePath, int quality)
        : base(imagePath)
    {
        if (quality < 1 || quality > 100)
            throw new ArgumentException(
                "Якість стиснення повинна бути від 1 до 100.");

        Quality = quality;
    }

    public override void Process()
    {
        ValidateFile();
        Console.WriteLine(
            $"[Compress] Зображення {ImagePath} стиснено. Якість: {Quality}%.");
    }
}

class ImageProcessorService
{
    public void ProcessAll(List<ImageProcessor> processors)
    {
        foreach (ImageProcessor processor in processors)
        {
            try
            {
                processor.Process();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"ERROR(Validation): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        File.WriteAllText("photo1.jpg", "test image");
        File.WriteAllText("photo2.jpg", "test image");

        List<ImageProcessor> processors = new List<ImageProcessor>
        {
            new ResizeProcessor("photo1.jpg", 1920, 1080),
            new FilterProcessor("photo2.jpg", "Grayscale"),
            new CompressProcessor("photo1.jpg", 80),
            new ResizeProcessor("missing.jpg", 800, 600)
        };

        ImageProcessorService service = new ImageProcessorService();
        service.ProcessAll(processors);

        try
        {
            ImageProcessor invalidProcessor =
                new CompressProcessor("photo1.jpg", 150);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ERROR(Validation): {ex.Message}");
        }

        File.Delete("photo1.jpg");
        File.Delete("photo2.jpg");
    }
}


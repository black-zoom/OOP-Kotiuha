using System;

class Container
{
    public virtual void Store()
    {
        Console.WriteLine("Зберігання предметів у контейнері.");
    }
}

class Box : Container
{
    public override void Store()
    {
        Console.WriteLine("Зберігання предметів у коробці.");
    }

    public void ShowBoxInfo()
    {
        Console.WriteLine("Коробка — жорсткий контейнер для зберігання предметів.");
    }
}

class Bag : Container
{
    public new void Store()
    {
        Console.WriteLine("Зберігання предметів у сумці.");
    }

    public void ShowBagInfo()
    {
        Console.WriteLine("Сумка — гнучкий контейнер для зберігання предметів.");
    }
}

class Program
{
    static void Main()
    {
        Container container = new Container();
        Container box = new Box();
        Container bag = new Bag();

        Console.WriteLine("Контейнер:");
        container.Store();

        Console.WriteLine("\nКоробка через Container:");
        box.Store();
        Console.WriteLine("override: викликається метод класу Box.");

        Console.WriteLine("\nСумка через Container:");
        bag.Store();
        Console.WriteLine("new: викликається метод базового класу Container.");

        Console.WriteLine("\nКоробка через Box:");
        ((Box)box).Store();

        Console.WriteLine("\nСумка через Bag:");
        ((Bag)bag).Store();
        Console.WriteLine("new: після приведення до Bag викликається метод класу Bag.");

        Console.WriteLine("\nДодаткові методи:");
        ((Box)box).ShowBoxInfo();
        ((Bag)bag).ShowBagInfo();
    }
}

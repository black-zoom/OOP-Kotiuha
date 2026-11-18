using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void TopProductsByCategory(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQty = g.Sum(s => s.Quantity) })
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { p.Name, p.Category, s.TotalQty })
            .GroupBy(x => x.Category)
            .Select(g => new
            {
                Category = g.Key,
                TopProducts = g.OrderByDescending(x => x.TotalQty).Take(3).ToList()
            })
            .ToList();

        sw.Stop();
        Console.WriteLine($"Pipeline 1: {sw.ElapsedMilliseconds} ms");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Category}:");
            foreach (var p in item.TopProducts)
            {
                Console.WriteLine($"  {p.Name} - {p.TotalQty}");
            }
        }
    }

    static void TotalRevenueByCity(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s, p })
            .Join(customers, sp => sp.s.CustomerId, c => c.Id, (sp, c) => new { c.City, Revenue = sp.s.Quantity * sp.p.UnitPrice })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, TotalRevenue = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();

        sw.Stop();
        Console.WriteLine($"\nPipeline 2: {sw.ElapsedMilliseconds} ms");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.City}: {item.TotalRevenue:F2}");
        }
    }

    static void SalesByMonthAndYear(List<Sale> sales, List<Product> products)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.SaleDate.Year, s.SaleDate.Month, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => new { x.Year, x.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalQty = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        sw.Stop();
        Console.WriteLine($"\nPipeline 3: {sw.ElapsedMilliseconds} ms");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.Year}-{item.Month:D2}: Qty={item.TotalQty}, Rev={item.TotalRevenue:F2}");
        }
    }

    static void TopCustomersBySpend(List<Sale> sales, List<Product> products, List<Customer> customers)
    {
        var sw = Stopwatch.StartNew();

        var result = sales
            .Join(products, s => s.ProductId, p => p.Id, (s, p) => new { s.CustomerId, s.Quantity, p.UnitPrice, p.Category })
            .Join(customers, sp => sp.CustomerId, c => c.Id, (sp, c) => new { c.Name, Revenue = sp.Quantity * sp.UnitPrice, sp.Category })
            .GroupBy(x => x.Name)
            .Select(g => new
            {
                CustomerName = g.Key,
                TotalSpend = g.Sum(x => x.Revenue),
                UniqueCategories = g.Select(x => x.Category).Distinct().Count()
            })
            .Where(x => x.UniqueCategories >= 3)
            .OrderByDescending(x => x.TotalSpend)
            .Take(5)
            .ToList();

        sw.Stop();
        Console.WriteLine($"\nPipeline 4: {sw.ElapsedMilliseconds} ms");
        foreach (var item in result)
        {
            Console.WriteLine($"{item.CustomerName}: Spend={item.TotalSpend:F2}, Cats={item.UniqueCategories}");
        }
    }

    static void Main()
    {
        var rnd = new Random(42);

        var customers = Enumerable.Range(1, 100).Select(i => new Customer
        {
            Id = i,
            Name = $"Customer_{i}",
            City = $"City_{rnd.Next(1, 11)}"
        }).ToList();

        var products = Enumerable.Range(1, 100).Select(i => new Product
        {
            Id = i,
            Name = $"Product_{i}",
            Category = $"Category_{rnd.Next(1, 6)}",
            UnitPrice = (decimal)(rnd.NextDouble() * 100 + 10)
        }).ToList();

        var sales = Enumerable.Range(1, 1000).Select(i => new Sale
        {
            Id = i,
            ProductId = rnd.Next(1, 101),
            CustomerId = rnd.Next(1, 101),
            Quantity = rnd.Next(1, 20),
            SaleDate = new DateTime(2023, 1, 1).AddDays(rnd.Next(0, 730))
        }).ToList();

        TopProductsByCategory(sales, products);
        TotalRevenueByCity(sales, products, customers);
        SalesByMonthAndYear(sales, products);
        TopCustomersBySpend(sales, products, customers);
    }
}

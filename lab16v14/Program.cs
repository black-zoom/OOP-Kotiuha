using System;
using System.Collections.Generic;
using System.Linq;

class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; } = new();
}

class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Supplier> Suppliers { get; set; } = new();
}

class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; }
}

static class RecipeExtensions
{
    public static bool HasIngredient(this Recipe recipe, string ingredientName)
    {
        return recipe.Ingredients.Any(i =>
            i.Name.Equals(ingredientName, StringComparison.OrdinalIgnoreCase));
    }
}

class Program
{
    static void Main()
    {
        var suppliers = new List<Supplier>
        {
            new Supplier { Id = 1, Name = "FreshFood" },
            new Supplier { Id = 2, Name = "GreenMarket" },
            new Supplier { Id = 3, Name = "FoodExpress" },
            new Supplier { Id = 4, Name = "EcoProducts" }
        };

        var recipes = new List<Recipe>
        {
            new Recipe
            {
                Id = 1,
                Name = "Борщ",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 1, Name = "Буряк", Suppliers = new() { suppliers[0], suppliers[1] } },
                    new Ingredient { Id = 2, Name = "Картопля", Suppliers = new() { suppliers[1] } }
                }
            },
            new Recipe
            {
                Id = 2,
                Name = "Піца",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 3, Name = "Сир", Suppliers = new() { suppliers[2] } },
                    new Ingredient { Id = 4, Name = "Помідори", Suppliers = new() { suppliers[0], suppliers[3] } }
                }
            },
            new Recipe
            {
                Id = 3,
                Name = "Салат",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 5, Name = "Огірок", Suppliers = new() { suppliers[1] } },
                    new Ingredient { Id = 6, Name = "Помідори", Suppliers = new() { suppliers[0] } }
                }
            },
            new Recipe
            {
                Id = 4,
                Name = "Паста",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 7, Name = "Макарони", Suppliers = new() { suppliers[2] } },
                    new Ingredient { Id = 8, Name = "Сир", Suppliers = new() { suppliers[3] } }
                }
            },
            new Recipe
            {
                Id = 5,
                Name = "Омлет",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 9, Name = "Яйця", Suppliers = new() { suppliers[0] } },
                    new Ingredient { Id = 10, Name = "Молоко", Suppliers = new() { suppliers[1], suppliers[2] } }
                }
            },
            new Recipe
            {
                Id = 6,
                Name = "Млинці",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient { Id = 11, Name = "Борошно", Suppliers = new() { suppliers[3] } },
                    new Ingredient { Id = 12, Name = "Молоко", Suppliers = new() { suppliers[1] } }
                }
            }
        };

        var uniqueSuppliers = recipes
            .SelectMany(r => r.Ingredients)
            .SelectMany(i => i.Suppliers)
            .DistinctBy(s => s.Id)
            .ToList();

        Console.WriteLine("Постачальники:");

        foreach (var supplier in uniqueSuppliers)
            Console.WriteLine(supplier.Name);

        int pageSize = 3;
        int totalPages = (int)Math.Ceiling(recipes.Count / (double)pageSize);

        for (int page = 1; page <= totalPages; page++)
        {
            Console.WriteLine($"\nСторінка {page}:");

            var pageRecipes = recipes
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            foreach (var recipe in pageRecipes)
                Console.WriteLine(recipe.Name);
        }

        Console.WriteLine("\nПеревірка інгредієнтів:");

        foreach (var recipe in recipes)
        {
            if (recipe.HasIngredient("Сир"))
                Console.WriteLine(recipe.Name);
        }
    }
}

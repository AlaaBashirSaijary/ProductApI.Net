using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.EnsureCreatedAsync();
        if (await db.Products.AnyAsync()) return;

        db.Products.AddRange(
            new Product("Laptop", 1499.00m),
            new Product("Wireless Headphones", 89.90m),
            new Product("Mechanical Keyboard", 45.00m),
            new Product("Gaming Mouse", 29.99m),
            new Product("27\" Monitor", 329.50m),
            new Product("USB-C Hub", 24.75m),
            new Product("Webcam HD", 59.00m),
            new Product("External SSD 1TB", 119.99m));
        await db.SaveChangesAsync();
    }
}

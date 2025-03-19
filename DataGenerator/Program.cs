using System;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using DAL;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new WebstoreContext())
        {
            Console.WriteLine("Tworzenie bazy danych...");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Console.WriteLine("Wypełnianie danymi...");
            SeedDatabase(context);

            Console.WriteLine("Dane zostały dodane do bazy!");
        }
    }

    static void SeedDatabase(WebstoreContext context)
    {
        var userGroupFaker = new Faker<UserGroup>()
            .RuleFor(ug => ug.Name, f => f.Company.CompanyName());

        var userGroups = userGroupFaker.Generate(3);
        context.UserGroups.AddRange(userGroups);
        context.SaveChanges();

        var productGroupFaker = new Faker<ProductGroup>()
            .RuleFor(g => g.Name, f => f.Commerce.Department())
            .RuleFor(g => g.ParentID, f => f.Random.Bool(0.3f) ? f.Random.Int(1, 5) : (int?)null);


        var groups = productGroupFaker.Generate(30);
        context.ProductGroups.AddRange(groups);
        context.SaveChanges();

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Double(5, 500))
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.GroupID, f => f.PickRandom(groups).ID);

        var products = productFaker.Generate(20);
        context.Products.AddRange(products);
        context.SaveChanges();

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Login, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Type, f => f.PickRandom<UserType>())
            .RuleFor(u => u.IsActive, f => f.Random.Bool())
            .RuleFor(u => u.GroupID, f => f.PickRandom(userGroups).ID);

        var users = userFaker.Generate(5);
        context.Users.AddRange(users);
        context.SaveChanges();

        var basketPositions = new HashSet<(int UserID, int ProductID)>();
        var basketList = new List<BasketPosition>();

        foreach (var user in users)
        {
            foreach (var product in products.OrderBy(_ => Guid.NewGuid()).Take(3))
            {
                if (basketPositions.Add((user.ID, product.ID)))
                {
                    basketList.Add(new BasketPosition { UserID = user.ID, ProductID = product.ID, Amount = new Random().Next(1, 5) });
                }
            }
        }

        context.BasketPositions.AddRange(basketList);
        context.SaveChanges();

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.UserID, f => f.PickRandom(users).ID)
            .RuleFor(o => o.Date, f => f.Date.Past(1));

        var orders = orderFaker.Generate(5);
        context.Orders.AddRange(orders);
        context.SaveChanges();

        var orderPositions = new HashSet<(int OrderID, int ProductID)>();
        var orderPositionsList = new List<OrderPosition>();

        foreach (var order in orders)
        {
            foreach (var product in products.OrderBy(_ => Guid.NewGuid()).Take(3))
            {
                if (orderPositions.Add((order.ID, product.ID)))
                {
                    orderPositionsList.Add(new OrderPosition
                    {
                        OrderID = order.ID,
                        ProductID = product.ID,
                        Amount = new Random().Next(1, 5),
                        Price = product.Price
                    });
                }
            }
        }

        context.OrderPositions.AddRange(orderPositionsList);
        context.SaveChanges();
    }

}

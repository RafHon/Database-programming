using Bogus;
using DAL;
using Model;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Linq;

public class DataSeeder
{
    private readonly WebstoreContext _context;

    public DataSeeder(WebstoreContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Products.Any()) return;

        var productGroupFaker = new Faker<ProductGroup>()
            .RuleFor(g => g.Name, f => f.Commerce.Department());

        var groups = productGroupFaker.Generate(5);
        _context.ProductGroups.AddRange(groups);
        _context.SaveChanges();

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Double(5, 500))
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.IsActive, f => f.Random.Bool())
            .RuleFor(p => p.GroupID, f => f.PickRandom(groups).ID);

        var products = productFaker.Generate(20);
        _context.Products.AddRange(products);
        _context.SaveChanges();

        var userFaker = new Faker<User>()
            .RuleFor(u => u.Login, f => f.Internet.UserName())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Type, f => f.PickRandom<UserType>())
            .RuleFor(u => u.IsActive, f => f.Random.Bool())
            .RuleFor(u => u.GroupID, f => null);

        var users = userFaker.Generate(5);
        _context.Users.AddRange(users);
        _context.SaveChanges();

        var basketFaker = new Faker<BasketPosition>()
            .RuleFor(b => b.UserID, f => f.PickRandom(users).ID)
            .RuleFor(b => b.ProductID, f => f.PickRandom(products).ID)
            .RuleFor(b => b.Amount, f => f.Random.Int(1, 5));

        _context.BasketPositions.AddRange(basketFaker.Generate(10));
        _context.SaveChanges();

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.UserID, f => f.PickRandom(users).ID)
            .RuleFor(o => o.Date, f => f.Date.Past(1));

        var orders = orderFaker.Generate(5);
        _context.Orders.AddRange(orders);
        _context.SaveChanges();

        var orderPositionsFaker = new Faker<OrderPosition>()
            .RuleFor(op => op.OrderID, f => f.PickRandom(orders).ID)
            .RuleFor(op => op.Amount, f => f.Random.Int(1, 5))
            .RuleFor(op => op.Price, (f, op) => f.Random.Double(5, 500));


        _context.OrderPositions.AddRange(orderPositionsFaker.Generate(15));
        _context.SaveChanges();
    }
}

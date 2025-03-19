using Microsoft.EntityFrameworkCore;
using Model.Models;
public class WebstoreContext : DbContext
{
    public WebstoreContext(DbContextOptions<WebstoreContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductGroup> ProductGroups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<BasketPosition> BasketPositions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderPosition> OrderPositions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Shop;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BasketPosition>()
            .HasKey(bp => new { bp.ProductID, bp.UserID });

        modelBuilder.Entity<OrderPosition>()
            .HasKey(op => new { op.OrderID, op.ProductID });

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductGroup)
            .WithMany(pg => pg.Products)
            .HasForeignKey(p => p.GroupID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ProductGroup>()
            .HasOne(pg => pg.ParentGroup)
            .WithMany(c=>c.ChildProductGroups)
            .HasForeignKey(pg => pg.ParentID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserGroup)
            .WithMany(ug => ug.Users)
            .HasForeignKey(u => u.GroupID)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BasketPosition>()
            .HasOne(bp => bp.User)
            .WithMany(u => u.BasketPositions)
            .HasForeignKey(bp => bp.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BasketPosition>()
            .HasOne(bp => bp.Product)
            .WithMany(p => p.BasketPositions)
            .HasForeignKey(bp => bp.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderPosition>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderPositions)
            .HasForeignKey(op => op.OrderID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderPosition>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderPositions)
            .HasForeignKey(op => op.ProductID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

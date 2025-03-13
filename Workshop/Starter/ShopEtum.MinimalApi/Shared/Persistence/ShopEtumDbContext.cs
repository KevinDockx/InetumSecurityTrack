namespace ShopEtum.MinimalApi.Shared.Persistence;

using Microsoft.EntityFrameworkCore;
using ShopEtum.MinimalApi.Shared.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

public class ShopEtumDbContext(DbContextOptions<ShopEtumDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }
        } 

        return base.SaveChangesAsync(cancellationToken);
    } 

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Cart)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CartId);

        // Seed data
        var creationDate = new DateTime(2025, 1, 15);

        modelBuilder.Entity<Product>().HasData(
            new Product("The Shining", "A horror novel by Stephen King", 19.99m, creationDate) { Id = 1 },
            new Product("It", "A horror novel by Stephen King", 24.99m, creationDate) { Id = 2 },
            new Product("Misery", "A psychological horror novel by Stephen King", 14.99m, creationDate) { Id = 3 },
            new Product("Carrie", "A horror novel by Stephen King", 12.99m, creationDate) { Id = 4 },
            new Product("1984", "A dystopian novel by George Orwell", 9.99m, creationDate) { Id = 5 },
            new Product("Brave New World", "A dystopian novel by Aldous Huxley", 8.99m, creationDate) { Id = 6 },
            new Product("To Kill a Mockingbird", "A novel by Harper Lee", 7.99m, creationDate) { Id = 7 },
            new Product("The Great Gatsby", "A novel by F. Scott Fitzgerald", 10.99m, creationDate) { Id = 8 },
            new Product("Moby Dick", "A novel by Herman Melville", 11.99m, creationDate) { Id = 9 },
            new Product("War and Peace", "A novel by Leo Tolstoy", 13.99m, creationDate) { Id = 10 },
            new Product("Fred Again.. - Actual Life 3", "Vinyl record by Fred Again..", 29.99m, creationDate) { Id = 11 },
            new Product("Jamie XX - In Colour", "Vinyl record by Jamie XX", 27.99m, creationDate) { Id = 12 },
            new Product("Abbey Road - The Beatles", "Vinyl record by The Beatles", 25.99m, creationDate) { Id = 13 },
            new Product("Dark Side of the Moon - Pink Floyd", "Vinyl record by Pink Floyd", 26.99m, creationDate) { Id = 14 },
            new Product("Thriller - Michael Jackson", "Vinyl record by Michael Jackson", 24.99m, creationDate) { Id = 15 },
            new Product("Back in Black - AC/DC", "Vinyl record by AC/DC", 23.99m, creationDate) { Id = 16 },
            new Product("Rumours - Fleetwood Mac", "Vinyl record by Fleetwood Mac", 22.99m, creationDate) { Id = 17 },
            new Product("Hotel California - Eagles", "Vinyl record by Eagles", 21.99m, creationDate) { Id = 18 },
            new Product("The Wall - Pink Floyd", "Vinyl record by Pink Floyd", 20.99m, creationDate) { Id = 19 },
            new Product("Led Zeppelin IV - Led Zeppelin", "Vinyl record by Led Zeppelin", 19.99m, creationDate) { Id = 20 }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order(1, 1, creationDate) { Id = 1, CartId = 1 },
            new Order(2, 2, creationDate) { Id = 2, CartId = 1 },
            new Order(11, 1, creationDate) { Id = 3, CartId = 1 },
            new Order(3, 1, creationDate) { Id = 4, CartId = 2 },
            new Order(12, 2, creationDate) { Id = 5, CartId = 2 },
            new Order(4, 1, creationDate) { Id = 6, CartId = 3 },
            new Order(13, 1, creationDate) { Id = 7, CartId = 3 },
            new Order(14, 1, creationDate) { Id = 8, CartId = 3 }
        );

        modelBuilder.Entity<Cart>().HasData(
            new Cart("kevin.dockx@gmail.com", creationDate) { Id = 1 },
            new Cart("jerry.vanechelpoel@inetum-realdolmen.world", creationDate) { Id = 2 },
            new Cart("dirk.slembrouck@inetum-realdolmen.world", creationDate) { Id = 3 },
            new Cart("kevin.dockx@gmail.com", creationDate) { Id = 4 }
        );
    }
}

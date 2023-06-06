using BezeqFinalProject.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BezeqFinalProject.Common.Data.Contexts;

public class MainContext : DbContext {
    public MainContext(DbContextOptions<MainContext> options)
        : base(options) {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseInMemoryDatabase("SampleDB");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        //modelBuilder.Entity<User>().HasData(new User { Id = 1, Name = "Liron", Email = "lironco@sela.co.il", PwdHash = "123456".Hash() });

        modelBuilder.Entity<Product>().HasData(new Product { Id = 1, Name = "Keyboard", Price = 80 });
        modelBuilder.Entity<Product>().HasData(new Product { Id = 2, Name = "Mouse", Price = 150 });
        modelBuilder.Entity<Product>().HasData(new Product { Id = 3, Name = "Adapter", Price = 35 });
    }
}
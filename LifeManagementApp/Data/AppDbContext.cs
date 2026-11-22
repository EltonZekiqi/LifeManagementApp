using LifeManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LifeManagementApp.Data;

// DbContext = silta C#-olioiden ja SQLite-tietokannan välillä
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Jokanen DbSet = yks taulu
    public DbSet<TodoItem> Todos => Set<TodoItem>();
    public DbSet<Category> Categories => Set<Category>();

    // Code First: tässä kerrotaan miltä kanta näyttää, EF tekee taulut tän perusteella
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);

            // 1–N relaatio: TodoItemilla on yks Category, Categorylla monta Todoo
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Todos)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.SetNull); // Jos kategoria poistetaan, to-do jää ilman
        });

        modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(50);

        // Valmiit kategoriat kantaan heti alussa (seed data)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "School" },
            new Category { Id = 2, Name = "Work" },
            new Category { Id = 3, Name = "Home" },
            new Category { Id = 4, Name = "Sports" });
    }
}

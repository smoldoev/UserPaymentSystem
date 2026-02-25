using Microsoft.EntityFrameworkCore;
using UserPaymentSystem.Models;

namespace UserPaymentSystem.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // База данных будет в папке с проектом
        optionsBuilder.UseSqlite("Data Source=users.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связей
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Если удаляем пользователя, удаляем и его платежи
    }
}
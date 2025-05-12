using InputValidation.Models;
using Microsoft.EntityFrameworkCore;

namespace InputValidation.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    public DbSet<Student> Students { get; set; }
}

using EFTransactions.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFTransactions.Data.Db.DbContexts;
public class TransactionsDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; } = null!;

    public TransactionsDbContext()
    {
    }

    public TransactionsDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
using Microsoft.EntityFrameworkCore;
using Zyfra_Task4.DataAccess.Entities;

namespace Zyfra_Task4.DataAccess;

public sealed class DatabaseContext : DbContext
{
    public DbSet<DataEntry> DataEntries { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.DataEntryConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
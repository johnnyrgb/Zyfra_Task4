using Microsoft.EntityFrameworkCore;
using Zyfra_Task4.DataAccess.Entities;

namespace Zyfra_Task4.DataAccess;

public class DatabaseSeed
{
    private readonly DatabaseContext _context;

    public DatabaseSeed(DatabaseContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (!await _context.DataEntries.AnyAsync())
        {
            var dataEntries = new[]
            {
                    new DataEntry { Id = 1, Value = "Первая запись" },
                    new DataEntry { Id = 2, Value = "Вторая запись" },
                    new DataEntry { Id = 3, Value = "Третья запись" },
                };
            await _context.DataEntries.AddRangeAsync(dataEntries);
            await _context.SaveChangesAsync();
        }
    }
}
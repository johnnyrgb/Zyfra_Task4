using Zyfra_Task4.DataAccess.Entities;
using Zyfra_Task4.DataAccess.Interfaces;

namespace Zyfra_Task4.DataAccess.Repositories;

public class DbRepository
{
    private readonly DatabaseContext _context;
    private IEntityRepository<DataEntry>? _dataEntryRepository;

    public DbRepository(DatabaseContext context)
    {
        _context = context;
    }

    public IEntityRepository<DataEntry> Habit
    {
        get
        {
            if (_dataEntryRepository == null)
            {
                _dataEntryRepository = new DataEntryRepository(_context);
            }
            return _dataEntryRepository;
        }
    }
}
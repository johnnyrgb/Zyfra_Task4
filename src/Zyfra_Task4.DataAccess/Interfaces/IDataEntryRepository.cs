using Zyfra_Task4.DataAccess.Entities;

namespace Zyfra_Task4.DataAccess.Interfaces;

public interface IDataEntryRepository : IEntityRepository<DataEntry>
{
    Task<DataEntry?> GetByValueAsync(string value);
}
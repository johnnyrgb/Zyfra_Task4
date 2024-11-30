using Zyfra_Task4_API.Models;

namespace Zyfra_Task4_API.Services
{
    public interface IFileDataService
    {
        List<DataEntry> GetAll();
        DataEntry? GetById(int id);
        bool CreateOrUpdate(DataEntry entry);
    }
}

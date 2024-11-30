using Zyfra_Task4.BusinessLogic.DTOs;

namespace Zyfra_Task4.BusinessLogic.Interfaces;

public interface IDataEntryService
{
    Task<DataEntryDTO?> GetDataEntryAsync(int id);
    Task<IEnumerable<DataEntryDTO>> GetAllDataEntriesAsync();
    Task CreateDataEntryAsync(DataEntryDTO dataEntryDTO);
    Task UpdateDataEntryAsync(DataEntryDTO dataEntryDTO);
    Task DeleteDataEntryAsync(int id);
}
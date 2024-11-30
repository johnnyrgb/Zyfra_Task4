using Zyfra_Task4.BusinessLogic.DTOs;

namespace Zyfra_Task4.BusinessLogic.Interfaces;

public interface IDataEntryService
{
    Task<DataEntryDTO?> GetDataEntryAsync(int id);
    Task<IEnumerable<DataEntryDTO>> GetAllDataEntriesAsync();
    Task CreateDataEntryAsync(DataEntryDTO dataEntryDTO);
    Task UpdateHabitRecordAsync(DataEntryDTO dataEntryDTO);
    Task DeleteHabitRecordAsync(int id);
}
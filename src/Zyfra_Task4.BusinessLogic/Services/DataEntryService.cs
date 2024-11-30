using Zyfra_Task4.BusinessLogic.DTOs;
using Zyfra_Task4.BusinessLogic.Interfaces;
using Zyfra_Task4.DataAccess.Entities;
using Zyfra_Task4.DataAccess.Interfaces;

namespace Zyfra_Task4.BusinessLogic.Services;

public class DataEntryService : IDataEntryService
{
    private readonly IDbRepository _repository;

    public DataEntryService(IDbRepository repository)
    {
        _repository = repository;
    }
    public async Task<DataEntryDTO?> GetDataEntryAsync(int id)
    {
        var dataEntry = await _repository.DataEntry.GetByIdAsync(id);
        return dataEntry != null ? MapToDTO(dataEntry) : null;
    }

    public async Task<IEnumerable<DataEntryDTO>> GetAllDataEntriesAsync()
    {
        var dataEntries = await _repository.DataEntry.GetAllAsync();
        return (dataEntries ?? []).Select(MapToDTO);
    }

    public async Task CreateDataEntryAsync(DataEntryDTO dataEntryDTO)
    {
        var dataEntry = MapToEntity(dataEntryDTO);
        await _repository.DataEntry.CreateAsync(dataEntry);
    }

    public async Task UpdateHabitRecordAsync(DataEntryDTO dataEntryDTO)
    {
        var habit = MapToEntity(dataEntryDTO);
        await _repository.DataEntry.UpdateAsync(habit);
    }

    public async Task DeleteHabitRecordAsync(int id)
    {
        await _repository.DataEntry.DeleteByIdAsync(id);
    }

    // Маппинг из сущности в DTO
    private DataEntryDTO MapToDTO(DataEntry dataEntry)
    {
        return new DataEntryDTO
        {
            Id = dataEntry.Id,
            Value = dataEntry.Value,
        };
    }

    // Маппинг из DTO в сущность
    private DataEntry MapToEntity(DataEntryDTO dataEntryDTO)
    {
        return new DataEntry
        {
            Id = dataEntryDTO.Id,
            Value = dataEntryDTO.Value,
        };
    }
}
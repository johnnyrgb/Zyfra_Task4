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
    
    /// <summary>
    /// Возвращает запись по Id. Либо null, если запись не найдена.
    /// </summary>
    /// <param name="id"></param>
    public async Task<DataEntryDTO?> GetDataEntryAsync(int id)
    {
        var dataEntry = await _repository.DataEntry.GetByIdAsync(id);
        return dataEntry != null ? MapToDTO(dataEntry) : null;
    }

    /// <summary>
    /// Возвращает запись по значению. Либо null, если запись не найдена.
    /// </summary>
    /// <param name="id"></param>
    public async Task<DataEntryDTO?> GetDataEntryAsync(string value)
    {
        var dataEntry = await _repository.DataEntry.GetByValueAsync(value);
        return dataEntry != null ? MapToDTO(dataEntry) : null;
    }

    /// <summary>
    /// Возвращает список всех записей. Либо пустую коллекцию, если записей нет.
    /// </summary>
    public async Task<IEnumerable<DataEntryDTO>> GetAllDataEntriesAsync()
    {
        var dataEntries = await _repository.DataEntry.GetAllAsync();
        return (dataEntries ?? []).Select(MapToDTO);
    }

    /// <summary>
    /// Создает новую запись
    /// </summary>
    /// <param name="dataEntryDTO"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task CreateDataEntryAsync(DataEntryDTO dataEntryDTO)
    {
        if (string.IsNullOrWhiteSpace(dataEntryDTO.Value))
        {
            throw new ArgumentNullException(nameof(dataEntryDTO.Value), "Значение не может быть пустым.");
        }
        var dataEntry = MapToEntity(dataEntryDTO);
        await _repository.DataEntry.CreateAsync(dataEntry);
    }

    /// <summary>
    /// Обновляет запись
    /// </summary>
    /// <param name="dataEntryDTO"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task UpdateDataEntryAsync(DataEntryDTO dataEntryDTO)
    {
        if (string.IsNullOrWhiteSpace(dataEntryDTO.Value))
        {
            throw new ArgumentNullException(nameof(dataEntryDTO.Value), "Значение не может быть пустым.");
        }
        var dataEntry = MapToEntity(dataEntryDTO);
        await _repository.DataEntry.UpdateAsync(dataEntry);
    }

    /// <summary>
    /// Удаляет запись
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteDataEntryAsync(int id)
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
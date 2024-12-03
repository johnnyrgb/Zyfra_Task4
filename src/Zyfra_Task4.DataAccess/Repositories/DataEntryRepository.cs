﻿using Microsoft.EntityFrameworkCore;
using Zyfra_Task4.DataAccess.Entities;
using Zyfra_Task4.DataAccess.Interfaces;

namespace Zyfra_Task4.DataAccess.Repositories;

public class DataEntryRepository : IDataEntryRepository
{
    private readonly DatabaseContext _databaseContext;

    public DataEntryRepository(DatabaseContext context)
    {
        _databaseContext = context;
    }

    // Получение записи по идентификатору
    public async Task<DataEntry?> GetByIdAsync(int id)
    {
        return await _databaseContext.DataEntries
            .FirstOrDefaultAsync(de => de.Id == id);
    }

    public async Task<DataEntry?> GetByValueAsync(string value)
    {
        return await _databaseContext.DataEntries
            .FirstOrDefaultAsync(de => de.Value == value);
    }

    // Получение всех записей
    public async Task<IEnumerable<DataEntry>> GetAllAsync()
    {
        return await _databaseContext.DataEntries.ToListAsync();
    }

    // Создание новой записи
    public async Task CreateAsync(DataEntry entity)
    {
        await _databaseContext.DataEntries.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();
    }

    // Обновление записи
    public async Task UpdateAsync(DataEntry entity)
    {
        _databaseContext.DataEntries.Update(entity);
        await _databaseContext.SaveChangesAsync();
    }

    // Удаление записи по идентификатору
    public async Task DeleteByIdAsync(int id)
    {
        var dataEntry = await _databaseContext.DataEntries.FindAsync(id);
        if (dataEntry != null)
        {
            _databaseContext.DataEntries.Remove(dataEntry);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
using Zyfra_Task4_API.Models;

namespace Zyfra_Task4_API.Services;

public class FileDataService : IFileDataService
{
    private readonly string _filePath;


    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="filePath">Путь к текстовому файлу.</param>
    public FileDataService(string filePath)
    {
        _filePath = filePath;

        // Создаем файл, если он отсутствует
        if (!File.Exists(_filePath))
        {
            using (File.Create(_filePath)) { }
        }
    }
    /// <summary>
    /// Возвращает все записи из файла.
    /// </summary>
    /// <returns>Список записей.</returns>
    public List<DataEntry> GetAll()
    {
        var lines = File.ReadAllLines(_filePath);
        var entries = new List<DataEntry>();

        foreach (var line in lines)
        {
            try
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    entries.Add(new DataEntry
                    {
                        Id = int.Parse(parts[0].Trim()),
                        Value = parts[1].Trim()
                    });
                }
            }
            catch
            {
                // Пропускаем неверные строки
            }
        }

        return entries;
    }
    /// <summary>
    /// Возвращает запись с данным Id.
    /// </summary>
    /// <param name="id">Id возвращаемой записи.</param>
    /// <returns></returns>
    public DataEntry? GetById(int id)
    {
        return GetAll().FirstOrDefault(entry => entry.Id == id);
    }
    /// <summary>
    /// Создает или обновляет запись с данным Id.
    /// </summary>
    /// <param name="entry">Запись.</param>
    /// <returns>True, если запись существует, и была обновлена. False, если запись не существует, и была добавлена</returns>
    public bool CreateOrUpdate(DataEntry entry)
    {
        var entries = GetAll();
        var existing = entries.FirstOrDefault(e => e.Id == entry.Id);

        if (existing != null)
        {
            existing.Value = entry.Value;
        }
        else
        {
            entries.Add(entry);
        }

        Save(entries);
        return existing != null;
    }

    private void Save(List<DataEntry> entries)
    {
        var lines = entries.Select(e => $"{e.Id} = {e.Value}");
        File.WriteAllLines(_filePath, lines);
    }
}

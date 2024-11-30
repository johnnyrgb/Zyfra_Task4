using System;
using System.IO;

class Program
{
    const string filePath = "data.txt"; 

    // Парсинг строк файла
    static (int Id, string Value) ParseLine(string line)
    {
        var parts = line.Split('='); // Слева - Id, Справа - Значение
        if (parts.Length == 2)
        {
            return (int.Parse(parts[0].Trim()), parts[1].Trim());
        }
        else throw new Exception("Неверная запись!");
    }

    static (int id, string? oldValue, string newValue) CreateOrUpdate()
    {
        string? input;
        int id;
        Console.Write("Введите id элемента: ");
        do // Пока не будет введено число (Id)
        {
            input = Console.ReadLine();
        } while (input == null || !int.TryParse(input, out id));

        // Проверка существования файла, создание, если он не существует
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath)) { }  // Создание пустого файла
            Console.WriteLine("Файл не найден, создаем новый");
        }

        // Прочитаем все строки в память
        var lines = File.ReadAllLines(filePath);
        string? oldValue = null; // Старое значение (если найдено)
        string? newValue; // Новое значение

        // Находим позицию нужного элемента
        for (int i = 0; i < lines.Length; i++)
        {
            try
            {
                var entry = ParseLine(lines[i]);
                if (entry.Id == id)
                {
                    Console.WriteLine($"Id {id} найден!"); // Если Id найден, то запоминаем его значение
                    oldValue = entry.Value;

                    Console.Write("Введите новое значение: ");
                    do
                    {
                        newValue = Console.ReadLine();
                    } while (newValue == null);

                    // Обновляем строку
                    lines[i] = $"{id} = {newValue.Trim()}";
                    File.WriteAllLines(filePath, lines);  // Перезаписываем весь файл
                    return (id, oldValue, newValue);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        // Если элемент не найден, добавляем новую строку
        Console.Write("Введите значение: ");
        do
        {
            newValue = Console.ReadLine();
        } while (newValue == null);

        using (StreamWriter writer = new StreamWriter(filePath, append: true)) // Записываем строку с помощью потока записи в конец файла
        {
            writer.WriteLine($"{id} = {newValue.Trim()}");
        }

        return (id, oldValue, newValue);
    }

    static void Broadcast(int id, string? oldValue, string newValue) // Вещаем об обновлении / добавлении
    {
        string message = oldValue != null ? $"Изменение: {id} = {oldValue} --> {newValue}" : $"Добавление: {id} = {newValue}";
        Console.WriteLine(message);
    }

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите 'exit' для завершения или нажмите Enter, чтобы продолжить");
            string? command = Console.ReadLine();

            if (command?.ToLower() == "exit")
            {
                break; 
            }

            var (id, oldValue, newValue) = CreateOrUpdate();
            Broadcast(id, oldValue, newValue);

            Console.WriteLine("Изменение записано.");
        }
    }
}

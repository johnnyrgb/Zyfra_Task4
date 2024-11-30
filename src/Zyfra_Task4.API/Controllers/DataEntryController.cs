using Microsoft.AspNetCore.Mvc;
using Zyfra_Task4.BusinessLogic.DTOs;
using Zyfra_Task4.BusinessLogic.Interfaces;


namespace Zyfra_Task4.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataEntryController : ControllerBase
{
    private readonly IDataEntryService _dataEntryService;

    public DataEntryController(IDataEntryService dataEntryService)
    {
        _dataEntryService = dataEntryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _dataEntryService.GetAllDataEntriesAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entry = await _dataEntryService.GetDataEntryAsync(id);
        if (entry == null)
            return NotFound($"Элемент с Id {id} не найден.");

        return Ok(entry);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] DataEntryDTO? dataEntryDTO)
    {
        if (dataEntryDTO == null)
            return BadRequest("Некорректные данные.");

        // Если Id не передан, создаем новый объект
        if (dataEntryDTO.Id == 0)
        {
            try
            {
                // Проверяем, не существует ли уже записи с таким значением
                var existingEntry = await _dataEntryService.GetDataEntryAsync(dataEntryDTO.Value);
                if (existingEntry != null)
                {
                    return Conflict($"Запись с значением '{dataEntryDTO.Value}' уже существует.");
                }

                // Создаем и возвращаем новую запись
                await _dataEntryService.CreateDataEntryAsync(dataEntryDTO);
                var createdEntry = await _dataEntryService.GetDataEntryAsync(dataEntryDTO.Value);
                return CreatedAtAction(nameof(CreateOrUpdate), new { id = createdEntry.Id }, createdEntry);
            }
            catch (Exception ex)
            {
                // Логирование?
                return StatusCode(500, "Произошла ошибка при создании записи.");
            }
        }
        else
        {
            try
            {
                // Проверяем, не существует ли уже записи с таким Id
                var entry = await _dataEntryService.GetDataEntryAsync(dataEntryDTO.Id);
                if (entry == null)
                {
                    return NotFound($"Запись с Id {dataEntryDTO.Id} не найдена.");
                }

                await _dataEntryService.UpdateDataEntryAsync(dataEntryDTO);
                return Ok($"Элемент с Id {dataEntryDTO.Id} был обновлен.");
            }
            catch (Exception ex)
            {
                // Логирование?
                return StatusCode(500, "Произошла ошибка при обновлении записи.");
            }
        }
    }
}

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
    public IActionResult GetAll()
    {
        return Ok(_dataEntryService.GetAllDataEntriesAsync());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var entry = _dataEntryService.GetDataEntryAsync(id);
        if (entry == null)
            return NotFound($"Элемент с Id {id} не найден.");

        return Ok(entry);
    }

    [HttpPost]
    public IActionResult CreateOrUpdate([FromBody] DataEntryDTO? dataEntryDTO)
    {
        if (dataEntryDTO == null)
            return BadRequest("Некорректные данные.");

        var entry = _dataEntryService.GetDataEntryAsync(dataEntryDTO.Id);

        try
        {
            if (entry == null)
            {
                _dataEntryService.CreateDataEntryAsync(dataEntryDTO);
                return CreatedAtAction(nameof(CreateOrUpdate), entry);
            }
            else
            {
                _dataEntryService.UpdateDataEntryAsync(dataEntryDTO);
                return Ok($"Элемент с Id {dataEntryDTO.Id} был обновлен.");
            }
        }
        catch (Exception ex)
        {
            // Логирование?
            throw;
        }
    }
}

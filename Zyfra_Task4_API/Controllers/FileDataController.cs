using Zyfra_Task4_API.Services;
using Microsoft.AspNetCore.Mvc;
using Zyfra_Task4_API.Models;

namespace Zyfra_Task4_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileDataController : ControllerBase
{
    private readonly IFileDataService _fileDataService;

    public FileDataController(IFileDataService fileDataService)
    {
        _fileDataService = fileDataService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_fileDataService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var entry = _fileDataService.GetById(id);
        if (entry == null)
            return NotFound($"Элемент с Id {id} не найден.");

        return Ok(entry);
    }

    [HttpPost]
    public IActionResult CreateOrUpdate([FromBody] DataEntry? entry)
    {
        if (entry == null)
            return BadRequest("Некорректные данные.");

        var isUpdated = _fileDataService.CreateOrUpdate(entry);
        return isUpdated == true
            ? Ok($"Элемент с Id {entry.Id} был обновлен.")
            : CreatedAtAction(nameof(CreateOrUpdate), entry);
    }
}

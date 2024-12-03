using Moq;
using Zyfra_Task4.BusinessLogic.DTOs;
using Zyfra_Task4.BusinessLogic.Services;
using Zyfra_Task4.DataAccess.Entities;
using Zyfra_Task4.DataAccess.Interfaces;

namespace Zyfra_Task4.BusinessLogic.Tests;
public class DataEntryServiceTests
{
    private readonly Mock<IDbRepository> _repositoryMock;
    private readonly DataEntryService _service;

    public DataEntryServiceTests()
    {
        _repositoryMock = new Mock<IDbRepository>();
        _service = new DataEntryService(_repositoryMock.Object);
    }

    /// <summary>
    /// Возвращает запись по Id, если существует
    /// </summary>
    [Fact]
    public async Task GetDataEntryAsync_ById_ReturnsCorrectDTO()
    {
        // Arrange
        var dataEntry = new DataEntry { Id = 1, Value = "Тестовое значение" };
        _repositoryMock.Setup(repo => repo.DataEntry.GetByIdAsync(1))
            .ReturnsAsync(dataEntry);

        // Act
        var result = await _service.GetDataEntryAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dataEntry.Id, result!.Id);
        Assert.Equal(dataEntry.Value, result.Value);
    }

    /// <summary>
    /// Возвращает запись по значению, если существует
    /// </summary>
    [Fact]
    public async Task GetDataEntryAsync_ByValue_ReturnsCorrectDTO()
    {
        // Arrange
        var dataEntry = new DataEntry { Id = 1, Value = "Тестовое значение" };
        _repositoryMock.Setup(repo => repo.DataEntry.GetByValueAsync("Тестовое значение"))
            .ReturnsAsync(dataEntry);

        // Act
        var result = await _service.GetDataEntryAsync("Тестовое значение");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dataEntry.Id, result!.Id);
        Assert.Equal(dataEntry.Value, result.Value);
    }

    /// <summary>
    /// Возвращает список всех записей
    /// </summary>
    [Fact]
    public async Task GetAllDataEntriesAsync_ReturnsAllDTOs()
    {
        // Arrange
        var dataEntries = new List<DataEntry>
        {
            new DataEntry { Id = 1, Value = "Тестовое значение 1" },
            new DataEntry { Id = 2, Value = "Тестовое значение 2" }
        };
        _repositoryMock.Setup(repo => repo.DataEntry.GetAllAsync())
            .ReturnsAsync(dataEntries);

        // Act
        var result = await _service.GetAllDataEntriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, dto => dto.Value == "Тестовое значение 1");
        Assert.Contains(result, dto => dto.Value == "Тестовое значение 2");
    }

    /// <summary>
    /// Возвращает исключение при создании, если Value = null
    /// </summary>
    [Fact]
    public async Task CreateDataEntryAsync_InvalidData_ThrowsException()
    {
        // Arrange
        var dto = new DataEntryDTO { Value = "" };

        // Act Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateDataEntryAsync(dto));
    }

    /// <summary>
    /// Возвращает исключение при обновлении, если Value = null
    /// </summary>
    [Fact]
    public async Task UpdateDataEntryAsync_InvalidData_ThrowsException()
    {
        // Arrange
        var dto = new DataEntryDTO { Id = 1, Value = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateDataEntryAsync(dto));
    }
}

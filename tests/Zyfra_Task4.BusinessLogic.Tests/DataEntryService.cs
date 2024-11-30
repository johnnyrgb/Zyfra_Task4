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

    [Fact]
    public async Task GetDataEntryAsync_ShouldReturnDTO_WhenEntityExists()
    {
        // Arrange
        var dataEntry = new DataEntry { Id = 1, Value = "Test" };
        _repositoryMock.Setup(r => r.DataEntry.GetByIdAsync(1))
                       .ReturnsAsync(dataEntry);

        // Act
        var result = await _service.GetDataEntryAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dataEntry.Id, result.Id);
        Assert.Equal(dataEntry.Value, result.Value);
    }

    [Fact]
    public async Task GetDataEntryAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Arrange
        _repositoryMock.Setup(r => r.DataEntry.GetByIdAsync(1))
                       .ReturnsAsync((DataEntry?)null);

        // Act
        var result = await _service.GetDataEntryAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateDataEntryAsync_ShouldCallRepository_WhenValueIsValid()
    {
        // Arrange
        var dataEntryDTO = new DataEntryDTO { Id = 1, Value = "ValidValue" };

        // Act
        _repositoryMock.Setup(r => r.DataEntry.CreateAsync(It.IsAny<DataEntry>())).Returns(Task.CompletedTask);
        await _service.CreateDataEntryAsync(dataEntryDTO);

        // Assert
        _repositoryMock.Verify(r => r.DataEntry.CreateAsync(It.Is<DataEntry>(e =>
            e.Id == dataEntryDTO.Id && e.Value == dataEntryDTO.Value
        )), Times.Once);
    }

    [Fact]
    public async Task CreateDataEntryAsync_ShouldThrowException_WhenValueIsEmpty()
    {
        // Arrange
        var dataEntryDTO = new DataEntryDTO { Id = 1, Value = " " };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.CreateDataEntryAsync(dataEntryDTO));
    }

    [Fact]
    public async Task UpdateDataEntryAsync_ShouldCallRepository_WhenValueIsValid()
    {
        // Arrange
        var dataEntryDTO = new DataEntryDTO { Id = 1, Value = "UpdatedValue" };

        // Act
        await _service.UpdateDataEntryAsync(dataEntryDTO);

        // Assert
        _repositoryMock.Verify(r => r.DataEntry.UpdateAsync(It.Is<DataEntry>(e =>
            e.Id == dataEntryDTO.Id && e.Value == dataEntryDTO.Value
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateDataEntryAsync_ShouldThrowException_WhenValueIsEmpty()
    {
        // Arrange
        var dataEntryDTO = new DataEntryDTO { Id = 1, Value = "" };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            _service.UpdateDataEntryAsync(dataEntryDTO));
    }

    [Fact]
    public async Task DeleteDataEntryAsync_ShouldCallRepository()
    {
        // Arrange
        var id = 1;

        // Act
        await _service.DeleteDataEntryAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.DataEntry.DeleteByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetAllDataEntriesAsync_ShouldReturnMappedDTOs()
    {
        // Arrange
        var dataEntries = new List<DataEntry>
        {
            new DataEntry { Id = 1, Value = "Value1" },
            new DataEntry { Id = 2, Value = "Value2" }
        };

        _repositoryMock.Setup(r => r.DataEntry.GetAllAsync())
                       .ReturnsAsync(dataEntries);

        // Act
        var result = await _service.GetAllDataEntriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dataEntries.Count, result.Count());
        Assert.Contains(result, r => r.Id == 1 && r.Value == "Value1");
        Assert.Contains(result, r => r.Id == 2 && r.Value == "Value2");
    }
}

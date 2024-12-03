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
    /// ���������� ������ �� Id, ���� ����������
    /// </summary>
    [Fact]
    public async Task GetDataEntryAsync_ById_ReturnsCorrectDTO()
    {
        // Arrange
        var dataEntry = new DataEntry { Id = 1, Value = "�������� ��������" };
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
    /// ���������� ������ �� ��������, ���� ����������
    /// </summary>
    [Fact]
    public async Task GetDataEntryAsync_ByValue_ReturnsCorrectDTO()
    {
        // Arrange
        var dataEntry = new DataEntry { Id = 1, Value = "�������� ��������" };
        _repositoryMock.Setup(repo => repo.DataEntry.GetByValueAsync("�������� ��������"))
            .ReturnsAsync(dataEntry);

        // Act
        var result = await _service.GetDataEntryAsync("�������� ��������");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dataEntry.Id, result!.Id);
        Assert.Equal(dataEntry.Value, result.Value);
    }

    /// <summary>
    /// ���������� ������ ���� �������
    /// </summary>
    [Fact]
    public async Task GetAllDataEntriesAsync_ReturnsAllDTOs()
    {
        // Arrange
        var dataEntries = new List<DataEntry>
        {
            new DataEntry { Id = 1, Value = "�������� �������� 1" },
            new DataEntry { Id = 2, Value = "�������� �������� 2" }
        };
        _repositoryMock.Setup(repo => repo.DataEntry.GetAllAsync())
            .ReturnsAsync(dataEntries);

        // Act
        var result = await _service.GetAllDataEntriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, dto => dto.Value == "�������� �������� 1");
        Assert.Contains(result, dto => dto.Value == "�������� �������� 2");
    }

    /// <summary>
    /// ���������� ���������� ��� ��������, ���� Value = null
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
    /// ���������� ���������� ��� ����������, ���� Value = null
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

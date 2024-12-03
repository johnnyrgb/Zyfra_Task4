using Microsoft.AspNetCore.Mvc;
using Moq;
using Zyfra_Task4.API.Controllers;
using Zyfra_Task4.BusinessLogic.DTOs;
using Zyfra_Task4.BusinessLogic.Interfaces;

namespace Zyfra_Task4.API.Tests
{
    public class DataEntryControllerTests
    {
        private readonly Mock<IDataEntryService> _dataEntryServiceMock;
        private readonly DataEntryController _controller;

        public DataEntryControllerTests()
        {
            _dataEntryServiceMock = new Mock<IDataEntryService>();
            _controller = new DataEntryController(_dataEntryServiceMock.Object);
        }

        /// <summary>
        /// ���������� Ok � ��� ������
        /// </summary>
        [Fact]
        public async Task GetAll_ReturnsOkWithData()
        {
            // Arrange
            var entries = new List<DataEntryDTO>
            {
                new DataEntryDTO { Id = 1, Value = "Value1" },
                new DataEntryDTO { Id = 2, Value = "Value2" }
            };
            _dataEntryServiceMock.Setup(service => service.GetAllDataEntriesAsync())
                                 .ReturnsAsync(entries);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<DataEntryDTO>>(okResult.Value);
            Assert.Equal(2, data.Count());
        }

        /// <summary>
        /// ���������� Ok � ������ � ��������������� Id
        /// </summary>
        [Fact]
        public async Task GetById_ExistingId_ReturnsOkWithData()
        {
            // Arrange
            var entry = new DataEntryDTO { Id = 1, Value = "Value1" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                                 .ReturnsAsync(entry);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<DataEntryDTO>(okResult.Value);
            Assert.Equal(entry.Id, data.Id);
            Assert.Equal(entry.Value, data.Value);
        }

        /// <summary>
        /// ���������� NotFound
        /// </summary>
        [Fact]
        public async Task GetById_NonExistingId_IdNull_ReturnsNotFound()
        {
            // Arrange
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                                 .ReturnsAsync((DataEntryDTO?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("������� � Id 1 �� ������.", notFoundResult.Value);
        }

        /// <summary>
        /// ���������� Created, ���� Id �� �������, � ������ � ����� ��������� �� ����������
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOrUpdate_ValidNewEntry_IdNull_ReturnsCreated()
        {
            // Arrange
            var newEntry = new DataEntryDTO { Value = "����� ��������" };

            _dataEntryServiceMock.SetupSequence(service => service.GetDataEntryAsync("����� ��������"))
                .ReturnsAsync((DataEntryDTO?)null) // ������ ����� ���������� null
                .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "����� ��������" }); // ������ ����� ���������� ������

            _dataEntryServiceMock.Setup(service => service.CreateDataEntryAsync(newEntry))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(newEntry);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdData = Assert.IsType<DataEntryDTO>(createdResult.Value);
            Assert.Equal(1, createdData.Id);
            Assert.Equal("����� ��������", createdData.Value);
        }
        
        /// <summary>
        /// ���������� Conflict, ���� Id �� �������, � ������ � ����� ��������� ���������� 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOrUpdate_ExistingValue_ReturnsConflict()
        {
            // Arrange
            var newEntry = new DataEntryDTO { Value = "������������ ��������" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync("������������ ��������"))
                                 .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "������������ ��������" });

            // Act
            var result = await _controller.CreateOrUpdate(newEntry);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("������ � ��������� '������������ ��������' ��� ����������.", conflictResult.Value);
        }

        /// <summary>
        /// ��������� ��� ������������ ��������, ���� Id �������
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_UpdateExistingEntry_IdNotNull_ReturnsOk()
        {
            // Arrange
            var updatedEntry = new DataEntryDTO { Id = 1, Value = "����� ��������" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                                 .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "������ ��������" });
            _dataEntryServiceMock.Setup(service => service.UpdateDataEntryAsync(updatedEntry))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(updatedEntry);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("������� � Id 1 ��� ��������.", okResult.Value);
        }

        /// <summary>
        /// ��������� ��� ������������ ��������, ���� Id �������
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_UpdateNotExistingEntry_IdNotNull_ReturnsNotFound()
        {
            // Arrange
            var updatedEntry = new DataEntryDTO { Id = 1337, Value = "����� ��������" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "������ ��������" });
            _dataEntryServiceMock.Setup(service => service.UpdateDataEntryAsync(updatedEntry))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(updatedEntry);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"������ � Id {updatedEntry.Id} �� �������.", notFound.Value);
        }


        /// <summary>
        /// ������� ������, ���������� BadRequest
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_NullDTO_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateOrUpdate(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("������������ ������.", badRequestResult.Value);
        }
    }
}
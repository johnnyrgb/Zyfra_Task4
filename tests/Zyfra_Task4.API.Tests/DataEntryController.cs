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
        /// Возвращает Ok и все записи
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
        /// Возвращает Ok и запись с соответствующим Id
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
        /// Возвращает NotFound
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
            Assert.Equal("Элемент с Id 1 не найден.", notFoundResult.Value);
        }

        /// <summary>
        /// Возвращает Created, если Id не передан, а записи с таким значением не существует
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOrUpdate_ValidNewEntry_IdNull_ReturnsCreated()
        {
            // Arrange
            var newEntry = new DataEntryDTO { Value = "Новое значение" };

            _dataEntryServiceMock.SetupSequence(service => service.GetDataEntryAsync("Новое значение"))
                .ReturnsAsync((DataEntryDTO?)null) // Первый вызов возвращает null
                .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "Новое значение" }); // Второй вызов возвращает объект

            _dataEntryServiceMock.Setup(service => service.CreateDataEntryAsync(newEntry))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(newEntry);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdData = Assert.IsType<DataEntryDTO>(createdResult.Value);
            Assert.Equal(1, createdData.Id);
            Assert.Equal("Новое значение", createdData.Value);
        }
        
        /// <summary>
        /// Возвращает Conflict, если Id не передан, а запись с таким значением существует 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateOrUpdate_ExistingValue_ReturnsConflict()
        {
            // Arrange
            var newEntry = new DataEntryDTO { Value = "Существующее значение" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync("Существующее значение"))
                                 .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "Существующее значение" });

            // Act
            var result = await _controller.CreateOrUpdate(newEntry);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Запись с значением 'Существующее значение' уже существует.", conflictResult.Value);
        }

        /// <summary>
        /// Обновляет уже существующее значение, если Id передан
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_UpdateExistingEntry_IdNotNull_ReturnsOk()
        {
            // Arrange
            var updatedEntry = new DataEntryDTO { Id = 1, Value = "Новое значение" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                                 .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "Старое значение" });
            _dataEntryServiceMock.Setup(service => service.UpdateDataEntryAsync(updatedEntry))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(updatedEntry);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Элемент с Id 1 был обновлен.", okResult.Value);
        }

        /// <summary>
        /// Обновляет уже существующее значение, если Id передан
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_UpdateNotExistingEntry_IdNotNull_ReturnsNotFound()
        {
            // Arrange
            var updatedEntry = new DataEntryDTO { Id = 1337, Value = "Новое значение" };
            _dataEntryServiceMock.Setup(service => service.GetDataEntryAsync(1))
                .ReturnsAsync(new DataEntryDTO { Id = 1, Value = "Старое значение" });
            _dataEntryServiceMock.Setup(service => service.UpdateDataEntryAsync(updatedEntry))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateOrUpdate(updatedEntry);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Запись с Id {updatedEntry.Id} не найдена.", notFound.Value);
        }


        /// <summary>
        /// Нулевые данные, возвращает BadRequest
        /// </summary>
        [Fact]
        public async Task CreateOrUpdate_NullDTO_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.CreateOrUpdate(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Некорректные данные.", badRequestResult.Value);
        }
    }
}
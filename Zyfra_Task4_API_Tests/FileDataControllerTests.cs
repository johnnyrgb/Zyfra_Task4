using Microsoft.AspNetCore.Mvc;
using Moq;
using Zyfra_Task4_API.Controllers;
using Zyfra_Task4_API.Models;
using Zyfra_Task4_API.Services;

namespace Zyfra_Task4_API_Tests;

public class DataControllerTests
{
    [Fact]
    /* ÂÎÇÂÂĞÀÒ 200, ÅÑËÈ ÎÁÍÎÂËÅÍÀ ÑÓÙÅÑÒÂÓŞÙÀß ÇÀÏÈÑÜ */
    public void CreateOrUpdate_ShouldReturnOk_WhenElementUpdated()
    {
        // Arrange
        var mockService = new Mock<IFileDataService>();
        mockService.Setup(s => s.CreateOrUpdate(It.IsAny<DataEntry>())).Returns(true);

        var controller = new FileDataController(mockService.Object);
        var entry = new DataEntry { Id = 1, Value = "Îáíîâëåííîå çíà÷åíèå" };

        // Act
        var result = controller.CreateOrUpdate(entry);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"İëåìåíò ñ Id {entry.Id} áûë îáíîâëåí.", okResult.Value);
    }

    [Fact]
    /* ÂÎÇÂÂĞÀÒ 201, ÅÑËÈ ÄÎÁÀÂËÅÍÀ ÍÎÂÀß ÇÀÏÈÑÜ */
    public void CreateOrUpdate_ShouldReturnCreated_WhenElementAdded()
    {
        // Arrange
        var mockService = new Mock<IFileDataService>();
        mockService.Setup(s => s.CreateOrUpdate(It.IsAny<DataEntry>())).Returns(false);

        var controller = new FileDataController(mockService.Object);
        var entry = new DataEntry { Id = 2, Value = "Íîâîå çíà÷åíèå" };

        // Act
        var result = controller.CreateOrUpdate(entry);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(entry, createdResult.Value);
    }

    [Fact]
    /* ÂÎÇÂÂĞÀÒ 400, ÅÑËÈ ÂÂÅÄÅÍÛ ÍÅÊÎĞĞÅÊÒÍÛÅ ÄÀÍÍÛÅ (NULL) */
    public void CreateOrUpdate_ShouldReturnBadRequest_WhenEntryIsNull()
    {
        // Arrange
        var mockService = new Mock<IFileDataService>();
        var controller = new FileDataController(mockService.Object);

        // Act
        var result = controller.CreateOrUpdate(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Íåêîğğåêòíûå äàííûå.", badRequestResult.Value);
    }
}
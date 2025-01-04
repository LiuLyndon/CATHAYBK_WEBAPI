using BasicEIP_Core.NLog;
using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Bitcoin;
using CATHAYBK_Service.Service;
using CATHAYBK_WEBAPI.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CATHAYBK_WEBAPI.Tests.Controllers
{
    public class BitcoinControllerTests
    {
        private readonly Mock<BitcoinService> _mockService;
        private readonly Mock<IAppLogger<BitcoinController>> _mockLogger;
        private readonly BitcoinController _controller;

        public BitcoinControllerTests()
        {
            // Mock Service 和 Logger
            _mockService = new Mock<BitcoinService>();
            _mockLogger = new Mock<IAppLogger<BitcoinController>>();

            // 建立 Controller
            _controller = new BitcoinController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllBitcoinsOrderedByCode()
        {
            // Arrange
            var mockData = new List<tblBitcoin>
            {
                new tblBitcoin { Id = 1, Code = "BTC" },
                new tblBitcoin { Id = 2, Code = "ETH" }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<tblBitcoin>>(okResult.Value);

            value.Should().BeEquivalentTo(mockData.OrderBy(b => b.Code));
        }

        [Fact]
        public async Task GetById_ShouldReturnBitcoin_WhenBitcoinExists()
        {
            // Arrange
            var mockBitcoin = new tblBitcoin { Id = 1, Code = "BTC" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(mockBitcoin);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<tblBitcoin>(okResult.Value);

            Assert.Equal(mockBitcoin, value);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenBitcoinDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((tblBitcoin)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedResponse_WhenValidRequest()
        {
            // Arrange
            var request = new CreateBitcoinRequest
            {
                Code = "BTC",
                Symbol = "₿",
                Rate = 50000,
                Description = "Bitcoin",
                RateFloat = 50000
            };

            var createdBitcoin = new tblBitcoin
            {
                Id = 1,
                Code = request.Code,
                Symbol = request.Symbol,
                Rate = request.Rate,
                Description = request.Description,
                RateFloat = request.RateFloat
            };

            _mockService.Setup(s => s.AddAsync(It.IsAny<tblBitcoin>()))
                        .Callback<tblBitcoin>(b => b.Id = 1)
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var value = Assert.IsAssignableFrom<tblBitcoin>(createdAtActionResult.Value);

            Assert.Equal(1, value.Id);
            Assert.Equal(request.Code, value.Code);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenBitcoinExists()
        {
            // Arrange
            var bitcoin = new tblBitcoin { Id = 1, Code = "BTC" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(bitcoin);
            _mockService.Setup(s => s.DeleteAsync(bitcoin)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenBitcoinDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((tblBitcoin)null);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

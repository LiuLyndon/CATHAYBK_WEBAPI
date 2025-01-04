using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Coindesk;
using CATHAYBK_Service.Service;
using CATHAYBK_WEBAPI.Base;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CATHAYBK_WEBAPI.Tests.Controllers
{
    public class CoindeskControllerTests
    {
        private readonly Mock<CoindeskService> _mockCoindeskService;
        private readonly Mock<CurrencyService> _mockCurrencyService;
        private readonly Mock<BitcoinService> _mockBitcoinService;
        private readonly CoindeskController _controller;

        public CoindeskControllerTests()
        {
            _mockCoindeskService = new Mock<CoindeskService>();
            _mockCurrencyService = new Mock<CurrencyService>();
            _mockBitcoinService = new Mock<BitcoinService>();

            _controller = new CoindeskController(
                _mockCoindeskService.Object,
                _mockCurrencyService.Object,
                _mockBitcoinService.Object
            );
        }

        [Fact]
        public async Task FetchAndSave_ShouldFetchAndSaveBitcoinData()
        {
            // Arrange
            var mockResponse = new CoindeskResponse
            {
                Bpi = new Dictionary<string, CurrencyInfo>
                {
                    { "USD", new CurrencyInfo { Code = "USD", Symbol = "$", Rate = "50,000", Description = "United States Dollar", rate_float = 50000 } },
                    { "EUR", new CurrencyInfo { Code = "EUR", Symbol = "€", Rate = "45,000", Description = "Euro", rate_float = 45000 } }
                }
            };

            _mockCoindeskService.Setup(s => s.GetCurrentPriceAsync()).ReturnsAsync(mockResponse);

            // Act
            var result = await _controller.FetchAndSave();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Data fetched and saved successfully.", okResult.Value);

            _mockBitcoinService.Verify(s => s.AddAsync(It.IsAny<tblBitcoin>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetAllCurrencies_ShouldReturnAllCurrenciesOrderedByCode()
        {
            // Arrange
            var mockCurrencies = new List<tblCurrency>
            {
                new tblCurrency { Id = 1, Code = "USD", Name = "United States Dollar" },
                new tblCurrency { Id = 2, Code = "EUR", Name = "Euro" }
            };

            _mockCurrencyService.Setup(s => s.GetAllAsync()).ReturnsAsync(mockCurrencies);

            // Act
            var result = await _controller.GetAllCurrencies();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<tblCurrency>>(okResult.Value);

            Assert.Equal(mockCurrencies.OrderBy(c => c.Code), value);
        }

        [Fact]
        public async Task AddCurrency_ShouldAddNewCurrency()
        {
            // Arrange
            var mockRequest = new CurrencyRequert
            {
                Code = "JPY",
                Name = "Japanese Yen"
            };

            _mockCurrencyService.Setup(s => s.AddAsync(mockRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddCurrency(mockRequest);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(mockRequest, createdResult.Value);
        }

        [Fact]
        public async Task UpdateCurrency_ShouldReturnNoContent_WhenCurrencyUpdated()
        {
            // Arrange
            var mockRequest = new CurrencyRequert
            {
                Code = "JPY",
                Name = "Japanese Yen"
            };

            _mockCurrencyService.Setup(s => s.UpdateAsync(1, mockRequest)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCurrency(1, mockRequest);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateCurrency_ShouldReturnNotFound_WhenCurrencyDoesNotExist()
        {
            // Arrange
            var mockRequest = new CurrencyRequert
            {
                Code = "JPY",
                Name = "Japanese Yen"
            };

            _mockCurrencyService.Setup(s => s.UpdateAsync(1, mockRequest))
                .Throws(new KeyNotFoundException("Currency not found"));

            // Act
            var result = await _controller.UpdateCurrency(1, mockRequest);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Currency not found", ((dynamic)notFoundResult.Value).message);
        }

        [Fact]
        public async Task DeleteCurrency_ShouldReturnNoContent_WhenCurrencyDeleted()
        {
            // Arrange
            _mockCurrencyService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCurrency(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCurrency_ShouldReturnNotFound_WhenCurrencyDoesNotExist()
        {
            // Arrange
            _mockCurrencyService.Setup(s => s.DeleteAsync(1))
                .Throws(new KeyNotFoundException("Currency not found"));

            // Act
            var result = await _controller.DeleteCurrency(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Currency not found", ((dynamic)notFoundResult.Value).message);
        }
    }
}


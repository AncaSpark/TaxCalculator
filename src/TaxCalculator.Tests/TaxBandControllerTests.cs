//using Moq;
//using TaxCalculator.Domain.Interfaces;
//using TaxCalculator.Domain.Models;

//namespace TaxCalculator.Tests
//{
//    [TestFixture]
//    public class TaxBandControllerTests
//    {
//        private Mock<ITaxBandRepository> _taxBandRepositoryMock;
//        private TaxBandController _taxBandController;

//        [SetUp]
//        public void SetUp()
//        {
//            _taxBandRepositoryMock = new Mock<ITaxBandRepository>();
//            _taxBandController = new TaxBandController(_taxBandRepositoryMock.Object);
//        }

//        [Test]
//        public async Task GetAllTaxBands_ShouldReturnOkResult_WithListOfTaxBands()
//        {
//            // Arrange
//            var taxBands = new List<TaxBand>
//            {
//                new TaxBand { Id = 1, LowerLimit = 0, UpperLimit = 50000, TaxRate = 10 },
//                new TaxBand { Id = 2, LowerLimit = 50000, UpperLimit = 100000, TaxRate = 20 }
//            };
//            _taxBandRepositoryMock.Setup(repo => repo.GetAllTaxBandsAsync()).ReturnsAsync(taxBands);

//            // Act
//            var result = await _taxBandController.GetAllTaxBands();

//            // Assert
//            Assert.IsInstanceOf<OkObjectResult>(result.Result);
//            var okResult = result.Result as OkObjectResult;
//            Assert.IsNotNull(okResult);
//            Assert.AreEqual(taxBands, okResult.Value);
//        }

//        [Test]
//        public async Task GetTaxBandById_ShouldReturnOkResult_WithTaxBand()
//        {
//            // Arrange
//            var taxBand = new TaxBand { Id = 1, LowerLimit = 0, UpperLimit = 50000, TaxRate = 10 };
//            _taxBandRepositoryMock.Setup(repo => repo.GetTaxBandByIdAsync(1)).ReturnsAsync(taxBand);

//            // Act
//            var result = await _taxBandController.GetTaxBandById(1);

//            // Assert
//            Assert.IsInstanceOf<OkObjectResult>(result.Result);
//            var okResult = result.Result as OkObjectResult;
//            Assert.IsNotNull(okResult);
//            Assert.AreEqual(taxBand, okResult.Value);
//        }

//        [Test]
//        public async Task GetTaxBandById_ShouldReturnNotFound_WhenTaxBandDoesNotExist()
//        {
//            // Arrange
//            _taxBandRepositoryMock.Setup(repo => repo.GetTaxBandByIdAsync(1)).ReturnsAsync((TaxBand)null);

//            // Act
//            var result = await _taxBandController.GetTaxBandById(1);

//            // Assert
//            Assert.IsInstanceOf<NotFoundResult>(result.Result);
//        }

//        [Test]
//        public async Task AddTaxBand_ShouldReturnCreatedAtActionResult()
//        {
//            // Arrange
//            var taxBand = new TaxBand { Id = 1, LowerLimit = 0, UpperLimit = 50000, TaxRate = 10 };

//            // Act
//            var result = await _taxBandController.AddTaxBand(taxBand);

//            // Assert
//            Assert.IsInstanceOf<CreatedAtActionResult>(result);
//            var createdAtActionResult = result as CreatedAtActionResult;
//            Assert.IsNotNull(createdAtActionResult);
//            Assert.AreEqual("GetTaxBandById", createdAtActionResult.ActionName);
//            Assert.AreEqual(taxBand.Id, createdAtActionResult.RouteValues["id"]);
//            Assert.AreEqual(taxBand, createdAtActionResult.Value);
//        }

//        [Test]
//        public async Task UpdateTaxBand_ShouldReturnNoContentResult()
//        {
//            // Arrange
//            var taxBand = new TaxBand { Id = 1, LowerLimit = 0, UpperLimit = 50000, TaxRate = 10 };

//            // Act
//            var result = await _taxBandController.UpdateTaxBand(1, taxBand);

//            // Assert
//            Assert.IsInstanceOf<NoContentResult>(result);
//        }

//        [Test]
//        public async Task UpdateTaxBand_ShouldReturnBadRequest_WhenIdDoesNotMatch()
//        {
//            // Arrange
//            var taxBand = new TaxBand { Id = 1, LowerLimit = 0, UpperLimit = 50000, TaxRate = 10 };

//            // Act
//            var result = await _taxBandController.UpdateTaxBand(2, taxBand);

//            // Assert
//            Assert.IsInstanceOf<BadRequestResult>(result);
//        }

//        [Test]
//        public async Task DeleteTaxBand_ShouldReturnNoContentResult()
//        {
//            // Arrange
//            _taxBandRepositoryMock.Setup(repo => repo.DeleteTaxBandAsync(1)).Returns(Task.CompletedTask);

//            // Act
//            var result = await _taxBandController.DeleteTaxBand(1);

//            // Assert
//            Assert.IsInstanceOf<NoContentResult>(result);
//        }
//    }
//}

using Moq;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using TaxCalculator.Application.Services;
using Microsoft.Extensions.Logging;
using TaxCalculator.Domain.Exceptions;


namespace TaxCalculator.Tests.Services
{
    [TestFixture]
    public class TaxCalculatorServiceTests
    {
        private Mock<ITaxBandRepository> _taxBandRepositoryMock;
        private Mock<ILogger<TaxCalculatorService>> _loggerMock;
        private TaxCalculatorService _taxCalculatorService;

        [SetUp]
        public void SetUp()
        {
            _taxBandRepositoryMock = new Mock<ITaxBandRepository>();
            _loggerMock = new Mock<ILogger<TaxCalculatorService>>();
            _taxCalculatorService = new TaxCalculatorService(_taxBandRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task CalculateTaxAsync_ShouldReturnCorrectTaxCalculationResult()
        {
            // Arrange
            var taxBands = new List<TaxBand>
            {
                new TaxBand { LowerLimit = 0m, UpperLimit = 5000.00m, TaxRate = 0 },
                new TaxBand { LowerLimit = 5000.00m, UpperLimit = 20000.00m, TaxRate = 20 },
                new TaxBand { LowerLimit = 20000.00m, UpperLimit = null, TaxRate = 40 }
            };
            _taxBandRepositoryMock.Setup(repo => repo.GetAllTaxBandsAsync()).ReturnsAsync(taxBands);

            var grossAnnualSalary = 10000m;

            // Act
            var result = await _taxCalculatorService.CalculateTaxAsync(grossAnnualSalary);

            // Assert
            Assert.AreEqual(grossAnnualSalary, result.GrossAnnualSalary);
            Assert.AreEqual(1000m, result.AnnualTaxPaid);
        }


        [Test]
        public void CalculateTaxAsync_ShouldThrowArgumentException_WhenSalaryIsNegative()
        {
            // Arrange
            var grossAnnualSalary = -1000m;

            // Act & Assert
            var ex = Assert.ThrowsAsync<TaxCalculationException>(() => _taxCalculatorService.CalculateTaxAsync(grossAnnualSalary));
            Assert.AreEqual("An error occurred while calculating tax", ex.Message);
        }

        [Test]
        public void CalculateTaxAsync_ShouldThrowInvalidTaxBandException_WhenNoTaxBandsFound()
        {
            // Arrange
            _taxBandRepositoryMock.Setup(repo => repo.GetAllTaxBandsAsync()).ReturnsAsync(new List<TaxBand>());

            var grossAnnualSalary = 50000m;

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidTaxBandException>(() => _taxCalculatorService.CalculateTaxAsync(grossAnnualSalary));
            Assert.AreEqual("No tax bands found in the system", ex.Message);
        }


    }
}
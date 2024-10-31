using Microsoft.Extensions.Logging;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;


namespace TaxCalculator.Application.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly ITaxBandRepository _taxBandRepository;
        private readonly ILogger<TaxCalculatorService> _logger;

        public TaxCalculatorService(ITaxBandRepository taxBandRepository, ILogger<TaxCalculatorService> logger)
        {
            _taxBandRepository = taxBandRepository;
            _logger = logger;
        }

        public async Task<TaxCalculationResult> CalculateTaxAsync(decimal grossAnnualSalary)
        {
            try
            {
                _logger.LogInformation("Starting tax calculation for salary: {Salary}", grossAnnualSalary);

                if (grossAnnualSalary < 0)
                {
                    throw new ArgumentException("Salary cannot be negative", nameof(grossAnnualSalary));
                }

                var taxBands = await _taxBandRepository.GetAllTaxBandsAsync();

                if (!taxBands.Any())
                {
                    throw new InvalidTaxBandException("No tax bands found in the system");
                }

                decimal totalTax = 0;

                foreach (var band in taxBands)
                {
                    if (grossAnnualSalary <= band.LowerLimit)
                        continue;

                    decimal taxableAmountInBand;

                    if (band.UpperLimit.HasValue)
                    {
                        taxableAmountInBand = Math.Min(
                            grossAnnualSalary - band.LowerLimit,
                            band.UpperLimit.Value - band.LowerLimit
                        );
                    }
                    else
                    {
                        taxableAmountInBand = grossAnnualSalary - band.LowerLimit;
                    }

                    var taxInBand = taxableAmountInBand * (band.TaxRate / 100m);
                    totalTax += taxInBand;

                    _logger.LogDebug(
                        "Tax calculated for band {BandName}: Taxable Amount = {TaxableAmount}, Tax Rate = {TaxRate}%, Tax = {Tax}",
                        band.Name, taxableAmountInBand, band.TaxRate, taxInBand
                    );
                }

                var result = new TaxCalculationResult
                {
                    GrossAnnualSalary = grossAnnualSalary,
                    AnnualTaxPaid = totalTax
                };

                _logger.LogInformation(
                    "Tax calculation completed. Gross Salary: {GrossSalary}, Total Tax: {TotalTax}",
                    grossAnnualSalary, totalTax
                );

                return result;
            }
            catch (InvalidTaxBandException ex)
            {
                _logger.LogError(ex, "Invalid tax band configuration encountered");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating tax for salary {Salary}", grossAnnualSalary);
                throw new TaxCalculationException("An error occurred while calculating tax", ex);
            }
        }
    }
}

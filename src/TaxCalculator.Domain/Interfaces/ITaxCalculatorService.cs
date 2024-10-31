using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxCalculatorService
    {
        Task<TaxCalculationResult> CalculateTaxAsync(decimal grossAnnualSalary);
    }
}

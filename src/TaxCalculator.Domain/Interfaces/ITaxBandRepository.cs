using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxBandRepository
    {
        Task<IEnumerable<TaxBand>> GetAllTaxBandsAsync();
    }
}

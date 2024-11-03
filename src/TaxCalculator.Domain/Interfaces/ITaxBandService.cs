using TaxCalculator.Domain.Models;

namespace TaxCalculator.Domain.Interfaces
{
    public interface ITaxBandService
    {
        Task<List<TaxBand>> FindContainingIntervals(TaxBand taxBand);
    }
}

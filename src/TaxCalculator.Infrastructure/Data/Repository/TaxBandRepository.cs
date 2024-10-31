using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Infrastructure.Data.Repository
{
    public class TaxBandRepository : ITaxBandRepository
    {
        private readonly TaxCalculatorDbContext _context;
        private readonly ILogger<TaxBandRepository> _logger;

        public TaxBandRepository(TaxCalculatorDbContext context, ILogger<TaxBandRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<TaxBand>> GetAllTaxBandsAsync()
        {
            try
            {
                return await _context.TaxBands
                    .AsNoTracking()
                    .OrderBy(x => x.LowerLimit)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tax bands from database");
                throw;
            }
        }

        public async Task<TaxBand> AddTaxBandAsync(TaxBand taxBand)
        {
            try
            {
                _context.TaxBands.Add(taxBand);
                await _context.SaveChangesAsync();
                return taxBand;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tax band to database");
                throw;
            }
        }

        public async Task UpdateTaxBandAsync(TaxBand taxBand)
        {
            try
            {
                _context.Entry(taxBand).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tax band in database");
                throw;
            }
        }
    }
}
using Microsoft.Extensions.Logging;

using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Application.Services
{
    public class TaxBandService : ITaxBandService
    {
        private readonly ITaxBandRepository _taxBandRepository;
        private readonly ILogger<TaxBandService> _logger;

        public TaxBandService(ITaxBandRepository taxBandRepository, ILogger<TaxBandService> logger)
        {
            _taxBandRepository = taxBandRepository;
            _logger = logger;
        }


        public async Task<List<TaxBand>> FindContainingIntervals(TaxBand taxBand)
        {
            var intervals = await _taxBandRepository.GetAllTaxBandsAsync();

            if (!intervals.Any())
                return new List<TaxBand>();

            var result = new List<TaxBand>();

            foreach (var interval in intervals)
            {
                // If target starts after interval end (if interval has an end), skip this interval
                if (interval.UpperLimit.HasValue && taxBand.LowerLimit > interval.UpperLimit.Value)
                    continue;

                // If target ends (if it has an end) before interval start, we can stop searching
                // as intervals are sorted
                if (taxBand.UpperLimit.HasValue && taxBand.UpperLimit.Value < interval.LowerLimit)
                    break;

                // At this point, we know:
                // 1. Either interval is infinite (End is null) OR target starts before/at interval end
                // 2. Either target is infinite OR target ends after/at interval start

                // Add interval to result if any of these conditions are true:
                // 1. Interval contains target start point
                // 2. Target contains interval start point (for overlapping)
                // 3. Both intervals are infinite
                if (interval.ContainsPoint(taxBand.LowerLimit) ||
                    (taxBand.UpperLimit.HasValue && interval.LowerLimit <= taxBand.UpperLimit.Value) ||
                    (!interval.UpperLimit.HasValue && !taxBand.UpperLimit.HasValue))
                {
                    result.Add(interval);
                }
            }

            return result;
        }

    }
}

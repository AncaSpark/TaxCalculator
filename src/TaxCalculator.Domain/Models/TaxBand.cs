namespace TaxCalculator.Domain.Models
{
    public class TaxBand
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal? UpperLimit { get; set; }
        public int TaxRate { get; set; }


        public bool ContainsPoint(decimal point)
        {
            return LowerLimit <= point && (!UpperLimit.HasValue || point <= UpperLimit.Value);
        }
        public bool Contains(TaxBand other)
        {
            // If this interval has no end (infinite), it contains the other interval
            // if its start is less than or equal to the other's start
            if (!UpperLimit.HasValue)
                return LowerLimit <= other.LowerLimit;

            // If the other interval has no end, this finite interval cannot contain it
            if (!other.UpperLimit.HasValue)
                return false;

            // Both intervals have ends, check regular containment
            return LowerLimit <= other.LowerLimit && UpperLimit.Value >= other.UpperLimit.Value;
        }
        public bool Overlaps(TaxBand other)
        {
            // If either interval has no end (infinite), they overlap if the start of each
            // is less than or equal to the end of the other (if it exists)
            if (!UpperLimit.HasValue)
                return !other.UpperLimit.HasValue || LowerLimit <= other.UpperLimit.Value;

            if (!other.UpperLimit.HasValue)
                return other.LowerLimit <= UpperLimit.Value;

            // Both intervals have ends, check regular overlap
            return LowerLimit <= other.UpperLimit.Value && UpperLimit.Value >= other.LowerLimit;
        }
    }
}


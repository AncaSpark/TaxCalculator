namespace TaxCalculator.Domain.Exceptions
{
    public class TaxCalculationException : Exception
    {
        public TaxCalculationException(string message) : base(message) { }
        public TaxCalculationException(string message, Exception innerException) : base(message, innerException) { }
    }
}

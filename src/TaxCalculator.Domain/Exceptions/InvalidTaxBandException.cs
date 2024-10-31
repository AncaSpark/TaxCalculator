namespace TaxCalculator.Domain.Exceptions
{
    public class InvalidTaxBandException : Exception
    {
        public InvalidTaxBandException(string message) : base(message) { }
    }
}

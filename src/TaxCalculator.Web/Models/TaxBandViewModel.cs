using System.ComponentModel.DataAnnotations;

namespace TaxCalculator.Web.Models
{
    public class TaxBandViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Lower limit is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Lower limit must be a non-negative number.")]
        public decimal LowerLimit { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Upper limit must be a non-negative number.")]
        public decimal? UpperLimit { get; set; }

        [Required(ErrorMessage = "Tax rate is required.")]
        [Range(0, 100, ErrorMessage = "Tax rate must be between 0 and 100.")]
        public int TaxRate { get; set; }
    }
}



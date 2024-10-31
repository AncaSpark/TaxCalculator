using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaxCalculator.Domain.Exceptions;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Web.Models;

namespace TaxCalculator.Web.Controllers
{
    public class TaxCalculatorController : Controller
    {
        private readonly ITaxCalculatorService _taxCalculatorService;
        private readonly ILogger<TaxCalculatorController> _logger;

        public TaxCalculatorController(ITaxCalculatorService taxCalculatorService, ILogger<TaxCalculatorController> logger)
        {
            _taxCalculatorService = taxCalculatorService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(decimal grossAnnualSalary)
        {
            try
            {
                _logger.LogInformation("Calculating tax for salary: {Salary}", grossAnnualSalary);

                if (grossAnnualSalary < 0)
                {
                    ModelState.AddModelError("grossAnnualSalary", "Salary cannot be negative");
                    return View("Index");
                }

                var result = await _taxCalculatorService.CalculateTaxAsync(grossAnnualSalary);
                return View("Index", result);
            }
            catch (TaxCalculationException ex)
            {
                _logger.LogError(ex, "Error calculating tax for salary {Salary}", grossAnnualSalary);
                ModelState.AddModelError("", "An error occurred while calculating tax. Please try again later.");
                return View("Index");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

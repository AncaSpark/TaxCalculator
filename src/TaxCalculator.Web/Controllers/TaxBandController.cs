using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Domain.Models;
using TaxCalculator.Web.Models;

namespace TaxCalculator.Web.Controllers
{
    public class TaxBandController : Controller
    {
        private readonly ITaxBandRepository _taxBandRepository;
        private readonly ITaxBandService _taxBandService;
        private readonly ILogger<TaxBandController> _logger;
        public TaxBandController(ITaxBandRepository taxBandRepository, ITaxBandService taxBandService, ILogger<TaxBandController> logger)
        {
            _taxBandRepository = taxBandRepository;
            _taxBandService = taxBandService;
            _logger = logger;
        }

        // GET: TaxBandController
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Fetching all tax bands.");
            var taxBands = await _taxBandRepository.GetAllTaxBandsAsync();
            var model = new List<TaxBandViewModel>();

            foreach (var taxBand in taxBands)
            {
                model.Add(new TaxBandViewModel
                {
                    Id = taxBand.Id,
                    LowerLimit = taxBand.LowerLimit,
                    UpperLimit = taxBand.UpperLimit,
                    TaxRate = taxBand.TaxRate
                });
            }

            return View(model);
        }

        // GET: TaxBandController/Details/3
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Fetching details for tax band with ID {id}.");

            if (id <= 0)
            {
                _logger.LogWarning($"Invalid tax band ID: {id}");
                return BadRequest("Invalid tax band ID.");
            }


            var taxBand = await _taxBandRepository.GetTaxBandByIdAsync(id);
            if (taxBand == null)
            {
                _logger.LogWarning($"Tax band with ID {id} not found.");
                return NotFound();
            }

            var model = new TaxBandViewModel
            {
                Id = taxBand.Id,
                LowerLimit = taxBand.LowerLimit,
                UpperLimit = taxBand.UpperLimit,
                TaxRate = taxBand.TaxRate
            };

            return View(model);
        }

        // GET: TaxBandController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TaxBandController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxBandViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Custom validation logic
                    if (model.LowerLimit >= model.UpperLimit)
                    {
                        ModelState.AddModelError("UpperLimit", "Upper limit must be greater than lower limit.");
                        return View(model);
                    }

                    var taxBand = new TaxBand
                    {
                        Name = model.Name,
                        LowerLimit = model.LowerLimit,
                        UpperLimit = model.UpperLimit,
                        TaxRate = model.TaxRate
                    };

                    var intervals = await _taxBandService.FindContainingIntervals(taxBand);
                    if (intervals.Any())
                    {
                        ModelState.AddModelError(string.Empty, "Tax band overlaps with existing tax bands.");
                        return View(model);
                    }   

                    await _taxBandRepository.AddTaxBandAsync(taxBand);
                    _logger.LogInformation("Tax band created successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var err = "Error occurred while creating tax band.";
                    _logger.LogError(ex, err);
                    ModelState.AddModelError(string.Empty, err);
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: TaxBandController/Edit/3
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Fetching tax band with ID {id} for editing.");
            var taxBand = await _taxBandRepository.GetTaxBandByIdAsync(id);
            if (taxBand == null)
            {
                _logger.LogWarning($"Tax band with ID {id} not found.");
                return NotFound();
            }

            var model = new TaxBandViewModel
            {
                Id = taxBand.Id,
                Name = taxBand.Name,
                LowerLimit = taxBand.LowerLimit,
                UpperLimit = taxBand.UpperLimit,
                TaxRate = taxBand.TaxRate
            };

            return View(model);
        }

        // POST: TaxBandController/Edit/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaxBandViewModel model)
        {
            if (id != model.Id)
            {
                _logger.LogWarning($"Tax band ID mismatch: {id} != {model.Id}");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Custom validation logic
                    if (model.LowerLimit >= model.UpperLimit)
                    {
                        ModelState.AddModelError("UpperLimit", "Upper limit must be greater than lower limit.");
                        return View(model);
                    } 
                    var taxBand = new TaxBand
                    {
                        Id = model.Id,
                        Name = model.Name,
                        LowerLimit = model.LowerLimit,
                        UpperLimit = model.UpperLimit,
                        TaxRate = model.TaxRate
                    };

                    await _taxBandRepository.UpdateTaxBandAsync(taxBand);
                    _logger.LogInformation($"Tax band with ID {id} updated successfully.");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while updating tax band with ID {id}.");
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: TaxBandController/Delete/3
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Fetching tax band with ID {id} for deletion.");
            var taxBand = await _taxBandRepository.GetTaxBandByIdAsync(id);
            if (taxBand == null)
            {
                _logger.LogWarning($"Tax band with ID {id} not found.");
                return NotFound();
            }

            var model = new TaxBandViewModel
            {
                Id = taxBand.Id,
                Name = taxBand.Name,
                LowerLimit = taxBand.LowerLimit,
                UpperLimit = taxBand.UpperLimit,
                TaxRate = taxBand.TaxRate
            };

            return View(model);
        }

        // POST: TaxBandController/Delete/3
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _taxBandRepository.DeleteTaxBandAsync(id);
                _logger.LogInformation($"Tax band with ID {id} deleted successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting tax band with ID {id}.");
                return View();
            }
        }
    }
}

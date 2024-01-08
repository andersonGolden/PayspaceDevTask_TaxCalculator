using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Domain.Entity;
using TaxCalculator.Models;

namespace TaxCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaxCalculatorService _taxCalculator;

        public HomeController(ILogger<HomeController> logger, ITaxCalculatorService taxCalculator)
        {
            _logger = logger;
            _taxCalculator = taxCalculator;
        }

        public async Task<IActionResult> Index()
        {
            var model = new TaxCalculatorViewModel();
            model.rates = new List<TaxRates>();
            var ratesResp = await _taxCalculator.GetComputedRates();
            if (ratesResp.succeeded)
            {
                model.rates = ratesResp.data; 
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ComputeTax(TaxCalculatorViewModel model)
        {
            var computeResult = await _taxCalculator.ComputeTax(model.request.postalCode, model.request.annualIncome);
            if (computeResult.succeeded)
            {
                TempData["ComputeMessage"] = $"Your annual tax is: {computeResult.data.ToString("N2")}";
            }
            else
            {
                TempData["ComputeMessage"] = $"Error: {computeResult.message}";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRate(long id)
        {
            var deleteRate = await _taxCalculator.DeactivateRate(id);
            return Json(new { success = deleteRate.succeeded });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
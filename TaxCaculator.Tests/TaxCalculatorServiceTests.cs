using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Core.Services;
using TaxCalculator.Core.Utilities;
using TaxCalculator.Domain.Persistence;

namespace TaxCalculator.Tests
{
    public class TaxCalculatorServiceTests
    {
        private readonly IConfiguration _config;
        private readonly DataContext _dbContext;
        private ITaxCalculatorService _taxCalculatorService;

        public TaxCalculatorServiceTests()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            var db = _config.GetConnectionString("DefaultConnection");
            var options = new DbContextOptionsBuilder<DataContext>().UseSqlServer(db).Options;
            _dbContext = new DataContext(options);
            _taxCalculatorService = new TaxCalculatorService(_dbContext);
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeUpTo8350_ReturnsCorrectTax()
        {
            decimal annualIncome = 8300; 
            string postalCode = "7441";
            decimal expectedTax = annualIncome * 0.10m; // Expected tax calculation for the income in the first bracket (i.e 10%)
            
            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeUpTo33950_ReturnsCorrectTax()
        {
            decimal annualIncome = 19351;
            string postalCode = "1000";
            decimal expectedTax = 8350 * 0.10m + (annualIncome - 8350) * 0.15m; // Expected tax calculation for the income in the 2nd bracket (i.e 15%)

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeUpTo82250_ReturnsCorrectTax()
        {
            decimal annualIncome = 35353;
            string postalCode = "7441";
            decimal expectedTax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (annualIncome - 33950) * 0.25m; // Expected tax calculation for the income in the 3rd bracket (i.e 25%)

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeUpTo171550_ReturnsCorrectTax()
        {
            decimal annualIncome = 95353;
            string postalCode = "1000";
            decimal expectedTax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (annualIncome - 82250) * 0.28m; // Expected tax calculation for the income in the 4th bracket (i.e 28%)

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeUpTo372950_ReturnsCorrectTax()
        {
            decimal annualIncome = 271551;
            string postalCode = "7441";
            decimal expectedTax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (annualIncome - 171550) * 0.33m; // Expected tax calculation for the income in the 5th bracket (i.e 33%)

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateProgressiveTax_IncomeGreaterThan372950_ReturnsCorrectTax()
        {
            decimal annualIncome = 473786;
            string postalCode = "1000";
            decimal expectedTax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (372950 - 171550) * 0.33m + (annualIncome - 372950) * 0.35m; // Expected tax calculation for the income in the 6th bracket (i.e 35%)

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateFlatValueTax_IncomeLessThan200000_ReturnsCorrectTax()
        {
            decimal annualIncome = 195876;
            string postalCode = "A100";
            decimal expectedTax = annualIncome * 0.05m; // Expected tax calculation for the income less than 200K/year

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateFlatValueTax_IncomeGreaterThanOrEqualTo200000_ReturnsCorrectTax()
        {
            decimal annualIncome = 235679;
            string postalCode = "A100";
            decimal expectedTax = 10000; // Expected tax calculation for the greater than or equal 200K/year

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateFlatRateTax_ReturnsCorrectTax()
        {
            decimal annualIncome = 235679;
            string postalCode = "7000";
            decimal expectedTax = annualIncome * 0.175m; // Expected tax calculation for the income less than 200K/year

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(expectedTax));
        }

        [Test]
        public async Task CalculateFlatRateTax_ReturnsError()
        {
            decimal annualIncome = 235679;
            string postalCode = "8000";
            decimal expectedTax = annualIncome * 0.175m; // Expected tax calculation for the income less than 200K/year

            var actualResult = await _taxCalculatorService.ComputeTax(postalCode, annualIncome);

            Assert.That(actualResult.succeeded, Is.False);
            Assert.That(actualResult.message, Is.EqualTo("Unknown Postal Code"));
        }

        [Test]
        public async Task GetListOfComputedTaxRates_ReturnsSuccess()
        {
            var actualResult = await _taxCalculatorService.GetComputedRates();

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data.Count(), Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public async Task DeactivateComputedTaxRates_ReturnsSuccess()
        {
            var actualResult = await _taxCalculatorService.DeactivateRate(32);

            Assert.That(actualResult.succeeded, Is.True);
            Assert.That(actualResult.data, Is.EqualTo(true));
        }

        [Test]
        public async Task DeactivateComputedTaxRates_ReturnsError()
        {
            var actualResult = await _taxCalculatorService.DeactivateRate(7500);

            Assert.That(actualResult.succeeded, Is.False);
            Assert.That(actualResult.data, Is.EqualTo(false));
        }
    }
}

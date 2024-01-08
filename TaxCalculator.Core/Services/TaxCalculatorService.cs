using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Core.Dtos.Responses;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Core.Utilities;
using TaxCalculator.Domain.Constants;
using TaxCalculator.Domain.Entity;
using TaxCalculator.Domain.Persistence;

namespace TaxCalculator.Core.Services
{
    public class TaxCalculatorService : ITaxCalculatorService
    {
        private readonly DataContext _dbContext;

        public TaxCalculatorService(DataContext dataContext)
        {
            _dbContext = dataContext;   
        }

        public async Task<IResult<List<TaxRates>>> GetComputedRates()
        {
            var rates = new List<TaxRates>();
            rates = await _dbContext.TaxRates.Where(x => x.Status == AppConstants.Active).OrderByDescending(x => x.CreatedOn).ToListAsync();
            return await Result<List<TaxRates>>.SuccessAsync(rates, SystemMessages.S0001, "success_get_tax_rates_01"); 
        }
        public async Task<IResult<bool>>DeactivateRate(long id)
        {
            var rate = await _dbContext.TaxRates.FirstOrDefaultAsync(x => x.Id == id);
            if (rate != null)
            {
                rate.Status = AppConstants.Inactive;
                _dbContext.Update(rate);
                await _dbContext.SaveChangesAsync();
                return await Result<bool>.SuccessAsync(true, SystemMessages.S0001, "success_deactivate_tax_rates_01");
            }
            return await Result<bool>.FailAsync(SystemMessages.E0002, "error_deactivate_tax_rates_01");
        }
        public async Task<IResult<decimal>> ComputeTax(string postalCode, decimal annualIncome)
        {
            try
            {
                //get tax info based on postal code
                var taxInfo = await _dbContext.TaxInformation.FirstOrDefaultAsync(x => x.PostalCode == postalCode);

                if(taxInfo == null)
                    return await Result<decimal>.FailAsync(SystemMessages.E0001, "error_compute_tax_01");

                //calculate tax based on taxInfo Tax calculation type
                decimal tax = taxInfo.TaxType switch
                {
                    AppConstants.ProgressiveTax => CalculateProgressiveTax(annualIncome),
                    AppConstants.FlatValueTax => CalculateFlatValueTax(annualIncome),
                    AppConstants.FlatRateTax => CalculateFlatRateTax(annualIncome),
                    _ => 0.0m
                };

                //save computed result to db
                var newTaxRateInfo = new TaxRates
                {
                    AnnualIncome = annualIncome,
                    PostalCode = postalCode,
                    CalculationType = taxInfo.TaxType,
                    Amount = tax
                };
                await _dbContext.TaxRates.AddAsync(newTaxRateInfo);
                await _dbContext.SaveChangesAsync();
                                
                return await Result<decimal>.SuccessAsync(tax, SystemMessages.S0001, "success_compute_tax_01");
            }
            catch (Exception ex)
            {
                return await Result<decimal>.FailAsync(SystemMessages.E0002, "error_compute_tax_99");
            }
        }

        private decimal CalculateProgressiveTax(decimal annualIncome)
        {
            decimal tax = 0;

            if (annualIncome <= 8350)
            {
                tax = annualIncome * 0.10m; // 10% tax rate for annualIncome up to $8,350
            }
            else if (annualIncome <= 33950)
            {
                tax = 8350 * 0.10m + (annualIncome - 8350) * 0.15m; // 10% for first bracket, 15% for the rest in bracket 2
            }
            else if (annualIncome <= 82250)
            {
                tax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (annualIncome - 33950) * 0.25m; // Calculating for brackets 1, 2, and 3
            }
            else if (annualIncome <= 171550)
            {
                tax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (annualIncome - 82250) * 0.28m; // Calculating for brackets 1, 2, 3, and 4
            }
            else if (annualIncome <= 372950)
            {
                tax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (annualIncome - 171550) * 0.33m; // Calculating for brackets 1, 2, 3, 4, and 5
            }
            else
            {
                tax = 8350 * 0.10m + (33950 - 8350) * 0.15m + (82250 - 33950) * 0.25m + (171550 - 82250) * 0.28m + (372950 - 171550) * 0.33m + (annualIncome - 372950) * 0.35m; // Calculating for all brackets
            }

            return tax;
        }

        private decimal CalculateFlatValueTax(decimal annualIncome)
        {
            decimal tax = 10000; //default flat value per year
            if(annualIncome < 200000)
            {
                tax = annualIncome * 0.05m; //5% tax for income less than 200k
            }

            return tax;
        }

        private decimal CalculateFlatRateTax(decimal annualIncome)
        {
            decimal tax = 0;

            tax = annualIncome * 0.175m;

            return tax;
        }
    }
}

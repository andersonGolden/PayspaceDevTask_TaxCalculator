using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Core.Dtos.Responses;
using TaxCalculator.Domain.Entity;

namespace TaxCalculator.Core.Interfaces
{
    public interface ITaxCalculatorService
    {
        Task<IResult<decimal>> ComputeTax(string postalCode, decimal annualIncome);
        Task<IResult<List<TaxRates>>> GetComputedRates();
        Task<IResult<bool>> DeactivateRate(long id);
    }
}

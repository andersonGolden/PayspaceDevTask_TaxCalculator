using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Core.Dtos.Responses
{
    public class TaxCalculatorResponse
    {
        public decimal annualIncome {  get; set; }
        public string postalCode { get; set; }
        public decimal taxAmount { get; set; }
    }
}

using System.ComponentModel;

namespace TaxCalculator.Core.Dtos.Requests
{
    public class TaxCalculatorRequest
    {
        [DisplayName("Annual Income")]
        public decimal annualIncome { get; set; }
        [DisplayName("Postal Code")]
        public string postalCode { get; set; }
    }
}

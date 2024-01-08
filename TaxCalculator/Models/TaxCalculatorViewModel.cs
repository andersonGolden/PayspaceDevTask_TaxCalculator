using System.ComponentModel;
using TaxCalculator.Core.Dtos.Requests;
using TaxCalculator.Domain.Entity;

namespace TaxCalculator.Models
{
    public class TaxCalculatorViewModel
    {
        [DisplayName("Result")]
        public decimal computeResult { get; set; }
        public TaxCalculatorRequest request { get; set; }
        public List<TaxRates> rates { get; set; }
    }
}

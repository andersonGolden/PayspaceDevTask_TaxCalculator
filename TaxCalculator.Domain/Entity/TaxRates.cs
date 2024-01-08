using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Entity
{
    public class TaxRates : BaseEntity
    {
        [DisplayName("Annual Income")]
        public decimal AnnualIncome { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [DisplayName("Calculation Type")]
        public string CalculationType { get; set; }
        [DisplayName("Tax Amount")]
        public decimal Amount { get; set; }
    }
}

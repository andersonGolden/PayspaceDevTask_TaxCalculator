using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Constants
{
    public class AppConstants
    {
        // Entity Status
        public const string Active = "A";
        public const string Inactive = "I";

        // Tax Calculation Type
        public const string ProgressiveTax = "Progressive";
        public const string FlatValueTax = "Flat Value";
        public const string FlatRateTax = "Flat rate";
    }
}

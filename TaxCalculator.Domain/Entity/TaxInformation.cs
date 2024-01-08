using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Domain.Entity
{
    public class TaxInformation:BaseEntity
    {
        public string PostalCode { get; set; }
        public string TaxType { get; set; }
    }
}

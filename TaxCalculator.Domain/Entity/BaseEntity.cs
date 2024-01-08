using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Constants;

namespace TaxCalculator.Domain.Entity
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public string Status { get; set; } = AppConstants.Active;
    }
}

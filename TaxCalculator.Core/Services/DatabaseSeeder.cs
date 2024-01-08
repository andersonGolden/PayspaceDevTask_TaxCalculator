using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Core.Interfaces;
using TaxCalculator.Domain.Constants;
using TaxCalculator.Domain.Entity;
using TaxCalculator.Domain.Persistence;

namespace TaxCalculator.Core.Services
{
    public class DatabaseSeeder : IDatabaseSeeder
    {

        private readonly DataContext _db;
        public DatabaseSeeder(DataContext db)
        {
           _db = db;
        }
        public void Initialize()
        {
            AddTaxInformation();
        }

        private void AddTaxInformation()
        {
            Task.Run(async () =>
            {
                if (!_db.TaxInformation.Any())
                {
                    await _db.TaxInformation.AddRangeAsync
                        (
                            new TaxInformation
                            {
                                PostalCode = "7441",
                                TaxType = AppConstants.ProgressiveTax,
                                CreatedOn = DateTime.Now,
                                Status = AppConstants.Active
                            },
                            new TaxInformation
                            {
                                PostalCode = "A100",
                                TaxType = AppConstants.FlatValueTax,
                                CreatedOn = DateTime.Now,
                                Status = AppConstants.Active
                            },
                            new TaxInformation
                            {
                                PostalCode = "7000",
                                TaxType = AppConstants.FlatRateTax,
                                CreatedOn = DateTime.Now,
                                Status = AppConstants.Active
                            },
                            new TaxInformation
                            {
                                PostalCode = "1000",
                                TaxType = AppConstants.ProgressiveTax,
                                CreatedOn = DateTime.Now,
                                Status = AppConstants.Active
                            }


                        );
                }

                await _db.SaveChangesAsync();
            }).GetAwaiter().GetResult();
        }
    }
}

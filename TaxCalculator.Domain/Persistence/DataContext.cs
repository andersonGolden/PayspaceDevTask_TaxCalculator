using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculator.Domain.Entity;

namespace TaxCalculator.Domain.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }
        /// <summary>
        /// Tax Information table
        /// </summary>
        public DbSet<TaxInformation> TaxInformation { get; set; }
        /// <summary>
        /// Tax Rates table
        /// </summary>
        public DbSet<TaxRates> TaxRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

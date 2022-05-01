using Microsoft.EntityFrameworkCore;
using SuperMarket.Entities;
using SuperMarket.Persistence.EF.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF
{
    public class EFDataContext : DbContext
    {

        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Stuff> Stuffs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
    }
}

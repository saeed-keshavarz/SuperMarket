using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Stuffs
{
    public class StuffEntityMap : IEntityTypeConfiguration<Stuff>
    {
        public void Configure(EntityTypeBuilder<Stuff> builder)
        {
            builder.ToTable("Stuffs");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(_=>_.Invoces)
                .WithOne(_=>_.Stuff)
                .HasForeignKey(_=>_.StuffId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(_ => _.Vouchers)
                .WithOne(_ => _.Stuff)
                .HasForeignKey(_ => _.StuffId)
                .OnDelete(DeleteBehavior.ClientNoAction);

        }
    }
}

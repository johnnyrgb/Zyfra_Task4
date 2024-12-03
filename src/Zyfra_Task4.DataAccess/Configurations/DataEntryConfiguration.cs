using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zyfra_Task4.DataAccess.Entities;

namespace Zyfra_Task4.DataAccess.Configurations
{
    public class DataEntryConfiguration : IEntityTypeConfiguration<DataEntry>
    {
        public void Configure(EntityTypeBuilder<DataEntry> builder)
        {
            builder.HasKey(de => de.Id); // Первичный ключ

            builder.Property(de => de.Value) // Значение
                .IsRequired()
                .HasMaxLength(4096);
        }
    }
}



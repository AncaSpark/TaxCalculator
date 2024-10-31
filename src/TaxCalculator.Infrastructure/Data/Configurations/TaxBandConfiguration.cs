using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Infrastructure.Data.Configurations
{
    public class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
    {
        public void Configure(EntityTypeBuilder<TaxBand> builder)
        {
            builder.ToTable("TaxBands");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LowerLimit)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.UpperLimit)
                .HasPrecision(18, 2);

            builder.Property(x => x.TaxRate)
                .IsRequired();

            // Index for faster queries
            builder.HasIndex(x => x.LowerLimit);

            // Ensure tax bands don't overlap
            builder.HasCheckConstraint(
                "CK_TaxBand_Limits",
                "[LowerLimit] >= 0 AND ([UpperLimit] IS NULL OR [UpperLimit] > [LowerLimit])"
            );
        }
    }

}

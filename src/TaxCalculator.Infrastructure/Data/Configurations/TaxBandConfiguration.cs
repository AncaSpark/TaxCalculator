using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain.Models;

namespace TaxCalculator.Infrastructure.Data.Configurations
{
    public class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
    {
        public void Configure(EntityTypeBuilder<TaxBand> builder)
        {
            builder.ToTable("TaxBands", t =>
            {
                // Ensure tax bands don't overlap
                t.HasCheckConstraint("CK_TaxBands_Limits", "[LowerLimit] < [UpperLimit]");
            });

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

        }
    }

}

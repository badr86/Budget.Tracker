using Budget.Tracker.Core.MovementAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Tracker.InfraStructure.EntityConfigurations
{
    public class DirectionEntityTypeConfiguration : IEntityTypeConfiguration<Direction>
    {
        public void Configure(EntityTypeBuilder<Direction> builder)
        {
            builder.ToTable($"{nameof(Direction)}s");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).HasDefaultValue(1).ValueGeneratedNever().IsRequired();

            builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        }
    }
}
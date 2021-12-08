using Budget.Tracker.Core.MovementAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budget.Tracker.InfraStructure.EntityConfigurations
{
    public class MovementItemEntityTypeConfiguration : IEntityTypeConfiguration<MovementItem>
    {
        public void Configure(EntityTypeBuilder<MovementItem> builder)
        {
            builder.ToTable($"{nameof(MovementItem)}s");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).ValueGeneratedOnAdd().IsRequired();

            builder.Property(e => e.MovementId).IsRequired();

            builder.Property(e => e.Date).IsRequired();

            builder.Property(e => e.Amount).IsRequired();

            builder.Property(e => e.Description).IsRequired(false);
            
            builder.Property(e => e.OwnerId).IsRequired(false);
            
            //builder.HasOne<User>().WithMany().HasForeignKey(e => e.OwnerId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletService.Domain.Entities;

namespace WalletService.Persistence.Configurations;

public class BonusWalletTransactionConfiguration : IEntityTypeConfiguration<BonusWalletTransaction>
{
    public void Configure(EntityTypeBuilder<BonusWalletTransaction> builder)
    {
        builder.ToTable("BonusWalletTransaction");
        builder.HasKey(x => x.BonusWalletTransactionId);

        builder.Property(x => x.BonusWalletTransactionId)
            .UseIdentityColumn();

        builder.Property(x => x.TransactionNo)
            .IsRequired(false);

        builder.Property(x => x.TransactionReference)
            .IsRequired(false);

        builder.Property(x => x.PromotionId)
            .HasColumnType("bigint")
            .IsRequired(true);

        builder.Property(x => x.DateStarted)
            .IsRequired(true);

        builder.Property(x => x.DateExpired)
            .IsRequired(true);

        builder.Property(x => x.ModeOfTransaction)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .IsRequired(false);

        builder.HasOne(x => x.BonusAccount)
            .WithMany(f => f.BonusWalletTransactions)
            .HasForeignKey(f => f.AccountId);
    }
}
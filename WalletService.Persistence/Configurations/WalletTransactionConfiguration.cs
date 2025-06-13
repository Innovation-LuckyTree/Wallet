using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletService.Domain.Entities;

namespace WalletService.Persistence.Configurations;

public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
{
    public void Configure(EntityTypeBuilder<WalletTransaction> builder)
    {
        builder.ToTable("WalletTransaction");
        builder.HasKey(x => x.WalletTransactionId);

        builder.Property(x => x.WalletTransactionId)
            .UseIdentityColumn();

        builder.Property(x => x.TransactionNo)
            .IsRequired(false);

        builder.Property(x => x.TransactionReference)
            .IsRequired(false);

        builder.Property(x => x.ModeOfTransaction)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .IsRequired(false);

        builder.HasOne(x => x.Account)
            .WithMany(f => f.WalletTransactions)
            .HasForeignKey(f => f.AccountId);
    }
}
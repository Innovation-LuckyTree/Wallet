using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletService.Domain.Entities;

namespace WalletService.Persistence.Configurations;

public class BonusAccountConfiguration : IEntityTypeConfiguration<BonusAccount>
{
    public void Configure(EntityTypeBuilder<BonusAccount> builder)
    {
        builder.ToTable("BonusAccount");
        builder.HasKey(x => x.BonusAccountId);
    }
}
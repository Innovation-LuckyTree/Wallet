// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using WalletService.Domain.Entities;

// namespace WalletService.Persistence.Configurations;

// public class AccountWalletDocConfiguration : IEntityTypeConfiguration<AccountWalletDoc>
// {
//     public void Configure(EntityTypeBuilder<AccountWalletDoc> builder)
//     {
//         builder.Property(o => o.Account)
//             .HasColumnType("jsonb");
//     }
// }
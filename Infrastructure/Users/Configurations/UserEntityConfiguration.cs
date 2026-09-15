using Domain.Common.Consts;
using Domain.Common.ValueObjects;
using Domain.Users.Consts;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users.Configurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(x => x.Email);
        builder.HasIndex(x => x.UserName);
        
        builder.Property(u => u.Email)
            .HasConversion(c => c.Value, c => new Email(c))
            .IsRequired()
            .HasMaxLength(CommonConsts.Email_MaxLength);
        
        builder.Property(u => u.UserName)
            .HasConversion(c => c.Value, c => new UserName(c))
            .IsRequired()
            .HasMaxLength(UserConsts.UserName_MaxLength);

        builder.Property(e => e.Password)
            .HasConversion(c => c.Value, c => new UserPassword(c))
            .IsRequired();

        builder
            .HasDiscriminator<string>("Type")
            .HasValue<AdminEntity>("admin")
            .HasValue<RegularUserEntity>("regular");

        builder.OwnsOne(u => u.ModificationInfo, o =>
        {
            o.Property(a => a.ModifiedAt).IsRequired();
            o.Property(a => a.ModifiedById);
            o.Property(a => a.ModifiedByName);
        });

        builder.OwnsOne(u => u.DeletionInfo, o =>
        {
            o.Property(a => a.DeletedAt).IsRequired();
            o.Property(a => a.DeletedById);
            o.Property(a => a.DeletedByName);
        });

        builder.HasQueryFilter(u => u.DeletionInfo == null);
    }
}

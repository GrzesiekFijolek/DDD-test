using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Users.Configurations;

internal sealed  class RegularUserEntityConfiguration : IEntityTypeConfiguration<RegularUserEntity>
{
    public void Configure(EntityTypeBuilder<RegularUserEntity> builder)
    {
        builder.Property(e => e.Department)
            .HasConversion(c => c.Value, c => UserDepartment.From(c))
            .IsRequired();
    }
}
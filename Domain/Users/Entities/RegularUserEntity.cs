using Domain.Common.Audit;
using Domain.Common.ValueObjects;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public sealed class RegularUserEntity : UserEntity
{
    public UserDepartment Department { get; set; } 

    private RegularUserEntity(Email email, UserName userName, UserPassword password, UserDepartment department) : base(email, userName, password)
    {
        Department = department;
    }

    public static RegularUserEntity Create(Email email, UserName userName, UserPassword password, UserDepartment department)
    {
        return new RegularUserEntity(email, userName, password, department);
    }
}

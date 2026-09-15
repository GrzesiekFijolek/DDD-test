using Domain.Common.Exceptions;
using Domain.Users.ValueObjects;

namespace Domain.Users.Exceptions;

internal sealed class InvalidUserDepartmentException : CustomException
{
    public string Value { get; }
    
    public InvalidUserDepartmentException(string value) : base($"Value {value} is invalid for {nameof(UserDepartment)}")
    {
        Value = value;
    }
}
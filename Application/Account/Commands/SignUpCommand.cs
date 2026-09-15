using Application.Common.CQRS;
using Domain.Users.ValueObjects;

namespace Application.Account.Commands;

public record SignUpCommand : ICommand
{
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public UserDepartment Department { get; set; } = default!;
}

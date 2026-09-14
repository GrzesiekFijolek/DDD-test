using Application.Common.CQRS;

namespace Application.Users.Commands.SingUp;

public record SignUpCommand : ICommand
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}

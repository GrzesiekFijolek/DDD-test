using Application.Common.CQRS;

namespace Application.Users.Commands.SingUp;

public record SignUpCommand : ICommand
{
    public string Email { get; set; }

    public string Password { get; set; }
}
using Application.Common.CQRS;
using Application.Security;

namespace Application.Users.Commands.SingUp;

internal sealed class SingUpCommandHandler : ICommandHandler<SignUpCommand>
{
    private readonly IPasswordManager _passwordManager;

    public SingUpCommandHandler(IPasswordManager passwordManager)
    {
        _passwordManager = passwordManager;
    }

    public async Task HandleAsync(SignUpCommand command)
    {
        var securedPassword = _passwordManager.Secure(command.Password);
    }
}
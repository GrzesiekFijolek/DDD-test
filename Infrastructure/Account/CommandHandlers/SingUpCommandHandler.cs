using Application.Account.Commands;
using Application.Common.CQRS;
using Application.Security;
using Infrastructure.Database;

namespace Infrastructure.Account.CommandHandlers;

internal sealed class SingUpCommandHandler : ICommandHandler<SignUpCommand>
{
    private readonly IPasswordManager _passwordManager;
    private readonly AppDbContext _appDbContext;

    public SingUpCommandHandler(IPasswordManager passwordManager, AppDbContext appDbContext)
    {
        _passwordManager = passwordManager;
        _appDbContext = appDbContext;
    }

    public async Task HandleAsync(SignUpCommand command)
    {
        var securedPassword = _passwordManager.Secure(command.Password);
    }
}
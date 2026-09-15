using Application.Auth.Commands;
using Application.Common.CQRS;
using Application.Security;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Infrastructure.Database;

namespace Infrastructure.Auth.CommandHandlers;

internal sealed class RegularUserSignUpCommandHandler : ICommandHandler<RegularUserSignUpCommand>
{
    private readonly IPasswordManager _passwordManager;
    private readonly AppDbContext _appDbContext;

    public RegularUserSignUpCommandHandler(IPasswordManager passwordManager, AppDbContext appDbContext)
    {
        _passwordManager = passwordManager;
        _appDbContext = appDbContext;
    }


    public Task HandleAsync(RegularUserSignUpCommand command)
    {
        var securedPassword = _passwordManager.Secure(command.Request.Password);

        var newUser = RegularUserEntity.Create(command.Request.Email, command.Request.UserName, securedPassword,
            UserDepartment.From(command.Request.Department));

        _appDbContext.Add(newUser);

        return Task.CompletedTask;
    }
}
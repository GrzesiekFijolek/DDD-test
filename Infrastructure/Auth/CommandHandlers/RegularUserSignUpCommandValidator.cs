using Application.Auth.Commands;
using Domain.Common.Consts;
using Domain.Users.Consts;
using FluentValidation;

namespace Infrastructure.Auth.CommandHandlers;

internal sealed class RegularUserSignUpCommandValidator : AbstractValidator<RegularUserSignUpCommand>
{
    public RegularUserSignUpCommandValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .MaximumLength(CommonConsts.Email_MaxLength);

        RuleFor(x => x.Request.UserName)
            .NotEmpty()
            .MinimumLength(UserConsts.UserName_MinLength)
            .MaximumLength(UserConsts.UserName_MaxLength);

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(UserConsts.UserPassword_MinLength)
            .MaximumLength(UserConsts.UserPassword_MaxLength)
            .Matches(UserConsts.UserPassword_Regex)
                .WithMessage("Password must contain at least one uppercase letter, one digit, and no whitespace.");

        RuleFor(x => x.Request.Department)
            .NotEmpty();
    }
}

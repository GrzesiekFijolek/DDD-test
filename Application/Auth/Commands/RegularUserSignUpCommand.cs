using Application.Auth.Requests;
using Application.Common.CQRS;

namespace Application.Auth.Commands;

public record RegularUserSignUpCommand(RegularUserSignUpRequest Request) : ICommand;


 
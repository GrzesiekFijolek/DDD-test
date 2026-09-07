namespace Application.Common.CQRS;

public interface ICommand;

public interface ICommand<TResponse> where TResponse : class;


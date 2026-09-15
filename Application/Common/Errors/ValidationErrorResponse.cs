namespace Application.Common.Errors;

public record ValidationErrorResponse(string Code, IReadOnlyDictionary<string, string[]> Errors);

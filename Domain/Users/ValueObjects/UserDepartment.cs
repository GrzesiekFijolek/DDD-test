using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public sealed record UserDepartment
{
    public string Value { get; }

    public static readonly UserDepartment Engineering    = new(nameof(Engineering));
    public static readonly UserDepartment Product        = new(nameof(Product));
    public static readonly UserDepartment Marketing      = new(nameof(Marketing));
    public static readonly UserDepartment Finance        = new(nameof(Finance));
    public static readonly UserDepartment HumanResources = new(nameof(HumanResources));
    public static readonly UserDepartment It             = new(nameof(It));

    public static IReadOnlyCollection<UserDepartment> All { get; } =
        [Engineering, Product, Marketing, Finance, HumanResources, It];

    private UserDepartment(string value) => Value = value;

    public static UserDepartment From(string value) =>
        All.FirstOrDefault(d => d.Value == value) ?? throw new InvalidUserDepartmentException(value);

    public override string ToString() => Value;
}

namespace Domain.Users.Consts;

public static class UserConsts
{
    public const int UserName_MinLength = 5;
    public const int UserName_MaxLength = 50;

    public const int UserPassword_MinLength = 8;
    public const int UserPassword_MaxLength = 50;
    public const string UserPassword_Regex = @"^(?=.*[A-Z])(?=.*\d)(?!.*\s).+$";
}
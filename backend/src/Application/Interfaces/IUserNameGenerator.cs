namespace Application.Interfaces;

public interface IUserNameGenerator
{
    Task<string> GenerateUserNameAsync(string firstName, string lastName);
}
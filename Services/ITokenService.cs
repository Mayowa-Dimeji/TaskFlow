public interface ITokenService
{
    Task<string?> GetToken();
    Task SaveToken(string token);
    Task ClearToken();
    Task<string?> GetUsername();
}

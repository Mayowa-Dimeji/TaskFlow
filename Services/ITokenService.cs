public interface ITokenService
{
    Task<string?> GetToken();
    void SaveToken(string token);
    void ClearToken();
    Task<string?> GetUsername();
}

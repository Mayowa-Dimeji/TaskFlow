using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;

#pragma warning disable CA1050 // Declare types in namespaces
public class TokenService : ITokenService
#pragma warning restore CA1050 // Declare types in namespaces
{
    private readonly IJSRuntime _js;

    private const string TokenKey = "authToken";

    public TokenService(IJSRuntime js)
    {
        _js = js;
    }

    public async void SaveToken(string token)
    {
        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    }

    public async Task<string?> GetToken()
    {
        return await _js.InvokeAsync<string>("localStorage.getItem", TokenKey);
    }

    public async void ClearToken()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
    }

    public async Task<string?> GetUsername()
    {
        var token = await GetToken();
        if (string.IsNullOrWhiteSpace(token)) return null;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        return jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
    }
}

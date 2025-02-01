using System.IdentityModel.Tokens.Jwt;

namespace InventoryManagementBlazorServer.Services;

public class TokenStorageService
{
    private readonly string _filePath = "authToken.txt";

    public async Task SaveTokenAsync(string token)
    {
        await File.WriteAllTextAsync(_filePath, token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return File.Exists(_filePath) ? await File.ReadAllTextAsync(_filePath) : null;
    }

    public void DeleteToken()
    {
        if(File.Exists(_filePath))
        {
            File.Delete(_filePath);
        }
    }

    public async Task<int?> GetUserIdFromTokenAsync()
    {
        var token = await GetTokenAsync();
        if(string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        var userIdClaim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "nameid");

        return userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
    }
}
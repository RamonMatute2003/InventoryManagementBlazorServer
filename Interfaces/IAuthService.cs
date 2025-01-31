using InventoryManagementBlazorServer.Helpers;

namespace InventoryManagementBlazorServer.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<string>> LoginAsync(string username, string password);
    Task LogoutAsync();
}
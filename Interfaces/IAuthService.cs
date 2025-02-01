using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.DTOs;

namespace InventoryManagementBlazorServer.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password);
    Task LogoutAsync();
}
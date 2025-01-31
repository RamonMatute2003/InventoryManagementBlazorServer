using InventoryManagement.Helpers;

using InventoryManagementBlazorServer.DTOs;
using InventoryManagementBlazorServer.Helpers;

namespace InventoryManagementBlazorServer.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password);
    Task LogoutAsync();
}
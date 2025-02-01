using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.DTOs;

namespace InventoryManagementBlazorServer.Interfaces;

public interface IInventoryService
{
    Task<List<BranchDto>> GetBranchesAsync();
    Task<List<ProductDto>> GetProductsAsync();
    Task<int> GetAvailableStockAsync(int productId);
    Task<ApiResponse<string>> RegisterInventoryOutAsync(InventoryOutDto inventoryOut);
    Task<ApiResponse<ProductDetailsDto>> GetProductDetailsAsync(int productId);
}
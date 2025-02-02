using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.DTOs;
using InventoryManagementBlazorServer.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InventoryManagementBlazorServer.Services;

public class InventoryService(HttpClient httpClient, TokenStorageService tokenStorageService, INotificationService notificationService) : IInventoryService
{
    private async Task SetAuthorizationHeader()
    {
        var token = await tokenStorageService.GetTokenAsync();

        if(!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<BranchDto>> GetBranchesAsync()
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await httpClient.GetFromJsonAsync<ApiResponse<List<BranchDto>>>(ApiEndpoints.GetBranches);
            return response?.Body?.Where(b => b.Id != 1).ToList() ?? [];
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al obtener sucursales: {ex.Message}");
            return [];
        }
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<ApiResponse<List<ProductDto>>>(ApiEndpoints.GetProducts);
            return response?.Body ?? [];
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al obtener productos: {ex.Message}");
            return [];
        }
    }

    public async Task<int> GetAvailableStockAsync(int productId)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await httpClient.GetFromJsonAsync<ApiResponse<int>>(ApiEndpoints.GetAvailableStock(productId));
            return response?.Body ?? 0;
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al obtener stock: {ex.Message}");
            return 0;
        }
    }

    public async Task<ApiResponse<string>> RegisterInventoryOutAsync(InventoryOutDto inventoryOut)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await httpClient.PostAsJsonAsync(ApiEndpoints.RegisterInventoryOut, inventoryOut);

            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<InventoryOutDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(apiResponse == null || !apiResponse.IsSuccess)
                {
                    notificationService.NotifyError($"{apiResponse?.Message}");
                    return new ApiResponse<string>(500, "Error en el servidor.");
                }

                return new ApiResponse<string>(apiResponse.Code, apiResponse.Message);
            } catch(JsonException)
            {
                notificationService.NotifyError($"Error al parsear la respuesta del backend");
                return new ApiResponse<string>(500, "Formato de respuesta inválido.");
            }
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error en la solicitud: {ex.Message}");
            return new ApiResponse<string>(500, "Error desconocido.");
        }
    }

    public async Task<ApiResponse<ProductDetailsDto>> GetProductDetailsAsync(int productId)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await httpClient.GetFromJsonAsync<ApiResponse<ProductDetailsDto>>(
                $"{ApiEndpoints.GetProductDetails(productId)}");

            return response ?? new ApiResponse<ProductDetailsDto>(500, "Error desconocido");
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al obtener detalles del producto: {ex.Message}");
            return new ApiResponse<ProductDetailsDto>(500, "Error en la solicitud");
        }
    }

    public async Task<List<FilteredInventoryOutDto>> GetFilteredInventoryOutsAsync(DateTime? startDate, DateTime? endDate, int? branchId)
    {
        await SetAuthorizationHeader();

        try
        {
            var url = $"{ApiEndpoints.GetFilteredInventoryOuts}?startDate={startDate?.ToString("yyyy-MM-dd")}&endDate={endDate?.ToString("yyyy-MM-dd")}&branchId={branchId}";
            var response = await httpClient.GetFromJsonAsync<ApiResponse<List<FilteredInventoryOutDto>>>(url);

            return response?.Body ?? [];
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al obtener las salidas de inventario: {ex.Message}");
            return [];
        }
    }

    public async Task<ApiResponse<string>> MarkAsReceivedAsync(int id)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await httpClient.PutAsync(ApiEndpoints.ReceiveInventoryOut(id), null);
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiResponse<string>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? new ApiResponse<string>(500, "Error al procesar la solicitud.");
        } catch(Exception ex)
        {
            notificationService.NotifyError($"Error al marcar como recibida: {ex.Message}");
            return new ApiResponse<string>(500, "Error desconocido.");
        }
    }
}
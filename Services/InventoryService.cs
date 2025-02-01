using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.DTOs;
using InventoryManagementBlazorServer.Interfaces;
using Blazored.LocalStorage;
using System.Net.Http.Headers;
using Microsoft.JSInterop;
using System.Text.Json;

namespace InventoryManagementBlazorServer.Services;

public class InventoryService : IInventoryService
{
    private readonly HttpClient _httpClient;
    private readonly TokenStorageService _tokenStorageService;
    private readonly INotificationService _notificationService;

    public InventoryService(HttpClient httpClient, TokenStorageService tokenStorageService, INotificationService notificationService)
    {
        _httpClient = httpClient;
        _tokenStorageService = tokenStorageService;
        _notificationService = notificationService;
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _tokenStorageService.GetTokenAsync();

        if(!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<BranchDto>> GetBranchesAsync()
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<BranchDto>>>(ApiEndpoints.GetBranches);
            return response?.Body ?? [];
        } catch(Exception ex)
        {
            _notificationService.NotifyError($"Error al obtener sucursales: {ex.Message}");
            return (List<BranchDto>) new List<BranchDto>().Where( b => b.Id != 1);
        }
    }

    public async Task<List<ProductDto>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<ProductDto>>>(ApiEndpoints.GetProducts);
            return response?.Body ?? [];
        } catch(Exception ex)
        {
            _notificationService.NotifyError($"Error al obtener productos: {ex.Message}");
            return [];
        }
    }

    public async Task<int> GetAvailableStockAsync(int productId)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<int>>(ApiEndpoints.GetAvailableStock(productId));
            return response?.Body ?? 0;
        } catch(Exception ex)
        {
            _notificationService.NotifyError($"Error al obtener stock: {ex.Message}");
            return 0;
        }
    }

    public async Task<ApiResponse<string>> RegisterInventoryOutAsync(InventoryOutDto inventoryOut)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.RegisterInventoryOut, inventoryOut);

            var responseContent = await response.Content.ReadAsStringAsync();

            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<InventoryOutDto>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if(apiResponse == null || !apiResponse.IsSuccess)
                {
                    _notificationService.NotifyError($"❌ Error en el backend: {apiResponse?.Message ?? "Respuesta vacía"}");
                    return new ApiResponse<string>(500, "Error en el servidor.");
                }

                _notificationService.NotifySuccess(apiResponse.Message);
                return new ApiResponse<string>(apiResponse.Code, apiResponse.Message);
            } catch(JsonException)
            {
                _notificationService.NotifyError($"❌ Error al parsear la respuesta del backend: {responseContent}");
                return new ApiResponse<string>(500, "Formato de respuesta inválido.");
            }
        } catch(Exception ex)
        {
            _notificationService.NotifyError($"❌ Error en la solicitud: {ex.Message}");
            return new ApiResponse<string>(500, "Error desconocido.");
        }
    }

    public async Task<ApiResponse<ProductDetailsDto>> GetProductDetailsAsync(int productId)
    {
        await SetAuthorizationHeader();

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ApiResponse<ProductDetailsDto>>(
                $"{ApiEndpoints.GetProductDetails(productId)}");

            return response ?? new ApiResponse<ProductDetailsDto>(500, "Error desconocido");
        } catch(Exception ex)
        {
            _notificationService.NotifyError($"❌ Error al obtener detalles del producto: {ex.Message}");
            return new ApiResponse<ProductDetailsDto>(500, "Error en la solicitud");
        }
    }
}
using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.Interfaces;
using Microsoft.AspNetCore.Components;
using InventoryManagementBlazorServer.DTOs;

namespace InventoryManagementBlazorServer.Services;

public class AuthService(HttpClient httpClient, NavigationManager navigation, TokenStorageService tokenStorageService) : IAuthService
{
    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password)
    {
        var response = await httpClient.PostAsJsonAsync(ApiEndpoints.Login, new { Username = username, Password = password });

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

        if(apiResponse == null )
        {
            return new ApiResponse<LoginResponseDto>(500, "Sin respuesta del servidor");
        }else if(apiResponse.Body == null)
        {
            return new ApiResponse<LoginResponseDto>(500, apiResponse.Message);
        }

        if(!apiResponse.IsSuccess)
        {
            return apiResponse;
        }

        var token = apiResponse.Body.Token;

        await tokenStorageService.SaveTokenAsync(token);

        navigation.NavigateTo("/inventory-out", forceLoad: true);

        return apiResponse;
    }

    public void LogoutAsync()
    {
        tokenStorageService.DeleteToken();
        navigation.NavigateTo("/");
    }
}
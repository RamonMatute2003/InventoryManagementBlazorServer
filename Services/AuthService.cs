using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.Interfaces;
using Microsoft.AspNetCore.Components;
using InventoryManagementBlazorServer.DTOs;

namespace InventoryManagementBlazorServer.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigation;
    private readonly TokenStorageService _tokenStorageService;

    public AuthService(HttpClient httpClient, NavigationManager navigation, TokenStorageService tokenStorageService)
    {
        _httpClient = httpClient;
        _navigation = navigation;
        _tokenStorageService = tokenStorageService;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Login, new { Username = username, Password = password });

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

        if(apiResponse == null || apiResponse.Body == null)
        {
            return new ApiResponse<LoginResponseDto>(500, "Error desconocido");
        }

        if(!apiResponse.IsSuccess)
        {
            return apiResponse;
        }

        var token = apiResponse.Body.Token;

        await _tokenStorageService.SaveTokenAsync(token);

        _navigation.NavigateTo("/inventory-out", forceLoad: true);

        return apiResponse;
    }

    public async Task LogoutAsync()
    {
        _tokenStorageService.DeleteToken();
        _navigation.NavigateTo("/");
    }
}
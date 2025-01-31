using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.Interfaces;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using InventoryManagement.Helpers;
using InventoryManagementBlazorServer.DTOs;

namespace InventoryManagementBlazorServer.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigation;
    private readonly INotificationService _notificationService;
    private readonly ILocalStorageService _localStorageService;

    public AuthService(HttpClient httpClient, NavigationManager navigation, INotificationService notificationService, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _navigation = navigation;
        _notificationService = notificationService;
        _localStorageService = localStorageService;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(string username, string password)
    {
        var requestDto = new LoginRequestDto { Username = username, Password = password };

        var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Login, requestDto);

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseDto>>();

        if(apiResponse == null)
        {
            _notificationService.NotifyError("Error, no se obtuvo respuesta");
            return new ApiResponse<LoginResponseDto>(500, "Error desconocido");
        }

        if(!apiResponse.IsSuccess)
        {
            _notificationService.NotifyError(apiResponse.Message);
        } else
        {
            if(!string.IsNullOrEmpty(apiResponse.Body?.Token))
            {
                await _localStorageService.SetItemAsync("authToken", apiResponse.Body.Token);
                _notificationService.NotifySuccess(apiResponse.Message);
                _navigation.NavigateTo("/");
            } else
            {
                _notificationService.NotifyError(apiResponse.Message);
            }
        }

        return apiResponse;
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("authToken");
        _notificationService.NotifySuccess("Sesión cerrada correctamente.");
        _navigation.NavigateTo("/");
    }
}
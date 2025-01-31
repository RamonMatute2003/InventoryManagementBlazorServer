using InventoryManagementBlazorServer.Helpers;
using InventoryManagementBlazorServer.Interfaces;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

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

    public async Task<ApiResponse<string>> LoginAsync(string username, string password)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiEndpoints.Login, new { Username = username, Password = password });

        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

        if(apiResponse == null)
        {
            _notificationService.NotifyError("Error desconocido al procesar la respuesta.");
            return new ApiResponse<string>(500, "Error desconocido");
        }

        if(!apiResponse.IsSuccess)
        {
            _notificationService.NotifyError(apiResponse.Message);
        } else
        {
            await _localStorageService.SetItemAsync("authToken", apiResponse.Body);
            _notificationService.NotifySuccess("Login exitoso!");
            _navigation.NavigateTo("/");
        }

        return apiResponse;
    }

    public async Task LogoutAsync()
    {
        await _localStorageService.RemoveItemAsync("authToken");
        _notificationService.NotifySuccess("Sesión cerrada correctamente.");
        _navigation.NavigateTo("/login");
    }
}
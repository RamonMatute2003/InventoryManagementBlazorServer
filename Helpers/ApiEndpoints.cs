namespace InventoryManagementBlazorServer.Helpers;

public static class ApiEndpoints
{
    private const string BaseUrl = "https://inventorymanagementserver-c3bth5ccb0f0dhc0.centralus-01.azurewebsites.net/api/";

    public static string Login => $"{BaseUrl}/auth/login";
    public static string Logout => $"{BaseUrl}/auth/logout";
    public static string GetUser => $"{BaseUrl}/users/me";
    public static string GetInventory => $"{BaseUrl}/inventory";
}
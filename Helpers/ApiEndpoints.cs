namespace InventoryManagementBlazorServer.Helpers;

public static class ApiEndpoints
{
    private const string BaseUrl = "https://inventorymanagementserver-c3bth5ccb0f0dhc0.centralus-01.azurewebsites.net/api/";

    public static string Login => $"{BaseUrl}/auth/login";

    public static string GetAvailableStock(int productId) => $"{BaseUrl}inventory/{productId}/available-stock";

    public static string RegisterInventoryOut => $"{BaseUrl}inventory-outs/create";
    public static string ReceiveInventoryOut(int id) => $"{BaseUrl}inventory-outs/{id}/receive";

    public static string GetBranches => $"{BaseUrl}/branches";
    public static string GetProducts => $"{BaseUrl}/products";
    public static string GetProductDetails(int productId) => $"{BaseUrl}inventory-outs/product-details/{productId}";

    public static string GetFilteredInventoryOuts => $"{BaseUrl}inventory-outs";
}
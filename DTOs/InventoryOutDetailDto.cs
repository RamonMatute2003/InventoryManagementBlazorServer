using System.Text.Json.Serialization;

namespace InventoryManagementBlazorServer.DTOs;

public class InventoryOutDetailDto
{
    [JsonPropertyName("idProduct")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("batchId")]
    public int BatchId { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("cost")]
    public decimal Cost { get; set; }

    [JsonPropertyName("expirationDate")]
    public DateOnly ExpirationDate { get; set; }
}
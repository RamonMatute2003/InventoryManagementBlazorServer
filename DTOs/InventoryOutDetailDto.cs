using System.Text.Json.Serialization;

namespace InventoryManagementBlazorServer.DTOs;

public class InventoryOutDetailDto
{
    [JsonPropertyName("idProduct")]
    public int ProductId { get; set; }
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
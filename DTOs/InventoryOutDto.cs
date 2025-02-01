using System.Text.Json.Serialization;

namespace InventoryManagementBlazorServer.DTOs;

public class InventoryOutDto
{
    [JsonPropertyName("totalCost")]
    public decimal TotalCost { get; set; }
    [JsonPropertyName("idBranch")]
    public int IdBranch { get; set; }
    [JsonPropertyName("idUser")]
    public int IdUser { get; set; }
    [JsonPropertyName("details")]
    public List<InventoryOutDetailDto> Details { get; set; } = [];
}
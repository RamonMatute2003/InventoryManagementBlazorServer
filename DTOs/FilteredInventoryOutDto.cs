namespace InventoryManagementBlazorServer.DTOs;

public class FilteredInventoryOutDto
{
    public int IdOutHeader { get; set; }
    public DateTime OutDate { get; set; }
    public int TotalUnits { get; set; }
    public decimal TotalCost { get; set; }
    public string Status { get; set; }
    public string UserName { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public string? ReceivedBy { get; set; }
}
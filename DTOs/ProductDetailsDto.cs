﻿namespace InventoryManagementBlazorServer.DTOs;

public class ProductDetailsDto
{
    public int IdProduct { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public List<ProductLotDto> Lots { get; set; } = [];
}